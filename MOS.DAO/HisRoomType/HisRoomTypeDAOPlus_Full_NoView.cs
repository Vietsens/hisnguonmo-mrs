using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRoomType
{
    public partial class HisRoomTypeDAO : EntityBase
    {
        public HIS_ROOM_TYPE GetByCode(string code, HisRoomTypeSO search)
        {
            HIS_ROOM_TYPE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_ROOM_TYPE> GetDicByCode(HisRoomTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_ROOM_TYPE> result = new Dictionary<string, HIS_ROOM_TYPE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
