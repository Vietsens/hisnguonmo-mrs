using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServ
{
    public partial class HisSereServDAO : EntityBase
    {
        private HisSereServGet GetWorker
        {
            get
            {
                return (HisSereServGet)Worker.Get<HisSereServGet>();
            }
        }
        public List<HIS_SERE_SERV> Get(HisSereServSO search, CommonParam param)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
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

        public HIS_SERE_SERV GetById(long id, HisSereServSO search)
        {
            HIS_SERE_SERV result = null;
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
