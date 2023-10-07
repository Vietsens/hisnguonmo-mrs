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

namespace MRS.Processor.Mrs00707
{
    public class Mrs00707RDO
    {
        public string TRANSACTION_CODE { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public long TRANSACTION_DATE { get; set; }
        public short? IS_CANCEL { get; set; }
        public short? IS_PRINTED { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public decimal TDL_TOTAL_PRICE { get; set; }
        public long? PRES_NUMBER { get; set; }
        public decimal AMOUNT { get; set; }
    }
    
    public class PrintLogUnique
    {
        public string UNIQUE_CODE { get; set; }
        public string TRANSACTION_CODE { get; set; }
    }
}
