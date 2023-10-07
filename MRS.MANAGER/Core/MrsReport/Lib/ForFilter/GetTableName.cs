using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.Lib
{
    class GetTableName
    {
        public static string HisFilterTypes(string KeyFilter)
        {
            string result = null;
            try
            {
                if (KeyFilter != null && !IsCodeField(KeyFilter))
                {
                    if (KeyFilter.StartsWith("\"CURRENTBRANCH_"))
                    {
                        if (KeyFilter.Contains("DEPARTMENT_ID"))
                        {
                            result = "HIS_DEPARTMENT";
                        }
                        else if (KeyFilter.Contains("ROOM_ID"))
                        {
                            result = "V_HIS_ROOM";
                        }
                        else if (KeyFilter.Contains("MEDI_STOCK_ID"))
                        {
                            result = "HIS_MEDI_STOCK";
                        }
                    }
                    else if (KeyFilter.StartsWith("\"EXACT_"))
                    {
                        if (KeyFilter.Contains("BRANCH_ROOM_ID"))
                        {
                            result = "V_HIS_ROOM";
                        }
                        if (KeyFilter.Contains("CASHIER_ROOM_ID"))
                        {
                            result = "HIS_CASHIER_ROOM";
                        }
                        else if (KeyFilter.Contains("EXT_CASHIER_ROOM_ID"))
                        {
                            result = "HIS_CASHIER_ROOM";
                        }
                        else if (KeyFilter.Contains("PARENT_SERVICE_ID"))
                        {
                            result = "HIS_SERVICE";
                        }
                        else if (KeyFilter.Contains("CHILD_SERVICE_ID"))
                        {
                            result = "HIS_SERVICE";
                        }
                        else if (KeyFilter.Contains("MEST_ROOM_ID"))
                        {
                            result = "HIS_MEST_ROOM";
                        }
                        else if (KeyFilter.Contains("SAMPLE_ROOM_ID"))
                        {
                            result = "HIS_SAMPLE_ROOM";
                        }
                        else if (KeyFilter.Contains("RECEPTION_ROOM_ID"))
                        {
                            result = "HIS_RECEPTION_ROOM";
                        }
                        else if (KeyFilter.Contains("BED_ROOM_ID"))
                        {
                            result = "HIS_BED_ROOM";
                        }
                        else if (KeyFilter.Contains("EXECUTE_ROOM_ID"))
                        {
                            result = "HIS_EXECUTE_ROOM";
                        }
                        else if (KeyFilter.Contains("TREATMENT_BED_ROOM_ID"))
                        {
                            result = "HIS_TREATMENT_BED_ROOM";
                        }
                        else if (KeyFilter.Contains("SERVICE_ROOM_ID"))
                        {
                            result = "HIS_SERVICE_ROOM";
                        }
                        else if (KeyFilter.Contains("STORE_MEDI_STOCK_ID"))
                        {
                            result = "HIS_STORE_MEDI_STOCK";
                        }
                    }
                    else if (KeyFilter.Contains("PATY_AREA_ID"))
                    {
                        result = "HIS_AREA";
                    }
                    else if (KeyFilter.Contains("DEPA_AREA_ID"))
                    {
                        result = "HIS_AREA";
                    }
                    else if (KeyFilter.Contains("AREA_ID"))
                    {
                        result = "HIS_AREA";
                    }
                    else if (KeyFilter.Contains("ACCIDENT_RESULT_ID"))
                    {
                        result = "HIS_ACCIDENT_RESULT";
                    }
                    else if (KeyFilter.Contains("SERVICE_UNIT_ID"))
                    {
                        result = "HIS_SERVICE_UNIT";
                    }
                    else if (KeyFilter.Contains("MEDICINE_USE_FORM_ID"))
                    {
                        result = "HIS_MEDICINE_USE_FORM";
                    }
                    else if (KeyFilter.Contains("MEDICINE_GROUP_ID"))
                    {
                        result = "HIS_MEDICINE_GROUP";
                    }
                    else if (KeyFilter.Contains("ACCIDENT_HURT_TYPE_ID"))
                    {
                        result = "HIS_ACCIDENT_HURT_TYPE";
                    }
                    else if (KeyFilter.Contains("WORKING_SHIFT_ID"))
                    {
                        result = "HIS_WORKING_SHIFT";
                    }
                    else if (KeyFilter.Contains("EMPLOYEE_ID"))
                    {
                        result = "HIS_EMPLOYEE";
                    }
                    else if (KeyFilter.Contains("MEDICINE_ID"))
                    {
                        result = "HIS_MEDICINE";
                    }
                    else if (KeyFilter.Contains("PAY_FORM_ID"))
                    {
                        result = "HIS_PAY_FORM";
                    }
                    else if (KeyFilter.Contains("FUND_ID"))
                    {
                        result = "HIS_FUND";
                    }
                    else if (KeyFilter.Contains("MATERIAL_ID"))
                    {
                        result = "HIS_MATERIAL";
                    }
                    else if (KeyFilter.Contains("BLOOD_TYPE_ID"))
                    {
                        result = "HIS_BLOOD_TYPE";
                    }
                    else if (KeyFilter.Contains("CASHIER_LOGINNAME"))
                    {
                        result = "HIS_CASHIER_LOGINNAME";
                    }
                    else if (KeyFilter.Contains("MEDI_SUPPLIER_STOCK_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("MEDI_BIG_STOCK_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("REQUEST_ROOM_ID"))
                    {
                        result = "V_HIS_ROOM";
                    }
                    else if (KeyFilter.Contains("MY_SURG_ROOM_ID"))
                    {
                        result = "V_HIS_ROOM";
                    }
                    else if (KeyFilter.Contains("EXAM_ROOM_ID"))
                    {
                        result = "V_HIS_ROOM";
                    }
                    else if (KeyFilter.Contains("SURG_ROOM_ID"))
                    {
                        result = "V_HIS_ROOM";
                    }
                    else if (KeyFilter.Contains("MEDICINE_TYPE_ID"))
                    {
                        result = "HIS_MEDICINE_TYPE";
                    }
                    else if (KeyFilter.Contains("MATERIAL_TYPE_ID"))
                    {
                        result = "HIS_MATERIAL_TYPE";
                    }
                    else if (KeyFilter.Contains("SUPPLIER_ID"))
                    {
                        result = "HIS_SUPPLIER";
                    }
                    else if (KeyFilter.Contains("INVOICE_ID"))
                    {
                        result = "HIS_INVOICE";
                    }
                    else if (KeyFilter.Contains("INVOICE_BOOK_ID"))
                    {
                        result = "HIS_INVOICE_BOOK";
                    }
                    else if (KeyFilter.Contains("IMP_SOURCE_ID"))
                    {
                        result = "HIS_IMP_SOURCE";
                    }
                    else if (KeyFilter.Contains("MEDI_STOCK_NOT_BUSINESS_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("MEDI_STOCK_BUSINESS_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("SERVICE_GROUP_ID"))
                    {
                        result = "HIS_SERVICE_GROUP";
                    }
                    else if (KeyFilter.Contains("CAREER_ID"))
                    {
                        result = "HIS_CAREER";
                    }
                    else if (KeyFilter.Contains("KSK_CONTRACT_ID"))
                    {
                        result = "HIS_KSK_CONTRACT";
                    }
                    else if (KeyFilter.Contains("BRANCH_ID"))
                    {
                        result = "HIS_BRANCH";
                    }
                    else if (KeyFilter.Contains("CAREER_ID"))
                    {
                        result = "HIS_CAREER";
                    }
                    else if (KeyFilter.Contains("DEATH_CAUSE_ID"))
                    {
                        result = "HIS_DEATH_CAUSE";
                    }
                    else if (KeyFilter.Contains("PROGRAM_ID"))
                    {
                        result = "HIS_PROGRAM";
                    }
                    else if (KeyFilter.Contains("PATIENT_TYPE_ID"))
                    {
                        result = "HIS_PATIENT_TYPE";
                    }
                    else if (KeyFilter.Contains("IMP_MEST_TYPE_ID"))
                    {
                        result = "HIS_IMP_MEST_TYPE";
                    }
                    else if (KeyFilter.Contains("IMP_MEST_STT_ID"))
                    {
                        result = "HIS_IMP_MEST_STT";
                    }
                    else if (KeyFilter.Contains("EXECUTE_DEPARTMENT_ID"))
                    {
                        result = "HIS_DEPARTMENT";
                    }
                    else if (KeyFilter.Contains("CLINICAL_DEPARTMENT_ID"))
                    {
                        result = "HIS_CLINICAL_DEPARTMENT";
                    }
                    else if (KeyFilter.Contains("MY_DEPARTMENT_ID"))
                    {
                        result = "HIS_DEPARTMENT";
                    }
                    else if (KeyFilter.Contains("ROOM_TYPE_ID"))
                    {
                        result = "HIS_ROOM_TYPE";
                    }
                    else if (KeyFilter.Contains("EXECUTE_GROUP_ID"))
                    {
                        result = "HIS_EXECUTE_GROUP";
                    }
                    else if (KeyFilter.Contains("INFUSION_STT_ID"))
                    {
                        result = "HIS_INFUSION_STT";
                    }
                    else if (KeyFilter.Contains("GENDER_ID"))
                    {
                        result = "HIS_GENDER";
                    }
                    else if (KeyFilter.Contains("EXP_MEST_STT_ID"))
                    {
                        result = "HIS_EXP_MEST_STT";
                    }
                    else if (KeyFilter.Contains("EXP_MEST_TYPE_ID"))
                    {
                        result = "HIS_EXP_MEST_TYPE";
                    }
                    else if (KeyFilter.Contains("TREATMENT_RESULT_ID"))
                    {
                        result = "HIS_TREATMENT_RESULT";
                    }
                    else if (KeyFilter.Contains("TREATMENT_END_TYPE_ID"))
                    {
                        result = "HIS_TREATMENT_END_TYPE";
                    }
                    else if (KeyFilter.Contains("SERVICE_GROUP_ID"))
                    {
                        result = "HIS_SERVICE_GROUP";
                    }
                    else if (KeyFilter.Contains("SERVICE_TYPE_ID"))
                    {
                        result = "HIS_SERVICE_TYPE";
                    }
                    else if (KeyFilter.Contains("SERVICE_ID"))
                    {
                        result = "HIS_SERVICE";
                    }
                    else if (KeyFilter.Contains("TREATMENT_TYPE_ID"))
                    {
                        result = "HIS_TREATMENT_TYPE";
                    }
                    else if (KeyFilter.Contains("ACCOUNT_BOOK_ID"))
                    {
                        result = "HIS_ACCOUNT_BOOK";
                    }
                    else if (KeyFilter.Contains("ICD_GROUP_ID"))
                    {
                        result = "HIS_ICD_GROUP";
                    }
                    else if (KeyFilter.Contains("PTTT_METHOD_ID"))
                    {
                        result = "HIS_PTTT_METHOD";
                    }
                    else if (KeyFilter.Contains("PTTT_GROUP_ID"))
                    {
                        result = "HIS_PTTT_GROUP";
                    }
                    else if (KeyFilter.Contains("BLOOD_ID"))
                    {
                        result = "HIS_BLOOD";
                    }
                    else if (KeyFilter.Contains("BLOOD_RH_ID"))
                    {
                        result = "HIS_BLOOD_RH";
                    }
                    else if (KeyFilter.Contains("BID_ID"))
                    {
                        result = "HIS_BID";
                    }
                    else if (KeyFilter.Contains("BED_ID"))
                    {
                        result = "HIS_BED";
                    }
                    else if (KeyFilter.Contains("EXECUTE_ROLE_ID"))
                    {
                        result = "HIS_EXECUTE_ROLE";
                    }
                    else if (KeyFilter.Contains("SERVICE_REQ_TYPE_ID"))
                    {
                        result = "HIS_SERVICE_REQ_TYPE";
                    }
                    else if (KeyFilter.Contains("SERVICE_REQ_STT_ID"))
                    {
                        result = "HIS_SERVICE_REQ_STT";
                    }
                    else if (KeyFilter.Contains("REPORT_TYPE_CAT_ID"))
                    {
                        result = "HIS_REPORT_TYPE_CAT";
                    }
                    else if (KeyFilter.Contains("OTHER_MEDI_STOCK_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("IMP_MEDI_STOCK_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("MEDI_STOCK_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("DEPARTMENT_ID"))
                    {
                        result = "HIS_DEPARTMENT";
                    }
                    else if (KeyFilter.Contains("ICD_ID"))
                    {
                        result = "HIS_ICD";
                    }
                    else if (KeyFilter.Contains("MEST_ROOM_ID"))
                    {
                        result = "HIS_MEST_ROOM";
                    }
                    else if (KeyFilter.Contains("ROOM_ID"))
                    {
                        result = "V_HIS_ROOM";
                    }
                    else if (KeyFilter.Contains("EXP_MEST_REASON_ID"))
                    {
                        result = "HIS_EXP_MEST_REASON";
                    }
                    else if (KeyFilter.Contains("MEDI_STOCK_PERIOD_ID"))
                    {
                        result = "HIS_MEDI_STOCK_PERIOD";
                    }
                    else if (KeyFilter.Contains("WORK_PLACE_ID"))
                    {
                        result = "HIS_WORK_PLACE";
                    }
                    else if (KeyFilter.Contains("THE_BRANCH__ID") || KeyFilter.Contains("THE_BRANCH__ID_ADMIN"))
                    {
                        result = "THE_BRANCH";
                    }
                    else if (KeyFilter.Contains("TRANSACTION_TYPE_ID"))
                    {
                        result = "HIS_TRANSACTION_TYPE";
                    }
                    else if (KeyFilter.Contains("MACHINE_ID"))
                    {
                        result = "HIS_MACHINE";
                    }
                    else if (KeyFilter.Contains("MEDI_ORG_ID"))
                    {
                        result = "HIS_MEDI_ORG";
                    }
                    else if (KeyFilter.Contains("AOS_BANK_ACCOUNT_TYPE_ID"))
                    {
                        result = "AOS_BANK_ACCOUNT_TYPE";
                    }
                    else if (KeyFilter.Contains("MEDI_STOCK_CABINET_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                    else if (KeyFilter.Contains("MEST_ROOM_STOCK_ID"))
                    {
                        result = "HIS_MEDI_STOCK";
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static bool IsCodeField(string filter)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(filter) && (filter.Contains("CODE") || filter.Contains("LOGINNAME")))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Warn(ex);
            }
            return result;
        }
    }
    // MRS.MANAGER.Core.MrsReport.Lib.DataGet
    public class DataGet
    {
        public long ID { get; set; }

        public string CODE { get; set; }

        public string NAME { get; set; }
    }

}
