using System;
using System.Linq;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweepics.Core.Parse;
using Tweepics.Core.Config;

namespace Tweepics.Core.Requests
{
    // Iterate through a user's timeline to capture their latest 500 tweets
    // as a means of gathering a large set of initial data.

    public class TimelineHistorical
    {
        public List<TweetData> Request(long userID)
        {
            UserTimelineParameters timelineParameters = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                IncludeRTS = false
            };

            var twitterResponse = Timeline.GetUserTimeline(userID, timelineParameters);
            var latestException = ExceptionHandler.GetLastException();

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
                List<TweetData> tweetData = new List<TweetData>();

                foreach (var tweet in twitterResponse)
                {
                    tweetData.Add(new TweetData(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
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

                    var tweetsLaterPulls = Timeline.GetUserTimeline(userID, timelineParameters);

                    foreach (var tweet in tweetsLaterPulls)
                    {
                        tweetData.Add(new TweetData(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
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