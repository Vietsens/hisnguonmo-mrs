using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBlood;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisSereServ;
using MRS.Processor.Mrs00145.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00145
{
    class Mrs00145Processor : AbstractProcessor
    {
        Mrs00145Filter castFilter = null;

        List<Mrs00145RDO> ListRdo_A_Generic = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_A_Cpyhct = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_A_Vtyhct = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_A_Other = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_B_Generic = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_B_Cpyhct = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_B_Vtyhct = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_B_Other = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_C_Generic = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_C_Cpyhct = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_C_Other = new List<Mrs00145RDO>();
        List<Mrs00145RDO> ListRdo_C_Vtyhct = new List<Mrs00145RDO>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

        const string ROUTE_A = "A";
        const string ROUTE_B = "B";
        const string ROUTE_C = "C";

        List<long> listHeinServiceTypeId;

        HIS_BRANCH _Branch = null;

        public Mrs00145Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = null;

        public override Type FilterType()
        {
            return typeof(Mrs00145Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00145Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00145: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                //this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID); 
                //if (this._Branch == null)
                //    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac"); 
                CommonParam paramGet = new CommonParam();

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                HisMedicineTypeViewFilterQuery mediTypeFilter = new HisMedicineTypeViewFilterQuery();
                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(mediTypeFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong quan trinh lay du lieu MRS00145");
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
                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__THUOC && HisBloodIntoMedicineGenericCFG.BloodIntoMedicine__Generic)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }
                    CommonParam paramGet = new CommonParam();
                    foreach (var heinApprovalBhyt in ListHeinApproval)
                    {
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyt.BRANCH_ID);
                        if (this._Branch != null)
                        {
                            if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApprovalBhyt.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(heinApprovalBhyt.HEIN_CARD_NUMBER))
                            {
                                hisHeinAppBhyts_A.Add(heinApprovalBhyt);
                            }
                            else if (checkBhytProvinceCode(heinApprovalBhyt.HEIN_CARD_NUMBER))
                            {
                                hisHeinAppBhyts_B.Add(heinApprovalBhyt);
                            }
                            else
                            {
                                hisHeinAppBhyts_C.Add(heinApprovalBhyt);
                            }
                        }
                    }

                    if (IsNotNullOrEmpty(hisHeinAppBhyts_A))
                    {
                        ProcessListHeinApprovalRoute(hisHeinAppBhyts_A, ROUTE_A);
                    }
                    if (IsNotNullOrEmpty(hisHeinAppBhyts_B))
                    {
                        ProcessListHeinApprovalRoute(hisHeinAppBhyts_B, ROUTE_B);
                    }
                    if (IsNotNullOrEmpty(hisHeinAppBhyts_C))
                    {
                        ProcessListHeinApprovalRoute(hisHeinAppBhyts_C, ROUTE_C);
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00145");
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
                result = false;
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
            return result;
        }

        private void ProcessListHeinApprovalRoute(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, string code)
        {
            if (!IsNotNullOrEmpty(hisHeinApprovals))
                return;
            CommonParam paramGet = new CommonParam();
            int start = 0;
            int count = hisHeinApprovals.Count;
            while (count > 0)
            {
                int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                var heinApprovalBhyts = hisHeinApprovals.Skip(start).Take(limit).ToList();

                HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                ssFilter.HEIN_APPROVAL_IDs = heinApprovalBhyts.Select(s => s.ID).ToList();
                var ListSereServ = new HisSereServManager(paramGet).GetView3(ssFilter);
                if (ListSereServ != null)
                {
                    ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                }

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00145");
                }
                ProcessListSereServHein(heinApprovalBhyts, ListSereServ, code);

                start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
            }
        }

        private void ProcessListSereServHein(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServ, string code)
        {
            Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

            if (IsNotNullOrEmpty(heinApprovalBhyts))
            {
                foreach (var item in heinApprovalBhyts)
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
                        Mrs00145RDO rdo = new Mrs00145RDO();
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
                        rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
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
                                if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD || (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU && HisBloodIntoMedicineGenericCFG.BloodIntoMedicine__Generic))
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
                                if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD || (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU && HisBloodIntoMedicineGenericCFG.BloodIntoMedicine__Generic))
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
                                if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD || (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU && HisBloodIntoMedicineGenericCFG.BloodIntoMedicine__Generic))
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

        private bool checkBhytProvinceCode(string HeinNumber)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(HeinNumber) && HeinNumber.Length == 15)
            {
                string provinceCode = HeinNumber.Substring(3, 2);
                if (this._Branch.HEIN_PROVINCE_CODE.Equals(provinceCode))
                {
                    result = true;
                }
            }
            return result;
        }

        private List<Mrs00145RDO> ProcessListRDO(List<Mrs00145RDO> listRDO)
        {
            List<Mrs00145RDO> listCurrent = new List<Mrs00145RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRDO))
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.TOTAL_HEIN_PRICE, o.MEDICINE_SODANGKY_NAME, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00145RDO> listsub = group.ToList<Mrs00145RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00145RDO rdo = new Mrs00145RDO();
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                Inventec.Common.FlexCellExport.ProcessSingleTag singleTag = new Inventec.Common.FlexCellExport.ProcessSingleTag();
                Inventec.Common.FlexCellExport.ProcessObjectTag objTag = new Inventec.Common.FlexCellExport.ProcessObjectTag();
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report1", ListRdo_A_Generic);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report2", ListRdo_A_Cpyhct);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report3", ListRdo_A_Vtyhct);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report10", ListRdo_A_Other);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report4", ListRdo_B_Generic);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report5", ListRdo_B_Cpyhct);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report6", ListRdo_B_Vtyhct);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report11", ListRdo_B_Other);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report7", ListRdo_C_Generic);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report8", ListRdo_C_Cpyhct);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report9", ListRdo_C_Vtyhct);
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report12", ListRdo_C_Other);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
