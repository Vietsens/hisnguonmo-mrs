using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPtttTable;

namespace MRS.MANAGER.Config
{
    public class HisPtttTableCFG
    {
        private const string PTTT_TABLE_CODE__GROUP1 = "MRS.HIS_PTTT_TABLE.PTTT_TABLE_CODE.GROUP1";
        private const string PTTT_TABLE_CODE__GROUP2 = "MRS.HIS_PTTT_TABLE.PTTT_TABLE_CODE.GROUP2";
        private const string PTTT_TABLE_CODE__GROUP3 = "MRS.HIS_PTTT_TABLE.PTTT_TABLE_CODE.GROUP3";
        private const string PTTT_TABLE_CODE__GROUP4 = "MRS.HIS_PTTT_TABLE.PTTT_TABLE_CODE.GROUP4";

        private static long PtttTableIdGroup1;
        public static long PTTT_TABLE_ID__GROUP1
        {
            get
            {
                if (PtttTableIdGroup1 == 0)
                {
                    PtttTableIdGroup1 = GetId(PTTT_TABLE_CODE__GROUP1);
                }
                return PtttTableIdGroup1;
            }
            set
            {
                PtttTableIdGroup1 = value;
            }
        }

        private static long PtttTableIdGroup2;
        public static long PTTT_TABLE_ID__GROUP2
        {
            get
            {
                if (PtttTableIdGroup2 == 0)
                {
                    PtttTableIdGroup2 = GetId(PTTT_TABLE_CODE__GROUP2);
                }
                return PtttTableIdGroup2;
            }
            set
            {
                PtttTableIdGroup2 = value;
            }
        }

        private static long PtttTableIdGroup3;
        public static long PTTT_TABLE_ID__GROUP3
        {
            get
            {
                if (PtttTableIdGroup3 == 0)
                {
                    PtttTableIdGroup3 = GetId(PTTT_TABLE_CODE__GROUP3);
                }
                return PtttTableIdGroup3;
            }
            set
            {
                PtttTableIdGroup3 = value;
            }
        }

        private static long PtttTableIdGroup4;
        public static long PTTT_TABLE_ID__GROUP4
        {
            get
            {
                if (PtttTableIdGroup4 == 0)
                {
                    PtttTableIdGroup4 = GetId(PTTT_TABLE_CODE__GROUP4);
                }
                return PtttTableIdGroup4;
            }
            set
            {
                PtttTableIdGroup4 = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_TABLE> PtttTables;
        public static List<MOS.EFMODEL.DataModels.HIS_PTTT_TABLE> PTTT_TABLEs
        {
            get
            {
                if (PtttTables == null)
                {
                    PtttTables = GetAll();
                }
                return PtttTables;
            }
            set
            {
                PtttTables = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_TABLE> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_PTTT_TABLE> result = null;
            try
            {
                HisPtttTableFilterQuery filter = new HisPtttTableFilterQuery();
                result = new HisPtttTableManager().Get(filter);
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
                var data = new HisPtttTableManager(new CommonParam()).GetByCode(value);
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
                PtttTables = null;
                PtttTableIdGroup1 = 0;
                PtttTableIdGroup2 = 0;
                PtttTableIdGroup3 = 0;
                PtttTableIdGroup4 = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
