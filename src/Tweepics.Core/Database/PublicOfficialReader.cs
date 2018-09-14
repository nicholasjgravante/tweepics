using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class PublicOfficialReader
    {
        private readonly string _connectionString;

        public List<PublicOfficial> ReadFromDb()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<PublicOfficial> representatives = new List<PublicOfficial>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * from representative_information"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string tweepicsId = dataReader[0].ToString();
                    string firstName = dataReader[1].ToString();
                    string middleName = dataReader[2].ToString();
                    string lastName = dataReader[3].ToString();
                    string fullName = dataReader[4].ToString();
                    string state = dataReader[6].ToString();
                    string party = dataReader[5].ToString();
                    long twitterId = Convert.ToInt64(dataReader[7]);
                    string twitterScreenName = dataReader[8].ToString();

                    representatives.Add(new PublicOfficial(tweepicsId, firstName, middleName, lastName, 
                                                           state, party, twitterId, twitterScreenName));
                }
                dataReader.Close();
                connection.Close();

                return representatives;
            }
        }

        public List<PublicOfficial> ReadFromFile(string filePath)
        {
            string fileText = File.ReadAllText($"{filePath}");

            List<string> rawDataStrings = new List<string>();
            rawDataStrings = fileText.Split("\n").ToList();

            List<PublicOfficial> officials = new List<PublicOfficial>();

            foreach (string line in rawDataStrings)
            {
                List<string> oneLineOfData = new List<string>();
                oneLineOfData = line.Split(",").ToList();

                if (oneLineOfData.Any())
                {
                    string tweepicsId = oneLineOfData[0];
                    string firstName = oneLineOfData[1];
                    string middleName = oneLineOfData[2];
                    string lastName = oneLineOfData[3];
                    string state = oneLineOfData[4];
                    string party = oneLineOfData[5];
                    long twitterId = Convert.ToInt64(oneLineOfData[6]);
                    string twitterScreenName = oneLineOfData[7];

                    officials.Add(new PublicOfficial(tweepicsId, firstName, middleName, lastName,
                                                     state, party, twitterId, twitterScreenName));
                }
                else
                    break;
            }
            return officials;
        }

            public PublicOfficialReader(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}
