using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00098
{
    class Mrs00098RDO
    {
        public long INTRUCTION_TIME { get;  set;  }
        public long PATIENT_ID { get;  set;  }

        public string INTRUCTION_DATE_STR { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string DOB_YEAR { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string ICD_SUIM { get;  set;  }
        public string REQUEST_ROOM { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SUIM_RESULT { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        Dictionary<long, HIS_ICD> result = new Dictionary<long, HIS_ICD>(); 

        public Mrs00098RDO() { }

        public Mrs00098RDO(V_HIS_SERE_SERV sereServ, List<V_HIS_SERVICE_REQ> serviceReqs, List<HIS_SERE_SERV_EXT> exts, Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter)
        {
            try
            {
                V_HIS_SERVICE_REQ serviceReq = serviceReqs.Where(o => o.ID == sereServ.SERVICE_REQ_ID).FirstOrDefault() ?? new V_HIS_SERVICE_REQ(); 
                HIS_SERE_SERV_EXT ext = exts.Where(o => o.SERE_SERV_ID == sereServ.ID).FirstOrDefault() ?? new HIS_SERE_SERV_EXT();
                List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlter = dicPatientTypeAlter.ContainsKey(sereServ.TDL_TREATMENT_ID.Value) ? dicPatientTypeAlter[sereServ.TDL_TREATMENT_ID.Value] : new List<V_HIS_PATIENT_TYPE_ALTER>();
                EXECUTE_USERNAME = serviceReq.EXECUTE_USERNAME;
                REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                this.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME; 
                this.REQUEST_ROOM = serviceReq.REQUEST_ROOM_NAME; 
                this.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                this.SUIM_RESULT = ext != null ? ext.CONCLUDE : ""; 
                this.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE; 
                this.PATIENT_ID = serviceReq.TDL_PATIENT_ID; 
                this.INTRUCTION_TIME = serviceReq.INTRUCTION_TIME; 
                this.GENDER_NAME = serviceReq.TDL_PATIENT_GENDER_NAME; 
                this.INTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq.INTRUCTION_TIME); 
                if(sereServ.PATIENT_TYPE_ID ==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                this.HEIN_CARD_NUMBER = string.Join(",",patientTypeAlter.Select(o=>o.HEIN_CARD_NUMBER).Distinct().ToList()); 
                this.VIR_ADDRESS = serviceReq.TDL_PATIENT_ADDRESS; 
                this.DOB_YEAR = serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                if (serviceReq != null)
                {
                    ICD_SUIM = serviceReq.ICD_NAME ;
                }
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private Dictionary<long, HIS_ICD> GetIcd()
        {
            try
            {
                if (result == null || result.Count == 0)
                {
                    CommonParam param = new CommonParam(); 
                    HisIcdFilterQuery filter = new HisIcdFilterQuery(); 
                    var listIcd = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(filter); 
                    foreach (var item in listIcd)
                    {
                        if (!result.ContainsKey(item.ID)) result[item.ID] = item; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return null; 
            }
            return result; 
        }

    }
}
