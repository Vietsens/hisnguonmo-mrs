using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServSuin
{
    public partial class HisSereServSuinDAO : EntityBase
    {
        private HisSereServSuinGet GetWorker
        {
            get
            {
                return (HisSereServSuinGet)Worker.Get<HisSereServSuinGet>();
            }
        }
        public List<HIS_SERE_SERV_SUIN> Get(HisSereServSuinSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_SUIN> result = new List<HIS_SERE_SERV_SUIN>();
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

        public HIS_SERE_SERV_SUIN GetById(long id, HisSereServSuinSO search)
        {
            HIS_SERE_SERV_SUIN result = null;
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
