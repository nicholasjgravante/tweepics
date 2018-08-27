using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public List<Tweet> GetTimeline(long userID)
        {
            TweetReader tweetReader = new TweetReader(Keys.mySqlConnectionString);
            long? latestTweetID = tweetReader.FindMostRecentTweetID(userID);

            List<Tweet> untaggedTweets = new List<Tweet>();

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

            var twitterResponse = Tweetinvi.Timeline.GetUserTimeline(userID, timelineParameters);
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
                List<Tweet> tweetData = new List<Tweet>();

                foreach (var tweet in twitterResponse)
                {
                    tweetData.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                            tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                            tweet.FullText));
                }

                DataToFile.Write(userID, tweetData, twitterResponse, "Current");
                return tweetData;
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

            var twitterResponse = Tweetinvi.Timeline.GetUserTimeline(userID, timelineParameters);
            var latestException = Tweetinvi.ExceptionHandler.GetLastException();

            if (latestException != null)
            {
                throw new InvalidOperationException
                    ($"{latestException.StatusCode} : {latestException.TwitterDescription}");
            }
            else if (twitterResponse == null || !twitterResponse.Any())
            {
                Console.WriteLine($"No tweets were returned for user id {userID}.");
                return null;
            }
            else
            {
                List<Tweet> tweetData = new List<Tweet>();

                foreach (var tweet in twitterResponse)
                {
                    tweetData.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                            tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                            tweet.FullText));
                }

                while (tweetData.Count <= 500)
                {
                    // Max ID set to lowest tweet ID in our collection (i.e. the oldest tweet) minus 1.
                    // This number serves as a point of reference for requesting tweets older than those
                    // Twitter previously returned.

                    long maxTweetID = tweetData.Min(x => x.TweetID) - 1;
                    timelineParameters = new UserTimelineParameters
                    {
                        MaximumNumberOfTweetsToRetrieve = 200,
                        IncludeRTS = false,
                        MaxId = maxTweetID
                    };

                    var tweetsLaterPulls = Tweetinvi.Timeline.GetUserTimeline(userID, timelineParameters);

                    foreach (var tweet in tweetsLaterPulls)
                    {
                        tweetData.Add(new Tweet(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                                tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                                tweet.FullText));
                    }
                }

                DataToFile.Write(userID, tweetData, twitterResponse, "Historical");
                return tweetData;
            }
        }
    }
}
