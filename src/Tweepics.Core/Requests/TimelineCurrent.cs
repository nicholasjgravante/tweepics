using System;
using System.Linq;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweepics.Core.Parse;
using Tweepics.Core.Config;
using Tweetinvi.Exceptions;

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
            var latestException = ExceptionHandler.GetLastException();

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
        }
    }
}