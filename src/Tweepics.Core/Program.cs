using System;
using System.Collections.Generic;
using Tweepics.Core.Config;
using Tweepics.Core.Models;
using Tweepics.Core.Tagging;
using Tweepics.Core.Requests;
using Tweepics.Core.Database;
using System.Linq;

namespace Tweepics
{
    class Program
    {
        static void Main(string[] args)
        {
            PublicOfficialReader publicOfficialReader = new PublicOfficialReader(Keys.mySqlConnectionString);
            List<PublicOfficial> publicOfficials = publicOfficialReader.ReadFromDb();

            List<long> userId = publicOfficials.Where(official => official.TwitterId != 0).Select(official => official.TwitterId).ToList();

            Timeline timeline = new Timeline();

            foreach (long id in userId)
            {
                List<Tweet> untaggedTweets = new List<Tweet>();
                untaggedTweets = timeline.GetTimeline(id);

                if (untaggedTweets == null)
                    continue;

                TweetAdder tweetAdder = new TweetAdder(Keys.mySqlConnectionString);
                tweetAdder.AddUntaggedTweets(untaggedTweets);

                Console.WriteLine($"{untaggedTweets.Count} tweets were added.");

                TagReader tagReader = new TagReader(Keys.mySqlConnectionString);
                List<Tag> tags = new List<Tag>();
                tags = tagReader.ReadAll();

                TweetTagger tagger = new TweetTagger();
                List<TaggedTweet> taggedTweets = new List<TaggedTweet>();
                taggedTweets = tagger.Tag(untaggedTweets, tags);

                tweetAdder.AddTaggedTweets(taggedTweets);

                Console.WriteLine($"{taggedTweets.Count} tweets were tagged.");
                Console.WriteLine("--------------------");
            }

            Console.WriteLine("The program ran to completion.");
            Console.ReadLine();
        }
    }
}