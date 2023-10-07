using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisWorkPlace
{
    partial class HisWorkPlaceGet : BusinessBase
    {
        internal HisWorkPlaceGet()
            : base()
        {

        }

        internal HisWorkPlaceGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_WORK_PLACE> Get(HisWorkPlaceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisWorkPlaceDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_WORK_PLACE GetById(long id)
        {
            try
            {
                return GetById(id, new HisWorkPlaceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_WORK_PLACE GetById(long id, HisWorkPlaceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisWorkPlaceDAO.GetById(id, filter.Query());
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
