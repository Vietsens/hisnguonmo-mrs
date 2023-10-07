using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedType
{
    public partial class HisBedTypeDAO : EntityBase
    {
        private HisBedTypeGet GetWorker
        {
            get
            {
                return (HisBedTypeGet)Worker.Get<HisBedTypeGet>();
            }
        }
        public List<HIS_BED_TYPE> Get(HisBedTypeSO search, CommonParam param)
        {
            List<HIS_BED_TYPE> result = new List<HIS_BED_TYPE>();
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

        public HIS_BED_TYPE GetById(long id, HisBedTypeSO search)
        {
            HIS_BED_TYPE result = null;
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
