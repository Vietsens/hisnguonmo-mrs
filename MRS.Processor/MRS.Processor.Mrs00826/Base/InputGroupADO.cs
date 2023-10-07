using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.Base
{
    public class InputGroupADO
    {
        public HIS_BRANCH Branch { get; set; }

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
        ///1: Hiển thị số ngày điều trị giống bảng kê 6556
        ///khác 1: như cũ
        /// </summary>
        public string IsTreatmentDayCount6556 { get; set; }

        /// <summary>
        /// Truyền vào danh sách hồ sơ điều trị
        /// </summary>
        public List<V_HIS_TREATMENT_3> Treatments { get; set; }

        /// <summary>
        /// lấy thông tin bệnh nhân chuyển viện(chuyển đi, chuyển đến)
        /// </summary>
        public List<V_HIS_HEIN_APPROVAL> HeinApprovals { get; set; }

        /// <summary>
        /// các dịch vụ bệnh nhân đã sử dụng
        /// </summary>
        public List<V_HIS_SERE_SERV_2> ListSereServ { get; set; }

        public List<V_HIS_SERE_SERV_TEIN> SereServTeins { get; set; }
        public List<HIS_TRACKING> Trackings { get; set; }
        public List<V_HIS_SERE_SERV_PTTT> SereServPttts { get; set; }
        public List<HIS_DHST> Dhsts { get; set; }

        /// <summary>
        /// Lấy thông tin giường
        /// </summary>
        public List<V_HIS_BED_LOG> BedLogs { get; set; }

        /// <summary>
        /// lấy thông tin mã bác sĩ
        /// </summary>
        public List<HIS_EKIP_USER> EkipUsers { get; set; }

        public List<HIS_MEDI_ORG> ListHeinMediOrg { get; set; }

        public List<HIS_CONFIG> ConfigData { get; set; }

        public List<V_HIS_SERVICE> TotalSericeData { get; set; }

        public List<HIS_ICD> TotalIcdData { get; set; }

        public List<HIS_DEBATE> ListDebate { get; set; }

        public List<HIS_DHST> ListDhsts { get; set; }
    }
}
