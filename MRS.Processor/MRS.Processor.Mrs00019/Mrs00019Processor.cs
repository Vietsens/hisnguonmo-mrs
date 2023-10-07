using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisHeinServiceType;
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

namespace MRS.Processor.Mrs00019
{
    class Mrs00019Processor : AbstractProcessor
    {
        Mrs00019Filter castFilter = null;
        List<Mrs00019RDO> ListExamRdo = new List<Mrs00019RDO>();
        List<Mrs00019RDO> ListBedRdo = new List<Mrs00019RDO>();
        List<Mrs00019RDO> ListTestRdo = new List<Mrs00019RDO>();
        List<Mrs00019RDO> ListDiimFuexRdo = new List<Mrs00019RDO>();
        List<Mrs00019RDO> ListSurgMisuRdo = new List<Mrs00019RDO>();
        List<Mrs00019RDO> ListOtherRdo = new List<Mrs00019RDO>();
        List<Mrs00019RDO> ListDetail = new List<Mrs00019RDO>();

        private decimal TotalAmount = 0;
        List<V_HIS_HEIN_APPROVAL> ListHeinApprovalBhyt = new List<V_HIS_HEIN_APPROVAL>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<HIS_HEIN_SERVICE_TYPE> ListHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();

        HIS_BRANCH _Branch = null;
        short IS_TRUE = 1;
        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        private const string RouteCodeC = "C";
        private const string NoiTinh = "NOI";
        private const string NgoaiTinh = "NGOAI";
        public Mrs00019Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00019Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00019Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00019:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => this.castFilter.BRANCH_ID == null || o.ID == this.castFilter.BRANCH_ID);
                Inventec.Common.Logging.LogSystem.Info("MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT:" + MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT);
                Inventec.Common.Logging.LogSystem.Info("HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT:" + HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT);
                CommonParam paramGet = new CommonParam();
                ListService = new HisServiceManager().Get(new HisServiceFilterQuery());
                if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                    if (castFilter.END_DEPARTMENT_IDs != null)
                    {
                        treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                    }
                    ListTreatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                    //if (castFilter.PATIENT_TYPE_IDs != null)
                    //    ListTreatment = ListTreatment.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();

                    ListTreatment = ListTreatment.Where(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                    int skip = 0;
                    while (ListTreatment.Count - skip > 0)
                    {
                        var listId = ListTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                        approvalFilter.TREATMENT_IDs = listId.Select(s => s.ID).ToList();
                        approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                        approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                        approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                        approvalFilter.ORDER_DIRECTION = "ASC";
                        var hein = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                        if (IsNotNullOrEmpty(hein))
                        {
                            ListHeinApprovalBhyt.AddRange(hein);
                        }
                    }
                }
                else
                {
                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                    approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    ListHeinApprovalBhyt = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                    ListTreatment = new List<HIS_TREATMENT>();
                    List<long> treatmentIds = ListHeinApprovalBhyt.Select(s => s.TREATMENT_ID).Distinct().ToList();
                    int skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listId = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        treatmentFilter.IDs = listId;
                        treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                        var treatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                        if (IsNotNullOrEmpty(treatment))
                        {
                            ListTreatment.AddRange(treatment);
                        }
                    }
                    //if (castFilter.PATIENT_TYPE_IDs != null)
                    //    ListTreatment = ListTreatment.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();

