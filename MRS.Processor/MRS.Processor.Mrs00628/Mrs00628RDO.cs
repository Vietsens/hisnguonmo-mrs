using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00628
{
    class Mrs00628RDO
    {
        public decimal AMOUNT { get; set; }
        public string BID_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public string CONCENTRA { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? DISCOUNT { get; set; }
        public string EXP_LOGINNAME { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public long? EXP_TIME { get; set; }
        public string EXP_USERNAME { get; set; }
        public long? EXPIRED_DATE { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public decimal? INTERNAL_PRICE { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public long? MEDI_MATE_NUM_ORDER { get; set; }
        public string MEDI_MATE_REGISTER_NUMBER { get; set; }
        public string MEDI_MATE_TYPE_CODE { get; set; }
        public string MEDI_MATE_TYPE_NAME { get; set; }
        public long? MEDI_MATE_TYPE_NUM_ORDER { get; set; }
        public string NATIONAL_NAME { get; set; }
        public long? NUM_ORDER { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public decimal? TH_AMOUNT { get; set; }
        public string TUTORIAL { get; set; }
        public long? USE_TIME_TO { get; set; }
        public decimal? VAT_RATIO { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public long? DOB { get; set; }

        public Mrs00628RDO(MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_MEDICINE medi, MOS.EFMODEL.DataModels.HIS_EXP_MEST expMest,List<HIS_IMP_MEST_MEDICINE> listImpMedi)
        {
            try
            {
                if (medi != null)
                {
                    this.AMOUNT = medi.AMOUNT;
                    this.BID_NAME = medi.BID_NAME;
                    this.BID_NUMBER = medi.BID_NUMBER;
                    this.CONCENTRA = medi.CONCENTRA;
                    this.DESCRIPTION = medi.DESCRIPTION;
                    this.DISCOUNT = medi.DISCOUNT;
                    this.EXP_MEST_CODE = medi.EXP_MEST_CODE;
                    this.EXP_LOGINNAME = medi.EXP_LOGINNAME;
                    this.EXP_TIME = medi.EXP_TIME;
                    this.EXP_USERNAME = medi.EXP_USERNAME;
                    this.EXPIRED_DATE = medi.EXPIRED_DATE;
                    this.IMP_PRICE = medi.IMP_PRICE;
                    this.IMP_VAT_RATIO = medi.IMP_VAT_RATIO;
                    this.INTERNAL_PRICE = medi.INTERNAL_PRICE;
                    this.MANUFACTURER_CODE = medi.MANUFACTURER_CODE;
                    this.MANUFACTURER_NAME = medi.MANUFACTURER_NAME;
                    this.MEDI_MATE_NUM_ORDER = medi.MEDICINE_NUM_ORDER;
                    this.MEDI_MATE_REGISTER_NUMBER = medi.MEDICINE_REGISTER_NUMBER;
                    this.MEDI_MATE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                    this.MEDI_MATE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                    this.MEDI_MATE_TYPE_NUM_ORDER = medi.MEDICINE_TYPE_NUM_ORDER;
                    this.NATIONAL_NAME = medi.NATIONAL_NAME;
                    this.NUM_ORDER = medi.NUM_ORDER;
                    this.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                    this.PATIENT_TYPE_CODE = medi.PATIENT_TYPE_CODE;
                    this.PATIENT_TYPE_NAME = medi.PATIENT_TYPE_NAME;
                    this.PRICE = medi.PRICE * (1 + (medi.VAT_RATIO ?? 0));
                    this.REGISTER_NUMBER = medi.REGISTER_NUMBER;
                    this.SERVICE_UNIT_CODE = medi.SERVICE_UNIT_CODE;
                    this.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                    this.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                    this.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                    this.TH_AMOUNT = medi.TH_AMOUNT;
                    this.TUTORIAL = medi.TUTORIAL;
                    this.USE_TIME_TO = medi.USE_TIME_TO;
                    this.VAT_RATIO = medi.VAT_RATIO;
                }

                if (expMest != null)
                {
                    this.EXP_MEST_CODE = expMest.EXP_MEST_CODE;
                    this.VIR_PATIENT_NAME = expMest.TDL_PATIENT_NAME;
                    this.DOB = expMest.TDL_PATIENT_DOB;
                    this.GENDER_NAME = expMest.TDL_PATIENT_GENDER_NAME;
                }
                this.TRUST_AMOUNT = this.AMOUNT;
                if (listImpMedi.Exists(o => o.TH_EXP_MEST_MEDICINE_ID == medi.ID))
                {
                    this.TRUST_AMOUNT -= listImpMedi.Where(o => o.TH_EXP_MEST_MEDICINE_ID == medi.ID).Sum(s=>s.AMOUNT);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00628RDO(MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_MATERIAL mate, MOS.EFMODEL.DataModels.HIS_EXP_MEST expMest,List<HIS_IMP_MEST_MATERIAL> listImpMate)
        {
            try
            {
                if (mate != null)
                {
                    this.AMOUNT = mate.AMOUNT;
                    this.BID_NAME = mate.BID_NAME;
                    this.BID_NUMBER = mate.BID_NUMBER;
                    this.DESCRIPTION = mate.DESCRIPTION;
                    this.DISCOUNT = mate.DISCOUNT;
                    this.EXP_MEST_CODE = mate.EXP_MEST_CODE;
                    this.EXP_LOGINNAME = mate.EXP_LOGINNAME;
                    this.EXP_TIME = mate.EXP_TIME;
                    this.EXP_USERNAME = mate.EXP_USERNAME;
                    this.EXPIRED_DATE = mate.EXPIRED_DATE;
                    this.IMP_PRICE = mate.IMP_PRICE;
                    this.IMP_VAT_RATIO = mate.IMP_VAT_RATIO;
                    this.INTERNAL_PRICE = mate.INTERNAL_PRICE;
                    this.MANUFACTURER_CODE = mate.MANUFACTURER_CODE;
                    this.MANUFACTURER_NAME = mate.MANUFACTURER_NAME;
                    this.MEDI_MATE_NUM_ORDER = mate.MATERIAL_NUM_ORDER;
                    this.MEDI_MATE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                    this.MEDI_MATE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                    this.MEDI_MATE_TYPE_NUM_ORDER = mate.MATERIAL_TYPE_NUM_ORDER;
                    this.NATIONAL_NAME = mate.NATIONAL_NAME;
                    this.NUM_ORDER = mate.NUM_ORDER;
                    this.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                    this.PATIENT_TYPE_CODE = mate.PATIENT_TYPE_CODE;
                    this.PATIENT_TYPE_NAME = mate.PATIENT_TYPE_NAME;
                    this.PRICE = mate.PRICE*(1+(mate.VAT_RATIO??0));
                    this.SERVICE_UNIT_CODE = mate.SERVICE_UNIT_CODE;
                    this.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                    this.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                    this.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                    this.TH_AMOUNT = mate.TH_AMOUNT;
                    this.VAT_RATIO = mate.VAT_RATIO;
                }

                if (expMest != null)
                {
                    this.EXP_MEST_CODE = expMest.EXP_MEST_CODE;
                    this.VIR_PATIENT_NAME = expMest.TDL_PATIENT_NAME;
                    this.DOB = expMest.TDL_PATIENT_DOB;
                    this.GENDER_NAME = expMest.TDL_PATIENT_GENDER_NAME;
                }
                this.TRUST_AMOUNT = this.AMOUNT;
                if (listImpMate.Exists(o => o.TH_EXP_MEST_MATERIAL_ID == mate.ID))
                {
                    this.TRUST_AMOUNT -= listImpMate.Where(o => o.TH_EXP_MEST_MATERIAL_ID == mate.ID).Sum(s=>s.AMOUNT);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public decimal TRUST_AMOUNT { get; set; }
    }
}
