using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
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

namespace MRS.Processor.Mrs00247
{
    public class Mrs00247Processor : AbstractProcessor
    {
        Mrs00247Filter castFilter = null;

        List<Mrs00247RDO> ListRdoA = new List<Mrs00247RDO>();
        List<Mrs00247RDO> ListRdoB = new List<Mrs00247RDO>();
        List<Mrs00247RDO> ListRdoC = new List<Mrs00247RDO>();

        Dictionary<short, Mrs00247RDO> dicRdoA = new Dictionary<short, Mrs00247RDO>();
        Dictionary<short, Mrs00247RDO> dicRdoB = new Dictionary<short, Mrs00247RDO>();
        Dictionary<short, Mrs00247RDO> dicRdoC = new Dictionary<short, Mrs00247RDO>();

        List<Mrs00247RDO> ListTotalRdoA = new List<Mrs00247RDO>();
        List<Mrs00247RDO> ListTotalRdoB = new List<Mrs00247RDO>();
        List<Mrs00247RDO> ListTotalRdoC = new List<Mrs00247RDO>();

        const short DungTuyen = 1;
        const short TraiTuyen = 0;
        decimal TotalAmount = 0;

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        HIS_BRANCH _Branch = null;

        public Mrs00247Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00247Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00247Filter)this.reportFilter);
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, Mrs00247, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, Mrs00247");
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

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if ((castFilter.DEPARTMENT_ID ?? 0) != 0)
                        {
                            ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
                        }
                        ListSereServ = ListSereServ.Where(o => ListTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu Mrs00247.");
                        }
                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRdoA = dicRdoA.Select(s => s.Value).OrderBy(o => o.DUNG_TRAI).ToList();
                    ListRdoB = dicRdoB.Select(s => s.Value).OrderBy(o => o.DUNG_TRAI).ToList();
                    ListRdoC = dicRdoC.Select(s => s.Value).OrderBy(o => o.DUNG_TRAI).ToList();

                    ListRdoA = CheckList(ListRdoA);
                    ListRdoB = CheckList(ListRdoB);
                    ListRdoC = CheckList(ListRdoC);
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
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServHein = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

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
                                if (!dicSereServHein.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                                    dicSereServHein[sere.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServHein[sere.HEIN_APPROVAL_ID.Value].Add(sere);
                            }
                        }
                    }
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && CheckHeinCardNumberType(heinApproval.HEIN_CARD_NUMBER))
                        {
                            Mrs00247RDO rdo = null;
                            short isDungTrai;
                            if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            {
                                isDungTrai = DungTuyen;
                            }
                            else
                            {
                                isDungTrai = TraiTuyen;
                            }

                            if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.HEIN_MEDI_ORG_CODE))
                            {
                                if (!dicRdoA.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00247RDO(heinApproval);
                                    dicRdoA[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoA[isDungTrai];
                                }
                            }
                            else
                            {
                                if (!dicRdoB.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00247RDO(heinApproval);
                                    dicRdoB[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoB[isDungTrai];
                                }
                            }


                            if (IsNotNull(rdo) && dicSereServHein.ContainsKey(heinApproval.ID) && dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServHein[heinApproval.ID], heinApproval, dicTreatment[heinApproval.TREATMENT_ID]);
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

        private void ProcessTotalPrice(Mrs00247RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs, V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisSereServs))
                {
                    decimal totalHeinPrice = 0;
                    foreach (var sereServ in hisSereServs)
                    {
                        if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                            continue;

                        var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
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

                            var patient = Math.Round(TotalPriceTreatment, 2, MidpointRounding.AwayFromZero) - Math.Round((sereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2, MidpointRounding.AwayFromZero);
                            rdo.TOTAL_PATIENT_PRICE += patient;
                            totalHeinPrice += TotalPriceTreatment - patient;
                            rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                        }
                    }
                    rdo.TOTAL_COUNT++;
                    //if (treatment.CLINICAL_IN_TIME.HasValue && treatment.OUT_TIME.HasValue)
                    //{
                    //    rdo.TOTAL_DATE += Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT)??0;
                    //}
                    if (treatment.OUT_TIME.HasValue)
                    {
                        if (treatment.TREATMENT_DAY_COUNT.HasValue)
                        {
                            rdo.TOTAL_DATE += Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                        }
                        else
                        {
                            rdo.TOTAL_DATE += Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                        }
                    }
                    if (checkBhytNsd(heinApproval, treatment))
                    {
                        rdo.TOTAL_HEIN_PRICE_NDS += totalHeinPrice;
                    }
                    else
                    {
                        rdo.TOTAL_HEIN_PRICE += totalHeinPrice;
                    }
                    TotalAmount += totalHeinPrice;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<Mrs00247RDO> CheckList(List<Mrs00247RDO> listRdo)
        {
            List<Mrs00247RDO> result = new List<Mrs00247RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    foreach (var rdo in listRdo)
                    {
                        //khong co gia thi bo qua
                        if (!CheckPrice(rdo)) continue;

                        result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<Mrs00247RDO>();
            }
            return result;
        }

        private bool CheckPrice(Mrs00247RDO rdo)
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
                    if (IsNotNullOrEmpty(MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02))
                    {
                        foreach (var type in MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02)
                        {
                            if (HeinCardNumber.StartsWith(type))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                    else
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


        private bool checkBhytNsd(V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                if (MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(treatment.ICD_CODE))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(treatment.ICD_CODE))
                {
                    if (heinApproval.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(treatment.ICD_CODE.Substring(0, 3)))
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

        private void ProcessSumTotal()
        {
            try
            {
                Mrs00247RDO rdoA = new Mrs00247RDO();
                foreach (var item in ListRdoA)
                {
                    rdoA.BED_PRICE += item.BED_PRICE;
                    rdoA.BLOOD_PRICE += item.BLOOD_PRICE;
                    rdoA.DIIM_PRICE += item.DIIM_PRICE;
                    rdoA.EXAM_PRICE += item.EXAM_PRICE;
                    rdoA.MATERIAL_PRICE += item.MATERIAL_PRICE;
                    rdoA.MATERIAL_PRICE_RATIO += item.MATERIAL_PRICE_RATIO;
                    rdoA.MEDICINE_PRICE += item.MEDICINE_PRICE;
                    rdoA.MEDICINE_PRICE_RATIO += item.MEDICINE_PRICE_RATIO;
                    rdoA.SERVICE_PRICE_RATIO += item.SERVICE_PRICE_RATIO;
                    rdoA.SURGMISU_PRICE += item.SURGMISU_PRICE;
                    rdoA.TEST_PRICE += item.TEST_PRICE;
                    rdoA.TOTAL_DATE += item.TOTAL_DATE;
                    rdoA.TOTAL_COUNT += item.TOTAL_COUNT;
                    rdoA.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                    rdoA.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                    rdoA.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                    rdoA.TOTAL_PRICE += item.TOTAL_PRICE;
                    rdoA.TRAN_PRICE += item.TRAN_PRICE;
                    rdoA.TT_PRICE += item.TT_PRICE;
                    rdoA.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                }
                ListTotalRdoA.Add(rdoA);

                Mrs00247RDO rdoB = new Mrs00247RDO();
                foreach (var item in ListRdoB)
                {
                    rdoB.BED_PRICE += item.BED_PRICE;
                    rdoB.BLOOD_PRICE += item.BLOOD_PRICE;
                    rdoB.DIIM_PRICE += item.DIIM_PRICE;
                    rdoB.EXAM_PRICE += item.EXAM_PRICE;
                    rdoB.MATERIAL_PRICE += item.MATERIAL_PRICE;
                    rdoB.MATERIAL_PRICE_RATIO += item.MATERIAL_PRICE_RATIO;
                    rdoB.MEDICINE_PRICE += item.MEDICINE_PRICE;
                    rdoB.MEDICINE_PRICE_RATIO += item.MEDICINE_PRICE_RATIO;
                    rdoB.SERVICE_PRICE_RATIO += item.SERVICE_PRICE_RATIO;
                    rdoB.SURGMISU_PRICE += item.SURGMISU_PRICE;
                    rdoB.TEST_PRICE += item.TEST_PRICE;
                    rdoB.TOTAL_DATE += item.TOTAL_DATE;
                    rdoB.TOTAL_COUNT += item.TOTAL_COUNT;
                    rdoB.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                    rdoB.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                    rdoB.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                    rdoB.TOTAL_PRICE += item.TOTAL_PRICE;
                    rdoB.TRAN_PRICE += item.TRAN_PRICE;
                    rdoB.TT_PRICE += item.TT_PRICE;
                    rdoB.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                }
                ListTotalRdoB.Add(rdoB);

                Mrs00247RDO rdoC = new Mrs00247RDO();
                foreach (var item in ListRdoC)
                {
                    rdoC.BED_PRICE += item.BED_PRICE;
                    rdoC.BLOOD_PRICE += item.BLOOD_PRICE;
                    rdoC.DIIM_PRICE += item.DIIM_PRICE;
                    rdoC.EXAM_PRICE += item.EXAM_PRICE;
                    rdoC.MATERIAL_PRICE += item.MATERIAL_PRICE;
                    rdoC.MATERIAL_PRICE_RATIO += item.MATERIAL_PRICE_RATIO;
                    rdoC.MEDICINE_PRICE += item.MEDICINE_PRICE;
                    rdoC.MEDICINE_PRICE_RATIO += item.MEDICINE_PRICE_RATIO;
                    rdoC.SERVICE_PRICE_RATIO += item.SERVICE_PRICE_RATIO;
                    rdoC.SURGMISU_PRICE += item.SURGMISU_PRICE;
                    rdoC.TEST_PRICE += item.TEST_PRICE;
                    rdoC.TOTAL_DATE += item.TOTAL_DATE;
                    rdoC.TOTAL_COUNT += item.TOTAL_COUNT;
                    rdoC.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                    rdoC.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                    rdoC.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                    rdoC.TOTAL_PRICE += item.TOTAL_PRICE;
                    rdoC.TRAN_PRICE += item.TRAN_PRICE;
                    rdoC.TT_PRICE += item.TT_PRICE;
                    rdoC.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                }
                ListTotalRdoC.Add(rdoC);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
            dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            ProcessSumTotal();
            objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
            objectTag.AddObjectData(store, "SumTotalAs", ListTotalRdoA);
            objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
            objectTag.AddObjectData(store, "SumTotalBs", ListTotalRdoB);
            objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
            objectTag.AddObjectData(store, "SumTotalCs", ListTotalRdoC);
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