using SDA.DAO.Base;
using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SDA.DAO.SdaLicense
{
    partial class SdaLicenseGet : EntityBase
    {
        public List<SDA_LICENSE> Get(SdaLicenseSO search, CommonParam param)
        {
            List<SDA_LICENSE> list = new List<SDA_LICENSE>();
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.SDA_LICENSE.AsQueryable();
                        if (search.listSdaLicenseExpression != null && search.listSdaLicenseExpression.Count > 0)
                        {
                            foreach (var item in search.listSdaLicenseExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        
                        
                        if (!string.IsNullOrWhiteSpace(search.OrderField) && !string.IsNullOrWhiteSpace(search.OrderDirection)) { if (!param.Start.HasValue || !param.Limit.HasValue) { list = query.OrderByProperty(search.OrderField, search.OrderDirection).ToList(); } else { param.Count = (from r in query select r).Count(); query = query.OrderByProperty(search.OrderField, search.OrderDirection); if (param.Count <= param.Limit.Value && param.Start.Value == 0) { list = query.ToList(); } else { list = query.Skip(param.Start.Value).Take(param.Limit.Value).ToList(); } } } else { list = query.ToList(); }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param), LogType.Error);
                LogSystem.Error(ex);
                list.Clear();
            }
            return list;
        }

        public SDA_LICENSE GetById(long id, SdaLicenseSO search)
        {
            SDA_LICENSE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.SDA_LICENSE.AsQueryable().Where(p => p.ID == id);
                        if (search.listSdaLicenseExpression != null && search.listSdaLicenseExpression.Count > 0)
                        {
                            foreach (var item in search.listSdaLicenseExpression)
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
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => id), id) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
