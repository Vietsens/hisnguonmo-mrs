using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisServiceReq
{
    partial class HisServiceReqGet : EntityBase
    {
        public List<V_HIS_SERVICE_REQ_6> GetView6(HisServiceReqSO search, CommonParam param)
        {
            List<V_HIS_SERVICE_REQ_6> list = new List<V_HIS_SERVICE_REQ_6>();
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.V_HIS_SERVICE_REQ_6.AsNoTracking().AsQueryable();
                        if (search.listVHisServiceReq6Expression != null && search.listVHisServiceReq6Expression.Count > 0)
                        {
                            foreach (var item in search.listVHisServiceReq6Expression)
                            {
                                query = query.Where(item);
                            }
                        }
                        
                        if (!string.IsNullOrWhiteSpace(search.OrderField) && !string.IsNullOrWhiteSpace(search.OrderDirection))
                        {
                            if (param.Start.HasValue && param.Limit.HasValue)
                            {
                                param.Count = (from r in query select r).Count();
                            }

                            //Mac dinh luon thuc hien theo order_field, order_direction de tranh loi o ham Skip o phia duoi
                            query = query.OrderByProperty(search.OrderField, search.OrderDirection);

                            //Cac extra_order chi thuc hien khi co truyen vao
                            if (IsNotNullOrEmpty(search.ExtraOrderDirection1) && IsNotNullOrEmpty(search.ExtraOrderField1))
                            {
                                query = query.ThenByProperty(search.ExtraOrderField1, search.ExtraOrderDirection1);
                            }
                            if (IsNotNullOrEmpty(search.ExtraOrderDirection2) && IsNotNullOrEmpty(search.ExtraOrderField2))
                            {
                                query = query.ThenByProperty(search.ExtraOrderField2, search.ExtraOrderDirection2);
                            }
                            if (IsNotNullOrEmpty(search.ExtraOrderDirection3) && IsNotNullOrEmpty(search.ExtraOrderField3))
                            {
                                query = query.ThenByProperty(search.ExtraOrderField3, search.ExtraOrderDirection3);
                            }
                            if (IsNotNullOrEmpty(search.ExtraOrderDirection4) && IsNotNullOrEmpty(search.ExtraOrderField4))
                            {
                                query = query.ThenByProperty(search.ExtraOrderField4, search.ExtraOrderDirection4);
                            }

                            if (!param.Start.HasValue || !param.Limit.HasValue)
                            {
                                list = query.ToList();
                            }
                            else
                            {
                                list = query.Skip(param.Start.Value).Take(param.Limit.Value).ToList();
                            }
                        }
                        else
                        {
                            list = query.ToList();
                        }
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
    }
}
