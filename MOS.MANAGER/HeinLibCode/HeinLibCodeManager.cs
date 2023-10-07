using Inventec.Common.Logging;
using Inventec.Core;
using MOS.LibraryHein.Factory;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HeinLibCode
{
    public class HeinLibCodeManager : BusinessBase
    {
        public HeinLibCodeManager()
            : base()
        {

        }

        public HeinLibCodeManager(CommonParam param)
            : base(param)
        {

        }

        [Logger]
        public  List<HeinLibCodeData> Get()
        {
             List<HeinLibCodeData> result = null;
            try
            {
                result = HeinLibCodeData.GetLibCodes());
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
