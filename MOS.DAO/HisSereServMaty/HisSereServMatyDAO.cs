using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServMaty
{
    public partial class HisSereServMatyDAO : EntityBase
    {
        private HisSereServMatyGet GetWorker
        {
            get
            {
                return (HisSereServMatyGet)Worker.Get<HisSereServMatyGet>();
            }
        }

        public List<HIS_SERE_SERV_MATY> Get(HisSereServMatySO search, CommonParam param)
        {
            List<HIS_SERE_SERV_MATY> result = new List<HIS_SERE_SERV_MATY>();
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

        public HIS_SERE_SERV_MATY GetById(long id, HisSereServMatySO search)
        {
            HIS_SERE_SERV_MATY result = null;
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
