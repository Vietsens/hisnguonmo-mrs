using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using FlexCel.Report;
using HIS.Common.Treatment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisPatientCase;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00020
{
    public class Mrs00020Processor : AbstractProcessor
    {
        Mrs00020Filter castFilter = null;
        List<Mrs00020RDO> ListRdo = new List<Mrs00020RDO>();
        List<Mrs00020RDO> ListRdoA = new List<Mrs00020RDO>();
        List<Mrs00020RDO> ListRdoB = new List<Mrs00020RDO>();
        List<Mrs00020RDO> ListRdoC = new List<Mrs00020RDO>();
        //Dictionary<long, Mrs00020RDO> dicTreatmentRdo = new Dictionary<long, Mrs00020RDO>();
        //Dictionary<long, Mrs00020RDO> dicTreatmentA = new Dictionary<long, Mrs00020RDO>();
        //Dictionary<long, Mrs00020RDO> dicTreatmentB = new Dictionary<long, Mrs00020RDO>();
        //Dictionary<long, Mrs00020RDO> dicTreatmentC = new Dictionary<long, Mrs00020RDO>();
        List<Mrs00020RDO> ListSumTotal = new List<Mrs00020RDO>();
        List<Mrs00020RDO> Departments = new List<Mrs00020RDO>();
        List<Mrs00020RDO> listRdoAll = new List<Mrs00020RDO>();
        Dictionary<string, Mrs00020RDO> dicDepartment = new Dictionary<string, Mrs00020RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_PATIENT_CASE> ListPatientCase = new List<HIS_PATIENT_CASE>();
        Dictionary<long, V_HIS_SERVICE> dicParent = new Dictionary<long, V_HIS_SERVICE>();
        List<AccumTreatment> ListAccumTreatment = new List<AccumTreatment>();
        HIS_BRANCH _Branch = null;
        string MaterialPriceOption = "";
        short HasSereServ = 0;
        private decimal TotalAmount = 0;
        List<long> treatmentIds = new List<long>();

        public Mrs00020Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00020Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = true;
            try
            {

                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                castFilter = ((Mrs00020Filter)this.reportFilter);
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => this.castFilter.BRANCH_ID == null || o.ID == this.castFilter.BRANCH_ID);
                LoadDataToRam();

                ListAccumTreatment = new ManagerSql().GetAccumTreatment(castFilter) ?? new List<AccumTreatment>();


                //lọc nội tỉnh ngoại tỉnh
                FilterProvinceType();

                //lọc đúng tuyến trái tuyến
                FilterRouteType();


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private void FilterRouteType()
        {
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 1)
            {
                ListHeinApproval = ListHeinApproval.Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE || !string.IsNullOrWhiteSpace(o.RIGHT_ROUTE_TYPE_CODE)).ToList();
            }
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 2)
            {
                ListHeinApproval = ListHeinApproval.Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE).ToList();
            }
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 3)
            {
                ListHeinApproval = ListHeinApproval.Where(o => string.IsNullOrWhiteSpace(o.RIGHT_ROUTE_CODE) && string.IsNullOrWhiteSpace(o.RIGHT_ROUTE_TYPE_CODE)).ToList();
            }
        }

        private void FilterProvinceType()
        {
            if (castFilter.INPUT_DATA_ID_PROVINCE_TYPE == 1)
            {
                ListHeinApproval = ListHeinApproval.Where(o => checkBhytProvinceCode(o.HEIN_CARD_NUMBER)).ToList();
            }
            if (castFilter.INPUT_DATA_ID_PROVINCE_TYPE == 2)
            {
                ListHeinApproval = ListHeinApproval.Where(o => !checkBhytProvinceCode(o.HEIN_CARD_NUMBER)).ToList();
            }
        }

        private void LoadDataToRam()
        {
            if (castFilter.TIME_FROM != null && castFilter.TIME_TO != null && castFilter.INPUT_DATA_ID_TIME_TYPE != 1)
            {
                castFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                castFilter.OUT_TIME_TO = castFilter.TIME_TO;
            }
            if (castFilter.TIME_FROM == null && castFilter.TIME_TO == null && castFilter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                castFilter.TIME_FROM = castFilter.OUT_TIME_FROM;
                castFilter.TIME_TO = castFilter.OUT_TIME_TO;
                castFilter.OUT_TIME_FROM = null;
                castFilter.OUT_TIME_TO = null;
            }
            if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
            {
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                // nếu loại thời gian là khóa viện phí
                if (castFilter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.FEE_LOCK_TIME_TO = castFilter.OUT_TIME_TO;
                }
                // nếu loại thời gian là ra viện
                else if (castFilter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    treatmentFilter.IS_PAUSE = true;
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                }
                else
                {
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                }
                treatmentFilter.IS_PAUSE = true;
                if (castFilter.END_DEPARTMENT_IDs != null)
                {
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                }
                treatmentFilter.END_ROOM_IDs = castFilter.END_ROOM_IDs;
                ListTreatment = new HisTreatmentManager().GetView(treatmentFilter);
                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.FEE_LOCK_ROOM_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.FEE_LOCK_ROOM_IDs.Contains(o.FEE_LOCK_ROOM_ID ?? 0)).ToList();
                }
                if (castFilter.FEE_LOCK_LOGINNAMEs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.FEE_LOCK_LOGINNAMEs.Contains(o.FEE_LOCK_LOGINNAME ?? "")).ToList();
                }
                ListTreatment = ListTreatment.Where(o => HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT == o.TDL_PATIENT_TYPE_ID).ToList();
                int skip = 0;
                while (ListTreatment.Count - skip > 0)
                {
                    var listId = ListTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.TREATMENT_IDs = listId.Select(s => s.ID).ToList();
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    var hein = new HisHeinApprovalManager().GetView(approvalFilter);

                    if (IsNotNullOrEmpty(hein))
                    {
                        ListHeinApproval.AddRange(hein);
                    }
                }
            }
            else
            {
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);

                Inventec.Common.Logging.LogSystem.Info("ListHeinApproval" + ListHeinApproval.Count);
                ListTreatment = new List<V_HIS_TREATMENT>();
                List<long> ListTreatmentIds = ListHeinApproval.Select(o => o.TREATMENT_ID).Distinct().ToList();
                int skip = 0;
                while (ListTreatmentIds.Count - skip > 0)
                {
                    var listId = ListTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.IDs = listId;
                    treatmentFilter.END_ROOM_IDs = castFilter.END_ROOM_IDs;
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;

                    var treatment = new HisTreatmentManager().GetView(treatmentFilter);

                    if (castFilter.TREATMENT_TYPE_IDs != null)
                    {
                        treatment = treatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    }

                    if (castFilter.FEE_LOCK_ROOM_IDs != null)
                    {
                        treatment = treatment.Where(o => castFilter.FEE_LOCK_ROOM_IDs.Contains(o.FEE_LOCK_ROOM_ID ?? 0)).ToList();
                    }
                    if (castFilter.FEE_LOCK_LOGINNAMEs != null)
                    {
                        treatment = treatment.Where(o => castFilter.FEE_LOCK_LOGINNAMEs.Contains(o.FEE_LOCK_LOGINNAME ?? "")).ToList();
                    }
                    if (IsNotNullOrEmpty(treatment))
                    {
                        ListTreatment.AddRange(treatment);
                    }
                }

            }
            if (castFilter.PATIENT_CLASSIFY_IDs != null)
            {
                ListTreatment = ListTreatment.Where(o => castFilter.PATIENT_CLASSIFY_IDs.Contains(o.TDL_PATIENT_CLASSIFY_ID ?? 0)).ToList();
            }
            var treatmentIds = ListTreatment.Select(o => o.ID).ToList();
            ListHeinApproval = ListHeinApproval.Where(o => treatmentIds.Contains(o.TREATMENT_ID)).ToList();
            HisPatientCaseFilterQuery pcFilter = new HisPatientCaseFilterQuery();
            ListPatientCase = new HisPatientCaseManager().Get(pcFilter);


            GetParent();
        }

        private void GetParent()
        {
            try
            {
                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                var services = new HisServiceManager(new CommonParam()).GetView(serviceFilter);
                if (services != null)
                {
                    foreach (var item in services)
                    {
                        var parent = new V_HIS_SERVICE();
                        dicParent.Add(item.ID, parent);

                        var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();

                        parent.NUM_ORDER = (serviceType.NUM_ORDER ?? 999) * 1000;
                        parent.SERVICE_CODE = string.Format("{0}_", serviceType.SERVICE_TYPE_CODE);
                        parent.SERVICE_NAME = serviceType.SERVICE_TYPE_NAME;
                        if (item.PARENT_ID != null)
                        {
                            var pr = services.FirstOrDefault(o => o.ID == item.PARENT_ID);
                            if (pr != null)
                            {
                                parent.NUM_ORDER += pr.NUM_ORDER;
                                parent.SERVICE_CODE += pr.SERVICE_CODE;
                                parent.SERVICE_NAME = pr.SERVICE_NAME;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessListHeinApproval();

                listRdoAll.AddRange(ListRdoA ?? new List<Mrs00020RDO>());
                listRdoAll.AddRange(ListRdoB ?? new List<Mrs00020RDO>());
                listRdoAll.AddRange(ListRdoC ?? new List<Mrs00020RDO>());
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_TREA") && this.dicDataFilter["KEY_GROUP_TREA"] != null)
                {
                    string KeyGroupTrea = this.dicDataFilter["KEY_GROUP_TREA"].ToString();
                    GroupByKey(KeyGroupTrea);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GroupByKey(string KeyGroupTrea)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(KeyGroupTrea))
                {
                    //var group = listRdoAll.GroupBy(o => string.Format(KeyGroupTrea, o.END_DEPARTMENT_ID,o.END_ROOM_DEPARTMENT_ID, o.END_ROOM_ID, o.EXECUTE_DATE)).ToList();
                    var group = listRdoAll.GroupBy(o => new { KeyGroupTrea, o.END_DEPARTMENT_ID, o.END_ROOM_ID, o.EXECUTE_DATE }).ToList();
                    listRdoAll.Clear();
                    listRdoAll = new List<Mrs00020RDO>();
                    foreach (var item in group)
                    {
                        List<Mrs00020RDO> list = item.ToList<Mrs00020RDO>();
                        Mrs00020RDO first = item.First();
                        Mrs00020RDO rdo = new Mrs00020RDO();
                        rdo.END_DEPARTMENT_ID = first.END_DEPARTMENT_ID;
                        rdo.END_DEPARTMENT_CODE = first.END_DEPARTMENT_CODE;
                        rdo.END_DEPARTMENT_NAME = first.END_DEPARTMENT_NAME;
                        rdo.END_ROOM_DEPARTMENT_ID = first.END_ROOM_DEPARTMENT_ID;
                        rdo.END_ROOM_DEPARTMENT_NAME = first.END_ROOM_DEPARTMENT_NAME;
                        rdo.END_ROOM_DEPARTMENT_CODE = first.END_ROOM_DEPARTMENT_CODE;
                        rdo.END_ROOM_ID = first.END_ROOM_ID;
                        rdo.END_ROOM_CODE = first.END_ROOM_CODE;
                        rdo.END_ROOM_NAME = first.END_ROOM_NAME;
                        rdo.EXECUTE_DATE = first.EXECUTE_DATE;
                        rdo.TOTAL_PRICE = list.Sum(s => s.TOTAL_PRICE);
                        rdo.TEST_PRICE = list.Sum(s => s.TEST_PRICE);
                        rdo.DIIM_PRICE = list.Sum(s => s.DIIM_PRICE);
                        rdo.MEDICINE_PRICE = list.Sum(s => s.MEDICINE_PRICE);
                        rdo.MEDICINE_PRICE_NDM = list.Sum(s => s.MEDICINE_PRICE_NDM);
                        rdo.BLOOD_PRICE = list.Sum(s => s.BLOOD_PRICE);
                        rdo.CPM_PRICE = list.Sum(s => s.CPM_PRICE);
                        rdo.FUEX_PRICE = list.Sum(s => s.FUEX_PRICE);
                        rdo.SURGMISU_PRICE = list.Sum(s => s.SURGMISU_PRICE);
                        rdo.MATERIAL_PRICE = list.Sum(s => s.MATERIAL_PRICE);
                        rdo.SERVICE_PRICE_RATIO = list.Sum(s => s.SERVICE_PRICE_RATIO);
                        rdo.MEDICINE_PRICE_RATIO = list.Sum(s => s.MEDICINE_PRICE_RATIO);
                        rdo.MATERIAL_PRICE_RATIO = list.Sum(s => s.MATERIAL_PRICE_RATIO);
                        rdo.EXAM_PRICE = list.Sum(s => s.EXAM_PRICE);
                        rdo.BED_PRICE = list.Sum(s => s.BED_PRICE);
                        rdo.TRAN_PRICE = list.Sum(s => s.TRAN_PRICE);
                        rdo.TT_PRICE = list.Sum(s => s.TT_PRICE);
                        rdo.TOTAL_PATIENT_PRICE = list.Sum(s => s.TOTAL_PATIENT_PRICE);
                        rdo.TOTAL_PATIENT_PRICE_BHYT = list.Sum(s => s.TOTAL_PATIENT_PRICE_BHYT);
                        rdo.TOTAL_HEIN_PRICE = list.Sum(s => s.TOTAL_HEIN_PRICE);
                        rdo.TOTAL_HEIN_PRICE_NDS = list.Sum(s => s.TOTAL_HEIN_PRICE_NDS);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = list.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
                        rdo.COUNT_TREATMENT = list.GroupBy(o => o.TREATMENT_CODE).Select(o => o.First()).Count();
                        listRdoAll.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListHeinApproval()
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    ListHeinApproval = ListHeinApproval.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM).ToList();
                }

                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
                        List<V_HIS_TRANSACTION> ListTransaction = new List<V_HIS_TRANSACTION>();

                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        var ListSereServ = new HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (castFilter.SERVICE_TYPE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID)).ToList();
                        }
                        if (castFilter.SERVICE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => castFilter.SERVICE_IDs.Contains(o.SERVICE_ID)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(x => castFilter.PATIENT_TYPE_IDs.Contains(x.PATIENT_TYPE_ID)).ToList();
                        }
                        if (castFilter.OTHER_PAY_SOURCE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(x => castFilter.OTHER_PAY_SOURCE_IDs.Contains(x.OTHER_PAY_SOURCE_ID ?? 0)).ToList();
                            var ssIds = ListSereServ.Select(o => o.HEIN_APPROVAL_ID ?? 0).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => ssIds.Contains(o.ID)).ToList();
                        }
                        List<HIS_SERVICE_REQ> serviceReqs = this.GetServiceReq(ListSereServ);
                        HisSereServBillFilterQuery billFilter = new HisSereServBillFilterQuery();
                        billFilter.SERE_SERV_IDs = ListSereServ.Select(x => x.ID).Distinct().ToList();
                        var Bills = new HisSereServBillManager(paramGet).Get(billFilter);
                        Bills = Bills.Where(o => o.IS_CANCEL == null).ToList();
                        ListSereServBill.AddRange(Bills);
                        HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                        tranFilter.IDs = Bills.Select(x => x.BILL_ID).Distinct().ToList();
                        tranFilter.IS_CANCEL = false;
                        var trans = new HisTransactionManager(paramGet).GetView(tranFilter);


                        if (castFilter.BHYT100 == true && castFilter.PAY_FORM_IDs != null)
                        {

                            var treaIdsBHYT100 = ListSereServ.Where(o => o.VIR_TOTAL_PATIENT_PRICE == 0).Select(p => p.TDL_TREATMENT_ID).Distinct().ToList();
                            var treaIds = trans.Where(o => castFilter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).Select(p => p.TREATMENT_ID ?? 0).Distinct().ToList();


                            hisHeinApprovals = hisHeinApprovals.Where(o => treaIds.Contains(o.TREATMENT_ID) || treaIdsBHYT100.Contains(o.TREATMENT_ID)).ToList();
                            ListSereServ = ListSereServ.Where(o => treaIds.Contains(o.TDL_TREATMENT_ID ?? 0) || treaIdsBHYT100.Contains(o.TDL_TREATMENT_ID)).ToList();
                            serviceReqs = serviceReqs.Where(o => treaIds.Contains(o.TREATMENT_ID) || treaIdsBHYT100.Contains(o.TREATMENT_ID)).ToList();

                        }
                    
                        if (castFilter.BHYT100 == true && castFilter.PAY_FORM_IDs == null)
                        {

                            var treaIdsBHYT100 = ListSereServ.Where(o => o.VIR_TOTAL_PATIENT_PRICE == 0).Select(p => p.TDL_TREATMENT_ID).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treaIdsBHYT100.Contains(o.TREATMENT_ID)).ToList();
                            ListSereServ = ListSereServ.Where(o => treaIdsBHYT100.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                            serviceReqs = serviceReqs.Where(o => treaIdsBHYT100.Contains(o.TREATMENT_ID)).ToList();
                        
                        }
                        
                        //loc theo hinh thuc thanh toan
                        if (castFilter.PAY_FORM_IDs != null && castFilter.BHYT100 != true)
                        {
                            
                            var treaIds = trans.Where(o => castFilter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).Select(p => p.TREATMENT_ID ?? 0).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treaIds.Contains(o.TREATMENT_ID)).ToList();
                            ListSereServ = ListSereServ.Where(o => treaIds.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                            serviceReqs = serviceReqs.Where(o => treaIds.Contains(o.TREATMENT_ID)).ToList();
                        }
                        //end loc theo hinh thuc thanh toan

                        //loc theo trang thai giao dich
                        if (castFilter.INPUT_DATA_ID_STTRAN_TYPE != null && (castFilter.INPUT_DATA_ID_STTRAN_TYPE == 1 || castFilter.INPUT_DATA_ID_STTRAN_TYPE == 2))
                        {
                            var treaIds = trans.Where(o => castFilter.INPUT_DATA_ID_STTRAN_TYPE == 1 && o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE || castFilter.INPUT_DATA_ID_STTRAN_TYPE == 2 && o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.TREATMENT_ID ?? 0).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treaIds.Contains(o.TREATMENT_ID)).ToList();
                            ListSereServ = ListSereServ.Where(o => treaIds.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                            serviceReqs = serviceReqs.Where(o => treaIds.Contains(o.TREATMENT_ID)).ToList();
                        }
                        //end loc theo trang thai giao dich

                        ListTransaction.AddRange(trans);
                        List<V_HIS_TREATMENT> treatment = new List<V_HIS_TREATMENT>();
                        if (!IsNotNullOrEmpty(ListTreatment))
                        {
                            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                            treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                            treatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        }
                        else
                        {
                            treatment.AddRange(ListTreatment);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00020");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, serviceReqs, treatment, ListSereServBill, ListTransaction);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoA.Clear();
                ListRdoB.Clear();
                ListRdoC.Clear();
            }
        }

        private List<HIS_SERVICE_REQ> GetServiceReq(List<V_HIS_SERE_SERV_3> ListSereServ)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                if (ListSereServ == null)
                {
                    return null;
                }
                CommonParam paramGet = new CommonParam();
                var listTreatmentId = ListSereServ.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH && o.TDL_TREATMENT_ID.HasValue).Select(p => p.TDL_TREATMENT_ID.Value).Distinct().ToList();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                    filterSr.TREATMENT_IDs = listIDs;
                    filterSr.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    filterSr.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                    filterSr.HAS_EXECUTE = true;
                    var listServiceReqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                    if (IsNotNullOrEmpty(listServiceReqSub))
                    {
                        result.AddRange(listServiceReqSub);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<HIS_SERVICE_REQ> serviceReqs, List<V_HIS_TREATMENT> ListTreatment, List<HIS_SERE_SERV_BILL> ListSereServBill, List<V_HIS_TRANSACTION> ListTransaction)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("hisHeinApprovals.Count  " + hisHeinApprovals.Count.ToString());
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
                    Inventec.Common.Logging.LogSystem.Info("ListTreatment.Count  " + ListTreatment.Count.ToString());

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var treatment in ListTreatment)
                        {
                            dicTreatment[treatment.ID] = treatment;
                        }
                    }

                    Inventec.Common.Logging.LogSystem.Info("ListSereServ  " + ListSereServ.Count.ToString());
                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var sere in ListSereServ)
                        {
                            if (sere.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.AMOUNT > 0 && sere.HEIN_APPROVAL_ID.HasValue)
                            {
                                if (!dicSereServ.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                                    dicSereServ[sere.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServ[sere.HEIN_APPROVAL_ID.Value].Add(sere);
                            }
                        }
                    }


                    hisHeinApprovals = hisHeinApprovals.Where(o => dicTreatment.ContainsKey(o.TREATMENT_ID)).ToList();
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                        {

                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);

                            Mrs00020RDO rdo = new Mrs00020RDO(heinApproval);

                            //tích lũy số lượt điều trị
                            var accumTreatment = ListAccumTreatment.FirstOrDefault(o => o.ID == heinApproval.TREATMENT_ID);
                            if (accumTreatment != null)
                            {
                                rdo.ACCUM_TREATMENT = accumTreatment.ACCUM_TREATMENT;
                            }
                            if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                var treatment = dicTreatment[heinApproval.TREATMENT_ID];
                                if (treatment != null && castFilter.END_DEPARTMENT_ID != null)
                                {
                                    if (treatment.END_DEPARTMENT_ID != castFilter.END_DEPARTMENT_ID)
                                    {
                                        continue;
                                    }
                                }
                                if (treatment != null && castFilter.END_ROOM_ID != null)
                                {
                                    if (treatment.END_ROOM_ID != castFilter.END_ROOM_ID)
                                    {
                                        continue;
                                    }
                                }

                                bool doWork = false;

                                if (treatment != null && castFilter.IS_KB == true && (treatment.END_DEPARTMENT_CODE == castFilter.KB1 || treatment.END_DEPARTMENT_CODE == castFilter.KB2))
                                {
                                    doWork = true;
                                }
                                else if (treatment != null && castFilter.IS_TNT == true && treatment.END_DEPARTMENT_CODE == castFilter.TNT)
                                {
                                    doWork = true;
                                }
                                else if (treatment != null && castFilter.IS_CC == true && treatment.END_DEPARTMENT_CODE == castFilter.CC)
                                {
                                    doWork = true;
                                }
                                else if (castFilter.IS_KB == null && castFilter.IS_TNT == null && castFilter.IS_CC == null)
                                {
                                    doWork = true;
                                }
                                else if (castFilter.IS_KB == false && castFilter.IS_TNT == false && castFilter.IS_CC == false)
                                {
                                    doWork = true;
                                }

                                if (!doWork)
                                {
                                    continue;
                                }
                                rdo.RIGHT_ROUTE_CODE = string.IsNullOrWhiteSpace(heinApproval.RIGHT_ROUTE_TYPE_CODE) ? heinApproval.RIGHT_ROUTE_CODE : MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE;
                                if (rdo.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                                {
                                    rdo.RIGHT_ROUTE_NAME = "Đúng tuyến";
                                }
                                if (rdo.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                                {
                                    rdo.RIGHT_ROUTE_NAME = "Trái tuyến";
                                }
                                rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                                rdo.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                                rdo.CLOSE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                                rdo.EXECUTE_DATE = (heinApproval.EXECUTE_TIME ?? 0) - (heinApproval.EXECUTE_TIME ?? 0) % 1000000;
                                rdo.END_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID ?? 0;
                                rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                                rdo.END_DEPARTMENT_CODE = treatment.END_DEPARTMENT_CODE;
                                rdo.END_ROOM_ID = treatment.END_ROOM_ID ?? 0;
                                rdo.END_ROOM_CODE = treatment.END_ROOM_CODE;
                                rdo.END_ROOM_NAME = treatment.END_ROOM_NAME;
                                rdo.FEE_LOCK_TIME = treatment.FEE_LOCK_TIME;
                                rdo.END_USERNAME = treatment.END_USERNAME;
                                rdo.END_LOGINNAME = treatment.END_LOGINNAME;
                                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                {
                                    rdo.END_ROOM_DEPARTMENT_CODE = treatment.END_ROOM_CODE;
                                    rdo.END_ROOM_DEPARTMENT_NAME = treatment.END_ROOM_NAME;
                                    rdo.END_ROOM_DEPARTMENT_ID = treatment.END_ROOM_ID ?? 0;

                                }
                                else
                                {
                                    rdo.END_ROOM_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID ?? 0;
                                    rdo.END_ROOM_DEPARTMENT_CODE = treatment.END_DEPARTMENT_CODE;
                                    rdo.END_ROOM_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                                }
                                //rdo.END_ROOM_DEPARTMENT_ID = treatment.END_ROOM_ID??0;
                                if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                {
                                    rdo.TOTAL_DATE = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                }
                                else
                                {
                                    rdo.TOTAL_DATE = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                }

                                if (rdo.TOTAL_DATE == 0)
                                {
                                    rdo.TOTAL_DATE = null;
                                }

                                rdo.CASHIER_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.FEE_LOCK_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                                rdo.CASHIER_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.FEE_LOCK_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                                rdo.CASHIER_USERNAME = treatment.FEE_LOCK_USERNAME;
                                rdo.CASHIER_LOGINNAME = treatment.FEE_LOCK_LOGINNAME;
                            }
                            if (dicSereServ.ContainsKey(heinApproval.ID))
                            {
                                //Inventec.Common.Logging.LogSystem.Info("heinApproval.TREATMENT_ID  " + heinApproval.TREATMENT_ID);
                                HasSereServ = 0;
                                if (serviceReqs != null)
                                {

                                    ProcessPatientCase(rdo, serviceReqs, dicSereServ[heinApproval.ID], dicTreatment[heinApproval.TREATMENT_ID].IN_ROOM_ID);
                                }
                                //gop theo ho so dieu tri: danh sach da chua ho so dieu tri do thi chi cong tien vao ho so do chu khong tao them ho so moi vao danh sach
                                if (castFilter.IS_MERGE_TREATMENT == true && treatmentIds.Contains(heinApproval.TREATMENT_ID))
                                {
                                    rdo = ListRdoA.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE) ?? ListRdoB.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE) ?? ListRdoC.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE);
                                    if (rdo != null)
                                    {
                                        ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID], ListSereServBill, ListTransaction);
                                    }
                                    continue;
                                }
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID], ListSereServBill, ListTransaction);
                                if (HasSereServ == 0)
                                {
                                    continue;
                                }
                                //khong co gia thi bo qua
                                if (!CheckPrice(rdo)) continue;
                                
                                if (IsInFilterMediOrgAccept(castFilter.ACCEPT_HEIN_MEDI_ORG_CODE,rdo.HEIN_APPROVAL.HEIN_MEDI_ORG_CODE)
                                    && (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(rdo.HEIN_APPROVAL.HEIN_MEDI_ORG_CODE)
                                    && checkBhytProvinceCode(rdo.HEIN_APPROVAL.HEIN_CARD_NUMBER))
                                {
                                    ListRdo.Add(rdo);
                                    ListRdoA.Add(rdo);
                                    treatmentIds.Add(heinApproval.TREATMENT_ID);
                                }
                                else if (checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
                                {
                                    ListRdo.Add(rdo);
                                    ListRdoB.Add(rdo);
                                    treatmentIds.Add(heinApproval.TREATMENT_ID);
                                }
                                else
                                {
                                    ListRdo.Add(rdo);
                                    ListRdoC.Add(rdo);
                                    treatmentIds.Add(heinApproval.TREATMENT_ID);
                                }
                            }
                        }
                    }

                    Inventec.Common.Logging.LogSystem.Info("ListRdoA" + ListRdoA.Count);
                    Inventec.Common.Logging.LogSystem.Info("ListRdoB" + ListRdoB.Count);
                    Inventec.Common.Logging.LogSystem.Info("ListRdoC" + ListRdoC.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool IsInFilterMediOrgAccept(string filterAccept, string HeinMediOrgCode)
        {
            if (string.IsNullOrWhiteSpace(filterAccept)) return true;
            if (filterAccept.Contains(HeinMediOrgCode ?? "")) return true;
            return false;
        }

        private void ProcessPatientCase(Mrs00020RDO rdo, List<HIS_SERVICE_REQ> serviceReqs, List<V_HIS_SERE_SERV_3> sereServs, long? inRoomId)
        {
            HIS_SERVICE_REQ sr = null;

            if (inRoomId != null)
            {
                sr = serviceReqs.FirstOrDefault(o => sereServs.Exists(p => p.TDL_TREATMENT_ID == o.TREATMENT_ID) && o.EXECUTE_ROOM_ID == inRoomId && o.PATIENT_CASE_ID.HasValue);
            }
            if (sr == null)
            {
                sr = serviceReqs.OrderByDescending(p => p.FINISH_TIME).FirstOrDefault(o => sereServs.Exists(p => p.SERVICE_REQ_ID == o.ID) && o.PATIENT_CASE_ID.HasValue);
            }
            if (sr != null)
            {
                if (ListPatientCase != null)
                {
                    var pc = ListPatientCase.FirstOrDefault(o => o.ID == sr.PATIENT_CASE_ID);
                    if (pc != null)
                    {
                        rdo.PATIENT_CASE_NAME = pc.PATIENT_CASE_NAME;
                    }
                }
            }
        }

        private void ProcessTotalPrice(Mrs00020RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs, List<HIS_SERE_SERV_BILL> ListSereServBill, List<V_HIS_TRANSACTION> ListTransaction)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {


                    var sereBill = ListSereServBill.Where(x => x.SERE_SERV_ID == sereServ.ID).FirstOrDefault();
                    V_HIS_TRANSACTION tran = new V_HIS_TRANSACTION();
                    if (sereBill != null)
                    {
                        rdo.PATIENT_PAY_PRICE += sereBill.PATIENT_PAY_PRICE ?? 0;
                        tran = ListTransaction.Where(x => x.ID == sereBill.BILL_ID).FirstOrDefault();
                        if (tran != null)
                        {
                            rdo.EINVOICE_NUMBER = tran.EINVOICE_NUM_ORDER;
                            rdo.TRANS_REQ_CODE = tran.TRANS_REQ_CODE;
                            rdo.BANK_TRANSACTION_CODE = tran.BANK_TRANSACTION_CODE;
                            rdo.BANK_TRANSACTION_TIME = tran.BANK_TRANSACTION_TIME;
                        }
                    }

                    if (castFilter.DEPARTMENT_ID != null)
                    {
                        if ((sereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH && sereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT && sereServ.TDL_REQUEST_DEPARTMENT_ID != castFilter.DEPARTMENT_ID)
                            ||
                            ((sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT) && sereServ.TDL_EXECUTE_DEPARTMENT_ID != castFilter.DEPARTMENT_ID))
                        {
                            continue;
                        }
                    }

                    HasSereServ = 1;
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits, sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID == o.ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
                   
                    
                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            rdo.TEST_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            rdo.DIIM_PRICE += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                rdo.FUEX_PRICE += TotalPriceTreatment;
                            }
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                        {
                            rdo.MEDICINE_PRICE += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                            {
                                rdo.MEDICINE_PRICE_NDM += TotalPriceTreatment;
                            }
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                        {
                            rdo.BLOOD_PRICE += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                            {
                                rdo.CPM_PRICE += TotalPriceTreatment;
                            }
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                        {
                            rdo.SURGMISU_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM)
                        {
                            rdo.MATERIAL_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                        {
                            rdo.MATERIAL_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                        {
                            rdo.MEDICINE_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            rdo.BED_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            rdo.SERVICE_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            rdo.TRAN_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)
                        {
                            rdo.TT_PRICE += TotalPriceTreatment;
                        }

                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                        bool IsPassBill = true;
                        if (this.castFilter.PAY_FORM_IDs != null && !this.castFilter.PAY_FORM_IDs.Contains(tran.PAY_FORM_ID))
                            IsPassBill = false;
                        if (castFilter.INPUT_DATA_ID_STTRAN_TYPE == 1 && tran.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE)
                            IsPassBill = false;
                        if (castFilter.INPUT_DATA_ID_STTRAN_TYPE == 2 && tran.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            IsPassBill = false;
                        if (IsPassBill)
                        {
                            rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                            ProcessDicPatientPrice(rdo, sereServ, TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0));
                            rdo.TOTAL_PATIENT_PRICE_BHYT += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                            ProcessDicPatientPriceBhyt(rdo, sereServ, sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        }
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        ProcessDicHeinPrice(rdo, sereServ, sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                        ProcessDicParentPrice(rdo, sereServ, TotalPriceTreatment);
                    }
                }

                TotalAmount += rdo.TOTAL_HEIN_PRICE;
                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDicParentPrice(Mrs00020RDO rdo, V_HIS_SERE_SERV_3 sereServ, decimal totalPrice)
        {
            if (rdo.DIC_PARENT_PRICE == null)
            {
                rdo.DIC_PARENT_PRICE = new Dictionary<string, decimal>();
            }
            string ParentCode = GetParentCode(sereServ.SERVICE_ID);
            if (!rdo.DIC_PARENT_PRICE.ContainsKey(ParentCode))
            {
                rdo.DIC_PARENT_PRICE.Add(ParentCode, 0);
            }
            rdo.DIC_PARENT_PRICE[ParentCode] += totalPrice;
        }

        private void ProcessDicHeinPrice(Mrs00020RDO rdo, V_HIS_SERE_SERV_3 sereServ, decimal totalHeinPrice)
        {
            if (rdo.DIC_HSVT_HEIN_PRICE == null)
            {
                rdo.DIC_HSVT_HEIN_PRICE = new Dictionary<string, decimal>();
            }
            if (!rdo.DIC_HSVT_HEIN_PRICE.ContainsKey(sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE"))
            {
                rdo.DIC_HSVT_HEIN_PRICE.Add(sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE", 0);
            }
            rdo.DIC_HSVT_HEIN_PRICE[sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE"] += totalHeinPrice;
        }

        private void ProcessDicPatientPrice(Mrs00020RDO rdo, V_HIS_SERE_SERV_3 sereServ, decimal totalPatientPrice)
        {
            if (rdo.DIC_HSVT_PATIENT_PRICE == null)
            {
                rdo.DIC_HSVT_PATIENT_PRICE = new Dictionary<string, decimal>();
            }
            if (!rdo.DIC_HSVT_PATIENT_PRICE.ContainsKey(sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE"))
            {
                rdo.DIC_HSVT_PATIENT_PRICE.Add(sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE", 0);
            }
            rdo.DIC_HSVT_PATIENT_PRICE[sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE"] += totalPatientPrice;
        }

        private void ProcessDicPatientPriceBhyt(Mrs00020RDO rdo, V_HIS_SERE_SERV_3 sereServ, decimal totalPatientPriceBhyt)
        {
            if (rdo.DIC_HSVT_PATIENT_PRICE_BHYT == null)
            {
                rdo.DIC_HSVT_PATIENT_PRICE_BHYT = new Dictionary<string, decimal>();
            }
            if (!rdo.DIC_HSVT_PATIENT_PRICE_BHYT.ContainsKey(sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE"))
            {
                rdo.DIC_HSVT_PATIENT_PRICE_BHYT.Add(sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE", 0);
            }
            rdo.DIC_HSVT_PATIENT_PRICE_BHYT[sereServ.HEIN_SERVICE_TYPE_CODE ?? "NONE"] += totalPatientPriceBhyt;
        }

        private string GetParentCode(long serviceId)
        {

            string result = "";// (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == serviceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "" + "_";
            if (dicParent.ContainsKey(serviceId))
            {
                result = dicParent[serviceId].SERVICE_CODE ?? "";
            }

            return result;
        }

        private Mrs00020RDO TotalTreatment(List<Mrs00020RDO> list)
        {
            Mrs00020RDO result = new Mrs00020RDO();
            try
            {
                result = list.First();
                result.HEIN_CARD_NUMBER_EXSUM = string.Join(",", list.Select(o => o.HEIN_CARD_NUMBER).Distinct().ToList());
                result.TOTAL_PRICE_EXSUM = list.Sum(s => s.TOTAL_PRICE);
                result.TEST_PRICE_EXSUM = list.Sum(s => s.TEST_PRICE);
                result.DIIM_PRICE_EXSUM = list.Sum(s => s.DIIM_PRICE);
                result.MEDICINE_PRICE_EXSUM = list.Sum(s => s.MEDICINE_PRICE);
                result.MEDICINE_PRICE_NDM_EXSUM = list.Sum(s => s.MEDICINE_PRICE_NDM);
                result.BLOOD_PRICE_EXSUM = list.Sum(s => s.BLOOD_PRICE);
                result.CPM_PRICE_EXSUM = list.Sum(s => s.CPM_PRICE);
                result.FUEX_PRICE_EXSUM = list.Sum(s => s.FUEX_PRICE);
                result.SURGMISU_PRICE_EXSUM = list.Sum(s => s.SURGMISU_PRICE);
                result.MATERIAL_PRICE_EXSUM = list.Sum(s => s.MATERIAL_PRICE);
                result.SERVICE_PRICE_RATIO_EXSUM = list.Sum(s => s.SERVICE_PRICE_RATIO);
                result.MEDICINE_PRICE_RATIO_EXSUM = list.Sum(s => s.MEDICINE_PRICE_RATIO);
                result.MATERIAL_PRICE_RATIO_EXSUM = list.Sum(s => s.MATERIAL_PRICE_RATIO);
                result.EXAM_PRICE_EXSUM = list.Sum(s => s.EXAM_PRICE);
                result.BED_PRICE_EXSUM = list.Sum(s => s.BED_PRICE);
                result.TRAN_PRICE_EXSUM = list.Sum(s => s.TRAN_PRICE);
                result.TT_PRICE_EXSUM = list.Sum(s => s.TT_PRICE);
                result.TOTAL_PATIENT_PRICE_EXSUM = list.Sum(s => s.TOTAL_PATIENT_PRICE);
                result.TOTAL_HEIN_PRICE_EXSUM = list.Sum(s => s.TOTAL_HEIN_PRICE);
                result.TOTAL_HEIN_PRICE_NDS_EXSUM = list.Sum(s => s.TOTAL_HEIN_PRICE_NDS);
                result.TOTAL_OTHER_SOURCE_PRICE_EXSUM = list.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new Mrs00020RDO();
            }
            return result;
        }

        private Mrs00020RDO TotalTreatmentAll(List<Mrs00020RDO> list)
        {
            Mrs00020RDO result = new Mrs00020RDO();
            try
            {
                result = list.First();
                result.HEIN_CARD_NUMBER_ALL = string.Join(",", list.Select(o => o.HEIN_CARD_NUMBER).Distinct().ToList());
                result.TOTAL_PRICE_ALL = list.Sum(s => s.TOTAL_PRICE);
                result.TEST_PRICE_ALL = list.Sum(s => s.TEST_PRICE);
                result.DIIM_PRICE_ALL = list.Sum(s => s.DIIM_PRICE);
                result.MEDICINE_PRICE_ALL = list.Sum(s => s.MEDICINE_PRICE);
                result.MEDICINE_PRICE_NDM_ALL = list.Sum(s => s.MEDICINE_PRICE_NDM);
                result.BLOOD_PRICE_ALL = list.Sum(s => s.BLOOD_PRICE);
                result.CPM_PRICE_ALL = list.Sum(s => s.CPM_PRICE);
                result.FUEX_PRICE_ALL = list.Sum(s => s.FUEX_PRICE);
                result.SURGMISU_PRICE_ALL = list.Sum(s => s.SURGMISU_PRICE);
                result.MATERIAL_PRICE_ALL = list.Sum(s => s.MATERIAL_PRICE);
                result.SERVICE_PRICE_RATIO_ALL = list.Sum(s => s.SERVICE_PRICE_RATIO);
                result.MEDICINE_PRICE_RATIO_ALL = list.Sum(s => s.MEDICINE_PRICE_RATIO);
                result.MATERIAL_PRICE_RATIO_ALL = list.Sum(s => s.MATERIAL_PRICE_RATIO);
                result.EXAM_PRICE_ALL = list.Sum(s => s.EXAM_PRICE);
                result.BED_PRICE_ALL = list.Sum(s => s.BED_PRICE);
                result.TRAN_PRICE_ALL = list.Sum(s => s.TRAN_PRICE);
                result.TT_PRICE_ALL = list.Sum(s => s.TT_PRICE);
                result.TOTAL_PATIENT_PRICE_ALL = list.Sum(s => s.TOTAL_PATIENT_PRICE);
                result.TOTAL_HEIN_PRICE_ALL = list.Sum(s => s.TOTAL_HEIN_PRICE);
                result.TOTAL_HEIN_PRICE_NDS_ALL = list.Sum(s => s.TOTAL_HEIN_PRICE_NDS);
                result.TOTAL_OTHER_SOURCE_PRICE_ALL = list.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new Mrs00020RDO();
            }
            return result;
        }

        private bool CheckPrice(Mrs00020RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.FUEX_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.CPM_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TRAN_PRICE > 0 || rdo.TT_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool checkBhytProvinceCode(string HeinNumber)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(HeinNumber) && HeinNumber.Length == 15)
                {
                    string provinceCode = HeinNumber.Substring(3, 2);
                    if (this._Branch.HEIN_PROVINCE_CODE.Equals(provinceCode))
                    {
                        result = true;
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

        private bool checkBhytNsd(Mrs00020RDO rdo)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
                }
                if (castFilter.FEE_LOCK_LOGINNAMEs != null)
                {
                    var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery() { });
                    dicSingleTag.Add("FEE_LOCK_USERNAMEs", string.Join(";", acsUser.Where(o => castFilter.FEE_LOCK_LOGINNAMEs.Contains(o.LOGINNAME ?? "")).Select(p => p.USERNAME).ToList()));
                }
                else
                {
                    dicSingleTag.Add("FEE_LOCK_USERNAMEs", "");
                }

                ProcessSumTotal();
                ProcessSumDepartment();

                objectTag.AddObjectData(store, "Department", Departments);

                objectTag.AddObjectData(store, "DepartmentTrust", dicDepartment.Values.ToList());

                objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                objectTag.AddObjectData(store, "PatientTypes", listRdoAll);
                objectTag.AddObjectData(store, "SumTotals", ListSumTotal);
                objectTag.AddObjectData(store, "ReportTreatmentA", ListRdoA.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.AddObjectData(store, "ReportTreatmentB", ListRdoB.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.AddObjectData(store, "ReportTreatmentC", ListRdoC.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.AddObjectData(store, "ReportTreatmentAll", listRdoAll.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatmentAll(p.ToList())).ToList());

                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData());
                var listServ = dicParent.Values.GroupBy(o => o.SERVICE_CODE).Select(p => p.First()).OrderBy(q => q.NUM_ORDER).ToList();
                for (int i = 0; i < listServ.Count; i++)
                {
                    dicSingleTag.Add(string.Format("PARENT_SERVICE_CODE__{0}", i + 1), listServ[i].SERVICE_CODE);
                    dicSingleTag.Add(string.Format("PARENT_SERVICE_NAME__{0}", i + 1), listServ[i].SERVICE_NAME);
                }
                objectTag.AddObjectData(store, "AccumTreatment", ListAccumTreatment);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessSumDepartment()
        {
            try
            {
                var ListRdoExam = ListRdo.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                var ListRdoNotExam = ListRdo.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                foreach (var room in HisRoomCFG.HisRooms)
                {
                    Mrs00020RDO rdo = new Mrs00020RDO();
                    rdo.END_ROOM_DEPARTMENT_ID = room.ID;
                    rdo.END_ROOM_DEPARTMENT_CODE = room.ROOM_CODE;
                    rdo.END_ROOM_DEPARTMENT_NAME = room.ROOM_NAME;
                    rdo.END_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                    rdo.END_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                    rdo.COUNT_TREATMENT = ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Count();
                    rdo.TOTAL_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_PRICE));
                    rdo.TEST_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TEST_PRICE));
                    rdo.FUEX_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.FUEX_PRICE));
                    rdo.DIIM_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.DIIM_PRICE));
                    rdo.MEDICINE_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MEDICINE_PRICE));
                    rdo.BLOOD_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.BLOOD_PRICE));
                    rdo.CPM_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.CPM_PRICE));
                    rdo.SURGMISU_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.SURGMISU_PRICE));
                    rdo.MATERIAL_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MATERIAL_PRICE));
                    rdo.SERVICE_PRICE_RATIO = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.SERVICE_PRICE_RATIO));
                    rdo.MEDICINE_PRICE_RATIO = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MEDICINE_PRICE_RATIO));
                    rdo.MATERIAL_PRICE_RATIO = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MATERIAL_PRICE_RATIO));
                    rdo.BED_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.BED_PRICE));
                    rdo.EXAM_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.EXAM_PRICE));
                    rdo.TRAN_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TRAN_PRICE));
                    rdo.TT_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TT_PRICE));
                    rdo.TOTAL_PATIENT_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_PATIENT_PRICE));
                    rdo.TOTAL_HEIN_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_HEIN_PRICE));
                    rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                    rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                    if (rdo.TOTAL_PRICE > 0)
                    {
                        Departments.Add(rdo);

                    }
                }

                foreach (var room in HisDepartmentCFG.DEPARTMENTs)
                {
                    Mrs00020RDO rdo = new Mrs00020RDO();
                    rdo.END_ROOM_DEPARTMENT_ID = room.ID;
                    rdo.END_ROOM_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                    rdo.END_ROOM_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                    rdo.END_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                    rdo.END_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                    rdo.COUNT_TREATMENT = ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Count();
                    rdo.TOTAL_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_PRICE));
                    rdo.TEST_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TEST_PRICE));
                    rdo.FUEX_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.FUEX_PRICE));
                    rdo.DIIM_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.DIIM_PRICE));
                    rdo.MEDICINE_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MEDICINE_PRICE));
                    rdo.BLOOD_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.BLOOD_PRICE));
                    rdo.CPM_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.CPM_PRICE));
                    rdo.SURGMISU_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.SURGMISU_PRICE));
                    rdo.MATERIAL_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MATERIAL_PRICE));
                    rdo.SERVICE_PRICE_RATIO = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.SERVICE_PRICE_RATIO));
                    rdo.MEDICINE_PRICE_RATIO = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MEDICINE_PRICE_RATIO));
                    rdo.MATERIAL_PRICE_RATIO = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.MATERIAL_PRICE_RATIO));
                    rdo.BED_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.BED_PRICE));
                    rdo.EXAM_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.EXAM_PRICE));
                    rdo.TRAN_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TRAN_PRICE));
                    rdo.TT_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TT_PRICE));
                    rdo.TOTAL_PATIENT_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_PATIENT_PRICE));
                    rdo.TOTAL_HEIN_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_HEIN_PRICE));
                    rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                    rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoNotExam.Where(o => o.END_ROOM_DEPARTMENT_ID == room.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                    if (rdo.TOTAL_PRICE > 0)
                    {
                        Departments.Add(rdo);

                    }
                }
                foreach (var room in Departments)
                {
                    if (!dicDepartment.ContainsKey(room.END_DEPARTMENT_CODE))
                    {
                        dicDepartment[room.END_DEPARTMENT_CODE] = new Mrs00020RDO();
                        dicDepartment[room.END_DEPARTMENT_CODE].END_ROOM_DEPARTMENT_ID = room.END_DEPARTMENT_ID;
                        dicDepartment[room.END_DEPARTMENT_CODE].END_ROOM_DEPARTMENT_CODE = room.END_DEPARTMENT_CODE;
                        dicDepartment[room.END_DEPARTMENT_CODE].END_ROOM_DEPARTMENT_NAME = room.END_DEPARTMENT_NAME;
                    }
                    dicDepartment[room.END_DEPARTMENT_CODE].COUNT_TREATMENT += room.COUNT_TREATMENT;
                    dicDepartment[room.END_DEPARTMENT_CODE].TOTAL_PRICE += room.TOTAL_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].TEST_PRICE += room.TEST_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].FUEX_PRICE += room.FUEX_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].DIIM_PRICE += room.DIIM_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].MEDICINE_PRICE += room.MEDICINE_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].BLOOD_PRICE += room.BLOOD_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].CPM_PRICE += room.CPM_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].SURGMISU_PRICE += room.SURGMISU_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].MATERIAL_PRICE += room.MATERIAL_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].SERVICE_PRICE_RATIO += room.SERVICE_PRICE_RATIO;
                    dicDepartment[room.END_DEPARTMENT_CODE].MEDICINE_PRICE_RATIO += room.MEDICINE_PRICE_RATIO;
                    dicDepartment[room.END_DEPARTMENT_CODE].MATERIAL_PRICE_RATIO += room.MATERIAL_PRICE_RATIO;
                    dicDepartment[room.END_DEPARTMENT_CODE].BED_PRICE += room.BED_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].EXAM_PRICE += room.EXAM_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].TRAN_PRICE += room.TRAN_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].TT_PRICE += room.TT_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].TOTAL_PATIENT_PRICE += room.TOTAL_PATIENT_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].TOTAL_HEIN_PRICE += room.TOTAL_HEIN_PRICE;
                    dicDepartment[room.END_DEPARTMENT_CODE].TOTAL_HEIN_PRICE_NDS += room.TOTAL_HEIN_PRICE_NDS;
                    dicDepartment[room.END_DEPARTMENT_CODE].TOTAL_OTHER_SOURCE_PRICE += room.TOTAL_OTHER_SOURCE_PRICE;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private void ProcessSumTotal()
        {
            try
            {
                Mrs00020RDO rdo = new Mrs00020RDO();
                rdo.TOTAL_PRICE = (ListRdoA.Sum(s => s.TOTAL_PRICE) + ListRdoB.Sum(s => s.TOTAL_PRICE) + ListRdoC.Sum(s => s.TOTAL_PRICE));
                rdo.TEST_PRICE = (ListRdoA.Sum(s => s.TEST_PRICE) + ListRdoB.Sum(s => s.TEST_PRICE) + ListRdoC.Sum(s => s.TEST_PRICE));
                rdo.DIIM_PRICE = (ListRdoA.Sum(s => s.DIIM_PRICE) + ListRdoB.Sum(s => s.DIIM_PRICE) + ListRdoC.Sum(s => s.DIIM_PRICE));
                rdo.MEDICINE_PRICE = (ListRdoA.Sum(s => s.MEDICINE_PRICE) + ListRdoB.Sum(s => s.MEDICINE_PRICE) + ListRdoC.Sum(s => s.MEDICINE_PRICE));
                rdo.BLOOD_PRICE = (ListRdoA.Sum(s => s.BLOOD_PRICE) + ListRdoB.Sum(s => s.BLOOD_PRICE) + ListRdoC.Sum(s => s.BLOOD_PRICE));
                rdo.CPM_PRICE = (ListRdoA.Sum(s => s.CPM_PRICE) + ListRdoB.Sum(s => s.CPM_PRICE) + ListRdoC.Sum(s => s.CPM_PRICE));
                rdo.FUEX_PRICE = (ListRdoA.Sum(s => s.FUEX_PRICE) + ListRdoB.Sum(s => s.FUEX_PRICE) + ListRdoC.Sum(s => s.FUEX_PRICE));
                rdo.SURGMISU_PRICE = (ListRdoA.Sum(s => s.SURGMISU_PRICE) + ListRdoB.Sum(s => s.SURGMISU_PRICE) + ListRdoC.Sum(s => s.SURGMISU_PRICE));
                rdo.MATERIAL_PRICE = (ListRdoA.Sum(s => s.MATERIAL_PRICE) + ListRdoB.Sum(s => s.MATERIAL_PRICE) + ListRdoC.Sum(s => s.MATERIAL_PRICE));
                rdo.SERVICE_PRICE_RATIO = (ListRdoA.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoB.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Sum(s => s.SERVICE_PRICE_RATIO));
                rdo.MEDICINE_PRICE_RATIO = (ListRdoA.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoB.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO));
                rdo.MATERIAL_PRICE_RATIO = (ListRdoA.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoB.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO));
                rdo.EXAM_PRICE = (ListRdoA.Sum(s => s.EXAM_PRICE) + ListRdoB.Sum(s => s.EXAM_PRICE) + ListRdoC.Sum(s => s.EXAM_PRICE));
                rdo.BED_PRICE = (ListRdoA.Sum(s => s.BED_PRICE) + ListRdoB.Sum(s => s.BED_PRICE) + ListRdoC.Sum(s => s.BED_PRICE));
                rdo.TRAN_PRICE = (ListRdoA.Sum(s => s.TRAN_PRICE) + ListRdoB.Sum(s => s.TRAN_PRICE) + ListRdoC.Sum(s => s.TRAN_PRICE));
                rdo.TT_PRICE = (ListRdoA.Sum(s => s.TT_PRICE) + ListRdoB.Sum(s => s.TT_PRICE) + ListRdoC.Sum(s => s.TT_PRICE));
                rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                rdo.TOTAL_HEIN_PRICE = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                ListSumTotal.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        class RDOCustomerFuncRownumberData : TFlexCelUserFunction
        {
            public RDOCustomerFuncRownumberData()
            {
            }
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length < 1)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                long result = 0;
                try
                {
                    long rownumber = Convert.ToInt64(parameters[0]);
                    result = (rownumber + 1);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }

                return result;
            }
        }
    }
}
