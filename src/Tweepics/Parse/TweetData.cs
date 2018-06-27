using System;
using System.Collections.Generic;

namespace Tweepics.Parse
{
    public class TweetData
    {
        public string FullName { get; set; }
        public string ScreenName { get; set; }
        public long UserID { get; set; }
        public DateTime TweetDateTime { get; set; }
        public long TweetID { get; set; }
        public string Text { get; set; }
        public List<string> TopicTags { get; set; }

        public TweetData() { }

        public TweetData(string tweeterName, string tweeterScreenName, long tweeterUserID,
            DateTime dateTime, long tweetID, string tweetText)
            : this(tweeterName, tweeterScreenName, tweeterUserID, dateTime, tweetID, tweetText, new List<string> { }) { }

        public TweetData(string tweeterName, string tweeterScreenName, long tweeterUserID,
            DateTime dateTime, long tweetID, string tweetText, List<string> tweetTags)
        {
            FullName = tweeterName;
            ScreenName = tweeterScreenName;
            UserID = tweeterUserID;
            TweetDateTime = dateTime;
            TweetID = tweetID;
            Text = tweetText;
            TopicTags = tweetTags;
        }
    }
}
