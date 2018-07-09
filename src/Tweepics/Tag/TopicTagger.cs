using System;
using System.Linq;
using System.Collections.Generic;
using Tweepics.Parse;

namespace Tweepics.Tag
{
    class TopicTagger
    {
        public List<TweetData> Tag(List<TweetData> tweets)
        {
            Dictionary<string, List<string>> tagsAndCategories = new Dictionary<string, List<string>>();
            tagsAndCategories.Add("Trump", new List<string> { "trump", "president", "white house" });
            tagsAndCategories.Add("Immigration", new List<string> { "border", "immigration", "wall" });
            tagsAndCategories.Add("Economy", new List<string> { "economy", "business", "jobs", "wages", "unemployment" });
            tagsAndCategories.Add("Russia", new List<string> { "russia", "putin", "kremlin", "russia investigation" });
            tagsAndCategories.Add("Media", new List<string> { "media", "new york times", "washington post", "fake news" });

            List<TweetData> tweetsAndTopics = new List<TweetData>();

            foreach (var tweet in tweets)
            {
                List<string> tweetTags = new List<string>();
                List<string> categories = new List<string>();
                string lowercaseTweet = tweet.Text.ToLower();

                foreach (var entry in tagsAndCategories)
                    foreach (var value in entry.Value)
                        if (lowercaseTweet.Contains(value))
                        {
                            if (!tweetTags.Contains(entry.Key))
                                tweetTags.Add(entry.Key);
                            else
                                continue;
                        }

                if (tweetTags.Any())
                {
                    tweetsAndTopics.Add(new TweetData(tweet.FullName, tweet.ScreenName, tweet.UserID,
                                                      tweet.TweetDateTime, tweet.TweetID, tweet.Text, 
                                                      tweetTags));
                }
            }
            return tweetsAndTopics;
        }
    }
}