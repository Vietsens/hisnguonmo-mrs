using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipate
{
    partial class HisAnticipateGet : BusinessBase
    {
        internal HisAnticipateGet()
            : base()
        {

        }

        internal HisAnticipateGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ANTICIPATE> Get(HisAnticipateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE GetById(long id)
        {
            try
            {
                return GetById(id, new HisAnticipateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE GetById(long id, HisAnticipateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateDAO.GetById(id, filter.Query());
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
