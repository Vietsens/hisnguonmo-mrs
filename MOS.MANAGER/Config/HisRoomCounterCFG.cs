using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    public class HisRoomCounterCFG
    {
        private static Object thisLock = new Object();

        private static List<V_HIS_ROOM_COUNTER_1> data;
        public static List<V_HIS_ROOM_COUNTER_1> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisRoomGet().GetCounter1View(new HisRoomCounter1ViewFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public static void ResfeshCounter()
        {
            Reload();
        }

        //Tang so luong count theo cac service_req vua duoc them vao
        public static void AddCount(List<HIS_SERVICE_REQ> newServiceReqs)
        {
            if (newServiceReqs != null && newServiceReqs.Count > 0)
            {
                //lock de dam bao ko co 2 thread cung chay doan code nay cung 1 thoi diem
                lock (thisLock)
                {
                    if (DATA != null)
                    {
                        foreach (HIS_SERVICE_REQ sr in newServiceReqs)
                        {
                            V_HIS_ROOM_COUNTER_1 counter = DATA.Where(o => o.ID == sr.EXECUTE_ROOM_ID).FirstOrDefault();
                            if (counter != null)
                            {
                                counter.TOTAL_OPEN_TODAY = counter.TOTAL_OPEN_TODAY.HasValue ? counter.TOTAL_OPEN_TODAY.Value + 1 : 1;
                            }
                        }
                    }
                }
            }
        }

        public static void Reload()
        {
            var tmp = new HisRoomGet().GetCounter1View(new HisRoomCounter1ViewFilterQuery());
            data = tmp;
        }
    }
}
