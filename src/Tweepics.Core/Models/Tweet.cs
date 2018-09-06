using System;
using System.Web;

namespace Tweepics.Core.Models
{
    public class Tweet
    {
        public string FullName { get; set; }
        public string ScreenName { get; set; }
        public long UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public long TweetID { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }
        public DateTime AddedToDatabaseAt { get; set; }

        public Tweet() { }

        public Tweet(string fullName, string screenName, long userID, DateTime tweetDateTime, 
                     long tweetID, string text, string url, string html)
            : this(fullName, screenName, userID, tweetDateTime, tweetID, text, 
                   url, html, new DateTime()) { }

        public Tweet(string fullName, string screenName, long userID, DateTime tweetDateTime, 
                     long tweetID, string text, string url, string html, 
                     DateTime addedDateTime)
        {
            FullName = fullName;
            ScreenName = screenName;
            UserID = userID;
            CreatedAt = tweetDateTime;
            TweetID = tweetID;
            Text = HttpUtility.HtmlDecode(text);
            Url = url;
            Html = html;
            AddedToDatabaseAt = addedDateTime;
        }
    }
}