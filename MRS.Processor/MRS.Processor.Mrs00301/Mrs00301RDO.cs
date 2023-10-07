using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00301
{
    public class Mrs00301RDO : V_HIS_TREATMENT
    {
        public string EXECUTE_ROOM_NAME { get;  set;  }
        public string IN_TIME_STR { get;  set;  }
        public string OUT_TIME_STR { get;  set;  }
        public string DOB_YEAR { get;  set;  }
        public string HEIN_CARD_NUMBER_STR { get;  set;  }

        public Mrs00301RDO(V_HIS_TREATMENT data, Dictionary<long, List<HIS_SERVICE_REQ>> dicServiceReq, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<HIS_EXECUTE_ROOM> listExamRoom)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<V_HIS_TREATMENT>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));   
                }
                this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.IN_TIME); 
                this.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.OUT_TIME ?? 0); 
                data.TDL_PATIENT_DOB = System.DateTime.Now.Year - (int)(data.TDL_PATIENT_DOB / 10000000000); 
                this.DOB_YEAR = data.TDL_PATIENT_DOB.ToString(); 
                this.HEIN_CARD_NUMBER_STR = LastHeinCardnumber(data, dicPatientTypeAlter); 
                this.EXECUTE_ROOM_NAME = ExcuteRoom(data, dicServiceReq, listExamRoom); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private string ExcuteRoom(V_HIS_TREATMENT data, Dictionary<long, List<HIS_SERVICE_REQ>> dicServiceReq, List<HIS_EXECUTE_ROOM> listExamRoom)
        {
            string result = ""; 
            try
            {
                if (dicServiceReq.ContainsKey(data.ID))
                {
                    var sub = listExamRoom.Where(o => dicServiceReq[data.ID].Select(p => p.EXECUTE_ROOM_ID).Contains(o.ROOM_ID)).ToList(); 
                    result = string.Join(", ", sub.Select(o => o.EXECUTE_ROOM_NAME).ToList()); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return result; 
            }
            return result; 
        }

        private string LastHeinCardnumber(V_HIS_TREATMENT data, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter)
        {
            string result = ""; 
            try
            {
                if (dicPatientTypeAlter.ContainsKey(data.ID))
                {
                    result = dicPatientTypeAlter[data.ID].HEIN_CARD_NUMBER; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return result; 
            }
            return result; 
        }

    }
}
