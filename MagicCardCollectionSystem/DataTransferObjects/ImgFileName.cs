using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ImgFileName // file name for card image
    {
        public string ImgFileNameID { get; set; }
        public int CardID { get; set; }
        public override string ToString()
        {
            return ImgFileNameID;
        }
    }
}
