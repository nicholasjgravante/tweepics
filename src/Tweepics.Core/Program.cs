using System;
using System.Collections.Generic;
using Tweepics.Core.Config;
using Tweepics.Core.Models;
using Tweepics.Core.Tagging;
using Tweepics.Core.Requests;
using Tweepics.Core.Database;
using System.Net;
using System.IO;
using Tweetinvi.Logic.Model;
using Tweetinvi.Models.DTO;

namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            List<long> userIDs = new List<long>
            {
            30354991, // Kamala Harris
            29442313, // Bernie Sanders
            15808765  // Cory Booker
            };

            Timeline timeline = new Timeline();

            foreach (int ID in userIDs)
            {
                List<Tweet> untaggedTweets = new List<Tweet>();
                untaggedTweets = timeline.GetTimeline(ID);

                if (untaggedTweets == null)
                    continue;

                TweetAdder tweetAdder = new TweetAdder(Keys.mySqlConnectionString);
                tweetAdder.AddUntaggedTweets(untaggedTweets);

                Console.WriteLine($"{untaggedTweets.Count} tweets were added for user {ID}.");

                TagReader tagReader = new TagReader(Keys.mySqlConnectionString);
                List<Tag> tags = new List<Tag>();
                tags = tagReader.ReadAll();

                TweetTagger tagger = new TweetTagger();
                List<TaggedTweet> taggedTweets = new List<TaggedTweet>();
                taggedTweets = tagger.Tag(untaggedTweets, tags);

                tweetAdder.AddTaggedTweets(taggedTweets);

                Console.WriteLine($"{taggedTweets.Count} tweets were tagged and added for user {ID}.");
            }

            Console.WriteLine("The program ran to completion.");
            Console.ReadLine();
        }
    }
}