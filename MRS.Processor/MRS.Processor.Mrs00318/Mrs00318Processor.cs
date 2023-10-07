using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisHeinApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Common.Treatment;
using FlexCel.Report;
using Inventec.Common.Logging;
using MOS.MANAGER.HisOtherPaySource;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisIcd;

namespace MRS.Processor.Mrs00318
{
    class Mrs00318Processor : AbstractProcessor
    {
        Mrs00318Filter castFilter = null;
        List<Mrs00318RDO> ListRdoA = new List<Mrs00318RDO>();
        List<Mrs00318RDO> ListRdoB = new List<Mrs00318RDO>();
        List<Mrs00318RDO> ListRdoC = new List<Mrs00318RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<long> listSereServIdFilter = new List<long>();
        HIS_BRANCH _Branch = null;
        List<long> OtherPaySourceIds = new List<long>();

        Dictionary<string, HIS_ICD> dicIcd = new Dictionary<string, HIS_ICD>();

        public Mrs00318Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00318Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00318Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    this._Branch = new HIS_BRANCH();

                ListHeinApproval = GetHeinApproval();
                GetOtherPaySource();

                //get icd
                GetIcd();

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetIcd()
        {
            var Icds = new HisIcdManager().Get(new HisIcdFilterQuery());
            dicIcd = Icds.GroupBy(o => o.ICD_CODE).ToDictionary(p => p.Key, q => q.First());
        }

        private List<V_HIS_HEIN_APPROVAL> GetHeinApproval()
        {
            if (castFilter.INPUT_DATA_ID__TIME_TYPE == 1)//vào viện
            {
                HisTreatmentFilterQuery treaFilter = new HisTreatmentFilterQuery();
                treaFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                treaFilter.IN_TIME_TO = castFilter.TIME_TO;
                var treatments = new HisTreatmentManager().Get(treaFilter);
                var patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                var treatmentIds = treatments.Where(o => o.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt).Select(o => o.ID).ToList();
                var skip = 0;

                List<V_HIS_HEIN_APPROVAL> result = new List<V_HIS_HEIN_APPROVAL>();

                while (treatmentIds.Count - skip > 0)
                {
                    var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.TREATMENT_IDs = limit;
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    var listSelectionHap = new HisHeinApprovalManager().GetView(approvalFilter);
                    if (listSelectionHap != null && listSelectionHap.Count > 0)
                    {
                        result.AddRange(listSelectionHap);
                    }
                }
                return result;

            }
            else if (castFilter.INPUT_DATA_ID__TIME_TYPE == 2)//ra viện
            {
                HisTreatmentFilterQuery treaFilter = new HisTreatmentFilterQuery();
                treaFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treaFilter.OUT_TIME_TO = castFilter.TIME_TO;
                var treatments = new HisTreatmentManager().Get(treaFilter);
                var patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                var treatmentIds = treatments.Where(o => o.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt).Select(o => o.ID).ToList();
                var skip = 0;

                List<V_HIS_HEIN_APPROVAL> result = new List<V_HIS_HEIN_APPROVAL>();

                while (treatmentIds.Count - skip > 0)
                {
                    var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.TREATMENT_IDs = limit;
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    var listSelectionHap = new HisHeinApprovalManager().GetView(approvalFilter);
                    if (listSelectionHap != null && listSelectionHap.Count > 0)
                    {
                        result.AddRange(listSelectionHap);
                    }
                }
                return result;
            }
            else if (castFilter.INPUT_DATA_ID__TIME_TYPE == 3)//chỉ định
            {
                HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                sereServFilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sereServFilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                var sereServs = new HisSereServManager().Get(sereServFilter);
                var patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                var heinApprovalIds = sereServs.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).Select(o => o.HEIN_APPROVAL_ID ?? 0).Distinct().ToList();
                listSereServIdFilter = sereServs.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).Select(o => o.ID).ToList();
                var skip = 0;

                List<V_HIS_HEIN_APPROVAL> result = new List<V_HIS_HEIN_APPROVAL>();

