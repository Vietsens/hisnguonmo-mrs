using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00334
{
    public class Mrs00334RDO
    {
        public int NUMBER { get; set; }
        public long SERE_SERV_ID { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string REQUEST_DEPARTMENT { get; set; }  // khoa chỉ định
        public string PATIENT_CODE { get;  set;  }        // mã bệnh nhân
        public string PATIENT_NAME { get;  set;  }        // tên bệnh nhân
        public string YEAR { get;  set;  }                // năm sinh
        public string SERVICE_NAME { get;  set;  }        // tên dịch vụ
        public string PTTT_GROUP_NAME { get;  set;  }     // loại PTTT
        public decimal AMOUNT { get;  set;  }             // số lượng
        public decimal? PRICE { get;  set;  }             // giá dịch vụ
        public decimal? TOTAL_PRICE { get;  set;  }       // tổng tiền
        public decimal? MEDICINE_PRICE { get;  set;  }    // tiền thuốc đi kèm
        public decimal? MATERIAL_PRICE { get;  set;  }    // tiền vật tư đi kèm
        public decimal? PTTT_PRICE { get; set; }        // chi phí cho ekip thực hiện
        public string MEDI_MATE_TYPE_NAME { get; set; }//Tên thuốc vật tư
        public decimal? MEDI_MATE_AMOUNT { get; set; }//số lượng thuốc vật tư
        public decimal? MEDI_MATE_TOTAL_PRICE { get; set; }//Thành tiền thuốc vật tư

        public Mrs00334RDO()
        {

        }

        public string MEDI_MATE_TYPE_CODE { get; set; }

        public string SERVICE_CODE { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }
    }
}

