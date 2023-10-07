using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
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
using HIS.Common.Treatment;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00276
{
    public class Mrs00276Processor : AbstractProcessor
    {
        Mrs00276Filter castFilter = null;

        List<Mrs00276RDO> ListRdoA = new List<Mrs00276RDO>();
        List<Mrs00276RDO> ListRdoB = new List<Mrs00276RDO>();
        List<Mrs00276RDO> ListRdoC = new List<Mrs00276RDO>();

        Dictionary<int, Mrs00276RDO> dicRdoA = new Dictionary<int, Mrs00276RDO>();
        Dictionary<int, Mrs00276RDO> dicRdoB = new Dictionary<int, Mrs00276RDO>();
        Dictionary<int, Mrs00276RDO> dicRdoC = new Dictionary<int, Mrs00276RDO>();

        List<Mrs00276RDO> ListTotalRdoA = new List<Mrs00276RDO>();
        List<Mrs00276RDO> ListTotalRdoB = new List<Mrs00276RDO>();
        List<Mrs00276RDO> ListTotalRdoC = new List<Mrs00276RDO>();

        const int DT = 1;
        const int DTCC = 2;
        const int TT = 3;
        decimal TotalAmount = 0;

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        HIS_BRANCH _Branch = null;

        public Mrs00276Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00276Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00276Filter)this.reportFilter);
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, Mrs00276, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, Mrs00276");
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
                    ListHeinApproval = ListHeinApproval.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM).ToList();
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
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu Mrs00276.");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ListRdoA = dicRdoA.Select(s => s.Value).OrderBy(o => o.IS_DUNG_TRAI).ToList();
                    ListRdoB = dicRdoB.Select(s => s.Value).OrderBy(o => o.IS_DUNG_TRAI).ToList();
                    ListRdoC = dicRdoC.Select(s => s.Value).OrderBy(o => o.IS_DUNG_TRAI).ToList();

                    ListRdoA = CheckList(ListRdoA);
                    ListRdoB = CheckList(ListRdoB);
                    ListRdoC = CheckList(ListRdoC);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoA.Clear();
                ListRdoC.Clear();
                ListRdoB.Clear();
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
                            if (sere.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.AMOUNT > 0 && sere.HEIN_APPROVAL_ID.HasValue && sere.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            {
                                if (!dicSereServ.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                                    dicSereServ[sere.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServ[sere.HEIN_APPROVAL_ID.Value].Add(sere);
                            }
                        }
                    }

                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                        {
                            Mrs00276RDO rdo = null;
                            int isDungTrai = 0;
                            if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            {
                                if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                                {
                                    isDungTrai = DTCC;
                                }
                                else
                                {
                                    isDungTrai = DT;
                                }
                            }
                            else
                            {
                                isDungTrai = TT;
                            }

                            if (!String.IsNullOrWhiteSpace(this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                            {
                                if (!dicRdoA.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00276RDO(heinApproval);
                                    dicRdoA[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoA[isDungTrai];
                                }
                            }
                            else if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                            {
                                if (!dicRdoB.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00276RDO(heinApproval);
                                    dicRdoB[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoB[isDungTrai];
                                }
                            }
                            else
                            {
                                if (!dicRdoC.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00276RDO(heinApproval);
                                    dicRdoC[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoC[isDungTrai];
                                }
                            }

                            if (IsNotNull(rdo) && dicSereServ.ContainsKey(heinApproval.ID) && dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID], heinApproval, dicTreatment[heinApproval.TREATMENT_ID]);
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

        private void ProcessTotalPrice(Mrs00276RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs, V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment)
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
                            totalHeinPrice += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                        }
                    }

                    rdo.TOTAL_COUNT++;
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

        private List<Mrs00276RDO> CheckList(List<Mrs00276RDO> listRdo)
        {
            List<Mrs00276RDO> result = new List<Mrs00276RDO>();
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
                result = new List<Mrs00276RDO>();
            }
            return result;
        }

        private bool CheckPrice(Mrs00276RDO rdo)
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
                Mrs00276RDO rdoA = new Mrs00276RDO();
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

                Mrs00276RDO rdoB = new Mrs00276RDO();
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

                Mrs00276RDO rdoC = new Mrs00276RDO();
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
            try
            {
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                ProcessSumTotal();

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotalAs", ListTotalRdoA);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotalBs", ListTotalRdoB);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotalCs", ListTotalRdoC);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
