using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestBlood
{
    partial class HisImpMestBloodGet : BusinessBase
    {
        internal HisImpMestBloodGet()
            : base()
        {

        }

        internal HisImpMestBloodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_BLOOD> Get(HisImpMestBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestBloodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_BLOOD GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestBloodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_BLOOD GetById(long id, HisImpMestBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestBloodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_BLOOD> GetViewByImpMestId(long impMestId)
        {
            HisImpMestBloodViewFilterQuery filter = new HisImpMestBloodViewFilterQuery();
            filter.IMP_MEST_ID = impMestId;
            return this.GetView(filter);
        }

        internal List<HIS_IMP_MEST_BLOOD> GetByImpMestId(long impMestId)
        {
            HisImpMestBloodFilterQuery filter = new HisImpMestBloodFilterQuery();
            filter.IMP_MEST_ID = impMestId;
            return this.Get(filter);
        }

        internal List<HIS_IMP_MEST_BLOOD> GetByBloodId(long bloodId)
        {
            HisImpMestBloodFilterQuery filter = new HisImpMestBloodFilterQuery();
            filter.BLOOD_ID = bloodId;
            return this.Get(filter);
        }
    }
}
