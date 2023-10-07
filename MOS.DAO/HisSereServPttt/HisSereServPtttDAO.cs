using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServPttt
{
    public partial class HisSereServPtttDAO : EntityBase
    {
        private HisSereServPtttGet GetWorker
        {
            get
            {
                return (HisSereServPtttGet)Worker.Get<HisSereServPtttGet>();
            }
        }
        public List<HIS_SERE_SERV_PTTT> Get(HisSereServPtttSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_PTTT> result = new List<HIS_SERE_SERV_PTTT>();
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

        public HIS_SERE_SERV_PTTT GetById(long id, HisSereServPtttSO search)
        {
            HIS_SERE_SERV_PTTT result = null;
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
