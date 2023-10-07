using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornPosition
{
    partial class HisBornPositionGet : BusinessBase
    {
        internal HisBornPositionGet()
            : base()
        {

        }

        internal HisBornPositionGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BORN_POSITION> Get(HisBornPositionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornPositionDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_POSITION GetById(long id)
        {
            try
            {
                return GetById(id, new HisBornPositionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_POSITION GetById(long id, HisBornPositionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornPositionDAO.GetById(id, filter.Query());
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
