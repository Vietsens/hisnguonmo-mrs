using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00104
{
    class Mrs00104RDO
    {
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public decimal VIR_PRICE { get; set; }

        public Mrs00104RDO() { }

        public Mrs00104RDO(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                SERVICE_ID = ListSereServ.First().SERVICE_ID;
                SERVICE_CODE = ListSereServ.First().TDL_SERVICE_CODE;
                SERVICE_NAME = ListSereServ.First().TDL_SERVICE_NAME;
                SERVICE_UNIT_NAME = ListSereServ.First().SERVICE_UNIT_NAME;
                VIR_PRICE = ListSereServ.First().VIR_PRICE ?? 0;
                TOTAL_AMOUNT = ListSereServ.Sum(s => s.AMOUNT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
