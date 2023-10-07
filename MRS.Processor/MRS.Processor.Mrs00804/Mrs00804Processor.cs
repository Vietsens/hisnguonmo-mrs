using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisEmployee;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00804
{
    public class Mrs00804Processor: AbstractProcessor
    {
        public Mrs00804Filter filter;
        public List<Mrs00804RDO> ListRdo = new List<Mrs00804RDO>();
        public List<HIS_EMPLOYEE> ListEmployee = new List<HIS_EMPLOYEE>();
        CommonParam commonParam = new CommonParam();
        public Mrs00804Processor(CommonParam param, string reportTypeCode) :base(param,reportTypeCode)
        {

        }
        public override Type FilterType()
        {
          return typeof(Mrs00804Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                HisEmployeeFilterQuery  employeeFilter = new HisEmployeeFilterQuery();
                employeeFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                employeeFilter.CREATE_TIME_TO = filter.TIME_TO;
               ListEmployee = new HisEmployeeManager(commonParam).Get(employeeFilter);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(ListEmployee))
                {
                    foreach (var item in ListEmployee)
                    {
                        Mrs00804RDO rdo = new Mrs00804RDO();
                        rdo.DIPLOMA = item.DIPLOMA;
                        rdo.DOB = item.DOB;
                        rdo.TITLE = item.TITLE;
                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
