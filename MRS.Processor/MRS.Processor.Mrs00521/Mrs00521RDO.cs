using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00521
{
    public class Mrs00521RDO
    {


        public decimal TREATMENT_ID { get; set; }	
        public decimal TOTAL_TREATMENT_EXAM_BHYT { get; set; }	
        public decimal TOTAL_TREATMENT_EXAM_VP { get; set; }
        public decimal TOTAL_TREATMENT_EXAM { get; set; }

        public decimal TOTAL_TREATMENT_FETUS_EXAM_BHYT { get; set; }
        public decimal TOTAL_TREATMENT_FETUS_EXAM_VP { get; set; }	
        public decimal TOTAL_TREATMENT_FETUS_EXAM { get; set; }

        public decimal TOTAL_TREATMENT_GYNECOLOGICAL_EXAM_BHYT { get; set; }	
        public decimal TOTAL_TREATMENT_GYNECOLOGICAL_EXAM_VP { get; set; }	
        public decimal TOTAL_TREATMENT_GYNECOLOGICAL_EXAM { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_BHYT_EXAM { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_VP_EXAM { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_EXAM { get; set; }

        public decimal TOTAL_TREATMENT_HBSAG_TEST_BHYT_EXAM { get; set; }
        public decimal TOTAL_TREATMENT_HBSAG_TEST_VP_EXAM { get; set; }
        public decimal TOTAL_TREATMENT_HBSAG_TEST_EXAM { get; set; }

        public decimal TOTAL_TREATMENT_HIV_TEST_BHYT_TREATOUT { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_VP_TREATOUT { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_TREATOUT { get; set; }

        public decimal TOTAL_TREATMENT_HBSAG_TEST_BHYT_TREATOUT { get; set; }
        public decimal TOTAL_TREATMENT_HBSAG_TEST_VP_TREATOUT { get; set; }
        public decimal TOTAL_TREATMENT_HBSAG_TEST_TREATOUT { get; set; }

        public decimal TOTAL_TREATMENT_HIV_TEST_BHYT_TREATIN { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_VP_TREATIN { get; set; }
        public decimal TOTAL_TREATMENT_HIV_TEST_TREATIN { get; set; }

        public decimal TOTAL_TREATMENT_HBSAG_TEST_BHYT_TREATIN { get; set; }
        public decimal TOTAL_TREATMENT_HBSAG_TEST_VP_TREATIN { get; set; }
        public decimal TOTAL_TREATMENT_HBSAG_TEST_TREATIN { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_EXAM_GYNECOLOGICAL { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_TREAT_GYNECOLOGICAL { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_TREAT_PROC { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_EXAM_PROC { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_SURG { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_EXAM_GYNECOLOGICAL_SURG { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_ABORTION { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_DIFFICULT { get; set; }


        public decimal TOTAL_TREATMENT_VP_PROCREATE { get; set; }

        public decimal TOTAL_TREATMENT_VP_EXAM_GYNECOLOGICAL { get; set; }

        public decimal TOTAL_TREATMENT_VP_TREAT_GYNECOLOGICAL { get; set; }

        public decimal TOTAL_TREATMENT_VP_TREAT_PROC { get; set; }

        public decimal TOTAL_TREATMENT_VP_EXAM_PROC { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_SURG { get; set; }

        public decimal TOTAL_TREATMENT_VP_EXAM_GYNECOLOGICAL_SURG { get; set; }

        public decimal TOTAL_TREATMENT_VP_ABORTION { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_DIFFICULT { get; set; }

        public Mrs00521RDO(HIS_SERE_SERV r, List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter)
        {
            this.TREATMENT_ID = r.TDL_TREATMENT_ID??0;
            var lastPatientTypeAlter = listHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == (r.TDL_TREATMENT_ID ?? 0)) ?? new HIS_PATIENT_TYPE_ALTER();
            var currentPatientTypeAlter = listHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == (r.TDL_TREATMENT_ID ?? 0) && o.LOG_TIME <= r.TDL_INTRUCTION_TIME) ?? new HIS_PATIENT_TYPE_ALTER();

            if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                && HisServiceCFG.SERVICE_CODE__HIVs.Contains(r.TDL_SERVICE_CODE))
            {
                this.TOTAL_TREATMENT_HIV_TEST_EXAM = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_HIV_TEST_BHYT_EXAM = 1;
                else
                    this.TOTAL_TREATMENT_HIV_TEST_VP_EXAM = 1;
            }

            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM
                && HisServiceCFG.SERVICE_CODE__HBSAGs.Contains(r.TDL_SERVICE_CODE))
            {
                this.TOTAL_TREATMENT_HBSAG_TEST_EXAM = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_HBSAG_TEST_BHYT_EXAM = 1;
                else
                    this.TOTAL_TREATMENT_HBSAG_TEST_VP_EXAM = 1;
            }
            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU
                && HisServiceCFG.SERVICE_CODE__HIVs.Contains(r.TDL_SERVICE_CODE))
            {
                this.TOTAL_TREATMENT_HIV_TEST_TREATOUT = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_HIV_TEST_BHYT_TREATOUT = 1;
                else
                    this.TOTAL_TREATMENT_HIV_TEST_VP_TREATOUT = 1;
            }

            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU
                && HisServiceCFG.SERVICE_CODE__HBSAGs.Contains(r.TDL_SERVICE_CODE))
            {
                this.TOTAL_TREATMENT_HBSAG_TEST_TREATOUT = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_HBSAG_TEST_BHYT_TREATOUT = 1;
                else
                    this.TOTAL_TREATMENT_HBSAG_TEST_VP_TREATOUT = 1;
            }
            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                && HisServiceCFG.SERVICE_CODE__HIVs.Contains(r.TDL_SERVICE_CODE))
            {
                this.TOTAL_TREATMENT_HIV_TEST_TREATIN = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_HIV_TEST_BHYT_TREATIN = 1;
                else
                    this.TOTAL_TREATMENT_HIV_TEST_VP_TREATIN = 1;
            }

            else if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                && HisServiceCFG.SERVICE_CODE__HBSAGs.Contains(r.TDL_SERVICE_CODE))
            {
                this.TOTAL_TREATMENT_HBSAG_TEST_TREATIN = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_HBSAG_TEST_BHYT_TREATIN = 1;
                else
                    this.TOTAL_TREATMENT_HBSAG_TEST_VP_TREATIN = 1;
            }
            else
                this.TREATMENT_ID = 0;
        }

        public Mrs00521RDO(HIS_SERVICE_REQ r, List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter)
        {
            this.TREATMENT_ID = r.TREATMENT_ID;
            var lastPatientTypeAlter = listHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == r.TREATMENT_ID) ?? new HIS_PATIENT_TYPE_ALTER();

            this.TOTAL_TREATMENT_EXAM = 1;
            if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                this.TOTAL_TREATMENT_EXAM_BHYT = 1;
            else
                this.TOTAL_TREATMENT_EXAM_VP = 1;

            if (HisIcdCFG.ICD_CODE__FETUS_EXAM.Contains(r.ICD_CODE))
            {
                this.TOTAL_TREATMENT_FETUS_EXAM = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_FETUS_EXAM_BHYT = 1;
                else
                    this.TOTAL_TREATMENT_FETUS_EXAM_VP = 1;
            }
            else
            {
                this.TOTAL_TREATMENT_GYNECOLOGICAL_EXAM = 1;
                if (lastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    this.TOTAL_TREATMENT_GYNECOLOGICAL_EXAM_BHYT = 1;
                else
                    this.TOTAL_TREATMENT_GYNECOLOGICAL_EXAM_VP = 1;
            }
        }

        public Mrs00521RDO()
        {
            // TODO: Complete member initialization
        }

        public Mrs00521RDO(HIS_TREATMENT r, List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter,List<HIS_SERE_SERV> listSereServAb, Mrs00521Filter mrs00521Filter)
        {
            this.TREATMENT_ID = r.ID;
            HIS_PATIENT_TYPE_ALTER patientTypeAlter = listHisPatientTypeAlter.FirstOrDefault(o=>o.TREATMENT_ID==r.ID)??new HIS_PATIENT_TYPE_ALTER();
         List<HIS_SERE_SERV> listSereServAbSub = listSereServAb.Where(o=>o.TDL_TREATMENT_ID==r.ID).ToList();

         if ((mrs00521Filter.ICD_CODE__PROCREATE_COMMONs != null && mrs00521Filter.ICD_CODE__PROCREATE_COMMONs.Contains(r.ICD_CODE))
             || (mrs00521Filter.ICD_CODE__PROCREATE_DIFFICULTs != null && mrs00521Filter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(r.ICD_CODE))
             ||(mrs00521Filter.ICD_CODE__PROCREATE_SURGs != null && mrs00521Filter.ICD_CODE__PROCREATE_SURGs.Contains(r.ICD_CODE)))
         {
              if ((mrs00521Filter.ICD_CODE__PROCREATE_COMMONs != null && mrs00521Filter.ICD_CODE__PROCREATE_COMMONs.Contains(r.ICD_CODE))
             || (mrs00521Filter.ICD_CODE__PROCREATE_DIFFICULTs != null && mrs00521Filter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(r.ICD_CODE)))
              {
                  if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                  {
                      this.TOTAL_TREATMENT_BHYT_PROCREATE = 1;
                  }
                  else
                  {
                      this.TOTAL_TREATMENT_VP_PROCREATE = 1;
                  }
                  if ((mrs00521Filter.ICD_CODE__PROCREATE_DIFFICULTs != null && mrs00521Filter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(r.ICD_CODE)))
                  {
                      if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                      {
                          this.TOTAL_TREATMENT_BHYT_PROCREATE_DIFFICULT = 1;
                      }
                      else
                      {
                          this.TOTAL_TREATMENT_VP_PROCREATE_DIFFICULT = 1;
                      }

                  }
               
              }
             else
              {
                  
                  if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                  {
                      this.TOTAL_TREATMENT_BHYT_PROCREATE_SURG = 1;
                  }
                  else
                  {
                      this.TOTAL_TREATMENT_VP_PROCREATE_SURG = 1;
                  }
              }
             if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
             {
                 if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                  {
                      this.TOTAL_TREATMENT_BHYT_EXAM_PROC = 1;
                  }
                  else
                  {
                      this.TOTAL_TREATMENT_BHYT_EXAM_PROC = 1;
                  }
             }
             else if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
             {
                 if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                 {
                     this.TOTAL_TREATMENT_BHYT_NGTRU_PROC = 1;
                 }
                 else
                 {
                     this.TOTAL_TREATMENT_VP_NGTRU_PROC = 1;
                 }
             }
             else if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
             {
                 if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                 {
                     this.TOTAL_TREATMENT_BHYT_NOITRU_PROC = 1;
                 }
                 else
                 {
                     this.TOTAL_TREATMENT_VP_NOITRU_PROC = 1;
                 }
             }
         }
         else if ((mrs00521Filter.ICD_CODE__GYNECOLOGICALs != null && mrs00521Filter.ICD_CODE__GYNECOLOGICALs.Contains(r.ICD_CODE)))
         {
             if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
             {
                 if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                 {
                     this.TOTAL_TREATMENT_BHYT_EXAM_GYNECOLOGICAL = 1;
                 }
                 else
                 {
                     this.TOTAL_TREATMENT_VP_EXAM_GYNECOLOGICAL = 1;
                 }
             }
             else
             {
                 if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                 {
                     this.TOTAL_TREATMENT_BHYT_TREAT_GYNECOLOGICAL = 1;
                 }
                 else
                 {
                     this.TOTAL_TREATMENT_VP_TREAT_GYNECOLOGICAL = 1;
                 }
             }
         }
         else if ((mrs00521Filter.ICD_CODE__GYNECOLOGICAL_SURGs != null && mrs00521Filter.ICD_CODE__GYNECOLOGICAL_SURGs.Contains(r.ICD_CODE)))
         {
             if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
             {
                 this.TOTAL_TREATMENT_BHYT_EXAM_GYNECOLOGICAL_SURG = 1;
             }
             else
             {
                 this.TOTAL_TREATMENT_VP_EXAM_GYNECOLOGICAL_SURG = 1;
             }
         }

       
            if ((mrs00521Filter.SERVICE_CODE__ABORTIONs != null && listSereServAbSub.Exists(o=>mrs00521Filter.SERVICE_CODE__ABORTIONs.Contains(o.TDL_SERVICE_CODE))))
         {
             if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
             {
                 this.TOTAL_TREATMENT_BHYT_ABORTION = 1;
             }
             else
             {
                 this.TOTAL_TREATMENT_BHYT_ABORTION = 1;
             }
         }
        }





        public int TOTAL_TREATMENT_VP_NGTRU_PROC { get; set; }

        public int TOTAL_TREATMENT_BHYT_NGTRU_PROC { get; set; }

        public int TOTAL_TREATMENT_BHYT_NOITRU_PROC { get; set; }

        public int TOTAL_TREATMENT_VP_NOITRU_PROC { get; set; }

        public int TOTAL_TREATMENT_BHYT_TREAT_NGTRU_GYNECOLOGICAL { get; set; }

        public int TOTAL_TREATMENT_VP_TREAT_NGTRU_GYNECOLOGICAL { get; set; }

        public int TOTAL_TREATMENT_BHYT_TREAT_NOITRU_GYNECOLOGICAL { get; set; }

        public int TOTAL_TREATMENT_VP_TREAT_NOITRU_GYNECOLOGICAL { get; set; }
    }
}
