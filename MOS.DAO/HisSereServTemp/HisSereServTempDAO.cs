using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServTemp
{
    public partial class HisSereServTempDAO : EntityBase
    {
        private HisSereServTempGet GetWorker
        {
            get
            {
                return (HisSereServTempGet)Worker.Get<HisSereServTempGet>();
            }
        }
        public List<HIS_SERE_SERV_TEMP> Get(HisSereServTempSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_TEMP> result = new List<HIS_SERE_SERV_TEMP>();
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

        public HIS_SERE_SERV_TEMP GetById(long id, HisSereServTempSO search)
        {
            HIS_SERE_SERV_TEMP result = null;
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
