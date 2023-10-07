using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBranch
{
    partial class HisBranchGet : BusinessBase
    {
        internal HisBranchGet()
            : base()
        {

        }

        internal HisBranchGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BRANCH> Get(HisBranchFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBranchDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BRANCH GetById(long id)
        {
            try
            {
                return GetById(id, new HisBranchFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BRANCH GetById(long id, HisBranchFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBranchDAO.GetById(id, filter.Query());
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
