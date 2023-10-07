using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAnticipate
{
    public partial class HisAnticipateDAO : EntityBase
    {
        private HisAnticipateGet GetWorker
        {
            get
            {
                return (HisAnticipateGet)Worker.Get<HisAnticipateGet>();
            }
        }
        public List<HIS_ANTICIPATE> Get(HisAnticipateSO search, CommonParam param)
        {
            List<HIS_ANTICIPATE> result = new List<HIS_ANTICIPATE>();
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

        public HIS_ANTICIPATE GetById(long id, HisAnticipateSO search)
        {
            HIS_ANTICIPATE result = null;
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
