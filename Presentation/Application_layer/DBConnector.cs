using Application_layer.Properties;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer
{
    public class DBConnector
    {
        private static readonly string connectionString = string.Format("Server={0}; Database={1}; User Id={2}; Password={3};", Settings.Default.dbserver, Settings.Default.dbname, Settings.Default.dbuser, Settings.Default.dbpassword);

        internal DBConnector()
        {

        }

        /// <summary>
        /// Inserts all missing workteams in the repository provided with the ID of the workteam in the key of the repository.
        /// However, it keeps all deleted workteams if they're removed from the database after getting them.
        /// </summary>
        /// <param name="workteams"></param>
        internal void GetAllWorkteams(Dictionary<Workteam, int> workteams)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllWorkteams", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        string foreman = (string)reader["foreman"];

                        if (!workteams.ContainsValue(id))
                        {
                            Workteam workteam = new Workteam(foreman);

                            workteams.Add(workteam, id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the Workteam into the database. Adds the workteam to the Dictionary provided and inserts the database ID inserted for the workteam
        /// </summary>
        /// <param name="workteams"></param>
        /// <param name="workteam"></param>
        internal void CreateWorkteam(Dictionary<Workteam, int> workteams, Workteam workteam)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_CreateWorkteam", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@foreman", workteam.Foreman));

                int id = (int)cmd.ExecuteScalar();
                
                workteams.Add(workteam, id);
            }
        }

        internal void CreateOrder(Dictionary<Order, int> orders, Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_CreateOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                throw new NotImplementedException();
                /*cmd.Parameters.Add(new SqlParameter("@foreman", workteam.Foreman));

                int id = (int)cmd.ExecuteScalar();

                workteams.Add(workteam, id);*/
            }
        }

        public static void CLEARTABLES()
        {

        }
    }
}
