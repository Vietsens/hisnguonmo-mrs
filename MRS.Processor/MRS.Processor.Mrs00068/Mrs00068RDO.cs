using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00068
{
    class Mrs00068RDO
    {
        public string PATIENT_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DOB_YEAR { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_NOTBHYT { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public string OPEN_TIME_SEPARATE_STR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string DATE_IN_STR { get;  set;  }

        public decimal TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_HEIN_PRICE { get;  set;  }
        public decimal TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_BILL_DEPOSIT_PRICE { get;  set;  }
        public decimal TOTAL_REPAY_PRICE { get;  set;  }
        public decimal TOTAL_EXCESS_PRICE { get;  set;  }
        public decimal TOTAL_NOTENOUGH_PRICE { get;  set;  }
        public decimal? TOTAL_RESIDUAL_PRICE { get;  set;  }
        public decimal? TOTAL_OWE_PRICE { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }

        public Mrs00068RDO() { }

        public Mrs00068RDO(V_HIS_TREATMENT_FEE treatmentFee, ref CommonParam paramGet, string isBHYT, Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran)
        {
            try
            {
                TREATMENT_CODE = treatmentFee.TREATMENT_CODE; 
                PATIENT_CODE = treatmentFee.TDL_PATIENT_CODE; 
                VIR_PATIENT_NAME = treatmentFee.TDL_PATIENT_NAME; 
                TOTAL_PRICE = treatmentFee.TOTAL_PRICE ?? 0; 
                TOTAL_HEIN_PRICE = treatmentFee.TOTAL_HEIN_PRICE ?? 0; 
                TOTAL_PATIENT_PRICE = treatmentFee.TOTAL_PATIENT_PRICE ?? 0; 
                TOTAL_BILL_DEPOSIT_PRICE = (treatmentFee.TOTAL_BILL_AMOUNT ?? 0) + (treatmentFee.TOTAL_DEPOSIT_AMOUNT ?? 0) - (treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT ?? 0); 
                TOTAL_REPAY_PRICE = treatmentFee.TOTAL_REPAY_AMOUNT ?? 0; 
                decimal totalDepositPlus = TOTAL_PATIENT_PRICE + TOTAL_REPAY_PRICE - TOTAL_BILL_DEPOSIT_PRICE; 
                if (totalDepositPlus > 0)
                {
                    TOTAL_OWE_PRICE = totalDepositPlus; 
                }
                else
                {
                    TOTAL_RESIDUAL_PRICE = -(totalDepositPlus); 
                }

                IS_BHYT = isBHYT; 
                if (String.IsNullOrEmpty(isBHYT))
                {
                    IS_NOTBHYT = "X"; 
                }

                SetYearAndAgeDob(treatmentFee); 
                SetDepartmentTranTreatment(treatmentFee, paramGet, dicDepartmentTran); 
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

        private void SetDepartmentTranTreatment(V_HIS_TREATMENT_FEE treatmentFee, CommonParam paramGet, Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran)
        {
            try
            {
                //lay chuyen khoa cua hsdt do
                var hisDepartmentTran = dicDepartmentTran.ContainsKey(treatmentFee.ID) ? dicDepartmentTran[treatmentFee.ID] : new List<V_HIS_DEPARTMENT_TRAN>(); 
                //sap xep giam dan theo thoi gian
                hisDepartmentTran = hisDepartmentTran.OrderByDescending(o => o.DEPARTMENT_IN_TIME).ToList(); 
                //khoa dieu tri = khoa cuoi cung
                if (hisDepartmentTran != null && hisDepartmentTran.Count > 0)
                {
                    DEPARTMENT_NAME = hisDepartmentTran.FirstOrDefault(o => o.DEPARTMENT_IN_TIME != null).DEPARTMENT_NAME; 
                    DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(hisDepartmentTran.Last().DEPARTMENT_IN_TIME ?? 0); 
                }
                ////lay chuyen khoa cua hsdt do
                //HisDepartmentTranViewFilterQuery depaFilter = new HisDepartmentTranViewFilterQuery(); 
                //depaFilter.TREATMENT_ID = treatmentFee.ID; 
                //var hisDepartmentTran = new His.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(depaFilter); 

                //if (hisDepartmentTran != null && hisDepartmentTran.Count > 0)
                //{
                //    //sap xep giam dan theo thoi gian
                //    hisDepartmentTran = hisDepartmentTran.OrderByDescending(o => o.LOG_TIME).ToList(); 
                //    //neu ck dau tien la vao khoa
                //    if (hisDepartmentTran.First().IN_OUT == IMSys.DbConfig.HIS_RS.HIS_DEPARTMENT_TRAN.IN_OUT__IN)
                //    {
                //        DEPARTMENT_NAME = hisDepartmentTran.First().DEPARTMENT_NAME; //khoa dieu tri = vao khoa cuoi cung
                //    }
                //    else
                //    {
                //        if (hisDepartmentTran.First().NEXT_DEPARTMENT_ID > 0)
                //        {
                //            DEPARTMENT_NAME = "Chờ tiếp nhận: " + hisDepartmentTran.First().NEXT_DEPARTMENT_NAME; 
                //        }
                //        else
                //        {
                //            DEPARTMENT_NAME = hisDepartmentTran.First().DEPARTMENT_NAME; 
                //        }

                //    }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
