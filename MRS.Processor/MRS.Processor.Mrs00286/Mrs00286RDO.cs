using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00286
{
    public class Mrs00286RDO
    {
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DOB { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_TEXT { get; set; }
        public string REQ_USERNAME { get; set; }
        public string INSTRUCTION_TIME { get; set; }
        public string IN_CODE { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string BARCODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }

    }

}
