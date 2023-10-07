using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00143
{
    public class Mrs00143Processor : AbstractProcessor
    {
        private const short IS_EMERGENCY_TRUE = 1;
        List<Mrs00143RDO> _listSereServRdo = new List<Mrs00143RDO>();
        Mrs00143Filter CastFilter;

        List<V_HIS_SERE_SERV> listSereServViews = new List<V_HIS_SERE_SERV>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterViews = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_TREATMENT> listTreatmentViews;
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCatViews;

        public Mrs00143Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00143Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00143Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter MRS00143: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                LogSystem.Info("Bat dau lay du lieu filter MRS00143 ===============================================================");
                //-------------------------------------------------------------------------------------------------- V_HIS_TREATMENT
                var metyFilterTreatment = new HisTreatmentViewFilterQuery
                {
                    OUT_TIME_FROM = CastFilter.DATE_FROM,
                    OUT_TIME_TO = CastFilter.DATE_TO
                };
                listTreatmentViews = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyFilterTreatment);
                //-------------------------------------------------------------------------------------------------- V_HIS_SERE_SERV
                var listTreatmentViewIds = listTreatmentViews.Select(s => s.ID).ToList();
                var skip = 0;
                while (listTreatmentViewIds.Count - skip > 0)
                {
                    var listIds = listTreatmentViewIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterSereServ = new HisSereServViewFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var sereServViews = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(metyFilterSereServ);
                    listSereServViews.AddRange(sereServViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_PATIENT_TYPE_ALTER
                skip = 0;
                while (listTreatmentViewIds.Count - skip > 0)
                {
                    var listIds = listTreatmentViewIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var PatientTypeAlterViews = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);
                    listPatientTypeAlterViews.AddRange(PatientTypeAlterViews);
                }


                //-------------------------------------------------------------------------------------------------- V_HIS_SERVICE_RETY_CAT
                var metyFilterServiceRetyCat = new HisServiceRetyCatViewFilterQuery
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00143"
                };
                listServiceRetyCatViews = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(metyFilterServiceRetyCat);
                //--------------------------------------------------------------------------------------------------
                LogSystem.Info("Ket thuc lay du lieu filter MRS00143 ===============================================================");
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00143." +
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
                    ProcessFilterData(listTreatmentViews, listSereServViews, listPatientTypeAlterViews, listServiceRetyCatViews);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_TREATMENT> listTreatments, List<V_HIS_SERE_SERV> listSereServs, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats)
        {
            //Tên khoảng thời gian lấy dữ liệu
            var dateTimeFrom = Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM);
            var dateTimeTo = Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO);
            var nameFilter = string.Format("Số liệu thống kê từ {0} đến {1}", dateTimeFrom, dateTimeTo);
            //Giường bệnh kế hoạch-----------------Bỏ trống
            //Giường bệnh thực kê-----------------Bỏ trống
            //Tổng số lượt khám
            var totalExam = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList();
            //Tổng số lượt khám trẻ em dưới 6 tuổi
            var listTreatmentExamIds = totalExam.Select(s => s.TDL_TREATMENT_ID).ToList();
            var yearAgeChildren = DateTime.Now.Year - AgeChildren.ReportAgeChildrenExam;
            var dateAgeChildren = long.Parse(string.Format("{0}0101000000", yearAgeChildren));
            var totalExamChildren = listTreatments.Where(s => listTreatmentExamIds.Contains(s.ID) && s.TDL_PATIENT_DOB >= dateAgeChildren).ToList();
            //Tổng số lượt khám người ngèo
            var PatientTypeAlters = listPatientTypeAlters.Where(s => listTreatmentExamIds.Contains(s.TREATMENT_ID)).ToList();
            var PatientTypeAlterIds = PatientTypeAlters.Select(s => s.ID).ToList();
            var totalPatientKindPoor = PatientTypeAlters.Where(s => s.HEIN_CARD_NUMBER != null && s.HEIN_CARD_NUMBER.Contains(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__NN)).ToList();
            //Tổng số lượt khám cận ngèo
            var totalPatientPoorAccess = PatientTypeAlters.Where(s => s.HEIN_CARD_NUMBER != null && s.HEIN_CARD_NUMBER.Contains(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__CN)).ToList();
            //Tổng số lượt khám y học cổ truyền
            var listServiceIdYhct = listServiceRetyCats.Where(s => s.REPORT_TYPE_CAT_ID == HisServiceRetyCatCFG.CATEGORY_ID_MRS00143_YHCT).Select(s => s.SERVICE_ID).ToList();
            var totalServiceYhct = listSereServs.Where(s => listServiceIdYhct.Contains(s.SERVICE_ID)).ToList();
            //Bệnh nhân điều trị ngoại trú (lấy loại điều trị cuối cùng của bệnh nhân)
            var patientOutpatientTreatment = listPatientTypeAlters.Where(s => s.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
            var listTreatmentIdFromPatientOutpatientTreatment = patientOutpatientTreatment.Select(s => s.TREATMENT_ID).ToList();
            var totalTreatmentTypeForOuttreatments = listPatientTypeAlters.Where(s => listTreatmentIdFromPatientOutpatientTreatment.Contains(s.TREATMENT_ID)).GroupBy(s => s.TREATMENT_ID).ToList();
            var totalPatientOutpatientTreatment = 0;
            foreach (var totalTreatmentTypeForTreatment in totalTreatmentTypeForOuttreatments)
            {
                var lastTreatmentType = totalTreatmentTypeForTreatment.OrderByDescending(s => s.LOG_TIME).FirstOrDefault();
                if (lastTreatmentType.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    totalPatientOutpatientTreatment = totalPatientOutpatientTreatment + 1;
            }
            //Tổng số điều trị nội trú (lấy loại điều trị cuối cùng của bệnh nhân)
            var patientInpatientTreatments = listPatientTypeAlters.Where(s => s.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
            var listTreatmentIdFromPatientInpatientTreatment = patientInpatientTreatments.Select(s => s.TREATMENT_ID).ToList();
            var totalTreatmentTypeForIntreatments = listPatientTypeAlters.Where(s => listTreatmentIdFromPatientInpatientTreatment.Contains(s.TREATMENT_ID)).GroupBy(s => s.TREATMENT_ID).ToList();
            var totalPatientInpatientTreatments = 0;
            foreach (var totalTreatmentTypeForIntreatment in totalTreatmentTypeForIntreatments)
            {
                var lastTreatmentType = totalTreatmentTypeForIntreatment.OrderByDescending(s => s.LOG_TIME).FirstOrDefault();
                if (lastTreatmentType.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    totalPatientInpatientTreatments = totalPatientInpatientTreatments + 1;
            }
            //Tổng số điều trị nội trú là người nghèo
            var PatientTypeAlterIds2 = patientInpatientTreatments.Select(s => s.ID).ToList();
            var totalPatientInpatientTreatmentKindPoor = patientInpatientTreatments.Where(s => s.HEIN_CARD_NUMBER != null && s.HEIN_CARD_NUMBER.Contains(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__NN)).ToList();
            //Tổng số điều trị nội trú là người cận nghèo
            var totalPatientInpatientTreatmentAccessExam = patientInpatientTreatments.Where(s => s.HEIN_CARD_NUMBER != null && s.HEIN_CARD_NUMBER.Contains(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__CN)).ToList();
            //Tổng số điều trị nội trú y học cổ truyền-----------------Bỏ trống
            //Tổng số ngày điều trị nội trú
            var listPatientInpatientTreatments = listTreatments.Where(s => patientInpatientTreatments.Select(ss => ss.TREATMENT_ID).Contains(s.ID)).ToList();
            var totalDateInpatientTreatment = 0;
            foreach (var listPatientInpatientTreatment in listPatientInpatientTreatments)
            {
                var differenceDate = Inventec.Common.DateTime.Calculation.DifferenceDate(listPatientInpatientTreatment.IN_TIME, listPatientInpatientTreatment.OUT_TIME.Value);
                if (differenceDate > 0)
                    totalDateInpatientTreatment = totalDateInpatientTreatment + (differenceDate + 1);
                else
                    totalDateInpatientTreatment = totalDateInpatientTreatment + 1;
            }
            //Ngày điều trị trung bình = tổng số ngày điều trị của bn nội trú / số bệnh nhân nội trú
            decimal timeAverageTreatmentRounding = 0;
            if (patientInpatientTreatments.Count != 0)
                timeAverageTreatmentRounding = (decimal)totalDateInpatientTreatment / (decimal)patientInpatientTreatments.Count;
            //Công xuất sử dụng giường bệnh (% tính theo GB KH)-----------------Bỏ trống
            //Tỉ lệ bệnh nhân chuyển viện (lấy số bn kết thúc là chuyển viện/ tổng số hồ sơ trong khoảng thời gian filter)
            var lisPatientHospitalTransferred = listTreatments.Where(s => HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__CV.Contains(s.TREATMENT_END_TYPE_ID.Value)).ToList();
            decimal ratioTransferHospital = 0;
            var totalEmergencyPatient = 0;
            var totalDeath = 0;
            if (listTreatments.Count != 0)
            {
                ratioTransferHospital = (decimal)lisPatientHospitalTransferred.Count / (decimal)listTreatments.Count;
                //Số bệnh nhân ngộ độc-----------------Bỏ trống
                //Số bệnh nhân cấp cứu
                totalEmergencyPatient = listTreatments.Where(o => o.IS_EMERGENCY != null && o.IS_EMERGENCY == IS_EMERGENCY_TRUE).ToList().Count;
                //Số tai nạn giao thông-----------------Bỏ trống
                //Tử vong do tai nạn giao thông-----------------Bỏ trống
                //Tổng số tử vọng tại bệnh viện
                totalDeath = listTreatments.Where(o => o.TREATMENT_END_TYPE_ID != null && o.TREATMENT_END_TYPE_ID == HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH).ToList().Count;
            }
            //Số lần phẩu thuật
            var totalPatientSurgery = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList();
            //Số lần nội soi
            var totalEndoscopic = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).ToList();
            //Số lần Xquang
            var listServiceIdXq = listServiceRetyCats.Where(s => s.REPORT_TYPE_CAT_ID == HisServiceRetyCatCFG.CATEGORY_ID_MRS00143_XQ).Select(s => s.SERVICE_ID).ToList();
            var totalServiceXq = listSereServs.Where(s => listServiceIdXq.Contains(s.SERVICE_ID)).ToList();
            //Số lần chụp CT
            var listServiceIdCt = listServiceRetyCats.Where(s => s.REPORT_TYPE_CAT_ID == HisServiceRetyCatCFG.CATEGORY_ID_MRS00143_CT).Select(s => s.SERVICE_ID).ToList();
            var totalServiceCt = listSereServs.Where(s => listServiceIdCt.Contains(s.SERVICE_ID)).ToList();
            //Số lần siêu âm
            var totalServiceSuim = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).ToList();
            //Số lần điện tim
            var listServiceIdDt = listServiceRetyCats.Where(s => s.REPORT_TYPE_CAT_ID == HisServiceRetyCatCFG.CATEGORY_ID_MRS00143_DT).Select(s => s.SERVICE_ID).ToList();
            var totalServiceDt = listSereServs.Where(s => listServiceIdDt.Contains(s.SERVICE_ID)).ToList();
            //Số lần xét nghiệm
            var totalServiceTest = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).ToList();
            var rdo = new Mrs00143RDO
            {
                NAME_FILTER = nameFilter,
                TOTAL_EXAM = totalExam.Count > 0 ? (long?)totalExam.Count : null,
                TOTAL_EXAM_CHILDREN = totalExamChildren.Count > 0 ? (long?)totalExamChildren.Count : null,
                TOTAL_PATIENT_KIND_POOR_EXAM = totalPatientKindPoor.Count > 0 ? (long?)totalPatientKindPoor.Count : null,
                TOTAL_SERVICE_YHCT = totalServiceYhct.Count > 0 ? (long?)totalServiceYhct.Count : null,
                TOTAL_PATIENT_POOR_ACCESS_EXAM = totalPatientPoorAccess.Count > 0 ? (long?)totalPatientPoorAccess.Count : null,
                TOTAL_PATIENT_OUTPATIENT_TREATMENT_EXAM = totalPatientOutpatientTreatment > 0 ? (long?)totalPatientOutpatientTreatment : null,
                TOTAL_PATIENT_INPATIENT_TREATMENT_TREAT = totalPatientInpatientTreatments > 0 ? (long?)totalPatientInpatientTreatments : null,
                TOTAL_PATIENT_INPATIENT_TREATMENT_KIND_POOR = totalPatientInpatientTreatmentKindPoor.Count > 0 ? (long?)totalPatientInpatientTreatmentKindPoor.Count : null,
                TOTAL_PATIENT_INPATIENT_TREATMENT_ACCESS_EXAM = totalPatientInpatientTreatmentAccessExam.Count > 0 ? (long?)totalPatientInpatientTreatmentAccessExam.Count : null,
                TIME_AVERAGE_TREATMENT = timeAverageTreatmentRounding > 0 ? (decimal?)timeAverageTreatmentRounding : null,
                TOTAL_TIME_AVERAGE_INPATIENT_TREATMENT = totalDateInpatientTreatment > 0 ? (long?)totalDateInpatientTreatment : null,
                RATIO_TRANSFER_HOSPITAL = ratioTransferHospital > 0 ? (decimal?)ratioTransferHospital : null,
                TOTAL_EMERGENCY_PATIENT = totalEmergencyPatient > 0 ? (long?)totalEmergencyPatient : null,
                TOTAL_DEATH = totalDeath > 0 ? (long?)totalDeath : null,
                TOTAL_PATIENT_SURGERY = totalPatientSurgery.Count > 0 ? (long?)totalPatientSurgery.Count : null,
                TOTAL_PATIENT_ENDOSCOPIC = totalEndoscopic.Count > 0 ? (long?)totalEndoscopic.Count : null,
                TOTAL_SERVICE_XQ = totalServiceXq.Count > 0 ? (long?)totalServiceXq.Count : null,
                TOTAL_SERVICE_CT = totalServiceCt.Count > 0 ? (long?)totalServiceCt.Count : null,
                TOTAL_SERVICE_SUIM = totalServiceSuim.Count > 0 ? (long?)totalServiceSuim.Count : null,
                TOTAL_SERVICE_DT = totalServiceDt.Count > 0 ? (long?)totalServiceDt.Count : null,
                TOTAL_SERVICE_TEST = totalServiceTest.Count > 0 ? (long?)totalServiceTest.Count : null,
            };
            _listSereServRdo.Add(rdo);
        }

        private class AgeChildren
        {
            private static string MRS_AGE_CHILDREN_EXAM = "MRS_AGE_CHILDREN"; //Độ tuổi trẻ em

            private static long? reportAgeChildrenExam;
            public static long? ReportAgeChildrenExam
            {
                get
                {
                    if (reportAgeChildrenExam == null)
                    {
                        reportAgeChildrenExam = long.Parse(GetListValue(MRS_AGE_CHILDREN_EXAM).FirstOrDefault());
                    }
                    return reportAgeChildrenExam;
                }
            }

            private static List<string> GetListValue(string code)
            {
                var result = new List<string>();
                try
                {
                    var config = MRS.MANAGER.Config.Loader.dictionaryConfig[code];
                    if (config == null) throw new ArgumentNullException(code);
                    var value = string.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    var arr = value.Split(',');
                    result.AddRange(arr);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
                return result;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                objectTag.AddObjectData(store, "Report", _listSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
