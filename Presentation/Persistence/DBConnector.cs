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
            return GetAllAssignments().Any(o => o == assignment);
        }

        public Assignment CreateAssignment(Order order, Workform workform, int duration)
        {
            Assignment assignment;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_CreateAssignment", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@order", orders[order]));
                cmd.Parameters.Add(new SqlParameter("@workform", workform));
                cmd.Parameters.Add(new SqlParameter("@duration", duration));

                int id = (int)cmd.ExecuteScalar();

                assignment = new Assignment(workform, duration);

                assignments.Add(assignment, id);

                order.assignments.Add(assignment);
            }

            return assignment;
        }

        public Offday CreateOffday(Workteam workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            Offday offday;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_CreateOffday", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@workteam", workteams[workteam]));
                cmd.Parameters.Add(new SqlParameter("@reason", reason));
                cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("@duration", duration));

                int id = (int)cmd.ExecuteScalar();

                offday = new Offday(reason, startDate, duration);

                offdays.Add(offday, id);

                workteam.offdays.Add(offday);
            }

            return offday;
        }

        public Order CreateOrder(Workteam workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            Order order;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_CreateOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@workteam", workteams[workteam]));
                cmd.Parameters.Add(new SqlParameter("@orderNumber", orderNumber));
                cmd.Parameters.Add(new SqlParameter("@address", address));
                cmd.Parameters.Add(new SqlParameter("@remark", remark));
                cmd.Parameters.Add(new SqlParameter("@area", area));
                cmd.Parameters.Add(new SqlParameter("@amount", amount));
                cmd.Parameters.Add(new SqlParameter("@prescription", prescription));
                cmd.Parameters.Add(new SqlParameter("@deadline", deadline));
                cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("@customer", customer));
                cmd.Parameters.Add(new SqlParameter("@machine", machine));
                cmd.Parameters.Add(new SqlParameter("@asphaltWork", asphaltWork));

                int id = (int)cmd.ExecuteScalar();

                order = new Order(orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

                orders.Add(order, id);

                workteam.orders.Add(order);
            }

            return order;
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_DeleteAssignment", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", assignments[assignment]));

                cmd.ExecuteNonQuery();
            }

            order.assignments.Remove(assignment);
            return assignments.Remove(assignment);
        }

        public bool DeleteOffday(Workteam workteam, Offday offday)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_DeleteOffday", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", offdays[offday]));

                cmd.ExecuteNonQuery();
            }

            workteam.offdays.Remove(offday);
            return offdays.Remove(offday);
        }

        public bool DeleteOrder(Workteam workteam, Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_DeleteOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", orders[order]));

                cmd.ExecuteNonQuery();
            }

            workteam.orders.Remove(order);
            return orders.Remove(order);
        }

        public bool DeleteWorkteam(Workteam workteam)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_DeleteWorkteam", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", workteams[workteam]));

                cmd.ExecuteNonQuery();
            }

            return workteams.Remove(workteam);
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
