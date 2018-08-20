using System.Collections.Generic;

namespace Tweepics.Core.Tag
{
    public class TaggedTweets
    {
        public long TweetID { get; set; }
        public List<string> TagID { get; set; }

        public TaggedTweets(long tweetID, List<string> tagID)
        {
            TweetID = tweetID;
            TagID = tagID;
        }
    }
}
