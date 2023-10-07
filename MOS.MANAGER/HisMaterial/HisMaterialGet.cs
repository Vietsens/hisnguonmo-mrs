using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisMaterialType;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MOS.MANAGER.HisMaterial
{
    partial class HisMaterialGet : GetBase
    {
        internal HisMaterialGet()
            : base()
        {

        }

        internal HisMaterialGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MATERIAL> Get(HisMaterialFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL> GetView(HisMaterialViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL GetById(long id)
        {
            try
            {
                return GetById(id, new HisMaterialFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MATERIAL GetById(long id, HisMaterialFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMaterialViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL GetViewById(long id, HisMaterialViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL> GetByMaterialTypeId(long id)
        {
            try
            {
                HisMaterialFilterQuery filter = new HisMaterialFilterQuery();
                filter.MATERIAL_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL> GetBySupplierId(long id)
        {
            try
            {
                HisMaterialFilterQuery filter = new HisMaterialFilterQuery();
                filter.SUPPLIER_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MATERIAL> GetByIds(List<long> materialIds)
        {
            if (IsNotNullOrEmpty(materialIds))
            {
                HisMaterialFilterQuery filter = new HisMaterialFilterQuery();
                filter.IDs = materialIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_MATERIAL> GetViewByIds(List<long> materialIds)
        {
            if (IsNotNullOrEmpty(materialIds))
            {
                HisMaterialViewFilterQuery filter = new HisMaterialViewFilterQuery();
                filter.IDs = materialIds;
                return this.GetView(filter);
            }
            return null;
        }
    }
}
