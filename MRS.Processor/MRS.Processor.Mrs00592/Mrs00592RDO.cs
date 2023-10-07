using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00592
{
    public class Mrs00592RDO
    {


        public Decimal TREATMENT_ID { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_SURG { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_DIFFICULT { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_SURG { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_DIFFICULT { get; set; }

        public Mrs00592RDO()
        {
            // TODO: Complete member initialization
        }

        public Mrs00592RDO(HIS_TREATMENT r, List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter, List<HIS_BABY> listHisBaby, Mrs00592Filter mrs00592Filter)
        {
            this.TREATMENT_ID = r.ID;
            HIS_PATIENT_TYPE_ALTER patientTypeAlter = listHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == r.ID) ?? new HIS_PATIENT_TYPE_ALTER();
            List<HIS_BABY> listHisBabySub = listHisBaby.Where(o => o.TREATMENT_ID == r.ID).ToList();


            if ((mrs00592Filter.ICD_CODE__PROCREATE_DIFFICULTs != null && mrs00592Filter.ICD_CODE__PROCREATE_DIFFICULTs.Contains(r.ICD_CODE)))
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
            if ((mrs00592Filter.ICD_CODE__PROCREATE_COMMONs != null && mrs00592Filter.ICD_CODE__PROCREATE_COMMONs.Contains(r.ICD_CODE)))
            {
                if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.TOTAL_TREATMENT_BHYT_PROCREATE_NORMAL = 1;
                }
                else
                {
                    this.TOTAL_TREATMENT_VP_PROCREATE_NORMAL = 1;
                }

            }
            if ((mrs00592Filter.ICD_CODE__PROCREATE_SURGs != null && mrs00592Filter.ICD_CODE__PROCREATE_SURGs.Contains(r.ICD_CODE)))
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


            if ((mrs00592Filter.ICD_CODE__PROCREATE_EARLYs != null && mrs00592Filter.ICD_CODE__PROCREATE_EARLYs.Contains(r.ICD_CODE)))
            {
                if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.TOTAL_TREATMENT_BHYT_PROCREATE_EARLY = 1;
                }
                else
                {
                    this.TOTAL_TREATMENT_VP_PROCREATE_EARLY = 1;
                }
            }

            if ((mrs00592Filter.ICD_CODE__PROCREATE_FX_GHs != null && mrs00592Filter.ICD_CODE__PROCREATE_FX_GHs.Contains(r.ICD_CODE)))
            {
                if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.TOTAL_TREATMENT_BHYT_PROCREATE_FX_GH = 1;
                }
                else
                {
                    this.TOTAL_TREATMENT_VP_PROCREATE_FX_GH = 1;
                }
            }
            if (listHisBabySub.Exists(o => o.BORN_RESULT_ID == 1))
            {
                if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.TOTAL_TREATMENT_BHYT_BIRTH = 1;
                }
                else
                {
                    this.TOTAL_TREATMENT_VP_BIRTH = 1;
                }
                if (listHisBabySub.Exists(o => o.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE))
                {
                    if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.TOTAL_TREATMENT_BHYT_BIRTH_M = 1;
                    }
                    else
                    {
                        this.TOTAL_TREATMENT_VP_BIRTH_M = 1;
                    }
                }
                else
                {
                    if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.TOTAL_TREATMENT_BHYT_BIRTH_F = 1;
                    }
                    else
                    {
                        this.TOTAL_TREATMENT_VP_BIRTH_F = 1;
                    }
                }
            }
        }




        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_NORMAL { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_NORMAL { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_EARLY { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_EARLY { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_PROCREATE_FX_GH { get; set; }

        public decimal TOTAL_TREATMENT_VP_PROCREATE_FX_GH { get; set; }

        public decimal TOTAL_TREATMENT_VP_BIRTH { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_BIRTH { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_BIRTH_M { get; set; }

        public decimal TOTAL_TREATMENT_VP_BIRTH_M { get; set; }

        public decimal TOTAL_TREATMENT_BHYT_BIRTH_F { get; set; }

        public decimal TOTAL_TREATMENT_VP_BIRTH_F { get; set; }
    }
}
