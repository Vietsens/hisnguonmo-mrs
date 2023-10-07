using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTein
{
    public partial class HisSereServTeinManager : BusinessBase
    {
        public HisSereServTeinManager()
            : base()
        {

        }

        public HisSereServTeinManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERE_SERV_TEIN> Get(HisSereServTeinFilterQuery filter)
        {
             List<HIS_SERE_SERV_TEIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_TEIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).Get(filter);
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

        
        public  HIS_SERE_SERV_TEIN GetById(long data)
        {
             HIS_SERE_SERV_TEIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_TEIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).GetById(data);
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

        
        public  HIS_SERE_SERV_TEIN GetById(long data, HisSereServTeinFilterQuery filter)
        {
             HIS_SERE_SERV_TEIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_TEIN resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERE_SERV_TEIN> GetByTestIndexId(long filter)
        {
             List<HIS_SERE_SERV_TEIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_TEIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTeinGet(param).GetByTestIndexId(filter);
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

        
        public  List<HIS_SERE_SERV_TEIN> GetBySereServIds(List<long> filter)
        {
             List<HIS_SERE_SERV_TEIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_TEIN> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_TEIN>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServTeinGet(param).GetBySereServIds(Ids));
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
