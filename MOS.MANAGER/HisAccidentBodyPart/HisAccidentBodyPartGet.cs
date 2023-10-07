using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentBodyPart
{
    partial class HisAccidentBodyPartGet : BusinessBase
    {
        internal HisAccidentBodyPartGet()
            : base()
        {

        }

        internal HisAccidentBodyPartGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_BODY_PART> Get(HisAccidentBodyPartFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentBodyPartDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_BODY_PART GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentBodyPartFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_BODY_PART GetById(long id, HisAccidentBodyPartFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentBodyPartDAO.GetById(id, filter.Query());
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
