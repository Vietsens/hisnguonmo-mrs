using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDepartment
{
    public partial class HisDepartmentDAO : EntityBase
    {
        private HisDepartmentGet GetWorker
        {
            get
            {
                return (HisDepartmentGet)Worker.Get<HisDepartmentGet>();
            }
        }
        public List<HIS_DEPARTMENT> Get(HisDepartmentSO search, CommonParam param)
        {
            List<HIS_DEPARTMENT> result = new List<HIS_DEPARTMENT>();
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

        public HIS_DEPARTMENT GetById(long id, HisDepartmentSO search)
        {
            HIS_DEPARTMENT result = null;
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
