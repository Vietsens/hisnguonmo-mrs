using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMety
{
    partial class HisServiceMetyGet : BusinessBase
    {
        internal HisServiceMetyGet()
            : base()
        {

        }

        internal HisServiceMetyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_METY> Get(HisServiceMetyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMetyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_METY> GetView(HisServiceMetyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMetyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_METY GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceMetyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_METY GetById(long id, HisServiceMetyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceMetyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_METY> GetByMaterialTypeId(long id)
        {
            try
            {
                HisServiceMetyFilterQuery filter = new HisServiceMetyFilterQuery();
                filter.MEDICINE_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_METY> GetByServiceId(long id)
        {
            try
            {
                HisServiceMetyFilterQuery filter = new HisServiceMetyFilterQuery();
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
