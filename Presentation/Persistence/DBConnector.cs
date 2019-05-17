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
            return assignments.Keys.Any(o => o == assignment);
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
        }

        public void FillWorkteamWithOffdays(Workteam workteam)
        {
        }

        public void FillWorkteamWithOrders(Workteam workteam)
        {
        }

        public List<Assignment> GetAllAssignments()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllAssignments", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        int orderId = (int)reader["order"];
                        Workform workform = (Workform)reader["workform"];
                        int duration = (int)reader["duration"];

                        if (!assignments.ContainsValue(id))
                        {
                            Assignment assignment = new Assignment(workform, duration);

                            assignments.Add(assignment, id);

                            orders.First(o => o.Value == orderId).Key.assignments.Add(assignment);
                        }
                    }
                }
            }

            return assignments.Keys.ToList();
        }

        public List<Assignment> GetAllAssignmentsFromOrder(Order order)
        {
            return order.assignments;
        }

        public List<Offday> GetAllOffdays()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllOffdays", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        int workteamId = (int)reader["workteam"];
                        OffdayReason reason = (OffdayReason)reader["reason"];
                        DateTime startDate = (DateTime)reader["startDate"];
                        int duration = (int)reader["duration"];

                        if (!offdays.ContainsValue(id))
                        {
                            Offday offday = new Offday(reason, startDate, duration);

                            offdays.Add(offday, id);

                            workteams.First(o => o.Value == workteamId).Key.offdays.Add(offday);
                        }
                    }
                }
            }

            return offdays.Keys.ToList();
        }

        public List<Offday> GetAllOffdaysFromWorkteam(Workteam workteam)
        {
            return workteam.offdays;
        }

        public List<Order> GetAllOrders()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetAllOrders", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        int workteamId = (int)reader["workteam"];
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

                        if (!orders.ContainsValue(id))
                        {
                            Order order = new Order(orderNumber, address, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork);

                            orders.Add(order, id);

                            workteams.First(o => o.Value == workteamId).Key.orders.Add(order);
                        }
                    }
                }
            }

            return orders.Keys.ToList();
        }

        public List<Order> GetAllOrdersFromWorkteam(Workteam workteam)
        {
            return workteam.orders;
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

            // FIX THIS MADNESS, we don't want to bring everything in at once!
            GetAllOffdays();
            GetAllOrders();
            GetAllAssignments();

            return workteams.Keys.ToList();
        }

        public bool OffdayExists(Offday offday)
        {
            return offdays.Keys.Any(o => o == offday);
        }

        public bool OrderExists(Order order)
        {
            return orders.Keys.Any(o => o == order);
        }

        public void SwapOrdersPriority(Workteam workteam, Order firstOrder, Order secondOrder)
        {
            List<Order> orders = workteam.orders;

            int indexOfFirstOrder = orders.IndexOf(firstOrder);
            int indexOfSecondOrder = orders.IndexOf(secondOrder);

            orders[indexOfFirstOrder] = secondOrder;
            orders[indexOfSecondOrder] = firstOrder;

            // Server-side now
            int firstOrderPriority = GetOrderPriority(firstOrder);
            SetOrderPriority(firstOrder, GetOrderPriority(secondOrder));
            SetOrderPriority(secondOrder, firstOrderPriority);
        }

        private void SetOrderPriority(Order order, int? priority)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_UpdateOrderPriority", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", orders[order]));
                cmd.Parameters.Add(new SqlParameter("@priority", priority));

                cmd.ExecuteNonQuery();
            }
        }

        private int GetOrderPriority(Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_GetOrderPriority", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", orders[order]));

                return (int)cmd.ExecuteScalar();
            }
        }

        public void UpdateOrder(Order order, int? orderNumber, string address, string remark, int? area, int? amount, string prescription, DateTime? deadline, DateTime? startDate, string customer, string machine, string asphaltWork)
        {
            order.OrderNumber = orderNumber;
            order.Address = address;
            order.Remark = remark;
            order.Area = area;
            order.Amount = amount;
            order.Prescription = prescription;
            order.Deadline = deadline;
            order.StartDate = startDate;
            order.Customer = customer;
            order.Machine = machine;
            order.AsphaltWork = asphaltWork;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_UpdateOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", orders[order]));
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

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateOrderStartDate(Order order, DateTime? startDate)
        {
            order.StartDate = startDate;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_UpdateOrderStartDate", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", orders[order]));
                cmd.Parameters.Add(new SqlParameter("@startDate", startDate));

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateWorkteam(Workteam workteam, string foreman)
        {
            workteam.Foreman = foreman;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CS02PExam_UpdateWorkteam", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@id", workteams[workteam]));
                cmd.Parameters.Add(new SqlParameter("@foreman", foreman));

                cmd.ExecuteNonQuery();
            }
        }

        public bool WorkteamExists(Workteam workteam)
        {
            return workteams.Keys.Any(o => o == workteam);
        }

        /// <summary>
        /// Uses the master objects the supplier has a match for, if supplier has something master doesn't have, master will add that.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="master"></param>
        /// <param name="supplier"></param>
        private void Merge<T>(Dictionary<T, int> master, Dictionary<T, int> supplier)
        {
            throw new NotImplementedException();
        }
    }
}
