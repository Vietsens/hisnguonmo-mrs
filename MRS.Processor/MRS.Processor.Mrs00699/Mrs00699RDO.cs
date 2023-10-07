using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00699
{
    class Mrs00699RDO
    {
        /// <summary>
        /// Số lưu trữ | Mã chứng từ
        /// STORE_CODE
        /// </summary>
        public string MA_CT { get; set; }

        /// <summary>
        /// "Ngày tạo chứng từ
        /// Định dạng: dd/MM/yyyy"
        /// OUT_TIME
        /// </summary>
        public string NGAY_CT { get; set; }

        /// <summary>
        /// Không điền số seri: Số seri để trống thì khi lưu hệ thống BHYT sẽ tự sinh
        /// </summary>
        public string SO_SERI { get; set; }

        /// <summary>
        /// Mã BHYT cơ sở KCB
        /// HEIN_MEDI_ORG_CODE trong HIS_BRANCH tương ứng với hồ sơ
        /// </summary>
        public string MA_CSKCB { get; set; }

        /// <summary>
        /// Mã BHYT của khoa kết thúc điều trị
        /// END_DEPARTMENT_ID
        /// </summary>
        public string MA_KHOA { get; set; }

        /// <summary>
        /// 10 số cuối số thẻ BHYT
        /// TDL_HEIN_CARD_NUMBER
        /// </summary>
        public string MA_SOBHXH { get; set; }

        /// <summary>
        /// Số thẻ BHYT
        /// TDL_HEIN_CARD_NUMBER
        /// </summary>
        public string MA_THE { get; set; }

        /// <summary>
        /// Địa chỉ thẻ
        /// 
        /// </summary>
        public string DIA_CHI { get; set; }

        /// <summary>
        /// Họ tên của người ra viện
        /// VIR_PATIENT_NAME
        /// </summary>
        public string HO_TEN { get; set; }

        /// <summary>
        /// Định dạng dd/MM/yyyy hoặc yyyy
        /// DOB
        /// </summary>
        public string NGAY_SINH { get; set; }

        /// <summary>
        /// Dân tộc
        /// ETHNIC_NAME
        /// </summary>
        public string DAN_TOC { get; set; }

        /// <summary>
        /// Dân tộc
        /// ETHNIC_CODE
        /// </summary>
        public string DAN_TOC_CODE { get; set; }
        /// <summary>
        /// Giới tính
        /// 1: Nam - 2: Nữ - 3: Không xác định
        /// GENDER_ID
        /// </summary>
        public string GIOI_TINH { get; set; }

        /// <summary>
        /// Nghề nghiệp
        /// CAREER_NAME
        /// </summary>
        public string NGHE_NGHIEP { get; set; }

        /// <summary>
        /// Ngày vào viện
        /// Định dạng: dd/MM/yyyy HH:mm
        /// IN_TIME
        /// </summary>
        public string NGAY_VAO { get; set; }

        /// <summary>
        /// Ngày ra viện
        /// Định dạng: dd/MM/yyyy HH:mm
        /// OUT_TIME
        /// </summary>
        public string NGAY_RA { get; set; }

        /// <summary>
        /// Trường hợp đình chỉ thai nghén, điền số tuần tuổi thai (từ 1 đến 42)
        /// </summary>
        public string TUOI_THAI { get; set; }

        /// <summary>
        /// Chẩn đoán bệnh
        /// ICD_NAME
        /// </summary>
        public string CHAN_DOAN { get; set; }

        /// <summary>
        /// Phương pháp điều trị
        /// TREATMENT_METHOD
        /// </summary>
        public string PP_DIEUTRI { get; set; }

        /// <summary>
        /// Ghi chú 
        /// </summary>
        public string GHI_CHU { get; set; }

        /// <summary>
        /// Người đại diện - Thủ trưởng đơn vị
        /// RELATIVE_NAME
        /// </summary>
        public string NGUOI_DAI_DIEN { get; set; }

        /// <summary>
        /// Chứng chỉ hành nghề
        /// Bác sĩ kết thúc điều trị
        /// DOCTOR_LOGINNAME --> DIPLOMA
        /// </summary>
        public string MA_TRUONGKHOA { get; set; }

        /// <summary>
        /// Tên bác sĩ kết thúc điều trị
        /// DOCTOR_USERNAME
        /// </summary>
        public string TEN_TRUONGKHOA { get; set; }

        /// <summary>
        /// Họ và tên của cha hoặc người nuôi dưỡng
        /// FATHER_NAME | RELATIVE_NAME
        /// Giấy chứng sinh: HIS_BABY --> FATHER_NAME 
        /// </summary>
        public string HO_TEN_CHA { get; set; }

        /// <summary>
        /// Họ và tên của cha hoặc người nuôi dưỡng
        /// MOTHER_NAME | RELATIVE_NAME
        /// Giấy chứng sinh: VIR_PATIENT_NAME
        /// </summary>
        public string HO_TEN_ME { get; set; }

        /// <summary>
        /// "Trường hợp Trẻ em không thẻ (trẻ dưới 72 tháng tuổi) điền số: 1.
        /// Để trống MA_SOBHXH, MA_THE. 
        /// Nhập HO_TEN_CHA hoặc HO_TEN_ME"
        /// </summary>
        public string TEKT { get; set; }

        // Giấy chứng sinh

        /// <summary>
        /// 10 số cuối số thẻ BHYT
        /// TDL_HEIN_CARD_NUMBER
        /// </summary>
        public string MA_SOBHXH_ME { get; set; }

        ///// <summary>
        ///// Họ và tên của mẹ hoặc người nuôi dưỡng trên giấy chứng sinh
        ///// VIR_PATIENT_NAME
        ///// </summary>
        //public string HO_TEN_ME_CS { get; set; }

        /// <summary>
        /// Số CMND của mẹ hoặc người nuôi dưỡng
        /// CMND_NUMBER | RELATIVE_CMND_NUMBER
        /// </summary>
        public string CMND { get; set; }

        /// <summary>
        /// "Ngày cấp CMND  của mẹ hoặc người nuôi dưỡng
        /// Định dạng: dd/MM/yyyy"
        /// CMND_DATE
        /// </summary>
        public string NGAY_CAP_CMND { get; set; }

        /// <summary>
        /// Nơi cấp CMND của mẹ hoặc người nuôi dưỡng
        /// CMND_PLACE
        /// </summary>
        public string NOI_CAP_CMND { get; set; }

        /// <summary>
        /// Ngày sinh con
        /// Định dạng dd/MM/yyyy HH:mm
        /// BORN_TIME
        /// </summary>
        public string NGAY_SINHCON { get; set; }

        /// <summary>
        /// Nơi sinh con (tại bệnh viện bé sinh ra)
        /// Theo 3 cấp (xã/huyện/tỉnh)
        /// BRANCH_NAME
        /// </summary>
        public string NOI_SINH_CON { get; set; }

        /// <summary>
        /// Dự định đặt tên con (Có thể thay đổi)
        /// BABY_NAME
        /// </summary>
        public string TEN_CON { get; set; }

        /// <summary>
        /// Số con trong lần đẻ này
        /// Count HIS_BABY
        /// </summary>
        public long SO_CON { get; set; }

        /// <summary>
        /// Giới tính con
        /// HIS_BABY --> GENDER_NAME
        /// </summary>
        public string GIOI_TINH_CON { get; set; }

        /// <summary>
        /// Cân nặng con (tính theo đơn vị Gram)
        /// WEIGHT
        /// </summary>
        public decimal CAN_NANG_CON { get; set; }

        /// <summary>
        /// Tình trạng con
        /// BORN_RESULT_ID
        /// </summary>
        public string TINH_TRANG_CON { get; set; }

        /// <summary>
        /// Người đỡ đẻ
        /// MIDWIFE
        /// </summary>
        public string NGUOI_DO_DE { get; set; }

        /// <summary>
        /// Người ghi phiếu
        /// HIS_BABY --> CREATOR
        /// </summary>
        public string NGUOI_GHI_PHIEU { get; set; }

        /// <summary>
        /// Trường hợp sinh con phải phẫu thuật, ghi số: Không: 0, Có: 1
        /// BORN_TYPE_CODE = MO
        /// </summary>
        public long SINHCON_PHAUTHUAT { get; set; }

        /// <summary>
        /// Trường hợp sinh con dưới 32 tuần, ghi số: Không: 0, Có: 1
        /// WEEK_COUNT
        /// </summary>
        public long SINHCON_DUOI32TUAN { get; set; }

        /// <summary>
        /// Số (nếu cấp lại, theo 56/2017/TT-BYT)
        /// Lưu ý: Nếu thêm "Số" và "Quyển số" thì phải thêm đồng thời
        /// </summary>
        public string SO { get; set; }

        /// <summary>
        /// Quyển số  (nếu cấp lại, theo 56/2017/TT-BYT)
        /// Lưu ý: Nếu thêm "Số" và "Quyển số" thì phải thêm đồng thời
        /// </summary>
        public string QUYEN_SO { get; set; }

        //Giấy nghỉ ốm

        /// <summary>
        /// Mã cơ sở đăng nhập
        /// HEIN_MEDI_ORG_CODE trong HIS_BRANCH tương ứng với hồ sơ
        /// </summary>
        public string MA_BV { get; set; }

        /// <summary>
        /// Chứng chỉ hành nghề Bác sĩ kết thúc điều trị
        /// DOCTOR_LOGINNAME --> DIPLOMA
        /// </summary>
        public string MA_BS { get; set; }

        /// <summary>
        /// Mã đơn vị công tác
        /// </summary>
        public string MA_DVI { get; set; }

        /// <summary>
        /// Tên đơn vị công tác
        /// WORK_PLACE
        /// </summary>
        public string TEN_DVI { get; set; }

        /// <summary>
        /// Nghỉ việc từ ngày
        /// Định dạng: dd/MM/yyyy
        /// SICK_LEAVE_FROM
        /// </summary>
        public string TU_NGAY { get; set; }

        /// <summary>
        /// Nghỉ việc đến ngày
        /// Định dạng: dd/MM/yyyy
        /// SICK_LEAVE_TO
        /// </summary>
        public string DEN_NGAY { get; set; }

        /// <summary>
        /// Số ngày nghỉ
        /// SICK_LEAVE_DAY
        /// </summary>
        public long SO_NGAY { get; set; }

        /// <summary>
        /// Số khám chữa bệnh
        /// TREATMENT_CODE
        /// </summary>
        public string SO_KCB { get; set; }

        /// <summary>
        /// Tên bác sĩ kết thúc điều trị
        /// DOCTOR_USERNAME
        /// </summary>
        public string TEN_BSY { get; set; }

        /// <summary>
        /// Mẫu số của chứng từ
        /// </summary>
        public string MAU_SO { get; set; }

        public string TEN_DVI_NO { get; set; }
        public string TEN_DVI_NO_NAME { get; set; }
        public string LOAI_QUAN_HE { get;  set; }
        public string NGAY_GIO_SINH_CON { set; get; }

        public string SO_CT { get; set; }
        public string ICD_FULL { get; set; }
        public string LOI_DAN_BS { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string SICK_HEIN_CARD_NUMBER { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_SOCIAL_INSURANCE_NUMBER { get; set; }
        public string END_DEPARTMENT_HEAD_LOGINNAME { get; set; }
        public string END_DEPARTMENT_HEAD_USERNAME { get; set; }
        public string END_DEPARTMENT_HEAD_DIPLOMA { get; set; }
    }

    class GIOI_TINH
    {
        internal static string NAM { get { return "1"; } }
        internal static string NU { get { return "2"; } }
        internal static string KHONG_XAC_DINH { get { return "3"; } }
    }
}
