using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPackingType
{
    public partial class HisPackingTypeDAO : EntityBase
    {
        private HisPackingTypeGet GetWorker
        {
            get
            {
                return (HisPackingTypeGet)Worker.Get<HisPackingTypeGet>();
            }
        }
        public List<HIS_PACKING_TYPE> Get(HisPackingTypeSO search, CommonParam param)
        {
            List<HIS_PACKING_TYPE> result = new List<HIS_PACKING_TYPE>();
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

        public HIS_PACKING_TYPE GetById(long id, HisPackingTypeSO search)
        {
            HIS_PACKING_TYPE result = null;
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
