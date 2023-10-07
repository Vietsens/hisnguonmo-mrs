using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCashout
{
    partial class HisCashoutGet : BusinessBase
    {
        internal HisCashoutGet()
            : base()
        {

        }

        internal HisCashoutGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CASHOUT> Get(HisCashoutFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashoutDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHOUT GetById(long id)
        {
            try
            {
                return GetById(id, new HisCashoutFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CASHOUT GetById(long id, HisCashoutFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCashoutDAO.GetById(id, filter.Query());
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
