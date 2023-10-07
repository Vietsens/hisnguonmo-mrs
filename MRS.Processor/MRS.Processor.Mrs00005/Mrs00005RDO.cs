using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00005
{
    class Mrs00005RDO
    {
        public string TYPE { get; set; }
        public V_HIS_IMP_MEST_MEDICINE V_HIS_IMP_MEST_MEDICINE { get; set; }
        public V_HIS_IMP_MEST_MATERIAL V_HIS_IMP_MEST_MATERIAL { get; set; }
        public long IMP_MEST_ID { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string IMP_MEST_SUB_CODE { get; set; }
        public string NAME { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public long? EXPIRED_DATE { get;  set;  }
        public string EXPIRED_DATE_STR { get;  set;  }
        public decimal? IMP_PRICE { get;  set;  }
        public decimal? IMP_VAT_100 { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public long? IMP_TIME { get;  set;  }
        public string IMP_DATE_STR { get;  set;  }

        public string BID_NUMBER { get;  set;  }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }
        public string MANUFACTURER_CODE { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }
        public long? NUM_ORDER { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public string REGISTER_NUMBER { get;  set;  }
        public string SERVICE_UNIT_CODE { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal? VAT_RATIO { get;  set;  }

        public Mrs00005RDO(V_HIS_IMP_MEST_MATERIAL imp, List<HIS_MATERIAL> _listMaterial, List<HIS_IMP_MEST> _listImpMest)
        {
            try
            {
                V_HIS_IMP_MEST_MATERIAL = imp;
                V_HIS_IMP_MEST_MEDICINE = new MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MEDICINE();
                IMP_MEST_ID = imp.IMP_MEST_ID;
                IMP_MEST_CODE = imp.IMP_MEST_CODE; 
                NAME = imp.MATERIAL_TYPE_NAME; 
                AMOUNT = imp.AMOUNT; 
                IMP_PRICE = imp.IMP_PRICE; 
                IMP_VAT_100 = imp.IMP_VAT_RATIO * 100;
                TOTAL_PRICE = imp.AMOUNT * (imp.PRICE ?? 0) * (1 + (imp.VAT_RATIO ?? 0));
                TOTAL_VAT = imp.AMOUNT * (imp.PRICE ?? 0) * ((imp.VAT_RATIO ?? 0));
                NATIONAL_NAME = imp.NATIONAL_NAME; 
                MANUFACTURER_NAME = imp.MANUFACTURER_NAME; 
                IMP_TIME = imp.IMP_TIME; 
                SERVICE_UNIT_NAME = imp.SERVICE_UNIT_NAME; 

                BID_NUMBER = imp.BID_NUMBER; 
                SUPPLIER_CODE = imp.SUPPLIER_CODE; 
                SUPPLIER_NAME = imp.SUPPLIER_NAME; 
                MANUFACTURER_CODE = imp.MANUFACTURER_CODE; 
                MANUFACTURER_NAME = imp.MANUFACTURER_NAME; 
                NUM_ORDER = imp.NUM_ORDER; 
                PRICE = imp.PRICE; 
                //REGISTER_NUMBER = imp.REGISTER_NUMBER; 
                SERVICE_UNIT_CODE = imp.SERVICE_UNIT_CODE; 
                SERVICE_UNIT_NAME = imp.SERVICE_UNIT_NAME; 
                VAT_RATIO = imp.VAT_RATIO;
                EXPIRED_DATE = imp.EXPIRED_DATE;

                if (_listImpMest != null)
                {
                    var impMest = _listImpMest.FirstOrDefault(o => o.ID == imp.IMP_MEST_ID);
                    if (impMest != null)
                    {
                        this.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                        this.DESCRIPTION = impMest.DESCRIPTION;
                        this.MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        this.MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                        this.DOCUMENT_DATE = impMest.DOCUMENT_DATE;
                        this.MEDICAL_CONTRACT_ID = impMest.MEDICAL_CONTRACT_ID;
                    }
                }
                if (_listMaterial != null)
                {
                    var m = _listMaterial.FirstOrDefault(o => o.ID == imp.MATERIAL_ID);
                    if (m != null)
                    {
                        this.TDL_BID_NUMBER = m.TDL_BID_NUMBER;
                    }
                }
                IMP_MEST_TYPE_ID = imp.IMP_MEST_TYPE_ID;
                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        public Mrs00005RDO(V_HIS_IMP_MEST_MEDICINE imp,List<HIS_MEDICINE> _listMedicine, List<HIS_IMP_MEST> _listImpMest)
        {
            try
            {
                V_HIS_IMP_MEST_MEDICINE = imp;
                V_HIS_IMP_MEST_MATERIAL = new MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MATERIAL();
                IMP_MEST_ID = imp.IMP_MEST_ID;
                IMP_MEST_CODE = imp.IMP_MEST_CODE; 
                NAME = imp.MEDICINE_TYPE_NAME; 
                AMOUNT = imp.AMOUNT; 
                IMP_PRICE = imp.IMP_PRICE;
                IMP_VAT_100 = imp.IMP_VAT_RATIO * 100;
                TOTAL_PRICE = imp.AMOUNT * (imp.PRICE ?? 0) * (1 + (imp.VAT_RATIO ?? 0));
                TOTAL_VAT = imp.AMOUNT * (imp.PRICE ?? 0) * ((imp.VAT_RATIO ?? 0));
                NATIONAL_NAME = imp.NATIONAL_NAME; 
                MANUFACTURER_NAME = imp.MANUFACTURER_NAME; 
                IMP_TIME = imp.IMP_TIME; 
                PACKAGE_NUMBER = imp.PACKAGE_NUMBER; 
                SERVICE_UNIT_NAME = imp.SERVICE_UNIT_NAME; 

                BID_NUMBER = imp.BID_NUMBER; 
                SUPPLIER_CODE = imp.SUPPLIER_CODE; 
                SUPPLIER_NAME = imp.SUPPLIER_NAME; 
                MANUFACTURER_CODE = imp.MANUFACTURER_CODE; 
                MANUFACTURER_NAME = imp.MANUFACTURER_NAME; 
                NUM_ORDER = imp.NUM_ORDER; 
                PRICE = imp.PRICE; 
                REGISTER_NUMBER = imp.REGISTER_NUMBER; 
                SERVICE_UNIT_CODE = imp.SERVICE_UNIT_CODE; 
                SERVICE_UNIT_NAME = imp.SERVICE_UNIT_NAME; 
                VAT_RATIO = imp.VAT_RATIO;
                EXPIRED_DATE = imp.EXPIRED_DATE;
                if (_listImpMest != null)
                {
                    var impMest = _listImpMest.FirstOrDefault(o => o.ID == imp.IMP_MEST_ID);
                    if (impMest != null)
                    {
                        this.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                        this.DESCRIPTION = impMest.DESCRIPTION;
                        this.MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        this.MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                        this.MEDICAL_CONTRACT_ID = impMest.MEDICAL_CONTRACT_ID;
                    }
                }
                if (_listMedicine != null)
                {
                    var m = _listMedicine.FirstOrDefault(o => o.ID == imp.MEDICINE_ID);
                    if (m != null)
                    {
                        this.TDL_BID_NUMBER = m.TDL_BID_NUMBER;
                    }
                }
                IMP_MEST_TYPE_ID = imp.IMP_MEST_TYPE_ID;
                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00005RDO data)
        {
            EXPIRED_DATE_STR = RDOCommon.ConvertTimeToDateString(data.EXPIRED_DATE); 
            IMP_DATE_STR = RDOCommon.ConvertTimeToDateString(data.IMP_TIME); 
        }

        public string DESCRIPTION { get; set; }

        public string MEDI_STOCK_CODE { get; set; }

        public string MEDI_STOCK_NAME { get; set; }

        public string TDL_BID_NUMBER { get; set; }

        public long IMP_MEST_TYPE_ID { get; set; }

        public string IMP_MEST_TYPE_CODE { get; set; }

        public string IMP_MEST_TYPE_NAME { get; set; }

        public long? DOCUMENT_DATE { get; set; }

        public long? MEDICAL_CONTRACT_ID { get; set; }

        public string MEDICAL_CONTRACT_CODE { get; set; }

        public string MEDICAL_CONTRACT_NAME { get; set; }

        public decimal TOTAL_VAT { get; set; }
    }
}
