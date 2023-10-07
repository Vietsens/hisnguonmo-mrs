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

namespace MRS.Processor.Mrs00384
{
    class Mrs00384Processor : AbstractProcessor
    {
        Mrs00384Filter castFilter = null; 

        List<Mrs00384RDO> listRdo = new List<Mrs00384RDO>(); 
        Dictionary<long, Dictionary<long, Mrs00384RDO>> dicRdo = new Dictionary<long, Dictionary<long, Mrs00384RDO>>(); 
        const string CATEGORY_CODE__HH = "384HH"; 
        const string CATEGORY_CODE__ML = "384ML"; 
        public Mrs00384Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERVICE_REQ> listServiceReq = null; 
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = null; 
        Dictionary<string, TestIndexNumOrder> dicTestIndexHH = new Dictionary<string, TestIndexNumOrder>(); 
        Dictionary<string, TestIndexNumOrder> dicTestIndexML = new Dictionary<string, TestIndexNumOrder>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyHH = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyML = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        string ML_1H = null; 
        string ML_2H = null; 

        Dictionary<long, List<HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<HIS_SERE_SERV>>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        Dictionary<long, List<V_HIS_SERE_SERV_TEIN>> dicSereServTeinHH = new Dictionary<long, List<V_HIS_SERE_SERV_TEIN>>(); 
        Dictionary<long, List<V_HIS_SERE_SERV_TEIN>> dicSereServTeinML = new Dictionary<long, List<V_HIS_SERE_SERV_TEIN>>(); 

        HIS_DEPARTMENT department = null; 

