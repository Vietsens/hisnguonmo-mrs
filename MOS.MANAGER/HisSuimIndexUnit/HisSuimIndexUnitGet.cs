using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndexUnit
{
    class HisSuimIndexUnitGet : GetBase
    {
        internal HisSuimIndexUnitGet()
            : base()
        {

        }

        internal HisSuimIndexUnitGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SUIM_INDEX_UNIT> Get(HisSuimIndexUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexUnitDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX_UNIT GetById(long id)
        {
            try
            {
                return GetById(id, new HisSuimIndexUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX_UNIT GetById(long id, HisSuimIndexUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexUnitDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX_UNIT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisSuimIndexUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUIM_INDEX_UNIT GetByCode(string code, HisSuimIndexUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSuimIndexUnitDAO.GetByCode(code, filter.Query());
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
