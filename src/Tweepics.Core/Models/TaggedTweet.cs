using System.Collections.Generic;

namespace Tweepics.Core.Models
{
    public class TaggedTweet
    {
        public long TweetID { get; set; }
        public List<string> TagID { get; set; }

        public TaggedTweet(long tweetID, List<string> tagID)
        {
            TweetID = tweetID;
            TagID = tagID;
        }
    }
}
