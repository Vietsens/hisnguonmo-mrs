using IcdVn;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00661
{
    class Mrs00661Processor : AbstractProcessor
    {
        private Mrs00661Filter filter;
        private List<Mrs00661RDO> listMrs00661Rdos = new List<Mrs00661RDO>();
        private List<Mrs00661RDO> ListRdos = new List<Mrs00661RDO>();
        private List<HIS_TREATMENT> lisTreatments = new List<HIS_TREATMENT>();
        private List<HIS_ICD> listIcds = new List<HIS_ICD>();
        private List<HIS_TREATMENT> listDeaths = new List<HIS_TREATMENT>();
        private List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        private List<HIS_TREATMENT_D> lisTreatmentDirect = new List<HIS_TREATMENT_D>();
        List<SUM_ICD_OUT_TREAT> listSumIcdOutTreat = new List<SUM_ICD_OUT_TREAT>();
        List<SUM_ICD_IN_TREAT> listSumIcdInTreat = new List<SUM_ICD_IN_TREAT>();
        List<SUM_ICD_IN_TREAT> listSumNNN = new List<SUM_ICD_IN_TREAT>();
        List<SUM_ICD_IN_TREAT> listSumNTV = new List<SUM_ICD_IN_TREAT>();
        string sql = "";
        public Mrs00661Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00661Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00661Filter)reportFilter;
            try
            {
                CommonParam paramGet = new CommonParam();
                sql = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 1);
                lisTreatmentDirect = new ManagerSql().GetSum(filter,sql)  ?? new List<HIS_TREATMENT_D>();
                if (lisTreatmentDirect.Count > 0)
                {
                    return true;
                }
                
                //--------------------------------------------------------------------------------------------------HIS_TREATMENT
                var treatmentFilter = new HisTreatmentFilterQuery
                {
                    IS_PAUSE = true,
                    OUT_TIME_FROM = filter.TIME_FROM,
                    OUT_TIME_TO = filter.TIME_TO,
                };
                if (filter.END_DEPARTMENT_ID.HasValue)
                {
                    treatmentFilter.END_DEPARTMENT_IDs = new List<long>() { filter.END_DEPARTMENT_ID.Value };
                }

                lisTreatments = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                //--------------------------------------------------------------------------------------------------HIS_ICD


                var hisIcdFilter = new HisIcdFilterQuery();
                listIcds = new HisIcdManager(paramGet).Get(hisIcdFilter);

                //--------------------------------------------------------------------------------------------------HIS_PATIENT_TYPE_ALTER
                var listTreatmentIds = lisTreatments.Select(s => s.ID).ToList();
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var patientTypeAlterViews = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                    listPatientTypeAlters.AddRange(patientTypeAlterViews);
                }
                //--------------------------------------------------------------------------------------------------

                listDeaths = lisTreatments.Where(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET || o.TREATMENT_END_TYPE_ID == HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH).ToList();
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
            var result = true;
            try
            {
                if (lisTreatmentDirect != null&&sql.StartsWith(" --Ngoaitru"))
                {
                    var groupByIcdCodeAndIcdCauseCode = lisTreatmentDirect.GroupBy(o => new { o.icd_code,o.icd_cause_code}).ToList();
                    foreach (var item in groupByIcdCodeAndIcdCauseCode)
                    {
                        List<HIS_TREATMENT_D> listSub = item.ToList<HIS_TREATMENT_D>();
                        SUM_ICD_OUT_TREAT rdo = new SUM_ICD_OUT_TREAT();
                        rdo.ICD_CODE = listSub.First().icd_code;
                        rdo.ICD_CAUSE_CODE = listSub.First().icd_cause_code;
                        rdo.ICD_NAME = listSub.First().icd_name;
                        rdo.ICDVN_CODE = new IcdVnIcd(listSub.First().icd_code).ICDVN_CODE;
                        rdo.TOTAL = listSub.Count;
                        rdo.TOTAL_BHYT = listSub.Count(o => o.tdl_patient_type_id == 1);
                        rdo.TOTAL_CHILD = listSub.Count(o => o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_DEATH = listSub.Count(o => o.treatment_end_type_id == 1);
                        rdo.TOTAL_FEMALE = listSub.Count(o => o.tdl_patient_gender_id == 1);
                        listSumIcdOutTreat.Add(rdo);
                    }
                    return true;
                }
                if (lisTreatmentDirect != null && sql.StartsWith(" --Noitru"))
                {
                    var groupByIcdVnCode = lisTreatmentDirect.GroupBy(o => new { new IcdVnIcd(o.icd_code).ICDVN_CODE }).ToList();
                    foreach (var item in groupByIcdVnCode)
                    {
                        List<HIS_TREATMENT_D> listSub = item.ToList<HIS_TREATMENT_D>();
                        SUM_ICD_IN_TREAT rdo = new SUM_ICD_IN_TREAT();
                        rdo.ICD_CODE = listSub.First().icd_code;
                        rdo.ICD_NAME = listSub.First().icd_name;
                        rdo.ICDVN_CODE = new IcdVnIcd(listSub.First().icd_code).ICDVN_CODE;
                        rdo.TOTAL = listSub.Count;
                        rdo.TOTAL_DEATH = listSub.Count(o => o.treatment_end_type_id == 1);
                        rdo.TOTAL_DAY = listSub.Sum(o => o.treatment_day_count ?? 0);

                        rdo.TOTAL_CHILD = listSub.Count(o => o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_CHILD_DEATH = listSub.Count(o => o.treatment_end_type_id == 1 && o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_CHILD_DAY = listSub.Where(p => p.tdl_patient_dob > p.in_time - 150000000000).Sum(o => o.treatment_day_count ?? 0);

                        rdo.TOTAL_CHILD4 = listSub.Count(o => o.tdl_patient_dob > o.in_time - 40000000000);
                        rdo.TOTAL_CHILD4_DEATH = listSub.Count(o => o.treatment_end_type_id == 1 && o.tdl_patient_dob > o.in_time - 40000000000);
                        rdo.TOTAL_CHILD4_DAY = listSub.Where(p => p.tdl_patient_dob > p.in_time - 40000000000).Sum(o => o.treatment_day_count ?? 0);
                        listSumIcdInTreat.Add(rdo);
                    }
                    return true;
                }
                if (lisTreatmentDirect != null && sql.StartsWith(" --NNN"))
                {
                    var groupByIcdVnCode = lisTreatmentDirect.GroupBy(o => new { new IcdVnIcd(o.icd_cause_code).ICDVN_CODE }).ToList();
                    foreach (var item in groupByIcdVnCode)
                    {
                        List<HIS_TREATMENT_D> listSub = item.ToList<HIS_TREATMENT_D>();
                        SUM_ICD_IN_TREAT rdo = new SUM_ICD_IN_TREAT();
                        rdo.ICD_CODE = listSub.First().icd_code;
                        rdo.ICD_NAME = listSub.First().icd_name;
                        rdo.ICDVN_CODE = new IcdVnIcd(listSub.First().icd_code).ICDVN_CODE;
                        rdo.ICDVN_CAUSE_CODE = new IcdVnIcd(listSub.First().icd_cause_code).ICDVN_CODE;
                        rdo.TOTAL = listSub.Count;
                        rdo.TOTAL_DEATH = listSub.Count(o => o.treatment_end_type_id == 1);
                        rdo.TOTAL_DAY = listSub.Sum(o => o.treatment_day_count ?? 0);

                        rdo.TOTAL_CHILD = listSub.Count(o => o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_CHILD_DEATH = listSub.Count(o => o.treatment_end_type_id == 1 && o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_CHILD_DAY = listSub.Where(p => p.tdl_patient_dob > p.in_time - 150000000000).Sum(o => o.treatment_day_count ?? 0);

                        rdo.TOTAL_CHILD4 = listSub.Count(o => o.tdl_patient_dob > o.in_time - 40000000000);
                        rdo.TOTAL_CHILD4_DEATH = listSub.Count(o => o.treatment_end_type_id == 1 && o.tdl_patient_dob > o.in_time - 40000000000);
                        rdo.TOTAL_CHILD4_DAY = listSub.Where(p => p.tdl_patient_dob > p.in_time - 40000000000).Sum(o => o.treatment_day_count ?? 0);
                        listSumNNN.Add(rdo);
                    }
                    return true;
                }
                if (lisTreatmentDirect != null && sql.StartsWith(" --NTV"))
                {
                    var groupByIcdVnCode = lisTreatmentDirect.GroupBy(o => new { new IcdVnIcd(o.icd_cause_code).ICDVN_CODE }).ToList();
                    foreach (var item in groupByIcdVnCode)
                    {
                        List<HIS_TREATMENT_D> listSub = item.ToList<HIS_TREATMENT_D>();
                        SUM_ICD_IN_TREAT rdo = new SUM_ICD_IN_TREAT();
                        rdo.ICD_CODE = listSub.First().icd_code;
                        rdo.ICD_NAME = listSub.First().icd_name;
                        rdo.ICDVN_CODE = new IcdVnIcd(listSub.First().icd_code).ICDVN_CODE;
                        rdo.ICDVN_CAUSE_CODE = new IcdVnIcd(listSub.First().icd_cause_code).ICDVN_CODE;
                        rdo.TOTAL = listSub.Count;
                        rdo.TOTAL_DEATH = listSub.Count(o => o.treatment_end_type_id == 1);
                        rdo.TOTAL_DAY = listSub.Sum(o => o.treatment_day_count ?? 0);

                        rdo.TOTAL_CHILD = listSub.Count(o => o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_CHILD_DEATH = listSub.Count(o => o.treatment_end_type_id == 1 && o.tdl_patient_dob > o.in_time - 150000000000);
                        rdo.TOTAL_CHILD_DAY = listSub.Where(p => p.tdl_patient_dob > p.in_time - 150000000000).Sum(o => o.treatment_day_count ?? 0);

                        rdo.TOTAL_CHILD4 = listSub.Count(o => o.tdl_patient_dob > o.in_time - 40000000000);
                        rdo.TOTAL_CHILD4_DEATH = listSub.Count(o => o.treatment_end_type_id == 1 && o.tdl_patient_dob > o.in_time - 40000000000);
                        rdo.TOTAL_CHILD4_DAY = listSub.Where(p => p.tdl_patient_dob > p.in_time - 40000000000).Sum(o => o.treatment_day_count ?? 0);
                        listSumNTV.Add(rdo);
                    }
                    return true;
                }

                var treatements = lisTreatments.Where(o => o.TREATMENT_RESULT_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET && o.TREATMENT_END_TYPE_ID != HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH).ToList();
                listIcds = listIcds.Where(o => treatements.Exists(s => s.ICD_CODE == o.ICD_CODE) || listDeaths.Exists(e => e.ICD_CAUSE_CODE == o.ICD_CODE)).Distinct().ToList();
                foreach (var hisIcd in listIcds)
                {
                    //lấy những bệnh nhân có cùng mã bệnh
                    var treatmentSameIcds = treatements.Where(s => s.ICD_CODE == hisIcd.ICD_CODE).ToList();

                    var roomGroup = treatmentSameIcds.GroupBy(s => s.END_ROOM_ID).ToList();
                    foreach (var endroom in roomGroup)
                    {
                        var death = listDeaths.Where(o => o.ICD_CAUSE_CODE == endroom.First().ICD_CODE && o.END_ROOM_ID == endroom.First().END_ROOM_ID).ToList();
                        //tổng số BN đã khám bệnh
                        var listTreatmentAtDepartmentMedicalExaminations = endroom.Where(s => !treatmentTypeId(s, listPatientTypeAlters).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).ToList();
                        //số bệnh nhân nữ đã khám bệnh
                        var listfemaleAtDepartmentMedicalExaminations = listTreatmentAtDepartmentMedicalExaminations.Where(s => s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();
                        //tính số trẻ em dưới 15 tuổi đã khám bệnh
                        var numberChidreUnder15Age = listTreatmentAtDepartmentMedicalExaminations.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 15);
                        //tính số trẻ em dưới 5 tuổi đã khám bệnh
                        var numberChidreUnder5Age = listTreatmentAtDepartmentMedicalExaminations.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 5);
                        //tính số bệnh nhân đã khám bênh và tử vong
                        var treatmentDeathAtDepartmentMaterialExamination = death != null ? death.Where(s => !treatmentTypeId(s, listPatientTypeAlters).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)) : new List<HIS_TREATMENT>();
                        //tính số trẻ em dưới 15 tuổi đã khám bệnh tử vong
                        var numberChidreUnder15AgeDeath = treatmentDeathAtDepartmentMaterialExamination.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 15);
                        //tính số trẻ em dưới 5 tuổi đã khám bệnh tử vong
                        var numberChidreUnder5AgeDeath = treatmentDeathAtDepartmentMaterialExamination.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 5);

                        //lấy DS bệnh nhân điều trị nội trú thuộc mã bệnh đang chọn
                        var listTreatmentSickBoarding = endroom.Where(s => treatmentTypeId(s, listPatientTypeAlters).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).ToList();
                        //số bệnh nhân là nữ mắc bệnh đã điều trị
                        var listTreatmentFemaleSickBoarding = listTreatmentSickBoarding.Where(s => s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();
                        //tổng số ca tử vong khi điều trị nội trú
                        var listDeathWhenBoarding = death != null ? death.Where(s => treatmentTypeId(s, listPatientTypeAlters).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).ToList() : new List<HIS_TREATMENT>();
                        //tính số ca tử vong là nữ điều trị
                        var totalFemaleDeathBoarding = listDeathWhenBoarding.Count(s => s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
                        //số bệnh nhân là trẻ em dưới 15 tuổi điều trị nội trú
                        var listChildrenUnder15AgeSickBoarding = listTreatmentSickBoarding.Where(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 15).ToList();
                        //số bệnh nhân là trẻ em dưới 5 tuổi điều trị nội trú
                        var totalChildrenUnder5AgeSickBoarding = listChildrenUnder15AgeSickBoarding.Where(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 5).ToList();
                        //số bệnh nhân là trẻ em dưới 15 tuổi tử vong khi điều trị nội trú
                        var totalChildrenUnder15AgeDeathBoarding = listDeathWhenBoarding.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 15);
                        //số bệnh nhân là trẻ em dưới 5 tuổi tử vong khi điều trị nội trú
                        var totalChildrenUnder5AgeDeathBoarding = listDeathWhenBoarding.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 5);

                        //tổng số ngày điều trị điều trị nội trú
                        var _TOTAL_BOARDING_DATE = listTreatmentSickBoarding.Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                        //tổng số ngày điều trị nữ điều trị nội trú
                        var _TOTAL_FEMALE_BOARDING_DATE = listTreatmentFemaleSickBoarding.Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                        //tổng số ngày điều trị của trẻ em dưới 15 tuổi điều trị
                        var _TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE = listChildrenUnder15AgeSickBoarding.Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                        //tổng số ngày điều trị của trẻ em dưới 5 tuổi điều trị
                        var _TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE = totalChildrenUnder5AgeSickBoarding.Sum(s => s.TREATMENT_DAY_COUNT ?? 0);

                        var rdo = new Mrs00661RDO
                        {
                            ICD_NAME = hisIcd.ICD_NAME,
                            ICD_CODE = hisIcd.ICD_CODE,
                            ICD_CODE_SUB = hisIcd.ICD_CODE.Substring(0, 3),
                            TOTAL_EXAMINATION = listTreatmentAtDepartmentMedicalExaminations.Count > 0 ? (int?)listTreatmentAtDepartmentMedicalExaminations.Count : null,
                            FEMALE_EXAMINATION = listfemaleAtDepartmentMedicalExaminations.Count > 0 ? (int?)listfemaleAtDepartmentMedicalExaminations.Count : null,
                            CHILDREN_UNDER_15_AGE_EXAMINATION = numberChidreUnder15Age > 0 ? (int?)numberChidreUnder15Age : null,
                            CHILDREN_UNDER_5_AGE_EXAMINATION = numberChidreUnder5Age > 0 ? (int?)numberChidreUnder5Age : null,
                            TOTAL_DEAD_EXAMINATION = treatmentDeathAtDepartmentMaterialExamination.Count(),
                            CHILDREN_UNDER_15_AGE_DEAD_EXAMINATION = numberChidreUnder15AgeDeath > 0 ? (int?)numberChidreUnder15AgeDeath : null,
                            CHILDREN_UNDER_5_AGE_DEAD_EXAMINATION = numberChidreUnder5AgeDeath > 0 ? (int?)numberChidreUnder5AgeDeath : null,

                            TOTAL_SICK_BOARDING = listTreatmentSickBoarding.Count > 0 ? (int?)listTreatmentSickBoarding.Count : null,
                            TOTAL_FEMALE_SICK_BOARDING = listTreatmentFemaleSickBoarding.Count > 0 ? (int?)listTreatmentFemaleSickBoarding.Count : null,
                            TOTAL_DEAD_BOARDING = listDeathWhenBoarding.Count > 0 ? (int?)listDeathWhenBoarding.Count : null,
                            TOTAL_FEMALE_DEAD_BOARDING = totalFemaleDeathBoarding > 0 ? (int?)totalFemaleDeathBoarding : null,
                            TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING = listChildrenUnder15AgeSickBoarding.Count > 0 ? (int?)listChildrenUnder15AgeSickBoarding.Count : null,
                            TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING = totalChildrenUnder5AgeSickBoarding.Count > 0 ? (int?)totalChildrenUnder5AgeSickBoarding.Count : null,
                            TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING = totalChildrenUnder15AgeDeathBoarding > 0 ? (int?)totalChildrenUnder15AgeDeathBoarding : null,
                            TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING = totalChildrenUnder5AgeDeathBoarding > 0 ? (int?)totalChildrenUnder5AgeDeathBoarding : null,
                            TOTAL_BOARDING_DATE = _TOTAL_BOARDING_DATE > 0 ? (int?)_TOTAL_BOARDING_DATE : null,
                            TOTAL_FEMALE_BOARDING_DATE = _TOTAL_FEMALE_BOARDING_DATE > 0 ? (int?)_TOTAL_FEMALE_BOARDING_DATE : null,
                            TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE = _TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE > 0 ? (int?)_TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE : null,
                            TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE = _TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE > 0 ? (int?)_TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE : null,
                            IS_CAUSE = hisIcd.IS_CAUSE == (short)1 ? "X" : "",

                            ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o=>o.ID==endroom.First().END_ROOM_ID)??new V_HIS_ROOM()).ROOM_NAME,
                            DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == endroom.First().END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME
                        };

                        listMrs00661Rdos.Add(rdo);
                    }
                }

                ProcessGroupData();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessGroupData()
        {
            try
            {
                if (IsNotNullOrEmpty(listMrs00661Rdos))
                {
                    ListRdos = new List<Mrs00661RDO>();
                    var groups = listMrs00661Rdos.GroupBy(g => g.ICD_CODE_SUB).ToList();
                    foreach (var g in groups)
                    {
                        Mrs00661RDO rdo = new Mrs00661RDO();
                        rdo.ICD_CODE_SUB = g.First().ICD_CODE_SUB;
                        rdo.CHILDREN_UNDER_15_AGE_EXAMINATION = g.Sum(s => s.CHILDREN_UNDER_15_AGE_EXAMINATION);
                        rdo.FEMALE_EXAMINATION = g.Sum(s => s.FEMALE_EXAMINATION);
                        rdo.TOTAL_BOARDING_DATE = g.Sum(s => s.TOTAL_BOARDING_DATE);
                        rdo.TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE = g.Sum(s => s.TOTAL_CHILDREN_UNDER_15_AGE_BOARDING_DATE);
                        rdo.TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING = g.Sum(s => s.TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING);
                        rdo.TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING = g.Sum(s => s.TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING);
                        rdo.TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE = g.Sum(s => s.TOTAL_CHILDREN_UNDER_5_AGE_BOARDING_DATE);
                        rdo.TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING = g.Sum(s => s.TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING);
                        rdo.TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING = g.Sum(s => s.TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING);
                        rdo.TOTAL_DEAD_BOARDING = g.Sum(s => s.TOTAL_DEAD_BOARDING);
                        rdo.TOTAL_DEAD_EXAMINATION = g.Sum(s => s.TOTAL_DEAD_EXAMINATION);
                        rdo.TOTAL_EXAMINATION = g.Sum(s => s.TOTAL_EXAMINATION);
                        rdo.TOTAL_FEMALE_BOARDING_DATE = g.Sum(s => s.TOTAL_FEMALE_BOARDING_DATE);
                        rdo.TOTAL_FEMALE_DEAD_BOARDING = g.Sum(s => s.TOTAL_FEMALE_DEAD_BOARDING);
                        rdo.TOTAL_FEMALE_SICK_BOARDING = g.Sum(s => s.TOTAL_FEMALE_SICK_BOARDING);
                        rdo.TOTAL_SICK_BOARDING = g.Sum(s => s.TOTAL_SICK_BOARDING);

                        ListRdos.Add(rdo);
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
                dicSingleTag.Add("DATE_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM ?? 0));
                dicSingleTag.Add("DATE_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO ?? 0));

               
                ListRdos = ListRdos.OrderBy(o => o.ICD_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdos);
                objectTag.AddObjectData(store, "ReportAll", listMrs00661Rdos);
                objectTag.AddObjectData(store, "SumIcdOutTreat", listSumIcdOutTreat.OrderBy(o => Convert.ToInt16(o.ICDVN_CODE)).ToList());
                objectTag.AddObjectData(store, "SumIcdInTreat", listSumIcdInTreat.OrderBy(o => Convert.ToInt16(o.ICDVN_CODE)).ToList());
                objectTag.AddObjectData(store, "NNN", listSumNNN.OrderBy(o => Convert.ToInt16(o.ICDVN_CAUSE_CODE)).ToList());
                objectTag.AddObjectData(store, "NTV", listSumNTV.OrderBy(o => o.ICDVN_CAUSE_CODE).ToList());
                store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<long> treatmentTypeId(HIS_TREATMENT thisData, List<HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            List<long> result = new List<long>();
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.ID).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Select(p => p.TREATMENT_TYPE_ID).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<long>();
            }

            return result;
        }
    }
}
