using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServBill;
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

namespace MRS.Processor.Mrs00413
{
    class Mrs00413Processor : AbstractProcessor
    {
        Mrs00413Filter castFilter = null;
        List<Mrs00413RDO> listRdo = new List<Mrs00413RDO>();

        public Mrs00413Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();
        List<HIS_CASHOUT> listCashOuts = new List<HIS_CASHOUT>();
        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_SERVICE> listServices = new List<V_HIS_SERVICE>();
        List<V_HIS_SERVICE> listServiceParents = new List<V_HIS_SERVICE>();
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>();

        public override Type FilterType()
        {
            return typeof(Mrs00413Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00413Filter)this.reportFilter;

                var skip = 0;

                // Dich vu nop quy
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
                    listTransactions.AddRange(listTransaction);
                }

                var listTransactionIds = listTransactions.Select(s => s.ID).ToList();

                listBills = listTransactions.Where(s => s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                var listBillIds = listBills.Select(s => s.ID).ToList();
                skip = 0;
                while (listBillIds.Count - skip > 0)
                {
                    var listBillId = listBillIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery();
                    sereServBillFilter.BILL_IDs = listBillId;
                    var listSereServBill = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(sereServBillFilter);
                    var listSereServIdByBills = listSereServBill.Select(s => s.SERE_SERV_ID).ToList();
                    var skip_ = 0;
                    while (listSereServIdByBills.Count - skip_ > 0)
                    {
                        var listSereServId = listSereServIdByBills.Skip(skip_).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip_ += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                        sereServFilter.IDs = listSereServId;
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter);
                        listSereServs.AddRange(listSereServ);
                    }
                    listSereServBills.AddRange(listSereServBill);
                }

                var listTreatmentIds = listSereServs.Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList();
                skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listTreatmentId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.IDs = listTreatmentId;
                    var listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);
                    listTreatments.AddRange(listTreatment);
                }

                var listServiceIds = listSereServs.Select(s => s.SERVICE_ID).Distinct().ToList();
                skip = 0;
                while (listServiceIds.Count - skip > 0)
                {
                    var listServiceId = listServiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                    serviceFilter.IDs = listServiceId;
                    var listService = new MOS.MANAGER.HisService.HisServiceManager(paramGet).GetView(serviceFilter);
                    listServices.AddRange(listService);
                }

                var listConcreteIds = listServices.Select(s => s.ID).Distinct().ToList();
                skip = 0;
                while (listConcreteIds.Count - skip > 0)
                {
                    var listConcreteId = listConcreteIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceViewFilterQuery serviceParentFilter = new HisServiceViewFilterQuery();
                    serviceParentFilter.IDs = listConcreteId;
                    var listServiceParent = new MOS.MANAGER.HisService.HisServiceManager(paramGet).GetView(serviceParentFilter);
                    listServiceParents.AddRange(listServiceParent);
                }
                var TreatmentIds = listSereServs.Select(o => o.TDL_TREATMENT_ID.Value).Distinct().ToList();
                List<V_HIS_SERVICE_REQ> ServiceReqSub = new List<V_HIS_SERVICE_REQ>();
                if (IsNotNullOrEmpty(TreatmentIds))
                {
                    var sk = 0;
                    while (TreatmentIds.Count - sk > 0)
                    {
                        var limit = TreatmentIds.Skip(sk).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        sk = sk + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var reqFilter = new HisServiceReqViewFilterQuery()
                        {
                            TREATMENT_IDs = limit
                        };
                        var listServiceReqSub = new MOS.MANAGER.HisServiceReq.HisServiceReqManager().GetView(reqFilter);
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
                        if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0)) continue;
                        req = dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0];
                        var service = listServices.Where(w => w.ID == sereServ.SERVICE_ID);
                        var serviceParent = listServiceParents.Where(w => w.ID == service.FirstOrDefault().PARENT_ID).ToList();
                        var treatment = listTreatments.Where(w => w.ID == sereServ.TDL_TREATMENT_ID);
                        var sereServBills = listSereServBills.Where(w => w.SERE_SERV_ID == sereServ.ID).ToList();
                        foreach (var sereServBill in sereServBills)
                        {
                            var bill = listBills.Where(w => w.ID == sereServBill.BILL_ID);
                            var transaction = listTransactions.Where(w => w.ID == bill.FirstOrDefault().ID);
                            var cashOut = listCashOuts.Where(w => w.ID == transaction.FirstOrDefault().CASHOUT_ID);
                            Mrs00413RDO rdo = new Mrs00413RDO();
                            rdo.PATIENT_CODE = req.TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = req.TDL_PATIENT_NAME;
                            rdo.DEPARTMENT = sereServ.EXECUTE_ROOM_NAME;
                            rdo.BILL_NUMBER = sereServBill.BILL_ID;
                            rdo.SERVICE_TYPE_NAME = sereServ.SERVICE_TYPE_NAME;
                            if (IsNotNullOrEmpty(serviceParent))
                            {
                                rdo.SERVICE_CONCRETE_NAME = serviceParent.FirstOrDefault().SERVICE_NAME;
                            }
                            rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                            rdo.SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FirstOrDefault().IN_TIME);
                            rdo.BILL_CREATE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServBill.CREATE_TIME ?? 0);
                            rdo.CASHOUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(cashOut.FirstOrDefault().CREATE_TIME ?? 0);
                            rdo.AMOUNT = sereServ.AMOUNT;
                            rdo.PRICE = rdo.PRICE;
                            rdo.TOTAL_MONEY = sereServ.VIR_TOTAL_PRICE;
                            rdo.HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE;
                            rdo.PATIENT_PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE;
                            rdo.USER_NAME = bill.FirstOrDefault().CASHIER_LOGINNAME;

                            listRdo.Add(rdo);
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
