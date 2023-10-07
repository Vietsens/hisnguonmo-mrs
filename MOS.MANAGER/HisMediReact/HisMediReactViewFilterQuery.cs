using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReact
{
    public class HisMediReactViewFilterQuery : HisMediReactViewFilter
    {
        public HisMediReactViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_REACT, bool>>> listVHisMediReactExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEDI_REACT, bool>>>();

        

        internal HisMediReactSO Query()
        {
            HisMediReactSO search = new HisMediReactSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisMediReactExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisMediReactExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisMediReactExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisMediReactExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisMediReactExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.MEDI_REACT_TYPE_ID.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.MEDI_REACT_TYPE_ID == this.MEDI_REACT_TYPE_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDI_REACT_SUM_ID.HasValue)
                {
                    listVHisMediReactExpression.Add(o => o.MEDI_REACT_SUM_ID == this.MEDI_REACT_SUM_ID.Value);
                }
                if (this.MEDI_REACT_SUM_IDs != null)
                {
                    listVHisMediReactExpression.Add(o => this.MEDI_REACT_SUM_IDs.Contains(o.MEDI_REACT_SUM_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisMediReactExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID??-1));
                }

                search.listVHisMediReactExpression.AddRange(listVHisMediReactExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisMediReactExpression.Clear();
                search.listVHisMediReactExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
