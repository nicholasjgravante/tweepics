using System.Linq;
using System.Collections.Generic;
using Tweepics.Core.Parse;
using Tweepics.Core.Database.Operations;
using Tweepics.Core.Config;

namespace Tweepics.Core.Tag
{
    class TopicTagger
    {
        public List<TaggedTweets> Tag(List<TweetData> tweets)
        {
            List<Tags> tagsKeywords = new List<Tags>();
            TagsFromDB dbTagReader = new TagsFromDB();
            tagsKeywords = dbTagReader.Read(Keys.mySqlConnectionString);

            List<TaggedTweets> taggedTweets = new List<TaggedTweets>();

            foreach (var tweet in tweets)
            {
                List<string> tagIDs = new List<string>();

                foreach (var singleTag in tagsKeywords)
                    foreach (string keyword in singleTag.KeywordList)
                        if (tweet.Text.ToLower().Contains(keyword))
                        {
                            if (!tagIDs.Contains(singleTag.ID))
                            {
                                tagIDs.Add(singleTag.ID);
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