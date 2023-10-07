using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisHtu
{
    public partial class HisHtuDAO : EntityBase
    {
        private HisHtuGet GetWorker
        {
            get
            {
                return (HisHtuGet)Worker.Get<HisHtuGet>();
            }
        }
        public List<HIS_HTU> Get(HisHtuSO search, CommonParam param)
        {
            List<HIS_HTU> result = new List<HIS_HTU>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_HTU GetById(long id, HisHtuSO search)
        {
            HIS_HTU result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
