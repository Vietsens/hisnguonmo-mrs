using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00520;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatmentLogging;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisHeinApproval;

namespace MRS.Processor.Mrs00520
{
    public class Mrs00520Processor : AbstractProcessor
    {
        private List<Mrs00520RDO> ListRdo = new List<Mrs00520RDO>();
        List<HIS_TREATMENT_LOGGING> ListHisTreatmentLogging = new List<HIS_TREATMENT_LOGGING>();
        List<HIS_TREATMENT_LOGGING> listHisTreatmentLoggingHeinLock = new List<HIS_TREATMENT_LOGGING>();

        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        
        Mrs00520Filter filter = null;

        string thisReportTypeCode = "";

        public Mrs00520Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00520Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00520Filter)this.reportFilter;
            try
            {
                //cac ho so da huy duyet gia dinh

                HisTreatmentLoggingFilterQuery HisTreatmentLoggingfilter = new HisTreatmentLoggingFilterQuery();
                HisTreatmentLoggingfilter.CREATE_TIME_FROM = filter.TIME_FROM;
                HisTreatmentLoggingfilter.CREATE_TIME_TO = filter.TIME_TO;
                HisTreatmentLoggingfilter.ORDER_DIRECTION = "ASC";
                HisTreatmentLoggingfilter.ORDER_FIELD = "CREATE_TIME";
                HisTreatmentLoggingfilter.TREATMENT_LOG_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_LOG_TYPE.ID__MKBH;
                ListHisTreatmentLogging = new HisTreatmentLoggingManager().Get(HisTreatmentLoggingfilter);

                var listTreatmentId = ListHisTreatmentLogging.Select(o => o.TREATMENT_ID).Distinct().ToList();
                // HSDT tuong ung
                if (listTreatmentId != null && listTreatmentId.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                        HisTreatmentfilter.IDs = limit;
                        var listHisTreatmentSub = new HisTreatmentManager().Get(HisTreatmentfilter);
                        if (listHisTreatmentSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisTreatmentSub Get null");
                        else
                            listHisTreatment.AddRange(listHisTreatmentSub);
                    }
                }

                // Duyet giam dinh tuong ung
                if (listTreatmentId != null && listTreatmentId.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentLoggingFilterQuery HisTreatmentLoggingHeinLockfilter = new HisTreatmentLoggingFilterQuery();
                        HisTreatmentLoggingHeinLockfilter.TREATMENT_IDs = limit;
                        HisTreatmentLoggingHeinLockfilter.ORDER_DIRECTION = "DESC";
                        HisTreatmentLoggingHeinLockfilter.ORDER_FIELD = "CREATE_TIME";
                        HisTreatmentLoggingHeinLockfilter.TREATMENT_LOG_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_LOG_TYPE.ID__KBH;
                        var listHisTreatmentLoggingSub = new HisTreatmentLoggingManager().Get(HisTreatmentLoggingfilter);
                        if (listHisTreatmentLoggingSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisTreatmentLoggingSub Get null");
                        else
                            listHisTreatmentLoggingHeinLock.AddRange(listHisTreatmentLoggingSub);
                    }
                }
                
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
                ListRdo = (from r in ListHisTreatmentLogging select new Mrs00520RDO(r, listHisTreatment, listHisTreatmentLoggingHeinLock)).ToList();
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00520RDO>();
                result = false;
            }
            return result;
        }

       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
        }

       

    }

}
