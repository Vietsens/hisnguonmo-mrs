using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00117
{
    class Mrs00117RDO
    {
        public string PATIENT_NAME { get;  set;  }
        public string PATIENT_DATE_OF_TIME { get;  set;  }
        public decimal? GROUP_SERVICE_1 { get;  set;  }
        public decimal? GROUP_SERVICE_2 { get;  set;  }
        public decimal? GROUP_SERVICE_3 { get;  set;  }
        public decimal? GROUP_SERVICE_4 { get;  set;  }
        public decimal? GROUP_SERVICE_5 { get;  set;  }
        public decimal? GROUP_SERVICE_6 { get;  set;  }
        public decimal? GROUP_SERVICE_7 { get;  set;  }
        public decimal? GROUP_SERVICE_8 { get;  set;  }
        public decimal? GROUP_SERVICE_9 { get;  set;  }
        public decimal? GROUP_SERVICE_10 { get;  set;  }
        public decimal? GROUP_SERVICE_11 { get;  set;  }
        public decimal? GROUP_SERVICE_12 { get;  set;  }
        public decimal? GROUP_SERVICE_13 { get;  set;  }

        public Mrs00117RDO() { }

        public Mrs00117RDO(List<decimal> listService)
        {
            GROUP_SERVICE_1 = listService[0]; //siêu âm
            GROUP_SERVICE_2 = listService[1]; //điện tim
            GROUP_SERVICE_3 = listService[2]; //nội soi
            GROUP_SERVICE_4 = listService[3]; //XQ
            GROUP_SERVICE_5 = listService[4]; //CT
            GROUP_SERVICE_6 = listService[5]; //xét nghiệm
            GROUP_SERVICE_7 = listService[6]; //điện não
            GROUP_SERVICE_8 = listService[7]; //bột
            GROUP_SERVICE_9 = listService[8]; //thủ thuật
            GROUP_SERVICE_10 = listService[9]; //đại tràng
            GROUP_SERVICE_11 = listService[10]; //B6
            GROUP_SERVICE_12 = listService[11]; //B7
            GROUP_SERVICE_13 = listService[12]; //lý liệu
        }
    }
}
