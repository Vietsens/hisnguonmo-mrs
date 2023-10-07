using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00386
{
    class PatientTypeAlter:HIS_TREATMENT
    {
        public long TREATMENT_ID { get; set; }

        public string RIGHT_ROUTE_CODE { get; set; }

        public string HEIN_MEDI_ORG_CODE { get; set; }

        public long TREATMENT_TYPE_ID { get; set; }

        public long LOG_TIME { get; set; }

        public long PATIENT_TYPE_ID { get; set; }
    }
}
