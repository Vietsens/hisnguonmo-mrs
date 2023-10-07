using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRestRetrType
{
    partial class HisRestRetrTypeGet : BusinessBase
    {
        internal HisRestRetrTypeGet()
            : base()
        {

        }

        internal HisRestRetrTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REST_RETR_TYPE> Get(HisRestRetrTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRestRetrTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_REST_RETR_TYPE> GetView(HisRestRetrTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRestRetrTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REST_RETR_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisRestRetrTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REST_RETR_TYPE GetById(long id, HisRestRetrTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRestRetrTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_REST_RETR_TYPE> GetByRehaTrainTypeId(long id)
        {
            try
            {
                HisRestRetrTypeFilterQuery filter = new HisRestRetrTypeFilterQuery();
                filter.REHA_TRAIN_TYPE_ID = id;
                return Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_REST_RETR_TYPE> GetByRehaServiceTypeId(long id)
        {
            try
            {
                HisRestRetrTypeFilterQuery filter = new HisRestRetrTypeFilterQuery();
                filter.REHA_SERVICE_TYPE_ID = id;
                return Get(filter);
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
