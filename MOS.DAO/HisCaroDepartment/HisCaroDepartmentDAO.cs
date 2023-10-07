using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCaroDepartment
{
    public partial class HisCaroDepartmentDAO : EntityBase
    {
        private HisCaroDepartmentGet GetWorker
        {
            get
            {
                return (HisCaroDepartmentGet)Worker.Get<HisCaroDepartmentGet>();
            }
        }
        public List<HIS_CARO_DEPARTMENT> Get(HisCaroDepartmentSO search, CommonParam param)
        {
            List<HIS_CARO_DEPARTMENT> result = new List<HIS_CARO_DEPARTMENT>();
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

        public HIS_CARO_DEPARTMENT GetById(long id, HisCaroDepartmentSO search)
        {
            HIS_CARO_DEPARTMENT result = null;
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
