using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisBranch;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Common.Treatment;

namespace MRS.Processor.Mrs00203
{
    public class Mrs00203Processor : AbstractProcessor
    {
        Mrs00203Filter castFilter = null;

        List<Mrs00203RDO> ListRdoA = new List<Mrs00203RDO>();
        List<Mrs00203RDO> ListRdoB = new List<Mrs00203RDO>();
        List<Mrs00203RDO> ListRdoC = new List<Mrs00203RDO>();

        List<Mrs00203RDO> ListRouteB = new List<Mrs00203RDO>();
        List<Mrs00203RDO> ListRouteC = new List<Mrs00203RDO>();

        List<Mrs00203RDO> ListSumTotal = new List<Mrs00203RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        private decimal TotalAmount = 0;
        string a = "";
        HIS_BRANCH _Branch = new HIS_BRANCH();

        public Mrs00203Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00203Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            var result = true;
            try
            {
                castFilter = (Mrs00203Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, Mrs00203 filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);
                Inventec.Common.Logging.LogSystem.Info("ListHeinApproval" + ListHeinApproval.Count);

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
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
                        var hisHeinApprovalBhyts = ListHeinApproval.Skip(start).Take(limit).ToList();
                        var listHeinApprovalId = hisHeinApprovalBhyts.Select(s => s.ID).ToList();
                        var listTreatmentId = hisHeinApprovalBhyts.Select(s => s.TREATMENT_ID).ToList();

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = listHeinApprovalId;
                        List<V_HIS_SERE_SERV_3> ListSereServ = new HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        Inventec.Common.Logging.LogSystem.Info("ListSereServ" + ListSereServ.Count);
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = listTreatmentId;
                        List<V_HIS_TREATMENT> ListTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        Inventec.Common.Logging.LogSystem.Info("ListSereServ" + ListTreatment.Count);
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00203.");
                        }
                        GeneralDataByListHeinApproval(hisHeinApprovalBhyts, ListSereServ, ListTreatment);
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00203.");
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRouteB = ListRdoB.GroupBy(g => g.RIGHT_ROUTE_ID).Select(s => new Mrs00203RDO { RIGHT_ROUTE_ID = s.First().RIGHT_ROUTE_ID, RIGHT_ROUTE_NAME = s.First().RIGHT_ROUTE_NAME, TOTAL_DATE = s.Sum(s1 => s1.TOTAL_DATE), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), TEST_PRICE = s.Sum(s3 => s3.TEST_PRICE), DIIM_PRICE = s.Sum(s4 => s4.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s5 => s5.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s6 => s6.BLOOD_PRICE), SURGMISU_PRICE = s.Sum(s7 => s7.SURGMISU_PRICE), MATERIAL_PRICE = s.Sum(s8 => s8.MATERIAL_PRICE), SERVICE_PRICE_RATIO = s.Sum(s9 => s9.SERVICE_PRICE_RATIO), MEDICINE_PRICE_RATIO = s.Sum(s10 => s10.MEDICINE_PRICE_RATIO), MATERIAL_PRICE_RATIO = s.Sum(s11 => s11.MATERIAL_PRICE_RATIO), BED_PRICE = s.Sum(s12 => s12.BED_PRICE), EXAM_PRICE = s.Sum(s13 => s13.EXAM_PRICE), TRAN_PRICE = s.Sum(s14 => s14.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s15 => s15.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s16 => s16.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s17 => s17.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s18 => s18.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                    ListRouteC = ListRdoC.GroupBy(g => g.RIGHT_ROUTE_ID).Select(s => new Mrs00203RDO { RIGHT_ROUTE_ID = s.First().RIGHT_ROUTE_ID, RIGHT_ROUTE_NAME = s.First().RIGHT_ROUTE_NAME, TOTAL_DATE = s.Sum(s1 => s1.TOTAL_DATE), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), TEST_PRICE = s.Sum(s3 => s3.TEST_PRICE), DIIM_PRICE = s.Sum(s4 => s4.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s5 => s5.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s6 => s6.BLOOD_PRICE), SURGMISU_PRICE = s.Sum(s7 => s7.SURGMISU_PRICE), MATERIAL_PRICE = s.Sum(s8 => s8.MATERIAL_PRICE), SERVICE_PRICE_RATIO = s.Sum(s9 => s9.SERVICE_PRICE_RATIO), MEDICINE_PRICE_RATIO = s.Sum(s10 => s10.MEDICINE_PRICE_RATIO), MATERIAL_PRICE_RATIO = s.Sum(s11 => s11.MATERIAL_PRICE_RATIO), BED_PRICE = s.Sum(s12 => s12.BED_PRICE), EXAM_PRICE = s.Sum(s13 => s13.EXAM_PRICE), TRAN_PRICE = s.Sum(s14 => s14.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s15 => s15.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s16 => s16.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s17 => s17.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s18 => s18.TOTAL_OTHER_SOURCE_PRICE) }).ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListTreatment))
                {
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();

                    foreach (var item in ListSereServ)
                    {
                        if (item.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && item.AMOUNT > 0 && item.HEIN_APPROVAL_ID.HasValue)
                        {
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                        }
                    }

                    foreach (var treatment in ListTreatment)
                    {
                        dicTreatment[treatment.ID] = treatment;
                    }

                    foreach (var heinApproval in hisHeinApprovalBhyts)
                    {
                        Mrs00203RDO rdo = new Mrs00203RDO(heinApproval);
                        bool valid = false;
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                        {
                            valid = true;
                            if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                            {
                                rdo.RIGHT_ROUTE_ID = 2;
                                rdo.RIGHT_ROUTE_NAME = "Cấp Cứu";
                            }
                            else if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            {
                                rdo.RIGHT_ROUTE_ID = 1;
                                rdo.RIGHT_ROUTE_NAME = "Đúng Tuyến";
                            }
                            else if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                            {
                                rdo.RIGHT_ROUTE_ID = 3;
                                rdo.RIGHT_ROUTE_NAME = "Trái Tuyến";
                            }
                        }

                        if (valid)
                        {
                            rdo.HEIN_CARD_NUMBER = rdo.HEIN_APPROVAL_BHYT.HEIN_CARD_NUMBER;
                            rdo.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
                            rdo.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_CODE;
                            rdo.MEDIORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
                            if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.DOB_MALE = GetYearDob(heinApproval.TDL_PATIENT_DOB);
                            }
                            else if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.DOB_FEMALE = GetYearDob(heinApproval.TDL_PATIENT_DOB);
                            }
                            if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                var treatment = dicTreatment[heinApproval.TREATMENT_ID];
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
                                        rdo.MAIN_DAY = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT) ?? 0;
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
                                        rdo.TOTAL_DATE = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT) ?? 0;
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
                            if (dicSereServ.ContainsKey(heinApproval.ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID]);
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

        private void ProcessTotalPrice(Mrs00203RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, new HIS_BRANCH());

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
                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                    }
                }
                TotalAmount += rdo.TOTAL_HEIN_PRICE;
                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }

                //khong co gia thi bo qua
                if (!CheckPrice(rdo)) return;

                if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(rdo.HEIN_APPROVAL_BHYT.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
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
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessSumTotal(Dictionary<string, object> dicSingleData)
        {
            try
            {
                //A
                dicSingleData.Add("A_TOTAL_DATE", ListRdoA.Sum(s => (s.TOTAL_DATE ?? 0)));
                dicSingleData.Add("A_MAIN_DAY", ListRdoA.Sum(s => (s.MAIN_DAY ?? 0)));
                dicSingleData.Add("A_TOTAL_PRICE", ListRdoA.Sum(s => s.TOTAL_PRICE));
                dicSingleData.Add("A_TEST_PRICE", ListRdoA.Sum(s => s.TEST_PRICE));
                dicSingleData.Add("A_DIIM_PRICE", ListRdoA.Sum(s => s.DIIM_PRICE));
                dicSingleData.Add("A_MEDICINE_PRICE", ListRdoA.Sum(s => s.MEDICINE_PRICE));
                dicSingleData.Add("A_BLOOD_PRICE", ListRdoA.Sum(s => s.BLOOD_PRICE));
                dicSingleData.Add("A_SURGMISU_PRICE", ListRdoA.Sum(s => s.SURGMISU_PRICE));
                dicSingleData.Add("A_MATERIAL_PRICE", ListRdoA.Sum(s => s.MATERIAL_PRICE));
                dicSingleData.Add("A_SERVICE_PRICE_RATIO", ListRdoA.Sum(s => s.SERVICE_PRICE_RATIO));
                dicSingleData.Add("A_MEDICINE_PRICE_RATIO", ListRdoA.Sum(s => s.MEDICINE_PRICE_RATIO));
                dicSingleData.Add("A_MATERIAL_PRICE_RATIO", ListRdoA.Sum(s => s.MATERIAL_PRICE_RATIO));
                dicSingleData.Add("A_BED_PRICE", ListRdoA.Sum(s => s.BED_PRICE));
                dicSingleData.Add("A_EXAM_PRICE", ListRdoA.Sum(s => s.EXAM_PRICE));
                dicSingleData.Add("A_TRAN_PRICE", ListRdoA.Sum(s => s.TRAN_PRICE));
                dicSingleData.Add("A_TOTAL_PATIENT_PRICE", ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE));
                dicSingleData.Add("A_TOTAL_HEIN_PRICE", ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE));
                dicSingleData.Add("A_TOTAL_HEIN_PRICE_NDS", ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                dicSingleData.Add("A_TOTAL_OTHER_SOURCE_PRICE", ListRdoA.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));

                //B
                dicSingleData.Add("B_TOTAL_DATE", ListRdoB.Sum(s => (s.TOTAL_DATE ?? 0)));
                dicSingleData.Add("B_MAIN_DAY", ListRdoB.Sum(s => (s.MAIN_DAY ?? 0)));
                dicSingleData.Add("B_TOTAL_PRICE", ListRdoB.Sum(s => s.TOTAL_PRICE));
                dicSingleData.Add("B_TEST_PRICE", ListRdoB.Sum(s => s.TEST_PRICE));
                dicSingleData.Add("B_DIIM_PRICE", ListRdoB.Sum(s => s.DIIM_PRICE));
                dicSingleData.Add("B_MEDICINE_PRICE", ListRdoB.Sum(s => s.MEDICINE_PRICE));
                dicSingleData.Add("B_BLOOD_PRICE", ListRdoB.Sum(s => s.BLOOD_PRICE));
                dicSingleData.Add("B_SURGMISU_PRICE", ListRdoB.Sum(s => s.SURGMISU_PRICE));
                dicSingleData.Add("B_MATERIAL_PRICE", ListRdoB.Sum(s => s.MATERIAL_PRICE));
                dicSingleData.Add("B_SERVICE_PRICE_RATIO", ListRdoB.Sum(s => s.SERVICE_PRICE_RATIO));
                dicSingleData.Add("B_MEDICINE_PRICE_RATIO", ListRdoB.Sum(s => s.MEDICINE_PRICE_RATIO));
                dicSingleData.Add("B_MATERIAL_PRICE_RATIO", ListRdoB.Sum(s => s.MATERIAL_PRICE_RATIO));
                dicSingleData.Add("B_BED_PRICE", ListRdoB.Sum(s => s.BED_PRICE));
                dicSingleData.Add("B_EXAM_PRICE", ListRdoB.Sum(s => s.EXAM_PRICE));
                dicSingleData.Add("B_TRAN_PRICE", ListRdoB.Sum(s => s.TRAN_PRICE));
                dicSingleData.Add("B_TOTAL_PATIENT_PRICE", ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE));
                dicSingleData.Add("B_TOTAL_HEIN_PRICE", ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE));
                dicSingleData.Add("B_TOTAL_HEIN_PRICE_NDS", ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                dicSingleData.Add("B_TOTAL_OTHER_SOURCE_PRICE", ListRdoB.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));

                //C
                dicSingleData.Add("C_TOTAL_DATE", ListRdoC.Sum(s => (s.TOTAL_DATE ?? 0)));
                dicSingleData.Add("C_MAIN_DAY", ListRdoC.Sum(s => (s.MAIN_DAY ?? 0)));
                dicSingleData.Add("C_TOTAL_PRICE", ListRdoC.Sum(s => s.TOTAL_PRICE));
                dicSingleData.Add("C_TEST_PRICE", ListRdoC.Sum(s => s.TEST_PRICE));
                dicSingleData.Add("C_DIIM_PRICE", ListRdoC.Sum(s => s.DIIM_PRICE));
                dicSingleData.Add("C_MEDICINE_PRICE", ListRdoC.Sum(s => s.MEDICINE_PRICE));
                dicSingleData.Add("C_BLOOD_PRICE", ListRdoC.Sum(s => s.BLOOD_PRICE));
                dicSingleData.Add("C_SURGMISU_PRICE", ListRdoC.Sum(s => s.SURGMISU_PRICE));
                dicSingleData.Add("C_MATERIAL_PRICE", ListRdoC.Sum(s => s.MATERIAL_PRICE));
                dicSingleData.Add("C_SERVICE_PRICE_RATIO", ListRdoC.Sum(s => s.SERVICE_PRICE_RATIO));
                dicSingleData.Add("C_MEDICINE_PRICE_RATIO", ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO));
                dicSingleData.Add("C_MATERIAL_PRICE_RATIO", ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO));
                dicSingleData.Add("C_BED_PRICE", ListRdoC.Sum(s => s.BED_PRICE));
                dicSingleData.Add("C_EXAM_PRICE", ListRdoC.Sum(s => s.EXAM_PRICE));
                dicSingleData.Add("C_TRAN_PRICE", ListRdoC.Sum(s => s.TRAN_PRICE));
                dicSingleData.Add("C_TOTAL_PATIENT_PRICE", ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                dicSingleData.Add("C_TOTAL_HEIN_PRICE", ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                dicSingleData.Add("C_TOTAL_HEIN_PRICE_NDS", ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                dicSingleData.Add("C_TOTAL_OTHER_SOURCE_PRICE", ListRdoC.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckPrice(Mrs00203RDO rdo)
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

        private long? GetYearDob(long dob)
        {
            long? result = null;
            try
            {
                if (dob > 0)
                {
                    result = long.Parse((dob.ToString()).Substring(0, 4));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private bool checkBhytNsd(Mrs00203RDO rdo)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
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

            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            }

            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            }

            ProcessSumTotal(dicSingleTag);

            objectTag.AddObjectData(store, "RightRouteBs", ListRouteB);
            objectTag.AddObjectData(store, "RightRouteCs", ListRouteC);
            objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
            objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
            objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);

            objectTag.AddRelationship(store, "RightRouteBs", "PatientTypeBs", "RIGHT_ROUTE_ID", "RIGHT_ROUTE_ID");
            objectTag.AddRelationship(store, "RightRouteCs", "PatientTypeCs", "RIGHT_ROUTE_ID", "RIGHT_ROUTE_ID");

            objectTag.SetUserFunction(store, "FuncRownumber", new FuncRownumberMultiData());


        }
    }
    class FuncRownumberMultiData : FlexCel.Report.TFlexCelUserFunction
    {
        int RightId = 0;
        long ApprovalId = 0;
        int numOrder = 0;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                int rightid = System.Convert.ToInt32(parameters[0]);
                long approvalid = System.Convert.ToInt64(parameters[1]);

                if (rightid > 0 && approvalid > 0)
                {
                    if (RightId == rightid && ApprovalId == approvalid)
                    {
                        numOrder = numOrder + 1;
                    }
                    else
                    {
                        RightId = rightid;
                        ApprovalId = approvalid;
                        numOrder = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return numOrder;
        }
    }
}
