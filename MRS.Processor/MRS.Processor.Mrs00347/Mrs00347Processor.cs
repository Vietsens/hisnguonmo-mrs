using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisDepositReq;
using MOS.MANAGER.HisHeinApproval;


namespace MRS.Processor.Mrs00347
{
    public class Mrs00347Processor : AbstractProcessor
    {
        List<Mrs00347RDO> ListRdo = new List<Mrs00347RDO>();
        List<Mrs00347RDO> ListRdoDepartment = new List<Mrs00347RDO>();
        List<Mrs00347RDODepatmentDetail> ListDepartmentDetail = new List<Mrs00347RDODepatmentDetail>();
        Mrs00347Filter castFilter = new Mrs00347Filter();
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>();
        List<HIS_TREATMENT_STT> ListTreatmentStt = new List<HIS_TREATMENT_STT>();
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>();
        Dictionary<long, List<HIS_TRANSACTION>> dicTransaction = new Dictionary<long, List<HIS_TRANSACTION>>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_SERE_SERV> listSereServExpend = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<HIS_DEPOSIT_REQ> ListDepositreq = new List<HIS_DEPOSIT_REQ>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        CommonParam paramGet = new CommonParam();
        public Mrs00347Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00347Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00347Filter)this.reportFilter);
                ListPatientType = new HisPatientTypeManager().Get(new HisPatientTypeFilterQuery());
                // V_HIS_TREATMENT: lấy danh sách bệnh nhân xuất viện


                ListTreatmentStt = new ManagerSql().GetTreatmentSTT();
                if (castFilter.INPUT_DATA_ID_TIME_TYPE.HasValue)
                {

                    if (castFilter.INPUT_DATA_ID_TIME_TYPE.Value == 3)
                    {
                        HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();
                        treatmentFeeViewFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFeeViewFilter.IN_TIME_TO = castFilter.TIME_TO;
                        if (castFilter.IS_ACTIVE == true)
                        {
                            treatmentFeeViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                        }
                        var treatmentfee = new HisTreatmentManager().GetFeeView(treatmentFeeViewFilter);

                        if (IsNotNullOrEmpty(treatmentfee))
                        {
                            listTreatmentFees.AddRange(treatmentfee);
                        }
                    }
                    else if (castFilter.INPUT_DATA_ID_TIME_TYPE.Value == 1)
                    {
                        HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();
                        treatmentFeeViewFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFeeViewFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                        if (castFilter.IS_ACTIVE == true)
                        {
                            treatmentFeeViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                        }
                        var treatmentfee = new HisTreatmentManager().GetFeeView(treatmentFeeViewFilter);

                        if (IsNotNullOrEmpty(treatmentfee))
                        {
                            listTreatmentFees.AddRange(treatmentfee);
                        }
                    }
                    else if (castFilter.INPUT_DATA_ID_TIME_TYPE.Value == 2)
                    {
                        HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();
                        HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                        approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                        approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;

                        approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                        approvalFilter.ORDER_DIRECTION = "ASC";
                        ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager().GetView(approvalFilter);

                        if (IsNotNullOrEmpty(ListHeinApproval))
                        {
                            var treatmentIds = ListHeinApproval.Select(s => s.TREATMENT_ID).Distinct().ToList();

                            int skip = 0;
                            while (treatmentIds.Count - skip > 0)
                            {
                                var listId = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                                treatmentFeeViewFilter.IDs = listId;


                                if (castFilter.IS_ACTIVE == true)
                                {
                                    treatmentFeeViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                                }
                                var treatmentfee = new HisTreatmentManager().GetFeeView(treatmentFeeViewFilter);

                                if (IsNotNullOrEmpty(treatmentfee))
                                {
                                    listTreatmentFees.AddRange(treatmentfee);
                                }
                            }

                        }
                        ListHeinApproval = ListHeinApproval.Where(o => listTreatmentFees.Exists(p => p.ID == o.TREATMENT_ID)).ToList();
                    }

                }
                else
                {
                    HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();

                    if (castFilter.TRUE_FALSE == true)
                    {
                        treatmentFeeViewFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFeeViewFilter.IN_TIME_TO = castFilter.TIME_TO;
                    }
                    else if (castFilter.IS_FEE_LOCK_TIME == true)
                    {
                        treatmentFeeViewFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFeeViewFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    }
                    else
                    {
                        treatmentFeeViewFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFeeViewFilter.OUT_TIME_TO = castFilter.TIME_TO;
                    }

                    if (castFilter.IS_ACTIVE == true)
                    {
                        treatmentFeeViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    }
                    listTreatmentFees = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(treatmentFeeViewFilter);
                }




                if (IsNotNullOrEmpty(castFilter.END_DEPARTMENT_IDs))
                {
                    listTreatmentFees = listTreatmentFees.Where(o => castFilter.END_DEPARTMENT_IDs.Contains(o.LAST_DEPARTMENT_ID ?? 0)).ToList();
                }
                if (castFilter.BRANCH_IDs != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => castFilter.BRANCH_IDs.Contains(o.BRANCH_ID)).ToList();
                }

                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.PATIENT_TYPE_IDs != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.CASHIER_LOGINNAMEs != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.FEE_LOCK_LOGINNAME ?? "")).ToList();
                }
                if (castFilter.TREATMENT_END_TYPE_IDs != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => castFilter.TREATMENT_END_TYPE_IDs.Contains(o.TREATMENT_END_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.IS_PATIENT_PRICE_ZERO == true)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => Math.Round(o.TOTAL_PATIENT_PRICE ?? 0) == 0).ToList();
                }
                var listTreatmentId = listTreatmentFees.Select(o => o.ID).ToList();
                if (castFilter.IS_NOT_REQUIRED_DETAIL != true)
                {
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                            //Dich vu hao phi
                            HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                            sereServFilter.TREATMENT_IDs = listIDs;
                            //sereServFilter.IS_EXPEND = true;
                            var ss = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).Get(sereServFilter);
                            if (ss != null)
                            {
                                listSereServ.AddRange(ss);
                            }
                            //yêu cầu tạm ứng
                            HisDepositReqFilterQuery depoReqFilter = new HisDepositReqFilterQuery();
                            depoReqFilter.TREATMENT_IDs = listIDs;
                            var depoReq = new MOS.MANAGER.HisDepositReq.HisDepositReqManager().Get(depoReqFilter);
                            if (depoReq != null)
                            {
                                ListDepositreq.AddRange(depoReq);
                            }
                        }
                    }
                    //lọc bỏ các yêu cầu chưa thực hiện
                    ListDepositreq = ListDepositreq.Where(o => o.DEPOSIT_ID.HasValue).ToList();

                    listSereServExpend = listSereServ.Where(o => o.IS_EXPEND == 1).ToList();
                }
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var ListTransaction = new List<HIS_TRANSACTION>();
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        //Giao dich
                        HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                        transactionFilter.TREATMENT_IDs = listIDs;
                        transactionFilter.IS_CANCEL = false;
                        var transactionSub = new HisTransactionManager(paramGet).Get(transactionFilter);
                        if (transactionSub != null)
                        {
                            ListTransaction.AddRange(transactionSub);
                        }
                    }
                    dicTransaction = ListTransaction.GroupBy(o => o.TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());
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
                if (IsNotNullOrEmpty(listTreatmentFees))
                {
                    long number = 0;
                    foreach (var treatment in listTreatmentFees)
                    {
                        number++;
                        var rdo = new Mrs00347RDO();
                        rdo.NUMBER = number;
                        rdo.TREATMENT_ID = treatment.ID;
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.DOB = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        rdo.TDL_PATIENT_DOB = treatment.TDL_PATIENT_DOB;
                        rdo.GENDER = treatment.TDL_PATIENT_GENDER_NAME;
                        rdo.TDL_PATIENT_PHONE = treatment.TDL_PATIENT_PHONE ?? treatment.TDL_PATIENT_MOBILE;
                        rdo.END_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID ?? treatment.LAST_DEPARTMENT_ID;
                        if (treatment.END_DEPARTMENT_ID.HasValue)
                        {
                            rdo.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.END_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        }
                        else
                        {
                            var currentDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID);
                            if (currentDepartment != null)
                            {
                                rdo.END_DEPARTMENT_NAME = currentDepartment.DEPARTMENT_NAME;
                                rdo.END_DEPARTMENT_CODE = currentDepartment.DEPARTMENT_CODE;
                            }
                        }
                        rdo.END_ROOM_ID = treatment.END_ROOM_ID ?? treatment.IN_ROOM_ID ?? treatment.TDL_FIRST_EXAM_ROOM_ID;
                        if (rdo.END_ROOM_ID.HasValue)
                        {
                            rdo.END_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                            rdo.END_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        }


                        rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                        if (treatment.OUT_TIME.HasValue)
                        {
                            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                        }
                        if (treatment.FEE_LOCK_TIME.HasValue)
                        {
                            rdo.FEE_LOCK_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME.Value);
                        }
                        var patientType = ListPatientType.Where(x => x.ID == treatment.TDL_PATIENT_TYPE_ID).FirstOrDefault();
                        if (patientType != null)
                        {
                            rdo.TDL_PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                            rdo.TDL_PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                        }
                        var treatStt = ListTreatmentStt.Where(x => x.ID == treatment.TREATMENT_STT_ID).FirstOrDefault();
                        if (treatStt != null)
                        {
                            rdo.TREATMENT_STT_NAME = treatStt.TREATMENT_STT_NAME;
                        }
                        rdo.WORK_PLACE_NAME = treatment.TDL_PATIENT_WORK_PLACE_NAME;
                        rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.ICD_CODE = treatment.ICD_CODE;
                        rdo.ICD_NAME = treatment.ICD_NAME;

                        rdo.TOTAL_PRICE = treatment.TOTAL_PRICE ?? 0;
                        rdo.TOTAL_HEIN_PRICE = treatment.TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_PATIENT_PRICE = treatment.TOTAL_PATIENT_PRICE ?? 0;
                        rdo.TOTAL_DEPOSIT_AMOUNT = treatment.TOTAL_DEPOSIT_AMOUNT ?? 0;
                        rdo.TOTAL_BILL_AMOUNT = treatment.TOTAL_BILL_AMOUNT ?? 0;
                        rdo.BILL_NUM_ORDER = dicTransaction.ContainsKey(treatment.ID)?string.Join(", ",dicTransaction[treatment.ID].Where(o=>o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(p=>p.NUM_ORDER).ToList()):null;

                        rdo.TOTAL_BILL_FUND = treatment.TOTAL_BILL_FUND ?? 0;
                        rdo.TOTAL_BILL_OTHER_AMOUNT = treatment.TOTAL_BILL_OTHER_AMOUNT ?? 0;
                        rdo.TOTAL_BILL_TRANSFER_AMOUNT = treatment.TOTAL_BILL_TRANSFER_AMOUNT ?? 0;
                        rdo.TOTAL_REPAY_AMOUNT = treatment.TOTAL_REPAY_AMOUNT ?? 0;
                        rdo.TOTAL_DISCOUNT = treatment.TOTAL_DISCOUNT ?? 0;
                        rdo.TOTAL_BILL_EXEMPTION = treatment.TOTAL_BILL_EXEMPTION ?? 0;

                        rdo.IS_LOCK_HEIN = treatment.IS_LOCK_HEIN;
                        rdo.IS_ACTIVE = treatment.IS_ACTIVE;
                        rdo.IS_PAUSE = treatment.IS_PAUSE;
                        rdo.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID;
                        var Residual = (rdo.TOTAL_DEPOSIT_AMOUNT - rdo.TOTAL_REPAY_AMOUNT + (rdo.TOTAL_BILL_AMOUNT - rdo.TOTAL_BILL_TRANSFER_AMOUNT)) - (rdo.TOTAL_PATIENT_PRICE);

                        var money = -Residual;
                        if (castFilter.IS_OWE == true && money <= 0)
                        {
                            continue;
                        }

                        if (money >= 0)
                        {
                            rdo.PATIENT_PRICE = money;
                            rdo.GIVE_BACK = 0;
                            if (castFilter.IS_GIVE_BACK == true)
                            {
                                continue;
                            }
                        }
                        else if (money < 0)
                        {
                            rdo.PATIENT_PRICE = 0;
                            rdo.GIVE_BACK = money;
                            rdo.NOTE = "BV nợ bệnh nhân";
                        }

                        rdo.EXPEND_PRICE = listSereServExpend.Where(o => o.TDL_TREATMENT_ID == treatment.ID).Sum(s => s.AMOUNT * s.PRICE);
                        ListRdo.Add(rdo);
                        if (castFilter.IS_NOT_REQUIRED_DETAIL != true)
                        {
                            ProcessDepartmentDetail(treatment);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessDepartmentDetail(V_HIS_TREATMENT_FEE treatment)
        {
            try
            {
                if (IsNotNull(treatment))
                {
                    List<long> lstDepaIds = new List<long>();
                    var lstDepositReq = ListDepositreq.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                    var lstSs = listSereServ.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();

                    if (IsNotNullOrEmpty(lstDepositReq))
                    {
                        lstDepaIds = lstDepositReq.OrderByDescending(o => o.CREATE_TIME).Select(s => s.REQUEST_DEPARTMENT_ID).ToList();
                    }

                    if (IsNotNullOrEmpty(lstSs))
                    {
                        lstDepaIds = lstSs.OrderByDescending(o => o.TDL_INTRUCTION_TIME).Select(s => s.TDL_REQUEST_DEPARTMENT_ID).ToList();
                    }

                    lstDepaIds = lstDepaIds.Distinct().ToList();

                    foreach (var item in lstDepaIds)
                    {
                        Mrs00347RDODepatmentDetail rdo = new Mrs00347RDODepatmentDetail();
                        var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item) ?? new HIS_DEPARTMENT();

                        rdo.TREATMENT_ID = treatment.ID;
                        rdo.DEPARTMENT_ID = item;
                        rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;

                        rdo.TOTAL_DEPOSIT_AMOUNT = lstDepositReq.Where(o => o.REQUEST_DEPARTMENT_ID == item).Sum(s => s.AMOUNT);
                        rdo.TOTAL_PATIENT_PRICE = lstSs.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == item).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                        var money = rdo.TOTAL_PATIENT_PRICE - rdo.TOTAL_DEPOSIT_AMOUNT;
                        if (money >= 0)
                        {
                            rdo.PATIENT_PRICE = money;
                            rdo.GIVE_BACK = 0;
                        }
                        else if (money < 0)
                        {
                            rdo.PATIENT_PRICE = 0;
                            rdo.GIVE_BACK = money;
                        }

                        ListDepartmentDetail.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<Mrs00347RDO> ProcessGroupDepartment(List<Mrs00347RDO> listRdo)
        {
            List<Mrs00347RDO> result = new List<Mrs00347RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    result = listRdo.GroupBy(o => o.END_DEPARTMENT_ID).Select(s =>
                        new Mrs00347RDO()
                        {
                            END_DEPARTMENT_CODE = s.First().END_DEPARTMENT_CODE,
                            END_DEPARTMENT_ID = s.First().END_DEPARTMENT_ID,
                            END_DEPARTMENT_NAME = s.First().END_DEPARTMENT_NAME,
                            TOTAL_BILL_AMOUNT = s.Sum(t => t.TOTAL_BILL_AMOUNT),
                            TOTAL_HEIN_PRICE = s.Sum(t => t.TOTAL_HEIN_PRICE),
                            TOTAL_PATIENT_PRICE = s.Sum(t => t.TOTAL_PATIENT_PRICE),
                            TOTAL_PRICE = s.Sum(t => t.TOTAL_PRICE),
                            PATIENT_PRICE = s.Sum(t => t.PATIENT_PRICE),
                            GIVE_BACK = s.Sum(t => t.GIVE_BACK),
                            EXPEND_PRICE = s.Sum(t => t.EXPEND_PRICE),
                            TOTAL_BILL_FUND = s.Sum(t => t.TOTAL_BILL_FUND),
                            TOTAL_BILL_OTHER_AMOUNT = s.Sum(t => t.TOTAL_BILL_OTHER_AMOUNT),
                            TOTAL_BILL_TRANSFER_AMOUNT = s.Sum(t => t.TOTAL_BILL_TRANSFER_AMOUNT),
                            TOTAL_REPAY_AMOUNT = s.Sum(t => t.TOTAL_REPAY_AMOUNT),
                            TOTAL_DISCOUNT = s.Sum(t => t.TOTAL_DISCOUNT),
                            TOTAL_BILL_EXEMPTION = s.Sum(t => t.TOTAL_BILL_EXEMPTION),
                            TOTAL_DEPOSIT_AMOUNT = s.Sum(t => t.TOTAL_DEPOSIT_AMOUNT)
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                result = new List<Mrs00347RDO>();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                dicSingleTag.Add("CREATE_TIME", "Ngày " + (DateTime.Now.Day < 10 ? ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString()) + " tháng " + (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString()) + " năm " + DateTime.Now.Year);

                ListRdoDepartment = ProcessGroupDepartment(ListRdo);

                var listReport = ListRdo.Where(o => Math.Round(o.PATIENT_PRICE) > 0 || Math.Round(o.GIVE_BACK) > 0).ToList();
                var listReportDepartment = ProcessGroupDepartment(listReport);
                var listReportDepartmentDetail = ListDepartmentDetail.Where(o => listReport.Select(s => s.TREATMENT_ID).Contains(o.TREATMENT_ID)).ToList();

                var listReportBHYT = listReport.Where(x => x.TDL_PATIENT_TYPE_NAME == "BHYT").ToList();
                var listReportDepartmentBHYT = ProcessGroupDepartment(listReportBHYT);

                objectTag.AddObjectData(store, "Rdo", ListRdo);
                objectTag.AddObjectData(store, "RdoDepartment", ListRdoDepartment);
                objectTag.AddRelationship(store, "RdoDepartment", "Rdo", "END_DEPARTMENT_ID", "END_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "RdoDepartmentDetail", ListDepartmentDetail);
                objectTag.AddRelationship(store, "Rdo", "RdoDepartmentDetail", "TREATMENT_ID", "TREATMENT_ID");

                objectTag.AddObjectData(store, "Report", listReport);
                objectTag.AddObjectData(store, "ReportBHYT", listReportBHYT);
                objectTag.AddObjectData(store, "ReportDepartment", listReportDepartment);
                objectTag.AddObjectData(store, "ReportDepartmentBHYT", listReportDepartmentBHYT);
                objectTag.AddRelationship(store, "ReportDepartment", "Report", "END_DEPARTMENT_ID", "END_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "ReportDepartmentDetail", ListDepartmentDetail);
                objectTag.AddRelationship(store, "Report", "ReportDepartmentDetail", "TREATMENT_ID", "TREATMENT_ID");

                objectTag.AddObjectData(store, "Transaction", dicTransaction.SelectMany(o=>o.Value).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
