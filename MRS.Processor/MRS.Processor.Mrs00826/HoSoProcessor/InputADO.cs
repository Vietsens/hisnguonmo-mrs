using ACS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.HoSoProcessor
{
    public class InputADO
    {
        /// <summary>
        /// Mỗi lần truyền vào 1 hồ sơ điều trị
        /// </summary>
        public V_HIS_TREATMENT_3 Treatment { get; set; }

        /// <summary>
        /// Thông tin 1 thẻ BHYT của 1 bệnh nhân
        /// </summary>
        public V_HIS_HEIN_APPROVAL HeinApproval { get; set; }

        /// <summary>
        /// lấy thông tin bệnh nhân chuyển viện(chuyển đi, chuyển đến)
        /// </summary>
        public List<V_HIS_HEIN_APPROVAL> HeinApprovals { get; set; }

        //public V_HIS_ACCIDENT_HURT AccidentHurt { get; set; }//thông tin tai nạn thương tích

        /// <summary>
        /// các dịch vụ bệnh nhân đã sử dụng
        /// </summary>
        public List<V_HIS_SERE_SERV_2> ListSereServ { get; set; }

        //chủ yếu lấy thông tin cân nặng ở XML1
        public HIS_DHST Dhst { get; set; }
        public HIS_BRANCH Branch { get; set; }
        public List<V_HIS_SERE_SERV_TEIN> SereServTeins { get; set; }
        public List<HIS_TRACKING> Trackings { get; set; }
        public List<V_HIS_SERE_SERV_PTTT> SereServPttts { get; set; }

        /// <summary>
        /// Lấy thông tin giường
        /// </summary>
        public List<V_HIS_BED_LOG> BedLogs { get; set; }

        /// <summary>
        /// lấy thông tin mã bác sĩ
        /// </summary>
        public List<HIS_EKIP_USER> EkipUsers { get; set; }

        /// <summary>
        /// kiểm tra vật tư là stent
        /// </summary>
        public List<HIS_MATERIAL_TYPE> MaterialTypes { get; set; }

        /// <summary>
        /// gói vật tư y tế được kê khác khoa
        /// </summary>
        public string MaterialPackageOption { get; set; }

        /// <summary>
        /// Đơn giá của vật tư là giá gốc hay giá bán
        /// </summary>
        public string MaterialPriceOriginalOption { get; set; }

        public string MaterialStentRatio { get; set; }

        /// <summary>
        /// TenBenhOption
        /// 1: Chỉ lấy treatment
        /// 2: Lấy cả service_req ghép với treatment
        /// </summary>
        public string TenBenhOption { get; set; }

        /// <summary>
        /// danh sách các mã thuốc không kiểm tra liều dùng phân cách bởi dấu |
        /// sử dụng cho dịch vụ Oxy
        /// </summary>
        public string HeinServiceTypeCodeNoTutorial { get; set; }

        /// <summary>
        /// Cấu hình thông tin hiển thị mã bác sĩ của dịch vụ khám thêm trong xml3
        /// 1: BS chỉ định;BS thực hiện
        /// </summary>
        public string MaBacSiOption { get; set; }

        /// <summary>
        /// Danh sách các xml cho phép xuất phân cách nhau bởi dấu phẩy
        /// vd: 1,2,3,5 --> thì xuất ra xml1, xml2, xml3, xml5
        /// </summary>
        public string XMLNumbers { get; set; }

        /// <summary>
        ///1: Nếu stent thứ 2 có (HEIN_LIMIT_PRICE >= PRIMARY_PRICE/2) T_TRANTT để trống.
        ///khác 1: T_TRANTT = HEIN_LIMIT_PRICE
        /// </summary>
        public string MaterialStent2Limit { get; set; }

        /// <summary>
        /// Ho So Dieu Tri Phuc Vu Xuat XML Ho So Chung Tu
        /// </summary>
        public V_HIS_TREATMENT_10 Treatment2076 { get; set; }

        /// <summary>
        /// Danh sac tre so sinh phuc vu xuat Xml ho so chung tu
        /// </summary>
        public List<V_HIS_BABY> Babys { get; set; }

        /// <summary>
        /// Danh sach nhan vien
        /// </summary>
        public List<HIS_EMPLOYEE> Employees { get; set; }

        public List<ACS_USER> AcsUsers { get; set; }

        /// <summary>
        ///1: Hiển thị số ngày điều trị giống bảng kê 6556
        ///khác 1: như cũ
        /// </summary>
        public string IsTreatmentDayCount6556 { get; set; }

        public string XML2076__DOC_CODE { get; set; }

        /// <summary>
        /// Diện điều trị cuối cùng
        /// </summary>
        public V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter { get; set; }

        /// <summary>
        /// y lệnh khám có thời gian y lệnh nhỏ nhất.
        /// để lấy số thứ tự khám.
        /// </summary>
        public HIS_SERVICE_REQ ExamServiceReq { get; set; }

        public List<HIS_MEDI_ORG> ListHeinMediOrg { get; set; }

        public List<HIS_CONFIG> ConfigData { get; set; }

        public List<V_HIS_SERVICE> TotalSericeData { get; set; }

        public List<HIS_ICD> TotalIcdData { get; set; }

        public List<HIS_DEBATE> ListDebate { get; set; }

        public List<HIS_DHST> ListDhsts { get; set; }
    }
}
