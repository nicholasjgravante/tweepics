using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;
using Tweepics.Core.Parse;

namespace Tweepics.Core.Requests
{
    public class DataToFile
    {
        public static void Write(long userID, List<TweetData> tweetData, 
                                 IEnumerable<ITweet> rawTimelineData, string currentOrHistorical)
        {
            string exePath = Environment.CurrentDirectory;
            string tweetDataFolderPath = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\..\..\tweet_data"));
            string now = DateTime.Now.ToString("yyyy.MM.dd.HH-mm");

            using (StreamWriter tweetDataFile = new StreamWriter 
                                                ($@"{tweetDataFolderPath}\{currentOrHistorical}TweetData_{userID}_{now}.txt",
                                                 false, Encoding.Unicode))
            {
                foreach (var tweet in tweetData)
                {
                    tweetDataFile.WriteLine($"{tweet.FullName} ||||| {tweet.ScreenName} ||||| {tweet.UserID} ||||| " +
                        $"{tweet.TweetDateTime} ||||| {tweet.TweetID} ||||| {tweet.Text} *****");
                }
            }

            var jsonTweets = JsonSerializer.ToJson(rawTimelineData);

            using (StreamWriter jsonFile = new StreamWriter
                                            ($@"{tweetDataFolderPath}\{currentOrHistorical}JSON_{userID}_{now}.txt",
                                             false, Encoding.Unicode))
            {
                jsonFile.WriteLine(jsonTweets);
            }
        }
    }
}