using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00024
{
    class Mrs00024RDO : V_HIS_SERE_SERV
    {
        public long PATIENT_ID { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }

        
        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string CONSULTANT_LOGINNAME { get; set; }
        public string CONSULTANT_USERNAME { get; set; }
        public string INTRUCTION_TIME_STR { get; set; }
        public decimal TOTAL_PRICE { get; set; }


        public string REQUEST_LOGINNAME { get; set; }

        public string REQUEST_USERNAME { get; set; }
    }
}
