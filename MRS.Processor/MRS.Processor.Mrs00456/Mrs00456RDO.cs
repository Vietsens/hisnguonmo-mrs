using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00456
{
    public class Mrs00456RDO
    {
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public decimal TOTAL_PATIENT { get; set; }
        public decimal TREATMENT_ID { get; set; }
        //hh
        public decimal TOTAL_PATIENT_HH { get; set; }   // bệnh nhân
        public decimal TOTAL_HH { get; set; }           // bệnh nhân
        public decimal TOTAL_PATIENT_TBHH { get; set; } // tiêu bản
        public decimal TOTAL_PRICE_HH { get; set; }     // tiền
        public decimal TOTAL_TBM { get; set; }
        public decimal TOTAL_MDMC { get; set; }
        public decimal TOTAL_NM { get; set; }
        public decimal TOTAL_ML { get; set; }
        public decimal TOTAL_HIV { get; set; }
        public decimal TOTAL_HBSAG { get; set; }
        public decimal TOTAL_SR { get; set; }
        public decimal TOTAL_HH_BHYT { get; set; }
        public decimal TOTAL_HH_NU { get; set; }
        public decimal TOTAL_HH_15 { get; set; }
        public decimal TOTAL_HH_DTTS { get; set; }

        //sh
        public decimal TOTAL_PATIENT_SH { get; set; }
        public decimal TOTAL_PATIENT_TBSH { get; set; }
        public decimal TOTAL_PRICE_SH { get; set; }
        public decimal TOTAL_SH { get; set; }

        public decimal TOTAL_PATIENT_SHM { get; set; }
        public decimal TOTAL_PATIENT_TBSHM { get; set; }
        public decimal TOTAL_PRICE_SHM { get; set; }
        public decimal TOTAL_SHM { get; set; }
        public decimal TOTAL_SHM_BHYT { get; set; }
        public decimal TOTAL_SHM_NU { get; set; }
        public decimal TOTAL_SHM_15 { get; set; }
        public decimal TOTAL_SHM_DTTS { get; set; }

        public decimal TOTAL_PATIENT_NT { get; set; }
        public decimal TOTAL_PATIENT_TBNT { get; set; }
        public decimal TOTAL_PRICE_NT { get; set; }
        public decimal TOTAL_NT { get; set; }
        public decimal TOTAL_NT_BHYT { get; set; }
        public decimal TOTAL_NT_NU { get; set; }
        public decimal TOTAL_NT_15 { get; set; }
        public decimal TOTAL_NT_DTTS { get; set; }

        public decimal TOTAL_PATIENT_XND { get; set; }
        public decimal TOTAL_PATIENT_TBXND { get; set; }
        public decimal TOTAL_PRICE_XND { get; set; }
        public decimal TOTAL_XND { get; set; }
        public decimal TOTAL_XND_BHYT { get; set; }
        public decimal TOTAL_XND_NU { get; set; }
        public decimal TOTAL_XND_15 { get; set; }
        public decimal TOTAL_XND_DTTS { get; set; }

        //vk
        public decimal TOTAL_PATIENT_VK { get; set; }
        public decimal TOTAL_PATIENT_TBVK { get; set; }
        public decimal TOTAL_PRICE_VK { get; set; }
        public decimal TOTAL_VK { get; set; }
        public decimal TOTAL_LAO { get; set; }
        public decimal TOTAL_KST { get; set; }
        public decimal TOTAL_VK_BHYT { get; set; }
        public decimal TOTAL_VK_NU { get; set; }
        public decimal TOTAL_VK_15 { get; set; }
        public decimal TOTAL_VK_DTTS { get; set; }

    }
}
