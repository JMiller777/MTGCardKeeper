using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class Card
    {
        /*
        [CardID] 				[int] IDENTITY (100000,1) 	NOT NULL,
	    [Name]					[nvarchar](150)				NOT NULL,
	    [ColorID]				[nvarchar](100)				NOT	NULL,
	    [TypeID]				[nvarchar](100)				NOT NULL,
	    [EditionID]				[nvarchar](100)				NOT NULL,
	    [RarityID]				[nvarchar](100)				NOT NULL,
	    [IsFoil]				[bit]						NOT NULL DEFAULT 0,
        [Active]				[bit]						NOT NULL DEFAULT 1,
	    [CardText]				[nvarchar](1000)			NOT NULL,
	    [ImgFileName]			[nvarchar](50)				NULL
        */
        public int CardID { get; set; }
        public string Name { get; set; }
        public string ColorID { get; set; }
        public string TypeID { get; set; }
        public string EditionID { get; set; }
        public string RarityID { get; set; }
        public bool IsFoil { get; set; }
        public bool Active { get; set; }
        public string CardText { get; set; }
        public string ImgFileName { get; set; }
    }
}
