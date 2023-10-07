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

namespace MRS.Processor.Mrs00163
{
    class Mrs00163RDO
    {
        public long MEDICINE_ID { get;  set;  }
        public long MEDICINE_BEAN_ID { get;  set;  }
        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }

        public long MATERIAL_ID { get;  set;  }
        public long MATERIAL_BEAN_ID { get;  set;  }
        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public string ACTIVE_INGR_BHYT_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal? IMP_PRICE { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string EXPIRED_DATE_STR { get;  set;  }
        public string MEDI_STOCK_AMOUNT { get;  set;  }
        public decimal TOTAL_AMOUNT { get;  set;  }
        public long? EXPIRED_DATE { get;  set;  }

        public long? MEDI_STOCK_ID { get;  set;  }
        public string MEDI_STOCK_NAME { get;  set;  }

        public long? NUM_ORDER { get;  set;  }

        public Mrs00163RDO()
        {
        }

        public Mrs00163RDO(V_HIS_MEDICINE_BEAN medicineBean, ref List<V_HIS_MEDICINE_TYPE> hisMedicineTypes)
        {
            try
            {
                if (medicineBean != null)
                {
                    MEDICINE_ID = medicineBean.MEDICINE_ID; 
                    MEDICINE_BEAN_ID = medicineBean.ID; 
                    MEDICINE_TYPE_ID = medicineBean.MEDICINE_TYPE_ID; 
                    MEDICINE_TYPE_CODE = medicineBean.MEDICINE_TYPE_CODE; 
                    MEDICINE_TYPE_NAME = medicineBean.MEDICINE_TYPE_NAME; 
                    SERVICE_UNIT_NAME = medicineBean.SERVICE_UNIT_NAME; 
                    IMP_PRICE = medicineBean.IMP_PRICE; 
                    PACKAGE_NUMBER = medicineBean.PACKAGE_NUMBER; 
                    MEDI_STOCK_ID = medicineBean.MEDI_STOCK_ID; 
                    MEDI_STOCK_NAME = medicineBean.MEDI_STOCK_NAME; 
                    TOTAL_AMOUNT = medicineBean.AMOUNT; 
                    NUM_ORDER = medicineBean.NUM_ORDER; 
                    if (medicineBean.EXPIRED_DATE.HasValue)
                    {
                        EXPIRED_DATE = Convert.ToInt64(medicineBean.EXPIRED_DATE.Value.ToString().Substring(0, 8)); 
                        EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Convert.ToInt64(medicineBean.EXPIRED_DATE.Value)); 
                    }
                    SetActiveIngrName(medicineBean.MEDICINE_TYPE_ID, hisMedicineTypes); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00163RDO(V_HIS_MATERIAL_BEAN materialBean, ref List<V_HIS_MATERIAL_TYPE> hisMaterialTypes)
        {
            try
            {
                if (materialBean != null)
                {
                    MATERIAL_ID = materialBean.MATERIAL_ID; 
                    MATERIAL_BEAN_ID = materialBean.ID; 
                    MATERIAL_TYPE_ID = materialBean.MATERIAL_TYPE_ID; 
                    MATERIAL_TYPE_CODE = materialBean.MATERIAL_TYPE_CODE; 
                    MATERIAL_TYPE_NAME = materialBean.MATERIAL_TYPE_NAME; 
                    SERVICE_UNIT_NAME = materialBean.SERVICE_UNIT_NAME; 
                    IMP_PRICE = materialBean.IMP_PRICE; 
                    PACKAGE_NUMBER = materialBean.PACKAGE_NUMBER; 
                    MEDI_STOCK_ID = materialBean.MEDI_STOCK_ID; 
                    MEDI_STOCK_NAME = materialBean.MEDI_STOCK_NAME; 
                    TOTAL_AMOUNT = materialBean.AMOUNT; 
                    NUM_ORDER = materialBean.NUM_ORDER; 
                    if (materialBean.EXPIRED_DATE.HasValue)
                    {
                        EXPIRED_DATE = Convert.ToInt64(materialBean.EXPIRED_DATE.Value.ToString().Substring(0, 8)); 
                        EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Convert.ToInt64(materialBean.EXPIRED_DATE.Value)); 
                    }
                    SetActiveIngrName(materialBean.MATERIAL_TYPE_ID, hisMaterialTypes); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetActiveIngrName(long medicineTypeId, List<V_HIS_MEDICINE_TYPE> hisMedicineTypes)
        {
            try
            {
                if (medicineTypeId > 0)
                {
                    var medicineType = hisMedicineTypes.SingleOrDefault(o => o.ID == medicineTypeId); 
                    if (medicineType != null)
                    {
                        ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME; 
                    }
                    //else
                    //{
                    //    medicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager().GetViewById(medicineTypeId); 
                    //    if (medicineType != null)
                    //    {
                    //        ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME; 
                    //        hisMedicineTypes.Add(medicineType); 
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetActiveIngrName(long materialTypeId, List<V_HIS_MATERIAL_TYPE> hisMaterialTypes)
        {
            try
            {
                //if (materialTypeId > 0)
                //{
                //    var materialType = hisMaterialTypes.SingleOrDefault(o => o.ID == materialTypeId); 
                //    if (materialType != null)
                //    {
                //        ACTIVE_INGR_BHYT_NAME = materialType.ACC; 
                //    }
                //    else
                //    {
                //        materialType = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager().GetViewById(materialTypeId); 
                //        if (materialType != null)
                //        {
                //            ACTIVE_INGR_BHYT_NAME = materialType.TDL_HEIN_SERVICE_BHYT_NAME; 
                //            hisMaterialTypes.Add(materialType); 
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
