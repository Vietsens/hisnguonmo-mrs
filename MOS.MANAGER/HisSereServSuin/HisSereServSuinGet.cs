using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServSuin
{
    partial class HisSereServSuinGet : BusinessBase
    {
        internal HisSereServSuinGet()
            : base()
        {

        }

        internal HisSereServSuinGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_SUIN> Get(HisSereServSuinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServSuinDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERE_SERV_SUIN> GetView(HisSereServSuinViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServSuinDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_SUIN GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServSuinFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_SUIN GetById(long id, HisSereServSuinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServSuinDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_SERE_SERV_SUIN GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSereServSuinViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_SUIN GetViewById(long id, HisSereServSuinViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServSuinDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_SUIN> GetBySereServIds(List<long> sereServIds)
        {
            try
            {
                HisSereServSuinFilterQuery filter = new HisSereServSuinFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_SUIN> GetBySuimIndexId(long suimIndexId)
        {
            try
            {
                HisSereServSuinFilterQuery filter = new HisSereServSuinFilterQuery();
                filter.SUIM_INDEX_ID = suimIndexId;
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
