using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTreatmentSO : StagingObjectBase
    {
        public HisTreatmentSO()
        {
            //listHisTreatmentExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisTreatmentExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT, bool>>> listHisTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TREATMENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT, bool>>> listVHisTreatmentExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_FEE, bool>>> listVHisTreatmentFeeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_FEE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_1, bool>>> listVHisTreatment1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_2, bool>>> listVHisTreatment2Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_2, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_3, bool>>> listVHisTreatment3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_3, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_4, bool>>> listVHisTreatment4Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_4, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_6, bool>>> listVHisTreatment6Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_6, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_7, bool>>> listVHisTreatment7Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_7, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_FEE_1, bool>>> listVHisTreatmentFee1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TREATMENT_FEE_1, bool>>>();
    }
}
