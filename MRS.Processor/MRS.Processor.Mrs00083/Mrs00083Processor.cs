using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00083
{
    public class Mrs00083Processor : AbstractProcessor
    {
        Mrs00083Filter castFilter = null;
        List<Mrs00083RDO> ListRdo = new List<Mrs00083RDO>();

        CommonParam paramGet = new CommonParam();
        public Mrs00083Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00083Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = (Mrs00083Filter)this.reportFilter;

                ListRdo = new Mrs00083RDOManager().GetRdo(castFilter);
                result = true;

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
            bool result = false;
            try
            {
                ProcessOneSereServFromList();

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        private void ProcessOneSereServFromList()
        {
            try
            {

                foreach (var rdo in ListRdo)
                {
                    rdo.EXECUTE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.TDL_INTRUCTION_TIME ?? 0);
                    rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.TDL_INTRUCTION_TIME ?? 0);
                    rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.finish_Time ?? 0);
                    rdo.START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.TDL_START_TIME ?? 0);
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    var patientType2 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.PATIENT_TYPE_CODE_1 = patientType2.PATIENT_TYPE_CODE;
                    rdo.PATIENT_TYPE_NAME_1 = patientType2.PATIENT_TYPE_NAME;
                    if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.HEIN_CARD_NUMBER = rdo.HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                    }
                    else if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_01 = "VP";
                    else
                    {
                        rdo.HEIN_CARD_NUMBER = rdo.HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "XHH";
                    }
                    rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => rdo.REQUEST_ROOM_ID == o.ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.FUEX_RESULT = rdo.CONCLUDE;
                    rdo.ICD_FUEX = rdo.ICD_NAME + "; " + rdo.ICD_TEXT;
                    CalcuatorAge(rdo);
                    IsBhyt(rdo);

                    //thêm thông tin khoa phòng chỉ định thực hiện
                    var eroom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.EXECUTE_ROOM_ID);
                    if (eroom != null)
                    {
                        rdo.EXECUTE_ROOM_NAME = eroom.ROOM_NAME;
                        rdo.EXECUTE_DEPARTMENT_NAME = eroom.DEPARTMENT_NAME;
                    }
                    var rroom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.REQUEST_ROOM_ID);
                    if (rroom != null)
                    {
                        rdo.REQUEST_ROOM_NAME = rroom.ROOM_NAME;
                        rdo.REQUEST_DEPARTMENT_NAME = rroom.DEPARTMENT_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void CalcuatorAge(Mrs00083RDO rdo)
        {
            try
            {
                if (rdo != null)
                {
                    int? tuoi = RDOCommon.CalculateAge(rdo.TDL_PATIENT_DOB ?? 0);
                    if (tuoi >= 0)
                    {
                        if (rdo.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                            rdo.MALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB ?? 0);
                        }
                        else
                        {
                            rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                            rdo.FEMALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB ?? 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private void IsBhyt(Mrs00083RDO rdo)
        {
            try
            {
                if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00083Filter)this.reportFilter).TIME_FROM ?? 0)); // + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00083Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00083Filter)this.reportFilter).TIME_TO ?? 0)); // + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00083Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
            if (((Mrs00083Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00083Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            if (((Mrs00083Filter)this.reportFilter).EXECUTE_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => ((Mrs00083Filter)this.reportFilter).EXECUTE_ROOM_ID == o.ID);
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
            }
            if (((Mrs00083Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00083Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TDL_INTRUCTION_TIME).ToList());

                     
            if (castFilter.PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
            if (castFilter.TDL_PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("TDL_PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
        }

    }
}
