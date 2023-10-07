using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialBean
{
    partial class HisMaterialBeanGet : GetBase
    {
        internal HisMaterialBeanGet()
            : base()
        {

        }

        internal HisMaterialBeanGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MATERIAL_BEAN> Get(HisMaterialBeanFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_BEAN> GetView(HisMaterialBeanViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_BEAN> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMaterialBeanViewFilterQuery filter = new HisMaterialBeanViewFilterQuery();
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

        internal List<HIS_MATERIAL_BEAN> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMaterialBeanFilterQuery filter = new HisMaterialBeanFilterQuery();
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

        internal HIS_MATERIAL_BEAN GetById(long id)
        {
            try
            {
                return GetById(id, new HisMaterialBeanFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL_BEAN GetById(long id, HisMaterialBeanFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MATERIAL_BEAN GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMaterialBeanViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_BEAN GetViewById(long id, HisMaterialBeanViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL_BEAN> GetByMediStockId(long id)
        {
            try
            {
                HisMaterialBeanFilterQuery filter = new HisMaterialBeanFilterQuery();
                filter.MEDI_STOCK_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL_BEAN> GetByMaterialId(long id)
        {
            try
            {
                HisMaterialBeanFilterQuery filter = new HisMaterialBeanFilterQuery();
                filter.MATERIAL_ID = id;
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
