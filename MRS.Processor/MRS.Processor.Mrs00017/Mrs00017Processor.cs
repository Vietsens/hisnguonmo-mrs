using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMaterial;

namespace MRS.Processor.Mrs00017
{
    class Mrs00017Processor : AbstractProcessor
    {
        Mrs00017Filter castFilter = null;
        List<Mrs00017RDO> ListRdo = new List<Mrs00017RDO>();
        List<Mrs00017RDO> ListDetail = new List<Mrs00017RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<string, HIS_BID_MATERIAL_TYPE> dicBidMaterialType = new Dictionary<string, HIS_BID_MATERIAL_TYPE>();
        Dictionary<long, List<HIS_BID>> dicBid = new Dictionary<long, List<HIS_BID>>();
        List<V_HIS_SERVICE_RETY_CAT> LisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_MATERIAL> ListMaterial = new List<HIS_MATERIAL>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<long> listHeinServiceTypeId;
        string MaterialPriceOption = "";

        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        private const string RouteCodeC = "C";
        private const string NoiTinh = "NOI";
        private const string NgoaiTinh = "NGOAI";
        short IS_TRUE = 1;
        HIS_BRANCH _Branch = null;
        private decimal TotalAmount = 0;

        public Mrs00017Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00017Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00017Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00017:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                CommonParam paramGet = new CommonParam();
                if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                    
                    ListTreatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                    if(castFilter.TDL_PATIENT_TYPE_IDs !=null)
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
                    ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                    ListTreatment = new List<HIS_TREATMENT>();
                    List<long> TreatmentIds = ListHeinApproval.Select(o => o.TREATMENT_ID).Distinct().ToList();
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
                    ListHeinApproval = ListHeinApproval.Where(o => ListTreatment.Exists(e => e.ID == o.TREATMENT_ID)).ToList();
                }

                ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
                var listBidMaterialType = new HisBidMaterialTypeManager().Get(new HisBidMaterialTypeFilterQuery());
                if (listBidMaterialType != null)
                {
                    dicBidMaterialType = listBidMaterialType.GroupBy(o => (o.MATERIAL_TYPE_ID + "_" + o.BID_ID)).ToDictionary(p => p.Key, p => p.First());
                }
                dicBid = new HisBidManager(paramGet).Get(new HisBidFilterQuery()).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.ToList());
                if (castFilter.CATEGORY_CODE__DC != null)
                {
                    HisServiceRetyCatViewFilterQuery HisServiceRetyCatViewfilter = new HisServiceRetyCatViewFilterQuery();
                    HisServiceRetyCatViewfilter.REPORT_TYPE_CODE__EXACT = "MRS00017";
                    LisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(HisServiceRetyCatViewfilter);
                }

                //ListMaterial = new ManagerSql(paramGet).GetMaterial(castFilter);
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00017");
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
                    if (IsNotNullOrEmpty(ListMaterialType))
                    {
                        foreach (var material in ListMaterialType)
                        {
                            dicMaterialType[material.ID] = material;
                        }
                    }
                    CommonParam paramGet = new CommonParam();
                    listHeinServiceTypeId = new List<long>()
                        {
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                        };

                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }

                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__VTYT)
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
                        if (castFilter.SERVICE_TYPE_ID != null && castFilter.SERVICE_TYPE_ID != 0)
                        {
                            ListSereServHein = ListSereServHein.Where(x => x.TDL_SERVICE_TYPE_ID == castFilter.SERVICE_TYPE_ID).ToList();
                        }
                        if (castFilter.EXECUTE_DEPARTMENT_IDs!=null)
                        {
                            ListSereServHein = ListSereServHein.Where(x => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(x.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_IDs!=null)
                        {
                            ListSereServHein = ListSereServHein.Where(x => castFilter.PATIENT_TYPE_IDs.Contains(x.PATIENT_TYPE_ID)).ToList();
                        }
                        if (ListSereServHein != null)
                        {
                            ListSereServHein = ListSereServHein.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0 && o.AMOUNT > 0 && o.PRICE > 0 && o.IS_NO_EXECUTE != IS_TRUE && o.IS_EXPEND != IS_TRUE).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00017");
                        }

                        if (!IsNotNullOrEmpty(ListTreatment))
                        {
                            HisTreatmentFilterQuery TreatmentFilter = new HisTreatmentFilterQuery();
                            TreatmentFilter.IDs = heinAppBhyts.Select(s => s.TREATMENT_ID).Distinct().ToList();
                            ListTreatment = new HisTreatmentManager(paramGet).Get(TreatmentFilter);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00017");
                        }

                        ProcessListHeinApprovalDetail(heinAppBhyts, ListSereServHein, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRdo = ProcessListRDO();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo = new List<Mrs00017RDO>();
                result = false;
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
            ListMaterial = GetMaterial(ListSereServHein);
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
                        var treatment = listTreatment.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID);
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID);
                        var serviceUnit = HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == item.TDL_SERVICE_UNIT_ID);
                        Mrs00017RDO rdo = new Mrs00017RDO();
                        rdo.TREATMENT_CODE = heinAproval.TREATMENT_CODE;
                        rdo.HEIN_APPROVAL_CODE = heinAproval.HEIN_APPROVAL_CODE;
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
                        rdo.MATERIAL_CODE_DMBYT = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.MATERIAL_CODE_DMBYT_1 = item.TDL_MATERIAL_GROUP_BHYT;
                        rdo.MATERIAL_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.MATERIAL_TYPE_NAME_BYT = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MATERIAL_TYPE_NAME = item.TDL_SERVICE_NAME;
                        rdo.MATERIAL_QUYCACH_NAME = item.MATERIAL_PACKING_TYPE_NAME;

                        rdo.MATERIAL_PRICE = (item.MATERIAL_IMP_PRICE ?? 0) * ((item.MATERIAL_IMP_VAT_RATIO ?? 0) + 1);
                        if(item.USE_ORIGINAL_UNIT_FOR_PRES != 1 && serviceUnit!=null && serviceUnit.CONVERT_RATIO>0)
                        {
                            rdo.MATERIAL_PRICE = rdo.MATERIAL_PRICE / (serviceUnit.CONVERT_RATIO??0);
                        }    
                        if (castFilter.IS_BID_NUM_ORDER_MERGE != true)
                        {
                            rdo.BID_NUM_ORDER = dicBidMaterialType.ContainsKey((item.MATERIAL_TYPE_ID ?? 0) + "_" + (item.MATERIAL_BID_ID ?? 0)) ? dicBidMaterialType[(item.MATERIAL_TYPE_ID ?? 0) + "_" + (item.MATERIAL_BID_ID ?? 0)].BID_NUM_ORDER : item.MATERIAL_BID_NUM_ORDER ?? ((ListMaterial ?? new List<HIS_MATERIAL>()).FirstOrDefault(o => o.ID == item.MATERIAL_ID) ?? new HIS_MATERIAL()).TDL_BID_NUM_ORDER;
                        }
                        rdo.BID_NUMBER = dicBid.ContainsKey(item.MATERIAL_BID_ID ?? 0) ? dicBid[item.MATERIAL_BID_ID ?? 0].First().BID_NUMBER : ((ListMaterial ?? new List<HIS_MATERIAL>()).FirstOrDefault(o => o.ID == item.MATERIAL_ID) ?? new HIS_MATERIAL()).TDL_BID_NUMBER;

                        rdo.PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                        if (item.USE_ORIGINAL_UNIT_FOR_PRES != 1 && serviceUnit != null && serviceUnit.CONVERT_RATIO > 0)
                        {
                            rdo.PRICE = rdo.PRICE / (serviceUnit.CONVERT_RATIO ?? 0);
                        }
                        rdo.MATERIAL_UNIT_NAME = item.SERVICE_UNIT_NAME;

                        bool valid = false;

                        if (castFilter.IS_TREAT.HasValue)
                        {
                            if (castFilter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                valid = true;
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                            }
                            else if (!castFilter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                valid = true;
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }
                            rdo.TOTAL_HEIN_PRICE = (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
                            rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                            rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
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

                            rdo.TOTAL_HEIN_PRICE = (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApprovalBhyts.FirstOrDefault(p => p.ID == item.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
                            rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                            rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                        }

                        if (item.USE_ORIGINAL_UNIT_FOR_PRES != 1 && serviceUnit != null && serviceUnit.CONVERT_RATIO > 0)
                        {
                            rdo.AMOUNT_NOITRU = rdo.AMOUNT_NOITRU * (serviceUnit.CONVERT_RATIO ?? 0);
                            rdo.AMOUNT_NGOAITRU = rdo.AMOUNT_NGOAITRU * (serviceUnit.CONVERT_RATIO ?? 0);
                            var convertUnit  = HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == serviceUnit.CONVERT_ID);
                            if(convertUnit!=null)
                            {
                                rdo.MATERIAL_UNIT_NAME = convertUnit.SERVICE_UNIT_NAME;
                            }    
                            
                        }
                        if (!String.IsNullOrWhiteSpace(this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(heinAproval.HEIN_MEDI_ORG_CODE))
                        {
                            rdo.ROUTE_CODE = RouteCodeA;
                        }
                        else
                        {
                            rdo.ROUTE_CODE = RouteCodeB;
                        }
                        if (checkBhytProvinceCode(heinAproval.HEIN_CARD_NUMBER))
                        {
                            rdo.PROVINCE_TYPE = NoiTinh;
                        }
                        else
                        {
                            rdo.PROVINCE_TYPE = NgoaiTinh;
                        }
                        rdo.RIGHT_ROUTE_CODE = heinAproval.RIGHT_ROUTE_CODE;
                        if (valid)
                        {
                            ListRdo.Add(rdo);
                            ListDetail.Add(rdo);
                        }
                    }
                }
            }
        }

        private List<HIS_MATERIAL> GetMaterial(List<V_HIS_SERE_SERV_3> ListSereServHein)
        {
            List<HIS_MATERIAL> result = new List<HIS_MATERIAL>();
            var materialIds = ListSereServHein.Select(o=>o.MATERIAL_ID??0).Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                        if (MaterialSub != null)
                        {
                            result.AddRange(MaterialSub);
                        }
                    }
                }
                return result;
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

        private List<Mrs00017RDO> ProcessListRDO()
        {
            List<Mrs00017RDO> listCurrent = new List<Mrs00017RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var groupRDOs = ListRdo.GroupBy(o => new { o.BID_NUM_ORDER, o.MATERIAL_CODE_DMBYT, o.MATERIAL_TYPE_NAME_BYT, o.BHYT_PAY_RATE, o.MATERIAL_PRICE, o.PRICE, o.ROUTE_CODE }).ToList();

                    ListRdo.Clear();
                    foreach (var group in groupRDOs)
                    {
                        var listsub = group.ToList<Mrs00017RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00017RDO rdo = new Mrs00017RDO();
                            rdo.ROUTE_CODE = listsub[0].ROUTE_CODE;
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.PROVINCE_TYPE = listsub[0].PROVINCE_TYPE;
                            rdo.RIGHT_ROUTE_CODE = listsub[0].RIGHT_ROUTE_CODE;
                            if (LisServiceRetyCat != null)
                            {
                                if (LisServiceRetyCat.Exists(o => o.SERVICE_ID == listsub[0].SERVICE_ID && o.CATEGORY_CODE == castFilter.CATEGORY_CODE__DC))
                                {
                                    rdo.IS_DC = true;
                                }
                            }
                            rdo.MATERIAL_CODE_DMBYT = listsub[0].MATERIAL_CODE_DMBYT;
                            rdo.MATERIAL_CODE_DMBYT_1 = listsub[0].MATERIAL_CODE_DMBYT_1;
                            rdo.MATERIAL_STT_DMBYT = listsub[0].MATERIAL_STT_DMBYT;
                            rdo.MATERIAL_TYPE_NAME_BYT = listsub[0].MATERIAL_TYPE_NAME_BYT;
                            rdo.MATERIAL_TYPE_NAME = listsub[0].MATERIAL_TYPE_NAME;
                            rdo.MATERIAL_QUYCACH_NAME = listsub[0].MATERIAL_QUYCACH_NAME;
                            rdo.MATERIAL_PRICE = listsub[0].MATERIAL_PRICE;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MATERIAL_UNIT_NAME = listsub[0].MATERIAL_UNIT_NAME;
                            rdo.BID_NUM_ORDER = listsub[0].BID_NUM_ORDER;
                            rdo.BID_NUMBER = listsub[0].BID_NUMBER;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
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
            return listCurrent.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
            if (castFilter.IS_RIGHT_ROUTE!=null )
            {
                if (castFilter.IS_RIGHT_ROUTE==true)
                {
                    ListRdo = ListRdo.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                    ListDetail = ListDetail.Where(x => x.RIGHT_ROUTE_CODE == "DT").ToList();
                }
                if (castFilter.IS_RIGHT_ROUTE == false)
                {
                    ListRdo = ListRdo.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                    ListDetail = ListDetail.Where(x => x.RIGHT_ROUTE_CODE == "TT").ToList();
                }
            }
            if (castFilter.IS_IN_PROVICE != null)
            {
                if (castFilter.IS_IN_PROVICE == true)
                {
                    ListRdo = ListRdo.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                    ListDetail = ListDetail.Where(x => x.PROVINCE_TYPE == NoiTinh).ToList();
                }
                if (castFilter.IS_IN_PROVICE == false)
                {
                    ListRdo = ListRdo.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                    ListDetail = ListDetail.Where(x => x.PROVINCE_TYPE == NgoaiTinh).ToList();
                }
            }
            objectTag.AddObjectData(store, "ListRdoNoLimit", ListRdo.Where(o => o.IS_DC == false).ToList());
            objectTag.AddObjectData(store, "ReportA", ListRdo.Where(o => o.ROUTE_CODE == "A" && o.IS_DC == false).ToList());
            objectTag.AddObjectData(store, "ReportB", ListRdo.Where(o => o.ROUTE_CODE == "B" && o.IS_DC == false).ToList());
            objectTag.AddObjectData(store, "ReportDC", ListRdo.Where(o => o.IS_DC == true).ToList());
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
                if (castFilter.IS_ROUTE ?? false)
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
