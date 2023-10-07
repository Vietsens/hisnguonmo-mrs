using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00716
{
    public class Mrs00716RDO
    {
        public int COUNT_NAM_BHYT { get; set; }
        public int COUNT_NAM_ND { get; set; }
        public int COUNT_NAM_BOARDING_BHYT { get; set; }
        public int COUNT_NAM_BOARDING_ND { get; set; }
        public int COUNT_NU_BHYT { get; set; }
        public int COUNT_NU_ND { get; set; }
        public int COUNT_NU_BOARDING_BHYT { get; set; }
        public int COUNT_NU_BOARDING_ND { get; set; }
        public int COUNT_CHILD_LESS6_BHYT { get; set; }
        public int COUNT_CHILD_LESS6_ND { get; set; }
        public int COUNT_CHILD_LESS6_BOARDING_BHYT{ get; set; }
        public int COUNT_CHILD_LESS6_BOARDING_ND { get; set; }
        public int COUNT_CHILD_LESS15_BHYT { get; set; }
        public int COUNT_CHILD_LESS15_ND { get; set; }
        public int COUNT_CHILD_LESS15_BOARDING_BHYT { get; set; }
        public int COUNT_CHILD_LESS15_BOARDING_ND { get; set; }
        public int COUNT_OLD_MOREIS_EQUAL60_BHYT { get; set; }
        public int COUNT_OLD_MOREIS_EQUAL60_ND { get; set; }
        public int COUNT_OLD_MOREIS_EQUAL60_BOARDING_BHYT { get; set; }
        public int COUNT_OLD_MOREIS_EQUAL60_BOARDING_ND { get; set; }
        public int COUNT_TRANSFER_EXAM { get; set; }
        public int COUNT_TRANSFER_BOARDING { get; set; }
        public int COUNT_TRANSFER_EXTERNAL { get; set; }

    }
}
