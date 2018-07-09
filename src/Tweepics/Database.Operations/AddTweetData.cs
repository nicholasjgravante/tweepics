using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Parse;
using Tweepics.Config;

namespace Tweepics.Database.Operations
{
    public class AddTweetData
    {
        public void AddData(List<TweetData> tweets)
        {
            string connectionString = Keys.mySqlConnectionString;
            DateTime now = DateTime.Now;

            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                foreach (var tweet in tweets)
                {
                    List<string> tags = new List<string>();
                    tags = tweet.TopicTags;

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = @"INSERT INTO tweet_data (full_name, screen_name, user_id, tweet_datetime, 
                                        tweet_id, text, topic_tags, added_datetime)
                                        VALUES (?full_name, ?screen_name, ?user_id, ?tweet_datetime, 
                                        ?tweet_id, ?text, ?topic_tags, ?added_datetime)"
                    };
                    cmd.Parameters.AddWithValue("?full_name", tweet.FullName);
                    cmd.Parameters.AddWithValue("?screen_name", tweet.ScreenName);
                    cmd.Parameters.AddWithValue("?user_id", tweet.UserID);
                    cmd.Parameters.AddWithValue("?tweet_datetime", tweet.TweetDateTime);
                    cmd.Parameters.AddWithValue("?tweet_id", tweet.TweetID);
                    cmd.Parameters.AddWithValue("?text", tweet.Text);
                    cmd.Parameters.AddWithValue("?topic_tags", string.Join(", ", tags));
                    cmd.Parameters.AddWithValue("?added_datetime", now);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}