using Inventec.Common.Logging;

using MOS.EFMODEL.DataModels;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisServiceReqTypeMappingCFG
    {
        public static Dictionary<long, long> MAPPING = new Dictionary<long, long>() {
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KHAC},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PHCN},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN},
        };

        public static void Reload()
        {
            Dictionary<long, long> map = new Dictionary<long, long>() {
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KHAC},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PHCN},
            {IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN},
        };
            MAPPING = map;
        }
    }
}
