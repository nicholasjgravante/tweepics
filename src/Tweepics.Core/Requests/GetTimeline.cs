using System.Collections.Generic;
using Tweetinvi;
using Tweepics.Core.Parse;
using Tweepics.Core.Config;
using Tweepics.Core.Database.Operations;

namespace Tweepics.Core.Requests
{
    public class GetTimeline
    {
        public List<TweetData> User(long userID)
        {
            Auth.SetUserCredentials(Keys.twitterConsumerKey,
                                    Keys.twitterConsumerSecret,
                                    Keys.twitterAccessToken,
                                    Keys.twitterAccessTokenSecret);

            MostRecentTweetID findSinceID = new MostRecentTweetID();
            long? latestTweetID = findSinceID.Find(userID);

            List<TweetData> untaggedTweets = new List<TweetData>();

            if(latestTweetID == null)
            {
                TimelineHistorical timelineHistorical = new TimelineHistorical();
                untaggedTweets = timelineHistorical.Request(userID);

            }
            else
            {
                TimelineCurrent timelineCurrent = new TimelineCurrent();
                untaggedTweets = timelineCurrent.Request(userID, latestTweetID.Value);
            }

            return untaggedTweets;
        }
    }
}
