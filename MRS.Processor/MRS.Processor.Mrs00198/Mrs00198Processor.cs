using MOS.MANAGER.HisTreatmentEndType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatient;

namespace MRS.Processor.Mrs00198
{
    internal class Mrs00198Processor : AbstractProcessor
    {
        List<VSarReportMrs00198RDO> _listSarReportMrs00198Rdos = new List<VSarReportMrs00198RDO>();
        List<HIS_PATIENT> listHisPatient = new List<HIS_PATIENT>();
        Mrs00198Filter CastFilter;
        string thisReportTypeCode = "";
        public Mrs00198Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        

        public override Type FilterType()
        {
            return typeof(Mrs00198Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00198Filter)this.reportFilter;
                //-------------------------------------------------------------------------------------------------- V_HIS_APPOINTMENT
                var treatmentApointmentFilter = new HisTreatmentViewFilterQuery
                {
                    APPOINTMENT_TIME_FROM = CastFilter.DATE_FROM,
                    APPOINTMENT_TIME_TO = CastFilter.DATE_TO,
                    TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN
                };
                var listTreatmentAppointments = new ManagerSql().GetTreatment(CastFilter);
                if (this.CastFilter.BRANCH_ID != null)
                {
                    listTreatmentAppointments = listTreatmentAppointments.Where(o => o.BRANCH_ID == this.CastFilter.BRANCH_ID).ToList();
                }
                var patientIds = listTreatmentAppointments.Select(o => o.PATIENT_ID).Distinct().ToList();

                if (patientIds != null && patientIds.Count > 0)
                {

                    var skip = 0;
                    while (patientIds.Count - skip > 0)
                    {
                        var Ids = patientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery HisPatientfilter = new HisPatientFilterQuery();
                        HisPatientfilter.IDs = Ids;
                        HisPatientfilter.ORDER_DIRECTION = "ID";
                        HisPatientfilter.ORDER_FIELD = "ASC";
                        var listHisPatientSub = new HisPatientManager(param).Get(HisPatientfilter);
                        if (listHisPatientSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientSub GetView null");
                        else
                            listHisPatient.AddRange(listHisPatientSub);
                    }

                }

                ProcessFilterData(listTreatmentAppointments);

                result = true;

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private Dictionary<string, HIS_ICD> GetIcd()
        {
            Dictionary<string, HIS_ICD> result = new Dictionary<string, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new HisIcdManager(param).Get(filter);
                foreach (var item in listIcd)
                {
                    if (String.IsNullOrEmpty(item.ICD_CODE)) continue;
                    if (!result.ContainsKey(item.ICD_CODE)) result[item.ICD_CODE] = item;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        protected override bool ProcessData()
        { return true; }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00198Rdos);
        }
        
        private void ProcessFilterData(List<V_HIS_TREATMENT> listTreatmentApointments)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00198 ===============================================================");
                Dictionary<string, HIS_ICD> dicIcd = GetIcd();
                foreach (var treatmentAppointment in listTreatmentApointments)
                {
                    var genderNameMen = string.Empty;
                    var genderNameWoment = string.Empty;
                    if (treatmentAppointment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        genderNameMen = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatmentAppointment.TDL_PATIENT_DOB);
                    if (treatmentAppointment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        genderNameWoment = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatmentAppointment.TDL_PATIENT_DOB);
                    //Tính số ngày chờ khám
                    var outTimeTreatmentToSystem = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatmentAppointment.OUT_TIME??0)??new DateTime();
                    var appointmentTimeToSystem = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatmentAppointment.APPOINTMENT_TIME.Value).Value;
                    var outTimeSystem = new DateTime(outTimeTreatmentToSystem.Year, outTimeTreatmentToSystem.Month, outTimeTreatmentToSystem.Day, 00, 00, 00);
                    var appointmentTimeSystem = new DateTime(appointmentTimeToSystem.Year, appointmentTimeToSystem.Month, appointmentTimeToSystem.Day, 00, 00, 00);
                    var totalDateStandby = (appointmentTimeSystem - outTimeSystem).Days.ToString();
                    var patient = listHisPatient.FirstOrDefault(o => o.ID == treatmentAppointment.PATIENT_ID);
                    var end_department_name = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatmentAppointment.END_DEPARTMENT_ID);
                    var end_room_name = MRS.MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatmentAppointment.END_ROOM_ID);
                    var rdo = new VSarReportMrs00198RDO
                   {
                       DOCTOR_USERNAME = treatmentAppointment.DOCTOR_USERNAME,
                       PATIENT_CODE = treatmentAppointment.TDL_PATIENT_CODE,
                       PATIENT_NAME = treatmentAppointment.TDL_PATIENT_NAME,
                       PATIENT_UNSIGNED_NAME = treatmentAppointment.TDL_PATIENT_UNSIGNED_NAME,
                       GENDER_NAME_MALE = genderNameMen,
                        GENDER_NAME_FEMALE = genderNameWoment,
                        VIR_ADRESS = treatmentAppointment.TDL_PATIENT_ADDRESS,
                        ICD_CODE = treatmentAppointment.ICD_CODE,
                        ICD_NAME = treatmentAppointment.ICD_NAME!=""?treatmentAppointment.ICD_NAME :( (dicIcd.ContainsKey(treatmentAppointment.ICD_CODE) ? dicIcd[treatmentAppointment.ICD_CODE].ICD_NAME : "")),
                        ICD_TEXT = treatmentAppointment.ICD_TEXT,
                        ICD_NAME_MAIN_EXAMINATION = treatmentAppointment.ICD_NAME,
                        TIME_EXAMINATION_FIRST = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatmentAppointment.OUT_TIME ?? 0),
                        TOTAL_DATE_STANDBY = totalDateStandby,
                        APPOINTMENT_TIME = treatmentAppointment.APPOINTMENT_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatmentAppointment.APPOINTMENT_TIME.Value) : null,
                        PHONE_NUMBER = patient != null ? patient.PHONE : "",
                        END_DEPARTMENT_NAME = end_department_name != null ? end_department_name.DEPARTMENT_NAME : "",
                        END_ROOM_NAME = end_room_name != null ? end_room_name.ROOM_NAME : "",
                        APPOINTMENT_ROOM_NAME = string.Join(" - ", HisRoomCFG.HisRooms.Where(o => treatmentAppointment.APPOINTMENT_EXAM_ROOM_IDS != null && string.Format(",{0},", treatmentAppointment.APPOINTMENT_EXAM_ROOM_IDS).Contains(string.Format(",{0},", o.ID))).Select(p => p.ROOM_NAME).ToList()),
                        APPOINTMENT_DEPARTMENT_NAME = string.Join(" - ", HisRoomCFG.HisRooms.Where(o => treatmentAppointment.APPOINTMENT_EXAM_ROOM_IDS != null && string.Format(",{0},", treatmentAppointment.APPOINTMENT_EXAM_ROOM_IDS).Contains(string.Format(",{0},", o.ID))).Select(p => p.DEPARTMENT_NAME).Distinct().ToList()),
                        IN_TIME = treatmentAppointment.IN_TIME,
                        CLINICAL_IN_TIME = treatmentAppointment.CLINICAL_IN_TIME,
                        OUT_TIME = treatmentAppointment.OUT_TIME
                    };
                    _listSarReportMrs00198Rdos.Add(rdo);
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00198 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }
    }
}
