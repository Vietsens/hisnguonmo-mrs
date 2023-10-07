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

namespace MRS.Processor.Mrs00470
{
    class Mrs00470Processor : AbstractProcessor
    {
        Mrs00470Filter castFilter = null;
        List<Mrs00470RDO> ListRdo = new List<Mrs00470RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        HIS_BRANCH _Branch = null;


        public Mrs00470Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00470Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00470Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00470: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00470");
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
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        if (this.castFilter.END_DEPARTMENT_IDs != null)
                        {
                            ListTreatment = ListTreatment.Where(o => this.castFilter.END_DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID ?? 0)).ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => ListTreatment.Exists(p => p.ID == o.TREATMENT_ID)).ToList();
                        }
                        if (this.castFilter.EXECUTE_DEPARTMENT_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => this.castFilter.EXECUTE_DEPARTMENT_IDs.Contains(o.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => ListSereServ.Exists(p => p.HEIN_APPROVAL_ID == o.ID)).ToList();
                        }
                        if (this.castFilter.REQUEST_DEPARTMENT_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => this.castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => ListSereServ.Exists(p => p.HEIN_APPROVAL_ID == o.ID)).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00470");
                        }
                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment)
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
                            if (item.HEIN_APPROVAL_ID == null || item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.AMOUNT <= 0)
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

                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                        Mrs00470RDO rdo = new Mrs00470RDO(heinApproval);
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

                        rdo.TDL_PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
                        rdo.IN_CODE = dicTreatment.ContainsKey(heinApproval.TREATMENT_ID) ? dicTreatment[heinApproval.TREATMENT_ID].IN_CODE : "";
                        rdo.TREATMENT_CODE = dicTreatment.ContainsKey(heinApproval.TREATMENT_ID) ? dicTreatment[heinApproval.TREATMENT_ID].TREATMENT_CODE : "";
                        rdo.TDL_PATIENT_DOB = Convert.ToInt64(heinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 8));
                        if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.GENDER_CODE = "1";
                        }
                        else if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.GENDER_CODE = "2";
                        }
                        rdo.TDL_PATIENT_ADDRESS = heinApproval.ADDRESS;
                        rdo.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
                        rdo.HEIN_MEDI_ORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
                        rdo.HEIN_CARD_FROM_TIME_STR = Convert.ToInt64(heinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8));
                        rdo.HEIN_CARD_TO_TIME_STR = Convert.ToInt64(heinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8));
                        if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                        {
                            var treatment = dicTreatment[heinApproval.TREATMENT_ID];
                            rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                            rdo.ICD_NAME_MAIN = treatment.ICD_NAME;
                            rdo.END_LOGINNAME = treatment.END_LOGINNAME;
                            rdo.END_USERNAME = treatment.END_USERNAME;
                            rdo.DOCTOR_LOGINNAME = treatment.DOCTOR_LOGINNAME;
                            rdo.DOCTOR_USERNAME = treatment.DOCTOR_USERNAME;
                            rdo.OPEN_TIME_SEPARATE_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                            rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.HasValue ? treatment.OUT_TIME.Value.ToString().Substring(0, 12) : "";
                            rdo.EXECUTE_USERNAME = heinApproval.EXECUTE_USERNAME;
                            rdo.EXECUTE_LOGINNAME = heinApproval.EXECUTE_LOGINNAME;
                            rdo.FEE_LOCK_LOGINNAME = treatment.FEE_LOCK_LOGINNAME;
                            rdo.FEE_LOCK_USERNAME = treatment.FEE_LOCK_USERNAME;
                            //rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT) ?? 0;
                            if (treatment.TREATMENT_DAY_COUNT.HasValue)
                            {
                                rdo.TOTAL_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                            }
                            else
                            {
                                rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                            }
                            if (treatment.END_DEPARTMENT_ID.HasValue)
                            {
                                var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                                if (departmentCodeBHYT != null)
                                {
                                    rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                                    rdo.END_DEPARTMENT_CODE = departmentCodeBHYT.DEPARTMENT_CODE;
                                    rdo.END_DEPARTMENT_NAME = departmentCodeBHYT.DEPARTMENT_NAME;
                                    rdo.DOCTOR = treatment.DOCTOR_USERNAME;
                                    rdo.END_USERNAME = treatment.END_USERNAME;
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
                            rdo.ICD_CODE_EXTRA = treatment.ICD_SUB_CODE; //
                            rdo.ICD_NAME_EXTRA = treatment.ICD_TEXT; //

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
                            //Ma noi chuyen

                            if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                rdo.MEDI_ORG_FROM_CODE = dicTreatment[heinApproval.TREATMENT_ID].MEDI_ORG_CODE;
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
                            if (dicSereServ.ContainsKey(heinApproval.ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID]);
                            }

                            //khong co gia thi bo qua
                            if (!CheckPrice(rdo)) continue;

                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessTotalPrice(Mrs00470RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());

                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            if (sereServ.ORIGINAL_PRICE == sereServ.PRICE && String.IsNullOrEmpty(rdo.SPECIALITY_CODE))
                            {
                                // rdo.SPECIALITY_CODE = sereServ.SPECIALITY_CODE; 
                            }
                        }
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
                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
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

        private bool CheckPrice(Mrs00470RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TRAN_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool checkBhytNsd(Mrs00470RDO rdo)
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
