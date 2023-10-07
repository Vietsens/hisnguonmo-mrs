using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00074
{
    class Mrs00074RDO : V_HIS_TREATMENT_FEE
    {
        public string DOB_YEAR { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_NOTBHYT { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public string OPEN_TIME_SEPARATE_STR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string DATE_IN_STR { get;  set;  }
        public string DATE_OUT_STR { get;  set;  }

        //public decimal TOTAL_PRICE { get;  set;  }
        //public decimal TOTAL_HEIN_PRICE { get;  set;  }
        //public decimal TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_BILL_DEPOSIT_PRICE { get; set; }
        public decimal TOTAL_REPAY_PRICE { get; set; }
        public decimal? TOTAL_RESIDUAL_PRICE { get;  set;  }
        public decimal? TOTAL_OWE_PRICE { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }

        public Mrs00074RDO() { }

        public Mrs00074RDO(V_HIS_TREATMENT_FEE treatment)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TREATMENT_FEE>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(treatment))); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        internal void ProcessTotalPrice(V_HIS_TREATMENT_FEE treatmentFee, ref CommonParam paramGet)
        {
            try
            {
                if (treatmentFee != null)
                {
                    TOTAL_PRICE = treatmentFee.TOTAL_PRICE ?? 0; 
                    TOTAL_HEIN_PRICE = treatmentFee.TOTAL_HEIN_PRICE ?? 0; 
                    TOTAL_PATIENT_PRICE = treatmentFee.TOTAL_PATIENT_PRICE ?? 0; 
                    TOTAL_BILL_DEPOSIT_PRICE = (treatmentFee.TOTAL_BILL_AMOUNT ?? 0) + (treatmentFee.TOTAL_DEPOSIT_AMOUNT ?? 0) - (treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT ?? 0); 
                    TOTAL_REPAY_PRICE = treatmentFee.TOTAL_REPAY_AMOUNT ?? 0; 
                    decimal totalDepositPlus = (TOTAL_PATIENT_PRICE??0) + TOTAL_REPAY_PRICE - TOTAL_BILL_DEPOSIT_PRICE; 
                    if (totalDepositPlus > 0)
                    {
                        TOTAL_OWE_PRICE = totalDepositPlus; 
                    }
                    else
                    {
                        TOTAL_RESIDUAL_PRICE = -(totalDepositPlus); 
                    }
                    IsBhyt(treatmentFee, paramGet); 
                    SetYearAndAgeDob(treatmentFee);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void IsBhyt(V_HIS_TREATMENT_FEE treatmentFee, CommonParam paramGet)
        {
            try
            {
                if (treatmentFee.TDL_PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    IS_BHYT = "X";
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==treatmentFee.TDL_PATIENT_TYPE_ID);
                    if(patientType != null)
                    {
                        PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                        PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetYearAndAgeDob(V_HIS_TREATMENT_FEE treatmentFee)
        {
            try
            {
                if (treatmentFee.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    MALE_YEAR = treatmentFee.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                    MALE_AGE = (DateTime.Now.Year - (int.Parse(MALE_YEAR))); 
                }
                else if (treatmentFee.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    FEMALE_YEAR = treatmentFee.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                    FEMALE_AGE = (DateTime.Now.Year - (int.Parse(FEMALE_YEAR))); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public string PATIENT_TYPE_NAME { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }

        public string MAIN_SURG_USERNAME { get; set; }

        public string EINVOICE_NUM_ORDER { get; set; }
    }
    public class MAIN_SURG
    {
        public long TREATMENT_ID { get; set; }
        public string USERNAME { get; set; }
    }
    public class EINVOICE_INF
    {
        public long TREATMENT_ID { get; set; }
        public string EINVOICE_NUM_ORDER { get; set; }
    }
}
