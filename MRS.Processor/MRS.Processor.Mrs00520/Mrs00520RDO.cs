using MRS.Processor.Mrs00520;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MRS.MANAGER.Config;
using MOS.EFMODEL.DataModels;

namespace MRS.Proccessor.Mrs00520
{
    public class Mrs00520RDO:HIS_TREATMENT
    {
        public string OUT_TIME_STR { get; set; }
        public string FEE_LOCK_TIME_STR { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string DOB_STR { get; set; }
        public string GENDER_NAME { get; set; }	
        public string ADDRESS { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_LOCK_TIME { get; set; }	
        public string HEIN_UNLOCK_TIME { get; set; }
        public string HEIN_RELOCK_TIME { get; set; }
        public string HEIN_LOCK_LOGINNAME { get; set; }
        public string HEIN_UNLOCK_LOGINNAME { get; set; }
        public string HEIN_RELOCK_LOGINNAME { get; set; }


        public Mrs00520RDO(HIS_TREATMENT_LOGGING r, List<HIS_TREATMENT> listHisTreatment, List<HIS_TREATMENT_LOGGING> listHisTreatmentLoggingHeinLock)
        {
            PropertyInfo[] p = typeof(HIS_TREATMENT).GetProperties();
            var treatment = listHisTreatment.FirstOrDefault(o => o.ID == r.TREATMENT_ID) ?? new HIS_TREATMENT();
            var treatmentLoggingHeinLock = listHisTreatmentLoggingHeinLock.FirstOrDefault(o => o.TREATMENT_ID == r.TREATMENT_ID && o.CREATE_TIME < r.CREATE_TIME) ?? new HIS_TREATMENT_LOGGING();
            var treatmentLoggingHeinReLock = listHisTreatmentLoggingHeinLock.LastOrDefault(o => o.TREATMENT_ID == r.TREATMENT_ID && o.CREATE_TIME > r.CREATE_TIME) ?? new HIS_TREATMENT_LOGGING();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(treatment));
            }
            this.HEIN_LOCK_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatmentLoggingHeinLock.CREATE_TIME ?? 0);
            this.HEIN_LOCK_LOGINNAME = treatmentLoggingHeinLock.LOGINNAME;
            this.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0);
            this.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.FEE_LOCK_TIME ?? 0);
            this.HEIN_UNLOCK_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(r.CREATE_TIME ?? 0);
            this.HEIN_UNLOCK_LOGINNAME = r.LOGINNAME;
            this.HEIN_RELOCK_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatmentLoggingHeinReLock.CREATE_TIME ?? 0);
            this.HEIN_RELOCK_LOGINNAME = treatmentLoggingHeinReLock.LOGINNAME;
            this.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
            this.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
            this.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
            this.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
            this.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
            this.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
        }

        public Mrs00520RDO()
        {
            // TODO: Complete member initialization
        }
        
    }
}
