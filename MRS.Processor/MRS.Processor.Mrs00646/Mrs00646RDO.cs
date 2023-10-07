using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00646
{
    class Mrs00646RDO : V_HIS_EXP_MEST_MEDICINE
    {
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string REQ_LOGINNAME { get; set; }
        public string REQ_USERNAME { get; set; }

        public string TDL_AGGR_EXP_MEST_CODE { get; set; }
        public string TDL_DISPENSE_CODE { get; set; }
        public long? TDL_INTRUCTION_TIME { get; set; }
        public string TDL_MANU_IMP_MEST_CODE { get; set; }

        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public short? TDL_PATIENT_IS_HAS_NOT_DAY_DOB { get; set; }
        public string TDL_PATIENT_LAST_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }

        public string TDL_PRESCRIPTION_CODE { get; set; }
        public string TDL_PRESCRIPTION_REQ_LOGINNAME { get; set; }
        public string TDL_PRESCRIPTION_REQ_USERNAME { get; set; }
        public string TDL_SERVICE_REQ_CODE { get; set; }
        public decimal? TDL_TOTAL_PRICE { get; set; }

        public int? TOTAL_DAY_USE { get; set; }

        public string PACKING_TYPE_NAME { get; set; }

        public Mrs00646RDO(MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_MEDICINE medi, MOS.EFMODEL.DataModels.HIS_EXP_MEST expMest)
        {
            try
            {
                if (medi != null)
                {
                    this.AMOUNT = medi.AMOUNT;
                    this.BID_NAME = medi.BID_NAME;
                    this.BID_NUMBER = medi.BID_NUMBER;
                    this.CONCENTRA = medi.CONCENTRA;
                    this.DESCRIPTION = medi.DESCRIPTION;
                    this.DISCOUNT = medi.DISCOUNT;
                    this.EXP_MEST_CODE = medi.EXP_MEST_CODE;
                    this.EXP_LOGINNAME = medi.EXP_LOGINNAME;
                    this.EXP_TIME = medi.EXP_TIME;
                    this.EXP_USERNAME = medi.EXP_USERNAME;
                    this.EXPIRED_DATE = medi.EXPIRED_DATE;
                    this.IMP_PRICE = medi.IMP_PRICE;
                    this.IMP_VAT_RATIO = medi.IMP_VAT_RATIO;
                    this.INTERNAL_PRICE = medi.INTERNAL_PRICE;
                    this.MANUFACTURER_CODE = medi.MANUFACTURER_CODE;
                    this.MANUFACTURER_NAME = medi.MANUFACTURER_NAME;
                    this.MEDICINE_NUM_ORDER = medi.MEDICINE_NUM_ORDER;
                    this.MEDICINE_REGISTER_NUMBER = medi.MEDICINE_REGISTER_NUMBER;
                    this.MEDICINE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                    this.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                    this.MEDICINE_TYPE_NUM_ORDER = medi.MEDICINE_TYPE_NUM_ORDER;
                    this.NATIONAL_NAME = medi.NATIONAL_NAME;
                    this.NUM_ORDER = medi.NUM_ORDER;
                    this.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                    this.PATIENT_TYPE_CODE = medi.PATIENT_TYPE_CODE;
                    this.PATIENT_TYPE_NAME = medi.PATIENT_TYPE_NAME;
                    this.PRICE = medi.PRICE;
                    this.REGISTER_NUMBER = medi.REGISTER_NUMBER;
                    this.SERVICE_UNIT_CODE = medi.SERVICE_UNIT_CODE;
                    this.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                    this.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                    this.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                    this.TH_AMOUNT = medi.TH_AMOUNT;
                    this.TUTORIAL = medi.TUTORIAL;
                    this.USE_TIME_TO = medi.USE_TIME_TO;
                    this.VAT_RATIO = medi.VAT_RATIO;
                    this.MEDICINE_GROUP_ID = medi.MEDICINE_GROUP_ID;
                }

                if (expMest != null)
                {
                    this.REQ_LOGINNAME = expMest.REQ_LOGINNAME;
                    this.REQ_USERNAME = expMest.REQ_USERNAME;
                    var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == expMest.REQ_DEPARTMENT_ID);
                    if (department != null)
                    {
                        this.REQ_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                    }


                    this.TDL_AGGR_EXP_MEST_CODE = expMest.TDL_AGGR_EXP_MEST_CODE;
                    this.TDL_DISPENSE_CODE = expMest.TDL_DISPENSE_CODE;
                    this.TDL_INTRUCTION_TIME = expMest.TDL_INTRUCTION_TIME;
                    this.TDL_MANU_IMP_MEST_CODE = expMest.TDL_MANU_IMP_MEST_CODE;
                    this.TDL_PATIENT_ADDRESS = expMest.TDL_PATIENT_ADDRESS;
                    this.TDL_PATIENT_CODE = expMest.TDL_PATIENT_CODE;
                    this.TDL_PATIENT_DOB = expMest.TDL_PATIENT_DOB;
                    this.TDL_PATIENT_FIRST_NAME = expMest.TDL_PATIENT_FIRST_NAME;
                    this.TDL_PATIENT_GENDER_ID = expMest.TDL_PATIENT_GENDER_ID;
                    this.TDL_PATIENT_GENDER_NAME = expMest.TDL_PATIENT_GENDER_NAME;
                    this.TDL_PATIENT_IS_HAS_NOT_DAY_DOB = expMest.TDL_PATIENT_IS_HAS_NOT_DAY_DOB;
                    this.TDL_PATIENT_LAST_NAME = expMest.TDL_PATIENT_LAST_NAME;
                    this.TDL_PATIENT_NAME = expMest.TDL_PATIENT_NAME;
                    this.TDL_TREATMENT_CODE = expMest.TDL_TREATMENT_CODE;
                    this.TDL_PRESCRIPTION_CODE = expMest.TDL_PRESCRIPTION_CODE;
                    this.TDL_PRESCRIPTION_REQ_LOGINNAME = expMest.TDL_PRESCRIPTION_REQ_LOGINNAME;
                    this.TDL_PRESCRIPTION_REQ_USERNAME = expMest.TDL_PRESCRIPTION_REQ_USERNAME;
                    this.TDL_SERVICE_REQ_CODE = expMest.TDL_SERVICE_REQ_CODE;
                    this.TDL_TOTAL_PRICE = expMest.TDL_TOTAL_PRICE;
                }

                if (this.TDL_INTRUCTION_TIME.HasValue && this.USE_TIME_TO.HasValue)
                {
                    this.TOTAL_DAY_USE = CountDayBetween(this.TDL_INTRUCTION_TIME.Value, this.USE_TIME_TO.Value);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int? CountDayBetween(long intructionTime, long useTimeTo)
        {
            int? result = null;
            try
            {
                if (intructionTime > useTimeTo)
                {
                    DateTime dtUseTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(useTimeTo).Value;
                    DateTime dtUseTimeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(intructionTime).Value;
                    TimeSpan ts = dtUseTimeTo - dtUseTime;
                    result = ts.Days + 1;
                }
                else
                {
                    DateTime dtUseTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(intructionTime).Value;
                    DateTime dtUseTimeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(useTimeTo).Value;
                    TimeSpan ts = dtUseTimeTo - dtUseTime;
                    result = ts.Days + 1;
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
