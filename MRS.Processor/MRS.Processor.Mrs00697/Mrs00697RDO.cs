using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00697
{
    class Mrs00697RDO
    {
        public long SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public string TDL_SERVICE_TYPE_NAME { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string TDL_HEIN_ORDER { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_CODE { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_NAME { get; set; }
        public long TDL_SERVICE_UNIT_ID { get; set; }
        public string TDL_SERVICE_UNIT_NAME { get; set; }

        public decimal NT_AMOUNT { get; set; }
        public decimal NGT_AMOUNT { get; set; }

        public decimal NEW_PRICE { get; set; }
        public decimal OLD_PRICE { get; set; }

        public Mrs00697RDO(MOS.EFMODEL.DataModels.HIS_SERE_SERV sereServ)
        {
            try
            {
                if (sereServ != null)
                {
                    this.SERVICE_ID = sereServ.SERVICE_ID;
                    this.TDL_SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
                    this.TDL_SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                    this.TDL_HEIN_ORDER = sereServ.TDL_HEIN_ORDER;
                    this.TDL_HEIN_SERVICE_BHYT_CODE = sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
                    this.TDL_HEIN_SERVICE_BHYT_NAME = sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
                    this.TDL_SERVICE_UNIT_ID = sereServ.TDL_SERVICE_UNIT_ID;
                    var unit = MANAGER.Config.HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_UNIT_ID);
                    this.TDL_SERVICE_UNIT_NAME = unit != null ? unit.SERVICE_UNIT_NAME : "";
                    this.TDL_SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID;
                    var serviceType = MANAGER.Config.HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID);
                    this.TDL_SERVICE_TYPE_NAME = serviceType != null ? serviceType.SERVICE_TYPE_NAME : "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00697RDO(List<Mrs00697RDO> s)
        {
            if (s != null && s.Count > 0)
            {
                this.NEW_PRICE = s.First().NEW_PRICE;
                this.NGT_AMOUNT = s.Sum(o => o.NGT_AMOUNT);
                this.NT_AMOUNT = s.Sum(o => o.NT_AMOUNT);
                this.OLD_PRICE = s.First().OLD_PRICE;
                this.SERVICE_ID = s.First().SERVICE_ID;
                this.TDL_HEIN_ORDER = s.First().TDL_HEIN_ORDER;
                this.TDL_HEIN_SERVICE_BHYT_CODE = s.First().TDL_HEIN_SERVICE_BHYT_CODE;
                this.TDL_HEIN_SERVICE_BHYT_NAME = s.First().TDL_HEIN_SERVICE_BHYT_NAME;
                this.TDL_SERVICE_CODE = s.First().TDL_SERVICE_CODE;
                this.TDL_SERVICE_NAME = s.First().TDL_SERVICE_NAME;
                this.TDL_SERVICE_UNIT_ID = s.First().TDL_SERVICE_UNIT_ID;
                this.TDL_SERVICE_UNIT_NAME = s.First().TDL_SERVICE_UNIT_NAME;
                this.TDL_SERVICE_TYPE_ID = s.First().TDL_SERVICE_TYPE_ID;
                this.TDL_SERVICE_TYPE_NAME = s.First().TDL_SERVICE_TYPE_NAME;
            }
        }
    }
}
