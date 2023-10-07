using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
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
using MRS.MANAGER.Config;
//using MOS.MANAGER.HisEmergency; 


namespace MRS.Processor.Mrs00253
{
    public class Mrs00253Processor : AbstractProcessor
    {
        private List<Mrs00253RDO> ListRdo = new List<Mrs00253RDO>();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_TREATMENT> dicTreatmeant = new Dictionary<long, V_HIS_TREATMENT>();


        CommonParam paramGet = new CommonParam();
        public Mrs00253Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00253Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00253Filter)reportFilter);
            var result = true;
            try
            {
                //HisDepartmentFilterQuery listDepartmentfilter = new HisDepartmentFilterQuery()
                //{
                //    IDs = filter.DEPARTMENT_IDs
                //}; 
                //listDepartment = new HisDepartmentManager(paramGet).Get(listDepartmentfilter); 

                var treatmentFilter = new HisTreatmentViewFilterQuery
                {
                    IN_TIME_FROM = filter.TIME_FROM,
                    IN_TIME_TO = filter.TIME_TO
                };
                listTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                if (IsNotNullOrEmpty(listTreatment))
                {
                    foreach (var item in listTreatment)
                    {
                        dicTreatmeant[item.ID] = item;
                    }
                }

                //Lấy danh sách bệnh nhân theo khoa
                var listTreatmentIds = listTreatment.Select(s => s.ID).ToList();

                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var departmentTranViewFilter = new HisDepartmentTranViewFilterQuery
                    {
                        TREATMENT_IDs = listIDs,
                        IS_RECEIVE = true
                    };
                    var departmentTranViews = new HisDepartmentTranManager(paramGet).GetView(departmentTranViewFilter);
                    listDepartmentTrans.AddRange(departmentTranViews);
                }

                //Lay danh sach patient_tye_alter tuong ung
                patientTypeAlters = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(listTreatmentIds);
                //if (IsNotNullOrEmpty(patientTypeAlters))
                //{
                //    foreach (var item in patientTypeAlters)
                //    {
                //        if (!dicPatientTypeAlters.ContainsKey(item.DEPARTMENT_TRAN_ID))
                //            dicPatientTypeAlters[item.DEPARTMENT_TRAN_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                //        dicPatientTypeAlters[item.DEPARTMENT_TRAN_ID].Add(item);
                //    }
                //}
                //Lay danh sach Emergency tuong ung
                //Emergencys = new HisEmergencyManager(paramGet).GetViewByTreatmentIds(listTreatmentIds); 
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
                var groupByDepartmentId = listDepartmentTrans.GroupBy(o => o.DEPARTMENT_ID).ToList();

                foreach (var group in groupByDepartmentId)
                {
                    List<V_HIS_DEPARTMENT_TRAN> listSub = group.ToList<V_HIS_DEPARTMENT_TRAN>();
                    Mrs00253RDO rdo = new Mrs00253RDO();

                    rdo.DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                    rdo.COUNT_TOTAL = listSub.Where(o => isExam(o)).ToList().Count;
                    rdo.COUNT_BHYT = listSub.Where(o => isExamBHYT(o)).ToList().Count;
                    rdo.COUNT_DV = listSub.Where(o => isExamVP(o)).ToList().Count;
                    rdo.COUNT_KH = listSub.Where(o => !isExamVP(o) && !isExamBHYT(o) && isExam(o)).ToList().Count;
                    rdo.COUNT_EM = listSub.Where(o => isExamEm(o)).ToList().Count;
                    rdo.COUNT_IN = listSub.Where(o => isExamThenInTreat(o)).ToList().Count;
                    rdo.COUNT_TRAN = listSub.Where(o => isExamThenTran(o)).ToList().Count;
                    rdo.COUNT_DIE = listSub.Where(o => isExamThenDie(o)).ToList().Count;
                    rdo.COUNT_TOTAL_DT = listSub.Where(o => isExamThenOutTreat(o)).ToList().Count;
                    rdo.COUNT_TIME = listSub.Select(o => TIME(o)).ToList().Sum();

                    ListRdo.Add(rdo);
                }
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00253Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00253Filter)this.reportFilter).TIME_TO));

            objectTag.AddObjectData(store, "Report", ListRdo);
        }

        private bool isExam(V_HIS_DEPARTMENT_TRAN depTran)
        {
            return this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
           
        }

        private bool isExamBHYT(V_HIS_DEPARTMENT_TRAN depTran)
        {
            return this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                && this.patientTypeId(depTran, patientTypeAlters) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        }

        private bool isExamVP(V_HIS_DEPARTMENT_TRAN depTran)
        {
            return this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                 && this.patientTypeId(depTran, patientTypeAlters) == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        }

        private bool isExamEm(V_HIS_DEPARTMENT_TRAN depTran)
        {
            bool result = false;
            if (dicTreatmeant.ContainsKey(depTran.TREATMENT_ID) &&
                dicTreatmeant[depTran.TREATMENT_ID].IS_EMERGENCY == 1)
            {
                return this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
            }
            return result;
        }

        private bool isExamThenInTreat(V_HIS_DEPARTMENT_TRAN depTran)
        {
            bool result = false;
            
                if (this.treatmentTypeId(NextDepartment(depTran), patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    if (this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        return true;
                    }
                }
            return result;
        }

        private bool isExamThenTran(V_HIS_DEPARTMENT_TRAN depTran)
        {
            bool result = false;
            
                if (dicTreatmeant.ContainsKey(depTran.TREATMENT_ID) &&
                dicTreatmeant[depTran.TREATMENT_ID].END_DEPARTMENT_ID == depTran.DEPARTMENT_ID
                && dicTreatmeant[depTran.TREATMENT_ID].TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                {
                    if (this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        return true;
                    }
                }
            return result;
        }
        private bool isExamThenDie(V_HIS_DEPARTMENT_TRAN depTran)
        {
            bool result = false;

            if (dicTreatmeant.ContainsKey(depTran.TREATMENT_ID) &&
            dicTreatmeant[depTran.TREATMENT_ID].END_DEPARTMENT_ID == depTran.DEPARTMENT_ID
            && dicTreatmeant[depTran.TREATMENT_ID].TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
            {
                if (this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    return true;
                }
            }
            return result;
        }

        private bool isExamThenOutTreat(V_HIS_DEPARTMENT_TRAN depTran)
        {
            bool result = false;

            if (this.treatmentTypeId(NextDepartment(depTran), patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
            {
                if (this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    return true;
                }
            }
            return result;
        }
        private double TIME(V_HIS_DEPARTMENT_TRAN depTran)
        {
            double result = 0;
            try
            {
                if (this.treatmentTypeId(NextDepartment(depTran), patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    if (this.treatmentTypeId(depTran, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        long IN_TIME = NextDepartment(depTran).DEPARTMENT_IN_TIME ?? 0;
                        long OUT_TIME = NextDepartment(NextDepartment(depTran)).DEPARTMENT_IN_TIME ?? 0;

                        if (OUT_TIME > 0 && IN_TIME > 0)
                        {
                            result = MyDateDiff(IN_TIME, OUT_TIME);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return 0;
            }
            return result;
        }

        private double MyDateDiff(long intime, long outtime)
        {
            double result = 0;
            try
            {
                if (outtime < intime) return 0;
                DateTime InTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(intime);
                DateTime OutTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(outtime);
                TimeSpan sp = OutTime - InTime;
                var days = sp.TotalDays;
                if ((days - (long)days) < 0.25)
                {
                    result = (long)days;
                }
                else if ((days - (long)days) < 0.75)
                {
                    result = (long)days + 0.5;
                }
                else
                {
                    result = (long)days + 1;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }
        //Dien dieu tri
        private long treatmentTypeId(V_HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            long result = 0;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID;
                }
                else
                {
                    patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                        && o.LOG_TIME > departmentTran.DEPARTMENT_IN_TIME
                        && o.LOG_TIME < (NextDepartment(departmentTran).DEPARTMENT_IN_TIME ?? 99999999999999)).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().TREATMENT_TYPE_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        //Doi tuong BN
        private long patientTypeId(V_HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            long result = 0;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().PATIENT_TYPE_ID;
                }
                else
                {
                    patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                        && o.LOG_TIME > departmentTran.DEPARTMENT_IN_TIME
                        && o.LOG_TIME < (NextDepartment(departmentTran).DEPARTMENT_IN_TIME ?? 99999999999999)).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().PATIENT_TYPE_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        //khoa lien ke
        private V_HIS_DEPARTMENT_TRAN NextDepartment(V_HIS_DEPARTMENT_TRAN o)
        {

            return listDepartmentTrans.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new V_HIS_DEPARTMENT_TRAN();

        }
       

       
    }
}
