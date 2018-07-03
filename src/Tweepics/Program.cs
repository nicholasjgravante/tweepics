using System;
using System.Collections.Generic;
using Tweepics.Tag;
using Tweepics.Parse;
using Tweepics.Requests;

namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            TopicTagger tagTweets = new TopicTagger();
            List<TweetData> untaggedTweets = new List<TweetData>();
            List<TweetData> taggedTweets = new List<TweetData>();

            TimelineHistorical currentTweets = new TimelineHistorical();
            untaggedTweets = currentTweets.Request(216776631);

            taggedTweets = tagTweets.Tag(untaggedTweets);

            Console.WriteLine("The program ran to completion");
            Console.ReadLine();
        }
    }
}
