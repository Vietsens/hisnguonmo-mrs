using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServDebt;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00030
{
    class Mrs00030Processor : AbstractProcessor
    {
        List<Mrs00030RDO> ListRdo = new List<Mrs00030RDO>();
        CommonParam paramGet = new CommonParam();
        HIS_BRANCH Branch = new HIS_BRANCH();
        List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV_DEBT> ListSereServDebt = new List<HIS_SERE_SERV_DEBT>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();

        List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();
        List<HIS_CASHIER_ROOM> ListCashierRoom = new List<HIS_CASHIER_ROOM>();
        List<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION> ListCurrentTransaction = new List<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION>();

        List<Mrs00030RDODetail> ListRdoDetail = new List<Mrs00030RDODetail>();
        List<Mrs00030RDODetail> ListRdoDetailGroupType = new List<Mrs00030RDODetail>();
        List<Mrs00030RDODetail> ListRdoDetailGroupCashier = new List<Mrs00030RDODetail>();
        List<long> listSereServIdNotDeposit = new List<long>();
        public Mrs00030Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00030Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00030Filter)reportFilter);
            bool result = true;
            try
            {
                if ((long)filter.BRANCH_ID != 0)
                {
                    Branch = new HisBranchManager(paramGet).GetById(filter.BRANCH_ID);

                    ListRoom = MANAGER.Config.HisRoomCFG.HisRooms.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                    if (IsNotNullOrEmpty(ListRoom))
                    {
                        var skip = 0;
                        while (ListRoom.Count - skip > 0)
                        {
                            var listIds = ListRoom.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisCashierRoomFilterQuery filterCashierRoom = new HisCashierRoomFilterQuery();
                            filterCashierRoom.ROOM_IDs = listIds.Select(o => o.ID).ToList();
                            ListCashierRoom.AddRange(new HisCashierRoomManager(paramGet).Get(filterCashierRoom));
                        }
                    }
                }

                HisTransactionViewFilterQuery filterTransaction = new HisTransactionViewFilterQuery();
                filterTransaction.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                filterTransaction.TRANSACTION_TIME_TO = filter.TIME_TO;
                filterTransaction.IS_CANCEL = false;
                ListCurrentTransaction = new HisTransactionManager().GetView(filterTransaction);

                if ((long)filter.BRANCH_ID != 0)
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => ListCashierRoom.Select(p => p.ID).Contains(o.CASHIER_ROOM_ID)).ToList();

                if (filter.CASHIER_LOGINNAME != null)
                {
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }

                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                {
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                }

                if (filter.LOGINNAME != null)
                {
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }

                if (IsNotNullOrEmpty(filter.LOGINNAMEs))
                {
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                }

                List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
                var treatmentId = ListCurrentTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(treatmentId))
                {
                    var ski = 0;
                    while (treatmentId.Count - ski > 0)
                    {
                        var listIds = treatmentId.Skip(ski).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        ski += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery FilterTreatment = new HisTreatmentFilterQuery();
                        FilterTreatment.IDs = listIds;
                        var CurrentTreatment = new HisTreatmentManager(paramGet).Get(FilterTreatment);
                        if (IsNotNullOrEmpty(CurrentTreatment))
                            listTreatment.AddRange(CurrentTreatment);
                    }
                }

                if (filter.PATIENT_TYPE_ID.HasValue)
                {
                    listTreatment = listTreatment.Where(o => o.TDL_PATIENT_TYPE_ID == filter.PATIENT_TYPE_ID).ToList();
                }

                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    listTreatment = listTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID == filter.TREATMENT_TYPE_ID).ToList();
                }

                ListCurrentTransaction = ListCurrentTransaction.Where(o => listTreatment.Select(s => s.ID).Contains(o.TREATMENT_ID ?? 0)).ToList();

                if (filter.HAS_DETAIL == true)
                {
                    var depositIds = ListCurrentTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Select(o => o.ID).ToList();
                    if (IsNotNullOrEmpty(depositIds))
                    {
                        var skip = 0;
                        while (depositIds.Count - skip > 0)
                        {
                            var listIds = depositIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServDepositFilterQuery FilterSereServDeposit = new HisSereServDepositFilterQuery() { DEPOSIT_IDs = listIds };
                            var CurrentSereServDeposit = new HisSereServDepositManager(paramGet).Get(FilterSereServDeposit);
                            if (IsNotNullOrEmpty(CurrentSereServDeposit))
                                ListSereServDeposit.AddRange(CurrentSereServDeposit);
                        }
                    }
                    var billIds = ListCurrentTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(o => o.ID).ToList();
                    if (IsNotNullOrEmpty(billIds))
                    {
                        var skip = 0;
                        while (billIds.Count - skip > 0)
                        {
                            var listIds = billIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery() { BILL_IDs = listIds };
                            var currentSereServBill = new HisSereServBillManager(paramGet).Get(sereServBillFilter);
                            if (IsNotNullOrEmpty(currentSereServBill))
                                ListSereServBill.AddRange(currentSereServBill);
                        }
                    }
                    var debtIds = ListCurrentTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__NO).Select(o => o.ID).ToList();
                    if (IsNotNullOrEmpty(debtIds))
                    {
                        var skip = 0;
                        while (debtIds.Count - skip > 0)
                        {
                            var listIds = debtIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServDebtFilterQuery sereServDebtFilter = new HisSereServDebtFilterQuery() { DEBT_IDs = listIds };
                            var currentSereServDebt = new HisSereServDebtManager(paramGet).Get(sereServDebtFilter);
                            if (IsNotNullOrEmpty(currentSereServDebt))
                                ListSereServDebt.AddRange(currentSereServDebt);
                        }
                    }

                    listSereServIdNotDeposit = ListSereServBill.Where(o => !ListSereServDeposit.Exists(e => e.SERE_SERV_ID == o.SERE_SERV_ID)).Select(s => s.SERE_SERV_ID).Distinct().ToList();
                    //khong lay cac sere_serv co deposit khong thuoc thoi gian lay bao cao
                    if (IsNotNullOrEmpty(listSereServIdNotDeposit))
                    {
                        List<long> ssIdRemove = new List<long>();
                        var skip = 0;
                        while (listSereServIdNotDeposit.Count - skip > 0)
                        {
                            var listIds = listSereServIdNotDeposit.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServDepositFilterQuery FilterSereServDeposit = new HisSereServDepositFilterQuery() { SERE_SERV_IDs = listIds };
                            var CurrentSereServDeposit = new HisSereServDepositManager(paramGet).Get(FilterSereServDeposit);
                            if (IsNotNullOrEmpty(CurrentSereServDeposit))
                            {
                                ssIdRemove.AddRange(CurrentSereServDeposit.Select(s => s.SERE_SERV_ID));
                            }
                        }

                        if (IsNotNullOrEmpty(ssIdRemove))
                        {
                            listSereServIdNotDeposit = listSereServIdNotDeposit.Where(o => !ssIdRemove.Contains(0)).ToList();
                        }
                    }

                    List<long> listSereServIds = new List<long>();
                    listSereServIds.AddRange(ListSereServDeposit.Select(s => s.SERE_SERV_ID).ToList());
                    listSereServIds.AddRange(ListSereServDebt.Select(s => s.SERE_SERV_ID).ToList());
                    listSereServIds.AddRange(listSereServIdNotDeposit);
                    listSereServIds = listSereServIds.Distinct().ToList();
                    var skip1 = 0;
                    while (listSereServIds.Count - skip1 > 0)
                    {
                        var listIds = listSereServIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip1 += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery() { IDs = listIds };
                        var currentSereServ = new HisSereServManager(paramGet).Get(sereServFilter);
                        if (IsNotNullOrEmpty(currentSereServ))
                            ListSereServ.AddRange(currentSereServ);
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

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListRdo.Clear();
                Dictionary<long, V_HIS_TRANSACTION> dicTransaction = new Dictionary<long, V_HIS_TRANSACTION>();

                if (ListCurrentTransaction != null && ListCurrentTransaction.Count > 0)
                {
                    var ListValidTransaction = ListCurrentTransaction.Where(o =>/* /* o.IS_TRANSFER_ACCOUNTING != 1 &&*/ o.IS_CANCEL != 1).ToList();
                    if (ListValidTransaction != null && ListValidTransaction.Count > 0)
                    {
                        dicTransaction = ListValidTransaction.ToDictionary(o => o.ID, o => o);
                        var Groups = ListValidTransaction.OrderBy(o => o.CASHIER_LOGINNAME).ToList().GroupBy(g => g.CASHIER_LOGINNAME).ToList();
                        foreach (var group in Groups)
                        {
                            List<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION> listSub = group.ToList<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION>();
                            if (listSub != null && listSub.Count > 0)
                            {
                                Mrs00030RDO dataRDO = new Mrs00030RDO();
                                dataRDO.CASHIER_LOGINNAME = listSub[0].CASHIER_LOGINNAME;
                                dataRDO.CASHIER_USERNAME = listSub[0].CASHIER_USERNAME;
                                foreach (var transaction in listSub)
                                {
                                    if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                                    {
                                        dataRDO.KC_AMOUNT += transaction.KC_AMOUNT ?? 0;
                                        dataRDO.TOTAL_BILL_AMOUNT += transaction.AMOUNT;
                                        if (transaction.KC_AMOUNT > 0)
                                        {
                                            dataRDO.TOTAL_BILL_AMOUNT_BEFORE += transaction.AMOUNT;
                                        }
                                        else
                                        {
                                            dataRDO.TOTAL_BILL_AMOUNT_AFTER += transaction.AMOUNT;
                                        }

                                        dataRDO.TOTAL_EXEMPTION += transaction.EXEMPTION ?? 0;
                                    }
                                    else if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                                    {
                                        if (transaction.TDL_SERE_SERV_DEPOSIT_COUNT == null || transaction.TDL_SERE_SERV_DEPOSIT_COUNT == 0)
                                            dataRDO.TOTAL_DEPOSIT_AMOUNT += transaction.AMOUNT;

                                        else dataRDO.TOTAL_DEPOSITS_AMOUNT += transaction.AMOUNT;
                                    }
                                    else if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                                    {
                                        if (transaction.TDL_SESE_DEPO_REPAY_COUNT == null || transaction.TDL_SESE_DEPO_REPAY_COUNT == 0)
                                            dataRDO.TOTAL_REPAY_AMOUNT += transaction.AMOUNT;
                                        else dataRDO.TOTAL_REPAYS_AMOUNT += transaction.AMOUNT;
                                    }
                                }

                                ListRdo.Add(dataRDO);
                            }
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var sereServ in ListSereServ)
                    {
                        long transactionID = 0;
                        var deposit = ListSereServDeposit.FirstOrDefault(o => o.SERE_SERV_ID == sereServ.ID);
                        if (deposit != null)
                        {
                            transactionID = deposit.DEPOSIT_ID;
                        }
                        else
                        {
                            var bill = ListSereServBill.FirstOrDefault(o => o.SERE_SERV_ID == sereServ.ID);
                            var debt = ListSereServDebt.FirstOrDefault(o => o.SERE_SERV_ID == sereServ.ID);
                            if (bill != null)
                            {
                                transactionID = bill.BILL_ID;
                                if (debt != null)
                                {
                                    dicTransaction[transactionID].IS_DEBT_COLLECTION = null;//nếu thu nợ luôn trong kỳ báo cáo thì không gọi là thu nợ mà gọi là thanh toán
                                }
                            }
                            else if(debt !=null)
                            {
                                transactionID = debt.DEBT_ID;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (!dicTransaction.ContainsKey(transactionID))
                        {
                            continue;
                        }

                        Mrs00030RDODetail rdo = new Mrs00030RDODetail(sereServ, dicTransaction[transactionID]);

                        var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_ROOM_ID);
                        if (room != null)
                        {
                            rdo.TDL_REQUEST_ROOM_NAME = room.ROOM_NAME;
                        }

                        var unit = MANAGER.Config.HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_UNIT_ID);
                        if (unit != null)
                        {
                            rdo.TDL_SERVICE_UNIT_NAME = unit.SERVICE_UNIT_NAME;
                        }

                        var type = MANAGER.Config.HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID);
                        if (type != null)
                        {
                            rdo.TDL_SERVICE_TYPE_NAME = type.SERVICE_TYPE_NAME;
                        }

                        ListRdoDetail.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(ListRdoDetail))
                {
                    ListRdoDetail = ListRdoDetail.OrderBy(o => o.CASHIER_LOGINNAME).ThenBy(o => o.TDL_PATIENT_CODE).ThenBy(o => o.TDL_TREATMENT_CODE).ThenBy(o => o.TDL_SERVICE_TYPE_NAME).ThenBy(o => o.TDL_SERVICE_CODE).ToList();
                    var grouptype = ListRdoDetail.GroupBy(g => new { g.TDL_SERVICE_TYPE_ID, g.CASHIER_LOGINNAME }).ToList();
                    var groupCashier = ListRdoDetail.GroupBy(g => g.CASHIER_LOGINNAME).ToList();

                    foreach (var item in grouptype)
                    {
                        Mrs00030RDODetail rdo = new Mrs00030RDODetail();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00030RDODetail>(rdo, item.First());
                        rdo.DISCOUNT = item.Sum(s => s.DISCOUNT);
                        rdo.VIR_TOTAL_PRICE = item.Sum(s => s.VIR_TOTAL_PRICE);
                        rdo.VIR_TOTAL_HEIN_PRICE = item.Sum(s => s.VIR_TOTAL_HEIN_PRICE);
                        rdo.VIR_TOTAL_PATIENT_PRICE = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE);
                        rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT);
                        rdo.VIR_TOTAL_PATIENT_PRICE_NO_DC = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_NO_DC);
                        rdo.VIR_TOTAL_PATIENT_PRICE_TEMP = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_TEMP);
                        rdo.VIR_TOTAL_PRICE_NO_ADD_PRICE = item.Sum(s => s.VIR_TOTAL_PRICE_NO_ADD_PRICE);
                        rdo.VIR_TOTAL_PRICE_NO_EXPEND = item.Sum(s => s.VIR_TOTAL_PRICE_NO_EXPEND);
                        ListRdoDetailGroupType.Add(rdo);
                    }

                    foreach (var item in groupCashier)
                    {
                        Mrs00030RDODetail rdo = new Mrs00030RDODetail();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00030RDODetail>(rdo, item.First());
                        rdo.DISCOUNT = item.Sum(s => s.DISCOUNT);
                        rdo.VIR_TOTAL_PRICE = item.Sum(s => s.VIR_TOTAL_PRICE);
                        rdo.VIR_TOTAL_HEIN_PRICE = item.Sum(s => s.VIR_TOTAL_HEIN_PRICE);
                        rdo.VIR_TOTAL_PATIENT_PRICE = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE);
                        rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT);
                        rdo.VIR_TOTAL_PATIENT_PRICE_NO_DC = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_NO_DC);
                        rdo.VIR_TOTAL_PATIENT_PRICE_TEMP = item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_TEMP);
                        rdo.VIR_TOTAL_PRICE_NO_ADD_PRICE = item.Sum(s => s.VIR_TOTAL_PRICE_NO_ADD_PRICE);
                        rdo.VIR_TOTAL_PRICE_NO_EXPEND = item.Sum(s => s.VIR_TOTAL_PRICE_NO_EXPEND);
                        ListRdoDetailGroupCashier.Add(rdo);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if ((long)((Mrs00030Filter)reportFilter).BRANCH_ID != 0)
                dicSingleTag.Add("BRANCH_NAME", Branch.BRANCH_NAME);
            else dicSingleTag.Add("BRANCH_NAME", "");
            if (((Mrs00030Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00030Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00030Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00030Filter)reportFilter).TIME_TO));
            }

            objectTag.AddObjectData(store, "Report", ListRdo);

            objectTag.AddObjectData(store, "Detail", ListRdoDetail);
            objectTag.AddObjectData(store, "GroupType", ListRdoDetailGroupType);
            objectTag.AddObjectData(store, "GroupCashier", ListRdoDetailGroupCashier);

            objectTag.AddRelationship(store, "GroupCashier", "Detail", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
            objectTag.AddRelationship(store, "GroupCashier", "GroupType", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");

            objectTag.AddRelationship(store, "GroupType", "Detail", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");
        }
    }
}
