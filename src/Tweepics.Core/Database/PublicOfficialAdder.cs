using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class PublicOfficialAdder
    {
        private readonly string _connectionString;

        public void Add(List<PublicOfficial> officials)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var official in officials)
                {
                    Guid guid = Guid.NewGuid();
                    string officialTweepicsId = guid.ToString();

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = @"INSERT INTO representatives (tweepics_id, first_name, middle_name, 
                                        last_name, suffix, office_government, office_branch, office_state, 
                                        office_title, party, twitter_id, twitter_screen_name)
                                        VALUES (@tweepics_id, @first_name, @middle_name, @last_name, @suffix, 
                                        @office_government, @office_branch, @office_state, @office_title, 
                                        @party, @twitter_id, @twitter_screen_name)
                                        ON DUPLICATE KEY UPDATE middle_name=@middle_name"
                    };
                    cmd.Parameters.Add("@tweepics_id", MySqlDbType.VarChar).Value = officialTweepicsId;
                    cmd.Parameters.Add("@first_name", MySqlDbType.VarChar).Value = official.Name.First;
                    cmd.Parameters.Add("@middle_name", MySqlDbType.VarChar).Value = official.Name.Middle;
                    cmd.Parameters.Add("@last_name", MySqlDbType.VarChar).Value = official.Name.Last;
                    cmd.Parameters.Add("@suffix", MySqlDbType.VarChar).Value = official.Name.Suffix;
                    cmd.Parameters.Add("@office_government", MySqlDbType.VarChar).Value = official.Office.Government;
                    cmd.Parameters.Add("@office_branch", MySqlDbType.VarChar).Value = official.Office.Branch;
                    cmd.Parameters.Add("@office_state", MySqlDbType.VarChar).Value = official.Office.State;
                    cmd.Parameters.Add("@office_title", MySqlDbType.VarChar).Value = official.Office.Title;
                    cmd.Parameters.Add("@party", MySqlDbType.VarChar).Value = official.Party;
                    cmd.Parameters.Add("@twitter_id", MySqlDbType.Int64).Value = official.TwitterId;
                    cmd.Parameters.Add("@twitter_screen_name", MySqlDbType.VarChar).Value = official.TwitterScreenName;

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public PublicOfficialAdder(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}
