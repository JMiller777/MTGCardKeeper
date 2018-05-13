using DataTransferObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        private IUserAccessor _userAccessor;

        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }

        public User AuthenticateUser(string username, string password)
        {
            // hash the password first, store it as passwordHash
            User user = null; // user token to build
            var passwordHash = HashSha256(password);

            try
            {
                // need to pass username and passwordHash to access method
                // 1 means the user is validated. Nothing else is valid.
                var validationResult = _userAccessor.VerifyUserNameAndPassword(username, passwordHash);

                if (validationResult == 1) // user is validated
                {
                    // Need employee object and roles for user object

                    // So, Get employee
                    var collector = _userAccessor.RetrieveCollectorByUsername(username);

                    // Get Employee's roles
                    var roles = _userAccessor.RetrieveRolesByCollectorID(collector.CollectorID);

                    // prevent the user from using the app without 
                    // changing password first
                    bool passwordMustBeChanged = false;

                    // set user as "newuser" if it's their first time using the app
                    if (password == "newuser")
                    {
                        passwordMustBeChanged = true;
                        roles.Clear(); // clear user's roles and lock em out
                        roles.Add(new Role() { RoleID = "New User" });
                    }

                    user = new User(collector, roles, passwordMustBeChanged);
                }
                else // not validated
                {
                    throw new ApplicationException("Login Failed. Bad username (email address) or password.");
                }
            }
            catch (ApplicationException) // Re-Throw the application exception
            {
                throw;
            }
            catch (Exception ex) // Wrap and throw other types of exceptions
            {
                throw new ApplicationException("There was a problem with the server", ex);
            }
            return user;
        }

        // apply a SHA256 hash algorithm to a password
        // to store or compare with the user's 
        // passwordHash in the database
        private string HashSha256(string source)
        {
            var result = "";

            // create a byte array
            byte[] data;

            // create a .Net Hash provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            // build the result string
            var s = new StringBuilder();

            // loop through byte array creating
            // letters to add to SB
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            // get a string from the StringBuilder
            result = s.ToString();
            return result;
        }

        public User UpdatePassword(User user, string oldPassword, string newPassword)
        {
            User newUser = null;
            int rowsAffected = 0;

            string oldPasswordHash = HashSha256(oldPassword);
            string newPasswordHash = HashSha256(newPassword);

            // attempt to invoke the access method
            try
            {
                rowsAffected = _userAccessor.UpdatePasswordHash(user.Collector.CollectorID,
                    oldPasswordHash, newPasswordHash);

                if (rowsAffected == 1)
                {

                    if (user.Roles[0].RoleID == "New User")
                    {
                        // get the roles and create a new user
                        var roles = _userAccessor.RetrieveRolesByCollectorID(user.Collector.CollectorID);
                        newUser = new User(user.Collector, roles); // defaults to false
                    }
                    else
                    {
                        newUser = user;
                    }
                }
                else
                {
                    throw new ApplicationException("Update returned 0 rows affected.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Password change failed.", ex);
            }

            return newUser;
        }
    }
}
