using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00887
{
    class Mrs00887Processor : AbstractProcessor
    {
        List<Mrs00887RDO> listRdo = new List<Mrs00887RDO>();
        Mrs00887Filter filter = null;
        public Mrs00887Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00887Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00887Filter)this.reportFilter;
            bool result = true;
            try
            {
                listRdo = new ManagerSql().GetRdo(filter);
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
                if (listRdo != null)
                {
                    foreach (Mrs00887RDO item in listRdo)
                    {
                        item.MONTH_STR = item.MONTH.ToString().Substring(4, 2);
                        //if (item.ICD_GROUP_ID.HasValue && item.ICD_GROUP_ID == 15 && !string.IsNullOrWhiteSpace(item.ICD_NAME))
                        //{
                        //    if (item.ICD_NAME.Contains("Băng huyết"))
                        //    {
                        //        item.ICD_TYPE = "BH";
                        //    }
                        //    else if (item.ICD_NAME.Contains("Sản giật"))
                        //    {
                        //        item.ICD_TYPE = "SG";
                        //    }
                        //    else if (item.ICD_NAME.Contains("Uốn ván sơ sinh"))
                        //    {
                        //        item.ICD_TYPE = "UVSS";
                        //    }
                        //    else if (item.ICD_NAME.Contains("Vỡ tử cung"))
                        //    {
                        //        item.ICD_TYPE = "VTC";
                        //    }
                        //    else if (item.ICD_NAME.Contains("Nhiễm trùng sau"))
                        //    {
                        //        item.ICD_TYPE = "NTS";
                        //    }
                        //    else if (item.ICD_NAME.Contains("Phá thai"))
                        //    {
                        //        item.ICD_TYPE = "PT";
                        //    }
                        //    else
                        //    {
                        //        item.ICD_TYPE = "KHAC";
                        //    }
                        //}
                        if (item.TREATMENT_END_TYPE_ID.HasValue)
                        {
                            item.END_TYPE_NAME = item.TREATMENT_END_TYPE_NAME;
                            item.END_TYPE_CODE = item.TREATMENT_END_TYPE_CODE;
                        }
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
            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
