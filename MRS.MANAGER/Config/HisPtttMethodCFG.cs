using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPtttMethod;

namespace MRS.MANAGER.Config
{
    public class HisPtttMethodCFG
    {
        private const string PTTT_METHOD_CODE__GROUP1 = "MRS.HIS_PTTT_METHOD.PTTT_METHOD_CODE.GROUP1";
        private const string PTTT_METHOD_CODE__GROUP2 = "MRS.HIS_PTTT_METHOD.PTTT_METHOD_CODE.GROUP2";
        private const string PTTT_METHOD_CODE__GROUP3 = "MRS.HIS_PTTT_METHOD.PTTT_METHOD_CODE.GROUP3";
        private const string PTTT_METHOD_CODE__GROUP4 = "MRS.HIS_PTTT_METHOD.PTTT_METHOD_CODE.GROUP4";

        private static long PtttMethodIdGroup1;
        public static long PTTT_METHOD_ID__GROUP1
        {
            get
            {
                if (PtttMethodIdGroup1 == 0)
                {
                    PtttMethodIdGroup1 = GetId(PTTT_METHOD_CODE__GROUP1);
                }
                return PtttMethodIdGroup1;
            }
            set
            {
                PtttMethodIdGroup1 = value;
            }
        }

        private static long PtttMethodIdGroup2;
        public static long PTTT_METHOD_ID__GROUP2
        {
            get
            {
                if (PtttMethodIdGroup2 == 0)
                {
                    PtttMethodIdGroup2 = GetId(PTTT_METHOD_CODE__GROUP2);
                }
                return PtttMethodIdGroup2;
            }
            set
            {
                PtttMethodIdGroup2 = value;
            }
        }

        private static long PtttMethodIdGroup3;
        public static long PTTT_METHOD_ID__GROUP3
        {
            get
            {
                if (PtttMethodIdGroup3 == 0)
                {
                    PtttMethodIdGroup3 = GetId(PTTT_METHOD_CODE__GROUP3);
                }
                return PtttMethodIdGroup3;
            }
            set
            {
                PtttMethodIdGroup3 = value;
            }
        }

        private static long PtttMethodIdGroup4;
        public static long PTTT_METHOD_ID__GROUP4
        {
            get
            {
                if (PtttMethodIdGroup4 == 0)
                {
                    PtttMethodIdGroup4 = GetId(PTTT_METHOD_CODE__GROUP4);
                }
                return PtttMethodIdGroup4;
            }
            set
            {
                PtttMethodIdGroup4 = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_METHOD> PtttMethods;
        public static List<MOS.EFMODEL.DataModels.HIS_PTTT_METHOD> PTTT_METHODs
        {
            get
            {
                if (PtttMethods == null)
                {
                    PtttMethods = GetAll();
                }
                return PtttMethods;
            }
            set
            {
                PtttMethods = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_METHOD> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_PTTT_METHOD> result = null;
            try
            {
                HisPtttMethodFilterQuery filter = new HisPtttMethodFilterQuery();
                result = new HisPtttMethodManager().Get(filter);
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
                var data = new HisPtttMethodManager(new CommonParam()).GetByCode(value);
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
                PtttMethods = null;
                PtttMethodIdGroup1 = 0;
                PtttMethodIdGroup2 = 0;
                PtttMethodIdGroup3 = 0;
                PtttMethodIdGroup4 = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
