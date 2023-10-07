using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00106
{
    public class Mrs00106Processor : AbstractProcessor
    {
        Mrs00106Filter castFilter = null;
        List<Mrs00106RDO> ListRdo_A1 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_A2 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_A3 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_A4 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_A5 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_A6 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_AOther = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_B1 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_B2 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_B3 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_B4 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_B5 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_B6 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_BOther = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_C1 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_C2 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_C3 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_C4 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_C5 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_C6 = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdo_COther = new List<Mrs00106RDO>();

        List<Mrs00106RDO> ListRdoA = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdoB = new List<Mrs00106RDO>();
        List<Mrs00106RDO> ListRdoC = new List<Mrs00106RDO>();

        decimal TotalAmount = 0;

        HIS_BRANCH _Branch = null;
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval;

        public Mrs00106Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00106Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = false;
            try
            {
                this.castFilter = (Mrs00106Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu V_HIS_HEIN_APPROVAL, MRS00106 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00106." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
                    throw new DataMisalignedException();
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
            bool result = false;
            try
            {
                ProcessListHeinApproval(ListHeinApproval);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApproval(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
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

                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("co exception xay ra tai DAOGET trong qua tinh tong hop du lieu MRS00106.");
                        }
                        ProcessDetailHeinApproval(hisHeinApprovals, ListSereServ);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ProcessGroupListRdoA();
                    ProcessGroupListRdoB();
                    ProcessGroupListRdoC();
                    ProcessListRdo();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo_A1.Clear();
                ListRdo_A2.Clear();
                ListRdo_A3.Clear();
                ListRdo_A4.Clear();
                ListRdo_A5.Clear();
                ListRdo_A6.Clear();
                ListRdo_AOther.Clear();
                ListRdo_B1.Clear();
                ListRdo_B2.Clear();
                ListRdo_B3.Clear();
                ListRdo_B4.Clear();
                ListRdo_B5.Clear();
                ListRdo_B6.Clear();
                ListRdo_BOther.Clear();
                ListRdo_C1.Clear();
                ListRdo_C2.Clear();
                ListRdo_C3.Clear();
                ListRdo_C4.Clear();
                ListRdo_C5.Clear();
                ListRdo_C6.Clear();
                ListRdo_COther.Clear();
            }
        }

        private void ProcessDetailHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var item in ListSereServ)
                        {
                            if (item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.AMOUNT <= 0 || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                continue;
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                        }
                    }
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                        {
                            Mrs00106RDO rdo = new Mrs00106RDO(heinApproval);
                            if (dicSereServ.ContainsKey(heinApproval.ID))
                            {
                                foreach (var sereServ in dicSereServ[heinApproval.ID])
                                {
                                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                                    {
                                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__TEST)
                                        {
                                            rdo.TEST_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA
                                            //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__DIIM 
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__FUEX)
                                        {
                                            rdo.DIIM_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM
                                            //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM 
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM
                                            //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM 
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                                        {
                                            rdo.MEDICINE_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                                        {
                                            rdo.BLOOD_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                                        //Config.IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                                        {
                                            rdo.SURG_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM
                                            //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_IN 
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM
                                            //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_OUT 
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_REPLACE)
                                        {
                                            rdo.MATERIAL_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL)
                                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_RATIO)
                                        {
                                            rdo.MATERIAL_RATIO_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                                        {
                                            rdo.MEDICINE_RATIO_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID ==
IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                                        {
                                            rdo.EXAM_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                                            || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                                        {
                                            rdo.BED_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__HIGHTECH)
                                        {
                                            rdo.SERVICE_RATIO_PRICE += TotalPriceTreatment;
                                        }
                                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                                        {
                                            rdo.TRAN_PRICE += TotalPriceTreatment;
                                        }
                                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                                    }
                                }
                            }

                            if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(rdo.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
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

        private void ProcessGroupListRdoA()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdoA))
                {
                    ListRdo_A1.AddRange(ListRdoA.Where(o => o.GROUP == 1).ToList());
                    ListRdo_A2.AddRange(ListRdoA.Where(o => o.GROUP == 2).ToList());
                    ListRdo_A3.AddRange(ListRdoA.Where(o => o.GROUP == 3).ToList());
                    ListRdo_A4.AddRange(ListRdoA.Where(o => o.GROUP == 4).ToList());
                    ListRdo_A5.AddRange(ListRdoA.Where(o => o.GROUP == 5).ToList());
                    ListRdo_A6.AddRange(ListRdoA.Where(o => o.GROUP == 6).ToList());
                    ListRdo_AOther.AddRange(ListRdoA.Where(o => o.GROUP == 7).ToList());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupListRdoB()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdoB))
                {
                    ListRdo_B1.AddRange(ListRdoB.Where(o => o.GROUP == 1).ToList());
                    ListRdo_B2.AddRange(ListRdoB.Where(o => o.GROUP == 2).ToList());
                    ListRdo_B3.AddRange(ListRdoB.Where(o => o.GROUP == 3).ToList());
                    ListRdo_B4.AddRange(ListRdoB.Where(o => o.GROUP == 4).ToList());
                    ListRdo_B5.AddRange(ListRdoB.Where(o => o.GROUP == 5).ToList());
                    ListRdo_B6.AddRange(ListRdoB.Where(o => o.GROUP == 6).ToList());
                    ListRdo_BOther.AddRange(ListRdoB.Where(o => o.GROUP == 7).ToList());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupListRdoC()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdoC))
                {
                    ListRdo_C1.AddRange(ListRdoC.Where(o => o.GROUP == 1).ToList());
                    ListRdo_C2.AddRange(ListRdoC.Where(o => o.GROUP == 2).ToList());
                    ListRdo_C3.AddRange(ListRdoC.Where(o => o.GROUP == 3).ToList());
                    ListRdo_C4.AddRange(ListRdoC.Where(o => o.GROUP == 4).ToList());
                    ListRdo_C5.AddRange(ListRdoC.Where(o => o.GROUP == 5).ToList());
                    ListRdo_C6.AddRange(ListRdoC.Where(o => o.GROUP == 6).ToList());
                    ListRdo_COther.AddRange(ListRdoC.Where(o => o.GROUP == 7).ToList());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListRdo()
        {
            try
            {
                ListRdo_A1 = ListRdo_A1.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_A2 = ListRdo_A2.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_A3 = ListRdo_A3.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_A4 = ListRdo_A4.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                //ListRdo_A5 = ListRdo_A5.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS) }).ToList(); 

                ListRdo_A5 = ListRdo_A5.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_A6 = ListRdo_A6.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_AOther = ListRdo_AOther.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_B1 = ListRdo_B1.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_B2 = ListRdo_B2.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_B3 = ListRdo_B3.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_B4 = ListRdo_B4.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_B5 = ListRdo_B5.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_B6 = ListRdo_B6.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_BOther = ListRdo_BOther.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_C1 = ListRdo_C1.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_C2 = ListRdo_C2.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_C3 = ListRdo_C3.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_C4 = ListRdo_C4.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_C5 = ListRdo_C5.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_C6 = ListRdo_C6.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();

                ListRdo_COther = ListRdo_COther.GroupBy(g => g.TYPE_CODE).Select(s => new Mrs00106RDO { TYPE_CODE = s.First().TYPE_CODE, TOTAL_AMOUNT = s.Count(), TOTAL_PRICE = s.Sum(s1 => s1.TOTAL_PRICE), TEST_PRICE = s.Sum(s2 => s2.TEST_PRICE), DIIM_PRICE = s.Sum(s3 => s3.DIIM_PRICE), MEDICINE_PRICE = s.Sum(s4 => s4.MEDICINE_PRICE), BLOOD_PRICE = s.Sum(s5 => s5.BLOOD_PRICE), SURG_PRICE = s.Sum(s6 => s6.SURG_PRICE), MATERIAL_PRICE = s.Sum(s7 => s7.MATERIAL_PRICE), MATERIAL_RATIO_PRICE = s.Sum(s8 => s8.MATERIAL_RATIO_PRICE), SERVICE_RATIO_PRICE = s.Sum(s9 => s9.SERVICE_RATIO_PRICE), MEDICINE_RATIO_PRICE = s.Sum(s10 => s10.MEDICINE_RATIO_PRICE), EXAM_PRICE = s.Sum(s11 => s11.EXAM_PRICE), TRAN_PRICE = s.Sum(s12 => s12.TRAN_PRICE), TOTAL_PATIENT_PRICE = s.Sum(s13 => s13.TOTAL_PATIENT_PRICE), TOTAL_HEIN_PRICE = s.Sum(s14 => s14.TOTAL_HEIN_PRICE), TOTAL_HEIN_PRICE_NDS = s.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS), TOTAL_OTHER_SOURCE_PRICE = s.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE) }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo_A1.Clear();
                ListRdo_A2.Clear();
                ListRdo_A3.Clear();
                ListRdo_A4.Clear();
                ListRdo_A5.Clear();
                ListRdo_A6.Clear();
                ListRdo_AOther.Clear();
                ListRdo_B1.Clear();
                ListRdo_B2.Clear();
                ListRdo_B3.Clear();
                ListRdo_B4.Clear();
                ListRdo_B5.Clear();
                ListRdo_B6.Clear();
                ListRdo_BOther.Clear();
                ListRdo_C1.Clear();
                ListRdo_C2.Clear();
                ListRdo_C3.Clear();
                ListRdo_C4.Clear();
                ListRdo_C5.Clear();
                ListRdo_C6.Clear();
                ListRdo_COther.Clear();
            }
        }

        private void ProcessTotalAmount()
        {
            try
            {
                TotalAmount = 0;
                if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_1))
                {
                    TotalAmount += ListRdo_A1.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B1.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C1.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_2))
                {
                    TotalAmount += ListRdo_A2.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B2.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C2.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_3))
                {
                    TotalAmount += ListRdo_A3.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B3.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C3.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_4))
                {
                    TotalAmount += ListRdo_A4.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B4.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C4.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_5))
                {
                    TotalAmount += ListRdo_A5.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B5.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C5.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_6))
                {
                    TotalAmount += ListRdo_A6.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B6.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C6.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_Other == 1)
                {
                    TotalAmount += ListRdo_AOther.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_BOther.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_COther.Sum(s => s.TOTAL_HEIN_PRICE);
                }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                ProcessTotalAmount();
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                //Nhóm A: Đúng tuyến không giới thiệu
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_1))
                objectTag.AddObjectData(store, "Report_A1", ListRdo_A1);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_2))
                objectTag.AddObjectData(store, "Report_A2", ListRdo_A2);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_3))
                objectTag.AddObjectData(store, "Report_A3", ListRdo_A3);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_4))
                objectTag.AddObjectData(store, "Report_A4", ListRdo_A4);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_5))
                objectTag.AddObjectData(store, "Report_A5", ListRdo_A5);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_6))
                objectTag.AddObjectData(store, "Report_A6", ListRdo_A6);
                //if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_Other == 1)
                objectTag.AddObjectData(store, "Report_AOther", ListRdo_AOther);

                ////Nhóm B: Nội tính đến
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_1))
                objectTag.AddObjectData(store, "Report_B1", ListRdo_B1);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_2))
                objectTag.AddObjectData(store, "Report_B2", ListRdo_B2);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_3))
                objectTag.AddObjectData(store, "Report_B3", ListRdo_B3);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_4))
                objectTag.AddObjectData(store, "Report_B4", ListRdo_B4);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_5))
                objectTag.AddObjectData(store, "Report_B5", ListRdo_B5);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_6))
                objectTag.AddObjectData(store, "Report_B6", ListRdo_B6);
                //if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_Other == 1)
                objectTag.AddObjectData(store, "Report_BOther", ListRdo_BOther);

                ////Nhóm C: ngoại tỉnh đến
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_1))
                objectTag.AddObjectData(store, "Report_C1", ListRdo_C1);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_2))
                objectTag.AddObjectData(store, "Report_C2", ListRdo_C2);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_3))
                objectTag.AddObjectData(store, "Report_C3", ListRdo_C3);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_4))
                objectTag.AddObjectData(store, "Report_C4", ListRdo_C4);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_5))
                objectTag.AddObjectData(store, "Report_C5", ListRdo_C5);
                //if (IsNotNullOrEmpty(MRS.MANAGER.Config.HeinCardNumberGroups.Group_6))
                objectTag.AddObjectData(store, "Report_C6", ListRdo_C6);
                //if (MRS.MANAGER.Config.HeinCardNumberGroups.Group_Other == 1)
                objectTag.AddObjectData(store, "Report_COther", ListRdo_COther);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
