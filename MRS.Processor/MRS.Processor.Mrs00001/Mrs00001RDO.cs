using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using IcdVn;

namespace MRS.Processor.Mrs00001
{
    public class Mrs00001RDO
    {
        public Mrs00001RDO()
        {
            // TODO: Complete member initialization
        }
        public long FILTER_TIME { get; set; }
        public long FILTER_DATE { get; set; }
        public long FILTER_MONTH { get; set; }
        public long WEEK_DAY { get; set; }
        public long? PATIENT_CASE_ID { get; set; }
        public string RELATIVE_MOBILE { get; set; }
        public string RELATIVE_PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }//Nơi chuyển bệnh nhân đến
        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }//Nơi chuyển bệnh nhân đến
        public string PATIENT_TYPE_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }//Phòng thực hiện
        public string EXECUTE_DEPARTMENT_NAME { get; set; }//Khoa thực hiện
        public string REQUEST_ROOM_NAME { get; set; }//Phòng thực hiện
        public string IS_BHYT { get; set; }// Là đối tượng bệnh nhân bảo hiểm
        public string IS_THUPHI { get; set; } // là bệnh nhân thu phí
        public string IS_FREE { get; set; } // là bệnh nhân viện phí
        public string IS_NOBHYT { get; set; }// Không là BN bảo hiểm
        public string IS_CHILD { get; set; }// là bệnh nhân trẻ em
        public string IS_CHILD_BHYT { get; set; }// là bệnh nhân trẻ em có bhyt
        public string IS_CHILD_6 { get; set; }// là bệnh nhân trẻ em dưới 6 tuổi
        public string IS_CHILD_6_BHYT { get; set; }// là bệnh nhân trẻ em dưới 6 tuổi có bhyt
        public string HEIN_CARD_NUMBER { get; set; }// mã thẻ bh
        public string MEDI_ORG_NAME { get; set; }// nơi khám bệnh ban đầu
        public string ICD_NAME_TEXT { get; set; }//Chẩn đoán tuyến dưới
        public string ICD_NAMEs { get; set; }//chẩn đoán chính
        public string ICD_SUB { get; set; }//Chẩn đoán phụ
        public string PATIENT_TYPE_GROUP_SERVICE_TICK { get; set; }//Tích đối tượng dịch vụ
        public string PATIENT_TYPE_GROUP_FREE_TICK { get; set; }//Tích đối tượng miễn phí
        public string PATIENT_TYPE_GROUP_HEIN_TICK { get; set; }//Tích đối tượng bảo hiểm
        public string EXSR_FINISH_TYPE_HOSPITALIZED_IN_TICK { get; set; }//Tích xử lí nhập viện nội trú
        public string EXSR_FINISH_TYPE_HOSPITALIZED_OUT_TICK { get; set; }// tích xử lí điều trị ngoại trú
        public string EXSR_FINISH_TYPE_HOME_TICK { get; set; }//Tích cấp toa cho về
        public string EXSR_FINISH_TYPE_TRANSPORT_TICK { get; set; }//Tích kết thúc chuyển viện
        public string EXSR_FINISH_TYPE_TRANSPORT_UP_TICK { get; set; }//Tích kết thúc chuyển viện tuyến trên
        public string EXSR_FINISH_TYPE_TRANSPORT_DOWN_TICK { get; set; }//Tích kết thúc chuyển viện tuyến dưới
        public string EXSR_FINISH_TYPE_TRANSPORT_EQUAL_TICK { get; set; }//Tích kết thúc chuyển viện cùng tuyến
        public string HAS_MISU_SERVICE_REQ_TICK { get; set; }// tích có làm thủ thuật
        public string MISU_SERVICE_TYPE_NAMEs { get; set; }
        public string IS_SPECIALITY_TICK { get; set; }
        public string IS_EMERGENCY_TICK { get; set; }
        public string IS_MOV_ROOM_TICK { get; set; }
        public int? EXECUTE_TIME { get; set; }
        public string MEDICINE_TYPE_NAMEs { get; set; }
        public string SERVICE_NAMEs { get; set; }
        public string ICDVN_CODE { get; set; }
        public string ICDVN_CAUSE_CODE { get; set; }
        public string ETHNIC_NAME { get; set; }
        public string PATIENT_CASE_NAME { get; set; }
        public long? CREATE_TIME { get; set; }
        public long ID { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long TREATMENT_ID { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        
        public string AN { get; set; }
        public string CDHA { get; set; }
        public string G { get; set; }
        public string GPBL { get; set; }
        public string THUOC { get; set; }
        public string VT { get; set; }
        public string KHAC { get; set; }
        public string MAU { get; set; }
        public string NS { get; set; }
        public string PHCN { get; set; }
        public string PT { get; set; }
        public string SA { get; set; }
        public string TDCN { get; set; }
        public string TT { get; set; }
        public string XN { get; set; }
        public string THUOC_BAN { get; set; }

        public string NEXT_MEDI_ORG_NAME { get; set; }

        public string NEXT_MEDI_ORG_CODE { get; set; }

        public string TREATMENT_END_TYPE_NAME { get; set; }

        public string TREATMENT_END_TYPE_CODE { get; set; }

        public string DEPARTMENT_IN_NAME { get; set; }

        public string ICD_CODE { get; set; }

        public string ICD_NAME { get; set; }

        public string ICD_SUB_CODE { get; set; }

        public string ICD_TEXT { get; set; }

        public string EXECUTE_USERNAME { get; set; }

        public string EXECUTE_LOGINNAME { get; set; }

        public long? FINISH_TIME { get; set; }

        public string ICD_CAUSE_CODE { get; set; }

        public string ICD_CAUSE_NAME { get; set; }

        public long INTRUCTION_DATE { get; set; }

        public short? IS_EMERGENCY { get; set; }

        public short? IS_MAIN_EXAM { get; set; }
        
        public string REQUEST_LOGINNAME { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long SERVICE_REQ_TYPE_ID { get; set; }
        public string SESSION_CODE { get; set; }
        public long? SICK_DAY { get; set; }
        public long? START_TIME { get; set; }
        public short? IS_NOT_REQUIRE_FEE { get; set; }
        public long? TRACKING_ID { get; set; }
        public string TREATMENT_INSTRUCTION { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }

        public string TDL_PATIENT_MOBILE { get; set; }

        public string TDL_PATIENT_CAREER_NAME { get; set; }

        public string TDL_PATIENT_CODE { get; set; }

        public string TDL_PATIENT_COMMUNE_CODE { get; set; }

        public string TDL_PATIENT_PHONE { get; set; }

      //  public string TDL_PATIENT_RELATIVE_PHONE { get; set; }
        

        public string TDL_PATIENT_DISTRICT_CODE { get; set; }

        public long? TDL_PATIENT_DOB { get; set; }

        public long? TDL_PATIENT_GENDER_ID { get; set; }

        public long TDL_PATIENT_ID { get; set; }

        public string TDL_PATIENT_MILITARY_RANK_NAME { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public string TDL_PATIENT_NATIONAL_NAME { get; set; }

        public string TDL_PATIENT_PROVINCE_CODE { get; set; }

        public string TDL_PATIENT_WORK_PLACE { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public string TDL_PATIENT_WORK_PLACE_NAME { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }

        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }

        public string TDL_HEIN_MEDI_ORG_NAME { get; set; }

        public string TDL_HEIN_MEDI_ORG_GRADE { get; set; }

        public string TDL_TREATMENT_CODE { get; set; }

        public long? IN_ROOM_ID { get; set; }

        public long? END_ROOM_ID { get; set; }

        public short? IS_PAUSE { get; set; }

        public string TDL_PATIENT_ADDRESS { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public string TRANSFER_IN_ICD_NAME { get; set; }

        public string TRANSFER_IN_ICD_CODE { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public string MEDI_ORG_CODE { get; set; }

        public long? TREATMENT_END_TYPE_ID { get; set; }

        public long? TRAN_PATI_FORM_ID { get; set; }

        public long? TRAN_PATI_REASON_ID { get; set; }

        public bool? HAS_COUNT { get; set; }

        public long? END_DEPARTMENT_ID { get; set; }

        public long? IN_TREATMENT_TYPE_ID { get; set; }

        public long? TDL_FIRST_EXAM_ROOM_ID { get; set; }
        public string APPOINTMENT_TICK { get; set; }

        public string EXAM_SERVICE_CODE { get; set; }

        public string EXAM_SERVICE_NAME { get; set; }

        public string TDL_SERVICE_IDS { get; set; }

        public string SERVICE_GROUP_NAME { get; set; }
    }
}
