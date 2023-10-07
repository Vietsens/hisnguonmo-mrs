using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using System.Reflection;
using MRS.MANAGER.Config; 
namespace MRS.Processor.Mrs00381
{
    public class Mrs00381RDO: V_HIS_EXP_MEST_MEDICINE
    {
        public long SERVICE_STOCK_ID { get; set; }
        public string SERVICE_STOCK_NAME { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public long IMP_MEST_ID { get; set; }
        public string IMP_TIME_STR { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string IMP_MEST_SUB_CODE { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public decimal TOTAL_IMP_PRICE { get; set; }

        public Mrs00381RDO(V_HIS_EXP_MEST_MEDICINE exp, HIS_EXP_MEST chmsexp, HIS_IMP_MEST imp, HIS_MEDICINE_TYPE type)
        {
            PropertyInfo[] pi = typeof(V_HIS_EXP_MEST_MEDICINE).GetProperties(); 
            foreach (var p in pi)
            {
                p.SetValue(this, p.GetValue(exp));  
            }
            SetExtentField(this, chmsexp, imp, type); 
        }

        private void SetExtentField(V_HIS_EXP_MEST_MEDICINE exp, HIS_EXP_MEST chmsexp, HIS_IMP_MEST imp, HIS_MEDICINE_TYPE type)
        {
            try
            {
                if (chmsexp == null) chmsexp = new HIS_EXP_MEST();
                if (imp == null) imp = new HIS_IMP_MEST();
                if (type == null) type = new HIS_MEDICINE_TYPE();
                SERVICE_STOCK_ID = chmsexp.IMP_MEDI_STOCK_ID ?? 0;
                SERVICE_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == (chmsexp.IMP_MEDI_STOCK_ID ?? 0)) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == (chmsexp.MEDI_STOCK_ID)) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                IMP_MEST_ID = imp.ID;
                IMP_MEST_CODE = imp.IMP_MEST_CODE;
                IMP_MEST_SUB_CODE = imp.IMP_MEST_SUB_CODE;
                IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(imp.IMP_TIME ?? 0);
                PACKING_TYPE_NAME = type.PACKING_TYPE_NAME;
                SERVICE_ID = exp.MEDICINE_ID ?? 0; 
                SERVICE_TYPE_CODE = exp.MEDICINE_TYPE_CODE; 
                SERVICE_TYPE_NAME = exp.MEDICINE_TYPE_NAME;
                IMP_PRICE = IMP_PRICE * (1 + IMP_VAT_RATIO);
                TOTAL_IMP_PRICE = exp.IMP_PRICE*exp.AMOUNT; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
       
        public Mrs00381RDO() { }

    }
    public class Mrs00381RDO_ : V_HIS_EXP_MEST_MATERIAL
    {
        public long SERVICE_STOCK_ID { get;  set;  }
        public string SERVICE_STOCK_NAME { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public long IMP_MEST_ID { get; set; }
        public string IMP_TIME_STR { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string IMP_MEST_SUB_CODE { get; set; }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public decimal TOTAL_IMP_PRICE { get;  set;  }
        public Mrs00381RDO_(V_HIS_EXP_MEST_MATERIAL exp, HIS_EXP_MEST chmsexp, HIS_IMP_MEST imp, HIS_MATERIAL_TYPE type)
        {
            PropertyInfo[] pi = typeof(V_HIS_EXP_MEST_MATERIAL).GetProperties(); 
            foreach (var p in pi)
            {
                p.SetValue(this, p.GetValue(exp));  
            }
            SetExtentField(this, chmsexp, imp, type); 
        }

        private void SetExtentField(V_HIS_EXP_MEST_MATERIAL exp, HIS_EXP_MEST chmsexp, HIS_IMP_MEST imp, HIS_MATERIAL_TYPE type)
        {
            try
            {
                if (chmsexp == null) chmsexp = new HIS_EXP_MEST();
                if (imp == null) imp = new HIS_IMP_MEST();
                if (type == null) type = new HIS_MATERIAL_TYPE();
                SERVICE_STOCK_ID = chmsexp.IMP_MEDI_STOCK_ID ?? 0;
                SERVICE_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == (chmsexp.IMP_MEDI_STOCK_ID ?? 0)) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == (chmsexp.MEDI_STOCK_ID)) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                IMP_MEST_ID = imp.ID;
                IMP_MEST_CODE = imp.IMP_MEST_CODE;
                IMP_MEST_SUB_CODE = imp.IMP_MEST_SUB_CODE;
                IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(imp.IMP_TIME ?? 0);
                PACKING_TYPE_NAME = type.PACKING_TYPE_NAME;
                SERVICE_ID = exp.MATERIAL_ID ?? 0; 
                SERVICE_TYPE_CODE = exp.MATERIAL_TYPE_CODE;
                SERVICE_TYPE_NAME = exp.MATERIAL_TYPE_NAME;
                IMP_PRICE = IMP_PRICE * (1 + IMP_VAT_RATIO);
                TOTAL_IMP_PRICE = exp.IMP_PRICE * exp.AMOUNT; 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
        public Mrs00381RDO_() { }

    }
}
