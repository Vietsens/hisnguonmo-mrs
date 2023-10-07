using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBedLog;
using MOS.MANAGER.HisConfig;
using MOS.MANAGER.HisDebate;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDhst;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisEmployee;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMediOrg;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisSereServTein;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTracking;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.Processor.Mrs00826.Base;
using MRS.Processor.Mrs00826.HoSoProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826
{
    public partial class Mrs00826Processor
    {

        private List<HIS_CONFIG> GetNewConfig()
        {
            List<HIS_CONFIG> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                HisConfigFilterQuery configFilter = new HisConfigFilterQuery();
                configFilter.IS_ACTIVE = 1;
                result = new HisConfigManager().Get(configFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_MEDI_ORG> GetMediOrg()
        {
            List<HIS_MEDI_ORG> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                HisMediOrgFilterQuery mediOrgFilter = new HisMediOrgFilterQuery();
                mediOrgFilter.IS_ACTIVE = 1;
                result = new HisMediOrgManager().Get(mediOrgFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_EMPLOYEE> GetEmployees()
        {
            List<HIS_EMPLOYEE> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                HisEmployeeFilterQuery filter = new HisEmployeeFilterQuery();
                filter.IS_ACTIVE = 1;
                result = new HisEmployeeManager().Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_DEPARTMENT> GetDepartments()
        {
            List<HIS_DEPARTMENT> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                filter.IS_ACTIVE = 1;
                result = new HisDepartmentManager().Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<V_HIS_SERVICE> GetService()
        {
            List<V_HIS_SERVICE> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                serviceFilter.IS_ACTIVE = 1;
                result = new HisServiceManager().GetView(serviceFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_MATERIAL_TYPE> GetMaterialType()
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                HisMaterialTypeFilterQuery materialTypeFilter = new HisMaterialTypeFilterQuery();
                materialTypeFilter.IS_ACTIVE = 1;
                result = new HisMaterialTypeManager().Get(materialTypeFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_ICD> GetIcd()
        {
            List<HIS_ICD> result = null;
            try
            {
                HisIcdFilterQuery icdFilter = new HisIcdFilterQuery();
                icdFilter.IS_ACTIVE = 1;
                result = new HisIcdManager().Get(icdFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        
        private void CreateThreadGetData(List<V_HIS_TREATMENT_1> listSelection)
        {
            System.Threading.Thread HeinApproval = new System.Threading.Thread(ThreadGetHeinApproval);
            System.Threading.Thread SereServ2 = new System.Threading.Thread(ThreadGetSereServ2);
            System.Threading.Thread Treatment3 = new System.Threading.Thread(ThreadGetTreatment3);
            System.Threading.Thread Dhst_Tracking = new System.Threading.Thread(ThreadGetDhst_Tracking);
            try
            {
                HeinApproval.Start(listSelection);
                SereServ2.Start(listSelection);
                Treatment3.Start(listSelection);
                Dhst_Tracking.Start(listSelection);

                HeinApproval.Join();
                SereServ2.Join();
                Treatment3.Join();
                Dhst_Tracking.Join();
            }
            catch (Exception ex)
            {
                HeinApproval.Abort();
                SereServ2.Abort();
                Treatment3.Abort();
                Dhst_Tracking.Abort();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ThreadGetDhst_Tracking(object obj)
        {
            try
            {
                if (obj == null) return;
                List<V_HIS_TREATMENT_1> listSelection = (List<V_HIS_TREATMENT_1>)obj;

                var skip = 0;
                while (listSelection.Count - skip > 0)
                {
                    var limit = listSelection.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    CommonParam param = new CommonParam();

                    HisDhstFilterQuery dhstFilter = new HisDhstFilterQuery();
                    dhstFilter.TREATMENT_IDs = limit.Select(o => o.ID).ToList();
                    var resultDhst = new HisDhstManager().Get(dhstFilter);
                    if (resultDhst != null && resultDhst.Count > 0)
                    {
                        listDhst.AddRange(resultDhst);
                    }

                    HisTrackingFilterQuery trackingFilter = new HisTrackingFilterQuery();
                    trackingFilter.TREATMENT_IDs = limit.Select(o => o.ID).ToList();
                    var resultTracking = new HisTrackingManager().Get(trackingFilter);
                    if (resultTracking != null && resultTracking.Count > 0)
                    {
                        hisTrackings.AddRange(resultTracking);
                    }

                    HisDebateFilterQuery debateFilter = new HisDebateFilterQuery();
                    debateFilter.TREATMENT_IDs = limit.Select(o => o.ID).ToList();
                    var resultDebate = new HisDebateManager().Get(debateFilter);
                    if (resultDebate != null && resultDebate.Count > 0)
                    {
                        ListDebates.AddRange(resultDebate);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ThreadGetTreatment3(object obj)
        {
            try
            {
                if (obj == null) return;
                List<V_HIS_TREATMENT_1> listSelection = (List<V_HIS_TREATMENT_1>)obj;

                var skip = 0;
                while (listSelection.Count - skip > 0)
                {
                    var limit = listSelection.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    CommonParam param = new CommonParam();
                    HisTreatmentView3FilterQuery treatmentFilter = new HisTreatmentView3FilterQuery();
                    treatmentFilter.IDs = limit.Select(o => o.ID).ToList();
                    var resultTreatment = new HisTreatmentManager().GetView3(treatmentFilter);
                    if (resultTreatment != null && resultTreatment.Count > 0)
                    {
                        hisTreatments.AddRange(resultTreatment);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ThreadGetSereServ2(object obj)
        {
            try
            {
                if (obj == null) return;
                List<V_HIS_TREATMENT_1> listSelection = (List<V_HIS_TREATMENT_1>)obj;

                var skip = 0;
                while (listSelection.Count - skip > 0)
                {
                    var limit = listSelection.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var resultSS = new ManagerSql().GetSereServView2(filter, limit.Select(o => o.ID).ToList());
                    if (resultSS != null && resultSS.Count > 0)
                    {
                        resultSS = resultSS.Distinct().ToList();
                        ListSereServ.AddRange(resultSS);

                        var ekipIds = resultSS.Select(o => o.EKIP_ID ?? 0).Distinct().ToList();
                        if (ekipIds != null && ekipIds.Count > 1)//null sẽ có 1 id bằng 0
                        {
                            HisEkipUserFilterQuery ekipFilter = new HisEkipUserFilterQuery();
                            ekipFilter.EKIP_IDs = ekipIds;
                            var resultEkip = new HisEkipUserManager().Get(ekipFilter);
                            if (resultEkip != null && resultEkip.Count > 0)
                            {
                                ListEkipUser.AddRange(resultEkip);
                            }
                        }

                        HisSereServTeinViewFilterQuery ssTeinFilter = new HisSereServTeinViewFilterQuery();
                        ssTeinFilter.SERE_SERV_IDs = resultSS.Select(o => o.ID).ToList();
                        var resulTein = new HisSereServTeinManager().GetView(ssTeinFilter);
                        if (resulTein != null && resulTein.Count > 0)
                        {
                            hisSereServTeins.AddRange(resulTein);
                        }

                        HisSereServPtttViewFilterQuery ssPtttFilter = new HisSereServPtttViewFilterQuery();
                        ssPtttFilter.SERE_SERV_IDs = resultSS.Select(o => o.ID).ToList();
                        var resultPttt = new HisSereServPtttManager().GetView(ssPtttFilter);
                        if (resultPttt != null && resultPttt.Count > 0)
                        {
                            hisSereServPttts.AddRange(resultPttt);
                        }

                        List<long> serviceReqIDs = resultSS.Where(o => o.SERVICE_REQ_ID.HasValue).Select(o => o.SERVICE_REQ_ID.Value).Distinct().ToList() ?? new List<long>();
                        HisBedLogViewFilterQuery bedFilter = new HisBedLogViewFilterQuery();
                        bedFilter.SERVICE_REQ_IDs = serviceReqIDs;
                        var resultBed = new HisBedLogManager().GetView(bedFilter);
                        if (resultBed != null && resultBed.Count > 0)
                        {
                            ListBedlog.AddRange(resultBed);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ThreadGetHeinApproval(object obj)
        {
            try
            {
                if (obj == null) return;
                List<V_HIS_TREATMENT_1> listSelection = (List<V_HIS_TREATMENT_1>)obj;

                var skip = 0;
                while (listSelection.Count - skip > 0)
                {
                    var limit = listSelection.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.TREATMENT_IDs = limit.Select(s => s.ID).ToList();
                    if (filter.INPUT_DATA_ID__TIME_TYPE == 7 || filter.INPUT_DATA_ID__TIME_TYPE == null)
                    {
                        approvalFilter.EXECUTE_TIME_FROM = filter.TIME_FROM;
                        approvalFilter.EXECUTE_TIME_TO = filter.TIME_TO;
                    }
                    var resultHeinApproval = new HisHeinApprovalManager().GetView(approvalFilter);
                    if (resultHeinApproval != null && resultHeinApproval.Count > 0)
                    {
                        listHeinApproval.AddRange(resultHeinApproval);
                    }
                    var treatmentIdNotApproval = limit.Where(o => resultHeinApproval == null || !resultHeinApproval.Exists(p => p.TREATMENT_ID == o.ID)).ToList();
                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                    patientTypeAlterFilter.TREATMENT_IDs = treatmentIdNotApproval.Select(s => s.ID).ToList();
                    var resultPatientTypeAlter = new HisPatientTypeAlterManager().GetView(patientTypeAlterFilter);
                    if (resultPatientTypeAlter != null && resultPatientTypeAlter.Count > 0)
                    {
                        listPatientTypeAlter.AddRange(resultPatientTypeAlter);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
