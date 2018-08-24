using System.Linq;
using System.Collections.Generic;
using Tweepics.Core.Parse;
using Tweepics.Core.Database;
using Tweepics.Core.Config;

namespace Tweepics.Core.Tag
{
    class TweetTagger
    {
        public List<TaggedTweets> Tag(List<TweetData> untaggedTweets, List<Tags> tags)
        {
            List<TaggedTweets> taggedTweets = new List<TaggedTweets>();

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

                taggedTweets.Add(new TaggedTweets(tweet.TweetID, tagIDs));
            }
            return taggedTweets;
        }
    }
}