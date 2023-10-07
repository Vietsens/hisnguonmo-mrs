using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
    public partial class HisSereServManager : BusinessBase
    {
        public HisSereServManager()
            : base()
        {

        }

        public HisSereServManager(CommonParam param)
            : base(param)
        {

        }


        public List<HIS_SERE_SERV> Get(HisSereServFilterQuery filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServGet(param).Get(filter);
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


        public HIS_SERE_SERV GetById(long data)
        {
            HIS_SERE_SERV result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV resultData = null;
                if (valid)
                {
                    resultData = new HisSereServGet(param).GetById(data);
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


        public HIS_SERE_SERV GetById(long data, HisSereServFilterQuery filter)
        {
            HIS_SERE_SERV result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV resultData = null;
                if (valid)
                {
                    resultData = new HisSereServGet(param).GetById(data, filter);
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


        public List<HIS_SERE_SERV> GetByIds(List<long> filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServGet(param).GetByIds(Ids));
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


        public List<HIS_SERE_SERV> GetByServiceReqIds(List<long> filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServGet(param).GetByServiceReqIds(Ids));
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


        public List<HIS_SERE_SERV> GetByParentIds(List<long> filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_SERE_SERV>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisSereServGet(param).GetByParentIds(Ids));
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


        public List<HIS_SERE_SERV> GetByHeinApprovalId(long filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServGet(param).GetByHeinApprovalId(filter);
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


        public List<HIS_SERE_SERV> GetByTreatmentId(long filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServGet(param).GetByTreatmentId(filter);
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


        public List<HIS_SERE_SERV> GetByServiceId(long filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServGet(param).GetByServiceId(filter);
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
