using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExecuteGroup
{
    public partial class HisExecuteGroupDAO : EntityBase
    {
        private HisExecuteGroupGet GetWorker
        {
            get
            {
                return (HisExecuteGroupGet)Worker.Get<HisExecuteGroupGet>();
            }
        }
        public List<HIS_EXECUTE_GROUP> Get(HisExecuteGroupSO search, CommonParam param)
        {
            List<HIS_EXECUTE_GROUP> result = new List<HIS_EXECUTE_GROUP>();
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

        public HIS_EXECUTE_GROUP GetById(long id, HisExecuteGroupSO search)
        {
            HIS_EXECUTE_GROUP result = null;
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
