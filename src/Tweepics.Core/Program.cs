using System;
using System.Collections.Generic;
using Tweepics.Core.Tag;
using Tweepics.Core.Parse;
using Tweepics.Core.Requests;
using Tweepics.Core.Database.Operations;


namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            GetTimeline getTimeline = new GetTimeline();
            List<TweetData> untaggedTweets = new List<TweetData>();
            untaggedTweets = getTimeline.User(970207298);

            AddTweetData addUntaggedTweets = new AddTweetData();
            addUntaggedTweets.Add(untaggedTweets);

            TopicTagger topicTagger = new TopicTagger();
            List<TaggedTweets> taggedTweets = new List<TaggedTweets>();
            taggedTweets = topicTagger.Tag(untaggedTweets);

            AddTaggedTweets addTaggedTweets = new AddTaggedTweets();
            addTaggedTweets.Add(taggedTweets);

            Console.WriteLine("The program ran to completion.");
            Console.ReadLine();
        }
    }
}