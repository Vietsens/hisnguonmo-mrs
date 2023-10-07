using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MRS.MANAGER.Config;
using System.Reflection;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00594
{
    public class Mrs00594Processor : AbstractProcessor
    {
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        Dictionary<string, decimal> DIC_AMOUNT = new Dictionary<string, decimal>();
        Mrs00594Filter filter = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00594Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00594Filter);
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00594Filter)reportFilter);
            var result = true;
            try
            {
                //Danh sách yêu cầu
                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter = this.MapFilter<Mrs00594Filter, HisServiceReqFilterQuery>(filter, serviceReqFilter);
                listHisServiceReq = new HisServiceReqManager(paramGet).Get(serviceReqFilter);
                var listTreatmentIds = listHisServiceReq.Select(s => s.TREATMENT_ID).Distinct().ToList();
                //Danh sách Chuyển đối tượng
                if (listTreatmentIds != null && listTreatmentIds.Count > 0)
                {
                    var skip = 0;

                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var limit = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery treatmentFilter = new HisPatientTypeAlterFilterQuery();
                        treatmentFilter.TREATMENT_IDs = limit;
                        treatmentFilter.ORDER_FIELD = "ID";
                        treatmentFilter.ORDER_DIRECTION = "ASC";

                        var listPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(treatmentFilter);
                        if (listPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listPatientTypeAlterSub);
                    }

                    listHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).GroupBy(q => q.TREATMENT_ID).Select(r => r.Last()).ToList();

                }
                if (filter.IS_TREAT.HasValue)
                {
                    if (filter.IS_TREAT.Value)
                    {
                        listHisPatientTypeAlter = listHisPatientTypeAlter.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                    else
                    {
                        listHisPatientTypeAlter = listHisPatientTypeAlter.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }
                //Lấy danh sách dịch vụ yc
                if (listTreatmentIds != null && listTreatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var sereServFilter = new HisSereServFilterQuery
                        {
                            TREATMENT_IDs = listIDs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false
                        };
                        var listHisSereServSub = new HisSereServManager(paramGet).Get(sereServFilter);
                        listHisSereServ.AddRange(listHisSereServSub);
                    }
                    listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();

                    if (listHisPatientTypeAlter != null) 
                    {
                        listHisSereServ = listHisSereServ.Where(o => listHisPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TDL_TREATMENT_ID)).ToList();
                    }

                    if(filter.PATIENT_TYPE_IDs !=null)
                    {
                        listHisSereServ = listHisSereServ.Where(o=>filter.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID)).ToList();
                    }    

                }
                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00594"
                };
                reportTypeCats = new HisReportTypeCatManager().Get(HisReportTypeCatfilter);
                var reportTypeCatIds = reportTypeCats.Select(o => o.ID).ToList() ?? new List<long>();
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CAT_IDs = reportTypeCatIds
                };
                listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);
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
            var result = true;
            try
            {
                
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_FROM ?? filter.START_TIME_FROM ?? filter.FINISH_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_TO ?? filter.START_TIME_TO ?? filter.FINISH_TIME_TO ?? 0));

            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            if (filter.IS_TREAT.HasValue)
            {
                if (filter.IS_TREAT.Value)
                {
                    dicSingleTag.Add("TITLE", "Nội trú");
                }
                else
                {
                    dicSingleTag.Add("TITLE", "Ngoại trú");
                }
            }
            else 
            {
                dicSingleTag.Add("TITLE", "");
            }
            foreach (var r in reportTypeCats)
            {
                    dicSingleTag.Add(r.CATEGORY_CODE, listHisSereServ.Where(p => listHisServiceRetyCat.Exists(q => q.SERVICE_ID == p.SERVICE_ID && q.REPORT_TYPE_CAT_ID == r.ID)).Sum(o => o.AMOUNT));

            }
           
        }
    }
}
