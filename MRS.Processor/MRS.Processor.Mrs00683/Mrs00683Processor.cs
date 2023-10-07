using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Core.SdaProvince.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00683
{
    class Mrs00683Processor : AbstractProcessor
    {
        private Mrs00683Filter castFilter;
        private List<HIS_TREATMENT> LisTreatments = new List<HIS_TREATMENT>();
        private HIS_BRANCH Branch = new HIS_BRANCH();//HEIN_PROVINCE_CODE check cùng tỉnh. Là ô Mã tỉnh ở chức năng cơ sở/xã phường.
        private List<Mrs00683RDO> ListRdo = new List<Mrs00683RDO>();
        private List<SDA_PROVINCE> ListProvice = new List<SDA_PROVINCE>();
        private List<SDA_DISTRICT> ListDistrict = new List<SDA_DISTRICT>();

        public Mrs00683Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00683Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                castFilter = (Mrs00683Filter)reportFilter;

                Branch = new MOS.MANAGER.HisBranch.HisBranchManager().GetById(this.branch_id);

                ListProvice = new SDA.MANAGER.Manager.SdaProvinceManager(paramGet).Get<List<SDA_PROVINCE>>(new SdaProvinceFilterQuery());
                ListDistrict = new SDA.MANAGER.Manager.SdaDistrictManager(paramGet).Get<List<SDA_DISTRICT>>(new SdaDistrictFilterQuery());

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

                if (IsNotNullOrEmpty(LisTreatments) && IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                {
                    LisTreatments = LisTreatments.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
            }
            catch (Exception ex)
            {
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
                    //gom theo tỉnh để gom cả trường hợp có tỉnh mà ko có huyện
                    var groupProvince = LisTreatments.GroupBy(o => o.TDL_PATIENT_PROVINCE_CODE).ToList();
                    foreach (var grProvice in groupProvince)
                    {
                        var provice = ListProvice.FirstOrDefault(o => o.PROVINCE_CODE == grProvice.First().TDL_PATIENT_PROVINCE_CODE);
                        if (provice == null || provice.ID <= 0)
                        {
                            provice = new SDA_PROVINCE();
                            provice.PROVINCE_CODE = "9999";
                            provice.PROVINCE_NAME = "Khác";
                        }

                        var groupDistrict = grProvice.GroupBy(o => o.TDL_PATIENT_DISTRICT_CODE).ToList();
                        foreach (var grdistrict in groupDistrict)
                        {
                            Mrs00683RDO rdo = new Mrs00683RDO();

                            var district = ListDistrict.FirstOrDefault(o => o.DISTRICT_CODE == grdistrict.First().TDL_PATIENT_DISTRICT_CODE);
                            if (district == null || district.ID <= 0)
                            {
                                district = new SDA_DISTRICT();
                                district.DISTRICT_CODE = "99999";
                                district.DISTRICT_NAME = "Khác";
                            }

                            rdo.DISTRICT_CODE = district.DISTRICT_CODE;
                            rdo.DISTRICT_NAME = string.Format("{0} {1}", district.INITIAL_NAME, district.DISTRICT_NAME);

                            rdo.PROVINCE_CODE = provice.PROVINCE_CODE;
                            rdo.PROVINCE_NAME = provice.PROVINCE_NAME;

                            rdo.TOTAL_UNDER_6_AGE = grdistrict.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 6);
                            rdo.TOTAL_UPPER_6_AGE = grdistrict.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 6);

                            var maleTreatment = grdistrict.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).ToList();
                            var femaleTreatment = grdistrict.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList();

                            if (IsNotNullOrEmpty(maleTreatment))
                            {
                                rdo.TOTAL_6_AGE_MALE = maleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 5 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) <= 6);
                                rdo.TOTAL_UNDER_1_AGE_MALE = maleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 1);
                                rdo.TOTAL_UNDER_3_AGE_MALE = maleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 1 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 3);
                                rdo.TOTAL_UNDER_5_AGE_MALE = maleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 3 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 5);
                                rdo.TOTAL_UPPER_6_AGE_MALE = maleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) > 6);
                            }

                            if (IsNotNullOrEmpty(femaleTreatment))
                            {
                                rdo.TOTAL_6_AGE_FEMALE = femaleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 5 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) <= 6);
                                rdo.TOTAL_UNDER_1_AGE_FEMALE = femaleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 1);
                                rdo.TOTAL_UNDER_3_AGE_FEMALE = femaleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 1 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 3);
                                rdo.TOTAL_UNDER_5_AGE_FEMALE = femaleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) >= 3 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) < 5);
                                rdo.TOTAL_UPPER_6_AGE_FEMALE = femaleTreatment.Count(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB, o.IN_TIME) > 6);
                            }

                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
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

                dicSingleTag.Add("TOTAL_6_AGE_FEMALE", ListRdo.Sum(s => s.TOTAL_6_AGE_FEMALE ?? 0));
                dicSingleTag.Add("TOTAL_6_AGE_MALE", ListRdo.Sum(s => s.TOTAL_6_AGE_MALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_1_AGE_FEMALE", ListRdo.Sum(s => s.TOTAL_UNDER_1_AGE_FEMALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_1_AGE_MALE", ListRdo.Sum(s => s.TOTAL_UNDER_1_AGE_MALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_3_AGE_FEMALE", ListRdo.Sum(s => s.TOTAL_UNDER_3_AGE_FEMALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_3_AGE_MALE", ListRdo.Sum(s => s.TOTAL_UNDER_3_AGE_MALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_5_AGE_FEMALE", ListRdo.Sum(s => s.TOTAL_UNDER_5_AGE_FEMALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_5_AGE_MALE", ListRdo.Sum(s => s.TOTAL_UNDER_5_AGE_MALE ?? 0));
                dicSingleTag.Add("TOTAL_UNDER_6_AGE", ListRdo.Sum(s => s.TOTAL_UNDER_6_AGE ?? 0));
                dicSingleTag.Add("TOTAL_UPPER_6_AGE", ListRdo.Sum(s => s.TOTAL_UPPER_6_AGE ?? 0));
                dicSingleTag.Add("TOTAL_UPPER_6_AGE_FEMALE", ListRdo.Sum(s => s.TOTAL_UPPER_6_AGE_FEMALE ?? 0));
                dicSingleTag.Add("TOTAL_UPPER_6_AGE_MALE", ListRdo.Sum(s => s.TOTAL_UPPER_6_AGE_MALE ?? 0));

                List<Mrs00683RDO> ListRdoProvince = new List<Mrs00683RDO>();

                //true: tỉnh khác; false: cùng tỉnh
                //true: gom theo tỉnh
                //false: gom theo huyện
                if (castFilter.IS_PROVINCE)
                {
                    dicSingleTag.Add("IS_PROVINCE", "x");
                    ListRdoProvince = ListRdo.Where(o => o.PROVINCE_CODE != Branch.HEIN_PROVINCE_CODE).GroupBy(g => g.PROVINCE_CODE).Select(s =>
                        new Mrs00683RDO()
                        {
                            DISTRICT_CODE = s.First().PROVINCE_CODE,
                            DISTRICT_NAME = s.First().PROVINCE_NAME,
                            TOTAL_6_AGE_FEMALE = s.Sum(o => o.TOTAL_6_AGE_FEMALE ?? 0),
                            TOTAL_6_AGE_MALE = s.Sum(o => o.TOTAL_6_AGE_MALE ?? 0),
                            TOTAL_UNDER_1_AGE_FEMALE = s.Sum(o => o.TOTAL_UNDER_1_AGE_FEMALE ?? 0),
                            TOTAL_UNDER_1_AGE_MALE = s.Sum(o => o.TOTAL_UNDER_1_AGE_MALE ?? 0),
                            TOTAL_UNDER_3_AGE_FEMALE = s.Sum(o => o.TOTAL_UNDER_3_AGE_FEMALE ?? 0),
                            TOTAL_UNDER_3_AGE_MALE = s.Sum(o => o.TOTAL_UNDER_3_AGE_MALE ?? 0),
                            TOTAL_UNDER_5_AGE_FEMALE = s.Sum(o => o.TOTAL_UNDER_5_AGE_FEMALE ?? 0),
                            TOTAL_UNDER_5_AGE_MALE = s.Sum(o => o.TOTAL_UNDER_5_AGE_MALE ?? 0),
                            TOTAL_UNDER_6_AGE = s.Sum(o => o.TOTAL_UNDER_6_AGE ?? 0),
                            TOTAL_UPPER_6_AGE = s.Sum(o => o.TOTAL_UPPER_6_AGE ?? 0),
                            TOTAL_UPPER_6_AGE_FEMALE = s.Sum(o => o.TOTAL_UPPER_6_AGE_FEMALE ?? 0),
                            TOTAL_UPPER_6_AGE_MALE = s.Sum(o => o.TOTAL_UPPER_6_AGE_MALE ?? 0)
                        }
                        ).ToList();
                }
                else
                {
                    dicSingleTag.Add("IS_PROVINCE", "");
                    ListRdoProvince = ListRdo.Where(o => o.PROVINCE_CODE == Branch.HEIN_PROVINCE_CODE).ToList();
                }

                objectTag.AddObjectData(store, "Report", ListRdoProvince.OrderBy(o => o.DISTRICT_CODE).ToList());
                store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
