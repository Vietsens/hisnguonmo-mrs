using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00634
{
    class Mrs00634RDO
    {
        public long TDL_TREATMENT_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public HIS_SERE_SERV sereServ { get; set; }
        public HIS_SERVICE_REQ HisServiceReq { get; set; }
        public HIS_TRANSACTION HisTransaction { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_ROOM_CODE { get; set; }

        public Mrs00634RDO() { }

        public Mrs00634RDO(HIS_SERE_SERV data, HIS_SERVICE_REQ serviceReq, HIS_TRANSACTION transaction)
        {
            try
            {
                TDL_TREATMENT_ID = data.TDL_TREATMENT_ID ?? 0;
                VIR_TOTAL_PRICE = data.VIR_TOTAL_PRICE ?? 0;
                PATIENT_TYPE_ID = data.PATIENT_TYPE_ID;
                PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(q => q.ID == data.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                sereServ = data;
                HisServiceReq = serviceReq;
                HisTransaction = transaction;

                TOTAL_AMOUNT = data.AMOUNT;
                var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.EXECUTE_ROOM_ID);
                if (room != null)
                {
                    EXECUTE_ROOM_NAME = room.ROOM_NAME;
                    EXECUTE_ROOM_CODE = room.ROOM_CODE;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
