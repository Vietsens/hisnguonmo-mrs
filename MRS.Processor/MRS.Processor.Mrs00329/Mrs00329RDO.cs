using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00329
{
    public class Mrs00329RDO : V_HIS_PATIENT_TYPE_ALTER
    {
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long TOTAL_DATE { get; set; }

        public string ICD_CODE_MAIN { get; set; }

        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string FEE_LOCK_USERNAME { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public string VIR_PATIENT_NAME { get; set; }

        //for template 315
        public string PATIENT_CODE { get; set; }
        public long DOB { get; set; }
        public string GENDER_CODE { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string REASON_INPUT_CODE { get; set; }
        public string MEDI_ORG_FROM_CODE { get; set; }
        public decimal TOTAL_DAY { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public long INSURANCE_YEAR { get; set; }
        public long INSURANCE_MONTH { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public string TREATMENT_CODE { get; set; }


        private void SetExtendField(Dictionary<long, V_HIS_TREATMENT> dicTreatment)
        {
            if (dicTreatment.ContainsKey(this.TREATMENT_ID) && dicTreatment[this.TREATMENT_ID].TDL_PATIENT_DOB > 0)
            {
                try
                {
                    if (dicTreatment[this.TREATMENT_ID].TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.DOB_MALE = long.Parse((dicTreatment[this.TREATMENT_ID].TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    else if (dicTreatment[this.TREATMENT_ID].TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.DOB_FEMALE = long.Parse((dicTreatment[this.TREATMENT_ID].TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    this.VIR_PATIENT_NAME = dicTreatment[this.TREATMENT_ID].TDL_PATIENT_NAME;
                    this.TREATMENT_CODE = dicTreatment[this.TREATMENT_ID].TREATMENT_CODE;
                    this.PATIENT_CODE = dicTreatment[this.TREATMENT_ID].TDL_PATIENT_CODE;
                    this.DOB = dicTreatment[this.TREATMENT_ID].TDL_PATIENT_DOB;
                    this.GENDER_CODE = (HisGenderCFG.HisGender.FirstOrDefault(o => o.ID == dicTreatment[this.TREATMENT_ID].TDL_PATIENT_GENDER_ID) ?? new HIS_GENDER()).GENDER_CODE;
                    this.ICD_CODE_EXTRA = dicTreatment[this.TREATMENT_ID].ICD_SUB_CODE;
                    this.MEDI_ORG_FROM_CODE = dicTreatment[this.TREATMENT_ID].TRANSFER_IN_MEDI_ORG_CODE;
                    this.TOTAL_DAY = dicTreatment[this.TREATMENT_ID].TREATMENT_DAY_COUNT ?? 0;
                    this.TREATMENT_RESULT_CODE = dicTreatment[this.TREATMENT_ID].TREATMENT_RESULT_CODE;
                    this.TREATMENT_END_TYPE_CODE = dicTreatment[this.TREATMENT_ID].TREATMENT_END_TYPE_CODE;
                    this.IN_TIME = dicTreatment[this.TREATMENT_ID].IN_TIME;
                    this.OUT_TIME = dicTreatment[this.TREATMENT_ID].OUT_TIME;
                    if (dicTreatment[this.TREATMENT_ID].END_DEPARTMENT_ID.HasValue)
                    {
                        var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == dicTreatment[this.TREATMENT_ID].END_DEPARTMENT_ID);
                        if (departmentCodeBHYT != null)
                        {
                            this.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                            this.DEPARTMENT_NAME = departmentCodeBHYT.DEPARTMENT_NAME;
                        }
                    }
                    if (dicTreatment[this.TREATMENT_ID].FEE_LOCK_TIME.HasValue)
                    {
                        this.INSURANCE_YEAR = Convert.ToInt64(dicTreatment[this.TREATMENT_ID].FEE_LOCK_TIME.ToString().Substring(0, 4));
                        this.INSURANCE_MONTH = Convert.ToInt64(dicTreatment[this.TREATMENT_ID].FEE_LOCK_TIME.ToString().Substring(4, 2));
                    }
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
        }

        private void SetExtendField(V_HIS_PATIENT_TYPE_ALTER heinApproval)
        {
            if (heinApproval!=null)
            {
                try
                {
                    if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                    {
                        if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            this.REASON_INPUT_CODE = "2";
                        }
                        else
                        {
                            this.REASON_INPUT_CODE = "1";
                        }
                    }
                    else if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    {
                        this.REASON_INPUT_CODE = "3";
                    }
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
        }

        public Mrs00329RDO()
        {

        }

        public Mrs00329RDO(V_HIS_PATIENT_TYPE_ALTER heinApproval, Dictionary<long, V_HIS_TREATMENT> dicTreatment)
        {
            try
            {
                if (heinApproval != null)
                {
                    System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<V_HIS_PATIENT_TYPE_ALTER>();
                    foreach (var item in pis)
                    {
                        item.SetValue(this, item.GetValue(heinApproval));
                    }
                    SetExtendField(dicTreatment);
                    SetExtendField(heinApproval);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
