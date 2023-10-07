using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00068
{
    /// <summary>
    /// Báo cáo tình hình thanh toán viện phí của bệnh nhân: MRS_BaoCaoTinhHinhThanhToanVienPhiCuaBenhNhan_A4N_001
    /// </summary>
    class Mrs00068Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { get;  set;  }

        /// <summary>
        ///  0. OWED còn thiếu;  1. RESIDUAL đủ hoặc thừa
        /// </summary>
        public short? OWED_OR_RESIDUAL { get;  set;  }

        public Mrs00068Filter()
            : base()
        {
        }
    }
}
