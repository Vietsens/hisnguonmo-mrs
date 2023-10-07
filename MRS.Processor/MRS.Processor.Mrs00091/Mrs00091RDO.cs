using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Proccessor.Mrs00091
{
    public class Mrs00091RDO 
    {
        public long MATERIAL_ID { get;  set;  }
        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public long MEDICINE_ID { get;  set;  }
        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }

        public long? NUM_ORDER { get;  set;  }

        public long? SUPPLIER_ID { get;  set;  }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string EXPIRED_STR { get;  set;  }

        public Mrs00091RDO()
        {

        }

        internal void SetMediConcentraAndEndAmount(List<V_HIS_MEDICINE_TYPE> ListMedicineType, ref CommonParam paramGet)
        {
            try
            {
                END_AMOUNT = BEGIN_AMOUNT + IMP_AMOUNT - EXP_AMOUNT; 
                V_HIS_MEDICINE_TYPE medicineType = ListMedicineType.FirstOrDefault(f => f.ID == MEDICINE_TYPE_ID); 
                if (medicineType != null)
                {
                    CONCENTRA = medicineType.CONCENTRA; 
                }
                else
                {
                    medicineType = new HisMedicineTypeManager(paramGet).GetViewById(MEDICINE_TYPE_ID); 
                    if (medicineType != null)
                    {
                        CONCENTRA = medicineType.CONCENTRA; 
                        ListMedicineType.Add(medicineType); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        internal void SetMateConcentraAndEndAmount(List<V_HIS_MATERIAL_TYPE> ListMaterialType, ref CommonParam paramGet)
        {
            try
            {
                END_AMOUNT = BEGIN_AMOUNT + IMP_AMOUNT - EXP_AMOUNT; 
                V_HIS_MATERIAL_TYPE materialType = ListMaterialType.FirstOrDefault(f => f.ID == MATERIAL_TYPE_ID); 
                if (materialType != null)
                {
                    CONCENTRA = materialType.CONCENTRA; 
                }
                else
                {
                    materialType = new HisMaterialTypeManager(paramGet).GetViewById(MATERIAL_TYPE_ID); 
                    if (materialType != null)
                    {
                        CONCENTRA = materialType.CONCENTRA; 
                        ListMaterialType.Add(materialType); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
