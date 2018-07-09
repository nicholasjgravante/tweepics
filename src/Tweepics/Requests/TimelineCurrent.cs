using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweepics.Parse;
using Tweepics.Config;
using Tweepics.Database.Operations;

namespace Tweepics.Requests
{
    public class TimelineCurrent
    {
        public List<TweetData> Request(long userID)
        {
            Auth.SetUserCredentials(Keys.twitterConsumerKey,
                                    Keys.twitterConsumerSecret,
                                    Keys.twitterAccessToken,
                                    Keys.twitterAccessTokenSecret);

            FindMostRecentTweetID findSinceID = new FindMostRecentTweetID();
            long latestTweetID = findSinceID.FindID(userID);

            var timelineParametersInitial = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                IncludeRTS = false,
                SinceId = latestTweetID
            };

            var rawResponse = Timeline.GetUserTimeline(userID, timelineParametersInitial);

            List<TweetData> tweetData = new List<TweetData>();

            foreach (var tweet in rawResponse)
            {
                tweetData.Add(new TweetData(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                            tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id, 
                                            tweet.FullText));

            }

            DataToFile.Write(userID, tweetData, rawResponse, "Current");

            return tweetData;
        }
    }
}