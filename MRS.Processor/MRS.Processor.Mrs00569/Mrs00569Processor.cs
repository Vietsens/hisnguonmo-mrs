using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
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
using TYT.MANAGER.Core.TytMalaria.Get;
using TYT.MANAGER.Manager;

namespace MRS.Processor.Mrs00569
{
    public class Mrs00569Processor : AbstractProcessor
    {
        Mrs00569Filter castFilter = null;
        CommonParam paramGet = new CommonParam();
        private string a = "";
        List<Mrs00569RDO> ListRdo = new List<Mrs00569RDO>();
        List<TYT_MALARIA> tytMalaria = new List<TYT_MALARIA>();
        public Mrs00569Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00569Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00569Filter)this.reportFilter;
               
                //get dữ liệu:
                CommonParam paramGet = new CommonParam();
                TytMalariaFilterQuery TytMalariafilter = new TytMalariaFilterQuery();
                TytMalariafilter = this.MapFilter<Mrs00569Filter, TytMalariaFilterQuery>(castFilter, TytMalariafilter);

                tytMalaria = new TytMalariaManager(paramGet).Get<List<TYT_MALARIA>>(TytMalariafilter);

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
                ListRdo = (from r in tytMalaria select new Mrs00569RDO(r)).ToList();
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00569Filter)this.reportFilter).CREATE_TIME_FROM?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00569Filter)this.reportFilter).CREATE_TIME_TO  ?? 0));
           
            objectTag.AddObjectData(store,"Report",ListRdo);
           
        }
       
    }
}