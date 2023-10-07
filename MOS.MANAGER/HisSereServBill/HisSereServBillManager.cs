using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServBill
{
    public partial class HisSereServBillManager : BusinessBase
    {
        public HisSereServBillManager()
            : base()
        {

        }

        public HisSereServBillManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SERE_SERV_BILL> Get(HisSereServBillFilterQuery filter)
        {
            List<HIS_SERE_SERV_BILL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_BILL> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).Get(filter);
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

        
        public HIS_SERE_SERV_BILL GetById(long data)
        {
            HIS_SERE_SERV_BILL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_BILL resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetById(data);
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

        
        public HIS_SERE_SERV_BILL GetById(long data, HisSereServBillFilterQuery filter)
        {
            HIS_SERE_SERV_BILL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_BILL resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetById(data, filter);
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

        
        public List<HIS_SERE_SERV_BILL> GetBySereServIds(List<long> filter)
        {
            List<HIS_SERE_SERV_BILL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_BILL> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_BILL>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServBillGet(param).GetBySereServIds(Ids));
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

        
        public List<HIS_SERE_SERV_BILL> GetNoCancelBySereServIds(List<long> filter)
        {
            List<HIS_SERE_SERV_BILL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_BILL> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_BILL>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServBillGet(param).GetNoCancelBySereServIds(Ids));
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

        
        public List<HIS_SERE_SERV_BILL> GetNoCancelBySereServId(long filter)
        {
            List<HIS_SERE_SERV_BILL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_BILL> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetNoCancelBySereServId(filter);
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

        
        public List<HIS_SERE_SERV_BILL> GetByBillId(long filter)
        {
            List<HIS_SERE_SERV_BILL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_BILL> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetByBillId(filter);
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
