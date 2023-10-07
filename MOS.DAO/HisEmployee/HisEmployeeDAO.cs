using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmployee
{
    public partial class HisEmployeeDAO : EntityBase
    {
        private HisEmployeeGet GetWorker
        {
            get
            {
                return (HisEmployeeGet)Worker.Get<HisEmployeeGet>();
            }
        }
        public List<HIS_EMPLOYEE> Get(HisEmployeeSO search, CommonParam param)
        {
            List<HIS_EMPLOYEE> result = new List<HIS_EMPLOYEE>();
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

        public HIS_EMPLOYEE GetById(long id, HisEmployeeSO search)
        {
            HIS_EMPLOYEE result = null;
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
