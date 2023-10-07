using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00542
{
    class Mrs00542Processor : AbstractProcessor
    {
        Mrs00542Filter castFilter = null;
        List<Mrs00542RDO> ListRdo = new List<Mrs00542RDO>();
        List<Mrs00542RDO> ListRdoLimit = new List<Mrs00542RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<HIS_MATERIAL_TYPE> listHisMaterialType = new List<HIS_MATERIAL_TYPE>();
        List<long> listHeinServiceTypeId;

        string MaterialPriceOption = "";
        public Mrs00542Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00542Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00542Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00542:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;

                listHisMaterialType = new HisMaterialTypeManager().Get(new HisMaterialTypeFilterQuery());

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00542");
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

                        if (ListSereServHein != null)
                        {
                            ListSereServHein = ListSereServHein.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = heinAppBhyts.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00542");
                        }

                        ProcessListHeinApprovalDetail(heinAppBhyts, ListSereServHein, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    // ap dung voi limit
                    ListRdoLimit = ProcessListAboutLimitRDO();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo = new List<Mrs00542RDO>();
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
                if (castFilter.MATERIAL_TYPE_ID != null)
                {
                    ListSereServHein = ListSereServHein.Where(o => o.MATERIAL_TYPE_ID == castFilter.MATERIAL_TYPE_ID).ToList();
                }

                foreach (var item in ListSereServHein)
                {
                    if (item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                        || item.AMOUNT <= 0 || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.HEIN_APPROVAL_ID == null)
                        continue;
                    if (!item.VIR_TOTAL_HEIN_PRICE.HasValue || item.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                    {
                        var heinAproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                        Mrs00542RDO rdo = new Mrs00542RDO();
                        var materialType = listHisMaterialType.FirstOrDefault(o => o.ID == item.MATERIAL_TYPE_ID) ?? new HIS_MATERIAL_TYPE();
                        rdo.IS_STENT = materialType.IS_STENT??0;
                        rdo.V_HIS_TREATMENT = listTreatment.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID) ?? new V_HIS_TREATMENT();
                        rdo.V_HIS_SERE_SERV_3 = item;
                        rdo.V_HIS_HEIN_APPROVAL = heinAproval;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.V_HIS_TREATMENT.IN_TIME);
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.V_HIS_TREATMENT.OUT_TIME ?? 0);
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.MATERIAL_CODE_DMBYT = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.MATERIAL_CODE_DMBYT_1 = item.TDL_MATERIAL_GROUP_BHYT;
                        rdo.MATERIAL_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.MATERIAL_TYPE_NAME_BYT = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MATERIAL_TYPE_NAME = item.TDL_SERVICE_NAME;
                        rdo.MATERIAL_QUYCACH_NAME = item.MATERIAL_PACKING_TYPE_NAME;
                        rdo.MATERIAL_PRICE = (item.MATERIAL_IMP_PRICE ?? 0) * ((item.MATERIAL_IMP_VAT_RATIO ?? 0) + 1);
                        rdo.PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);

                        rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);

                        rdo.MATERIAL_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;

                        bool valid = false;

                        if (castFilter.IS_TREAT.HasValue)
                        {
                            if (castFilter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                valid = true;
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                                rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1"); 
                            }
                            else if (!castFilter.IS_TREAT.Value && heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                valid = true;
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                                rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
                            }
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

                            rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");
                        }

                        if (valid)
                        {
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
        }


        private List<Mrs00542RDO> ProcessListAboutLimitRDO()
        {
            List<Mrs00542RDO> listCurrent = new List<Mrs00542RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var groupRDOs = ListRdo.GroupBy(o => new { o.MATERIAL_CODE_DMBYT, o.MATERIAL_PRICE, o.PRICE, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        var listsub = group.ToList<Mrs00542RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00542RDO rdo = new Mrs00542RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.IS_STENT = listsub[0].IS_STENT;
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
            return listCurrent.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", ListRdoLimit);
                objectTag.AddObjectData(store, "ReportDetail", ListRdo.OrderBy(o=>o.MATERIAL_CODE_DMBYT).ToList());
                objectTag.AddRelationship(store, "Report", "ReportDetail", new string[] { "MATERIAL_CODE_DMBYT", "MATERIAL_PRICE", "PRICE" }, new string[] { "MATERIAL_CODE_DMBYT", "MATERIAL_PRICE", "PRICE" });

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
