using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisSuimIndex
{
    class HisSuimIndexGet : GetBase
    {
        internal HisSuimIndexGet()
            : base()
        {

        }

        internal HisSuimIndexGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SUIM_INDEX> Get(HisSuimIndexFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SUIM_INDEX> GetView(HisSuimIndexViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX GetById(long id)
        {
            try
            {
                return GetById(id, new HisSuimIndexFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX GetById(long id, HisSuimIndexFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SUIM_INDEX GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSuimIndexViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SUIM_INDEX GetViewById(long id, HisSuimIndexViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisSuimIndexFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX GetByCode(string code, HisSuimIndexFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SUIM_INDEX GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisSuimIndexViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SUIM_INDEX GetViewByCode(string code, HisSuimIndexViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SUIM_INDEX> GetBySuimIndexUnitId(long id)
        {
            try
            {
                HisSuimIndexFilterQuery filter = new HisSuimIndexFilterQuery();
                filter.SUIM_INDEX_UNIT_ID = id;
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
