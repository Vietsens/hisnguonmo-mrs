using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMaterialBean;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisMaterialType
{
    partial class HisMaterialTypeGet : GetBase
    {
        internal HisMaterialTypeGet()
            : base()
        {

        }

        internal HisMaterialTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MATERIAL_TYPE> Get(HisMaterialTypeFilterQuery filter)
        {
            try
            {
                List<HIS_MATERIAL_TYPE> result = DAOWorker.HisMaterialTypeDAO.Get(filter.Query(), param);
                if (IsNotNullOrEmpty(result))
                {
                    result.ForEach(o =>
                    {
                        o.HIS_MATERIAL_TYPE1 = null;
                        o.HIS_MATERIAL_TYPE2 = null;
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

        internal List<HIS_MATERIAL_TYPE> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMaterialTypeFilterQuery filter = new HisMaterialTypeFilterQuery();
                    filter.IDs = ids;
                    return this.Get(filter);
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

        internal List<V_HIS_MATERIAL_TYPE> GetView(HisMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_TYPE> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMaterialTypeViewFilterQuery filter = new HisMaterialTypeViewFilterQuery();
                    filter.IDs = ids;
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

        internal HIS_MATERIAL_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMaterialTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL_TYPE GetById(long id, HisMaterialTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMaterialTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_TYPE GetViewById(long id, HisMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialTypeDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMaterialTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL_TYPE GetByCode(string code, HisMaterialTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialTypeDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_TYPE GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisMaterialTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_TYPE GetViewByCode(string code, HisMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialTypeDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL_TYPE> GetByParentId(long parentId)
        {
            try
            {
                HisMaterialTypeFilterQuery filter = new HisMaterialTypeFilterQuery();
                filter.PARENT_ID = parentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL_TYPE> GetByManufacturerId(long id)
        {
            try
            {
                HisMaterialTypeFilterQuery filter = new HisMaterialTypeFilterQuery();
                filter.MANUFACTURER_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL_TYPE> GetActiveStent()
        {
            HisMaterialTypeFilterQuery filter = new HisMaterialTypeFilterQuery();
            filter.IS_STENT = true;
            return this.Get(filter);
        }

        internal List<HIS_MATERIAL_TYPE> GetByMemaGroupId(long memaGroupId)
        {
            try
            {
                HisMaterialTypeFilterQuery filter = new HisMaterialTypeFilterQuery();
                filter.MEMA_GROUP_ID = memaGroupId;
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
