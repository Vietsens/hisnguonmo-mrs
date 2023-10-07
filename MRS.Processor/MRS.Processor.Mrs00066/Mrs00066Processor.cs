using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using FlexCel.Report;
using HIS.Common.Treatment;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00066
{
    public class Mrs00066Processor : AbstractProcessor
    {
        Mrs00066Filter castFilter = null;
        List<Mrs00066RDO> ListRdo = new List<Mrs00066RDO>();
        List<Mrs00066RDO> ListRdoPTTM = new List<Mrs00066RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        HIS_BRANCH _Branch = null;
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        string MaterialPriceOption = "";

        public Mrs00066Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00066Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = false;
            try
            {
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                castFilter = ((Mrs00066Filter)this.reportFilter);

                LoadDataToRam();
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
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    ListHeinApproval = ListHeinApproval.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT).ToList();
                }

                if (ListHeinApproval != null && ListHeinApproval.Count > 0)
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

                        List<V_HIS_TREATMENT> treatment = new List<V_HIS_TREATMENT>();
                        if (!IsNotNullOrEmpty(ListTreatment))
                        {
                            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                            treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                            treatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                            if (castFilter.PATIENT_TYPE_IDs != null)
                            {
                                treatment = treatment.Where(p => castFilter.PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                            }
                        }
                        else
                        {
                            treatment.AddRange(ListTreatment);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra taij DAOGET trong qua trinh tong hop du lieu MRS00066.");
                        }

                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ, treatment);
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
                            if (item.HEIN_APPROVAL_ID == null || item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.AMOUNT <= 0)
                                continue;
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                        }
                    }

                    foreach (var heinApproval in hisHeinApprovals.OrderByDescending(o => o.HEIN_CARD_TO_TIME).ToList())
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            continue;
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);

                        if (dicSereServ.ContainsKey(heinApproval.ID))
                        {
                            var existsedRdo = ListRdo.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE && (o.HEIN_CARD_NUMBER == heinApproval.HEIN_CARD_NUMBER || this.castFilter.IS_MERGE_TREATMENT == true));
                            if (existsedRdo != null)
                            {
                                int id = ListRdo.IndexOf(existsedRdo);
                                ProcessTotalPrice(ListRdo[id], dicSereServ[heinApproval.ID]);
                            }
                            else
                            {
                                Mrs00066RDO rdo = new Mrs00066RDO(heinApproval);

                                rdo.SENDER_HEIN_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
                                if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                {
                                    rdo.TREATMENT_TYPE_CODE = "1";
                                }
                                else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)// Config.IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                                {
                                    rdo.TREATMENT_TYPE_CODE = "2";
                                }
                                else
                                {
                                    rdo.TREATMENT_TYPE_CODE = "3";
                                }

                                rdo.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
                                rdo.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
                                rdo.DOB_DATE = Inventec.Common.TypeConvert.Parse.ToInt64(heinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 8));
                                if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                                {
                                    rdo.HEIN_GENDER_CODE = "1";
                                }
                                else if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                                {
                                    rdo.HEIN_GENDER_CODE = "2";
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
                                        if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                        {
                                            rdo.MAIN_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                        }
                                        else
                                            rdo.MAIN_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                    }

                                    rdo.IN_TIME = treatment.IN_TIME;
                                    rdo.OUT_TIME = treatment.OUT_TIME;
                                    rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                                    rdo.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID;
                                    rdo.TREATMENT_DAY_COUNT_6556 = HIS.Common.Treatment.Calculation.DayOfTreatment6556(treatment.IN_TIME, treatment.CLINICAL_IN_TIME, treatment.OUT_TIME, treatment.TDL_TREATMENT_TYPE_ID ?? 0) ?? 0;

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
                                    else
                                    {
                                        rdo.TREATMENT_END_TYPE_CODE = "1";
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
                                    ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID]);

                                    //khong co gia thi bo qua
                                    if (!CheckPrice(rdo)) continue;

                                    ListRdo.Add(rdo);
                                }
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
        
        private void ProcessTotalPrice(Mrs00066RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    //var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM)
                        {
                            rdo.MATERIAL_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                        {
                            rdo.MATERIAL_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
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
                        if (rdo.TOTAL_PATIENT_PRICE<0)
                        {
                            rdo.TOTAL_PATIENT_PRICE = 0;
                        }
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                    }

                    
                }
                if (rdo.HEIN_CARD_NUMBER != null)
                {
                    if (checkBhytNsd(rdo))
                    {
                        rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                        rdo.TOTAL_HEIN_PRICE = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckPrice(Mrs00066RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TRAN_PRICE > 0|| rdo.TT_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool checkBhytNsd(Mrs00066RDO rdo)
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

        private void LoadDataToRam()
        {
            try
            {
                if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                    ListTreatment = new HisTreatmentManager().GetView(treatmentFilter);

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
                    ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager().GetView(approvalFilter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportPTTM", ListRdoPTTM);
                objectTag.SetUserFunction(store, "fRound", new CustomerFuncRoundData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public static decimal TotalPrice(object sese, HIS_BRANCH branch, bool IsMaterialOriginalPriceEqualPrice)
        {
            var SereServ = new HIS_SERE_SERV();
            decimal result = 0;
            try
            {

                if (sese is HIS_SERE_SERV)
                {
                    SereServ = (HIS_SERE_SERV)sese;
                    if ((sese as HIS_SERE_SERV).IS_NO_EXECUTE == 1) return 0;
                    if ((sese as HIS_SERE_SERV).IS_EXPEND == 1) return 0;
                    if ((sese as HIS_SERE_SERV).VIR_TOTAL_HEIN_PRICE == 0) return 0;
                }
                else if (sese is V_HIS_SERE_SERV_3)
                {
                    if ((sese as V_HIS_SERE_SERV_3).IS_NO_EXECUTE == 1) return 0;
                    if ((sese as V_HIS_SERE_SERV_3).IS_EXPEND == 1) return 0;
                    if ((sese as V_HIS_SERE_SERV_3).VIR_TOTAL_HEIN_PRICE == 0) return 0;
                    SereServ.HEIN_LIMIT_PRICE = (sese as V_HIS_SERE_SERV_3).HEIN_LIMIT_PRICE;
                    SereServ.ORIGINAL_PRICE = (sese as V_HIS_SERE_SERV_3).ORIGINAL_PRICE;
                    SereServ.VAT_RATIO = (sese as V_HIS_SERE_SERV_3).VAT_RATIO;
                    SereServ.PRICE = (sese as V_HIS_SERE_SERV_3).PRICE;
                    SereServ.TDL_HEIN_SERVICE_TYPE_ID = (sese as V_HIS_SERE_SERV_3).TDL_HEIN_SERVICE_TYPE_ID;
                    SereServ.TDL_HST_BHYT_CODE = (sese as V_HIS_SERE_SERV_3).TDL_HST_BHYT_CODE;
                    SereServ.AMOUNT = (sese as V_HIS_SERE_SERV_3).AMOUNT;
                    SereServ.VIR_TOTAL_PRICE = (sese as V_HIS_SERE_SERV_3).VIR_TOTAL_PRICE;
                }

                var listHeinServiceTypeMate = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                };
                if (IsMaterialOriginalPriceEqualPrice == true && listHeinServiceTypeMate.Contains(SereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                {
                    SereServ.ORIGINAL_PRICE = SereServ.PRICE;
                }
                var tyle = SereServ.HEIN_LIMIT_PRICE.HasValue ? (SereServ.HEIN_LIMIT_PRICE.Value / (SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO))) : (SereServ.PRICE / SereServ.ORIGINAL_PRICE);

                var listHeinServiceTypeMedi = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU
                };
                if (listHeinServiceTypeMedi.Contains(SereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                {
                    result = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 4) * SereServ.AMOUNT;
                }
                else if ((SereServ.TDL_HST_BHYT_CODE == "15" /*&& (xml3.TyLeTT == 50 || xml3.TyLeTT == 30)*/)
                    || (SereServ.TDL_HST_BHYT_CODE == "13" /*&& (xml3.TyLeTT == 30 || xml3.TyLeTT == 10)*/)
                    || (SereServ.TDL_HST_BHYT_CODE == "8" && ((tyle * 100) == 50 || (tyle * 100) == 80))
                    || branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)
                {
                    result = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 4) * SereServ.AMOUNT * tyle;
                }
                else
                {
                    result = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 4) * SereServ.AMOUNT;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
                result = 0;
            }
            if (result != (SereServ.VIR_TOTAL_PRICE ?? 0))
            {
                Inventec.Common.Logging.LogSystem.Info(";ss.id:" + SereServ.ID + ";ss.HEIN_LIMIT_PRICE:" + SereServ.HEIN_LIMIT_PRICE + ";ss.ORIGINAL_PRICE:" + SereServ.ORIGINAL_PRICE + ";ss.PRICE:" + SereServ.PRICE + ";ss.AMOUNT:" + SereServ.AMOUNT + ";ss.PRICE:" + SereServ.PRICE + ";ss.VIR_TOTAL_PRICE:" + SereServ.VIR_TOTAL_PRICE);
            }
            return result;
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
