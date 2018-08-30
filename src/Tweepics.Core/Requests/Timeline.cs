using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweepics.Core.Config;
using Tweepics.Core.Models;
using Tweepics.Core.Database;
using System.Threading;

namespace Tweepics.Core.Requests
{
    public class Timeline
    {
        private static int RequestsRemaining { get; set; }
        private static DateTime ResetDateTime { get; set; }

        public Timeline()
        {
            Tweetinvi.Auth.SetUserCredentials(Keys.twitterConsumerKey,
                                              Keys.twitterConsumerSecret,
                                              Keys.twitterAccessToken,
                                              Keys.twitterAccessTokenSecret);

            Tweetinvi.RateLimit.RateLimitTrackerMode = Tweetinvi.RateLimitTrackerMode.TrackAndAwait;
            var rateLimit = Tweetinvi.RateLimit.GetQueryRateLimit("https://api.twitter.com/1.1/statuses/user_timeline.json");

            RequestsRemaining = rateLimit.Remaining;
            ResetDateTime = rateLimit.ResetDateTime;
        }

        public List<Tweet> GetTimeline(long userID)
        {
            TweetReader tweetReader = new TweetReader(Keys.mySqlConnectionString);
            long? latestTweetID = tweetReader.FindMostRecentTweetID(userID);

            List<Tweet> untaggedTweets = new List<Tweet>();

            while (RequestsRemaining < 10)
            {
                Console.WriteLine("Less than 10 requests remaining in window.");
                Console.WriteLine("We're going to wait 15 minutes before making any more requests...");

                Thread.Sleep(90 * 1000 * 10); // Wait 900,000 milliseconds, i.e. 15 minutes

                var newRateLimit = Tweetinvi.RateLimit.GetQueryRateLimit("https://api.twitter.com/1.1/statuses/user_timeline.json");
                RequestsRemaining = newRateLimit.Remaining;
                ResetDateTime = newRateLimit.ResetDateTime;
            }

            if (latestTweetID == null)
            {
                Console.WriteLine($"Requests remaining: {RequestsRemaining}");
                untaggedTweets = Historical(userID);
            }
            else
            {
                Console.WriteLine($"Requests remaining: {RequestsRemaining}");
                untaggedTweets = Current(userID, latestTweetID.Value);
            }
            return untaggedTweets;
        }

        public List<Tweet> Current(long userID, long latestTweetID)
        {
            UserTimelineParameters timelineParameters = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                IncludeRTS = false,
                SinceId = latestTweetID
            };

            Task<IEnumerable<ITweet>> userTimlineAsync =
                Tweetinvi.Sync.ExecuteTaskAsync(() =>
                {
                    return Tweetinvi.Timeline.GetUserTimeline(userID, timelineParameters);
                });

            RequestsRemaining--;

            var twitterResponse = userTimlineAsync.Result;
            var latestException = Tweetinvi.ExceptionHandler.GetLastException();

            if (latestException != null)
            {
                throw new InvalidOperationException
                    ($"{latestException.StatusCode} : {latestException.TwitterDescription}");
            }
            else if (twitterResponse == null || !twitterResponse.Any())
            {
                Console.WriteLine
                    ($"No new tweets were returned for user id {userID} since {latestTweetID}.");
                return null; ;
            }
            else
            {
                List<Tweet> tweets = new List<Tweet>();

                foreach (var tweet in twitterResponse)
                {
                    tweets.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                         tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                         tweet.FullText));
                }

                DataToFile.Write(userID, tweets, twitterResponse, "Current");
                return tweets;
            }
        }

        // Iterate through a user's timeline to capture their latest 500 tweets
        // as a means of gathering a large set of initial data.

        public List<Tweet> Historical(long userID)
        {
            UserTimelineParameters timelineParameters = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                IncludeRTS = false
            };

            Task<IEnumerable<ITweet>> userTimlineAsync =
                Tweetinvi.Sync.ExecuteTaskAsync(() =>
                {
                    return Tweetinvi.Timeline.GetUserTimeline(userID, timelineParameters);
                });

            RequestsRemaining--;

            var lastResponse = userTimlineAsync.Result;
            var latestException = Tweetinvi.ExceptionHandler.GetLastException();

            if (latestException != null)
            {
                throw new InvalidOperationException
                    ($"{latestException.StatusCode} : {latestException.TwitterDescription}");
            }
            else if (lastResponse == null || !lastResponse.Any())
            {
                Console.WriteLine($"No tweets were returned for user id {userID}.");
                return null;
            }
            else
            {
                List<Tweet> allTweets = new List<Tweet>();

                foreach (var tweet in lastResponse)
                {
                    allTweets.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                            tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                            tweet.FullText));
                }

                while (lastResponse.ToArray().Length > 0 && allTweets.Count <= 500 && RequestsRemaining >= 10)
                {
                    // Max ID set to lowest tweet ID in our collection (i.e. the oldest tweet) minus 1.
                    // This number serves as a point of reference for requesting tweets older than those
                    // Twitter previously returned.

                    long maxTweetID = allTweets.Min(x => x.TweetID) - 1;

                    timelineParameters = new UserTimelineParameters
                    {
                        MaximumNumberOfTweetsToRetrieve = 200,
                        IncludeRTS = false,
                        MaxId = maxTweetID
                    };

                    userTimlineAsync = Tweetinvi.Sync.ExecuteTaskAsync(() =>
                    {
                        return Tweetinvi.Timeline.GetUserTimeline(userID, timelineParameters);
                    });

                    RequestsRemaining--;

                    lastResponse = userTimlineAsync.Result;

                    foreach (var tweet in lastResponse)
                    {
                        allTweets.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                                tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                                tweet.FullText));
                    }
                }

                if (RequestsRemaining < 10)
                {
                    Console.WriteLine("Less than 10 requests remaining; held off further iterations");
                }

                DataToFile.Write(userID, allTweets, lastResponse, "Historical");
                return allTweets;
            }
        }
    }
}
