using System.Collections.Generic;
using Tweepics.Core.Models;
using Tweepics.Core.Config;

namespace Tweepics.Core.Requests
{
    public class GetTimeline
    {
        public List<Tweet> User(long userID, long? mostRecentTweetID)
        {
            Tweetinvi.Auth.SetUserCredentials(Keys.twitterConsumerKey,
                                              Keys.twitterConsumerSecret,
                                              Keys.twitterAccessToken,
                                              Keys.twitterAccessTokenSecret);

            List<Tweet> untaggedTweets = new List<Tweet>();

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
