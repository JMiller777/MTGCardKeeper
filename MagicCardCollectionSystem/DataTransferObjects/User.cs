using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class User
    {
        public Collector Collector { get; private set; }
        public List<Role> Roles { get; private set; }

        public bool PasswordMustBeChanged { get; private set; }
        public User(Collector collector, List<Role> roles, bool passwordMustBeChanged = false) // default value false
        {
            this.Collector = collector;
            this.Roles = roles;
            this.PasswordMustBeChanged = passwordMustBeChanged;
        }
    }
}
