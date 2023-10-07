using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisMedicineType;
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
using MRS.SDO;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00199
{
    class Mrs00199Processor : AbstractProcessor
    {
        Mrs00199Filter castFilter = null;

        List<VSarReportMrs00199RDO> ListRdo_A_Generic = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_A_Cpyhct = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_A_Vtyhct = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_A_Other = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_B_Generic = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_B_Cpyhct = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_B_Vtyhct = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_B_Other = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_C_Generic = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_C_Cpyhct = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_C_Vtyhct = new List<VSarReportMrs00199RDO>();
        List<VSarReportMrs00199RDO> ListRdo_C_Other = new List<VSarReportMrs00199RDO>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

        const string ROUTE_A = "A";
        const string ROUTE_B = "B";
        const string ROUTE_C = "C";

        List<long> listHeinServiceTypeId;

        HIS_BRANCH _Branch = null;

        string thisReportTypeCode = "";
        public Mrs00199Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00199Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.castFilter = (Mrs00199Filter)this.reportFilter;
                this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                CommonParam paramGet = new CommonParam();

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                HisMedicineTypeViewFilterQuery mediTypeFilter = new HisMedicineTypeViewFilterQuery();
                ListMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(mediTypeFilter);

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
        { return true; }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }

            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }


            //Route A
            objectTag.AddObjectData(store, "ListRdo_A_Generic", ListRdo_A_Generic);
            objectTag.AddObjectData(store, "ListRdo_A_Cpyhct", ListRdo_A_Cpyhct);
            objectTag.AddObjectData(store, "ListRdo_A_Vtyhct", ListRdo_A_Vtyhct);
            objectTag.AddObjectData(store, "ListRdo_A_Other", ListRdo_A_Other);

            //Route B
            objectTag.AddObjectData(store, "ListRdo_B_Generic", ListRdo_B_Generic);
            objectTag.AddObjectData(store, "ListRdo_B_Cpyhct", ListRdo_B_Cpyhct);
            objectTag.AddObjectData(store, "ListRdo_B_Vtyhct", ListRdo_B_Vtyhct);
            objectTag.AddObjectData(store, "ListRdo_B_Other", ListRdo_B_Other);

            //Route C
            objectTag.AddObjectData(store, "ListRdo_C_Generic", ListRdo_C_Generic);
            objectTag.AddObjectData(store, "ListRdo_C_Cpyhct", ListRdo_C_Cpyhct);
            objectTag.AddObjectData(store, "ListRdo_C_Vtyhct", ListRdo_C_Vtyhct);
            objectTag.AddObjectData(store, "ListRdo_C_Other", ListRdo_C_Other);

        }

        private void ProcessListHeinApproval(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    List<V_HIS_HEIN_APPROVAL> hisHeinAppBhyts_A = new List<V_HIS_HEIN_APPROVAL>();
                    List<V_HIS_HEIN_APPROVAL> hisHeinAppBhyts_B = new List<V_HIS_HEIN_APPROVAL>();
                    List<V_HIS_HEIN_APPROVAL> hisHeinAppBhyts_C = new List<V_HIS_HEIN_APPROVAL>();
                    if (IsNotNullOrEmpty(ListMedicineType))
                    {
                        foreach (var medicine in ListMedicineType)
                        {
                            dicMedicineType[medicine.ID] = medicine;
                        }
                    }
                    listHeinServiceTypeId = new List<long>()
                    {
                       IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL
                    };

                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }
                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }
                    CommonParam paramGet = new CommonParam();
                    foreach (var heinApproval in ListHeinApproval)
                    {

                        if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                        {
                            hisHeinAppBhyts_A.Add(heinApproval);
                        }
                        else if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                        {
                            hisHeinAppBhyts_B.Add(heinApproval);
                        }
                        else
                        {
                            hisHeinAppBhyts_C.Add(heinApproval);
                        }
                    }

                    if (IsNotNullOrEmpty(hisHeinAppBhyts_A))
                    {
                        ProcessListHeinApprovalRoute(paramGet, hisHeinAppBhyts_A, ROUTE_A);
                    }
                    if (IsNotNullOrEmpty(hisHeinAppBhyts_B))
                    {
                        ProcessListHeinApprovalRoute(paramGet, hisHeinAppBhyts_B, ROUTE_B);
                    }
                    if (IsNotNullOrEmpty(hisHeinAppBhyts_C))
                    {
                        ProcessListHeinApprovalRoute(paramGet, hisHeinAppBhyts_C, ROUTE_C);
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00199");
                    }

                    ListRdo_A_Generic = ProcessListRDO(ListRdo_A_Generic);
                    ListRdo_A_Cpyhct = ProcessListRDO(ListRdo_A_Cpyhct);
                    ListRdo_A_Vtyhct = ProcessListRDO(ListRdo_A_Vtyhct);
                    ListRdo_A_Other = ProcessListRDO(ListRdo_A_Other);
                    ListRdo_B_Generic = ProcessListRDO(ListRdo_B_Generic);
                    ListRdo_B_Cpyhct = ProcessListRDO(ListRdo_B_Cpyhct);
                    ListRdo_B_Vtyhct = ProcessListRDO(ListRdo_B_Vtyhct);
                    ListRdo_B_Other = ProcessListRDO(ListRdo_B_Other);
                    ListRdo_C_Generic = ProcessListRDO(ListRdo_C_Generic);
                    ListRdo_C_Cpyhct = ProcessListRDO(ListRdo_C_Cpyhct);
                    ListRdo_C_Vtyhct = ProcessListRDO(ListRdo_C_Vtyhct);
                    ListRdo_C_Other = ProcessListRDO(ListRdo_C_Other);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo_A_Generic.Clear();
                ListRdo_A_Cpyhct.Clear();
                ListRdo_A_Vtyhct.Clear();
                ListRdo_A_Other.Clear();
                ListRdo_B_Generic.Clear();
                ListRdo_B_Cpyhct.Clear();
                ListRdo_B_Vtyhct.Clear();
                ListRdo_B_Other.Clear();
                ListRdo_C_Generic.Clear();
                ListRdo_C_Cpyhct.Clear();
                ListRdo_C_Vtyhct.Clear();
                ListRdo_C_Other.Clear();
            }
        }

        private void ProcessListHeinApprovalRoute(CommonParam paramGet, List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, string code)
        {
            try
            {
                if (!IsNotNullOrEmpty(hisHeinApprovals))
                    return;

                int start = 0;
                int count = hisHeinApprovals.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var heinApprovals = hisHeinApprovals.Skip(start).Take(limit).ToList();

                    HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                    ssFilter.HEIN_APPROVAL_IDs = heinApprovals.Select(s => s.ID).ToList();
                    var ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                    if (ListSereServ != null)
                    {
                        ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00199");
                    }
                    ProcessListSereServHein(heinApprovals, ListSereServ, code);

                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListSereServHein(List<V_HIS_HEIN_APPROVAL> heinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, string code)
        {
            try
            {
                Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

                if (IsNotNullOrEmpty(heinApprovals))
                {
                    foreach (var item in heinApprovals)
                    {
                        dicHeinApproval[item.ID] = item;
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var item in ListSereServ)
                    {
                        if (item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                            || item.AMOUNT <= 0 || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.HEIN_APPROVAL_ID == null)
                            continue;
                        if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                        {
                            var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                            VSarReportMrs00199RDO rdo = new VSarReportMrs00199RDO();
                            rdo.SERVICE_ID = item.SERVICE_ID;
                            rdo.MEDICINE_SODANGKY_NAME = item.TDL_HEIN_SERVICE_BHYT_CODE;
                            rdo.MEDICINE_STT_DMBYT = item.TDL_HEIN_ORDER;
                            rdo.MEDICINE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                            rdo.MEDICINE_HAMLUONG_NAME = item.MEDICINE_TYPE_CONCENTRA;
                            rdo.TOTAL_HEIN_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                            rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                            if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                            }
                            else if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }
                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                            rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                            if (item.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                if (code == ROUTE_A)
                                {
                                    ListRdo_A_Other.Add(rdo);
                                }
                                else if (code == ROUTE_B)
                                {
                                    ListRdo_B_Other.Add(rdo);
                                }
                                else if (code == ROUTE_C)
                                {
                                    ListRdo_C_Other.Add(rdo);
                                }
                            }
                            else
                            {
                                V_HIS_MEDICINE_TYPE medicineType = null;
                                if (item.MEDICINE_TYPE_ID.HasValue && dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID.Value))
                                {
                                    medicineType = dicMedicineType[item.MEDICINE_TYPE_ID.Value];
                                }
                                if (IsNotNull(medicineType))
                                {
                                    rdo.MEDICINE_HOATCHAT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                    rdo.MEDICINE_CODE_DMBYT = medicineType.ACTIVE_INGR_BHYT_CODE;
                                    rdo.MEDICINE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                                    rdo.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                                    if (code == ROUTE_A)
                                    {
                                        if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD)
                                        {
                                            ListRdo_A_Generic.Add(rdo);
                                        }
                                        else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                        {
                                            ListRdo_A_Cpyhct.Add(rdo);
                                        }
                                        else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                        {
                                            ListRdo_A_Vtyhct.Add(rdo);
                                        }
                                    }
                                    else if (code == ROUTE_B)
                                    {
                                        if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD)
                                        {
                                            ListRdo_B_Generic.Add(rdo);
                                        }
                                        else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                        {
                                            ListRdo_B_Cpyhct.Add(rdo);
                                        }
                                        else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                        {
                                            ListRdo_B_Vtyhct.Add(rdo);
                                        }
                                    }
                                    else if (code == ROUTE_C)
                                    {
                                        if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD)
                                        {
                                            ListRdo_C_Generic.Add(rdo);
                                        }
                                        else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                        {
                                            ListRdo_C_Cpyhct.Add(rdo);
                                        }
                                        else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                        {
                                            ListRdo_C_Vtyhct.Add(rdo);
                                        }
                                    }
                                }
                                else
                                {
                                    if (code == ROUTE_A)
                                    {
                                        ListRdo_A_Other.Add(rdo);
                                    }
                                    else if (code == ROUTE_B)
                                    {
                                        ListRdo_B_Other.Add(rdo);
                                    }
                                    else if (code == ROUTE_C)
                                    {
                                        ListRdo_C_Other.Add(rdo);
                                    }
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

        private List<VSarReportMrs00199RDO> ProcessListRDO(List<VSarReportMrs00199RDO> listRDO)
        {
            List<VSarReportMrs00199RDO> listCurrent = new List<VSarReportMrs00199RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRDO))
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.TOTAL_HEIN_PRICE, o.MEDICINE_SODANGKY_NAME, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<VSarReportMrs00199RDO> listsub = group.ToList<VSarReportMrs00199RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            VSarReportMrs00199RDO rdo = new VSarReportMrs00199RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.MEDICINE_SODANGKY_NAME = listsub[0].MEDICINE_SODANGKY_NAME;
                            rdo.MEDICINE_HAMLUONG_NAME = listsub[0].MEDICINE_HAMLUONG_NAME;
                            rdo.MEDICINE_DUONGDUNG_NAME = listsub[0].MEDICINE_DUONGDUNG_NAME;
                            rdo.MEDICINE_HOATCHAT_NAME = listsub[0].MEDICINE_HOATCHAT_NAME;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MEDICINE_UNIT_NAME = listsub[0].MEDICINE_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
                            if (rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0)
                            {
                                listCurrent.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.MEDICINE_STT_DMBYT).ToList();
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
