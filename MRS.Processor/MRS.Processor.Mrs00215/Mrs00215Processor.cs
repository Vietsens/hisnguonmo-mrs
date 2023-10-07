using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
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
using FlexCel.Report;
using HIS.Common.Treatment;

namespace MRS.Processor.Mrs00215
{
    public class Mrs00215Processor : AbstractProcessor
    {
        Mrs00215Filter castFilter = null;

        List<Mrs00215RDO> ListRdoA = new List<Mrs00215RDO>();
        List<Mrs00215RDO> ListRdoB = new List<Mrs00215RDO>();
        List<Mrs00215RDO> ListRdoC = new List<Mrs00215RDO>();

        List<Mrs00215RDO> ListSumTotal = new List<Mrs00215RDO>();

        private decimal TotalAmount = 0;

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        HIS_BRANCH _Branch = null;
        string MaterialPriceOption = "";

        public Mrs00215Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00215Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00215Filter)this.reportFilter;

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
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00215");
                }

                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
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

                        if ((castFilter.DEPARTMENT_ID ?? 0) != 0)
                        {
                            ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
                        }
                        ListSereServ = ListSereServ.Where(o => ListTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00215.");
                        }
                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("Khong lay duoc du lieu HEIN_APPROVAL_BHYT: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ListHeinApproval), ListHeinApproval));
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoA.Clear();
                ListRdoB.Clear();
                ListRdoC.Clear();
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

                    //issues/13048
                    //Sua doi voi bn co >= 2 the bhyt trong 1 lan dieu tri thi chi hien thi 1 dong tren bao cao. Lay dong the bhyt dau tien. Gop tat ca cac chi phi vao ma the dau tien

                    var groupTreatment = hisHeinApprovals.GroupBy(o => o.TREATMENT_ID).ToList();

                    foreach (var heinApproval in groupTreatment)
                    {
                        if (heinApproval.ToList().Exists(s => s.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && CheckHeinCardNumberType(s.HEIN_CARD_NUMBER)))
                        {

                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.First().BRANCH_ID);
                            Mrs00215RDO rdo = new Mrs00215RDO(heinApproval.First());
                            if (dicTreatment.ContainsKey(heinApproval.First().TREATMENT_ID))
                            {
                                var treatment = dicTreatment[heinApproval.First().TREATMENT_ID];
                                rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                                rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
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
                                }

                                if (treatment.OUT_TIME.HasValue & treatment.CLINICAL_IN_TIME.HasValue)
                                {
                                    rdo.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME.Value);
                                    rdo.CLOSE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                                    if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.TOTAL_DATE = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DATE = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                                    }
                                    if (rdo.TOTAL_DATE == 0)
                                    {
                                        rdo.TOTAL_DATE = null;
                                    }
                                }
                                else if (treatment.CLINICAL_IN_TIME.HasValue)
                                {
                                    rdo.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME.Value);
                                }
                            }

                            foreach (var item in heinApproval)
                            {
                                if (dicSereServHein.ContainsKey(item.ID))
                                {
                                    ProcessTotalPrice(rdo, dicSereServHein[item.ID]);
                                }
                            }

                            TotalAmount += rdo.TOTAL_HEIN_PRICE;
                            //khong co gia thi bo qua
                            if (!CheckPrice(rdo)) continue;

                            if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(rdo.HEIN_MEDI_ORG_CODE)
                                && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
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
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessTotalPrice(Mrs00215RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            //try
            //{
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
            rdo.TOTAL_HEIN_PRICE = rdo.TOTAL_PRICE - rdo.TOTAL_PATIENT_PRICE;
            if (checkBhytNsd(rdo))
            {
                rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                rdo.TOTAL_HEIN_PRICE = 0;
            }
        }

        private bool CheckPrice(Mrs00215RDO rdo)
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

        private void ProcessSumTotal()
        {
            try
            {
                Mrs00215RDO rdo = new Mrs00215RDO();
                rdo.TOTAL_PRICE = (ListRdoA.Sum(s => s.TOTAL_PRICE) + ListRdoB.Sum(s => s.TOTAL_PRICE) + ListRdoC.Sum(s => s.TOTAL_PRICE));
                rdo.TEST_PRICE = (ListRdoA.Sum(s => s.TEST_PRICE) + ListRdoB.Sum(s => s.TEST_PRICE) + ListRdoC.Sum(s => s.TEST_PRICE));
                rdo.DIIM_PRICE = (ListRdoA.Sum(s => s.DIIM_PRICE) + ListRdoB.Sum(s => s.DIIM_PRICE) + ListRdoC.Sum(s => s.DIIM_PRICE));
                rdo.MEDICINE_PRICE = (ListRdoA.Sum(s => s.MEDICINE_PRICE) + ListRdoB.Sum(s => s.MEDICINE_PRICE) + ListRdoC.Sum(s => s.MEDICINE_PRICE));
                rdo.BLOOD_PRICE = (ListRdoA.Sum(s => s.BLOOD_PRICE) + ListRdoB.Sum(s => s.BLOOD_PRICE) + ListRdoC.Sum(s => s.BLOOD_PRICE));
                rdo.SURGMISU_PRICE = (ListRdoA.Sum(s => s.SURGMISU_PRICE) + ListRdoB.Sum(s => s.SURGMISU_PRICE) + ListRdoC.Sum(s => s.SURGMISU_PRICE));
                rdo.MATERIAL_PRICE = (ListRdoA.Sum(s => s.MATERIAL_PRICE) + ListRdoB.Sum(s => s.MATERIAL_PRICE) + ListRdoC.Sum(s => s.MATERIAL_PRICE));
                rdo.SERVICE_PRICE_RATIO = (ListRdoA.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoB.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Sum(s => s.SERVICE_PRICE_RATIO));
                rdo.MEDICINE_PRICE_RATIO = (ListRdoA.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoB.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO));
                rdo.MATERIAL_PRICE_RATIO = (ListRdoA.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoB.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO));
                rdo.BED_PRICE = (ListRdoA.Sum(s => s.BED_PRICE) + ListRdoB.Sum(s => s.BED_PRICE) + ListRdoC.Sum(s => s.BED_PRICE));
                rdo.EXAM_PRICE = (ListRdoA.Sum(s => s.EXAM_PRICE) + ListRdoB.Sum(s => s.EXAM_PRICE) + ListRdoC.Sum(s => s.EXAM_PRICE));
                rdo.TRAN_PRICE = (ListRdoA.Sum(s => s.TRAN_PRICE) + ListRdoB.Sum(s => s.TRAN_PRICE) + ListRdoC.Sum(s => s.TRAN_PRICE));
                rdo.TT_PRICE = (ListRdoA.Sum(s => s.TT_PRICE) + ListRdoB.Sum(s => s.TT_PRICE) + ListRdoC.Sum(s => s.TT_PRICE));
                rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                rdo.TOTAL_HEIN_PRICE = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoA.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoB.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoC.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                ListSumTotal.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
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

        private bool checkBhytNsd(Mrs00215RDO rdo)
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

            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
            dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            ProcessSumTotal();
            objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
            objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
            objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
            objectTag.AddObjectData(store, "SumTotals", ListSumTotal);
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