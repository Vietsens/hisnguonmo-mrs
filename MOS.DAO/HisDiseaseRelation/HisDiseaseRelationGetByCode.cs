using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDiseaseRelation
{
    partial class HisDiseaseRelationGet : EntityBase
    {
        public HIS_DISEASE_RELATION GetByCode(string code, HisDiseaseRelationSO search)
        {
            HIS_DISEASE_RELATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_DISEASE_RELATION.AsQueryable().Where(p => p.DISEASE_RELATION_CODE == code);
                        if (search.listHisDiseaseRelationExpression != null && search.listHisDiseaseRelationExpression.Count > 0)
                        {
                            foreach (var item in search.listHisDiseaseRelationExpression)
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
