using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceReqSO : StagingObjectBase
    {
        public HisServiceReqSO()
        {
            //listHisServiceReqExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listHisServiceReqExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReqExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReq1Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReq2Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReq3Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReq6Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReq7Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisServiceReq9Expression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ, bool>>> listHisServiceReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ, bool>>> listVHisServiceReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_2, bool>>> listVHisServiceReq2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_2, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_1, bool>>> listVHisServiceReq1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_3, bool>>> listVHisServiceReq3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_3, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_6, bool>>> listVHisServiceReq6Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_6, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_7, bool>>> listVHisServiceReq7Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_7, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_9, bool>>> listVHisServiceReq9Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ_9, bool>>>();
    }
}
