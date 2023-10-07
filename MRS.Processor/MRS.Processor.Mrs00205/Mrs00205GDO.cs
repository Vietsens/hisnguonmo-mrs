using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.Logging;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00205
{
    public class Mrs00205GDO
    {
        public List<HIS_TREATMENT> HIS_TREATMENT { get; set; }
        public List<HIS_SERE_SERV> HIS_SERE_SERV { get; set; }
        public List<HIS_TRANSACTION> HIS_TRANSACTION { get; set; }
        public  List<HIS_TREATMENT_RESULT> HIS_TREATMENT_RESULT { set; get; }
        public List<HIS_SERVICE_REQ> HIS_SERVICE_REQ { set; get; }
        public List<V_HIS_BED_LOG> V_HIS_BED_LOG { set; get; }
    }
}
