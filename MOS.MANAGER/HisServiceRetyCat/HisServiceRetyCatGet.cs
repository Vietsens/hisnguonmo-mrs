using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRetyCat
{
    partial class HisServiceRetyCatGet : BusinessBase
    {
        internal HisServiceRetyCatGet()
            : base()
        {

        }

        internal HisServiceRetyCatGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_RETY_CAT> Get(HisServiceRetyCatFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRetyCatDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_RETY_CAT> GetView(HisServiceRetyCatViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRetyCatDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_RETY_CAT GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceRetyCatFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_RETY_CAT GetById(long id, HisServiceRetyCatFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceRetyCatDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_RETY_CAT> GetByReportTypeCatId(long id)
        {
            try
            {
                HisServiceRetyCatFilterQuery filter = new HisServiceRetyCatFilterQuery();
                filter.REPORT_TYPE_CAT_ID = id;
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
