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

            TimelineCurrent currentTweets = new TimelineCurrent();
            untaggedTweets = currentTweets.Request(25073877);

            taggedTweets = tagTweets.Tag(untaggedTweets);

            foreach (var tweet in taggedTweets)
            {
                Console.WriteLine("TWEET CONTENT: " + tweet.Text);
                Console.WriteLine("TWEET TAGS: " + string.Join(", ", tweet.TopicTags));
                Console.WriteLine("");
            }

            Console.WriteLine("The program ran to completion");
            Console.ReadLine();
        }
    }
}