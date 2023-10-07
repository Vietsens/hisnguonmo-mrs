using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInfusion
{
    public partial class HisInfusionDAO : EntityBase
    {
        private HisInfusionGet GetWorker
        {
            get
            {
                return (HisInfusionGet)Worker.Get<HisInfusionGet>();
            }
        }
        public List<HIS_INFUSION> Get(HisInfusionSO search, CommonParam param)
        {
            List<HIS_INFUSION> result = new List<HIS_INFUSION>();
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

        public HIS_INFUSION GetById(long id, HisInfusionSO search)
        {
            HIS_INFUSION result = null;
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
