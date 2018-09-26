using System.Collections.Generic;

namespace Tweepics.Core.Models
{
    public class TweetsByOfficial
    {
        public List<Tweet> Tweets { get; set; }
        public PublicOfficial PublicOfficial { get; set; }

        public TweetsByOfficial(List<Tweet> tweets, PublicOfficial official)
        {
            Tweets = tweets;
            PublicOfficial = official;
        }
    }
}
