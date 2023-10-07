using MOS.MANAGER.HisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00268
{
    class Mrs00268RDO
    {
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public Decimal BEGIN_AMOUNT { get; set; }
        public Decimal IMP_PRICE { get; set; }
        public Decimal BEGIN_MONEY { get; set; }
        public Decimal IMP_MANU_AMOUNT { get; set; }
        public Decimal IMP_CHMS_AMOUNT { get; set; }
        public Decimal IMP_MOBA_AMOUNT { get; set; }
        public Decimal IMP_ANOTHER_AMOUNT { get; set; }
        public Decimal IMP_AMOUNT { get; set; }
        public Decimal IMP_MONEY { get; set; }
        public Decimal EXP_PRES_AMOUNT { get; set; }
        public Decimal EXP_DEPA_AMOUNT { get; set; }
        public Decimal EXP_CHMS_AMOUNT { get; set; }
        public Decimal EXP_MANU_AMOUNT { get; set; }
        public Decimal EXP_EXPE_AMOUNT { get; set; }
        public Decimal EXP_LOST_AMOUNT { get; set; }
        public Decimal EXP_SALE_AMOUNT { get; set; }
        public Decimal EXP_LIQU_AMOUNT { get; set; }
        public Decimal EXP_ANOTHER_AMOUNT { get; set; }
        public Decimal EXP_AMOUNT { get; set; }
        public Decimal EXP_MONEY { get; set; }
        public Decimal END_AMOUNT { get; set; }
        public Decimal END_MONEY { get; set; }
        public string PARENT_NAME { get; set; }

        public Dictionary<string, decimal> DIC_EXP_MEST_REASON { get; set; }

        public Mrs00268RDO(V_HIS_MEST_PERIOD_MATE r, List<V_HIS_MATERIAL_TYPE> listMaterialType)
        {
            var materialType = listMaterialType.FirstOrDefault(o => o.ID == r.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE();
            var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID) ?? new V_HIS_MATERIAL_TYPE();

            this.SERVICE_CODE = materialType.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = materialType.MATERIAL_TYPE_NAME;
            this.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            this.PARENT_NAME = parent.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.BEGIN_MONEY = r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.BEGIN_AMOUNT = r.AMOUNT;
            this.END_MONEY = r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.END_AMOUNT = r.AMOUNT;
        }

        public Mrs00268RDO(V_HIS_IMP_MEST_MATERIAL r, List<V_HIS_MATERIAL_TYPE> listMaterialType, bool before)
        {
            var materialType = listMaterialType.FirstOrDefault(o => o.ID == r.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE();
            var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID) ?? new V_HIS_MATERIAL_TYPE();

            this.SERVICE_CODE = materialType.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = materialType.MATERIAL_TYPE_NAME;
            this.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            this.PARENT_NAME = parent.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            if (before)
            {
                this.BEGIN_MONEY = r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
                this.BEGIN_AMOUNT = r.AMOUNT;

            }
            else
            {
                this.IMP_AMOUNT= r.AMOUNT;
                this.IMP_MONEY = r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
                if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC) this.IMP_MANU_AMOUNT = r.AMOUNT;
                else if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL || r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS) this.IMP_CHMS_AMOUNT = r.AMOUNT;
                else if (r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH || r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL || r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL || r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL || r.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL) this.IMP_MOBA_AMOUNT = r.AMOUNT;
                else this.IMP_ANOTHER_AMOUNT = r.AMOUNT;
            }
            this.END_MONEY = r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.END_AMOUNT = r.AMOUNT;
        }

        public Mrs00268RDO(V_HIS_EXP_MEST_MATERIAL r, List<V_HIS_MATERIAL_TYPE> listMaterialType, List<ExpMestIdReason> ExpMestIdReasons, bool before)
        {
            var materialType = listMaterialType.FirstOrDefault(o => o.ID == r.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE();
            var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID) ?? new V_HIS_MATERIAL_TYPE();

            this.SERVICE_CODE = materialType.MATERIAL_TYPE_CODE;
            this.SERVICE_NAME = materialType.MATERIAL_TYPE_NAME;
            this.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
            this.SUPPLIER_NAME = r.SUPPLIER_NAME;
            this.PARENT_NAME = parent.MATERIAL_TYPE_NAME;
            this.IMP_PRICE = r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            if (before)
            {
                this.BEGIN_MONEY = -r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
                this.BEGIN_AMOUNT = -r.AMOUNT;

            }
            else
            {
                this.EXP_AMOUNT = r.AMOUNT;
                this.EXP_MONEY = r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
                if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC) this.EXP_MANU_AMOUNT = r.AMOUNT;
                //else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL) this.EXP_LOST_AMOUNT = r.AMOUNT;
                //else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL) this.EXP_EXPE_AMOUNT = r.AMOUNT;
                //else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM) this.EXP_LIQU_AMOUNT = r.AMOUNT;
                else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) this.EXP_DEPA_AMOUNT = r.AMOUNT;
                else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK || r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS || r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL) this.EXP_CHMS_AMOUNT = r.AMOUNT;
                else if ((r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK || r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL || r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__THPK || r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT || r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT)) this.EXP_PRES_AMOUNT = r.AMOUNT;
                else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN) this.EXP_SALE_AMOUNT = r.AMOUNT;
                else if (r.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCT) this.EXP_BCT_AMOUNT = r.AMOUNT;
                else this.EXP_ANOTHER_AMOUNT = r.AMOUNT;
                var expMest = ExpMestIdReasons.FirstOrDefault(o => o.EXP_MEST_ID == r.EXP_MEST_ID);
                this.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                if (expMest != null && expMest.EXP_MEST_REASON_CODE != null)
                {

                    this.DIC_EXP_MEST_REASON.Add(expMest.EXP_MEST_REASON_CODE, r.AMOUNT);
                }
            }
            this.END_MONEY = -r.AMOUNT * r.IMP_PRICE * (1 + r.IMP_VAT_RATIO);
            this.END_AMOUNT = -r.AMOUNT;
        }

        public Mrs00268RDO()
        {
            // TODO: Complete member initialization
        }
        public decimal EXP_BCT_AMOUNT { get; set; }


    }

    public class ExpMestIdReason
    {
        public long EXP_MEST_ID { get; set; }

        public string EXP_MEST_REASON_CODE { get; set; }
    }
}
