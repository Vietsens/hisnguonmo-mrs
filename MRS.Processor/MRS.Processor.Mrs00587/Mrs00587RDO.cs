using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00587
{
    class Mrs00587RDO
    {
       public string TREATMENT_CODE { get; set;}
        public string PATIENT_NAME { get; set;}
        public long DOB { get; set;}	
        public string GENDER_NAME { get; set;}	
        public string ADDRESS { get; set;}	
        public string HEIN_CARD_NUMBER { get; set;}	
        public long FEE_LOCK_TIME { get; set;}
        public string EXECUTE_USERNAME { get; set;}
        public decimal DIFF_TOTAL_PRICE { get; set;}
    }
}
