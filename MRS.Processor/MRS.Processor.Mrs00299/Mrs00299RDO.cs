using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00299
{
    public class Mrs00299RDO :HIS_TREATMENT
    {
        public string EXECUTE_ROOM_NAME { get;  set;  }
        public string IN_TIME_STR { get;  set;  }
        public string OUT_TIME_STR { get;  set;  }
        public string DOB_YEAR { get;  set;  }
        public string HEIN_CARD_NUMBER_STR { get;  set;  }
        public decimal MEDICINE_PRICE { get; set; }

        public Mrs00299RDO(HIS_TREATMENT data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HIS_TREATMENT>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data))); 
                }
                this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.IN_TIME); 
                this.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.OUT_TIME ?? 0); 
                data.TDL_PATIENT_DOB = System.DateTime.Now.Year - (int)(data.TDL_PATIENT_DOB / 10000000000); 
                this.DOB_YEAR = data.TDL_PATIENT_DOB.ToString();
                this.HEIN_CARD_NUMBER_STR = data.TDL_HEIN_CARD_NUMBER; 
                this.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o=>o.ID==data.END_ROOM_ID)??new V_HIS_ROOM()).ROOM_NAME; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
