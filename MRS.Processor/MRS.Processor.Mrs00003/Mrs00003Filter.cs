using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00003
{
    class Mrs00003Filter
    {
        /// <summary>
        /// Ky bat dau tinh, ngay dau tien cua bao cao se la ngay dau tien cua ky.
        /// Trong truong hop null, se duoc hieu la ky hien tai (chua chot).
        /// </summary>
        public long? MEDI_STOCK_PERIOD_ID { get;  set;  }

        /// <summary>
        /// Su dung trong truong hop lay du lieu theo ky hien tai (Khong the su dung MEDI_STOCK_PERIOD_ID)
        /// </summary>
        public long? MEDI_STOCK_ID { get;  set;  }

        public long MEDICINE_TYPE_ID { get;  set;  } //bat buoc co du lieu, neu khong set thi se = 0 ==> ko tim duoc chi tiet nao

        public Mrs00003Filter()
            : base()
        {
        }
    }
}
