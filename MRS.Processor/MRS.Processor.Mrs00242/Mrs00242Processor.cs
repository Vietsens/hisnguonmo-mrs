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
using FlexCel.Report;
using HIS.Common.Treatment;
using MOS.MANAGER.HisMediOrg;

namespace MRS.Processor.Mrs00242
{
    public class Mrs00242Processor : AbstractProcessor
    {
        List<Mrs00242RDO> ListRdo = new List<Mrs00242RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        Dictionary<string, HIS_MEDI_ORG> dicMediOrg = new Dictionary<string, HIS_MEDI_ORG>();

        Mrs00242Filter castFilter = null;

        HIS_BRANCH _Branch = null;
        string MaterialPriceOption = "";

        public Mrs00242Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00242Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00242Filter)this.reportFilter);

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, Mrs00242");
                }

                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;

                //danh sách bệnh viện
                GetMediOrg();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMediOrg()
        {
            var mediOrgs = new HisMediOrgManager().Get(new HisMediOrgFilterQuery()) ?? new List<HIS_MEDI_ORG>();
            dicMediOrg = mediOrgs.GroupBy(o => o.MEDI_ORG_CODE ?? "").ToDictionary(p => p.Key, q => q.First());
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

                        if ((castFilter.DEPARTMENT_ID ?? 0) != 0)
                        {
                            ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
                        }
                        ListSereServ = ListSereServ.Where(o => ListTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu Mrs00242.");
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

                    //issues/13048
                    //Sua doi voi bn co >= 2 the bhyt trong 1 lan dieu tri thi chi hien thi 1 dong tren bao cao. Lay dong the bhyt dau tien. Gop tat ca cac chi phi vao ma the dau tien

                    var groupTreatment = hisHeinApprovals.GroupBy(o => o.TREATMENT_ID).ToList();

                    foreach (var heinApproval in groupTreatment)
                    {
                        if (heinApproval.ToList().Exists(s => s.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && CheckHeinCardNumberType(s.HEIN_CARD_NUMBER)))
                        {
                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.First().BRANCH_ID) ?? new HIS_BRANCH();
                            Mrs00242RDO rdo = new Mrs00242RDO(heinApproval.First());
                            //Loai kham chua benh:(1: kham benh;  2: dieu tri ngoai tru;  3: dieu tri noi tru
                            if (heinApproval.First().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                rdo.TREATMENT_TYPE_CODE = "1";
                            }
                            else if (heinApproval.First().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            {
                                rdo.TREATMENT_TYPE_CODE = "2";
                            }
                            else if (heinApproval.First().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.TREATMENT_TYPE_CODE = "3";
                            }
                            else
                                rdo.TREATMENT_TYPE_CODE = "";

                            //Ly do vao vien:

                            var mediOrg = dicMediOrg.ContainsKey(heinApproval.First().HEIN_MEDI_ORG_CODE ?? "") ? dicMediOrg[heinApproval.First().HEIN_MEDI_ORG_CODE ?? ""] : new HIS_MEDI_ORG();
                            if (heinApproval.First().RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            {
                                if (heinApproval.First().RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                                {
                                    rdo.MA_LYDO_VVIEN = "2";
                                }
                                else if (heinApproval.First().RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.OVER)
                                {
                                    rdo.MA_LYDO_VVIEN = "4";
                                }
                                else if (heinApproval.First().RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT)
                                {
                                    rdo.MA_LYDO_VVIEN = "1";
                                }

                                else if (!String.IsNullOrWhiteSpace(heinApproval.First().HEIN_MEDI_ORG_CODE) &&
                                    (heinApproval.First().HEIN_MEDI_ORG_CODE == this._Branch.HEIN_MEDI_ORG_CODE
                                    || (!String.IsNullOrWhiteSpace(this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.First().HEIN_MEDI_ORG_CODE))
                                    ))
                                {
                                    rdo.MA_LYDO_VVIEN = "1";
                                }
                                else if (this._Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || this._Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE
                                    )
                                {
                                    rdo.MA_LYDO_VVIEN = "3";
                                    if (((heinApproval.First().HEIN_MEDI_ORG_CODE ?? "  ").Substring(0, 2) == this._Branch.HEIN_PROVINCE_CODE) && mediOrg != null && (mediOrg.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || mediOrg.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE))
                                    {
                                        rdo.MA_LYDO_VVIEN = "4";
                                    }
                                }
                                else
                                    rdo.MA_LYDO_VVIEN = "1";
                            }
                            else if (heinApproval.First().RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                            {
                                rdo.MA_LYDO_VVIEN = "3";
                            }

                            if (dicTreatment.ContainsKey(heinApproval.First().TREATMENT_ID))
                            {
                                var treatment = dicTreatment[heinApproval.First().TREATMENT_ID];
                                rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                                rdo.IN_TIME_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                                if (treatment.OUT_TIME.HasValue)
                                {
                                    if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.MAIN_DAY = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.MAIN_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                                    }
                                    //rdo.MAIN_DAY = Calculation.DayOfTreatment(treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                }

                                rdo.IN_TIME = treatment.IN_TIME;
                                rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                                rdo.OUT_TIME = treatment.OUT_TIME;

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
                                        rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                                    }
                                    //rdo.TOTAL_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                                    if (rdo.TOTAL_DAY == 0)
                                    {
                                        rdo.TOTAL_DAY = null;
                                    }
                                }
                                else if (treatment.CLINICAL_IN_TIME.HasValue)
                                {
                                    rdo.OPEN_TIME_SEPARATE_STR = treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                                }

                                if (treatment.END_DEPARTMENT_ID.HasValue)
                                {
                                    var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                                    if (departmentCodeBHYT != null)
                                    {
                                        rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                                        rdo.END_DEPARTMENT_NAME = departmentCodeBHYT.DEPARTMENT_NAME;
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

                                rdo.MEDI_ORG_FROM_CODE = treatment.MEDI_ORG_CODE;
                            }
                            rdo.CURRENT_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;


                            foreach (var item in heinApproval)
                            {
                                if (dicSereServ.ContainsKey(item.ID))
                                {
                                    ProcessTotalPrice(rdo, dicSereServ[item.ID]);
                                }
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

        private void ProcessTotalPrice(Mrs00242RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                if (IsNotNullOrEmpty(hisSereServs))
                {
                    hisSereServs = hisSereServs.Where(o => o.VIR_TOTAL_HEIN_PRICE.HasValue && o.VIR_TOTAL_HEIN_PRICE.Value > 0).ToList();
                }

                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;

                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == rdo.BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            rdo.BED_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            rdo.SERVICE_PRICE_RATIO += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            rdo.TRAN_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)
                        {
                            rdo.TT_PRICE += Math.Round(TotalPriceTreatment, 2);
                        }
                        else
                        {
                            continue;
                        }
                        rdo.TOTAL_PRICE += Math.Round(TotalPriceTreatment, 2);

                        rdo.TOTAL_PATIENT_PRICE += Math.Round(TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2);
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

        private bool CheckPrice(Mrs00242RDO rdo)
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

        private bool checkBhytNsd(Mrs00242RDO rdo)
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
            dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.SetUserFunction(store, "fRound", new CustomerFuncRoundData());

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