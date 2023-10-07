using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialType
{
    public partial class HisMaterialTypeManager : BusinessBase
    {
        public HisMaterialTypeManager()
            : base()
        {

        }

        public HisMaterialTypeManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_MATERIAL_TYPE> Get(HisMaterialTypeFilterQuery filter)
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public HIS_MATERIAL_TYPE GetById(long data)
        {
            HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public HIS_MATERIAL_TYPE GetById(long data, HisMaterialTypeFilterQuery filter)
        {
            HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public HIS_MATERIAL_TYPE GetByCode(string data)
        {
            HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetByCode(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public HIS_MATERIAL_TYPE GetByCode(string data, HisMaterialTypeFilterQuery filter)
        {
            HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetByCode(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MATERIAL_TYPE> GetByIds(List<long> data)
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MATERIAL_TYPE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialTypeGet(param).GetByIds(Ids));
                    }
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MATERIAL_TYPE> GetByParentId(long parentId)
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(parentId);
                List<HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetByParentId(parentId);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MATERIAL_TYPE> GetByManufacturerId(long id)
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetByManufacturerId(id);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MATERIAL_TYPE> GetByMemaGroupId(long id)
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetByMemaGroupId(id);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MATERIAL_TYPE> GetActiveStent()
        {
            List<HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetActiveStent();
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
