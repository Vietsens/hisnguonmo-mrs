using MRS.Processor.Mrs00519;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00519
{
    public class Mrs00519RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public decimal COUNT_OLD { get; set; }

        public decimal COUNT_NEW { get; set; }
        public decimal COUNT_NEW_TE { get; set; }
        public decimal COUNT_NEW_EMERGENCY { get; set; }
        public decimal COUNT_NEW_BHYT { get; set; }

        public decimal DAY_BHYT { get; set; }
        public decimal DAY_DV { get; set; }
        public decimal DAY { get; set; }

        public decimal COUNT_OUT_KHOI { get; set; }
        public decimal COUNT_OUT_DO { get; set; }
        public decimal COUNT_OUT_KTD { get; set; }
        public decimal COUNT_OUT_NANG { get; set; }
        public decimal COUNT_OUT_CV { get; set; }

        public decimal COUNT_DIE { get; set; }
        public decimal COUNT_DIE_TE { get; set; }
        public decimal COUNT_DIE_24 { get; set; }
        public decimal COUNT_END { get; set; }

        public Mrs00519RDO(HIS_DEPARTMENT_TRAN inDepartmentTran, HIS_DEPARTMENT_TRAN nextDepartmentTran, List<V_HIS_TREATMENT_4> listHisTreatment, List<HIS_PATIENT_TYPE_ALTER> lastPatienttypeAlter, Mrs00519Filter mrs00519Filter)
        {
           
            var treatment = listHisTreatment.FirstOrDefault(o=>o.ID==inDepartmentTran.TREATMENT_ID)??new V_HIS_TREATMENT_4();
            this.TREATMENT_CODE = treatment.TREATMENT_CODE;
            //var bill = listBill.FirstOrDefault(o => o.TREATMENT_ID == inDepartmentTran.TREATMENT_ID) ?? new HIS_TRANSACTION();
            var patientTypeAlter = lastPatienttypeAlter.FirstOrDefault(o => o.TREATMENT_ID == inDepartmentTran.TREATMENT_ID);

            //if (nextDepartmentTran.DEPARTMENT_IN_TIME == null)
            //{
            //    if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT) nextDepartmentTran.DEPARTMENT_IN_TIME = treatment.FEE_LOCK_TIME 
            //        ?? 99999999999999;
            //    else nextDepartmentTran.DEPARTMENT_IN_TIME = bill.TRANSACTION_TIME > 0 ? bill.TRANSACTION_TIME : 99999999999999;
            //}
            if (nextDepartmentTran.DEPARTMENT_IN_TIME == null) nextDepartmentTran.DEPARTMENT_IN_TIME = treatment.OUT_TIME??99999999999999;
            this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==inDepartmentTran.DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            //BN dau ki se lay:
            // vao khoa truoc Time_from
            if (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00519Filter.TIME_FROM && mrs00519Filter.TIME_FROM <= nextDepartmentTran.DEPARTMENT_IN_TIME)
                this.COUNT_OLD = 1;
            //Nguoi benh vao dieu tri noi tru se lay:
            //vao khoa tu time_from den time_to
            if (inDepartmentTran.DEPARTMENT_IN_TIME >= mrs00519Filter.TIME_FROM && inDepartmentTran.DEPARTMENT_IN_TIME < mrs00519Filter.TIME_TO)
            {
                this.COUNT_NEW = 1;
                if (MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB)))
                    this.COUNT_NEW_TE = 1;
                if (treatment.IS_EMERGENCY==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    this.COUNT_NEW_EMERGENCY = 1;
                if (patientTypeAlter.PATIENT_TYPE_ID==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.COUNT_NEW_BHYT = 1;
            }

            if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && treatment.OUT_TIME < mrs00519Filter.TIME_TO && treatment.OUT_TIME >= mrs00519Filter.TIME_FROM)
            {
                if (nextDepartmentTran.ID == 0)
                {
                    this.DAY = Inventec.Common.DateTime.Calculation.DifferenceDate(treatment.CLINICAL_IN_TIME ?? 0, treatment.OUT_TIME ?? 0) + 1;
                    if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        this.DAY_BHYT = Inventec.Common.DateTime.Calculation.DifferenceDate(treatment.CLINICAL_IN_TIME ?? 0, treatment.OUT_TIME ?? 0) + 1;
                    else
                        this.DAY_DV = Inventec.Common.DateTime.Calculation.DifferenceDate(treatment.CLINICAL_IN_TIME ?? 0, treatment.OUT_TIME ?? 0) + 1;

                    switch (treatment.TREATMENT_RESULT_ID)
                    {
                        case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI:
                            this.COUNT_OUT_KHOI = 1;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO:
                            this.COUNT_OUT_DO = 1;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD:
                            this.COUNT_OUT_KTD = 1;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG:
                            this.COUNT_OUT_NANG = 1;
                            break;
                    }
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                        this.COUNT_OUT_CV = 1;
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                    {
                        this.COUNT_DIE = 1;
                        if (MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB)))
                            this.COUNT_DIE_TE = 1;
                        if (treatment.DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                            this.COUNT_DIE_24 = 1;
                    }
                }
            }
            ////BN cuoi ki se lay:
            ////"BN vien phi chua thanh toan truoc Time_to" 
            ////hoac "BN BHYT chua khoa vien phi  truoc Time_to"
            //if (nextDepartmentTran.DEPARTMENT_IN_TIME >= mrs00519Filter.TIME_TO
            //    && ((patientTypeAlter.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
            //         && (bill.TRANSACTION_TIME == 0 || bill.TRANSACTION_TIME>mrs00519Filter.TIME_TO))
            //         || (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
            //         && (treatment.FEE_LOCK_TIME==null||treatment.FEE_LOCK_TIME > mrs00519Filter.TIME_TO
            //         ))))
            //    this.COUNT_END = 1;
            if (nextDepartmentTran.DEPARTMENT_IN_TIME >=mrs00519Filter.TIME_TO)
                this.COUNT_END = 1;
        }

        public Mrs00519RDO()
        {

        }
    }
}
