using System;
using System.Linq;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweepics.Core.Parse;

namespace Tweepics.Core.Requests
{
    public class TimelineCurrent
    {
        public List<TweetData> Request(long userID, long latestTweetID)
        {
            UserTimelineParameters timelineParameters = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                IncludeRTS = false,
                SinceId = latestTweetID
            };

            var twitterResponse = Timeline.GetUserTimeline(userID, timelineParameters);

            if (twitterResponse != null && twitterResponse.Any())
            {
                List<TweetData> tweetData = new List<TweetData>();

                foreach (var tweet in twitterResponse)
                {
                    tweetData.Add(new TweetData(tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName,
                                                tweet.CreatedBy.Id, tweet.CreatedAt, tweet.Id,
                                                tweet.FullText));
                }

                DataToFile.Write(userID, tweetData, twitterResponse, "Current");
                return tweetData;
            }
            else
            {
                throw new NullReferenceException
                    ($"No tweets were returned for user id {userID} since {latestTweetID}.");
            }
        }
    }
}