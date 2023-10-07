using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMedicineType;
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
using MOS.MANAGER.HisDepartmentTran;

namespace MRS.Processor.Mrs00395
{
    class Mrs00395Processor : AbstractProcessor
    {
        Mrs00395Filter castFilter = null;
        List<Mrs00395RDO> listRdo = new List<Mrs00395RDO>();
        List<Mrs00395RDO> listRdoDetail = new List<Mrs00395RDO>();
        List<Mrs00395RDO> listRdoGroup = new List<Mrs00395RDO>();
        List<Mrs00395RDO> listRdoGroups = new List<Mrs00395RDO>();

        public Mrs00395Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT> listPatients = new List<V_HIS_PATIENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>();

        public override Type FilterType()
        {
            return typeof(Mrs00395Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00395Filter)this.reportFilter;

                var skip = 0;
                //V_HIS_TREATMENT
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.IN_TIME_FROM = this.castFilter.TIME_FROM;
                treatmentFilter.IN_TIME_TO = this.castFilter.TIME_TO;

                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList();
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listTreatmentId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                    patientTypeAlterFilter.TREATMENT_IDs = listTreatmentId;
                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                    listPatientTypeAlters.AddRange(listPatientTypeAlter);
                    HisDepartmentTranViewFilterQuery DepartmentTranFilter = new HisDepartmentTranViewFilterQuery();
                    DepartmentTranFilter.TREATMENT_IDs = listTreatmentId;
                    var listDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(DepartmentTranFilter);
                    listDepartmentTrans.AddRange(listDepartmentTran);
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

                        foreach (var patientTypeAlter in listPatientTypeAlter)
                        {
                            Mrs00395RDO rdo = new Mrs00395RDO();

                            Inventec.Common.Mapper.DataObjectMapper.Map<V_HIS_TREATMENT>(rdo, treatment);
                            var dpt = listDepartmentTrans.OrderByDescending(o=>o.ID).FirstOrDefault(s => s.TREATMENT_ID == treatment.ID);
                            if (dpt != null)
                            {
                                rdo.END_DEPARTMENT_NAME = dpt.DEPARTMENT_NAME;
                            }
                            rdo.NATIONAL_NAME = treatment.TDL_PATIENT_NATIONAL_NAME;
                            if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                rdo.TOTAL_EXAM = 1;
                            }
                            if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.TOTAL_IN = 1;
                            }
                            if (treatment.CLINICAL_IN_TIME != null)
                            {
                                rdo.TREATMENT_TIME = DateDiff.diffDate(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME);
                            }
                            rdo.ICD_10 = treatment.ICD_CODE;

                            listRdo.Add(rdo);
                            listRdoDetail.Add(rdo);
                        }

                    }

                    listRdo = listRdo.Where(s => s.NATIONAL_NAME != MANAGER.Config.HisMedicineTypeCFG.MEDICINE_TYPE_NATIOANL_VN).ToList();

                    if (IsNotNullOrEmpty(listRdo))
                    {
                        listRdoGroups = listRdo.GroupBy(gr => new
                        {
                            gr.NATIONAL_NAME,
                            gr.ICD_10,
                        }).Select(s => new Mrs00395RDO
                        {
                            NATIONAL_NAME = s.Key.NATIONAL_NAME,
                            ICD_10 = s.Key.ICD_10,
                            TOTAL_IN = s.Sum(c => c.TOTAL_IN),
                            TREATMENT_TIME = s.Sum(c => c.TREATMENT_TIME)
                        }).ToList();

                        listRdoGroup = listRdo.GroupBy(gr => new
                        {
                            gr.NATIONAL_NAME,
                        }).Select(s => new Mrs00395RDO
                        {
                            NATIONAL_NAME = s.Key.NATIONAL_NAME,
                            TOTAL_EXAM = s.Sum(c => c.TOTAL_EXAM)
                        }).ToList();

                        var listResult = from t1 in listRdoGroup
                                         join t2 in listRdoGroups
                                         on t1.NATIONAL_NAME equals t2.NATIONAL_NAME
                                         select new Mrs00395RDO
                                         {
                                             NATIONAL_NAME = t2.NATIONAL_NAME,
                                             TOTAL_EXAM = t1.TOTAL_EXAM,
                                             TOTAL_IN = t2.TOTAL_IN,
                                             TREATMENT_TIME = t2.TREATMENT_TIME,
                                             ICD_10 = t2.ICD_10
                                         };
                        listRdo.Clear();
                        listRdo.AddRange(listResult);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(s => s.NATIONAL_NAME).ToList());
                objectTag.AddObjectData(store, "Detail", listRdoDetail.OrderBy(s => s.NATIONAL_NAME).ToList());
                objectTag.SetUserFunction(store, "MergeManyCellFunc", new MergeCellMany());
                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class MergeCellMany : FlexCel.Report.TFlexCelUserFunction
    {
        public MergeCellMany() { }

        string s_Result;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                string s_param = Convert.ToString(parameters[0]);

                if (s_Result == s_param)
                {
                    return true;
                }
                else
                {
                    s_Result = s_param;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return false;
        }
    }
}
