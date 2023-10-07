using Inventec.Common.Repository; 
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Reflection; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00488
{
    class Mrs00488RDO: V_HIS_EXP_MEST_MEDICINE
    {
        public string CONCENTRA { get;  set;  }
        public Mrs00488RDO() { }
        public Mrs00488RDO(V_HIS_EXP_MEST_MEDICINE data,Dictionary<long,HIS_MEDICINE_TYPE> dicMedicineType)
        {
            PropertyInfo[] p = Properties.Get<V_HIS_EXP_MEST_MEDICINE>(); 
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(data)); 
            }
            this.CONCENTRA = dicMedicineType.ContainsKey(data.MEDICINE_TYPE_ID) ? dicMedicineType[data.MEDICINE_TYPE_ID].CONCENTRA : ""; 
        }
    }
}