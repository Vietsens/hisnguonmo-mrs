using MOS.MANAGER.HisAccidentHurt;
using MOS.MANAGER.HisTreatment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisAccidentBodyPart;
using MOS.MANAGER.HisDeathCause;
using System.Data;
using MOS.MANAGER.HisMediOrg;

namespace MRS.Processor.Mrs00589
{
    class Mrs00589Processor : AbstractProcessor
    {
        Mrs00589Filter castFilter = null;
        List<V_HIS_ACCIDENT_HURT> listAccidentHurts = new List<V_HIS_ACCIDENT_HURT>();
        List<HIS_TREATMENT_DEATH> listTreatmentDeath = new List<HIS_TREATMENT_DEATH>();
        List<Mrs00589RDO> ListRdo = new List<Mrs00589RDO>();
        public List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        public List<HIS_MEDI_ORG> ListMediOrg = new List<HIS_MEDI_ORG>();
        List<List<DataTable>> dataObject = new List<List<DataTable>>();
        private Dictionary<string, string> dicSingleKey = new Dictionary<string, string>();

        long COUNT = 0;
        long COUNT_LESS6 = 0;
        long COUNT_LESS15_OVER6 = 0;
        long COUNT_OVER15 = 0;
        long COUNT_MALE = 0;
        long COUNT_FEMALE = 0;
        long COUNT_SOFT = 0;
        long COUNT_BONE = 0;
        long COUNT_BRAIN = 0;
        long COUNT_MULTI = 0;
        long COUNT_TRAN = 0;
        long COUNT_DEATH = 0;
        long COUNT_DEATH_LATE = 0;
        long COUNT_DEATH_HEAVY = 0;
        long COUNT_DEATH_EMERGENCY = 0;

