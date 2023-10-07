using MRS.Processor.Mrs00578;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00578
{
    public class Mrs00578RDO
    {
        public long? TRANSACTION_TIME { get; set; }
        public string TRANSACTION_TIME_STR { get; set;}
        public string TREATMENT_CODE { get; set;}
        public string PATIENT_NAME { get; set;}
        public long? DOB { get; set;}	
        public string DOB_STR { get; set;}	
        public string HEIN_CARD_NUMBER { get; set;}
        public string SERVICE_NAME { get; set;}	
        public string PTTT_GROUP_CODE { get; set;}	
        public decimal? PACKAGE_PRICE { get; set;}
        public decimal? TOTAL_PRICE_IN_FEE { get; set; }
        public decimal? TOTAL_PRICE_FEE { get; set; }

        
    }
}
