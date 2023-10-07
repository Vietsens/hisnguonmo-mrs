using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00615
{
    public class Mrs00615Processor : AbstractProcessor
    {
        Mrs00615Filter castFilter = null;
        private List<Mrs00615RDO> ListRdo = new List<Mrs00615RDO>();
        List<HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        CommonParam paramGet = new CommonParam();
        public Mrs00615Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00615Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00615Filter)reportFilter);
            var result = true;
            try
            {
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                reportTypeCatFilter.ID = castFilter.REPORT_TYPE_CAT_ID;
                ListReportTypeCat = new HisReportTypeCatManager().Get(reportTypeCatFilter);
                if (ListReportTypeCat != null)
                {
                    HisServiceRetyCatFilterQuery srFilter = new HisServiceRetyCatFilterQuery();
                    srFilter.REPORT_TYPE_CAT_ID = ListReportTypeCat.Select(o => o.ID).FirstOrDefault();
                    ListServiceRetyCat = new HisServiceRetyCatManager().Get(srFilter);
                }

                var serviceIds = ListServiceRetyCat.Select(s => s.SERVICE_ID).Distinct().ToList();

                //YC
                HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                filterSr.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filterSr.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                filterSr.HAS_EXECUTE = true;
                filterSr.SERVICE_REQ_STT_ID = 3;
                filterSr.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                filterSr.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID;
                filterSr.REQUEST_ROOM_ID = castFilter.REQUEST_ROOM_ID;
                var listServicereqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                if (IsNotNullOrEmpty(listServicereqSub))
                    ListServiceReq.AddRange(listServicereqSub);
                //Inventec.Common.Logging.LogSystem.Info("ListServiceReq" + ListServiceReq.Count);
                if (IsNotNullOrEmpty(serviceIds))
                {
                    var skip = 0;
                    while (serviceIds.Count - skip > 0)
                    {
                        var listIDs = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //YC - DV
                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                        filterSs.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                        filterSs.HAS_EXECUTE = true;
                        filterSs.SERVICE_IDs = listIDs;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSs);
                        if (IsNotNullOrEmpty(listSereServSub))
                            ListSereServ.AddRange(listSereServSub);
                        //DV
                        HisServiceFilterQuery filterSv = new HisServiceFilterQuery();
                        filterSv.IDs = listIDs;
                        var listServiceSub = new HisServiceManager(paramGet).Get(filterSv);
                        if (IsNotNullOrEmpty(listServiceSub))
                            listHisService.AddRange(listServiceSub);
                    }
                    ListSereServ = ListSereServ.Where(o => ListServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }

                var treatmentIds = ListServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList();
                var dicPatientTypeAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(treatmentIds).OrderBy(q => q.LOG_TIME).ThenBy(o => o.ID).GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList());

                if (IsNotNull(castFilter.TREATMENT_TYPE_ID))
                {
                    ListSereServ = ListSereServ.Where(o => castFilter.TREATMENT_TYPE_ID == (dicPatientTypeAlter.ContainsKey(o.TDL_TREATMENT_ID ?? 0) ? dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0].Last().TREATMENT_TYPE_ID : 0)).ToList();
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
            var result = true;
            string key = "";
            try
            {
                List<Mrs00615RDO> listRdo = new List<Mrs00615RDO>();
                if (ListSereServ != null)
                {
                    var groupByDate = ListSereServ.GroupBy(o => o.TDL_INTRUCTION_DATE).ToList();
                    foreach (var g in groupByDate)
                    {
                        Mrs00615RDO rdo = new Mrs00615RDO();

                        key = g.Key.ToString();
                        rdo.DIC_ROOM_AMOUNT = g.ToList().GroupBy(o => RoomCode(o.TDL_REQUEST_ROOM_ID)).ToDictionary(p => p.Key, q => q.Sum(s => NumberOfFilm(s.SERVICE_ID) ?? s.AMOUNT));
                        rdo.DIC_DEPARTMENT_AMOUNT = g.ToList().GroupBy(o => DepartmentCode(o.TDL_REQUEST_DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Sum(s => NumberOfFilm(s.SERVICE_ID) ?? s.AMOUNT));
                        rdo.INTRUCTION_DATE = g.Key;
                        rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(g.Key);

                        listRdo.Add(rdo);
                    }
                }

                DateTime Itime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM ?? 0);
                while (Itime < (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_TO ?? 0))
                {
                    DateTime IDate = new System.DateTime(Itime.Year, Itime.Month, Itime.Day);
                    Mrs00615RDO rdo = listRdo.FirstOrDefault(o => (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.INTRUCTION_DATE) == IDate);
                    if (rdo == null)
                    {
                        rdo = new Mrs00615RDO();
                        rdo.INTRUCTION_DATE = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(IDate) ?? 0;
                        rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.INTRUCTION_DATE);
                    }

                    ListRdo.Add(rdo);
                    Itime = Itime.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
                Inventec.Common.Logging.LogSystem.Info("Key" + key);
            }
            return result;
        }

        private decimal? NumberOfFilm(long serviceId)
        {
            var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
            if (service != null && service.NUMBER_OF_FILM > 0)
            {
                return service.NUMBER_OF_FILM;
            }
            else
            {
                return null;
            }
        }

        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                result = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string RoomCode(long roomId)
        {
            string result = "";
            try
            {
                result = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == roomId) ?? new V_HIS_ROOM()).ROOM_TYPE_CODE + "_" + (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == roomId) ?? new V_HIS_ROOM()).ROOM_CODE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00615Filter)this.reportFilter).TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00615Filter)this.reportFilter).TIME_TO ?? 0));
            if (ListReportTypeCat != null)
            {
                dicSingleTag.Add("CATEGORY_NAME", ((ListReportTypeCat.FirstOrDefault() ?? new HIS_REPORT_TYPE_CAT()).CATEGORY_NAME ?? "").ToUpper());
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.INTRUCTION_DATE).ToList());

            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
