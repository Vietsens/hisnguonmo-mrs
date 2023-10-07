using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMaty
{
    public partial class HisAnticipateMatyManager : BusinessBase
    {
        public HisAnticipateMatyManager()
            : base()
        {

        }

        public HisAnticipateMatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ANTICIPATE_MATY> Get(HisAnticipateMatyFilterQuery filter)
        {
             List<HIS_ANTICIPATE_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ANTICIPATE_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMatyGet(param).Get(filter);
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

        
        public  HIS_ANTICIPATE_MATY GetById(long data)
        {
             HIS_ANTICIPATE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ANTICIPATE_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMatyGet(param).GetById(data);
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

        
        public  HIS_ANTICIPATE_MATY GetById(long data, HisAnticipateMatyFilterQuery filter)
        {
             HIS_ANTICIPATE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ANTICIPATE_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMatyGet(param).GetById(data, filter);
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
