using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialPaty
{
    public class HisMaterialPatyFilterQuery : HisMaterialPatyFilter
    {
        public HisMaterialPatyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_PATY, bool>>> listHisMaterialPatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_PATY, bool>>>();

        

        internal HisMaterialPatySO Query()
        {
            HisMaterialPatySO search = new HisMaterialPatySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMaterialPatyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisMaterialPatyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisMaterialPatyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisMaterialPatyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.MATERIAL_ID.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    search.listHisMaterialPatyExpression.Add(o => this.MATERIAL_IDs.Contains(o.MATERIAL_ID));
                }
                if (this.ID__NOT_EQUAL.HasValue)
                {
                    search.listHisMaterialPatyExpression.Add(o => o.ID != this.ID__NOT_EQUAL.Value);
                }

                search.listHisMaterialPatyExpression.AddRange(listHisMaterialPatyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMaterialPatyExpression.Clear();
                search.listHisMaterialPatyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