                while (heinApprovalIds.Count - skip > 0)
                {
                    var limit = heinApprovalIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.IDs = limit;
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    var listSelectionHap = new HisHeinApprovalManager().GetView(approvalFilter);
                    if (listSelectionHap != null && listSelectionHap.Count > 0)
                    {
                        result.AddRange(listSelectionHap);
                    }
                }
                return result;
            }
            else //giám định bảo hiểm
            {
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                return new HisHeinApprovalManager().GetView(approvalFilter);
            }
        }

        private void GetOtherPaySource()
        {
            var listOtherPaySource = new HisOtherPaySourceManager().Get(new HisOtherPaySourceFilterQuery()) ?? new List<HIS_OTHER_PAY_SOURCE>();
            if (!string.IsNullOrWhiteSpace(castFilter.OTHER_PAY_SOURCE_CODE_ALLOWS))
            {
                listOtherPaySource = listOtherPaySource.Where(o => ("," + castFilter.OTHER_PAY_SOURCE_CODE_ALLOWS + ",").Contains("," + o.OTHER_PAY_SOURCE_CODE + ",")).ToList();
                OtherPaySourceIds = listOtherPaySource.Select(o => o.ID).ToList();
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        //if (ListSereServ != null)
                        //{
                        //    ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        //}
                        if (listSereServIdFilter != null && listSereServIdFilter.Count > 0)
                        {
                            ListSereServ = ListSereServ.Where(o => listSereServIdFilter.Contains(o.ID)).ToList();
                        }

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        if ((castFilter.END_DEPARTMENT_ID ?? 0) != 0)
                        {
                            ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.END_DEPARTMENT_ID).ToList();
                        }
                        HisServiceReqFilterQuery ServiceReqFilter = new HisServiceReqFilterQuery();
                        ServiceReqFilter.TREATMENT_IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<HIS_SERVICE_REQ> serviceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(ServiceReqFilter);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00065");
                        }
                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ, ListTreatment, serviceReqs);
                        if (castFilter.IS_GROUP_BY_TREATMENT.HasValue && castFilter.IS_GROUP_BY_TREATMENT == true)
                        {
                            List<Mrs00318RDO> listAll = new List<Mrs00318RDO>();
                            if (ListRdoA != null)
                            {
                                listAll.AddRange(ListRdoA);
                            }
                            if (ListRdoB != null)
                            {
                                listAll.AddRange(ListRdoB);
                            }
                            if (ListRdoC != null)
                            {
                                listAll.AddRange(ListRdoC);
                            }

                            ListRdoA = ProcessGroupByTreatment(ListRdoA);
                            ListRdoB = ProcessGroupByTreatment(ListRdoB);
                            ListRdoC = ProcessGroupByTreatment(ListRdoC);

                        }
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
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment, List<HIS_SERVICE_REQ> ListServiceReq)
        {
            try
            {
                Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var item in ListTreatment)
                        {
                            dicTreatment[item.ID] = item;
                        }
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var item in ListSereServ)
                        {
                            if (item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.AMOUNT <= 0)
                                continue;
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                        }
                    }

                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            continue;
                        if (!dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            continue;
                        var treatment = dicTreatment[heinApproval.TREATMENT_ID];

                        var sereServSub = dicSereServ.ContainsKey(heinApproval.ID) ? dicSereServ[heinApproval.ID] : new List<V_HIS_SERE_SERV_3>();
                        var serviceReqIds = sereServSub.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                        var serviceReqSub = ListServiceReq.Where(o => serviceReqIds.Contains(o.ID)).ToList();
                        string keySplitSr = "";
                        //khi có điều kiện lọc từ template thì đổi sang key từ template
                        if (this.dicDataFilter.ContainsKey("KEY_SPLIT_SR") && this.dicDataFilter["KEY_SPLIT_SR"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_SPLIT_SR"].ToString()))
                        {
                            keySplitSr = this.dicDataFilter["KEY_SPLIT_SR"].ToString();
                        }
                        var group = serviceReqSub.GroupBy(o => string.Format(keySplitSr, o.REQUEST_DEPARTMENT_ID, o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH?o.EXECUTE_ROOM_ID:o.REQUEST_ROOM_ID)).ToList();
                        foreach (var item in group)
                        {
                            var srIds = item.Select(o => o.ID).Distinct().ToList();
                            var ssSub = sereServSub.Where(o => srIds.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();
                            AddRdo(heinApproval, treatment, ssSub, item.ToList());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void AddRdo(V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment, List<V_HIS_SERE_SERV_3> listSS, List<HIS_SERVICE_REQ> listSr)
        {
            Mrs00318RDO rdo = new Mrs00318RDO(heinApproval);
            if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                rdo.TREATMENT_TYPE_CODE = "1";
            }
            else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
            {
                rdo.TREATMENT_TYPE_CODE = "2";
            }
            else
            {
                rdo.TREATMENT_TYPE_CODE = "3";
            }

            rdo.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
            rdo.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
            rdo.DOB = Inventec.Common.TypeConvert.Parse.ToInt64(heinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 8));
            if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
            {
                rdo.GENDER_CODE = "1";
            }
            else if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            {
                rdo.GENDER_CODE = "2";
            }
            rdo.VIR_ADDRESS = heinApproval.ADDRESS;
            rdo.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
            rdo.HEIN_MEDI_ORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
            rdo.HEIN_CARD_FROM_TIME_STR = Inventec.Common.TypeConvert.Parse.ToInt64(heinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8));
            rdo.HEIN_CARD_TO_TIME_STR = Inventec.Common.TypeConvert.Parse.ToInt64(heinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8));


            if (treatment != null && castFilter.END_DEPARTMENT_ID != null)
            {
                if (treatment.END_DEPARTMENT_ID != castFilter.END_DEPARTMENT_ID)
                {
                    return;
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
                return;
            }



            rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
            rdo.END_CODE = treatment.END_CODE;
            rdo.OPEN_TIME_SEPARATE_STR = treatment.IN_TIME.ToString().Substring(0, 12);
            if (treatment.OUT_TIME.HasValue)
            {
                rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    rdo.TOTAL_DAY = 0;
                }
                else if (treatment.CLINICAL_IN_TIME.HasValue)
                {
                    //rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT) ?? 0;
                    if (treatment.TREATMENT_DAY_COUNT.HasValue)
                    {
                        rdo.TOTAL_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                    }
                    else
                    {
                        rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                    }
                }
                if (rdo.TOTAL_DAY == 0)
                {
                    rdo.TOTAL_DAY = null;
                }
            }
            //DataRDO.TOTAL_DAY = 0; 
            if (treatment.END_DEPARTMENT_ID.HasValue)
            {
                var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                if (departmentCodeBHYT != null)
                {
                    rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                    rdo.DEPARTMENT_NAME = departmentCodeBHYT.DEPARTMENT_NAME;
                }
            }
            if (treatment.END_ROOM_ID.HasValue)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.END_ROOM_ID);
                if (room != null)
                {
                    rdo.END_ROOM_CODE = room.ROOM_CODE;
                    rdo.END_ROOM_NAME = room.ROOM_NAME;
                }
            }

            //Ket qua dieu tri: 1: Khỏi;  2: Đỡ;  3: Không thay đổi;  4: Nặng hơn;  5: Tử vong
            if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
            {
                rdo.TREATMENT_RESULT_CODE = "1";
            }
            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
            {
                rdo.TREATMENT_RESULT_CODE = "2";
            }
            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
            {
                rdo.TREATMENT_RESULT_CODE = "3";
            }
            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
            {
                rdo.TREATMENT_RESULT_CODE = "4";
            }
            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
            {
                rdo.TREATMENT_RESULT_CODE = "5";
            }
            //Map tinh trang ra vien
            if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV || treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN)
            {
                treatment.TREATMENT_END_TYPE_ID = HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN;
            }
            //Tinh trang ra vien: 1: Ra viện;  2: Chuyển viện;  3: Trốn viện;  4: Xin ra viện
            if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN)
            {
                rdo.TREATMENT_END_TYPE_CODE = "1";
            }
            else if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN)
            {
                rdo.TREATMENT_END_TYPE_CODE = "2";
            }
            else if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__TRONVIEN)
            {
                rdo.TREATMENT_END_TYPE_CODE = "3";
            }
            else if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__XINRAVIEN)
            {
                rdo.TREATMENT_END_TYPE_CODE = "4";
            }

            if (IsNotNullOrEmpty(treatment.ICD_SUB_CODE) && treatment.ICD_SUB_CODE.StartsWith(";"))
            {
                treatment.ICD_SUB_CODE = treatment.ICD_SUB_CODE.Substring(1, treatment.ICD_SUB_CODE.Length - 1);
            }
            rdo.ICD_CODE_EXTRA = treatment.ICD_SUB_CODE; //

            if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
            {
                if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                {
                    rdo.REASON_INPUT_CODE = "2";
                }
                else
                {
                    rdo.REASON_INPUT_CODE = "1";
                }
            }
            else if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
            {
                rdo.REASON_INPUT_CODE = "3";
            }

            if (dicIcd.ContainsKey(rdo.ICD_CODE_MAIN ?? ""))
            {
                var icd = dicIcd[rdo.ICD_CODE_MAIN ?? ""];
                if (icd.IS_LATENT_TUBERCULOSIS == 1)
                {
                    if (treatment.TUBERCULOSIS_ISSUED_ORG_NAME != null && treatment.TUBERCULOSIS_ISSUED_ORG_NAME.Length > 0)
                    {
                        if (treatment.TUBERCULOSIS_ISSUED_DATE > 0)
                        {
                            rdo.IS_TUBERCULOSIS = (short)1;
                        }
                    }
                }
            }
            //Ma noi chuyen

            {
                rdo.MEDI_ORG_FROM_CODE = treatment.MEDI_ORG_CODE;
                rdo.TRANSFER_IN_MEDI_ORG_CODE = treatment.TRANSFER_IN_MEDI_ORG_CODE;
                rdo.MEDI_ORG_FROM_NAME = treatment.MEDI_ORG_NAME;
                rdo.TRANSFER_IN_MEDI_ORG_NAME = treatment.TRANSFER_IN_MEDI_ORG_NAME;
            }

            if (heinApproval.EXECUTE_TIME.HasValue)
            {
                rdo.INSURANCE_YEAR = Convert.ToInt64(heinApproval.EXECUTE_TIME.ToString().Substring(0, 4));
                rdo.INSURANCE_MONTH = Convert.ToInt64(heinApproval.EXECUTE_TIME.ToString().Substring(4, 2));
            }
            rdo.HEIN_LIVE_AREA_CODE = heinApproval.LIVE_AREA_CODE;

            rdo.CURRENT_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
            //Noi thanh toan: 1: thanh toan tai co so;  2: thanh toan truc tiep
            rdo.PLACE_PAYMENT_CODE = 1;
            //Giam dinh: 0: không thẩm định;  1: chấp nhận;  2: điều chỉnh;  3: xuất toán
            rdo.INSURANCE_STT = 0;
            rdo.REASON_OUT_PRICE = 0;
            rdo.REASON_OUT = "";
            rdo.POLYLINE_PRICE = 0;
            rdo.EXCESS_PRICE = 0;
            rdo.ROUTE_CODE = "";
            if (listSS.Count > 0)
            {
                ProcessServiceReq(rdo, listSr);
                ProcessTotalPrice(rdo, listSS);
            }
            //khong co gia thi bo qua
            if (!CheckPrice(rdo)) return;
            var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH();
            if (!string.IsNullOrEmpty(branch.ACCEPT_HEIN_MEDI_ORG_CODE) && branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(rdo.HEIN_APPROVAL_BHYT.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
            {
                ListRdoA.Add(rdo);
            }
            else if (checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
            {
                ListRdoB.Add(rdo);
            }
            else
            {
                ListRdoC.Add(rdo);
            }

        }

        private void ProcessServiceReq(Mrs00318RDO rdo, List<HIS_SERVICE_REQ> listSr)
        {
            var requestDepartmentIds = listSr.Select(o => o.REQUEST_DEPARTMENT_ID).ToList();
            var departments = HisDepartmentCFG.DEPARTMENTs.Where(o => requestDepartmentIds.Contains(o.ID)).ToList();
            rdo.REQUEST_DEPARTMENT_CODE = string.Join(";", departments.Select(o => o.DEPARTMENT_CODE).ToList());
            rdo.REQUEST_DEPARTMENT_NAME = string.Join(";", departments.Select(o => o.DEPARTMENT_NAME).ToList());

            var requestRoomIds = listSr.Select(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH ? o.EXECUTE_ROOM_ID : o.REQUEST_ROOM_ID).ToList();
            var rooms = HisRoomCFG.HisRooms.Where(o => requestRoomIds.Contains(o.ID)).ToList();
            rdo.REQUEST_ROOM_CODE = string.Join(";", rooms.Select(o => o.ROOM_CODE).ToList());
            rdo.REQUEST_ROOM_NAME = string.Join(";", rooms.Select(o => o.ROOM_NAME).ToList());
        }
        private void ProcessTotalPrice(Mrs00318RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {


                    decimal? totalOtherSourcePrice = null;

                    //trường hợp không có tiền bảo hiểm chi trả:
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                    {
                        //mặc định không tính nguồn khác
                        totalOtherSourcePrice = 0;
                        //nếu là thuốc vật tư thì cũng không tính nguồn khác
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                        {
                            totalOtherSourcePrice = 0;
                        }
                        //nếu không phải thuốc vật tư thì thêm tiền nguồn khác vào báo cáo
                        else if (sereServ.OTHER_PAY_SOURCE_ID != null && OtherPaySourceIds.Contains(sereServ.OTHER_PAY_SOURCE_ID ?? 0))
                        {
                            totalOtherSourcePrice = (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                        }

                    }

                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        var TotalPriceTreatment = totalOtherSourcePrice == null ? Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH()) : (totalOtherSourcePrice ?? 0);
                        //TotalPriceTreatment = Math.Round(TotalPriceTreatment, 2);
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            rdo.TEST_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            rdo.DIIM_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                        {
                            rdo.MEDICINE_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                        {
                            rdo.BLOOD_PRICE += TotalPriceTreatment;
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            rdo.TDCN_PRICE += TotalPriceTreatment;
                        }
                        rdo.TOTAL_PRICE += TotalPriceTreatment;

                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += totalOtherSourcePrice == null ? (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT : (totalOtherSourcePrice ?? 0);
                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                    }
                }
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

        private bool CheckPrice(Mrs00318RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TRAN_PRICE > 0 || rdo.TDCN_PRICE > 0 || rdo.TT_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool checkBhytNsd(Mrs00318RDO rdo)
        {
            bool result = false;
            try
            {
                if (MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
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
        protected List<Mrs00318RDO> ProcessGroupByTreatment(List<Mrs00318RDO> ListRdo)
        {
            List<Mrs00318RDO> result = new List<Mrs00318RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var grRdo = ListRdo.OrderByDescending(p => p.HEIN_CARD_TO_TIME_STR).GroupBy(o => o.TREATMENT_CODE);
                    foreach (var item in grRdo)
                    {
                        Mrs00318RDO rdo = new Mrs00318RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00318RDO>(rdo, item.First());
                        //trường hợp cùng 1 thẻ thì phải gộp hạn dùng thẻ
                        //gộp 2 cách
                        //C1: Hạn từ thẻ 1; hạn từ thẻ 2; ...
                        //C2: Hạn từ thẻ 1 - hạn đến thẻ 2
                        rdo.GR_1_HEIN_FROM_TIME_STR = item.Last().HEIN_CARD_FROM_TIME_STR;
                        rdo.GR_1_HEIN_TO_TIME_STR = item.First().HEIN_CARD_TO_TIME_STR;
                        rdo.GR_2_HEIN_FROM_TIME_STR = string.Join(";", item.Select(s => s.HEIN_CARD_FROM_TIME_STR));
                        rdo.GR_2_HEIN_TO_TIME_STR = string.Join(";", item.Select(s => s.HEIN_CARD_TO_TIME_STR));
                        rdo.HEIN_CARD_NUMBER = string.Join(";", item.Select(s => s.HEIN_CARD_NUMBER));
                        rdo.BED_PRICE = item.Sum(s => s.BED_PRICE);
                        rdo.BLOOD_PRICE = item.Sum(s => s.BLOOD_PRICE);
                        rdo.DIIM_PRICE = item.Sum(s => s.DIIM_PRICE);
                        rdo.EXAM_PRICE = item.Sum(s => s.EXAM_PRICE);
                        rdo.MATERIAL_PRICE = item.Sum(s => s.MATERIAL_PRICE);
                        rdo.MEDICINE_PRICE = item.Sum(s => s.MEDICINE_PRICE);
                        rdo.SURGMISU_PRICE = item.Sum(s => s.SURGMISU_PRICE);
                        rdo.TEST_PRICE = item.Sum(s => s.TEST_PRICE);
                        rdo.TOTAL_HEIN_PRICE = item.Sum(s => s.TOTAL_HEIN_PRICE);
                        rdo.TOTAL_HEIN_PRICE_NDS = item.Sum(s => s.TOTAL_HEIN_PRICE_NDS);
                        rdo.TOTAL_PATIENT_PRICE = item.Sum(s => s.TOTAL_PATIENT_PRICE);
                        rdo.TRAN_PRICE = item.Sum(s => s.TRAN_PRICE);
                        rdo.TT_PRICE = item.Sum(s => s.TT_PRICE);
                        rdo.TDCN_PRICE = item.Sum(s => s.TDCN_PRICE);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = item.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
                        rdo.MATERIAL_PRICE_RATIO = item.Sum(s => s.MATERIAL_PRICE_RATIO);
                        rdo.MEDICINE_PRICE_RATIO = item.Sum(s => s.MEDICINE_PRICE_RATIO);
                        rdo.SERVICE_PRICE_RATIO = item.Sum(s => s.SERVICE_PRICE_RATIO);
                        rdo.TOTAL_PRICE = item.Sum(s => s.TOTAL_PRICE);
                        result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = ListRdo;
            }
            return result;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
                objectTag.SetUserFunction(store, "fRound", new CustomerFuncRoundData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
    class CustomerFuncRoundData : TFlexCelUserFunction
    {

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            decimal result = 0;
            try
            {
                decimal dataIn1 = Convert.ToDecimal(parameters[0]);
                int dataIn2 = Convert.ToInt32(parameters[1]);
                result = Math.Round(dataIn1, dataIn2);


            }
            catch (Exception ex)
            {
                return 0;
            }

            return result;
        }
    }
}
