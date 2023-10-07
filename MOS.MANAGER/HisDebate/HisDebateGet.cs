using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDebate
{
    partial class HisDebateGet : BusinessBase
    {
        internal HisDebateGet()
            : base()
        {

        }

        internal HisDebateGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEBATE> Get(HisDebateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDebateDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEBATE GetById(long id)
        {
            try
            {
                return GetById(id, new HisDebateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEBATE GetById(long id, HisDebateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDebateDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_DEBATE> GetByTreatmentId(long id)
        {
            try
            {
                HisDebateFilterQuery filter = new HisDebateFilterQuery();
                filter.TREATMENT_ID = id;
                return this.Get(filter);
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
