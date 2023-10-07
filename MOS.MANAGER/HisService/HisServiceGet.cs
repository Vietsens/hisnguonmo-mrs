using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisService
{
    public class HisServiceGet : GetBase
    {
        internal HisServiceGet()
            : base()
        {

        }

        internal HisServiceGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE> Get(HisServiceFilterQuery filter)
        {
            try
            {
                List<HIS_SERVICE> result = DAOWorker.HisServiceDAO.Get(filter.Query(), param);
                if (IsNotNullOrEmpty(result))
                {
                    result.ForEach(o =>
                    {
                        o.HIS_SERVICE1 = null;
                        o.HIS_SERVICE2 = null;
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE> GetByIds(List<long> ids)
        {
            try
            {
                HisServiceFilterQuery filter = new HisServiceFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE> GetByParentId(long parentId)
        {
            try
            {
                HisServiceFilterQuery filter = new HisServiceFilterQuery();
                filter.PARENT_ID = parentId;
                return DAOWorker.HisServiceDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE> GetView(HisServiceViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE> GetViewByIds(List<long> ids)
        {
            try
            {
                HisServiceViewFilterQuery filter = new HisServiceViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE GetById(long id, HisServiceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisServiceViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE GetViewById(long id, HisServiceViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE> GetByServiceTypeId(long id)
        {
            try
            {
                HisServiceFilterQuery filter = new HisServiceFilterQuery();
                filter.SERVICE_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE> GetViewByServiceTypeId(long id)
        {
            try
            {
                HisServiceViewFilterQuery filter = new HisServiceViewFilterQuery();
                filter.SERVICE_TYPE_ID = id;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE> GetByServiceUnitId(long id)
        {
            try
            {
                HisServiceFilterQuery filter = new HisServiceFilterQuery();
                filter.SERVICE_UNIT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE> GetActive()
        {
            try
            {
                HisServiceFilterQuery filter = new HisServiceFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE> GetActiveView()
        {
            HisServiceViewFilterQuery filter = new HisServiceViewFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.GetView(filter);
        }

        internal List<HIS_SERVICE> GetByPtttGroupId(long ptttGroupId)
        {
            HisServiceFilterQuery filter = new HisServiceFilterQuery();
            filter.PTTT_GROUP_ID = ptttGroupId;
            return this.Get(filter);
        }

        internal List<HIS_SERVICE> GetByIcdCmId(long icdCmId)
        {
            HisServiceFilterQuery filter = new HisServiceFilterQuery();
            filter.ICD_CM_ID = icdCmId;
            return this.Get(filter);
        }

        internal List<V_HIS_SERVICE_1> GetView1(HisServiceView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisServiceView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_1 GetView1ById(long id, HisServiceView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceDAO.GetView1ById(id, filter.Query());
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
