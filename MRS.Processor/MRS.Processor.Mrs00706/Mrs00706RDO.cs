using MOS.MANAGER.HisService;
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
using MOS.MANAGER.HisSereServ; 

namespace MRS.Processor.Mrs00706
{
    public class Mrs00706RDO : V_HIS_MEDICINE_TYPE
    {
        public Dictionary<string,decimal> DIC_PRICE { get;  set;  }
        public string SUPPLIER_NAMEs { get;  set;  }
    }

    public class MedicineTypePrice
    {
        public long MEDICINE_TYPE_ID { get; set; }
        public decimal PRICE { get; set; }
    }

    public class MedicineTypeSupplier
    {
        public long MEDICINE_TYPE_ID { get; set; }
        public string SUPPLIER_NAME { get; set; }
    }

    public class MediStockMety
    {
        public long MEDICINE_TYPE_ID { get; set; }
        public long MEDI_STOCK_ID { get; set; }
    }
}
