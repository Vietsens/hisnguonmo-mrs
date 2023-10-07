using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisOtherPaySource;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00018
{
    class Mrs00018Processor : AbstractProcessor
    {
        Mrs00018Filter castFilter = null;

        List<Mrs00018RDO> ListRdoOxy = new List<Mrs00018RDO>();
        List<Mrs00018RDO> ListRdoVcm = new List<Mrs00018RDO>();
        List<Mrs00018RDO> ListRdoGeneric = new List<Mrs00018RDO>();
        List<Mrs00018RDO> ListRdoCpyhct = new List<Mrs00018RDO>();
        List<Mrs00018RDO> ListRdoVtyhct = new List<Mrs00018RDO>();
        List<Mrs00018RDO> ListRdoOther = new List<Mrs00018RDO>();
        List<Mrs00018RDO> ListDetail = new List<Mrs00018RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApprovalBhyt = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_SERVICE_RETY_CAT> LisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<string, HIS_BID_MEDICINE_TYPE> dicBidMedicineType = new Dictionary<string, HIS_BID_MEDICINE_TYPE>();
        Dictionary<long, List<HIS_BID>> dicBid = new Dictionary<long, List<HIS_BID>>();
        List<HIS_MEDICINE> ListMedicine = new List<HIS_MEDICINE>();
        List<long> listHeinServiceTypeId;
        HIS_BRANCH _Branch = null;
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        List<long> OtherPaySourceIds = new List<long>();

        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        private const string RouteCodeC = "C";
        private const string Taivien = "TAIVIEN";
        private const string NoiTinh = "NOI";
        private const string NgoaiTinh = "NGOAI";
        short IS_TRUE = 1;

        private decimal TotalAmount = 0;
        public Mrs00018Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00018Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00018Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("bat dau lay du lieu MRS00018: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.castFilter), this.castFilter));
                CommonParam paramGet = new CommonParam();

                if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;

                    ListTreatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                    if (castFilter.TDL_PATIENT_TYPE_IDs != null)
                        ListTreatment = ListTreatment.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();

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
                    approvalFilter.ORDER_DIRECTION = "ACS";
                    ListHeinApprovalBhyt = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                    ListTreatment = new List<HIS_TREATMENT>();
                    List<long> TreatmentIds = ListHeinApprovalBhyt.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    int skip = 0;
                    while (TreatmentIds.Count - skip > 0)
                    {
                        var listId = TreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
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
                    if (castFilter.PATIENT_TYPE_IDs != null)
                        ListTreatment = ListTreatment.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                    ListHeinApprovalBhyt = ListHeinApprovalBhyt.Where(o => ListTreatment.Exists(e => e.ID == o.TREATMENT_ID)).ToList();
                }

                if (castFilter.CATEGORY_CODE__OXY != null || castFilter.CATEGORY_CODE__VCM != null)
                {
                    HisServiceRetyCatViewFilterQuery HisServiceRetyCatViewfilter = new HisServiceRetyCatViewFilterQuery();
                    HisServiceRetyCatViewfilter.REPORT_TYPE_CODE__EXACT = "MRS00018";
                    LisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(HisServiceRetyCatViewfilter);
                }

                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
                dicBidMedicineType = new HisBidMedicineTypeManager().Get(new HisBidMedicineTypeFilterQuery()).GroupBy(o => (o.MEDICINE_TYPE_ID + "_" + o.BID_ID)).ToDictionary(p => p.Key, p => p.First());

                dicBid = new HisBidManager(paramGet).Get(new HisBidFilterQuery()).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.ToList());

                //ListMedicine = new ManagerSql(paramGet).GetMedicine(castFilter);

                //get nguồn khác
                GetOtherPaySource();

                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DOAGET trong qua trinh lay du lieu MRS00018");
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
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT
                    };

                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM);
                    }
                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }

                    int start = 0;
                    int count = ListHeinApprovalBhyt.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts = ListHeinApprovalBhyt.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = heinApprovalBhyts.Select(s => s.ID).ToList();
                        var ListSereServHein = new HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (castFilter.SERVICE_TYPE_ID != null && castFilter.SERVICE_TYPE_ID != 0)
                        {
                            ListSereServHein = ListSereServHein.Where(x => x.TDL_SERVICE_TYPE_ID == castFilter.SERVICE_TYPE_ID).ToList();
                        }
                        if (castFilter.EXECUTE_DEPARTMENT_IDs != null)
                        {
                            ListSereServHein = ListSereServHein.Where(x => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(x.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            ListSereServHein = ListSereServHein.Where(x => castFilter.PATIENT_TYPE_IDs.Contains(x.PATIENT_TYPE_ID)).ToList();
                        }
                        //if (ListSereServHein != null)
                        //{
                        //    ListSereServHein = ListSereServHein.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        //}
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00018");
                        }

                        if (!IsNotNullOrEmpty(ListTreatment))
                        {
                            HisTreatmentFilterQuery TreatmentFilter = new HisTreatmentFilterQuery();
                            TreatmentFilter.IDs = heinApprovalBhyts.Select(s => s.TREATMENT_ID).Distinct().ToList();
                            ListTreatment = new HisTreatmentManager(paramGet).Get(TreatmentFilter);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00018");
                        }
                        if (castFilter.MEDICINE_TYPE_IDs != null)
                        {
                            ListSereServHein = ListSereServHein.Where(o => castFilter.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID ?? 0)).ToList();
                            var heinApprovalIds = ListSereServHein.Select(o => o.HEIN_APPROVAL_ID ?? 0).Distinct().ToList();
                            heinApprovalBhyts = heinApprovalBhyts.Where(o => heinApprovalIds.Contains(o.ID)).ToList();
                        }
                        ProcessListHeinApprovalDetail(heinApprovalBhyts, ListSereServHein, ListTreatment);
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
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetOtherPaySource()
        {
            var listOtherPaySource = new HisOtherPaySourceManager().Get(new HisOtherPaySourceFilterQuery()) ?? new List<HIS_OTHER_PAY_SOURCE>();
            if (!string.IsNullOrWhiteSpace(castFilter.OTHER_PAY_SOURCE_CODE_ALLOWS))
            {
                listOtherPaySource = listOtherPaySource.Where(o => ("," + castFilter.OTHER_PAY_SOURCE_CODE_ALLOWS + ",").Contains("," + o.OTHER_PAY_SOURCE_CODE + ",")).ToList();
                OtherPaySourceIds = listOtherPaySource.Select(o => o.ID).ToList();
            }
        }

        private List<HIS_MEDICINE> GetMedicine(List<V_HIS_SERE_SERV_3> ListSereServHein)
        {
            List<HIS_MEDICINE> result = new List<HIS_MEDICINE>();
            var materialIds = ListSereServHein.Select(o => o.MEDICINE_ID ?? 0).Distinct().ToList();

            if (materialIds != null && materialIds.Count > 0)
            {
                var skip = 0;
                while (materialIds.Count - skip > 0)
                {
                    var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                    Medicinefilter.IDs = limit;
                    var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                    if (MedicineSub != null)
                    {
                        result.AddRange(MedicineSub);
                    }
                }
            }
            return result;
        }
        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServHein, List<HIS_TREATMENT> listTreatment)
        {
            Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

            if (IsNotNullOrEmpty(heinApprovalBhyts))
            {
                foreach (var item in heinApprovalBhyts)
                {
                    dicHeinApproval[item.ID] = item;
                }
            }
            ListMedicine = GetMedicine(ListSereServHein);
            if (IsNotNullOrEmpty(ListSereServHein))
            {
                Inventec.Common.Logging.LogSystem.Info("ssCount:" + ListSereServHein.Count);
                foreach (var item in ListSereServHein)
                {

                    

                    if (!(item.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && item.AMOUNT > 0 && item.HEIN_APPROVAL_ID.HasValue && item.TDL_HEIN_SERVICE_TYPE_ID.HasValue && item.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE))
                    {
                        Inventec.Common.Logging.LogSystem.Info("ssId:" + item.ID);
                        continue;
                    }
                    if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                    {
                        //trường hợp không có tiền bảo hiểm chi trả:
                        if (!item.VIR_TOTAL_HEIN_PRICE.HasValue || item.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        {
                            if (item.OTHER_PAY_SOURCE_ID != null && OtherPaySourceIds.Contains(item.OTHER_PAY_SOURCE_ID ?? 0))
                            {
                                //totalOtherSourcePrice = (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Info("ssId0:" + item.ID);
                                continue;
                            }

                        }
                        var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                        var treatment = listTreatment.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID);
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                        Mrs00018RDO rdo = new Mrs00018RDO();
                        rdo.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
                        rdo.HEIN_APPROVAL_CODE = heinApproval.HEIN_APPROVAL_CODE;
                        rdo.HEIN_CARD_NUMBER = item.HEIN_CARD_NUMBER;
                        rdo.INTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_INTRUCTION_TIME);
                        rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_INTRUCTION_TIME);
                        rdo.TDL_SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo.AX_CODE = item.TDL_ACTIVE_INGR_BHYT_CODE;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                        rdo.AMOUNT_CC = 0;
                        rdo.AMOUNT_TNT = 0;
                        rdo.CATEGORY_CODE = null;
                        rdo.CATEGORY_NAME = null;
                        if (treatment != null)
                        {
                            rdo.END_CODE = treatment.END_CODE;
                            rdo.HEIN_MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
                            rdo.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                            rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                            rdo.ICD_NAME = treatment.ICD_NAME;
                        }

                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.MEDICINE_SODANGKY_NAME = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.REGISTER_NUMBER = item.REGISTER_NUMBER;
                        rdo.MEDICINE_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.MEDICINE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MEDICINE_HAMLUONG_NAME = item.MEDICINE_TYPE_CONCENTRA;
                        rdo.MEDICINE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        if (castFilter.IS_BID_NUM_ORDER_MERGE != true)
                        {
                            rdo.BID_NUM_ORDER = dicBidMedicineType.ContainsKey((item.MEDICINE_TYPE_ID ?? 0) + "_" + (item.MEDICINE_BID_ID ?? 0)) ? dicBidMedicineType[(item.MEDICINE_TYPE_ID ?? 0) + "_" + (item.MEDICINE_BID_ID ?? 0)].BID_NUM_ORDER : item.MEDICINE_BID_NUM_ORDER ?? item.TDL_MEDICINE_BID_NUM_ORDER ?? ((ListMedicine ?? new List<HIS_MEDICINE>()).FirstOrDefault(o => o.ID == item.MEDICINE_ID) ?? new HIS_MEDICINE()).TDL_BID_NUM_ORDER;
                        }

                        rdo.BID_NUMBER = dicBid.ContainsKey(item.MEDICINE_BID_ID ?? 0) ? dicBid[item.MEDICINE_BID_ID ?? 0].First().BID_NUMBER : ((ListMedicine ?? new List<HIS_MEDICINE>()).FirstOrDefault(o => o.ID == item.MEDICINE_ID) ?? new HIS_MEDICINE()).TDL_BID_NUMBER;
                        if (castFilter.IS_ROUTE == true)
                        {
                            if (!String.IsNullOrWhiteSpace(this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.HEIN_MEDI_ORG_CODE))
                            {
                                rdo.ROUTE_CODE = RouteCodeA;
                            }
                            else
                            {
                                rdo.ROUTE_CODE = RouteCodeB;
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinApproval.HEIN_MEDI_ORG_CODE))
                        {
                            rdo.PROVINCE_CHECK = Taivien;
                        }
                        else
                        {
                            if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                            {
                                rdo.PROVINCE_CHECK = NoiTinh;
                            }
                            else
                            {
                                rdo.PROVINCE_CHECK = NgoaiTinh;
                            }
                        }
                        if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                        {
                            rdo.PROVINCE_TYPE = NoiTinh;
                        }
                        else
                        {
                            rdo.PROVINCE_TYPE = NgoaiTinh;
                        }
                        rdo.RIGHT_ROUTE_CODE = heinApproval.RIGHT_ROUTE_CODE;

                        rdo.PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);

                        bool valid = false;
                        if (castFilter.IS_TREAT.HasValue)
                        {
                            if (castFilter.IS_TREAT.Value && heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                valid = true;
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                                rdo.TOTAL_HEIN_PRICE = (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                                rdo.TOTAL_PRICE =  Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                                rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                            }
                            else if (!castFilter.IS_TREAT.Value && heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                valid = true;
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                                rdo.TOTAL_HEIN_PRICE = (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                                rdo.TOTAL_PRICE =  Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH()) ;
                                rdo.TOTAL_OTHER_SOURCE_PRICE =(item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                            }
                        }
                        else
                        {
                            valid = true;
                            if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU =item.AMOUNT;
                            }
                            else if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }

                            rdo.TOTAL_HEIN_PRICE = (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH()) ;
                            rdo.TOTAL_OTHER_SOURCE_PRICE =(item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT ;
                        }
                        if ((item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM) && Config.HisBloodIntoMedicineGenericCFG.BloodIntoMedicine__Generic)
                        {
                            ListRdoGeneric.Add(rdo);
                            ListDetail.Add(rdo);
                        }
                        else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            ListRdoOther.Add(rdo);
                            ListDetail.Add(rdo);
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
                                if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs00018.HIS_MEDICINE_LINE.ID__TTD)
                                {
                                    ListRdoGeneric.Add(rdo);
                                    ListDetail.Add(rdo);
                                }
                                else if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs00018.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                {
                                    ListRdoCpyhct.Add(rdo);
                                    ListDetail.Add(rdo);
                                }
                                else if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs00018.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                {
                                    ListRdoVtyhct.Add(rdo);
                                    ListDetail.Add(rdo);
                                }
                                else
                                {
                                    ListRdoGeneric.Add(rdo);
                                    ListDetail.Add(rdo);
                                }
                            }
                            else
                            {
                                ListRdoOther.Add(rdo);
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

        private List<Mrs00018RDO> ProcessListRDO(List<Mrs00018RDO> listRDO)
        {
            List<Mrs00018RDO> listCurrent = new List<Mrs00018RDO>();
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.BID_NUM_ORDER, o.SERVICE_ID, o.PRICE, o.MEDICINE_SODANGKY_NAME, o.ROUTE_CODE, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00018RDO> listsub = group.ToList<Mrs00018RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00018RDO rdo = new Mrs00018RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            if (LisServiceRetyCat != null)
                            {
                                if (LisServiceRetyCat.Exists(o => o.SERVICE_ID == listsub[0].SERVICE_ID && o.CATEGORY_CODE == castFilter.CATEGORY_CODE__OXY))
                                {
                                    rdo.IS_OXY = true;
                                }
                                if (LisServiceRetyCat.Exists(o => o.SERVICE_ID == listsub[0].SERVICE_ID && o.CATEGORY_CODE == castFilter.CATEGORY_CODE__VCM))
                                {
                                    rdo.IS_VCM = true;
                                }
                            }
                            rdo.ROUTE_CODE = listsub[0].ROUTE_CODE;
                            rdo.RIGHT_ROUTE_CODE = listsub[0].RIGHT_ROUTE_CODE;
                            rdo.PROVINCE_TYPE = listsub[0].PROVINCE_TYPE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.MEDICINE_SODANGKY_NAME = listsub[0].MEDICINE_SODANGKY_NAME;
                            rdo.REGISTER_NUMBER = listsub[0].REGISTER_NUMBER;
                            rdo.MEDICINE_HAMLUONG_NAME = listsub[0].MEDICINE_HAMLUONG_NAME;
                            rdo.MEDICINE_DUONGDUNG_NAME = listsub[0].MEDICINE_DUONGDUNG_NAME;
                            rdo.MEDICINE_HOATCHAT_NAME = listsub[0].MEDICINE_HOATCHAT_NAME;
                            rdo.BID_NUMBER = listsub[0].BID_NUMBER;
                            rdo.BID_NUM_ORDER = listsub[0].BID_NUM_ORDER;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.MEDICINE_UNIT_NAME = listsub[0].MEDICINE_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
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
            return listCurrent.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            if (castFilter.IS_RIGHT_ROUTE != null)
            {
                if (castFilter.IS_RIGHT_ROUTE == true)
                {
                    ListRdoOxy = ListRdoOxy.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListRdoVcm = ListRdoVcm.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListRdoGeneric = ListRdoGeneric.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListRdoCpyhct = ListRdoCpyhct.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListRdoVtyhct = ListRdoVtyhct.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListRdoOther = ListRdoOther.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListDetail = ListDetail.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                }
                if (castFilter.IS_RIGHT_ROUTE == false)
                {
                    ListRdoOxy = ListRdoOxy.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListRdoVcm = ListRdoVcm.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListRdoGeneric = ListRdoGeneric.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListRdoCpyhct = ListRdoCpyhct.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListRdoVtyhct = ListRdoVtyhct.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListRdoOther = ListRdoOther.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListDetail = ListDetail.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                }
            }
            if (castFilter.IS_IN_PROVICE != null)
            {
                if (castFilter.IS_IN_PROVICE == true)
                {
                    ListRdoOxy = ListRdoOxy.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListRdoVcm = ListRdoVcm.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListRdoGeneric = ListRdoGeneric.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListRdoCpyhct = ListRdoCpyhct.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListRdoVtyhct = ListRdoVtyhct.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListRdoOther = ListRdoOther.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListDetail = ListDetail.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                }
                if (castFilter.IS_IN_PROVICE == false)
                {
                    ListRdoOxy = ListRdoOxy.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListRdoVcm = ListRdoVcm.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListRdoGeneric = ListRdoGeneric.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListRdoCpyhct = ListRdoCpyhct.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListRdoVtyhct = ListRdoVtyhct.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListRdoOther = ListRdoOther.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListDetail = ListDetail.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                }
            }
            AddGeneral(dicSingleTag, objectTag, store);
            AddA(dicSingleTag, objectTag, store);
            AddB(dicSingleTag, objectTag, store);
            AddOxy(dicSingleTag, objectTag, store);
            AddVcm(dicSingleTag, objectTag, store);

        }

        private void AddOxy(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            ListRdoOxy.AddRange(ListRdoGeneric.Where(o => o.IS_OXY == true).ToList() ?? new List<Mrs00018RDO>());
            ListRdoOxy.AddRange(ListRdoCpyhct.Where(o => o.IS_OXY == true).ToList() ?? new List<Mrs00018RDO>());
            ListRdoOxy.AddRange(ListRdoVtyhct.Where(o => o.IS_OXY == true).ToList() ?? new List<Mrs00018RDO>());
            ListRdoOxy.AddRange(ListRdoOther.Where(o => o.IS_OXY == true).ToList() ?? new List<Mrs00018RDO>());
            objectTag.AddObjectData(store, "ReportOxy", ListRdoOxy);
            objectTag.AddObjectData(store, "ReportOxyA", ListRdoOxy.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportOxyB", ListRdoOxy.Where(o => o.ROUTE_CODE == "B").ToList());
        }

        private void AddVcm(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            ListRdoVcm.AddRange(ListRdoGeneric.Where(o => o.IS_VCM == true).ToList() ?? new List<Mrs00018RDO>());
            ListRdoVcm.AddRange(ListRdoCpyhct.Where(o => o.IS_VCM == true).ToList() ?? new List<Mrs00018RDO>());
            ListRdoVcm.AddRange(ListRdoVtyhct.Where(o => o.IS_VCM == true).ToList() ?? new List<Mrs00018RDO>());
            ListRdoVcm.AddRange(ListRdoOther.Where(o => o.IS_VCM == true).ToList() ?? new List<Mrs00018RDO>());
            objectTag.AddObjectData(store, "ReportVcm", ListRdoVcm);
            objectTag.AddObjectData(store, "ReportVcmA", ListRdoVcm.Where(o => o.ROUTE_CODE == "A").ToList());
            objectTag.AddObjectData(store, "ReportVcmB", ListRdoVcm.Where(o => o.ROUTE_CODE == "B").ToList());
        }

        private void AddA(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            objectTag.AddObjectData(store, "ReportA1", ListRdoGeneric.Where(o => o.ROUTE_CODE == "A" && o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "ReportA2", ListRdoCpyhct.Where(o => o.ROUTE_CODE == "A" && o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "ReportA3", ListRdoVtyhct.Where(o => o.ROUTE_CODE == "A" && o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "ReportA4", ListRdoOther.Where(o => o.ROUTE_CODE == "A" && o.IS_OXY == false && o.IS_VCM == false).ToList());
        }

        private void AddB(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            objectTag.AddObjectData(store, "ReportB1", ListRdoGeneric.Where(o => o.ROUTE_CODE == "B" && o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "ReportB2", ListRdoCpyhct.Where(o => o.ROUTE_CODE == "B" && o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "ReportB3", ListRdoVtyhct.Where(o => o.ROUTE_CODE == "B" && o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "ReportB4", ListRdoOther.Where(o => o.ROUTE_CODE == "B" && o.IS_OXY == false && o.IS_VCM == false).ToList());
        }

        private void AddGeneral(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
            objectTag.AddObjectData(store, "Report1", ListRdoGeneric.Where(o => o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "Report2", ListRdoCpyhct.Where(o => o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "Report3", ListRdoVtyhct.Where(o => o.IS_OXY == false && o.IS_VCM == false).ToList());
            objectTag.AddObjectData(store, "Report4", ListRdoOther.Where(o => o.IS_OXY == false && o.IS_VCM == false).ToList());

            objectTag.AddObjectData(store, "Detail", ListDetail);

            objectTag.AddObjectData(store, "Detail1", ListDetail);

            objectTag.AddObjectData(store, "Medis", ListDetail.GroupBy(o => o.SERVICE_ID).Select(p => p.First()).ToList());

            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;
        }
        private void SelectSheet(ref Inventec.Common.FlexCellExport.Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {

                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);
                if (castFilter.IS_ROUTE != false)
                {
                    xls.ActiveSheet = 1;
                }
                else
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
