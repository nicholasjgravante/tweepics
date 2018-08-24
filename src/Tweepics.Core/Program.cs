using System;
using System.Collections.Generic;
using Tweepics.Core.Tag;
using Tweepics.Core.Parse;
using Tweepics.Core.Requests;
using Tweepics.Core.Database;

namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            long userID = 30354991; 

            TweetReader tweetReader = new TweetReader();
            long? mostRecentTweetID = tweetReader.FindMostRecentTweetID(userID);

            GetTimeline getTimeline = new GetTimeline();
            List<TweetData> untaggedTweets = new List<TweetData>();
            untaggedTweets = getTimeline.User(userID, mostRecentTweetID);

            TweetAdder tweetAdder = new TweetAdder();
            tweetAdder.AddUntaggedTweets(untaggedTweets);

            TagReader tagReader = new TagReader();
            List<Tags> tags = new List<Tags>();
            tags = tagReader.ReadAll();

            TweetTagger tweetTagger = new TweetTagger();
            List<TaggedTweets> taggedTweets = new List<TaggedTweets>();
            taggedTweets = tweetTagger.Tag(untaggedTweets, tags);

            tweetAdder.AddTaggedTweets(taggedTweets);

            Console.WriteLine("The program ran to completion.");
            Console.ReadLine();
        }
    }
}