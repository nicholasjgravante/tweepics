using System;
using System.Linq;
using System.Collections.Generic;
using Tweetinvi.Parameters;
using Tweepics.Core.Models;

namespace Tweepics.Core.Requests
{
    public class TimelineCurrent
    {
        public List<Tweet> Request(long userID, long latestTweetID)
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
    }
}