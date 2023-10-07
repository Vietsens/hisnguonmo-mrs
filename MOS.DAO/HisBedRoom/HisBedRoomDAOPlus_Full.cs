using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedRoom
{
    public partial class HisBedRoomDAO : EntityBase
    {
        public List<V_HIS_BED_ROOM> GetView(HisBedRoomSO search, CommonParam param)
        {
            List<V_HIS_BED_ROOM> result = new List<V_HIS_BED_ROOM>();

            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public HIS_BED_ROOM GetByCode(string code, HisBedRoomSO search)
        {
            HIS_BED_ROOM result = null;

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
        
        public V_HIS_BED_ROOM GetViewById(long id, HisBedRoomSO search)
        {
            V_HIS_BED_ROOM result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_BED_ROOM GetViewByCode(string code, HisBedRoomSO search)
        {
            V_HIS_BED_ROOM result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_BED_ROOM> GetDicByCode(HisBedRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_BED_ROOM> result = new Dictionary<string, HIS_BED_ROOM>();
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
