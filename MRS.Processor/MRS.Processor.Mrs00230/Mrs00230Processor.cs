using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisIcdGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDeathWithin;

namespace MRS.Processor.Mrs00230
{
    public class Mrs00230Processor : AbstractProcessor
    {
        private Mrs00230Filter filter;
        private CommonParam paramGet = new CommonParam();
        private List<Mrs00230RDO_PARENT> listMrs00230RdoParents = new List<Mrs00230RDO_PARENT>();
        private List<Mrs00230RDO> listMrs00230Rdos = new List<Mrs00230RDO>();
        List<Ms00230RDO_DEAD_488> listMrs00230RdoDead = new List<Ms00230RDO_DEAD_488>();
        List<Mrs00230RDO_NEW> listRdoNew = new List<Mrs00230RDO_NEW>();
        private List<V_HIS_TREATMENT> lisTreatments = new List<V_HIS_TREATMENT>();
        private List<HIS_ICD> listIcds = new List<HIS_ICD>();
        private List<HIS_ICD_GROUP> listIcdGroups = new List<HIS_ICD_GROUP>();
        private List<V_HIS_TREATMENT> listDeaths = new List<V_HIS_TREATMENT>();
        private List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        private List<HIS_DEATH_WITHIN> listDeathWithThin = new List<HIS_DEATH_WITHIN>();
        private List<COUNT_IN> listCountIn = new List<COUNT_IN>();
        private List<ICD_GROUP_DETAIL> listIcdGroupDetail = new List<ICD_GROUP_DETAIL>();
        private List<ICD_GROUP_DETAIL> listIcdGroupDetailData = new List<ICD_GROUP_DETAIL>();
        public Mrs00230Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00230Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00230Filter)reportFilter;
            try
            {

                var treatmentFilter = new HisTreatmentViewFilterQuery
                {
                    IS_PAUSE = true,
                    OUT_TIME_FROM = filter.DATE_TIME_FROM,
                    OUT_TIME_TO = filter.DATE_TIME_TO,
                    END_DEPARTMENT_IDs = filter.DEPARTMENT_IDs
                };
                lisTreatments = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                if (filter.DEPARTMENT_ID != null)
                {
                    lisTreatments = lisTreatments.Where(o => o.END_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    lisTreatments = lisTreatments.Where(o => filter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID??0)).ToList();
                }
                listDeaths = lisTreatments.Where(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET || o.TREATMENT_END_TYPE_ID == HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH).ToList();
                listCountIn = new ManagerSql().GetVaoVien(filter);

                // get phân loại nhóm ICD
                listIcdGroupDetail = new ManagerSql().GetIcdGroupDetail(filter);

                 var skip = 0;

                var hisIcdFilter = new HisIcdFilterQuery
                {

                };
                var icds = new HisIcdManager(paramGet).Get(hisIcdFilter);
                listIcds.AddRange(icds);
                var icdGroupFilter = new HisIcdGroupFilterQuery
                {
                };
                var icdGroups = new HisIcdGroupManager(paramGet).Get(icdGroupFilter);
                listIcdGroups.AddRange(icdGroups);

                var listTreatmentIds = lisTreatments.Select(s => s.ID).ToList();

                //--------------------------------------------------------------------------------------------------V_HIS_PATIENT_TYPE_ALTER
                skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var patientTypeAlterViews = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                    listPatientTypeAlters.AddRange(patientTypeAlterViews);
                }
                var deathWithThinIds = lisTreatments.Select(x => x.DEATH_WITHIN_ID ?? 0).Distinct().ToList();
                HisDeathWithinFilterQuery deathFilter = new HisDeathWithinFilterQuery();
                deathFilter.IDs = deathWithThinIds;
                listDeathWithThin = new HisDeathWithinManager().Get(deathFilter);
                //--------------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }
        private void ProcessDataDeath()
        {
            listIcds = listIcds.Where(o => lisTreatments.Exists(s => s.ICD_CODE == o.ICD_CODE || s.ICD_CAUSE_CODE == o.ICD_CODE)).ToList();
            var listIcdGroupbyIcdGroups = listIcds.Distinct().GroupBy(s => s.ICD_GROUP_ID).ToList();
            foreach (var listIcdGroupbyIcdGroup in listIcdGroupbyIcdGroups)
            {
                var icdGroup = listIcdGroups.Where(s => s.ID == listIcdGroupbyIcdGroup.Key).ToList();
                if (icdGroup.Count > 0)
                {
                    var mrs00230Parent = new Mrs00230RDO_PARENT { ICD_GROUP = icdGroup.First() };
                    listMrs00230RdoParents.Add(mrs00230Parent);
                }
                foreach (var hisIcd in listIcdGroupbyIcdGroup)
                {
                    Mrs00230RDO_NEW rdo = new Mrs00230RDO_NEW();
                    if (icdGroup.Count > 0)
                    {
                        rdo.ICD_GROUP_CODE = icdGroup.First().ICD_GROUP_CODE ?? " ";
                        rdo.ICD_GROUP_NAME = icdGroup.First().ICD_GROUP_NAME;
                    }
                    rdo.ICD_CODE = hisIcd.ICD_CODE;
                    rdo.ICD_NAME = hisIcd.ICD_NAME;
                    if (rdo.ICD_CODE.Length >= 3)
                    {
                        rdo.ICD_CODE_SUB = hisIcd.ICD_CODE.Substring(0, 3);
                    }
                    rdo.TOTAL_VAO_VIEN = ((listCountIn??new List<COUNT_IN>()).FirstOrDefault(o => o.ICD_CODE==hisIcd.ICD_CODE)?? new COUNT_IN()).TOTAL_VAO_VIEN;
                    rdo.TOTAL_DEAD_RA_VIEN = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE).Count();
                    rdo.TOTAL_RA_VIEN = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE).Count();
                    rdo.TOTAL_FEMALE_RA_VIEN = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE&& x.TDL_PATIENT_GENDER_ID==IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.TOTAL_TREATMENT_DAY_FEMALE_RA_VIEN = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Sum(x=>x.TREATMENT_DAY_COUNT);
                    rdo.TOTAL_TREATMENT_DAY_RA_VIEN = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE).Sum(x => x.TREATMENT_DAY_COUNT);

