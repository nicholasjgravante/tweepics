using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Tag;
using Tweepics.Config;

namespace Tweepics.Database.Operations
{
    public class ReadTagsFromDB
    {
        public List<Tags> Read()
        {
            List<Tags> tags = new List<Tags>();

            try
            {
                string connectionString = Keys.mySqlConnectionString;
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * from tweet_tags"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string id = dataReader[0].ToString();
                    string tag = dataReader[1].ToString();
                    string keywordString = dataReader[2].ToString();

                    tags.Add(new Tags(id, tag, keywordString));
                }
                return tags;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
