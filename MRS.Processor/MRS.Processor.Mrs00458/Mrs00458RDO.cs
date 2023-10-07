using Inventec.Common.Repository; 
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Reflection; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00458
{

    class Mrs00458RDO : V_HIS_TREATMENT
   
    {
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  } 
        public string TREATMENT_CODE { get;  set;  } 
        public string BIRTH_DAY { get;  set;  } 
        public long DEPARTMENT_CODE { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public string ROOM_CODE { get;  set;  }
        public string ROOM_NAME { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string FINISH_TIME { get;  set;  }
        public string DOB_STR { get;  set;  }
        public string GENDER_STR { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }

        public Mrs00458RDO(V_HIS_TREATMENT data) 
        {
            try
            {
                PropertyInfo[] pi = Properties.Get<V_HIS_TREATMENT>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, item.GetValue(data)); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00458RDO() { }
    }
}
