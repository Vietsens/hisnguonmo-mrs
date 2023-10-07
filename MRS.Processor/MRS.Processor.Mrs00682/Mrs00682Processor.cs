using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00682
{
    class Mrs00682Processor : AbstractProcessor
    {
        private Mrs00682Filter castFilter;
        private List<HIS_TREATMENT> LisTreatments = new List<HIS_TREATMENT>();
        private List<HIS_ICD> ListIcds = new List<HIS_ICD>();
        private HIS_BRANCH Branch = new HIS_BRANCH();//HEIN_PROVINCE_CODE check cùng tỉnh. Là ô Mã tỉnh ở chức năng cơ sở/xã phường.

        private List<Mrs00682RDO> ListRdo = new List<Mrs00682RDO>();

        public Mrs00682Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00682Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                castFilter = (Mrs00682Filter)reportFilter;

                var hisIcdFilter = new HisIcdFilterQuery { };
                ListIcds = new HisIcdManager(paramGet).Get(hisIcdFilter);

                Branch = new MOS.MANAGER.HisBranch.HisBranchManager().GetById(this.branch_id);

                if (castFilter.EXAM_TIME_FROM.HasValue || castFilter.EXAM_TIME_TO.HasValue)
                {
                    HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                    reqFilter.START_TIME_FROM = castFilter.EXAM_TIME_FROM;
                    reqFilter.START_TIME_TO = castFilter.EXAM_TIME_TO;
                    reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    var serviceReq = new HisServiceReqManager().Get(reqFilter);
                    if (IsNotNullOrEmpty(serviceReq))
                    {
                        var treatmentIds = serviceReq.Select(s => s.TREATMENT_ID).Distinct().ToList();

                        int skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var listIds = treatmentIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                            treatFilter.IDs = listIds;
                            var treatment = new HisTreatmentManager().Get(treatFilter);
                            if (IsNotNullOrEmpty(treatment))
                            {
                                LisTreatments.AddRange(treatment);
                            }
                        }
                    }

                    if (IsNotNullOrEmpty(LisTreatments))
                    {
                        LisTreatments = LisTreatments.Where(o => !o.CLINICAL_IN_TIME.HasValue || (o.CLINICAL_IN_TIME < castFilter.EXAM_TIME_FROM || o.CLINICAL_IN_TIME > castFilter.EXAM_TIME_TO)).ToList();
                    }
                }
                else if (castFilter.OUT_TIME_FROM.HasValue || castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                    treatFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    LisTreatments = new HisTreatmentManager().Get(treatFilter);

                    if (IsNotNullOrEmpty(LisTreatments))
                    {
                        LisTreatments = LisTreatments.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(LisTreatments))
                {
                    Dictionary<string, List<HIS_TREATMENT>> dicTreatment = new Dictionary<string, List<HIS_TREATMENT>>();
                    foreach (var item in LisTreatments)
                    {
                        if (!String.IsNullOrWhiteSpace(item.ICD_CODE))
                        {
                            if (!dicTreatment.ContainsKey(item.ICD_CODE))
                                dicTreatment[item.ICD_CODE] = new List<HIS_TREATMENT>();

                            dicTreatment[item.ICD_CODE].Add(item);
                        }
                    }

                    ListIcds = ListIcds.Where(o => dicTreatment.Keys.Contains(o.ICD_CODE)).ToList();

                    foreach (var item in ListIcds)
                    {
                        if (!dicTreatment.ContainsKey(item.ICD_CODE)) continue;

                        Mrs00682RDO rdo = new Mrs00682RDO();

                        rdo.ICD_CODE = item.ICD_CODE;
                        rdo.ICD_NAME = item.ICD_NAME;

                        rdo.TOTAL = dicTreatment[item.ICD_CODE].Count;
                        var treatFemale = dicTreatment[item.ICD_CODE].Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();
                        if (IsNotNullOrEmpty(treatFemale))
                        {
                            rdo.TOTAL_FEMALE = treatFemale.Count();

                            var treatFemaleUnder6 = treatFemale.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 7).ToList();
                            var treatFemaleUpder6 = treatFemale.Where(o => !treatFemaleUnder6.Exists(e => e.ID == o.ID)).ToList();

                            if (IsNotNullOrEmpty(treatFemaleUnder6))
                            {
                                rdo.TOTAL_FEMALE_UNDER_6_AGE_IN = treatFemaleUnder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE == Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_FEMALE_UNDER_6_AGE_OUT = treatFemaleUnder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE != Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_FEMALE_UNDER_6_AGE_OTHER = treatFemaleUnder6.Count(o => String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE));
                            }

                            if (IsNotNullOrEmpty(treatFemaleUpder6))
                            {
                                rdo.TOTAL_FEMALE_UPPER_6_AGE_IN = treatFemaleUpder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE == Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_FEMALE_UPPER_6_AGE_OUT = treatFemaleUpder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE != Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_FEMALE_UPPER_6_AGE_OHTER = treatFemaleUpder6.Count(o => String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE));
                            }
                        }

                        var treatMale = dicTreatment[item.ICD_CODE].Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).ToList();
                        if (IsNotNullOrEmpty(treatMale))
                        {
                            rdo.TOTAL_MALE = treatMale.Count();

                            var treatMaleUnder6 = treatMale.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 7).ToList();
                            var ttreatMaleUpder6 = treatMale.Where(o => !treatMaleUnder6.Exists(e => e.ID == o.ID)).ToList();

                            if (IsNotNullOrEmpty(treatMaleUnder6))
                            {
                                rdo.TOTAL_MALE_UNDER_6_AGE_IN = treatMaleUnder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE == Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_MALE_UNDER_6_AGE_OUT = treatMaleUnder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE != Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_MALE_UNDER_6_AGE_OTHER = treatMaleUnder6.Count(o => String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE));
                            }

                            if (IsNotNullOrEmpty(ttreatMaleUpder6))
                            {
                                rdo.TOTAL_MALE_UPPER_6_AGE_IN = ttreatMaleUpder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE == Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_MALE_UPPER_6_AGE_OUT = ttreatMaleUpder6.Count(o => !String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE) && o.TDL_PATIENT_PROVINCE_CODE != Branch.HEIN_PROVINCE_CODE);
                                rdo.TOTAL_MALE_UPPER_6_AGE_OHTER = ttreatMaleUpder6.Count(o => String.IsNullOrWhiteSpace(o.TDL_PATIENT_PROVINCE_CODE));
                            }
                        }

                        ListRdo.Add(rdo);
                    }
                }

                ProcessRemove0();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessRemove0()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    foreach (var item in ListRdo)
                    {
                        if (item.TOTAL_FEMALE_UNDER_6_AGE_IN == 0) item.TOTAL_FEMALE_UNDER_6_AGE_IN = null;
                        if (item.TOTAL_FEMALE_UNDER_6_AGE_OTHER == 0) item.TOTAL_FEMALE_UNDER_6_AGE_OTHER = null;
                        if (item.TOTAL_FEMALE_UNDER_6_AGE_OUT == 0) item.TOTAL_FEMALE_UNDER_6_AGE_OUT = null;
                        if (item.TOTAL_FEMALE_UPPER_6_AGE_IN == 0) item.TOTAL_FEMALE_UPPER_6_AGE_IN = null;
                        if (item.TOTAL_FEMALE_UPPER_6_AGE_OHTER == 0) item.TOTAL_FEMALE_UPPER_6_AGE_OHTER = null;
                        if (item.TOTAL_FEMALE_UPPER_6_AGE_OUT == 0) item.TOTAL_FEMALE_UPPER_6_AGE_OUT = null;
                        if (item.TOTAL_MALE_UNDER_6_AGE_IN == 0) item.TOTAL_MALE_UNDER_6_AGE_IN = null;
                        if (item.TOTAL_MALE_UNDER_6_AGE_OTHER == 0) item.TOTAL_MALE_UNDER_6_AGE_OTHER = null;
                        if (item.TOTAL_MALE_UNDER_6_AGE_OUT == 0) item.TOTAL_MALE_UNDER_6_AGE_OUT = null;
                        if (item.TOTAL_MALE_UPPER_6_AGE_IN == 0) item.TOTAL_MALE_UPPER_6_AGE_IN = null;
                        if (item.TOTAL_MALE_UPPER_6_AGE_OHTER == 0) item.TOTAL_MALE_UPPER_6_AGE_OHTER = null;
                        if (item.TOTAL_MALE_UPPER_6_AGE_OUT == 0) item.TOTAL_MALE_UPPER_6_AGE_OUT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (castFilter.EXAM_TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.EXAM_TIME_FROM.Value));
                }
                else if (castFilter.OUT_TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM.Value));
                }

                if (castFilter.EXAM_TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.EXAM_TIME_TO.Value));
                }
                else if (castFilter.OUT_TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.OUT_TIME_TO.Value));
                }

                ListRdo = ListRdo.OrderBy(o => o.ICD_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
