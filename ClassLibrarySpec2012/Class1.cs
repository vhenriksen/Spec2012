using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace ClassLibrarySpec2012
{
    public class Service
    {
        static DbProviderFactory dbFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
        static DbConnection conn = dbFactory.CreateConnection();
        static DbCommand cmd = dbFactory.CreateCommand();

        static DbDataAdapter da = dbFactory.CreateDataAdapter();
        static DbDataReader reader = null;

        static string connStr = @"Data Source=localhost\SQLExpress; database=dbtools; Integrated Security=True;";

        private static Service service;

        public static Service getInstance()
        {
            if (service == null)
            {
                service = new Service();
                return service;
            }
            else
            {
                return service;
            }

        }

        private Service()
        {
            conn.ConnectionString = connStr;
        }

        public void testConnection()
        {
            try
            {
                conn.Open();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getTools()
        {
            return getDataTable("SELECT id, description FROM tool WHERE in_use = 1");
        }

        public DataTable getEmployees()
        {
            return getDataTable("SELECT id, name FROM employee WHERE is_active = 1");
        }

        public DataTable getLocations()
        {
            return getDataTable("SELECT id, location FROM location WHERE is_active = 1");
        }

        private DataTable getDataTable(string sqlCommand)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlCommand;

            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public List<string> getStartTimes()
        {
            List<string> times = new List<string>();

            for (int i = 7; i <= 15; i++)
            {
                if (i < 10) times.Add("0" + i + ":00");
                else times.Add("" + i + ":00");
            }

            return times;
        }

        public List<string> getEndTimes()
        {
            List<string> times = new List<string>();

            for (int i = 8; i <= 16; i++)
            {
                if (i < 10) times.Add("0" + i + ":00");
                else times.Add("" + i + ":00");
            }

            return times;
        }

        public DateTime findNextDate(int toolid, int hours)
        {
            DateTime start = getValidStartDate(DateTime.Parse(DateTime.Now.ToShortDateString()).AddHours(DateTime.Now.Hour + 1));
            DateTime end = getValidEndDate(start, hours);

            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = " SELECT startdate, enddate ";
            cmd.CommandText += "FROM reservation r JOIN reservationTool rt ON r.id = rt.reservationid ";
            cmd.CommandText += "WHERE rt.toolid = @toolId AND enddate > @dateStart";

            DbParameter paramToolId = dbFactory.CreateParameter();
            paramToolId.ParameterName = "@toolId";
            paramToolId.DbType = DbType.Int32;
            paramToolId.Value = toolid;
            cmd.Parameters.Add(paramToolId);

            DbParameter paramDateStart = dbFactory.CreateParameter();
            paramDateStart.ParameterName = "@dateStart";
            paramDateStart.DbType = DbType.DateTime;
            paramDateStart.Value = start;
            cmd.Parameters.Add(paramDateStart);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime resStart = (DateTime)reader["startdate"];
                    DateTime resEnd = (DateTime)reader["enddate"];

                    if (start >= resStart || end > resStart)
                    {
                        start = getValidStartDate(resEnd);
                        end = getValidEndDate(start, hours);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return start;
        }

        private DateTime getValidStartDate(DateTime start)
        {
            if (start.Hour < 7)
            {
                start = start.AddHours(-start.Hour);
                start = start.AddHours(7);
            }
            else if (start.Hour > 15)
            {
                start = start.AddDays(1);
                start = start.AddHours(-start.Hour);
                start = start.AddHours(7);
            }

            if (start.DayOfWeek == DayOfWeek.Saturday)
                start = start.AddDays(2);
            else if (start.DayOfWeek == DayOfWeek.Sunday)
                start = start.AddDays(1);

            return start;
        }

        public DateTime getValidEndDate(DateTime start, int hours)
        {
            int daysToAdd = hours / 9;
            int hoursToAdd = hours % 9;

            //Hvis fx 3 hele dage så er tiden 2 dage og 9 timer
            if (hours % 9 == 0)
            {
                daysToAdd--;
                hoursToAdd += 9;
            }

            while (daysToAdd > 0)
            {
                start = start.AddDays(1);

                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                    daysToAdd--;
            }

            //Check om timer overskrider dagen
            if (start.Hour + hoursToAdd > 16)
            {
                //I så fald hvor med hvor meange timer
                int hoursRemaining = hoursToAdd - (16 - start.Hour);

                start = start.AddDays(1);
                start = start.AddHours(-start.Hour);
                start = start.AddHours(7 + hoursRemaining);

                //Check efter justering om der er landet i en weekend
                if (start.DayOfWeek == DayOfWeek.Saturday) start = start.AddDays(2);
                else if (start.DayOfWeek == DayOfWeek.Sunday) start = start.AddDays(1);
            }
            else
            {
                start = start.AddHours(hoursToAdd);
            }

            return start; // som så nu er et gyldigt sluttidspunkt
        }

        public void deleteEmployee(int employeeid)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM employee WHERE id = @EmployeeId";

            DbParameter paramId = dbFactory.CreateParameter();
            paramId.ParameterName = "@EmployeeId";
            paramId.DbType = DbType.Int32;
            paramId.Value = employeeid;
            cmd.Parameters.Add(paramId);

            executeNonQuery(cmd);
        }

        public void deleteTool(int toolid)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM tool WHERE id = @ToolId";

            DbParameter paramId = dbFactory.CreateParameter();
            paramId.ParameterName = "@ToolId";
            paramId.DbType = DbType.Int32;
            paramId.Value = toolid;
            cmd.Parameters.Add(paramId);

            executeNonQuery(cmd);
        }

        private void executeNonQuery(DbCommand cmd)
        {
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public void createTool(string description, DateTime buydate)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO tool VALUES (@Description, @Date, 1)";

            DbParameter paramDescription = dbFactory.CreateParameter();
            paramDescription.ParameterName = "@Description";
            paramDescription.DbType = DbType.String;
            paramDescription.Value = description;
            cmd.Parameters.Add(paramDescription);

            DbParameter paramDate = dbFactory.CreateParameter();
            paramDate.ParameterName = "@Date";
            paramDate.DbType = DbType.DateTime;
            paramDate.Value = buydate;
            cmd.Parameters.Add(paramDate);

            executeNonQuery(cmd);
        }

        public Tool getTool(int toolid)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tool WHERE id = @ToolId";

            DbParameter paramId = dbFactory.CreateParameter();
            paramId.ParameterName = "@ToolId";
            paramId.DbType = DbType.Int32;
            paramId.Value = toolid;
            cmd.Parameters.Add(paramId);

            Tool tool = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                int id = (int)reader["id"];
                string description = (string)reader["description"];
                DateTime buydate = (DateTime)reader["buydate"];
                Boolean in_use = (Boolean)reader["in_use"];

                tool = new Tool(id, description, buydate, in_use);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return tool;
        }

        public void UpdateTool(int toolid, string description, DateTime buydate, bool in_use)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE tool SET description = @Description, buydate = @BuyDate, in_use = @In_Use WHERE id = @ToolId";

            DbParameter paramId = dbFactory.CreateParameter();
            paramId.ParameterName = "@ToolId";
            paramId.DbType = DbType.Int32;
            paramId.Value = toolid;
            cmd.Parameters.Add(paramId);

            DbParameter paramDescription = dbFactory.CreateParameter();
            paramDescription.ParameterName = "@Description";
            paramDescription.DbType = DbType.String;
            paramDescription.Value = description;
            cmd.Parameters.Add(paramDescription);

            DbParameter paramBuyDate = dbFactory.CreateParameter();
            paramBuyDate.ParameterName = "@BuyDate";
            paramBuyDate.DbType = DbType.DateTime;
            paramBuyDate.Value = buydate;
            cmd.Parameters.Add(paramBuyDate);

            DbParameter paramInUse = dbFactory.CreateParameter();
            paramInUse.ParameterName = "@In_Use";
            paramInUse.DbType = DbType.Boolean;
            paramInUse.Value = in_use;
            cmd.Parameters.Add(paramInUse);

            executeNonQuery(cmd);
        }

        public void createEmployee(string name, string initials, string password, DateTime hireddate)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO employee VALUES (@Name, @Initials, @Password, @HireDate, 1)";

            DbParameter paramName = dbFactory.CreateParameter();
            paramName.ParameterName = "@Name";
            paramName.DbType = DbType.String;
            paramName.Value = name;
            cmd.Parameters.Add(paramName);

            DbParameter paramInitials = dbFactory.CreateParameter();
            paramInitials.ParameterName = "@Initials";
            paramInitials.DbType = DbType.String;
            paramInitials.Value = initials;
            cmd.Parameters.Add(paramInitials);

            DbParameter paramPassword = dbFactory.CreateParameter();
            paramPassword.ParameterName = "@Password";
            paramPassword.DbType = DbType.String;
            paramPassword.Value = password;
            cmd.Parameters.Add(paramPassword);

            DbParameter paramHireDate = dbFactory.CreateParameter();
            paramHireDate.ParameterName = "@HireDate";
            paramHireDate.DbType = DbType.DateTime;
            paramHireDate.Value = hireddate;
            cmd.Parameters.Add(paramHireDate);

            executeNonQuery(cmd);
        }

        public Employee getEmployee(int employeeid)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM employee WHERE id = @EmployeeId";

            DbParameter paramEmployeeId = dbFactory.CreateParameter();
            paramEmployeeId.ParameterName = "@EmployeeId";
            paramEmployeeId.DbType = DbType.Int32;
            paramEmployeeId.Value = employeeid;
            cmd.Parameters.Add(paramEmployeeId);

            Employee employee = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                int id = (int)reader["id"];
                string name = (string)reader["name"];
                string initials = (string)reader["initials"];
                string password = (string)reader["password"];
                DateTime hireDate = (DateTime)reader["hiredate"];
                Boolean is_active = (Boolean)reader["is_active"];

                employee = new Employee(id, name, initials, password, hireDate, is_active);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return employee;
        }

        public void UpdateEmployee(int employeeId, string name, string initials, string password, DateTime hireDate, bool is_active)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE employee SET name = @Name, initials = @Initials, password = @Password, hiredate = @HireDate, is_active = @Is_Active WHERE id = @EmployeeId";

            DbParameter paramEmployeeId = dbFactory.CreateParameter();
            paramEmployeeId.ParameterName = "@EmployeeId";
            paramEmployeeId.DbType = DbType.Int32;
            paramEmployeeId.Value = employeeId;
            cmd.Parameters.Add(paramEmployeeId);

            DbParameter paramName = dbFactory.CreateParameter();
            paramName.ParameterName = "@Name";
            paramName.DbType = DbType.String;
            paramName.Value = name;
            cmd.Parameters.Add(paramName);

            DbParameter paramInitials = dbFactory.CreateParameter();
            paramInitials.ParameterName = "@Initials";
            paramInitials.DbType = DbType.String;
            paramInitials.Value = initials;
            cmd.Parameters.Add(paramInitials);

            DbParameter paramPassword = dbFactory.CreateParameter();
            paramPassword.ParameterName = "@Password";
            paramPassword.DbType = DbType.String;
            paramPassword.Value = password;
            cmd.Parameters.Add(paramPassword);

            DbParameter paramHireDate = dbFactory.CreateParameter();
            paramHireDate.ParameterName = "@HireDate";
            paramHireDate.DbType = DbType.DateTime;
            paramHireDate.Value = hireDate;
            cmd.Parameters.Add(paramHireDate);

            DbParameter paramIsActive = dbFactory.CreateParameter();
            paramIsActive.ParameterName = "@Is_Active";
            paramIsActive.DbType = DbType.Boolean;
            paramIsActive.Value = is_active;
            cmd.Parameters.Add(paramIsActive);

            executeNonQuery(cmd);
        }

        public void CommentOnTool(int toolid, int employeeid, string comment)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO comment VALUES (GETDATE(), @Comment, @ToolId, @EmployeeId)";

            DbParameter paramToolId = dbFactory.CreateParameter();
            paramToolId.ParameterName = "@ToolId";
            paramToolId.DbType = DbType.Int32;
            paramToolId.Value = toolid;
            cmd.Parameters.Add(paramToolId);

            DbParameter paramEmployeeId = dbFactory.CreateParameter();
            paramEmployeeId.ParameterName = "@EmployeeId";
            paramEmployeeId.DbType = DbType.Int32;
            paramEmployeeId.Value = employeeid;
            cmd.Parameters.Add(paramEmployeeId);

            DbParameter paramComment = dbFactory.CreateParameter();
            paramComment.ParameterName = "@Comment";
            paramComment.DbType = DbType.String;
            paramComment.Value = comment;
            cmd.Parameters.Add(paramComment);

            executeNonQuery(cmd);
        }

        public void createReservation(DateTime dateStart, DateTime dateEnd, int toolId, int employeeId, int locationId)
        {
            if (DateTime.Now > dateStart) throw new Exception("Starttime is in the past");
            else if (dateStart > dateEnd) throw new Exception("Endtime is earlier than starttime");
            else if (dateStart == dateEnd) throw new Exception("Starttime and endtime is the same");
            else if (dateStart.DayOfWeek == DayOfWeek.Saturday || dateStart.DayOfWeek == DayOfWeek.Sunday) throw new Exception("Starttime is in a weekend");
            else if (dateEnd.DayOfWeek == DayOfWeek.Saturday || dateStart.DayOfWeek == DayOfWeek.Sunday) throw new Exception("Endtime is in a weekend");
            else if (timeIsReserved(toolId, dateStart, dateEnd)) throw new Exception("Time is already reserved");
            else
            {
                cmd = dbFactory.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO reservation VALUES (@dateStart, @dateEnd, ";

                DbParameter paramDateStart = dbFactory.CreateParameter();
                paramDateStart.ParameterName = "@dateStart";
                paramDateStart.DbType = DbType.DateTime;
                paramDateStart.Value = dateStart;
                cmd.Parameters.Add(paramDateStart);

                DbParameter paramDateEnd = dbFactory.CreateParameter();
                paramDateEnd.ParameterName = "@dateEnd";
                paramDateEnd.DbType = DbType.DateTime;
                paramDateEnd.Value = dateEnd;
                cmd.Parameters.Add(paramDateEnd);

                if (employeeId == -1 && locationId == -1)
                {
                    cmd.CommandText += "null, null)";
                }
                else
                {
                    cmd.CommandText += "@employeeId, @locationId)";

                    DbParameter paramEmployeeId = dbFactory.CreateParameter();
                    paramEmployeeId.ParameterName = "@employeeId";
                    paramEmployeeId.DbType = DbType.Int32;
                    paramEmployeeId.Value = employeeId;
                    cmd.Parameters.Add(paramEmployeeId);

                    DbParameter paramLocationId = dbFactory.CreateParameter();
                    paramLocationId.ParameterName = "@locationId";
                    paramLocationId.DbType = DbType.Int32;
                    paramLocationId.Value = locationId;
                    cmd.Parameters.Add(paramLocationId);

                }

                cmd.CommandText += " INSERT INTO reservationTool VALUES (@toolId, @@IDENTITY)";

                DbParameter paramToolId = dbFactory.CreateParameter();
                paramToolId.ParameterName = "@toolId";
                paramToolId.DbType = DbType.Int32;
                paramToolId.Value = toolId;
                cmd.Parameters.Add(paramToolId);

                executeNonQuery(cmd);
            }
        }

        public void createMultiToolReservation(DateTime dateStart, DateTime dateEnd, List<int> toolId, int employeeId, int locationId)
        {
            DbCommand cmm = getMultiReservationCMD(dateStart, dateEnd, toolId);
            cmm.Connection = conn;
            DbTransaction tran= null;

            try
            {
                conn.Open();
                //tran = conn.BeginTransaction(IsolationLevel.Serializable); - kommenter ud for at teste
                tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                cmm.Transaction = tran;

                Object o = cmm.ExecuteScalar();
                int conflictingReservations = (int)o;

                if (conflictingReservations > 0) throw new Exception("Reservation not possible. " + conflictingReservations + " conflicting reservations");
                else
                {
                    Console.WriteLine("Reservation possible. Any key to continue...");
                    Console.ReadKey();

                    cmm = getMultiToolReservationInsertCMD(dateStart, dateEnd, toolId, employeeId, locationId);
                    cmm.Connection = conn;
                    cmm.Transaction = tran;

                    DbCommand cdd = getMultiReservationCMD(dateStart, dateEnd, toolId);
                    cdd.Connection = conn;
                    cdd.Transaction = tran;

                    //Udkommenter næste linie for at prøve serializable
                    if ((int)cdd.ExecuteScalar() > 0) throw new Exception("Reservation no longer possible. Another user have reserved the time, while you where idling");

                    cmm.ExecuteNonQuery();

                    Console.WriteLine("Data inserted succesfully. Any key to commit...");
                    Console.ReadKey();

                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally 
            {
                conn.Close();
            }
        }

        private DbCommand getMultiReservationCMD(DateTime dateStart, DateTime dateEnd, List<int> toolId) 
        {
            DbCommand cmd = dbFactory.CreateCommand();
            cmd.CommandText = " SELECT COUNT(*) AS conflicting_reservations ";
            cmd.CommandText += "FROM reservation r JOIN reservationTool rt ON r.id = rt.reservationid ";
            cmd.CommandText += "WHERE ";

            for (int i = 0; i < toolId.Count; i++)
            {
                cmd.CommandText += "rt.toolid = @" + toolId[i] + " AND r.enddate > @dateStart AND r.startdate < @dateStart OR ";
                cmd.CommandText += "rt.toolid = @" + toolId[i] + " AND r.startdate > @dateStart AND r.startdate < @dateEnd OR ";

                if (i == toolId.Count - 1)
                    cmd.CommandText += "rt.toolid = @" + toolId[i] + " AND r.startdate = @dateStart AND r.enddate = @dateEnd ";
                else
                    cmd.CommandText += "rt.toolid = @" + toolId[i] + " AND r.startdate = @dateStart AND r.enddate = @dateEnd OR ";

                DbParameter paramToolId = dbFactory.CreateParameter();
                paramToolId.ParameterName = "@" + toolId[i];
                paramToolId.DbType = DbType.Int32;
                paramToolId.Value = toolId[i];
                cmd.Parameters.Add(paramToolId);
            }

            DbParameter paramDateStart = dbFactory.CreateParameter();
            paramDateStart.ParameterName = "@dateStart";
            paramDateStart.DbType = DbType.DateTime;
            paramDateStart.Value = dateStart;
            cmd.Parameters.Add(paramDateStart);

            DbParameter paramDateEnd = dbFactory.CreateParameter();
            paramDateEnd.ParameterName = "@dateEnd";
            paramDateEnd.DbType = DbType.DateTime;
            paramDateEnd.Value = dateEnd;
            cmd.Parameters.Add(paramDateEnd);

            //Prøv at udkommentere det her og læs
            //Console.WriteLine(cmd.CommandText);
            //Console.ReadLine();

            return cmd;
        }

        private DbCommand getMultiToolReservationInsertCMD(DateTime dateStart, DateTime dateEnd, List<int> toolId, int employeeId, int locationId)
        {
            DbCommand cmd = dbFactory.CreateCommand();
            cmd.CommandText = "INSERT INTO reservation VALUES (@dateStart, @dateEnd, ";

            DbParameter paramDateStart = dbFactory.CreateParameter();
            paramDateStart.ParameterName = "@dateStart";
            paramDateStart.DbType = DbType.DateTime;
            paramDateStart.Value = dateStart;
            cmd.Parameters.Add(paramDateStart);

            DbParameter paramDateEnd = dbFactory.CreateParameter();
            paramDateEnd.ParameterName = "@dateEnd";
            paramDateEnd.DbType = DbType.DateTime;
            paramDateEnd.Value = dateEnd;
            cmd.Parameters.Add(paramDateEnd);

            if (employeeId == -1 && locationId == -1)
            {
                cmd.CommandText += "null, null) ";
            }
            else
            {
                cmd.CommandText += "@employeeId, @locationId) ";

                DbParameter paramEmployeeId = dbFactory.CreateParameter();
                paramEmployeeId.ParameterName = "@employeeId";
                paramEmployeeId.DbType = DbType.Int32;
                paramEmployeeId.Value = employeeId;
                cmd.Parameters.Add(paramEmployeeId);

                DbParameter paramLocationId = dbFactory.CreateParameter();
                paramLocationId.ParameterName = "@locationId";
                paramLocationId.DbType = DbType.Int32;
                paramLocationId.Value = locationId;
                cmd.Parameters.Add(paramLocationId);

            }

            cmd.CommandText += "DECLARE @resId int = @@IDENTITY ";

            foreach (int i in toolId)
            {
                cmd.CommandText += "INSERT INTO reservationTool VALUES (@" + i + ", @resId) ";

                DbParameter paramToolId = dbFactory.CreateParameter();
                paramToolId.ParameterName = "@" + i;
                paramToolId.DbType = DbType.Int32;
                paramToolId.Value = i;
                cmd.Parameters.Add(paramToolId);
            }

            return cmd;
        }

        private bool timeIsReserved(int toolId, DateTime dateStart, DateTime dateEnd)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = " SELECT COUNT(*) AS conflicting_reservations ";
            cmd.CommandText += "FROM reservation r JOIN reservationTool rt ON r.id = rt.reservationid ";
            cmd.CommandText += "WHERE rt.toolid = @toolId AND r.enddate > @dateStart AND r.startdate < @dateStart OR ";
            cmd.CommandText += "      rt.toolid = @toolId AND r.startdate > @dateStart AND r.startdate < @dateEnd OR ";
            cmd.CommandText += "      rt.toolid = @toolId AND r.startdate = @dateStart AND r.enddate = @dateEnd";

            DbParameter paramToolId = dbFactory.CreateParameter();
            paramToolId.ParameterName = "@toolId";
            paramToolId.DbType = DbType.Int32;
            paramToolId.Value = toolId;
            cmd.Parameters.Add(paramToolId);

            DbParameter paramDateStart = dbFactory.CreateParameter();
            paramDateStart.ParameterName = "@dateStart";
            paramDateStart.DbType = DbType.DateTime;
            paramDateStart.Value = dateStart;
            cmd.Parameters.Add(paramDateStart);

            DbParameter paramDateEnd = dbFactory.CreateParameter();
            paramDateEnd.ParameterName = "@dateEnd";
            paramDateEnd.DbType = DbType.DateTime;
            paramDateEnd.Value = dateEnd;
            cmd.Parameters.Add(paramDateEnd);

            bool result = true;

            try
            {
                conn.Open();
                Object o = cmd.ExecuteScalar();
                int conflictingReservations = (int)o;
                if (conflictingReservations == 0) result = false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public string getLoginName(int employeeId, string password)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = " SELECT name ";
            cmd.CommandText += "FROM employee ";
            cmd.CommandText += "WHERE id = @EmployeeId AND password = @Password AND is_active = 1";

            DbParameter paramEmployeeId = dbFactory.CreateParameter();
            paramEmployeeId.ParameterName = "@EmployeeId";
            paramEmployeeId.DbType = DbType.Int32;
            paramEmployeeId.Value = employeeId;
            cmd.Parameters.Add(paramEmployeeId);

            DbParameter paramPassword = dbFactory.CreateParameter();
            paramPassword.ParameterName = "@Password";
            paramPassword.DbType = DbType.String;
            paramPassword.Value = password;
            cmd.Parameters.Add(paramPassword);

            string result = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    result = (string)reader["name"];
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return result;
        }

        public DataTable getToolsLocationATM()
        {
            string cmd = " SELECT * FROM getToolsCurrentLocation()";

            return getDataTable(cmd);
        }

        public string getLocation(int locationId)
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = " SELECT location FROM location WHERE id = @LocationId";

            DbParameter paramLocation = dbFactory.CreateParameter();
            paramLocation.ParameterName = "@LocationId";
            paramLocation.DbType = DbType.Int32;
            paramLocation.Value = locationId;
            cmd.Parameters.Add(paramLocation);

            string result = "";

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                result = (string)reader["location"];

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return result;
        }

        public List<string> getLocationsAsStrings()
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT 'ID ' + CONVERT(VARCHAR, id) + ': ' + location AS loc FROM location WHERE is_active = 1";

            List<string> result = new List<string>();

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add((string)reader["loc"]);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return result;
        }

        public List<string> getEmployeesAsStrings()
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT 'ID ' + CONVERT(VARCHAR, id) + ': ' + name AS emp FROM employee WHERE is_active = 1";

            List<string> result = new List<string>();

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add((string)reader["emp"]);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return result;
        }


        public List<string> getToolsAsStrings()
        {
            cmd = dbFactory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT 'ID ' + CONVERT(VARCHAR, id) + ': ' + description AS tool FROM tool WHERE in_use = 1";

            List<string> result = new List<string>();

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add((string)reader["tool"]);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            return result;
        }
    }

    public class Tool
    {
        public int id { get; private set; }
        public string description { get; set; }
        public DateTime buyDate { get; set; }
        public bool in_use { get; set; }

        public Tool(int id, string description, DateTime buyDate, bool in_use)
        {
            this.id = id;
            this.description = description;
            this.buyDate = buyDate;
            this.in_use = in_use;
        }

    }

    public class Employee
    {
        public int id { get; private set; }
        public string name { get; set; }
        public string initials { get; set; }
        public string password { get; set; }
        public DateTime hireDate { get; set; }
        public bool is_active { get; set; }

        public Employee(int id, string name, string initials, string password, DateTime hireDate, bool is_active)
        {
            this.id = id;
            this.name = name;
            this.initials = initials;
            this.password = password;
            this.hireDate = hireDate;
            this.is_active = is_active;
        }

    }
}
