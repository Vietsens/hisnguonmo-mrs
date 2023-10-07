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
using TYT.MANAGER.Core.TytGdsk.Get;
using TYT.MANAGER.Manager;

namespace MRS.Processor.Mrs00570
{
    public class Mrs00570Processor : AbstractProcessor
    {
        Mrs00570Filter castFilter = null;
        CommonParam paramGet = new CommonParam();
        private string a = "";
        List<Mrs00570RDO> ListRdo = new List<Mrs00570RDO>();
        List<TYT_GDSK> tytGdsk = new List<TYT_GDSK>();
        public Mrs00570Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00570Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00570Filter)this.reportFilter;
               
                //get dữ liệu:
                CommonParam paramGet = new CommonParam();
                TytGdskFilterQuery TytGdskfilter = new TytGdskFilterQuery();
                TytGdskfilter = this.MapFilter<Mrs00570Filter, TytGdskFilterQuery>(castFilter, TytGdskfilter);

                tytGdsk = new TytGdskManager(paramGet).Get<List<TYT_GDSK>>(TytGdskfilter);

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
                ListRdo = (from r in tytGdsk select new Mrs00570RDO(r)).ToList();
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00570Filter)this.reportFilter).GDSK_TIME_FROM ?? ((Mrs00570Filter)this.reportFilter).CREATE_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00570Filter)this.reportFilter).GDSK_TIME_TO ?? ((Mrs00570Filter)this.reportFilter).CREATE_TIME_TO ?? 0));
           
            objectTag.AddObjectData(store,"Report",ListRdo);
           
        }
       
    }
}