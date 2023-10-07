using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServExt;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00162
{
    public class Mrs00162Processor : AbstractProcessor
    {
        Mrs00162Filter castFilter = null;
        private List<Mrs00162RDO> ListRdo = new List<Mrs00162RDO>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, HIS_SERE_SERV_EXT> dicSereServExt = new Dictionary<long, HIS_SERE_SERV_EXT>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        CommonParam paramGet = new CommonParam();
        public Mrs00162Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00162Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00162Filter)reportFilter);
            var result = true;
            try
            {
                HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                ssFilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                ssFilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                ssFilter.HAS_EXECUTE = true;
                ssFilter.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS;
                List<HIS_SERE_SERV> ListSereServ = new HisSereServManager(paramGet).Get(ssFilter);
                ProcessListSereServ(ListSereServ);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListSereServ(List<HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    CommonParam paramGet = new CommonParam();
                    ListSereServ = ListSereServ.OrderBy(d => d.TDL_PATIENT_ID).ToList();
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListSereServ = ListSereServ.Where(o => o.TDL_EXECUTE_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList();
                    }
                  
                    var treatmentIds = ListSereServ.Select(o => o.TDL_TREATMENT_ID??0).Distinct().ToList();
                    if (treatmentIds.Count > 0)
                    {
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery();
                            patientTypeAlterFilter.TREATMENT_IDs = listIds;
                            listPatientTypeAlter.AddRange(new HisPatientTypeAlterManager(param).Get(patientTypeAlterFilter) ?? new List<HIS_PATIENT_TYPE_ALTER>());
                            HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                            serviceReqFilter.TREATMENT_IDs = listIds;
                            listHisServiceReq.AddRange(new HisServiceReqManager(param).Get(serviceReqFilter)??new List<HIS_SERVICE_REQ>());
                        }
                        listHisServiceReq = listHisServiceReq.Where(o => ListSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID)).ToList();
                    }

                    ProcessOneSereServFromList(ListSereServ, listPatientTypeAlter, listHisServiceReq);
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00162.");
                    }
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ListRdo.OrderBy(t => t.PATIENT_CODE).ThenBy(b => b.TREATMENT_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }
        private void ProcessSereServExt(CommonParam paramGet, List<HIS_SERE_SERV> hisSereServs)
        {
            try
            {
                if (IsNotNullOrEmpty(hisSereServs))
                {
                    List<long> sereServId = hisSereServs.Select(s => s.ID).ToList();
                    List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
                    int start = 0;
                    int count = sereServId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServExtFilterQuery serExtFilter = new HisSereServExtFilterQuery();
                        serExtFilter.SERE_SERV_IDs = sereServId.Skip(start).Take(limit).ToList();
                        var hisSereServExts = new HisSereServExtManager(paramGet).Get(serExtFilter);
                        if (IsNotNullOrEmpty(hisSereServExts))
                        {
                            ListSereServExt.AddRange(hisSereServExts);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    foreach (var o in ListSereServExt) if (!dicSereServExt.ContainsKey(o.SERE_SERV_ID)) dicSereServExt[o.SERE_SERV_ID] = o;

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessOneSereServFromList(List<HIS_SERE_SERV> sereServs, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, List<HIS_SERVICE_REQ> listHisServiceReq)
        {
            try
            {
                if (IsNotNullOrEmpty(sereServs))
                {
                    ProcessSereServExt(param, sereServs);
                    foreach (var sereServ in sereServs)
                    {
                        var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).FirstOrDefault() ?? new HIS_PATIENT_TYPE_ALTER();
                        var serviceReq = listHisServiceReq.Where(o => o.ID == sereServ.SERVICE_REQ_ID).FirstOrDefault() ?? new HIS_SERVICE_REQ();
                        Mrs00162RDO rdo = new Mrs00162RDO();
                        if (CheckPatientType(listPatientTypeAlter, rdo, sereServ.TDL_TREATMENT_ID ?? 0))
                        {
                            rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID ?? 0;
                            rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;
                            rdo.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                            rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o=>o.ID==sereServ.TDL_REQUEST_ROOM_ID)??new V_HIS_ROOM()).ROOM_NAME;
                            rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                            rdo.EXECUTE_USERNAME = serviceReq.EXECUTE_USERNAME;
                            rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                            rdo.ENDO_RESULT = dicSereServExt.ContainsKey(sereServ.ID) ? dicSereServExt[sereServ.ID].CONCLUDE : "";
                            rdo.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE;
                            if (serviceReq != null)
                            {
                                rdo.ICD_ENDO = serviceReq.ICD_NAME ?? serviceReq.ICD_TEXT;
                            }
                            CalcuatorAge(rdo, serviceReq.TDL_PATIENT_ADDRESS,serviceReq.TDL_PATIENT_DOB,serviceReq.TDL_PATIENT_GENDER_ID);
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

        
       private bool CheckPatientType(List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, Mrs00162RDO rdo, long treatmentId)
        {
            bool result = false;
            try
            {
                if (treatmentId > 0 && IsNotNullOrEmpty(listPatientTypeAlter))
                {
                    var patientTypeAlters = listPatientTypeAlter.Where(o => o.TREATMENT_ID == treatmentId).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlters))
                    {
                        var currentPatientType = patientTypeAlters.OrderByDescending(o => o.LOG_TIME).First();
                        if (!castFilter.PATIENT_TYPE_ID.HasValue)
                        {
                            result = true;
                        }
                        else if (currentPatientType.PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID.Value)
                        {
                            result = true;
                        }
                        if (result)
                        {
                            if (currentPatientType.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.IS_BHYT = "X";
                            }
                        }
                        if (result)
                        {
                            if (castFilter.TREATMENT_TYPE_ID.HasValue)
                            {
                                if (currentPatientType.TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID.Value)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                            {
                                if (castFilter.TREATMENT_TYPE_IDs.Contains(currentPatientType.TREATMENT_TYPE_ID))
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                result = true;
                            }
                        }
                        
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

        private void CalcuatorAge(Mrs00162RDO rdo, string TDL_PATIENT_ADDRESS, long TDL_PATIENT_DOB, long? TDL_PATIENT_GENDER_ID)
        {
            try
            {

                rdo.VIR_ADDRESS = TDL_PATIENT_ADDRESS;
                    int? tuoi = RDOCommon.CalculateAge(TDL_PATIENT_DOB);
                    if (tuoi >= 0)
                    {
                        if (TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                            rdo.MALE_YEAR = ProcessYearDob(TDL_PATIENT_DOB);
                        }
                        else
                        {
                            rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                            rdo.FEMALE_YEAR = ProcessYearDob(TDL_PATIENT_DOB);
                        }
                    }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private void ProcessDepartmentAndPatientType(Dictionary<string, object> dicSingleTag)
        {
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = new HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value);
                    if (department != null)
                    {
                        dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
                    }
                }
                if (castFilter.PATIENT_TYPE_ID.HasValue)
                {
                    var patientType = new HisPatientTypeManager().GetById(castFilter.PATIENT_TYPE_ID.Value);
                    if (patientType != null)
                    {
                        dicSingleTag.Add("PATIENT_TYPE_NAME", patientType.PATIENT_TYPE_NAME);
                    }
                }
                if (castFilter.TREATMENT_TYPE_ID.HasValue)
                {
                    var treatmentType = new HisTreatmentTypeManager().GetById(castFilter.TREATMENT_TYPE_ID.Value);
                    if (IsNotNull(treatmentType))
                    {
                        dicSingleTag.Add("TREATMENT_TYPE_NAMEs", treatmentType.TREATMENT_TYPE_NAME);
                    }
                }
                else if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                {
                    var treatmentTypes = HisTreatmentTypeCFG.HisTreatmentTypes.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.ID)).ToList();
                    if (IsNotNullOrEmpty(treatmentTypes))
                    {
                        string treatmentTypeNames = "";
                        foreach (var treatmentType in treatmentTypes)
                        {
                            if (String.IsNullOrEmpty(treatmentTypeNames))
                                treatmentTypeNames = treatmentType.TREATMENT_TYPE_NAME;
                            else
                                treatmentTypeNames = treatmentTypeNames + "-" + treatmentType.TREATMENT_TYPE_NAME;

                        }
                        dicSingleTag.Add("TREATMENT_TYPE_NAMEs", treatmentTypeNames);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            #region Cac the Single
            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }
            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }
            ProcessDepartmentAndPatientType(dicSingleTag);
            #endregion

            objectTag.AddObjectData(store, "SereServs", ListRdo);
            objectTag.SetUserFunction(store, "FuncSameTitleColTreatmentCode", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColPatientCode", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColPatientName", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColAddress", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColAgeMale", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColAgeFermale", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleColHeinCard", new RDOCustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new RDOCustomerFuncMergeSameData());
        }
    }


}
