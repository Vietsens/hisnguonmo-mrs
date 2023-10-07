using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00584
{
    class Mrs00584RDO : IMP_EXP_MEST_TYPE
    {
        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public int SERVICE_TYPE_ID { get; set; }
        public long MEDICINE_ID { get; set; }
        public long MATERIAL_ID { get; set; }
        public long SERVICE_ID { get; set; }

        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public decimal BEGIN_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }
        public string MEDICINE_LINE_CODE { get; set; }
        public string MEDICINE_LINE_NAME { get; set; }

        public long? MEDICINE_GROUP_ID { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }

        //public long? MEMA_GROUP_ID { get; set; }
        //public string MEMA_GROUP_CODE { get; set; }
        //public string MEMA_GROUP_NAME { get; set; }
        public long? PARENT_MEDICINE_TYPE_ID { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }

        public string PACKAGE_NUMBER { get; set; }//Số lô
        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public decimal IMP_TOTAL_AMOUNT { get; set; }
        public decimal EXP_TOTAL_AMOUNT { get; set; }

        public decimal? VAT_RATIO { get; set; }
        public string VAT_RATIO_STR { get; set; }
        public string CONCENTRA { get; set; }//Hàm lượng
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }//Biệt dược
        public string NATIONAL_NAME { get; set; }//Quốc gia
        public string MANUFACTURER_NAME { get; set; }//Hãng sx
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string PACKING_TYPE_NAME { get; set; }//Quy cách đóng gói
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string BID_GROUP_CODE { get; set; }//Mã nhóm thầu
        public string BID_NUMBER { get; set; }//Số thầu
        public string BID_NUM_ORDER { get; set; }//Số thứ tự thầu
        public decimal INPUT_END_AMOUNT { get; set; }//Số lượng thực tế khi kiểm kê
        public string SCIENTIFIC_NAME { get; set; } // tên khoa học
        public string PREPROCESSING { get; set; } // sơ chế
        public string PROCESSING { get; set; } //phức chế
        public string USED_PART { get; set; }// bộ phận dùng điều chế
        public string DOSAGE_FORM { get; set; } // dạng bào chế
        public string DISTRIBUTED_SL { get; set; } // số lượng
        

        public Dictionary<string, decimal> DIC_EXP_MEST_REASON { get; set; }

        public bool IS_FROM_SUPPLIER { get; set; }//Là nhập từ nhà cung cấp

        public decimal EXP_BH_AMOUNT { get; set; }
        public decimal EXP_ND_AMOUNT { get; set; }

          public Mrs00584RDO(V_HIS_IMP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines)
        {
            this.SERVICE_TYPE_NAME = "Thuốc";
            this.SERVICE_TYPE_ID = 1; 
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDICINE_ID = r.MEDICINE_ID; 
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER; 
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.SERVICE_ID = r.SERVICE_ID; 
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE; 
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME; 
            this.IMP_PRICE = r.IMP_PRICE*(1+r.IMP_VAT_RATIO); 
            this.VAT_RATIO = r.IMP_VAT_RATIO; 
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME; 
            if (dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.CONCENTRA = dicMedicineType[r.MEDICINE_TYPE_ID].CONCENTRA; 
                this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME; 
                this.MANUFACTURER_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME; 
                this.HEIN_SERVICE_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = dicMedicineType[r.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                this.SCIENTIFIC_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].SCIENTIFIC_NAME;
                this.PREPROCESSING = dicMedicineType[r.MEDICINE_TYPE_ID].PREPROCESSING;
                this.PROCESSING = dicMedicineType[r.MEDICINE_TYPE_ID].PROCESSING;
                this.USED_PART = dicMedicineType[r.MEDICINE_TYPE_ID].USED_PART;
                this.DOSAGE_FORM = dicMedicineType[r.MEDICINE_TYPE_ID].DOSAGE_FORM;
                this.DISTRIBUTED_SL = dicMedicineType[r.MEDICINE_TYPE_ID].DISTRIBUTED_AMOUNT;
           }
            this.NATIONAL_NAME = r.NATIONAL_NAME; 
            this.SUPPLIER_ID = r.SUPPLIER_ID; 
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            this.BEGIN_AMOUNT = r.AMOUNT;
            this.END_AMOUNT = r.AMOUNT;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.BID_NUMBER = Medicine.TDL_BID_NUMBER;
            
            if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
            {
                this.IS_FROM_SUPPLIER = true;
            }
        }

        public Mrs00584RDO(V_HIS_IMP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials)
          {
              this.SERVICE_TYPE_NAME = "Vật tư";
              this.SERVICE_TYPE_ID = 2; 
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MATERIAL_ID = r.MATERIAL_ID; 
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER; 
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.SERVICE_ID = r.SERVICE_ID; 
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE; 
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME; 
            this.IMP_PRICE = r.IMP_PRICE*(1+r.IMP_VAT_RATIO);
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.BEGIN_AMOUNT = r.AMOUNT;
            this.END_AMOUNT = r.AMOUNT; 
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME; 
            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA; 
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME; 
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
            }
            this.NATIONAL_NAME = r.NATIONAL_NAME; 
            this.SUPPLIER_ID = r.SUPPLIER_ID; 
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.BID_NUMBER = Material.TDL_BID_NUMBER;
            if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
            {
                this.IS_FROM_SUPPLIER = true;
            }
        }

        public Mrs00584RDO(Mrs00584RDO r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00584RDO>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
            //this.TYPE = THUOC;
            this.SERVICE_TYPE_NAME = "Thuốc";
            this.SERVICE_TYPE_ID = 1; 

            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID);
            if (Medicine != null)
            {
                this.PACKAGE_NUMBER = Medicine.PACKAGE_NUMBER;
                this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Medicine.EXPIRED_DATE ?? 0);
                this.SERVICE_ID = Medicine.TDL_SERVICE_ID;
                this.IMP_PRICE = Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO);
                this.VAT_RATIO = Medicine.IMP_VAT_RATIO;
                if (dicMedicineType.ContainsKey(Medicine.MEDICINE_TYPE_ID))
                {
                    this.SERVICE_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE;
                    this.SERVICE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME;
                    this.SERVICE_UNIT_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME;
                    this.CONCENTRA = dicMedicineType[Medicine.MEDICINE_TYPE_ID].CONCENTRA;
                    this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                    this.MEDICINE_TYPE_USE_FORM_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME; 
                    this.MANUFACTURER_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                    this.HEIN_SERVICE_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                    this.HEIN_SERVICE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    this.ACTIVE_INGR_BHYT_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                    this.ACTIVE_INGR_BHYT_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                    this.PACKING_TYPE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                    this.REGISTER_NUMBER = dicMedicineType[Medicine.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                    this.NATIONAL_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].NATIONAL_NAME;
                }

                this.SUPPLIER_ID = Medicine.SUPPLIER_ID;
                this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
                this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
                this.BID_NUMBER = Medicine.TDL_BID_NUMBER;
            }
        }

        public Mrs00584RDO(Mrs00584RDO r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00584RDO>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
            //this.TYPE = THUOC;
            this.SERVICE_TYPE_NAME = "Vật tư";
            this.SERVICE_TYPE_ID = 2;

            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID);
            if (Material != null)
            {
                this.PACKAGE_NUMBER = Material.PACKAGE_NUMBER;
                this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Material.EXPIRED_DATE ?? 0);
                this.SERVICE_ID = Material.TDL_SERVICE_ID;
                this.IMP_PRICE = Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO);
                this.VAT_RATIO = Material.IMP_VAT_RATIO;
                if (dicMaterialType.ContainsKey(Material.MATERIAL_TYPE_ID))
                {
                    this.SERVICE_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE;
                    this.SERVICE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME;
                    this.SERVICE_UNIT_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME;
                    this.CONCENTRA = dicMaterialType[Material.MATERIAL_TYPE_ID].CONCENTRA;
                    //this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_PROPRIETARY_NAME;
                    this.MANUFACTURER_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                    this.HEIN_SERVICE_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                    this.HEIN_SERVICE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    //this.ACTIVE_INGR_BHYT_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                    //this.ACTIVE_INGR_BHYT_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                    this.PACKING_TYPE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
                    //this.REGISTER_NUMBER = dicMaterialType[Material.MATERIAL_TYPE_ID].REGISTER_NUMBER;
                    this.NATIONAL_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].NATIONAL_NAME;
                }

                this.SUPPLIER_ID = Material.SUPPLIER_ID;
                this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
                this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
                this.BID_NUMBER = Material.TDL_BID_NUMBER;
            }
        }

        public Mrs00584RDO(V_HIS_IMP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, Dictionary<long, PropertyInfo> dicImpAmountType, List<HIS_MEDICINE> Medicines)
        {
            this.SERVICE_TYPE_NAME = "Thuốc";
            this.SERVICE_TYPE_ID = 1; 
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDICINE_ID = r.MEDICINE_ID; 
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.END_AMOUNT = r.AMOUNT; 
            this.IMP_TOTAL_AMOUNT = r.AMOUNT; 
            if (dicImpAmountType.ContainsKey(r.IMP_MEST_TYPE_ID))
                dicImpAmountType[r.IMP_MEST_TYPE_ID].SetValue(this, r.AMOUNT); 

            this.SERVICE_ID = r.SERVICE_ID; 
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE; 
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME; 
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME; 
            this.VAT_RATIO = r.IMP_VAT_RATIO; 
            if (dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.CONCENTRA = dicMedicineType[r.MEDICINE_TYPE_ID].CONCENTRA; 
                this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                this.MEDICINE_TYPE_USE_FORM_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME; 
                this.MANUFACTURER_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME; 
                this.HEIN_SERVICE_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = dicMedicineType[r.MEDICINE_TYPE_ID].REGISTER_NUMBER; 
            }
            this.NATIONAL_NAME = r.NATIONAL_NAME; 
            this.SUPPLIER_ID = r.SUPPLIER_ID; 
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
            {
                this.IS_FROM_SUPPLIER = true;
            }
        }

        public Mrs00584RDO(V_HIS_IMP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, Dictionary<long, PropertyInfo> dicImpAmountType, List<HIS_MATERIAL> Materials)
        {
            this.SERVICE_TYPE_NAME = "Vật tư";
            this.SERVICE_TYPE_ID = 2; 
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MATERIAL_ID = r.MATERIAL_ID; 
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.IMP_PRICE = r.IMP_PRICE*(1+r.IMP_VAT_RATIO);
            this.IMP_TOTAL_AMOUNT = r.AMOUNT;
            this.END_AMOUNT = r.AMOUNT; 
            if (dicImpAmountType.ContainsKey(r.IMP_MEST_TYPE_ID))
                dicImpAmountType[r.IMP_MEST_TYPE_ID].SetValue(this, r.AMOUNT); 

            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA; 
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME; 
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
            }
            this.SERVICE_ID = r.SERVICE_ID; 
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE; 
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME; 
            this.VAT_RATIO = r.IMP_VAT_RATIO; 
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME; 
            this.NATIONAL_NAME = r.NATIONAL_NAME; 
            this.SUPPLIER_ID = r.SUPPLIER_ID; 
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.BID_NUMBER = Material.TDL_BID_NUMBER;
            if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
            {
                this.IS_FROM_SUPPLIER = true;
            }
            
        }
        public Mrs00584RDO(V_HIS_EXP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, Dictionary<long, PropertyInfo> dicExpAmountType, List<HIS_MEDICINE> Medicines, List<ExpMestIdReason> ExpMestIdReasons)
        {
            this.SERVICE_TYPE_NAME = "Thuốc";
            this.SERVICE_TYPE_ID = 1;  
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDICINE_ID = r.MEDICINE_ID ?? 0; 
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.IMP_PRICE = r.IMP_PRICE*(1+r.IMP_VAT_RATIO); 
            this.SERVICE_ID = r.SERVICE_ID; 
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE; 
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME; 
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.EXP_TOTAL_AMOUNT = r.AMOUNT;
            
            this.END_AMOUNT = -r.AMOUNT; 
            if (dicExpAmountType.ContainsKey(r.EXP_MEST_TYPE_ID))
                dicExpAmountType[r.EXP_MEST_TYPE_ID].SetValue(this, r.AMOUNT); 

            if (dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.CONCENTRA = dicMedicineType[r.MEDICINE_TYPE_ID].CONCENTRA; 
                this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME; 
                this.MANUFACTURER_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                this.MEDICINE_TYPE_USE_FORM_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME; 
                this.HEIN_SERVICE_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = dicMedicineType[r.MEDICINE_TYPE_ID].REGISTER_NUMBER;
            }
            if (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == r.MEDI_STOCK_ID).MEDI_STOCK_NAME.ToLower().Contains("kho thuốc chính"))
            {
                if (r.CK_IMP_MEST_MEDICINE_ID != null)
                {
                    if (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == r.CK_IMP_MEST_MEDICINE_ID).MEDI_STOCK_NAME.ToLower().Contains("kho bhyt"))
                    {
                        this.CK_EXP_AMOUNT = r.AMOUNT;
                    }
                }
                
            }
            
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.BID_NUMBER = Medicine.TDL_BID_NUMBER;
            var expMest = ExpMestIdReasons.FirstOrDefault(o => o.EXP_MEST_ID == r.EXP_MEST_ID);
            this.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
            if (expMest != null && expMest.EXP_MEST_REASON_CODE != null)
            {
                this.DIC_EXP_MEST_REASON.Add(expMest.EXP_MEST_REASON_CODE, r.AMOUNT);
                
            }
            if (r.PATIENT_TYPE_ID == 1&&r.IS_EXPEND==null &&r.TDL_TREATMENT_ID!=null)
            {
                this.EXP_BH_AMOUNT = r.AMOUNT;
            }
            else
            {
                this.EXP_ND_AMOUNT = r.AMOUNT;
            }
            this.MEDICINE_GROUP_CODE = r.MEDICINE_GROUP_CODE;
            this.MEDICINE_GROUP_ID = r.MEDICINE_GROUP_ID;
        }

        public Mrs00584RDO(V_HIS_EXP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, Dictionary<long, PropertyInfo> dicExpAmountType, List<HIS_MATERIAL> Materials, List<ExpMestIdReason> ExpMestIdReasons)
        {
            this.SERVICE_TYPE_NAME = "Vật tư";
            this.SERVICE_TYPE_ID = 2; 
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MATERIAL_ID = r.MATERIAL_ID ?? 0; 
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER; 
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0); 
            this.IMP_PRICE = r.IMP_PRICE*(1+r.IMP_VAT_RATIO); 

            this.SERVICE_ID = r.SERVICE_ID; 
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE; 
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME; 
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.EXP_TOTAL_AMOUNT = r.AMOUNT;
            this.END_AMOUNT = -r.AMOUNT; 
            if (dicExpAmountType.ContainsKey(r.EXP_MEST_TYPE_ID))
                dicExpAmountType[r.EXP_MEST_TYPE_ID].SetValue(this, r.AMOUNT); 

            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA; 
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME; 
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
            }
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.BID_NUMBER = Material.TDL_BID_NUMBER;
            var expMest = ExpMestIdReasons.FirstOrDefault(o => o.EXP_MEST_ID == r.EXP_MEST_ID);
            this.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
            if (expMest != null && expMest.EXP_MEST_REASON_CODE != null)
            {

                this.DIC_EXP_MEST_REASON.Add(expMest.EXP_MEST_REASON_CODE, r.AMOUNT);
            }
            if (r.PATIENT_TYPE_ID == 1)
            {
                this.EXP_BH_AMOUNT = r.AMOUNT;
            }
            else
            {
                this.EXP_ND_AMOUNT = r.AMOUNT;
            }
            if (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == r.MEDI_STOCK_ID).MEDI_STOCK_NAME.ToLower().Contains("kho thuốc chính"))
            {
                if (r.CK_IMP_MEST_MATERIAL_ID != null)
                {
                    if (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == r.CK_IMP_MEST_MATERIAL_ID).MEDI_STOCK_NAME.ToLower().Contains("kho bhyt"))
                    {
                        this.CK_EXP_AMOUNT = r.AMOUNT;
                    }
                }

            }
        }

        public Mrs00584RDO()
        {
            // TODO: Complete member initialization
        }

        public string BID_PACKAGE_CODE { get; set; }

        public string MEDICINE_REGISTER_NUMBER { get; set; }

        public decimal CK_EXP_AMOUNT { get; set; }

        public string EXP_MEST_REASON_CODE { get; set; }

        public string MEDICINE_TYPE_USE_FORM_NAME { get; set; }
    }

    public class ExpMestIdReason
    {
        public long EXP_MEST_ID { get; set; }
        public long EXP_MEST_REASON_ID { get; set; }
        public string EXP_MEST_REASON_CODE { get; set; }
    }
}
