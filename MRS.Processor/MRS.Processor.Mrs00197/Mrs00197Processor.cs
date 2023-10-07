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
using MRS.SDO;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00197
{
    internal class Mrs00197Processor : AbstractProcessor
    {
        Mrs00197Filter castFilter;

        List<VSarReportMrs00197RDO> ListExamRdo_A = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListBedRdo_A = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListTestRdo_A = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListDiimFuexRdo_A = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListSurgMisuRdo_A = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListOtherRdo_A = new List<VSarReportMrs00197RDO>();

        List<VSarReportMrs00197RDO> ListExamRdo_B = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListBedRdo_B = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListTestRdo_B = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListDiimFuexRdo_B = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListSurgMisuRdo_B = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListOtherRdo_B = new List<VSarReportMrs00197RDO>();

        List<VSarReportMrs00197RDO> ListExamRdo_C = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListBedRdo_C = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListTestRdo_C = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListDiimFuexRdo_C = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListSurgMisuRdo_C = new List<VSarReportMrs00197RDO>();
        List<VSarReportMrs00197RDO> ListOtherRdo_C = new List<VSarReportMrs00197RDO>();

        private const string EXAM = "EXAM";
        private const string BED = "BED";
        private const string TEST = "TEST";
        private const string DIIMFUEX = "DIIMFUEX";
        private const string SURGMISU = "SURGMISU";
        private const string OTHER = "OTHER";

        HIS_BRANCH _Branch = null;
        public Mrs00197Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00197Filter);
        }
        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            var result = false;
            try
            {
                this.castFilter = (Mrs00197Filter)this.reportFilter;
                this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu V_HIS_HEIN_APPROVAL, MRS00197 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);

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
            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            }

            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            }

            objectTag.AddObjectData(store, "ListExamRdo_A", ListExamRdo_A);
            objectTag.AddObjectData(store, "ListBedRdo_A", ListBedRdo_A);
            objectTag.AddObjectData(store, "ListTestRdo_A", ListTestRdo_A);
            objectTag.AddObjectData(store, "ListDiimFuexRdo_A", ListDiimFuexRdo_A);
            objectTag.AddObjectData(store, "ListSurgMisuRdo_A", ListSurgMisuRdo_A);
            objectTag.AddObjectData(store, "ListOtherRdo_A", ListOtherRdo_A);

            objectTag.AddObjectData(store, "ListExamRdo_B", ListExamRdo_B);
            objectTag.AddObjectData(store, "ListBedRdo_B", ListBedRdo_B);
            objectTag.AddObjectData(store, "ListTestRdo_B", ListTestRdo_B);
            objectTag.AddObjectData(store, "ListDiimFuexRdo_B", ListDiimFuexRdo_B);
            objectTag.AddObjectData(store, "ListSurgMisuRdo_B", ListSurgMisuRdo_B);
            objectTag.AddObjectData(store, "ListOtherRdo_B", ListOtherRdo_B);

            objectTag.AddObjectData(store, "ListExamRdo_C", ListExamRdo_C);
            objectTag.AddObjectData(store, "ListBedRdo_C", ListBedRdo_C);
            objectTag.AddObjectData(store, "ListTestRdo_C", ListTestRdo_C);
            objectTag.AddObjectData(store, "ListDiimFuexRdo_C", ListDiimFuexRdo_C);
            objectTag.AddObjectData(store, "ListSurgMisuRdo_C", ListSurgMisuRdo_C);
            objectTag.AddObjectData(store, "ListOtherRdo_C", ListOtherRdo_C);
        }

        private void ProcessListHeinApproval(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count();
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_HEIN_APPROVAL> heinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = heinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00197");
                        }
                        ProcessListSereServHein(heinApprovals, ListSereServ);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListExamRdo_A = ProcessListRDO(ListExamRdo_A);
                    ListBedRdo_A = ProcessListRDO(ListBedRdo_A);
                    ListTestRdo_A = ProcessListRDO(ListTestRdo_A);
                    ListDiimFuexRdo_A = ProcessListRDO(ListDiimFuexRdo_A);
                    ListSurgMisuRdo_A = ProcessListRDO(ListSurgMisuRdo_A);
                    ListOtherRdo_A = ProcessListRDO(ListOtherRdo_A);
                    ListExamRdo_B = ProcessListRDO(ListExamRdo_B);
                    ListBedRdo_B = ProcessListRDO(ListBedRdo_B);
                    ListTestRdo_B = ProcessListRDO(ListTestRdo_B);
                    ListDiimFuexRdo_B = ProcessListRDO(ListDiimFuexRdo_B);
                    ListSurgMisuRdo_B = ProcessListRDO(ListSurgMisuRdo_B);
                    ListOtherRdo_B = ProcessListRDO(ListOtherRdo_B);
                    ListExamRdo_C = ProcessListRDO(ListExamRdo_C);
                    ListBedRdo_C = ProcessListRDO(ListBedRdo_C);
                    ListTestRdo_C = ProcessListRDO(ListTestRdo_C);
                    ListDiimFuexRdo_C = ProcessListRDO(ListDiimFuexRdo_C);
                    ListSurgMisuRdo_C = ProcessListRDO(ListSurgMisuRdo_C);
                    ListOtherRdo_C = ProcessListRDO(ListOtherRdo_C);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListExamRdo_A.Clear();
                ListBedRdo_A.Clear();
                ListTestRdo_A.Clear();
                ListDiimFuexRdo_A.Clear();
                ListSurgMisuRdo_A.Clear();
                ListOtherRdo_A.Clear();
                ListExamRdo_B.Clear();
                ListBedRdo_B.Clear();
                ListTestRdo_B.Clear();
                ListDiimFuexRdo_B.Clear();
                ListSurgMisuRdo_B.Clear();
                ListOtherRdo_B.Clear();
                ListExamRdo_C.Clear();
                ListBedRdo_C.Clear();
                ListTestRdo_C.Clear();
                ListDiimFuexRdo_C.Clear();
                ListSurgMisuRdo_C.Clear();
                ListOtherRdo_C.Clear();
            }
        }

        private void ProcessListSereServHein(List<V_HIS_HEIN_APPROVAL> heinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ)
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
                        if (item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.AMOUNT <= 0 || item.VIR_TOTAL_HEIN_PRICE == 0 || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;
                        if (!dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                            continue;
                        var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                        VSarReportMrs00197RDO rdo = new VSarReportMrs00197RDO();
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE_DMBYT = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.SERVICE_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.SERVICE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.TOTAL_HEIN_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                        rdo.PRICE = item.HEIN_LIMIT_PRICE.HasValue ? item.HEIN_LIMIT_PRICE.Value : item.PRICE;
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                        bool valid = false;

                        if (castFilter.IS_TREAT.HasValue)
                        {
                            if (castFilter.IS_TREAT.Value && heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                valid = true;
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                                rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                            }
                            else if (!castFilter.IS_TREAT.Value && heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                valid = true;
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                                rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                            }
                        }
                        else
                        {
                            valid = true;
                            if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                            }
                            else if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }

                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                        }
                        if (valid)
                        {
                            if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                            {
                                rdo.TOTAL_HEIN_PRICE = item.PRICE;
                                ProcessRdoToList(rdo, heinApproval, EXAM);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                            {
                                ProcessRdoToList(rdo, heinApproval, BED);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                            {
                                ProcessRdoToList(rdo, heinApproval, TEST);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                ProcessRdoToList(rdo, heinApproval, DIIMFUEX);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                            {
                                ProcessRdoToList(rdo, heinApproval, SURGMISU);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                            {
                                ProcessRdoToList(rdo, heinApproval, OTHER);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                ProcessRdoToList(rdo, heinApproval, OTHER);
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

        private void ProcessRdoToList(VSarReportMrs00197RDO rdo, V_HIS_HEIN_APPROVAL heinApproval, string code)
        {
            try
            {
                if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                {
                    ProcessAddListRdo_A(rdo, code);
                }
                else if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                {
                    ProcessAddListRdo_B(rdo, code);
                }
                else
                {
                    ProcessAddListRdo_C(rdo, code);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddListRdo_A(VSarReportMrs00197RDO rdo, string Code)
        {
            try
            {
                switch (Code)
                {
                    case EXAM: ListExamRdo_A.Add(rdo); break;
                    case BED: ListBedRdo_A.Add(rdo); break;
                    case TEST: ListTestRdo_A.Add(rdo); break;
                    case DIIMFUEX: ListDiimFuexRdo_A.Add(rdo); break;
                    case SURGMISU: ListSurgMisuRdo_A.Add(rdo); break;
                    case OTHER: ListOtherRdo_A.Add(rdo); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddListRdo_B(VSarReportMrs00197RDO rdo, string Code)
        {
            try
            {
                switch (Code)
                {
                    case EXAM: ListExamRdo_B.Add(rdo); break;
                    case BED: ListBedRdo_B.Add(rdo); break;
                    case TEST: ListTestRdo_B.Add(rdo); break;
                    case DIIMFUEX: ListDiimFuexRdo_B.Add(rdo); break;
                    case SURGMISU: ListSurgMisuRdo_B.Add(rdo); break;
                    case OTHER: ListOtherRdo_B.Add(rdo); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddListRdo_C(VSarReportMrs00197RDO rdo, string Code)
        {
            try
            {
                switch (Code)
                {
                    case EXAM: ListExamRdo_C.Add(rdo); break;
                    case BED: ListBedRdo_C.Add(rdo); break;
                    case TEST: ListTestRdo_C.Add(rdo); break;
                    case DIIMFUEX: ListDiimFuexRdo_C.Add(rdo); break;
                    case SURGMISU: ListSurgMisuRdo_C.Add(rdo); break;
                    case OTHER: ListOtherRdo_C.Add(rdo); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<VSarReportMrs00197RDO> ProcessListRDO(List<VSarReportMrs00197RDO> listRdo)
        {
            List<VSarReportMrs00197RDO> listCurrent = new List<VSarReportMrs00197RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<VSarReportMrs00197RDO> listsub = group.ToList<VSarReportMrs00197RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            VSarReportMrs00197RDO rdo = new VSarReportMrs00197RDO();
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.AMOUNT_NOITRU = listsub.Sum(s => s.AMOUNT_NOITRU);
                            rdo.AMOUNT_NGOAITRU = listsub.Sum(s => s.AMOUNT_NGOAITRU);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);
                            rdo.TOTAL_OTHER_SOURCE_PRICE = listsub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
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
            return listCurrent.OrderBy(o => o.SERVICE_ID).ThenByDescending(o => o.PRICE).ToList();
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
