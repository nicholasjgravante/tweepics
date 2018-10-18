using System.Linq;
using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Core.Tagging
{
    class TweetTagger
    {
        public List<TaggedTweet> Tag(List<Tweet> untaggedTweets, List<Tag> tags)
        {
            List<TaggedTweet> taggedTweets = new List<TaggedTweet>();

            foreach (var tweet in untaggedTweets)
            {
                List<string> tagIds = new List<string>();

                foreach (var tag in tags)
                    foreach (string keyword in tag.KeywordList)
                        if (tweet.Text.ToLower().Contains(keyword))
                        {
                            if (!tagIds.Contains(tag.Id))
                            {
                                tagIds.Add(tag.Id);
                            }
                            else
                                continue;
                        }

                if (!tagIds.Any())
                    continue;

                taggedTweets.Add(new TaggedTweet(tweet.TweetId, tagIds));
            }
            return taggedTweets;
        }
    }
}