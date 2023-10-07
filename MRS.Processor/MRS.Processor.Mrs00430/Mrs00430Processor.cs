using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;

using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00430
{
    class Mrs00430Processor : AbstractProcessor
    {

        List<Mrs00430RDO> ListRdo = new List<Mrs00430RDO>();
        CommonParam paramGet = new CommonParam();
        HIS_BRANCH Branch = new HIS_BRANCH();
        List<V_HIS_TRANSACTION> ListCurrentDeposit = new List<V_HIS_TRANSACTION>();
        List<HIS_SERE_SERV_DEPOSIT> ListDepositDetail = new List<HIS_SERE_SERV_DEPOSIT>();
        List<V_HIS_TRANSACTION> ListCurrentRepay = new List<V_HIS_TRANSACTION>();
        List<HIS_SESE_DEPO_REPAY> ListRepayDetail = new List<HIS_SESE_DEPO_REPAY>();
        List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();

        List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();
        List<HIS_CASHIER_ROOM> ListCashierRoom = new List<HIS_CASHIER_ROOM>();
        List<V_HIS_TRANSACTION> ListCurrentTransaction = new List<V_HIS_TRANSACTION>();
        public Mrs00430Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00430Filter);
        }

        Mrs00430Filter filter = null;
        protected override bool GetData()
        {
          filter = ((Mrs00430Filter)reportFilter);
            bool result = true;
            try
            {
                if ((long)filter.BRANCH_ID != 0)
                {
                    HisBranchFilterQuery filterBranch = new HisBranchFilterQuery();
                    filterBranch.ID = filter.BRANCH_ID;
                    Branch = new HisBranchManager(paramGet).Get(filterBranch).FirstOrDefault();

                    HisDepartmentFilterQuery filterDepartment = new HisDepartmentFilterQuery();
                    filterDepartment.BRANCH_ID = filter.BRANCH_ID;
                    var listDepartment = new HisDepartmentManager(paramGet).Get(filterDepartment);
                    foreach (var department in listDepartment)
                    {
                        HisRoomViewFilterQuery filterRoom = new HisRoomViewFilterQuery();
                        filterRoom.DEPARTMENT_ID = department.ID;
                        var listRoom = new HisRoomManager(paramGet).GetView(filterRoom);
                        ListRoom.AddRange(listRoom);
                    }

                    HisCashierRoomFilterQuery filterCashierRoom = new HisCashierRoomFilterQuery();
                    filterCashierRoom.ROOM_IDs = ListRoom.Select(o => o.ID).ToList();
                    ListCashierRoom = new HisCashierRoomManager(paramGet).Get(filterCashierRoom);

                }

                HisTransactionViewFilterQuery filterTransaction = new HisTransactionViewFilterQuery();
                filterTransaction.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                filterTransaction.TRANSACTION_TIME_TO = filter.TIME_TO;
                ListCurrentTransaction = new HisTransactionManager().GetView(filterTransaction);
                var TransactionIds = ListCurrentTransaction.Select(o => o.ID).ToList();
                var TreatmentIds = ListCurrentTransaction.Where(w => w.TREATMENT_ID != null).Select(s => s.TREATMENT_ID).Distinct().ToList();
                ListCurrentDeposit = ListCurrentTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();

                var listDepositId = ListCurrentDeposit.Select(o => o.ID).ToList();
                var skip = 0;
                while (listDepositId.Count - skip > 0)
                {
                    var listIds = listDepositId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServDepositFilterQuery FilterDepositDetail = new HisSereServDepositFilterQuery()
                    {
                        DEPOSIT_IDs = listIds
                    };
                    var CurrentDere = new HisSereServDepositManager(paramGet).Get(FilterDepositDetail);
                    ListDepositDetail.AddRange(CurrentDere);
                }
                ListCurrentRepay = ListCurrentTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();

                var ListRepayId = ListCurrentRepay.Select(o => o.ID).ToList();
                skip = 0;
                while (ListRepayId.Count - skip > 0)
                {
                    var listIds = ListRepayId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSeseDepoRepayFilterQuery FilterSeseDepoRepay = new HisSeseDepoRepayFilterQuery()
                    {
                        REPAY_IDs = listIds
                    };
                    var CurrentDere = new HisSeseDepoRepayManager(paramGet).Get(FilterSeseDepoRepay);
                    ListRepayDetail.AddRange(CurrentDere);
                }

                skip = 0;
                while (TreatmentIds.Count - skip > 0)
                {
                    var listIds = TreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientTypeAlterViewFilterQuery ft = new HisPatientTypeAlterViewFilterQuery();
                    ft.TREATMENT_IDs = listIds.Select(s => s.Value).ToList();
                    var patientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(ft);
                    ListPatientTypeAlter.AddRange(patientTypeAlters);
                }


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

                if (ListCurrentTransaction != null && ListCurrentTransaction.Count > 0)
                {
                    var ListValidTransaction = ListCurrentTransaction.Where(o =>/* /* o.IS_TRANSFER_ACCOUNTING != 1 &&*/ o.IS_CANCEL != 1).ToList();
                    if (ListValidTransaction != null && ListValidTransaction.Count > 0)
                    {
                        string keyGroup ="{0}";
                        if (filter.IS_DETAIL_PF == true)
                        {
                            keyGroup = "{0}_{1}";
                        }
                        var Groups = ListValidTransaction.OrderBy(o => o.CASHIER_LOGINNAME).ToList().GroupBy(g => string.Format(keyGroup,g.CASHIER_LOGINNAME,g.PAY_FORM_ID)).ToList();
                        foreach (var group in Groups)
                        {
                            List<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION> listSub = group.ToList<MOS.EFMODEL.DataModels.V_HIS_TRANSACTION>();
                            if (listSub != null && listSub.Count > 0)
                            {
                                Mrs00430RDO dataRDO = new Mrs00430RDO();
                                dataRDO.CASHIER_LOGINNAME = listSub[0].CASHIER_LOGINNAME;
                                dataRDO.CASHIER_USERNAME = listSub[0].CASHIER_USERNAME;
                                dataRDO.PAY_FORM_CODE = listSub[0].PAY_FORM_CODE;
                                dataRDO.PAY_FORM_NAME = listSub[0].PAY_FORM_NAME;
                                foreach (var transaction in listSub)
                                {
                                    var patyTypeAlter = ListPatientTypeAlter.Where(w => w.TREATMENT_ID == transaction.TREATMENT_ID).OrderByDescending(o => o.LOG_TIME).ToList();
                                    if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                                    {

                                        dataRDO.TOTAL_BILL_AMOUNT += transaction.AMOUNT;
                                        if (IsNotNullOrEmpty(patyTypeAlter))
                                        {
                                            if (patyTypeAlter.FirstOrDefault().PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                            {
                                                dataRDO.TOTAL_BILL_HEIN += transaction.AMOUNT;
                                            }
                                            else
                                            {
                                                dataRDO.TOTAL_BILL_FEE += transaction.AMOUNT;
                                            }
                                            //if (patyTypeAlter.FirstOrDefault().PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                                            //{
                                            //    dataRDO.TOTAL_BILL_FEE += transaction.AMOUNT; 
                                            //}
                                        }

                                        dataRDO.TOTAL_EXEMPTION += transaction.EXEMPTION ?? 0;
                                    }
                                    else if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                                    {
                                        if (ListDepositDetail.Where(o => o.DEPOSIT_ID == ListCurrentDeposit.Where(p => p.ID == transaction.ID).First().ID).ToList().Count == 0)
                                        {
                                            dataRDO.TOTAL_DEPOSIT_AMOUNT += transaction.AMOUNT;
                                            if (IsNotNullOrEmpty(patyTypeAlter))
                                            {
                                                if (patyTypeAlter.FirstOrDefault().PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                                {
                                                    dataRDO.TOTAL_DEPOSIT_HEIN += transaction.AMOUNT;
                                                }
                                                else
                                                {
                                                    dataRDO.TOTAL_DEPOSIT_FEE += transaction.AMOUNT;
                                                }
                                                //if (patyTypeAlter.FirstOrDefault().PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                                                //{
                                                //    dataRDO.TOTAL_DEPOSIT_FEE += transaction.AMOUNT; 
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            dataRDO.TOTAL_DEPOSITS_AMOUNT += transaction.AMOUNT;
                                        }
                                    }
                                    else if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                                    {
                                        if (ListRepayDetail.Where(o => o.REPAY_ID == ListCurrentRepay.Where(p => p.ID == transaction.ID).First().ID).ToList().Count == 0)
                                        {
                                            dataRDO.TOTAL_REPAY_AMOUNT += transaction.AMOUNT;
                                            if (IsNotNullOrEmpty(patyTypeAlter))
                                            {
                                                if (patyTypeAlter.FirstOrDefault().PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                                {
                                                    dataRDO.TOTAL_REPAY_HEIN += transaction.AMOUNT;
                                                }
                                                else
                                                {
                                                    dataRDO.TOTAL_REPAY_FEE += transaction.AMOUNT;
                                                }
                                                //if (patyTypeAlter.FirstOrDefault().PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                                                //{
                                                //    dataRDO.TOTAL_REPAY_FEE += transaction.AMOUNT; 
                                                //}
                                            }
                                        }
                                        else dataRDO.TOTAL_REPAYS_AMOUNT += transaction.AMOUNT;
                                    }
                                }
                                ListRdo.Add(dataRDO);
                            }
                        }
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
            if ((long)((Mrs00430Filter)reportFilter).BRANCH_ID != 0)
                dicSingleTag.Add("BRANCH_NAME", Branch.BRANCH_NAME);
            else dicSingleTag.Add("BRANCH_NAME", "");
            if (((Mrs00430Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00430Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00430Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00430Filter)reportFilter).TIME_TO));
            }

            objectTag.AddObjectData(store, "Report", ListRdo);
        }




    }
}
