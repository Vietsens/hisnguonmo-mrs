using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisGender;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisIcdGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;

namespace MRS.Processor.Mrs00130
{
    public class Mrs00130Processor : AbstractProcessor
    {
        List<Mrs00130RDO> _lisMrs00130RDO = new List<Mrs00130RDO>();
        Mrs00130Filter CastFilter;
        private string GROUP_ICD_NAME;
        private long? AGE_FROM;
        private long? AGE_TO;
        private string GENDER_NAME;
        List<V_HIS_TREATMENT> listTreatmentViews = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE> listPatientTypes = new List<HIS_PATIENT_TYPE>();

        public Mrs00130Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00130Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00130Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                var yearNow = DateTime.Now.Year;

                //đổi ngược thời gian fom, to
                long? dobFrom = null;
                long? dobTo = null;
                if (CastFilter.AGE_FROM != null)
                {
                    var yearFrom = yearNow - CastFilter.AGE_FROM;
                    var dayInMonth = DateTime.DaysInMonth((int)yearFrom, 12);
                    dobTo = long.Parse(string.Format("{0}12{1}235959", yearFrom, dayInMonth));
                }
                if (CastFilter.AGE_TO != null)
                {
                    var yearTo = yearNow - CastFilter.AGE_TO;
                    dobFrom = long.Parse(string.Format("{0}0101000000", yearTo));
                }
                AGE_FROM = CastFilter.AGE_FROM;
                AGE_TO = CastFilter.AGE_TO;
                ////////////////////////////////////////////////////////////////////////////////// -HIS_ICD_GROUP
                var metyFilterIcdGroup = new HisIcdGroupFilterQuery
                {
                    ID = CastFilter.ICD_GROUP_ID
                };
                var icdGroup = new MOS.MANAGER.HisIcdGroup.HisIcdGroupManager(paramGet).Get(metyFilterIcdGroup);
                GROUP_ICD_NAME = icdGroup.Select(s => s.ICD_GROUP_NAME).FirstOrDefault();
                ////////////////////////////////////////////////////////////////////////////////// -HIS_GENDER
                var metyFilterGender = new HisGenderFilterQuery
                {
                    ID = CastFilter.GENDER_ID
                };
                var gender = new MOS.MANAGER.HisGender.HisGenderManager(paramGet).Get(metyFilterGender);
                GENDER_NAME = CastFilter.GENDER_ID == null ? "Tất cả" : gender.Select(s => s.GENDER_NAME).FirstOrDefault();
                ////////////////////////////////////////////////////////////////////////////////// -HIS_ICD
                var metyFilterIcd = new HisIcdFilterQuery
                {
                    ICD_GROUP_ID = CastFilter.ICD_GROUP_ID
                };
                var listIcds = new MOS.MANAGER.HisIcd.HisIcdManager(paramGet).Get(metyFilterIcd);
                ////////////////////////////////////////////////////////////////////////////////// -V_HIS_TREATMENT
                var listIcdCodes = listIcds.Select(s => s.ICD_CODE).ToList();
                //var number1 = listIcdCodes.Count / 100;
                //var remainder1 = listIcdCodes.Count % 100;
                //if (remainder1 > 0)
                //    number1 = number1 + 1;
                //var skip1 = 0;
                //var take1 = 100;
                //for (var i = 0; i < number1; i++)
                //{
                //    var list = listIcdCodes.Skip(skip1).Take(take1).ToList();
                //    skip1 = skip1 + 100;
                //    if (listIcdCodes.Count - skip1 < 100)
                //        take1 = listIcdCodes.Count - skip1;
                //    var metyFilterTreatmnet = new HisTreatmentViewFilterQuery
                //    {
                //        ICD_CODEs = list,
                //        OUT_TIME_FROM = CastFilter.DATE_FROM,
                //        OUT_TIME_TO = CastFilter.DATE_TO,
                //    };
                //    if (CastFilter.GENDER != 0 && CastFilter.GENDER != null)
                //        //metyFilterTreatmnet.GE = CastFilter.GENDER; 
                //        if (CastFilter.AGE_FROM != 0)
                //            metyFilterTreatmnet.DOB_FROM = dobFrom;
                //    if (CastFilter.AGE_TO != 0)
                //        metyFilterTreatmnet.DOB_TO = dobTo;

                //    var listTreatmentView = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyFilterTreatmnet);
                //    listTreatmentViews.AddRange(listTreatmentView);
                //}
                var metyFilterTreatmnet = new HisTreatmentViewFilterQuery
                {
                    //ICD_CODEs = listIds,
                    OUT_TIME_FROM = CastFilter.DATE_FROM,
                    OUT_TIME_TO = CastFilter.DATE_TO,
                };
                //if (CastFilter.GENDER != 0 && CastFilter.GENDER != null)
                //    //metyFilterTreatmnet.GE = CastFilter.GENDER; 
                if (dobFrom.HasValue)
                    metyFilterTreatmnet.DOB_FROM = dobFrom;
                if (dobTo.HasValue)
                    metyFilterTreatmnet.DOB_TO = dobTo;

                var listTreatmentView = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyFilterTreatmnet);
                if (IsNotNullOrEmpty(listTreatmentView))
                {
                    listTreatmentView = listTreatmentView.Where(o => !String.IsNullOrWhiteSpace(o.ICD_CODE) && listIcdCodes.Contains(o.ICD_CODE)).ToList();
                    if (IsNotNullOrEmpty(listTreatmentView))
                        listTreatmentViews.AddRange(listTreatmentView);
                }

