using Inventec.Common.Logging;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using Inventec.Token.ResourceSystem;
using MOS.EFMODEL.DataModels;
using MOS.SDO;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Manager
{
    internal sealed class TokenManager : BusinessBase
    {
        public TokenManager()
            : base()
        {

        }

        public TokenManager(CommonParam param)
            : base(param)
        {

        }

        private static string WorkPlaceKey
        {
            get
            {
                return string.Format("{0}|{1}", ResourceTokenManager.GetApplicationCode(), "WORK_PLACE");
            }
        }

        private static string BranchKey
        {
            get
            {
                return string.Format("{0}|{1}", ResourceTokenManager.GetApplicationCode(), "BRANCH_KEY");
            }
        }

        internal static List<WorkPlaceSDO> GetWorkPlaceList()
        {
            List<WorkPlaceSDO> workPlaceSdos = ResourceTokenManager.GetCredentialData<List<WorkPlaceSDO>>(WorkPlaceKey);
            if (workPlaceSdos == null)
            {
                LogSystem.Warn("workPlaceList null");
            }
            return workPlaceSdos;
        }

        public HIS_BRANCH GetBranch()
        {
            HIS_BRANCH branch = ResourceTokenManager.GetCredentialData<HIS_BRANCH>(BranchKey);
            if (branch == null)
            {
                LogSystem.Warn("branch null");
            }
            return branch;
        }
    }
}
