using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBidMedicineType;
using ACS.Filter;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisSereServ;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00280
{
    class Mrs00280Processor : AbstractProcessor
    {
        List<Mrs00280RDO> ListRdoGeneric = new List<Mrs00280RDO>();
        List<Mrs00280RDO> ListRdoCpyhct = new List<Mrs00280RDO>();
        List<Mrs00280RDO> ListRdoVtyhct = new List<Mrs00280RDO>();
        List<Mrs00280RDO> ListRdoOther = new List<Mrs00280RDO>();
        Dictionary<string, HIS_BID_MEDICINE_TYPE> dicBidMedicineType = new Dictionary<string, HIS_BID_MEDICINE_TYPE>();
        Dictionary<long, List<HIS_BID>> dicBid = new Dictionary<long, List<HIS_BID>>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

        List<long> listHeinServiceTypeId;

        HIS_BRANCH _Branch = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00280Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00280Filter);
        }


        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            var filter = ((Mrs00280Filter)reportFilter);
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == filter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = filter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = filter.TIME_TO;
                approvalFilter.BRANCH_ID = filter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);
                ListMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());

                dicBidMedicineType = new HisBidMedicineTypeManager().Get(new HisBidMedicineTypeFilterQuery()).GroupBy(o => (o.MEDICINE_TYPE_ID + "_" + o.BID_ID)).ToDictionary(p => p.Key, p => p.First());

                dicBid = new HisBidManager(paramGet).Get(new HisBidFilterQuery()).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.ToList());
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
            var result = true;
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    if (IsNotNullOrEmpty(ListMedicineType))
                    {
                        foreach (var medicine in ListMedicineType)
                        {
                            dicMedicineType[medicine.ID] = medicine;
                        }
                    }
                    CommonParam paramGet = new CommonParam();
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

                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts = ListHeinApproval.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = heinApprovalBhyts.Select(s => s.ID).ToList();
                        var ListSereServHein = new HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (ListSereServHein != null)
                        {
                            ListSereServHein = ListSereServHein.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00018");
                        }

                        ProcessListHeinApprovalDetail(heinApprovalBhyts, ListSereServHein);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ListRdoGeneric = ProcessListRDO(ListRdoGeneric);
                    ListRdoCpyhct = ProcessListRDO(ListRdoCpyhct);
                    ListRdoVtyhct = ProcessListRDO(ListRdoVtyhct);
                    ListRdoOther = ProcessListRDO(ListRdoOther);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            if (((Mrs00280Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00280Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00280Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00280Filter)reportFilter).TIME_TO));
            }
            objectTag.AddObjectData(store, "Generic", ListRdoGeneric);
            objectTag.AddObjectData(store, "Cpyhct", ListRdoCpyhct);
            objectTag.AddObjectData(store, "Vtyhct", ListRdoVtyhct);
            objectTag.AddObjectData(store, "Othe", ListRdoOther);
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServHein)
        {
            try
            {
                Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

                if (IsNotNullOrEmpty(heinApprovalBhyts))
                {
                    foreach (var item in heinApprovalBhyts)
                    {
                        dicHeinApproval[item.ID] = item;
                    }
                }

                if (IsNotNullOrEmpty(ListSereServHein))
                {
                    foreach (var item in ListSereServHein)
                    {
                        if (item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
|| item.AMOUNT <= 0 || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.HEIN_APPROVAL_ID == null)
                            continue;
                        if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                        {
                            var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                            Mrs00280RDO rdo = new Mrs00280RDO();
                            rdo.SERVICE_ID = item.SERVICE_ID;
                            rdo.MEDICINE_SODANGKY_NAME = item.TDL_HEIN_SERVICE_BHYT_CODE;
                            rdo.MEDICINE_STT_DMBYT = item.TDL_HEIN_ORDER;
                            rdo.MEDICINE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                            rdo.MEDICINE_HAMLUONG_NAME = item.MEDICINE_TYPE_CONCENTRA;
                            rdo.TOTAL_HEIN_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                            rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                            rdo.BID_NUM_ORDER = dicBidMedicineType.ContainsKey((item.MEDICINE_TYPE_ID ?? 0) + "_" + (item.MEDICINE_BID_ID ?? 0)) ? dicBidMedicineType[(item.MEDICINE_TYPE_ID ?? 0) + "_" + (item.MEDICINE_BID_ID ?? 0)].BID_NUM_ORDER : item.MEDICINE_BID_NUM_ORDER ?? item.TDL_MEDICINE_BID_NUM_ORDER;
                            rdo.BID_NUMBER = dicBid.ContainsKey(item.MEDICINE_BID_ID ?? 0) ? dicBid[item.MEDICINE_BID_ID ?? 0].First().BID_NUMBER : "";
                            if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                            }
                            else if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }
                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                            rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;

                            if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                ListRdoOther.Add(rdo);
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

                                    if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs000280.HIS_MEDICINE_LINE.ID__TTD)
                                    {
                                        ListRdoGeneric.Add(rdo);
                                    }
                                    else if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs000280.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                    {
                                        ListRdoCpyhct.Add(rdo);
                                    }
                                    else if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs000280.HIS_MEDICINE_LINE.ID__VT_YHCT)
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
        private List<Mrs00280RDO> ProcessListRDO(List<Mrs00280RDO> listRDO)
        {
            List<Mrs00280RDO> listCurrent = new List<Mrs00280RDO>();
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.IMP_PRICE, o.TOTAL_HEIN_PRICE, o.MEDICINE_SODANGKY_NAME, o.BID_NUMBER, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00280RDO> listsub = group.ToList<Mrs00280RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00280RDO rdo = new Mrs00280RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.BID_NUMBER = listsub[0].BID_NUMBER;
                            rdo.BID_NUM_ORDER = listsub[0].BID_NUM_ORDER;
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

    }
}
