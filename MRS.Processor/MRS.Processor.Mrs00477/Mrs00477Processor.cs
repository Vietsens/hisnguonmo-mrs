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
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport.RDO;

namespace MRS.Processor.Mrs00477
{
    public class Mrs00477Processor : AbstractProcessor
    {
        List<Mrs00477RDO> listRdo = new List<Mrs00477RDO>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
        Mrs00477Filter filter = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00477Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00477Filter);
        }
        protected override bool GetData()///
        {
            filter = ((Mrs00477Filter)reportFilter);
            var result = true;
            try
            {
                //Danh sách yêu cầu
                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter = this.MapFilter<Mrs00477Filter, HisServiceReqFilterQuery>(filter, serviceReqFilter);
                listHisServiceReq = new HisServiceReqManager(paramGet).Get(serviceReqFilter);
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    listHisServiceReq = listHisServiceReq.Where(o => o.TREATMENT_TYPE_ID == filter.TREATMENT_TYPE_ID).ToList();
                }
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
                        var TreatmentFilter = new HisTreatmentFilterQuery
                        {
                            IDs = listIDs
                        };
                        var listHisTreatmentSub = new HisTreatmentManager(paramGet).Get(TreatmentFilter);
                        if (listHisTreatmentSub != null)
                        {
                            listHisTreatment.AddRange(listHisTreatmentSub);
                        }
                        var sereServFilter = new HisSereServFilterQuery
                        {
                            TREATMENT_IDs = listIDs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false,
                            TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                        };
                        var listHisSereServSub = new HisSereServManager(paramGet).Get(sereServFilter);
                        if (listHisSereServSub != null)
                        {
                            listHisSereServ.AddRange(listHisSereServSub);
                        }
                    }
                    listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }

                var listServiceRetyCat = new HisServiceRetyCatManager().GetView(new HisServiceRetyCatViewFilterQuery() { REPORT_TYPE_CODE__EXACT = "MRS00583" });
                if (filter.CATEGORY_CODE__XQ != null)
                {
                    listServiceRetyCat = listServiceRetyCat.Where(o => o.CATEGORY_CODE == filter.CATEGORY_CODE__XQ).ToList();
                }
                listHisSereServ = listHisSereServ.Where(o => listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID)).ToList();
                if (IsNotNullOrEmpty(listHisSereServ))
                {
                    List<long> sereServId = listHisSereServ.Select(s => s.ID).ToList();
                    int start = 0;
                    int count = sereServId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServExtFilterQuery serExtFilter = new HisSereServExtFilterQuery();
                        serExtFilter.SERE_SERV_IDs = sereServId.Skip(start).Take(limit).ToList();
                        var hisSereServExts = new HisSereServExtManager(paramGet).Get(serExtFilter);
                        if (IsNotNullOrEmpty(hisSereServExts))
                        {
                            ListSereServExt.AddRange(hisSereServExts);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
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
                foreach (var item in listHisSereServ)
                {
                    var treatment = listHisTreatment.FirstOrDefault(o => o.ID == (item.TDL_TREATMENT_ID ?? 0));
                    var serviceReq = listHisServiceReq.FirstOrDefault(o => o.ID == (item.SERVICE_REQ_ID ?? 0));
                    var ext = ListSereServExt.Where(o => o.SERE_SERV_ID == item.ID).ToList();
                    Mrs00477RDO rdo = new Mrs00477RDO();
                    if (treatment != null)
                    {
                        rdo.STORE_CODE = treatment.STORE_CODE;
                        rdo.IN_CODE = treatment.IN_CODE;
                        rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        CalcuatorAge(treatment.TDL_PATIENT_DOB, treatment.TDL_PATIENT_GENDER_ID, rdo);
                    }
                    if (serviceReq != null)
                    {
                        rdo.INTRUCTION_TIME = serviceReq.INTRUCTION_TIME;
                        rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.INTRUCTION_TIME);
                        rdo.FINISH_TIME = serviceReq.FINISH_TIME ?? 0;
                        rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.FINISH_TIME ?? 0);
                        rdo.START_TIME = serviceReq.START_TIME ?? 0;
                        rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.START_TIME ?? 0);
                        rdo.EXECUTE_USERNAME = serviceReq.EXECUTE_USERNAME;
                        rdo.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                        rdo.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE;
                        rdo.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.ICD_NAME = serviceReq.ICD_NAME + ";" + serviceReq.ICD_TEXT;
                    }
                    if (ext != null)
                    {
                        rdo.RESULT = string.Join("; ", ext.Select(o => o.CONCLUDE ?? o.DESCRIPTION ?? o.NOTE).ToList());
                        rdo.BEGIN_TIME = (ext.FirstOrDefault() ?? new HIS_SERE_SERV_EXT()).BEGIN_TIME;
                        rdo.BEGIN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((ext.FirstOrDefault() ?? new HIS_SERE_SERV_EXT()).BEGIN_TIME ?? 0);
                        rdo.END_TIME = (ext.FirstOrDefault() ?? new HIS_SERE_SERV_EXT()).END_TIME;
                        rdo.END_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((ext.FirstOrDefault() ?? new HIS_SERE_SERV_EXT()).END_TIME??0);
                    }
                    rdo.HEIN_CARD_NUMBER = item.HEIN_CARD_NUMBER;
                    rdo.PRICE = item.PRICE;
                    rdo.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                    rdo.TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                    rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                    IsBhyt(item.PATIENT_TYPE_ID,rdo);
                    listRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void CalcuatorAge(long dob, long genderId, Mrs00477RDO rdo)
        {
            try
            {

                int? tuoi = RDOCommon.CalculateAge(dob);
                if (tuoi >= 0)
                {
                    if (genderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void IsBhyt(long patientTypeId, Mrs00477RDO rdo)
        {
            try
            {
                if (patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_FROM ?? filter.START_TIME_FROM ?? filter.FINISH_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_TO ?? filter.START_TIME_TO ?? filter.FINISH_TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", listRdo.OrderByDescending(o => o.TREATMENT_CODE).ToList());

        }
    }
}
