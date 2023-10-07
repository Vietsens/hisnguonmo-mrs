using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisMedicineType;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MOS.MANAGER.HisMedicine
{
    partial class HisMedicineGet : GetBase
    {
        internal HisMedicineGet()
            : base()
        {

        }

        internal HisMedicineGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE> Get(HisMedicineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisMedicineFilterQuery filter = new HisMedicineFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal List<V_HIS_MEDICINE> GetViewByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisMedicineViewFilterQuery filter = new HisMedicineViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
            }
            return null;
        }

        internal List<V_HIS_MEDICINE> GetView(HisMedicineViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE GetById(long id, HisMedicineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMedicineViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE GetViewById(long id, HisMedicineViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE> GetByMedicineTypeId(long id)
        {
            try
            {
                HisMedicineFilterQuery filter = new HisMedicineFilterQuery();
                filter.MEDICINE_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE> GetBySupplierId(long id)
        {
            try
            {
                HisMedicineFilterQuery filter = new HisMedicineFilterQuery();
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
    }
}
