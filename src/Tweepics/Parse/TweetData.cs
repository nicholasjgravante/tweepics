using System;
using System.Web;
using System.Collections.Generic;
using Tweepics.Tag;

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
        public List<string> TagIDs { get; set; }
        public DateTime AddedDateTime { get; set; }

        public TweetData() { }

        public TweetData(string fullName, string screenName, long userID,
                         DateTime tweetDateTime, long tweetID, string text)
            : this(fullName, screenName, userID, tweetDateTime, tweetID, text, new List<string> { }, new DateTime()) { }

        public TweetData(string fullName, string screenName, long userID,
                         DateTime tweetDateTime, long tweetID, string text, List<string> tagIDs)
            : this(fullName, screenName, userID, tweetDateTime, tweetID, text, tagIDs, new DateTime()) { }

        public TweetData(string fullName, string screenName, long userID,
                         DateTime tweetDateTime, long tweetID, string text, List<string> tagIDs, DateTime addedDateTime)
        {
            FullName = fullName;
            ScreenName = screenName;
            UserID = userID;
            TweetDateTime = tweetDateTime;
            TweetID = tweetID;
            Text = HttpUtility.HtmlDecode(text);
            TagIDs = tagIDs;
            AddedDateTime = addedDateTime;
        }
    }
}