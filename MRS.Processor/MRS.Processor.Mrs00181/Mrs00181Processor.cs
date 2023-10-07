using MOS.MANAGER.HisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using FlexCel.Core; 

namespace MRS.Processor.Mrs00181
{
    internal class Mrs00181Processor : AbstractProcessor
    {
        List<VSarReportMrs00181RDO> _listSarReportMrs00181Rdos = new List<VSarReportMrs00181RDO>();
        List<VSarReportMrs00181RDO> listRdoMedi = new List<VSarReportMrs00181RDO>();
        List<VSarReportMrs00181RDO> listRdoNotMedi = new List<VSarReportMrs00181RDO>();
        Mrs00181Filter CastFilter;
        List<ReqTypeUsed> listRdo = new List<ReqTypeUsed>();
        Dictionary<long, ReqTypeUsed> dicVSs = new Dictionary<long, ReqTypeUsed>();

        public Mrs00181Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00181Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00181Filter)this.reportFilter;
                long serviceIdXQ = -1;
                long serviceIdDT = -1;
                List<HIS_SERVICE> listService = new HisServiceManager().Get(new HisServiceFilterQuery() { SERVICE_CODEs = new List<string>() { CastFilter.SERVICE_CODE__DT, CastFilter.SERVICE_CODE__XQ } });
                if (listService != null && listService.Count > 0)
                {
                    var serviceDT= listService.FirstOrDefault(o => o.SERVICE_CODE == CastFilter.SERVICE_CODE__DT);
                    if (serviceDT != null)
                    {
                        serviceIdDT = serviceDT.ID;
                    }
                    var serviceXQ = listService.FirstOrDefault(o => o.SERVICE_CODE == CastFilter.SERVICE_CODE__XQ);
                    if (serviceXQ != null)
                    {
                        serviceIdXQ = serviceXQ.ID;
                    }
                }
                listRdo = new ManagerSql().GetReqTypeUsed(CastFilter, serviceIdDT, serviceIdXQ);
                if (CastFilter.PARENT_SV_CODE__VSs != null && CastFilter.PARENT_SV_CODE__VSs.Length > 0)
                {
                    var VSs = new ManagerSql().GetReqTypeVsUsed(CastFilter);
                    if (VSs != null)
                    {
                        dicVSs = VSs.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.First());
                    }
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


        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    _listSarReportMrs00181Rdos = ProcessFilterData(listRdo);
                    //if (CastFilter.IS_SPLIT_HASMEDI == true)
                    {
                        var medis = listRdo.Where(o => o.FINISH_TIME > 0 && o.START_EXAM_TIME>0).ToList();
                        foreach (var item in medis)
                        {
                            item.OUT_TIME = item.FINISH_TIME;
                            item.IN_TIME = item.START_EXAM_TIME??0;
                        }
                        listRdoMedi = ProcessFilterData(medis);
                        var mediNots = listRdo.Where(o => o.FINISH_TIME == null && o.START_EXAM_TIME>0 && o.FINISH_EXAM_TIME>0).ToList();
                        foreach (var item in mediNots)
                        {
                            item.OUT_TIME = item.FINISH_EXAM_TIME ?? 0;
                            item.IN_TIME = item.START_EXAM_TIME ?? 0;
                        }
                        listRdoNotMedi = ProcessFilterData(mediNots);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (CastFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.TIME_FROM));
                }
                if (CastFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", _listSarReportMrs00181Rdos);

                objectTag.AddObjectData(store, "ReportMedi", listRdoMedi);

                objectTag.AddObjectData(store, "ReportNotMedi", listRdoNotMedi);
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<VSarReportMrs00181RDO> ProcessFilterData(List<ReqTypeUsed> listRdo)
        {
            List<VSarReportMrs00181RDO> result = new List<VSarReportMrs00181RDO>();
            try
            {
                List<VSarReportMrs00181RDO> services = new List<VSarReportMrs00181RDO>();
                VSarReportMrs00181RDO service = new VSarReportMrs00181RDO();
                service.SERVICE_NAMEs = "Khám";
                createService(service);
                services.Add(service);
                VSarReportMrs00181RDO service1 = new VSarReportMrs00181RDO();
                service1.SERVICE_NAMEs = "Khám + Xét nghiệm";
                createService(service1);
                services.Add(service1);
                VSarReportMrs00181RDO service2 = new VSarReportMrs00181RDO();
                service2.SERVICE_NAMEs = "Khám + Xét nghiệm + CĐHA";
                createService(service2);
                services.Add(service2);
                VSarReportMrs00181RDO service3 = new VSarReportMrs00181RDO();
                service3.SERVICE_NAMEs = "Khám + Xét nghiệm + CĐHA + TDCN";
                createService(service3);
                services.Add(service3);
                VSarReportMrs00181RDO service4 = new VSarReportMrs00181RDO();
                service4.SERVICE_NAMEs = "Khám + Khác";
                createService(service4);
                services.Add(service4);
                VSarReportMrs00181RDO service5 = new VSarReportMrs00181RDO();
                service5.SERVICE_NAMEs = "Tổng thời gian khám trung bình cho một lượt khám.";
                createService(service5);
                services.Add(service5);
                if (CastFilter.PARENT_SV_CODE__VSs != null && CastFilter.PARENT_SV_CODE__VSs.Length > 0)
                {
                    VSarReportMrs00181RDO service6 = new VSarReportMrs00181RDO();
                    service6.SERVICE_NAMEs = "Khám + Xét nghiệm vi sinh";
                    createService(service6);
                    services.Add(service6);
                }
                string kham = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH.ToString();
                string xn = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN.ToString();
                string tdcn = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN.ToString();
                string cdha = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA.ToString();
                string ns = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS.ToString();
                string sa = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA.ToString();

                foreach (var item in listRdo)
                {
                    if (string.IsNullOrEmpty(item.LIST_TYPE))
                        continue;
                    string[] types = item.LIST_TYPE.Split('_');
                    if (types.Contains(kham))
                    {
                        if (types.Contains(xn))
                        {
                            if (types.Contains(cdha)
                                ||types.Contains(ns)
                            || types.Contains(sa))
                            {
                                if (types.Contains(tdcn))
                                {
                                    FillDataToDic(services[3], item);
                                }
                                else
                                {
                                    FillDataToDic(services[2], item);
                                }
                            }
                            else
                            {
                                if (types.Contains(tdcn))
                                {
                                    FillDataToDic(services[4], item);
                                }
                                else
                                {
                                    FillDataToDic(services[1], item);
                                    if (dicVSs.ContainsKey(item.TREATMENT_ID))
                                    {
                                        FillDataToDic(services[6], item);
                                    }
                                }
                            }
                        }
                        else if (types.Distinct().ToArray().Length==1)
                        {
                            FillDataToDic(services[0], item);
                        }
                        else
                        {
                            FillDataToDic(services[4], item);
                        }
                    }
                    FillDataToDic(services[5], item);
                }
                foreach (var item in services)
                {
                    if (item.COUNT != 0)
                    {
                        long avg = (long)(item.SUM_TIME / (item.COUNT));
                        item.A = ((int)(avg / 3600)).ToString() + ":" + ((int)((avg % 3600) / 60)).ToString() + ":" + ((int)((avg % 3600) % 60)).ToString();
                    }
                    result.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void FillDataToDic(VSarReportMrs00181RDO service, ReqTypeUsed item)
        {
            long in_time = item.IN_TIME;
            long out_time = (long)item.OUT_TIME;
            System.DateTime? in_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(in_time);
            System.DateTime? out_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(out_time);
            TimeSpan tdiff = out_t.Value - in_t.Value;
            FillDataToTotal(service, item, tdiff);
            FillDataToDicMonth(service, item, tdiff);
            FillDataToDicDate(service, item, tdiff, in_t);
            FillDataToDicHour(service, item, tdiff);
        }

        private void FillDataToTotal(VSarReportMrs00181RDO service, ReqTypeUsed item, TimeSpan tdiff)
        {
            service.COUNT++;
            service.SUM_TIME += (int)tdiff.TotalSeconds;
        }

        private void FillDataToDicHour(VSarReportMrs00181RDO service, ReqTypeUsed item, TimeSpan tdiff)
        {
            string key = "";
            if (item.IN_TIME % 1000000 > 120000)
            {
                key = "PM";
            }
            else
            {
                key = "AM";
            }
            if (!service.DIC_HOUR_COUNT.ContainsKey(key))
            {
                service.DIC_HOUR_COUNT.Add(key, 0);
                service.DIC_HOUR_SUM_TIME.Add(key, 0);
            }
            service.DIC_HOUR_COUNT[key]++;
            service.DIC_HOUR_SUM_TIME[key] += (int)tdiff.TotalMinutes;
        }

        private void FillDataToDicDate(VSarReportMrs00181RDO service, ReqTypeUsed item, TimeSpan tdiff, System.DateTime? in_t)
        {
            DayOfWeek date = in_t.Value.DayOfWeek;
            if (!service.DIC_DATE_COUNT.ContainsKey(date.ToString()))
            {
                service.DIC_DATE_COUNT.Add(date.ToString(), 0);
                service.DIC_DATE_SUM_TIME.Add(date.ToString(), 0);
            }
            service.DIC_DATE_COUNT[date.ToString()]++;
            service.DIC_DATE_SUM_TIME[date.ToString()] += (int)tdiff.TotalMinutes;
        }

        private void FillDataToDicMonth(VSarReportMrs00181RDO service, ReqTypeUsed item, TimeSpan tdiff)
        {
            if (!service.DIC_MONTH_COUNT.ContainsKey(item.IN_TIME.ToString().Substring(4, 2)))
            {
                service.DIC_MONTH_COUNT.Add(item.IN_TIME.ToString().Substring(4, 2), 0);
                service.DIC_MONTH_SUM_TIME.Add(item.IN_TIME.ToString().Substring(4, 2), 0);
            }
            service.DIC_MONTH_COUNT[item.IN_TIME.ToString().Substring(4, 2)]++;
            service.DIC_MONTH_SUM_TIME[item.IN_TIME.ToString().Substring(4, 2)] += (int)tdiff.TotalMinutes;
        }

        private void createService(VSarReportMrs00181RDO service)
        {
            service.DIC_DATE_COUNT = new Dictionary<string, int>();
            service.DIC_DATE_SUM_TIME = new Dictionary<string, int>();
            service.DIC_HOUR_COUNT = new Dictionary<string, int>();
            service.DIC_HOUR_SUM_TIME = new Dictionary<string, int>();
            service.DIC_MONTH_COUNT = new Dictionary<string, int>();
            service.DIC_MONTH_SUM_TIME = new Dictionary<string, int>();
        }

    }
}
