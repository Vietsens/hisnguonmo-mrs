using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceReq;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;

namespace MRS.Processor.Mrs00310
{
    public class Mrs00310Processor : AbstractProcessor
    {
        Mrs00310Filter filter = null;
        private List<Mrs00310RDO> ListRdo = new List<Mrs00310RDO>();
        private List<Mrs00310RDO> ListDepartment = new List<Mrs00310RDO>();
        public Mrs00310Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<ACS_USER> listAcsUser = new List<ACS_USER>();

        public override Type FilterType()
        {
            return typeof(Mrs00310Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.filter = ((Mrs00310Filter)reportFilter);
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 

                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.CLINICAL_IN_TIME_FROM = filter.TIME_FROM;
                treatmentFilter.CLINICAL_IN_TIME_TO = filter.TIME_TO;
                ListTreatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                listAcsUser = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery() { IS_ACTIVE = 1 });
                   

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00310");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListTreatment.Count;

                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = ListTreatment.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterFilterQuery patyAlterFilter = new HisPatientTypeAlterFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        var listPatientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).Get(patyAlterFilter);
                        if (listPatientTypeAlterSub != null)
                        {
                            listPatientTypeAlter.AddRange(listPatientTypeAlterSub);

                        }
                        HisDepartmentTranFilterQuery depaTranFilter = new HisDepartmentTranFilterQuery();
                        depaTranFilter.TREATMENT_IDs = patyAlterFilter.TREATMENT_IDs;
                        var listDepaTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).Get(depaTranFilter);
                        if (listDepaTran != null)
                        {
                            listDepartmentTran.AddRange(listDepaTran);

                        }

                        HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                        serviceReqFilter.TREATMENT_IDs = patyAlterFilter.TREATMENT_IDs;
                        serviceReqFilter.HAS_EXECUTE = true;
                        serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        var listServiceReqSub = new HisServiceReqManager(paramGet).Get(serviceReqFilter);
                        if (listServiceReqSub != null)
                        {
                            listServiceReq.AddRange(listServiceReqSub);

                        }
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh tong lay du lieu MRS00310");
                        }



                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    this.ProcessDataDetail();
                    this.ProcessListRdo();
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        void ProcessDataDetail()
        {
            foreach (var treatment in ListTreatment)
            {
                if (!treatment.CLINICAL_IN_TIME.HasValue)
                    continue;
                var patyALter = listPatientTypeAlter.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TREATMENT_ID == treatment.ID).OrderBy(p => p.LOG_TIME).ThenBy(q => q.ID).FirstOrDefault();
                if(patyALter==null)
                {
                continue;
                }
                var departmentTran = listDepartmentTran.FirstOrDefault(o => o.ID == patyALter.DEPARTMENT_TRAN_ID);
                var nextDepartmentTran = listDepartmentTran.FirstOrDefault(o => o.PREVIOUS_ID == departmentTran.ID);
                var prevDepartmentTran = listDepartmentTran.FirstOrDefault(o => o.ID == departmentTran.PREVIOUS_ID);
                if (departmentTran == null)
                {
                    continue;
                }

                if (filter.DEPARTMENT_ID != null)
                {
                    if (departmentTran.DEPARTMENT_ID != filter.DEPARTMENT_ID)
                    {
                        continue;
                    }
                }

                var serviceReq = listServiceReq.Where(o => o.TREATMENT_ID == treatment.ID).OrderBy(p => p.FINISH_TIME).LastOrDefault();
                Mrs00310RDO rdo = new Mrs00310RDO();
                rdo.TREATMENT_ID = treatment.ID;
                rdo.CREATOR = treatment.CREATOR;
                rdo.CREATE_USERNAME = (listAcsUser.FirstOrDefault(o=>o.LOGINNAME==treatment.CREATOR)??new ACS_USER()).USERNAME;
                rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                rdo.ICD_NAME = treatment.ICD_NAME;
                rdo.IN_LOGINNAME = treatment.IN_LOGINNAME;
                rdo.IN_USERNAME = treatment.IN_USERNAME; 
                rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE; 
                rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                rdo.IN_DEPARTMENT_ID = departmentTran.DEPARTMENT_ID;
                rdo.IN_CODE = treatment.IN_CODE;
                rdo.STORE_CODE = treatment.STORE_CODE;
                rdo.IN_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                if (prevDepartmentTran != null)
                {
                    rdo.GIVE_DEPARTMENT_ID = prevDepartmentTran.DEPARTMENT_ID;
                    rdo.GIVE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == prevDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                }
                rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME.Value); 
                rdo.TIME_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.CLINICAL_IN_TIME.Value); 
                rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME; 
                this.CalcuatorAge(rdo, treatment);
               
                    if (patyALter.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.IS_BHYT = "X";
                    }
                if (!String.IsNullOrEmpty(treatment.ICD_NAME))
                {
                    rdo.DIAGNOSE_KDT = treatment.ICD_NAME; 
                }

                if (serviceReq!=null)
                {
                    rdo.DIAGNOSE_KKB = serviceReq.ICD_NAME;
                }

                rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                rdo.GIOITHIEU = treatment.TRANSFER_IN_MEDI_ORG_NAME; 

                rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;

                rdo.DEPARTMENT_IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(departmentTran.DEPARTMENT_IN_TIME ?? 0);

                if (nextDepartmentTran != null)
                {
                    rdo.DEPARTMENT_OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0);
                }
                rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0);
                rdo.IN_CODE = treatment.IN_CODE;
                ListRdo.Add(rdo); 
            }
        }

        void ProcessListRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.OrderBy(o => o.CLINICAL_IN_TIME).ToList();
                    ListDepartment = ListRdo.GroupBy(g => g.IN_DEPARTMENT_ID).Select(s => new Mrs00310RDO() { IN_DEPARTMENT_ID = s.First().IN_DEPARTMENT_ID, IN_DEPARTMENT_NAME = s.First().IN_DEPARTMENT_NAME }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CalcuatorAge(Mrs00310RDO rdo, HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                }
                if (tuoi == 0)
                {
                    rdo.IS_DUOI_12THANG = "X";
                }
                else if (tuoi >= 1 && tuoi <= 15)
                {
                    rdo.IS_1DEN15TUOI = "X";
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00310Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("CLINICAL_IN_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00310Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00310Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("CLINICAL_IN_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00310Filter)reportFilter).TIME_TO));
            }
            bool exportSuccess = true;
            objectTag.AddObjectData(store, "ListDepartment", ListDepartment);
            objectTag.AddObjectData(store, "Treatment", ListRdo);
            objectTag.AddRelationship(store, "ListDepartment", "Treatment", "IN_DEPARTMENT_ID", "IN_DEPARTMENT_ID");
            exportSuccess = exportSuccess && store.SetCommonFunctions();
        }

    }
}
