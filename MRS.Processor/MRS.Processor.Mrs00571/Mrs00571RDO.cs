using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00571
{
    public class Mrs00571RDO
    {
        public long? CREATE_TIME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_ETHNIC_NAME { get; set; }
        public string TDL_PATIENT_CAREER_NAME { get; set; }
        public long? DEATH_TIME { get; set; }
        public string DEATH_PLACE { get; set; }
        public long? DEATH_CAUSE_ID { get; set; }
        public string DEATH_CAUSE_NAME { get; set; }
        public string DEATH_CERT_ISSUER_USERNAME { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        
    }
}
