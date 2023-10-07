using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00400
{
    class Mrs00400Processor : AbstractProcessor
    {
        Mrs00400Filter castFilter = null; 
        List<Mrs00400RDO> listRdo = new List<Mrs00400RDO>(); 
        List<Mrs00400RDO> listRdoGroup = new List<Mrs00400RDO>(); 
        List<Mrs00400RDO> listGroup = new List<Mrs00400RDO>(); 
        List<Mrs00400RDO> listTotal = new List<Mrs00400RDO>(); 

        public Mrs00400Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_TREATMENT> listDeaths = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_PATIENT> listPatients = new List<V_HIS_PATIENT>(); 
        HIS_BRANCH listBrand = new HIS_BRANCH(); 

        public override Type FilterType()
        {
            return typeof(Mrs00400Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00400Filter)this.reportFilter; 

                listBrand = new MOS.MANAGER.HisBranch.HisBranchManager(paramGet).GetById(this.castFilter.BRANCH_ID); 

                var skip = 0; 
                //V_HIS_TREATMENT
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.IN_TIME_FROM = this.castFilter.TIME_FROM; 
                treatmentFilter.IN_TIME_TO = this.castFilter.TIME_TO; 

                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 
                HisTreatmentViewFilterQuery treatmentFilter1 = new HisTreatmentViewFilterQuery(); 
                treatmentFilter1.OUT_TIME_FROM = this.castFilter.TIME_FROM; 
                treatmentFilter1.OUT_TIME_TO = this.castFilter.TIME_TO; 
                var listTreatmentOutTime = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter1); 
                listTreatments.AddRange(listTreatmentOutTime); 
                listTreatments = listTreatments.Distinct().ToList(); 


                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList(); 
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listTreatmentId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterFilter.TREATMENT_IDs = listTreatmentId; 
                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter); 
                    listPatientTypeAlters.AddRange(listPatientTypeAlter); 

                    listDeaths = listTreatments.Where(o => o.DEATH_WITHIN_ID != null).ToList(); 
                }

                var listPatientIds = listTreatments.Select(s => s.PATIENT_ID).ToList(); 
                skip = 0; 
                while (listPatientIds.Count - skip > 0)
                {
                    var listPatientId = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisPatientViewFilterQuery patientFilter = new HisPatientViewFilterQuery(); 
                    patientFilter.IDs = listPatientId; 
                    var listPatient = new MOS.MANAGER.HisPatient.HisPatientManager(paramGet).GetView(patientFilter); 
                    listPatients.AddRange(listPatient); 

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
                if (IsNotNullOrEmpty(listTreatments))
                {
                    foreach (var treatment in listTreatments)
                    {
                        var listPatientTypeAlter = listPatientTypeAlters.Where(s => s.TREATMENT_ID == treatment.ID).ToList(); 
                        var death = listDeaths.Where(s => s.ID == treatment.ID).ToList(); 
                        var listPatient = listPatients.Where(s => s.ID == treatment.PATIENT_ID).ToList(); 

                        foreach (var patientTypeAlter in listPatientTypeAlter)
                        {
                            foreach (var patient in listPatient)
                            {
                                
                                    Mrs00400RDO rdo = new Mrs00400RDO(); 
                                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && treatment.OUT_TIME >= this.castFilter.TIME_FROM && treatment.OUT_TIME<= this.castFilter.TIME_TO)
                                {
                                    //rdo.PROVINCE_NAME = patient.PROVINCE_NAME; 
                                    //if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                    //{
                                    //    rdo.TOTAL_EXAM = 1; 
                                    //}
                                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                    {
                                        rdo.TOTAL_IN = 1;
                                        rdo.IN_TREATMENT_TIME = DateDiff.diffDate(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME); 
                                    }
                                    //if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                                    //{
                                    //    rdo.TOTAL_OUT = 1; 
                                    //    rdo.OUT_TREATMENT_TIME = HIS.Common.Treatment.Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME); 
                                    //}
                                    if (death.Count > 0)
                                    {
                                        rdo.TOTAL_DEATH = 1; 
                                    }
                                    if (patient.PROVINCE_CODE == listBrand.HEIN_PROVINCE_CODE)
                                    {
                                        rdo.PROVINCE_TYPE = "CƠ SỞ"; 
                                    }
                                    else
                                    {
                                        rdo.PROVINCE_TYPE = "TỈNH (*)"; 
                                    }
                                }
                                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                {
                                    rdo.PROVINCE_NAME = patient.PROVINCE_NAME??"z Tỉnh khác"; 
                                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                    {
                                        rdo.TOTAL_EXAM = 1; 
                                    }
                                    //if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                    //{
                                    //    rdo.TOTAL_IN = 1; 
                                    //    rdo.IN_TREATMENT_TIME = HIS.Common.Treatment.Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME); 
                                    //}
                                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                                    {
                                        rdo.TOTAL_OUT = 1;
                                        rdo.OUT_TREATMENT_TIME = DateDiff.diffDate(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME); 
                                    }
                                    if (death.Count > 0)
                                    {
                                        rdo.TOTAL_DEATH = 1; 
                                    }
                                    if (patient.PROVINCE_CODE == listBrand.HEIN_PROVINCE_CODE)
                                    {
                                        rdo.PROVINCE_TYPE = "CƠ SỞ"; 
                                    }
                                    else
                                    {
                                        rdo.PROVINCE_TYPE = "TỈNH (*)"; 
                                    }
                                }
                                    
                                    listRdo.Add(rdo); 

                                }

                            }

                        }

                    listRdoGroup = listRdo.GroupBy(gr => new
                    {
                        gr.PROVINCE_TYPE,
                        gr.PROVINCE_NAME,
                    }).Select(s => new Mrs00400RDO
                    {
                        PROVINCE_NAME = s.Key.PROVINCE_NAME,
                        PROVINCE_TYPE = s.Key.PROVINCE_TYPE,
                        TOTAL_EXAM = s.Sum(c => c.TOTAL_EXAM),
                        TOTAL_IN = s.Sum(c => c.TOTAL_IN),
                        IN_TREATMENT_TIME = s.Sum(c => c.IN_TREATMENT_TIME),
                        TOTAL_OUT = s.Sum(c => c.TOTAL_OUT),
                        OUT_TREATMENT_TIME = s.Sum(c => c.OUT_TREATMENT_TIME),
                        TOTAL_DEATH = s.Sum(c => c.TOTAL_DEATH)
                    }).ToList(); 

                    listGroup = listRdo.GroupBy(g => new { g.PROVINCE_TYPE}).Select(s => new Mrs00400RDO
                    {
                        PROVINCE_TYPE = s.Key.PROVINCE_TYPE,
                        TOTAL_EXAM = s.Sum(c => c.TOTAL_EXAM),
                        TOTAL_IN = s.Sum(c => c.TOTAL_IN),
                        IN_TREATMENT_TIME = s.Sum(c => c.IN_TREATMENT_TIME),
                        TOTAL_OUT = s.Sum(c => c.TOTAL_OUT),
                        OUT_TREATMENT_TIME = s.Sum(c => c.OUT_TREATMENT_TIME),
                        TOTAL_DEATH = s.Sum(c => c.TOTAL_DEATH)
                    }).ToList(); 

                    listTotal = listRdo.GroupBy(g => 1).Select(s => new Mrs00400RDO
                    {
                        TOTAL_EXAM = s.Sum(c => c.TOTAL_EXAM),
                        TOTAL_IN = s.Sum(c => c.TOTAL_IN),
                        IN_TREATMENT_TIME = s.Sum(c => c.IN_TREATMENT_TIME),
                        TOTAL_OUT = s.Sum(c => c.TOTAL_OUT),
                        OUT_TREATMENT_TIME = s.Sum(c => c.OUT_TREATMENT_TIME),
                        TOTAL_DEATH = s.Sum(c => c.TOTAL_DEATH)
                    }).ToList(); 

                    }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Total", listTotal); 
                objectTag.AddObjectData(store, "Report", listRdoGroup.OrderBy(s => s.PROVINCE_NAME).ToList()); 
                objectTag.AddObjectData(store, "Group", listGroup.OrderBy(s => s.PROVINCE_TYPE).ToList()); 
                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "PROVINCE_TYPE", "PROVINCE_TYPE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

}
