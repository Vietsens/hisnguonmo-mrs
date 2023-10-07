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
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPtttGroup;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00224
{
    public class Mrs00224Processor : AbstractProcessor
    {
        Mrs00224Filter castFilter = null;
        List<Mrs00224RDO> ListExamRdo = new List<Mrs00224RDO>();
        List<Mrs00224RDO> ListBedRdo = new List<Mrs00224RDO>();
        List<Mrs00224RDO> ListTestRdo = new List<Mrs00224RDO>();
        List<Mrs00224RDO> ListDiimFuexRdo = new List<Mrs00224RDO>();
        List<Mrs00224RDO> ListSurgMisuRdo = new List<Mrs00224RDO>();
        List<Mrs00224RDO> ListOtherRdo = new List<Mrs00224RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<HIS_PTTT_GROUP> ListPtttGroup = new List<HIS_PTTT_GROUP>();

        HIS_BRANCH _Branch = null;

        public Mrs00224Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00224Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00224Filter)this.reportFilter;
                //this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                //if (this._Branch == null)
                //    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, MRS00224, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                serviceFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListService = new HisServiceManager(paramGet).Get(serviceFilter);

                HisPtttGroupFilterQuery ptttGroupFilter = new HisPtttGroupFilterQuery();
                ptttGroupFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListPtttGroup = new HisPtttGroupManager(paramGet).Get(ptttGroupFilter);

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00223");
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
                    CommonParam paramGet = new CommonParam();
                    ListHeinApproval = ListHeinApproval.Where(o => CheckHeinCardNumberType(o.HEIN_CARD_NUMBER)).ToList();
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

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00223.");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (IsNotNullOrEmpty(ListBedRdo))
                    {
                        ListBedRdo = ProcessListRDO(ListBedRdo);
                    }
                    if (IsNotNullOrEmpty(ListDiimFuexRdo))
                    {
                        ListDiimFuexRdo = ProcessListRDO(ListDiimFuexRdo);
                    }
                    if (IsNotNullOrEmpty(ListExamRdo))
                    {
                        ListExamRdo = ProcessListRDO(ListExamRdo);
                    }
                    if (IsNotNullOrEmpty(ListOtherRdo))
                    {
                        ListOtherRdo = ProcessListRDO(ListOtherRdo);
                    }
                    if (IsNotNullOrEmpty(ListSurgMisuRdo))
                    {
                        ListSurgMisuRdo = ProcessListRDO(ListSurgMisuRdo);
                    }
                    if (IsNotNullOrEmpty(ListTestRdo))
                    {
                        ListTestRdo = ProcessListRDO(ListTestRdo);
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
                    foreach (var item in hisHeinApprovals)
                    {
                        dicHeinApprovalBhyt[item.ID] = item;
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var sere in ListSereServ)
                    {
                        if (sere.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || sere.AMOUNT <= 0 || sere.PRICE == 0 || sere.HEIN_APPROVAL_ID == null || sere.TDL_HEIN_SERVICE_TYPE_ID == null || sere.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;
                        bool valid = false;
                        Mrs00224RDO rdo = new Mrs00224RDO(sere);
                        rdo.VIR_TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sere, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == hisHeinApprovals.FirstOrDefault(p => p.ID == sere.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                        rdo.VIR_TOTAL_HEIN_PRICE = sere.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (sere.OTHER_SOURCE_PRICE ?? 0) * sere.AMOUNT;
                        rdo.PARENT_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sere.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                        var service = ListService.FirstOrDefault(o => o.ID == sere.SERVICE_ID) ?? new HIS_SERVICE();
                        rdo.PTTT_GROUP_NAME = (ListPtttGroup.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                        if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            valid = true;
                            ListExamRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            valid = true;
                            ListBedRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            valid = true;
                            var parent = ListService.FirstOrDefault(o => o.ID == service.PARENT_ID) ?? new HIS_SERVICE();
                            rdo.PARENT_NAME = parent.SERVICE_NAME;
                            ListTestRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            valid = true;
                            ListDiimFuexRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            valid = true;
                            ListSurgMisuRdo.Add(rdo);
                        }
                        else if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                        {
                            valid = true;
                            ListOtherRdo.Add(rdo);
                        }
                        else if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            valid = true;
                            ListOtherRdo.Add(rdo);
                        }
                        if (valid)
                        {
                            if (dicHeinApprovalBhyt[sere.HEIN_APPROVAL_ID.Value].HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sere.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_NOITRU = sere.AMOUNT;
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

        private List<Mrs00224RDO> ProcessListRDO(List<Mrs00224RDO> listRdo)
        {
            List<Mrs00224RDO> listCurrent = new List<Mrs00224RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00224RDO> listsub = group.ToList<Mrs00224RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00224RDO rdo = new Mrs00224RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.PARENT_NAME = listsub[0].PARENT_NAME;
                            rdo.PTTT_GROUP_NAME = listsub[0].PTTT_GROUP_NAME;
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.VIR_PRICE = listsub[0].VIR_PRICE;
                            rdo.PRICE = listsub[0].PRICE;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
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
            return listCurrent.OrderBy(o => o.SERVICE_ID).ThenByDescending(o => o.PRICE).ToList();
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
                if (ListExamRdo.Count > 0) Total += ListExamRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListBedRdo.Count > 0) Total += ListBedRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListTestRdo.Count > 0) Total += ListTestRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListDiimFuexRdo.Count > 0) Total += ListDiimFuexRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListSurgMisuRdo.Count > 0) Total += ListSurgMisuRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListOtherRdo.Count > 0) Total += ListOtherRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (Total > 0) dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(Total.ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: Không đồng");
                Decimal TotalHein = 0;
                if (ListExamRdo.Count > 0) TotalHein += ListExamRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                if (ListBedRdo.Count > 0) TotalHein += ListBedRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                if (ListTestRdo.Count > 0) TotalHein += ListTestRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                if (ListDiimFuexRdo.Count > 0) TotalHein += ListDiimFuexRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                if (ListSurgMisuRdo.Count > 0) TotalHein += ListSurgMisuRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                if (ListOtherRdo.Count > 0) TotalHein += ListOtherRdo.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                if (Total > 0) dicSingleTag.Add("TOTAL_MONEY_HEIN_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(TotalHein.ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_HEIN_STR", "Tổng: Không đồng");
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ExamSereServ", ListExamRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "BedSereServ", ListBedRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TestSereServ", ListTestRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "DiimSereServ", ListDiimFuexRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SurgSereServ", ListSurgMisuRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "OtherSereServ", ListOtherRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
