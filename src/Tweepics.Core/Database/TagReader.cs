using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Tag;
using Tweepics.Core.Config;

namespace Tweepics.Core.Database
{
    public class TagReader
    {
        public List<Tags> ReadAll()
        {
            using (MySqlConnection connection = new MySqlConnection(Keys.mySqlConnectionString))
            {
                connection.Open();

                List<Tags> tags = new List<Tags>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * from tweet_tags"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string id = dataReader[0].ToString();
                    string tagNotCapitalizedFirst = dataReader[1].ToString();
                    string tagCapitalizedFirst = tagNotCapitalizedFirst.First().ToString().ToUpper() +
                                                 tagNotCapitalizedFirst.Substring(1);

                    string keywordString = dataReader[2].ToString();

                    tags.Add(new Tags(id, tagCapitalizedFirst, keywordString));
                }
                dataReader.Close();
                connection.Close();

                return tags;
            }
        }
    }
}
