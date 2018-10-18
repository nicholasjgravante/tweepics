using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Tweetinvi.Models;
using Tweepics.Core.Models;

namespace Tweepics.Core.Requests
{
    public class DataToFile
    {
        public static void Write(long userId, List<Tweet> tweets, IEnumerable<ITweet> rawTimelineData, 
                                 string currentOrHistorical)
        {
            string exePath = Environment.CurrentDirectory;
            string tweetDataFolderPath = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\..\..\tweet_data"));
            string now = DateTime.Now.ToString("yyyy.MM.dd.HH-mm");

            using (StreamWriter tweetDataFile = new StreamWriter 
                ($@"{tweetDataFolderPath}\{currentOrHistorical}TweetData_{userId}_{now}.txt",
                false, Encoding.Unicode))
            {
                foreach (var tweet in tweets)
                {
                    tweetDataFile.WriteLine($"{tweet.FullName} ||||| {tweet.ScreenName} ||||| {tweet.UserId} ||||| " +
                        $"{tweet.Created} ||||| {tweet.TweetId} ||||| {tweet.Text} ||||| {tweet.Url} ||||| {tweet.Html} *****");
                }
            }

            var jsonTweets = Tweetinvi.JsonSerializer.ToJson(rawTimelineData);

            using (StreamWriter jsonFile = new StreamWriter
                ($@"{tweetDataFolderPath}\{currentOrHistorical}JSON_{userId}_{now}.txt",
                false, Encoding.Unicode))
            {
                jsonFile.WriteLine(jsonTweets);
            }
        }
    }
}