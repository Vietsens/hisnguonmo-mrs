using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReactSum
{
    partial class HisMediReactSumGet : BusinessBase
    {
        internal HisMediReactSumGet()
            : base()
        {

        }

        internal HisMediReactSumGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_REACT_SUM> Get(HisMediReactSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactSumDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT_SUM GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediReactSumFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT_SUM GetById(long id, HisMediReactSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactSumDAO.GetById(id, filter.Query());
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
