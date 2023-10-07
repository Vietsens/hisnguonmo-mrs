using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBidMaterialType;
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

namespace MRS.Processor.Mrs00279
{
    class Mrs00279Processor : AbstractProcessor
    {
        List<Mrs00279RDO> ListRdo = new List<Mrs00279RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<long> listHeinServiceTypeId;
        Dictionary<long, List<HIS_BID>> dicBid = new Dictionary<long, List<HIS_BID>>();
        Dictionary<long, List<HIS_BID_MATERIAL_TYPE>> dicBidMaterialType = new Dictionary<long, List<HIS_BID_MATERIAL_TYPE>>();

        HIS_BRANCH _Branch = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00279Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00279Filter);
        }


        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            var filter = ((Mrs00279Filter)reportFilter);
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == filter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len BranchId khong chinh xac");
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = filter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = filter.TIME_TO;
                approvalFilter.BRANCH_ID = filter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);
                dicBidMaterialType = new HisBidMaterialTypeManager().Get(new HisBidMaterialTypeFilterQuery()).GroupBy(o => o.MATERIAL_TYPE_ID??0).ToDictionary(p => p.Key, p => p.ToList());
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
                    CommonParam paramGet = new CommonParam();
                    listHeinServiceTypeId = new List<long>()
                        {
                            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                        //HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_IN,
                        //HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_OUT,
                        //HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_RATIO,
                        //HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_REPLACE
                        };

                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }

                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }

                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> heinAppBhyts = ListHeinApproval.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = heinAppBhyts.Select(s => s.ID).ToList();
                        var ListSereServHein = new HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (ListSereServHein != null)
                        {
                            ListSereServHein = ListSereServHein.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00017");
                        }

                        ProcessListHeinApprovalDetail(heinAppBhyts, ListSereServHein);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRdo = ProcessListRDO();
                }
            }

            catch (Exception ex)
            {
                result = false;
                ListRdo.Clear();
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            if (((Mrs00279Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00279Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00279Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00279Filter)reportFilter).TIME_TO));
            }
            objectTag.AddObjectData(store, "Report", ListRdo);

        }
        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServHein)
        {
            try
            {
                var filter = ((Mrs00279Filter)reportFilter);
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
                            var heinAproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                            Mrs00279RDO rdo = new Mrs00279RDO();
                            rdo.SERVICE_ID = item.SERVICE_ID;
                            rdo.MATERIAL_CODE_DMBYT = item.TDL_HEIN_SERVICE_BHYT_CODE;
                            rdo.MATERIAL_CODE_DMBYT_1 = item.TDL_MATERIAL_GROUP_BHYT;
                            rdo.MATERIAL_STT_DMBYT = item.TDL_HEIN_ORDER;
                            rdo.MATERIAL_TYPE_NAME_BYT = item.TDL_HEIN_SERVICE_BHYT_NAME;
                            rdo.MATERIAL_TYPE_NAME = item.TDL_SERVICE_NAME;
                            rdo.BID_NUM_ORDER = dicBidMaterialType.ContainsKey(item.MATERIAL_BID_ID ?? 0) ? dicBidMaterialType[item.MATERIAL_BID_ID ?? 0].First().BID_NUM_ORDER : "";
                            rdo.BID_NUMBER = dicBid.ContainsKey(item.MATERIAL_BID_ID ?? 0) ? dicBid[item.MATERIAL_BID_ID ?? 0].First().BID_NUMBER : "";
                            rdo.MATERIAL_QUYCACH_NAME = item.MATERIAL_PACKING_TYPE_NAME;
                            rdo.MATERIAL_PRICE = item.MATERIAL_IMP_PRICE ?? 0;
                            rdo.TOTAL_HEIN_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                            rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                            rdo.MATERIAL_UNIT_NAME = item.SERVICE_UNIT_NAME;
                            rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                            bool valid = false;

                            if (filter.IS_TREAT.HasValue)
                            {
                                if (filter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                                {
                                    valid = true;
                                    rdo.AMOUNT_NOITRU = item.AMOUNT;
                                    rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                                }
                                else if (!filter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                                {
                                    valid = true;
                                    rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                                    rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                                }
                            }
                            else
                            {
                                valid = true;
                                if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                                {
                                    rdo.AMOUNT_NOITRU = item.AMOUNT;
                                }
                                else if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                                {
                                    rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                                }

                                rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                            }

                            if (valid)
                            {
                                ListRdo.Add(rdo);
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
        private List<Mrs00279RDO> ProcessListRDO()
        {
            List<Mrs00279RDO> listCurrent = new List<Mrs00279RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var groupRDOs = ListRdo.GroupBy(o => new { o.SERVICE_ID, o.MATERIAL_PRICE, o.TOTAL_HEIN_PRICE, o.BID_NUMBER, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        var listsub = group.ToList<Mrs00279RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00279RDO rdo = new Mrs00279RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MATERIAL_CODE_DMBYT = listsub[0].MATERIAL_CODE_DMBYT;
                            rdo.MATERIAL_CODE_DMBYT_1 = listsub[0].MATERIAL_CODE_DMBYT_1;
                            rdo.MATERIAL_STT_DMBYT = listsub[0].MATERIAL_STT_DMBYT;
                            rdo.MATERIAL_TYPE_NAME_BYT = listsub[0].MATERIAL_TYPE_NAME_BYT;
                            rdo.MATERIAL_TYPE_NAME = listsub[0].MATERIAL_TYPE_NAME;
                            rdo.MATERIAL_QUYCACH_NAME = listsub[0].MATERIAL_QUYCACH_NAME;
                            rdo.BID_NUMBER = listsub[0].BID_NUMBER;
                            rdo.BID_NUM_ORDER = listsub[0].BID_NUM_ORDER;
                            rdo.MATERIAL_PRICE = listsub[0].MATERIAL_PRICE;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MATERIAL_UNIT_NAME = listsub[0].MATERIAL_UNIT_NAME;
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
            return listCurrent.OrderBy(o => o.MATERIAL_STT_DMBYT).ToList();
        }

    }
}
