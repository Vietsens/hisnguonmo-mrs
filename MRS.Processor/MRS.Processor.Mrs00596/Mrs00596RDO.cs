using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00596
{
    public class Mrs00596RDO
    {
        private V_HIS_BID_MEDICINE_TYPE item;
        private List<V_HIS_MEDICINE_TYPE> Metys;

        public long MEDI_STOCK_ID { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }

        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }

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

        public string PACKAGE_NUMBER { get; set; }//Số lô
        public string EXPIRED_DATE_STR { get; set; }//Hạn dùng
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public decimal IMP_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal BID_AMOUNT { get; set; }
        public decimal DIFF_AMOUNT { get; set; }
        public decimal IMP_TOTAL_PRICE { get; set; }
        public decimal END_TOTAL_PRICE { get; set; }
        public decimal BID_TOTAL_PRICE { get; set; }
        public decimal DIFF_TOTAL_PRICE { get; set; }

        public string CONCENTRA { get; set; }//Hàm lượng
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }//Biệt dược
        public string NATIONAL_NAME { get; set; }//Quốc gia
        public string MANUFACTURER_NAME { get; set; }//Hãng sx
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string PACKING_TYPE_NAME { get; set; }//Quy cách đóng gói
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string BID_GROUP_CODE { get; set; }//Mã nhóm thầu
        public string BID_NUMBER { get; set; }//
        public string BID_NUM_ORDER { get; set; }//Số thứ tự thầu
        public long BID_ID { get; set; }
        public long? IMP_TIME { get; set; }


        public long SERVICE_TYPE_ID { get; set; }
        public long SERVICE_ID { get; set; }

        public decimal MONTH_1_AMOUNT { get; set; }
        public decimal MONTH_2_AMOUNT { get; set; }
        public decimal MONTH_3_AMOUNT { get; set; }
        public decimal MONTH_4_AMOUNT { get; set; }
        public decimal MONTH_5_AMOUNT { get; set; }
        public decimal MONTH_6_AMOUNT { get; set; }
        public decimal MONTH_7_AMOUNT { get; set; }
        public decimal MONTH_8_AMOUNT { get; set; }
        public decimal MONTH_9_AMOUNT { get; set; }
        public decimal MONTH_10_AMOUNT { get; set; }
        public decimal MONTH_11_AMOUNT { get; set; }
        public decimal MONTH_12_AMOUNT { get; set; }
        public long BID_CREATE_TIME { get; set; }

        public Mrs00596RDO(V_HIS_IMP_MEST_MEDICINE r, List<V_HIS_MEDICINE_TYPE> listHisMedicineType, List<V_HIS_BID_MEDICINE_TYPE> listHisBidMedicineType, List<HIS_MEDICINE_BEAN> listHisMedicineBean, List<V_HIS_MEDICINE> listHisMedicine)
        {
            this.SERVICE_TYPE_ID = 1;
            this.MEMA_ID = r.MEDICINE_ID;
            this.SERVICE_ID = r.SERVICE_ID;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.MEDICINE_TYPE_ID = r.MEDICINE_TYPE_ID;
            this.MEDICINE_TYPE_CODE = r.MEDICINE_TYPE_CODE;
            this.MEDICINE_TYPE_NAME = r.MEDICINE_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.IMP_TIME = r.IMP_TIME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.BID_NUMBER = r.BID_NUMBER;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            var medicineType = listHisMedicineType.FirstOrDefault(o => o.ID == r.MEDICINE_TYPE_ID);
            if (medicineType != null)
            {
                this.CONCENTRA = medicineType.CONCENTRA;
                this.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.MEDICINE_TYPE_PROPRIETARY_NAME;
                this.MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = medicineType.HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = medicineType.HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = medicineType.REGISTER_NUMBER;
            }
            V_HIS_BID_MEDICINE_TYPE bidMedicineType = listHisBidMedicineType.FirstOrDefault(o => o.BID_ID == r.BID_ID && o.MEDICINE_TYPE_ID == r.MEDICINE_TYPE_ID);
            if (bidMedicineType != null)
            {
                this.BID_ID = bidMedicineType.BID_ID;
                this.BID_GROUP_CODE = bidMedicineType.BID_GROUP_CODE;
                this.BID_PACKAGE_CODE = bidMedicineType.BID_PACKAGE_CODE;
                this.BID_AMOUNT = bidMedicineType.AMOUNT;
                this.BID_TOTAL_PRICE = bidMedicineType.AMOUNT * this.IMP_PRICE;
                this.BID_CREATE_TIME = bidMedicineType.CREATE_TIME??0;
            }
            V_HIS_MEDICINE medicine = listHisMedicine.FirstOrDefault(o => o.ID == r.MEDICINE_ID);
            if (medicine != null)
            {
                this.BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;
            }
            this.IMP_AMOUNT = r.AMOUNT;
            this.IMP_TOTAL_PRICE = r.AMOUNT * this.IMP_PRICE;
            var medicineBeanSubs = listHisMedicineBean.Where(o => o.MEDICINE_ID == r.MEDICINE_ID).ToList();
            if (medicineBeanSubs != null)
            {
                this.END_AMOUNT = medicineBeanSubs.Sum(s => s.AMOUNT);
                this.END_TOTAL_PRICE = this.END_AMOUNT * this.IMP_PRICE;
                this.AVAILABLE_AMOUNT = medicineBeanSubs.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(s => s.AMOUNT);
                this.AVAILABLE_TOTAL_PRICE = this.AVAILABLE_AMOUNT * this.IMP_PRICE;
            }
            long Month = GetMonth(r.IMP_TIME ?? 0);
            if (Month == 1)
            {
                MONTH_1_AMOUNT += r.AMOUNT;
            }
            else if (Month == 2)
            {
                MONTH_2_AMOUNT += r.AMOUNT;
            }
            else if (Month == 3)
            {
                MONTH_3_AMOUNT += r.AMOUNT;
            }
            else if (Month == 4)
            {
                MONTH_4_AMOUNT += r.AMOUNT;
            }
            else if (Month == 5)
            {
                MONTH_5_AMOUNT += r.AMOUNT;
            }
            else if (Month == 6)
            {
                MONTH_6_AMOUNT += r.AMOUNT;
            }
            else if (Month == 7)
            {
                MONTH_7_AMOUNT += r.AMOUNT;
            }
            else if (Month == 8)
            {
                MONTH_8_AMOUNT += r.AMOUNT;
            }
            else if (Month == 9)
            {
                MONTH_9_AMOUNT += r.AMOUNT;
            }
            else if (Month == 10)
            {
                MONTH_10_AMOUNT += r.AMOUNT;
            }
            else if (Month == 11)
            {
                MONTH_11_AMOUNT += r.AMOUNT;
            }
            else if (Month == 12)
            {
                MONTH_12_AMOUNT += r.AMOUNT;
            }
        }

        public Mrs00596RDO(V_HIS_IMP_MEST_MATERIAL r, List<V_HIS_MATERIAL_TYPE> listHisMaterialType, List<V_HIS_BID_MATERIAL_TYPE> listHisBidMaterialType, List<HIS_MATERIAL_BEAN> listHisMaterialBean, List<V_HIS_MATERIAL> listHisMaterial)
        {
            this.SERVICE_TYPE_ID = 2;
            this.MEMA_ID = r.MATERIAL_ID;
            this.SERVICE_ID = r.SERVICE_ID;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.MEDICINE_TYPE_ID = r.MATERIAL_TYPE_ID;
            this.MEDICINE_TYPE_CODE = r.MATERIAL_TYPE_CODE;
            this.MEDICINE_TYPE_NAME = r.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.IMP_TIME = r.IMP_TIME;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.BID_NUMBER = r.BID_NUMBER;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            var materialType = listHisMaterialType.FirstOrDefault(o => o.ID == r.MATERIAL_TYPE_ID);
            if (materialType != null)
            {
                if (materialType.IS_CHEMICAL_SUBSTANCE == 1) this.SERVICE_TYPE_ID = 3;
                this.CONCENTRA = materialType.CONCENTRA;
                this.MANUFACTURER_NAME = materialType.MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = materialType.HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = materialType.HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = materialType.PACKING_TYPE_NAME;
            }
            V_HIS_BID_MATERIAL_TYPE bidMaterialType = listHisBidMaterialType.FirstOrDefault(o => o.BID_ID == r.BID_ID && o.MATERIAL_TYPE_ID == r.MATERIAL_TYPE_ID);
            if (bidMaterialType != null)
            {
                this.BID_ID = bidMaterialType.BID_ID;
                this.BID_GROUP_CODE = bidMaterialType.BID_GROUP_CODE;
                this.BID_PACKAGE_CODE = bidMaterialType.BID_PACKAGE_CODE;
                this.BID_AMOUNT = bidMaterialType.AMOUNT;
                this.BID_TOTAL_PRICE = bidMaterialType.AMOUNT * this.IMP_PRICE;
                this.BID_CREATE_TIME = bidMaterialType.CREATE_TIME ?? 0;
            }
            V_HIS_MATERIAL material = listHisMaterial.FirstOrDefault(o => o.ID == r.MATERIAL_ID);
            if (material != null)
            {
                this.BID_NUM_ORDER = material.TDL_BID_NUM_ORDER;
            }
            this.IMP_AMOUNT = r.AMOUNT;
            this.IMP_TOTAL_PRICE = r.AMOUNT * this.IMP_PRICE;
            var materialBeanSubs = listHisMaterialBean.Where(o => o.MATERIAL_ID == r.MATERIAL_ID).ToList();
            if (materialBeanSubs != null)
            {
                this.END_AMOUNT = materialBeanSubs.Sum(s => s.AMOUNT);
                this.END_TOTAL_PRICE = this.END_AMOUNT * this.IMP_PRICE;
                this.AVAILABLE_AMOUNT = materialBeanSubs.Where(o=>o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(s => s.AMOUNT);
                this.AVAILABLE_TOTAL_PRICE = this.AVAILABLE_AMOUNT * this.IMP_PRICE;
            }
            long Month = GetMonth(r.IMP_TIME ?? 0);
            if (Month == 1)
            {
                MONTH_1_AMOUNT += r.AMOUNT;
            }
            else if (Month == 2)
            {
                MONTH_2_AMOUNT += r.AMOUNT;
            }
            else if (Month == 3)
            {
                MONTH_3_AMOUNT += r.AMOUNT;
            }
            else if (Month == 4)
            {
                MONTH_4_AMOUNT += r.AMOUNT;
            }
            else if (Month == 5)
            {
                MONTH_5_AMOUNT += r.AMOUNT;
            }
            else if (Month == 6)
            {
                MONTH_6_AMOUNT += r.AMOUNT;
            }
            else if (Month == 7)
            {
                MONTH_7_AMOUNT += r.AMOUNT;
            }
            else if (Month == 8)
            {
                MONTH_8_AMOUNT += r.AMOUNT;
            }
            else if (Month == 9)
            {
                MONTH_9_AMOUNT += r.AMOUNT;
            }
            else if (Month == 10)
            {
                MONTH_10_AMOUNT += r.AMOUNT;
            }
            else if (Month == 11)
            {
                MONTH_11_AMOUNT += r.AMOUNT;
            }
            else if (Month == 12)
            {
                MONTH_12_AMOUNT += r.AMOUNT;
            }
        }

        public Mrs00596RDO(V_HIS_BID_MEDICINE_TYPE r, List<V_HIS_MEDICINE_TYPE> Metys)
        {
            var medicineType = Metys.FirstOrDefault(o => o.ID == r.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE();
            this.SERVICE_TYPE_ID = 1;
            this.SERVICE_ID = medicineType.SERVICE_ID;
            this.MEDI_STOCK_ID = 0;
            this.PACKAGE_NUMBER = "";
            this.EXPIRED_DATE_STR = "";
            this.MEDICINE_TYPE_ID = r.MEDICINE_TYPE_ID;
            this.MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
            this.MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
            this.IMP_PRICE = 0;
            this.IMP_TIME = 0;
            this.SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
            this.BID_NUMBER = r.BID_NUMBER;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            if (medicineType != null)
            {
                this.CONCENTRA = medicineType.CONCENTRA;
                this.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.MEDICINE_TYPE_PROPRIETARY_NAME;
                this.MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = medicineType.HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = medicineType.HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = medicineType.REGISTER_NUMBER;
            }
            {
                this.BID_ID = r.BID_ID;
                this.BID_GROUP_CODE = r.BID_GROUP_CODE;
                this.BID_PACKAGE_CODE = r.BID_PACKAGE_CODE;
                this.BID_AMOUNT = r.AMOUNT;
                this.BID_TOTAL_PRICE = r.AMOUNT * (r.IMP_PRICE ?? 0) * (1 + (r.IMP_VAT_RATIO ?? 0));
                this.BID_CREATE_TIME = r.CREATE_TIME ?? 0;
            }
            {
                this.BID_NUM_ORDER = r.BID_NUM_ORDER;
            }
            this.IMP_AMOUNT = 0;
            this.IMP_TOTAL_PRICE = 0;
            {
                this.END_AMOUNT = 0;
                this.END_TOTAL_PRICE = 0;
            }
        }

        public Mrs00596RDO(V_HIS_BID_MATERIAL_TYPE r, List<V_HIS_MATERIAL_TYPE> Matys)
        {
            var materialType = Matys.FirstOrDefault(o => o.ID == r.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE();
            this.SERVICE_TYPE_ID = 2;
            this.SERVICE_ID = materialType.SERVICE_ID;
            this.MEDI_STOCK_ID = 0;
            this.PACKAGE_NUMBER = "";
            this.EXPIRED_DATE_STR = "";
            this.MEDICINE_TYPE_ID = r.MATERIAL_TYPE_ID ?? 0;
            this.MEDICINE_TYPE_CODE = materialType.MATERIAL_TYPE_CODE;
            this.MEDICINE_TYPE_NAME = materialType.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = 0;
            this.IMP_TIME = 0;
            this.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
            this.BID_NUMBER = r.BID_NUMBER;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            if (materialType != null)
            {
                if (materialType.IS_CHEMICAL_SUBSTANCE == 1) this.SERVICE_TYPE_ID = 3;
                this.CONCENTRA = materialType.CONCENTRA;
                this.MANUFACTURER_NAME = materialType.MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = materialType.HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = materialType.HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = materialType.PACKING_TYPE_NAME;
            }
            {
                this.BID_ID = r.BID_ID;
                this.BID_GROUP_CODE = r.BID_GROUP_CODE;
                this.BID_PACKAGE_CODE = r.BID_PACKAGE_CODE;
                this.BID_AMOUNT = r.AMOUNT;
                this.BID_TOTAL_PRICE = r.AMOUNT * (r.IMP_PRICE ?? 0) * (1 + (r.IMP_VAT_RATIO ?? 0));
                this.BID_CREATE_TIME = r.CREATE_TIME ?? 0;
            }
            {
                this.BID_NUM_ORDER = r.BID_NUM_ORDER;
            }
            this.IMP_AMOUNT = 0;
            this.IMP_TOTAL_PRICE = 0;
            {
                this.END_AMOUNT = 0;
                this.END_TOTAL_PRICE = 0;
            }
        }
        private long GetMonth(long time)
        {
            long result = 0;
            try
            {
                if (time > 0)
                {
                    //20171208235959
                    string timeStr = time.ToString();
                    string month = timeStr.Substring(4, 2);
                    result = Inventec.Common.TypeConvert.Parse.ToInt64(month);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        public Mrs00596RDO()
        {
            // TODO: Complete member initialization
        }




        public string BID_PACKAGE_CODE { get; set; }

        public decimal AVAILABLE_AMOUNT { get; set; }

        public decimal AVAILABLE_TOTAL_PRICE { get; set; }

        public long MEMA_ID { get; set; }
    }
}
