using System;
using System.Linq;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweepics.Parse;
using Tweepics.Config;

namespace Tweepics.Requests
{
    // Iterate through a user's timeline to capture their latest 500 tweets
    // as a means of gathering a large set of initial data.

    public class TimelineHistorical
    {
        public List<TweetData> Request(long userID)
        {
            Auth.SetUserCredentials(Keys.twitterConsumerKey, 
                                    Keys.twitterConsumerSecret, 
                                    Keys.twitterAccessToken, 
                                    Keys.twitterAccessTokenSecret);

            var timelineParametersInitial = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                IncludeRTS = false
            };

            var rawResponse = Timeline.GetUserTimeline(userID, timelineParametersInitial);

            List<TweetData> tweetData = new List<TweetData>();

            foreach (var tweet in rawResponse)
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
                var timelineParametersNew = new UserTimelineParameters
                {
                    MaximumNumberOfTweetsToRetrieve = 200,
                    IncludeRTS = false,
                    MaxId = maxTweetID
                };

                var tweetsLaterPulls = Timeline.GetUserTimeline(userID, timelineParametersNew);

                foreach (var tweet in tweetsLaterPulls)
                {
                    tweetData.Add(new TweetData(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, 
                                                tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id, 
                                                tweet.FullText));
                }
            }

            DataToFile.Write(userID, tweetData, rawResponse, "Historical");

            return tweetData;
        }
    }
}