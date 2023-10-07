using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSampleRoom
{
    public partial class HisSampleRoomDAO : EntityBase
    {
        public List<V_HIS_SAMPLE_ROOM> GetView(HisSampleRoomSO search, CommonParam param)
        {
            List<V_HIS_SAMPLE_ROOM> result = new List<V_HIS_SAMPLE_ROOM>();

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

        public HIS_SAMPLE_ROOM GetByCode(string code, HisSampleRoomSO search)
        {
            HIS_SAMPLE_ROOM result = null;

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
        
        public V_HIS_SAMPLE_ROOM GetViewById(long id, HisSampleRoomSO search)
        {
            V_HIS_SAMPLE_ROOM result = null;

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

        public V_HIS_SAMPLE_ROOM GetViewByCode(string code, HisSampleRoomSO search)
        {
            V_HIS_SAMPLE_ROOM result = null;

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

        public Dictionary<string, HIS_SAMPLE_ROOM> GetDicByCode(HisSampleRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_SAMPLE_ROOM> result = new Dictionary<string, HIS_SAMPLE_ROOM>();
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
