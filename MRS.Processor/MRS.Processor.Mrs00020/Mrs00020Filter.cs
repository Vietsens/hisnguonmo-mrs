using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00020
{
     public class Mrs00020Filter
    {
        public long? BRANCH_ID { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public long? END_ROOM_ID { get; set; }
        public List<long> END_ROOM_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        
        public long? DEPARTMENT_ID { get; set; }
        public bool? IS_KB { get; set; }
        public bool? IS_TNT { get; set; }
        public bool? IS_CC { get; set; }
        public string KB1 { get; set; }
        public string KB2 { get; set; }
        public string TNT { get; set; }
        public string CC { get; set; }

        public bool? BHYT100 { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public Mrs00020Filter()
            : base()
        {
        }

        public List<long> BRANCH_IDs { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }
        public List<long> FEE_LOCK_ROOM_IDs { get; set; }
        public List<string> FEE_LOCK_LOGINNAMEs { get; set; }
        public string KEY_GROUP_TREA { get; set; }

        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //1:duyet giám định bhyt; 2: khoa vien phi; 3: ra vien; 4: vao vien
        public List<long> PATIENT_CLASSIFY_IDs { get; set; } //đối tượng chi tiêt
        public List<long> PATIENT_TYPE_IDs { get; set; }// đối tượng thanh toán
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public short? INPUT_DATA_ID_PROVINCE_TYPE { get; set; } //1:Noi tinh, 2:Ngoai tinh

        public short? INPUT_DATA_ID_ROUTE_TYPE { get; set; } //1:Dung tuyen, 2:Trai tuyen

        public List<long> OTHER_PAY_SOURCE_IDs { get; set; }

        public List<long> PAY_FORM_IDs { get; set; }

        public short? INPUT_DATA_ID_STTRAN_TYPE { get; set; } //Trang thai: 1: khoa; 2: mo khoa; 3: Tat ca

        public string ACCEPT_HEIN_MEDI_ORG_CODE { get; set; }
    }
}
