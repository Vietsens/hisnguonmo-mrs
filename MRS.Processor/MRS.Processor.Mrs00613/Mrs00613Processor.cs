using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBillFund;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisFund;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00613
{
    class Mrs00613Processor : AbstractProcessor
    {
        Mrs00613Filter castFilter = null;
        List<Mrs00613RDO> listRdo = new List<Mrs00613RDO>();

        List<HIS_TRANSACTION_D> listBill = new List<HIS_TRANSACTION_D>();
        //List<HIS_BILL_FUND> listBillFund = new List<HIS_BILL_FUND>();
        List<HIS_FUND> listFund = new List<HIS_FUND>();
        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();

        public Mrs00613Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00613Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00613Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_BILL, V_HIS_DEPOSIT, V_HIS_REPAY, MRS00613: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();

                listBill = new Mrs00613RDOManager().GetTransaction(castFilter);

                HisCashierRoomViewFilterQuery cashierFilter = new HisCashierRoomViewFilterQuery();
                cashierFilter.BRANCH_ID = castFilter.BRANCH_ID;
                listCashierRoom = new HisCashierRoomManager(paramGet).GetView(cashierFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00613");
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
                if (listBill != null)
                {
                    if(listCashierRoom!=null)
                    {
                        listBill = listBill.Where(o => listCashierRoom.Exists(p => p.ID == o.CASHIER_ROOM_ID)).ToList();
                    }
                    foreach (var item in listBill)
                    {
                       
                       
                        Mrs00613RDO rdo = new Mrs00613RDO(item);
                        rdo.NUM_ORDER_STR = string.Format("{0:0000000}", Convert.ToInt64(item.NUM_ORDER));
                        rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.TRANSACTION_TIME);
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IN_TIME);
                        rdo.DOB_YEAR_STR = (item.TDL_PATIENT_DOB != null && item.TDL_PATIENT_DOB.ToString().Length > 4) ? item.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                        rdo.USE_FUND_REASON = "";
                        rdo.FUND_NAME = item.FUND_NAME;
                        rdo.FUND_AMOUNT = item.BILL_FUND_AMOUNT;
                        listRdo.Add(rdo);
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                if (castFilter.BRANCH_ID!=null)
                {
                    dicSingleTag.Add("BRANCH_NAME", (HisBranchCFG.HisBranchs.FirstOrDefault(o=>o.ID==castFilter.BRANCH_ID)??new HIS_BRANCH()).BRANCH_NAME);
                }
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
