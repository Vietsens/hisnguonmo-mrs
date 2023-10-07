using MOS.MANAGER.HisDepartment;
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00124
{
    class Mrs00124SDO
    {
        public long DEPARTMENT_ID { get;  set;  }
        public string DEPARTMENT_CODE { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }

        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }

        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public decimal AMOUNT { get;  set;  }

        public Mrs00124SDO()
        {

        }

        public Mrs00124SDO(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicines, long departmentId)
        {
            try
            {
                if (hisExpMestMedicines != null && hisExpMestMedicines.Count > 0)
                {
                    var department = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId); 
                    if (department != null)
                    {
                        DEPARTMENT_ID = department.ID; 
                        DEPARTMENT_CODE = department.DEPARTMENT_CODE; 
                        DEPARTMENT_NAME = department.DEPARTMENT_NAME; 
                    }
                    MEDICINE_TYPE_ID = hisExpMestMedicines.First().MEDICINE_TYPE_ID; 
                    MEDICINE_TYPE_CODE = hisExpMestMedicines.First().MEDICINE_TYPE_CODE; 
                    MEDICINE_TYPE_NAME = hisExpMestMedicines.First().MEDICINE_TYPE_NAME; 
                    AMOUNT = hisExpMestMedicines.Sum(s => s.AMOUNT); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00124SDO(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterials, long departmentId)
        {
            try
            {
                if (hisExpMestMaterials != null && hisExpMestMaterials.Count > 0)
                {
                    var department = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId); 
                    if (department != null)
                    {
                        DEPARTMENT_ID = department.ID; 
                        DEPARTMENT_CODE = department.DEPARTMENT_CODE; 
                        DEPARTMENT_NAME = department.DEPARTMENT_NAME; 
                    }
                    MATERIAL_TYPE_ID = hisExpMestMaterials.First().MATERIAL_TYPE_ID; 
                    MATERIAL_TYPE_CODE = hisExpMestMaterials.First().MATERIAL_TYPE_CODE; 
                    MATERIAL_TYPE_NAME = hisExpMestMaterials.First().MATERIAL_TYPE_NAME; 
                    AMOUNT = hisExpMestMaterials.Sum(s => s.AMOUNT); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00124SDO(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicines, long departmentId)
        {
            try
            {
                if (hisImpMestMedicines != null && hisImpMestMedicines.Count > 0)
                {
                    var department = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId); 
                    if (department != null)
                    {
                        DEPARTMENT_ID = department.ID; 
                        DEPARTMENT_CODE = department.DEPARTMENT_CODE; 
                        DEPARTMENT_NAME = department.DEPARTMENT_NAME; 
                    }
                    MEDICINE_TYPE_ID = hisImpMestMedicines.First().MEDICINE_TYPE_ID; 
                    MEDICINE_TYPE_CODE = hisImpMestMedicines.First().MEDICINE_TYPE_CODE; 
                    MEDICINE_TYPE_NAME = hisImpMestMedicines.First().MEDICINE_TYPE_NAME; 
                    AMOUNT = hisImpMestMedicines.Sum(s => (-s.AMOUNT)); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00124SDO(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterilas, long departmentId)
        {
            try
            {
                if (hisImpMestMaterilas != null && hisImpMestMaterilas.Count > 0)
                {
                    var department = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId); 
                    if (department != null)
                    {
                        DEPARTMENT_ID = department.ID; 
                        DEPARTMENT_CODE = department.DEPARTMENT_CODE; 
                        DEPARTMENT_NAME = department.DEPARTMENT_NAME; 
                    }
                    MATERIAL_TYPE_ID = hisImpMestMaterilas.First().MATERIAL_TYPE_ID; 
                    MATERIAL_TYPE_CODE = hisImpMestMaterilas.First().MATERIAL_TYPE_CODE; 
                    MATERIAL_TYPE_NAME = hisImpMestMaterilas.First().MATERIAL_TYPE_NAME; 
                    AMOUNT = hisImpMestMaterilas.Sum(s => (-s.AMOUNT)); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
