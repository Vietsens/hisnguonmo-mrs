using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 
using MRS.MANAGER.Config; 
using Inventec.Common.Logging; 
using Inventec.Common.Repository; 

namespace MRS.Processor.Mrs00490
{
    
    public class Mrs00490RDO: V_HIS_SERE_SERV
    {

        public Mrs00490RDO(V_HIS_SERE_SERV ss, V_HIS_PATIENT_TYPE_ALTER pt,HIS_PATIENT patient)
        {
            PropertyInfo[] p = Properties.Get<V_HIS_SERE_SERV>(); 
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(ss)); 
            }
            ProcessHeinCard(pt); 
            SetExtensionField(this, patient); 
        }

        private void SetExtensionField(Mrs00490RDO mrs00490RDO, HIS_PATIENT patient)
        {
            this.MEDI_EXPEND_AMOUNT = this.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC ? this.VIR_TOTAL_PRICE_NO_EXPEND ?? 0 : 0;
            this.MATE_EXPEND_AMOUNT = this.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT ? this.VIR_TOTAL_PRICE_NO_EXPEND ?? 0 : 0;
            this.DOB_YEAR_STR = patient.DOB.ToString().Substring(0, 4);
            this.PATIENT_CODE = patient.PATIENT_CODE;
            this.VIR_PATIENT_NAME = patient.VIR_PATIENT_NAME;
            this.GENDER_NAME = (HisGenderCFG.HisGender.FirstOrDefault(o=>o.ID==patient.GENDER_ID)??new HIS_GENDER()).GENDER_NAME; 
        }

        
        private void ProcessHeinCard(V_HIS_PATIENT_TYPE_ALTER data)
        {
            try
            {
                if (data != null && data.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    if (data.HEIN_CARD_NUMBER != null)
                    {
                        this.MEDI_ORG_NAME = data.HEIN_MEDI_ORG_NAME; 
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string MEDI_ORG_NAME { get; set; }
        public Decimal MEDI_EXPEND_AMOUNT { get;  set;  }
        public Decimal MATE_EXPEND_AMOUNT { get;  set;  }
        public string DOB_YEAR_STR { get;  set;  }
        public Mrs00490RDO() { }
    }
}
