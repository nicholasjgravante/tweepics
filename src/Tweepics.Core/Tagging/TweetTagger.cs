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
                List<string> tagIDs = new List<string>();

                foreach (var tag in tags)
                    foreach (string keyword in tag.KeywordList)
                        if (tweet.Text.ToLower().Contains(keyword))
                        {
                            if (!tagIDs.Contains(tag.ID))
                            {
                                tagIDs.Add(tag.ID);
                            }
                            else
                                continue;
                        }

                if (!tagIDs.Any())
                    continue;

                taggedTweets.Add(new TaggedTweet(tweet.TweetID, tagIDs));
            }
            return taggedTweets;
        }
    }
}