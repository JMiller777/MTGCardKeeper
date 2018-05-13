using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;

namespace LogicLayer
{
    public interface IUserManager
    {
        User AuthenticateUser(string username, string password);
        User UpdatePassword(User user, string oldPassword, string newPassword);
    }
}