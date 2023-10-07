using MRS.Processor.Mrs00529;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00529
{
    public class Mrs00529RDO : HIS_SERE_SERV
    {
        public string EXECUTE_ROOM_CODE { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public decimal AMOUNT_BHYT { get; set; }
        public decimal TOTAL_PRICE_BHYT { get; set; }
        public decimal AMOUNT_VP { get; set; }
        public decimal TOTAL_PRICE_VP { get; set; }


        public Mrs00529RDO(HIS_SERE_SERV r, List<V_HIS_ROOM> listHisRoom)
        {
            PropertyInfo[] p = typeof(HIS_SERE_SERV).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            var room = listHisRoom.FirstOrDefault(o=>o.ID==r.TDL_EXECUTE_ROOM_ID)??new V_HIS_ROOM();
            this.EXECUTE_ROOM_CODE = room.ROOM_CODE;
            this.EXECUTE_ROOM_NAME = room.ROOM_NAME;
            this.EXECUTE_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
            this.EXECUTE_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
            if (r.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                this.AMOUNT_BHYT = r.AMOUNT;
                this.TOTAL_PRICE_BHYT = r.VIR_TOTAL_PRICE ?? 0;

            }
            else
            {
                this.AMOUNT_VP = r.AMOUNT;
                this.TOTAL_PRICE_VP = r.VIR_TOTAL_PRICE??0;

            }
        }

        public Mrs00529RDO()
        {

        }
    }
}
