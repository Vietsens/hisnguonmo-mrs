using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00657
{
    class Mrs00657Processor : AbstractProcessor
    {
        Mrs00657Filter castFilter = null;
        List<D_HIS_SERE_SERV_BILL> ListSereServBill = new List<D_HIS_SERE_SERV_BILL>();
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        Dictionary<long, HIS_SERVICE> DicService = new Dictionary<long, HIS_SERVICE>();
        string DSA = "657_DSA";//stt: 4
        string HH = "657_HH";//stt: 10
        string CT = "657_CT";//stt: 12
        string MRI = "657_MRI";//stt: 13
        string VLTL = "657_VLTL";//stt: 14
        string SP = "657_SP";//stt: 15
        string XQ = "657_XQ";//stt: 7

        string ReportType = "";

        List<Mrs00657RDO> ListRdo = new List<Mrs00657RDO>();
        List<Mrs00657RDO> ListRdoOther = new List<Mrs00657RDO>();
        List<Mrs00657RDO> ListRdoDetail = new List<Mrs00657RDO>();
        List<Mrs00657RDO> ListRdoOtherDetail = new List<Mrs00657RDO>();

        List<HIS_DEPARTMENT> DEPARTMENTs = new List<HIS_DEPARTMENT>();
        List<V_HIS_ROOM> HisRooms = new List<V_HIS_ROOM>();

        public Mrs00657Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            ReportType = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00657Filter);
        }

        //tach mrs 330
        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00657Filter)reportFilter);
                CommonParam paramGet = new CommonParam();
                ListSereServBill = new ManagerSql().GetSSB(castFilter);

                if (IsNotNullOrEmpty(MANAGER.Config.HisDepartmentCFG.DEPARTMENTs))
                {
                    DEPARTMENTs.AddRange(MANAGER.Config.HisDepartmentCFG.DEPARTMENTs);
                }

                if (IsNotNullOrEmpty(MANAGER.Config.HisRoomCFG.HisRooms))
                {
                    HisRooms.AddRange(MANAGER.Config.HisRoomCFG.HisRooms);
                }

                //thanh toán 2 sổ sẽ tạo ra th 1 ss có 2 ssBill . sql sẽ trả về 2 dòng ss. lọc trùng
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    Dictionary<long, D_HIS_SERE_SERV_BILL> dic = new Dictionary<long, D_HIS_SERE_SERV_BILL>();
                    foreach (var item in ListSereServBill)
                    {
                        if (!dic.ContainsKey(item.ID))
                        {
                            dic[item.ID] = item;
                        }
                    }

                    ListSereServBill = dic.Values.ToList();
                }

                HisServiceRetyCatViewFilterQuery retyCatFilter = new HisServiceRetyCatViewFilterQuery();
                retyCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                retyCatFilter.IS_ACTIVE = 1;
                ListServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(retyCatFilter);

                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                reportTypeCatFilter.IS_ACTIVE = 1;
                ListReportTypeCat = new HisReportTypeCatManager(paramGet).Get(reportTypeCatFilter);

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                var ListService = new HisServiceManager(paramGet).Get(serviceFilter);

                if (IsNotNullOrEmpty(ListService))
                {
                    DicService = ListService.ToDictionary(o => o.ID, o => o);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    ListRdo = new List<Mrs00657RDO>();
                    ListRdoDetail = new List<Mrs00657RDO>();

                    if (ListSereServBill.Exists(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV && !o.PRIMARY_PATIENT_TYPE_ID.HasValue))
                    {
                        var dicPaty = MANAGER.Config.HisServicePatyCFG.DicServicePaty;
                        var lstPaty = MANAGER.Config.HisServicePatyCFG.DATAs;
                    }

                    List<Thread> threadData = new List<Thread>();
                    var maxReq = ListSereServBill.Count / 5 + 1;
                    Inventec.Common.Logging.LogSystem.Info(maxReq + "/" + ListSereServBill.Count);
                    int skip = 0;
                    while (ListSereServBill.Count - skip > 0)
                    {
                        var datas = ListSereServBill.Skip(skip).Take(maxReq).ToList();
                        skip += maxReq;
                        Thread hein = new Thread(ProcessListData);
                        hein.Start(datas);
                        threadData.Add(hein);
                    }

                    for (int i = 0; i < threadData.Count; i++)
                    {
                        threadData[i].Join();
                        Thread.Sleep(100);
                    }
                    //ProcessRdo(ListSereServBill);

                    ProcessDataTotal();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessDataTotal()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.GroupBy(g => g.TYPE_ID).Select(s => new Mrs00657RDO()
                    {
                        TYPE_ID = s.First().TYPE_ID,
                        ROW_POS = s.First().ROW_POS,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        TOTAL_FEE_PRICE = s.Sum(o => o.TOTAL_FEE_PRICE),
                        TOTAL_HEIN_PRICE = s.Sum(o => o.TOTAL_HEIN_PRICE),
                        TOTAL_PARTIENT_PRICE = s.Sum(o => o.TOTAL_PARTIENT_PRICE),
                        TOTAL_PRICE = s.Sum(o => o.TOTAL_PRICE),
                        TOTAL_SERVICE_PRICE = s.Sum(o => o.TOTAL_SERVICE_PRICE),
                        DISCOUNT = s.Sum(o => o.DISCOUNT ?? 0)
                    }).ToList();
                }

                if (IsNotNullOrEmpty(ListRdoOther))
                {
                    ListRdoOther = ListRdoOther.GroupBy(g => g.TYPE_ID).Select(s => new Mrs00657RDO()
                    {
                        TYPE_ID = s.First().TYPE_ID,
                        ROW_POS = s.First().ROW_POS,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        TOTAL_FEE_PRICE = s.Sum(o => o.TOTAL_FEE_PRICE),
                        TOTAL_HEIN_PRICE = s.Sum(o => o.TOTAL_HEIN_PRICE),
                        TOTAL_PARTIENT_PRICE = s.Sum(o => o.TOTAL_PARTIENT_PRICE),
                        TOTAL_PRICE = s.Sum(o => o.TOTAL_PRICE),
                        TOTAL_SERVICE_PRICE = s.Sum(o => o.TOTAL_SERVICE_PRICE),
                        DISCOUNT = s.Sum(o => o.DISCOUNT ?? 0)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                ListRdo.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListData(object data)
        {
            try
            {
                if (data != null)
                {
                    ProcessRdo((List<D_HIS_SERE_SERV_BILL>)data);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessRdo(List<D_HIS_SERE_SERV_BILL> listSereServBill)
        {
            try
            {
                if (IsNotNullOrEmpty(listSereServBill))
                {
                    if (IsNotNullOrEmpty(ListServiceRetyCat))
                    {
                        var grCategory = ListServiceRetyCat.GroupBy(o => o.REPORT_TYPE_CAT_ID).ToList();
                        foreach (var grService in grCategory)
                        {
                            if (!IsNotNullOrEmpty(listSereServBill)) break;

                            var retyCat = ListReportTypeCat.FirstOrDefault(o => o.ID == grService.First().REPORT_TYPE_CAT_ID);
                            if (retyCat == null) continue;

                            var lstSereServ = listSereServBill.Where(o => grService.Select(s => s.SERVICE_ID).Contains(o.SERVICE_ID)).ToList();
                            if (IsNotNullOrEmpty(lstSereServ))
                            {
                                foreach (var item in lstSereServ)
                                {
                                    Mrs00657RDO rdo = new Mrs00657RDO();

                                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00657RDO>(rdo, item);

                                    rdo.TYPE_ID = retyCat.ID;
                                    rdo.SERVICE_TYPE_NAME = retyCat.CATEGORY_NAME;
                                    rdo.ROW_POS = retyCat.NUM_ORDER ?? 9999;

                                    rdo.TDL_SERVICE_TYPE_NAME = item.SERVICE_TYPE_NAME;
                                    rdo.END_DEPARTMENT_NAME = (DEPARTMENTs.FirstOrDefault(o => o.ID == (item.END_DEPARTMENT_ID ?? 0)) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                    rdo.TDL_EXECUTE_DEPARTMENT_NAME = (DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                    rdo.TDL_REQUEST_DEPARTMENT_NAME = (DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                    rdo.TDL_EXECUTE_ROOM_NAME = (HisRooms.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                                    rdo.TDL_REQUEST_ROOM_NAME = (HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                                    if (item.SERVICE_PARENT_ID.HasValue && DicService.ContainsKey(item.SERVICE_PARENT_ID.Value))
                                    {
                                        rdo.PARENT_SERVICE_NAME = DicService[item.SERVICE_PARENT_ID.Value].SERVICE_NAME;
                                    }

                                    ProcessDataPrice(item, rdo);
                                    lock (ListRdoDetail)
                                    {
                                        ListRdoDetail.Add(rdo);
                                    }
                                    lock (ListRdo)
                                    {
                                        ListRdo.Add(rdo);
                                    }
                                }

                                //bo cac dich vu duoc cau hinh tranh tinh tien 2 lan.
                                listSereServBill = listSereServBill.Where(o => !lstSereServ.Select(s => s.ID).Contains(o.ID)).ToList();
                            }
                            else
                            {
                                Mrs00657RDO rdo = new Mrs00657RDO();
                                rdo.TYPE_ID = retyCat.ID;
                                rdo.SERVICE_TYPE_NAME = retyCat.CATEGORY_NAME;
                                rdo.ROW_POS = retyCat.NUM_ORDER ?? 9999;
                                lock (ListRdo)
                                {
                                    ListRdo.Add(rdo);
                                }
                            }
                        }
                    }

                    //if (IsNotNullOrEmpty(listSereServBill))
                    //{
                    //    var grServiceType = listSereServBill.GroupBy(o => o.TDL_SERVICE_TYPE_ID).ToList();
                    //    foreach (var serviceType in grServiceType)
                    //    {
                    //        if (!IsNotNullOrEmpty(listSereServBill)) break;

                    //        Mrs00657RDO rdo = new Mrs00657RDO();
                    //        rdo.TYPE_ID = serviceType.First().TDL_SERVICE_TYPE_ID;
                    //        rdo.SERVICE_TYPE_NAME = serviceType.First().SERVICE_TYPE_NAME;
                    //        rdo.ROW_POS = serviceType.First().NUM_ORDER ?? 0;

                    //        if (serviceType.First().TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                    //            || serviceType.First().TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                    //        {
                    //            rdo.ROW_POS = 3;

                    //            var rdoOther = ListRdo.FirstOrDefault(o => o.ROW_POS == 3);
                    //            if (rdoOther != null)
                    //            {
                    //                rdo.SERVICE_TYPE_NAME = "PTTT";
                    //                rdo.TOTAL_FEE_PRICE += rdoOther.TOTAL_FEE_PRICE;
                    //                rdo.TOTAL_HEIN_PRICE += rdoOther.TOTAL_HEIN_PRICE;
                    //                rdo.TOTAL_PARTIENT_PRICE += rdoOther.TOTAL_PARTIENT_PRICE;
                    //                rdo.TOTAL_PRICE += rdoOther.TOTAL_PRICE;
                    //                rdo.TOTAL_SERVICE_PRICE += rdoOther.TOTAL_SERVICE_PRICE;
                    //                ListRdo.Remove(rdoOther);
                    //            }
                    //        }

                    //        if (rdo.ROW_POS > 0)
                    //        {
                    //            ProcessDataPrice(serviceType.ToList(), rdo);
                    //            ListRdoOther.Add(rdo);
                    //            //bo cac dich vu duoc cau hinh tranh tinh tien 2 lan.
                    //            listSereServBill = listSereServBill.Where(o => !serviceType.Select(s => s.ID).Contains(o.ID)).ToList();
                    //        }
                    //    }
                    //}

                    //Con lai vao khac
                    if (IsNotNullOrEmpty(listSereServBill))
                    {
                        foreach (var item in listSereServBill)
                        {
                            Mrs00657RDO rdo = new Mrs00657RDO();

                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00657RDO>(rdo, item);

                            rdo.TYPE_ID = -1;
                            rdo.SERVICE_TYPE_NAME = "KHÁC";
                            rdo.ROW_POS = 9999;

                            rdo.TDL_SERVICE_TYPE_NAME = item.SERVICE_TYPE_NAME;
                            rdo.END_DEPARTMENT_NAME = (DEPARTMENTs.FirstOrDefault(o => o.ID == (item.END_DEPARTMENT_ID ?? 0)) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.TDL_EXECUTE_DEPARTMENT_NAME = (DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.TDL_REQUEST_DEPARTMENT_NAME = (DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.TDL_EXECUTE_ROOM_NAME = (HisRooms.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                            rdo.TDL_REQUEST_ROOM_NAME = (HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                            ProcessDataPrice(item, rdo);
                            ListRdoOtherDetail.Add(rdo);
                            ListRdoOther.Add(rdo);
                        }

                        //Mrs00657RDO rdo = new Mrs00657RDO();
                        //rdo.SERVICE_TYPE_NAME = "KHÁC";
                        //rdo.ROW_POS = 19;
                        //ProcessDataPrice(listSereServBill, rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDataPrice(D_HIS_SERE_SERV_BILL sereServ, Mrs00657RDO rdo)
        {
            try
            {
                if (IsNotNull(sereServ))
                {
                    decimal PRICE_FEE = 0;
                    decimal PRICE_SERVICE = 0;

                    decimal TotalPrice = sereServ.VIR_PRICE ?? 0;

                    if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV && !sereServ.PRIMARY_PATIENT_TYPE_ID.HasValue)
                    {
                        List<V_HIS_SERVICE_PATY> patys = new List<V_HIS_SERVICE_PATY>();
                        if (MANAGER.Config.HisServicePatyCFG.DicServicePaty != null && MANAGER.Config.HisServicePatyCFG.DicServicePaty.ContainsKey(sereServ.SERVICE_ID))
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DicServicePaty[sereServ.SERVICE_ID];
                        }
                        else
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DATAs;
                        }

                        var currentPaty = MOS.ServicePaty.ServicePatyUtil.GetApplied(patys, this.branch_id, null, sereServ.TDL_REQUEST_ROOM_ID, sereServ.TDL_REQUEST_DEPARTMENT_ID, sereServ.TDL_INTRUCTION_TIME, sereServ.TREATMENT_IN_TIME, sereServ.SERVICE_ID, sereServ.TREATMENT_TDL_PATIENT_TYPE_ID ?? 0, null);
                        if (currentPaty != null)
                        {
                            PRICE_FEE = currentPaty.PRICE * (1 + currentPaty.VAT_RATIO);
                        }
                    }
                    else
                    {
                        if (sereServ.HEIN_LIMIT_PRICE.HasValue && (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH))
                        {
                            PRICE_FEE = sereServ.HEIN_LIMIT_PRICE.Value;
                        }
                        else if (sereServ.LIMIT_PRICE.HasValue)
                        {
                            PRICE_FEE = sereServ.LIMIT_PRICE.Value;
                        }
                        else if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                            || sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            PRICE_FEE = TotalPrice;
                        }
                    }

                    PRICE_SERVICE = TotalPrice - PRICE_FEE;

                    //Y nghia la co chech lech thi tach ra
                    if (PRICE_SERVICE == 0 && sereServ.HEIN_LIMIT_PRICE.HasValue && TotalPrice > sereServ.HEIN_LIMIT_PRICE)
                    {
                        //khi có chênh lệch thì phần chênh lệch chỉ dồn sang khi là dịch vụ khám hoặc giường.
                        if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            PRICE_FEE = sereServ.HEIN_LIMIT_PRICE.Value;
                            PRICE_SERVICE = TotalPrice - PRICE_FEE;
                        }
                    }

                    rdo.TOTAL_PARTIENT_PRICE += (PRICE_FEE * sereServ.AMOUNT) - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                    rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                    rdo.TOTAL_FEE_PRICE += PRICE_FEE * sereServ.AMOUNT;
                    rdo.TOTAL_SERVICE_PRICE += PRICE_SERVICE * sereServ.AMOUNT;
                    rdo.TOTAL_PRICE += (PRICE_FEE + PRICE_SERVICE) * sereServ.AMOUNT;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDataPrice(List<D_HIS_SERE_SERV_BILL> lstSereServ, Mrs00657RDO rdo)
        {
            if (IsNotNullOrEmpty(lstSereServ))
            {
                foreach (var item in lstSereServ)
                {
                    decimal PRICE_FEE = 0;
                    decimal PRICE_SERVICE = 0;

                    decimal TotalPrice = item.VIR_PRICE ?? 0;

                    if (item.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV && !item.PRIMARY_PATIENT_TYPE_ID.HasValue)
                    {
                        List<V_HIS_SERVICE_PATY> patys = new List<V_HIS_SERVICE_PATY>();
                        if (MANAGER.Config.HisServicePatyCFG.DicServicePaty != null && MANAGER.Config.HisServicePatyCFG.DicServicePaty.ContainsKey(item.SERVICE_ID))
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DicServicePaty[item.SERVICE_ID];
                        }
                        else
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DATAs;
                        }

                        var currentPaty = MOS.ServicePaty.ServicePatyUtil.GetApplied(patys, this.branch_id, null, item.TDL_REQUEST_ROOM_ID, item.TDL_REQUEST_DEPARTMENT_ID, item.TDL_INTRUCTION_TIME, item.TREATMENT_IN_TIME, item.SERVICE_ID, item.TREATMENT_TDL_PATIENT_TYPE_ID ?? 0, null);
                        if (currentPaty != null)
                        {
                            PRICE_FEE = currentPaty.PRICE * (1 + currentPaty.VAT_RATIO);
                        }
                    }
                    else
                    {
                        if (item.HEIN_LIMIT_PRICE.HasValue && (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH))
                        {
                            PRICE_FEE = item.HEIN_LIMIT_PRICE.Value;
                        }
                        else if (item.LIMIT_PRICE.HasValue)
                        {
                            PRICE_FEE = item.LIMIT_PRICE.Value;
                        }
                        else if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                            || item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            PRICE_FEE = TotalPrice;
                        }
                    }

                    PRICE_SERVICE = TotalPrice - PRICE_FEE;

                    //Y nghia la co chech lech thi tach ra
                    if (PRICE_SERVICE == 0 && item.HEIN_LIMIT_PRICE.HasValue && TotalPrice > item.HEIN_LIMIT_PRICE)
                    {
                        //khi có chênh lệch thì phần chênh lệch chỉ dồn sang khi là dịch vụ khám hoặc giường.
                        if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            PRICE_FEE = item.HEIN_LIMIT_PRICE.Value;
                            PRICE_SERVICE = TotalPrice - PRICE_FEE;
                        }
                    }

                    rdo.TOTAL_PARTIENT_PRICE += (PRICE_FEE * item.AMOUNT) - (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                    rdo.TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE ?? 0;
                    rdo.TOTAL_FEE_PRICE += PRICE_FEE * item.AMOUNT;
                    rdo.TOTAL_SERVICE_PRICE += PRICE_SERVICE * item.AMOUNT;
                    rdo.TOTAL_PRICE += (PRICE_FEE + PRICE_SERVICE) * item.AMOUNT;
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }
                else if (castFilter.OUT_TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }
                else if (castFilter.OUT_TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
                }

                ListRdo = ListRdo.OrderBy(o => o.ROW_POS).ThenBy(o => o.SERVICE_TYPE_NAME).ToList();
                ListRdoDetail = ListRdoDetail.OrderBy(o => o.ROW_POS).ThenBy(o => o.TDL_TREATMENT_CODE).ThenBy(o => o.TDL_SERVICE_CODE).ToList();

                dicSingleTag.Add("TOTAL_FEE_PRICE", ListRdo.Sum(s => s.TOTAL_FEE_PRICE));
                dicSingleTag.Add("TOTAL_HEIN_PRICE", ListRdo.Sum(s => s.TOTAL_HEIN_PRICE));
                dicSingleTag.Add("TOTAL_PARTIENT_PRICE", ListRdo.Sum(s => s.TOTAL_PARTIENT_PRICE));
                dicSingleTag.Add("TOTAL_PRICE", ListRdo.Sum(s => s.TOTAL_PRICE));
                dicSingleTag.Add("TOTAL_SERVICE_PRICE", ListRdo.Sum(s => s.TOTAL_SERVICE_PRICE));
                dicSingleTag.Add("DISCOUNT", ListRdo.Sum(s => s.DISCOUNT));

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
                objectTag.AddRelationship(store, "Report", "ReportDetail", "TYPE_ID", "TYPE_ID");

                objectTag.AddObjectData(store, "ReportOther", ListRdoOther);
                objectTag.AddObjectData(store, "ReportOtherDetail", ListRdoOtherDetail);
                objectTag.AddRelationship(store, "ReportOther", "ReportOtherDetail", "TYPE_ID", "TYPE_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
