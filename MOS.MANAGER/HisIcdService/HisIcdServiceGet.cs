using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdService
{
    partial class HisIcdServiceGet : BusinessBase
    {
        internal HisIcdServiceGet()
            : base()
        {

        }

        internal HisIcdServiceGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ICD_SERVICE> Get(HisIcdServiceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdServiceDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_SERVICE GetById(long id)
        {
            try
            {
                return GetById(id, new HisIcdServiceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_SERVICE GetById(long id, HisIcdServiceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdServiceDAO.GetById(id, filter.Query());
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
