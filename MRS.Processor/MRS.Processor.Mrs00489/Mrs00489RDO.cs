using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00489
{
    public class Mrs00489RDO
    {

        public Mrs00489RDO(V_HIS_SERE_SERV ss, List<V_HIS_EKIP_USER> listEkipUser, V_HIS_SERE_SERV_PTTT sp, V_HIS_TREATMENT t, Dictionary<string, PropertyInfo> dicAmountField, Dictionary<long, string> dicPTTTServiceType)
        {
            this.SERE_SERV_ID = ss.ID; 
            this.SERVICE_ID = ss.SERVICE_ID; 
            this.TREATMENT_ID = t.ID; 
            this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(t.IN_TIME); 
            this.TDL_PATIENT_NAME = t.TDL_PATIENT_NAME; 
            this.YEAR_DOB = t.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
            this.TDL_SERVICE_NAME = ss.TDL_SERVICE_NAME; 
            this.PRICE = ss.PRICE; 
            this.SS_PTTT_GROUP_NAME = sp.PTTT_GROUP_NAME; 
            this.PTTT_GROUP_NAME = dicPTTTServiceType.ContainsKey(ss.SERVICE_ID) ? dicPTTTServiceType[ss.SERVICE_ID] : ""; 
            this.AMOUNT = ss.AMOUNT; 
            
            var ekipUserSub = listEkipUser.Where(o => o.EKIP_ID == ss.EKIP_ID).ToList() ?? new List<V_HIS_EKIP_USER>();
            foreach (var item in ekipUserSub)
            {
                if (item.EXECUTE_ROLE_NAME != null) dicAmountField[item.EXECUTE_ROLE_NAME].SetValue(this, item.USERNAME); 
            }
            
        }
        public long SERVICE_ID { get;  set;  }
        public long TREATMENT_ID { get;  set;  }
        public long SERE_SERV_ID { get;  set;  }
        public string IN_TIME_STR { get;  set;  }
        public string TDL_PATIENT_NAME { get;  set;  }
        public string YEAR_DOB { get;  set;  }
        public string TDL_SERVICE_NAME { get;  set;  }
        public Decimal PRICE { get;  set;  }
        public string PTTT_GROUP_NAME { get;  set;  }
        public string SS_PTTT_GROUP_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public string COUNT_EXECUTE_ROLE_1 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_2 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_3 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_4 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_5 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_6 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_7 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_8 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_9 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_10 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_11 { get;  set;  }
        public string COUNT_EXECUTE_ROLE_12 { get;  set;  }
        public Mrs00489RDO() { }
    }
}
