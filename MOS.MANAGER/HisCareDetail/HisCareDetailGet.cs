using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareDetail
{
    partial class HisCareDetailGet : BusinessBase
    {
        internal HisCareDetailGet()
            : base()
        {

        }

        internal HisCareDetailGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CARE_DETAIL> Get(HisCareDetailFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareDetailDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_DETAIL GetById(long id)
        {
            try
            {
                return GetById(id, new HisCareDetailFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_DETAIL GetById(long id, HisCareDetailFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareDetailDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE_DETAIL> GetByCareTypeId(long id)
        {
            try
            {
                HisCareDetailFilterQuery filter = new HisCareDetailFilterQuery();
                filter.CARE_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE_DETAIL> GetByCareId(long id)
        {
            try
            {
                HisCareDetailFilterQuery filter = new HisCareDetailFilterQuery();
                filter.CARE_ID = id;
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
