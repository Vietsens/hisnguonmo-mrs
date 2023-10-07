using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMaty
{
    partial class HisServiceMatyGet : BusinessBase
    {
        internal HisServiceMatyGet()
            : base()
        {

        }

        internal HisServiceMatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_MATY> Get(HisServiceMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_MATY> GetView(HisServiceMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_MATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceMatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_MATY GetById(long id, HisServiceMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_MATY> GetByMaterialTypeId(long id)
        {
            try
            {
                HisServiceMatyFilterQuery filter = new HisServiceMatyFilterQuery();
                filter.MATERIAL_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_MATY> GetByServiceId(long id)
        {
            try
            {
                HisServiceMatyFilterQuery filter = new HisServiceMatyFilterQuery();
                filter.SERVICE_ID = id;
                return this.Get(filter);
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
