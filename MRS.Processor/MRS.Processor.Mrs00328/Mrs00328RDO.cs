using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00328
{
    class Mrs00328RDO
    {
        public string ACTIVE_INGR_BHYT_CODE { get;  set;  }
        public string ACTIVE_INGR_BHYT_NAME { get;  set;  }
        public string MEDICINE_USE_FORM_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }

        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal AMOUNT_NOI_TRU { get;  set;  }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_KHAC { get; set; }
        public decimal AMOUNT_XUATXA { get; set; }
        public decimal AMOUNT_HPKP { get;  set;  }
       // public decimal AMOUNT_LS { get; set; }
        public Dictionary<string, decimal> DIC_REQ_DEPARTMENT { get; set; }

        public Mrs00328RDO(MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_MEDICINE data)
        {
            try
            {
                if (data != null)
                {
                    this.MEDICINE_TYPE_CODE = data.MEDICINE_TYPE_CODE; 
                    this.MEDICINE_TYPE_NAME = data.MEDICINE_TYPE_NAME; 
                    this.SERVICE_UNIT_NAME = data.SERVICE_UNIT_NAME; 
                    this.IMP_PRICE = data.IMP_PRICE; 
                    this.ACTIVE_INGR_BHYT_CODE = data.ACTIVE_INGR_BHYT_CODE;
                    this.ACTIVE_INGR_BHYT_NAME = data.ACTIVE_INGR_BHYT_NAME;
                    this.DIC_REASON = new Dictionary<string, decimal>();
                    this.DIC_EXP_MEST_TYPE = new Dictionary<string, decimal>();
                    this.DIC_MOBA_IMT = new Dictionary<string, decimal>();
                    this.DIC_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00328RDO(MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MEDICINE data)
        {
            try
            {
                if (data != null)
                {
                    this.MEDICINE_TYPE_CODE = data.MEDICINE_TYPE_CODE;
                    this.MEDICINE_TYPE_NAME = data.MEDICINE_TYPE_NAME;
                    this.SERVICE_UNIT_NAME = data.SERVICE_UNIT_NAME;
                    this.IMP_PRICE = data.IMP_PRICE;
                    this.ACTIVE_INGR_BHYT_CODE = data.ACTIVE_INGR_BHYT_CODE;
                    this.ACTIVE_INGR_BHYT_NAME = data.ACTIVE_INGR_BHYT_NAME;
                    this.DIC_REASON = new Dictionary<string, decimal>();
                    this.DIC_EXP_MEST_TYPE = new Dictionary<string, decimal>();
                    this.DIC_MOBA_IMT = new Dictionary<string, decimal>();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public decimal AMOUNT_XUAT_KHAC { get; set; }
        public decimal AMOUNT_TNCC { get; set; }
        public Dictionary<string, decimal> DIC_REASON { get; set; }
        public Dictionary<string, decimal> DIC_EXP_MEST_TYPE { get; set; }
        public Dictionary<string, decimal> DIC_MOBA_IMT { get; set; }

        public decimal AMOUNT_CLS { get; set; }

        public decimal AMOUNT_LS { get; set; }

        public decimal AMOUNT_KKB { get; set; }

        public decimal AMOUNT_K { get; set; }

        public string PARENT_MEDICINE_TYPE_CODE { get; set; }

        public string PARENT_MEDICINE_TYPE_NAME { get; set; }

        public string MEDICINE_GROUP_CODE { get; set; }

        public string MEDICINE_GROUP_NAME { get; set; }
    }
}
