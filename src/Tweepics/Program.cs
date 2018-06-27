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
            GetTimeline timeline = new GetTimeline();
            TopicTagger tagTweets = new TopicTagger();
            List<TweetData> tweets = new List<TweetData>();

            tweets = timeline.Request(216776631);
            tagTweets.Tag(tweets);

            Console.WriteLine("The program ran to completion");
            Console.ReadLine();
        }
    }
}
