using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class MilitaryHeinCFG
    {
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__QD = "MRS.HEIN_CARD_NUMBER.PREFIX.QD";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__QH = "MRS.HEIN_CARD_NUMBER.PREFIX.QH";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__TQ = "MRS.HEIN_CARD_NUMBER.PREFIX.TQ";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__TA4 = "MRS.HEIN_CARD_NUMBER.PREFIX.TA4";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__TY4 = "MRS.HEIN_CARD_NUMBER.PREFIX.TY4";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__TQ497 = "MRS.HEIN_CARD_NUMBER.PREFIX.TQ497";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__A = "MRS.HEIN_CARD_NUMBER.PREFIX.A";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__TE = "MRS.HEIN_CARD_NUMBER.PREFIX.TE";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__NN = "MRS.HEIN_CARD_NUMBER.PREFIX.NN";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__CN = "MRS.HEIN_CARD_NUMBER.PREFIX.CN";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__CB = "MRS.HEIN_CARD_NUMBER.PREFIX.CB";
        private const string HEIN_CARD_NUMBER_PREFIX_CFG__CC = "MRS.HEIN_CARD_NUMBER.PREFIX.CC";

        private static List<string> heinCardNumberPrefixQDs;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__QDS
        {
            get
            {
                if (heinCardNumberPrefixQDs == null)
                {
                    heinCardNumberPrefixQDs = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__QD);
                }
                return heinCardNumberPrefixQDs;
            }
            set
            {
                heinCardNumberPrefixQDs = value;
            }
        }

        private static List<string> heinCardNumberPrefixQHs;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__QHS
        {
            get
            {
                if (heinCardNumberPrefixQHs == null)
                {
                    heinCardNumberPrefixQHs = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__QH);
                }
                return heinCardNumberPrefixQHs;
            }
            set
            {
                heinCardNumberPrefixQHs = value;
            }
        }

        private static List<string> heinCardNumberPrefixTQs;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__TQS
        {
            get
            {
                if (heinCardNumberPrefixTQs == null)
                {
                    heinCardNumberPrefixTQs = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__TQ);
                }
                return heinCardNumberPrefixTQs;
            }
            set
            {
                heinCardNumberPrefixTQs = value;
            }
        }

        private static List<string> heinCardNumberPrefixTQ497s;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__TQ497S
        {
            get
            {
                if (heinCardNumberPrefixTQ497s == null)
                {
                    heinCardNumberPrefixTQ497s = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__TQ497);
                }
                return heinCardNumberPrefixTQ497s;
            }
            set
            {
                heinCardNumberPrefixTQ497s = value;
            }
        }

        private static List<string> heinCardNumberPrefixTY4s;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__TY4S
        {
            get
            {
                if (heinCardNumberPrefixTY4s == null)
                {
                    heinCardNumberPrefixTY4s = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__TY4);
                }
                return heinCardNumberPrefixTY4s;
            }
            set
            {
                heinCardNumberPrefixTY4s = value;
            }
        }

        private static List<string> heinCardNumberPrefixTA4s;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__TA4S
        {
            get
            {
                if (heinCardNumberPrefixTA4s == null)
                {
                    heinCardNumberPrefixTA4s = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__TA4);
                }
                return heinCardNumberPrefixTA4s;
            }
            set
            {
                heinCardNumberPrefixTA4s = value;
            }
        }

        private static List<string> heinCardNumberPrefixAs;
        public static List<string> HEIN_CARD_NUMBER_PREFIX__AS
        {
            get
            {
                if (heinCardNumberPrefixAs == null)
                {
                    heinCardNumberPrefixAs = ConfigUtil.GetStrConfigs(HEIN_CARD_NUMBER_PREFIX_CFG__A);
                }
                return heinCardNumberPrefixAs;
            }
            set
            {
                heinCardNumberPrefixAs = value;
            }
        }

        private static string heinCardNumberPrefixTE;
        public static string HEIN_CARD_NUMBER_PREFIX__TE
        {
            get
            {
                if (heinCardNumberPrefixTE == null)
                {
                    heinCardNumberPrefixTE = ConfigUtil.GetStrConfig(HEIN_CARD_NUMBER_PREFIX_CFG__TE);
                }
                return heinCardNumberPrefixTE;
            }
            set
            {
                heinCardNumberPrefixTE = value;
            }
        }

        private static string heinCardNumberPrefixNN;
        public static string HEIN_CARD_NUMBER_PREFIX__NN
        {
            get
            {
                if (heinCardNumberPrefixNN == null)
                {
                    heinCardNumberPrefixNN = ConfigUtil.GetStrConfig(HEIN_CARD_NUMBER_PREFIX_CFG__NN);
                }
                return heinCardNumberPrefixNN;
            }
            set
            {
                heinCardNumberPrefixNN = value;
            }
        }

        private static string heinCardNumberPrefixCN;
        public static string HEIN_CARD_NUMBER_PREFIX__CN
        {
            get
            {
                if (heinCardNumberPrefixCN == null)
                {
                    heinCardNumberPrefixCN = ConfigUtil.GetStrConfig(HEIN_CARD_NUMBER_PREFIX_CFG__CN);
                }
                return heinCardNumberPrefixCN;
            }
            set
            {
                heinCardNumberPrefixCN = value;
            }
        }

        private static string heinCardNumberPrefixCC;
        public static string HEIN_CARD_NUMBER_PREFIX__CC
        {
            get
            {
                if (heinCardNumberPrefixCC == null)
                {
                    heinCardNumberPrefixCC = ConfigUtil.GetStrConfig(HEIN_CARD_NUMBER_PREFIX_CFG__CC);
                }
                return heinCardNumberPrefixCC;
            }
            set
            {
                heinCardNumberPrefixCC = value;
            }
        }

        private static string heinCardNumberPrefixCB;
        public static string HEIN_CARD_NUMBER_PREFIX__CB
        {
            get
            {
                if (heinCardNumberPrefixCB == null)
                {
                    heinCardNumberPrefixCB = ConfigUtil.GetStrConfig(HEIN_CARD_NUMBER_PREFIX_CFG__CB);
                }
                return heinCardNumberPrefixCB;
            }
            set
            {
                heinCardNumberPrefixCB = value;
            }
        }

        public static void Refresh()
        {
            try
            {
                heinCardNumberPrefixQDs = null;
                heinCardNumberPrefixQHs = null;
                heinCardNumberPrefixTQs = null;
                heinCardNumberPrefixTQ497s = null;
                heinCardNumberPrefixTY4s = null;
                heinCardNumberPrefixTA4s = null;
                heinCardNumberPrefixAs = null;
                heinCardNumberPrefixTE = null;
                heinCardNumberPrefixNN = null;
                heinCardNumberPrefixCN = null;
                heinCardNumberPrefixCC = null;
                heinCardNumberPrefixCB = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
