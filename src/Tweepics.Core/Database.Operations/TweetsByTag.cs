using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Parse;

namespace Tweepics.Core.Database.Operations
{
    public class TweetsByTag
    {
        // Connection string as parameter for use by Tweepics.Web
        // with its own config file

        public List<TweetData> Find(string tag, string connectionString)
        {
            List<TweetData> tweets = new List<TweetData>();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            // (1) Find tag id from argument in tweet_tags, (2) find tweet ids from 
            // tag id in tagmap, and (3) find tweets from tweet ids in tweet_data

            MySqlCommand cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"SELECT td.* 
                                FROM tweet_data td, tweet_tags tt, tagmap tm
                                WHERE td.tweet_id = tm.tweet_id
                                AND tm.tag_id = tt.id
                                AND tt.tag = @tag"
            };
            cmd.Parameters.AddWithValue("@tag", tag.ToLower());

            MySqlDataReader dataReader = cmd.ExecuteReader();

            if (!dataReader.HasRows)
            {
                return null;
            }

            while (dataReader.Read())
            {
                string fullName = dataReader[0].ToString();
                string screenName = dataReader[1].ToString();
                long userID = Convert.ToInt64(dataReader[2]);
                DateTime tweetDateTime = Convert.ToDateTime(dataReader[3]);
                long tweetID = Convert.ToInt64(dataReader[4]);
                string tweetText = dataReader[5].ToString();

                tweets.Add(new TweetData(fullName, screenName, userID, tweetDateTime, tweetID, tweetText));
            }
            dataReader.Close();
            return tweets;
        }
    }
}
