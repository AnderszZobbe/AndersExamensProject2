using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Persistence.Properties;

namespace Persistence
{
    internal class DataProvider : IDataProvider
    {
        private static readonly string connectionString = string.Format("Server={0}; Database={1}; User Id={2}; Password={3};", Settings.Default.dbserver, Settings.Default.dbname, Settings.Default.dbuser, Settings.Default.dbpassword);

        private int ExecuteScalarInt(string storedProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                return (int)cmd.ExecuteScalar();
            }
        }

        private void ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                cmd.ExecuteNonQuery();
            }
        }

        //private SqlDataReader ExecuteReader(string storedProcedure, params SqlParameter[] parameters)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand cmd = new SqlCommand(storedProcedure, connection)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        foreach (SqlParameter parameter in parameters)
        //        {
        //            cmd.Parameters.Add(parameter);
        //        }

        //        return cmd.ExecuteReader();
        //    }
        //}

        public KeyValuePair<Workteam, int> CreateWorkteam(string foreman)
        {
            int id = ExecuteScalarInt(
                "CS02PExam_CreateWorkteam",
                new SqlParameter("@foreman", foreman));

            return new KeyValuePair<Workteam, int>(new Workteam(foreman), id);
        }

        public void DeleteWorkteam(int id)
        {
            ExecuteNonQuery(
                "CS02PExam_DeleteWorkteam",
                new SqlParameter("@id", id));
        }

        public Dictionary<Workteam, int> GetAllWorkteams()
        {
            Dictionary<Workteam, int> workteams = new Dictionary<Workteam, int>();

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

                        workteams.Add(new Workteam(foreman), id);
                    }
                }
            }

            return workteams;
        }

        public void UpdateWorkteamForeman(int id, string foreman)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateWorkteamForeman",
                new SqlParameter("@id", id),
                new SqlParameter("@foreman", foreman));
        }

        public KeyValuePair<Offday, int> CreateOffday(int workteam, OffdayReason reason, DateTime startDate, int duration)
        {
            int id = ExecuteScalarInt(
                "CS02PExam_CreateOffday",
                new SqlParameter("@workteam", workteam),
                new SqlParameter("@reason", reason),
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@duration", duration));

            return new KeyValuePair<Offday, int>(new Offday(reason, startDate, duration), id);
        }

        public Dictionary<Offday, int> GetAllOffdaysByWorkteam(int workteam)
        {
            Dictionary<Offday, int> offdays = new Dictionary<Offday, int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllOffdaysByWorkteam", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@workteam", workteam));

                SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        OffdayReason reason = (OffdayReason)reader["reason"];
                        DateTime startDate = (DateTime)reader["startDate"];
                        int duration = (int)reader["duration"];

                        Offday offday = new Offday(reason, startDate, duration);

                        offdays.Add(offday, id);
                    }
                }
            }

            return offdays;
        }

        public void DeleteOffday(int id)
        {
            ExecuteNonQuery(
                "CS02PExam_DeleteOffday",
                new SqlParameter("@id", id));
        }

        public KeyValuePair<Assignment, int> CreateAssignment(int order, Workform workform, int duration)
        {
            int id = ExecuteScalarInt(
                "CS02PExam_CreateAssignment",
                new SqlParameter("@order", order),
                new SqlParameter("@workform", workform),
                new SqlParameter("@duration", duration));

            return new KeyValuePair<Assignment, int>(new Assignment(workform, duration), id);
        }

        public Dictionary<Assignment, int> GetAllAssignmentsByOrder(int order)
        {
            Dictionary<Assignment, int> assignments = new Dictionary<Assignment, int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllAssignmentsByOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@order", order));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        Workform workform = (Workform)reader["workform"];
                        int duration = (int)reader["duration"];

                        Assignment assignment = new Assignment(workform, duration);

                        assignments.Add(assignment, id);
                    }
                }
            }

            return assignments;
        }

        public void DeleteAssignment(int assignment)
        {
            ExecuteNonQuery(
                "CS02PExam_DeleteAssignment",
                new SqlParameter("@id", assignment));
        }

        public KeyValuePair<Order, int> CreateOrder(int workteam, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            int id = ExecuteScalarInt(
                "CS02PExam_CreateOrder",
                new SqlParameter("@workteam", workteam),
                new SqlParameter("@orderNumber", orderNumber),
                new SqlParameter("@address", address),
                new SqlParameter("@remark", remark),
                new SqlParameter("@area", area),
                new SqlParameter("@amount", amount),
                new SqlParameter("@prescription", prescription),
                new SqlParameter("@deadline", deadline),
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@customer", customer),
                new SqlParameter("@machine", machine),
                new SqlParameter("@asphaltWork", asphaltWork));

            Order order = new Order(orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

            return new KeyValuePair<Order, int>(order, id);
        }

        public Dictionary<Order, int> GetAllOrdersByWorkteam(int workteam)
        {
            Dictionary<Order, int> orders = new Dictionary<Order, int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllOrdersByWorkteam", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@workteam", workteam));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        int? orderNumber = reader["orderNumber"] as int? ?? null;
                        string address = (string)reader["address"];
                        string remark = (string)reader["remark"];
                        int? area = reader["area"] as int? ?? null;
                        int? amount = reader["amount"] as int? ?? null;
                        string prescription = (string)reader["prescription"];
                        DateTime? deadline = reader["deadline"] as DateTime? ?? null;
                        DateTime? startDate = reader["startDate"] as DateTime? ?? null;
                        string customer = (string)reader["customer"];
                        string machine = (string)reader["machine"];
                        string asphaltWork = (string)reader["asphaltWork"];

                        Order order = new Order(orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

                        orders.Add(order, id);
                    }
                }
            }

            return orders;
        }

        public void SwapOrderPriorities(int firstOrder, int secondOrder)
        {
            int firstOrderPriority = ExecuteScalarInt(
                "CS02PExam_GetOrderPriority",
                new SqlParameter("@id", firstOrder));
            int secondOrderPriority = ExecuteScalarInt(
                "CS02PExam_GetOrderPriority",
                new SqlParameter("@id", secondOrder));

            ExecuteNonQuery(
                "CS02PExam_SetOrderPriority",
                new SqlParameter("@id", firstOrder),
                new SqlParameter("@priority", secondOrderPriority));
            ExecuteNonQuery(
                "CS02PExam_SetOrderPriority",
                new SqlParameter("@id", secondOrder),
                new SqlParameter("@priority", firstOrderPriority));
        }

        public void UpdateOrderOrderNumber(int order, int? orderNumber)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderOrderNumber",
                new SqlParameter("@id", order),
                new SqlParameter("@orderNumber", orderNumber));
        }

        public void UpdateOrderAddress(int order, string address)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderAddress",
                new SqlParameter("@id", order),
                new SqlParameter("@address", address));
        }

        public void UpdateOrderRemark(int order, string remark)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderRemark",
                new SqlParameter("@id", order),
                new SqlParameter("@remark", remark));
        }

        public void UpdateOrderArea(int order, int? area)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderArea",
                new SqlParameter("@id", order),
                new SqlParameter("@area", area));
        }

        public void UpdateOrderAmount(int order, int? amount)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderAmount",
                new SqlParameter("@id", order),
                new SqlParameter("@amount", amount));
        }

        public void UpdateOrderPrescription(int order, string prescription)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderPrescription",
                new SqlParameter("@id", order),
                new SqlParameter("@prescription", prescription));
        }

        public void UpdateOrderDeadline(int order, DateTime? deadline)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderDeadline",
                new SqlParameter("@id", order),
                new SqlParameter("@deadline", deadline));
        }

        public void UpdateOrderStartDate(int order, DateTime? startDate)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderStartDate",
                new SqlParameter("@id", order),
                new SqlParameter("@startDate", startDate));
        }

        public void UpdateOrderCustomer(int order, string customer)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderCustomer",
                new SqlParameter("@id", order),
                new SqlParameter("@customer", customer));
        }

        public void UpdateOrderMachine(int order, string machine)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderMachine",
                new SqlParameter("@id", order),
                new SqlParameter("@machine", machine));
        }

        public void UpdateOrderAsphaltWork(int order, string asphaltWork)
        {
            ExecuteNonQuery(
                "CS02PExam_UpdateOrderAsphaltWork",
                new SqlParameter("@id", order),
                new SqlParameter("@asphaltWork", asphaltWork));
        }

        public void DeleteOrder(int order)
        {
            ExecuteNonQuery(
                "CS02PExam_DeleteOrder",
                new SqlParameter("@id", order));
        }
    }
}
