using MOS.MANAGER.HisTreatment;
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00103
{
    class Mrs00103RDO : V_HIS_TREATMENT_FEE
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
        public decimal TOTAL_BILL_DEPOSIT_PRICE { get;  set;  }
        public decimal TOTAL_REPAY_PRICE { get;  set;  }
        public decimal? TOTAL_RESIDUAL_PRICE { get;  set;  }
        public decimal? TOTAL_OWE_PRICE { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }

        public Mrs00103RDO() { }

        internal void ProcessHisTreatmentFee(V_HIS_TREATMENT_FEE treatmentFee)
        {
            try
            {
                if (treatmentFee != null)
                {
                    System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TREATMENT_FEE>(); 
                    foreach (var item in pi)
                    {
                        item.SetValue(this, (item.GetValue(treatmentFee))); 
                    }
                    ProcessSetExtendField(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessSetExtendField()
        {
            try
            {
                //TOTAL_PRICE = TOTAL_PRICE ?? 0; 
                //TOTAL_HEIN_PRICE = TOTAL_HEIN_PRICE ?? 0; 
                //TOTAL_PATIENT_PRICE = TOTAL_PATIENT_PRICE ?? 0; 
                TOTAL_BILL_DEPOSIT_PRICE = (TOTAL_BILL_AMOUNT ?? 0) + (TOTAL_DEPOSIT_AMOUNT ?? 0) - (TOTAL_BILL_TRANSFER_AMOUNT ?? 0); 
                TOTAL_REPAY_PRICE = TOTAL_REPAY_AMOUNT ?? 0; 
                decimal totalDepositPlus = (TOTAL_PATIENT_PRICE ?? 0) + TOTAL_REPAY_PRICE - TOTAL_BILL_DEPOSIT_PRICE; 
                if (totalDepositPlus > 0)
                {
                    TOTAL_OWE_PRICE = totalDepositPlus; 
                }
                else
                {
                    TOTAL_RESIDUAL_PRICE = -(totalDepositPlus); 
                }
                SetYearAndAgeDob(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetYearAndAgeDob()
        {
            try
            {
                if (TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    MALE_YEAR = TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                    MALE_AGE = (DateTime.Now.Year - (int.Parse(MALE_YEAR))); 
                }
                else if (TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    FEMALE_YEAR = TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                    FEMALE_AGE = (DateTime.Now.Year - (int.Parse(FEMALE_YEAR))); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
