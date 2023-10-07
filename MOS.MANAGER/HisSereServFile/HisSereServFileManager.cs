using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServFile
{
    public partial class HisSereServFileManager : BusinessBase
    {
        public HisSereServFileManager()
            : base()
        {

        }

        public HisSereServFileManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SERE_SERV_FILE> Get(HisSereServFileFilterQuery filter)
        {
            List<HIS_SERE_SERV_FILE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_FILE> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServFileGet(param).Get(filter);
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

        
        public HIS_SERE_SERV_FILE GetById(long data)
        {
            HIS_SERE_SERV_FILE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_FILE resultData = null;
                if (valid)
                {
                    resultData = new HisSereServFileGet(param).GetById(data);
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

        
        public HIS_SERE_SERV_FILE GetById(long data, HisSereServFileFilterQuery filter)
        {
            HIS_SERE_SERV_FILE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_FILE resultData = null;
                if (valid)
                {
                    resultData = new HisSereServFileGet(param).GetById(data, filter);
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

        
        public List<HIS_SERE_SERV_FILE> GetBySereServIds(List<long> data)
        {
            List<HIS_SERE_SERV_FILE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERE_SERV_FILE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV_FILE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServFileGet(param).GetBySereServIds(Ids));
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

        
        public FileHolder GetFile(long data)
        {
            FileHolder result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                FileHolder resultData = null;
                if (valid)
                {
                    resultData = new HisSereServFileGet(param).GetFile(data);
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
