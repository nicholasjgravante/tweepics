using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweepics.Parse;
using Tweepics.Config;

namespace Tweepics.Requests
{
    public class GetTimeline
    {
        public List<TweetData> Request(long userIdentifier)
        {
            Auth.SetUserCredentials(Keys.twitterConsumerKey, Keys.twitterConsumerSecret, Keys.twitterAccessToken, Keys.twitterAccessTokenSecret);

            var timelineParameters = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 100,
                IncludeRTS = false
            };

            var tweets = Timeline.GetUserTimeline(userIdentifier, timelineParameters);

            List<TweetData> tweetData = new List<TweetData>();

            foreach (var tweet in tweets)
            {
                tweetData.Add(new TweetData
                    (tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.CreatedBy.Id,
                    tweet.CreatedAt, tweet.Id, tweet.FullText));
            }

            // Write tweet data to files (in the tweet_data folder) for future reference
            string exePath = Environment.CurrentDirectory;
            string tweetDataFolderPath = Path.GetFullPath(Path.Combine(exePath, @"..\..\..\..\..\tweet_data"));
            string now = DateTime.Now.ToString("yyyy.MM.dd.HH-mm");

            using (StreamWriter classFile = new StreamWriter
                ($@"{tweetDataFolderPath}\TweetData_{userIdentifier}_{now}.txt",
                false, Encoding.Unicode))
            {
                foreach (var tweet in tweetData)
                {
                    classFile.WriteLine($"{tweet.FullName} ||||| {tweet.ScreenName} ||||| {tweet.UserID} ||||| " +
                        $"{tweet.TweetDateTime} ||||| {tweet.TweetID} ||||| {tweet.Text} *****");
                }
            }

            var jsonTweets = JsonSerializer.ToJson(tweets);

            using (StreamWriter jsonFile = new StreamWriter
                ($@"{tweetDataFolderPath}\JSON_{userIdentifier}_{now}.txt",
                false, Encoding.Unicode))
            {
                jsonFile.WriteLine(jsonTweets);
            }

            return tweetData;
        }
    }
}
