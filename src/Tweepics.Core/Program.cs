using System;
using System.Collections.Generic;
using Tweepics.Tag;
using Tweepics.Parse;
using Tweepics.Requests;
using Tweepics.Database.Operations;
using Tweepics.Config;

namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            TopicTagger tagTweets = new TopicTagger();
            List<TweetData> untaggedTweets = new List<TweetData>();
            List<TweetData> taggedTweets = new List<TweetData>();

            TimelineCurrent currentTweets = new TimelineCurrent();
            untaggedTweets = currentTweets.Request(25073877);

            taggedTweets = tagTweets.Tag(untaggedTweets);

            AddTweetData addTweetData = new AddTweetData();
            addTweetData.AddData(taggedTweets);

            Console.WriteLine("The program ran to completion");
            Console.ReadLine();
        }
    }
}