using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00004
{
    /// <summary>
    /// Filter báo cáo thẻ kho vật tư
    /// </summary>
    class Mrs00004Filter
    {
        /// <summary>
        /// Ky bat dau tinh, ngay dau tien cua bao cao se la ngay dau tien cua ky.
        /// Trong truong hop null, se duoc hieu la ky hien tai (chua chot).
        /// </summary>
        public long? MEDI_STOCK_PERIOD_ID { get;  set;  }

        /// <summary>
        /// Su dung trong truong hop lay du lieu theo ky hien tai. Khong the su dung MEDI_STOCK_PERIOD_ID
        /// </summary>
        public long? MEDI_STOCK_ID { get;  set;  }

        /// <summary>
        /// true - tinh den thoi diem moi nhat hien tai (thoi diem lay report).
        /// false/null - tinh den het ky. Trong truong hop ky hien tai thi false cung co y nghia nhu true.
        /// </summary>
        public bool? IS_TO_NOW { get;  set;  }

        public long MATERIAL_TYPE_ID { get;  set;  } //bat buoc co du lieu, neu khong set thi se = 0 ==> ko tim duoc chi tiet nao

        public Mrs00004Filter()
            : base()
        {
        }
    }
}
