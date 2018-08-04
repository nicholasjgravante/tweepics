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
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = @"INSERT INTO tweet_data (full_name, screen_name, user_id, tweet_datetime, 
                                        tweet_id, tweet_text, topic_tags, added_datetime)
                                        VALUES (@full_name, @screen_name, @user_id, @tweet_datetime, 
                                        @tweet_id, @tweet_text, @topic_tags, @added_datetime)"
                    };
                    cmd.Parameters.Add("@full_name", MySqlDbType.VarChar).Value = tweet.FullName;
                    cmd.Parameters.Add("@screen_name", MySqlDbType.VarChar).Value = tweet.ScreenName;
                    cmd.Parameters.Add("@user_id", MySqlDbType.Int64).Value = tweet.UserID;
                    cmd.Parameters.Add("@tweet_datetime", MySqlDbType.DateTime).Value = tweet.TweetDateTime;
                    cmd.Parameters.Add("@tweet_id", MySqlDbType.Int64).Value = tweet.TweetID;
                    cmd.Parameters.Add("@tweet_text", MySqlDbType.VarChar).Value = tweet.Text;
                    cmd.Parameters.Add("@topic_tags", MySqlDbType.VarChar).Value = string.Join(", ", tweet.TagIDs);
                    cmd.Parameters.Add("@added_datetime", MySqlDbType.DateTime).Value = now;

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}