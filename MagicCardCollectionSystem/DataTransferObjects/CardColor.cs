using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CardColor
    {
        public string ColorID { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return ColorID;
        }

    }
}
