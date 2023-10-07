using MRS.Processor.Mrs00534;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Repository;

namespace MRS.Proccessor.Mrs00534
{
    public class Mrs00534RDO : V_HIS_TREATMENT_4
    {
        //Tổng số bệnh nhân vào khám
        public Decimal COUNT_EXAM { get; set; }
        //Tổng số nữ KH
        public Decimal COUNT_EXAM_FEMALE { get; set; }
        //Tổng số BHYT KH
        public Decimal COUNT_EXAM_BHYT { get; set; }
        //Tổng số sức khỏe KH
        public Decimal COUNT_EXAM_KSK { get; set; }
        //Tổng số dịch vụ KH
        public Decimal COUNT_EXAM_DV { get; set; }
        //Tổng số YHCT KH
        public Decimal COUNT_EXAM_YHCT { get; set; }
        //Tổng số Trẻ em dưới 15 tuổi KH
        public Decimal COUNT_EXAM_TE { get; set; }

        ////Tổng số bệnh nhân vào điều trị
        public Decimal COUNT_TREAT { get; set; }
        //Tổng số nữ DT
        public Decimal COUNT_TREAT_FEMALE { get; set; }
        //Tổng số BHYT DT
        public Decimal COUNT_TREAT_BHYT { get; set; }
        //Tổng số YHCT DT
        public Decimal COUNT_TREAT_YHCT { get; set; }
        //Tổng số Trẻ em dưới 15 tuổi DT
        public Decimal COUNT_TREAT_TE { get; set; }
        //Tổng số Trẻ em dưới 15 tuổi DT
        public Decimal COUNT_TREAT_DAY { get; set; }

        public string BRANCH_NAME { get; set; }

        private List<HIS_SERVICE_REQ> listHisServiceReqSub;
        private List<V_HIS_BED_ROOM> listHisBedRoomSub;

        public Mrs00534RDO(V_HIS_TREATMENT_4 r, List<HIS_SERVICE_REQ> listHisServiceReq, List<HIS_TREATMENT_BED_ROOM> listHisTreatmentBedRoom, List<V_HIS_BED_ROOM> listHisBedRoom)
        {

            this.listHisServiceReqSub = listHisServiceReq.Where(o => o.TREATMENT_ID == r.ID).ToList();
            var listHisTreatmentBedRoomSub = listHisTreatmentBedRoom.Where(o => o.TREATMENT_ID == r.ID).ToList();
            this.listHisBedRoomSub = listHisBedRoom.Where(o => listHisTreatmentBedRoomSub.Exists(p => p.BED_ROOM_ID == o.ID)).ToList();

            System.Reflection.PropertyInfo[] pi = Properties.Get<MOS.EFMODEL.DataModels.V_HIS_TREATMENT_4>();

            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
            this.BRANCH_NAME = (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == r.BRANCH_ID) ?? new HIS_BRANCH()).BRANCH_NAME;
            IdentityfierExamInfo(r);
            IdentityfierTreatInfo(r);
            this.listHisServiceReqSub.Clear();
            this.listHisBedRoomSub.Clear();
        }

        private void IdentityfierTreatInfo(V_HIS_TREATMENT_4 r)
        {
            try
            {
                if (r.CLINICAL_IN_TIME != null && r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.COUNT_TREAT = 1;
                    if (r.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.COUNT_TREAT_BHYT = 1;
                    }
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_TREAT_FEMALE = 1;
                    }
                    if (Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) / 10000000000 - r.TDL_PATIENT_DOB / 10000000000 < 15)
                    {
                        this.COUNT_TREAT_TE = 1;
                    }
                    if (this.listHisBedRoomSub.Exists(o => HisDepartmentCFG.HIS_DEPARTMENT_ID__YHCT.Contains(o.DEPARTMENT_ID)))
                    {
                        this.COUNT_TREAT_YHCT = 1;
                    }
                    if (r.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.COUNT_TREAT_DAY = DateDiff.diffDate(r.CLINICAL_IN_TIME ?? 0, OUT_TIME ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void IdentityfierExamInfo(V_HIS_TREATMENT_4 r)
        {
            try
            {
                if (listHisServiceReqSub.Count > 0)
                {
                    this.COUNT_EXAM = 1;
                    if (r.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.COUNT_EXAM_BHYT = 1;
                    }
                    else if (r.TDL_KSK_CONTRACT_ID.HasValue)
                    {
                        this.COUNT_EXAM_KSK = 1;
                    }
                    else
                    {
                        this.COUNT_EXAM_DV = 1;
                    }
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_EXAM_FEMALE = 1;
                    }
                    if (Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) / 10000000000 - r.TDL_PATIENT_DOB / 10000000000 < 15)
                    {
                        this.COUNT_EXAM_TE = 1;
                    }
                    if (this.listHisServiceReqSub.Exists(o => HisRoomCFG.ROOM_ID__YHCTs != null && HisRoomCFG.ROOM_ID__YHCTs.Contains(o.EXECUTE_ROOM_ID)))
                    {
                        this.COUNT_EXAM_YHCT = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00534RDO()
        {

        }
    }
}
