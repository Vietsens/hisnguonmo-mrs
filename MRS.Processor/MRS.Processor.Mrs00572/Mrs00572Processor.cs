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
using TYT.MANAGER.Core.TytHiv.Get;
using TYT.MANAGER.Manager;

namespace MRS.Processor.Mrs00572
{
    public class Mrs00572Processor : AbstractProcessor
    {
        Mrs00572Filter castFilter = null;
        CommonParam paramGet = new CommonParam();
        private string a = "";
        List<Mrs00572RDO> ListRdo = new List<Mrs00572RDO>();
        List<TYT_HIV> tytHiv = new List<TYT_HIV>();
        public Mrs00572Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00572Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00572Filter)this.reportFilter;
               
                //get dữ liệu:
                CommonParam paramGet = new CommonParam();
                TytHivFilterQuery TytHivfilter = new TytHivFilterQuery();
                TytHivfilter = this.MapFilter<Mrs00572Filter, TytHivFilterQuery>(castFilter, TytHivfilter);

                tytHiv = new TytHivManager(paramGet).Get<List<TYT_HIV>>(TytHivfilter);

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
                ListRdo = (from r in tytHiv select new Mrs00572RDO(r)).ToList();
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00572Filter)this.reportFilter).CREATE_TIME_FROM?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00572Filter)this.reportFilter).CREATE_TIME_TO  ?? 0));
           
            objectTag.AddObjectData(store,"Report",ListRdo);
           
        }
       
    }
}