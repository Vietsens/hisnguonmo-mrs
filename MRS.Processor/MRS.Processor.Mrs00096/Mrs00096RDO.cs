using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00096
{
    class Mrs00096RDO
    {
        public long MEDICINE_BEAN_ID { get;  set;  }
        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        
        public string HEIN_SERVICE_BHYT_NAME { get; set; }

        public long MATERIAL_BEAN_ID { get;  set;  }
        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }
        public string SUPPLIER_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public decimal IMP_PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }

        public long MATY_METY_BEAN_ID { get; set; }
        public string MATY_METY_TYPE_CODE { get; set; }
        public string MATY_METY_TYPE_NAME { get; set; }
        public long? NUM_ORDER { get;  set;  }
        public string TDL_BID_NUMBER { get; set; }// Số QĐ thầu
        public string TDL_BID_GROUP_CODE { get; set; }//Ma nhom cua thuoc trong goi thau
        public string TDL_BID_PACKAGE_CODE { get; set; }//Ma goi cua thuoc trong goi thau
        public string TDL_BID_YEAR { get; set; }// năm thầu
        public string MEDICINE_REGISTER_NUMBER { get; set; }// số đăng ký
        public string MEDI_STOCK_NAME { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public Dictionary<string, decimal> dicMedistock { set; get; }
        public string TYPE { get; set; }
        public decimal IMP_NCC_AMOUNT { get; set; }
        public decimal EXP_NCC_AMOUNT { get; set; }
        public decimal IMP_BU_AMOUNT { get; set; }
        public decimal EXP_THIEU_AMOUNT { get; set; }
        public decimal EXP_CANCEL_AMOUNT { get; set; }
        public decimal EXP_HP_AMOUNT { get; set; }
        public decimal EXP_TREATMENT_NOITRU_BHYT_AMOUNT { get; set; }
        public decimal EXP_TREATMENT_NOITRU_VIENPHI_AMOUNT { get; set; }
        public decimal EXP_TREATMENT_NGOAITRU_BHYT_AMOUNT { get; set; }
        public decimal EXP_TREATMENT_NGOAITRU_VIENPHI_AMOUNT { get; set; }
        public Mrs00096RDO()
        {

        }

        public Mrs00096RDO(V_HIS_MEDICINE_BEAN medicineBean, List<V_HIS_MEDICINE_TYPE> hisMedicineTypes, List<V_HIS_MEDICINE> hisMedicine)
        {
            try
            {
                var medicineType = hisMedicineTypes.FirstOrDefault(o => o.ID == medicineBean.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE();
                var medicine = hisMedicine.FirstOrDefault(o => o.ID == medicineBean.MEDICINE_ID) ?? new V_HIS_MEDICINE();
                MEDICINE_BEAN_ID = medicineBean.ID;
                MATY_METY_BEAN_ID = medicineBean.MEDICINE_TYPE_ID;
                MATY_METY_TYPE_CODE = medicineBean.MEDICINE_TYPE_CODE;
                MATY_METY_TYPE_NAME = medicineBean.MEDICINE_TYPE_NAME;
                MEDICINE_TYPE_ID = medicineBean.MEDICINE_TYPE_ID; 
                MEDICINE_TYPE_CODE = medicineBean.MEDICINE_TYPE_CODE; 
                MEDICINE_TYPE_NAME = medicineBean.MEDICINE_TYPE_NAME;
                ACTIVE_INGR_BHYT_CODE = medicineBean.ACTIVE_INGR_BHYT_CODE;
                ACTIVE_INGR_BHYT_NAME = medicineBean.ACTIVE_INGR_BHYT_NAME;
                HEIN_SERVICE_BHYT_NAME = medicineType.HEIN_SERVICE_BHYT_NAME;
                MANUFACTURER_NAME = medicineBean.MANUFACTURER_NAME;
                NATIONAL_NAME = medicineBean.NATIONAL_NAME;
                SUPPLIER_NAME = medicineBean.SUPPLIER_NAME;
                SERVICE_UNIT_NAME = medicineBean.SERVICE_UNIT_NAME; 
                NUM_ORDER = medicineBean.NUM_ORDER; 
                IMP_PRICE = medicineBean.IMP_PRICE; 
                AMOUNT = medicineBean.AMOUNT;
                CONCENTRA = medicineBean.CONCENTRA;
                TDL_BID_NUMBER = medicineBean.TDL_BID_NUMBER;
                TDL_BID_GROUP_CODE = medicine.TDL_BID_GROUP_CODE;
                TDL_BID_YEAR = medicine.TDL_BID_YEAR;
                TDL_BID_PACKAGE_CODE = medicine.TDL_BID_PACKAGE_CODE;
                MEDICINE_REGISTER_NUMBER = medicine.MEDICINE_REGISTER_NUMBER;
                MEDI_STOCK_NAME = medicineBean.MEDI_STOCK_NAME;
                MEDI_STOCK_CODE = medicineBean.MEDI_STOCK_CODE;
                MEDI_STOCK_ID = medicineBean.MEDI_STOCK_ID??0;
                MEDICINE_USE_FORM_NAME = medicine.MEDICINE_USE_FORM_NAME;
                TYPE = "TH";
                 //dicMedistock.Add(medicineBean.MEDI_STOCK_CODE,medicineBean.AMOUNT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }



        public Mrs00096RDO(V_HIS_MATERIAL_BEAN materialBean, List<V_HIS_MATERIAL_TYPE> hisMaterialTypes,List<V_HIS_MATERIAL> hisMaterial)
        {
            try
            {
                var materialType = hisMaterialTypes.FirstOrDefault(o => o.ID == materialBean.MATERIAL_TYPE_ID)??new V_HIS_MATERIAL_TYPE();
                var material = hisMaterial.FirstOrDefault(o => o.ID == materialBean.MATERIAL_ID) ?? new V_HIS_MATERIAL();
                MATY_METY_BEAN_ID = materialBean.MATERIAL_TYPE_ID;
                MATY_METY_TYPE_CODE = materialBean.MATERIAL_TYPE_CODE;
                MATY_METY_TYPE_NAME = materialBean.MATERIAL_TYPE_NAME;
                MATERIAL_BEAN_ID = materialBean.ID; 
                MATERIAL_TYPE_ID = materialBean.MATERIAL_TYPE_ID; 
                MATERIAL_TYPE_CODE = materialBean.MATERIAL_TYPE_CODE; 
                MATERIAL_TYPE_NAME = materialBean.MATERIAL_TYPE_NAME;
                 HEIN_SERVICE_BHYT_NAME =  materialType.HEIN_SERVICE_BHYT_NAME;
                 MANUFACTURER_NAME = materialBean.MANUFACTURER_NAME;
                 NATIONAL_NAME = materialBean.NATIONAL_NAME;
                 SUPPLIER_NAME = materialBean.SUPPLIER_NAME;
                 SERVICE_UNIT_NAME = materialBean.SERVICE_UNIT_NAME;
                 NUM_ORDER = materialBean.NUM_ORDER;
                 IMP_PRICE = materialBean.IMP_PRICE;
                 AMOUNT = materialBean.AMOUNT;
                 CONCENTRA = materialBean.CONCENTRA;
                 TDL_BID_NUMBER = materialBean.TDL_BID_NUMBER;
                 TDL_BID_GROUP_CODE = material.TDL_BID_GROUP_CODE;
                 TDL_BID_YEAR = material.TDL_BID_YEAR;
                 TDL_BID_PACKAGE_CODE = material.TDL_BID_PACKAGE_CODE;
                 MEDICINE_REGISTER_NUMBER = material.MATERIAL_REGISTER_NUMBER;
                 MEDI_STOCK_NAME = materialBean.MEDI_STOCK_NAME;
                 MEDI_STOCK_CODE = materialBean.MEDI_STOCK_CODE;
                 MEDI_STOCK_ID = materialBean.MEDI_STOCK_ID ?? 0;
                 TYPE = "VT";
                 //dicMedistock.Add(materialBean.MEDI_STOCK_CODE, materialBean.AMOUNT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

       
    }
}
