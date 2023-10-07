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
using MOS.MANAGER.ConsumerManager;
using Inventec.Common.WebApiClient;
using System.Configuration;

namespace MOS.MANAGER.AcsUser
{
    class AcsUserGet : BusinessBase
    {
        internal AcsUserGet()
            : base()
        {

        }

        internal AcsUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<ACS_USER> Get(AcsUserFilter filter)
        {
            try
            {
                var ro = ApiConsumerStore.AcsConsumer.Get<Inventec.Core.ApiResultObject<List<ACS_USER>>>("/api/AcsUser/Get", param, filter);
                if (ro != null)
                {
                    param = ro.Param != null ? ro.Param : param;
                    return ro.Data;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        /// <summary>
        /// Cung cap cho third-party
        /// </summary>
        /// <returns></returns>
        internal List<AcsUserTDO> GetTDO()
        {
            try
            {
                List<ACS_USER> list = this.Get(new AcsUserFilter());
                if (IsNotNullOrEmpty(list))
                {
                    return list.Select(o => new AcsUserTDO
                    {
                        LoginName = o.LOGINNAME,
                        UserName = o.USERNAME
                    }).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal ACS_USER GetByLoginName(string loginName)
        {
            try
            {
                AcsUserFilter filter = new AcsUserFilter();
                filter.LOGINNAME = loginName;
                var ro = ApiConsumerStore.AcsConsumer.Get<Inventec.Core.ApiResultObject<List<ACS_USER>>>("/api/AcsUser/Get", param, filter);
                if (ro != null)
                {
                    param = ro.Param != null ? ro.Param : param;
                    return ro.Data != null && ro.Data.Count > 0 ? ro.Data[0] : null;
                }
                return null;
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
