using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisBranch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MOS.LibraryHein.Bhyt.HeinTreatmentType;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00196
{
    class Mrs00196Processor : AbstractProcessor
    {

        Mrs00196Filter castFilter = null;

        List<VSarReportMrs00196RDO> ListRdo_A1 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_A2 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_A3 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_A4 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_A5 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_A6 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_AOther = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_B1 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_B2 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_B3 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_B4 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_B5 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_B6 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_BOther = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_C1 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_C2 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_C3 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_C4 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_C5 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_C6 = new List<VSarReportMrs00196RDO>();
        List<VSarReportMrs00196RDO> ListRdo_COther = new List<VSarReportMrs00196RDO>();

        decimal TotalAmount = 0;

        HIS_BRANCH _Branch = null;
        public Mrs00196Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00196Filter);
        }
        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            var result = false;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = (Mrs00196Filter)this.reportFilter;
                //this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                //if (this._Branch == null)
                //    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");

                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APRROVAL_BHYT, MRS00196, Filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery appBhytFilter = new HisHeinApprovalViewFilterQuery();
                appBhytFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                appBhytFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                appBhytFilter.BRANCH_ID = castFilter.BRANCH_ID;
                appBhytFilter.BRANCH_IDs = castFilter.BRANCH_IDS;
                appBhytFilter.ORDER_FIELD = "EXECUTE_TIME";
                appBhytFilter.ORDER_DIRECTION = "ACS";
                List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(appBhytFilter);

                ProcessListHeinApproval(ListHeinApproval);

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
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            ProcessTotalAmount();
            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            }

            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            }

            //Nhóm A: Đúng tuyến không giới thiệu
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_1)) 
            objectTag.AddObjectData(store, "Report_A1", ListRdo_A1);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_2))
            objectTag.AddObjectData(store, "Report_A2", ListRdo_A2);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_3)) 
            objectTag.AddObjectData(store, "Report_A3", ListRdo_A3);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_4)) 
            objectTag.AddObjectData(store, "Report_A4", ListRdo_A4);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_5))
            objectTag.AddObjectData(store, "Report_A5", ListRdo_A5);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_6))
            objectTag.AddObjectData(store, "Report_A6", ListRdo_A6);
            //if (HeinCardNumberGroups.Group_Other == 1)
            objectTag.AddObjectData(store, "Report_AOther", ListRdo_AOther);

            //Nhóm B: Nội tính đến
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_1))
            objectTag.AddObjectData(store, "Report_B1", ListRdo_B1);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_2))
            objectTag.AddObjectData(store, "Report_B2", ListRdo_B2);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_3))
            objectTag.AddObjectData(store, "Report_B3", ListRdo_B3);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_4))
            objectTag.AddObjectData(store, "Report_B4", ListRdo_B4);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_5))
            objectTag.AddObjectData(store, "Report_B5", ListRdo_B5);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_6))
            objectTag.AddObjectData(store, "Report_B6", ListRdo_B6);
            //if (HeinCardNumberGroups.Group_Other == 1) 
            objectTag.AddObjectData(store, "Report_BOther", ListRdo_BOther);

            //Nhóm C: ngoại tỉnh đến
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_1)) 
            objectTag.AddObjectData(store, "Report_C1", ListRdo_C1);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_2)) 
            objectTag.AddObjectData(store, "Report_C2", ListRdo_C2);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_3)) 
            objectTag.AddObjectData(store, "Report_C3", ListRdo_C3);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_4)) 
            objectTag.AddObjectData(store, "Report_C4", ListRdo_C4);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_5))
            objectTag.AddObjectData(store, "Report_C5", ListRdo_C5);
            //if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_6))
            objectTag.AddObjectData(store, "Report_C6", ListRdo_C6);
            //if (HeinCardNumberGroups.Group_Other == 1) 
            objectTag.AddObjectData(store, "Report_COther", ListRdo_COther);
        }

        private void ProcessListHeinApproval(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
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

                        ProcessDetailHeinApproval(hisHeinApprovals, ListSereServ);
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("co exception xay ra tai DAOGET trong qua tinh tong hop du lieu MRS00196.");
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRdo_A1 = ProcessListRdo(ListRdo_A1);
                    ListRdo_A2 = ProcessListRdo(ListRdo_A2);
                    ListRdo_A3 = ProcessListRdo(ListRdo_A3);
                    ListRdo_A4 = ProcessListRdo(ListRdo_A4);
                    ListRdo_A5 = ProcessListRdo(ListRdo_A5);
                    ListRdo_A6 = ProcessListRdo(ListRdo_A6);
                    ListRdo_AOther = ProcessListRdo(ListRdo_AOther);
                    ListRdo_B1 = ProcessListRdo(ListRdo_B1);
                    ListRdo_B2 = ProcessListRdo(ListRdo_B2);
                    ListRdo_B3 = ProcessListRdo(ListRdo_B3);
                    ListRdo_B4 = ProcessListRdo(ListRdo_B4);
                    ListRdo_B5 = ProcessListRdo(ListRdo_B5);
                    ListRdo_B6 = ProcessListRdo(ListRdo_B6);
                    ListRdo_BOther = ProcessListRdo(ListRdo_BOther);
                    ListRdo_C1 = ProcessListRdo(ListRdo_C1);
                    ListRdo_C2 = ProcessListRdo(ListRdo_C2);
                    ListRdo_C3 = ProcessListRdo(ListRdo_C3);
                    ListRdo_C4 = ProcessListRdo(ListRdo_C4);
                    ListRdo_C5 = ProcessListRdo(ListRdo_C5);
                    ListRdo_C6 = ProcessListRdo(ListRdo_C6);
                    ListRdo_COther = ProcessListRdo(ListRdo_COther);
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
                Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var item in ListSereServ)
                    {
                        if (item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.AMOUNT <= 0)
                            continue;
                        if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                            dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                        dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                    }
                }
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                        if (_Branch == null) continue;

                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == HeinTreatmentTypeCode.TREAT)
                        {
                            if (!dicSereServ.ContainsKey(heinApproval.ID))
                                continue;

                            VSarReportMrs00196RDO rdo = new VSarReportMrs00196RDO(heinApproval);
                            foreach (var item in dicSereServ[heinApproval.ID])
                            {
                                var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                                if (item.TDL_HEIN_SERVICE_TYPE_ID != null)
                                {
                                    if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                                    {
                                        rdo.TEST_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                                    {
                                        rdo.DIIM_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                                    {
                                        rdo.MEDICINE_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                                    {
                                        rdo.BLOOD_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                                    {
                                        rdo.SURG_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                                    {
                                        rdo.MATERIAL_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL)
                                    {
                                        rdo.MATERIAL_RATIO_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                                    {
                                        rdo.MEDICINE_RATIO_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                                    {
                                        rdo.EXAM_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                                    {
                                        rdo.BED_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                                    {
                                        rdo.SERVICE_RATIO_PRICE += TotalPriceTreatment;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                                    {
                                        rdo.TRAN_PRICE += TotalPriceTreatment;
                                    }
                                    rdo.TOTAL_PRICE += TotalPriceTreatment;
                                    rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                                    rdo.TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE ?? 0;
                                    rdo.TOTAL_OTHER_SOURCE_PRICE += (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                                }
                            }
                            if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(rdo.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
                            {
                                ProcessGroupListRdoA(rdo);
                            }
                            else if (checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
                            {
                                ProcessGroupListRdoB(rdo);
                            }
                            else
                            {
                                ProcessGroupListRdoC(rdo);
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

        private void ProcessGroupListRdoA(VSarReportMrs00196RDO rdo)
        {
            try
            {
                switch (rdo.GROUP)
                {
                    case 1:
                        ListRdo_A1.Add(rdo);
                        break;
                    case 2:
                        ListRdo_A2.Add(rdo);
                        break;
                    case 3:
                        ListRdo_A3.Add(rdo);
                        break;
                    case 4:
                        ListRdo_A4.Add(rdo);
                        break;
                    case 5:
                        ListRdo_A5.Add(rdo);
                        break;
                    case 6:
                        ListRdo_A6.Add(rdo);
                        break;
                    case 7:
                        ListRdo_AOther.Add(rdo);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupListRdoB(VSarReportMrs00196RDO rdo)
        {
            try
            {
                switch (rdo.GROUP)
                {
                    case 1:
                        ListRdo_B1.Add(rdo);
                        break;
                    case 2:
                        ListRdo_B2.Add(rdo);
                        break;
                    case 3:
                        ListRdo_B3.Add(rdo);
                        break;
                    case 4:
                        ListRdo_B4.Add(rdo);
                        break;
                    case 5:
                        ListRdo_B5.Add(rdo);
                        break;
                    case 6:
                        ListRdo_B6.Add(rdo);
                        break;
                    case 7:
                        ListRdo_BOther.Add(rdo);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupListRdoC(VSarReportMrs00196RDO rdo)
        {
            try
            {
                switch (rdo.GROUP)
                {
                    case 1:
                        ListRdo_C1.Add(rdo);
                        break;
                    case 2:
                        ListRdo_C2.Add(rdo);
                        break;
                    case 3:
                        ListRdo_C3.Add(rdo);
                        break;
                    case 4:
                        ListRdo_C4.Add(rdo);
                        break;
                    case 5:
                        ListRdo_C5.Add(rdo);
                        break;
                    case 6:
                        ListRdo_C6.Add(rdo);
                        break;
                    case 7:
                        ListRdo_COther.Add(rdo);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<VSarReportMrs00196RDO> ProcessListRdo(List<VSarReportMrs00196RDO> listRdo)
        {
            List<VSarReportMrs00196RDO> result = new List<VSarReportMrs00196RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    var Group = listRdo.GroupBy(o => o.TYPE_CODE).ToList();
                    foreach (var group in Group)
                    {
                        var listSub = group.ToList<VSarReportMrs00196RDO>();
                        VSarReportMrs00196RDO rdo = new VSarReportMrs00196RDO()
                        {
                            TYPE_CODE = listSub.First().TYPE_CODE,
                            TOTAL_AMOUNT = listSub.Count(),
                            TOTAL_PRICE = listSub.Sum(s1 => s1.TOTAL_PRICE),
                            TEST_PRICE = listSub.Sum(s2 => s2.TEST_PRICE),
                            DIIM_PRICE = listSub.Sum(s3 => s3.DIIM_PRICE),
                            MEDICINE_PRICE = listSub.Sum(s4 => s4.MEDICINE_PRICE),
                            BLOOD_PRICE = listSub.Sum(s5 => s5.BLOOD_PRICE),
                            SURG_PRICE = listSub.Sum(s6 => s6.SURG_PRICE),
                            MATERIAL_PRICE = listSub.Sum(s7 => s7.MATERIAL_PRICE),
                            MATERIAL_RATIO_PRICE = listSub.Sum(s8 => s8.MATERIAL_RATIO_PRICE),
                            SERVICE_RATIO_PRICE = listSub.Sum(s9 => s9.SERVICE_RATIO_PRICE),
                            MEDICINE_RATIO_PRICE = listSub.Sum(s10 => s10.MEDICINE_RATIO_PRICE),
                            EXAM_PRICE = listSub.Sum(s11 => s11.EXAM_PRICE),
                            TRAN_PRICE = listSub.Sum(s12 => s12.TRAN_PRICE),
                            TOTAL_PATIENT_PRICE = listSub.Sum(s13 => s13.TOTAL_PATIENT_PRICE),
                            TOTAL_HEIN_PRICE = listSub.Sum(s14 => s14.TOTAL_HEIN_PRICE),
                            TOTAL_HEIN_PRICE_NDS = listSub.Sum(s15 => s15.TOTAL_HEIN_PRICE_NDS),
                            TOTAL_OTHER_SOURCE_PRICE = listSub.Sum(s16 => s16.TOTAL_OTHER_SOURCE_PRICE)
                        };
                        result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<VSarReportMrs00196RDO>();
            }
            return result;
        }

        private void ProcessTotalAmount()
        {
            try
            {
                TotalAmount = 0;
                if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_1))
                {
                    TotalAmount += ListRdo_A1.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B1.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C1.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_2))
                {
                    TotalAmount += ListRdo_A2.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B2.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C2.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_3))
                {
                    TotalAmount += ListRdo_A3.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B3.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C3.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_4))
                {
                    TotalAmount += ListRdo_A4.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B4.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C4.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_5))
                {
                    TotalAmount += ListRdo_A5.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B5.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C5.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (IsNotNullOrEmpty(HeinCardNumberGroups.Group_6))
                {
                    TotalAmount += ListRdo_A6.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_B6.Sum(s => s.TOTAL_HEIN_PRICE);
                    TotalAmount += ListRdo_C6.Sum(s => s.TOTAL_HEIN_PRICE);
                }

                if (HeinCardNumberGroups.Group_Other == 1)
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


    }
}
