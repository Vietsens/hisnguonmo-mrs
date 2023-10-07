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
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using LIS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00630
{
    public class Mrs00630Processor : AbstractProcessor
    {
        List<Mrs00630RDO> listRdo = new List<Mrs00630RDO>();

        Mrs00630Filter CastFilter;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        Mrs00630RDO SumCLS = new Mrs00630RDO();
        Mrs00630RDO SumNoCLS = new Mrs00630RDO();
        List<long> CLS_SERVICE_REQ_TYPE_ID = new List<long>() {
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA,

        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS,

        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN,

        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA,

        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN

        };
        List<SAMPLE> listSample = new List<SAMPLE>();
        public Mrs00630Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00630Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00630Filter)this.reportFilter;

                var listTreatmentFilter = new HisTreatmentFilterQuery
                {
                    IN_TIME_FROM = CastFilter.IN_TIME_FROM,
                    IN_TIME_TO = CastFilter.IN_TIME_TO,
                    OUT_TIME_FROM = CastFilter.OUT_TIME_FROM,
                    OUT_TIME_TO = CastFilter.OUT_TIME_TO,
                    FEE_LOCK_TIME_FROM = CastFilter.FEE_LOCK_TIME_FROM,
                    FEE_LOCK_TIME_TO = CastFilter.FEE_LOCK_TIME_TO,
                };
                listHisTreatment = new HisTreatmentManager(paramGet).Get(listTreatmentFilter);
                if (this.CastFilter.TREATMENT_TYPE_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => this.CastFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (CastFilter.OUT_TIME_FROM != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (CastFilter.FEE_LOCK_TIME_FROM != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => o.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                var listTreatmentId = listHisTreatment.Select(o => o.ID).Distinct().ToList();

                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE
                };
                listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);

                if (listTreatmentId != null && listTreatmentId.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var Ids = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                        HisServiceReqfilter.TREATMENT_IDs = Ids;
                        HisServiceReqfilter.ORDER_DIRECTION = "ID";
                        HisServiceReqfilter.ORDER_FIELD = "ASC";
                        HisServiceReqfilter.HAS_EXECUTE = true;
                        var listHisServiceReqSub = new HisServiceReqManager(param).Get(HisServiceReqfilter);
                        if (listHisServiceReqSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisServiceReqSub Get null");
                        else
                            listHisServiceReq.AddRange(listHisServiceReqSub);

                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = Ids;
                        HisSereServfilter.ORDER_DIRECTION = "ID";
                        HisSereServfilter.ORDER_FIELD = "ASC";
                        HisSereServfilter.HAS_EXECUTE = true;
                        var listHisSereServSub = new HisSereServManager(param).Get(HisSereServfilter);
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listHisSereServSub);
                    }
                    listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }
                if (CastFilter.SERVICE_TYPE_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => listHisSereServ.Exists(p => p.TDL_TREATMENT_ID == o.ID && CastFilter.SERVICE_TYPE_IDs.Contains(p.TDL_SERVICE_TYPE_ID))).ToList();
                }
                listSample = new ManagerSql().GetLisSample(CastFilter);
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
                if (IsNotNullOrEmpty(listHisTreatment))
                {
                    long kham = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    long xn = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN;
                    long tdcn = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN;
                    long cdha = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA;
                    long khac = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KHAC;
                    var listHisSereServCLS = listHisSereServ.Where(o => listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID && listHisServiceReq.Exists(q => q.TREATMENT_ID == p.ID && CLS_SERVICE_REQ_TYPE_ID.Contains(q.SERVICE_REQ_TYPE_ID))) && listHisServiceReq.Exists(q => q.ID == o.SERVICE_REQ_ID)).ToList();

                    var listHisServiceReqClsIds = listHisSereServCLS.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                    var listHisServiceReqCls = listHisServiceReq.Where(o => listHisServiceReqClsIds.Contains(o.ID)).ToList();
                    SumCLS.DIC_GROUP_TIME = listHisSereServCLS.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, p => AvgTime(listHisServiceReqCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                    SumCLS.DIC_WAIT_GROUP_TIME = listHisSereServCLS.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, p => AvgTimeWait(listHisServiceReqCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                    SumCLS.DIC_TYPE_TIME = listHisSereServCLS.GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => AvgTime(listHisServiceReqCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                    SumCLS.DIC_WAIT_TYPE_TIME = listHisSereServCLS.GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => AvgTimeWait(listHisServiceReqCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));

                    var listHisSereServNoCLS = listHisSereServ.Where(o => listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID && !listHisServiceReq.Exists(q => q.TREATMENT_ID == p.ID && CLS_SERVICE_REQ_TYPE_ID.Contains(q.SERVICE_REQ_TYPE_ID))) && listHisServiceReq.Exists(q => q.ID == o.SERVICE_REQ_ID)).ToList();
                    var listHisServiceReqNoClsIds = listHisSereServNoCLS.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                    var listHisServiceReqNoCls = listHisServiceReq.Where(o => listHisServiceReqNoClsIds.Contains(o.ID)).ToList();
                    SumNoCLS.DIC_GROUP_TIME = listHisSereServNoCLS.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, p => AvgTime(listHisServiceReqNoCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                    SumNoCLS.DIC_WAIT_GROUP_TIME = listHisSereServNoCLS.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, p => AvgTimeWait(listHisServiceReqNoCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                    SumNoCLS.DIC_TYPE_TIME = listHisSereServNoCLS.GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => AvgTime(listHisServiceReqNoCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                    SumNoCLS.DIC_WAIT_TYPE_TIME = listHisSereServNoCLS.GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => AvgTimeWait(listHisServiceReqNoCls.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));



                    foreach (var item in listHisTreatment)
                    {
                        var sereServSub = listHisSereServ.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList();
                        var serviceReqSub = listHisServiceReq.Where(o => o.TREATMENT_ID == item.ID).ToList();

                        Mrs00630RDO rdo = new Mrs00630RDO();
                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB;
                        rdo.IN_TIME = item.IN_TIME;
                        rdo.OUT_TIME = item.OUT_TIME;
                        rdo.FEE_LOCK_TIME = item.FEE_LOCK_TIME;

                        //phòng khám
                        rdo.TDL_FIRST_EXAM_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        if (serviceReqSub.Where(o => CLS_SERVICE_REQ_TYPE_ID.Contains(o.SERVICE_REQ_TYPE_ID)).Count() > 0)
                        {
                            rdo.HAS_CLS = true;
                        }
                        if (item.OUT_TIME.HasValue && item.FEE_LOCK_TIME.HasValue)
                        {
                            rdo.WAIT_FEE_LOCK_TIME = Inventec.Common.DateTime.Calculation.DifferenceTime(item.OUT_TIME.Value, item.FEE_LOCK_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
                            SumCLS.WAIT_FEE_LOCK_TIME += rdo.HAS_CLS ? rdo.WAIT_FEE_LOCK_TIME : 0;
                            SumNoCLS.WAIT_FEE_LOCK_TIME += rdo.HAS_CLS ? 0 : rdo.WAIT_FEE_LOCK_TIME;
                        }

                        if (item.OUT_TIME.HasValue)
                        {
                            rdo.ON_TIME = Inventec.Common.DateTime.Calculation.DifferenceTime(item.IN_TIME, item.OUT_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
                            SumCLS.ON_TIME += rdo.HAS_CLS ? rdo.ON_TIME : 0;
                            SumNoCLS.ON_TIME += rdo.HAS_CLS ? 0 : rdo.ON_TIME;
                        }

                        List<long> types = serviceReqSub.Select(p => p.SERVICE_REQ_TYPE_ID).ToList();
                        if (types != null)
                        {
                            if (types.Contains(kham))
                            {
                                if (types.Contains(xn))
                                {
                                    if (types.Contains(cdha))
                                    {
                                        if (types.Contains(tdcn))
                                        {
                                            rdo.LIST_SERVICE_TYPE = "4";

                                        }
                                        else
                                        {
                                            rdo.LIST_SERVICE_TYPE = "3";

                                        }
                                    }
                                    else
                                    {
                                        rdo.LIST_SERVICE_TYPE = "2";

                                    }
                                }
                                else
                                {
                                    if (types.Distinct().ToArray().Length == 1)
                                    {
                                        rdo.LIST_SERVICE_TYPE = "1";

                                    }
                                    else
                                    {
                                        rdo.LIST_SERVICE_TYPE = "5";

                                    }
                                }
                            }
                        }


                        SetExtendField(sereServSub, serviceReqSub, rdo);
                        listRdo.Add(rdo);
                    }
                    if (listRdo.Where(o => o.HAS_CLS).Count() > 0)
                    {
                        SumCLS.ON_TIME = (int)(SumCLS.ON_TIME / listRdo.Where(o => o.HAS_CLS).Count());
                        SumCLS.WAIT_FEE_LOCK_TIME = (int)(SumCLS.WAIT_FEE_LOCK_TIME / listRdo.Where(o => o.HAS_CLS).Count());
                    }
                    if (listRdo.Where(o => !o.HAS_CLS).Count() > 0)
                    {
                        SumNoCLS.ON_TIME = (int)(SumNoCLS.ON_TIME / listRdo.Where(o => !o.HAS_CLS).Count());
                        SumNoCLS.WAIT_FEE_LOCK_TIME = (int)(SumNoCLS.WAIT_FEE_LOCK_TIME / listRdo.Where(o => !o.HAS_CLS).Count());
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



        private int AvgTimeWait(List<HIS_SERVICE_REQ> listHisServiceReqSS)
        {
            int result = 0;
            try
            {
                var sr = listHisServiceReqSS.OrderBy(q => q.INTRUCTION_TIME).ToList();

                if (sr != null)
                {
                    int i = 0;
                    foreach (var item in sr)
                    {
                        if (item.START_TIME.HasValue && item.START_TIME>item.INTRUCTION_TIME)
                        {
                            result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.INTRUCTION_TIME, item.START_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
                            i++;
                        }
                    }
                    if (i > 0)
                    {
                        result = (int)(result / i);
                    }
                }


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private int AvgTime(List<HIS_SERVICE_REQ> listHisServiceReqSS)
        {
            int result = 0;
            try
            {
                var sr = listHisServiceReqSS.OrderBy(q => q.INTRUCTION_TIME).ToList();

                if (sr != null)
                {
                    int i = 0;
                    foreach (var item in sr)
                    {
                        var serviceReqTest = listSample.FirstOrDefault(o => o.SERVICE_REQ_ID == item.ID);
                        if (serviceReqTest != null && serviceReqTest.CREATE_TIME< serviceReqTest.RESULT_TIME)
                        {
                            result += Inventec.Common.DateTime.Calculation.DifferenceTime(serviceReqTest.CREATE_TIME, serviceReqTest.RESULT_TIME, Calculation.UnitDifferenceTime.SECOND);
                            i++;
                        }
                        else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK && item.FINISH_TIME.HasValue)
                        {
                            var treatment = listHisTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                            if (treatment != null && treatment.OUT_TIME.Value < item.FINISH_TIME)
                            {
                                //Inventec.Common.Logging.LogSystem.Info("treatment.out_time,finish_time: " + treatment.OUT_TIME + " - " + item.FINISH_TIME);
                                result += Inventec.Common.DateTime.Calculation.DifferenceTime(treatment.OUT_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
                                i++;
                            }
                        }
                        else if (item.START_TIME.HasValue && item.FINISH_TIME.HasValue && item.START_TIME.Value<item.FINISH_TIME.Value)
                        {
                            result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.START_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
                            i++;
                        }
                    }
                    if (i>0)
                    {
                        result = (int)(result / i);
                    }    
                }


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        //private int Sum(List<HIS_SERVICE_REQ> sr)
        //{
        //    int result = 0;
        //    try
        //    {
        //        foreach (var item in sr)
        //        {
        //            var serviceReqTest = listSample.FirstOrDefault(o => o.SERVICE_REQ_ID == item.ID);
        //            if (serviceReqTest != null)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(serviceReqTest.CREATE_TIME, serviceReqTest.RESULT_TIME, Calculation.UnitDifferenceTime.SECOND);
        //            }
        //            else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK && item.FINISH_TIME.HasValue)
        //            {
        //                var treatment = listHisTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
        //                if (treatment != null)
        //                {
        //                    //Inventec.Common.Logging.LogSystem.Info("treatment.out_time,finish_time: " + treatment.OUT_TIME + " - " + item.FINISH_TIME);
        //                    result += Inventec.Common.DateTime.Calculation.DifferenceTime(treatment.OUT_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //                }
        //            }
        //            else if (item.START_TIME.HasValue && item.FINISH_TIME.HasValue)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.START_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}
        //private int SumWait(List<HIS_SERVICE_REQ> sr)
        //{
        //    int result = 0;
        //    try
        //    {
        //        foreach (var item in sr)
        //        {
        //            if (item.START_TIME.HasValue)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.INTRUCTION_TIME, item.START_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}

        private void SetExtendField(List<HIS_SERE_SERV> ss, List<HIS_SERVICE_REQ> sr, Mrs00630RDO rdo)
        {
            try
            {

                rdo.DIC_TYPE_TIME = ss.GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => AvgTime(sr.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                rdo.DIC_WAIT_TYPE_TIME = ss.GroupBy(o => ServiceTypeCode(o.TDL_SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => AvgTimeWait(sr.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                rdo.DIC_GROUP_TIME = ss.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, p => AvgTime(sr.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                rdo.DIC_WAIT_GROUP_TIME = ss.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, p => AvgTimeWait(sr.Where(o => p.ToList().Exists(r => r.SERVICE_REQ_ID == o.ID)).ToList()));
                var srExam = sr.Where(o => ss.Exists(p => p.SERVICE_REQ_ID == o.ID && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)).OrderBy(q => q.INTRUCTION_TIME).ToList();
                var firstExamServiceReq = srExam.FirstOrDefault();
                if (firstExamServiceReq != null)
                {
                    rdo.START_TIME = firstExamServiceReq.START_TIME;
                    rdo.INTRUCTION_TIME = firstExamServiceReq.INTRUCTION_TIME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        //private int DiffTypeTime(List<HIS_SERVICE_REQ> sr, List<HIS_SERE_SERV> ss)
        //{
        //    int result = 0;
        //    try
        //    {
        //        foreach (var item in sr)
        //        {
        //            var serviceReqTest = listSample.FirstOrDefault(o => o.SERVICE_REQ_ID == item.ID);
        //            if (serviceReqTest != null)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(serviceReqTest.CREATE_TIME, serviceReqTest.RESULT_TIME, Calculation.UnitDifferenceTime.SECOND);
        //            }
        //            else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK && item.FINISH_TIME.HasValue)
        //            {
        //                var treatment = listHisTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
        //                if (treatment != null)
        //                {
        //                    //Inventec.Common.Logging.LogSystem.Info("treatment.out_time,finish_time: " + treatment.OUT_TIME + " - " + item.FINISH_TIME);
        //                    result += Inventec.Common.DateTime.Calculation.DifferenceTime(treatment.OUT_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //                }
        //            }
        //            else if (item.START_TIME.HasValue && item.FINISH_TIME.HasValue)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.START_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}
        //private int DiffGroupTime(List<HIS_SERVICE_REQ> sr, List<HIS_SERE_SERV> ss)
        //{
        //    int result = 0;
        //    try
        //    {
        //        foreach (var item in sr)
        //        {
        //            var serviceReqTest = listSample.FirstOrDefault(o => o.SERVICE_REQ_ID == item.ID);
        //            if (serviceReqTest != null)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(serviceReqTest.CREATE_TIME, serviceReqTest.RESULT_TIME, Calculation.UnitDifferenceTime.SECOND);
        //            }
        //            else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK && item.FINISH_TIME.HasValue)
        //            {
        //                var treatment = listHisTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
        //                if (treatment != null)
        //                {
        //                    //Inventec.Common.Logging.LogSystem.Info("treatment.out_time,finish_time: " + treatment.OUT_TIME + " - " + item.FINISH_TIME);
        //                    result += Inventec.Common.DateTime.Calculation.DifferenceTime(treatment.OUT_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //                }
        //            }
        //            else if (item.START_TIME.HasValue && item.FINISH_TIME.HasValue)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.START_TIME.Value, item.FINISH_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}


        //private int DiffWaitTypeTime(List<HIS_SERVICE_REQ> sr, List<HIS_SERE_SERV> ss)
        //{
        //    int result = 0;
        //    try
        //    {
        //        foreach (var item in sr)
        //        {
        //            if (item.START_TIME.HasValue)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.INTRUCTION_TIME, item.START_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}
        //private int DiffWaitGroupTime(List<HIS_SERVICE_REQ> sr, List<HIS_SERE_SERV> ss)
        //{
        //    int result = 0;
        //    try
        //    {
        //        foreach (var item in sr)
        //        {
        //            if (item.START_TIME.HasValue)
        //            {
        //                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.INTRUCTION_TIME, item.START_TIME.Value, Calculation.UnitDifferenceTime.SECOND);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}

        private string ServiceTypeCode(long ServiceTypeId)
        {
            string result = "";
            try
            {
                result = ((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == ServiceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.IN_TIME_FROM ?? CastFilter.OUT_TIME_FROM ?? CastFilter.FEE_LOCK_TIME_FROM ?? 0));

                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.IN_TIME_TO ?? CastFilter.OUT_TIME_TO ?? CastFilter.FEE_LOCK_TIME_TO ?? 0));

                objectTag.AddObjectData(store, "ReportCLS", listRdo.Where(o => o.HAS_CLS == true).ToList());
                objectTag.AddObjectData(store, "ReportNoCLS", listRdo.Where(o => o.HAS_CLS == false).ToList());
                objectTag.AddObjectData(store, "ReportCLSNumber", listRdo.Where(o => o.HAS_CLS == true).ToList());
                objectTag.AddObjectData(store, "ReportNoCLSNumber", listRdo.Where(o => o.HAS_CLS == false).ToList());
                objectTag.SetUserFunction(store, "Element", new RDOElement());
                objectTag.SetUserFunction(store, "Time", new RDOTime());
                dicSingleTag.Add("COUNTCLS", listRdo.Where(o => o.HAS_CLS == true).Count());
                dicSingleTag.Add("COUNTNoCLS", listRdo.Where(o => o.HAS_CLS == false).Count());

                objectTag.AddObjectData(store, "SumNoCLS", new List<Mrs00630RDO> { SumNoCLS });
                objectTag.AddObjectData(store, "SumCLS", new List<Mrs00630RDO> { SumCLS });

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
