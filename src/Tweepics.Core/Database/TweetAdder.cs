using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class TweetAdder
    {
        private readonly string _connectionString;

        public void AddUntaggedTweets(List<Tweet> untaggedTweets)
        {
            DateTime now = DateTime.Now;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                foreach (var tweet in untaggedTweets)
                {
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = @"INSERT INTO tweets (full_name, screen_name, user_id, created,
                                        tweet_id, tweet_text, url, html, added) 
                                        VALUES (@full_name, @screen_name, @user_id, @created, @tweet_id, 
                                        @tweet_text, @url, @html, @added)"
                    };
                    cmd.Parameters.Add("@full_name", MySqlDbType.VarChar).Value = tweet.FullName;
                    cmd.Parameters.Add("@screen_name", MySqlDbType.VarChar).Value = tweet.ScreenName;
                    cmd.Parameters.Add("@user_id", MySqlDbType.Int64).Value = tweet.UserId;
                    cmd.Parameters.Add("@created", MySqlDbType.DateTime).Value = tweet.Created;
                    cmd.Parameters.Add("@tweet_id", MySqlDbType.Int64).Value = tweet.TweetId;
                    cmd.Parameters.Add("@tweet_text", MySqlDbType.VarChar).Value = tweet.Text;
                    cmd.Parameters.Add("@url", MySqlDbType.VarChar).Value = tweet.Url;
                    cmd.Parameters.Add("@html", MySqlDbType.VarChar).Value = tweet.Html;
                    cmd.Parameters.Add("@added", MySqlDbType.DateTime).Value = now;

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
                
        public void AddTaggedTweets(List<TaggedTweet> taggedTweets)
        {
            DateTime now = DateTime.Now;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var tweet in taggedTweets)
                {
                    foreach (string singleTagID in tweet.TagId)
                    {
                        Guid guid = Guid.NewGuid();
                        string id = guid.ToString();

                        MySqlCommand cmd = new MySqlCommand
                        {
                            Connection = connection,
                            CommandText = @"INSERT INTO tagmap (id, tweet_id, tag_id)
                                            VALUES (@id, @tweet_id, @tag_id)"
                        };
                        cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("@tweet_id", MySqlDbType.Int64).Value = tweet.TweetId;
                        cmd.Parameters.Add("@tag_id", MySqlDbType.VarChar).Value = singleTagID;

                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            } 
        }
        
        public TweetAdder(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}