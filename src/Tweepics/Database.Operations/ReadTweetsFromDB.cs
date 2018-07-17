using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Parse;
using Tweepics.Config;

namespace Tweepics.Database.Operations
{
    public class ReadTweetsFromDB
    {
        public List<TweetData> Read()
        {
            string connectionString = Keys.mySqlConnectionString;
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

                List<string> tagIDs = new List<string> ();
                tagIDs = dataReader[6].ToString().Split(", ").ToList();

                DateTime addedDateTime = Convert.ToDateTime(dataReader[7]);

                tweets.Add(new TweetData(fullName, screenName, userID, tweetDateTime, 
                                         tweetID, text, tagIDs, addedDateTime));
            }
            dataReader.Close();
            conn.Close();

            return tweets;
        }
    }
}