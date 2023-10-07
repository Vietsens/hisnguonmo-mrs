using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPackingType
{
    partial class HisPackingTypeGet : BusinessBase
    {
        internal HisPackingTypeGet()
            : base()
        {

        }

        internal HisPackingTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PACKING_TYPE> Get(HisPackingTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPackingTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PACKING_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisPackingTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PACKING_TYPE GetById(long id, HisPackingTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPackingTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PACKING_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPackingTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PACKING_TYPE GetByCode(string code, HisPackingTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPackingTypeDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
