using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00645
{
    class Mrs00645Processor : AbstractProcessor
    {
        private Mrs00645Filter castFilter = null;
        private short IS_TRUE = 1;
        private List<V_HIS_HEIN_APPROVAL> ListHeinApprovalBhyt = new List<V_HIS_HEIN_APPROVAL>();

        private List<Mrs00645RDO> ListExamRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListBedRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListTestRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListDiimRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListFuexRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListSurgMisuRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListMisuRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListSurgRdo = new List<Mrs00645RDO>();
        private List<Mrs00645RDO> ListOtherRdo = new List<Mrs00645RDO>();
        private decimal TotalAmount = 0;

        public Mrs00645Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00645Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00645Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu Mrs00645:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                CommonParam paramGet = new CommonParam();
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApprovalBhyt = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00645");
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
                if (IsNotNullOrEmpty(ListHeinApprovalBhyt))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApprovalBhyt.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApprovalBhyt.Skip(start).Take(limit).ToList();
                        HisSereServFilterQuery ssHeinFilter = new HisSereServFilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        var ListSereServ = new HisSereServManager(paramGet).Get(ssHeinFilter);

                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00645.");
                        }
                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (IsNotNullOrEmpty(ListBedRdo))
                    {
                        ListBedRdo = ProcessListRDO(ListBedRdo);
                    }
                    if (IsNotNullOrEmpty(ListDiimRdo))
                    {
                        ListDiimRdo = ProcessListRDO(ListDiimRdo);
                    }
                    if (IsNotNullOrEmpty(ListFuexRdo))
                    {
                        ListFuexRdo = ProcessListRDO(ListFuexRdo);
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
                    if (IsNotNullOrEmpty(ListMisuRdo))
                    {
                        ListMisuRdo = ProcessListRDO(ListMisuRdo);
                    }
                    if (IsNotNullOrEmpty(ListSurgRdo))
                    {
                        ListSurgRdo = ProcessListRDO(ListSurgRdo);
                    }
                    if (IsNotNullOrEmpty(ListTestRdo))
                    {
                        ListTestRdo = ProcessListRDO(ListTestRdo);
                    }
                }
            }
            catch (Exception ex)
            {
                ListExamRdo.Clear();
                ListBedRdo.Clear();
                ListTestRdo.Clear();
                ListDiimRdo.Clear();
                ListFuexRdo.Clear();
                ListSurgMisuRdo.Clear();
                ListMisuRdo.Clear();
                ListSurgRdo.Clear();
                ListOtherRdo.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<HIS_SERE_SERV> ListSereServ)
        {
            Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApprovalBhyt = new Dictionary<long, V_HIS_HEIN_APPROVAL>();
            if (IsNotNullOrEmpty(heinApprovalBhyts))
            {
                foreach (var item in heinApprovalBhyts)
                {
                    dicHeinApprovalBhyt[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListSereServ))
            {
                foreach (var sere in ListSereServ)
                {
                    if (sere.IS_NO_EXECUTE == IS_TRUE || sere.IS_EXPEND == IS_TRUE
                        || sere.AMOUNT <= 0 || sere.PRICE <= 0 || sere.TDL_HEIN_SERVICE_TYPE_ID == null || sere.HEIN_APPROVAL_ID == null)
                        continue;
                    if (dicHeinApprovalBhyt.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                    {
                        var heinAproval = dicHeinApprovalBhyt[sere.HEIN_APPROVAL_ID.Value];
                        bool valid = false;
                        Mrs00645RDO rdo = new Mrs00645RDO(sere);
                        rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sere, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == sere.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (sere.OTHER_SOURCE_PRICE ?? 0) * sere.AMOUNT;
                        if (castFilter.IS_TREAT.HasValue)
                        {
                            if (castFilter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                valid = true;
                                rdo.AMOUNT_NOITRU = sere.AMOUNT;
                            }
                            else if (!castFilter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                valid = true;
                                rdo.AMOUNT_NGOAITRU = sere.AMOUNT;
                            }
                        }
                        else
                        {
                            valid = true;
                            if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = sere.AMOUNT;
                            }
                            else if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sere.AMOUNT;
                            }
                        }
                        if (valid)
                        {
                            if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                            {
                                ListExamRdo.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                            {
                                ListBedRdo.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                            {
                                ListTestRdo.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA)
                            {
                                ListDiimRdo.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                ListFuexRdo.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)
                            {
                                ListSurgMisuRdo.Add(rdo);
                                if(sere.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                                {
                                    ListMisuRdo.Add(rdo);
                                } 
                                else
                                {
                                    ListSurgRdo.Add(rdo);
                                }    
                            }
                            else
                            {
                                ListOtherRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
        }

        private List<Mrs00645RDO> ProcessListRDO(List<Mrs00645RDO> listRdo)
        {
            List<Mrs00645RDO> listCurrent = new List<Mrs00645RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.PRICE, o.RATIO }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00645RDO> listsub = group.ToList<Mrs00645RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00645RDO rdo = new Mrs00645RDO();
                            rdo.HEIN_SERVICE_BHYT_CODE = listsub.First().HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_BHYT_NAME = listsub.First().HEIN_SERVICE_BHYT_NAME;
                            rdo.HEIN_SERVICE_BHYT_ORDER = listsub.First().HEIN_SERVICE_BHYT_ORDER;

                            rdo.RATIO = listsub.First().RATIO;
                            rdo.RATIO_100 = listsub.First().RATIO_100;

                            rdo.ORIGINAL_PRICE = listsub.First().ORIGINAL_PRICE;
                            rdo.PRICE = listsub.First().PRICE;

                            rdo.AMOUNT_NOITRU = listsub.Sum(s => s.AMOUNT_NOITRU);
                            rdo.AMOUNT_NGOAITRU = listsub.Sum(s => s.AMOUNT_NGOAITRU);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);

                            rdo.TOTAL_HEIN_PRICE = listsub.Sum(o => o.TOTAL_HEIN_PRICE);
                            rdo.TOTAL_OTHER_SOURCE_PRICE = listsub.Sum(o => o.TOTAL_OTHER_SOURCE_PRICE);

                            TotalAmount += rdo.TOTAL_PRICE;
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
            return listCurrent.OrderBy(o => o.HEIN_SERVICE_BHYT_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
                objectTag.AddObjectData(store, "ReportExam", ListExamRdo);
                objectTag.AddObjectData(store, "ReportBed", ListBedRdo);
                objectTag.AddObjectData(store, "ReportTest", ListTestRdo);
                objectTag.AddObjectData(store, "ReportDiim", ListDiimRdo);
                objectTag.AddObjectData(store, "ReportFuex", ListFuexRdo);
                objectTag.AddObjectData(store, "ReportSurgMisu", ListSurgMisuRdo);
                objectTag.AddObjectData(store, "ReportMisu", ListMisuRdo);
                objectTag.AddObjectData(store, "ReportSurg", ListSurgRdo);
                objectTag.AddObjectData(store, "ReportOther", ListOtherRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
