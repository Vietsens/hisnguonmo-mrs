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
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00583
{
    public class Mrs00583Processor : AbstractProcessor
    {
        List<Mrs00583RDO> listRdo = new List<Mrs00583RDO>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        //List<HIS_SERE_SERV_EXT> listHisSereServExt = new List<HIS_SERE_SERV_EXT>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        Mrs00583Filter filter = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00583Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00583Filter);
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00583Filter)reportFilter);
            var result = true;
            try
            {
                //Danh sách yêu cầu
                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter = this.MapFilter<Mrs00583Filter, HisServiceReqFilterQuery>(filter, serviceReqFilter);
                listHisServiceReq = new HisServiceReqManager(paramGet).Get(serviceReqFilter);

                Inventec.Common.Logging.LogSystem.Info("listHisServiceReq" + listHisServiceReq.Count);
                var listTreatmentIds = listHisServiceReq.Select(s => s.TREATMENT_ID).Distinct().ToList();
                //Lấy danh sách dịch vụ yc
                if (listTreatmentIds != null && listTreatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                        if (listHisPatientTypeAlterSub != null)
                        {
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                        }
                        var sereServFilter = new HisSereServFilterQuery
                        {
                            TREATMENT_IDs = listIDs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false,
                            TDL_SERVICE_TYPE_ID=IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                        };
                        var listHisSereServSub = new HisSereServManager(paramGet).Get(sereServFilter);
                        if (listHisSereServSub!=null)
                        {
                            listHisSereServ.AddRange(listHisSereServSub);
                        }
                    }
                    listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }

                var listServiceRetyCat = new HisServiceRetyCatManager().GetView(new HisServiceRetyCatViewFilterQuery() {REPORT_TYPE_CODE__EXACT="MRS00583" });
                if (filter.CATEGORY_CODE__XQ != null)
                {
                    listServiceRetyCat = listServiceRetyCat.Where(o => o.CATEGORY_CODE == filter.CATEGORY_CODE__XQ).ToList();
                }
                listHisSereServ = listHisSereServ.Where(o => listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID)).ToList();
                //var sereServId = listHisSereServ.Select(o => o.ID).ToList();
                //if (sereServId != null && sereServId.Count > 0)
                //{
                //    var skip = 0;
                //    while (sereServId.Count - skip > 0)
                //    {
                //        var listIDs = sereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //        var HisSereServExtfilter = new HisSereServExtFilterQuery
                //        {
                //            SERE_SERV_IDs = listIDs
                //        };
                //        var listHisSereServExtSub = new HisSereServExtManager(paramGet).Get(HisSereServExtfilter);
                //        if (listHisSereServExtSub != null)
                //        {
                //            listHisSereServExt.AddRange(listHisSereServExtSub);

                //        }

                //    }
                    
                //    listHisSereServ = listHisSereServ.Where(o => listHisSereServExt.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                    
                //}
               
                //    listHisSereServ = listHisSereServ.Where(o => listHisSereServExt.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                    var serviceId = listHisSereServ.Select(o => o.SERVICE_ID).Distinct().ToList();
                    if (serviceId != null && serviceId.Count > 0)
                    {
                        var skip = 0;
                        while (serviceId.Count - skip > 0)
                        {
                            var listIDs = serviceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var HisServicefilter = new HisServiceFilterQuery
                            {
                                IDs = listIDs
                            };
                            var listHisServiceSub = new HisServiceManager(paramGet).Get(HisServicefilter);
                            if (listHisServiceSub != null)
                            {
                                listHisService.AddRange(listHisServiceSub);

                            }

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
                
                if(filter.INTRUCTION_TIME_FROM!=null)
                {
                var GroupByTime = listHisSereServ.GroupBy(o=>o.TDL_INTRUCTION_DATE).ToList();
                    ProcessRdo(GroupByTime);
                }
                if(filter.START_TIME_FROM!=null)
                {
                    var GroupByTime = listHisSereServ.GroupBy(o => StartDate(o)).ToList();
                    ProcessRdo(GroupByTime);
                }
                if (filter.FINISH_TIME_FROM != null)
                {
                    var GroupByTime = listHisSereServ.GroupBy(o => FinishDate(o)).ToList();
                    ProcessRdo(GroupByTime);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private long FinishDate(HIS_SERE_SERV ss)
        {
            long date = (long)((listHisServiceReq.FirstOrDefault(o => o.ID == ss.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ()).FINISH_TIME ?? 0) / 1000000;
            return date * 1000000;
        }

        private long StartDate(HIS_SERE_SERV ss)
        {
            long date = (long)((listHisServiceReq.FirstOrDefault(o => o.ID == ss.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ()).START_TIME ?? 0) / 1000000;
            return date*1000000;
        }

        private void ProcessRdo(List<IGrouping<long, HIS_SERE_SERV>> GroupByTime)
        {
            foreach (var item in GroupByTime)
            {
                List<HIS_SERE_SERV> listSub = item.ToList<HIS_SERE_SERV>();
                Mrs00583RDO rdo = new Mrs00583RDO();
                rdo.DATE_TIME = item.Key;
                rdo.DATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.Key);
                rdo.COUNT_TREATMENT = listSub.Select(o => o.TDL_TREATMENT_CODE).Distinct().Count();
                rdo.COUNT_SERE_SERV = listSub.Sum(s=>NumberOfFilm(s.SERVICE_ID));
                rdo.AMOUNT_BH_NT = listSub.Where(o=>PatientTypeId(o.TDL_TREATMENT_ID??0)==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT&&TreatmentTypeId(o.TDL_TREATMENT_ID??0)==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => NumberOfFilm(s.SERVICE_ID));
                    //listHisSereServExt.Where(o => listSub.Exists(p => p.ID == o.SERE_SERV_ID&&PatientTypeId(p.TDL_TREATMENT_ID??0)==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT&&TreatmentTypeId(p.TDL_TREATMENT_ID??0)==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.NUMBER_OF_FILM ?? 0);
                rdo.AMOUNT_BH_NGT = listSub.Where(o => PatientTypeId(o.TDL_TREATMENT_ID ?? 0) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && TreatmentTypeId(o.TDL_TREATMENT_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => NumberOfFilm(s.SERVICE_ID));
                //listHisSereServExt.Where(o => listSub.Exists(p => p.ID == o.SERE_SERV_ID && PatientTypeId(p.TDL_TREATMENT_ID ?? 0) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && TreatmentTypeId(p.TDL_TREATMENT_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.NUMBER_OF_FILM ?? 0);
                rdo.AMOUNT_VP_NT = listSub.Where(o => PatientTypeId(o.TDL_TREATMENT_ID ?? 0) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && TreatmentTypeId(o.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => NumberOfFilm(s.SERVICE_ID));
                //listHisSereServExt.Where(o => listSub.Exists(p => p.ID == o.SERE_SERV_ID && PatientTypeId(p.TDL_TREATMENT_ID ?? 0) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && TreatmentTypeId(p.TDL_TREATMENT_ID ?? 0) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.NUMBER_OF_FILM ?? 0);
                rdo.AMOUNT_VP_NGT = listSub.Where(o => PatientTypeId(o.TDL_TREATMENT_ID ?? 0) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && TreatmentTypeId(o.TDL_TREATMENT_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => NumberOfFilm(s.SERVICE_ID));
                //listHisSereServExt.Where(o => listSub.Exists(p => p.ID == o.SERE_SERV_ID && PatientTypeId(p.TDL_TREATMENT_ID ?? 0) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && TreatmentTypeId(p.TDL_TREATMENT_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.NUMBER_OF_FILM ?? 0);
                if(this.filter.SERVICE_CODE__TEETHs!=null)
                {
                    rdo.AMOUNT_TEETH_BH = listSub.Where(o => PatientTypeId(o.TDL_TREATMENT_ID ?? 0) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && this.filter.SERVICE_CODE__TEETHs.Contains(o.TDL_SERVICE_CODE)).Sum(s => NumberOfFilm(s.SERVICE_ID));
                    //listHisSereServExt.Where(o => listSub.Exists(p => p.ID == o.SERE_SERV_ID && PatientTypeId(p.TDL_TREATMENT_ID ?? 0) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT&&this.filter.SERVICE_CODE__TEETHs.Contains(p.TDL_SERVICE_CODE))).Sum(s => s.NUMBER_OF_FILM ?? 0);
                    rdo.AMOUNT_TEETH_VP = listSub.Where(o => PatientTypeId(o.TDL_TREATMENT_ID ?? 0) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && this.filter.SERVICE_CODE__TEETHs.Contains(o.TDL_SERVICE_CODE)).Sum(s => NumberOfFilm(s.SERVICE_ID));
                    //listHisSereServExt.Where(o => listSub.Exists(p => p.ID == o.SERE_SERV_ID && PatientTypeId(p.TDL_TREATMENT_ID ?? 0) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && this.filter.SERVICE_CODE__TEETHs.Contains(p.TDL_SERVICE_CODE))).Sum(s => s.NUMBER_OF_FILM ?? 0);
                }
                listRdo.Add(rdo);
            }
        }

        private decimal NumberOfFilm(long serviceId)
        {
            var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
            if (service != null)
            {
                return service.NUMBER_OF_FILM??0;
            }
            else
            {
                return 0;
            }
        }

        private long TreatmentTypeId(long treatmentId)
        {
            return (listHisPatientTypeAlter.Where(q => q.TREATMENT_ID == treatmentId).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).LastOrDefault() ?? new HIS_PATIENT_TYPE_ALTER()).TREATMENT_TYPE_ID;
        }

        private long PatientTypeId(long treatmentId)
        {
            return (listHisPatientTypeAlter.Where(q => q.TREATMENT_ID == treatmentId).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).LastOrDefault() ?? new HIS_PATIENT_TYPE_ALTER()).PATIENT_TYPE_ID;
        }
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_FROM ?? filter.START_TIME_FROM ?? filter.FINISH_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_TO ?? filter.START_TIME_TO ?? filter.FINISH_TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", listRdo.OrderByDescending(o => o.DATE_TIME).ToList());

        }
    }
}
