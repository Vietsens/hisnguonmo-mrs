using MRS.Processor.Mrs00517;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00517
{
    public class Mrs00517RDO : HIS_SERE_SERV
    {
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public Mrs00517RDO(HIS_SERE_SERV r, List<V_HIS_ROOM> listHisRoom)
        {
            PropertyInfo[] p = typeof(HIS_SERE_SERV).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            var room = listHisRoom.FirstOrDefault(o=>o.ID==r.TDL_REQUEST_ROOM_ID)??new V_HIS_ROOM();
            this.REQUEST_ROOM_CODE = room.ROOM_CODE;
            this.REQUEST_ROOM_NAME = room.ROOM_NAME;
            this.REQUEST_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
            this.REQUEST_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
            this.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID==r.TDL_SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
        }

        public Mrs00517RDO()
        {

        }
    }
}
