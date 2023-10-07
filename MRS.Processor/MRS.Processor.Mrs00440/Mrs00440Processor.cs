using MOS.MANAGER.HisAccidentHurt;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisAccidentLocation;
using MOS.MANAGER.HisAccidentHurtType;
using MOS.MANAGER.HisAccidentCare;
using MOS.MANAGER.HisAccidentBodyPart;
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

namespace MRS.Processor.Mrs00440
{
    class Mrs00440Processor : AbstractProcessor
    {
        Mrs00440Filter castFilter = null;
        List<Mrs00440RDO> listRdo = new List<Mrs00440RDO>();
        List<Mrs00440RDO> listRdoDetail = new List<Mrs00440RDO>();
        List<Mrs00440RDO> listGroupName = new List<Mrs00440RDO>();
        //List<V_HIS_ACCIDENT_HURT> listAccidentHurts = new List<V_HIS_ACCIDENT_HURT>();
        List<HIS_ACCIDENT_HURT_TYPE> listAccidentHurtTypes = new List<HIS_ACCIDENT_HURT_TYPE>();
        //List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        //List<HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_ACCIDENT_LOCATION> listAccidentLocations = new List<HIS_ACCIDENT_LOCATION>();
        List<HIS_ACCIDENT_BODY_PART> listAccidentBodyParts = new List<HIS_ACCIDENT_BODY_PART>();
        List<HIS_ACCIDENT_CARE> listAccidentCares = new List<HIS_ACCIDENT_CARE>();
        List<HIS_CAREER> listCareers = new List<HIS_CAREER>();
        //V_HIS_PATIENT

