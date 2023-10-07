using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00528
{
    class Mrs00528Processor : AbstractProcessor
    {
        Mrs00528Filter filter = null;
        List<Mrs00528RDO> ListRdo = new List<Mrs00528RDO>();
        List<Mrs00528RDO> ListGroup = new List<Mrs00528RDO>();
        CommonParam paramGet = new CommonParam();
        HIS_BRANCH Branch = new HIS_BRANCH();
        List<V_HIS_CASHIER_ROOM> ListCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        HIS_DEPARTMENT hisDepartment = new HIS_DEPARTMENT();
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
        List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<V_HIS_TRANSACTION> ListTransactionBillDeposit = new List<V_HIS_TRANSACTION>();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();

        List<long> listSereServId = new List<long>();
        List<long> listTreatmentId = new List<long>();

        public Mrs00528Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00528Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00528Filter)reportFilter);
            bool result = true;
            try
            {
                var serviceTypeList = HisServiceTypeCFG.HisServiceTypes.Where(p => p.ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Select(o => o.ID).ToList();
                if (filter.BRANCH_ID.HasValue)
                {
                    HisBranchFilterQuery filterBranch = new HisBranchFilterQuery();
                    filterBranch.ID = filter.BRANCH_ID;
                    Branch = new HisBranchManager(paramGet).Get(filterBranch).FirstOrDefault();

                    HisCashierRoomViewFilterQuery filterCashierRoom = new HisCashierRoomViewFilterQuery();
                    filterCashierRoom.BRANCH_ID = filter.BRANCH_ID;
                    ListCashierRoom = new HisCashierRoomManager(paramGet).GetView(filterCashierRoom);

                    //cac khoa cua chi nhanh
                    HisDepartmentFilterQuery filterDepartment = new HisDepartmentFilterQuery();
                    filterDepartment.BRANCH_ID = filter.BRANCH_ID;
                    ListDepartment = new HisDepartmentManager(paramGet).Get(filterDepartment);

                    //Giao dich
                    HisTransactionViewFilterQuery dfilter = new HisTransactionViewFilterQuery();
                    dfilter.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                    dfilter.TRANSACTION_TIME_TO = filter.TIME_TO;
                    dfilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU, IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                    dfilter.IS_CANCEL = false;
                    dfilter.HAS_SALL_TYPE = false;
                    ListTransactionBillDeposit = new HisTransactionManager(paramGet).GetView(dfilter);
                }

                //Khong thanh toan, tam ung nhung khoa vien phi
                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                treatmentViewFilter.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                treatmentViewFilter.FEE_LOCK_TIME_TO = filter.TIME_TO;
                treatmentViewFilter.END_DEPARTMENT_IDs = ListDepartment.Select(o => o.ID).ToList();
                var listTreatment = new HisTreatmentManager(paramGet).GetView(treatmentViewFilter);
                var listTreatmentFeeLockId = listTreatment.Where(p => p.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE || p.IS_ACTIVE == null).Select(o => o.ID).ToList();

                //Danh sách giao dịch tạm ứng dịch vụ:
                depositService();

                //Danh sách giao dịch thanh toán:
                billSevice();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    listSereServId = listSereServId.GroupBy(o => o).Select(o => o.First()).ToList();
                    //listTreatmentId.AddRange(listTreatmentNoTransactionID);
                    listTreatmentId.AddRange(listTreatmentFeeLockId);
                    listTreatmentId = listTreatmentId.GroupBy(o => o).Select(o => o.First()).ToList();
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listSub = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisSereServViewfilter = new HisSereServViewFilterQuery()
                        {
                            TREATMENT_IDs = listSub,
                            PATIENT_TYPE_IDs = (filter.PATIENT_TYPE_IDs != null && filter.PATIENT_TYPE_IDs.Count > 0) ? filter.PATIENT_TYPE_IDs : null,
                            HAS_EXECUTE = true
                        };
                        var listSereServSub = new HisSereServManager(paramGet).GetView(HisSereServViewfilter);
                        listSereServ.AddRange(listSereServSub);
                    }

                    if (IsNotNullOrEmpty(listSereServ) && this.filter.DEPARTMENT_ID != null && this.filter.DEPARTMENT_ID > 0)
                        listSereServ = listSereServ.Where(o => (o.TDL_REQUEST_DEPARTMENT_ID == this.filter.DEPARTMENT_ID && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH) || (o.TDL_EXECUTE_DEPARTMENT_ID == this.filter.DEPARTMENT_ID && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)).ToList();

                    var sereServIdNoDepositBillInCreateTime = listSereServ.Select(o => o.ID).Where(p => !listSereServId.Contains(p)).ToList();
                    var sereServThroughtId = new List<long>();
                    if (IsNotNullOrEmpty(sereServIdNoDepositBillInCreateTime))
                    {
                        var skip1 = 0;
                        while (sereServIdNoDepositBillInCreateTime.Count - skip1 > 0)
                        {
                            var listSub = sereServIdNoDepositBillInCreateTime.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery()
                            {
                                SERE_SERV_IDs = listSub
                            };
                            var ListSereServBillLib = new HisSereServBillManager(paramGet).GetView(sereServBillFilter) ?? new List<V_HIS_SERE_SERV_BILL>();
                            sereServThroughtId.AddRange(ListSereServBillLib.Select(o => o.SERE_SERV_ID));

                            HisSereServDepositFilterQuery sereServDepositFilter = new HisSereServDepositFilterQuery()
                            {
                                SERE_SERV_IDs = listSub
                            };
                            var ListSereServDepositLib = new HisSereServDepositManager(paramGet).Get(sereServDepositFilter) ?? new List<HIS_SERE_SERV_DEPOSIT>();
                            sereServThroughtId.AddRange(ListSereServDepositLib.Select(o => o.SERE_SERV_ID));
                        }
                    }
                    listSereServ = listSereServ.Where(o => listSereServId.Contains(o.ID) || ((listTreatmentFeeLockId.Contains(o.TDL_TREATMENT_ID ?? 0)) && (!listSereServId.Contains(o.ID)) && ((!sereServThroughtId.Contains(o.ID))))).ToList();

                }
                //Đối tượng
                List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
                //if (IsNotNullOrEmpty(listSereServ)) listSereServ = listSereServ.Where(o => o.VIR_TOTAL_PATIENT_PRICE > 0).ToList(); 
                if (IsNotNullOrEmpty(listSereServ))
                {
                    var treatmentIds = listSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listSub = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listSub
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        LisPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }

                    if (IsNotNullOrEmpty(LisPatientTypeAlter))
                        foreach (var item in LisPatientTypeAlter)
                        {
                            if (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                dicPatientTypeAlter[item.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                            dicPatientTypeAlter[item.TREATMENT_ID].Add(item);
                        }
                }

                if (IsNotNullOrEmpty(listSereServ) && filter.TREATMENT_TYPE_ID.HasValue)
                {
                    //lấy tất cả giường, phẫu thuật của hồ sơ điều trị đó không quan tâm chỉ định trước hay sau khi vào nội trú
                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        listSereServ = listSereServ.Where(o => dicPatientTypeAlter.ContainsKey(o.TDL_TREATMENT_ID ?? 0) &&
                            (treatmentTypeId(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0]) ==
                            IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || IsBed(o)
                            || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)).ToList();

                    //Lấy những chỉ định từ trước khi bn vào nội trú
                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                        listSereServ = listSereServ.Where(o => dicPatientTypeAlter.ContainsKey(o.TDL_TREATMENT_ID ?? 0) &&
                            (treatmentTypeId(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0]) ==
                            IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && IsNotBedAfterIn(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0]))).ToList();

                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        listSereServ = listSereServ.Where(o => dicPatientTypeAlter.ContainsKey(o.TDL_TREATMENT_ID ?? 0) &&
                            treatmentTypeId(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0]) !=
                            IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU &&
                            treatmentTypeId(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0]) !=
                            IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && IsNotBedAfterIn(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0])).ToList();
                }

                hisDepartment = new HisDepartmentManager(paramGet).GetById(filter.DEPARTMENT_ID ?? 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool IsNotBedAfterIn(V_HIS_SERE_SERV thisData, List<V_HIS_PATIENT_TYPE_ALTER> list)
        {
            bool result = true;
            try
            {
                //không lấy dịch vụ phẫu thuật,giường của bn nếu bn đó sau đó vào nội trú.
                if (IsNotNullOrEmpty(list))
                {
                    var patientTypeIn = list.OrderBy(o => o.LOG_TIME).FirstOrDefault(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    var servicePatient = list.OrderBy(o => o.LOG_TIME).FirstOrDefault(o => o.TREATMENT_TYPE_ID == treatmentTypeId(thisData, list));
                    if (patientTypeIn != null && servicePatient != null && patientTypeIn.LOG_TIME > servicePatient.LOG_TIME)
                    {
                        if (IsBed(thisData) || thisData.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                        {
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = true;
            }
            return result;
        }

        private bool IsNotBed(V_HIS_SERE_SERV o)
        {
            return o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G;
        }

        private bool IsBed(V_HIS_SERE_SERV o)
        {
            return o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G;
        }

        private void depositService()
        {
            try
            {
                var listDeposit = ListTransactionBillDeposit.Where(o => ListCashierRoom.Select(p => p.ID).ToList().Contains(o.CASHIER_ROOM_ID) && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && o.TDL_SESE_DEPO_REPAY_COUNT > 0).ToList();

                List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();
                if (IsNotNullOrEmpty(listDeposit))
                {
                    HisSereServDepositFilterQuery sereServDepositFilter = new HisSereServDepositFilterQuery();
                    sereServDepositFilter.IS_CANCEL = false;
                    sereServDepositFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    sereServDepositFilter.CREATE_TIME_TO = filter.TIME_TO;
                    var listSereServDeposit = new HisSereServDepositManager(paramGet).Get(sereServDepositFilter);

                    if (IsNotNullOrEmpty(listSereServDeposit))
                    {
                        listSereServDeposit = listSereServDeposit.Where(o => listDeposit.Select(p => p.ID).Contains(o.DEPOSIT_ID)).ToList();
                        ListSereServDeposit.AddRange(listSereServDeposit);
                    }
                }

                //dịch vụ tạm ứng
                if (IsNotNullOrEmpty(ListSereServDeposit))
                {
                    listSereServId.AddRange(ListSereServDeposit.Select(o => o.SERE_SERV_ID).ToList());
                    listTreatmentId.AddRange(ListSereServDeposit.Select(o => o.TDL_TREATMENT_ID).ToList());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void billSevice()
        {
            try
            {
                var ListBill = ListTransactionBillDeposit.Where(o => ListCashierRoom.Select(p => p.ID).ToList().Contains(o.CASHIER_ROOM_ID) && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                //Danh sách các giao dịch thanh toán dịch vụ:
                if (IsNotNullOrEmpty(ListBill))
                {
                    HisSereServBillFilterQuery ssBillFilter = new HisSereServBillFilterQuery();
                    ssBillFilter.IS_NOT_CANCEL = true;
                    ssBillFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    ssBillFilter.CREATE_TIME_TO = filter.TIME_TO;
                    var listSSBillSub = new HisSereServBillManager(paramGet).Get(ssBillFilter);
                    if (IsNotNullOrEmpty(listSSBillSub))
                    {
                        listSSBillSub = listSSBillSub.Where(o => ListBill.Select(p => p.ID).Contains(o.BILL_ID)).ToList();
                        ListSereServBill.AddRange(listSSBillSub);
                    }
                }

                var lisTreatmentBillId = ListSereServBill.Select(o => o.TDL_TREATMENT_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(lisTreatmentBillId))
                {
                    var listSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();
                    var skip = 0;
                    while (lisTreatmentBillId.Count - skip > 0)
                    {
                        var listSub = lisTreatmentBillId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServDepositFilterQuery sereServDepositFilter = new HisSereServDepositFilterQuery();
                        sereServDepositFilter.IS_CANCEL = false;
                        sereServDepositFilter.TDL_TREATMENT_IDs = listSub;
                        sereServDepositFilter.CREATE_TIME_TO = filter.TIME_FROM;
                        var listSereServDepositSub = new HisSereServDepositManager(paramGet).Get(sereServDepositFilter);
                        if (IsNotNullOrEmpty(listSereServDepositSub)) listSereServDepositSub = listSereServDepositSub.Where(o => ListSereServBill.Select(p => p.SERE_SERV_ID).Contains(o.SERE_SERV_ID)).ToList();
                        if (IsNotNullOrEmpty(listSereServDepositSub))
                        {
                            listSereServDeposit.AddRange(listSereServDepositSub);
                        }
                    }
                    ListSereServBill = ListSereServBill.Where(o => !listSereServDeposit.Select(p => p.SERE_SERV_ID).Contains(o.SERE_SERV_ID)).ToList();
                }

                //dịch vụ thanh toán
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    listSereServId.AddRange(ListSereServBill.Select(o => o.SERE_SERV_ID).ToList());
                    listTreatmentId.AddRange(ListSereServBill.Select(o => o.TDL_TREATMENT_ID).ToList());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListRdo.Clear();

                if (IsNotNullOrEmpty(listSereServ))
                {
                    Inventec.Common.Logging.LogSystem.Debug(string.Join(", ", listSereServ.Select(o => new { st = o.TDL_SERVICE_CODE + "_" + o.ID.ToString() }).ToList()));
                    var groupByServiceId = listSereServ.GroupBy(o => new { o.SERVICE_ID, o.PRICE, o.VAT_RATIO }).ToList();
                    foreach (var group in groupByServiceId)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        Mrs00528RDO rdo = new Mrs00528RDO();
                        rdo.SERVICE_NAME = listSub.First().TDL_SERVICE_NAME;
                        rdo.VAT_RATIO = listSub.First().VAT_RATIO;
                        rdo.PRICE = listSub.First().PRICE * (1 + listSub.First().VAT_RATIO);
                        rdo.AMOUNT = listSub.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * (o.PRICE * (1 + o.VAT_RATIO)));
                        rdo.HEIN_SERVICE_TYPE_NUM_ORDER = listSub.First().HEIN_SERVICE_TYPE_NUM_ORDER ?? 100;
                        rdo.SERVICE_TYPE_ID = listSub.First().TDL_SERVICE_TYPE_ID;
                        rdo.SERVICE_TYPE_NAME = listSub.First().SERVICE_TYPE_NAME;
                        ListRdo.Add(rdo);
                    }
                    ListRdo = ListRdo.OrderBy(q => q.HEIN_SERVICE_TYPE_NUM_ORDER).Where(o => o.VIR_TOTAL_PRICE > 0).ToList();

                    ListGroup = ListRdo.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).OrderBy(q => q.HEIN_SERVICE_TYPE_NUM_ORDER).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            if ((long)((Mrs00528Filter)reportFilter).BRANCH_ID != 0)
                dicSingleTag.Add("BRANCH_NAME", Branch.BRANCH_NAME);
            else dicSingleTag.Add("BRANCH_NAME", "");
            if (((Mrs00528Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00528Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00528Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00528Filter)reportFilter).TIME_TO));
            }
            Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count.ToString());
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Group", ListGroup);
            objectTag.AddRelationship(store, "Group", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            if (this.hisDepartment != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", this.hisDepartment.DEPARTMENT_NAME);
                dicSingleTag.Add("DEPARTMENT_CODE", this.hisDepartment.DEPARTMENT_CODE);
            }
        }

        private long treatmentTypeId(V_HIS_SERE_SERV thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.LOG_TIME <= thisData.TDL_INTRUCTION_TIME).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID;
                else
                {
                    LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.LOG_TIME >= thisData.TDL_INTRUCTION_TIME).ToList();
                    if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                        result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().TREATMENT_TYPE_ID;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }
    }
}
