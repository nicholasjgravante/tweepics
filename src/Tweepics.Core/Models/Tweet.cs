using System;
using System.Web;

namespace Tweepics.Core.Models
{
    public class Tweet
    {
        public string FullName { get; set; }
        public string ScreenName { get; set; }
        public long UserId { get; set; }
        public DateTime Created { get; set; }
        public long TweetId { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }
        public DateTime Added { get; set; }

        public Tweet() { }

        public Tweet(string fullName, string screenName, long userId, DateTime created, long tweetId, 
                     string text, string url, string html)
            : this(fullName, screenName, userId, created, tweetId, text, url, html, new DateTime()) { }

        public Tweet(string fullName, string screenName, long userId, DateTime created, long tweetId, 
                     string text, string url, string html, DateTime added)
        {
            FullName = fullName;
            ScreenName = screenName;
            UserId = userId;
            Created = created;
            TweetId = tweetId;
            Text = HttpUtility.HtmlDecode(text);
            Url = url;
            Html = html;
            Added = added;
        }
    }
}