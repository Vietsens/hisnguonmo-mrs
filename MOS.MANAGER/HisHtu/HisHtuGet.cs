using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHtu
{
    partial class HisHtuGet : BusinessBase
    {
        internal HisHtuGet()
            : base()
        {

        }

        internal HisHtuGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_HTU> Get(HisHtuFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHtuDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HTU GetById(long id)
        {
            try
            {
                return GetById(id, new HisHtuFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HTU GetById(long id, HisHtuFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHtuDAO.GetById(id, filter.Query());
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
