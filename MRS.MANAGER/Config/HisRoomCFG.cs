using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisRoomCFG
    {
        private const string ROOM_CODE__KSK = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.KSK";//KSK
        private const string ROOM_CODE__YHCTs = "MRS.HIS_RS.HIS_ROOM.ROOM_CODE.YHCT";//KSK
        //private const string ROOM_CODE__KTNG = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.KTNG";//PK truc ngoai
        //private const string ROOM_CODE__KTS = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.KTS";//PK truc san
        //private const string ROOM_CODE__KCC = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.KCC";//PK cap cuu

        private static long roomIdKSK;
        public static long ROOM_ID__KSK
        {
            get
            {
                if (roomIdKSK == 0)
                {
                    roomIdKSK = GetId(ROOM_CODE__KSK);
                }
                return roomIdKSK;
            }
            set
            {
                roomIdKSK = value;
            }
        }

        private static List<long> roomIdYHCT;
        public static List<long> ROOM_ID__YHCTs
        {
            get
            {
                if (roomIdYHCT == null || roomIdYHCT.Count == 0)
                {
                    roomIdYHCT = GetListId(ROOM_CODE__YHCTs);
                }
                return roomIdYHCT;
            }

        }

        //private static long roomIdKTS;
        //public static long ROOM_ID__KTS
        //{
        //    get
        //    {
        //        if (roomIdKTS == 0)
        //        {
        //            roomIdKTS = GetId(ROOM_CODE__KTS);
        //        }
        //        return roomIdKTS;
        //    }
        //    set
        //    {
        //        roomIdKTS = value;
        //    }
        //}

        //private static long roomIdKCC;
        //public static long ROOM_ID__KCC
        //{
        //    get
        //    {
        //        if (roomIdKCC == 0)
        //        {
        //            roomIdKCC = GetId(ROOM_CODE__KCC);
        //        }
        //        return roomIdKCC;
        //    }
        //    set
        //    {
        //        roomIdKCC = value;
        //    }
        //}

        private static string ROOM_CODE__XNSH = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.XNSH";//XNSH
        private static string ROOM_CODE__XNHH = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.XNHH";//XNHH
        private static string ROOM_CODE__XNVS = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.XNVS";//XNVS
        private static string ROOM_CODE__HAXQ = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.HAXQ";//HAXQ
        private static string ROOM_CODE__HASA = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.HASA";//HASA
        private static string ROOM_CODE__HANS = "DBCODE.HIS_RS.HIS_ROOM.ROOM_CODE.HANS";//HANS

        private static List<long> roomIdXNSH;
        public static List<long> ROOM_ID__XNSH
        {
            get
            {
                if (roomIdXNSH == null || roomIdXNSH.Count == 0)
                {
                    roomIdXNSH = GetListId(ROOM_CODE__XNSH);
                }
                return roomIdXNSH;
            }
            set
            {
                roomIdXNSH = value;
            }
        }

        private static List<long> roomIdXNHH;
        public static List<long> ROOM_ID__XNHH
        {
            get
            {
                if (roomIdXNHH == null || roomIdXNHH.Count == 0)
                {
                    roomIdXNHH = GetListId(ROOM_CODE__XNHH);
                }
                return roomIdXNHH;
            }
            set
            {
                roomIdXNHH = value;
            }
        }

        private static List<long> roomIdXNVS;
        public static List<long> ROOM_ID__XNVS
        {
            get
            {
                if (roomIdXNVS == null || roomIdXNVS.Count == 0)
                {
                    roomIdXNVS = GetListId(ROOM_CODE__XNVS);
                }
                return roomIdXNVS;
            }
            set
            {
                roomIdXNVS = value;
            }
        }

        private static List<long> roomIdHAXQ;
        public static List<long> ROOM_ID__HAXQ
        {
            get
            {
                if (roomIdHAXQ == null || roomIdHAXQ.Count == 0)
                {
                    roomIdHAXQ = GetListId(ROOM_CODE__HAXQ);
                }
                return roomIdHAXQ;
            }
            set
            {
                roomIdHAXQ = value;
            }
        }

        private static List<long> roomIdHASA;
        public static List<long> ROOM_ID__HASA
        {
            get
            {
                if (roomIdHASA == null || roomIdHASA.Count == 0)
                {
                    roomIdHASA = GetListId(ROOM_CODE__HASA);
                }
                return roomIdHASA;
            }
            set
            {
                roomIdHASA = value;
            }
        }

        private static List<long> roomIdHANS;
        public static List<long> ROOM_ID__HANS
        {
            get
            {
                if (roomIdHANS == null || roomIdHANS.Count == 0)
                {
                    roomIdHANS = GetListId(ROOM_CODE__HANS);
                }
                return roomIdHANS;
            }
            set
            {
                roomIdHANS = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                var data = HisRooms.FirstOrDefault(o => o.ROOM_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private static List<long> GetListId(string code)
        {
            List<long> result = new List<long>();
            try
            {

                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                var arr = value.Split(',');
                result = HisRooms.Where(o => arr.Contains(o.ROOM_CODE)).Select(p => p.ID).ToList();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Info("CODE:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code));
                LogSystem.Error(ex);
            }
            return result;
        }

        private static List<V_HIS_ROOM> hisRooms;

        public static List<V_HIS_ROOM> HisRooms
        {
            get
            {
                if (hisRooms == null || hisRooms.Count == 0)
                {
                    try
                    {
                        HisRoomViewFilterQuery filter = new HisRoomViewFilterQuery();
                        hisRooms = new HisRoomManager().GetView(filter);
                        hisRooms = hisRooms.Where(o => o.IS_DELETE == 0).ToList();
                    }
                    catch (Exception ex)
                    {
                        Inventec.Common.Logging.LogSystem.Error(ex);
                    }
                }
                return hisRooms;
            }
        }

        public static void Refresh()
        {
            try
            {
                roomIdKSK = 0;
                roomIdYHCT = null;
                roomIdXNSH = null;
                roomIdXNHH = null;
                roomIdXNVS = null;
                roomIdHAXQ = null;
                roomIdHASA = null;
                roomIdHANS = null;
                hisRooms = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
