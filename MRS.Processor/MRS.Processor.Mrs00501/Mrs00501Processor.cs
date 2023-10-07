using MOS.MANAGER.HisHeinServiceType;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00501
{
    public class Mrs00501Processor : AbstractProcessor
    {
        Mrs00501Filter castFilter = null;
        List<Mrs00501RDO> ListRdoOutPatient = new List<Mrs00501RDO>();
        List<Mrs00501RDO> ListRdoInPatient = new List<Mrs00501RDO>();

        decimal TotalAmount = 0;
        List<long> treatmentIds = new List<long>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ3 = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();
        Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
        Dictionary<long, HIS_BRANCH> dicBranch = new Dictionary<long, HIS_BRANCH>();
        public Mrs00501Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00501Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = false;
            try
            {
                this.castFilter = (Mrs00501Filter)this.reportFilter;

                GetBranch();

                GetHeinApproval();
                GetTreatment(ListHeinApproval);

                GetSereServ3(ListHeinApproval);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetTreatment(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                var skip = 0;
                List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
                var ListTreatmentId = ListHeinApproval.Select(o => o.TREATMENT_ID).Distinct().ToList();
                while (ListTreatmentId.Count - skip > 0)
                {
                    var listIds = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.IDs = listIds;
                    List<V_HIS_TREATMENT> ListTreamentSub = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                    ListTreatment.AddRange(ListTreamentSub ?? new List<V_HIS_TREATMENT>());
                }
                dicTreatment = ListTreatment.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetBranch()
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                dicBranch = (new HisBranchManager(paramGet).Get(branchFilter) ?? new List<HIS_BRANCH>()).ToDictionary(o => o.ID);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetSereServ3(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                var skip = 0;
                List<V_HIS_SERE_SERV_3> ListSereServ = new List<V_HIS_SERE_SERV_3>();
                var ListHeinApprovalId = ListHeinApproval.Select(o => o.ID).Distinct().ToList();
                while (ListHeinApprovalId.Count - skip > 0)
                {
                    var listIds = ListHeinApprovalId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                    ssFilter.HEIN_APPROVAL_IDs = listIds;
                    List<V_HIS_SERE_SERV_3> ListSereServSub = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                    ListSereServ.AddRange(ListSereServSub ?? new List<V_HIS_SERE_SERV_3>());
                    if (ListSereServ != null)
                    {
                        ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                    }
                }
                dicSereServ3 = ListSereServ.GroupBy(o => o.HEIN_APPROVAL_ID ?? 0).ToDictionary(p => p.Key, p => p.ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetHeinApproval()
        {
            try
            {
                CommonParam paramGet = new CommonParam();

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);
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
                if (IsNotNullOrEmpty(ListHeinApproval))
                {

                    foreach (var heinApproval in ListHeinApproval)
                    {
                        Mrs00501RDO rdo = new Mrs00501RDO(heinApproval);
                        if (dicSereServ3.ContainsKey(heinApproval.ID))
                        {
                            foreach (var sereServ in dicSereServ3[heinApproval.ID])
                            {
                                var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                                {
                                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                                    //Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__TEST)
                                    {
                                        rdo.TEST_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                                    {
                                        rdo.DIIM_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                                    {
                                        rdo.MEDICINE_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                                    {
                                        rdo.BLOOD_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                                    {
                                        rdo.SURG_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                                    {
                                        rdo.MATERIAL_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL)
                                    {
                                        rdo.MATERIAL_RATIO_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                                    {
                                        rdo.MEDICINE_RATIO_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID ==
IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                                    {
                                        rdo.EXAM_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                                        || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                                    {
                                        rdo.BED_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                                    {
                                        rdo.SERVICE_RATIO_PRICE += TotalPriceTreatment;
                                    }
                                    else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                                    {
                                        rdo.TRAN_PRICE += TotalPriceTreatment;
                                    }
                                    rdo.TOTAL_PRICE += TotalPriceTreatment;
                                    rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                                    rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                    rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                                }
                            }
                        }
                        if (rdo.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            rdo.RIGHT_ROUTE_AMOUNT = 1;
                        else if (rdo.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                            rdo.LEFT_ROUTE_AMOUNT = 1;
                        if (!treatmentIds.Contains(rdo.TREATMENT_ID))
                        {
                            treatmentIds.Add(rdo.TREATMENT_ID);
                            if (dicTreatment.ContainsKey(rdo.TREATMENT_ID))
                                rdo.NUM_DAY_AMOUNT = DateDiff.diffDate(dicTreatment[rdo.TREATMENT_ID].IN_TIME, dicTreatment[rdo.TREATMENT_ID].OUT_TIME);
                        }
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            ListRdoOutPatient.Add(rdo);
                        else
                            ListRdoInPatient.Add(rdo);
                    }
                }
                //Gom theo co so y te
                ProcessGroup();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessGroup()
        {
            try
            {
                GroupByBranchID(ref ListRdoOutPatient);
                GroupByBranchID(ref ListRdoInPatient);
                AddInfo(ref ListRdoOutPatient);
                AddInfo(ref ListRdoInPatient);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GroupByBranchID(ref List<Mrs00501RDO> ListRdo)
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => o.BRANCH_ID).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00501RDO rdo;
                List<Mrs00501RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00501RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00501RDO();
                    listSub = item.ToList<Mrs00501RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("_AMOUNT") || field.Name.Contains("_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum > 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private Mrs00501RDO IsMeaningful(List<Mrs00501RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00501RDO();
        }

        private void AddInfo(ref List<Mrs00501RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                item.BRANCH_NAME = dicBranch.ContainsKey(item.BRANCH_ID) ? dicBranch[item.BRANCH_ID].BRANCH_NAME : "";
                item.BRANCH_CODE = dicBranch.ContainsKey(item.BRANCH_ID) ? dicBranch[item.BRANCH_ID].BRANCH_CODE : "";
            }

        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            }

            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            }

            objectTag.AddObjectData(store, "ReportOutPatient", ListRdoOutPatient);
            objectTag.AddObjectData(store, "ReportInPatient", ListRdoInPatient);
        }
    }
}
