using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServ;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisHeinApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using HIS.Common.Treatment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisBranch;

namespace MRS.Processor.Mrs00079
{
    class Mrs00079Processor : AbstractProcessor
    {
        Mrs00079Filter castFilter = null;
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = null;
        List<Mrs00079RDO> listRdo = new List<Mrs00079RDO>();
        List<Mrs00079RDO> ListServiceRdo = new List<Mrs00079RDO>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<long> listHeinServiceTypeId;
        HIS_BRANCH _Branch = null;
        string MaterialPriceOption = "";
        public Mrs00079Processor(CommonParam param, string printTypeCode)
            : base(param, printTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00079Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                this.castFilter = (Mrs00079Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00079: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();
                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();

                branchFilter.ID = castFilter.BRANCH_ID;
                branchFilter.IDs = castFilter.BRANCH_IDs;
                var branchs = new HisBranchManager().Get(branchFilter);
                if (branchs != null && branchs.Count > 0)
                {
                    this._Branch = branchs.First();
                }
                else
                {
                    this._Branch = new HIS_BRANCH();
                }
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00079");
                }

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.IDs = ListHeinApproval.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();

                List<long> treatmentIds = ListHeinApproval != null ? ListHeinApproval.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList() : null;
                ListTreatment = treatmentIds != null && treatmentIds.Count > 0 ? new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetViewByIds(treatmentIds) : null;
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
            this.castFilter = (Mrs00079Filter)this.reportFilter;
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                if (IsNotNullOrEmpty(ListHeinApproval))
                {

                    listHeinServiceTypeId = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN, 
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L
                    };

                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__DVKT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }

                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        var ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00079.");
                        }
                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
                ListServiceRdo = ProcessListRDO();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListServiceRdo.Clear();
                result = false;
            }
            return result;
        }

        private bool checkBhytNsd(V_HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(treatment.ICD_CODE ?? ""))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(treatment.ICD_CODE))
                {
                    if ((treatment.TDL_HEIN_CARD_NUMBER ?? "  ").Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains((treatment.ICD_CODE ?? "   ").Substring(0, 3)))
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

        private bool checkBhytNsd(Mrs00079RDO rdo)
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
        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServ)
        {
            try
            {
                Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApprovalBhyt = new Dictionary<long, V_HIS_HEIN_APPROVAL>();
                Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
                if (IsNotNullOrEmpty(heinApprovalBhyts))
                {
                    foreach (var item in heinApprovalBhyts)
                    {
                        dicHeinApprovalBhyt[item.ID] = item;
                    }
                }
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

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    var heinServiceTypes = new HisHeinServiceTypeManager().Get(new HisHeinServiceTypeFilterQuery()) ?? new List<HIS_HEIN_SERVICE_TYPE>();
                    foreach (var sere in ListSereServ)
                    {
                        if (sere.IS_NO_EXECUTE == (short)1 || sere.IS_EXPEND == (short)1
                            || sere.AMOUNT <= 0 || sere.TDL_HEIN_SERVICE_TYPE_ID == null || sere.HEIN_APPROVAL_ID == null)
                            continue;
                        if (listHeinServiceTypeId.Contains(sere.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApprovalBhyt.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                        {
                            var heinAproval = dicHeinApprovalBhyt[sere.HEIN_APPROVAL_ID.Value];

                            Mrs00079RDO rdo = new Mrs00079RDO();
                            rdo.SERVICE_ID = sere.SERVICE_ID; var heinServiceType = heinServiceTypes.FirstOrDefault(o => o.ID == sere.TDL_HEIN_SERVICE_TYPE_ID);
                            if (heinServiceType != null)
                            {
                                rdo.HEIN_SERVICE_TYPE_CODE = heinServiceType.HEIN_SERVICE_TYPE_CODE;
                                rdo.HEIN_SERVICE_TYPE_NAME = heinServiceType.HEIN_SERVICE_TYPE_NAME;
                            }
                            rdo.SERVICE_CODE_DMBYT = sere.TDL_HEIN_SERVICE_BHYT_CODE;
                            rdo.SERVICE_STT_DMBYT = sere.TDL_HEIN_ORDER;
                            rdo.SERVICE_TYPE_NAME = sere.TDL_HEIN_SERVICE_BHYT_NAME;
                            rdo.TOTAL_HEIN_PRICE = sere.ORIGINAL_PRICE * (1 + sere.VAT_RATIO);
                            rdo.BHYT_PAY_RATE = Math.Round(sere.ORIGINAL_PRICE > 0 ? (sere.HEIN_LIMIT_PRICE.HasValue ? (sere.HEIN_LIMIT_PRICE.Value / (sere.ORIGINAL_PRICE * (1 + sere.VAT_RATIO))) * 100 : (sere.PRICE / sere.ORIGINAL_PRICE) * 100) : 0, 0);
                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sere, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH());
                            var treatment = dicTreatment.ContainsKey(sere.TDL_TREATMENT_ID??0)?dicTreatment[sere.TDL_TREATMENT_ID??0]: new V_HIS_TREATMENT();
                            if (checkBhytNsd(treatment))
                            {
                                rdo.TOTAL_HEIN_PRICE_NDS = sere.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                            else
                            {
                                rdo.VIR_TOTAL_HEIN_PRICE = sere.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                            rdo.TOTAL_OTHER_SOURCE_PRICE = (sere.OTHER_SOURCE_PRICE ?? 0) * sere.AMOUNT;
                            if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = sere.AMOUNT;
                            }
                            else if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sere.AMOUNT;
                            }
                            if (rdo.TOTAL_PRICE != 0)
                            {
                                ListServiceRdo.Add(rdo);
                            }
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListHeinApproval))
                {

                    foreach (var heinApproval in ListHeinApproval)
                    {
                        Mrs00079RDO rdo = new Mrs00079RDO(heinApproval);
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
                        rdo.TREATMENT_TYPE_NAME = heinApproval.TREATMENT_TYPE_NAME;
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
                            rdo.ICD_CODE_MAIN = treatment.ICD_CODE;//mã bệnh chính
                            rdo.ICD_NAME_MAIN = treatment.ICD_NAME;//tên bệnh chính
                            rdo.IN_TIME = treatment.IN_TIME;
                            rdo.OUT_TIME = treatment.OUT_TIME;
                            rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                            rdo.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID;
                            rdo.TREATMENT_DAY_COUNT_6556 = HIS.Common.Treatment.Calculation.DayOfTreatment6556(treatment.IN_TIME, treatment.CLINICAL_IN_TIME, treatment.OUT_TIME, treatment.TDL_TREATMENT_TYPE_ID ?? 0) ?? 0;

                            if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.OPEN_TIME_SEPARATE_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                                rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.HasValue ? treatment.OUT_TIME.Value.ToString().Substring(0, 12) : "";
                                if (treatment.OUT_TIME.HasValue)
                                {
                                    if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.TOTAL_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                    }
                                }
                            }
                            else
                            {
                                if (treatment.OUT_TIME.HasValue && treatment.CLINICAL_IN_TIME.HasValue)
                                {
                                    rdo.OPEN_TIME_SEPARATE_STR = treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                                    rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                                    if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.TOTAL_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
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
                                else
                                {
                                    rdo.OPEN_TIME_SEPARATE_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                                }
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
                            if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "1";
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "2";
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "3";
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "4";
                            }
                            rdo.ICD_CODE_EXTRA = treatment.ICD_SUB_CODE; //mã bệnh khác
                            rdo.ICD_NAME_EXTRA = treatment.ICD_TEXT; //tên bệnh khác
                            rdo.TRANSFER_IN_MEDI_ORG_CODE = treatment.TRANSFER_IN_MEDI_ORG_CODE;//mã nơi chuyển đến
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
                            var sereServ = ListSereServ.Where(x => x.HEIN_APPROVAL_ID == heinApproval.ID).ToList();
                            if (sereServ != null)
                            {
                                ProcessTotalPrice(rdo, sereServ);
                            }

                            listRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
         
        private void ProcessTotalPrice(Mrs00079RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;

                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                        {
                            rdo.MATERIAL_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL)
                        {
                            rdo.MATERIAL_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                        {
                            rdo.MEDICINE_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            rdo.BED_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += TotalPriceTreatment;
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
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0);
                    }
                }
                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE ?? 0;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private bool CheckPrice(Mrs00079RDO rdo)
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
        private List<Mrs00079RDO> ProcessListRDO()
        {
            List<Mrs00079RDO> listCurrent = new List<Mrs00079RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListServiceRdo))
                {
                    var groupExams = ListServiceRdo.GroupBy(o => new { o.SERVICE_CODE_DMBYT, o.TOTAL_HEIN_PRICE, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00079RDO> listsub = group.ToList<Mrs00079RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00079RDO rdo = new Mrs00079RDO();
                            rdo.HEIN_SERVICE_TYPE_CODE = listsub[0].HEIN_SERVICE_TYPE_CODE;
                            rdo.HEIN_SERVICE_TYPE_NAME = listsub[0].HEIN_SERVICE_TYPE_NAME;
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.HEIN_PRICE = listsub[0].HEIN_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                                rdo.VIR_TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE;
                                rdo.AMOUNT += item.AMOUNT;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
                            listCurrent.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.SERVICE_CODE_DMBYT).ThenByDescending(o => o.TOTAL_HEIN_PRICE).ToList();
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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", ListServiceRdo);
                objectTag.AddObjectData(store, "ReportDetail", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

     
    }
}
