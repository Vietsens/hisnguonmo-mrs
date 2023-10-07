using SAR.DAO.Base;
using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAR.DAO.SarReportType
{
    partial class SarReportTypeGet : EntityBase
    {
        public List<SAR_REPORT_TYPE> Get(SarReportTypeSO search, CommonParam param)
        {
            List<SAR_REPORT_TYPE> list = new List<SAR_REPORT_TYPE>();
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.SAR_REPORT_TYPE.AsQueryable();
                        if (search.listSarReportTypeExpression != null && search.listSarReportTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listSarReportTypeExpression)
                            {
                                query = query.Where(item);
                            }
                        }

                        //var queryHint = from emp in ctx.SAR_REPORT_TYPE.OracleHints("FULL(employees) PARALLEL(employees, 5)")
                        //
                        //                select emp;

                        //var result = query.ToList();


                        int start = param.Start.HasValue ? param.Start.Value : 0;
                        int limit = param.Limit.HasValue ? param.Limit.Value : Int32.MaxValue;
                        list = query.OrderByProperty(search.OrderField, search.OrderDirection).Skip(start).Take(limit).ToList();
                        param.Count = (from r in query select r).Count();
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

        public SAR_REPORT_TYPE GetById(long id, SarReportTypeSO search)
        {
            SAR_REPORT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsGreaterThanZero(id);
                if (valid)
                {
                    using (var ctx = new AppContext())
                    {
                        var query = ctx.SAR_REPORT_TYPE.AsQueryable().Where(p => p.ID == id);
                        if (search.listSarReportTypeExpression != null && search.listSarReportTypeExpression.Count > 0)
                        {
                            foreach (var item in search.listSarReportTypeExpression)
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
