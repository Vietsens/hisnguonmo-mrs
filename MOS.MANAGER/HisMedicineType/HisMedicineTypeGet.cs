using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMedicineBean;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisMedicineType
{
    partial class HisMedicineTypeGet : GetBase
    {
        internal HisMedicineTypeGet()
            : base()
        {

        }

        internal HisMedicineTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_TYPE> Get(HisMedicineTypeFilterQuery filter)
        {
            try
            {
                List<HIS_MEDICINE_TYPE> result = DAOWorker.HisMedicineTypeDAO.Get(filter.Query(), param);
                if (IsNotNullOrEmpty(result))
                {
                    result.ForEach(o =>
                        {
                            o.HIS_MEDICINE_TYPE1 = null;
                            o.HIS_MEDICINE_TYPE2 = null;
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

        internal List<HIS_MEDICINE_TYPE> GetActiveNeuroLogical()
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                filter.MEDICINE_GROUP_ID = IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_TYPE> GetActiveAddictive()
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                filter.MEDICINE_GROUP_ID = IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_TYPE> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
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

        internal List<V_HIS_MEDICINE_TYPE> GetView(HisMedicineTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_TYPE> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMedicineTypeViewFilterQuery filter = new HisMedicineTypeViewFilterQuery();
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

        internal HIS_MEDICINE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE GetById(long id, HisMedicineTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMedicineTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_TYPE GetViewById(long id, HisMedicineTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMedicineTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE GetByCode(string code, HisMedicineTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_TYPE GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisMedicineTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_TYPE GetViewByCode(string code, HisMedicineTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_TYPE> GetByParentId(long parentId)
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
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

        internal List<HIS_MEDICINE_TYPE> GetByManufacturerId(long id)
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
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

        internal List<HIS_MEDICINE_TYPE> GetByMedicineUseFormId(long id)
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
                filter.MEDICINE_USE_FORM_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_TYPE> GetByMedicineLineId(long id)
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
                filter.MEDICINE_LINE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_TYPE> GetByMemaGroupId(long memaGroupId)
        {
            try
            {
                HisMedicineTypeFilterQuery filter = new HisMedicineTypeFilterQuery();
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
