using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00173
{
    public class VSarReportMrs00173RDO
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string LOGINNAME { get; set; }
        public string USERNAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public string EXECUTE_ROLE_CODE { get; set; }
        public string PTTT_GROUP_CODE { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public long? PATIENT_ID { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MAIN_EXECUTE { get; set; }
        public string EXTRA_EXECUTE { get; set; }
        public string HELPING { get; set; }
        public string SERVICE_NAME { get; set; }
        public Dictionary<string, decimal> DIC_ROLE_GROUP { get; set; }
        
        public VSarReportMrs00173RDO() 
        {
            DIC_ROLE_GROUP = new Dictionary<string, decimal>();
        }

        public long? EXECUTE_TIME { get; set; }

        public string PATIENT_CODE { get; set; }

        public string EXECUTE_TIME_STR { get; set; }

        public long EXECUTE_ROLE_ID { get; set; }

        public string START_TIME_STR { get; set; }
    }
    public class PTTT_DETAIL:HIS_SERE_SERV
    {

        public PTTT_DETAIL(HIS_SERE_SERV r, HIS_SERVICE_REQ req, HIS_SERE_SERV_EXT sse)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<PTTT_DETAIL>(this, r);
            this.REQUEST_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
            this.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
            this.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
            this.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
            this.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            this.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            this.EXECUTE_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            this.EXECUTE_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            this.TDL_SERVICE_NAME = r.TDL_SERVICE_NAME;
            if (req != null)
            {
                this.START_TIME = req.START_TIME;
                this.FINISH_TIME = req.FINISH_TIME;
                this.EXECUTE_LOGINNAME = req.EXECUTE_LOGINNAME;
                this.EXECUTE_USERNAME = req.EXECUTE_USERNAME;
                //this.TDL_PATIENT_NAME = req.TDL_PATIENT_NAME;
            }
            if (sse != null)
            {
                this.BEGIN_TIME = sse.BEGIN_TIME;
                this.END_TIME = sse.END_TIME;
            }
        }
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_CODE { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public long? START_TIME { get; set; }
        public long? FINISH_TIME { get; set; }
        //public string TDL_PATIENT_NAME { get; set; }
        public string TDL_SERVICE_NAME { get; set; }

        public long? BEGIN_TIME { get; set; }

        public long? END_TIME { get; set; }
    }
}
