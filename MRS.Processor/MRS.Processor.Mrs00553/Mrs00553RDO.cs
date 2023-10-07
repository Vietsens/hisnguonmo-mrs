using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00553
{
    class Mrs00553RDO : MOS.EFMODEL.DataModels.V_HIS_IMP_MEST
    {
        public HIS_MEDICINE_USE_FORM HIS_MEDICINE_USE_FORM { set; get; }
        public HIS_MEDICINE HIS_MEDICINE { set; get; }
        public string MEDICINE_USE_FORM_NAME { set; get; }
        public string MEDICINE_USE_FORM_CODE { set; get; }
        public string IMP_SOURCE_NAME { set; get; }
        public string IMP_SOURCE_CODE { get; set; }
       
        //thông tin chi tiết
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public long? BID_ID { get; set; }
        public string BID_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public string BYT_NUM_ORDER { get; set; }
        public string CONCENTRA { get; set; }
        public decimal? CONTRACT_PRICE { get; set; }
        public decimal? CONVERT_RATIO { get; set; }
        public string CONVERT_UNIT_CODE { get; set; }
        public string CONVERT_UNIT_NAME { get; set; }
        public long? EXPIRED_DATE { get; set; }
        public long EXP_MEST_MEMA_ID { get; set; }///////////////////////////////////
        public decimal IMP_PRICE { get; set; }
        public decimal? IMP_UNIT_AMOUNT { get; set; }
        public string IMP_UNIT_CODE { get; set; }
        public string IMP_UNIT_NAME { get; set; }
        public decimal? IMP_UNIT_PRICE { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public decimal? INTERNAL_PRICE { get; set; }
        public short? IS_STAR_MARK { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public long? MANUFACTURER_ID { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public long? MATERIAL_NUM_ORDER { get; set; }
        public string MEDICINE_BYT_NUM_ORDER { get; set; }
        public long? MEDICINE_GROUP_ID { get; set; }
        public long MEDICINE_ID { get; set; }///////////////////////////////////////////////
        public short? MEDICINE_IS_STAR_MARK { get; set; }
        public long? MEDICINE_NUM_ORDER { get; set; }
        public string MEDICINE_REGISTER_NUMBER { get; set; }
        public string MEDICINE_TCY_NUM_ORDER { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }/////////////////////////////////////
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDI_MATE_CHEMI { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string NOTE { get; set; }
        public long? NUM_ORDER { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public decimal? PROFIT_RATIO { get; set; }
        public string RECORDING_TRANSACTION { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public decimal? REQ_AMOUNT { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public long SERVICE_UNIT_ID { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public long? SPECIAL_MEDICINE_TYPE { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string TCY_NUM_ORDER { get; set; }
        public decimal? TDL_IMP_UNIT_CONVERT_RATIO { get; set; }
        public long? TDL_IMP_UNIT_ID { get; set; }
        public long? TH_EXP_MEST_MEDICINE_ID { get; set; }
        public decimal? VAT_RATIO { get; set; }
        public decimal? VIR_PRICE { get; set; }
        public long PARENT_NUM_ORDER { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }

        public Mrs00553RDO(MOS.EFMODEL.DataModels.V_HIS_IMP_MEST impMest, Dictionary<long, MOS.EFMODEL.DataModels.HIS_SUPPLIER> DicSupplier)
        {
            try
            {
                if (impMest != null)
                {
                    System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST>();
                    foreach (var item in pis)
                    {
                        item.SetValue(this, item.GetValue(impMest));
                    }

                    if (DicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0))
                    {
                        this.SUPPLIER_NAME = DicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME;
                    }

                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00553RDO()
        {
            // TODO: Complete member initialization
        }

        public Mrs00553RDO(V_HIS_IMP_MEST_MEDICINE impMestMedicine)
        {
            // TODO: Complete member initialization
            try
            {
                if (impMestMedicine != null)
                {
                    this.ACTIVE_INGR_BHYT_CODE = impMestMedicine.ACTIVE_INGR_BHYT_CODE;
                    this.ACTIVE_INGR_BHYT_NAME = impMestMedicine.ACTIVE_INGR_BHYT_NAME;
                    this.AMOUNT = impMestMedicine.AMOUNT;
                    this.BID_ID = impMestMedicine.BID_ID;
                    this.BID_NAME = impMestMedicine.BID_NAME;
                    this.BID_NUMBER = impMestMedicine.BID_NUMBER;
                    this.BYT_NUM_ORDER = impMestMedicine.BYT_NUM_ORDER;
                    this.CONCENTRA = impMestMedicine.CONCENTRA;
                    this.CONTRACT_PRICE = impMestMedicine.CONTRACT_PRICE;
                    this.CONVERT_RATIO = impMestMedicine.CONVERT_RATIO;
                    this.CONVERT_UNIT_CODE = impMestMedicine.CONVERT_UNIT_CODE;
                    this.CONVERT_UNIT_NAME = impMestMedicine.CONVERT_UNIT_NAME;
                    this.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE;
                    //this.EXP_MEST_MEMA_ID = impMestMedicine.EXP_MEST_MEMA_ID;///////////////////////////////////
                    this.IMP_PRICE = impMestMedicine.IMP_PRICE;
                    this.IMP_UNIT_AMOUNT = impMestMedicine.IMP_UNIT_AMOUNT;
                    this.IMP_UNIT_CODE = impMestMedicine.IMP_UNIT_CODE;
                    this.IMP_UNIT_NAME = impMestMedicine.IMP_UNIT_NAME;
                    this.IMP_UNIT_PRICE = impMestMedicine.IMP_UNIT_PRICE;
                    this.IMP_VAT_RATIO = impMestMedicine.IMP_VAT_RATIO;
                    this.INTERNAL_PRICE = impMestMedicine.INTERNAL_PRICE;
                    this.IS_STAR_MARK = impMestMedicine.IS_STAR_MARK;
                    this.MANUFACTURER_CODE = impMestMedicine.MANUFACTURER_CODE;
                    this.MANUFACTURER_ID = impMestMedicine.MANUFACTURER_ID;
                    this.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;
                    this.MATERIAL_NUM_ORDER = impMestMedicine.MATERIAL_NUM_ORDER;
                    this.MEDICINE_BYT_NUM_ORDER = impMestMedicine.MEDICINE_BYT_NUM_ORDER;
                    this.MEDICINE_GROUP_ID = impMestMedicine.MEDICINE_GROUP_ID;
                    this.MEDICINE_ID = impMestMedicine.MEDICINE_ID;///////////////////////////////////////////////
                    this.MEDICINE_IS_STAR_MARK = impMestMedicine.MEDICINE_IS_STAR_MARK;
                    this.MEDICINE_NUM_ORDER = impMestMedicine.MEDICINE_NUM_ORDER;
                    this.MEDICINE_REGISTER_NUMBER = impMestMedicine.MEDICINE_REGISTER_NUMBER;
                    this.MEDICINE_TCY_NUM_ORDER = impMestMedicine.MEDICINE_TCY_NUM_ORDER;
                    this.MEDICINE_TYPE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;/////////////////////////////////////
                    this.MEDICINE_TYPE_ID = impMestMedicine.MEDICINE_TYPE_ID;
                    this.MEDICINE_TYPE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;
                    //this.MEDI_MATE_CHEMI = impMestMedicine.MEDI_MATE_CHEMI;
                    this.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                    this.NOTE = impMestMedicine.NOTE;
                    this.NUM_ORDER = impMestMedicine.NUM_ORDER;
                    this.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                    this.PACKING_TYPE_NAME = impMestMedicine.PACKING_TYPE_NAME;
                    this.PRICE = impMestMedicine.PRICE;
                    this.PROFIT_RATIO = impMestMedicine.PROFIT_RATIO;
                    this.RECORDING_TRANSACTION = impMestMedicine.RECORDING_TRANSACTION;
                    this.REGISTER_NUMBER = impMestMedicine.REGISTER_NUMBER;
                    this.REQ_AMOUNT = impMestMedicine.REQ_AMOUNT;
                    this.SERVICE_ID = impMestMedicine.SERVICE_ID;
                    this.SERVICE_UNIT_CODE = impMestMedicine.SERVICE_UNIT_CODE;
                    this.SERVICE_UNIT_ID = impMestMedicine.SERVICE_UNIT_ID;
                    this.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                    this.SPECIAL_MEDICINE_TYPE = impMestMedicine.SPECIAL_MEDICINE_TYPE;
                    this.SUPPLIER_CODE = impMestMedicine.SUPPLIER_CODE;
                    this.SUPPLIER_ID = impMestMedicine.SUPPLIER_ID;
                    this.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME;
                    this.TCY_NUM_ORDER = impMestMedicine.TCY_NUM_ORDER;
                    this.TDL_IMP_UNIT_CONVERT_RATIO = impMestMedicine.TDL_IMP_UNIT_CONVERT_RATIO;
                    this.TDL_IMP_UNIT_ID = impMestMedicine.TDL_IMP_UNIT_ID;
                    this.TH_EXP_MEST_MEDICINE_ID = impMestMedicine.TH_EXP_MEST_MEDICINE_ID;
                    this.VAT_RATIO = impMestMedicine.VAT_RATIO;
                    this.VIR_PRICE = impMestMedicine.VIR_PRICE;
                    this.EXP_MEST_MEMA_ID = impMestMedicine.ID;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00553RDO(V_HIS_IMP_MEST_BLOOD impMestMedicine)
        {
            // TODO: Complete member initialization
            try
            {
                if (impMestMedicine != null)
                {
                    //this.ACTIVE_INGR_BHYT_CODE = impMestMedicine.ACTIVE_INGR_BHYT_CODE;
                    //this.ACTIVE_INGR_BHYT_NAME = impMestMedicine.ACTIVE_INGR_BHYT_NAME;
                    this.AMOUNT = 1;// impMestMedicine.AMOUNT;
                    this.BID_ID = impMestMedicine.BID_ID;
                    this.BID_NAME = impMestMedicine.BID_NAME;
                    this.BID_NUMBER = impMestMedicine.BID_NUMBER;
                    //this.BYT_NUM_ORDER = impMestMedicine.BYT_NUM_ORDER;
                    //this.CONCENTRA = impMestMedicine.CONCENTRA;
                    //this.CONTRACT_PRICE = impMestMedicine.CONTRACT_PRICE;
                    //this.CONVERT_RATIO = impMestMedicine.CONVERT_RATIO;
                    //this.CONVERT_UNIT_CODE = impMestMedicine.CONVERT_UNIT_CODE;
                    //this.CONVERT_UNIT_NAME = impMestMedicine.CONVERT_UNIT_NAME;
                    this.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE;
                    //this.EXP_MEST_MEMA_ID = impMestMedicine.EXP_MEST_MEMA_ID;///////////////////////////////////
                    this.IMP_PRICE = impMestMedicine.IMP_PRICE;
                    //this.IMP_UNIT_AMOUNT = impMestMedicine.IMP_UNIT_AMOUNT;
                    //this.IMP_UNIT_CODE = impMestMedicine.IMP_UNIT_CODE;
                    //this.IMP_UNIT_NAME = impMestMedicine.IMP_UNIT_NAME;
                    //this.IMP_UNIT_PRICE = impMestMedicine.IMP_UNIT_PRICE;
                    this.IMP_VAT_RATIO = impMestMedicine.IMP_VAT_RATIO;
                    this.INTERNAL_PRICE = impMestMedicine.INTERNAL_PRICE;
                    //this.IS_STAR_MARK = impMestMedicine.IS_STAR_MARK;
                    //this.MANUFACTURER_CODE = impMestMedicine.MANUFACTURER_CODE;
                    //this.MANUFACTURER_ID = impMestMedicine.MANUFACTURER_ID;
                    //this.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;
                    //this.MATERIAL_NUM_ORDER = impMestMedicine.MATERIAL_NUM_ORDER;
                    //this.MEDICINE_BYT_NUM_ORDER = impMestMedicine.MEDICINE_BYT_NUM_ORDER;
                    //this.MEDICINE_GROUP_ID = impMestMedicine.MEDICINE_GROUP_ID;
                    //this.MEDICINE_ID = impMestMedicine.MEDICINE_ID;///////////////////////////////////////////////
                    //this.MEDICINE_IS_STAR_MARK = impMestMedicine.MEDICINE_IS_STAR_MARK;
                    //this.MEDICINE_NUM_ORDER = impMestMedicine.MEDICINE_NUM_ORDER;
                    //this.MEDICINE_REGISTER_NUMBER = impMestMedicine.MEDICINE_REGISTER_NUMBER;
                    //this.MEDICINE_TCY_NUM_ORDER = impMestMedicine.MEDICINE_TCY_NUM_ORDER;
                    //this.MEDICINE_TYPE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;/////////////////////////////////////
                    //this.MEDICINE_TYPE_ID = impMestMedicine.MEDICINE_TYPE_ID;
                    //this.MEDICINE_TYPE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;
                    ////this.MEDI_MATE_CHEMI = impMestMedicine.MEDI_MATE_CHEMI;
                    //this.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                    //this.NOTE = impMestMedicine.NOTE;
                    this.NUM_ORDER = impMestMedicine.NUM_ORDER;
                    this.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                    //this.PACKING_TYPE_NAME = impMestMedicine.PACKING_TYPE_NAME;
                    this.PRICE = impMestMedicine.PRICE;
                    //this.PROFIT_RATIO = impMestMedicine.PROFIT_RATIO;
                    //this.RECORDING_TRANSACTION = impMestMedicine.RECORDING_TRANSACTION;
                    //this.REGISTER_NUMBER = impMestMedicine.REGISTER_NUMBER;
                    //this.REQ_AMOUNT = impMestMedicine.REQ_AMOUNT;
                    this.SERVICE_ID = impMestMedicine.SERVICE_ID;
                    this.SERVICE_UNIT_CODE = impMestMedicine.SERVICE_UNIT_CODE;
                    this.SERVICE_UNIT_ID = impMestMedicine.SERVICE_UNIT_ID;
                    this.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                    //this.SPECIAL_MEDICINE_TYPE = impMestMedicine.SPECIAL_MEDICINE_TYPE;
                    this.SUPPLIER_CODE = impMestMedicine.SUPPLIER_CODE;
                    this.SUPPLIER_ID = impMestMedicine.SUPPLIER_ID;
                    this.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME;
                    //this.TCY_NUM_ORDER = impMestMedicine.TCY_NUM_ORDER;
                    //this.TDL_IMP_UNIT_CONVERT_RATIO = impMestMedicine.TDL_IMP_UNIT_CONVERT_RATIO;
                    //this.TDL_IMP_UNIT_ID = impMestMedicine.TDL_IMP_UNIT_ID;
                    //this.TH_EXP_MEST_MEDICINE_ID = impMestMedicine.TH_EXP_MEST_BLOOD_ID;
                    this.VAT_RATIO = impMestMedicine.VAT_RATIO;
                    this.VIR_PRICE = impMestMedicine.VIR_PRICE;
                    this.EXP_MEST_MEMA_ID = impMestMedicine.ID;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00553RDO(V_HIS_IMP_MEST_MATERIAL impMestMedicine)
        {
            // TODO: Complete member initialization
            try
            {
                if (impMestMedicine != null)
                {
                    //this.ACTIVE_INGR_BHYT_CODE = impMestMedicine.ACTIVE_INGR_BHYT_CODE;
                    //this.ACTIVE_INGR_BHYT_NAME = impMestMedicine.ACTIVE_INGR_BHYT_NAME;
                    this.AMOUNT = impMestMedicine.AMOUNT;
                    this.BID_ID = impMestMedicine.BID_ID;
                    this.BID_NAME = impMestMedicine.BID_NAME;
                    this.BID_NUMBER = impMestMedicine.BID_NUMBER;
                    //this.BYT_NUM_ORDER = impMestMedicine.BYT_NUM_ORDER;
                    //this.CONCENTRA = impMestMedicine.CONCENTRA;
                    this.CONTRACT_PRICE = impMestMedicine.CONTRACT_PRICE;
                    this.CONVERT_RATIO = impMestMedicine.CONVERT_RATIO;
                    this.CONVERT_UNIT_CODE = impMestMedicine.CONVERT_UNIT_CODE;
                    this.CONVERT_UNIT_NAME = impMestMedicine.CONVERT_UNIT_NAME;
                    this.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE;
                    //this.EXP_MEST_MEMA_ID = impMestMedicine.EXP_MEST_MEMA_ID;///////////////////////////////////
                    this.IMP_PRICE = impMestMedicine.IMP_PRICE;
                    this.IMP_UNIT_AMOUNT = impMestMedicine.IMP_UNIT_AMOUNT;
                    this.IMP_UNIT_CODE = impMestMedicine.IMP_UNIT_CODE;
                    this.IMP_UNIT_NAME = impMestMedicine.IMP_UNIT_NAME;
                    this.IMP_UNIT_PRICE = impMestMedicine.IMP_UNIT_PRICE;
                    this.IMP_VAT_RATIO = impMestMedicine.IMP_VAT_RATIO;
                    this.INTERNAL_PRICE = impMestMedicine.INTERNAL_PRICE;
                    //this.IS_STAR_MARK = impMestMedicine.IS_STAR_MARK;
                    this.MANUFACTURER_CODE = impMestMedicine.MANUFACTURER_CODE;
                    this.MANUFACTURER_ID = impMestMedicine.MANUFACTURER_ID;
                    this.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;
                    this.MATERIAL_NUM_ORDER = impMestMedicine.MATERIAL_NUM_ORDER;
                    //this.MEDICINE_BYT_NUM_ORDER = impMestMedicine.MATERIAL_BYT_NUM_ORDER;
                    //this.MEDICINE_GROUP_ID = impMestMedicine.MEDICINE_GROUP_ID;
                    this.MEDICINE_ID = impMestMedicine.MATERIAL_ID;///////////////////////////////////////////////
                    //this.MEDICINE_IS_STAR_MARK = impMestMedicine.MATERIAL_IS_STAR_MARK;
                    this.MEDICINE_NUM_ORDER = impMestMedicine.MEDICINE_NUM_ORDER;
                    //this.MEDICINE_REGISTER_NUMBER = impMestMedicine.MEDICINE_REGISTER_NUMBER;
                    //this.MEDICINE_TCY_NUM_ORDER = impMestMedicine.MEDICINE_TCY_NUM_ORDER;
                    this.MEDICINE_TYPE_CODE = impMestMedicine.MATERIAL_TYPE_CODE;/////////////////////////////////////
                    this.MEDICINE_TYPE_ID = impMestMedicine.MATERIAL_TYPE_ID;
                    this.MEDICINE_TYPE_NAME = impMestMedicine.MATERIAL_TYPE_NAME;
                    //this.MEDI_MATE_CHEMI = impMestMedicine.MEDI_MATE_CHEMI;
                    this.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                    this.NOTE = impMestMedicine.NOTE;
                    this.NUM_ORDER = impMestMedicine.NUM_ORDER;
                    this.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                    this.PACKING_TYPE_NAME = impMestMedicine.PACKING_TYPE_NAME;
                    this.PRICE = impMestMedicine.PRICE;
                    this.PROFIT_RATIO = impMestMedicine.PROFIT_RATIO;
                    this.RECORDING_TRANSACTION = impMestMedicine.RECORDING_TRANSACTION;
                    //this.REGISTER_NUMBER = impMestMedicine.REGISTER_NUMBER;
                    this.REQ_AMOUNT = impMestMedicine.REQ_AMOUNT;
                    this.SERVICE_ID = impMestMedicine.SERVICE_ID;
                    this.SERVICE_UNIT_CODE = impMestMedicine.SERVICE_UNIT_CODE;
                    this.SERVICE_UNIT_ID = impMestMedicine.SERVICE_UNIT_ID;
                    this.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                    //this.SPECIAL_MEDICINE_TYPE = impMestMedicine.SPECIAL_MEDICINE_TYPE;
                    this.SUPPLIER_CODE = impMestMedicine.SUPPLIER_CODE;
                    this.SUPPLIER_ID = impMestMedicine.SUPPLIER_ID;
                    this.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME;
                    //this.TCY_NUM_ORDER = impMestMedicine.TCY_NUM_ORDER;
                    this.TDL_IMP_UNIT_CONVERT_RATIO = impMestMedicine.TDL_IMP_UNIT_CONVERT_RATIO;
                    this.TDL_IMP_UNIT_ID = impMestMedicine.TDL_IMP_UNIT_ID;
                    this.TH_EXP_MEST_MEDICINE_ID = impMestMedicine.TH_EXP_MEST_MATERIAL_ID;
                    this.VAT_RATIO = impMestMedicine.VAT_RATIO;
                    this.VIR_PRICE = impMestMedicine.VIR_PRICE;
                    this.EXP_MEST_MEMA_ID = impMestMedicine.ID;
                    MEDICINE_REGISTER_NUMBER = impMestMedicine.MATERIAL_REGISTER_NUMBER;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


    }

    class V_IMP_MEST : V_HIS_IMP_MEST
    {
        public string KEY_ORDER { get; set; }
        public V_IMP_MEST(V_HIS_IMP_MEST impMest, string keyOrder)
        {
            try
            {
                if (impMest != null)
                {
                    System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST>();
                    foreach (var item in pis)
                    {
                        item.SetValue(this, item.GetValue(impMest));
                    }

                    if (keyOrder != null && keyOrder.Length >0)
                    {
                        this.KEY_ORDER = string.Format(keyOrder,SUPPLIER_NAME,MEDI_STOCK_NAME,IMP_MEST_CODE);
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
