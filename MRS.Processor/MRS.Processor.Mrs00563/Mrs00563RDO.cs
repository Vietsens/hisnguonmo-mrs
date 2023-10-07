using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00563
{
    public class Mrs00563RDO
    {
        public long COUNT_EXAM { get; set; }	
        public long COUNT_EXAM_FEMALE { get; set; }
        public long COUNT_EXAM_BHYT { get; set; }
        public long COUNT_EXAM_YHCT { get; set; }
        public long COUNT_EXAM_LESS15 { get; set; }
        public long COUNT_IN_TREAT { get; set; }	
        public long COUNT_IN_TREAT_FEMALE { get; set; }	
        public long COUNT_IN_TREAT_BHYT { get; set; }	
        public long COUNT_IN_TREAT_YHCT { get; set; }	
        public long COUNT_IN_TREAT_LESS15 { get; set; }	
        public long COUNT_IN_TREAT_DAY { get; set; }

        public long COUNT_DEATH { get; set; }

        public long COUNT_DEATH_LESS1 { get; set; }
        public long COUNT_DEATH_LESS1_FEMALE { get; set; }
        public long COUNT_DEATH_LESS1_ETHNIC { get; set; }

        public long COUNT_DEATH_LESS5 { get; set; }
        public long COUNT_DEATH_LESS5_FEMALE { get; set; }
        public long COUNT_DEATH_LESS5_ETHNIC { get; set; }

        public long COUNT_TEST { get; set; }
        public long COUNT_DIIM { get; set; }
        public long COUNT_SUIM { get; set; }
        public long COUNT_CT { get; set; }

        private List<V_HIS_SERE_SERV> listHisSereServSub = null;
        Mrs00563Filter filter = null;


        public Mrs00563RDO() { }
        public Mrs00563RDO(HIS_TREATMENT r, List<V_HIS_SERE_SERV> listHisSereServ, List<HIS_PATIENT> listPatientEthnic,  List<long> DepartmentIdYhct,Mrs00563Filter filter)
        {
            try
            {
                this.listHisSereServSub = listHisSereServ.Where(o => o.TDL_TREATMENT_ID == r.ID).ToList();
                this.filter = filter;

                if (listPatientEthnic.Exists(o => o.ID == r.PATIENT_ID))
                {
                    this.COUNT_ETHNIC = 1;
                    this.ETHNIC_CODE = listPatientEthnic.FirstOrDefault(o => o.ID == r.PATIENT_ID).ETHNIC_CODE;
                }
                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    this.COUNT_EXAM = 1;
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_EXAM_FEMALE = 1;
                    }
                    if (!String.IsNullOrEmpty(r.TDL_HEIN_CARD_NUMBER))
                    {
                        this.COUNT_EXAM_BHYT = 1;
                    }

                    if (listPatientEthnic.Exists(o => o.ID == r.PATIENT_ID))
                    {
                        this.COUNT_EXAM_ETHNIC = 1;
                    }
                    if (IsYHCT(listHisSereServSub, filter) || IsYHCT(r, DepartmentIdYhct))
                    {
                        this.COUNT_EXAM_YHCT= 1;
                    }
                    if (Inventec.Common.DateTime.Calculation.Age(r.TDL_PATIENT_DOB) < 15)
                    {
                        this.COUNT_EXAM_LESS15 = 1;
                    }
                }


                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    if (IsYHCT(r, DepartmentIdYhct))
                    {
                        this.COUNT_OUT_TREAT_YHCT = 1;
                    }
                }
                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.COUNT_IN_TREAT = 1;
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_IN_TREAT_FEMALE = 1;
                    }
                    if (!String.IsNullOrEmpty(r.TDL_HEIN_CARD_NUMBER))
                    {
                        this.COUNT_IN_TREAT_BHYT = 1;
                    }

                    if (IsYHCT(listHisSereServSub, filter) || IsYHCT(r, DepartmentIdYhct))
                    {
                        this.COUNT_IN_TREAT_YHCT = 1;
                    }
                    if (Inventec.Common.DateTime.Calculation.Age(r.TDL_PATIENT_DOB) < 15)
                    {
                        this.COUNT_IN_TREAT_LESS15 = 1;
                    }
                    if (r.CLINICAL_IN_TIME.HasValue && r.OUT_TIME.HasValue)
                    {
                        this.COUNT_IN_TREAT_DAY = CountInTreatDay(r.CLINICAL_IN_TIME.Value, r.OUT_TIME.Value);
                    }

                    if (listPatientEthnic.Exists(o => o.ID == r.PATIENT_ID))
                    {
                        this.COUNT_IN_ETHNIC = 1;
                    }
                }
                if (r.TREATMENT_END_TYPE_ID ==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET &&r.DEATH_TIME > 0&&r.DEATH_WITHIN_ID>0&&r.DEATH_CAUSE_ID>0)
                {
                    this.COUNT_DEATH = 1;
                    if (Inventec.Common.DateTime.Calculation.Age(r.TDL_PATIENT_DOB) < 5)
                    {
                        this.COUNT_DEATH_LESS5 = 1;
                        if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            this.COUNT_DEATH_LESS5_FEMALE = 1;
                        }
                        if (listPatientEthnic.Exists(o=>o.ID==r.PATIENT_ID))
                        {
                            this.COUNT_DEATH_LESS5_ETHNIC = 1;
                        }
                        if (Inventec.Common.DateTime.Calculation.Age(r.TDL_PATIENT_DOB) < 1)
                        {
                            this.COUNT_DEATH_LESS1 = 1;
                            if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                this.COUNT_DEATH_LESS1_FEMALE = 1;
                            }
                            if (listPatientEthnic.Exists(o => o.ID == r.PATIENT_ID))
                            {
                                this.COUNT_DEATH_LESS1_ETHNIC = 1;
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

        private bool IsYHCT(HIS_TREATMENT r, List<long> DepartmentIdYhct)
        {
            bool result = false;
            try
            {
                if (DepartmentIdYhct != null && r.LAST_DEPARTMENT_ID!=null)
                {
                    result = DepartmentIdYhct.Contains(r.LAST_DEPARTMENT_ID ?? 0);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            return result;
        }

        

        private bool IsYHCT(List<V_HIS_SERE_SERV> listHisSereServSub, Mrs00563Filter filter)
        {
            bool result = false;
            try
            {
                if (filter.ROOM_CODE_KHAMYHCTs != null)
                {
                    var HisRoomSub = HisRoomCFG.HisRooms.Where(o => filter.ROOM_CODE_KHAMYHCTs.Contains(o.ROOM_CODE)).ToList();
                    result = listHisSereServSub.Exists(o => HisRoomSub.Exists(p => p.ID == o.TDL_EXECUTE_ROOM_ID));
                }
                if (filter.SERVICE_CODE_KHAMYHCTs != null)
                {
                    result =result|| listHisSereServSub.Exists(o => filter.SERVICE_CODE_KHAMYHCTs.Contains(o.TDL_SERVICE_CODE));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            return result;
        }

        private long CountInTreatDay(long intime, long outtime)
        {
            long result = 0;
            try
            {
                if (outtime < intime) return 0;
                DateTime InTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(intime);
                DateTime OutTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(outtime);
                TimeSpan sp = OutTime - InTime;
                var days = sp.TotalDays;
                    result = (long)days + 1;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }

        public int COUNT_ETHNIC { get; set; }

        public int COUNT_EXAM_ETHNIC { get; set; }

        public int COUNT_IN_ETHNIC { get; set; }

        public string ETHNIC_CODE { get; set; }

        public Dictionary<string,int> DIC_ETHNIC { get; set; }

        public long COUNT_OUT_TREAT_YHCT { get; set; }
    }
    public class LastDepartment
    {
        public long TREATMENT_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
    }
}
