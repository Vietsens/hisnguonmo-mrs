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

namespace MRS.Processor.Mrs00709
{
    public class Mrs00709RDO
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string TDL_BID_NUMBER { get; set; }
        public string TDL_BID_YEAR { get; set; }
        public long? DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public long IMP_DATE { get; set; }
        public long MEMA_ID { get; set; }
        public long TYPE { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? IMP_AMOUNT { get; set; }
        public long? EXP_DATE { get; set; }
        public decimal? EXP_AMOUNT { get; set; }
        public decimal? END_AMOUNT { get; set; }
        public string JSON_EXP_DEPARTMENT { get; set; }

    }
}
