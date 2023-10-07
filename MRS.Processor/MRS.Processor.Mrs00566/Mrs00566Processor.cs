using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00566
{
    public class Mrs00566Processor : AbstractProcessor
    {
        Mrs00566Filter castFilter = null;
        CommonParam paramGet = new CommonParam();
        private string a = "";
        Decimal TOTAL_PRICE_BHYT = 0;
        Decimal TOTAL_PRICE_VP = 0;
        long YEAR = 0;
        public Mrs00566Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00566Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00566Filter)this.reportFilter;
                YEAR = (long)castFilter.TRANSACTION_TIME_FROM / 10000000000;
                
                var sereServs = new HisSereServManager().GetHisSereServByTransactionTime(castFilter.TRANSACTION_TIME_FROM, castFilter.TRANSACTION_TIME_TO);
                sereServs = sereServs.Where(o => o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                TOTAL_PRICE_BHYT = sereServs.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.VIR_TOTAL_PRICE ?? 0);
                TOTAL_PRICE_VP = sereServs.Where(p => p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.VIR_TOTAL_PRICE??0);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                
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
            dicSingleTag.Add("YEAR", YEAR);
            dicSingleTag.Add("TOTAL_PRICE_BHYT", TOTAL_PRICE_BHYT);
            dicSingleTag.Add("TOTAL_PRICE_VP", TOTAL_PRICE_VP);
           
        }
       
    }
}