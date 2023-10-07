using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00155
{
    public class Mrs00155Filter
    {
        public long DATE_FROM { get;  set;  }

        public long DATE_TO { get; set; }

        public long? IMP_MEST_TYPE_ID { get; set; }

        public List<long> IMP_MEST_TYPE_IDs { get; set; }

        public long? MEDI_STOCK_ID { get; set; }

        public string KEY_GROUP_IMP { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        //public bool? IS_CHEMICAL_SUBSTANCE { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }

        public List<long> INPUT_DATA_ID_IMT_TYPEs { get; set; } // loại nhập: 1. Nhập chuyển kho; 2. Nhập nhà cung cấp; 3. nhập thu hồi; 4. Nhập tổng hợp trả; 5. Nhập kiểm kê; 6. Nhập đầu kỳ; 7. Nhập khác; 8. Nhập bù cơ số; 9. Nhập bù lẻ; 10. Nhập đơn nội trú trả lại; 11. Nhập đơn tủ trực trả lại; 12. Nhập đơn máu trả lại; 13. Nhập hao phí trả lại; 14. Nhập bào chế thuốc; 15. Nhập bán trả lại; 16. Nhập đơn khám trả lại; 17. Nhập nhập tái sử dụng; 18. Nhập hiến máu; 19. Nhập bổ sung cơ số
        /* public const long ID__BCS = 8;
        public const long ID__BCT = 14;
        public const long ID__BL = 9;
        public const long ID__BTL = 15;
        public const long ID__CK = 1;
        public const long ID__DK = 6;
        public const long ID__DMTL = 12;
        public const long ID__DNTTL = 10;
        public const long ID__DONKTL = 16;
        public const long ID__DTTTL = 11;
        public const long ID__HM = 18;
        public const long ID__HPTL = 13;
        public const long ID__KHAC = 7;
        public const long ID__KK = 5;
        public const long ID__NCC = 2;
        public const long ID__TH = 3;
        public const long ID__THT = 4;
        public const long ID__TSD = 17;*/
     
    }
}
