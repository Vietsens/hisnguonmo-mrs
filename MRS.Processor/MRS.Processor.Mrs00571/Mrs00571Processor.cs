using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00571
{
    class Mrs00571Processor : MRS.MANAGER.Core.MrsReport.AbstractProcessor
    {
        Mrs00571Filter castFilter = null;
        List<Mrs00571RDO> ListRdo = new List<Mrs00571RDO>();
        List<Mrs00571RDO> ListgetRdo = new List<Mrs00571RDO>();

      
     //   CommonParam paramGet = new CommonParam();

        public Mrs00571Processor(Inventec.Core.CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00571Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00571Filter)this.reportFilter;
             //   CommonParam paramGet = new CommonParam();
                ListgetRdo = new ManagerSql().GetRdo(castFilter);
             //   result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
                
                
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
            //    result = true;
            //    ListRdo =   ListRdo;
            }
            catch (Exception ex)
            {
              //  ListRdo.Clear();
                
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

                if (ListgetRdo != null)
                {
                  //  ListRdo = new List<Mrs00571RDO>();
                    objectTag.AddObjectData(store, "Report", ListgetRdo);
                }

                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
