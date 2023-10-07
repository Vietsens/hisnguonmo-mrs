using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServDeposit;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00263
{
    class Mrs00263Processor : AbstractProcessor
    {

        List<Mrs00263RDO> ListRdo = new List<Mrs00263RDO>();
        CommonParam paramGet = new CommonParam();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();

        List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();
        List<V_HIS_CASHIER_ROOM> ListCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        public Mrs00263Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00263Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00263Filter)reportFilter);
            bool result = true;
            try
            {

                HisCashierRoomViewFilterQuery filterCashierRoom = new HisCashierRoomViewFilterQuery();
                filterCashierRoom.BRANCH_ID = filter.BRANCH_ID;
                ListCashierRoom = new HisCashierRoomManager(paramGet).GetView(filterCashierRoom);
                if (!IsNotNullOrEmpty(ListCashierRoom)) return result;
                HisTransactionFilterQuery filterHisDeposit = new HisTransactionFilterQuery();
                filterHisDeposit.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                filterHisDeposit.TRANSACTION_TIME_TO = filter.TIME_TO;
                filterHisDeposit.IS_CANCEL = false;
                //filterHisDeposit.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU;
                ListTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(filterHisDeposit);
                ListTransaction = ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                ListTransaction = ListTransaction.Where(o => !o.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue && !o.TDL_SESE_DEPO_REPAY_COUNT.HasValue).ToList();
                ListTransaction = ListTransaction.Where(o => filter.PAY_FORM_ID == null || o.PAY_FORM_ID == filter.PAY_FORM_ID).ToList();
                if (!String.IsNullOrEmpty(filter.CASHIER_LOGINNAME))
                {
                    ListTransaction = ListTransaction.Where(o => o.CASHIER_LOGINNAME == filter.CASHIER_LOGINNAME).ToList();
                }
                if (!String.IsNullOrEmpty(filter.LOGINNAME))
                {
                    ListTransaction = ListTransaction.Where(o => o.CASHIER_LOGINNAME == filter.LOGINNAME).ToList();
                }

                ListTransaction = ListTransaction.Where(o => ListCashierRoom.Select(p => p.ID).Contains(o.CASHIER_ROOM_ID)).ToList();
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

                if (IsNotNullOrEmpty(ListTransaction))
                {
                    ListTransaction = ListTransaction.Where(o => o.IS_CANCEL != 1 && o.IS_DELETE != 1).ToList();

                    ListRdo = (from r in ListTransaction select new Mrs00263RDO(r)).ToList();
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
            if (((Mrs00263Filter)reportFilter).BRANCH_ID != null)
            {
                dicSingleTag.Add("BRANCH_NAME", (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ((Mrs00263Filter)reportFilter).BRANCH_ID) ?? new HIS_BRANCH()).BRANCH_NAME);
            }
            else dicSingleTag.Add("BRANCH_NAME", "");
            if (((Mrs00263Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00263Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00263Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00263Filter)reportFilter).TIME_TO));
            }

            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
