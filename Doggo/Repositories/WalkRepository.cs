using Doggo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doggo.Repositories
{
    public class WalkRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // Pass a walker's Id to get their total duration of walks
        public int TotalDurationByWalkerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT SUM(Duration) AS Total                          
                        FROM Walks
                        WHERE WalkerId = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    int TotalDuration = 0;

                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("Total")))
                        {
                            TotalDuration = reader.GetInt32(reader.GetOrdinal("Total"));
                        }
                    };

                    reader.Close();

                    return TotalDuration / 60;
                }
            }
        }
        // Get walks of a walker along with the client's name
        public List<Walk> GetWalksByWalkerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id AS walkId, 
                                Date, 
                                Duration, 
                                o.[Name] AS client
                        FROM Walks w
                        JOIN Dog d ON w.DogId = d.Id
                        JOIN Owner o ON d.OwnerId = o.Id
                        WHERE WalkerId = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while (reader.Read())
                    {
                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("walkId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")) / 60,
                            Client = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("client"))
                            }
                        };

                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }
    }
}