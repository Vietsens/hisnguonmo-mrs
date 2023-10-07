using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPtttPriority;

namespace MRS.MANAGER.Config
{
    public class HisPtttPriorityCFG
    {
        private const string PTTT_PRIORITY_CODE__GROUP__P = "MRS.HIS_PTTT_PRIORITY.PTTT_PRIORITY_CODE.PHIEN";
        private const string PTTT_PRIORITY_CODE__GROUP__CC = "MRS.HIS_PTTT_PRIORITY.PTTT_PRIORITY_CODE.CAP_CUU";
        private const string PTTT_PRIORITY_CODE__GROUP__YC = "MRS.HIS_PTTT_PRIORITY.PTTT_PRIORITY_CODE.YEU_CAU";
        //private const string PTTT_PRIORITY_CODE__GROUP__K = "MRS.HIS_PTTT_PRIORITY.PTTT_PRIORITY_CODE.KHAC";

        private static long ptttGroupIdGroupP;
        public static long PTTT_PRIORITY_ID__GROUP__P
        {
            get
            {
                if (ptttGroupIdGroupP == 0)
                {
                    ptttGroupIdGroupP = GetId(PTTT_PRIORITY_CODE__GROUP__P);
                }
                return ptttGroupIdGroupP;
            }
            set
            {
                ptttGroupIdGroupP = value;
            }
        }

        private static long ptttGroupIdGroupCc;
        public static long PTTT_PRIORITY_ID__GROUP__CC
        {
            get
            {
                if (ptttGroupIdGroupCc == 0)
                {
                    ptttGroupIdGroupCc = GetId(PTTT_PRIORITY_CODE__GROUP__CC);
                }
                return ptttGroupIdGroupCc;
            }
            set
            {
                ptttGroupIdGroupCc = value;
            }
        }

        private static long ptttGroupIdGroupYc;
        public static long PTTT_PRIORITY_ID__GROUP__YC
        {
            get
            {
                if (ptttGroupIdGroupYc == 0)
                {
                    ptttGroupIdGroupYc = GetId(PTTT_PRIORITY_CODE__GROUP__YC);
                }
                return ptttGroupIdGroupYc;
            }
            set
            {
                ptttGroupIdGroupYc = value;
            }
        }

        //private static long ptttGroupIdGroupK;
        //public static long PTTT_PRIORITY_ID__GROUP__K
        //{
        //    get
        //    {
        //        if (ptttGroupIdGroupK == 0)
        //        {
        //            ptttGroupIdGroupK = GetId(PTTT_PRIORITY_CODE__GROUP__K);
        //        }
        //        return ptttGroupIdGroupK;
        //    }
        //    set
        //    {
        //        ptttGroupIdGroupK = value;
        //    }
        //}

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_PRIORITY> PtttPrioritys;
        public static List<MOS.EFMODEL.DataModels.HIS_PTTT_PRIORITY> PTTT_PRIORITYs
        {
            get
            {
                if (PtttPrioritys == null)
                {
                    PtttPrioritys = GetAll();
                }
                return PtttPrioritys;
            }
            set
            {
                PtttPrioritys = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PTTT_PRIORITY> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_PTTT_PRIORITY> result = null;
            try
            {
                HisPtttPriorityFilterQuery filter = new HisPtttPriorityFilterQuery();
                result = new HisPtttPriorityManager().Get(filter);
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
                var data = new HisPtttPriorityManager(new CommonParam()).GetByCode(value);
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
                PtttPrioritys = null;
                ptttGroupIdGroupP = 0;
                ptttGroupIdGroupCc = 0;
                ptttGroupIdGroupYc = 0;
                //ptttGroupIdGroupK = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
