using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00525
{
    /// <summary>
    /// Lấy các giao dịch tạm ứng nội trú
    /// Lấy các giao dịch hoàn ứng nội trú: lý do hoàn lại tiền tạm ứng (có thể lấy id fix trên DB release)
    /// Sắp xếp theo thời gian giao dịch
    /// Chọn ngày, chọn thu ngân (như hiện tại)
    /// Lấy ra thông tin mã điều trị, mã giao dịch, người thu, người chi, số tiền thu,số tiền chi của các giao dịch trên
    /// Cuối có tổng tiền thu, tổng tiền chi => tổng thực thu
    /// </summary>
    class Mrs00525Processor : AbstractProcessor
    {
        List<Mrs00525RDO> ListRdo = new List<Mrs00525RDO>();
        CommonParam paramGet = new CommonParam();
        HIS_BRANCH Branch = new HIS_BRANCH();
        List<V_HIS_TRANSACTION> ListCurrentDeposit = new List<V_HIS_TRANSACTION>();
        List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();
        Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();
        List<HIS_SESE_DEPO_REPAY> ListSeseDepoRepay = new List<HIS_SESE_DEPO_REPAY>();

        List<V_HIS_CASHIER_ROOM> ListCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        Mrs00525Filter filter = null;
        public Mrs00525Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00525Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00525Filter)reportFilter);
            bool result = true;
            try
            {
                if (filter.BRANCH_ID != 0)
                {
                    HisBranchFilterQuery filterBranch = new HisBranchFilterQuery();
                    filterBranch.ID = filter.BRANCH_ID;
                    Branch = new HisBranchManager(paramGet).Get(filterBranch).FirstOrDefault();

                    HisCashierRoomViewFilterQuery filterCashierRoom = new HisCashierRoomViewFilterQuery();
                    filterCashierRoom.BRANCH_ID = filter.BRANCH_ID;
                    ListCashierRoom = new HisCashierRoomManager(paramGet).GetView(filterCashierRoom);
                }
                HisTransactionViewFilterQuery filterHisDeposit = new HisTransactionViewFilterQuery();
                filterHisDeposit.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                filterHisDeposit.TRANSACTION_TIME_TO = filter.TIME_TO;
                filterHisDeposit.TRANSACTION_TYPE_IDs = new List<long>() { 
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU,
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU
                };
                ListCurrentDeposit = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(filterHisDeposit);

                ListCurrentDeposit = ListCurrentDeposit.Where(o => !o.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue && !o.TDL_SESE_DEPO_REPAY_COUNT.HasValue).ToList();


                if (!String.IsNullOrEmpty(filter.CASHIER_LOGINNAME))
                {
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => o.CASHIER_LOGINNAME == filter.CASHIER_LOGINNAME).ToList();
                }
                if (!String.IsNullOrEmpty(filter.LOGINNAME))
                {
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => o.CASHIER_LOGINNAME == filter.LOGINNAME).ToList();
                }

                var ListDepositId = ListCurrentDeposit.Select(o => o.ID).ToList();
                if (IsNotNullOrEmpty(ListDepositId))
                {
                    var skip = 0;
                    while (ListDepositId.Count - skip > 0)
                    {
                        var listIDs = ListDepositId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServDepositFilterQuery FilterSereServDeposit = new HisSereServDepositFilterQuery()
                        {
                            DEPOSIT_IDs = listIDs
                        };
                        var SereServDepositLib = new HisSereServDepositManager(paramGet).Get(FilterSereServDeposit);
                        ListSereServDeposit.AddRange(SereServDepositLib);

                        HisSeseDepoRepayFilterQuery repayFilter = new HisSeseDepoRepayFilterQuery();
                        repayFilter.REPAY_IDs = listIDs;
                        var sereRepay = new HisSeseDepoRepayManager(paramGet).Get(repayFilter);
                        ListSeseDepoRepay.AddRange(sereRepay);
                    }
                }

                var listTreatmentId = ListCurrentDeposit.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentFilterQuery treaFilter = new HisTreatmentFilterQuery();
                        treaFilter.IDs = listIDs;
                        var listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).Get(treaFilter);
                        if (IsNotNullOrEmpty(listTreatment))
                        {
                            foreach (var item in listTreatment)
                            {
                                dicTreatment[item.ID] = item;
                            }
                        }
                    }
                }

                if (ListCashierRoom != null && ListCashierRoom.Count > 0)
                {
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => ListCashierRoom.Select(p => p.ID).Contains(o.CASHIER_ROOM_ID)).ToList();
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

                if (IsNotNullOrEmpty(ListCurrentDeposit))
                {
                    List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => o.IS_CANCEL != 1 && o.IS_DELETE != 1).ToList();
                    if (IsNotNullOrEmpty(ListCurrentDeposit))
                    {
                        List<V_HIS_TRANSACTION> listDeposit = new List<V_HIS_TRANSACTION>();
                        List<V_HIS_TRANSACTION> listRepay = new List<V_HIS_TRANSACTION>();

                        listDeposit = ListCurrentDeposit.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                        listRepay = ListCurrentDeposit.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();

                        if (IsNotNullOrEmpty(listDeposit) && IsNotNullOrEmpty(ListSereServDeposit))
                        {
                            listDeposit = listDeposit.Where(o => ListSereServDeposit.Select(p => p.DEPOSIT_ID).Contains(o.ID) == false).ToList();
                        }

                        if (IsNotNullOrEmpty(listRepay) && IsNotNullOrEmpty(ListSeseDepoRepay))
                        {
                            listRepay = listRepay.Where(o => !ListSeseDepoRepay.Select(s => s.REPAY_ID).Contains(o.ID)).ToList();
                        }

                        if (IsNotNullOrEmpty(listRepay))
                            listRepay = listRepay.Where(o => /*((o.TDL_SESE_DEPO_REPAY_COUNT.HasValue && o.TDL_SESE_DEPO_REPAY_COUNT.Value > 0) ||
                                (dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID ?? 0) && dicPatientTypeAlter[o.TREATMENT_ID ?? 0].LOG_TIME > o.CREATE_TIME)) &&*/
                                o.REPAY_REASON_ID == MRS.MANAGER.Config.HisRepayReasonCFG.get_REPAY_REASON_CODE__01 //lý do hoàn lại tiền tạm ứng 
                                ).ToList();

                        if (IsNotNullOrEmpty(listDeposit))
                            listTransaction.AddRange(listDeposit);

                        if (IsNotNullOrEmpty(listRepay))
                            listTransaction.AddRange(listRepay);
                    }
                    var listEle = (filter.PAY_FORM_CODE__ELE ?? " ").Split(',');
                    ListRdo = (from r in listTransaction select new Mrs00525RDO(r, dicTreatment, listEle.ToList())).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if ((long)((Mrs00525Filter)reportFilter).BRANCH_ID != 0)
                dicSingleTag.Add("BRANCH_NAME", Branch.BRANCH_NAME);
            else dicSingleTag.Add("BRANCH_NAME", "");
            if (((Mrs00525Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00525Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00525Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00525Filter)reportFilter).TIME_TO));
            }

            if (IsNotNullOrEmpty(ListRdo))
            {
                ListRdo = ListRdo.OrderBy(o => o.TRANSACTION_TIME).ThenBy(o => o.VIR_PATIENT_NAME).ToList();
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
