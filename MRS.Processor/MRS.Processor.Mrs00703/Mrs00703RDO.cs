using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00703
{
    public class Mrs00703RDO
    {
        const long THUOC = 1;
        const long VATTU = 2;

        public long TYPE { get; set; }
        public long MEDI_MATE_ID { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public long MEDI_MATE_TYPE_ID { get; set; }
        public long IMP_MEST_TYPE_ID { get; set; }
        public long EXP_MEST_TYPE_ID { get; set; }

        public long? EXPIRED_DATE { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? PREVIOUS_MEDI_STOCK_ID { get; set; }

        public string PACKAGE_NUMBER { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string BID_GROUP_CODE { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string TDL_BID_NUMBER { get; set; }
        public string CONCENTRA { get; set; }
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string HEIN_SERVICE_CODE { get; set; }
        public string HEIN_SERVICE_NAME { get; set; }
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public string MEDICINE_USE_FORM_CODE { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public string BYT_NUM_ORDER { get; set; }
        public string TCY_NUM_ORDER { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string RECORDING_TRANSACTION { get; set; }
        public string PREVIOUS_MEDI_STOCK_CODE { get; set; }//Mã kho trước chuyển sang
        public string PREVIOUS_MEDI_STOCK_NAME { get; set; }//Tên kho trước chuyển sang

        public decimal VAT_RATIO { get; set; }
        public decimal BEGIN_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal EXP_TOTAL_AMOUNT { get; set; }
        public decimal IMP_TOTAL_AMOUNT { get; set; }

        public short? IS_CABINET { get; set; }

        public Mrs00703RDO(V_HIS_MEST_PERIOD_MEDI r, Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock)
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
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.CONCENTRA = dicMedicineType[r.MEDICINE_TYPE_ID].CONCENTRA;
                this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                this.MANUFACTURER_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = Medicine.ACTIVE_INGR_BHYT_CODE ?? dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = Medicine.ACTIVE_INGR_BHYT_NAME ?? dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = Medicine.MEDICINE_REGISTER_NUMBER ?? dicMedicineType[r.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                this.MEDICINE_USE_FORM_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_CODE;
                this.MEDICINE_USE_FORM_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME;
                this.BYT_NUM_ORDER = dicMedicineType[r.MEDICINE_TYPE_ID].BYT_NUM_ORDER;
                this.TCY_NUM_ORDER = dicMedicineType[r.MEDICINE_TYPE_ID].TCY_NUM_ORDER;
                this.RECORDING_TRANSACTION = dicMedicineType[r.MEDICINE_TYPE_ID].RECORDING_TRANSACTION;
            }

            if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID ?? 0))
            {
                this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID ?? 0].IS_CABINET;
                this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID ?? 0].MEDI_STOCK_NAME;
                this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID ?? 0].DEPARTMENT_NAME;
            }
        }

        public Mrs00703RDO(V_HIS_MEST_PERIOD_MATE r, Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock)
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
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA;
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
                this.SERVICE_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE;
                this.SERVICE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME;
                this.SERVICE_UNIT_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME;
                this.RECORDING_TRANSACTION = dicMaterialType[Material.MATERIAL_TYPE_ID].RECORDING_TRANSACTION;
            }

            if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID ?? 0))
            {
                this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID ?? 0].IS_CABINET;
                this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID ?? 0].MEDI_STOCK_NAME;
                this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID ?? 0].DEPARTMENT_NAME;
            }
        }

        public Mrs00703RDO(V_HIS_IMP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock, List<HIS_IMP_MEST> listImpMestAll, List<ImpMestIdChmsMediStockId> listImpMestIdChmsMediStockId, bool isBefore)
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
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            this.IMP_MEST_TYPE_ID = r.IMP_MEST_TYPE_ID;
            if (isBefore)
            {
                this.BEGIN_AMOUNT = r.AMOUNT;
            }
            else
            {
                this.IMP_TOTAL_AMOUNT = r.AMOUNT;
            }
            this.END_AMOUNT = r.AMOUNT;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            if (dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.CONCENTRA = dicMedicineType[r.MEDICINE_TYPE_ID].CONCENTRA;
                this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                this.MANUFACTURER_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = Medicine.ACTIVE_INGR_BHYT_CODE ?? dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = Medicine.ACTIVE_INGR_BHYT_NAME ?? dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = Medicine.MEDICINE_REGISTER_NUMBER ?? dicMedicineType[r.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                this.MEDICINE_USE_FORM_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_CODE;
                this.MEDICINE_USE_FORM_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME;
                this.BYT_NUM_ORDER = dicMedicineType[r.MEDICINE_TYPE_ID].BYT_NUM_ORDER;
                this.TCY_NUM_ORDER = dicMedicineType[r.MEDICINE_TYPE_ID].TCY_NUM_ORDER;
                this.RECORDING_TRANSACTION = dicMedicineType[r.MEDICINE_TYPE_ID].RECORDING_TRANSACTION;
            }

            if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID))
            {
                this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID].IS_CABINET;
                this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID].MEDI_STOCK_NAME;
                this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID].DEPARTMENT_NAME;
            }
            if (listImpMestAll != null)
            {
                var impMest = listImpMestAll.FirstOrDefault(o => o.ID == r.IMP_MEST_ID);
                if (impMest != null)
                {
                    if (impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS)
                    {
                        if (impMest.CHMS_MEDI_STOCK_ID == null && listImpMestIdChmsMediStockId != null)
                        {
                            var ImpMestIdChmsMediStockId = listImpMestIdChmsMediStockId.FirstOrDefault(o => o.IMP_MEST_ID == impMest.ID);
                            if (ImpMestIdChmsMediStockId != null && ImpMestIdChmsMediStockId.CHMS_MEDI_STOCK_ID > 0)
                            {
                                impMest.CHMS_MEDI_STOCK_ID = ImpMestIdChmsMediStockId.CHMS_MEDI_STOCK_ID;
                            }
                        }
                        this.PREVIOUS_MEDI_STOCK_ID = impMest.CHMS_MEDI_STOCK_ID;
                        this.PREVIOUS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        this.PREVIOUS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    }
                }
            }
        }

        public Mrs00703RDO(V_HIS_IMP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock, List<HIS_IMP_MEST> listImpMestAll, List<ImpMestIdChmsMediStockId> listImpMestIdChmsMediStockId, bool isBefore)
        {
            this.TYPE = VATTU;
            this.MEDI_STOCK_ID = r.MEDI_STOCK_ID;
            this.IMP_MEST_TYPE_ID = r.IMP_MEST_TYPE_ID;
            this.MEDI_MATE_ID = r.MATERIAL_ID;
            this.PACKAGE_NUMBER = r.PACKAGE_NUMBER;
            this.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.EXPIRED_DATE ?? 0);
            this.EXPIRED_DATE = r.EXPIRED_DATE;
            this.MEDI_MATE_TYPE_ID = r.MATERIAL_TYPE_ID;
            this.SERVICE_CODE = r.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = r.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.VAT_RATIO = r.IMP_VAT_RATIO;
            if (isBefore)
            {
                this.BEGIN_AMOUNT = r.AMOUNT;
            }
            else
            {
                this.IMP_TOTAL_AMOUNT = r.AMOUNT;
            }
            this.END_AMOUNT = r.AMOUNT;
            this.SERVICE_UNIT_NAME = r.SERVICE_UNIT_NAME;
            this.NATIONAL_NAME = r.NATIONAL_NAME;
            this.SUPPLIER_ID = r.SUPPLIER_ID;
            this.SUPPLIER_CODE = r.SUPPLIER_CODE;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA;
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
                this.SERVICE_CODE = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE;
                this.SERVICE_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME;
                this.SERVICE_UNIT_NAME = dicMaterialType[Material.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME;
                this.RECORDING_TRANSACTION = dicMaterialType[Material.MATERIAL_TYPE_ID].RECORDING_TRANSACTION;
            }

            if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID))
            {
                this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID].IS_CABINET;
                this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID].MEDI_STOCK_NAME;
                this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID].DEPARTMENT_NAME;
            }
            if (listImpMestAll != null)
            {
                var impMest = listImpMestAll.FirstOrDefault(o => o.ID == r.IMP_MEST_ID);
                if (impMest != null)
                {
                    if (impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS)
                    {
                        if (impMest.CHMS_MEDI_STOCK_ID == null && listImpMestIdChmsMediStockId != null)
                        {
                            var ImpMestIdChmsMediStockId = listImpMestIdChmsMediStockId.FirstOrDefault(o => o.IMP_MEST_ID == impMest.ID);
                            if (ImpMestIdChmsMediStockId != null && ImpMestIdChmsMediStockId.CHMS_MEDI_STOCK_ID > 0)
                            {
                                impMest.CHMS_MEDI_STOCK_ID = ImpMestIdChmsMediStockId.CHMS_MEDI_STOCK_ID;
                            }
                        }
                        this.PREVIOUS_MEDI_STOCK_ID = impMest.CHMS_MEDI_STOCK_ID;
                        this.PREVIOUS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        this.PREVIOUS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    }
                }
            }
        }

        public Mrs00703RDO(Mrs00703RDO r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00703RDO>(this, r);

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

                this.SUPPLIER_ID = Medicine.SUPPLIER_ID;
                this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
                this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
                this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
                if (dicMedicineType.ContainsKey(Medicine.MEDICINE_TYPE_ID))
                {
                    this.SERVICE_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE;
                    this.SERVICE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME;
                    this.SERVICE_UNIT_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME;
                    this.CONCENTRA = dicMedicineType[Medicine.MEDICINE_TYPE_ID].CONCENTRA;
                    this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                    this.MANUFACTURER_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                    this.HEIN_SERVICE_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                    this.HEIN_SERVICE_NAME = Medicine.ACTIVE_INGR_BHYT_CODE ?? dicMedicineType[Medicine.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    this.ACTIVE_INGR_BHYT_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                    this.ACTIVE_INGR_BHYT_NAME = Medicine.ACTIVE_INGR_BHYT_NAME ?? dicMedicineType[Medicine.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                    this.PACKING_TYPE_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                    this.REGISTER_NUMBER = Medicine.MEDICINE_REGISTER_NUMBER ?? dicMedicineType[Medicine.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                    this.NATIONAL_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].NATIONAL_NAME;
                    this.MEDICINE_USE_FORM_CODE = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_CODE;
                    this.MEDICINE_USE_FORM_NAME = dicMedicineType[Medicine.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME;
                    this.BYT_NUM_ORDER = dicMedicineType[Medicine.MEDICINE_TYPE_ID].BYT_NUM_ORDER;
                    this.TCY_NUM_ORDER = dicMedicineType[Medicine.MEDICINE_TYPE_ID].TCY_NUM_ORDER;
                    this.RECORDING_TRANSACTION = dicMedicineType[Medicine.MEDICINE_TYPE_ID].RECORDING_TRANSACTION;
                }

                if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID))
                {
                    this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID].IS_CABINET;
                    this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID].MEDI_STOCK_NAME;
                    this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID].DEPARTMENT_NAME;
                }
            }
        }

        public Mrs00703RDO(Mrs00703RDO r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00703RDO>(this, r);

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

                this.SUPPLIER_ID = Material.SUPPLIER_ID;
                this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
                this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
                this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
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
                    this.RECORDING_TRANSACTION = dicMaterialType[Material.MATERIAL_TYPE_ID].RECORDING_TRANSACTION;
                }

                if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID))
                {
                    this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID].IS_CABINET;
                    this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID].MEDI_STOCK_NAME;
                    this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID].DEPARTMENT_NAME;
                }
            }
        }

        public Mrs00703RDO(V_HIS_EXP_MEST_MEDICINE r, Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType, List<HIS_MEDICINE> Medicines, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock, List<HIS_EXP_MEST> listExpMest)
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

            this.END_AMOUNT = -r.AMOUNT;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
            this.EXP_MEST_TYPE_ID = r.EXP_MEST_TYPE_ID;

            if (dicMedicineType.ContainsKey(r.MEDICINE_TYPE_ID))
            {
                this.CONCENTRA = dicMedicineType[r.MEDICINE_TYPE_ID].CONCENTRA;
                this.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                this.MANUFACTURER_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.ACTIVE_INGR_BHYT_CODE = Medicine.ACTIVE_INGR_BHYT_CODE ?? dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                this.ACTIVE_INGR_BHYT_NAME = Medicine.ACTIVE_INGR_BHYT_NAME ?? dicMedicineType[r.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                this.REGISTER_NUMBER = Medicine.MEDICINE_REGISTER_NUMBER ?? dicMedicineType[r.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                this.MEDICINE_USE_FORM_CODE = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_CODE;
                this.MEDICINE_USE_FORM_NAME = dicMedicineType[r.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME;
                this.BYT_NUM_ORDER = dicMedicineType[r.MEDICINE_TYPE_ID].BYT_NUM_ORDER;
                this.TCY_NUM_ORDER = dicMedicineType[r.MEDICINE_TYPE_ID].TCY_NUM_ORDER;
                this.RECORDING_TRANSACTION = dicMedicineType[r.MEDICINE_TYPE_ID].RECORDING_TRANSACTION;
            }

            if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID))
            {
                this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID].IS_CABINET;
                this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID].MEDI_STOCK_NAME;
                this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID].DEPARTMENT_NAME;
            }

            if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK)
            {
                var expMest = listExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID);
                if (expMest != null)
                {
                    this.PREVIOUS_MEDI_STOCK_ID = expMest.IMP_MEDI_STOCK_ID;
                    this.PREVIOUS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMest.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    this.PREVIOUS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMest.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                }
            }
        }

        public Mrs00703RDO(V_HIS_EXP_MEST_MATERIAL r, Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType, List<HIS_MATERIAL> Materials, Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock, List<HIS_EXP_MEST> listExpMest)
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

            this.END_AMOUNT = -r.AMOUNT;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            this.EXP_MEST_TYPE_ID = r.EXP_MEST_TYPE_ID;

            if (dicMaterialType.ContainsKey(r.MATERIAL_TYPE_ID))
            {
                this.CONCENTRA = dicMaterialType[r.MATERIAL_TYPE_ID].CONCENTRA;
                this.MANUFACTURER_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                this.HEIN_SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                this.HEIN_SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                this.PACKING_TYPE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
                this.SERVICE_CODE = dicMaterialType[r.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE;
                this.SERVICE_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME;
                this.SERVICE_UNIT_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME;
                this.RECORDING_TRANSACTION = dicMaterialType[r.MATERIAL_TYPE_ID].RECORDING_TRANSACTION;
                this.NATIONAL_NAME = dicMaterialType[r.MATERIAL_TYPE_ID].NATIONAL_NAME;
            }

            if (dicMediStock != null && dicMediStock.ContainsKey(r.MEDI_STOCK_ID))
            {
                this.IS_CABINET = dicMediStock[r.MEDI_STOCK_ID].IS_CABINET;
                this.MEDI_STOCK_NAME = dicMediStock[r.MEDI_STOCK_ID].MEDI_STOCK_NAME;
                this.DEPARTMENT_NAME = dicMediStock[r.MEDI_STOCK_ID].DEPARTMENT_NAME;
            }

            if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK)
            {
                var expMest = listExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID);
                if (expMest != null)
                {
                    this.PREVIOUS_MEDI_STOCK_ID = expMest.IMP_MEDI_STOCK_ID;
                    this.PREVIOUS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMest.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    this.PREVIOUS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMest.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                }
            }
        }

        public Mrs00703RDO()
        {
            // TODO: Complete member initialization
        }
    }

    public class ImpMestIdChmsMediStockId
    {
        public long IMP_MEST_ID { get; set; }
        public long? CHMS_MEDI_STOCK_ID { get; set; }
    }

    public class TotalPriceInStock
    {
        public long MEDI_STOCK_ID { get; set; }
        public string RECORDING_TRANSACTION { get; set; }

        //public decimal IMP_PRICE { get; set; }
        //public decimal VAT_RATIO { get; set; }
        //public decimal BEGIN_AMOUNT { get; set; }
        //public decimal END_AMOUNT { get; set; }
        //public decimal EXP_TOTAL_AMOUNT { get; set; }
        //public decimal IMP_TOTAL_AMOUNT { get; set; }

        public decimal BEGIN_PRICE { get; set; }
        public decimal END_PRICE { get; set; }

        public decimal IMP_TOTAL_PRICE { get; set; }
        public decimal IMP_NCC_TOTAL_PRICE { get; set; }

        public decimal EXP_TOTAL_PRICE { get; set; }
        public decimal EXP_QT_TOTAL_PRICE { get; set; }
        public decimal EXP_OTHER_TOTAL_PRICE { get; set; }
    }

    public class TotalPriceImpExpStock
    {
        public string RECORDING_TRANSACTION { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }

        public long MEDI_STOCK_ID { get; set; }

        public decimal IMP_TOTAL_PRICE { get; set; }
        public decimal EXP_TOTAL_PRICE { get; set; }
    }
}
