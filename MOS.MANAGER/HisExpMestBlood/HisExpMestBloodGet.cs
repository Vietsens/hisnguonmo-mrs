using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBlood
{
    partial class HisExpMestBloodGet : BusinessBase
    {
        internal HisExpMestBloodGet()
            : base()
        {

        }

        internal HisExpMestBloodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_BLOOD> Get(HisExpMestBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBloodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_BLOOD GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestBloodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_BLOOD GetById(long id, HisExpMestBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBloodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_BLOOD> GetViewByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisExpMestBloodViewFilterQuery filter = new HisExpMestBloodViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
            }
            return null;
        }

        internal List<HIS_EXP_MEST_BLOOD> GetByExpMestId(long expMestId)
        {
            HisExpMestBloodFilterQuery filter = new HisExpMestBloodFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_BLOOD> GetExportedByExpMestId(long expMestId)
        {
            HisExpMestBloodFilterQuery filter = new HisExpMestBloodFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            filter.IS_EXPORT = true;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_BLOOD> GetUnexportByExpMestId(long expMestId)
        {
            HisExpMestBloodFilterQuery filter = new HisExpMestBloodFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            filter.IS_EXPORT = false;
            return this.Get(filter);
        }

        internal List<V_HIS_EXP_MEST_BLOOD> GetViewUnexportByExpMestId(long expMestId)
        {
            HisExpMestBloodViewFilterQuery filter = new HisExpMestBloodViewFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            filter.IS_EXPORT = false;
            return this.GetView(filter);
        }

        internal List<V_HIS_EXP_MEST_BLOOD> GetViewByExpMestIds(List<long> expMestIds)
        {
            try
            {
                if (expMestIds != null)
                {
                    HisExpMestBloodViewFilterQuery filter = new HisExpMestBloodViewFilterQuery();
                    filter.EXP_MEST_IDs = expMestIds;
                    return this.GetView(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_BLOOD> GetViewByExpMestId(long expMestId)
        {
            HisExpMestBloodViewFilterQuery filter = new HisExpMestBloodViewFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView(filter);
        }

        internal List<V_HIS_EXP_MEST_BLOOD> GetViewByAggrExpMestId(long aggrExpMestId)
        {
            HisExpMestBloodViewFilterQuery filter = new HisExpMestBloodViewFilterQuery();
            filter.AGGR_EXP_MEST_ID = aggrExpMestId;
            return this.GetView(filter);
        }

        internal List<HIS_EXP_MEST_BLOOD> GetByBloodId(long id)
        {
            HisExpMestBloodFilterQuery filter = new HisExpMestBloodFilterQuery();
            filter.BLOOD_ID = id;
            return this.Get(filter);
        }
    }
}
