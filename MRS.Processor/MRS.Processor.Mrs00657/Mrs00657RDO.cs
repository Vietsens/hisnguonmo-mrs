using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00657
{
    class Mrs00657RDO : MOS.EFMODEL.DataModels.HIS_SERE_SERV
    {
        public long TYPE_ID { get; set; }
        public long ROW_POS { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public string TDL_SERVICE_TYPE_NAME { get; set; }

        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }
        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string TDL_EXECUTE_ROOM_NAME { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }

        public decimal TOTAL_PARTIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_FEE_PRICE { get; set; }
        public decimal TOTAL_SERVICE_PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
    }
}
