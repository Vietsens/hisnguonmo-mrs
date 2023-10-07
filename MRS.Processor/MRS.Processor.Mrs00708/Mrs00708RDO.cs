using MOS.MANAGER.HisService;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using Inventec.Common.Logging; 
using MOS.Filter; 
using MOS.MANAGER.HisIcd; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServ; 

namespace MRS.Processor.Mrs00708
{
    public class Mrs00708RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string JSON_FORM_ID { get; set; }
        public long FORM_ID { get; set; }
        public long FORM_CREATE_TIME { get; set; }
        public string FORM_CREATOR { get; set; }
        public string FORM_CREATE_USERNAME { get; set; }
        public Dictionary<string,string> DIC_FORM_DATA { get; set; }

    }

    public class FormData
    {
        public long FORM_ID { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }
    }

    public class Form
    {
        public long ID { get; set; }
        public long CREATE_TIME { get; set; }
        public string CREATOR { get; set; }
    }
}
