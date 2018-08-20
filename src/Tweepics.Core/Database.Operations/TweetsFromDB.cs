using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Parse;

namespace Tweepics.Core.Database.Operations
{
    public class TweetsFromDB
    {
        // Connection string as parameter for use by Tweepics.Web
        // with its own config file

        public List<TweetData> Read(string connectionString)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MySqlCommand cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"SELECT * FROM tweet_data"
            };

            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();

            List<TweetData> tweets = new List<TweetData>();

            while (dataReader.Read())
            {
                string fullName = dataReader[0].ToString();
                string screenName = dataReader[1].ToString();
                long userID = Convert.ToInt64(dataReader[2]);
                DateTime tweetDateTime = Convert.ToDateTime(dataReader[3]);
                long tweetID = Convert.ToInt64(dataReader[4]);
                string text = dataReader[5].ToString();
                DateTime addedDateTime = Convert.ToDateTime(dataReader[6]);

                tweets.Add(new TweetData(fullName, screenName, userID, tweetDateTime, 
                                         tweetID, text, addedDateTime));
            }
            dataReader.Close();
            conn.Close();

            return tweets;
        }
    }
}