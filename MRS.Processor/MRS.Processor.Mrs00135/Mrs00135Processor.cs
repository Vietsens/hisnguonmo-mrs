using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisDepartment;
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

namespace MRS.Processor.Mrs00135
{
    public class Mrs00135Processor : AbstractProcessor
    {
        Mrs00135Filter castFilter = null;
        List<Mrs00135RDO> ListRdo = new List<Mrs00135RDO>();

        HIS_BRANCH _Branch = null;
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval;

        public Mrs00135Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00135Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00135Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_HEIN_APPROVAL, MRS00135 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00135.");
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
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00135");
                        }
                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListTreatment);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                    Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var treatment in ListTreatment)
                        {
                            dicTreatment[treatment.ID] = treatment;
                        }
                    }

                    if (IsNotNullOrEmpty(hisHeinApprovals))
                    {
                        foreach (var item in hisHeinApprovals)
                        {
                            dicHeinApproval[item.ID] = item;
                        }
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var item in ListSereServ)
                        {
                            if (item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.AMOUNT <= 0 || item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null)
                                continue;
                            if (dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                            {
                                var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                                Mrs00135RDO rdo = new Mrs00135RDO();
                                rdo.HEIN_APPROVAL_ID = heinApproval.ID;
                                rdo.SERVICE_ID = item.SERVICE_ID;
                                rdo.HEIN_APPROVAL_CODE = heinApproval.HEIN_APPROVAL_CODE;
                                if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM ||
                                    item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM ||
                                    item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL)
                                {
                                    rdo.MATERIAL_CODE = item.TDL_HEIN_SERVICE_BHYT_CODE;
                                    if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_VT_TDM;
                                    }
                                    else
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_VT_TL_NDM;
                                    }
                                }
                                else
                                {
                                    rdo.SERVICE_CODE = item.TDL_HEIN_SERVICE_BHYT_CODE;
                                    if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_XN;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_CDDHA;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_TDCN;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_DVKTC;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_MAU;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_PTTT;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_VC;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_KH;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT ||
                                        item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_GI_NT;
                                    }
                                    else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT ||
                                        item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN)
                                    {
                                        rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_GI_NGT;
                                    }
                                    var listChild = ListSereServ.Where(o =>
                                        o.PARENT_ID == item.ID &&
                                        (o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM ||
                                        o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM ||
                                        o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL ||
                                        o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                                        )).ToList();
                                    if (IsNotNullOrEmpty(listChild))
                                    {
                                        rdo.IS_ATTACH = true;
                                        foreach (var child in listChild)
                                        {
                                            if (String.IsNullOrEmpty(rdo.MATERIAL_CODE))
                                            {
                                                rdo.MATERIAL_CODE = child.TDL_HEIN_SERVICE_BHYT_CODE;
                                            }
                                            else
                                            {
                                                rdo.MATERIAL_CODE = rdo.MATERIAL_CODE + "; " + child.TDL_HEIN_SERVICE_BHYT_CODE;
                                            }
                                        }
                                    }
                                }
                                rdo.SERVICE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                                rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                                rdo.VIR_HEIN_PRICE = item.ORIGINAL_PRICE;
                                rdo.RATIO = item.HEIN_RATIO ?? 0;
                                rdo.VIR_TOTAL_HEIN_PRICE = item.VIR_TOTAL_HEIN_PRICE.Value;
                                rdo.AMOUNT = item.AMOUNT;
                                //rdo.INSTRUCTION_TIME = item; 
                                //rdo.INSTRUCTION_DATE = item.INTRUCTION_TIME.ToString().Substring(0, 12); 
                                //if (item.TIME.HasValue)
                                //{
                                //    rdo.RESULT_TIME = item.FINISH_TIME.Value; 
                                //    rdo.RESULT_DATE = item.FINISH_TIME.HasValue ? item.FINISH_TIME.Value.ToString().Substring(0, 12) : ""; 
                                //}
                                //var department = Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.DEPA); 
                                //if (IsNotNull(department))
                                //{
                                //    rdo.DEPARTMENT_BHYT_CODE = department.BHYT_CODE; 
                                //}
                                if (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                                {
                                    var treatment = dicTreatment[item.TDL_TREATMENT_ID ?? 0];
                                    rdo.ICD_CODEs = treatment.ICD_CODE;
                                    if (!String.IsNullOrEmpty(treatment.ICD_TEXT))
                                        rdo.ICD_CODEs = rdo.ICD_CODEs + "; " + treatment.ICD_TEXT;
                                }

                                rdo.PTTT_CODE = 0;
                                ListRdo.Add(rdo);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                objectTag.AddObjectData(store, "Services", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
