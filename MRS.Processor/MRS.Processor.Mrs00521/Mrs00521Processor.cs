using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00521
{
    class Mrs00521Processor : AbstractProcessor
    {
        Mrs00521Filter castFilter = null;

        public Mrs00521Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listSereServAb = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> listExamTreatment = new List<HIS_TREATMENT>();
        private List<Mrs00521RDO> ListRdo = new List<Mrs00521RDO>();
        private List<Mrs00521RDO> ListRdo1 = new List<Mrs00521RDO>();
        private List<Mrs00521RDO> sumRdo = new List<Mrs00521RDO>();
        private List<Mrs00521RDO> sumRdo1 = new List<Mrs00521RDO>();
        public override Type FilterType()
        {
            return typeof(Mrs00521Filter);
        }

        protected override bool GetData()
        {
            bool resutl = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00521Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00521: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                HisSereServfilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                HisSereServfilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                HisSereServfilter.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN;
                HisSereServfilter.TDL_REQUEST_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                HisSereServfilter.HAS_EXECUTE = true;
                listSereServ = new HisSereServManager(paramGet).Get(HisSereServfilter);

                HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                HisServiceReqfilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                HisServiceReqfilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                HisServiceReqfilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                HisServiceReqfilter.HAS_EXECUTE = true;
                HisServiceReqfilter.EXECUTE_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                listServiceReq = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);

                HisTreatmentFilterQuery ListTreatmentExamfilter = new HisTreatmentFilterQuery()
                {
                    OUT_TIME_FROM = this.castFilter.TIME_FROM,
                    OUT_TIME_TO = this.castFilter.TIME_TO,
                    IS_OUT = true
                };
                listExamTreatment = new HisTreatmentManager().Get(ListTreatmentExamfilter);

                List<long> listTreatmentId = new List<long>();
                listTreatmentId.AddRange(listSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList() ?? new List<long>());
                listTreatmentId.AddRange(listServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList() ?? new List<long>());
                listTreatmentId.AddRange(listExamTreatment.Select(o => o.ID).Distinct().ToList() ?? new List<long>());
                listTreatmentId = listTreatmentId.Distinct().ToList();
                // Doi tuong tuong ung
                if (listTreatmentId != null && listTreatmentId.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = limit;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "DESC";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ID";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager().Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    }
                }
                listHisPatientTypeAlter = listHisPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();

                if (this.castFilter.SERVICE_CODE__ABORTIONs != null && this.castFilter.SERVICE_CODE__ABORTIONs.Count > 0)
                {

                    var skip = 0;
                    while (this.castFilter.SERVICE_CODE__ABORTIONs.Count - skip > 0)
                    {
                        var limit = this.castFilter.SERVICE_CODE__ABORTIONs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceFilterQuery HisServicefilter = new HisServiceFilterQuery() 
                        {
                            SERVICE_CODEs = limit
                        };
                        var listService = new HisServiceManager().Get(HisServicefilter);
                        var listServiceIdSub = listService.Select(o => o.ID).ToList();
                        HisSereServFilterQuery ssfilter = new HisSereServFilterQuery();
                        ssfilter.SERVICE_IDs = listServiceIdSub;
                        ssfilter.ORDER_DIRECTION = "DESC";
                        ssfilter.ORDER_FIELD = "ID";
                        var listHisSereServSub = new HisSereServManager().Get(ssfilter);
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                        else
                            listSereServAb.AddRange(listHisSereServSub);
                    }
                }
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00521:");
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                resutl = false;
            }
            return resutl;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {

                ListRdo.AddRange((from r in listSereServ select new Mrs00521RDO(r, listHisPatientTypeAlter)).ToList() ?? new List<Mrs00521RDO>());
                ListRdo.AddRange((from r in listServiceReq select new Mrs00521RDO(r, listHisPatientTypeAlter)).ToList() ?? new List<Mrs00521RDO>());
                
                ListRdo.AddRange((from r in listExamTreatment select new Mrs00521RDO(r, listHisPatientTypeAlter, listSereServAb, this.castFilter)).ToList() ?? new List<Mrs00521RDO>());
                ListRdo = ListRdo.Where(o => o.TREATMENT_ID != 0).ToList();

                //SumRdo(ListRdo);
                if (listExamTreatment != null)
                {
                    var group = listExamTreatment.Select(p => p.ID).Distinct().ToList();
                    LogSystem.Info("groupCount: " + group.Count());
                    
                    foreach (var item in group)
                    {

                        var sereServ = listSereServ.FirstOrDefault(p => p.TDL_TREATMENT_ID == item) ?? new HIS_SERE_SERV();
                        var serviceReq = listServiceReq.FirstOrDefault(p => p.TREATMENT_ID == item) ?? new HIS_SERVICE_REQ();
                        var currentPatientTypeAlter = listHisPatientTypeAlter.FirstOrDefault(p => p.TREATMENT_ID == item) ?? new HIS_PATIENT_TYPE_ALTER(); ;
                        
                        Mrs00521RDO rdo = new Mrs00521RDO();
                        
                        if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                            && listSereServ.Where(p => HisServiceCFG.SERVICE_CODE__HIVs.Contains(p.TDL_SERVICE_CODE)).Count() > 0)
                        {
                            rdo.TOTAL_TREATMENT_HIV_TEST_EXAM = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_HIV_TEST_BHYT_EXAM = 1;
                            else
                                rdo.TOTAL_TREATMENT_HIV_TEST_VP_EXAM = 1;
                        }

                        else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                            && listSereServ.Where(p => HisServiceCFG.SERVICE_CODE__HIVs.Contains(p.TDL_SERVICE_CODE)).Count() > 0)
                        {
                            rdo.TOTAL_TREATMENT_HBSAG_TEST_EXAM = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_HBSAG_TEST_BHYT_EXAM = 1;
                            else
                                rdo.TOTAL_TREATMENT_HBSAG_TEST_VP_EXAM = 1;
                        }
                        else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU
                            && listSereServ.Where(p => HisServiceCFG.SERVICE_CODE__HIVs.Contains(p.TDL_SERVICE_CODE)).Count() > 0)
                        {
                            rdo.TOTAL_TREATMENT_HIV_TEST_TREATOUT = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_HIV_TEST_BHYT_TREATOUT = 1;
                            else
                                rdo.TOTAL_TREATMENT_HIV_TEST_VP_TREATOUT = 1;
                        }

                        else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU
                            && listSereServ.Where(p => HisServiceCFG.SERVICE_CODE__HIVs.Contains(p.TDL_SERVICE_CODE)).Count() > 0)
                        {
                            rdo.TOTAL_TREATMENT_HBSAG_TEST_TREATOUT = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_HBSAG_TEST_BHYT_TREATOUT = 1;
                            else
                                rdo.TOTAL_TREATMENT_HBSAG_TEST_VP_TREATOUT = 1;
                        }
                        else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                            && listSereServ.Where(p => HisServiceCFG.SERVICE_CODE__HIVs.Contains(p.TDL_SERVICE_CODE)).Count() > 0)
                        {
                            rdo.TOTAL_TREATMENT_HIV_TEST_TREATIN = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_HIV_TEST_BHYT_TREATIN = 1;
                            else
                                rdo.TOTAL_TREATMENT_HIV_TEST_VP_TREATIN = 1;
                        }

                        else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                            && listSereServ.Where(p => HisServiceCFG.SERVICE_CODE__HIVs.Contains(p.TDL_SERVICE_CODE)).Count() > 0)
                        {
                            rdo.TOTAL_TREATMENT_HBSAG_TEST_TREATIN = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_HBSAG_TEST_BHYT_TREATIN = 1;
                            else
                                rdo.TOTAL_TREATMENT_HBSAG_TEST_VP_TREATIN = 1;
                        }

                        rdo.TOTAL_TREATMENT_EXAM = 1;
                        if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            rdo.TOTAL_TREATMENT_EXAM_BHYT = 1;
                        else
                            rdo.TOTAL_TREATMENT_EXAM_VP = 1;

                        if (HisIcdCFG.ICD_CODE__FETUS_EXAM.Contains(serviceReq.ICD_CODE))
                        {
                            rdo.TOTAL_TREATMENT_FETUS_EXAM = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_FETUS_EXAM_BHYT = 1;
                            else
                                rdo.TOTAL_TREATMENT_FETUS_EXAM_VP = 1;
                        }
                        else 
                        {
                            rdo.TOTAL_TREATMENT_GYNECOLOGICAL_EXAM = 1;
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                rdo.TOTAL_TREATMENT_GYNECOLOGICAL_EXAM_BHYT = 1;
                            else
                                rdo.TOTAL_TREATMENT_GYNECOLOGICAL_EXAM_VP = 1;
                        }

                        if ((castFilter.ICD_CODE__PROCREATE_COMMONs != null && castFilter.ICD_CODE__PROCREATE_COMMONs.Contains(serviceReq.ICD_CODE))
                            || (castFilter.ICD_CODE__PROCREATE_DIFFICULTs != null && castFilter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(serviceReq.ICD_CODE))
                            || (castFilter.ICD_CODE__PROCREATE_SURGs != null && castFilter.ICD_CODE__PROCREATE_SURGs.Contains(serviceReq.ICD_CODE)))
                        {
                            if ((castFilter.ICD_CODE__PROCREATE_COMMONs != null && castFilter.ICD_CODE__PROCREATE_COMMONs.Contains(serviceReq.ICD_CODE))
                                || (castFilter.ICD_CODE__PROCREATE_DIFFICULTs != null && castFilter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(serviceReq.ICD_CODE)))
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_PROCREATE = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_PROCREATE = 1;
                                }
                                if ((castFilter.ICD_CODE__PROCREATE_DIFFICULTs != null && castFilter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(serviceReq.ICD_CODE)))
                                {
                                    if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        rdo.TOTAL_TREATMENT_BHYT_PROCREATE_DIFFICULT = 1;
                                    }
                                    else
                                    {
                                        rdo.TOTAL_TREATMENT_VP_PROCREATE_DIFFICULT = 1;
                                    }

                                }

                            }
                            else
                            {

                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_PROCREATE_SURG = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_PROCREATE_SURG = 1;
                                }
                            }
                            if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_EXAM_PROC = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_EXAM_PROC = 1;
                                }
                            }
                            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_NGTRU_PROC = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_NGTRU_PROC = 1;
                                }
                            }
                            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_NOITRU_PROC = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_NOITRU_PROC = 1;
                                }
                            }
                        }
                        else if ((castFilter.ICD_CODE__GYNECOLOGICALs != null && castFilter.ICD_CODE__GYNECOLOGICALs.Contains(serviceReq.ICD_CODE)))
                        {
                            if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_EXAM_GYNECOLOGICAL = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_EXAM_GYNECOLOGICAL = 1;
                                }
                            }
                            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_TREAT_NGTRU_GYNECOLOGICAL = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_TREAT_NGTRU_GYNECOLOGICAL = 1;
                                }
                            }
                            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.TOTAL_TREATMENT_BHYT_TREAT_NOITRU_GYNECOLOGICAL = 1;
                                }
                                else
                                {
                                    rdo.TOTAL_TREATMENT_VP_TREAT_NOITRU_GYNECOLOGICAL = 1;
                                }
                            }
                        }
                        else if ((castFilter.ICD_CODE__GYNECOLOGICAL_SURGs != null && castFilter.ICD_CODE__GYNECOLOGICAL_SURGs.Contains(serviceReq.ICD_CODE)))
                        {
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.TOTAL_TREATMENT_BHYT_EXAM_GYNECOLOGICAL_SURG = 1;
                            }
                            else
                            {
                                rdo.TOTAL_TREATMENT_VP_EXAM_GYNECOLOGICAL_SURG = 1;
                            }
                        }


                        if ((castFilter.SERVICE_CODE__ABORTIONs != null && listSereServAb != null && listSereServAb.Where(o => castFilter.SERVICE_CODE__ABORTIONs.Contains(o.TDL_SERVICE_CODE)) != null))
                        {
                            if (currentPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.TOTAL_TREATMENT_BHYT_ABORTION = 1;
                            }
                            else
                            {
                                rdo.TOTAL_TREATMENT_BHYT_ABORTION = 1;
                            }
                        }

                        ListRdo1.Add(rdo);
                    }
                    
                }
                
                //SumRdo1(ListRdo1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void SumRdo(List<Mrs00521RDO> listRdo)
        {
            string errorField = "";
            try
            {
                decimal sum = 0;
                
                Mrs00521RDO rdo =new Mrs00521RDO();
                listRdo = listRdo.Distinct().ToList();
                
                PropertyInfo[] pi = Properties.Get<Mrs00521RDO>();
                
                    foreach (var field in pi)
                    {
                        if (field.Name == "TREATMENT_ID") continue;
                        errorField = field.Name;

                        sum = ListRdo.Sum(s => (decimal)field.GetValue(s));
                            field.SetValue(rdo, sum);
                            
                        
                    }
                    
                    sumRdo.Add(rdo);
                    
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SumRdo1(List<Mrs00521RDO> listRdo1)
        {
            string errorField = "";
            try
            {
               
                decimal sum1 = 0;
                Mrs00521RDO rdo = new Mrs00521RDO();

                listRdo1 = listRdo1.Distinct().ToList();
                PropertyInfo[] pi1 = Properties.Get<Mrs00521RDO>();

                foreach (var field in pi1)
                {
                    if (field.Name == "TREATMENT_ID") continue;
                    errorField = field.Name;

                    sum1 = ListRdo.Sum(s => (decimal)field.GetValue(s));
                    field.SetValue(rdo, sum1);


                }

                sumRdo1.Add(rdo);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", sumRdo);
                objectTag.AddObjectData(store, "Report1", sumRdo1);
                LogSystem.Info("Report2: " + ListRdo1.Count());
                objectTag.AddObjectData(store, "Report2", ListRdo1);
                if ((castFilter.DEPARTMENT_ID ?? 0) != 0) dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

   
}
/*select count(1) from his_treatment trea where icd_code in ('O82','O80','O83') and exists (select 1 from his_patient_type_alter where trea.id = treatment_id and treatment_type_id >2)
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('O82','O80','O83') and not exists (select 1 from his_patient_type_alter where trea.id = treatment_id and treatment_type_id >2)
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('N84', 'N72','N70','N76', 'O20','O21') and exists (select 1 from his_patient_type_alter where trea.id = treatment_id and treatment_type_id >2)
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('N84', 'N72','N70','N76', 'O20','O21') and not exists (select 1 from his_patient_type_alter where trea.id = treatment_id and treatment_type_id >2)
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('O80', 'O83') 
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('O83') 
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('O82') 
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where icd_code in ('D24', 'D25', 'D26') 
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;
select count(1) from his_treatment trea where exists (select 1 from his_sere_serv where (is_no_execute <>1 or is_no_execute is null)
and tdl_treatment_id = trea.id and tdl_service_code in( 'TT945', 'TT942'))
and trea.end_department_id in (select id from his_department where department_code = 'KS') and trea.is_pause=1 and trea.out_time between 20180101000000 and 20180630235959;*/