                if (CastFilter.GENDER_ID != 0 && CastFilter.GENDER_ID != null)
                {
                    listTreatmentViews = listTreatmentViews.Where(o => o.TDL_PATIENT_GENDER_ID == CastFilter.GENDER_ID).ToList();
                }
                if (CastFilter.BRANCH_IDs != null)
                {

                    listTreatmentViews = listTreatmentViews.Where(o => CastFilter.BRANCH_IDs.Contains(o.BRANCH_ID)).ToList();
                }

                if (CastFilter.BRANCH_ID != null)
                {

                    listTreatmentViews = listTreatmentViews.Where(o =>o.BRANCH_ID== CastFilter.BRANCH_ID).ToList();
                }

                ////////////////////////////////////////////////////////////////////////////////// -V_HIS_PATIENT_TYPE_ALTER
                var listTreatmentIds = listTreatmentViews.Select(s => s.ID).ToList();
                //var number2 = listTreatmentIds.Count / 100;
                //var remainder2 = listTreatmentIds.Count % 100;
                //if (remainder2 > 0)
                //    number2 = number2 + 1;
                //var skip2 = 0;
                //var take2 = 100;
                //for (var i = 0; i < number2; i++)
                //{
                //    var list = listTreatmentIds.Skip(skip2).Take(take2).ToList();
                //    skip2 = skip2 + 100;
                //    if (listTreatmentIds.Count - skip2 < 100)
                //        take2 = listTreatmentIds.Count - skip2;
                //    var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                //    {
                //        TREATMENT_IDs = list
                //    };
                //    var listPatientTypeAlterView = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);
                //    listPatientTypeAlter.AddRange(listPatientTypeAlterView);
                //}

                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var listPatientTypeAlterView = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);
                    listPatientTypeAlter.AddRange(listPatientTypeAlterView);
                }
                //////////////////////////////////////////////////////////////////////////////////
                var listPatientTypeIds = listPatientTypeAlter.Select(s => s.PATIENT_TYPE_ID).ToList();
                //var number3 = listPatientTypeIds.Count / 100;
                //var remainder3 = listPatientTypeIds.Count % 100;
                //if (remainder3 > 0)
                //    number3 = number3 + 1;
                //var skip3 = 0;
                //var take3 = 100;
                //for (var i = 0; i < number3; i++)
                //{
                //    var list = listPatientTypeIds.Skip(skip3).Take(take3).ToList();
                //    skip3 = skip3 + 100;
                //    if (listPatientTypeIds.Count - skip3 < 100)
                //        take3 = listPatientTypeIds.Count - skip3;
                //    var metyFilterPatientType = new HisPatientTypeFilterQuery
                //    {
                //        IDs = list
                //    };
                //    var listPatientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(paramGet).Get(metyFilterPatientType);
                //    listPatientTypes.AddRange(listPatientType);
                //}

                skip = 0;
                while (listPatientTypeIds.Count - skip > 0)
                {
                    var listIds = listPatientTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var metyFilterPatientType = new HisPatientTypeFilterQuery
                    {
                        IDs = listIds
                    };
                    var listPatientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(paramGet).Get(metyFilterPatientType);
                    listPatientTypes.AddRange(listPatientType);
                }
                //////////////////////////////////////////////////////////////////////////////////
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00130." +
                        LogUtil.TraceData(
                            LogUtil.GetMemberName(() => paramGet), paramGet));
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
                if (IsNotNullOrEmpty(listTreatmentViews))
                {
                    ProcessFilterData(listTreatmentViews, listPatientTypeAlter, listPatientTypes);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_TREATMENT> listTreatments, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters, List<HIS_PATIENT_TYPE> listPatientTypes)
        {
            Dictionary<long, HIS_ICD> dicIcd = GetIcd();
            foreach (var listTreatment in listTreatments.OrderBy(s => s.TDL_PATIENT_NAME))
            {
                var patientId = listPatientTypeAlters.Where(s => s.TREATMENT_ID == listTreatment.ID).Select(s => s.PATIENT_TYPE_ID).FirstOrDefault();
                var patientName = listPatientTypes.Where(s => s.ID == patientId).Select(s => s.PATIENT_TYPE_NAME).FirstOrDefault();
                var rdo = new Mrs00130RDO
                {
                    TREATMENT_CODE = listTreatment.TREATMENT_CODE,
                    PATIENT_NAME = listTreatment.TDL_PATIENT_NAME,
                    PATIENT_GENDER = listTreatment.TDL_PATIENT_GENDER_NAME,
                    PATIENT_AGE = Inventec.Common.DateTime.Calculation.Age(listTreatment.TDL_PATIENT_DOB),
                    PATIENT_ADRESS = listTreatment.TDL_PATIENT_ADDRESS,
                    PATIENT_TYPE = patientName,
                    ICD_NAME = listTreatment.ICD_NAME
                };
                _lisMrs00130RDO.Add(rdo);
            }
        }
        private Dictionary<long, HIS_ICD> GetIcd()
        {
            Dictionary<long, HIS_ICD> result = new Dictionary<long, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(filter);
                foreach (var item in listIcd)
                {
                    if (!result.ContainsKey(item.ID)) result[item.ID] = item;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                dicSingleTag.Add("GROUP_ICD_NAME", GROUP_ICD_NAME);
                dicSingleTag.Add("AGE_FROM", AGE_FROM);
                dicSingleTag.Add("AGE_TO", AGE_TO);
                dicSingleTag.Add("GENDER_NAME", GENDER_NAME);

                objectTag.AddObjectData(store, "Report", _lisMrs00130RDO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
