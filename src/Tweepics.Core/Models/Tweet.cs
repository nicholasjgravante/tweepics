using System;
using System.Web;

namespace Tweepics.Core.Models
{
    public class Tweet
    {
        public string FullName { get; set; }
        public string ScreenName { get; set; }
        public long UserID { get; set; }
        public DateTime TweetDateTime { get; set; }
        public long TweetID { get; set; }
        public string Text { get; set; }
        public DateTime AddedDateTime { get; set; }

        public Tweet() { }

        public Tweet(string fullName, string screenName, long userID, 
                     DateTime tweetDateTime, long tweetID, string tweetText)
            : this(fullName, screenName, userID, tweetDateTime, tweetID, tweetText, new DateTime()) { }

        public Tweet(string fullName, string screenName, long userID,
                     DateTime tweetDateTime, long tweetID, string tweetText, DateTime addedDateTime)
        {
            FullName = fullName;
            ScreenName = screenName;
            UserID = userID;
            TweetDateTime = tweetDateTime;
            TweetID = tweetID;
            Text = HttpUtility.HtmlDecode(tweetText);
            AddedDateTime = addedDateTime;
        }
    }
}