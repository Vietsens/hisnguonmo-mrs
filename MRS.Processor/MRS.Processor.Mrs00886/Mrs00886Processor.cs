using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00886
{
    class Mrs00886Processor : AbstractProcessor
    {
        List<Mrs00886RDO> listDataExam = new List<Mrs00886RDO>();
        List<Mrs00886RDO> listDataOther = new List<Mrs00886RDO>();
        Mrs00886Filter filter = null;
        public Mrs00886Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00886Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00886Filter)this.reportFilter;
            bool result = true;
            try
            {
                listDataOther = new ManagerSql().GetListOther(filter);
                listDataExam = new ManagerSql().GetListExam(filter);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (listDataOther != null)
                {
                    foreach (var item in listDataOther)
                    {
                        item.MONTH_STR = item.MONTH.ToString().Substring(4, 2);
                    }

                }

                if (listDataExam != null)
                {
                    foreach (var item in listDataExam)
                    {
                        item.MONTH_STR = item.MONTH.ToString().Substring(4, 2);
                        item.AGE = Inventec.Common.DateTime.Calculation.Age(item.TDL_PATIENT_DOB??0);
                    }

                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listDataOther);
            objectTag.AddObjectData(store, "ReportExam", listDataExam); 
        }
    }
}
