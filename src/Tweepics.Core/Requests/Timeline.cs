using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweepics.Core.Config;
using Tweepics.Core.Models;
using Tweepics.Core.Database;

namespace Tweepics.Core.Requests
{
    public class Timeline
    {
        public Timeline()
        {
            Tweetinvi.Auth.SetUserCredentials(Keys.twitterConsumerKey,
                                              Keys.twitterConsumerSecret,
                                              Keys.twitterAccessToken,
                                              Keys.twitterAccessTokenSecret);

            Tweetinvi.RateLimit.RateLimitTrackerMode = Tweetinvi.RateLimitTrackerMode.TrackAndAwait;
        }

        public List<Tweet> GetTimeline(long userID)
        {
            TweetReader tweetReader = new TweetReader(Keys.mySqlConnectionString);
            long? latestTweetID = tweetReader.FindMostRecentTweetID(userID);

            List<Tweet> untaggedTweets = new List<Tweet>();

            Console.WriteLine(Tweetinvi.RateLimit.GetCurrentCredentialsRateLimits());

            if (latestTweetID == null)
            {
                untaggedTweets = Historical(userID);
            }
            else
            {
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

                // More than 5 iterations on one user ID probably means we've encountered a problem
                // This will serve as our fail-safe for now
                int iterationCounter = 0;

                while (lastResponse.ToArray().Length > 0 && allTweets.Count <= 500 && iterationCounter <= 5)
                {
                    iterationCounter++;

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

                    lastResponse = userTimlineAsync.Result;

                    foreach (var tweet in lastResponse)
                    {
                        allTweets.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                                tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                                tweet.FullText));
                    }
                }

                DataToFile.Write(userID, allTweets, lastResponse, "Historical");
                return allTweets;
            }
        }
    }
}
