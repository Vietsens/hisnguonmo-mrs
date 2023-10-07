using ACS.EFMODEL.DataModels;
using ACS.Filter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.MANAGER.Base;
using MOS.TDO;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.AcsUser
{
    public class AcsUserManager : BusinessBase
    {
        public AcsUserManager()
            : base()
        {

        }

        public AcsUserManager(CommonParam param)
            : base(param)
        {

        }

        [Logger]
        public  List<AcsUserTDO>   GetTdo()
        {
             List<AcsUserTDO>   result = null;
            
            try
            {
                bool valid = true;
                List<AcsUserTDO>  resultData = null;
                if (valid)
                {
                    resultData = new AcsUserGet(param).GetTDO();
                }
                result = this.PackCollectionResult(resultData;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
