using System.Collections.Generic;

namespace Tweepics.Core.Models
{
    public class TaggedTweet
    {
        public long TweetId { get; set; }
        public List<string> TagId { get; set; }

        public TaggedTweet(long tweetId, List<string> tagId)
        {
            TweetId = tweetId;
            TagId = tagId;
        }
    }
}
