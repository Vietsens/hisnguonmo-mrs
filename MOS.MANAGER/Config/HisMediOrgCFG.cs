using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMediOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    class HisMediOrgCFG
    {
        private static List<HIS_MEDI_ORG> data;
        public static List<HIS_MEDI_ORG> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisMediOrgGet().Get(new HisMediOrgFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public static void Reload()
        {
            var datas = new HisMediOrgGet().Get(new HisMediOrgFilterQuery());
            data = datas;
        }
    }
}
