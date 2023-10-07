using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00643
{
    public class Mrs00643GDO
    {
        public List<HIS_TREATMENT> listHisTreatment { get; set; }
        public List<HIS_SERE_SERV> listHisSereServ { get; set; }
    }
}