                    rdo.TOTAL_DEATH_LESS_THAN_1_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 1).Count();
                    rdo.TOTAL_FEMALE_LESS_THAN_1_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE&&Calculation.Age(x.TDL_PATIENT_DOB)<1).Count();
                    rdo.TOTAL_LESS_THAN_1_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 1).Count();
                    rdo.TOTAL_TREATMENT_DAY_FEMALE_LESS_THAN_1_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) < 1).Sum(x => x.TREATMENT_DAY_COUNT);
                    rdo.TOTAL_TREATMENT_DAY_LESS_THAN_1_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 1).Sum(x => x.TREATMENT_DAY_COUNT);

                    rdo.TOTAL_DEATH_LESS_THAN_5_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 5).Count();
                    rdo.TOTAL_FEMALE_LESS_THAN_5_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) < 5).Count();
                    rdo.TOTAL_LESS_THAN_5_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 5).Count();
                    rdo.TOTAL_TREATMENT_DAY_FEMALE_LESS_THAN_5_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) < 5).Sum(x => x.TREATMENT_DAY_COUNT);
                    rdo.TOTAL_TREATMENT_DAY_LESS_THAN_5_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 5).Sum(x => x.TREATMENT_DAY_COUNT);

                    rdo.TOTAL_DEATH_LESS_THAN_15_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 15).Count();
                    rdo.TOTAL_FEMALE_LESS_THAN_15_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) < 15).Count();
                    rdo.TOTAL_LESS_THAN_15_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 15).Count();
                    rdo.TOTAL_TREATMENT_DAY_FEMALE_LESS_THAN_15_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) < 15).Sum(x => x.TREATMENT_DAY_COUNT);
                    rdo.TOTAL_TREATMENT_DAY_LESS_THAN_15_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 15).Sum(x => x.TREATMENT_DAY_COUNT);

                    rdo.TOTAL_DEATH_MORE_THAN_60_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) > 60).Count();
                    rdo.TOTAL_FEMALE_MORE_THAN_60_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) > 60 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.TOTAL_FEMALE_DEATH_MORE_THAN_60_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) > 60).Count();
                    rdo.TOTAL_MORE_THAN_60_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >60).Count();
                    rdo.TOTAL_TREATMENT_DAY_FEMALE_MORE_THAN_60_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Calculation.Age(x.TDL_PATIENT_DOB) > 60).Sum(x => x.TREATMENT_DAY_COUNT);
                    rdo.TOTAL_TREATMENT_DAY_MORE_THAN_60_AGE = lisTreatments.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >60).Sum(x => x.TREATMENT_DAY_COUNT);


                    rdo.DIC_DEATH_24H_AMOUNT = listDeaths.Where(x=>x.ICD_CODE == hisIcd.ICD_CODE).GroupBy(x => x.DEATH_WITHIN_ID??0).ToDictionary(x => x.Key.ToString(),y=>y.Count());
                    rdo.DIC_DEATH_FEMALE_24H_AMOUNT = listDeaths.Where(x =>x.ICD_CODE == hisIcd.ICD_CODE&& x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(x => x.DEATH_WITHIN_ID ?? 0).ToDictionary(x => x.Key.ToString(), y => y.Count());
                    listRdoNew.Add(rdo);
                }
            }
        }
        private void ProcessData488()
        {
            try
            {
                listIcds = listIcds.Where(o => lisTreatments.Exists(s => s.ICD_CODE == o.ICD_CODE || s.ICD_CAUSE_CODE == o.ICD_CODE)).ToList();
                var listIcdGroupbyIcdGroups = listIcds.Distinct().GroupBy(s => s.ICD_GROUP_ID).ToList();
                foreach (var listIcdGroupbyIcdGroup in listIcdGroupbyIcdGroups)
                {
                    var icdGroup = listIcdGroups.Where(s => s.ID == listIcdGroupbyIcdGroup.Key).ToList();
                    if (icdGroup.Count > 0)
                    {
                        var mrs00230Parent = new Mrs00230RDO_PARENT { ICD_GROUP = icdGroup.First() };
                        listMrs00230RdoParents.Add(mrs00230Parent);
                    }
                    foreach (var hisIcd in listIcdGroupbyIcdGroup)
                    {
                        Ms00230RDO_DEAD_488 rdo = new Ms00230RDO_DEAD_488();
                        if (icdGroup.Count > 0)
                        {
                            rdo.ICD_GROUP_CODE = icdGroup.First().ICD_GROUP_CODE ?? " ";
                            rdo.ICD_GROUP_NAME = icdGroup.First().ICD_GROUP_NAME;
                        }
                        rdo.ICD_CODE = hisIcd.ICD_CODE;
                        rdo.ICD_NAME = hisIcd.ICD_NAME;
                        if (rdo.ICD_CODE.Length >= 3)
                        {
                            rdo.ICD_CODE_SUB = hisIcd.ICD_CODE.Substring(0, 3);
                        }
                        rdo.TOTAL_DEAD = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE).Count();
                        rdo.TOTAL_DEAD_FEMALE = listDeaths.Where(x => x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && x.ICD_CODE == hisIcd.ICD_CODE).Count();
                        rdo.TOTAL_1_AGE_TO_5_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 5 && Calculation.Age(x.TDL_PATIENT_DOB) >= 1).Count();
                        rdo.TOTAL_5_AGE_TO_10_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 5 && Calculation.Age(x.TDL_PATIENT_DOB) < 10).Count();
                        rdo.TOTAL_10_AGE_TO_15_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 10 && Calculation.Age(x.TDL_PATIENT_DOB) < 15).Count();
                        rdo.TOTAL_15_AGE_TO_20_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 15 && Calculation.Age(x.TDL_PATIENT_DOB) < 20).Count();
                        rdo.TOTAL_20_AGE_TO_30_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 20 && Calculation.Age(x.TDL_PATIENT_DOB) < 30).Count();
                        rdo.TOTAL_30_AGE_TO_40_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 30 && Calculation.Age(x.TDL_PATIENT_DOB) < 40).Count();
                        rdo.TOTAL_40_AGE_TO_50_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 40 && Calculation.Age(x.TDL_PATIENT_DOB) < 50).Count();
                        rdo.TOTAL_50_AGE_TO_60_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 50 && Calculation.Age(x.TDL_PATIENT_DOB) < 60).Count();
                        rdo.TOTAL_60_AGE_TO_70_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 60 && Calculation.Age(x.TDL_PATIENT_DOB) < 70).Count();
                        rdo.TOTAL_MORE_70_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 70).Count();

                        rdo.TOTAL_FEMALE_1_AGE_TO_5_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) < 5 && Calculation.Age(x.TDL_PATIENT_DOB) >= 1 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_5_AGE_TO_10_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 5 && Calculation.Age(x.TDL_PATIENT_DOB) < 10 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_10_AGE_TO_15_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 10 && Calculation.Age(x.TDL_PATIENT_DOB) < 15 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_15_AGE_TO_20_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 15 && Calculation.Age(x.TDL_PATIENT_DOB) < 20 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_20_AGE_TO_30_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 20 && Calculation.Age(x.TDL_PATIENT_DOB) < 30 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_30_AGE_TO_40_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 30 && Calculation.Age(x.TDL_PATIENT_DOB) < 40 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_40_AGE_TO_50_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 40 && Calculation.Age(x.TDL_PATIENT_DOB) < 50 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_50_AGE_TO_60_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 50 && Calculation.Age(x.TDL_PATIENT_DOB) < 60 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_60_AGE_TO_70_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 70 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        rdo.TOTAL_FEMALE_MORE_70_AGE = listDeaths.Where(x => x.ICD_CODE == hisIcd.ICD_CODE && Calculation.Age(x.TDL_PATIENT_DOB) >= 70 && x.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                        
                        listMrs00230RdoDead.Add(rdo);
                    }

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        protected override bool ProcessData()
        {
            var result = true;
            try
            {


                //lấy đối tượng điều trị cuối cùng của bệnh nhân
                //var listPatientTypeAlter = listPatientTypeAlters.GroupBy(s => s.TREATMENT_ID)
                //    .Select(s => s.OrderBy(a => a.LOG_TIME).Last()).ToList();
                listIcds = listIcds.Where(o => lisTreatments.Exists(s => s.ICD_CODE == o.ICD_CODE || s.ICD_CAUSE_CODE == o.ICD_CODE)).ToList();
                var listIcdGroupbyIcdGroups = listIcds.Distinct().GroupBy(s => s.ICD_GROUP_ID).ToList();
                foreach (var listIcdGroupbyIcdGroup in listIcdGroupbyIcdGroups)
                {
                    var icdGroup = listIcdGroups.Where(s => s.ID == listIcdGroupbyIcdGroup.Key).ToList();
                    if (icdGroup.Count > 0)
                    {
                        var mrs00230Parent = new Mrs00230RDO_PARENT { ICD_GROUP = icdGroup.First() };
                        listMrs00230RdoParents.Add(mrs00230Parent);
                    }
                    foreach (var hisIcd in listIcdGroupbyIcdGroup)
                    {
                        //lấy những bệnh nhân có cùng mã bệnh
                        var treatmentSameIcds = lisTreatments.Where(s => s.ICD_CODE == hisIcd.ICD_CODE || s.ICD_CAUSE_CODE == hisIcd.ICD_CODE).ToList();
                        //tổng số BN đã khám bệnh
                        var listTreatmentAtDepartmentMedicalExaminations = treatmentSameIcds;//.Where(s => treatmentTypeId(s, listPatientTypeAlters).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)).ToList();
                        //số bệnh nhân nữ đã khám bệnh
                        var listfemaleAtDepartmentMedicalExaminations = listTreatmentAtDepartmentMedicalExaminations.Where(s => s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();
                        //tính số trẻ em dưới 15 tuổi đã khám bệnh
                        var numberChidreUnder15Age = listTreatmentAtDepartmentMedicalExaminations.Count(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 15);
                        //tính số BN nặng xin về
                        var heavy = listTreatmentAtDepartmentMedicalExaminations.Count(s => s.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG && s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN);
                        //tính số bệnh nhân đã khám bênh và tử vong
                        var treatmentDeathAtDepartmentMaterialExamination = listDeaths.Count(s => listTreatmentAtDepartmentMedicalExaminations.Select(ss => ss.ID).Contains(s.ID));
                        //tính số bệnh nhân đã khám bênh và tử vong trước khi vào viện
                        var deathBefore = listDeaths.Count(s => listTreatmentAtDepartmentMedicalExaminations.Select(ss => ss.ID).Contains(s.ID) && s.DEATH_TIME < s.IN_TIME);
                        //tính số bệnh nhân đã khám bênh và tử vong tại viện
                        var deathOn = listDeaths.Count(s => listTreatmentAtDepartmentMedicalExaminations.Select(ss => ss.ID).Contains(s.ID) && s.DEATH_TIME < s.IN_TIME && s.DEATH_TIME < s.OUT_TIME);

                        //lấy DS bệnh nhân điều trị nội trú thuộc mã bệnh đang chọn
                        var listTreatmentSickBoarding = treatmentSameIcds.Where(s => treatmentTypeId(s, listPatientTypeAlters).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).ToList();
                        //số bệnh nhân là nữ mắc bệnh đã điều trị
                        var listTreatmentFemaleSickBoarding = listTreatmentSickBoarding.Where(s => s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();
                        //số bệnh nhân là nặng xin về đã điều trị
                        var inHeavy = listTreatmentSickBoarding.Where(s => s.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG && s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN).ToList();
                        //số bệnh nhân là nặng xin về đã điều trị
                        var inHeavyFeMale = listTreatmentSickBoarding.Where(s => s.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG && s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN && s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();
                        //tổng số ca tử vong khi điều trị nội trú
                        var listDeathWhenBoarding = listDeaths.Where(s => listTreatmentSickBoarding.Select(ss => ss.ID).Contains(s.ID)).ToList();
                        //tổng số ca tử vong khi điều trị nội trú có giấy báo tử
                        var inDeathDocument = listDeaths.Where(s => listTreatmentSickBoarding.Select(ss => ss.ID).Contains(s.ID) && s.DEATH_DOCUMENT_NUMBER != null).ToList();

                        //tính số ca tử vong là nữ điều trị
                        var totalFemaleDeathBoarding = treatmentSameIcds.Count(s => listDeathWhenBoarding.Select(a => a.ID).Contains(s.ID)
                            && s.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
                        //số bệnh nhân là trẻ em dưới 15 tuổi điều trị nội trú
                        var listChildrenUnder15AgeSickBoarding = treatmentSameIcds.Where(s => listTreatmentSickBoarding.Select(o => o.ID).Contains(s.ID)).
                            Where(a => Inventec.Common.DateTime.Calculation.Age(a.TDL_PATIENT_DOB) < 15).ToList();
                        //số bệnh nhân là trẻ em dưới 5 tuổi điều trị nội trú
                        var totalChildrenUnder5AgeSickBoarding = listChildrenUnder15AgeSickBoarding.Where(s => Inventec.Common.DateTime.Calculation.Age(s.TDL_PATIENT_DOB) < 5).ToList();
                        //số bệnh nhân là trẻ em dưới 15 tuổi tử vong khi điều trị nội trú
                        var totalChildrenUnder15AgeDeathBoarding = listDeaths.Count(s => listChildrenUnder15AgeSickBoarding.Select(a => a.ID).Contains(s.ID));
                        //số bệnh nhân là trẻ em dưới 5 tuổi tử vong khi điều trị nội trú
                        var totalChildrenUnder5AgeDeathBoarding = listDeaths.Count(s => totalChildrenUnder5AgeSickBoarding.Select(a => a.ID).Contains(s.ID));
                        //số cán bộ cơ sỏ
                        var totalPolice = lisTreatments.Count(s => !string.IsNullOrWhiteSpace(s.TDL_HEIN_CARD_NUMBER) && s.TDL_HEIN_CARD_NUMBER.StartsWith("CA") && (s.ICD_CODE == hisIcd.ICD_CODE || s.ICD_CAUSE_CODE == hisIcd.ICD_CODE));


                        var _TOTAL_DEAD_BOARDING_DATE = lisTreatments.Where(o => (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);  //tổng số ngày điều trị tử vong điều trị nội trú
                        var _TOTAL_FEMALE_DEAD_BOARDING_DATE = lisTreatments.Where(o => o.TDL_PATIENT_GENDER_ID == 1 && (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);  //tổng số ngày điều trị nữ tử vong điều trị nội trú
                        var _TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING_DATE = listDeaths.Where(o => (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) < 15).Sum(s => s.TREATMENT_DAY_COUNT ?? 0); 	//tổng số ngày điều trị của trẻ em dưới 15 tuổi điều trị tử vong
                        var _TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING_DATE = listDeaths.Where(o => (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) < 15).Sum(s => s.TREATMENT_DAY_COUNT ?? 0); 	//tổng số ngày điều trị của trẻ em dưới 5 tuổi điều trị tử vong
                        var _TOTAL_SICK_BOARDING_MORE_60_AGE = lisTreatments.Count(o => (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60); 		//tổng số người già trên 60 tuổi điều trị mắc bệnh
                        var _TOTAL_FEMALE_SICK_BOARDING_MORE_60_AGE = lisTreatments.Count(o => o.TDL_PATIENT_GENDER_ID == 1 && (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60); 	//tổng số  người già nữ trên 60 tuổi điều trị mắc bệnh
                        var _TOTAL_DEAD_BOARDING_MORE_60_AGE = listDeaths.Count(o => (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60); //tổng số người già trên 60 tuổi điều trị tử vong	
                        var _TOTAL_FEMALE_DEAD_BOARDING_MORE_60_AGE = listDeaths.Count(o => o.TDL_PATIENT_GENDER_ID == 1 && (o.ICD_CODE == hisIcd.ICD_CODE || o.ICD_CAUSE_CODE == hisIcd.ICD_CODE) && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60); //tổng số người già nữ trên 60 tuổi điều trị tử vong

                        var rdo = new Mrs00230RDO
                        {
                            PARENT_ID = listIcdGroupbyIcdGroup.Key,
                            ICD_NAME = hisIcd.ICD_NAME,
                            ICD_CODE = hisIcd.ICD_CODE,
                            ICD_GROUP_CODE = (icdGroup.Count > 0) ? (icdGroup.First().ICD_GROUP_CODE ?? " ") : " ",
                            ICD_GROUP_NAME = (icdGroup.Count > 0) ? (icdGroup.First().ICD_GROUP_NAME ?? " ") : " ",
                            ICD_CODE_SUB = hisIcd.ICD_CODE.Substring(0, 3),
                            TOTAL_EXAMINATION = listTreatmentAtDepartmentMedicalExaminations.Count > 0 ? (int?)listTreatmentAtDepartmentMedicalExaminations.Count : null,
                            FEMALE_EXAMINATION = listfemaleAtDepartmentMedicalExaminations.Count > 0 ? (int?)listfemaleAtDepartmentMedicalExaminations.Count : null,
                            HEAVY_EXAMINATION = heavy > 0 ? (int?)heavy : null,
                            DEATH_BEFORE_EXAMINATION = deathBefore > 0 ? (int?)deathBefore : null,
                            DEATH_ON_EXAMINATION = deathOn > 0 ? (int?)deathOn : null,
                            CHILDREN_UNDER_15_AGE_EXAMINATION = numberChidreUnder15Age > 0 ? (int?)numberChidreUnder15Age : null,
                            TOTAL_DEAD_EXAMINATION = treatmentDeathAtDepartmentMaterialExamination > 0 ? (int?)treatmentDeathAtDepartmentMaterialExamination : null,

                            TOTAL_SICK_BOARDING = listTreatmentSickBoarding.Count > 0 ? (int?)listTreatmentSickBoarding.Count : null,
                            TOTAL_FEMALE_SICK_BOARDING = listTreatmentFemaleSickBoarding.Count > 0 ? (int?)listTreatmentFemaleSickBoarding.Count : null,
                            TOTAL_HEAVY_SICK_BOARDING = inHeavy.Count > 0 ? (int?)inHeavy.Count : null,
                            TOTAL_HEAVY_FEMALE_SICK_BOARDING = inHeavyFeMale.Count > 0 ? (int?)inHeavyFeMale.Count : null,
                            TOTAL_DEAD_BOARDING = listDeathWhenBoarding.Count > 0 ? (int?)listDeathWhenBoarding.Count : null,
                            TOTAL_DEAD_DOCUMENT_BOARDING = inDeathDocument.Count > 0 ? (int?)inDeathDocument.Count : null,
                            TOTAL_FEMALE_DEAD_BOARDING = totalFemaleDeathBoarding > 0 ? (int?)totalFemaleDeathBoarding : null,
                            TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING = listChildrenUnder15AgeSickBoarding.Count > 0 ? (int?)listChildrenUnder15AgeSickBoarding.Count : null,
                            TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING = totalChildrenUnder5AgeSickBoarding.Count > 0 ? (int?)totalChildrenUnder5AgeSickBoarding.Count : null,
                            TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING = totalChildrenUnder15AgeDeathBoarding > 0 ? (int?)totalChildrenUnder15AgeDeathBoarding : null,
                            TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING = totalChildrenUnder5AgeDeathBoarding > 0 ? (int?)totalChildrenUnder5AgeDeathBoarding : null,
                            TOTAL_DEAD_BOARDING_DATE = _TOTAL_DEAD_BOARDING_DATE > 0 ? (int?)_TOTAL_DEAD_BOARDING_DATE : null,
                            TOTAL_FEMALE_DEAD_BOARDING_DATE = _TOTAL_FEMALE_DEAD_BOARDING_DATE > 0 ? (int?)_TOTAL_FEMALE_DEAD_BOARDING_DATE : null,
                            TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING_DATE = _TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING_DATE > 0 ? (int?)_TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING_DATE : null,
                            TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING_DATE = _TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING_DATE > 0 ? (int?)_TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING_DATE : null,
                            TOTAL_SICK_BOARDING_MORE_60_AGE = _TOTAL_SICK_BOARDING_MORE_60_AGE > 0 ? (int?)_TOTAL_SICK_BOARDING_MORE_60_AGE : null,
                            TOTAL_FEMALE_SICK_BOARDING_MORE_60_AGE = _TOTAL_FEMALE_SICK_BOARDING_MORE_60_AGE > 0 ? (int?)_TOTAL_FEMALE_SICK_BOARDING_MORE_60_AGE : null,
                            TOTAL_DEAD_BOARDING_MORE_60_AGE = _TOTAL_DEAD_BOARDING_MORE_60_AGE > 0 ? (int?)_TOTAL_DEAD_BOARDING_MORE_60_AGE : null,
                            TOTAL_FEMALE_DEAD_BOARDING_MORE_60_AGE = _TOTAL_FEMALE_DEAD_BOARDING_MORE_60_AGE > 0 ? (int?)_TOTAL_FEMALE_DEAD_BOARDING_MORE_60_AGE : null,
                            IS_CAUSE = hisIcd.IS_CAUSE == (short)1 ? "X" : "",
                            TOTAL_POLICE = totalPolice > 0 ? (int?)totalPolice : null
                        };
                        listMrs00230Rdos.Add(rdo);
                    }
                }
                ProcessData488();
                ProcessDataDeath();
                ProcessDataIcdGroupDetail();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessDataIcdGroupDetail()
        {
            foreach (var item in listIcdGroupDetail)
            {
                var treatmentSub = lisTreatments.Where(s => s.ICD_CODE == item.ICD_CODE || s.ICD_CAUSE_CODE == item.ICD_CODE).ToList();
                if (treatmentSub.Count > 0)
                {
                    foreach (var trea in treatmentSub)
                    {
                        ICD_GROUP_DETAIL rdo = new ICD_GROUP_DETAIL();
                        rdo.ICD_CODE = item.ICD_CODE;
                        rdo.ICD_GROUP = item.ICD_GROUP;
                        rdo.ICD_PARENT_GROUP = item.ICD_PARENT_GROUP;
                        rdo.TREATMENT_CODE = trea.TREATMENT_CODE;
                        rdo.TDL_PATIENT_CODE = trea.TDL_PATIENT_CODE;
                        rdo.TDL_PATIENT_NAME = trea.TDL_PATIENT_NAME;
                        rdo.TDL_PATIENT_DOB = trea.TDL_PATIENT_DOB;
                        rdo.TDL_PATIENT_GENDER_NAME = trea.TDL_PATIENT_GENDER_NAME;
                        rdo.IN_TIME = trea.IN_TIME;
                        rdo.OUT_TIME = trea.OUT_TIME;
                        rdo.LAST_DEPARTMENT_ID = trea.LAST_DEPARTMENT_ID;
                        rdo.LAST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==trea.LAST_DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        listIcdGroupDetailData.Add(rdo);
                    }
                }
                else
                {
                    ICD_GROUP_DETAIL rdo = new ICD_GROUP_DETAIL();
                    rdo.ICD_CODE = item.ICD_CODE;
                    rdo.ICD_GROUP = item.ICD_GROUP;
                    rdo.ICD_PARENT_GROUP = item.ICD_PARENT_GROUP;
                    listIcdGroupDetailData.Add(rdo);
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("DATE_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.DATE_TIME_FROM));
            dicSingleTag.Add("DATE_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.DATE_TIME_TO));
            listMrs00230Rdos = listMrs00230Rdos.OrderBy(o => o.ICD_CODE).ToList();
            //objectTag.AddObjectData(store, "ReportParent", listMrs00230RdoParents); 
            objectTag.AddObjectData(store, "Report", listMrs00230Rdos);
            store.SetCommonFunctions();
            objectTag.AddObjectData(store, "Death", listMrs00230RdoDead);
            objectTag.AddObjectData(store, "ReportTreatment", listRdoNew);
            //objectTag.AddRelationship(store, "ReportParent", "Report", "ICD_GROUP.ID", "PARENT_ID"); 
            objectTag.AddObjectData(store, "IcdGroupDetail", listIcdGroupDetailData);
        }
        private List<long> treatmentTypeId(V_HIS_TREATMENT thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
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
