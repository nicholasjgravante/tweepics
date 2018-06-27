using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Tweepics.Parse
{
    public class ParseTimelineFromFile
    {

        // Read and use tweet data from file in lieu of additional requests
        public List<TweetData> Parse()
        {
            List<TweetData> tweets = new List<TweetData>();

            // Path to tweet_data folder
            string exePath = Environment.CurrentDirectory;
            string tweetDataFolderPath = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\..\..\tweet_data"));

            string text = File.ReadAllText($@"{tweetDataFolderPath}\TweetData_216776631_2018.06.22.13-19.txt");
            List<string> rawData = new List<string>();
            rawData = text.Split(" *****").ToList();

            foreach (string tweet in rawData)
            {
                List<string> splitRawTweet = new List<string>();
                splitRawTweet = tweet.Split(" ||||| ").ToList();

                if (splitRawTweet.Count < 6)
                    break;

                long userId = Convert.ToInt64(splitRawTweet[2]);
                DateTime dateTime = DateTime.Parse(splitRawTweet[3]);
                long tweetId = Convert.ToInt64(splitRawTweet[4]);

                tweets.Add(new TweetData(splitRawTweet[0], splitRawTweet[1], userId, dateTime, tweetId, splitRawTweet[5]));
            }

            return tweets;
        }
    }
}