        public override Type FilterType()
        {
            return typeof(Mrs00384Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00384Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00384: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery(); 
                serviceReqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM; 
                serviceReqFilter.FINISH_TIME_TO = castFilter.TIME_TO; 
                serviceReqFilter.REQUEST_DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN; 
                serviceReqFilter.SERVICE_REQ_STT_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT}; 
                listServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(serviceReqFilter); 

                HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00384"; 
                listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyFilter); 

                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID.Value); 
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00384"); 
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
                        if (item.CATEGORY_CODE == CATEGORY_CODE__HH)
                        {
                            dicServiceRetyHH[item.SERVICE_ID] = item; 
                        }
                        else if (item.CATEGORY_CODE == CATEGORY_CODE__ML)
                        {
                            dicServiceRetyML[item.SERVICE_ID] = item; 
                        }
                    }
                    List<long> listSereServIdHH = new List<long>(); 
                    List<long> listSereServIdML = new List<long>(); 
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
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00384"); 
                        }

                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            foreach (var item in listSereServ)
                            {
                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    continue; 
                                if (!(dicServiceRetyHH.ContainsKey(item.SERVICE_ID) || dicServiceRetyML.ContainsKey(item.SERVICE_ID)))
                                    continue; 
                                if (!dicSereServ.ContainsKey(item.SERVICE_REQ_ID ?? 0))
                                    dicSereServ[item.SERVICE_REQ_ID ?? 0] = new List<HIS_SERE_SERV>(); 
                                dicSereServ[item.SERVICE_REQ_ID ?? 0].Add(item); 
                                if (dicServiceRetyHH.ContainsKey(item.SERVICE_ID))
                                {
                                    listSereServIdHH.Add(item.ID); 
                                }
                                else
                                {
                                    listSereServIdML.Add(item.ID); 
                                }
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

                    if (listSereServIdHH.Count > 0)
                    {
                        start = 0; 
                        count = listSereServIdHH.Count; 
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var listId = listSereServIdHH.Skip(start).Take(limit).ToList(); 
                            HisSereServTeinViewFilterQuery ssTeinFilter = new HisSereServTeinViewFilterQuery(); 
                            ssTeinFilter.SERE_SERV_IDs = listId; 
                            var listSereServTein = new MOS.MANAGER.HisSereServTein.HisSereServTeinManager(paramGet).GetView(ssTeinFilter); 

                            if (paramGet.HasException)
                            {
                                throw new Exception("Co excetion xay ra tai DAOGET trong qua trinh lay du lieu Mrs00384"); 
                            }

                            if (IsNotNullOrEmpty(listSereServTein))
                            {
                                foreach (var sere in listSereServTein)
                                {
                                    if (!dicSereServTeinHH.ContainsKey(sere.SERE_SERV_ID))
                                        dicSereServTeinHH[sere.SERE_SERV_ID] = new List<V_HIS_SERE_SERV_TEIN>(); 
                                    dicSereServTeinHH[sere.SERE_SERV_ID].Add(sere); 
                                    dicTestIndexHH[sere.TEST_INDEX_CODE] = new TestIndexNumOrder()
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

                    if (listSereServIdML.Count > 0)
                    {
                        start = 0; 
                        count = listSereServIdML.Count; 
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var listId = listSereServIdML.Skip(start).Take(limit).ToList(); 
                            HisSereServTeinViewFilterQuery ssTeinFilter = new HisSereServTeinViewFilterQuery(); 
                            ssTeinFilter.SERE_SERV_IDs = listId; 
                            var listSereServTein = new MOS.MANAGER.HisSereServTein.HisSereServTeinManager(paramGet).GetView(ssTeinFilter); 

                            if (paramGet.HasException)
                            {
                                throw new Exception("Co excetion xay ra tai DAOGET trong qua trinh lay du lieu Mrs00384"); 
                            }

                            if (IsNotNullOrEmpty(listSereServTein))
                            {
                                foreach (var sere in listSereServTein)
                                {
                                    if (!dicSereServTeinML.ContainsKey(sere.SERE_SERV_ID))
                                        dicSereServTeinML[sere.SERE_SERV_ID] = new List<V_HIS_SERE_SERV_TEIN>(); 
                                    dicSereServTeinML[sere.SERE_SERV_ID].Add(sere); 
                                    dicTestIndexML[sere.TEST_INDEX_CODE] = new TestIndexNumOrder()
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
                    if (dicTestIndexHH.Count > 0)
                    {
                        var listIndex = dicTestIndexHH.Select(s => s.Value).OrderByDescending(o => o.NumOrder).ToList(); 
                        long index = 1; 
                        foreach (var item in listIndex)
                        {
                            item.NumOrder = index; 
                            index++; 
                        }
                    }

                    if (dicTestIndexML.Count > 0)
                    {
                        var listIndex = dicTestIndexML.Select(s => s.Value).OrderByDescending(o => o.NumOrder).ToList(); 
                        if (listIndex.Count > 0)
                        {
                            ML_1H = listIndex[0].TestIndexCode; 
                        }
                        if (listIndex.Count > 1)
                        {
                            ML_2H = listIndex[1].TestIndexCode; 
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
                Mrs00384RDO rdo = null; 
                long DateFinish = Convert.ToInt64(serviceReq.FINISH_TIME.Value.ToString().Substring(0, 8)); 
                if (dicRdo.ContainsKey(DateFinish) && dicRdo[DateFinish].ContainsKey(serviceReq.TREATMENT_ID))
                {
                    rdo = dicRdo[DateFinish][serviceReq.TREATMENT_ID]; 
                }
                else
                {
                    rdo = new Mrs00384RDO(serviceReq); 
                    if (!dicRdo.ContainsKey(DateFinish))
                        dicRdo[DateFinish] = new Dictionary<long, Mrs00384RDO>(); 
                    dicRdo[DateFinish][serviceReq.TREATMENT_ID] = rdo; 
                }

                foreach (var sereServ in dicSereServ[serviceReq.ID])
                {
                    if (!(dicSereServTeinHH.ContainsKey(sereServ.ID) || dicSereServTeinML.ContainsKey(sereServ.ID)))
                    {
                        Inventec.Common.Logging.LogSystem.Info("SereServ Khong co SereServTein: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sereServ), sereServ)); 
                        continue; 
                    }
                    if (dicSereServTeinHH.ContainsKey(sereServ.ID))
                    {
                        foreach (var tein in dicSereServTeinHH[sereServ.ID])
                        {
                            var testIndex = dicTestIndexHH[tein.TEST_INDEX_CODE]; 
                            System.Reflection.PropertyInfo pi = typeof(Mrs00384RDO).GetProperty("VALUE_" + testIndex.NumOrder); 
                            pi.SetValue(rdo, tein.VALUE); 
                        }
                    }
                    else
                    {
                        foreach (var tein in dicSereServTeinML[sereServ.ID])
                        {
                            if (tein.TEST_INDEX_CODE == ML_1H)
                            {
                                rdo.VALUE_ML_1 = tein.VALUE; 
                            }
                            else if (tein.TEST_INDEX_CODE == ML_2H)
                            {
                                rdo.VALUE_ML_2 = tein.VALUE; 
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Info("Ma chi so xet nghiem khong thuoc MauLang 1h hay 2h: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => tein), tein)); 
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
                if (dicTestIndexHH.Count > 0)
                {
                    foreach (var dic in dicTestIndexHH)
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
