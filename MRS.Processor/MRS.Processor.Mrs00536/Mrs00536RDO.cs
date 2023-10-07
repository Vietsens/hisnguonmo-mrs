using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00536
{
    class Mrs00536RDO : V_HIS_MEDICINE_TYPE
    {
        public decimal IMP_AMOUNT { get; set; }
        public decimal CK_IMP_AMOUNT { get; set; }
        public decimal EXP_AMOUNT { get; set; }
        public decimal CK_EXP_AMOUNT { get; set; }
    }
}
