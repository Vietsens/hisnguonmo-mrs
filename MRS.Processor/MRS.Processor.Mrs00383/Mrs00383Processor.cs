using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServTein;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00383
{
    class Mrs00383Processor : AbstractProcessor
    {
        Mrs00383Filter castFilter = null; 

        List<Mrs00383RDO> listRdo = new List<Mrs00383RDO>(); 
        Dictionary<long, Dictionary<long, Mrs00383RDO>> dicRdo = new Dictionary<long, Dictionary<long, Mrs00383RDO>>(); 

        public Mrs00383Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERVICE_REQ> listServiceReq = null; 
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = null; 
        Dictionary<string, TestIndexNumOrder> dicTestIndex = new Dictionary<string, TestIndexNumOrder>(); 

        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRety = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, List<HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<HIS_SERE_SERV>>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        Dictionary<long, List<V_HIS_SERE_SERV_TEIN>> dicSereServTein = new Dictionary<long, List<V_HIS_SERE_SERV_TEIN>>(); 

        HIS_DEPARTMENT department = null; 

        public override Type FilterType()
        {
            return typeof(Mrs00383Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00383Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00383: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery(); 
                serviceReqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM; 
                serviceReqFilter.FINISH_TIME_TO = castFilter.TIME_TO; 
                serviceReqFilter.REQUEST_DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN; 
                serviceReqFilter.SERVICE_REQ_STT_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT}; 
                listServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(serviceReqFilter); 

                HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00383"; 
                listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyFilter); 

                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID.Value); 
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00383"); 
                }
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
                if (IsNotNullOrEmpty(listServiceReq) && IsNotNullOrEmpty(listServiceRetyCat))
                {
                    CommonParam paramGet = new CommonParam(); 
                    List<TestIndexNumOrder> listTestIndex = new List<TestIndexNumOrder>(); 
                    foreach (var item in listServiceRetyCat)
                    {
                        dicServiceRety[item.SERVICE_ID] = item; 
                    }
                    List<long> listSereServId = new List<long>(); 
                    int start = 0; 
                    int count = listServiceReq.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = listServiceReq.Skip(start).Take(limit).ToList(); 

                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery(); 
                        ssFilter.SERVICE_REQ_IDs = listSub.Select(s => s.ID).ToList(); 
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).Get(ssFilter); 

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.TREATMENT_ID).Distinct().ToList(); 
                        patyAlterFilter.ORDER_DIRECTION = "DESC"; 
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME"; 
                        var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter); 
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00383"); 
                        }

                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            foreach (var item in listSereServ)
                            {
                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    continue; 
                                if (!dicServiceRety.ContainsKey(item.SERVICE_ID))
                                    continue; 
                                if (!dicSereServ.ContainsKey(item.SERVICE_REQ_ID??0))
                                    dicSereServ[item.SERVICE_REQ_ID ?? 0] = new List<HIS_SERE_SERV>(); 
                                dicSereServ[item.SERVICE_REQ_ID ?? 0].Add(item); 
                                listSereServId.Add(item.ID); 
                            }
                        }

                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            foreach (var item in listPatientTypeAlter)
                            {
                                if (dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                    continue; 
                                dicPatientTypeAlter[item.TREATMENT_ID] = item; 
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }

                    if (listSereServId.Count > 0)
                    {
                        start = 0; 
                        count = listSereServId.Count; 
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var listId = listSereServId.Skip(start).Take(limit).ToList(); 
                            HisSereServTeinViewFilterQuery ssTeinFilter = new HisSereServTeinViewFilterQuery(); 
                            ssTeinFilter.SERE_SERV_IDs = listId; 
                            var listSereServTein = new MOS.MANAGER.HisSereServTein.HisSereServTeinManager(paramGet).GetView(ssTeinFilter); 

                            if (paramGet.HasException)
                            {
                                throw new Exception("Co excetion xay ra tai DAOGET trong qua trinh lay du lieu Mrs00383"); 
                            }

                            if (IsNotNullOrEmpty(listSereServTein))
                            {
                                foreach (var sere in listSereServTein)
                                {
                                    if (!dicSereServTein.ContainsKey(sere.SERE_SERV_ID))
                                        dicSereServTein[sere.SERE_SERV_ID] = new List<V_HIS_SERE_SERV_TEIN>(); 
                                    dicSereServTein[sere.SERE_SERV_ID].Add(sere); 
                                    dicTestIndex[sere.TEST_INDEX_CODE] = new TestIndexNumOrder()
                                    {
                                        TestIndexCode = sere.TEST_INDEX_CODE,
                                        TestIndexName = sere.TEST_INDEX_NAME,
                                        NumOrder = sere.NUM_ORDER ?? 0
                                    }; 
                                }
                            }

                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        }
                    }
                    if (dicTestIndex.Count > 0)
                    {
                        var listIndex = dicTestIndex.Select(s => s.Value).OrderByDescending(o => o.NumOrder).ToList(); 
                        long index = 1; 
                        foreach (var item in listIndex)
                        {
                            item.NumOrder = index; 
                            index++; 
                        }
                    }
                    this.ProcessDataDetail(); 
                    this.ProcessListRdo(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        void ProcessDataDetail()
        {
            foreach (var serviceReq in listServiceReq)
            {
                if (!serviceReq.FINISH_TIME.HasValue)
                    continue; 
                if (serviceReq.SERVICE_REQ_STT_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                    continue; 
                if (!dicSereServ.ContainsKey(serviceReq.ID))
                {
                    //Inventec.Common.Logging.LogSystem.Info("Yeu cau khong co dich vu tuong uong" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => serviceReq.SERVICE_REQ_CODE), serviceReq.SERVICE_REQ_CODE)); 
                    continue; 
                }

                if (!dicPatientTypeAlter.ContainsKey(serviceReq.TREATMENT_ID))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin doi tuong: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => serviceReq.TREATMENT_CODE), serviceReq.TREATMENT_CODE)); 
                }
                var patyAlter = dicPatientTypeAlter[serviceReq.TREATMENT_ID]; 
                Mrs00383RDO rdo = null; 
                long DateFinish = Convert.ToInt64(serviceReq.FINISH_TIME.Value.ToString().Substring(0, 8)); 
                if (dicRdo.ContainsKey(DateFinish) && dicRdo[DateFinish].ContainsKey(serviceReq.TREATMENT_ID))
                {
                    rdo = dicRdo[DateFinish][serviceReq.TREATMENT_ID]; 
                }
                else
                {
                    rdo = new Mrs00383RDO(serviceReq); 
                    if (!dicRdo.ContainsKey(DateFinish))
                        dicRdo[DateFinish] = new Dictionary<long, Mrs00383RDO>(); 
                    dicRdo[DateFinish][serviceReq.TREATMENT_ID] = rdo; 
                }
                foreach (var sereServ in dicSereServ[serviceReq.ID])
                {
                    if (!dicSereServTein.ContainsKey(sereServ.ID))
                    {
                        Inventec.Common.Logging.LogSystem.Info("SereServ Khong co SereServTein: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sereServ), sereServ)); 
                        continue; 
                    }
                    if (!IsNotNullOrEmpty(dicSereServTein[sereServ.ID]))
                    {
                        continue;
                    }
                    foreach (var tein in dicSereServTein[sereServ.ID])
                    {
                        if (!string.IsNullOrWhiteSpace(tein.TEST_INDEX_CODE) && dicTestIndex.ContainsKey(tein.TEST_INDEX_CODE))
                        {
                            var testIndex = dicTestIndex[tein.TEST_INDEX_CODE];

                            if (testIndex != null && testIndex.NumOrder>0)
                            {
                                System.Reflection.PropertyInfo pi = typeof(Mrs00383RDO).GetProperty("VALUE_" + testIndex.NumOrder);

                                if (pi != null)
                                {
                                    pi.SetValue(rdo, tein.VALUE);

                                }
                            }
                        }
                    }
                }

                if (patyAlter.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "1"; 
                }
                else
                {
                    rdo.IS_BHYT = "0"; 
                }
            }
        }

        void ProcessListRdo()
        {
            try
            {
                if (dicRdo.Count > 0)
                {
                    foreach (var dic in dicRdo)
                    {
                        listRdo.AddRange(dic.Value.Select(s => s.Value).ToList()); 
                    }

                    listRdo = listRdo.OrderBy(o => o.RESULT_DATE).ThenBy(o => o.TREATMENT_CODE).ToList(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (dicTestIndex.Count > 0)
                {
                    foreach (var dic in dicTestIndex)
                    {
                        dicSingleTag.Add("INDEX_NAME_" + dic.Value.NumOrder, dic.Value.TestIndexName); 
                    }
                }
                if (department != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
                }
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                objectTag.SetUserFunction(store, "MergeManyCellFunc", new MergeCellMany()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

    class TestIndexNumOrder
    {
        public string TestIndexCode { get;  set;  }
        public string TestIndexName { get;  set;  }
        public long NumOrder { get;  set;  }
    }

    class MergeCellMany : FlexCel.Report.TFlexCelUserFunction
    {
        public MergeCellMany() { }

        long DateResult; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            try
            {
                long date = Convert.ToInt64(parameters[0]); 

                if (DateResult == date)
                {
                    return true; 
                }
                else
                {
                    DateResult = date; 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex); 
            }

            return false; 
        }
    }
}