        string thisReportTypeCode = "";
        public Mrs00589Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00589Filter);
        }


        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00589Filter)this.reportFilter;
                List<long> executeRoomIds = null;
                if (this.castFilter.EXAM_ROOM_IDs != null)
                {
                    executeRoomIds = new List<long>();
                    executeRoomIds.AddRange(this.castFilter.EXAM_ROOM_IDs);
                }
                if (this.castFilter.EXE_ROOM_IDs != null)
                {
                    executeRoomIds = new List<long>();
                    executeRoomIds.AddRange(this.castFilter.EXE_ROOM_IDs);
                }
                castFilter.EXAM_ROOM_IDs = executeRoomIds;

                if (new MRS.MANAGER.Core.MrsReport.Lib.ProcessExcel().GetByCell<Mrs00589Filter>(ref dicSingleKey, ref dataObject, castFilter, this.reportTemplate.REPORT_TEMPLATE_URL, 15))
                {
                    return true;
                }
                HisAccidentHurtViewFilterQuery accidentHurtFilter = new HisAccidentHurtViewFilterQuery();
                accidentHurtFilter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                accidentHurtFilter.CREATE_TIME_TO = castFilter.TIME_TO;
                accidentHurtFilter.ACCIDENT_HURT_TYPE_ID = castFilter.ACCIDENT_HURT_TYPE_ID;
                //accidentHurtFilter.ACCIDENT_HURT_TYPE_IDs = castFilter.ACCIDENT_HURT_TYPE_IDs;
                //accidentHurtFilter.ACCIDENT_RESULT_ID = castFilter.ACCIDENT_RESULT_ID;
                //accidentHurtFilter.ACCIDENT_RESULT_IDs = castFilter.ACCIDENT_RESULT_IDs;
                listAccidentHurts = new HisAccidentHurtManager(param).GetView(accidentHurtFilter);
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    listAccidentHurts = listAccidentHurts.Where(o => o.EXECUTE_DEPARTMENT_ID.HasValue && castFilter.DEPARTMENT_IDs.Contains(o.EXECUTE_DEPARTMENT_ID.Value)).ToList();
                }
                if (castFilter.EXAM_ROOM_IDs != null)
                {
                    listAccidentHurts = listAccidentHurts.Where(o => o.EXECUTE_ROOM_ID.HasValue && castFilter.EXAM_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID.Value)).ToList();
                }
                var skip = 0;
                var treatmentIds = listAccidentHurts.Select(x => x.TREATMENT_ID).Distinct().ToList();
                while (treatmentIds.Count-skip>0)
                {
                    var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.IDs = limit;
                    var hisTreatments = new HisTreatmentManager(param).GetView(treatmentFilter);
                    listTreatment.AddRange(hisTreatments);
                }
                listTreatmentDeath = new ManagerSql().GetTreatmentDeath(this.castFilter) ?? new List<HIS_TREATMENT_DEATH>();

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

                if (dataObject.Count > 0)
                {
                    return true;
                }
                if (listAccidentHurts != null)
                {
                    this.COUNT = listAccidentHurts.Count();
                    this.COUNT_LESS6 = listAccidentHurts.Where(p => Age(p.IN_TIME, p.TDL_PATIENT_DOB) < 6).Count();
                    this.COUNT_LESS15_OVER6 = listAccidentHurts.Where(p => Age(p.IN_TIME, p.TDL_PATIENT_DOB) <= 15 && Age(p.IN_TIME, p.TDL_PATIENT_DOB) >= 6).Count();
                    this.COUNT_OVER15 = listAccidentHurts.Where(p => Age(p.IN_TIME, p.TDL_PATIENT_DOB) > 15).Count();
                    this.COUNT_MALE = listAccidentHurts.Where(p => p.TDL_PATIENT_GENDER_NAME == "Nam").Count();
                    this.COUNT_FEMALE = listAccidentHurts.Where(p => p.TDL_PATIENT_GENDER_NAME == "Nữ").Count();
                    if (castFilter.ACCIDENT_BODY_PART_CODE__SOFT != null)
                    {
                        this.COUNT_SOFT = listAccidentHurts.Where(p => castFilter.ACCIDENT_BODY_PART_CODE__SOFT.Contains(p.ACCIDENT_BODY_PART_CODE)).Count();
                    }
                    if (castFilter.ACCIDENT_BODY_PART_CODE__BONE != null)
                    {
                        this.COUNT_BONE = listAccidentHurts.Where(p => castFilter.ACCIDENT_BODY_PART_CODE__BONE.Contains(p.ACCIDENT_BODY_PART_CODE)).Count();
                    }
                    if (castFilter.ACCIDENT_BODY_PART_CODE__BRAIN != null)
                    {
                        this.COUNT_BRAIN = listAccidentHurts.Where(p => castFilter.ACCIDENT_BODY_PART_CODE__BRAIN.Contains(p.ACCIDENT_BODY_PART_CODE)).Count();
                    }
                    if (castFilter.ACCIDENT_BODY_PART_CODE__MULTI != null)
                    {
                        this.COUNT_MULTI = listAccidentHurts.Where(p => castFilter.ACCIDENT_BODY_PART_CODE__MULTI.Contains(p.ACCIDENT_BODY_PART_CODE)).Count();
                    }

                    this.COUNT_TRAN = listAccidentHurts.Where(p => p.ACCIDENT_RESULT_CODE == "03").Count();

                    this.COUNT_DEATH = listAccidentHurts.Where(p => p.ACCIDENT_RESULT_CODE == "02").Count();

                    if (castFilter.DEATH_CAUSE__LATE != null)
                    {
                        this.COUNT_DEATH_LATE = listAccidentHurts.Where(o => listTreatmentDeath.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && castFilter.DEATH_CAUSE__LATE.Contains(p.DEATH_CAUSE_CODE))).Count();
                    }
                    if (castFilter.DEATH_CAUSE__HEAVY != null)
                    {
                        this.COUNT_DEATH_HEAVY = listAccidentHurts.Where(o => listTreatmentDeath.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && castFilter.DEATH_CAUSE__HEAVY.Contains(p.DEATH_CAUSE_CODE))).Count();
                    }
                    if (castFilter.DEATH_CAUSE__EMERGENCY != null)
                    {
                        this.COUNT_DEATH_EMERGENCY = listAccidentHurts.Where(o => listTreatmentDeath.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && castFilter.DEATH_CAUSE__EMERGENCY.Contains(p.DEATH_CAUSE_CODE))).Count();
                    }
                    foreach (var item in listAccidentHurts)
                    {
                        Mrs00589RDO rdo = new Mrs00589RDO();
                        var treatment = listTreatment.Where(x => x.ID == item.TREATMENT_ID).FirstOrDefault();
                        if (treatment != null)
                        {
                            rdo.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                            rdo.TDL_PATIENT_CAREER_NAME = treatment.TDL_PATIENT_CAREER_NAME;
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.IS_FEMALE = "X";
                            }
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.IS_MALE = "X";
                            }
                            rdo.TRANSPORT_VEHICLE = treatment.TRANSPORT_VEHICLE;
                            rdo.IN_TIME = treatment.IN_TIME;
                            rdo.TREATMENT_RESULT_CODE = treatment.TREATMENT_RESULT_CODE;
                            rdo.TREATMENT_RESULT_NAME = treatment.TREATMENT_RESULT_NAME;
                            rdo.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
                            rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME ?? 0;
                            rdo.TRANSFER_IN_MEDI_ORG_CODE = treatment.TRANSFER_IN_MEDI_ORG_CODE;
                            if (rdo.TRANSFER_IN_MEDI_ORG_CODE != null)
                            {
                                rdo.IS_TRANSFER_IN_MEDI_ORG_CODE = "X";
                            }
                        }
                        var mediOrg = ListMediOrg.Where(x => x.MEDI_ORG_CODE == treatment.MEDI_ORG_CODE).FirstOrDefault();
                        if(mediOrg != null)
                        {
                            rdo.LEVEL_CODE = mediOrg.LEVEL_CODE;
                        
                        }

                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.VIR_ADDRESS = item.TDL_PATIENT_ADDRESS;
                        rdo.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB;
                        rdo.TDL_PATIENT_GENDER_NAME = item.TDL_PATIENT_GENDER_NAME;
                        rdo.ACCIDENT_TIME = item.ACCIDENT_TIME ?? 0;
                        rdo.ACCIDENT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.ACCIDENT_TIME ?? 0);
                        rdo.AGE = Age(item.IN_TIME, item.TDL_PATIENT_DOB);
                        rdo.AGE_STR = Inventec.Common.DateTime.Calculation.Age(item.TDL_PATIENT_DOB).ToString();
                        rdo.ACCIDENT_HURT_TYPE_CODE = item.ACCIDENT_HURT_TYPE_CODE;
                        rdo.ACCIDENT_BODY_PART_CODE = item.ACCIDENT_BODY_PART_CODE;
                        rdo.ACCIDENT_BODY_PART_NAME = item.ACCIDENT_BODY_PART_NAME;
                        rdo.ACCIDENT_HELMET_CODE = item.ACCIDENT_HELMET_CODE;
                        rdo.ACCIDENT_RESULT_CODE = item.ACCIDENT_RESULT_CODE;
                        rdo.ACCIDENT_RESULT_NAME = item.ACCIDENT_RESULT_NAME;
                        rdo.ACCIDENT_CARE_CODE = item.ACCIDENT_CARE_CODE;
                        rdo.IS_USE_ALCOHOL = item.IS_USE_ALCOHOL == 1 ? "X" : "";
                        rdo.USE_ALCOHOL = item.IS_USE_ALCOHOL;
                        rdo.ACCIDENT_LOCATION_CODE = item.ACCIDENT_LOCATION_CODE;
                        rdo.ACCIDENT_POISON_CODE = item.ACCIDENT_POISON_CODE;
                        rdo.ALCOHOL_TEST_RESULT = item.ALCOHOL_TEST_RESULT;
                        rdo.NARCOTICS_TEST_RESULT = item.NARCOTICS_TEST_RESULT;
                        ListRdo.Add(rdo);
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

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));

                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));

                dicSingleTag.Add("COUNT", COUNT);
                dicSingleTag.Add("COUNT_LESS6", COUNT_LESS6);
                dicSingleTag.Add("COUNT_LESS15_OVER6", COUNT_LESS15_OVER6);
                dicSingleTag.Add("COUNT_OVER15", COUNT_OVER15);
                dicSingleTag.Add("COUNT_MALE", COUNT_MALE);
                dicSingleTag.Add("COUNT_FEMALE", COUNT_FEMALE);
                dicSingleTag.Add("COUNT_SOFT", COUNT_SOFT);
                dicSingleTag.Add("COUNT_BONE", COUNT_BONE);
                dicSingleTag.Add("COUNT_BRAIN", COUNT_BRAIN);
                dicSingleTag.Add("COUNT_MULTI", COUNT_MULTI);
                dicSingleTag.Add("COUNT_TRAN", COUNT_TRAN);
                dicSingleTag.Add("COUNT_DEATH", COUNT_DEATH);
                dicSingleTag.Add("COUNT_DEATH_LATE", COUNT_DEATH_LATE);
                dicSingleTag.Add("COUNT_DEATH_HEAVY", COUNT_DEATH_HEAVY);
                dicSingleTag.Add("COUNT_DEATH_EMERGENCY", COUNT_DEATH_EMERGENCY);

                if (dicSingleKey != null && dicSingleKey.Count > 0)
                {
                    foreach (var item in dicSingleKey)
                    {
                        if (!dicSingleTag.ContainsKey(item.Key))
                        {
                            dicSingleTag.Add(item.Key, item.Value);
                        }
                        else
                        {
                            dicSingleTag[item.Key] = item.Value;
                        }
                    }
                }
                if (dataObject.Count > 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        objectTag.AddObjectData(store, "Report" + i, dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable());
                        objectTag.AddObjectData(store, "Parent" + i, dataObject[i].Count > 1 ? dataObject[i][1] : new DataTable());
                        objectTag.AddObjectData(store, "GrandParent" + i, dataObject[i].Count > 2 ? dataObject[i][2] : new DataTable());
                        objectTag.AddRelationship(store, "Parent" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
                        objectTag.AddRelationship(store, "GrandParent" + i, "Parent" + i, "GRAND_PARENT_KEY", "GRAND_PARENT_KEY");
                    }
                }
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

    }
}
