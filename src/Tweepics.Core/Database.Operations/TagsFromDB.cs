using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Tag;

namespace Tweepics.Core.Database.Operations
{
    public class TagsFromDB
    {
        // Connection string as parameter for use by Tweepics.Web
        // with its own config file

        public List<Tags> Read(string connectionString)
        {
            List<Tags> tags = new List<Tags>();

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
                string tagNotCapitalizedFirst = dataReader[1].ToString();
                string tagCapitalizedFirst = tagNotCapitalizedFirst.First().ToString().ToUpper() +
                                             tagNotCapitalizedFirst.Substring(1);

                string keywordString = dataReader[2].ToString();

                tags.Add(new Tags(id, tagCapitalizedFirst, keywordString));
            }
            conn.Close();
            return tags;
        }
    }
}
