using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Core.Parse
{
    // Read and use tweet data from file in lieu of additional Twitter requests.

    public class TimelineFromFile
    {
        public List<Tweet> Parse(string tweetDataFolderPath, string fileName)
        {
            string text = File.ReadAllText($@"{tweetDataFolderPath}\{fileName}");
            List<string> rawData = new List<string>();
            rawData = text.Split(" *****").ToList();

            List<Tweet> tweets = new List<Tweet>();

            foreach (string tweet in rawData)
            {
                List<string> splitRawTweet = new List<string>();
                splitRawTweet = tweet.Split(" ||||| ").ToList();

                if (splitRawTweet.Count < 6)
                    break;

                string fullName = splitRawTweet[0];
                string screenName = splitRawTweet[1];
                long userId = Convert.ToInt64(splitRawTweet[2]);
                DateTime tweetDateTime = DateTime.Parse(splitRawTweet[3]);
                long tweetId = Convert.ToInt64(splitRawTweet[4]);
                string tweetText = splitRawTweet[5];
                string url = splitRawTweet[6];
                string html = splitRawTweet[7];

                tweets.Add(new Tweet(fullName, screenName, userId, tweetDateTime, 
                                     tweetId, tweetText, url, html));
            }
            return tweets;
        }
    }
}