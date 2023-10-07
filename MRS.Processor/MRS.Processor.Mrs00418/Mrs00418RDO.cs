using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00418
{
    public class Mrs00418RDO
    {
        public long EXECUTE_ROOM_ID { get; set; }// phòng xử lý
        public string EXECUTE_ROOM_NAME { get; set; }
        public long EXECUTE_DEPARTMENT_ID { get; set; }// khoa xử lý
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public decimal TreatmentAllAmount { get;  set;  }// sổ lượng BN tổng
        public decimal TreatmentInAmount { get; set; }// số lượng BN nội trú
        public decimal TreatmentOutAmount { get; set; } // số lượng BN ngoại trú
        public decimal TreatmentExamAmount { get; set; } //số lượng BN khám


        public decimal ALL_AMOUNT { get; set; }// sổ lượng tổng
        public decimal IN_AMOUNT { get; set; }// số lượng nội trú
        public decimal OUT_AMOUNT { get; set; } // số lượng ngoại trú
        public decimal EXAM_AMOUNT { get; set; }// số lượng khám
    }
}
