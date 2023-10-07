using Inventec.Core;
using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTestIndex;
using MOS.MANAGER.HisTestIndexRange;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00700
{
    class Mrs00700Processor : AbstractProcessor
    {
        List<Mrs00700RDO> ListRdo = new List<Mrs00700RDO>();
        List<Mrs00700RDO> ListRdoTreatment = new List<Mrs00700RDO>();
        Mrs00700Filter castFilter = null;
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<V_HIS_TEST_INDEX> ListTestIndexs = new List<V_HIS_TEST_INDEX>();
        List<V_HIS_TEST_INDEX_RANGE> ListTestIndexRange = new List<V_HIS_TEST_INDEX_RANGE>();
        List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>();
        List<V_LIS_SAMPLE> ListSample = new List<V_LIS_SAMPLE>();
        List<V_LIS_RESULT> ListResult = new List<V_LIS_RESULT>();
        List<V_HIS_KSK_CONTRACT> ListKskContract = new List<V_HIS_KSK_CONTRACT>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        public Mrs00700Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00700Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00700Filter)this.reportFilter;

                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.START_TIME_FROM = castFilter.TIME_FROM;
                reqFilter.START_TIME_TO = castFilter.TIME_TO;
                reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN;
                reqFilter.IS_SEND_LIS = true;
                ListServiceReq = new HisServiceReqManager().Get(reqFilter);

                if (castFilter.PATIENT_TYPE_ID.HasValue)
                {
                    ListServiceReq = ListServiceReq.Where(o => o.TDL_PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID.Value).ToList();
                }

                List<long> treatmentIds = ListServiceReq.Select(s => s.TREATMENT_ID).ToList();
                int skip = 0;
                while (treatmentIds.Count - skip > 0)
                {
                    var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.IDs = listIds;
                    var treatment = new HisTreatmentManager().Get(treatmentFilter);
                    if (IsNotNullOrEmpty(treatment))
                    {
                        ListTreatment.AddRange(treatment);
                    }
                }

                var Samples = new ManagerSql().GetLisSample(ListServiceReq.Select(s => s.SERVICE_REQ_CODE).ToList());
                if (IsNotNullOrEmpty(Samples))
                {
                    ListSample.AddRange(Samples);
                    var listResult = new ManagerSql().GetLisResult(Samples.Select(s => s.ID).ToList());
                    if (IsNotNullOrEmpty(listResult))
                    {
                        ListResult.AddRange(listResult);
                    }
                }

                var testIndexFilter = new HisTestIndexViewFilterQuery();
                testIndexFilter.IS_ACTIVE = 1;
                ListTestIndexs = new HisTestIndexManager().GetView(testIndexFilter);

                var testIndexRangeFilter = new HisTestIndexRangeViewFilterQuery();
                testIndexRangeFilter.IS_ACTIVE = 1;
                ListTestIndexRange = new HisTestIndexRangeManager().GetView(testIndexRangeFilter);

                var serviceFilter = new HisServiceViewFilterQuery();
                serviceFilter.IS_ACTIVE = 1;
                serviceFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN;
                ListService = new HisServiceManager().GetView(serviceFilter);

                string sql = "SELECT * FROM V_HIS_KSK_CONTRACT";
                ListKskContract = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_KSK_CONTRACT>(sql);
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
                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    Dictionary<string, List<V_LIS_SAMPLE>> dicSample = new Dictionary<string, List<V_LIS_SAMPLE>>();
                    Dictionary<long, List<V_LIS_RESULT>> dicResult = new Dictionary<long, List<V_LIS_RESULT>>();
                    Dictionary<long, HIS_TREATMENT> dicTreatmentKsk = new Dictionary<long, HIS_TREATMENT>();

                    if (IsNotNullOrEmpty(ListResult))
                    {
                        foreach (var item in ListResult)
                        {
                            if (!IsNotNull(item.TEST_INDEX_CODE)) continue;

                            if (!dicResult.ContainsKey(item.SAMPLE_ID ?? 0))
                            {
                                dicResult[item.SAMPLE_ID ?? 0] = new List<V_LIS_RESULT>();
                            }

                            dicResult[item.SAMPLE_ID ?? 0].Add(item);
                        }

                        Inventec.Common.Logging.LogSystem.Warn("ListResult Count:" + ListResult.Count);
                    }

                    if (IsNotNullOrEmpty(ListSample))
                    {
                        //mẫu phải có ít nhất 1 kết quả
                        ListSample = ListSample.Where(o => ListResult.Select(s => s.SAMPLE_ID).Contains(o.ID)).ToList();
                        foreach (var item in ListSample)
                        {
                            if (!IsNotNull(item.SERVICE_REQ_CODE)) continue;

                            if (!dicSample.ContainsKey(item.SERVICE_REQ_CODE))
                            {
                                dicSample[item.SERVICE_REQ_CODE] = new List<V_LIS_SAMPLE>();
                            }

                            dicSample[item.SERVICE_REQ_CODE].Add(item);
                        }

                        Inventec.Common.Logging.LogSystem.Warn("ListSample Count:" + ListSample.Count);
                    }

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        var treatmentKsk = ListTreatment.Where(o => o.TDL_KSK_CONTRACT_ID.HasValue).ToList();
                        foreach (var item in treatmentKsk)
                        {
                            dicTreatmentKsk[item.ID] = item;
                        }
                    }

                    foreach (var item in ListServiceReq)
                    {
                        if (!dicSample.ContainsKey(item.SERVICE_REQ_CODE)) continue;
                        var treatmentType = MANAGER.Config.HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == item.TREATMENT_TYPE_ID);

                        foreach (var sample in dicSample[item.SERVICE_REQ_CODE])
                        {
                            if (dicResult.ContainsKey(sample.ID))
                            {
                                foreach (var res in dicResult[sample.ID])
                                {
                                    var testIndChild = ListTestIndexs.FirstOrDefault(o => o.TEST_INDEX_CODE == res.TEST_INDEX_CODE);
                                    Mrs00700RDO hisSereServTein = new Mrs00700RDO();
                                    hisSereServTein.ICD_CODE = item.ICD_CODE;
                                    hisSereServTein.ICD_NAME = item.ICD_NAME;
                                    hisSereServTein.REQUEST_ROOM_ID = item.REQUEST_ROOM_ID;
                                    hisSereServTein.TDL_REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(h => h.ID == item.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                                    hisSereServTein.TDL_REQUEST_LOGINNAME = item.REQUEST_LOGINNAME;
                                    hisSereServTein.TDL_REQUEST_USERNAME = item.REQUEST_USERNAME;
                                    hisSereServTein.EXECUTE_ROOM_ID = item.EXECUTE_ROOM_ID;
                                    hisSereServTein.TDL_EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(h => h.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                                    hisSereServTein.BARCODE = dicSample[item.SERVICE_REQ_CODE].First().BARCODE;
                                    hisSereServTein.SERVICE_REQ_CODE = item.SERVICE_REQ_CODE;
                                    hisSereServTein.TDL_HEIN_CARD_NUMBER = item.TDL_HEIN_CARD_NUMBER;
                                    hisSereServTein.TDL_HEIN_MEDI_ORG_CODE = item.TDL_HEIN_MEDI_ORG_CODE;
                                    hisSereServTein.TDL_HEIN_MEDI_ORG_NAME = item.TDL_HEIN_MEDI_ORG_NAME;
                                    hisSereServTein.TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;
                                    hisSereServTein.TDL_PATIENT_CAREER_NAME = item.TDL_PATIENT_CAREER_NAME;
                                    hisSereServTein.TDL_PATIENT_CODE = item.TDL_PATIENT_CODE;
                                    hisSereServTein.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB;
                                    hisSereServTein.TDL_PATIENT_GENDER_ID = item.TDL_PATIENT_GENDER_ID;
                                    if (item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                                    {
                                        hisSereServTein.FEMALE_DOB = RDOCommon.CalculateAge(item.TDL_PATIENT_DOB);
                                    }
                                    else if (item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                                    {
                                        hisSereServTein.MALE_DOB = RDOCommon.CalculateAge(item.TDL_PATIENT_DOB);
                                    }
                                    hisSereServTein.TDL_PATIENT_GENDER_NAME = item.TDL_PATIENT_GENDER_NAME;
                                    hisSereServTein.TDL_PATIENT_MILITARY_RANK_NAME = item.TDL_PATIENT_MILITARY_RANK_NAME;
                                    hisSereServTein.TDL_PATIENT_MOBILE = item.TDL_PATIENT_MOBILE;
                                    hisSereServTein.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                                    hisSereServTein.TDL_PATIENT_NATIONAL_NAME = item.TDL_PATIENT_NATIONAL_NAME;
                                    hisSereServTein.TDL_PATIENT_PHONE = item.TDL_PATIENT_PHONE;
                                    hisSereServTein.TDL_PATIENT_TYPE_ID = item.TDL_PATIENT_TYPE_ID;
                                    hisSereServTein.TDL_PATIENT_WORK_PLACE = item.TDL_PATIENT_WORK_PLACE;
                                    hisSereServTein.TDL_PATIENT_WORK_PLACE_NAME = item.TDL_PATIENT_WORK_PLACE_NAME;
                                    hisSereServTein.TDL_TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                                    hisSereServTein.TDL_TREATMENT_TYPE_ID = item.TDL_TREATMENT_TYPE_ID;
                                    hisSereServTein.TDL_TREATMENT_TYPE_NAME = treatmentType != null ? treatmentType.TREATMENT_TYPE_NAME : "";
                                    if (dicTreatmentKsk.ContainsKey(item.TREATMENT_ID))
                                    {
                                        var ksk = ListKskContract.FirstOrDefault(o => o.ID == dicTreatmentKsk[item.TREATMENT_ID].TDL_KSK_CONTRACT_ID);
                                        if (ksk != null)
                                        {
                                            hisSereServTein.KSK_WORK_PLACE_NAME = ksk.WORK_PLACE_NAME;
                                            hisSereServTein.KSK_CONTRACT_CODE = ksk.KSK_CONTRACT_CODE;
                                        }
                                    }

                                    if (testIndChild != null)
                                    {
                                        hisSereServTein.IS_IMPORTANT = testIndChild.IS_IMPORTANT;
                                        hisSereServTein.TEST_INDEX_UNIT_NAME = testIndChild.TEST_INDEX_UNIT_NAME;
                                        hisSereServTein.NUM_ORDER = testIndChild.NUM_ORDER;
                                    }
                                    else
                                    {
                                        hisSereServTein.NUM_ORDER = null;
                                    }

                                    var service = ListService.FirstOrDefault(o => o.SERVICE_CODE == res.SERVICE_CODE);
                                    if (service != null)
                                    {
                                        hisSereServTein.HIS_SERVICE_NUM_ORDER = service.NUM_ORDER ?? -1;
                                        hisSereServTein.HIS_SERVICE_CODE = service.SERVICE_CODE;
                                        hisSereServTein.HIS_SERVICE_NAME = service.SERVICE_NAME;
                                    }

                                    hisSereServTein.TEST_INDEX_CODE = res.TEST_INDEX_CODE ?? " ";
                                    hisSereServTein.TEST_INDEX_NAME = res.TEST_INDEX_NAME;
                                    hisSereServTein.VALUE_RANGE = res.VALUE;
                                    hisSereServTein.SAMPLE_ID = res.SAMPLE_ID;
                                    hisSereServTein.SAMPLE_SERVICE_ID = res.SAMPLE_SERVICE_ID;
                                    hisSereServTein.SAMPLE_SERVICE_STT_CODE = res.SAMPLE_SERVICE_STT_CODE;
                                    hisSereServTein.SAMPLE_SERVICE_STT_ID = res.SAMPLE_SERVICE_STT_ID;
                                    hisSereServTein.SAMPLE_SERVICE_STT_NAME = res.SAMPLE_SERVICE_STT_NAME;
                                    hisSereServTein.SERVICE_NUM_ORDER = res.SERVICE_NUM_ORDER ?? 9999;
                                    hisSereServTein.OLD_VALUE = res.OLD_VALUE;
                                    hisSereServTein.NOTE = res.DESCRIPTION;
                                    hisSereServTein.MACHINE_ID = res.MACHINE_ID.HasValue ? res.MACHINE_ID : res.SERVICE_MACHINE_ID;
                                    hisSereServTein.MACHINE_ID_OLD = res.MACHINE_ID.HasValue ? res.MACHINE_ID : res.SERVICE_MACHINE_ID;
                                    hisSereServTein.LIS_RESULT_ID = res.ID;
                                    hisSereServTein.DESCRIPTION = "";

                                    // gán test index range
                                    V_HIS_TEST_INDEX_RANGE testIndexRange = new V_HIS_TEST_INDEX_RANGE();
                                    testIndexRange = GetTestIndexRange(item.TDL_PATIENT_DOB, item.TDL_PATIENT_GENDER_ID ?? 0, hisSereServTein.TEST_INDEX_CODE, ListTestIndexRange);
                                    if (testIndexRange != null)
                                    {
                                        ProcessMaxMixValue(hisSereServTein, testIndexRange);
                                    }

                                    ListRdo.Add(hisSereServTein);
                                }
                            }
                        }
                    }
                }

                ListRdo = ListRdo.OrderBy(o => o.TDL_TREATMENT_CODE).ThenBy(o => o.SERVICE_REQ_CODE).ThenByDescending(o => o.HIS_SERVICE_NUM_ORDER).ThenBy(o => o.SERVICE_NUM_ORDER).ThenByDescending(p => p.NUM_ORDER).ToList();
                ListRdoTreatment = ListRdo.GroupBy(p => new { p.TDL_TREATMENT_CODE, p.REQUEST_ROOM_ID, p.TDL_REQUEST_LOGINNAME, p.EXECUTE_ROOM_ID }).Select(o => new Mrs00700RDO
                {
                    TDL_PATIENT_NAME = o.First().TDL_PATIENT_NAME,
                    TDL_TREATMENT_CODE = o.First().TDL_TREATMENT_CODE,
                    MALE_DOB = o.First().MALE_DOB,
                    FEMALE_DOB = o.First().FEMALE_DOB,
                    TDL_PATIENT_ADDRESS = o.First().TDL_PATIENT_ADDRESS,
                    ICD_CODE = o.First().ICD_CODE,
                    ICD_NAME = o.First().ICD_NAME,
                    TDL_REQUEST_ROOM_NAME = o.First().TDL_REQUEST_ROOM_NAME,
                    HIS_SERVICE_CODE = o.First().HIS_SERVICE_CODE,
                    HIS_SERVICE_NAME = o.First().HIS_SERVICE_NAME,
                    TDL_REQUEST_USERNAME = o.First().TDL_REQUEST_USERNAME,
                    TDL_EXECUTE_ROOM_NAME = o.First().TDL_EXECUTE_ROOM_NAME,
                }).ToList();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        V_HIS_TEST_INDEX_RANGE GetTestIndexRange(long dob, long genderId, string testIndexCode, List<V_HIS_TEST_INDEX_RANGE> testIndexRanges)
        {
            MOS.EFMODEL.DataModels.V_HIS_TEST_INDEX_RANGE testIndexRange = new V_HIS_TEST_INDEX_RANGE();
            try
            {
                if (testIndexRanges != null && testIndexRanges.Count > 0)
                {
                    long age = Inventec.Common.DateTime.Calculation.Age(dob);

                    var query = testIndexRanges.Where(o => o.TEST_INDEX_CODE == testIndexCode
                            && ((o.AGE_FROM.HasValue && o.AGE_FROM.Value <= age) || !o.AGE_FROM.HasValue)
                            && ((o.AGE_TO.HasValue && o.AGE_TO.Value >= age) || !o.AGE_TO.HasValue));
                    if (genderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        query = query.Where(o => o.IS_MALE == 1);
                    }
                    else if (genderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        query = query.Where(o => o.IS_FEMALE == 1);
                    }

                    testIndexRange = query.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                testIndexRange = new V_HIS_TEST_INDEX_RANGE();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return testIndexRange;
        }

        private void ProcessMaxMixValue(Mrs00700RDO ti, V_HIS_TEST_INDEX_RANGE testIndexRange)
        {
            try
            {
                Decimal minValue = 0, maxValue = 0, value = 0;
                if (ti != null && testIndexRange != null)
                {
                    ti.DESCRIPTION = "";
                    if (!String.IsNullOrWhiteSpace(testIndexRange.MIN_VALUE))
                    {
                        if (Decimal.TryParse((testIndexRange.MIN_VALUE ?? "").Replace('.', ','), out minValue))
                        {
                            ti.MIN_VALUE = minValue;
                        }
                        else
                        {
                            ti.MIN_VALUE = null;
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(testIndexRange.MAX_VALUE))
                    {
                        if (Decimal.TryParse((testIndexRange.MAX_VALUE ?? "").Replace('.', ','), out maxValue))
                        {
                            ti.MAX_VALUE = maxValue;
                        }
                        else
                        {
                            ti.MAX_VALUE = null;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(ti.VALUE_RANGE))
                    {
                        if (Decimal.TryParse((ti.VALUE_RANGE ?? "").Replace('.', ','), out value))
                        {
                            ti.VALUE = value;
                        }
                        else
                        {
                            ti.VALUE = null;
                        }
                    }

                    ti.IS_NORMAL = null;
                    ti.IS_HIGHER = null;
                    ti.IS_LOWER = null;

                    if (!String.IsNullOrEmpty(testIndexRange.NORMAL_VALUE))
                    {
                        ti.DESCRIPTION = testIndexRange.NORMAL_VALUE;
                        if (!String.IsNullOrWhiteSpace(ti.DESCRIPTION) && !String.IsNullOrWhiteSpace(ti.VALUE_RANGE) && ti.VALUE_RANGE.ToUpper() == ti.DESCRIPTION.ToUpper())
                        {
                            ti.IS_NORMAL = true;
                        }
                        else if (!String.IsNullOrWhiteSpace(ti.DESCRIPTION) && !String.IsNullOrWhiteSpace(ti.VALUE_RANGE) && ti.VALUE_RANGE.ToUpper() != ti.DESCRIPTION.ToUpper())
                        {
                            ti.IS_LOWER = true;
                            ti.IS_HIGHER = true;
                        }
                    }
                    else
                    {
                        if (testIndexRange.IS_ACCEPT_EQUAL_MIN == 1 && testIndexRange.IS_ACCEPT_EQUAL_MAX == null)
                        {
                            if (testIndexRange.MIN_VALUE != null)
                            {
                                ti.DESCRIPTION += testIndexRange.MIN_VALUE + "<= ";
                            }

                            ti.DESCRIPTION += "X";

                            if (testIndexRange.MAX_VALUE != null)
                            {
                                ti.DESCRIPTION += " < " + testIndexRange.MAX_VALUE;
                            }

                            if (ti.VALUE != null && ti.MIN_VALUE != null && ti.MIN_VALUE <= ti.VALUE && ti.MAX_VALUE != null && ti.VALUE < ti.MAX_VALUE)
                            {
                                ti.IS_NORMAL = true;
                            }
                            else if (ti.VALUE != null && ti.MIN_VALUE != null && ti.VALUE < ti.MIN_VALUE)
                            {
                                ti.IS_LOWER = true;
                            }
                            else if (ti.VALUE != null && ti.MAX_VALUE != null && ti.MAX_VALUE <= ti.VALUE)
                            {
                                ti.IS_HIGHER = true;
                            }
                        }
                        else if (testIndexRange.IS_ACCEPT_EQUAL_MIN == 1 && testIndexRange.IS_ACCEPT_EQUAL_MAX == 1)
                        {
                            if (testIndexRange.MIN_VALUE != null)
                            {
                                ti.DESCRIPTION += testIndexRange.MIN_VALUE + "<= ";
                            }

                            ti.DESCRIPTION += "X";

                            if (testIndexRange.MAX_VALUE != null)
                            {
                                ti.DESCRIPTION += " <= " + testIndexRange.MAX_VALUE;
                            }

                            if (ti.VALUE != null && ti.MIN_VALUE != null && ti.MIN_VALUE <= ti.VALUE && ti.MAX_VALUE != null && ti.VALUE <= ti.MAX_VALUE)
                            {
                                ti.IS_NORMAL = true;
                            }
                            else if (ti.VALUE != null && ti.MIN_VALUE != null && ti.VALUE < ti.MIN_VALUE)
                            {
                                ti.IS_LOWER = true;
                            }
                            else if (ti.VALUE != null && ti.MAX_VALUE != null && ti.MAX_VALUE < ti.VALUE)
                            {
                                ti.IS_HIGHER = true;
                            }
                        }
                        else if (testIndexRange.IS_ACCEPT_EQUAL_MIN == null && testIndexRange.IS_ACCEPT_EQUAL_MAX == 1)
                        {
                            if (testIndexRange.MIN_VALUE != null)
                            {
                                ti.DESCRIPTION += testIndexRange.MIN_VALUE + "< ";
                            }

                            ti.DESCRIPTION += "X";

                            if (testIndexRange.MAX_VALUE != null)
                            {
                                ti.DESCRIPTION += " <= " + testIndexRange.MAX_VALUE;
                            }

                            if (ti.VALUE != null && ti.MIN_VALUE != null && ti.MIN_VALUE < ti.VALUE && ti.MAX_VALUE != null && ti.VALUE <= ti.MAX_VALUE)
                            {
                                ti.IS_NORMAL = true;
                            }
                            else if (ti.VALUE != null && ti.MIN_VALUE != null && ti.VALUE < ti.MIN_VALUE)
                            {
                                ti.IS_LOWER = true;
                            }
                            else if (ti.VALUE != null && ti.MAX_VALUE != null && ti.MAX_VALUE < ti.VALUE)
                            {
                                ti.IS_HIGHER = true;
                            }
                        }
                        else if (testIndexRange.IS_ACCEPT_EQUAL_MIN == null && testIndexRange.IS_ACCEPT_EQUAL_MAX == null)
                        {
                            if (testIndexRange.MIN_VALUE != null)
                            {
                                ti.DESCRIPTION += testIndexRange.MIN_VALUE + "< ";
                            }

                            ti.DESCRIPTION += "X";

                            if (testIndexRange.MAX_VALUE != null)
                            {
                                ti.DESCRIPTION += " < " + testIndexRange.MAX_VALUE;
                            }
                            if (ti.VALUE != null && ti.MIN_VALUE != null && ti.MIN_VALUE < ti.VALUE && ti.MAX_VALUE != null && ti.VALUE < ti.MAX_VALUE)
                            {
                                ti.IS_NORMAL = true;
                            }
                            else if (ti.VALUE != null && ti.MIN_VALUE != null && ti.VALUE <= ti.MIN_VALUE)
                            {
                                ti.IS_LOWER = true;
                            }
                            else if (ti.VALUE != null && ti.MAX_VALUE != null && ti.MAX_VALUE <= ti.VALUE)
                            {
                                ti.IS_HIGHER = true;
                            }
                        }
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
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportTreatment", ListRdoTreatment);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
