using MOS.MANAGER.HisSereServ;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisTreatment; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisSereServBill; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisTransaction; 

namespace MRS.Processor.Mrs00436
{
    public class Mrs00436Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 

        private List<Mrs00436RDO> ListRdo = new List<Mrs00436RDO>();
        private List<V_HIS_TRANSACTION> ListBill = new List<V_HIS_TRANSACTION>();
        private List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        private List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>(); 
        private List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>(); 
        Mrs00436Filter filter = null;
        Dictionary<long, V_HIS_TRANSACTION> dicBill = new Dictionary<long, V_HIS_TRANSACTION>();
        Dictionary<long, V_HIS_SERVICE> dicService = new Dictionary<long, V_HIS_SERVICE>();
        Dictionary<string, CASHIER> dicCashier = new Dictionary<string, CASHIER>();
        Dictionary<string, BILL_OF_CASHIER> dicBillOfCashier = new Dictionary<string, BILL_OF_CASHIER>();

        private List<V_HIS_TRANSACTION> ListBillCancel = new List<V_HIS_TRANSACTION>(); 

        public Mrs00436Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00436Filter); 
        }

        protected override bool GetData()
        {
            filter = ((Mrs00436Filter)reportFilter); 
            bool result = true; 
            try
            {
                HisTransactionViewFilterQuery filtermain = new HisTransactionViewFilterQuery();
                filtermain.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                filtermain.TRANSACTION_TIME_TO = filter.TIME_TO; 
                filtermain.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT}; 
                ListBill = new HisTransactionManager(paramGet).GetView(filtermain);

                //danh sách hủy giao dịch

                HisTransactionViewFilterQuery filterCancel = new HisTransactionViewFilterQuery();
                filterCancel.CANCEL_TIME_FROM = filter.TIME_FROM;
                filterCancel.CANCEL_TIME_TO = filter.TIME_TO;
                filterCancel.IS_CANCEL = true;
                filterCancel.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                ListBillCancel = new HisTransactionManager(paramGet).GetView(filterCancel);

                if (filter.ACCOUNT_BOOK_ID != null)
                {
                    ListBill = ListBill.Where(o => o.ACCOUNT_BOOK_ID == filter.ACCOUNT_BOOK_ID).ToList();
                    ListBillCancel = ListBillCancel.Where(o => o.ACCOUNT_BOOK_ID == filter.ACCOUNT_BOOK_ID).ToList();
                }
                if (filter.CASHIER_LOGINNAME != null && filter.CASHIER_LOGINNAME != "")
                {
                    ListBill = ListBill.Where(o => o.CASHIER_LOGINNAME == filter.CASHIER_LOGINNAME).ToList();
                    ListBillCancel = ListBillCancel.Where(o => o.CANCEL_LOGINNAME == filter.CASHIER_LOGINNAME).ToList();
                }
                if (filter.LOGINNAME != null && filter.LOGINNAME != "")
                {
                    ListBill = ListBill.Where(o => o.CASHIER_LOGINNAME == filter.LOGINNAME).ToList();
                    ListBillCancel = ListBillCancel.Where(o => o.CANCEL_LOGINNAME == filter.LOGINNAME).ToList();
                }
                if (IsNotNullOrEmpty(ListBill))
                {
                    var listTreatmentId = ListBill.Select(o => o.TREATMENT_ID??0).ToList(); 
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        treatmentFilter.IDs = listIDs;
                        var treatments = new HisTreatmentManager(param).Get(treatmentFilter);
                        ListTreatment.AddRange(treatments); 
                    }
                }
                if (IsNotNullOrEmpty(ListBill))
                {
                    var listBillId = ListBill.Select(o => o.ID).ToList();
                    var skip = 0;
                    while (listBillId.Count - skip > 0)
                    {
                        var listIDs = listBillId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillViewFilterQuery sereServBillViewFilter = new HisSereServBillViewFilterQuery();
                        sereServBillViewFilter.BILL_IDs = listIDs;
                        var sereServBills = new HisSereServBillManager(param).GetView(sereServBillViewFilter);
                        ListSereServBill.AddRange(sereServBills);
                    }
                }
                HisServiceViewFilterQuery serPFilter = new HisServiceViewFilterQuery(); 
                serPFilter.IS_LEAF = false; 
                var serP = new HisServiceManager(param).GetView(serPFilter); 
                if (IsNotNullOrEmpty(serP)) ListService.AddRange(serP); 
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    var listServiceId = ListSereServBill.Select(o => o.SERVICE_ID).ToList(); 
                    var skip = 0; 
                    while (listServiceId.Count - skip > 0)
                    {
                        var listIDs = listServiceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery(); 
                        serviceFilter.IDs = listIDs; 
                        var sereServices = new HisServiceManager(param).GetView(serviceFilter); 
                        ListService.AddRange(sereServices); 
                    }
                }
                foreach (var bill in ListBill) dicBill.Add(bill.ID, bill);
                foreach (var service in ListService)
                {
                    if (dicService.ContainsKey(service.ID)) continue; 
                    dicService.Add(service.ID, service); 
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
            try
            {
                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListBill))
                {
                    var groupByCashier = ListBill.GroupBy(o => o.CASHIER_LOGINNAME).ToList();
                    foreach (var gr in groupByCashier)
                    {
                        List<V_HIS_TRANSACTION> ls = gr.ToList<V_HIS_TRANSACTION>();

                        if (!dicCashier.ContainsKey(gr.Key))
                        {
                            dicCashier[gr.Key] = new CASHIER();
                            dicCashier[gr.Key].CASHIER_LOGINNAME = gr.First().CASHIER_LOGINNAME;
                            dicCashier[gr.Key].CASHIER_USERNAME = gr.First().CASHIER_USERNAME;
                        }
                        var billIds = ls.Select(o => o.ID).ToList();
                        dicCashier[gr.Key].AMOUNT_SERVICE += ListSereServBill.Where(o => billIds.Contains(o.BILL_ID) && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.TDL_AMOUNT ?? 0);
                        dicCashier[gr.Key].PRICE_SERVICE += ListSereServBill.Where(o => billIds.Contains(o.BILL_ID) && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.PRICE);
                        dicCashier[gr.Key].PRICE_EXAM += ListSereServBill.Where(o => billIds.Contains(o.BILL_ID) && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.PRICE);
                        foreach (var item in ls)
                        {
                            string key = string.Format("{0}_{1}", gr.Key, item.ID);
                            if (!dicBillOfCashier.ContainsKey(key))
                            {
                                dicBillOfCashier[key] = new BILL_OF_CASHIER();
                                dicBillOfCashier[key].CASHIER_LOGINNAME = gr.First().CASHIER_LOGINNAME;
                                dicBillOfCashier[key].CASHIER_USERNAME = gr.First().CASHIER_USERNAME;
                                var treatment = ListTreatment.FirstOrDefault(o=>o.ID==item.TREATMENT_ID);
                                if (treatment != null)
                                {
                                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID);
                                    if (patientType != null)
                                    {
                                        dicBillOfCashier[key].PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                                        dicBillOfCashier[key].PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                                    }
                                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID);
                                    if (department != null)
                                    {
                                        dicBillOfCashier[key].DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                        dicBillOfCashier[key].DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                    }
                                }
                                dicBillOfCashier[key].CASHIER_ROOM_CODE = item.CASHIER_ROOM_CODE;
                                dicBillOfCashier[key].CASHIER_ROOM_NAME = item.CASHIER_ROOM_NAME;
                                dicBillOfCashier[key].TRANSACTION_CODE = item.TRANSACTION_CODE;
                                dicBillOfCashier[key].ACCOUNT_BOOK_CODE = item.ACCOUNT_BOOK_CODE;
                                dicBillOfCashier[key].NUM_ORDER = item.NUM_ORDER;
                                dicBillOfCashier[key].TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                            }
                            dicBillOfCashier[key].PRICE += ListSereServBill.Where(o => item.ID == o.BILL_ID).Sum(s => s.PRICE);
                        }
                    }
                }
                if (IsNotNullOrEmpty(ListBillCancel))
                {
                    var groupByCashier = ListBillCancel.GroupBy(o => o.CANCEL_LOGINNAME).ToList();
                    foreach (var gr in groupByCashier)
                    {
                        List<V_HIS_TRANSACTION> ls = gr.ToList<V_HIS_TRANSACTION>();

                        if (!dicCashier.ContainsKey(gr.Key))
                        {
                            dicCashier[gr.Key] = new CASHIER();
                            dicCashier[gr.Key].CASHIER_LOGINNAME = gr.First().CANCEL_LOGINNAME;
                            dicCashier[gr.Key].CASHIER_USERNAME = gr.First().CANCEL_USERNAME;
                        }
                        dicCashier[gr.Key].PRICE_CANCEL_BEFORE += gr.Where(o=>o.TRANSACTION_TIME<filter.TIME_FROM).Sum(s => s.AMOUNT);
                        dicCashier[gr.Key].PRICE_CANCEL_ON += gr.Where(o => o.TRANSACTION_TIME >= filter.TIME_FROM).Sum(s => s.AMOUNT);
                        foreach (var item in ls)
                        {
                            string key = string.Format("{0}_{1}", gr.Key,item.ID);
                            if (!dicBillOfCashier.ContainsKey(key))
                            {
                               dicBillOfCashier[key] = new BILL_OF_CASHIER();
                                dicBillOfCashier[key].CASHIER_LOGINNAME = gr.First().CANCEL_LOGINNAME;
                                dicBillOfCashier[key].CASHIER_USERNAME = gr.First().CANCEL_USERNAME;
                                var treatment = ListTreatment.FirstOrDefault(o=>o.ID==item.TREATMENT_ID);
                                if (treatment != null)
                                {
                                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID);
                                    if (patientType != null)
                                    {
                                        dicBillOfCashier[key].PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                                        dicBillOfCashier[key].PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                                    }
                                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID);
                                    if (department != null)
                                    {
                                        dicBillOfCashier[key].DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                        dicBillOfCashier[key].DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                    }
                                }
                                dicBillOfCashier[key].CASHIER_ROOM_CODE = item.CASHIER_ROOM_CODE;
                                dicBillOfCashier[key].CASHIER_ROOM_NAME = item.CASHIER_ROOM_NAME;
                                dicBillOfCashier[key].TRANSACTION_CODE = item.TRANSACTION_CODE;
                                dicBillOfCashier[key].ACCOUNT_BOOK_CODE = item.ACCOUNT_BOOK_CODE;
                                dicBillOfCashier[key].NUM_ORDER = item.NUM_ORDER;
                                dicBillOfCashier[key].TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                            }
                            dicBillOfCashier[key].PRICE_CANCEL_BEFORE += item.TRANSACTION_TIME < filter.TIME_FROM?item.AMOUNT:0;
                            dicBillOfCashier[key].PRICE_CANCEL_ON += item.TRANSACTION_TIME >= filter.TIME_FROM ? item.AMOUNT : 0;
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    var groupByBill = ListSereServBill.GroupBy(o => o.BILL_ID).ToList(); 
                    foreach (var gr in groupByBill)
                    {
                        List<V_HIS_SERE_SERV_BILL> ls = gr.ToList<V_HIS_SERE_SERV_BILL>(); 

                        Mrs00436RDO rdo = new Mrs00436RDO(); 
                        if (!dicBill.ContainsKey(ls.First().BILL_ID)) continue; 
                        rdo.NUM_ORDER_STR = string.Format("{0:0000000}", dicBill[ls.First().BILL_ID].NUM_ORDER); //số biên lai
                        rdo.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(dicBill[ls.First().BILL_ID].TRANSACTION_TIME); //ngày thanh toán

                        rdo.TREATMENT_CODE = dicBill[ls.First().BILL_ID].TREATMENT_CODE; 
                        rdo.PATIENT_NAME = dicBill[ls.First().BILL_ID].TDL_PATIENT_NAME; 
                        Dictionary<string, decimal> dicParent = new Dictionary<string, decimal>(); 
                        foreach (var s in ls)
                        {

                            if (!dicService.ContainsKey(s.SERVICE_ID)) continue; 
                            long? parentId = parent(s.SERVICE_ID); 
                            long? grandId = 0; 
                            while (parentId != null)
                            {
                                grandId = parentId; 
                                parentId = parent(parentId??0); 
                            }
                            if (grandId != 0)
                            {
                                if (!dicParent.ContainsKey(dicService[grandId ?? 0].SERVICE_NAME)) dicParent[dicService[grandId ?? 0].SERVICE_NAME] = 0; 
                                dicParent[dicService[grandId ?? 0].SERVICE_NAME] += s.PRICE; 
                            }
                            else 
                            {
                                if (!dicParent.ContainsKey(dicService[s.SERVICE_ID].SERVICE_TYPE_NAME)) dicParent[dicService[s.SERVICE_ID].SERVICE_TYPE_NAME] = 0; 
                                dicParent[dicService[s.SERVICE_ID].SERVICE_TYPE_NAME] += s.PRICE; 
                            }
                            rdo.TOTAL_NOT_VAT_RATIO += s.PRICE * (1 - s.VAT_RATIO); 
                            rdo.TOTAL_VAT_RATIO += s.PRICE; 
                        }
                        foreach (var type in dicParent.Keys)
                        {
                        rdo.REASON_STR+=string.Format("{0}: {1}. ",type,dicParent[type]); 
                        }
                        ListRdo.Add(rdo); 
                    }
                }


            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private long? parent(long p)
        {
            long? result = null; 
            try 
            {
                if (!dicService.ContainsKey(p)) return null; 
                var have = dicService.Where(o => o.Value.ID == dicService[p].PARENT_ID).ToList(); 
                result = have.Count>0?have.First().Key:0; 
                if (result == 0) result = null; 
            }
            catch (Exception ex)
            {
                result = null; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00436Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00436Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00436Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00436Filter)reportFilter).TIME_TO));
            }

            objectTag.AddObjectData(store, "Report", this.ListRdo.OrderBy(o => o.NUM_ORDER_STR).ToList());

            objectTag.AddObjectData(store, "BillOfCashier", this.dicBillOfCashier.Values.OrderBy(o => o.NUM_ORDER).ToList());

            objectTag.AddObjectData(store, "Cashier", this.dicCashier.Values.OrderBy(o => o.CASHIER_USERNAME).ToList());
            objectTag.AddObjectData(store, "CashierLoginname", this.dicBillOfCashier.Values.GroupBy(o => new { o.CASHIER_LOGINNAME, o.CASHIER_ROOM_CODE, o.PATIENT_TYPE_CODE }).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "CashierRoom", this.dicBillOfCashier.Values.GroupBy(o => new { o.CASHIER_ROOM_CODE, o.PATIENT_TYPE_CODE }).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "PatientType", this.dicBillOfCashier.Values.GroupBy(o => new { o.PATIENT_TYPE_CODE }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "PatientType", "CashierRoom", "PATIENT_TYPE_CODE", "PATIENT_TYPE_CODE");
            objectTag.AddRelationship(store, "CashierRoom", "CashierLoginname", new string[] { "CASHIER_ROOM_CODE", "PATIENT_TYPE_CODE" }, new string[] { "CASHIER_ROOM_CODE", "PATIENT_TYPE_CODE" });
            objectTag.AddRelationship(store, "CashierLoginname", "BillOfCashier", new string[] { "CASHIER_LOGINNAME", "CASHIER_ROOM_CODE", "PATIENT_TYPE_CODE" }, new string[] { "CASHIER_LOGINNAME", "CASHIER_ROOM_CODE", "PATIENT_TYPE_CODE" });
        }
        
    }

}
