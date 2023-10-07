using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00499
{
    public class Mrs00499RDO
    {

        public Mrs00499RDO(V_HIS_SERE_SERV sereServ, V_HIS_SERE_SERV_PTTT sereServPttt, V_HIS_TREATMENT treatment, List<HIS_EKIP_USER> listEkipUser, Dictionary<long, string> dicPTTTServiceType)
        {
            this.SERE_SERV_ID = sereServ.ID; 
            this.SERVICE_ID = sereServ.SERVICE_ID; 
            this.TREATMENT_ID = treatment.ID; 
            this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME); 
            this.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
            this.YEAR_DOB = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
            this.TDL_SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
            this.PRICE = sereServ.PRICE; 
            this.SS_PTTT_GROUP_NAME = sereServPttt.PTTT_GROUP_NAME; 
            this.PTTT_GROUP_NAME = dicPTTTServiceType.ContainsKey(sereServ.SERVICE_ID) ? dicPTTTServiceType[sereServ.SERVICE_ID] : ""; 
            this.AMOUNT = sereServ.AMOUNT; 
            this.COUNT_USER = listEkipUser.Where(o => o.EKIP_ID == sereServ.EKIP_ID).ToList().Count; 
        }
        public Mrs00499RDO(V_HIS_SERE_SERV sereServ, V_HIS_TREATMENT treatment, Dictionary<long, string> dicPTTTServiceType)
            : this(sereServ, new V_HIS_SERE_SERV_PTTT(), treatment, new List<HIS_EKIP_USER>(), dicPTTTServiceType)
        {
            
        }
        public long SERE_SERV_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public long TREATMENT_ID { get;  set;  }
        public string IN_TIME_STR { get;  set;  }
        public string TDL_PATIENT_NAME { get;  set;  }
        public string YEAR_DOB { get;  set;  }
        public string TDL_SERVICE_NAME { get;  set;  }
        public Decimal PRICE { get;  set;  }
        public string PTTT_GROUP_NAME { get;  set;  }
        public string SS_PTTT_GROUP_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public long COUNT_USER {get; set; }
        public Mrs00499RDO() { }
	
    }
}
