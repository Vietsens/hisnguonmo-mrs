using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00313
{
    public class Mrs00313RDO : V_HIS_SERE_SERV
    {

        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public long COUNT_TOTAL { get; set; }
        public long COUNT_IN { get; set; }
        public long COUNT_TT { get; set; }
        public long COUNT_BHYT { get; set; }
        public long COUNT_TE { get; set; }
        public long COUNT_DV { get; set; }
        public long COUNT_IN_1 { get; set; }
        public long COUNT_BHYT_1 { get; set; }
        public long COUNT_TE_1 { get; set; }
        public long COUNT_DV_1 { get; set; }
        public long COUNT_NGT { get; set; }
        public long COUNT_EXAM_TRAN_PATI { get; set; }
        public long COUNT_TREAT_TRAN_PATI { get; set; }

        public long AMOUNT_MALE_BHYT { get; set; }
        public long AMOUNT_FEMALE_BHYT { get; set; }
        public long AMOUNT_UNDER6_BHYT { get; set; }
        public long AMOUNT_UNDER16_BHYT { get; set; }
        public long AMOUNT_OVER60_BHYT { get; set; }
        public long AMOUNT_SUM { get; set; }
        public long AMOUNT_BHYT { get; set; }
        public long AMOUNT_MALE_VP { get; set; }
        public long AMOUNT_FEMALE_VP { get; set; }
        public long AMOUNT_UNDER6_VP { get; set; }
        public long AMOUNT_UNDER16_VP { get; set; }
        public long AMOUNT_OVER60_VP { get; set; }
        public long AMOUNT_VP { get; set; }
        public long AMOUNT_IN { get; set; }
        public long AMOUNT_TRAN { get; set; }
        public long AMOUNT_BHYT_TRAN { get; set; }
    }
    public class DepartmentAmount
    {
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public long AMOUNT_VP { get; set; }
        public long AMOUNT_BHYT { get; set; }
        public long AMOUNT { get; set; }
    }

    public class DATE_MONTH_AMOUNT
    {
        public string DATE_MONTH_STR { get; set; }
        public long DATE_MONTH_KEY { get; set; }
        public Dictionary<string,decimal> DIC_AMOUNT { get; set; }
    }
}
