using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using FlexCel.Report;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00077
{
    public class Mrs00077Processor : AbstractProcessor
    {
        Mrs00077Filter castFilter = null;
        List<Mrs00077RDO> ListRdo = new List<Mrs00077RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        List<long> listHeinServiceTypeId;
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
        List<HIS_MATERIAL> listMaterial = new List<HIS_MATERIAL>();
        HIS_BRANCH _Branch = null;

        public Mrs00077Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00077Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = false;
            try
            {
                castFilter = ((Mrs00077Filter)this.reportFilter);
                listMaterialType = new HisMaterialTypeManager(new CommonParam()).Get(new HisMaterialTypeFilterQuery());
                listMaterial = new HisMaterialManager(new CommonParam()).Get(new HisMaterialFilterQuery());
                LoadDataToRam();
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
            bool result = true;
            try
            {
                ProcessListHeinApproval();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApproval()
        {
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
                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_IN,
                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_OUT,
                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_RATIO,
                        //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__MATERIAL_REPLACE
                        };

                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU); //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU); 
                    }

                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC); // IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC); 
                    }

                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> heinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = heinApprovals.Select(s => s.ID).ToList();
                        ssFilter.PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID;
                        var ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly du lieu MRS00077");
                        }
                        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = heinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        treatments = new HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        ProcessListHeinApprovalDetail(heinApprovals, ListSereServ, treatments);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRdo = ProcessListRDO();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> treatments)
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
                        if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                            || item.AMOUNT <= 0 || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.HEIN_APPROVAL_ID == null)
                            continue;
                        if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                        {
                            var heinAproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID);
                            Mrs00077RDO rdo = new Mrs00077RDO();
                            rdo.SERVICE_ID = item.SERVICE_ID;
                            rdo.MATERIAL_TYPE_CODE = item.TDL_SERVICE_CODE;
                            rdo.MATERIAL_CODE_DMBYT = item.TDL_HEIN_SERVICE_BHYT_CODE;
                            rdo.MATERIAL_CODE_DMBYT_1 = item.TDL_MATERIAL_GROUP_BHYT;
                            rdo.MATERIAL_STT_DMBYT = item.TDL_HEIN_ORDER;
                            rdo.MATERIAL_TYPE_NAME_BYT = item.TDL_HEIN_SERVICE_BHYT_NAME;
                            rdo.MATERIAL_TYPE_NAME = item.TDL_SERVICE_NAME;
                            var material = listMaterial.Where(x => x.ID == item.MATERIAL_ID).FirstOrDefault();
                            if (material != null)
                            {
                                rdo.MATERIAL_PRICE = (material.IMP_PRICE) * (1 + (material.IMP_VAT_RATIO));
                                var materialType = listMaterialType.Where(x => x.ID == material.MATERIAL_TYPE_ID).FirstOrDefault();
                                if (materialType != null)
                                {
                                    rdo.MATERIAL_QUYCACH_NAME = materialType.PACKING_TYPE_NAME;
                                }
                            }
                            rdo.TOTAL_HEIN_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                            rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                            rdo.MATERIAL_UNIT_NAME = item.SERVICE_UNIT_NAME;
                            if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                            }
                            else if (heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }
                            var treatment = treatments.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID) ?? new V_HIS_TREATMENT();
                            if (rdo.TOTAL_HEIN_PRICE != null)
                            {
                                rdo.TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinAproval.BRANCH_ID) ?? new HIS_BRANCH());
                                if (checkBhytNsd(treatment))
                                {
                                    rdo.TOTAL_HEIN_PRICE_NDS = item.VIR_TOTAL_HEIN_PRICE ?? 0;
                                }
                                else
                                {
                                    rdo.VIR_TOTAL_HEIN_PRICE = item.VIR_TOTAL_HEIN_PRICE ?? 0;
                                }
                            }

                            rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool checkBhytNsd(V_HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(treatment.ICD_CODE ?? ""))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(treatment.ICD_CODE))
                {
                    if ((treatment.TDL_HEIN_CARD_NUMBER ?? "  ").Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains((treatment.ICD_CODE ?? "   ").Substring(0, 3)))
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
        private List<Mrs00077RDO> ProcessListRDO()
        {
            List<Mrs00077RDO> listCurrent = new List<Mrs00077RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var groupRDOs = ListRdo.GroupBy(o => new { o.MATERIAL_CODE_DMBYT, o.MATERIAL_TYPE_NAME, o.MATERIAL_PRICE, o.TOTAL_HEIN_PRICE, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        var listsub = group.ToList<Mrs00077RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00077RDO rdo = new Mrs00077RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MATERIAL_CODE_DMBYT_1 = listsub[0].MATERIAL_CODE_DMBYT_1;
                            rdo.MATERIAL_CODE_DMBYT = listsub[0].MATERIAL_CODE_DMBYT;
                            rdo.MATERIAL_STT_DMBYT = listsub[0].MATERIAL_STT_DMBYT;
                            rdo.MATERIAL_TYPE_NAME_BYT = listsub[0].MATERIAL_TYPE_NAME_BYT;
                            rdo.MATERIAL_TYPE_CODE = listsub[0].MATERIAL_TYPE_CODE;
                            rdo.MATERIAL_TYPE_NAME = listsub[0].MATERIAL_TYPE_NAME;
                            rdo.MATERIAL_QUYCACH_NAME = listsub[0].MATERIAL_QUYCACH_NAME;
                            rdo.MATERIAL_PRICE = listsub[0].MATERIAL_PRICE;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MATERIAL_UNIT_NAME = listsub[0].MATERIAL_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                                rdo.VIR_TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE;
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
            return listCurrent.OrderBy(o => o.MATERIAL_STT_DMBYT).ToList();
        }

        private void LoadDataToRam()
        {
            try
            {
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager().GetView(approvalFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
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
