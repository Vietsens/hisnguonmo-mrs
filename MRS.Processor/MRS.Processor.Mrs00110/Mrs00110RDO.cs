using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00110
{
    class Mrs00110RDO
    {
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }

        public long EXP_MEST_ID { get; set; }
        public string EXP_MEST_CODE { get; set; }

        public long EXP_TIME { get; set; }
        public long EXP_DATE { get; set; }
        public string EXP_DATE_STR { get; set; }

        public decimal TOTAL_AMOUNT { get; set; }

        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        #region Thong in phieu xuat
        public string EXP_MEST_TYPE_CODE { get; set; }

        public string REQ_LOGINNAME { get; set; }
        public string REQ_ROOM_CODE { get; set; }

        public string REQ_ROOM_NAME { get; set; }
        public string REQ_USERNAME { get; set; }
        public string TDL_AGGR_EXP_MEST_CODE { get; set; }
        public long? TDL_INTRUCTION_DATE { get; set; }
        public long? TDL_INTRUCTION_TIME { get; set; }
        public string TDL_MANU_IMP_MEST_CODE { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public short? TDL_PATIENT_IS_HAS_NOT_DAY_DOB { get; set; }
        public string TDL_PATIENT_LAST_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PRESCRIPTION_CODE { get; set; }
        public string TDL_SERVICE_REQ_CODE { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        #endregion
        public Mrs00110RDO() { }

        public Mrs00110RDO(List<V_HIS_EXP_MEST_MEDICINE> datas)
        {
            try
            {
                MEDICINE_TYPE_ID = datas.First().MEDICINE_TYPE_ID;
                MEDICINE_TYPE_CODE = datas.First().MEDICINE_TYPE_CODE;
                MEDICINE_TYPE_NAME = datas.First().MEDICINE_TYPE_NAME;
                EXP_MEST_ID = datas.First().EXP_MEST_ID ?? 0;
                EXP_MEST_CODE = datas.First().EXP_MEST_CODE;
                TOTAL_AMOUNT = datas.Sum(s => s.AMOUNT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        internal void SetExpMest(V_HIS_EXP_MEST depaExpMest)
        {
            try
            {
                EXP_TIME = depaExpMest.FINISH_TIME ?? 0;
                EXP_DATE = Convert.ToInt64(EXP_TIME.ToString().Substring(0, 8));
                EXP_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(EXP_TIME);
                DEPARTMENT_ID = depaExpMest.REQ_DEPARTMENT_ID;
                DEPARTMENT_CODE = depaExpMest.REQ_DEPARTMENT_CODE;
                DEPARTMENT_NAME = depaExpMest.REQ_DEPARTMENT_NAME;
                MEDI_STOCK_ID = depaExpMest.MEDI_STOCK_ID;
                MEDI_STOCK_CODE = depaExpMest.MEDI_STOCK_CODE;
                MEDI_STOCK_NAME = depaExpMest.MEDI_STOCK_NAME;
                EXP_MEST_TYPE_CODE = depaExpMest.EXP_MEST_TYPE_CODE;

                REQ_LOGINNAME = depaExpMest.REQ_LOGINNAME;
                REQ_ROOM_CODE = depaExpMest.REQ_ROOM_CODE;

                REQ_ROOM_NAME = depaExpMest.REQ_ROOM_NAME;
                REQ_USERNAME = depaExpMest.REQ_USERNAME;
                TDL_AGGR_EXP_MEST_CODE = depaExpMest.TDL_AGGR_EXP_MEST_CODE;
                TDL_INTRUCTION_DATE = depaExpMest.TDL_INTRUCTION_DATE;
                TDL_INTRUCTION_TIME = depaExpMest.TDL_INTRUCTION_TIME;
                TDL_MANU_IMP_MEST_CODE = depaExpMest.TDL_MANU_IMP_MEST_CODE;
                TDL_PATIENT_ADDRESS = depaExpMest.TDL_PATIENT_ADDRESS;
                TDL_PATIENT_CODE = depaExpMest.TDL_PATIENT_CODE;
                TDL_PATIENT_DOB = depaExpMest.TDL_PATIENT_DOB;
                TDL_PATIENT_FIRST_NAME = depaExpMest.TDL_PATIENT_FIRST_NAME;
                TDL_PATIENT_GENDER_NAME = depaExpMest.TDL_PATIENT_GENDER_NAME;
                TDL_PATIENT_IS_HAS_NOT_DAY_DOB = depaExpMest.TDL_PATIENT_IS_HAS_NOT_DAY_DOB;
                TDL_PATIENT_LAST_NAME = depaExpMest.TDL_PATIENT_LAST_NAME;
                TDL_PATIENT_NAME = depaExpMest.TDL_PATIENT_NAME;
                TDL_PRESCRIPTION_CODE = depaExpMest.TDL_PRESCRIPTION_CODE;
                TDL_SERVICE_REQ_CODE = depaExpMest.TDL_SERVICE_REQ_CODE;
                TDL_TREATMENT_CODE = depaExpMest.TDL_TREATMENT_CODE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
