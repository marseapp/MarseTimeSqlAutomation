using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MarseTimeSqlAutomation
{
    class SqlHandler
    {

        public void ImportData()
        {

            //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;


            //var sqlConnection = Environment.GetEnvironmentVariable("ConnectionStrings:MyConnectionString");
            var sqlConnection = Environment.GetEnvironmentVariable("MyConnectionString");

            // Insert TimeTableModel
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                String query = "INSERT INTO TimeTableModel(IntakeId, Name, Week) VALUES(@IntakeId, @Name, @Week); ";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@IntakeId", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Week", System.Data.SqlDbType.VarChar, 50);

                        var ttmList = DataParsing.ttmList;
                        for (int i = 0; i < ttmList.Count; i++)
                        {
                            command.Parameters["@IntakeId"].Value = ttmList[i].Id;
                            command.Parameters["@Name"].Value = ttmList[i].Name;
                            command.Parameters["@Week"].Value = ttmList[i].Week;

                            command.ExecuteNonQuery();
                        }
                        connection.Close();

                    }
                }
                catch (SqlException)
                {
                    // Unable to Insert
                    return;
                }

            }

            // Insert TimeTableDetailModel
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                String query = "INSERT INTO TimeTableDetailModel(IntakeId, Date, Time, Location, Classroom, Module, Lecturer) VALUES(@IntakeId, @Date, @Time, @Location, @Classroom, @Module, @Lecturer); ";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@IntakeId", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@Date", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Time", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Location", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Classroom", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Module", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Lecturer", System.Data.SqlDbType.VarChar, 50);

                        var ttdmList = DataParsing.ttdmList;
                        for (int i = 0; i < ttdmList.Count; i++)
                        {
                            command.Parameters["@IntakeId"].Value = ttdmList[i].IntakeId;
                            command.Parameters["@Date"].Value = ttdmList[i].Date;
                            command.Parameters["@Time"].Value = ttdmList[i].Time;
                            command.Parameters["@Location"].Value = ttdmList[i].Location;
                            command.Parameters["@Classroom"].Value = ttdmList[i].Classroom;
                            command.Parameters["@Module"].Value = ttdmList[i].Module;
                            command.Parameters["@Lecturer"].Value = ttdmList[i].Lecturer;

                            command.ExecuteNonQuery();
                        }
                        connection.Close();

                    }
                }
                catch (SqlException)
                {
                    // Unable to Insert
                    return;
                }
            }
        }

        public void ClearData()
        {

            //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            //var sqlConnection = Environment.GetEnvironmentVariable("ConnectionStrings:MyConnectionString");
            var sqlConnection = Environment.GetEnvironmentVariable("MyConnectionString");

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    String query = "DELETE FROM TimeTableModel";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                    }
                }
            }
            catch (SqlException)
            {
                // Unable to Insert
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    String query = "DELETE FROM TimeTableDetailModel";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                    }
                }
            }
            catch (SqlException)
            {
                // Unable to Insert
                return;
            }
        }

        public void LogData(string description)
        {
            var sqlConnection = Environment.GetEnvironmentVariable("MyConnectionString");

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    String query = "INSERT INTO LogModel(Date, Description) VALUES(@Date, @Description); ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@Date", System.Data.SqlDbType.VarChar, 50);
                        command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar, 50);

                        command.Parameters["@Date"].Value = DateTime.Now;
                        command.Parameters["@Description"].Value = description;

                        command.ExecuteNonQuery();

                        connection.Close();

                    }
                }
            }
            catch (SqlException)
            {
                // Unable to Insert
                return;
            }
        }

        public bool CheckDifference()
        {
            List<string> oldResult = new List<string>();
            List<string> newResult = new List<string>();

            var sqlConnection = Environment.GetEnvironmentVariable("MyConnectionString");

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    String query = "SELECT DISTINCT Week FROM TimeTableModel";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    oldResult.Add(reader.GetString(0));
                                }

                            }
                        }
                        connection.Close();
                    }

                }
            }
            catch (SqlException)
            {
                // Unable to Insert
                return false;
            }
            var ttmList = DataParsing.ttmList;
            newResult = ttmList.Select(o => o.Week).Distinct().ToList();

            //Sort it
            oldResult.Sort();
            newResult.Sort();

            bool result = oldResult.SequenceEqual(newResult);

            if (result)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
