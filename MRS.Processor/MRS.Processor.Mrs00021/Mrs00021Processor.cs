using HIS.Common.Treatment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisPatientCase;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00021
{
    public class Mrs00021Processor : AbstractProcessor
    {
        Mrs00021Filter castFilter = null;
        List<Mrs00021RDO> ListRdoA = new List<Mrs00021RDO>();
        List<Mrs00021RDO> ListRdoB = new List<Mrs00021RDO>();
        List<Mrs00021RDO> ListRdoC = new List<Mrs00021RDO>();
        Dictionary<string, Mrs00021RDO> dicRdoA = new Dictionary<string, Mrs00021RDO>();
        Dictionary<string, Mrs00021RDO> dicRdoB = new Dictionary<string, Mrs00021RDO>();
        Dictionary<string, Mrs00021RDO> dicRdoC = new Dictionary<string, Mrs00021RDO>();
        List<Mrs00021RDO> ListAll = new List<Mrs00021RDO>();
        List<Mrs00021RDO> ListSumTotal = new List<Mrs00021RDO>();
        List<Mrs00021RDO> Departments = new List<Mrs00021RDO>();
        List<Mrs00021RDO> ListRoom = new List<Mrs00021RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        HIS_BRANCH _Branch = null;
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_PATIENT_CASE> ListPatientCase = new List<HIS_PATIENT_CASE>();
        string MaterialPriceOption = "";
        const short VAOVIEN = 1;
        const short RAVIEN = 2;
        const short KHOAVIENPHI = 3;
        const short DUYETGIAMDINH = 4;
        private decimal TotalAmount = 0;

        public Mrs00021Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00021Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = true;
            try
            {
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                castFilter = ((Mrs00021Filter)this.reportFilter);

                LoadDataToRam();


                //lọc nội tỉnh ngoại tỉnh
                FilterProvinceType();

                //lọc đúng tuyến trái tuyến
                FilterRouteType();


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private void FilterRouteType()
        {
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 1)
            {
                ListHeinApproval = ListHeinApproval.Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE || !string.IsNullOrWhiteSpace(o.RIGHT_ROUTE_TYPE_CODE)).ToList();
            }
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 2)
            {
                ListHeinApproval = ListHeinApproval.Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE).ToList();
            }
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 3)
            {
                ListHeinApproval = ListHeinApproval.Where(o => string.IsNullOrWhiteSpace(o.RIGHT_ROUTE_CODE) && string.IsNullOrWhiteSpace(o.RIGHT_ROUTE_TYPE_CODE)).ToList();
            }
        }

        private void FilterProvinceType()
        {
            if (castFilter.INPUT_DATA_ID_PROVINCE_TYPE == 1)
            {
                ListHeinApproval = ListHeinApproval.Where(o => checkBhytProvinceCode(o.HEIN_CARD_NUMBER)).ToList();
            }
            if (castFilter.INPUT_DATA_ID_PROVINCE_TYPE == 2)
            {
                ListHeinApproval = ListHeinApproval.Where(o => !checkBhytProvinceCode(o.HEIN_CARD_NUMBER)).ToList();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                if (castFilter.INPUT_DATA_ID_TIME_TYPE == VAOVIEN || castFilter.INPUT_DATA_ID_TIME_TYPE == RAVIEN || castFilter.INPUT_DATA_ID_TIME_TYPE == KHOAVIENPHI)
                {
                    castFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                    castFilter.OUT_TIME_TO = castFilter.TIME_TO;
                }
                if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    if (castFilter.INPUT_DATA_ID_TIME_TYPE == VAOVIEN)
                    {
                        treatmentFilter.IN_TIME_FROM = castFilter.OUT_TIME_FROM;
                        treatmentFilter.IN_TIME_TO = castFilter.OUT_TIME_TO;
                    }
                    else if (castFilter.INPUT_DATA_ID_TIME_TYPE == KHOAVIENPHI)
                    {
                        treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                        treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.OUT_TIME_FROM;
                        treatmentFilter.FEE_LOCK_TIME_TO = castFilter.OUT_TIME_TO;
                    }
                    else
                    {
                        treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                        treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                        treatmentFilter.IS_PAUSE = true;
                    }
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;

                    if (castFilter.END_ROOM_IDs != null)
                    {
                        treatmentFilter.END_ROOM_IDs = castFilter.END_ROOM_IDs;
                    }

                    ListTreatment = new HisTreatmentManager().GetView(treatmentFilter);
                    if (castFilter.TREATMENT_TYPE_IDs != null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    }
                    if (castFilter.TREATMENT_END_TYPE_IDs != null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.TREATMENT_END_TYPE_IDs.Contains(o.TREATMENT_END_TYPE_ID ?? 0)).ToList();
                    }
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
                        var hein = new HisHeinApprovalManager().GetView(approvalFilter);
                        if (IsNotNullOrEmpty(hein))
                        {
                            ListHeinApproval.AddRange(hein);
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
                    ListHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);
                    Inventec.Common.Logging.LogSystem.Info("ListHeinApproval" + ListHeinApproval.Count);
                }
                HisPatientCaseFilterQuery pcFilter = new HisPatientCaseFilterQuery();
                ListPatientCase = new HisPatientCaseManager().Get(pcFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessListHeinApproval();
                ListRdoA = dicRdoA.Values.ToList();
                ListRdoB = dicRdoB.Values.ToList();
                ListRdoC = dicRdoC.Values.ToList();
                if (ListRdoA != null && ListRdoA.Count > 0)
                {
                    ListAll.AddRange(ListRdoA);
                }
                if (ListRdoB != null && ListRdoB.Count > 0)
                {
                    ListAll.AddRange(ListRdoB);
                }
                if (ListRdoC != null && ListRdoC.Count > 0)
                {
                    ListAll.AddRange(ListRdoC);
                }

                if (this.dicDataFilter.ContainsKey("KEY_GROUP_TREA") && this.dicDataFilter["KEY_GROUP_TREA"] != null)
                {
                    string KeyGroupTrea = this.dicDataFilter["KEY_GROUP_TREA"].ToString();
                    GroupByKey(KeyGroupTrea);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private void GroupByKey(string KeyGroupTrea)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(KeyGroupTrea))
                {
                    var group = ListAll.GroupBy(o => string.Format(KeyGroupTrea, o.END_DEPARTMENT_ID, o.END_ROOM_DEPARTMENT_ID, o.END_ROOM_ID, o.EXECUTE_DATE)).ToList();
                    ListAll.Clear();
                    ListAll = new List<Mrs00021RDO>();
                    foreach (var item in group)
                    {
                        List<Mrs00021RDO> list = item.ToList<Mrs00021RDO>();
                        Mrs00021RDO first = item.First();
                        Mrs00021RDO rdo = new Mrs00021RDO();
                        rdo.END_DEPARTMENT_ID = first.END_DEPARTMENT_ID;
                        rdo.END_DEPARTMENT_CODE = first.END_DEPARTMENT_CODE;
                        rdo.END_DEPARTMENT_NAME = first.END_DEPARTMENT_NAME;
                        rdo.END_ROOM_DEPARTMENT_ID = first.END_ROOM_DEPARTMENT_ID;
                        rdo.END_ROOM_DEPARTMENT_NAME = first.END_ROOM_DEPARTMENT_NAME;
                        rdo.END_ROOM_DEPARTMENT_CODE = first.END_ROOM_DEPARTMENT_CODE;
                        rdo.END_ROOM_ID = first.END_ROOM_ID;
                        rdo.END_ROOM_CODE = first.END_ROOM_CODE;
                        rdo.END_ROOM_NAME = first.END_ROOM_NAME;
                        rdo.EXECUTE_DATE = first.EXECUTE_DATE;
                        rdo.TOTAL_PRICE = list.Sum(s => s.TOTAL_PRICE);
                        rdo.TEST_PRICE = list.Sum(s => s.TEST_PRICE);
                        rdo.DIIM_PRICE = list.Sum(s => s.DIIM_PRICE);
                        rdo.MEDICINE_PRICE = list.Sum(s => s.MEDICINE_PRICE);
                        rdo.MEDICINE_PRICE_NDM = list.Sum(s => s.MEDICINE_PRICE_NDM);
                        rdo.BLOOD_PRICE = list.Sum(s => s.BLOOD_PRICE);
                        rdo.FUEX_PRICE = list.Sum(s => s.FUEX_PRICE);
                        rdo.SURGMISU_PRICE = list.Sum(s => s.SURGMISU_PRICE);
                        rdo.MATERIAL_PRICE = list.Sum(s => s.MATERIAL_PRICE);
                        rdo.SERVICE_PRICE_RATIO = list.Sum(s => s.SERVICE_PRICE_RATIO);
                        rdo.MEDICINE_PRICE_RATIO = list.Sum(s => s.MEDICINE_PRICE_RATIO);
                        rdo.MATERIAL_PRICE_RATIO = list.Sum(s => s.MATERIAL_PRICE_RATIO);
                        rdo.EXAM_PRICE = list.Sum(s => s.EXAM_PRICE);
                        rdo.BED_PRICE = list.Sum(s => s.BED_PRICE);
                        rdo.TRAN_PRICE = list.Sum(s => s.TRAN_PRICE);
                        rdo.TT_PRICE = list.Sum(s => s.TT_PRICE);
                        rdo.TOTAL_PATIENT_PRICE = list.Sum(s => s.TOTAL_PATIENT_PRICE);
                        rdo.TOTAL_PATIENT_PRICE_BHYT = list.Sum(s => s.TOTAL_PATIENT_PRICE_BHYT);
                        rdo.TOTAL_HEIN_PRICE = list.Sum(s => s.TOTAL_HEIN_PRICE);
                        rdo.TOTAL_HEIN_PRICE_NDS = list.Sum(s => s.TOTAL_HEIN_PRICE_NDS);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = list.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
                        rdo.COUNT_TREATMENT = list.GroupBy(o => o.TREATMENT_CODE).Select(o => o.First()).Count();
                        ListAll.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void ProcessListHeinApproval()
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    ListHeinApproval = ListHeinApproval.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT).ToList();
                }

                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();

                        List<V_HIS_TREATMENT> treatment = new List<V_HIS_TREATMENT>();
                        if (!IsNotNullOrEmpty(ListTreatment))
                        {
                            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                            treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                            treatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        }
                        else
                        {
                            treatment.AddRange(ListTreatment);
                        }
                        if (castFilter.END_ROOM_IDs != null)
                        {
                            treatment = treatment.Where(o => castFilter.END_ROOM_IDs.Contains(o.END_ROOM_ID ?? 0)).ToList();
                            var treatmentIdLimits = treatment.Select(o => o.ID).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treatmentIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }
                        if (castFilter.END_DEPARTMENT_IDs != null)
                        {
                            treatment = treatment.Where(o => castFilter.END_DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID ?? 0)).ToList();
                            var treatmentIdLimits = treatment.Select(o => o.ID).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treatmentIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }

                        if (castFilter.TREATMENT_TYPE_IDs != null)
                        {
                            treatment = treatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                            var treatmentIdLimits = treatment.Select(o => o.ID).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treatmentIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }
                        if (castFilter.TREATMENT_END_TYPE_IDs != null)
                        {
                            treatment = treatment.Where(o => castFilter.TREATMENT_END_TYPE_IDs.Contains(o.TREATMENT_END_TYPE_ID ?? 0)).ToList();
                            var treatmentIdLimits = treatment.Select(o => o.ID).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => treatmentIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }
                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();

                        var ListSereServ = new HisSereServManager(paramGet).GetView3(ssHeinFilter);

                        if (castFilter.SERVICE_TYPE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID)).ToList();
                            var heinApprovalIdLimits = ListSereServ.Select(o => o.HEIN_APPROVAL_ID ?? 0).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => heinApprovalIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }
                        if (castFilter.SERVICE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(o => castFilter.SERVICE_IDs.Contains(o.SERVICE_ID)).ToList();
                            var heinApprovalIdLimits = ListSereServ.Select(o => o.HEIN_APPROVAL_ID ?? 0).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => heinApprovalIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            ListSereServ = ListSereServ.Where(x => castFilter.PATIENT_TYPE_IDs.Contains(x.PATIENT_TYPE_ID)).ToList();
                            var heinApprovalIdLimits = ListSereServ.Select(o => o.HEIN_APPROVAL_ID ?? 0).Distinct().ToList();
                            hisHeinApprovals = hisHeinApprovals.Where(o => heinApprovalIdLimits.Contains(o.TREATMENT_ID)).ToList();
                        }
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        List<HIS_SERVICE_REQ> serviceReqs = this.GetServiceReq(ListSereServ);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00021.");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, serviceReqs, treatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoA.Clear();
                ListRdoB.Clear();
                ListRdoC.Clear();
            }
        }

        private List<HIS_SERVICE_REQ> GetServiceReq(List<V_HIS_SERE_SERV_3> ListSereServ)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                if (ListSereServ == null)
                {
                    return null;
                }
                CommonParam paramGet = new CommonParam();
                var listTreatmentId = ListSereServ.Where(o =>o.TDL_TREATMENT_ID.HasValue).Select(p => p.TDL_TREATMENT_ID.Value).Distinct().ToList();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                    filterSr.TREATMENT_IDs = listIDs;
                    //filterSr.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    //filterSr.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                    filterSr.HAS_EXECUTE = true;
                    var listServiceReqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                    if (IsNotNullOrEmpty(listServiceReqSub))
                    {
                        result.AddRange(listServiceReqSub);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<HIS_SERVICE_REQ> serviceReqs, List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServHein = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var treatment in ListTreatment)
                        {
                            dicTreatment[treatment.ID] = treatment;
                        }
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var sere in ListSereServ)
                        {
                            if (sere.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.AMOUNT > 0 && sere.HEIN_APPROVAL_ID.HasValue)
                            {
                                if (!dicSereServHein.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                                    dicSereServHein[sere.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServHein[sere.HEIN_APPROVAL_ID.Value].Add(sere);
                            }
                        }
                    }

                    string keySplitSr = "_{0}";
                    if (castFilter.IS_MERGE_TREATMENT != true)
                    {
                        keySplitSr += "_{1}_{2}";
                    }
                    if (castFilter.IS_SPLIT_DEPA == true)
                    {
                        keySplitSr += "_{3}";
                    }

                    //khi có điều kiện lọc từ template thì đổi sang key từ template
                    if (this.dicDataFilter.ContainsKey("KEY_SPLIT_SR") && this.dicDataFilter["KEY_SPLIT_SR"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_SPLIT_SR"].ToString()))
                    {
                        keySplitSr = this.dicDataFilter["KEY_SPLIT_SR"].ToString();
                    }

                    hisHeinApprovals = hisHeinApprovals.Where(o => dicTreatment.ContainsKey(o.TREATMENT_ID)).ToList();
                    foreach (var heinApproval in hisHeinApprovals.OrderByDescending(o => o.HEIN_CARD_TO_TIME).ToList())
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                        {
                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                            if (dicSereServHein.ContainsKey(heinApproval.ID))
                            {
                                var sereServSub = dicSereServHein.ContainsKey(heinApproval.ID) ? dicSereServHein[heinApproval.ID] : new List<V_HIS_SERE_SERV_3>();
                                var serviceReqIds = sereServSub.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                                var serviceReqSub = serviceReqs.Where(o => serviceReqIds.Contains(o.ID)).ToList();
                                var examServiceReqs = serviceReqs.Where(o =>o.TREATMENT_ID == heinApproval.TREATMENT_ID && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                                foreach (var sr in serviceReqSub)
                                {
                                    var ssSub = sereServSub.Where(o => sr.ID == o.SERVICE_REQ_ID).ToList();
                                    string key = string.Format(keySplitSr, heinApproval.TREATMENT_CODE, heinApproval.HEIN_CARD_NUMBER, heinApproval.HEIN_CARD_TO_TIME, sr.REQUEST_DEPARTMENT_ID);
                                    if (IsInFilterMediOrgAccept(castFilter.ACCEPT_HEIN_MEDI_ORG_CODE, heinApproval.HEIN_MEDI_ORG_CODE)
                                    && (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(heinApproval.HEIN_MEDI_ORG_CODE)
                                        && checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                                    {
                                        if (dicRdoA.ContainsKey(key))
                                        {
                                            ProcessTotalPrice(dicRdoA[key], ssSub);
                                        }
                                        else
                                        {
                                            dicRdoA.Add(key,ProcessRdoAndTotalPrice(ssSub, sr, heinApproval, dicTreatment[heinApproval.TREATMENT_ID], examServiceReqs));
                                        }
                                    }
                                    else if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                                    {
                                        if (dicRdoB.ContainsKey(key))
                                        {
                                            ProcessTotalPrice(dicRdoB[key], ssSub);
                                        }
                                        else
                                        {
                                            dicRdoB.Add(key,ProcessRdoAndTotalPrice(ssSub, sr, heinApproval, dicTreatment[heinApproval.TREATMENT_ID], examServiceReqs));

                                        }
                                    }
                                    else
                                    {
                                        if (dicRdoC.ContainsKey(key))
                                        {
                                            ProcessTotalPrice(dicRdoC[key], ssSub);
                                        }
                                        else
                                        {
                                            dicRdoC.Add(key,ProcessRdoAndTotalPrice(ssSub, sr, heinApproval, dicTreatment[heinApproval.TREATMENT_ID], examServiceReqs));

                                        }
                                    }

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

        private Mrs00021RDO ProcessRdoAndTotalPrice(List<V_HIS_SERE_SERV_3> ssSub, HIS_SERVICE_REQ sr, V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment, List<HIS_SERVICE_REQ> examServiceReqs)
        {
            Mrs00021RDO rdo = new Mrs00021RDO(heinApproval);
            rdo.SENDER_HEIN_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
            rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
            rdo.ICD_SUB_CODE = treatment.ICD_SUB_CODE;
            rdo.ICD_CODE_EXTRA = treatment.ICD_SUB_CODE;
            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
            rdo.HOSPITALIZATION_REASON = treatment.HOSPITALIZATION_REASON;
            rdo.TDL_HEIN_MEDI_ORG_NAME = treatment.TDL_HEIN_MEDI_ORG_NAME;
            rdo.TDL_PATIENT_GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
            rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
            rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
            rdo.HEIN_CARD_FROM_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(heinApproval.HEIN_CARD_FROM_TIME);
            rdo.HEIN_CARD_TO_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(heinApproval.HEIN_CARD_TO_TIME);
            rdo.HEIN_ADDRESS = heinApproval.ADDRESS;
            //khoa kết thúc
            rdo.END_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID ?? 0;
            rdo.END_DEPARTMENT_CODE = treatment.END_DEPARTMENT_CODE;
            rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;

            //phòng kết thúc
            rdo.END_ROOM_NAME = treatment.END_ROOM_NAME;
            rdo.END_ROOM_CODE = treatment.END_ROOM_CODE;
            rdo.END_ROOM_ID = treatment.END_ROOM_ID;

            //ngày duyệt giám định
            rdo.EXECUTE_DATE = (heinApproval.EXECUTE_TIME ?? 0) - (heinApproval.EXECUTE_TIME ?? 0) % 1000000;

            rdo.END_LOGINNAME = treatment.END_LOGINNAME;
            rdo.END_USERNAME = treatment.END_USERNAME;

            if (treatment.END_DEPARTMENT_ID.HasValue)
            {
                var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                if (departmentCodeBHYT != null)
                {
                    rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                    rdo.DEPARTMENT_NAME = departmentCodeBHYT.DEPARTMENT_NAME;
                }
            }

            if (heinApproval.EXECUTE_TIME.HasValue)
            {
                rdo.INSURANCE_YEAR = Convert.ToInt64(heinApproval.EXECUTE_TIME.ToString().Substring(0, 4));
                rdo.INSURANCE_MONTH = Convert.ToInt64(heinApproval.EXECUTE_TIME.ToString().Substring(4, 2));
            }

            rdo.CURRENT_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
            rdo.RIGHT_ROUTE_CODE = string.IsNullOrWhiteSpace(heinApproval.RIGHT_ROUTE_TYPE_CODE) ? heinApproval.RIGHT_ROUTE_CODE : MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE;
            if (rdo.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
            {
                rdo.RIGHT_ROUTE_NAME = "Đúng tuyến";
            }
            if (rdo.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
            {
                rdo.RIGHT_ROUTE_NAME = "Trái tuyến";
            }
            if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
            {
                if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                {
                    rdo.REASON_INPUT_CODE = "2";
                }
                else
                {
                    rdo.REASON_INPUT_CODE = "1";
                }
            }
            else if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
            {
                rdo.REASON_INPUT_CODE = "3";
            }

            if (treatment.OUT_TIME.HasValue)
            {
                rdo.MAIN_DAY = CountDay(treatment);
            }

            if (treatment.OUT_TIME.HasValue && treatment.CLINICAL_IN_TIME.HasValue)
            {
                rdo.OPEN_TIME = treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                rdo.CLOSE_TIME = treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                rdo.TOTAL_DATE = CountDay(treatment);
            }
            else if (treatment.CLINICAL_IN_TIME.HasValue)
            {
                rdo.OPEN_TIME = treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
            }

            if (examServiceReqs != null)
            {

                ProcessPatientCase(rdo, examServiceReqs,  treatment.IN_ROOM_ID);
            }
            rdo.HEIN_RATIO = ssSub.First().HEIN_RATIO;
            var requestDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sr.REQUEST_DEPARTMENT_ID);
            if (requestDepartment != null)
            {
                rdo.REQUEST_DEPARTMENT_CODE = requestDepartment.DEPARTMENT_CODE;
                rdo.REQUEST_DEPARTMENT_NAME = requestDepartment.DEPARTMENT_NAME;
            }
            rdo.HEIN_RATIO = ssSub.First().HEIN_RATIO;
            ProcessTotalPrice(rdo, ssSub);
            return rdo;
        }

        private void ProcessPatientCase(Mrs00021RDO rdo, List<HIS_SERVICE_REQ> serviceReqs, long? inRoomId)
        {
            HIS_SERVICE_REQ sr = null;

            if (inRoomId != null)
            {
                sr = serviceReqs.FirstOrDefault(o => o.EXECUTE_ROOM_ID == inRoomId && o.PATIENT_CASE_ID.HasValue);
            }
            if (sr == null)
            {
                sr = serviceReqs.OrderByDescending(p => p.FINISH_TIME).FirstOrDefault(o => o.PATIENT_CASE_ID.HasValue);
            }
            if (sr != null)
            {
                if (ListPatientCase != null)
                {
                    var pc = ListPatientCase.FirstOrDefault(o => o.ID == sr.PATIENT_CASE_ID);
                    if (pc != null)
                    {
                        rdo.PATIENT_CASE_NAME = pc.PATIENT_CASE_NAME;
                    }
                }
            }
        }

        private long CountDay(V_HIS_TREATMENT treatment)
        {
            long result = 0;
            try
            {
                PropertyInfo p = typeof(V_HIS_TREATMENT).GetProperty("TREATMENT_DAY_COUNT");
                if (p != null)
                {
                    var DayCount = p.GetValue(treatment);
                    if (DayCount != null)
                    {
                        result = Int64.Parse(DayCount.ToString());
                    }
                    else
                    {
                        result = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                    }
                }
                else
                {
                    result = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = 0;
            }
            return result;
        }

        private void ProcessTotalPrice(Mrs00021RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (castFilter.DEPARTMENT_ID != null)
                    {
                        if ((sereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH && sereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT && sereServ.TDL_REQUEST_DEPARTMENT_ID != castFilter.DEPARTMENT_ID)
                            ||
                            ((sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT) && sereServ.TDL_EXECUTE_DEPARTMENT_ID != castFilter.DEPARTMENT_ID))
                        {
                            continue;
                        }
                    }

                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits, sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");

                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            rdo.TEST_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            rdo.DIIM_PRICE += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                rdo.FUEX_PRICE += TotalPriceTreatment;
                            }
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                        {
                            rdo.MEDICINE_PRICE += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                            {
                                rdo.MEDICINE_PRICE_NDM += TotalPriceTreatment;
                            }
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                        {
                            rdo.BLOOD_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                        {
                            rdo.SURGMISU_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM)
                        {
                            rdo.MATERIAL_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                        {
                            rdo.MATERIAL_PRICE_RATIO += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                            {
                                rdo.MATERIAL_PRICE_RATIO_TT += TotalPriceTreatment;
                            }
                        }

                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                        {
                            rdo.MEDICINE_PRICE_RATIO += TotalPriceTreatment;
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                            {
                                rdo.MEDICINE_PRICE_RATIO_UT += TotalPriceTreatment;
                            }
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            rdo.BED_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            rdo.SERVICE_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            rdo.TRAN_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)
                        {
                            rdo.TT_PRICE += TotalPriceTreatment;
                        }

                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_PATIENT_PRICE_BHYT += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;

                    }
                }
                TotalAmount += rdo.TOTAL_HEIN_PRICE;

                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckPrice(Mrs00021RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 || rdo.FUEX_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TRAN_PRICE > 0 || rdo.TT_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsInFilterMediOrgAccept(string filterAccept, string HeinMediOrgCode)
        {
            if (string.IsNullOrWhiteSpace(filterAccept)) return true;
            if (filterAccept.Contains(HeinMediOrgCode ?? "")) return true;
            return false;
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

        private bool checkBhytNsd(Mrs00021RDO rdo)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
                }

                ProcessSumTotal();
                ProcessSumDepartment();
                ProcessSumRoom();

                objectTag.AddObjectData(store, "Department", Departments);
                objectTag.AddObjectData(store, "Rooms", ListRoom.Where(p => p.TOTAL_PRICE > 0).ToList());
                objectTag.AddRelationship(store, "Department", "Rooms", "END_DEPARTMENT_ID", "END_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                objectTag.AddObjectData(store, "Report", ListAll);
                objectTag.AddObjectData(store, "PatientTypes", ListAll);

                objectTag.AddObjectData(store, "SumTotals", ListSumTotal);
                objectTag.AddObjectData(store, "ReportTreatmentA", ListRdoA.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.AddObjectData(store, "ReportTreatmentB", ListRdoB.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.AddObjectData(store, "ReportTreatmentC", ListRdoC.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.AddObjectData(store, "ReportTreatmentAll", ListAll.GroupBy(o => o.TREATMENT_CODE).Select(p => TotalTreatment(p.ToList())).ToList());
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Mrs00021RDO TotalTreatment(List<Mrs00021RDO> list)
        {
            Mrs00021RDO result = new Mrs00021RDO();
            try
            {
                //result = list.First();
                //map chu khong gan bang vi se lam thay doi gia tri trong danh sach
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00021RDO>(result, list.First());
                result.HEIN_CARD_NUMBER = string.Join(",", list.Select(o => o.HEIN_CARD_NUMBER).Distinct().ToList());
                result.TOTAL_PRICE = list.Sum(s => s.TOTAL_PRICE);///////.Sum(s => s.TOTAL_PRICE) + ListRdoC.Sum(s => s.TOTAL_PRICE));
                result.TEST_PRICE = list.Sum(s => s.TEST_PRICE);///////.Sum(s => s.TEST_PRICE) + ListRdoC.Sum(s => s.TEST_PRICE));
                result.FUEX_PRICE = list.Sum(s => s.FUEX_PRICE);///////.Sum(s => s.TEST_PRICE) + ListRdoC.Sum(s => s.TEST_PRICE));
                result.DIIM_PRICE = list.Sum(s => s.DIIM_PRICE);///////.Sum(s => s.DIIM_PRICE) + ListRdoC.Sum(s => s.DIIM_PRICE));
                result.MEDICINE_PRICE = list.Sum(s => s.MEDICINE_PRICE);///////.Sum(s => s.MEDICINE_PRICE) + ListRdoC.Sum(s => s.MEDICINE_PRICE));
                result.BLOOD_PRICE = list.Sum(s => s.BLOOD_PRICE);///////.Sum(s => s.BLOOD_PRICE) + ListRdoC.Sum(s => s.BLOOD_PRICE));
                result.SURGMISU_PRICE = list.Sum(s => s.SURGMISU_PRICE);///////.Sum(s => s.SURGMISU_PRICE) + ListRdoC.Sum(s => s.SURGMISU_PRICE));
                result.MATERIAL_PRICE = list.Sum(s => s.MATERIAL_PRICE);///////.Sum(s => s.MATERIAL_PRICE) + ListRdoC.Sum(s => s.MATERIAL_PRICE));
                result.SERVICE_PRICE_RATIO = list.Sum(s => s.SERVICE_PRICE_RATIO);///////.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Sum(s => s.SERVICE_PRICE_RATIO));
                result.MEDICINE_PRICE_RATIO = list.Sum(s => s.MEDICINE_PRICE_RATIO);///////.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO));
                result.MATERIAL_PRICE_RATIO = list.Sum(s => s.MATERIAL_PRICE_RATIO);///////.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO));
                result.EXAM_PRICE = list.Sum(s => s.EXAM_PRICE);///////.Sum(s => s.EXAM_PRICE) + ListRdoC.Sum(s => s.EXAM_PRICE));
                result.BED_PRICE = list.Sum(s => s.BED_PRICE);///////.Sum(s => s.BED_PRICE) + ListRdoC.Sum(s => s.BED_PRICE));
                result.TRAN_PRICE = list.Sum(s => s.TRAN_PRICE);///////.Sum(s => s.TRAN_PRICE) + ListRdoC.Sum(s => s.TRAN_PRICE));
                result.TT_PRICE = list.Sum(s => s.TT_PRICE);///////.Sum(s => s.TRAN_PRICE) + ListRdoC.Sum(s => s.TRAN_PRICE));
                result.TOTAL_PATIENT_PRICE = list.Sum(s => s.TOTAL_PATIENT_PRICE);///////.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                result.TOTAL_HEIN_PRICE = list.Sum(s => s.TOTAL_HEIN_PRICE);///////.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                result.TOTAL_HEIN_PRICE_NDS = list.Sum(s => s.TOTAL_HEIN_PRICE_NDS);///////.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                result.TOTAL_OTHER_SOURCE_PRICE = list.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
                result.MEDICINE_PRICE_RATIO_UT = list.Sum(x => x.MEDICINE_PRICE_RATIO_UT);
                result.MATERIAL_PRICE_RATIO_TT = list.Sum(x => x.MATERIAL_PRICE_RATIO_TT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new Mrs00021RDO();
            }
            return result;
        }

        private void ProcessSumTotal()
        {
            try
            {
                Mrs00021RDO rdo = new Mrs00021RDO();
                rdo.TOTAL_PRICE = (ListRdoA.Sum(s => s.TOTAL_PRICE) + ListRdoB.Sum(s => s.TOTAL_PRICE) + ListRdoC.Sum(s => s.TOTAL_PRICE));
                rdo.FUEX_PRICE = (ListRdoA.Sum(s => s.FUEX_PRICE) + ListRdoB.Sum(s => s.FUEX_PRICE) + ListRdoC.Sum(s => s.FUEX_PRICE));
                rdo.TEST_PRICE = (ListRdoA.Sum(s => s.TEST_PRICE) + ListRdoB.Sum(s => s.TEST_PRICE) + ListRdoC.Sum(s => s.TEST_PRICE));
                rdo.DIIM_PRICE = (ListRdoA.Sum(s => s.DIIM_PRICE) + ListRdoB.Sum(s => s.DIIM_PRICE) + ListRdoC.Sum(s => s.DIIM_PRICE));
                rdo.MEDICINE_PRICE = (ListRdoA.Sum(s => s.MEDICINE_PRICE) + ListRdoB.Sum(s => s.MEDICINE_PRICE) + ListRdoC.Sum(s => s.MEDICINE_PRICE));
                rdo.BLOOD_PRICE = (ListRdoA.Sum(s => s.BLOOD_PRICE) + ListRdoB.Sum(s => s.BLOOD_PRICE) + ListRdoC.Sum(s => s.BLOOD_PRICE));
                rdo.SURGMISU_PRICE = (ListRdoA.Sum(s => s.SURGMISU_PRICE) + ListRdoB.Sum(s => s.SURGMISU_PRICE) + ListRdoC.Sum(s => s.SURGMISU_PRICE));
                rdo.MATERIAL_PRICE = (ListRdoA.Sum(s => s.MATERIAL_PRICE) + ListRdoB.Sum(s => s.MATERIAL_PRICE) + ListRdoC.Sum(s => s.MATERIAL_PRICE));
                rdo.SERVICE_PRICE_RATIO = (ListRdoA.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoB.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Sum(s => s.SERVICE_PRICE_RATIO));
                rdo.MEDICINE_PRICE_RATIO = (ListRdoA.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoB.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO));
                rdo.MATERIAL_PRICE_RATIO = (ListRdoA.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoB.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO));
                rdo.MEDICINE_PRICE_RATIO_UT = (ListRdoA.Sum(s => s.MEDICINE_PRICE_RATIO_UT) + ListRdoB.Sum(s => s.MEDICINE_PRICE_RATIO_UT) + ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO_UT));
                rdo.MATERIAL_PRICE_RATIO_TT = (ListRdoA.Sum(s => s.MATERIAL_PRICE_RATIO_TT) + ListRdoB.Sum(s => s.MATERIAL_PRICE_RATIO_TT) + ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO_TT));
                rdo.BED_PRICE = (ListRdoA.Sum(s => s.BED_PRICE) + ListRdoB.Sum(s => s.BED_PRICE) + ListRdoC.Sum(s => s.BED_PRICE));
                rdo.EXAM_PRICE = (ListRdoA.Sum(s => s.EXAM_PRICE) + ListRdoB.Sum(s => s.EXAM_PRICE) + ListRdoC.Sum(s => s.EXAM_PRICE));
                rdo.TRAN_PRICE = (ListRdoA.Sum(s => s.TRAN_PRICE) + ListRdoB.Sum(s => s.TRAN_PRICE) + ListRdoC.Sum(s => s.TRAN_PRICE));
                rdo.TT_PRICE = (ListRdoA.Sum(s => s.TT_PRICE) + ListRdoB.Sum(s => s.TT_PRICE) + ListRdoC.Sum(s => s.TT_PRICE));
                rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                rdo.TOTAL_HEIN_PRICE = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoA.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoB.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoC.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                ListSumTotal.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessSumDepartment()
        {
            try
            {
                List<long> departmentId__Kkb = HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM;
                if (departmentId__Kkb == null || departmentId__Kkb.Count == 0)
                {
                    departmentId__Kkb = HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB;
                }
                foreach (var department in HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList())
                {
                    Mrs00021RDO rdo = new Mrs00021RDO();
                    rdo.END_DEPARTMENT_ID = department.ID;
                    rdo.END_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                    rdo.END_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                    rdo.COUNT_TREATMENT = ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Count() + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Count() + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Count();
                    rdo.TOTAL_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_PRICE));
                    rdo.TEST_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TEST_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TEST_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TEST_PRICE));
                    rdo.FUEX_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.FUEX_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.FUEX_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.FUEX_PRICE));
                    rdo.DIIM_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.DIIM_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.DIIM_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.DIIM_PRICE));
                    rdo.MEDICINE_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MEDICINE_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MEDICINE_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MEDICINE_PRICE));
                    rdo.BLOOD_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.BLOOD_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.BLOOD_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.BLOOD_PRICE));
                    rdo.SURGMISU_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.SURGMISU_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.SURGMISU_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.SURGMISU_PRICE));
                    rdo.MATERIAL_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MATERIAL_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MATERIAL_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MATERIAL_PRICE));
                    rdo.SERVICE_PRICE_RATIO = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.SERVICE_PRICE_RATIO));
                    rdo.MEDICINE_PRICE_RATIO = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MEDICINE_PRICE_RATIO));
                    rdo.MATERIAL_PRICE_RATIO = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.MATERIAL_PRICE_RATIO));
                    rdo.BED_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.BED_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.BED_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.BED_PRICE));
                    rdo.EXAM_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.EXAM_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.EXAM_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.EXAM_PRICE));
                    rdo.TRAN_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TRAN_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TRAN_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TRAN_PRICE));
                    rdo.TT_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TT_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TT_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TT_PRICE));
                    rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_PATIENT_PRICE));
                    rdo.TOTAL_HEIN_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_HEIN_PRICE));
                    rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                    rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoA.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoB.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoC.Where(o => o.END_DEPARTMENT_ID == department.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                    if (departmentId__Kkb != null && departmentId__Kkb.Contains(department.ID))
                    {
                        continue;
                    }
                    //if (rdo.TOTAL_PRICE > 0)
                    {
                        Departments.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private void ProcessSumRoom()
        {
            try
            {
                foreach (var Rooms in HisRoomCFG.HisRooms.ToList())
                {
                    Mrs00021RDO rdo = new Mrs00021RDO();
                    rdo.END_DEPARTMENT_ID = Rooms.DEPARTMENT_ID;
                    rdo.END_ROOM_ID = Rooms.ID;
                    rdo.END_ROOM_NAME = Rooms.ROOM_NAME;
                    rdo.END_ROOM_CODE = Rooms.ROOM_CODE;
                    rdo.COUNT_TREATMENT = ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Count() + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Count() + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Count();
                    rdo.TOTAL_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_PRICE));
                    rdo.TEST_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TEST_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TEST_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TEST_PRICE));
                    rdo.FUEX_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.FUEX_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.FUEX_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.FUEX_PRICE));
                    rdo.DIIM_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.DIIM_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.DIIM_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.DIIM_PRICE));
                    rdo.MEDICINE_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MEDICINE_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MEDICINE_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MEDICINE_PRICE));
                    rdo.BLOOD_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.BLOOD_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.BLOOD_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.BLOOD_PRICE));
                    rdo.SURGMISU_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.SURGMISU_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.SURGMISU_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.SURGMISU_PRICE));
                    rdo.MATERIAL_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MATERIAL_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MATERIAL_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MATERIAL_PRICE));
                    rdo.SERVICE_PRICE_RATIO = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.SERVICE_PRICE_RATIO));
                    rdo.MEDICINE_PRICE_RATIO = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MEDICINE_PRICE_RATIO));
                    rdo.MATERIAL_PRICE_RATIO = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.MATERIAL_PRICE_RATIO));
                    rdo.BED_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.BED_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.BED_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.BED_PRICE));
                    rdo.EXAM_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.EXAM_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.EXAM_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.EXAM_PRICE));
                    rdo.TRAN_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TRAN_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TRAN_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TRAN_PRICE));
                    rdo.TT_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TT_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TT_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TT_PRICE));
                    rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_PATIENT_PRICE));
                    rdo.TOTAL_HEIN_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_HEIN_PRICE));
                    rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                    rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoA.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoB.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoC.Where(o => o.END_ROOM_ID == Rooms.ID).Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                    {
                        ListRoom.Add(rdo);
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }


        }

        class RDOCustomerFuncRownumberData : FlexCel.Report.TFlexCelUserFunction
        {
            public RDOCustomerFuncRownumberData()
            {
            }
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length < 1)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                long result = 0;
                try
                {
                    long rownumber = Convert.ToInt64(parameters[0]);
                    result = (rownumber + 1);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }

                return result;
            }
        }
    }
}
