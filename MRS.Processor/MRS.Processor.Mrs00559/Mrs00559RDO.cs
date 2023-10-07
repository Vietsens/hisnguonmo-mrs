using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;


namespace MRS.Processor.Mrs00559
{
    class Mrs00559RDO : ServiceReqDO
    {

        public int COUNT_GROUP1 { get; set; }
        public int COUNT_GROUP2 { get; set; }
        public int COUNT_GROUP3 { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public int COUNT_EXAM { get; set; }
        public int COUNT_FEMALE_EXAM { get; set; }
        public int COUNT_EMERGENCY_EXAM { get; set; }
        public int COUNT_TREATMENT_EXAM { get; set; }
        public int COUNT_CHILD_EXAM { get; set; }
        public int COUNT_CHILD_LESS_FIFTEEN_AGE_EXAM { get; set; }
        public int COUNT_CHILD_LESS_FIVE_AGE_EXAM { get; set; }
        public int COUNT_CHILD_BIGGER_SIXTY_AGE_EXAM { get; set; }
        public int COUNT_IN_TREAT_EXAM { get; set; }
        public int COUNT_TRANPATI_EXAM { get; set; }
        public int COUNT_MEDI_HOME_EXAM { get; set; }
        public int COUNT_MOVE_ROOM_EXAM { get; set; }
        public int COUNT_OUT_TREAT_EXAM { get; set; }
        public long DAY_OUT_TREAT_EXAM { get; set; }
        public long COUNT_FINISH_EXAM { get; set; }
        public long COUNT_WAIT_EXAM { get; set; }
        public int COUNT_ETHENIC_EXAM { set; get; }
        public int COUNT_FOREIGNER_EXAM { set; get; }
        public Mrs00559RDO(ServiceReqDO data)
        {
            try
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<ServiceReqDO>(this,data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        internal void CountExam()
        {
            try
            {
                COUNT_EXAM++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountFemaleExam(long gender)
        {
            try
            {
                if (gender == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.COUNT_FEMALE_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountExamYhct(ServiceReqDO sr)
        {
            try
            {
                if (HisRoomCFG.ROOM_ID__YHCTs != null && HisRoomCFG.ROOM_ID__YHCTs.Contains(sr.EXECUTE_ROOM_ID))
                {
                    this.COUNT_YHCT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountEmergencyExam(short? isEmergency)
        {
            try
            {
                if (isEmergency == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    COUNT_EMERGENCY_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountTreatmentExam(long requestRoomId)
        {
            try
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o=>o.ID==requestRoomId)??new V_HIS_ROOM();
                if (room.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD)
                {
                    COUNT_TREATMENT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountChildExam(long Dob)
        {
            try
            {
                if (MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(Dob)))
                {
                    COUNT_CHILD_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
       
        public void CountAgeExam(long Dob)
        {
            try
            {
                long dtNow = long.Parse(DateTime.UtcNow.ToString("yyyyMMddhhmmss"));
                long AGE = (dtNow - Dob) / 10000000000;
                if (AGE < 5 && AGE >= 0)
                {
                    COUNT_CHILD_LESS_FIVE_AGE_EXAM++;
                }
                if (AGE < 6 && AGE >= 5)
                {
                    COUNT_CHILD_EXAM++;
                }
                if (AGE < 15 && AGE >= 6)
                {
                    COUNT_CHILD_LESS_FIFTEEN_AGE_EXAM++;
                }
                if (AGE > 60)
                {
                    COUNT_CHILD_BIGGER_SIXTY_AGE_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        public void CountPatientType(string ETHNIC_NAME) {

            try
            {
                if (ETHNIC_NAME != "Kinh" && ETHNIC_NAME!="Người nước ngoài" && ETHNIC_NAME!= null)
                {
                    COUNT_ETHENIC_EXAM++;
                }
                if (ETHNIC_NAME == "Người nước ngoài" && ETHNIC_NAME != null)
                {
                    COUNT_FOREIGNER_EXAM++;
                }
            }
            catch (Exception ex)
            {
               LogSystem.Error(ex);
            }
        }
        internal void CountMoveRoomExam( long serviceReqId,List<ServiceReqDO> listPrevHisServiceReq)
        {
            try
            {
                if (listPrevHisServiceReq.Exists(o => o.PREVIOUS_SERVICE_REQ_ID == serviceReqId))
                {
                    COUNT_MOVE_ROOM_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountInTreatExam(ServiceReqDO sr,ref Dictionary<long,DepaInAmount> dicDepaIn, List<long> PatientTypeIdBHs)
        {
            try
            {
                if (sr.CLINICAL_IN_TIME != null
                    && sr.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    COUNT_IN_TREAT_EXAM++;
                    if (dicDepaIn.ContainsKey(sr.HOSPITALIZE_DEPARTMENT_ID??0))
                    {
                        dicDepaIn[sr.HOSPITALIZE_DEPARTMENT_ID??0].AMOUNT++;
                        if(PatientTypeIdBHs.Contains(sr.TDL_PATIENT_TYPE_ID??0))
                        {
                            dicDepaIn[sr.HOSPITALIZE_DEPARTMENT_ID??0].AMOUNT_BH++;
                        }    
                        else
                        {
                            dicDepaIn[sr.HOSPITALIZE_DEPARTMENT_ID??0].AMOUNT_VP++;
                        }    
                        
                    }    
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountTranpatiExam(long executeRoomId, ServiceReqDO sr)
        {
            try
            {
                if (sr.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && sr.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN
                    && sr.END_ROOM_ID == executeRoomId)
                {
                    COUNT_TRANPATI_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountMediHomeExam(long executeRoomId, ServiceReqDO sr)
        {
            try
            {
                if (sr.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && sr.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                    && sr.END_ROOM_ID == executeRoomId)
                {
                    COUNT_MEDI_HOME_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountOutTreatExam(ServiceReqDO sr)
        {
            try
            {
                if (sr.CLINICAL_IN_TIME != null
                    && sr.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    COUNT_OUT_TREAT_EXAM++;
                    DAY_OUT_TREAT_EXAM += DateDiff.diffDate(sr.CLINICAL_IN_TIME ?? 0, sr.OUT_TIME ?? 0);
                    if (MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(sr.TDL_PATIENT_DOB)))
                    {
                        COUNT_OUT_TREAT_CHILD_EXAM++;
                    }
                    if (HisRoomCFG.ROOM_ID__YHCTs != null && HisRoomCFG.ROOM_ID__YHCTs.Contains(sr.EXECUTE_ROOM_ID))
                    {
                        this.COUNT_OUT_TREAT_YHCT_EXAM++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal void CountPatientTypeGroup(long patientTypeId, List<long> PatientTypeIdBHs)
        {
            try
            {
                if (PatientTypeIdBHs.Contains(patientTypeId))
                {
                    COUNT_GROUP1++;
                }
                else
                {
                    COUNT_GROUP2++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }





        public int COUNT_OUT_TREAT_CHILD_EXAM { get; set; }

        public int COUNT_YHCT_EXAM { get; set; }

        public int COUNT_OUT_TREAT_YHCT_EXAM { get; set; }
    }

    internal class DepaInAmount
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public int AMOUNT_BH { get; set; }
        public int AMOUNT_VP { get; set; }
        public int AMOUNT { get; set; }
    }
}


