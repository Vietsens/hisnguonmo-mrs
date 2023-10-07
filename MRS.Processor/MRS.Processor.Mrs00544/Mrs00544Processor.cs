using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00544
{
    class Mrs00544Processor : AbstractProcessor
    {
        Mrs00544Filter castFilter = null;
        List<Mrs00544RDO> ListExamRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListBedRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListTestRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListDiimFuexRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListSurgMisuRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListOtherRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListRdo = new List<Mrs00544RDO>();
        List<Mrs00544RDO> ListParent = new List<Mrs00544RDO>();
        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        private const string RouteCodeC = "C";

        List<V_HIS_HEIN_APPROVAL> ListHeinApprovalBhyt = new List<V_HIS_HEIN_APPROVAL>();

        List<V_HIS_SERVICE> listService = new List<V_HIS_SERVICE>();
        V_HIS_SERVICE ParentService = null;
        HIS_SERVICE_TYPE ServiceType = null;

        HIS_BRANCH _Branch = null;
        short IS_TRUE = 1;

        public Mrs00544Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00544Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00544Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00544:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                //this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => this.castFilter.BRANCH_ID == null || o.ID == this.castFilter.BRANCH_ID);

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
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00544");
                }

                //Loai dich vu loc
                if (this.castFilter.SERVICE_TYPE_ID.HasValue)
                {
                    ServiceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == castFilter.SERVICE_TYPE_ID);
                    if (ServiceType == null)
                    {
                        throw new Exception("Khong lay duoc ServiceType theo ServiceTypeId:" + castFilter.SERVICE_TYPE_ID);
                    }
                }

                //Dich vu loc
                if (this.castFilter.SERVICE_ID.HasValue)
                {
                    listService = new MOS.MANAGER.HisService.HisServiceManager(paramGet).GetView(new HisServiceViewFilterQuery());
                    if (paramGet.HasException)
                    {
                        throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_SERVICE, MRS00544");
                    }

                    if (IsNotNullOrEmpty(listService))
                    {
                        ParentService = listService.FirstOrDefault(o => o.ID == castFilter.SERVICE_ID);
                    }
                    else
                    {
                        throw new Exception("Khong lay duoc Service theo ServiceId:" + castFilter.SERVICE_ID);
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
                        if (castFilter.REQUEST_DEPARTMENT_ID != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == castFilter.REQUEST_DEPARTMENT_ID).ToList();
                        }
                        if (castFilter.REQUEST_DEPARTMENT_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                        }
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00544.");
                        }
                        //Inventec.Common.Logging.LogSystem.Info("ListSereServ" + ListSereServ.Count);
                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ, ListTreatment);
                        //Inventec.Common.Logging.LogSystem.Info("ListExamRdo" + ListExamRdo.Count);
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
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListParent = ProcessListRDO(ListRdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListExamRdo.Clear();
                ListBedRdo.Clear();
                ListTestRdo.Clear();
                ListDiimFuexRdo.Clear();
                ListSurgMisuRdo.Clear();
                ListOtherRdo.Clear();
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<HIS_SERE_SERV> ListSereServ, List<V_HIS_TREATMENT> listTreatment)
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
                    if (this.ParentService != null)
                    {
                        var service = listService.FirstOrDefault(o => o.ID == sere.SERVICE_ID);
                        if (service == null) continue;
                        if (service.PARENT_ID != ParentService.ID)
                            continue;
                    }
                    else if (this.ServiceType != null)
                    {
                        if (sere.TDL_SERVICE_TYPE_ID != ServiceType.ID)
                            continue;
                    }

                    if (sere.IS_NO_EXECUTE == IS_TRUE || sere.IS_EXPEND == IS_TRUE
                        || sere.AMOUNT <= 0 || sere.TDL_HEIN_SERVICE_TYPE_ID == null || sere.HEIN_APPROVAL_ID == null)
                        continue;

                    if (!sere.VIR_TOTAL_HEIN_PRICE.HasValue || sere.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;

                    if (dicHeinApprovalBhyt.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                    {
                        var heinAproval = dicHeinApprovalBhyt[sere.HEIN_APPROVAL_ID.Value];
                        bool valid = false;
                        Mrs00544RDO rdo = new Mrs00544RDO(sere);

                        rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sere, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH());
                        rdo.V_HIS_TREATMENT = listTreatment.FirstOrDefault(o => o.ID == sere.TDL_TREATMENT_ID) ?? new V_HIS_TREATMENT();
                        rdo.HIS_SERE_SERV = sere;
                        rdo.V_HIS_HEIN_APPROVAL = heinAproval;
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.V_HIS_TREATMENT.IN_TIME);
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.V_HIS_TREATMENT.OUT_TIME ?? 0);
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sere.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        var curentBranch = MRS.Processor.Mrs00544.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH();
                        rdo.BRANCH_NAME = curentBranch.BRANCH_NAME;
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (sere.OTHER_SOURCE_PRICE ?? 0) * sere.AMOUNT;
                        if (castFilter.IS_ROUTE == true)
                        {
                            if (curentBranch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinAproval.HEIN_MEDI_ORG_CODE))
                            {
                                rdo.ROUTE_CODE = RouteCodeA;
                            }
                            else
                            {
                                rdo.ROUTE_CODE = RouteCodeB;
                            }

                        }
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
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                ListDiimFuexRdo.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                            {
                                ListSurgMisuRdo.Add(rdo);
                            }
                            else if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                            {
                                ListOtherRdo.Add(rdo);
                            }
                            else if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                ListOtherRdo.Add(rdo);
                            }

                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
        }

        private List<Mrs00544RDO> ProcessListRDO(List<Mrs00544RDO> listRdo)
        {
            List<Mrs00544RDO> listCurrent = new List<Mrs00544RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_CODE_DMBYT, o.SERVICE_TYPE_NAME, o.ROUTE_CODE, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00544RDO> listsub = group.ToList<Mrs00544RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00544RDO rdo = new Mrs00544RDO();
                            rdo.BRANCH_NAME = listsub[0].BRANCH_NAME;
                            rdo.ROUTE_CODE = listsub[0].ROUTE_CODE;
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.PRICE = listsub[0].PRICE;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
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
            return listCurrent.OrderBy(o => o.SERVICE_TYPE_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                if (castFilter.IS_ROUTE != null)
                {
                    dicSingleTag.Add("IS_ROUTE", castFilter.IS_ROUTE == true ? "1" : "");
                }
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.castFilter.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report1", ListExamRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report2", ListBedRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report3", ListTestRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report4", ListDiimFuexRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report5", ListSurgMisuRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report6", ListOtherRdo ?? new List<Mrs00544RDO>());
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Parent", ListParent);
                objectTag.AddRelationship(store, "Parent", "Report", new string[] { "SERVICE_CODE_DMBYT", "PRICE", "SERVICE_TYPE_NAME" }, new string[] { "SERVICE_CODE_DMBYT", "PRICE", "SERVICE_TYPE_NAME" });
                objectTag.SetUserFunction(store, "fRound", new CustomerFuncRoundData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class CustomerFuncRoundData : TFlexCelUserFunction
    {

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            decimal result = 0;
            try
            {
                decimal dataIn1 = Convert.ToDecimal(parameters[0]);
                int dataIn2 = Convert.ToInt32(parameters[1]);
                result = Math.Round(dataIn1, dataIn2);


            }
            catch (Exception ex)
            {
                return 0;
            }

            return result;
        }
    }
}
