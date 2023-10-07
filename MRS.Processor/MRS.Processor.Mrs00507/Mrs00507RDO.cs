using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00507
{
    public class Mrs00507RDO
    {

        public Mrs00507RDO(V_HIS_EXP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, Dictionary<string, HIS_BID_MATERIAL_TYPE> dicBidMaterialType)
        {
            this.BID_NUM_ORDER = dicBidMaterialType[r.MATERIAL_TYPE_ID + "_" + (r.BID_ID ?? 0)].BID_NUM_ORDER;
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.MANUFACTURER_NAME = dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID) ? dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME : "";
            this.PRICE = Math.Round((r.PRICE ?? (r.IMP_PRICE * (1 + r.IMP_VAT_RATIO))) * (1 + (r.VAT_RATIO ?? 0)), 0);
            this.AMOUNT = r.AMOUNT;
            this.VIR_TOTAL_PRICE = r.AMOUNT * this.PRICE;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.PACKING_TYPE_NAME = dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID) ? dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME : "";
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
        }

        public Mrs00507RDO(V_HIS_EXP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, Dictionary<string, HIS_BID_MEDICINE_TYPE> dicBidMedicineType)
        {
            this.BID_NUM_ORDER = dicBidMedicineType[r.MEDICINE_TYPE_ID + "_" + (r.BID_ID ?? 0)].BID_NUM_ORDER;
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.MANUFACTURER_NAME = dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID) ? dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME : "";
            this.PRICE = Math.Round((r.PRICE ?? (r.IMP_PRICE * (1 + r.IMP_VAT_RATIO))) * (1 + (r.VAT_RATIO ?? 0)), 0);
            this.AMOUNT = r.AMOUNT;
            this.VIR_TOTAL_PRICE = r.AMOUNT * this.PRICE;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.PACKING_TYPE_NAME = dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID) ? dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME : "";
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
        }
        public Mrs00507RDO(V_HIS_EXP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType)
        {
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.MANUFACTURER_NAME = dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID) ? dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME : "";
            this.PRICE = Math.Round((r.PRICE ?? (r.IMP_PRICE * (1 + r.IMP_VAT_RATIO))) * (1 + (r.VAT_RATIO ?? 0)), 0);
            this.AMOUNT = r.AMOUNT;
            this.VIR_TOTAL_PRICE = r.AMOUNT * this.PRICE;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.PACKING_TYPE_NAME = dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID) ? dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME : "";
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
        }

        public Mrs00507RDO(V_HIS_EXP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType)
        {
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.MANUFACTURER_NAME = dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID) ? dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME : "";
            this.PRICE = Math.Round((r.PRICE ?? (r.IMP_PRICE * (1 + r.IMP_VAT_RATIO))) * (1 + (r.VAT_RATIO ?? 0)), 0);
            this.AMOUNT = r.AMOUNT;
            this.VIR_TOTAL_PRICE = r.AMOUNT * this.PRICE;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.PACKING_TYPE_NAME = dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID) ? dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME : "";
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
        }

        public Mrs00507RDO()
        {
            // TODO: Complete member initialization
        }
        
        public string BID_NUM_ORDER { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public Decimal PRICE { get; set; }
        public Decimal AMOUNT { get; set; }
        public Decimal VIR_TOTAL_PRICE { get; set; }

    }
}
