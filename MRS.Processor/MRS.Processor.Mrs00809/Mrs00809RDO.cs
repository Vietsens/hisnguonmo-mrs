using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00809
{
    public class Mrs00809RDO
    {
        public string DEPARTMENT_NAME { set; get; }
        public string DEPARTMENT_CODE { set; get; }
        public string EXECUTE_ROOM_CODE { set; get; }
        public string EXECUTE_ROOM_NAME { set; get; }
        public decimal AMOUNT_OLD { set; get; }
        public decimal AMOUNT_NEW { set; get; }
        public decimal AMOUNT_RV { set;get; }
        public decimal AMOUNT_DIE { set; get; }
        public decimal AMOUNT_TREATMENT { set; get; }
        public decimal AMOUNT_CD { set; get; }// bệnh nhân chuyển đến
        public decimal AMOUNT_CK { set; get; }// bệnh nhân chyển  khoa 

        public decimal AMOUNT_CV { set; get; }// bệnh nhân chuyển viện 
        public decimal AMOUNT_CM { set; get; }// bệnh nhân chuyển mỗ
    }
    public class Mrs00809BloodRdo
    {
        public string BLOOD_NAME { set; get; }
        public string AMOUNT { set; get; }
    }
    public class Mrs0809ServiceRdo{
        public string SERVICE_NAME { set; get; }
        public string SERVICE_CODE
        {
            set;get;
        }
        public decimal AMOUNT { set; get; }
        public decimal TOTAL_AMOUNT { set; get; }
        public string SERVICE_PRINT { set; get; }
    }
    public class Mrs00809TreatmentRdo
    {
        public string EXECUTE_ROOM_CODE { set; get; }
        public string EXECUTE_ROOM_NAME { set; get; }
        public string TREATMENT_CODE { set; get; }
        public string TREATMENT_NAME { set; get; }
        public long TREATMENT_ID { set; get; }
        public string SERVICE_NAME { set; get; }
        public string SERVICE_CODE { set; get; }
        public long SERVICE_ID { set; get; }
        public decimal AMOUNT { set; get; }
        public string PRINT { set; get; }
    }
}
