using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class Edition
    {
        public string EditionID { get; set; }
        public override string ToString()
        {
            return EditionID;
        }
    }
}
