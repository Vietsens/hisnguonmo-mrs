using MOS.MANAGER.HisHeinServiceType;
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

namespace MRS.Processor.Mrs00218
{
    public class Mrs00218Processor : AbstractProcessor
    {
        Mrs00218Filter castFilter = null;

        List<Mrs00218RDO> ListRdo = new List<Mrs00218RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();

        List<long> listHeinServiceTypeId;

        HIS_BRANCH _Branch = null;

        public Mrs00218Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00218Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00218Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, MRS00218, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00218");
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
                    listHeinServiceTypeId = new List<long>();
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT);
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);

                    }
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__VTYT)
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
                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00218.");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ProcessListRDO(ListRdo);
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

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ)
        {
            try
            {
                Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApprovalBhyt = new Dictionary<long, V_HIS_HEIN_APPROVAL>();
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        dicHeinApprovalBhyt[heinApproval.ID] = heinApproval;
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var sereServ in ListSereServ)
                    {
                        if (sereServ.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || sereServ.AMOUNT <= 0 || sereServ.ORIGINAL_PRICE == 0 || sereServ.HEIN_APPROVAL_ID == null || sereServ.TDL_HEIN_SERVICE_TYPE_ID == null || sereServ.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;
                        if (!listHeinServiceTypeId.Contains(sereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                            continue;

                        Mrs00218RDO rdo = new Mrs00218RDO();
                        rdo.SERVICE_ID = sereServ.SERVICE_ID;
                        rdo.MATERIAL_CODE_DMBYT = sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.MATERIAL_CODE_DMBYT_1 = sereServ.TDL_MATERIAL_GROUP_BHYT;
                        rdo.MATERIAL_STT_DMBYT = sereServ.TDL_HEIN_ORDER;
                        rdo.MATERIAL_TYPE_NAME_BYT = sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MATERIAL_TYPE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.MATERIAL_QUYCACH_NAME = sereServ.MATERIAL_PACKING_TYPE_NAME;
                        rdo.IMP_PRICE = sereServ.MATERIAL_IMP_PRICE * (1 + (sereServ.MATERIAL_IMP_VAT_RATIO ?? 0));
                        rdo.VIR_PRICE = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(sereServ.ORIGINAL_PRICE > 0 ? (sereServ.HEIN_LIMIT_PRICE.HasValue ? (sereServ.HEIN_LIMIT_PRICE.Value / (sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO))) * 100 : (sereServ.PRICE / sereServ.ORIGINAL_PRICE) * 100) : 0, 0);
                        rdo.MATERIAL_UNIT_NAME = sereServ.SERVICE_UNIT_NAME;
                        if (dicHeinApprovalBhyt.ContainsKey(sereServ.HEIN_APPROVAL_ID.Value))
                        {
                            if (dicHeinApprovalBhyt[sereServ.HEIN_APPROVAL_ID.Value].HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sereServ.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_NOITRU = sereServ.AMOUNT;
                            }

                            if (rdo.VIR_PRICE != null)
                            {
                                rdo.VIR_TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == hisHeinApprovals.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                                rdo.VIR_TOTAL_HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }

                            rdo.TOTAL_OTHER_SOURCE_PRICE = (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;

                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<Mrs00218RDO> ProcessListRDO(List<Mrs00218RDO> listRDO)
        {
            List<Mrs00218RDO> listCurrent = new List<Mrs00218RDO>();
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.IMP_PRICE, o.VIR_PRICE, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00218RDO> listsub = group.ToList<Mrs00218RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00218RDO rdo = new Mrs00218RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MATERIAL_CODE_DMBYT = listsub[0].MATERIAL_CODE_DMBYT;
                            rdo.MATERIAL_CODE_DMBYT_1 = listsub[0].MATERIAL_CODE_DMBYT_1;
                            rdo.MATERIAL_STT_DMBYT = listsub[0].MATERIAL_STT_DMBYT;
                            rdo.MATERIAL_TYPE_NAME_BYT = listsub[0].MATERIAL_TYPE_NAME_BYT;
                            rdo.MATERIAL_TYPE_NAME = listsub[0].MATERIAL_TYPE_NAME;
                            rdo.MATERIAL_QUYCACH_NAME = listsub[0].MATERIAL_QUYCACH_NAME;
                            rdo.IMP_PRICE = listsub[0].IMP_PRICE;
                            rdo.VIR_PRICE = listsub[0].VIR_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MATERIAL_UNIT_NAME = listsub[0].MATERIAL_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE;
                                rdo.VIR_TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE;
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
            return listCurrent.OrderBy(o => o.MATERIAL_STT_DMBYT).ToList();
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
                if (ListRdo.Count > 0) dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(ListRdo.Sum(o => o.VIR_TOTAL_PRICE).ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: Không đồng");
                if (ListRdo.Count > 0) dicSingleTag.Add("TOTAL_MONEY_HEIN_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(ListRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE).ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_HEIN_STR", "Tổng: Không đồng");
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SereServ", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
