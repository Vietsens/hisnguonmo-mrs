using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class LimitHeinMedicinePriceCFG
    {
        private const string LIMIT_HEIN_MEDICINE_PRICE__RIGHT_MEDI_ORG_CFG = "MOS.LIMIT_HEIN_MEDICINE_PRICE.RIGHT_MEDI_ORG";
        private const string LIMIT_HEIN_MEDICINE_PRICE__TRANPATI_CFG = "MOS.LIMIT_HEIN_MEDICINE_PRICE.TRAN_PATI";

        private static decimal? limitHeinMedicinePriceRightMediOrg;
        public static decimal? LIMIT_HEIN_MEDICINE_PRICE__RIGHT_MEDI_ORG
        {
            get
            {
                if (!limitHeinMedicinePriceRightMediOrg.HasValue)
                {
                    limitHeinMedicinePriceRightMediOrg = ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__RIGHT_MEDI_ORG_CFG) == null ? -1 : ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__RIGHT_MEDI_ORG_CFG);//set bang -1 de ko query lai nhieu lan
                }
                return limitHeinMedicinePriceRightMediOrg;
            }
            set
            {
                limitHeinMedicinePriceRightMediOrg = value;
            }
        }

        private static decimal? limitHeinMedicinePriceTranpati;
        public static decimal? LIMIT_HEIN_MEDICINE_PRICE__TRANPATI
        {
            get
            {
                if (!limitHeinMedicinePriceTranpati.HasValue)
                {
                    limitHeinMedicinePriceTranpati = ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__TRANPATI_CFG) == null ? -1 : ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__TRANPATI_CFG);//set bang -1 de ko query lai nhieu lan
                }
                return limitHeinMedicinePriceTranpati;
            }
            set
            {
                limitHeinMedicinePriceTranpati = value;
            }
        }

        public static void Reload()
        {
            var priceMediOrg = ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__RIGHT_MEDI_ORG_CFG) == null ? -1 : ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__RIGHT_MEDI_ORG_CFG);
            var priceTranPati = ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__TRANPATI_CFG) == null ? -1 : ConfigUtil.GetDecimalConfig(LIMIT_HEIN_MEDICINE_PRICE__TRANPATI_CFG);

            limitHeinMedicinePriceTranpati = priceTranPati;
            limitHeinMedicinePriceRightMediOrg = priceMediOrg;
        }
    }
}
