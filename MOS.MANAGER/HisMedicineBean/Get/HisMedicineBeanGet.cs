using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    partial class HisMedicineBeanGet : GetBase
    {
        internal HisMedicineBeanGet()
            : base()
        {

        }

        internal HisMedicineBeanGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_BEAN> Get(HisMedicineBeanFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_BEAN> GetView(HisMedicineBeanViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_BEAN> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMedicineBeanViewFilterQuery filter = new HisMedicineBeanViewFilterQuery();
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

        internal List<HIS_MEDICINE_BEAN> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMedicineBeanFilterQuery filter = new HisMedicineBeanFilterQuery();
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

        internal HIS_MEDICINE_BEAN GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineBeanFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_BEAN GetById(long id, HisMedicineBeanFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEDICINE_BEAN GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMedicineBeanViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_BEAN GetViewById(long id, HisMedicineBeanViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_BEAN> GetByMediStockId(long id)
        {
            try
            {
                HisMedicineBeanFilterQuery filter = new HisMedicineBeanFilterQuery();
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

        internal List<HIS_MEDICINE_BEAN> GetByMedicineId(long id)
        {
            try
            {
                HisMedicineBeanFilterQuery filter = new HisMedicineBeanFilterQuery();
                filter.MEDICINE_ID = id;
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
