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

namespace MRS.Processor.Mrs00311
{
    class Mrs00311Processor : AbstractProcessor
    {
        Mrs00311Filter castFilter = null;
        List<Mrs00311RDO> ListRdo = new List<Mrs00311RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        HIS_BRANCH _Branch = null;

        public Mrs00311Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00311Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = false;
            try
            {
                this.castFilter = (Mrs00311Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);

                result = true;
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
            bool result = false;
            try
            {
                ProcessListHeinApproval();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void ProcessListHeinApproval()
        {
            try
            {
                if (ListHeinApproval != null && ListHeinApproval.Count > 0)
                {
                    CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
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


                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra taij DAOGET trong qua trinh tong hop du lieu MRS00311.");
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
            }
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
                            if (item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.AMOUNT <= 0)
                                continue;
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                        }
                    }


                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            continue;
                        Mrs00311RDO rdo = new Mrs00311RDO(heinApproval);
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
                        if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                        {
                            var treatment = dicTreatment[heinApproval.TREATMENT_ID];
                            rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                            rdo.IN_TIME_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                            if (treatment.OUT_TIME.HasValue)
                            {
                                //rdo.MAIN_DAY = HIS.Treatment.DateTime.Calculation.DayOfTreatment(treatment.IN_TIME, treatment.OUT_TIME.Value);
                                if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                {
                                    rdo.MAIN_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                }
                                else
                                {
                                    rdo.MAIN_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                                }
                            }

                            if (treatment.OUT_TIME.HasValue && treatment.CLINICAL_IN_TIME.HasValue)
                            {
                                rdo.OPEN_TIME_SEPARATE_STR = treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                                rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                                //rdo.TOTAL_DAY = HIS.Treatment.DateTime.Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.Value, treatment.OUT_TIME.Value);

                                if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                {
                                    rdo.TOTAL_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                }
                                else
                                {
                                    rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                                }
                                if (rdo.TOTAL_DAY == 0)
                                {
                                    rdo.TOTAL_DAY = null;
                                }
                            }
                            else if (treatment.CLINICAL_IN_TIME.HasValue)
                            {
                                rdo.OPEN_TIME_SEPARATE_STR = treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                            }
                            //DataRDO.TOTAL_DAY = 0; 
                            if (treatment.END_DEPARTMENT_ID.HasValue)
                            {
                                var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                                if (departmentCodeBHYT != null)
                                {
                                    rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
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

                            rdo.MEDI_ORG_FROM_CODE = dicTreatment[heinApproval.TREATMENT_ID].MEDI_ORG_CODE;

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

        private void ProcessTotalPrice(Mrs00311RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
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

        private bool CheckPrice(Mrs00311RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
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

        private bool checkBhytNsd(Mrs00311RDO rdo)
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
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "treatment", ListRdo);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
