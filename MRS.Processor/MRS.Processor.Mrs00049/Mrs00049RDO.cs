using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00049
{
    class Mrs00049RDO : HIS_SERE_SERV
    {
        public string INTRUCTION_DATE_STR { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public Mrs00049RDO(HIS_SERE_SERV sereServ)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HIS_SERE_SERV>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(sereServ)));
                }
                SetExtendField(sereServ);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(HIS_SERE_SERV data)
        {
            try
            {
                INTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.TDL_INTRUCTION_TIME);
                this.SERVICE_CODE = data.TDL_SERVICE_CODE;
                this.SERVICE_NAME = data.TDL_SERVICE_NAME;
                this.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID==data.TDL_SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
