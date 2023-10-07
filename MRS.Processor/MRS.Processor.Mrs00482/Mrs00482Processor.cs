using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using SDA.Filter;
using SDA.EFMODEL;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using System.Data;
using System.Reflection;
using MOS.MANAGER.HisEmployee;

namespace MRS.Processor.Mrs00482
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00482Processor : AbstractProcessor
    {
        Mrs00482Filter castFilter = new Mrs00482Filter();
        List<Mrs00482RDO> listDepositRepay = new List<Mrs00482RDO>();
        List<Mrs00482RDO> listDeposit = new List<Mrs00482RDO>();
        List<Mrs00482RDO> listDepositCancel = new List<Mrs00482RDO>();
        List<Mrs00482RDO> listRepay = new List<Mrs00482RDO>();
        List<Mrs00482RDO> listDepositAll = new List<Mrs00482RDO>();
        

        protected string BRANCH_NAME = "";

        public Mrs00482Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00482Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00482Filter)this.reportFilter;

                HisCashierRoomViewFilterQuery cashierRoomfilter = new HisCashierRoomViewFilterQuery();
                cashierRoomfilter.BRANCH_ID = this.castFilter.BRANCH_ID;
                
                var listCashierRooms = new HisCashierRoomManager(param).GetView(cashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                BRANCH_NAME = branch != null ? branch.BRANCH_NAME : null;
                GetDataDepositRepay(listCashierRooms.Select(o => o.ID).ToList());
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDataDepositRepay(List<long> cashierRoomIds)
        {
            this.listDeposit = new ManagerSql().GetDeposit(castFilter);
            this.listRepay = new ManagerSql().GetRepay(castFilter);
            AddInfo(listDeposit, IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU);
            listDepositRepay.AddRange(listDeposit.Where(o => o.CANCEL_VALUE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
            listDepositAll.AddRange(listDeposit);
            AddInfo(listRepay, IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU);
            listDepositRepay.AddRange(this.listRepay);
            if(castFilter.REMOVE_DUPLICATE!=true)
            { 
                this.listDepositCancel = new ManagerSql().GetDepositCancel(castFilter); 
            }   
            
            //xử lý ReMix 
            var listReMix = listDepositAll.Where(o => listDepositCancel.Exists(p => p.ID == o.ID && p.TRANSACTION_DATE == o.TRANSACTION_DATE && p.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME)).ToList();
            listDepositCancel = listDepositCancel.Where(o => !listReMix.Exists(p => p.ID == o.ID)).ToList();
            foreach (var item in listReMix)
            {
                item.AMOUNT_CANCEL = item.AMOUNT;
                
            }
            
            AddInfo(listDepositCancel, IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU);
            
            listDepositAll.AddRange(listDepositCancel);
        }

        


        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("listAll" + listDepositAll.Count);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        

        private void AddInfo(List<Mrs00482RDO> listTransaction, long tranSactionTypeId)
        {
            
            foreach (var item in listTransaction)
            {
                item.TRANSACTION = new V_HIS_TRANSACTION();
                item.TRANSACTION.ACCOUNT_BOOK_ID = item.ACCOUNT_BOOK_ID;
                item.TRANSACTION.ACCOUNT_BOOK_NAME = item.ACCOUNT_BOOK_NAME;
                item.TRANSACTION.NUM_ORDER = item.NUM_ORDER ?? 0;
                item.TRANSACTION.EINVOICE_NUM_ORDER = item.EINVOICE_NUM_ORDER;
                item.TRANSACTION.BANK_TRANSACTION_CODE = item.BANK_TRANSACTION_CODE;
                item.TRANSACTION.BANK_TRANSACTION_TIME = item.BANK_TRANSACTION_TIME;
                item.TRANSACTION.TRANSACTION_TIME = item.TRANSACTION_TIME;
                item.TRANSACTION.TRANSACTION_CODE = item.TRANSACTION_CODE;
                item.TRANSACTION.TREATMENT_CODE = item.TREATMENT_CODE;
                item.TRANSACTION.CASHIER_LOGINNAME = item.CASHIER_LOGINNAME;
                item.TRANSACTION.CASHIER_USERNAME = item.CASHIER_USERNAME;
                item.TRANSACTION.TRANSACTION_TYPE_ID = tranSactionTypeId;
                item.HIS_TREATMENT = new HIS_TREATMENT();
                item.HIS_TREATMENT.TDL_PATIENT_TYPE_ID = item.TDL_PATIENT_TYPE_ID;
                item.HIS_TREATMENT.TDL_PATIENT_CODE = item.TDL_PATIENT_CODE;
                item.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.TRANSACTION_TIME);
                item.HIS_TREATMENT.IN_TIME = item.IN_TIME;
                item.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IN_TIME);
                item.HIS_TREATMENT.OUT_TIME = item.OUT_TIME;
                item.HIS_TREATMENT.FEE_LOCK_TIME = item.FEE_LOCK_TIME;
                item.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.OUT_TIME ?? 0);
                item.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.FEE_LOCK_TIME ?? 0);
                item.IS_CANCEL = item.CANCEL_VALUE == 1 ? true : false;
                item.DOB_YEAR = item.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                
                if (tranSactionTypeId == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                {
                    //string query = string.Format("select * from his_transaction where transaction_date = {0} and tdl_patient_code = {1} and transaction_type_id = {2}", item.TRANSACTION_DATE, item.HIS_TREATMENT.TDL_PATIENT_CODE, IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU);
                    //Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    var check = this.listRepay.FirstOrDefault(o=>o.TRANSACTION_DATE==item.TRANSACTION_DATE && o.TREATMENT_CODE == item.TREATMENT_CODE);
                    //Inventec.Common.Logging.LogSystem.Info("check" + check.Count());
                    if (check!=null)
                    {
                        item.REPAY_STATUS = "Đã hoàn";
                    }
                    if (item.CANCEL_VALUE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.DEPOSIT_CASH = item.AMOUNT;
                        if (item.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM)
                            item.DEPOSIT_BLUNT = item.AMOUNT;
                        else if (item.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK)
                            item.DEPOSIT_TRANSFER = item.AMOUNT;
                        else
                            item.DEPOSIT_OTHER_PAY_FORM = item.AMOUNT;
                        
                    }
                    
                }
                else if (tranSactionTypeId == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    item.REPAY = item.AMOUNT;
                    item.REPAY_USERNAME = item.CASHIER_USERNAME;
                }
                
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                var cashername = "";
                if (castFilter.CASHIER_LOGINNAME != null && castFilter.CASHIER_LOGINNAME != "")
                {
                    if (listDepositRepay != null && listDepositRepay.Count > 0)
                        cashername = listDepositRepay.First().TRANSACTION.CASHIER_USERNAME;
                    else
                        cashername = castFilter.CASHIER_LOGINNAME;
                }

                //=======================================================================
                HisCashierRoomViewFilterQuery cashierRoomfilter = new HisCashierRoomViewFilterQuery();
                cashierRoomfilter.ID = this.castFilter.EXACT_CASHIER_ROOM_ID;
                var listCashierRooms = new HisCashierRoomManager(param).GetView(cashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                var employees = new HisEmployeeManager().Get(new HisEmployeeFilterQuery());
                if (!string.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME))
                {
                    dicSingleTag.Add("CASHIER_USERNAME", (employees.FirstOrDefault(o => o.LOGINNAME == castFilter.CASHIER_LOGINNAME) ?? new HIS_EMPLOYEE()).TDL_USERNAME);
                }
                if (castFilter.CASHIER_LOGINNAMEs!=null)
                {
                    dicSingleTag.Add("CASHIER_USERNAMEs", string.Join(", ",employees.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains( o.LOGINNAME??"")).Select(p=>p.TDL_USERNAME).ToList()));
                }
                dicSingleTag.Add("CASHIER_ROOM_NAME", (listCashierRooms.FirstOrDefault(o => o.ID == castFilter.EXACT_CASHIER_ROOM_ID) ?? new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);
                dicSingleTag.Add("BRANCH_NAME", this.BRANCH_NAME);


                bool exportSuccess = true;
                listDepositRepay = listDepositRepay.OrderBy(o => o.CASHIER_LOGINNAME).ThenBy(o => o.REPAY_USERNAME).ThenBy(o => o.TRANSACTION.TRANSACTION_CODE).ToList();
                objectTag.AddObjectData(store, "Report", listDepositRepay);                                                                                                                                                                                                                                                                                                                                                                      
                objectTag.AddObjectData(store, "ReportDate", listDepositRepay.Where(o => o.IS_CANCEL == false).GroupBy(p => new { p.TRANSACTION_DATE, p.CASHIER_LOGINNAME }).Select(q => q.First()).ToList());
                objectTag.AddObjectData(store, "ReportParent", listDepositRepay.Where(o => o.IS_CANCEL == false).GroupBy(p => p.CASHIER_LOGINNAME).Select(q => q.First()).ToList());
                objectTag.AddRelationship(store, "ReportParent", "ReportDate", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
                objectTag.AddRelationship(store, "ReportDate", "Report", new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" }, new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" });
                objectTag.AddRelationship(store, "ReportParent", "Report", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");


                objectTag.AddObjectData(store, "Deposit", listDeposit.Where(o => o.IS_CANCEL == false).OrderBy(p => p.NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "DepositDate", listDeposit.Where(o => o.IS_CANCEL == false).GroupBy(p => new { p.TRANSACTION_DATE, p.CASHIER_LOGINNAME }).Select(q => q.First()).ToList());
                objectTag.AddObjectData(store, "DepositLoginname", listDeposit.Where(o => o.IS_CANCEL == false).GroupBy(p => p.CASHIER_LOGINNAME).Select(q => q.First()).ToList());

                objectTag.AddRelationship(store, "DepositLoginname", "DepositDate", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
                objectTag.AddRelationship(store, "DepositDate", "Deposit", new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" }, new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" });
                objectTag.AddRelationship(store, "DepositLoginname", "Deposit", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
                ////
                
                objectTag.AddObjectData(store, "DepositAll", listDepositAll);
                objectTag.AddObjectData(store, "DepositAllDate", listDepositAll.GroupBy(p => new { p.TRANSACTION_DATE, p.CASHIER_LOGINNAME }).Select(q => q.First()).ToList());
                objectTag.AddObjectData(store, "DepositAllLoginname", listDepositAll.GroupBy(p => p.CASHIER_LOGINNAME).Select(q => q.First()).ToList());
                

                //objectTag.AddObjectData(store, "DepositCancel", listDepositCancel);
                //objectTag.AddObjectData(store, "DepositCancelLoginname", listDepositCancel.GroupBy(p => p.CASHIER_LOGINNAME).Select(q => q.First()).ToList());
                //objectTag.AddRelationship(store, "DepositCancelLoginname", "DepositCancel", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");

                objectTag.AddRelationship(store, "DepositAllLoginname", "DepositAllDate", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
                objectTag.AddRelationship(store, "DepositAllDate", "DepositAll", new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" }, new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" });
                objectTag.AddRelationship(store, "DepositAllLoginname", "DepositAll",  "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");



                objectTag.AddObjectData(store, "PayForms", HisPayFormCFG.ListPayForm);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
