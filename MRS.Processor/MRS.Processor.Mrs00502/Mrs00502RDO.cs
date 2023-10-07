using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Core; 

namespace MRS.Processor.Mrs00502
{
    public class Mrs00502RDO
    {
        public long ID { get;  set;  }
        public string INDEX { get;  set;  }

        public decimal JAN { get;  set;  }
        public decimal FEB { get;  set;  }
        public decimal MAR { get;  set;  }
        public decimal APR { get;  set;  }
        public decimal MAY { get;  set;  }
        public decimal JUN { get;  set;  }
        public decimal JUL { get;  set;  }
        public decimal AUG { get;  set;  }
        public decimal SEP { get;  set;  }
        public decimal OCT { get;  set;  }
        public decimal NOV { get;  set;  }
        public decimal DEC { get;  set;  }

        public decimal HEIN_JAN { get;  set;  }
        public decimal HEIN_FEB { get;  set;  }
        public decimal HEIN_MAR { get;  set;  }
        public decimal HEIN_APR { get;  set;  }
        public decimal HEIN_MAY { get;  set;  }
        public decimal HEIN_JUN { get;  set;  }
        public decimal HEIN_JUL { get;  set;  }
        public decimal HEIN_AUG { get;  set;  }
        public decimal HEIN_SEP { get;  set;  }
        public decimal HEIN_OCT { get;  set;  }
        public decimal HEIN_NOV { get;  set;  }
        public decimal HEIN_DEC { get;  set;  }

        public decimal FEE_JAN { get;  set;  }
        public decimal FEE_FEB { get;  set;  }
        public decimal FEE_MAR { get;  set;  }
        public decimal FEE_APR { get;  set;  }
        public decimal FEE_MAY { get;  set;  }
        public decimal FEE_JUN { get;  set;  }
        public decimal FEE_JUL { get;  set;  }
        public decimal FEE_AUG { get;  set;  }
        public decimal FEE_SEP { get;  set;  }
        public decimal FEE_OCT { get;  set;  }
        public decimal FEE_NOV { get;  set;  }
        public decimal FEE_DEC { get;  set;  }

        public decimal ACCUMULATED { get;  set;  }
        public decimal PERCEN { get;  set;  }

        public Mrs00502RDO() { }

        public Mrs00502RDO(long id, string name)
        {
            this.ID = id; 
            this.INDEX = name; 
        }
    }
}
