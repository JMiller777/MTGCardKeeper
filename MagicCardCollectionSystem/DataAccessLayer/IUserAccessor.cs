using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserAccessor
    {
        int VerifyUserNameAndPassword(string username, string passwordHash);
        Collector RetrieveCollectorByUsername(string username);
        List<Role> RetrieveRolesByCollectorID(string collectorID);
        int UpdatePasswordHash(string collectorID,
            string oldPasswordHash, string newPasswordHash);
    }
}