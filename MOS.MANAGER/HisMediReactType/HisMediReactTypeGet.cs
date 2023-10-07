using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReactType
{
    class HisMediReactTypeGet : BusinessBase
    {
        internal HisMediReactTypeGet()
            : base()
        {

        }

        internal HisMediReactTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_REACT_TYPE> Get(HisMediReactTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediReactTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT_TYPE GetById(long id, HisMediReactTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMediReactTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT_TYPE GetByCode(string code, HisMediReactTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactTypeDAO.GetByCode(code, filter.Query());
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
