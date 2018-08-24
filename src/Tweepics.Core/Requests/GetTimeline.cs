using System.Collections.Generic;
using Tweetinvi;
using Tweepics.Core.Parse;
using Tweepics.Core.Config;

namespace Tweepics.Core.Requests
{
    public class GetTimeline
    {
        public List<TweetData> User(long userID, long? mostRecentTweetID)
        {
            Auth.SetUserCredentials(Keys.twitterConsumerKey,
                                    Keys.twitterConsumerSecret,
                                    Keys.twitterAccessToken,
                                    Keys.twitterAccessTokenSecret);

            List<TweetData> untaggedTweets = new List<TweetData>();

            if(mostRecentTweetID == null)
            {
                TimelineHistorical timelineHistorical = new TimelineHistorical();
                untaggedTweets = timelineHistorical.Request(userID);

            }
            else
            {
                TimelineCurrent timelineCurrent = new TimelineCurrent();
                untaggedTweets = timelineCurrent.Request(userID, mostRecentTweetID.Value);
            }

            return untaggedTweets;
        }
    }
}
