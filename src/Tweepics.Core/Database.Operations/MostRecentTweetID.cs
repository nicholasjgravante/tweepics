using System;
using MySql.Data.MySqlClient;
using Tweepics.Core.Config;

namespace Tweepics.Core.Database.Operations
{
    public class MostRecentTweetID
    {
        public long? Find(long userID)
        {
            string connectionString = Keys.mySqlConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MySqlCommand cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"SELECT MAX(tweet_id) FROM tweet_data WHERE user_id = @user_id"
            };
            cmd.Parameters.AddWithValue("@user_id", userID);

            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();

            if (dataReader[0] == DBNull.Value)
            {
                return null;
            }

            long mostRecentTweetID = Convert.ToInt64(dataReader[0]);

            dataReader.Close();
            conn.Close();

            return mostRecentTweetID;
        }
    }
}