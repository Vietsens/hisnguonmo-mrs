using Inventec.Core;
using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisKskContract;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTestIndex;
using MOS.MANAGER.HisTestIndexRange;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00698
{
    class Mrs00698Processor : AbstractProcessor
    {
        Mrs00698Filter castFilter = null;
        List<Mrs00698RDO> ListRdo = new List<Mrs00698RDO>();
        List<SampleLisResultADO> ListRdoResult = new List<SampleLisResultADO>();
        List<SampleLisResultADO> ListRdoTitle = new List<SampleLisResultADO>();

        List<LIS_SAMPLE_SERVICE_STT> ListSampleServiceStt = new List<LIS_SAMPLE_SERVICE_STT>();
        List<InfoSampleResult> ListInfoSampleResult = new List<InfoSampleResult>();
        List<V_HIS_TEST_INDEX> ListTestIndexs = new List<V_HIS_TEST_INDEX>();
        List<V_HIS_TEST_INDEX_RANGE> ListTestIndexRange = new List<V_HIS_TEST_INDEX_RANGE>();
        List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>();
        string KskContractCode = "";

        public Mrs00698Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00698Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00698Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("_______Mrs00698Filter____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();


                ListRdo = new ManagerSql().GetTreatment(castFilter);

                if (castFilter.KSK_CONTRACT_ID.HasValue)
                {
                    var kskFilter = new HisKskContractFilterQuery();
                    kskFilter.ID = castFilter.KSK_CONTRACT_ID;
                    var kskcontract = new HisKskContractManager().Get(kskFilter);
                    if (IsNotNullOrEmpty(kskcontract))
                    {
                        KskContractCode = kskcontract.First().KSK_CONTRACT_CODE;
                    }
                }

                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListInfoSampleResult = new ManagerSql().GetInfoSampleResult(castFilter);
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
                ListSampleServiceStt = new ManagerSql().GetLisSampleServiceStt();
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
                ListRdoResult.Clear();
                if (IsNotNullOrEmpty(ListRdo))
                {
                    Dictionary<string, List<InfoSampleResult>> dicSample = new Dictionary<string, List<InfoSampleResult>>();
                    Dictionary<long, List<InfoSampleResult>> dicResult = new Dictionary<long, List<InfoSampleResult>>();

                    if (IsNotNullOrEmpty(ListInfoSampleResult))
                    {
                        foreach (var item in ListInfoSampleResult)
                        {
                            if (!IsNotNull(item.TEST_INDEX_CODE)) continue;

                            if (!dicResult.ContainsKey(item.SAMPLE_ID))
                            {
                                dicResult[item.SAMPLE_ID] = new List<InfoSampleResult>();
                            }

                            dicResult[item.SAMPLE_ID].Add(item);
                        }
                        foreach (var item in ListInfoSampleResult.GroupBy(o => o.SAMPLE_ID).Select(p => p.First()).ToList())
                        {
                            if (!IsNotNull(item.TREATMENT_CODE)) continue;

                            if (!dicSample.ContainsKey(item.TREATMENT_CODE))
                            {
                                dicSample[item.TREATMENT_CODE] = new List<InfoSampleResult>();
                            }
                            dicSample[item.TREATMENT_CODE].Add(item);
                        }
                    }

                    foreach (var item in ListRdo)
                    {
                        if (!dicSample.ContainsKey(item.TREATMENT_CODE)) continue;

                        item.BARCODE = dicSample[item.TREATMENT_CODE].First().BARCODE;
                        item.REQUEST_DEPARTMENT_NAME = dicSample[item.TREATMENT_CODE].First().REQUEST_DEPARTMENT_NAME;
                        item.REQUEST_ROOM_NAME = dicSample[item.TREATMENT_CODE].First().REQUEST_ROOM_NAME;
                        item.INTRUCTION_TIME = dicSample[item.TREATMENT_CODE].First().INTRUCTION_TIME;
                        item.START_TIME = dicSample[item.TREATMENT_CODE].First().START_TIME;
                        item.RESULT_TIME = dicSample[item.TREATMENT_CODE].First().START_TIME;
                        item.RESULT_USERNAME = dicSample[item.TREATMENT_CODE].First().RESULT_USERNAME;
                        item.RESULT_LOGINNAME = dicSample[item.TREATMENT_CODE].First().RESULT_USERNAME;
                        item.DIC_RESULT = new Dictionary<string, string>();
                        foreach (var sample in dicSample[item.TREATMENT_CODE])
                        {
                            if (dicResult.ContainsKey(sample.SAMPLE_ID))
                            {
                                foreach (var res in dicResult[sample.SAMPLE_ID])
                                {
                                    if (!String.IsNullOrWhiteSpace(res.VALUE))
                                    {
                                        item.DIC_RESULT[res.TEST_INDEX_CODE] = res.VALUE;
                                    }
                                    SampleLisResultADO hisSereServTein = new SampleLisResultADO();
                                    hisSereServTein.TREATMENT_ID = item.ID;
                                    var testIndChild = ListTestIndexs.FirstOrDefault(o => o.TEST_INDEX_CODE == res.TEST_INDEX_CODE);
                                    if (testIndChild != null)
                                    {
                                        //Inventec.Common.Mapper.DataObjectMapper.Map<SampleLisResultADO>(hisSereServTein, testIndChild);
                                        //hisSereServTein.IS_IMPORTANT = testIndChild.IS_IMPORTANT;
                                        //hisSereServTein.TEST_INDEX_UNIT_NAME = testIndChild.TEST_INDEX_UNIT_NAME;
                                        //hisSereServTein.NUM_ORDER = testIndChild.NUM_ORDER;
                                    }
                                    //else
                                    //{
                                    //    hisSereServTein.NUM_ORDER = null;
                                    //}

                                    var service = ListService.FirstOrDefault(o => o.SERVICE_CODE == res.SERVICE_CODE);
                                    if (service != null)
                                    {
                                        hisSereServTein.HIS_SERVICE_NUM_ORDER = service.NUM_ORDER ?? -1;
                                    }

                                    hisSereServTein.TEST_INDEX_CODE = res.TEST_INDEX_CODE ?? " ";
                                    hisSereServTein.TEST_INDEX_NAME = res.TEST_INDEX_NAME;
                                    hisSereServTein.VALUE_RANGE = res.VALUE;
                                    hisSereServTein.SAMPLE_ID = res.SAMPLE_ID;
                                    hisSereServTein.APPOINTMENT_TIME = sample.APPOINTMENT_TIME;
                                    hisSereServTein.SAMPLE_SERVICE_ID = res.SAMPLE_SERVICE_ID;
                                    if (ListSampleServiceStt != null && ListSampleServiceStt.Count > 0)
                                    {
                                        var sampleServiceResult = ListSampleServiceStt.FirstOrDefault(o => o.ID == res.SAMPLE_SERVICE_STT_ID);
                                        if (sampleServiceResult != null)
                                        {
                                            hisSereServTein.SAMPLE_SERVICE_STT_CODE = sampleServiceResult.SAMPLE_SERVICE_STT_CODE;
                                            hisSereServTein.SAMPLE_SERVICE_STT_NAME = sampleServiceResult.SAMPLE_SERVICE_STT_NAME;
                                        }

                                    }
                                    hisSereServTein.SAMPLE_SERVICE_STT_ID = res.SAMPLE_SERVICE_STT_ID;
                                    hisSereServTein.SERVICE_NUM_ORDER = res.SERVICE_NUM_ORDER ?? 9999;
                                    hisSereServTein.OLD_VALUE = res.OLD_VALUE;
                                    hisSereServTein.NOTE = res.DESCRIPTION;

                                    // gán test index range
                                    V_HIS_TEST_INDEX_RANGE testIndexRange = new V_HIS_TEST_INDEX_RANGE();
                                    //testIndexRange = GetTestIndexRange(item.TDL_PATIENT_DOB, item.TDL_PATIENT_GENDER_ID, hisSereServTein.TEST_INDEX_CODE, ListTestIndexRange);
                                    if (testIndexRange != null)
                                    {
                                        ProcessMaxMixValue(hisSereServTein, testIndexRange);
                                    }

                                    ListRdoResult.Add(hisSereServTein);
                                }
                            }
                        }
                    }
                    ListRdo = ListRdo.Where(o => o.DIC_RESULT != null && o.DIC_RESULT.Count > 0).ToList();
                }

                if (IsNotNullOrEmpty(ListRdoResult))
                {
                    ListRdoTitle = ListRdoResult.GroupBy(g => g.TEST_INDEX_CODE).Select(s => s.First()).ToList();
                    ListRdoTitle = ListRdoTitle.OrderByDescending(o => o.HIS_SERVICE_NUM_ORDER).ThenBy(o => o.SERVICE_NUM_ORDER).ThenByDescending(p => p.NUM_ORDER).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessMaxMixValue(SampleLisResultADO ti, V_HIS_TEST_INDEX_RANGE testIndexRange)
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

        private V_HIS_TEST_INDEX_RANGE GetTestIndexRange(long dob, long genderId, string testIndexId, List<V_HIS_TEST_INDEX_RANGE> listTestIndexRange)
        {
            V_HIS_TEST_INDEX_RANGE result = null;
            try
            {
                if (IsNotNullOrEmpty(listTestIndexRange))
                {
                    long age = Inventec.Common.DateTime.Calculation.Age(dob);

                    var query = listTestIndexRange.Where(o => o.TEST_INDEX_CODE == testIndexId
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

                    result = query.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
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

                if (!IsNotNullOrEmpty(ListRdoTitle))
                {
                    ListRdoTitle = new List<SampleLisResultADO>();
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportTitle", ListRdoTitle);
                objectTag.AddObjectData(store, "ReportDetail", ListRdoResult);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