        string thisReportTypeCode = "";
        public Mrs00440Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00440Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }


                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o=>o.SERVICE_GROUP_ID).ToList());
                objectTag.AddObjectData(store, "GroupName", listGroupName);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupName", "Report", "GROUP_ID", "GROUP_ID");

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00440Filter)this.reportFilter;

                listRdoDetail = new ManagerSql().GetAccident(castFilter);
                
                //HisCareerFilterQuery careerFilter = new HisCareerFilterQuery(); 
                listCareers = new MOS.MANAGER.HisCareer.HisCareerManager(param).Get(new HisCareerFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE });
                listAccidentHurtTypes = new MOS.MANAGER.HisAccidentHurtType.HisAccidentHurtTypeManager(param).Get(new HisAccidentHurtTypeFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE });
                listAccidentCares = new MOS.MANAGER.HisAccidentCare.HisAccidentCareManager(param).Get(new HisAccidentCareFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE });
                listAccidentBodyParts = new MOS.MANAGER.HisAccidentBodyPart.HisAccidentBodyPartManager(param).Get(new HisAccidentBodyPartFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE});
                listAccidentLocations = new MOS.MANAGER.HisAccidentLocation.HisAccidentLocationManager(param).Get(new HisAccidentLocationFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE });
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
                CommonParam paramGet = new CommonParam();
                
                
                ///////////////////////
                #region//NgheNghiep
                foreach (var careers in listCareers)
                {
                    #region
                    Mrs00440RDO rdo = new Mrs00440RDO();
                    rdo.GROUP_ID = 2;
                    rdo.GROUP_NAME = "Nghề nghiệp";
                    rdo.SERVICE_GROUP_CODE = careers.CAREER_CODE;
                    rdo.SERVICE_GROUP_NAME = careers.CAREER_NAME;
                    rdo.SERVICE_GROUP_ID = careers.ID;
                    var listTreatment = listRdoDetail.Where(s => s.CAREER_ID == careers.ID).ToList();
                    processCountTreatment(rdo, listTreatment);
                    listRdo.Add(rdo);
                    #endregion
                }
                #endregion
                #region//DiaDiem
                foreach (var accidentLocations in listAccidentLocations)
                {
                    #region
                    Mrs00440RDO rdo = new Mrs00440RDO();
                    rdo.GROUP_ID = 3;
                    rdo.GROUP_NAME = "Địa điêm xẩy ra";
                    rdo.SERVICE_GROUP_CODE = accidentLocations.ACCIDENT_LOCATION_CODE;
                    rdo.SERVICE_GROUP_NAME = accidentLocations.ACCIDENT_LOCATION_NAME;
                    rdo.SERVICE_GROUP_ID = accidentLocations.ID;

                    var listTreatment = listRdoDetail.Where(s => s.ACCIDENT_LOCATION_ID == accidentLocations.ID).ToList();
                    processCountTreatment(rdo, listTreatment);

                    listRdo.Add(rdo);
                    #endregion
                }
                #endregion
                #region//BoPhanChanThuongICD10
                if (this.castFilter.IS_BODY_PART_FROM_ICD == true)
                {
                    List<string> listStartWithS0ToS19 = new List<string>();
                    for (int i = 0; i < 20; i++)
                    {
                        string startwith = string.Format("S{0:00}", Convert.ToInt64(i));
                        listStartWithS0ToS19.Add(startwith);
                    }

                    AddGroupBophan(listStartWithS0ToS19, null, "S0ToS19", "Đầu, mặt, cổ (S00-S19)", -5);


                    List<string> listStartWithS20ToS39 = new List<string>();
                    for (int i = 20; i < 40; i++)
                    {
                        string startwith = string.Format("S{0:00}", Convert.ToInt64(i));
                        listStartWithS20ToS39.Add(startwith);
                    }

                    AddGroupBophan(listStartWithS20ToS39, null, "S20ToS39", "Thân mình (S20-S39)", -4);

                    List<string> listStartWithS40ToS99 = new List<string>();
                    for (int i = 40; i < 100; i++)
                    {
                        string startwith = string.Format("S{0:00}", Convert.ToInt64(i));
                        listStartWithS40ToS99.Add(startwith);
                    }
                    AddGroupBophan(listStartWithS40ToS99, null, "S40ToS99", "Chi (S40-S99)", -3);

                    List<string> listStartWithT00ToT07 = new List<string>();
                    for (int i = 0; i < 8; i++)
                    {
                        string startwith = string.Format("T{0:00}", Convert.ToInt64(i));
                        listStartWithT00ToT07.Add(startwith);
                    }
                    AddGroupBophan(listStartWithT00ToT07, null, "T00ToT07", "Đa chấn thuơng (T00-T07)", -2);

                    List<string> listStartWithAllBophan = new List<string>();
                    listStartWithAllBophan.AddRange(listStartWithS0ToS19);
                    listStartWithAllBophan.AddRange(listStartWithS20ToS39);
                    listStartWithAllBophan.AddRange(listStartWithS40ToS99);
                    listStartWithAllBophan.AddRange(listStartWithT00ToT07);
                    AddGroupBophan(null, listStartWithAllBophan, "khac", "Khác", -1);
                }
                else
                {
                    foreach (var cccidentBodyParts in listAccidentBodyParts)
                    {
                        #region
                        Mrs00440RDO rdo = new Mrs00440RDO();
                        rdo.GROUP_ID = 4;
                        rdo.GROUP_NAME = "Bộ phận bi thương - theo ICD10";
                        rdo.SERVICE_GROUP_CODE = cccidentBodyParts.ACCIDENT_BODY_PART_CODE;
                        rdo.SERVICE_GROUP_NAME = cccidentBodyParts.ACCIDENT_BODY_PART_NAME;
                        rdo.SERVICE_GROUP_ID = cccidentBodyParts.ID;
                        var listTreatment = listRdoDetail.Where(s => s.ACCIDENT_BODY_PART_ID == cccidentBodyParts.ID).ToList();
                        processCountTreatment(rdo, listTreatment);
                        listRdo.Add(rdo);
                        #endregion
                    }
                }
                #endregion
                #region//NguyenNhanBiThuongICD10
                if (castFilter.IS_CAUSE_FROM_ICD == true)
                {

                    List<string> listStartWithV01ToV99 = new List<string>();
                    for (int i = 0; i < 100; i++)
                    {
                        string startwith = string.Format("V{0:00}", Convert.ToInt64(i));
                        listStartWithV01ToV99.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithV01ToV99, null, "V01ToV99", "Tai nạn giao thông (V01-V99)", -10);

                    List<string> listStartWithW01ToW19 = new List<string>();
                    for (int i = 0; i < 20; i++)
                    {
                        string startwith = string.Format("W{0:00}", Convert.ToInt64(i));
                        listStartWithW01ToW19.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithW01ToW19, null, "W01ToW19", "Ngã (W01-W19)", -9);

                    List<string> listStartWithW20ToW49 = new List<string>();
                    for (int i = 20; i < 50; i++)
                    {
                        string startwith = string.Format("W{0:00}", Convert.ToInt64(i));
                        listStartWithW20ToW49.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithW20ToW49, null, "W20ToW49", "Tai nạn do lực cơ học bất động (W20-W49)", -8);

                    List<string> listStartWithW50ToW64 = new List<string>();
                    for (int i = 50; i < 65; i++)
                    {
                        string startwith = string.Format("W{0:00}", Convert.ToInt64(i));
                        listStartWithW50ToW64.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithW50ToW64, null, "W50ToW64", "Tai nạn do lực cơ học chuyển động: súc vật, động vật cắn, đốt, húc (W50-W64)", -7);

                    List<string> listStartWithW65ToW84 = new List<string>();
                    for (int i = 65; i < 85; i++)
                    {
                        string startwith = string.Format("W{0:00}", Convert.ToInt64(i));
                        listStartWithW65ToW84.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithW65ToW84, null, "W65ToW84", "Đuối nuớc (W65-W84)", -6);

                    List<string> listStartWithW85ToX19 = new List<string>();
                    for (int i = 85; i < 100; i++)
                    {
                        string startwith = string.Format("W{0:00}", Convert.ToInt64(i));
                        listStartWithW85ToX19.Add(startwith);
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        string startwith = string.Format("X{0:00}", Convert.ToInt64(i));
                        listStartWithW85ToX19.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithW85ToX19, null, "W85ToX19", "Bỏng (W85-W99, X00-X19)", -5);

                    List<string> listStartWithX20ToX49 = new List<string>();
                    for (int i = 20; i < 30; i++)
                    {
                        string startwith = string.Format("X{0:00}", Convert.ToInt64(i));
                        listStartWithX20ToX49.Add(startwith);
                    }
                    for (int i = 40; i < 50; i++)
                    {
                        string startwith = string.Format("X{0:00}", Convert.ToInt64(i));
                        listStartWithX20ToX49.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithX20ToX49, null, "X20ToX49", "Ngộ độc: hoá chất, thực phẩm, động vật, thực vật có độc (X20-X29, X40-X49)", -4);

                    List<string> listStartWithX60ToX84 = new List<string>();
                    for (int i = 60; i < 85; i++)
                    {
                        string startwith = string.Format("X{0:00}", Convert.ToInt64(i));
                        listStartWithX60ToX84.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithX60ToX84, null, "X65ToX84", "Tự tử (X60-X84)", -3);

                    List<string> listStartWithX85ToY09 = new List<string>();
                    for (int i = 85; i < 100; i++)
                    {
                        string startwith = string.Format("X{0:00}", Convert.ToInt64(i));
                        listStartWithX85ToY09.Add(startwith);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        string startwith = string.Format("Y{0:00}", Convert.ToInt64(i));
                        listStartWithX85ToY09.Add(startwith);
                    }
                    AddGroupNguyennhan(listStartWithX85ToY09, null, "X85ToY09", "Bạo lực, xung đột (X85-Y09)", -2);

                    List<string> listStartWithAllNguyennhan = new List<string>();
                    listStartWithAllNguyennhan.AddRange(listStartWithX85ToY09);
                    listStartWithAllNguyennhan.AddRange(listStartWithX60ToX84);
                    listStartWithAllNguyennhan.AddRange(listStartWithX20ToX49);
                    listStartWithAllNguyennhan.AddRange(listStartWithW85ToX19);
                    listStartWithAllNguyennhan.AddRange(listStartWithW65ToW84);
                    listStartWithAllNguyennhan.AddRange(listStartWithW50ToW64);
                    listStartWithAllNguyennhan.AddRange(listStartWithW20ToW49);
                    listStartWithAllNguyennhan.AddRange(listStartWithW01ToW19);
                    listStartWithAllNguyennhan.AddRange(listStartWithV01ToV99);
                    AddGroupNguyennhan(null, listStartWithAllNguyennhan, "khac", "Khác", -1);
                }
                else
                {
                    foreach (var accidentHurtTypes in listAccidentHurtTypes)
                    {
                        Mrs00440RDO rdo = new Mrs00440RDO();
                        rdo.GROUP_ID = 5;
                        rdo.GROUP_NAME = "Nguyên nhân TNTT - Theo ICD10";
                        rdo.SERVICE_GROUP_CODE = accidentHurtTypes.ACCIDENT_HURT_TYPE_CODE;
                        rdo.SERVICE_GROUP_NAME = accidentHurtTypes.ACCIDENT_HURT_TYPE_NAME;
                        rdo.SERVICE_GROUP_ID = accidentHurtTypes.ID;

                        var listTreatment = listRdoDetail.Where(s => s.ACCIDENT_HURT_TYPE_ID == accidentHurtTypes.ID).ToList();
                        processCountTreatment(rdo, listTreatment);
                        listRdo.Add(rdo);
                    }
                }
                #endregion
                #region//DieuTriBanDauSauTNTT
                foreach (var accidentCares in listAccidentCares)
                {
                    Mrs00440RDO rdo = new Mrs00440RDO();
                    rdo.GROUP_ID = 6;
                    rdo.GROUP_NAME = "Điều trị ban đầu sau TNTT";
                    rdo.SERVICE_GROUP_CODE = accidentCares.ACCIDENT_CARE_CODE;
                    rdo.SERVICE_GROUP_NAME = accidentCares.ACCIDENT_CARE_NAME;
                    rdo.SERVICE_GROUP_ID = accidentCares.ID;

                    var listTreatment = listRdoDetail.Where(s => s.ACCIDENT_CARE_ID == accidentCares.ID).ToList();
                    processCountTreatment(rdo, listTreatment);
                    listRdo.Add(rdo);
                }
                listRdo = listRdo.OrderBy(o => o.GROUP_ID).ThenBy(o => o.SERVICE_GROUP_ID).ToList();
                #endregion
                
                //var ID = 1; 
                listGroupName = listRdo.GroupBy(s => s.GROUP_ID).Select(s => new Mrs00440RDO
                {
                    //SERVICE_GROUP_ID = s.First().SERVICE_GROUP_ID,
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    TOTAL_STICKY = s.Sum(a => a.TOTAL_STICKY),
                    TOTAL_DIE = s.Sum(a => a.TOTAL_DIE),
                    TOTAL_STICKY_FEMALE = s.Sum(a => a.TOTAL_STICKY_FEMALE),
                    TOTAL_DIE_FEMALE = s.Sum(a => a.TOTAL_DIE_FEMALE),
                    TOTAL_STICKY_04 = s.Sum(a => a.TOTAL_STICKY_04),
                    TOTAL_DIE_04 = s.Sum(a => a.TOTAL_DIE_04),
                    TOTAL_STICKY_FEMALE_04 = s.Sum(a => a.TOTAL_STICKY_FEMALE_04),
                    TOTAL_DIE_FEMALE_04 = s.Sum(a => a.TOTAL_DIE_FEMALE_04),
                    TOTAL_STICKY_14 = s.Sum(a => a.TOTAL_STICKY_14),
                    TOTAL_DIE_14 = s.Sum(a => a.TOTAL_DIE_14),
                    TOTAL_STICKY_FEMALE_14 = s.Sum(a => a.TOTAL_STICKY_FEMALE_14),
                    TOTAL_DIE_FEMALE_14 = s.Sum(a => a.TOTAL_DIE_FEMALE_14),
                    TOTAL_STICKY_19 = s.Sum(a => a.TOTAL_STICKY_19),
                    TOTAL_DIE_19 = s.Sum(a => a.TOTAL_DIE_19),
                    TOTAL_STICKY_FEMALE_19 = s.Sum(a => a.TOTAL_STICKY_FEMALE_19),
                    TOTAL_DIE_FEMALE_19 = s.Sum(a => a.TOTAL_DIE_FEMALE_19),
                    TOTAL_STICKY_60 = s.Sum(a => a.TOTAL_STICKY_60),
                    TOTAL_DIE_60 = s.Sum(a => a.TOTAL_DIE_60),
                    TOTAL_STICKY_FEMALE_60 = s.Sum(a => a.TOTAL_STICKY_FEMALE_60),
                    TOTAL_DIE_FEMALE_60 = s.Sum(a => a.TOTAL_DIE_FEMALE_60),
                    TOTAL_STICKY_100 = s.Sum(a => a.TOTAL_STICKY_100),
                    TOTAL_DIE_100 = s.Sum(a => a.TOTAL_DIE_100),
                    TOTAL_STICKY_FEMALE_100 = s.Sum(a => a.TOTAL_STICKY_FEMALE_100),
                    TOTAL_DIE_FEMALE_100 = s.Sum(a => a.TOTAL_DIE_FEMALE_100)
                }).ToList();
                listGroupName = listGroupName.OrderBy(o => o.GROUP_ID).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddGroupBophan(List<string> listStartWith, List<string> listNoStartWith, string serviceGroupCode, string serviceGroupName, int serviceGroupId)
        {
            Mrs00440RDO rdo = new Mrs00440RDO();
            rdo.GROUP_ID = 4;
            rdo.GROUP_NAME = "Bộ phận bi thương - theo ICD10";
            rdo.SERVICE_GROUP_CODE = serviceGroupCode;
            rdo.SERVICE_GROUP_NAME = serviceGroupName;
            rdo.SERVICE_GROUP_ID = serviceGroupId;
            if (listStartWith != null && listStartWith.Count > 0)
            {
                processCountTreatment(rdo, listRdoDetail.Where(o => listStartWith.Exists(p => (o.ICD_CODE ?? "").StartsWith(p))).ToList());
            }
            if (listNoStartWith != null && listNoStartWith.Count > 0)
            {
                processCountTreatment(rdo, listRdoDetail.Where(o => !listNoStartWith.Exists(p => (o.ICD_CODE ?? "").StartsWith(p))).ToList());
            }
            listRdo.Add(rdo);
        }

        private void AddGroupNguyennhan(List<string> listStartWith, List<string> listNoStartWith, string serviceGroupCode, string serviceGroupName, int serviceGroupId)
        {
            Mrs00440RDO rdo = new Mrs00440RDO();
            rdo.GROUP_ID = 5;
            rdo.GROUP_NAME = "Nguyên nhân TNTT - Theo ICD10";
            rdo.SERVICE_GROUP_CODE = serviceGroupCode;
            rdo.SERVICE_GROUP_NAME = serviceGroupName;
            rdo.SERVICE_GROUP_ID = serviceGroupId;
            if (listStartWith != null && listStartWith.Count > 0)
            {
                processCountTreatment(rdo, listRdoDetail.Where(o => listStartWith.Exists(p => (o.ICD_CAUSE_CODE ?? "").StartsWith(p))).ToList());
            }
            if (listNoStartWith != null && listNoStartWith.Count > 0)
            {
                processCountTreatment(rdo, listRdoDetail.Where(o => !listNoStartWith.Exists(p => (o.ICD_CAUSE_CODE ?? "").StartsWith(p))).ToList());
            }
            listRdo.Add(rdo);
        }

        private void processCountTreatment(Mrs00440RDO rdo, List<Mrs00440RDO> listRdoDetail)
        {
            foreach (var item in listRdoDetail)
            {
                DateTime now = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.IN_TIME) ?? DateTime.Now;
                int AGE = now.Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.TDL_PATIENT_DOB) ?? DateTime.Now).Year;
                if (AGE <= 4 && (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.TDL_PATIENT_DOB) ?? DateTime.Now) > now.AddYears(-AGE)) AGE--;

                //TimeSpan
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH)
                {
                    rdo.TOTAL_STICKY += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH)
                {
                    rdo.TOTAL_DIE += 1;
                }
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    rdo.TOTAL_STICKY_FEMALE += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    rdo.TOTAL_DIE_FEMALE += 1;
                }
                ///////04
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 0 && AGE < 5)
                {
                    rdo.TOTAL_STICKY_04 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 0 && AGE < 5)
                {
                    rdo.TOTAL_DIE_04 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 0 && AGE < 5)
                {
                    rdo.TOTAL_STICKY_FEMALE_04 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 0 && AGE < 5)
                {
                    rdo.TOTAL_DIE_FEMALE_04 += 1;
                }
                ///////
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 5 && AGE < 15)
                {
                    rdo.TOTAL_STICKY_14 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 5 && AGE < 15)
                {
                    rdo.TOTAL_DIE_14 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 5 && AGE < 15)
                {
                    rdo.TOTAL_STICKY_FEMALE_14 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 5 && AGE < 15)
                {
                    rdo.TOTAL_DIE_FEMALE_14 += 1;
                }
                ///////
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 15 && AGE < 20)
                {
                    rdo.TOTAL_STICKY_19 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 15 && AGE < 20)
                {
                    rdo.TOTAL_DIE_19 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 15 && AGE < 20)
                {
                    rdo.TOTAL_STICKY_FEMALE_19 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 15 && AGE < 20)
                {
                    rdo.TOTAL_DIE_FEMALE_19 += 1;
                }
                //////
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 20 && AGE < 60)
                {
                    rdo.TOTAL_STICKY_60 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 20 && AGE < 60)
                {
                    rdo.TOTAL_DIE_60 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 20 && AGE < 60)
                {
                    rdo.TOTAL_STICKY_FEMALE_60 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 20 && AGE < 60)
                {
                    rdo.TOTAL_DIE_FEMALE_60 += 1;
                }
                ////////
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 60 && AGE < 100)
                {
                    rdo.TOTAL_STICKY_100 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && AGE > 60 && AGE < 100)
                {
                    rdo.TOTAL_DIE_100 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID != MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 60 && AGE < 100)
                {
                    rdo.TOTAL_STICKY_FEMALE_100 += 1;
                }
                if (item.TREATMENT_END_TYPE_ID == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH && item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && AGE > 60 && AGE < 100)
                {
                    rdo.TOTAL_DIE_FEMALE_100 += 1;
                }
            }
        }
    }
}
