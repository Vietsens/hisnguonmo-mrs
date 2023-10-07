using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPtttGroup;

namespace MRS.MANAGER.Config
{
    public class HisPtttGroupCFG
    {
        private const string PTTT_GROUP_CODE__GROUP1 = "MRS.HIS_PTTT_GROUP.PTTT_GROUP_CODE.GROUP1";
        private const string PTTT_GROUP_CODE__GROUP2 = "MRS.HIS_PTTT_GROUP.PTTT_GROUP_CODE.GROUP2";
        private const string PTTT_GROUP_CODE__GROUP3 = "MRS.HIS_PTTT_GROUP.PTTT_GROUP_CODE.GROUP3";
        private const string PTTT_GROUP_CODE__GROUP4 = "MRS.HIS_PTTT_GROUP.PTTT_GROUP_CODE.GROUP4";

        private static long ptttGroupIdGroup1;
        public static long PTTT_GROUP_ID__GROUP1
        {
            get
            {
                if (ptttGroupIdGroup1 == 0)
                {
                    ptttGroupIdGroup1 = GetId(PTTT_GROUP_CODE__GROUP1);
                }
                return ptttGroupIdGroup1;
            }
            set
            {
                ptttGroupIdGroup1 = value;
            }
        }

        private static long ptttGroupIdGroup2;
        public static long PTTT_GROUP_ID__GROUP2
        {
            get
            {
                if (ptttGroupIdGroup2 == 0)
                {
                    ptttGroupIdGroup2 = GetId(PTTT_GROUP_CODE__GROUP2);
                }
                return ptttGroupIdGroup2;
            }
            set
            {
                ptttGroupIdGroup2 = value;
            }
        }

        private static long ptttGroupIdGroup3;
        public static long PTTT_GROUP_ID__GROUP3
        {
            get
            {
                if (ptttGroupIdGroup3 == 0)
                {
                    ptttGroupIdGroup3 = GetId(PTTT_GROUP_CODE__GROUP3);
                }
                return ptttGroupIdGroup3;
            }
            set
            {
                ptttGroupIdGroup3 = value;
            }
        }

        private static long ptttGroupIdGroup4;
        public static long PTTT_GROUP_ID__GROUP4
        {
            get
            {
                if (ptttGroupIdGroup4 == 0)
                {
                    ptttGroupIdGroup4 = GetId(PTTT_GROUP_CODE__GROUP4);
                }
                return ptttGroupIdGroup4;
            }
            set
            {
                ptttGroupIdGroup4 = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_GROUP> PtttGroups;
        public static List<MOS.EFMODEL.DataModels.HIS_PTTT_GROUP> PTTT_GROUPs
        {
            get
            {
                if (PtttGroups == null)
                {
                    PtttGroups = GetAll();
                }
                return PtttGroups;
            }
            set
            {
                PtttGroups = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_GROUP> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_PTTT_GROUP> result = null;
            try
            {
                HisPtttGroupFilterQuery filter = new HisPtttGroupFilterQuery();
                result = new HisPtttGroupManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        private static long GetId(string code)
        {
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisPtttGroupManager(new CommonParam()).GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                PtttGroups = null;
                ptttGroupIdGroup1 = 0;
                ptttGroupIdGroup2 = 0;
                ptttGroupIdGroup3 = 0;
                ptttGroupIdGroup4 = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
