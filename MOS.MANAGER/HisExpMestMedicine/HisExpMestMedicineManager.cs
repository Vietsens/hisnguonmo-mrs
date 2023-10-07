using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMedicine
{
    public partial class HisExpMestMedicineManager : BusinessBase
    {
        public HisExpMestMedicineManager()
            : base()
        {

        }

        public HisExpMestMedicineManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXP_MEST_MEDICINE> Get(HisExpMestMedicineFilterQuery filter)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).Get(filter);
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

        
        public HIS_EXP_MEST_MEDICINE GetById(long data)
        {
            HIS_EXP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetById(data);
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

        
        public HIS_EXP_MEST_MEDICINE GetById(long data, HisExpMestMedicineFilterQuery filter)
        {
            HIS_EXP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetById(data, filter);
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetByIds(List<long> data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetByIds(Ids));
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetByExpMestId(long data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetByExpMestIds(List<long> data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetByExpMestIds(Ids));
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetExportedByExpMestId(long data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetExportedByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetExportedByExpMestIds(List<long> data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetExportedByExpMestIds(Ids));
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetUnexportByExpMestId(long data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetUnexportByExpMestId(data);
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetByMedicineId(long data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetByMedicineId(data);
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

        
        public List<HIS_EXP_MEST_MEDICINE> GetByMedicineIds(List<long> data)
        {
            List<HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_EXP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetByMedicineIds(Ids));
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
    }
}
