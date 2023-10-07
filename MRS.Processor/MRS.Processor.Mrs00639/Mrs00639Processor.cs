using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
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
using MOS.MANAGER.HisHeinApproval;
using MRS.MANAGER.Config;
using HIS.Common.Treatment;
using System.Reflection;

namespace MRS.Processor.Mrs00639
{
    public class Mrs00639Processor : AbstractProcessor
    {
        List<Mrs00639RDO> ListRdo = new List<Mrs00639RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();

        Mrs00639Filter castFilter = null;

        HIS_BRANCH _Branch = null;

        public Mrs00639Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00639Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00639Filter)this.reportFilter);
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                MOS.MANAGER.HisMaterialType.HisMaterialTypeFilterQuery mateFilter = new MOS.MANAGER.HisMaterialType.HisMaterialTypeFilterQuery();
                listMaterialType = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).Get(mateFilter);
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, Mrs00639");
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
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu Mrs00639.");
                        }
                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
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

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var treatment in ListTreatment)
                        {
                            dicTreatment[treatment.ID] = treatment;
                        }
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var sere in ListSereServ)
                        {
                            if (sere.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.AMOUNT > 0 && sere.HEIN_APPROVAL_ID.HasValue && sere.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.PRICE != 0)
                            {
                                if (!dicSereServ.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                                    dicSereServ[sere.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServ[sere.HEIN_APPROVAL_ID.Value].Add(sere);
                            }
                        }
                    }

                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM && CheckHeinCardNumberType(heinApproval.HEIN_CARD_NUMBER))
                        {
                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);

                            if (dicSereServ.ContainsKey(heinApproval.ID))
                            {
                                var existsedRdo = ListRdo.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE && o.HEIN_CARD_NUMBER == heinApproval.HEIN_CARD_NUMBER && o.HEIN_CARD_TO_TIME == heinApproval.HEIN_CARD_TO_TIME);
                                if (existsedRdo != null)
                                {
                                    int id = ListRdo.IndexOf(existsedRdo);
                                    ProcessTotalPrice(ListRdo[id], dicSereServ[heinApproval.ID]);
                                }
                                else
                                {
                                    Mrs00639RDO rdo = new Mrs00639RDO(heinApproval);
                                    if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                    {
                                        rdo.TREATMENT_TYPE_CODE = "1";
                                    }
                                    else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                                    {
                                        rdo.TREATMENT_TYPE_CODE = "2";
                                    }
                                    else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                    {
                                        rdo.TREATMENT_TYPE_CODE = "3";
                                    }
                                    else
                                        rdo.TREATMENT_TYPE_CODE = "";
                                    if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                                    {
                                        var treatment = dicTreatment[heinApproval.TREATMENT_ID];
                                        rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                                        rdo.OPEN_TIME_SEPARATE_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                                        if (treatment.CLINICAL_IN_TIME != null)
                                        {
                                            rdo.CLINICAL_IN_TIME_STR = treatment.CLINICAL_IN_TIME.ToString().Substring(0, 12);
                                        }
                                        rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.HasValue ? treatment.OUT_TIME.Value.ToString().Substring(0, 12) : "";
                                        if (treatment.OUT_TIME.HasValue)
                                        {
                                            rdo.TOTAL_DAY = CountDay(treatment);
                                        }
                                        //rdo.TOTAL_DAY = 0; 
                                        if (treatment.END_DEPARTMENT_ID.HasValue)
                                        {
                                            var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                                            if (departmentCodeBHYT != null)
                                            {
                                                rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                                            }
                                        }

                                        rdo.END_ROOM_NAME = treatment.END_ROOM_NAME;
                                        rdo.END_ROOM_CODE = treatment.END_ROOM_CODE;
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
                                        if (treatment.TREATMENT_END_TYPE_ID == MRS.MANAGER.Config.HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN)
                                        {
                                            rdo.TREATMENT_END_TYPE_CODE = "1";
                                        }
                                        else if (treatment.TREATMENT_END_TYPE_ID == MRS.MANAGER.Config.HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN)
                                        {
                                            rdo.TREATMENT_END_TYPE_CODE = "2";
                                        }
                                        else if (treatment.TREATMENT_END_TYPE_ID == MRS.MANAGER.Config.HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__TRONVIEN)
                                        {
                                            rdo.TREATMENT_END_TYPE_CODE = "3";
                                        }
                                        else if (treatment.TREATMENT_END_TYPE_ID == MRS.MANAGER.Config.HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__XINRAVIEN)
                                        {
                                            rdo.TREATMENT_END_TYPE_CODE = "4";
                                        }
                                        rdo.ICD_CODE_EXTRA = treatment.ICD_SUB_CODE; //
                                    }
                                    rdo.CURRENT_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
                                    rdo.MEDI_ORG_FROM_CODE = dicTreatment[heinApproval.TREATMENT_ID].MEDI_ORG_CODE;

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
        private long CountDay(V_HIS_TREATMENT treatment)
        {
            long result = 0;
            try
            {
                PropertyInfo p = typeof(V_HIS_TREATMENT).GetProperty("TREATMENT_DAY_COUNT");
                if (p != null)
                {
                    var DayCount = p.GetValue(treatment);
                    if (DayCount != null)
                    {
                        result = Int64.Parse(DayCount.ToString());
                    }
                    else
                    {
                        result = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                    }
                }
                else
                {
                    result = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private void ProcessTotalPrice(Mrs00639RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.TDL_TREATMENT_ID.HasValue)
                        continue;
                    if (sereServ.IS_NO_EXECUTE == 1 || sereServ.IS_EXPEND == 1)
                        continue;
                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == rdo.BRANCH_ID) ?? new HIS_BRANCH());

                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            rdo.TEST_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            rdo.DIIM_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                        {
                            rdo.MEDICINE_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                        {
                            rdo.BLOOD_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                        {
                            rdo.SURGMISU_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM)
                        {
                            rdo.MATERIAL_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                        {
                            rdo.MATERIAL_PRICE_RATIO += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                        {
                            rdo.MEDICINE_PRICE_RATIO += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            rdo.BED_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            rdo.SERVICE_PRICE_RATIO += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            rdo.TRAN_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else
                        {
                            continue;
                        }
                        rdo.TOTAL_PATIENT_PRICE += Math.Round(TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2);
                        rdo.TOTAL_PRICE += Math.Round(TotalPriceTreatment, 2);
                        rdo.TOTAL_HEIN_PRICE += Math.Round(TotalPriceTreatment, 2) - Math.Round(TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2);
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

        private bool CheckPrice(Mrs00639RDO rdo)
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

        private bool CheckHeinCardNumberType(string HeinCardNumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    result = true;
                    if (IsNotNullOrEmpty(MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__All))
                    {
                        foreach (var type in MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__All)
                        {
                            if (HeinCardNumber.StartsWith(type))
                            {
                                result = false;
                                break;
                            }
                        }
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

        private bool checkBhytNsd(Mrs00639RDO rdo)
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
