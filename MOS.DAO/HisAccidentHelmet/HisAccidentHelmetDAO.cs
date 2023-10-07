using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHelmet
{
    public partial class HisAccidentHelmetDAO : EntityBase
    {
        private HisAccidentHelmetGet GetWorker
        {
            get
            {
                return (HisAccidentHelmetGet)Worker.Get<HisAccidentHelmetGet>();
            }
        }
        public List<HIS_ACCIDENT_HELMET> Get(HisAccidentHelmetSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_HELMET> result = new List<HIS_ACCIDENT_HELMET>();
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

        public HIS_ACCIDENT_HELMET GetById(long id, HisAccidentHelmetSO search)
        {
            HIS_ACCIDENT_HELMET result = null;
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
