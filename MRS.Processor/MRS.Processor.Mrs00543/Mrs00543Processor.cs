using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00543
{
    class Mrs00543Processor : AbstractProcessor
    {

        Mrs00543Filter castFilter = null;

        List<Mrs00543RDO> ListRdoGeneric = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoCpyhct = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoVtyhct = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoOther = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdo = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListParent = new List<Mrs00543RDO>();

        List<Mrs00543RDO> ListRdoGenericLimit = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoCpyhctLimit = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoVtyhctLimit = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoOtherLimit = new List<Mrs00543RDO>();

        List<Mrs00543RDO> ListRdoGenericNoLimit = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoCpyhctNoLimit = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoVtyhctNoLimit = new List<Mrs00543RDO>();
        List<Mrs00543RDO> ListRdoOtherNoLimit = new List<Mrs00543RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApprovalBhyt = new List<V_HIS_HEIN_APPROVAL>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<string, HIS_BID_MEDICINE_TYPE> dicBidMedicineType = new Dictionary<string, HIS_BID_MEDICINE_TYPE>();
        Dictionary<long, List<HIS_BID>> dicBid = new Dictionary<long, List<HIS_BID>>();
        List<long> listHeinServiceTypeId;

        short IS_TRUE = 1;

        public Mrs00543Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00543Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00543Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("bat dau lay du lieu MRS00543: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.castFilter), this.castFilter));
                CommonParam paramGet = new CommonParam();


                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApprovalBhyt = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
                dicBidMedicineType = new HisBidMedicineTypeManager().Get(new HisBidMedicineTypeFilterQuery()).GroupBy(o => (o.MEDICINE_TYPE_ID + "_" + o.BID_ID)).ToDictionary(p => p.Key, p => p.First());

                dicBid = new HisBidManager(paramGet).Get(new HisBidFilterQuery()).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.ToList());
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DOAGET trong qua trinh lay du lieu MRS00543");
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

                        if (ListSereServHein != null)
                        {
                            ListSereServHein = ListSereServHein.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = heinApprovalBhyts.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);


                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00543");
                        }

                        ProcessListHeinApprovalDetail(heinApprovalBhyts, ListSereServHein, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ListRdoGenericLimit = ProcessListRDO(ListRdoGeneric);
                    ListRdoCpyhctLimit = ProcessListRDO(ListRdoCpyhct);
                    ListRdoVtyhctLimit = ProcessListRDO(ListRdoVtyhct);
                    ListRdoOtherLimit = ProcessListRDO(ListRdoOther);
                    ListParent = ProcessListRDO(ListRdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServHein, List<V_HIS_TREATMENT> listTreatment)
        {
            Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

            if (IsNotNullOrEmpty(heinApprovalBhyts))
            {
                foreach (var item in heinApprovalBhyts)
                {
                    dicHeinApproval[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListSereServHein))
            {
                if (castFilter.MEDICINE_TYPE_ID != null)
                {
                    ListSereServHein = ListSereServHein.Where(o => o.MEDICINE_TYPE_ID == castFilter.MEDICINE_TYPE_ID).ToList();
                }

                foreach (var item in ListSereServHein)
                {
                    if (item.IS_NO_EXECUTE == IS_TRUE || item.IS_EXPEND == IS_TRUE
                        || item.AMOUNT <= 0 || item.PRICE == 0 ||
                        item.TDL_HEIN_SERVICE_TYPE_ID == null || item.HEIN_APPROVAL_ID == null)
                        continue;
                    if (!item.VIR_TOTAL_HEIN_PRICE.HasValue || item.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;

                    if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                    {
                        var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                        Mrs00543RDO rdo = new Mrs00543RDO();
                        rdo.V_HIS_TREATMENT = listTreatment.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID) ?? new V_HIS_TREATMENT();
                        rdo.V_HIS_SERE_SERV_3 = item;
                        rdo.V_HIS_HEIN_APPROVAL = heinApproval;

                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.V_HIS_TREATMENT.IN_TIME);
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.V_HIS_TREATMENT.OUT_TIME ?? 0);
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.MEDICINE_SODANGKY_NAME = item.MEDICINE_REGISTER_NUMBER;
                        rdo.MEDICINE_HOATCHAT_NAME = item.TDL_ACTIVE_INGR_BHYT_NAME;
                        rdo.MEDICINE_CODE_DMBYT = item.TDL_ACTIVE_INGR_BHYT_CODE;
                        rdo.REGISTER_NUMBER = item.REGISTER_NUMBER;
                        rdo.MEDICINE_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.MEDICINE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MEDICINE_HAMLUONG_NAME = item.MEDICINE_TYPE_CONCENTRA;
                        rdo.BID_NUM_ORDER = dicBidMedicineType.ContainsKey((item.MEDICINE_TYPE_ID ?? 0) + "_" + (item.MEDICINE_BID_ID ?? 0)) ? dicBidMedicineType[(item.MEDICINE_TYPE_ID ?? 0) + "_" + (item.MEDICINE_BID_ID ?? 0)].BID_NUM_ORDER : item.MEDICINE_BID_NUM_ORDER ?? item.TDL_MEDICINE_BID_NUM_ORDER;

                        rdo.BID_NUMBER = dicBid.ContainsKey(item.MEDICINE_BID_ID ?? 0) ? dicBid[item.MEDICINE_BID_ID ?? 0].First().BID_NUMBER : "";
                        rdo.PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                        {
                            rdo.AMOUNT_NOITRU = item.AMOUNT;
                        }
                        else if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                        {
                            rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                        }
                        rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());

                        if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU && Config.HisBloodIntoMedicineGenericCFG.BloodIntoMedicine__Generic)
                        {
                            ListRdoGeneric.Add(rdo);
                        }
                        else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            ListRdoOther.Add(rdo);
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
                                rdo.MEDICINE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                                rdo.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                                rdo.MEDICINE_DUONGDUNG_CODE = medicineType.MEDICINE_USE_FORM_CODE;
                                if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs00543.HIS_MEDICINE_LINE.ID__TTD)
                                {
                                    ListRdoGeneric.Add(rdo);
                                }
                                else if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs00543.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                {
                                    ListRdoCpyhct.Add(rdo);
                                }
                                else if (medicineType.MEDICINE_LINE_ID == MRS.Processor.Mrs00543.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                {
                                    ListRdoVtyhct.Add(rdo);
                                }
                            }
                            else
                            {
                                ListRdoOther.Add(rdo);
                            }
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
        }

        private List<Mrs00543RDO> ProcessListRDO(List<Mrs00543RDO> listRDO)
        {
            List<Mrs00543RDO> listCurrent = new List<Mrs00543RDO>();
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.MEDICINE_CODE_DMBYT, o.PRICE, o.MEDICINE_SODANGKY_NAME, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00543RDO> listsub = group.ToList<Mrs00543RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00543RDO rdo = new Mrs00543RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.MEDICINE_SODANGKY_NAME = listsub[0].MEDICINE_SODANGKY_NAME;
                            rdo.REGISTER_NUMBER = listsub[0].REGISTER_NUMBER;
                            rdo.MEDICINE_HAMLUONG_NAME = listsub[0].MEDICINE_HAMLUONG_NAME;
                            rdo.MEDICINE_DUONGDUNG_NAME = listsub[0].MEDICINE_DUONGDUNG_NAME;
                            rdo.MEDICINE_DUONGDUNG_CODE = listsub[0].MEDICINE_DUONGDUNG_CODE;
                            rdo.MEDICINE_HOATCHAT_NAME = listsub[0].MEDICINE_HOATCHAT_NAME;
                            rdo.BID_NUMBER = listsub[0].BID_NUMBER;
                            rdo.BID_NUM_ORDER = listsub[0].BID_NUM_ORDER;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MEDICINE_UNIT_NAME = listsub[0].MEDICINE_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
                            if (castFilter.IS_ROUND_BY_TREA == true)
                            {
                                rdo.TOTAL_PRICE = listsub.GroupBy(g => g.V_HIS_TREATMENT.TREATMENT_CODE).Select(o => o.Sum(s => s.TOTAL_PRICE)).Sum(s => Math.Round(s, 2));
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
            return listCurrent.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {


            dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            objectTag.AddObjectData(store, "Report1", ListRdoGenericLimit);
            objectTag.AddObjectData(store, "Report2", ListRdoCpyhctLimit);
            objectTag.AddObjectData(store, "Report3", ListRdoVtyhctLimit);
            objectTag.AddObjectData(store, "Report4", ListRdoOtherLimit);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListParent);
            objectTag.AddRelationship(store, "Parent", "Report", new string[] { "MEDICINE_CODE_DMBYT", "PRICE", "MEDICINE_SODANGKY_NAME" }, new string[] { "MEDICINE_CODE_DMBYT", "PRICE", "MEDICINE_SODANGKY_NAME" });
            objectTag.SetUserFunction(store, "fRound", new CustomerFuncRoundData());
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
