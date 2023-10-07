using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisHeinApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00522
{
    class Mrs00522Processor : AbstractProcessor
    {
        Mrs00522Filter castFilter = null;

        List<Mrs00522RDO> ListExamRdo_A = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListBedRdo_A = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListTestRdo_A = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListDiimFuexRdo_A = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListSurgMisuRdo_A = new List<Mrs00522RDO>();

        List<Mrs00522RDO> ListExamRdo_B = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListBedRdo_B = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListTestRdo_B = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListDiimFuexRdo_B = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListSurgMisuRdo_B = new List<Mrs00522RDO>();

        List<Mrs00522RDO> ListExamRdo_C = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListBedRdo_C = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListTestRdo_C = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListDiimFuexRdo_C = new List<Mrs00522RDO>();
        List<Mrs00522RDO> ListSurgMisuRdo_C = new List<Mrs00522RDO>();

        private const string TYPE_A = "A";
        private const string TYPE_B = "B";
        private const string TYPE_C = "C";

        List<V_HIS_HEIN_APPROVAL> hisHeinAppBhyts_A = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_HEIN_APPROVAL> hisHeinAppBhyts_B = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_HEIN_APPROVAL> hisHeinAppBhyts_C = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        HIS_BRANCH _Branch = null;


        public Mrs00522Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00522Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = false;
            try
            {
                this.castFilter = (Mrs00522Filter)this.reportFilter;
                //this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);

                this._Branch = new MOS.MANAGER.HisBranch.HisBranchManager().GetById(this.castFilter.BRANCH_ID);
                CommonParam paramGet = new CommonParam();
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                result = true;
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
            bool result = false;
            try
            {
                ProcessListHeinApproval(ListHeinApproval);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void ProcessListHeinApproval(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    foreach (var heinApprovalBhyt in ListHeinApproval)
                    {

                        if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApprovalBhyt.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(heinApprovalBhyt.HEIN_CARD_NUMBER))
                        {
                            hisHeinAppBhyts_A.Add(heinApprovalBhyt);
                        }
                        else
                        {
                            hisHeinAppBhyts_B.Add(heinApprovalBhyt);
                        }

                    }
                    ProcessListHeinApproval_A();
                    ProcessListHeinApproval_B();
                    ProcessListHeinApproval_C();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListHeinApproval_A()
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinAppBhyts_A))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = hisHeinAppBhyts_A.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_HEIN_APPROVAL> heinApprovals = hisHeinAppBhyts_A.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = heinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00522.");
                        }
                        ProcessListSereServHein(heinApprovals, ListSereServ, TYPE_A);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListExamRdo_A = ProcessListRDO(ListExamRdo_A);
                    ListBedRdo_A = ProcessListRDO(ListBedRdo_A);
                    ListTestRdo_A = ProcessListRDO(ListTestRdo_A);
                    ListDiimFuexRdo_A = ProcessListRDO(ListDiimFuexRdo_A);
                    ListSurgMisuRdo_A = ProcessListRDO(ListSurgMisuRdo_A);
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
                ListExamRdo_B.Clear();
                ListBedRdo_B.Clear();
                ListTestRdo_B.Clear();
                ListDiimFuexRdo_B.Clear();
                ListSurgMisuRdo_B.Clear();
                ListExamRdo_C.Clear();
                ListBedRdo_C.Clear();
                ListTestRdo_C.Clear();
                ListDiimFuexRdo_C.Clear();
                ListSurgMisuRdo_C.Clear();
            }
        }

        private void ProcessListHeinApproval_B()
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinAppBhyts_B))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = hisHeinAppBhyts_B.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_HEIN_APPROVAL> heinApprovals = hisHeinAppBhyts_B.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = heinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00522.");
                        }
                        ProcessListSereServHein(heinApprovals, ListSereServ, TYPE_B);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListExamRdo_B = ProcessListRDO(ListExamRdo_B);
                    ListBedRdo_B = ProcessListRDO(ListBedRdo_B);
                    ListTestRdo_B = ProcessListRDO(ListTestRdo_B);
                    ListDiimFuexRdo_B = ProcessListRDO(ListDiimFuexRdo_B);
                    ListSurgMisuRdo_B = ProcessListRDO(ListSurgMisuRdo_B);
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
                ListExamRdo_B.Clear();
                ListBedRdo_B.Clear();
                ListTestRdo_B.Clear();
                ListDiimFuexRdo_B.Clear();
                ListSurgMisuRdo_B.Clear();
                ListExamRdo_C.Clear();
                ListBedRdo_C.Clear();
                ListTestRdo_C.Clear();
                ListDiimFuexRdo_C.Clear();
                ListSurgMisuRdo_C.Clear();
            }
        }

        private void ProcessListHeinApproval_C()
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinAppBhyts_C))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = hisHeinAppBhyts_C.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_HEIN_APPROVAL> heinApprovals = hisHeinAppBhyts_C.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = heinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00522.");
                        }
                        ProcessListSereServHein(heinApprovals, ListSereServ, TYPE_C);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListExamRdo_C = ProcessListRDO(ListExamRdo_C);
                    ListBedRdo_C = ProcessListRDO(ListBedRdo_C);
                    ListTestRdo_C = ProcessListRDO(ListTestRdo_C);
                    ListDiimFuexRdo_C = ProcessListRDO(ListDiimFuexRdo_C);
                    ListSurgMisuRdo_C = ProcessListRDO(ListSurgMisuRdo_C);
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
                ListExamRdo_B.Clear();
                ListBedRdo_B.Clear();
                ListTestRdo_B.Clear();
                ListDiimFuexRdo_B.Clear();
                ListSurgMisuRdo_B.Clear();
                ListExamRdo_C.Clear();
                ListBedRdo_C.Clear();
                ListTestRdo_C.Clear();
                ListDiimFuexRdo_C.Clear();
                ListSurgMisuRdo_C.Clear();
            }
        }

        private void ProcessListSereServHein(List<V_HIS_HEIN_APPROVAL> heinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, string Type)
        {
            try
            {
                Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApprovalBhyt = new Dictionary<long, V_HIS_HEIN_APPROVAL>();
                if (IsNotNullOrEmpty(heinApprovals))
                {
                    foreach (var item in heinApprovals)
                    {
                        dicHeinApprovalBhyt[item.ID] = item;
                    }
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {

                    foreach (var sere in ListSereServ)
                    {
                        if (sere.IS_EXPEND == 1 || sere.AMOUNT <= 0 || sere.TDL_HEIN_SERVICE_TYPE_ID == null || sere.HEIN_APPROVAL_ID == null || sere.IS_NO_EXECUTE == 1)
                            continue;
                        if (dicHeinApprovalBhyt.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                        {

                        }
                        var heinAproval = dicHeinApprovalBhyt[sere.HEIN_APPROVAL_ID.Value];
                        bool valid = false;
                        Mrs00522RDO rdo = new Mrs00522RDO();
                        rdo.SERVICE_ID = sere.SERVICE_ID;
                        rdo.SERVICE_CODE_DMBYT = sere.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.SERVICE_STT_DMBYT = sere.TDL_HEIN_ORDER;
                        rdo.SERVICE_TYPE_NAME = sere.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.ORIGINAL_PRICE = sere.ORIGINAL_PRICE;
                        rdo.HEIN_PRICE = sere.ORIGINAL_PRICE * (1 + sere.VAT_RATIO);
                        rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sere, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH());
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
                            //MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__EXAM)
                            {
                                if (Type == TYPE_A)
                                {
                                    ListExamRdo_A.Add(rdo);
                                }
                                else if (Type == TYPE_B)
                                {
                                    ListExamRdo_B.Add(rdo);
                                }
                                else if (Type == TYPE_C)
                                {
                                    ListExamRdo_C.Add(rdo);
                                }
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                            //MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__BED_IN || sere.TDL_HEIN_SERVICE_TYPE_ID == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__BED_OUT)
                            {
                                if (Type == TYPE_A)
                                {
                                    ListBedRdo_A.Add(rdo);
                                }
                                else if (Type == TYPE_B)
                                {
                                    ListBedRdo_B.Add(rdo);
                                }
                                else if (Type == TYPE_C)
                                {
                                    ListBedRdo_C.Add(rdo);
                                }
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                            //MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__TEST)
                            {
                                if (Type == TYPE_A)
                                {
                                    ListTestRdo_A.Add(rdo);
                                }
                                else if (Type == TYPE_B)
                                {
                                    ListTestRdo_B.Add(rdo);
                                }
                                else if (Type == TYPE_C)
                                {
                                    ListTestRdo_C.Add(rdo);
                                }
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                if (Type == TYPE_A)
                                {
                                    ListDiimFuexRdo_A.Add(rdo);
                                }
                                else if (Type == TYPE_B)
                                {
                                    ListDiimFuexRdo_B.Add(rdo);
                                }
                                else if (Type == TYPE_C)
                                {
                                    ListDiimFuexRdo_C.Add(rdo);
                                }
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                            {
                                if (Type == TYPE_A)
                                {
                                    ListSurgMisuRdo_A.Add(rdo);
                                }
                                else if (Type == TYPE_B)
                                {
                                    ListSurgMisuRdo_B.Add(rdo);
                                }
                                else if (Type == TYPE_C)
                                {
                                    ListSurgMisuRdo_C.Add(rdo);
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

        private List<Mrs00522RDO> ProcessListRDO(List<Mrs00522RDO> listRdo)
        {
            List<Mrs00522RDO> listCurrent = new List<Mrs00522RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.HEIN_PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00522RDO> listsub = group.ToList<Mrs00522RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            var subsub = listsub.Where(o => o.ORIGINAL_PRICE > 0).ToList();
                            Mrs00522RDO rdo = new Mrs00522RDO();
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            if (IsNotNullOrEmpty(subsub)) rdo.ORIGINAL_PRICE = subsub[0].ORIGINAL_PRICE;
                            rdo.HEIN_PRICE = listsub[0].HEIN_PRICE;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                            }
                            if ((rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0) && (rdo.HEIN_PRICE > 0))
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
            return listCurrent.OrderBy(o => o.SERVICE_ID).ThenByDescending(o => o.HEIN_PRICE).ToList();
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
            objectTag.AddObjectData(store, "ListExamRdo_B", ListExamRdo_B);
            objectTag.AddObjectData(store, "ListBedRdo_B", ListBedRdo_B);
            objectTag.AddObjectData(store, "ListTestRdo_B", ListTestRdo_B);
            objectTag.AddObjectData(store, "ListDiimFuexRdo_B", ListDiimFuexRdo_B);
            objectTag.AddObjectData(store, "ListSurgMisuRdo_B", ListSurgMisuRdo_B);
            objectTag.AddObjectData(store, "ListExamRdo_C", ListExamRdo_C);
            objectTag.AddObjectData(store, "ListBedRdo_C", ListBedRdo_C);
            objectTag.AddObjectData(store, "ListTestRdo_C", ListTestRdo_C);
            objectTag.AddObjectData(store, "ListDiimFuexRdo_C", ListDiimFuexRdo_C);
            objectTag.AddObjectData(store, "ListSurgMisuRdo_C", ListSurgMisuRdo_C);

            store.SetCommonFunctions();

        }
    }
}
