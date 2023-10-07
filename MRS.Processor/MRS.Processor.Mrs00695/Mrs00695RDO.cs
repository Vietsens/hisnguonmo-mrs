using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00695
{
    class Mrs00695RDO : IMP_EXP_MEST_TYPE
    {
        public const long THUOC = 1;
        public const long VATTU = 2;

        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public long MEDI_MATE_TYPE_ID { get; set; }
        public long TYPE { get; set; }
        public long MEDI_MATE_ID { get; set; }
        public string PACKAGE_NUMBER { get; set; }//Số lô
        public long? EXPIRED_DATE { get; set; }//Hạn dùng
        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public decimal IMP_TOTAL_AMOUNT { get; set; }
        public decimal EXP_TOTAL_AMOUNT { get; set; }

        public decimal BEGIN_AMOUNT { get; set; }
        public decimal? VAT_RATIO { get; set; }
        public string VAT_RATIO_STR { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }
        public string CONCENTRA { get; set; }//Hàm lượng
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }//Biệt dược
        public string NATIONAL_NAME { get; set; }//Quốc gia
        public string MANUFACTURER_NAME { get; set; }//Hãng sx
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }//Nhà cung cấp
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string PACKING_TYPE_NAME { get; set; }//Quy cách đóng gói
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string BID_GROUP_CODE { get; set; }//Mã nhóm thầu
        public string BID_NUM_ORDER { get; set; }//Số thứ tự thầu

        public decimal BEGIN_TOTAL_PRICE { get; set; }
        public decimal EXP_TOTAL_PRICE { get; set; }
        public decimal IMP_TOTAL_PRICE { get; set; }
        public decimal END_TOTAL_PRICE { get; set; }

        public string EXP_MEDI_STOCK_CODE { get; set; }//Mã kho xuất
        public string IMP_MEDI_STOCK_CODE { get; set; }//Mã kho nhập
        public decimal INPUT_END_AMOUNT { get; set; }//Số lượng thực tế khi kiểm kê

        public string TDL_BID_NUMBER { get; set; }
        public decimal? ALERT_MAX_IN_STOCK { get; set; }
        public decimal FAILED_AMOUNT { get; set; }

        public decimal EXP_TOTAL_BHYT_AMOUNT { get; set; }
        public decimal EXP_TOTAL_TP_AMOUNT { get; set; }
        public decimal EXP_TOTAL_HP_AMOUNT { get; set; }

        public decimal INVENTORY_AMOUNT { get; set; }

        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }

        public string MEDI_MATE_PARENT_CODE { get; set; }
        public string MEDI_MATE_PARENT_NAME { get; set; }
       

        public Mrs00695RDO(V_HIS_MEST_PERIOD_MEDI r, Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety)
        {
            this.TYPE = THUOC;
            this.MEDI_STOCK_ID = dicMediStockPeriod.Values.FirstOrDefault(o => o.ID == r.MEDI_STOCK_PERIOD_ID).MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MEDICINE_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.MEDI_MATE_TYPE_ID = r.MEDICINE_TYPE_ID;
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE;
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME;
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.BEGIN_AMOUNT = r.AMOUNT;
            this.END_AMOUNT = r.AMOUNT;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
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
                this.MEDICINE_GROUP_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_CODE;
                this.MEDICINE_GROUP_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_NAME;
                long parentId = dicMedicineType[r.MEDICINE_TYPE_ID].PARENT_ID ?? 0;
                if (dicMedicineType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMedicineType[parentId].MEDICINE_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMedicineType[parentId].MEDICINE_TYPE_NAME;
                }
            }

            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (DicMediStockMety.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMety[r.MEDICINE_TYPE_ID].ALERT_MAX_IN_STOCK;
            }

            this.INVENTORY_AMOUNT = r.INVENTORY_AMOUNT ?? 0;
        }

        public Mrs00695RDO(V_HIS_MEST_PERIOD_MATE r, Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty)
        {
            this.TYPE = VATTU;
            this.MEDI_STOCK_ID = dicMediStockPeriod.Values.FirstOrDefault(o => o.ID == r.MEDI_STOCK_PERIOD_ID).MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MATERIAL_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.BEGIN_AMOUNT = r.AMOUNT;
            this.END_AMOUNT = r.AMOUNT;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.MEDI_MATE_TYPE_ID = r.MATERIAL_TYPE_ID;
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME;
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA;
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;

                long parentId = dicMaterialType[r.MATERIAL_TYPE_ID].PARENT_ID ?? 0;
                if (dicMaterialType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMaterialType[parentId].MATERIAL_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMaterialType[parentId].MATERIAL_TYPE_NAME;
                }
            }

            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            if (DicMediStockMaty.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMaty[r.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
            }

            this.INVENTORY_AMOUNT = r.INVENTORY_AMOUNT ?? 0;
        }

        public Mrs00695RDO(V_HIS_IMP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety)
        {
            this.TYPE = THUOC;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MEDICINE_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.MEDI_MATE_TYPE_ID = r.MEDICINE_TYPE_ID;
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE;
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
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
                this.MEDICINE_GROUP_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_CODE;
                this.MEDICINE_GROUP_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_NAME;
                long parentId = dicMedicineType[r.MEDICINE_TYPE_ID].PARENT_ID ?? 0;
                if (dicMedicineType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMedicineType[parentId].MEDICINE_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMedicineType[parentId].MEDICINE_TYPE_NAME;
                }
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
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (DicMediStockMety.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMety[r.MEDICINE_TYPE_ID].ALERT_MAX_IN_STOCK;
            }
        }

        public Mrs00695RDO(V_HIS_IMP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty)
        {
            this.TYPE = VATTU;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MATERIAL_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.MEDI_MATE_TYPE_ID = r.MATERIAL_TYPE_ID;
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
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

                long parentId = dicMaterialType[r.MATERIAL_TYPE_ID].PARENT_ID ?? 0;
                if (dicMaterialType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMaterialType[parentId].MATERIAL_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMaterialType[parentId].MATERIAL_TYPE_NAME;
                }
            }

            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            if (DicMediStockMaty.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMaty[r.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
            }
        }

        public Mrs00695RDO(Mrs00695RDO r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00695RDO>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }

            this.TYPE = THUOC;

            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDI_MATE_ID);
            if (Medicine != null)
            {
                this.PACKAGE_NUMBER = Medicine.PACKAGE_NUMBER;
                this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Medicine.EXPIRED_DATE ?? 0);
                this.EXPIRED_DATE = r.EXPIRED_DATE;
                this.MEDI_MATE_TYPE_ID = Medicine.MEDICINE_TYPE_ID;
                this.IMP_PRICE = Medicine.IMP_PRICE * (1 + Medicine.IMP_VAT_RATIO);
                this.VAT_RATIO = Medicine.IMP_VAT_RATIO;
                if (dicMedicineType.ContainsKey(Medicine.MEDICINE_TYPE_ID))
                {
                    this.SERVICE_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE;
                    this.SERVICE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME;
                    this.SERVICE_UNIT_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME;
                    this.CONCENTRA = dicMedicineType[Medicine.MEDICINE_TYPE_ID].CONCENTRA;
                    this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                    this.MANUFACTURER_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                    this.HEIN_SERVICE_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                    this.HEIN_SERVICE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    this.ACTIVE_INGR_BHYT_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                    this.ACTIVE_INGR_BHYT_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                    this.PACKING_TYPE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                    this.REGISTER_NUMBER = dicMedicineType[Medicine.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                    this.NATIONAL_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].NATIONAL_NAME;
                    this.MEDICINE_GROUP_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_GROUP_CODE;
                    this.MEDICINE_GROUP_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_GROUP_NAME;
                    long parentId = dicMedicineType[Medicine.MEDICINE_TYPE_ID].PARENT_ID ?? 0;
                    if (dicMedicineType.ContainsKey(parentId))
                    {
                        this.MEDI_MATE_PARENT_CODE = dicMedicineType[parentId].MEDICINE_TYPE_CODE;
                        this.MEDI_MATE_PARENT_NAME = dicMedicineType[parentId].MEDICINE_TYPE_NAME;
                    }
                }

                this.SUPPLIER_ID = Medicine.SUPPLIER_ID;
                this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
                this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
                this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
                if (DicMediStockMety.ContainsKey(Medicine.MEDICINE_TYPE_ID))
                {
                    this.ALERT_MAX_IN_STOCK = DicMediStockMety[Medicine.MEDICINE_TYPE_ID].ALERT_MAX_IN_STOCK;
                }
            }
        }

        public Mrs00695RDO(Mrs00695RDO r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00695RDO>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }

            this.TYPE = VATTU;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MEDI_MATE_ID);
            if (Material != null)
            {
                this.PACKAGE_NUMBER = Material.PACKAGE_NUMBER;
                this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Material.EXPIRED_DATE ?? 0);
                this.EXPIRED_DATE = r.EXPIRED_DATE;
                this.MEDI_MATE_TYPE_ID = Material.MATERIAL_TYPE_ID;
                this.IMP_PRICE = Material.IMP_PRICE * (1 + Material.IMP_VAT_RATIO);
                this.VAT_RATIO = Material.IMP_VAT_RATIO;
                if (dicMaterialType.ContainsKey(Material.MATERIAL_TYPE_ID))
                {
                    this.SERVICE_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE;
                    this.SERVICE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME;
                    this.SERVICE_UNIT_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME;
                    this.CONCENTRA = dicMaterialType[Material.MATERIAL_TYPE_ID].CONCENTRA;
                    this.MANUFACTURER_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                    this.HEIN_SERVICE_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                    this.HEIN_SERVICE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    this.PACKING_TYPE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
                    this.NATIONAL_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].NATIONAL_NAME;

                    long parentId = dicMaterialType[Material.MATERIAL_TYPE_ID].PARENT_ID ?? 0;
                    if (dicMaterialType.ContainsKey(parentId))
                    {
                        this.MEDI_MATE_PARENT_CODE = dicMaterialType[parentId].MATERIAL_TYPE_CODE;
                        this.MEDI_MATE_PARENT_NAME = dicMaterialType[parentId].MATERIAL_TYPE_NAME;
                    }
                }

                this.SUPPLIER_ID = Material.SUPPLIER_ID;
                this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
                this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
                this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
                if (DicMediStockMaty.ContainsKey(Material.MATERIAL_TYPE_ID))
                {
                    this.ALERT_MAX_IN_STOCK = DicMediStockMaty[Material.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
                }
            }
        }

        public Mrs00695RDO(V_HIS_IMP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, Dictionary<long, PropertyInfo> dicImpAmountType, List<HIS_MEDICINE> Medicines, Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety)
        {
            this.TYPE = THUOC;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MEDICINE_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.IMP_TOTAL_AMOUNT = r.AMOUNT;
            if (dicImpAmountType.ContainsKey(r.IMP_MEST_TYPE_ID))
                dicImpAmountType[r.IMP_MEST_TYPE_ID].SetValue(this, r.AMOUNT);

            this.END_AMOUNT = r.AMOUNT;
            this.MEDI_MATE_TYPE_ID = r.MEDICINE_TYPE_ID;
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE;
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.VAT_RATIO = r.IMP_VAT_RATIO;
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
                this.MEDICINE_GROUP_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_CODE;
                this.MEDICINE_GROUP_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_NAME;
                long parentId = dicMedicineType[r.MEDICINE_TYPE_ID].PARENT_ID ?? 0;
                if (dicMedicineType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMedicineType[parentId].MEDICINE_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMedicineType[parentId].MEDICINE_TYPE_NAME;
                }
            }

            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (DicMediStockMety.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMety[r.MEDICINE_TYPE_ID].ALERT_MAX_IN_STOCK;
            }
        }

        public Mrs00695RDO(V_HIS_IMP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, Dictionary<long, PropertyInfo> dicImpAmountType, List<HIS_MATERIAL> Materials, Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty)
        {
            this.TYPE = VATTU;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MATERIAL_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
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

                long parentId = dicMaterialType[r.MATERIAL_TYPE_ID].PARENT_ID ?? 0;
                if (dicMaterialType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMaterialType[parentId].MATERIAL_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMaterialType[parentId].MATERIAL_TYPE_NAME;
                }
            }

            this.MEDI_MATE_TYPE_ID = r.MATERIAL_TYPE_ID;
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
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            if (DicMediStockMaty.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMaty[r.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
            }
        }

        public Mrs00695RDO(V_HIS_EXP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, Dictionary<long, PropertyInfo> dicExpAmountType, List<HIS_MEDICINE> Medicines, Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety)
        {
            this.TYPE = THUOC;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MEDICINE_ID ?? 0;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.MEDI_MATE_TYPE_ID = r.MEDICINE_TYPE_ID;
            this.SERVICE_CODE = r.MEDICINE_TYPE_CODE;
            this.SERVICE_NAME = r.MEDICINE_TYPE_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.EXP_TOTAL_AMOUNT = r.AMOUNT;
            if (dicExpAmountType.ContainsKey(r.EXP_MEST_TYPE_ID))
                dicExpAmountType[r.EXP_MEST_TYPE_ID].SetValue(this, r.AMOUNT);

            this.END_AMOUNT = -r.AMOUNT;
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
                this.MEDICINE_GROUP_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_CODE;
                this.MEDICINE_GROUP_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_GROUP_NAME;
                long parentId = dicMedicineType[r.MEDICINE_TYPE_ID].PARENT_ID ?? 0;
                if (dicMedicineType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMedicineType[parentId].MEDICINE_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMedicineType[parentId].MEDICINE_TYPE_NAME;
                }
            }

            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (DicMediStockMety.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMety[r.MEDICINE_TYPE_ID].ALERT_MAX_IN_STOCK;
            }

            if (r.PATIENT_TYPE_ID.HasValue)
            {
                if (r.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    EXP_TOTAL_BHYT_AMOUNT = r.AMOUNT;
                }
                else
                {
                    EXP_TOTAL_TP_AMOUNT = r.AMOUNT;
                }
            }
            else
            {
                EXP_TOTAL_HP_AMOUNT = r.AMOUNT;
            }
        }

        public Mrs00695RDO(V_HIS_EXP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, Dictionary<long, PropertyInfo> dicExpAmountType, List<HIS_MATERIAL> Materials, Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty)
        {
            this.TYPE = VATTU;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.MEDI_MATE_ID = r.MATERIAL_ID ?? 0;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);

            this.MEDI_MATE_TYPE_ID = r.MATERIAL_TYPE_ID;
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            this.EXP_TOTAL_AMOUNT = r.AMOUNT;
            if (dicExpAmountType.ContainsKey(r.EXP_MEST_TYPE_ID))
                dicExpAmountType[r.EXP_MEST_TYPE_ID].SetValue(this, r.AMOUNT);

            this.END_AMOUNT = -r.AMOUNT;
            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA;
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;

                long parentId = dicMaterialType[r.MATERIAL_TYPE_ID].PARENT_ID ?? 0;
                if (dicMaterialType.ContainsKey(parentId))
                {
                    this.MEDI_MATE_PARENT_CODE = dicMaterialType[parentId].MATERIAL_TYPE_CODE;
                    this.MEDI_MATE_PARENT_NAME = dicMaterialType[parentId].MATERIAL_TYPE_NAME;
                }
            }

            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            if (DicMediStockMaty.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.ALERT_MAX_IN_STOCK = DicMediStockMaty[r.MATERIAL_TYPE_ID].ALERT_MAX_IN_STOCK;
            }

            if (r.PATIENT_TYPE_ID.HasValue)
            {
                if (r.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    EXP_TOTAL_BHYT_AMOUNT = r.AMOUNT;
                }
                else
                {
                    EXP_TOTAL_TP_AMOUNT = r.AMOUNT;
                }
            }
            else
            {
                EXP_TOTAL_HP_AMOUNT = r.AMOUNT;
            }

            this.FAILED_AMOUNT = r.FAILED_AMOUNT ?? 0;
        }

        public Mrs00695RDO()
        {
            // TODO: Complete member initialization
        }
    }

    public class DETAIL
    {
        public string EXP_MEST_CODE { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string EXP_MEDI_STOCK_CODE { get; set; }
        public string IMP_MEDI_STOCK_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal IMP_PRICE_AFTER { get; set; }
        public decimal AMOUNT { get; set; }
        public string AGGR_IMP_MEST_CODE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
        public string MATE_TYPE_NAME { get; set; }
        public string MATE_TYPE_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string  MANUFACTURER_NAME { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public long EXPIRED_DATE { get; set; }
    }
}
