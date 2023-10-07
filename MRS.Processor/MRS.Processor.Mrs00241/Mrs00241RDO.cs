using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00241
{
    public class Mrs00241RDO : V_HIS_HEIN_APPROVAL
    {
        public long DOB_DATE { get; set; }
        public string HEIN_GENDER_CODE { get; set; }
        public long HEIN_CARD_FROM_TIME_STR { get; set; }
        public long HEIN_CARD_TO_TIME_STR { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string REASON_INPUT_CODE { get; set; }
        public string MEDI_ORG_FROM_CODE { get; set; }
        public string OPEN_TIME_SEPARATE_STR { get; set; }
        public string CLOSE_TIME_SEPARATE_STR { get; set; }
        public long? TOTAL_DAY { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
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
        public decimal TT_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public long INSURANCE_YEAR { get; set; }
        public long INSURANCE_MONTH { get; set; }
        public string BHYT_TREATMENT_TYPE_CODE { get; set; }
        public string CURRENT_MEDI_ORG_CODE { get; set; }
        public long PLACE_PAYMENT_CODE { get; set; }
        public long INSURANCE_STT { get; set; }
        public decimal REASON_OUT_PRICE { get; set; }
        public string REASON_OUT { get; set; }
        public decimal POLYLINE_PRICE { get; set; }
        public decimal EXCESS_PRICE { get; set; }
        public string ROUTE_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string VIR_ADDRESS { get; set; }

        //TheoThoi gian vao
        public string IN_TIME_STR { get; set; }
        public long? MAIN_DAY { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00241RDO(V_HIS_HEIN_APPROVAL data)
        {
            try
            {
                if (data != null)
                {
                    System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_HEIN_APPROVAL>();
                    foreach (var item in pi)
                    {
                        item.SetValue(this, item.GetValue(data));
                    }
                    this.PATIENT_CODE = data.TDL_PATIENT_CODE;
                    this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME;
                    this.VIR_ADDRESS = data.TDL_PATIENT_ADDRESS;
                    SetExtendField();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00241RDO()
        {
        }

        private void SetExtendField()
        {
            try
            {
                if (this.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    this.HEIN_GENDER_CODE = "1";
                }
                else if (this.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.HEIN_GENDER_CODE = "2";
                }

                this.DOB_DATE = Convert.ToInt64(this.TDL_PATIENT_DOB.ToString().Substring(0, 8));

                this.HEIN_CARD_FROM_TIME_STR = Convert.ToInt64(this.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8));
                this.HEIN_CARD_TO_TIME_STR = Convert.ToInt64(this.HEIN_CARD_TO_TIME.ToString().Substring(0, 8));

                //Ly do vao vien: 1 : Đúng tuyến;  2 : Cấp cứu;  3 : Trái tuyến
                if (this.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                {
                    if (this.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                    {
                        this.REASON_INPUT_CODE = "2";
                    }
                    else
                    {
                        this.REASON_INPUT_CODE = "1";
                    }
                }
                else if (this.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                {
                    this.REASON_INPUT_CODE = "3";
                }

                this.INSURANCE_YEAR = Convert.ToInt64(this.EXECUTE_TIME.ToString().Substring(0, 4));
                this.INSURANCE_MONTH = Convert.ToInt64(this.EXECUTE_TIME.ToString().Substring(4, 2));

                //Noi thanh toan: 1: thanh toan tai co so;  2: thanh toan truc tiep
                this.PLACE_PAYMENT_CODE = 1;
                //Giam dinh: 0: không thẩm định;  1: chấp nhận;  2: điều chỉnh;  3: xuất toán
                this.INSURANCE_STT = 0;
                this.REASON_OUT_PRICE = 0;
                this.REASON_OUT = "";
                this.POLYLINE_PRICE = 0;
                this.EXCESS_PRICE = 0;
                this.ROUTE_CODE = "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
