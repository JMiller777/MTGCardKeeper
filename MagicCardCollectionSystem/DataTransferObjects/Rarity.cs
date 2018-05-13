using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class Rarity // Card rarity
    {
        public string RarityID { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return RarityID;
        }
    }
}
