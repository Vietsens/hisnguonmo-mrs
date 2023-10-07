using MOS.MANAGER.HisMilitaryRank;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisMilitaryRankCFG
    {
        private static string HIS_MILITARY_RANK_MILITARY_RANK_CODE__SENIOR = "MRS.HIS_MILITARY_RANK.MILITARY_RANK_CODE__SENIOR";//Cac ma quan ham tuong ung voi can bo cao cap
        private static string HIS_MILITARY_RANK_MILITARY_RANK_CODE__GENERAL = "MRS.HIS_MILITARY_RANK.MILITARY_RANK_CODE__GENERAL";//tuong
        private static string HIS_MILITARY_RANK_MILITARY_RANK_CODE__3AND4SLASH = "MRS.HIS_MILITARY_RANK.MILITARY_RANK_CODE__3AND4SLASH";//thuong ta, dai ta
        private static string HIS_MILITARY_RANK_MILITARY_RANK_CODE__1AND2SLASH = "MRS.HIS_MILITARY_RANK.MILITARY_RANK_CODE__1AND2SLASH";//thieu ta, trung ta
        private static string HIS_MILITARY_RANK_MILITARY_RANK_CODE__LIEUTENANT = "MRS.HIS_MILITARY_RANK.MILITARY_RANK_CODE__LIEUTENANT";//cap uy
        private static string HIS_MILITARY_RANK_MILITARY_RANK_CODE__HSQCS = "MRS.HIS_MILITARY_RANK.MILITARY_RANK_CODE__HSQCS";//Ha si quan, chien si

        private static List<long> militaryRankIds_General;
        public static List<long> HIS_MILITARY_RANK_ID__GENERAL
        {
            get
            {
                if (militaryRankIds_General == null || militaryRankIds_General.Count == 0)
                {
                    militaryRankIds_General = GetIds(HIS_MILITARY_RANK_MILITARY_RANK_CODE__GENERAL);
                }
                return militaryRankIds_General;
            }
            set
            {
                militaryRankIds_General = value;
            }
        }

        private static List<long> militaryRankIds_Senior;
        public static List<long> HIS_MILITARY_RANK_ID__SENIOR
        {
            get
            {
                if (militaryRankIds_Senior == null || militaryRankIds_Senior.Count == 0)
                {
                    militaryRankIds_Senior = GetIds(HIS_MILITARY_RANK_MILITARY_RANK_CODE__SENIOR);
                }
                return militaryRankIds_Senior;
            }
            set
            {
                militaryRankIds_Senior = value;
            }
        }

        private static List<long> militaryRankIds_3And4Slash;
        public static List<long> HIS_MILITARY_RANK_ID__3AND4SLASH
        {
            get
            {
                if (militaryRankIds_3And4Slash == null || militaryRankIds_3And4Slash.Count == 0)
                {
                    militaryRankIds_3And4Slash = GetIds(HIS_MILITARY_RANK_MILITARY_RANK_CODE__3AND4SLASH);
                }
                return militaryRankIds_3And4Slash;
            }
            set
            {
                militaryRankIds_3And4Slash = value;
            }
        }

        private static List<long> militaryRankIds_1And2Slash;
        public static List<long> HIS_MILITARY_RANK_ID__1AND2SLASH
        {
            get
            {
                if (militaryRankIds_1And2Slash == null || militaryRankIds_1And2Slash.Count == 0)
                {
                    militaryRankIds_1And2Slash = GetIds(HIS_MILITARY_RANK_MILITARY_RANK_CODE__1AND2SLASH);
                }
                return militaryRankIds_1And2Slash;
            }
            set
            {
                militaryRankIds_1And2Slash = value;
            }
        }

        private static List<long> militaryRankIds_Lieutenant;
        public static List<long> HIS_MILITARY_RANK_ID__LIEUTENANT
        {
            get
            {
                if (militaryRankIds_Lieutenant == null || militaryRankIds_Lieutenant.Count == 0)
                {
                    militaryRankIds_Lieutenant = GetIds(HIS_MILITARY_RANK_MILITARY_RANK_CODE__LIEUTENANT);
                }
                return militaryRankIds_Lieutenant;
            }
            set
            {
                militaryRankIds_Lieutenant = value;
            }
        }

        private static List<long> militaryRankIds_Hsqcs;
        public static List<long> HIS_MILITARY_RANK_ID__HSQCS
        {
            get
            {
                if (militaryRankIds_Hsqcs == null || militaryRankIds_Hsqcs.Count == 0)
                {
                    militaryRankIds_Hsqcs = GetIds(HIS_MILITARY_RANK_MILITARY_RANK_CODE__HSQCS);
                }
                return militaryRankIds_Hsqcs;
            }
            set
            {
                militaryRankIds_Hsqcs = value;
            }
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = null;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisMilitaryRankFilterQuery filter = new HisMilitaryRankFilterQuery();
                //filter.DEPARTMENT_CODE = value;//TODO
                var data = new HisMilitaryRankManager().Get(filter).Where(o => value.Contains(o.MILITARY_RANK_CODE)).ToList();
                if (!(data != null && data.Count > 0)) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.Select(o => o.ID).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                militaryRankIds_General = null;
                militaryRankIds_Senior = null;
                militaryRankIds_3And4Slash = null;
                militaryRankIds_1And2Slash = null;
                militaryRankIds_Lieutenant = null;
                militaryRankIds_Hsqcs = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
