using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisInvoiceDetailCFG
    {
        private const string INVOICE_DETAIL__IS_GROUP_CFG = "MOS.INVOICE_DETAIL.IS_GROUP";
        private const string INVOICE_DETAIL__GOODS_GROUP_NAME_CFG = "MOS.INVOICE_DETAIL.GOODS_GROUP_NAME";
        private const string INVOICE_DETAIL__GOODS_GROUP_UNIT_CFG = "MOS.INVOICE_DETAIL.GOODS_GROUP_UNIT";
        private const string INVOICE_DETAIL__HEIN_DESCRIPTION_CFG = "MOS.INVOICE_DETAIL.HEIN_DESCRIPTION";

        private static bool? invoiceDetailIsGroup;
        public static bool INVOICE_DETAIL__IS_GROUP
        {
            get
            {
                if (!invoiceDetailIsGroup.HasValue)
                {
                    invoiceDetailIsGroup = ConfigUtil.GetIntConfig(INVOICE_DETAIL__IS_GROUP_CFG) == 1;
                }
                return invoiceDetailIsGroup.Value;
            }
            set
            {
                invoiceDetailIsGroup = value;
            }
        }

        private static string invoiceDetailGoodsGroupName;
        public static string INVOICE_DETAIL__GOODS_GROUP_NAME
        {
            get
            {
                if (invoiceDetailGoodsGroupName == null)
                {
                    invoiceDetailGoodsGroupName = ConfigUtil.GetStrConfig(INVOICE_DETAIL__GOODS_GROUP_NAME_CFG);
                }
                return invoiceDetailGoodsGroupName;
            }
            set
            {
                invoiceDetailGoodsGroupName = value;
            }
        }

        private static string invoiceDetailGoodsGroupUnit;
        public static string INVOICE_DETAIL__GOODS_GROUP_UNIT
        {
            get
            {
                if (invoiceDetailGoodsGroupUnit == null)
                {
                    invoiceDetailGoodsGroupUnit = ConfigUtil.GetStrConfig(INVOICE_DETAIL__GOODS_GROUP_UNIT_CFG);
                }
                return invoiceDetailGoodsGroupUnit;
            }
            set
            {
                invoiceDetailGoodsGroupUnit = value;
            }
        }

        private static string invoiceDetailHeinDescription;
        public static string INVOICE_DETAIL__HEIN_DESCRIPTION
        {
            get
            {
                if (invoiceDetailHeinDescription == null)
                {
                    invoiceDetailHeinDescription = ConfigUtil.GetStrConfig(INVOICE_DETAIL__HEIN_DESCRIPTION_CFG);
                }
                return invoiceDetailHeinDescription;
            }
            set
            {
                invoiceDetailHeinDescription = value;
            }
        }

        public static void Reload()
        {
            invoiceDetailIsGroup = ConfigUtil.GetIntConfig(INVOICE_DETAIL__IS_GROUP_CFG) == 1;
            invoiceDetailGoodsGroupName = ConfigUtil.GetStrConfig(INVOICE_DETAIL__GOODS_GROUP_NAME_CFG);
            invoiceDetailGoodsGroupUnit = ConfigUtil.GetStrConfig(INVOICE_DETAIL__GOODS_GROUP_UNIT_CFG);
            invoiceDetailHeinDescription = ConfigUtil.GetStrConfig(INVOICE_DETAIL__HEIN_DESCRIPTION_CFG);
        }
    }
}
