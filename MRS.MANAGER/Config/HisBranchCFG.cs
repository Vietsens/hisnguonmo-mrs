using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisBranchCFG
    {
        private static List<HIS_BRANCH> hisBranchs;
        public static List<HIS_BRANCH> HisBranchs
        {
            get
            {
                if (hisBranchs == null || hisBranchs.Count == 0)
                {
                    hisBranchs = GetBranch();
                }
                return hisBranchs;
            }
        }

        private static List<HIS_BRANCH> GetBranch()
        {
            List<HIS_BRANCH> result = new List<HIS_BRANCH>();
            try
            {
                result = new HisBranchManager(new CommonParam()).Get(new HisBranchFilterQuery());
                if (result == null) throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<HIS_BRANCH>();
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                hisBranchs = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
