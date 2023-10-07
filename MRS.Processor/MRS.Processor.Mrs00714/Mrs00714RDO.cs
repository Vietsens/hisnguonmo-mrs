using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00714
{
    public class Mrs00714RDO
    {
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public long? TDL_INTRUCTION_TIME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string HAS_PARRENT { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public string BEFORE_PTTT_ICD_NAME { get; set; }
        public string AFTER_PTTT_ICD_NAME { get; set; }
        public string PTTT_METHOL_NAME { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public string SV_PTTT_GROUP_NAME { get; set; }
        public string PTTT_GROUP_CODE { get; set; }
        public string SV_PTTT_GROUP_CODE { get; set; }
        public string START_TIME { get; set; }
        public string NOTE { get; set; }

        public string PATIENT_CODE { get; set; }
        public string IN_CODE { get; set; }
        public string TREATMENT_PATIENT_TYPE_NAME { get; set; }
        public decimal BHYT_RATIO { get; set; }
        public string BEGIN_TIME { get; set; }
        public string END_TIME { get; set; }
        public long PTTT_NUM_ORDER { get; set; }
        public long? EKIP_ID { get; set; }
        public long AGE { get; set; }

        public string DEATH_WITHIN_NAME { get; set; }
        public string EMOTIONLESS_METHOD_NAME { get; set; }
        public string PTTT_CATASTROPHE_NAME { get; set; }
        public string PTTT_CONDITION_NAME { get; set; }
        public string REAL_PTTT_METHOD_NAME { get; set; }
        public string MANNER { get; set; }

        public long? TDL_TREATMENT_ID { get; set; }
        public long? ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? PARENT_ID { get; set; }
        public long? PTTT_METHOD_ID { get; set; }
        public long? PTTT_GROUP_ID { get; set; }
        public long? SV_PTTT_GROUP_ID { get; set; }
        public long? DEATH_WITHIN_ID { get; set; }
        public long? EMOTIONLESS_METHOD_ID { get; set; }
        public long? PTTT_CATASTROPHE_ID { get; set; }
        public long? PTTT_CONDITION_ID { get; set; }
        public long? REAL_PTTT_METHOD_ID { get; set; }
        public long? EMOTIONLESS_METHOD_SECOND_ID { get; set; }
        public long SERVICE_REQ_ID { get; set; }
        public long? L_START_TIME { get; set; }
        public decimal ORIGINAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
        public decimal VAT_RATIO { get; set; }
        public decimal? HEIN_LIMIT_PRICE { get; set; }
        public long? DOB { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? L_BEGIN_TIME { get; set; }
        public long? L_END_TIME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string ICD_NAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string BEGIN_TIME_STR { get; set; }
        public long INTRUCTION_MONTH { get; set; }

        public Mrs00714RDO() { }

        /// <summary>
        /// tạo ado gom nhóm theo loại dịch vụ
        /// </summary>
        /// <param name="datas"></param>
        public Mrs00714RDO(List<Mrs00714RDO> datas)
        {
            if (datas != null && datas.Count > 0)
            {
                var data = datas.First();
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00714RDO>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }

                this.AMOUNT = datas.Sum(s => s.AMOUNT);
                this.VIR_TOTAL_PRICE = datas.Sum(s => s.VIR_TOTAL_PRICE);
                this.VIR_PRICE = 0;
                this.BHYT_RATIO = 0;
                this.ORIGINAL_PRICE = 0;
                this.PRICE = 0;
                this.VAT_RATIO = 0;
                this.HEIN_LIMIT_PRICE = null;
            }
        }
    }

    public class SS_USER_REMUNERATION : Mrs00714RDO
    {
        public long SERE_SERV_ID { get; set; }
        public string LOGINNAME { get; set; }
        public string USER_NAME { get; set; }
        public string EXECUTE_ROLE_CODE { get; set; }
        public string EXECUTE_ROLE_NAME { get; set; }
        public decimal REMUNERATION_PRICE { get; set; }
    }
    public class USER_COUNT_PTTT: Mrs00714RDO{
        public long SERE_SERV_ID { get; set; }
        public string LOGINNAME { get; set; }
        public string USER_NAME { get; set; }
        public string EXECUTE_ROLE_CODE { get; set; }
        public string EXECUTE_ROLE_NAME { get; set; }

        public long COUNT_PTTT_1_MAIN{set;get;}
        public long COUNT_PTTT_2_MAIN { set; get; }
        public long COUNT_PTTT_3_MAIN { set; get; }
        public long COUNT_PTTT_DB_MAIN { set; get; }
        public long COUNT_PTTT_KHAC_MAIN { get; set; }

        public long COUNT_PTTT_1_SUPPORT { set; get; }
        public long COUNT_PTTT_2_SUPPORT { set; get; }
        public long COUNT_PTTT_3_SUPPORT { set; get; }
        public long COUNT_PTTT_DB_SUPPORT { set; get; }
        public long COUNT_PTTT_KHAC_SUPPORT { get; set; }

        public long COUNT_PTTT_1_HELPER { set; get; }
        public long COUNT_PTTT_2_HELPER { set; get; }
        public long COUNT_PTTT_3_HELPER { set; get; }
        public long COUNT_PTTT_DB_HELPER { set; get; }
        public long COUNT_PTTT_KHAC_HELPER { get; set; }

        public long COUNT_PTTT_1_KHAC { set; get; }
        public long COUNT_PTTT_2_KHAC { set; get; }
        public long COUNT_PTTT_3_KHAC { set; get; }
        public long COUNT_PTTT_DB_KHAC { set; get; }
        public long COUNT_PTTT_KHAC_KHAC { get; set; }
    }
}
