using MOS.MANAGER.HisService;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisMedicineTypeAcin;
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

namespace MRS.Processor.Mrs00140
{
    public class Mrs00140Processor : AbstractProcessor
    {
        Mrs00140Filter castFilter = null; 
        List<Mrs00140RDO> ListRdo = new List<Mrs00140RDO>(); 
        List<V_HIS_MEDICINE_TYPE_ACIN> ListMedicineAcin = new List<V_HIS_MEDICINE_TYPE_ACIN>(); 
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>(); 
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>(); 
        List<long> listHeinServiceTypeId; 
        HIS_BRANCH _Branch = null; 
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval; 

        public Mrs00140Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00140Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00140Filter)this.reportFilter; 
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID); 
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac"); 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_HEIN_APPROVAL, MRS00140 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery(); 
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM; 
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO; 
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID; 
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME"; 
                approvalFilter.ORDER_DIRECTION = "ACS"; 
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter); 
                ListMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery()); 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00140."); 
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
                    listHeinServiceTypeId = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                        //IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL
                    }; 
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = ListHeinApproval.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var hisHeinApproval = ListHeinApproval.Skip(start).Take(limit).ToList(); 
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery(); 
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApproval.Select(s => s.ID).ToList();
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (listSereServ != null)
                        {
                            listSereServ = listSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (!paramGet.HasException)
                        {
                            if (IsNotNullOrEmpty(listSereServ))
                            {
                                listSereServ = listSereServ.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceTypeId.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value) && o.AMOUNT > 0).ToList(); 
                                ProcessGetMedicineTypeAndMedicineAcin(paramGet, listSereServ); 
                                if (!paramGet.HasException)
                                {
                                    ProcessDetailHeinApprovalByTreatment(paramGet, hisHeinApproval, listSereServ); 
                                }
                                else
                                {
                                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00140."); 
                                }
                            }
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00140."); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00140."); 
                    }
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ListRdo.OrderBy(o => o.HEIN_APPROVAL_ID).ThenBy(t => t.INSTRUCTION_TIME).ToList(); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessGetMedicineTypeAndMedicineAcin(CommonParam paramGet, List<V_HIS_SERE_SERV_3> listSereServ)
        {
            try
            {
                ListMedicineAcin.Clear(); 
                ListMedicineType.Clear(); 
                if (IsNotNullOrEmpty(listSereServ))
                {
                    var listMedicineTypeId = listSereServ.Select(s => s.MEDICINE_TYPE_ID.Value).Distinct().ToList(); 
                    int start = 0; 
                    int count = listMedicineTypeId.Count; 
                    while (count > 0)
                    {
                        if (!paramGet.HasException)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var medicineTypeIds = listMedicineTypeId.Skip(start).Take(limit).ToList(); 
                            HisMedicineTypeViewFilterQuery mediTypeFilter = new HisMedicineTypeViewFilterQuery(); 
                            mediTypeFilter.IDs = medicineTypeIds; 
                            var listMediType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(mediTypeFilter); 
                            if (IsNotNullOrEmpty(listMediType))
                            {
                                ListMedicineType.AddRange(listMediType); 
                            }

                            HisMedicineTypeAcinViewFilterQuery mediAcinFilter = new HisMedicineTypeAcinViewFilterQuery(); 
                            mediAcinFilter.MEDICINE_TYPE_IDs = medicineTypeIds; 
                            var listMediAcin = new MOS.MANAGER.HisMedicineTypeAcin.HisMedicineTypeAcinManager(paramGet).GetView(mediAcinFilter); 
                            if (IsNotNullOrEmpty(listMediAcin))
                            {
                                ListMedicineAcin.AddRange(listMediAcin); 
                            }

                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessDetailHeinApprovalByTreatment(CommonParam paramGet, List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinApprovals) && IsNotNullOrEmpty(ListSereServ))
                {
                    List<V_HIS_SERVICE_REQ> ListServiceReq = new List<V_HIS_SERVICE_REQ>(); 
                    var listServiceReqId = ListSereServ.Select(s => s.SERVICE_REQ_ID ?? 0).Distinct().ToList(); 
                    int start = 0; 
                    int count = listServiceReqId.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery(); 
                        reqFilter.IDs = listServiceReqId.Skip(start).Take(limit).ToList(); 
                        var listData = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(reqFilter); 
                        if (!paramGet.HasException)
                        {
                            if (IsNotNullOrEmpty(listData))
                            {
                                ListServiceReq.AddRange(listData); 
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (IsNotNullOrEmpty(ListServiceReq))
                    {
                        ProcessListSereServ(paramGet, hisHeinApprovals, ListSereServ, ListServiceReq); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListSereServ(CommonParam paramGet, List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_SERVICE_REQ> ListServiceReq)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListServiceReq))
                {
                    List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>(); 
                    foreach (var item in ListSereServ)
                    {
                        Mrs00140RDO rdo = new Mrs00140RDO(); 
                        rdo.SERVICE_ID = item.SERVICE_ID; 

                        var heinApproval = hisHeinApprovals.FirstOrDefault(o => o.ID == item.HEIN_APPROVAL_ID.Value); 
                        if (IsNotNull(heinApproval))
                        {
                            rdo.HEIN_APPROVAL_ID = heinApproval.ID; 
                            rdo.HEIN_APPROVAL_CODE = heinApproval.HEIN_APPROVAL_CODE; 
                        }

                        var mediAcin = ListMedicineAcin.Where(o => o.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID.Value).ToList(); 
                        if (IsNotNullOrEmpty(mediAcin))
                        {
                            rdo.ACTIVE_INGREDIENT_CODE = mediAcin.First().ACTIVE_INGREDIENT_CODE; 
                        }
                        if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                        {
                            rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_TH_TDM; 
                        }
                        else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                        {
                            rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_TH_UT; 
                        }
                        else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM ||
                            item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                        {
                            rdo.SERVICE_REPORT_CODE = MRS.MANAGER.Config.HeinGroupServiceReportCFG.GROUP_REPORT_TH_TL_NDM; 
                        }
                        rdo.HEIN_SERVICE_BHYT_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME; 
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME; 
                        var mediType = ListMedicineType.FirstOrDefault(o => o.ID == item.MEDICINE_TYPE_ID.Value); 
                        if (IsNotNull(mediType))
                        {
                            rdo.CONCENTRA = mediType.CONCENTRA; 
                            rdo.MEDICINE_USER_FROM_NAME = mediType.MEDICINE_USE_FORM_NAME; 
                        }
                        rdo.DOSE_AMOUNT = 0; 
                        rdo.HEIN_SERVICE_BHYT_CODE = item.TDL_HEIN_SERVICE_BHYT_CODE; 
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.VIR_PRICE = item.ORIGINAL_PRICE* (1 + item.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                        rdo.HEIN_RATIO = item.HEIN_RATIO ?? 1; 
                        rdo.VIR_TOTAL_HEIN_PRICE = item.VIR_TOTAL_HEIN_PRICE.Value; 

                        var serviceReq = ListServiceReq.FirstOrDefault(o => o.ID == item.SERVICE_REQ_ID); 
                        if (IsNotNull(serviceReq))
                        {
                            rdo.INSTRUCTION_TIME = serviceReq.INTRUCTION_TIME; 
                            rdo.INSTRUCTION_DATE = rdo.INSTRUCTION_TIME.ToString().Substring(0, 12); 
                            var department = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID); 
                            if (IsNotNull(department))
                            {
                                rdo.DEPARTMENT_BHYT_CODE = department.BHYT_CODE; 
                            }
                        }
                        var treatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetViewById(item.TDL_TREATMENT_ID ?? 0); 
                        if (IsNotNull(treatment))
                        {
                            rdo.ICD_CODEs = treatment.ICD_CODE; 
                            if (!String.IsNullOrEmpty(treatment.ICD_TEXT))
                                rdo.ICD_CODEs = rdo.ICD_CODEs + "; " + treatment.ICD_TEXT; 
                        }
                        rdo.PTTT_CODE = 0; 
                        ListRdo.Add(rdo); 
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
