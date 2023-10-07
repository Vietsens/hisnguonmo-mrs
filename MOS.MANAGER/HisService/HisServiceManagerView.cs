using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisService
{
    public partial class HisServiceManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE> GetView(HisServiceViewFilterQuery filter)
        {
            List<V_HIS_SERVICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetView(filter);
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

        
        public V_HIS_SERVICE GetViewById(long data)
        {
            V_HIS_SERVICE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERVICE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetViewById(data);
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

        
        public V_HIS_SERVICE GetViewById(long data, HisServiceViewFilterQuery filter)
        {
            V_HIS_SERVICE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERVICE resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_SERVICE> GetViewByServiceTypeId(long filter)
        {
            List<V_HIS_SERVICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetViewByServiceTypeId(filter);
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

        
        public List<V_HIS_SERVICE> GetActiveView()
        {
            List<V_HIS_SERVICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<V_HIS_SERVICE> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetActiveView();
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

        
        public List<V_HIS_SERVICE_1> GetView1(HisServiceView1FilterQuery filter)
        {
            List<V_HIS_SERVICE_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_1> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetView1(filter);
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

        
        public V_HIS_SERVICE_1 GetView1ById(long data)
        {
            V_HIS_SERVICE_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERVICE_1 resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetView1ById(data);
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

        
        public V_HIS_SERVICE_1 GetView1ById(long data, HisServiceView1FilterQuery filter)
        {
            V_HIS_SERVICE_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERVICE_1 resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGet(param).GetView1ById(data, filter);
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
