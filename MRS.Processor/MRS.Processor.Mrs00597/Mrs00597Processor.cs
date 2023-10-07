using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTranPatiReason;
using MOS.MANAGER.HisTranPatiForm;
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

using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
//using MOS.MANAGER.HisTranPati; 
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;


namespace MRS.Processor.Mrs00597
{
    public class Mrs00597Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00597RDO> ListRdo = new List<Mrs00597RDO>();
        private List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        private List<V_HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_DEPARTMENT_TRAN> ListNextDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        private Dictionary<long,List<HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long,List<HIS_PATIENT_TYPE_ALTER>>();
        private List<HIS_PATIENT_TYPE_ALTER> LastPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        public Mrs00597Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00597Filter);
        }

        protected override bool GetData()///
        {
            var filter = ((Mrs00597Filter)reportFilter);
            var result = true;
            try
            {
                HisDepartmentTranViewFilterQuery filtermain = new HisDepartmentTranViewFilterQuery();
                filtermain.DEPARTMENT_IN_TIME_FROM = filter.TIME_FROM;
                filtermain.DEPARTMENT_IN_TIME_TO = filter.TIME_TO;
                ListNextDepartmentTran = new HisDepartmentTranManager(paramGet).GetView(filtermain);

                var ListId = ListNextDepartmentTran.Where(p => p.PREVIOUS_ID.HasValue).Select(o => o.PREVIOUS_ID.Value).Distinct().ToList();

                //khoa trước đó

                if (IsNotNullOrEmpty(ListId))
                {
                    var skip = 0;
                    while (ListId.Count - skip > 0)
                    {
                        var listIDs = ListId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranViewFilterQuery NextDepartmentTranfilter = new HisDepartmentTranViewFilterQuery()
                        {
                            IDs = listIDs,
                            DEPARTMENT_ID=filter.DEPARTMENT_ID,
                            DEPARTMENT_IDs=filter.DEPARTMENT_IDs
                        };
                        var ListDepartmentTranSub = new HisDepartmentTranManager(paramGet).GetView(NextDepartmentTranfilter);
                        if (IsNotNullOrEmpty(ListDepartmentTranSub))
                            listHisDepartmentTran.AddRange(ListDepartmentTranSub);
                    }
                }
                if (IsNotNullOrEmpty(listHisDepartmentTran))
                {
                    var departmentTranIds = listHisDepartmentTran.Select(o => o.ID).ToList();
                    ListNextDepartmentTran = ListNextDepartmentTran.Where(o => departmentTranIds.Contains(o.PREVIOUS_ID??0)).ToList();
                }

                var ListTreatmentId = listHisDepartmentTran.Select(o => o.TREATMENT_ID).Distinct().ToList();

                //Đối tượng điều trị

                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery()
                        {
                            IDs = listIDs,
                            ORDER_DIRECTION = "ASC",
                            ORDER_FIELD = "ID"
                        };
                        var LisTreatmentLib = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                        if (IsNotNullOrEmpty(LisTreatmentLib))
                        ListTreatment.AddRange(LisTreatmentLib);
                       
                        HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            ORDER_DIRECTION = "ASC",
                            ORDER_FIELD = "ID"
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                        if (IsNotNullOrEmpty(LisPatientTypeAlterLib))
                        ListPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }
                    LastPatientTypeAlter = ListPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q=>q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                    dicPatientTypeAlter = ListPatientTypeAlter.GroupBy(o => o.TREATMENT_ID).ToDictionary(q=>q.Key,p => p.ToList());
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    var lastPatientTypeAlterIds = LastPatientTypeAlter.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID)).Select(p => p.TREATMENT_ID).Distinct().ToList();
                    listHisDepartmentTran = listHisDepartmentTran.Where(o => lastPatientTypeAlterIds.Contains(o.TREATMENT_ID)).ToList();
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
                ListRdo.Clear();

                foreach (var item in listHisDepartmentTran)
                {
                    var listPatientTypeAlterSub = dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID) ? dicPatientTypeAlter[item.TREATMENT_ID] : new List<HIS_PATIENT_TYPE_ALTER>();
                    if (!this.HasTreatIn(item, listPatientTypeAlterSub))
                    {
                        continue;
                    }
                    var treatment = ListTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                    var departmentTran = ListNextDepartmentTran.FirstOrDefault(o => o.PREVIOUS_ID == item.ID);
                    if (treatment != null && departmentTran != null)
                    {
                        Mrs00597RDO rdo = new Mrs00597RDO(departmentTran, treatment);
                        rdo.FROM_DEPARTMENT_CODE = item.DEPARTMENT_CODE;
                        rdo.FROM_DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                        //string totalDay = (departmentTran.DEPARTMENT_IN_TIME??treatment.IN_DATE - treatment.IN_DATE).ToString();
                        //rdo.TOTAL_DAYS_IN_TREATMENT = totalDay.Length > 6 ? (totalDay.Length == 7 ? totalDay.Remove(1,6) : totalDay.Remove(2,7)) : "0";
                        rdo.TOTAL_DAYS_IN_TREATMENT = Inventec.Common.DateTime.Calculation.DifferenceDate(treatment.IN_DATE,departmentTran.DEPARTMENT_IN_TIME ?? treatment.IN_DATE).ToString();
                        rdo.IN_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_DATE);
                        var listPatientTypeAlterHasCard = listPatientTypeAlterSub.Where(o =>o.HEIN_CARD_NUMBER != null).ToList();
                        if (IsNotNullOrEmpty(listPatientTypeAlterHasCard))
                        {
                            rdo.IS_BHYT = "x";
                        }

                        ListRdo.Add(rdo);
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        //Co dieu tri noi tru (tam thoi cat di va dung clinical_in_time, tdl_treatment_type_id trong his_treatment)
        private bool HasTreatIn(V_HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                   && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00597Filter)reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00597Filter)reportFilter).TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ((Mrs00597Filter)reportFilter).DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME);

            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o=>o.DEPARTMENT_IN_TIME).ToList());
        }

    }


}
