﻿using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00665
{
    public class Mrs00665Filter
    {
        public long TIME_FROM { get;  set;  }              
        public long TIME_TO { get;  set;  }

        public long? MEDI_STOCK_ID { get; set; }  
        

        public Mrs00665Filter() { }
    }
}