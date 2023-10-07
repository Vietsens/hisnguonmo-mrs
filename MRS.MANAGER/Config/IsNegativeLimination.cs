using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class IsNegativeLiminationCFG
    {
        private const string NEGATIVE_AMOUNT_ELIMINATION = "MRS.IS_NEGATIVE_AMOUNT_ELIMINATION";

        private static string IsNegativeLimination;
        public static string IS_NEGATIVE_AMOUNT_ELIMINATION
        {
            get
            {
                if (IsNegativeLimination == null)
                {
                    IsNegativeLimination = ConfigUtil.GetStrConfig(NEGATIVE_AMOUNT_ELIMINATION);
                }
                return IsNegativeLimination;
            }
            set
            {
                IsNegativeLimination = value;
            }
        }

        public static void Refresh()
        {
            try
            {
                IsNegativeLimination = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
