using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public int VerifyUserNameAndPassword(string username, string passwordHash)
        {
            var result = 0; // to return number of rows found (One, hopefully)

            // Connection
            var conn = DBConnection.GetDBConnection();

            // Command text
            var cmdText = @"sp_authenticate_user";

            // create the command
            var cmd = new SqlCommand(cmdText, conn);

            // for stored procedure, we need to set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters for Stored Procedure
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // Set parameter values
            cmd.Parameters["@Email"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // Try-Catch to execute the command
            try
            {
                // open connection
                conn.Open();

                // Execute the command
                result = (int)cmd.ExecuteScalar(); // returns object, cast to int to return.
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // close the connection
                conn.Close();
            }
            return result;
        }
        public Collector RetrieveCollectorByUsername(string username)
        {
            Collector collector = null;

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_retrieve_collector_by_email";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // Command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // parameter value
            cmd.Parameters["@Email"].Value = username;

            // try-catch for command execution
            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // Process the results (only one result, so no while loop here)
                if (reader.HasRows) // HasRows is boolean. set to true if reader has rows...
                {
                    reader.Read(); // read the next line in the result

                    // create a new user object
                    collector = new Collector()
                    {
                        CollectorID = reader.GetString(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        Email = reader.GetString(4),
                        Active = reader.GetBoolean(5)
                    };
                    if (collector.Active != true)
                    {
                        throw new ApplicationException("Not an active collector.");
                    }
                }
                else
                {
                    throw new ApplicationException("Collector record not found.");
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return collector;
        }

        public List<Role> RetrieveRolesByCollectorID(string collectorID)
        {
            List<Role> roles = new List<Role>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // cmdText
            var cmdText = @"sp_retrieve_collector_roles";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@CollectorID", SqlDbType.NVarChar, 20);

            // parameter values
            cmd.Parameters["@CollectorID"].Value = collectorID;

            // attempt connection
            try
            {
                // open the connection 
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // check for results 
                if (reader.HasRows)
                {
                    // multiple rows means it's time for a while-loop
                    while (reader.Read())
                    {
                        // create a role object
                        var role = new Role()
                        {
                            RoleID = reader.GetString(0)
                        };
                        // add role to the list
                        roles.Add(role);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return roles;
        }

        public int UpdatePasswordHash(string collectorID,
            string oldPasswordHash, string newPasswordHash)
        {
            int result = 0;

            // connection 
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_update_passwordHash";

            // command 
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@CollectorID", SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            // Parameter Values
            cmd.Parameters["@CollectorID"].Value = collectorID;
            cmd.Parameters["@oldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@newPasswordHash"].Value = newPasswordHash;

            // Try Catch
            try
            {
                // open the connection 
                conn.Open();

                // execute the command
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    throw new ApplicationException("Password update failed.");
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
