using System;
using System.Linq;
using System.Collections.Generic;
using Tweepics.Parse;
using Tweepics.Database.Operations;

namespace Tweepics.Tag
{
    class TopicTagger
    {
        public List<TweetData> Tag(List<TweetData> tweets)
        {
            List<Tags> tagsKeywords = new List<Tags>();
            ReadTagsFromDB dbTagReader = new ReadTagsFromDB();
            tagsKeywords = dbTagReader.Read();

            List<TweetData> taggedTweets = new List<TweetData>();

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

                taggedTweets.Add(new TweetData(tweet.FullName, tweet.ScreenName, tweet.UserID, 
                                               tweet.TweetDateTime, tweet.TweetID, tweet.Text, 
                                               tagIDs));
            }
            return taggedTweets;
        }
    }
}