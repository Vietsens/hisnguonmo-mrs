using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServTein
{
    public partial class HisSereServTeinDAO : EntityBase
    {
        private HisSereServTeinGet GetWorker
        {
            get
            {
                return (HisSereServTeinGet)Worker.Get<HisSereServTeinGet>();
            }
        }
        public List<HIS_SERE_SERV_TEIN> Get(HisSereServTeinSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_TEIN> result = new List<HIS_SERE_SERV_TEIN>();
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

        public HIS_SERE_SERV_TEIN GetById(long id, HisSereServTeinSO search)
        {
            HIS_SERE_SERV_TEIN result = null;
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
