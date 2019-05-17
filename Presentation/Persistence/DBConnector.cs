using Application_layer;
using Domain;
using Persistence.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DBConnector : IConnector
    {
        private static readonly string connectionString = string.Format("Server={0}; Database={1}; User Id={2}; Password={3};", Settings.Default.dbserver, Settings.Default.dbname, Settings.Default.dbuser, Settings.Default.dbpassword);

        private readonly Dictionary<Order, int> orders = new Dictionary<Order, int>();
        private readonly Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();
        private readonly Dictionary<Assignment, int> assignments = new Dictionary<Assignment, int>();
        private readonly Dictionary<Offday, int> offdays = new Dictionary<Offday, int>();

        public bool AssignmentExists(Assignment assignment)
        {
            throw new NotImplementedException();
        }

        public Assignment CreateAssignment(Order order, Workform workform, int duration)
        {
            throw new NotImplementedException();
        }

        public Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            throw new NotImplementedException();
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            throw new NotImplementedException();
        }

        public Workteam CreateWorkteam(string foreman)
        {
            Workteam workteam;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_CreateWorkteam", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@foreman", foreman));

                int id = (int)cmd.ExecuteScalar();

                workteam = new Workteam(foreman);

                workteams.Add(workteam, id);
            }

            return workteam;
        }

        public bool DeleteAssignment(Order order, Assignment assignment)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOffday(Workteam workteam, Offday offday)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            throw new NotImplementedException();
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            throw new NotImplementedException();
        }

        public void FillOrderWithAssignments(Order order)
        {
            throw new NotImplementedException();
        }

        public void FillWorkteamWithOffdays(Workteam workteam)
        {
            throw new NotImplementedException();
        }

        public void FillWorkteamWithOrders(Workteam workteam)
        {
            throw new NotImplementedException();
        }

        public List<Assignment> GetAllAssignments()
        {
            throw new NotImplementedException();
        }

        public List<Offday> GetAllOffdays()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public List<Workteam> GetAllWorkteams()
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

            return workteams.Keys.ToList();
        }

        public bool OffdayExists(Offday offday)
        {
            throw new NotImplementedException();
        }

        public bool OrderExists(Order order)
        {
            throw new NotImplementedException();
        }

        public void SwitchOrdersPriority(Order firstOrder, Order secondOrder)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderStartDate(Order order, DateTime? startDate)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkteam(Workteam workteam, string foreman)
        {
            throw new NotImplementedException();
        }

        public bool WorkteamExists(Workteam workteam)
        {
            throw new NotImplementedException();
        }
    }
}
