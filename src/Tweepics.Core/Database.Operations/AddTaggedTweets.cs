using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Tag;
using Tweepics.Core.Config;

namespace Tweepics.Core.Database.Operations
{
    public class AddTaggedTweets
    {
        public void Add(List<TaggedTweets> taggedTweets)
        {
            string connectionString = Keys.mySqlConnectionString;
            DateTime now = DateTime.Now;

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            foreach (var tweet in taggedTweets)
            {
                foreach (string singleTagID in tweet.TagID)
                {
                    Guid guid = Guid.NewGuid();
                    string id = guid.ToString();

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = @"INSERT INTO tagmap (id, tweet_id, tag_id)
                                        VALUES (@id, @tweet_id, @tag_id)"
                    };
                    cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                    cmd.Parameters.Add("@tweet_id", MySqlDbType.Int64).Value = tweet.TweetID;
                    cmd.Parameters.Add("@tag_id", MySqlDbType.VarChar).Value = singleTagID;

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }
}
