using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisRehaTrainType
{
    partial class HisRehaTrainTypeGet : EntityBase
    {
        public HIS_REHA_TRAIN_TYPE GetByCode(string code, HisRehaTrainTypeSO search)
        {
            HIS_REHA_TRAIN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_REHA_TRAIN_TYPE.AsQueryable().Where(p => p.REHA_TRAIN_TYPE_CODE == code);
                        if (search.listHisRehaTrainTypeExpression != null && search.listHisRehaTrainTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listHisRehaTrainTypeExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        result = query.SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
