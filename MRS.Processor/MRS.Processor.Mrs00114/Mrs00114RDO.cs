using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00114
{
    class Mrs00114RDO
    {
        public short? IS_CHEMICAL_SUBSTANCE { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal TOTAL_AMOUNT { get;  set;  }

        public Mrs00114RDO() { }

        public Mrs00114RDO(List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_METY> ListServiceMety)
        {
            try
            {
                if (ListServiceMety != null && ListServiceMety.Count > 0)
                {
                    SERVICE_ID = ListServiceMety.First().MEDICINE_TYPE_ID; 
                    SERVICE_CODE = ListServiceMety.First().MEDICINE_TYPE_CODE;
                    SERVICE_NAME = ListServiceMety.First().MEDICINE_TYPE_NAME; 
                    SERVICE_UNIT_NAME = ListServiceMety.First().SERVICE_UNIT_NAME; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00114RDO(List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_MATY> ListServiceMaty)
        {
            try
            {
                if (ListServiceMaty != null && ListServiceMaty.Count > 0)
                {
                    SERVICE_ID = ListServiceMaty.First().MATERIAL_TYPE_ID; 
                    SERVICE_CODE = ListServiceMaty.First().MATERIAL_TYPE_CODE;
                    SERVICE_NAME = ListServiceMaty.First().MATERIAL_TYPE_NAME;
                    SERVICE_UNIT_NAME = ListServiceMaty.First().SERVICE_UNIT_NAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