                    ListHeinApprovalBhyt = ListHeinApprovalBhyt.Where(o => ListTreatment.Exists(e => e.ID == o.TREATMENT_ID)).ToList();
                }

                //loại dịch vụ bảo hiểm
                GetHeinServiceType();
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00019");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetHeinServiceType()
        {
            this.ListHeinServiceType = new HisHeinServiceTypeManager().Get(new HisHeinServiceTypeFilterQuery());
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
                        if (castFilter.SERVICE_TYPE_ID!=null&&castFilter.SERVICE_TYPE_ID!=0)
                        {
                            ssHeinFilter.TDL_SERVICE_TYPE_ID = castFilter.SERVICE_TYPE_ID;
                        }
                        if (castFilter.SERVICE_TYPE_IDs != null)
                        {
                            ssHeinFilter.TDL_SERVICE_TYPE_IDs = castFilter.SERVICE_TYPE_IDs;
                        }
                        var ListSereServ = new HisSereServManager(paramGet).Get(ssHeinFilter);
                        if (castFilter.EXECUTE_DEPARTMENT_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(x => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(x.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(x => castFilter.PATIENT_TYPE_IDs.Contains(x.PATIENT_TYPE_ID)).ToList();
                        }
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0 && o.AMOUNT > 0 && o.PRICE > 0 && o.IS_NO_EXECUTE != IS_TRUE && o.IS_EXPEND != IS_TRUE).ToList();
                        }

                        if (!castFilter.OUT_TIME_FROM.HasValue || !castFilter.OUT_TIME_TO.HasValue)
                        {
                            HisTreatmentFilterQuery TreatmentFilter = new HisTreatmentFilterQuery();
                            TreatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).Distinct().ToList();
                            ListTreatment = new HisTreatmentManager(paramGet).Get(TreatmentFilter);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00018");
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

                    //gộp theo key
                    if (this.dicDataFilter.ContainsKey("KEY_GROUP_SS") && this.dicDataFilter["KEY_GROUP_SS"] != null)
                    {
                        string KeyGroupSS = this.dicDataFilter["KEY_GROUP_SS"].ToString();

                        ListDetail = ProcessGroup(ListDetail, KeyGroupSS);//ListDetail.
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

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<HIS_SERE_SERV> ListSereServ, List<HIS_TREATMENT> listTreatment)
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
                        var treatment = listTreatment.FirstOrDefault(o => o.ID == sere.TDL_TREATMENT_ID);
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID);
                        bool valid = false;
                        Mrs00019RDO rdo = new Mrs00019RDO(sere);
                        rdo.TREATMENT_CODE = heinAproval.TREATMENT_CODE;
                        rdo.HEIN_APPROVAL_CODE = heinAproval.HEIN_APPROVAL_CODE;
                        rdo.HEIN_CARD_NUMBER = sere.HEIN_CARD_NUMBER;
                        rdo.INTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sere.TDL_INTRUCTION_TIME);
                        rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sere.TDL_INTRUCTION_TIME);
                        rdo.TDL_SERVICE_NAME = sere.TDL_SERVICE_NAME;
                        rdo.AX_CODE = sere.TDL_ACTIVE_INGR_BHYT_CODE;
                        rdo.SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == sere.TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sere.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sere.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQUEST_USERNAME = sere.TDL_REQUEST_USERNAME;
                        rdo.AMOUNT_CC = 0;
                        rdo.AMOUNT_TNT = 0;
                        if (ListService != null)
                        {
                            var service = ListService.FirstOrDefault(o => o.ID == sere.SERVICE_ID);
                            if (service != null)
                            {
                                var parent = ListService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                                if (parent != null)
                                {
                                    rdo.CATEGORY_CODE = parent.SERVICE_CODE;
                                    rdo.CATEGORY_NAME = parent.SERVICE_NAME;
                                }
                            }
                        }
                        if (treatment != null)
                        {
                            rdo.END_CODE = treatment.END_CODE;
                            rdo.HEIN_MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
                            rdo.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                            rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                            rdo.ICD_NAME = treatment.ICD_NAME;
                        }
                        if (castFilter.IS_ROUTE == true)
                        {
                            if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinAproval.HEIN_MEDI_ORG_CODE))
                            {
                                rdo.ROUTE_CODE = RouteCodeA;
                                //nếu đúng là bệnh nhân ban đầu mà yêu cầu check thêm mã tỉnh thì check thêm mã tỉnh để khẳng định ban đầu
                                if (castFilter.IS_VALID_PROVINCE_FOR_A == true && !checkBhytProvinceCode(heinAproval.HEIN_CARD_NUMBER))
                                {
                                    rdo.ROUTE_CODE = RouteCodeB;
                                }
                            }
                            else
                            {
                                rdo.ROUTE_CODE = RouteCodeB;
                            }

                        }
                        rdo.PRICE = sere.ORIGINAL_PRICE * (1 + sere.VAT_RATIO);
                        rdo.TOTAL_HEIN_PRICE = (sere.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sere, (_Branch != null ? _Branch : new HIS_BRANCH()));
                        rdo.BHYT_PAY_RATE = Math.Round(sere.ORIGINAL_PRICE > 0 ? (sere.HEIN_LIMIT_PRICE.HasValue ? (sere.HEIN_LIMIT_PRICE.Value / (sere.ORIGINAL_PRICE * (1 + sere.VAT_RATIO))) * 100 : (sere.PRICE / sere.ORIGINAL_PRICE) * 100) : 0, 0);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (sere.OTHER_SOURCE_PRICE ?? 0) * sere.AMOUNT;
                        rdo.BRANCH_NAME = (_Branch != null ? _Branch : new HIS_BRANCH()).BRANCH_NAME;
                        if (checkBhytProvinceCode(heinAproval.HEIN_CARD_NUMBER))
                        {
                            rdo.PROVINCE_TYPE = NoiTinh;
                        }
                        else
                        {
                            rdo.PROVINCE_TYPE = NgoaiTinh;
                        }
                        rdo.RIGHT_ROUTE_CODE = heinAproval.RIGHT_ROUTE_CODE;
                        if (castFilter.IS_RIGHT_ROUTE != null)
                        {
                            if (castFilter.IS_RIGHT_ROUTE == true && rdo.RIGHT_ROUTE_CODE != "DT")
                            {
                                continue;
                            }
                            if (castFilter.IS_RIGHT_ROUTE == false && rdo.RIGHT_ROUTE_CODE != "TT")
                            {
                                continue;
                            }
                        }
                        if (castFilter.IS_IN_PROVICE != null)
                        {
                            if (castFilter.IS_IN_PROVICE == true && rdo.PROVINCE_TYPE != NoiTinh)
                            {
                                continue;
                            }
                            if (castFilter.IS_IN_PROVICE == false && rdo.PROVINCE_TYPE != NgoaiTinh)
                            {
                                continue;
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
                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 1;
                                rdo.HEIN_SERVICE_TYPE_NAME = (ListHeinServiceType.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_NAME;
                                ListExamRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                            {

                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 2;
                                
                                rdo.HEIN_SERVICE_TYPE_NAME = "Giường";
                                ListBedRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                            {
                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 3;
                                rdo.HEIN_SERVICE_TYPE_NAME = (ListHeinServiceType.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_NAME;
                                ListTestRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 3;
                                rdo.HEIN_SERVICE_TYPE_NAME = (ListHeinServiceType.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_NAME;
                                ListDiimFuexRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                            else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                            {
                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 4;
                                rdo.HEIN_SERVICE_TYPE_NAME = (ListHeinServiceType.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_NAME;
                                ListSurgMisuRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                            else if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                            {
                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 5;
                                rdo.HEIN_SERVICE_TYPE_NAME = (ListHeinServiceType.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_NAME;
                                ListOtherRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                            else if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                rdo.TDL_HEIN_SERVICE_TYPE_ID = 6;
                                rdo.HEIN_SERVICE_TYPE_NAME = (ListHeinServiceType.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_NAME;
                                ListOtherRdo.Add(rdo);
                                ListDetail.Add(rdo);
                            }
                        }
                    }
                }
            }
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
        private List<Mrs00019RDO> ProcessListRDO(List<Mrs00019RDO> listRdo)
        {
            List<Mrs00019RDO> listCurrent = new List<Mrs00019RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_CODE_DMBYT, o.SERVICE_TYPE_NAME, o.PRICE, o.ROUTE_CODE, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00019RDO> listsub = group.ToList<Mrs00019RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00019RDO rdo = new Mrs00019RDO();
                            rdo.BRANCH_NAME = listsub[0].BRANCH_NAME;
                            rdo.ROUTE_CODE = listsub[0].ROUTE_CODE;
                             rdo.PROVINCE_TYPE = listsub[0].PROVINCE_TYPE;
                            rdo.RIGHT_ROUTE_CODE = listsub[0].RIGHT_ROUTE_CODE;
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.CATEGORY_CODE = listsub[0].CATEGORY_CODE;
                            rdo.CATEGORY_NAME = listsub[0].CATEGORY_NAME;
                            rdo.HEIN_PRICE = listsub[0].HEIN_PRICE;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.SERVICE_UNIT_NAME = listsub[0].SERVICE_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
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
            return listCurrent.OrderBy(o => o.SERVICE_TYPE_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        private List<Mrs00019RDO> ProcessGroup(List<Mrs00019RDO> listRdo, string KeyGroupSS)
        {
            List<Mrs00019RDO> listCurrent = new List<Mrs00019RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => string.Format(KeyGroupSS, o.SERVICE_CODE_DMBYT, o.SERVICE_TYPE_NAME, o.PRICE, o.ROUTE_CODE, o.BHYT_PAY_RATE, o.REQUEST_DEPARTMENT_NAME, o.TDL_HEIN_SERVICE_TYPE_ID)).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00019RDO> listsub = group.ToList<Mrs00019RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00019RDO rdo = new Mrs00019RDO();
                            rdo.BRANCH_NAME = listsub[0].BRANCH_NAME;
                            rdo.ROUTE_CODE = listsub[0].ROUTE_CODE;
                            rdo.PROVINCE_TYPE = listsub[0].PROVINCE_TYPE;
                            rdo.RIGHT_ROUTE_CODE = listsub[0].RIGHT_ROUTE_CODE;
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.CATEGORY_CODE = listsub[0].CATEGORY_CODE;
                            rdo.CATEGORY_NAME = listsub[0].CATEGORY_NAME;
                            rdo.HEIN_PRICE = listsub[0].HEIN_PRICE;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.SERVICE_UNIT_NAME = listsub[0].SERVICE_UNIT_NAME;
                            rdo.REQUEST_DEPARTMENT_NAME = listsub[0].REQUEST_DEPARTMENT_NAME;
                            rdo.TDL_HEIN_SERVICE_TYPE_ID = listsub[0].TDL_HEIN_SERVICE_TYPE_ID;
                            rdo.HEIN_SERVICE_TYPE_NAME = listsub[0].HEIN_SERVICE_TYPE_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
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
            return listCurrent.OrderBy(o => o.SERVICE_TYPE_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            AddGeneral(dicSingleTag, objectTag, store);
            AddA(dicSingleTag, objectTag, store);
            AddB(dicSingleTag, objectTag, store);


        }

        private void AddA(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "ReportA1", ListExamRdo.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportA2", ListBedRdo.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportA3", ListTestRdo.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportA4", ListDiimFuexRdo.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportA5", ListSurgMisuRdo.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportA6", ListOtherRdo.Where(o => o.ROUTE_CODE == "A").ToList() ?? new List<Mrs00019RDO>());
        }

        private void AddB(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "ReportB1", ListExamRdo.Where(o => o.ROUTE_CODE == "B").ToList());
            objectTag.AddObjectData(store, "ReportB2", ListBedRdo.Where(o => o.ROUTE_CODE == "B").ToList());
            objectTag.AddObjectData(store, "ReportB3", ListTestRdo.Where(o => o.ROUTE_CODE == "B").ToList());
            objectTag.AddObjectData(store, "ReportB4", ListDiimFuexRdo.Where(o => o.ROUTE_CODE == "B").ToList());
            objectTag.AddObjectData(store, "ReportB5", ListSurgMisuRdo.Where(o => o.ROUTE_CODE == "B").ToList());
            objectTag.AddObjectData(store, "ReportB6", ListOtherRdo.Where(o => o.ROUTE_CODE == "B").ToList() ?? new List<Mrs00019RDO>());
        }

        private void AddGeneral(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
            dicSingleTag.Add("AMOUNT_TEXT_1", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount - ListOtherRdo.Sum(o => o.TOTAL_PRICE)).ToString()));
            objectTag.AddObjectData(store, "Report1", ListExamRdo);
            objectTag.AddObjectData(store, "Report2", ListBedRdo);
            objectTag.AddObjectData(store, "Report3", ListTestRdo);
            objectTag.AddObjectData(store, "Report4", ListDiimFuexRdo);
            objectTag.AddObjectData(store, "Report5", ListSurgMisuRdo);
            objectTag.AddObjectData(store, "Report6", ListOtherRdo ?? new List<Mrs00019RDO>());
            objectTag.AddObjectData(store, "Detail", ListDetail);
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;
        }

        private void SelectSheet(ref Inventec.Common.FlexCellExport.Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {

                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);
                if (castFilter.IS_ROUTE != false || castFilter.INPUT_DATA_ID_RPT == 1)
                {
                    xls.ActiveSheet = 1;
                }
                else if (castFilter.IS_ROUTE == false || castFilter.INPUT_DATA_ID_RPT == 2)
                {
                    xls.ActiveSheet = 2;
                }


                xls.Save(resultStream);
                //resultStream = result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
