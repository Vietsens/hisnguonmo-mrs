using ACS.EFMODEL.DataModels;
using ACS.Filter;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Token.ResourceSystem;
using MOS.MANAGER.Base;
using MOS.TDO;
using System;
using System.Linq;
using System.Collections.Generic;
using COS.EFMODEL.DataModels;
using COS.Filter;
using MOS.MANAGER.ConsumerManager;
using COS.SDO;

namespace MOS.MANAGER.CosCard
{
    class CosCardGet : BusinessBase
    {
        internal CosCardGet()
            : base()
        {

        }

        internal CosCardGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal CardOwnerSDO GetByServiceCode(string serviceCode)
        {
            try
            {
                return CosApiConsumerStore.CosConsumerWrapper.Get<CardOwnerSDO>(true, "/api/CosPeople/GetByServiceCode", param, serviceCode);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
