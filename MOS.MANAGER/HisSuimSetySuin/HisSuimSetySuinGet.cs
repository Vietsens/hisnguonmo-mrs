using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimSetySuin
{
    partial class HisSuimSetySuinGet : BusinessBase
    {
        internal HisSuimSetySuinGet()
            : base()
        {

        }

        internal HisSuimSetySuinGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SUIM_SETY_SUIN> Get(HisSuimSetySuinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimSetySuinDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SUIM_SETY_SUIN> GetView(HisSuimSetySuinViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimSetySuinDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_SETY_SUIN GetById(long id)
        {
            try
            {
                return GetById(id, new HisSuimSetySuinFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_SETY_SUIN GetById(long id, HisSuimSetySuinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimSetySuinDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_SUIM_SETY_SUIN GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSuimSetySuinViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SUIM_SETY_SUIN GetViewById(long id, HisSuimSetySuinViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimSetySuinDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SUIM_SETY_SUIN> GetBySuimIndexId(long id)
        {
            try
            {
                HisSuimSetySuinFilterQuery filter = new HisSuimSetySuinFilterQuery();
                filter.SUIM_INDEX_ID = id;
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
