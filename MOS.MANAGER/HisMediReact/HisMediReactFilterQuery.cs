using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReact
{
    public class HisMediReactFilterQuery : HisMediReactFilter
    {
        public HisMediReactFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT, bool>>> listHisMediReactExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_REACT, bool>>>();

        

        internal HisMediReactSO Query()
        {
            HisMediReactSO search = new HisMediReactSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisMediReactExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMediReactExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMediReactExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMediReactExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDI_REACT_TYPE_ID.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.MEDI_REACT_TYPE_ID == this.MEDI_REACT_TYPE_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDI_REACT_SUM_ID.HasValue)
                {
                    listHisMediReactExpression.Add(o => o.MEDI_REACT_SUM_ID == this.MEDI_REACT_SUM_ID.Value);
                }
                if (this.MEDI_REACT_SUM_IDs != null)
                {
                    listHisMediReactExpression.Add(o => this.MEDI_REACT_SUM_IDs.Contains(o.MEDI_REACT_SUM_ID));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listHisMediReactExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID??-1));
                }


                search.listHisMediReactExpression.AddRange(listHisMediReactExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMediReactExpression.Clear();
                search.listHisMediReactExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
