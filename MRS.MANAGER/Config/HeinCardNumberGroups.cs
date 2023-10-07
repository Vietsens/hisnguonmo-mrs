using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HeinCardNumberGroups
    {
        private const string HEIN_CARD_GROUP__GROUP_1 = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_1";
        private const string HEIN_CARD_GROUP__GROUP_2 = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_2";
        private const string HEIN_CARD_GROUP__GROUP_3 = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_3";
        private const string HEIN_CARD_GROUP__GROUP_4 = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_4";
        private const string HEIN_CARD_GROUP__GROUP_5 = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_5";
        private const string HEIN_CARD_GROUP__GROUP_6 = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_6";
        private const string HEIN_CARD_GROUP__GROUP_OTHER = "MRS.HEIN_CARD_NUMBER.HEIN_CARD_GROUP.GROUP_OTHER";

        private static List<string> group_1;
        public static List<string> Group_1
        {
            get
            {
                if (group_1 == null || group_1.Count == 0)
                {
                    group_1 = Get(HEIN_CARD_GROUP__GROUP_1);
                }
                return group_1;
            }
        }

        private static List<string> group_2;
        public static List<string> Group_2
        {
            get
            {
                if (group_2 == null || group_2.Count == 0)
                {
                    group_2 = Get(HEIN_CARD_GROUP__GROUP_2);
                }
                return group_2;
            }
        }

        private static List<string> group_3;
        public static List<string> Group_3
        {
            get
            {
                if (group_3 == null || group_3.Count == 0)
                {
                    group_3 = Get(HEIN_CARD_GROUP__GROUP_3);
                }
                return group_3;
            }
        }

        private static List<string> group_4;
        public static List<string> Group_4
        {
            get
            {
                if (group_4 == null || group_4.Count == 0)
                {
                    group_4 = Get(HEIN_CARD_GROUP__GROUP_4);
                }
                return group_4;
            }
        }

        private static List<string> group_5;
        public static List<string> Group_5
        {
            get
            {
                if (group_5 == null || group_5.Count == 0)
                {
                    group_5 = Get(HEIN_CARD_GROUP__GROUP_5);
                }
                return group_5;
            }
        }

        private static List<string> group_6;
        public static List<string> Group_6
        {
            get
            {
                if (group_6 == null || group_6.Count == 0)
                {
                    group_6 = Get(HEIN_CARD_GROUP__GROUP_6);
                }
                return group_6;
            }
        }

        private static short group_other;
        public static short Group_Other
        {
            get
            {
                group_other = GetOther(HEIN_CARD_GROUP__GROUP_OTHER);
                return group_other;
            }
        }

        public static List<string> Get(string code)
        {
            List<string> result = new List<string>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                result.AddRange(value.Split(new char[] { ',' }));
                if (result == null) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => result), result));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<string>();
            }
            return result;
        }

        public static short GetOther(string code)
        {
            short result;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                result = Convert.ToInt16(value);
                if (result == null) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => result), result));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        internal static void Refresh()
        {
            try
            {
                group_1 = null;
                group_2 = null;
                group_3 = null;
                group_4 = null;
                group_5 = null;
                group_6 = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
