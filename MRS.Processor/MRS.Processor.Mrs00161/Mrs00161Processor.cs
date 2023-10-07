using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisExecuteRole;
using AutoMapper;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatmentType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00161
{
    class Mrs00161Processor : AbstractProcessor
    {
        Mrs00161Filter castFilter = null; 
        List<Mrs00161RDO> ListRdo = new List<Mrs00161RDO>(); 

        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>(); 
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<long> ListServiceReqTypeId = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT };
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();

        public Mrs00161Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00161Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = (Mrs00161Filter)this.reportFilter;
                if (castFilter.INPUT_DATA_ID_SVT_TYPE == 1)
                {
                    ListServiceReqTypeId = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT };
                }
                if (castFilter.INPUT_DATA_ID_SVT_TYPE == 2)
                {
                    ListServiceReqTypeId = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT };
                }
                GetServiceReq(); 
                var listServiceReqId = dicServiceReq.Keys.Distinct().ToList(); 
                if (IsNotNullOrEmpty(listServiceReqId)) ListSereServ = GetSereServ(listServiceReqId); 
                ProcessListSereServ(ListSereServ); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private List<HIS_SERE_SERV> GetSereServ(List<long> listServiceReqId)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>(); 
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                var skip = 0; 
                while (listServiceReqId.Count - skip > 0)
                {
                    var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery filterSereServ = new HisSereServFilterQuery();
                    filterSereServ.SERVICE_REQ_IDs = listIDs;
                    //filterSereServ.TDL_SERVICE_TYPE_IDs = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT;
                    filterSereServ.HAS_EXECUTE = true; 
                    var listSereServSub = new HisSereServManager(paramGet).Get(filterSereServ); 
                    result.AddRange(listSereServSub); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new List<HIS_SERE_SERV>(); 
            }
            return result; 
        }

        private void GetServiceReq()
        {
            try
            {
                HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery(); 
                srFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                srFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                srFilter.SERVICE_REQ_TYPE_IDs = ListServiceReqTypeId;
                srFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                srFilter.EXECUTE_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                srFilter.EXECUTE_ROOM_IDs = castFilter.EXECUTE_ROOM_IDs;
                dicServiceReq = (new HisServiceReqManager().Get(srFilter)??new List<HIS_SERVICE_REQ>()).ToDictionary(o => o.ID);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override bool ProcessData()
        {
            return true; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
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

        }
        private HIS_SERVICE_REQ req(HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ(); 
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0]; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new HIS_SERVICE_REQ(); 
            }
            return result; 
        }

        private void ProcessListSereServ(List<HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                    ListSereServ = ListSereServ.OrderBy(d => d.TDL_PATIENT_ID).ToList(); 
                    
                    int start = 0; 
                    int count = ListSereServ.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                        List<HIS_SERE_SERV> sereServs = ListSereServ.Skip(start).Take(limit).ToList(); 
                        HisSereServPtttViewFilterQuery ssPtttFilter = new HisSereServPtttViewFilterQuery(); 
                        ssPtttFilter.SERE_SERV_IDs = sereServs.Select(s => s.ID).ToList(); 
                        List<V_HIS_SERE_SERV_PTTT> listSereServPttt = new HisSereServPtttManager(paramGet).GetView(ssPtttFilter); 
                        HisEkipUserViewFilterQuery ekipUserFilter = new HisEkipUserViewFilterQuery(); 
                        ekipUserFilter.EKIP_IDs = sereServs.Select(s => s.EKIP_ID ?? 0).Distinct().ToList(); 
                        List<V_HIS_EKIP_USER> listEkipUser = new HisEkipUserManager(paramGet).GetView(ekipUserFilter); 

                        HisPatientTypeAlterViewFilterQuery PatientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        PatientTypeAlterFilter.TREATMENT_IDs = sereServs.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList(); 
                        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetView(PatientTypeAlterFilter);

                        HisDepartmentFilterQuery DepartmentFilter = new HisDepartmentFilterQuery();
                        List<HIS_DEPARTMENT> listDepartment = new HisDepartmentManager(paramGet).Get(DepartmentFilter);
                        //if (listSereServPttt != null && listPatient != null)
                        //{
                        ProcessListSereServPttt(sereServs, listSereServPttt, listPatientTypeAlter, listEkipUser, listDepartment); 
                        //}
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00161."); 
                    }
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ListRdo.OrderBy(o => o.TREATMENT_ID).ToList(); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessListSereServPttt(List<HIS_SERE_SERV> sereServs, List<V_HIS_SERE_SERV_PTTT> listSereServPttt,List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, List<V_HIS_EKIP_USER> listEkipUser, List<HIS_DEPARTMENT> listDepartment)
        {
            try
            {
                if (IsNotNullOrEmpty(sereServs))
                {
                    foreach (var sereServ in sereServs)
                    {
                        Mrs00161RDO rdo = new Mrs00161RDO(); 
                        if (CheckPatientType(listPatientTypeAlter, rdo, sereServ.TDL_TREATMENT_ID ?? 0))
                        {
                            HIS_SERVICE_REQ serviceReq = req(sereServ);
                            rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID ?? 0; 
                            rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;
                            rdo.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                            rdo.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE; 
                            //rdo.DESCRIPTION_SURGERY = sereServ.DESCRIPTION; 
                            //rdo.NOTE = sereServ.NOTE; 
                            rdo.TIME_MISU_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.START_TIME??0); //EXECUTE_TIME thời gian thực hiện phẫu thuật,  intruction_time thời gian chỉ định phẫu thuật
                            CalcuatorAge(rdo, serviceReq); 

                            if (IsNotNullOrEmpty(listSereServPttt))
                            {
                                var data = listSereServPttt.FirstOrDefault(f => f.SERE_SERV_ID == sereServ.ID); 
                                if (data != null)
                                {
                                    rdo.MISU_PPTT = data.PTTT_METHOD_NAME; 
                                    rdo.MISU_PPVC = data.EMOTIONLESS_METHOD_NAME; 
                                    rdo.BEFORE_MISU = data.BEFORE_PTTT_ICD_NAME ;//?? data.BEFORE_PTTT_ICD_TEXT; 
                                    rdo.AFTER_MISU = data.AFTER_PTTT_ICD_NAME;// ?? data.AFTER_PTTT_ICD_TEXT; 
                                    rdo.MISU_TYPE_NAME = data.PTTT_GROUP_NAME; 
                                    if (IsNotNullOrEmpty(listEkipUser) && sereServ.EKIP_ID.HasValue)
                                    {
                                        var ekipMain = listEkipUser.FirstOrDefault(o => o.EKIP_ID == sereServ.EKIP_ID.Value && o.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Main); 
                                        if (IsNotNull(ekipMain))
                                        {
                                            rdo.EXECUTE_DOCTOR = ekipMain.USERNAME; 
                                        }

                                        var ekipAnes = listEkipUser.FirstOrDefault(o => o.EKIP_ID == sereServ.EKIP_ID.Value && o.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist); 
                                        if (IsNotNull(ekipAnes))
                                        {
                                            rdo.ANESTHESIA_DOCTOR = ekipAnes.USERNAME; 
                                        }
                                    }
                                }
                            }
                            if (IsNotNullOrEmpty(listDepartment))
                            {
                                var data = listDepartment.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID);
                                if (data != null)
                                {
                                    rdo.REQUEST_DEPARTMENT_NAME = data.DEPARTMENT_NAME;
                                }
                            
                            }

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

        private bool CheckPatientType(List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, Mrs00161RDO rdo, long treatmentId)
        {
            bool result = false; 
            try
            {
                if (treatmentId > 0 && IsNotNullOrEmpty(listPatientTypeAlter))
                {
                    var PatientTypeAlters = listPatientTypeAlter.Where(o => o.TREATMENT_ID == treatmentId).ToList(); 
                    if (IsNotNullOrEmpty(PatientTypeAlters))
                    {
                        var currentPatientType = PatientTypeAlters.OrderByDescending(o => o.LOG_TIME).First(); 
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
                                rdo.HEIN_CARD_NUMBER = currentPatientType.HEIN_CARD_NUMBER;
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
                            else if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
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

        private void CalcuatorAge(Mrs00161RDO rdo, HIS_SERVICE_REQ serviceReq)
        {
            try
            {
                rdo.VIR_ADDRESS = serviceReq.TDL_PATIENT_ADDRESS;
                int? tuoi = RDOCommon.CalculateAge(serviceReq.TDL_PATIENT_DOB); 
                    if (tuoi >= 0)
                    {
                        if (serviceReq.TDL_PATIENT_GENDER_ID== IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                            rdo.MALE_YEAR = ProcessYearDob(serviceReq.TDL_PATIENT_DOB); 
                        }
                        else
                        {
                            rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                            rdo.FEMALE_YEAR = ProcessYearDob(serviceReq.TDL_PATIENT_DOB); 
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

        private void ProcessDepartmentAndPatientType(Dictionary<string, object> dicSingleData)
        {
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = new HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value); 
                    if (department != null)
                    {
                        dicSingleData.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
                    }
                }
                if (castFilter.PATIENT_TYPE_ID.HasValue)
                {
                    var patientType = new HisPatientTypeManager().GetById(castFilter.PATIENT_TYPE_ID.Value); 
                    if (patientType != null)
                    {
                        dicSingleData.Add("PATIENT_TYPE_NAME", patientType.PATIENT_TYPE_NAME); 
                    }
                }
                if (castFilter.TREATMENT_TYPE_ID.HasValue)
                {
                    var treatmentType = new HisTreatmentTypeManager().GetById(castFilter.TREATMENT_TYPE_ID.Value); 
                    if (IsNotNull(treatmentType))
                    {
                        dicSingleData.Add("TREATMENT_TYPE_NAMEs", treatmentType.TREATMENT_TYPE_NAME); 
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
                        dicSingleData.Add("TREATMENT_TYPE_NAMEs", treatmentTypeNames); 
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
