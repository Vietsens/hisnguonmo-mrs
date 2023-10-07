using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisCashout;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisServiceReq; 
 

namespace MRS.Processor.Mrs00411
{
    class Mrs00411Processor : AbstractProcessor
    {
        Mrs00411Filter castFilter = null; 
        List<Mrs00411RDO> listRdo = new List<Mrs00411RDO>(); 

        public Mrs00411Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>(); 
        List<V_HIS_SERE_SERV_BILL> listSereServBills1 = new List<V_HIS_SERE_SERV_BILL>(); 
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>(); 
        List<HIS_CASHOUT> listCashOuts = new List<HIS_CASHOUT>(); 
        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        public override Type FilterType()
        {
            return typeof(Mrs00411Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00411Filter)this.reportFilter; 

                var skip = 0; 

                // Dich vu MRI
                HisServiceRetyCatViewFilterQuery retyCastFilter = new HisServiceRetyCatViewFilterQuery(); 
                retyCastFilter.REPORT_TYPE_CODE__EXACT = "MRS00411"; 
                var listServiceRetCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(retyCastFilter); 

                var listServiceIds = listServiceRetCats.Select(s => s.SERVICE_ID).ToList(); 

                // Dich vu nop quy
                //rut tien
                HisCashoutFilterQuery cashOutFilter = new HisCashoutFilterQuery(); 
                cashOutFilter.CREATE_TIME_FROM = this.castFilter.TIME_FROM; 
                cashOutFilter.CREATE_TIME_TO = this.castFilter.TIME_TO; 
                listCashOuts = new MOS.MANAGER.HisCashout.HisCashoutManager(paramGet).Get(cashOutFilter); 

                var listCashOutIds = listCashOuts.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (listCashOutIds.Count - skip > 0)
                {
                    var listCashOutId = listCashOutIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery(); 
                    transactionFilter.CASHOUT_IDs = listCashOutId; 

                    var listTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(transactionFilter); 
                    listTransactions.AddRange(listTransaction); //giao dich rut tien
                }

                listBills = listTransactions.Where(s => s.TRANSACTION_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList(); 

                var listBillIds = listBills.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (listBillIds.Count - skip > 0)
                {
                    var listBillId = listBillIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery(); 
                    sereServBillFilter.BILL_IDs = listBillId; 
                    var listSereServBill = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(sereServBillFilter); 
                    var listSereServIdByBills1 = listSereServBill.Select(s => s.SERE_SERV_ID).ToList(); 
                    var skip_ = 0; 
                    while (listSereServIdByBills1.Count - skip_ > 0)
                    {
                        var listSereServId = listSereServIdByBills1.Skip(skip_).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip_ += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery sereServFilter1 = new HisSereServViewFilterQuery(); 
                        sereServFilter1.IDs = listSereServId; 
                        sereServFilter1.SERVICE_IDs = listServiceIds; 
                        var listSereServ1 = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter1); 
                        listSereServs.AddRange(listSereServ1); 
                    }
                    listSereServBills.AddRange(listSereServBill); //dv- phieu TT
                }

                // Dich vu chi dinh da thu tien + BH thanh toan 100% -dv- 
                HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                sereServFilter.SERVICE_IDs = listServiceIds; 
                sereServFilter.INTRUCTION_DATE_FROM = this.castFilter.TIME_FROM; 
                sereServFilter.INTRUCTION_DATE_TO = this.castFilter.TIME_TO; 
                sereServFilter.PATIENT_TYPE_ID = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 

                var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter); 

                var listSereServIdByCashOuts = listSereServs.Select(s => s.ID).ToList(); 
                listSereServ = listSereServ.Where(w => w.VIR_TOTAL_PATIENT_PRICE == 0 && !listSereServIdByCashOuts.Contains(w.ID)).ToList(); //cac dich vụ ton tien =0


                List<V_HIS_SERVICE_REQ> ServiceReqSub = new List<V_HIS_SERVICE_REQ>(); 
                if (IsNotNullOrEmpty(listSereServs))
                {
                    var listTreatmentId = listSereServs.Select(o => o.TDL_TREATMENT_ID.Value).ToList(); 
                    int sk = 0; 
                    while (listTreatmentId.Count - sk > 0)
                    {
                        var limit = listTreatmentId.Skip(sk).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        sk = sk + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var reqFilter = new HisServiceReqViewFilterQuery()
                        {
                        TREATMENT_IDs=limit
                        }; 
                        var listServiceReqSub = new HisServiceReqManager().GetView(reqFilter); 
                        ServiceReqSub.AddRange(listServiceReqSub); 
                    }
                }
                dicServiceReq = ServiceReqSub.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 
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
                if (IsNotNullOrEmpty(listSereServs))
                {
                    V_HIS_SERVICE_REQ req = null; 
                    foreach (var sereServ in listSereServs)
                    {
                        if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID??0)) continue; 
                        req = dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0]; 
                        var sereServBills = listSereServBills.Where(w => w.SERE_SERV_ID == sereServ.ID).ToList(); 

                        if (IsNotNullOrEmpty(sereServBills))
                        {
                            foreach (var sereServBill in sereServBills)
                            {
                                var bill = listBills.Where(w => w.ID == sereServBill.BILL_ID).ToList(); 
                                var cashOut = listCashOuts.Where(w => w.ID == bill.FirstOrDefault().CASHOUT_ID).ToList(); 
                                Mrs00411RDO rdo = new Mrs00411RDO(); 
                                rdo.PATIENT_CODE = req.TDL_PATIENT_CODE; 
                                rdo.PATIENT_NAME = req.TDL_PATIENT_NAME; 
                                rdo.DEPARTMENT = sereServ.REQUEST_ROOM_NAME; 
                                if (sereServBill.TDL_BILL_TYPE_ID == null || sereServBill.TDL_BILL_TYPE_ID == MANAGER.Config.HisBillTypeCFG.bill_type_id_01)
                                {
                                    rdo.BILL_NUMBER = sereServBill.BILL_ID; 
                                }
                                if (sereServBill.TDL_BILL_TYPE_ID == MANAGER.Config.HisBillTypeCFG.bill_type_id_02)
                                {
                                    rdo.INVOICE_NUMBER = sereServBill.BILL_ID; 
                                }
                                rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                                rdo.BILL_CREATE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServBill.CREATE_TIME ?? 0); 
                                if (IsNotNullOrEmpty(cashOut))
                                {
                                    rdo.CASHOUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(cashOut.FirstOrDefault().CASHOUT_TIME); 
                                }
                                rdo.AMOUNT = sereServ.AMOUNT; 
                                rdo.PRICE = sereServ.PRICE; 
                                rdo.TOTAL_MONEY = sereServ.VIR_TOTAL_PRICE; 
                                rdo.PATIENT_PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE; 
                                rdo.HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE; 

                                listRdo.Add(rdo); 
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
