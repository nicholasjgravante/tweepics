using System;
using System.Collections.Generic;
using Tweepics.Core.Config;
using Tweepics.Core.Models;
using Tweepics.Core.Tagging;
using Tweepics.Core.Requests;
using Tweepics.Core.Database;

namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            long userID = 30354991;

            Timeline timeline = new Timeline();
            List<Tweet> untaggedTweets = new List<Tweet>();
            untaggedTweets = timeline.GetTimeline(userID);

            TweetAdder tweetAdder = new TweetAdder(Keys.mySqlConnectionString);
            tweetAdder.AddUntaggedTweets(untaggedTweets);

            TagReader tagReader = new TagReader(Keys.mySqlConnectionString);
            List<Tag> tags = new List<Tag>();
            tags = tagReader.ReadAll();

            TweetTagger tweetTagger = new TweetTagger();
            List<TaggedTweet> taggedTweets = new List<TaggedTweet>();
            taggedTweets = tweetTagger.Tag(untaggedTweets, tags);

            tweetAdder.AddTaggedTweets(taggedTweets);

            Console.WriteLine("The program ran to completion.");
            Console.ReadLine();
        }
    }
}