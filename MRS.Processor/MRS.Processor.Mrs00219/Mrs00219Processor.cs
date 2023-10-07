using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisMedicineTypeAcin;
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
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00219
{
    public class Mrs00219Processor : AbstractProcessor
    {
        Mrs00219Filter castFilter = null;

        List<Mrs00219RDO> ListRdoGeneric = new List<Mrs00219RDO>();
        List<Mrs00219RDO> ListRdoCpyhct = new List<Mrs00219RDO>();
        List<Mrs00219RDO> ListRdoVtyhct = new List<Mrs00219RDO>();
        List<Mrs00219RDO> ListRdoOther = new List<Mrs00219RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

        List<long> listHeinServiceTypeId;

        HIS_BRANCH _Branch = null;

        public Mrs00219Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00219Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00219Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, V_HIS_MEDICINE_TYPE, MRS00219, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                ListMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00219");
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
                if (IsNotNullOrEmpty(ListMedicineType))
                {
                    foreach (var medicine in ListMedicineType)
                    {
                        dicMedicineType[medicine.ID] = medicine;
                    }
                }
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    listHeinServiceTypeId = new List<long>();
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL);

                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }

                    CommonParam paramGet = new CommonParam();
                    ListHeinApproval = ListHeinApproval.Where(o => CheckHeinCardNumberType(o.HEIN_CARD_NUMBER)).ToList();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        var hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        List<V_HIS_MEDICINE_TYPE_ACIN> ListMedicineTypeAcin = new List<V_HIS_MEDICINE_TYPE_ACIN>();

                        if (IsNotNullOrEmpty(ListSereServ))
                        {
                            HisMedicineTypeAcinViewFilterQuery mediAcinFilter = new HisMedicineTypeAcinViewFilterQuery();
                            mediAcinFilter.MEDICINE_TYPE_IDs = ListSereServ.Select(s => s.MEDICINE_TYPE_ID ?? 0).Distinct().ToList();
                            ListMedicineTypeAcin = new MOS.MANAGER.HisMedicineTypeAcin.HisMedicineTypeAcinManager(paramGet).GetView(mediAcinFilter);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00219.");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListMedicineTypeAcin);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (IsNotNullOrEmpty(ListRdoCpyhct))
                    {
                        ListRdoCpyhct = ProcessListRDO(ListRdoCpyhct);
                    }

                    if (IsNotNullOrEmpty(ListRdoGeneric))
                    {
                        ListRdoGeneric = ProcessListRDO(ListRdoGeneric);
                    }

                    if (IsNotNullOrEmpty(ListRdoOther))
                    {
                        ListRdoOther = ProcessListRDO(ListRdoOther);
                    }

                    if (IsNotNullOrEmpty(ListRdoVtyhct))
                    {
                        ListRdoVtyhct = ProcessListRDO(ListRdoVtyhct);
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

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_MEDICINE_TYPE_ACIN> ListMedicineTypeAcin)
        {
            try
            {
                Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApprovalBhyt = new Dictionary<long, V_HIS_HEIN_APPROVAL>();
                Dictionary<long, List<V_HIS_MEDICINE_TYPE_ACIN>> dicMedicineTypeAcin = new Dictionary<long, List<V_HIS_MEDICINE_TYPE_ACIN>>();

                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        dicHeinApprovalBhyt[heinApproval.ID] = heinApproval;
                    }
                }

                if (IsNotNullOrEmpty(ListMedicineTypeAcin))
                {
                    foreach (var medicineAcin in ListMedicineTypeAcin)
                    {
                        if (!dicMedicineTypeAcin.ContainsKey(medicineAcin.MEDICINE_TYPE_ID))
                            dicMedicineTypeAcin[medicineAcin.MEDICINE_TYPE_ID] = new List<V_HIS_MEDICINE_TYPE_ACIN>();
                        dicMedicineTypeAcin[medicineAcin.MEDICINE_TYPE_ID].Add(medicineAcin);
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var sereServ in ListSereServ)
                    {
                        if (sereServ.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || sereServ.AMOUNT <= 0 || sereServ.PRICE == 0 || sereServ.HEIN_APPROVAL_ID == null || sereServ.TDL_HEIN_SERVICE_TYPE_ID == null || sereServ.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;
                        if (!listHeinServiceTypeId.Contains(sereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                            continue;
                        Mrs00219RDO rdo = new Mrs00219RDO();
                        rdo.SERVICE_ID = sereServ.SERVICE_ID;
                        rdo.MEDICINE_SODANGKY_NAME = sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.REGISTER_NUMBER = sereServ.REGISTER_NUMBER;
                        rdo.MEDICINE_STT_DMBYT = sereServ.TDL_HEIN_ORDER;
                        rdo.MEDICINE_TYPE_NAME = sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MEDICINE_HAMLUONG_NAME = sereServ.MEDICINE_TYPE_CONCENTRA;
                        rdo.MEDICINE_UNIT_NAME = sereServ.SERVICE_UNIT_NAME;
                        rdo.VIR_PRICE = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(sereServ.ORIGINAL_PRICE > 0 ? (sereServ.HEIN_LIMIT_PRICE.HasValue ? (sereServ.HEIN_LIMIT_PRICE.Value / (sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO))) * 100 : (sereServ.PRICE / sereServ.ORIGINAL_PRICE) * 100) : 0, 0);
                        if (sereServ.HEIN_APPROVAL_ID.HasValue && dicHeinApprovalBhyt.ContainsKey(sereServ.HEIN_APPROVAL_ID.Value))
                        {
                            if (dicHeinApprovalBhyt[sereServ.HEIN_APPROVAL_ID.Value].HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sereServ.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_NOITRU = sereServ.AMOUNT;
                            }
                            rdo.VIR_TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == hisHeinApprovals.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());

                            rdo.TOTAL_OTHER_SOURCE_PRICE = (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                ListRdoOther.Add(rdo);
                            }
                            else
                            {
                                V_HIS_MEDICINE_TYPE medicineType = null;
                                if (sereServ.MEDICINE_TYPE_ID.HasValue && dicMedicineType.ContainsKey(sereServ.MEDICINE_TYPE_ID.Value))
                                {
                                    medicineType = dicMedicineType[sereServ.MEDICINE_TYPE_ID.Value];
                                }
                                if (IsNotNull(medicineType))
                                {

                                    if (String.IsNullOrEmpty(rdo.MEDICINE_HOATCHAT_NAME))
                                        rdo.MEDICINE_HOATCHAT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;

                                    if (String.IsNullOrEmpty(rdo.MEDICINE_CODE_DMBYT))
                                        rdo.MEDICINE_CODE_DMBYT = medicineType.ACTIVE_INGR_BHYT_CODE;

                                    rdo.MEDICINE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                                    rdo.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                                    if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD)
                                    {

                                        ListRdoGeneric.Add(rdo);
                                    }
                                    else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                    {
                                        ListRdoCpyhct.Add(rdo);
                                    }
                                    else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                    {
                                        ListRdoVtyhct.Add(rdo);
                                    }
                                }
                                else
                                {
                                    ListRdoOther.Add(rdo);
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

        private List<Mrs00219RDO> ProcessListRDO(List<Mrs00219RDO> listRDO)
        {
            List<Mrs00219RDO> listCurrent = new List<Mrs00219RDO>();
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.VIR_PRICE, o.MEDICINE_SODANGKY_NAME, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00219RDO> listsub = group.ToList<Mrs00219RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00219RDO rdo = new Mrs00219RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.MEDICINE_SODANGKY_NAME = listsub[0].MEDICINE_SODANGKY_NAME;
                            rdo.REGISTER_NUMBER = listsub[0].REGISTER_NUMBER;
                            rdo.MEDICINE_HAMLUONG_NAME = listsub[0].MEDICINE_HAMLUONG_NAME;
                            rdo.MEDICINE_DUONGDUNG_NAME = listsub[0].MEDICINE_DUONGDUNG_NAME;
                            rdo.MEDICINE_HOATCHAT_NAME = listsub[0].MEDICINE_HOATCHAT_NAME;
                            rdo.VIR_PRICE = listsub[0].VIR_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MEDICINE_UNIT_NAME = listsub[0].MEDICINE_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
                            listCurrent.Add(rdo);
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

        private bool CheckHeinCardNumberType(string HeinCardNumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    if (IsNotNullOrEmpty(MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01))
                    {
                        foreach (var type in MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01)
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
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
                Decimal Total = 0;
                if (ListRdoGeneric.Count > 0) Total += ListRdoGeneric.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListRdoCpyhct.Count > 0) Total += ListRdoCpyhct.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListRdoVtyhct.Count > 0) Total += ListRdoVtyhct.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListRdoOther.Count > 0) Total += ListRdoOther.Sum(o => o.VIR_TOTAL_PRICE);
                if (Total > 0) dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(Total.ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: Không đồng");
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineGeneric", ListRdoGeneric);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineCpyhct", ListRdoCpyhct);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineVtyhct", ListRdoVtyhct);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineOther", ListRdoOther);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
