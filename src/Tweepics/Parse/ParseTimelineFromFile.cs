using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Tweepics.Parse
{
    // Read and use tweet data from file in lieu of additional Twitter requests.

    public class ParseTimelineFromFile
    {
        public List<TweetData> Parse(string tweetDataFolderPath, string fileName)
        {
            string text = File.ReadAllText($@"{tweetDataFolderPath}\{fileName}");
            List<string> rawData = new List<string>();
            rawData = text.Split(" *****").ToList();

            List<TweetData> tweets = new List<TweetData>();

            foreach (string tweet in rawData)
            {
                List<string> splitRawTweet = new List<string>();
                splitRawTweet = tweet.Split(" ||||| ").ToList();

                if (splitRawTweet.Count < 6)
                    break;

                long userId = Convert.ToInt64(splitRawTweet[2]);
                DateTime dateTime = DateTime.Parse(splitRawTweet[3]);
                long tweetId = Convert.ToInt64(splitRawTweet[4]);

                tweets.Add(new TweetData(splitRawTweet[0], splitRawTweet[1], userId, 
                                         dateTime, tweetId, splitRawTweet[5]));
            }
            return tweets;
        }
    }
}