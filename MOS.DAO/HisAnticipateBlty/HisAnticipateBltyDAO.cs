using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAnticipateBlty
{
    public partial class HisAnticipateBltyDAO : EntityBase
    {
        private HisAnticipateBltyGet GetWorker
        {
            get
            {
                return (HisAnticipateBltyGet)Worker.Get<HisAnticipateBltyGet>();
            }
        }
        public List<HIS_ANTICIPATE_BLTY> Get(HisAnticipateBltySO search, CommonParam param)
        {
            List<HIS_ANTICIPATE_BLTY> result = new List<HIS_ANTICIPATE_BLTY>();
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

        public HIS_ANTICIPATE_BLTY GetById(long id, HisAnticipateBltySO search)
        {
            HIS_ANTICIPATE_BLTY result = null;
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
