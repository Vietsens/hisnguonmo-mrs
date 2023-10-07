using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytUninfect.Get;
using TYT.MANAGER.Manager;

namespace MRS.Processor.Mrs00567
{
    public class Mrs00567Processor : AbstractProcessor
    {
        Mrs00567Filter castFilter = null;
        CommonParam paramGet = new CommonParam();
        private string a = "";
        List<Mrs00567RDO> ListRdo = new List<Mrs00567RDO>();
        List<TYT_UNINFECT> tytUninfect = new List<TYT_UNINFECT>();
        System.Data.DataTable listData = new System.Data.DataTable();
        public Mrs00567Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00567Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00567Filter)this.reportFilter;

                listData = new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }
                //get dữ liệu:
                CommonParam paramGet = new CommonParam();
                TytUninfectFilterQuery TytUninfectfilter = new TytUninfectFilterQuery();
                TytUninfectfilter = this.MapFilter<Mrs00567Filter, TytUninfectFilterQuery>(castFilter, TytUninfectfilter);

                tytUninfect = new TytUninfectManager(paramGet).Get<List<TYT_UNINFECT>>(TytUninfectfilter);

                if (!string.IsNullOrWhiteSpace(castFilter.ICD_CODE))
                {
                    tytUninfect = tytUninfect.Where(o => o.ICD_CODE == castFilter.ICD_CODE).ToList();
                }


                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }
        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }
                ListRdo = (from r in tytUninfect select new Mrs00567RDO(r)).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00567Filter)this.reportFilter).CREATE_TIME_FROM ?? ((Mrs00567Filter)this.reportFilter).DIAGNOSIS_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00567Filter)this.reportFilter).CREATE_TIME_TO ?? ((Mrs00567Filter)this.reportFilter).DIAGNOSIS_TIME_TO ?? 0));
                if (!string.IsNullOrWhiteSpace(castFilter.ICD_CODE) && IsNotNullOrEmpty(ListRdo))
                {
                    dicSingleTag.Add("ICD_NAME", ListRdo.First().ICD_NAME);
                }

                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    objectTag.AddObjectData(store, "Report", listData);
                    return;
                }
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}