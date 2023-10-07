using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisPatientTypeAlter;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisTestIndex;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatmentBedRoom;


namespace MRS.Processor.Mrs00557
{

    class Mrs00557Processor : AbstractProcessor
    {
        Mrs00557Filter castFilter = null;
        List<Mrs00557RDO> listRdoDetail = new List<Mrs00557RDO>();
        List<Mrs00557RDO> listRdo = new List<Mrs00557RDO>();
        List<Mrs00557RDO> listRdoDateTime = new List<Mrs00557RDO>();
        List<HIS_TEST_INDEX> listHisTestIndex = new List<HIS_TEST_INDEX>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_TREATMENT_BED_ROOM> listHisTreatmentBedRoom = new List<V_HIS_TREATMENT_BED_ROOM>();

        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();

        private string REPORT_TYPE_CODE = "";
        public Mrs00557Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            this.REPORT_TYPE_CODE = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00557Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00557Filter)this.reportFilter;
                HisServiceRetyCatViewFilterQuery HisServiceretyCatfilter = new HisServiceRetyCatViewFilterQuery();
                HisServiceretyCatfilter.REPORT_TYPE_CODE__EXACT = this.REPORT_TYPE_CODE;
                listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceretyCatfilter);
                HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                HisServiceReqfilter = this.MapFilter<Mrs00557Filter, HisServiceReqFilterQuery>(castFilter, HisServiceReqfilter);
                HisServiceReqfilter.EXECUTE_ROOM_ID = castFilter.EXE_ROOM_ID;
                listHisServiceReq.AddRange(new HisServiceReqManager(param).Get(HisServiceReqfilter));

                HisTreatmentBedRoomViewFilterQuery HisTreatmentBedRoomfilter = new HisTreatmentBedRoomViewFilterQuery();
                HisTreatmentBedRoomfilter.ADD_TIME_TO = castFilter.INTRUCTION_TIME_TO;
                HisTreatmentBedRoomfilter.IS_IN_ROOM = true;
                listHisTreatmentBedRoom.AddRange(new HisTreatmentBedRoomManager().GetView(HisTreatmentBedRoomfilter) ?? new List<V_HIS_TREATMENT_BED_ROOM>());

                HisTreatmentBedRoomfilter = new HisTreatmentBedRoomViewFilterQuery();
                HisTreatmentBedRoomfilter.ADD_TIME_TO = castFilter.INTRUCTION_TIME_TO;
                HisTreatmentBedRoomfilter.REMOVE_TIME_FROM = castFilter.INTRUCTION_TIME_FROM;
                listHisTreatmentBedRoom.AddRange(new HisTreatmentBedRoomManager().GetView(HisTreatmentBedRoomfilter) ?? new List<V_HIS_TREATMENT_BED_ROOM>());
                listHisTreatmentBedRoom = listHisTreatmentBedRoom.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                var listServiceReqIds = listHisServiceReq.Select(o => o.ID).ToList();
                if (IsNotNullOrEmpty(listServiceReqIds))
                {
                    var skip = 0;
                    while (listServiceReqIds.Count - skip > 0)
                    {
                        var listSVRIds = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID;
                        HisSereServfilter.SERVICE_REQ_IDs = listSVRIds;
                        HisSereServfilter.HAS_EXECUTE = true;
                        HisSereServfilter.IS_EXPEND = false;
                        var listHisSereServSub = new MOS.MANAGER.HisSereServ.HisSereServManager(param).Get(HisSereServfilter);
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Debug("HIS_SERE_SERV is null");
                        listHisSereServ.AddRange(listHisSereServSub);
                    }
                }
                if (castFilter.PATIENT_TYPE_ID != null)
                {
                    listHisSereServ = listHisSereServ.Where(o => o.PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID).ToList();
                }


                var treatmentIds = listHisSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = listIds;
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ID";
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ASC";
                        lastHisPatientTypeAlter.AddRange(new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter));
                    }
                    lastHisPatientTypeAlter = lastHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }

                listHisService = new HisServiceManager().Get(new HisServiceFilterQuery());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                if (IsNotNullOrEmpty(listHisSereServ))
                {
                    foreach (var r in listHisSereServ)
                    {
                        var patientTypeAlter = lastHisPatientTypeAlter.FirstOrDefault(s => s.TREATMENT_ID == r.TDL_TREATMENT_ID) ?? new HIS_PATIENT_TYPE_ALTER();
                        var service = listHisService.FirstOrDefault(s => s.ID == r.SERVICE_ID) ?? new HIS_SERVICE();
                        var serviceRetyCat = listHisServiceRetyCat.FirstOrDefault(s => s.SERVICE_ID == r.SERVICE_ID) ?? new V_HIS_SERVICE_RETY_CAT();
                        var rdo = new Mrs00557RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00557RDO>(rdo, r);
                        rdo.HIS_SERVICE_REQ = listHisServiceReq.FirstOrDefault(s => s.ID == r.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();

                        rdo.TYPE = this.TypeName(r.HEIN_CARD_NUMBER);

                        rdo.TREATMENT_TYPE_ID = patientTypeAlter.TREATMENT_TYPE_ID;
                        rdo.GR_CODE = serviceRetyCat.CATEGORY_CODE;
                        rdo.GR_NAME = serviceRetyCat.CATEGORY_NAME;
                        rdo.SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == r.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                        listRdoDetail.Add(rdo);
                    }
                    GroupByDateAndType();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GroupByDateAndType()
        {
            try
            {
                var groupByDateAndType = listRdoDetail.OrderBy(q=>q.TDL_INTRUCTION_TIME).Where(p => p.TDL_INTRUCTION_DATE != 0).GroupBy(o => new { o.TYPE, o.TDL_INTRUCTION_DATE }).ToList();

                foreach (var item in groupByDateAndType)
                {
                    Mrs00557RDO rdo = new Mrs00557RDO();
                    List<Mrs00557RDO> listSub = item.ToList<Mrs00557RDO>();
                    rdo.TYPE = listSub.First().TYPE;
                    rdo.TDL_INTRUCTION_DATE = listSub.First().TDL_INTRUCTION_DATE;
                    rdo.COUNT_TREATMENT_IN = listSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p=>p.TDL_TREATMENT_ID).Distinct().ToList().Count;
                    rdo.COUNT_TREATMENT_OUT = listSub.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().ToList().Count;

                    rdo.COUNT_TREATMENT_BED_ROOM_IN = listHisTreatmentBedRoom.Where(o => (Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.ADD_TIME)) == (Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.TDL_INTRUCTION_DATE)) && lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == o.TREATMENT_ID && q.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) && rdo.TYPE == this.TypeName(o.TDL_HEIN_CARD_NUMBER)).Select(p => p.TREATMENT_ID).Distinct().ToList().Count;

                    rdo.COUNT_TREATMENT_BED_ROOM_OUT = listHisTreatmentBedRoom.Where(o => (Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.ADD_TIME)) == (Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.TDL_INTRUCTION_DATE)) && lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == o.TREATMENT_ID && q.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) && rdo.TYPE == this.TypeName(o.TDL_HEIN_CARD_NUMBER)).Select(p => p.TREATMENT_ID).Distinct().ToList().Count;

                    rdo.DICT_AMOUNT_IN = listSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DICT_AMOUNT_OUT = listSub.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DICT_AMOUNT_TOTAL = listSub.GroupBy(p => p.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DICT_COUNT_IN = listSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, q => q.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DICT_COUNT_OUT = listSub.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, q => q.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DICT_COUNT_TOTAL = listSub.GroupBy(p => p.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, q => q.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().Count());

                    rdo.DICG_AMOUNT_IN = listSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DICG_AMOUNT_OUT = listSub.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DICG_AMOUNT_TOTAL = listSub.GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DICG_MONEY_TOTAL = listSub.GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));

                    rdo.DICG_COUNT_IN = listSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DICG_COUNT_OUT = listSub.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().Count());
                    rdo.DICG_COUNT_TOTAL = listSub.GroupBy(p => p.GR_CODE ?? "__").ToDictionary(q => q.Key, q => q.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().Count());

                    listRdo.Add(rdo);
                }
                listRdoDateTime = listRdo.GroupBy(s => s.TDL_INTRUCTION_DATE).Select(p => p.First()).ToList();
                foreach (var item in listRdoDateTime)
                {
                    if (!listRdo.Exists(o => o.TYPE == this.castFilter.H_01 && item.TDL_INTRUCTION_DATE == o.TDL_INTRUCTION_DATE))
                    {
                        listRdo.Add(new Mrs00557RDO() { TYPE = this.castFilter.H_01, TDL_INTRUCTION_DATE = item.TDL_INTRUCTION_DATE });
                    }
                    if (!listRdo.Exists(o => o.TYPE == this.castFilter.H_02 && item.TDL_INTRUCTION_DATE == o.TDL_INTRUCTION_DATE))
                    {
                        listRdo.Add(new Mrs00557RDO() { TYPE = this.castFilter.H_02, TDL_INTRUCTION_DATE = item.TDL_INTRUCTION_DATE });
                    }
                    if (!listRdo.Exists(o => o.TYPE == this.castFilter.H_OTHER && item.TDL_INTRUCTION_DATE == o.TDL_INTRUCTION_DATE))
                    {
                        listRdo.Add(new Mrs00557RDO() { TYPE = this.castFilter.H_OTHER, TDL_INTRUCTION_DATE = item.TDL_INTRUCTION_DATE });
                    }
                    if (!listRdo.Exists(o => o.TYPE == this.castFilter.TB && item.TDL_INTRUCTION_DATE == o.TDL_INTRUCTION_DATE))
                    {
                        listRdo.Add(new Mrs00557RDO() { TYPE = this.castFilter.TB, TDL_INTRUCTION_DATE = item.TDL_INTRUCTION_DATE });
                    }
                    if (!listRdo.Exists(o => o.TYPE == this.castFilter.CS && item.TDL_INTRUCTION_DATE == o.TDL_INTRUCTION_DATE))
                    {
                        listRdo.Add(new Mrs00557RDO() { TYPE = this.castFilter.CS, TDL_INTRUCTION_DATE = item.TDL_INTRUCTION_DATE });
                    }
                    if (!listRdo.Exists(o => o.TYPE == this.castFilter.DV && item.TDL_INTRUCTION_DATE == o.TDL_INTRUCTION_DATE))
                    {
                        listRdo.Add(new Mrs00557RDO() { TYPE = this.castFilter.DV, TDL_INTRUCTION_DATE = item.TDL_INTRUCTION_DATE });
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string TypeName(string HEIN_CARD_NUMBER)
        {
            string result = "";
            try
            {
                if (HEIN_CARD_NUMBER != null && HEIN_CARD_NUMBER != "")
                {
                    if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01 != null
                        && MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01.Exists(o => HEIN_CARD_NUMBER.StartsWith(o)))
                    {
                        result = this.castFilter.H_01;
                    }
                    else if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02 != null
                        && MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02.Exists(o => HEIN_CARD_NUMBER.StartsWith(o)))
                    {
                        result = this.castFilter.H_02;
                    }
                    else
                        result = this.castFilter.H_OTHER;
                }
                else
                {
                    result = this.castFilter.DV;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
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
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", castFilter.INTRUCTION_TIME_FROM ?? castFilter.FINISH_TIME_FROM ?? castFilter.START_TIME_FROM ?? 0);

            dicSingleTag.Add("TIME_TO", castFilter.INTRUCTION_TIME_TO ?? castFilter.FINISH_TIME_TO ?? castFilter.START_TIME_TO ?? 0);
            if (IsNotNullOrEmpty(listHisSereServ))
            {
                dicSingleTag.Add("DEPARTMENT_NAME", HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listHisSereServ.First().TDL_EXECUTE_DEPARTMENT_ID).DEPARTMENT_NAME);
            }

            objectTag.AddObjectData(store, "ReportDetail", listRdoDetail.OrderBy(s => s.HIS_SERVICE_REQ.INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "DateTime", listRdoDateTime);
            objectTag.AddObjectData(store, "Report1", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddRelationship(store, "DateTime", "Report", "TDL_INTRUCTION_DATE", "TDL_INTRUCTION_DATE");
            objectTag.AddObjectData(store, "ReportCountIn", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddObjectData(store, "ReportAmountIn", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddObjectData(store, "ReportCountOut", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddObjectData(store, "ReportAmountOut", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddObjectData(store, "ReportCountTotal", listRdo.OrderBy(s => s.TYPE).ToList());
            objectTag.AddObjectData(store, "ReportAmountTotal", listRdo.OrderBy(s => s.TYPE).ToList());

            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
