using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDebateUser
{
    partial class HisDebateUserGet : BusinessBase
    {
        internal HisDebateUserGet()
            : base()
        {

        }

        internal HisDebateUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEBATE_USER> Get(HisDebateUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDebateUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEBATE_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisDebateUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEBATE_USER GetById(long id, HisDebateUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDebateUserDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal List<HIS_DEBATE_USER> GetByDebateId(long id)
        {
            try
            {
                HisDebateUserFilterQuery filter = new HisDebateUserFilterQuery();
                filter.DEBATE_ID = id;
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
