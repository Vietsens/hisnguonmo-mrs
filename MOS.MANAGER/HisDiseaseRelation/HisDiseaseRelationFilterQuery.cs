using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDiseaseRelation
{
    public class HisDiseaseRelationFilterQuery : HisDiseaseRelationFilter
    {
        public HisDiseaseRelationFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_DISEASE_RELATION, bool>>> listHisDiseaseRelationExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DISEASE_RELATION, bool>>>();

        

        internal HisDiseaseRelationSO Query()
        {
            HisDiseaseRelationSO search = new HisDiseaseRelationSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisDiseaseRelationExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisDiseaseRelationExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisDiseaseRelationExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisDiseaseRelationExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisDiseaseRelationExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisDiseaseRelationExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisDiseaseRelationExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisDiseaseRelationExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisDiseaseRelationExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisDiseaseRelationExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisDiseaseRelationExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DISEASE_RELATION_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DISEASE_RELATION_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisDiseaseRelationExpression.AddRange(listHisDiseaseRelationExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisDiseaseRelationExpression.Clear();
                search.listHisDiseaseRelationExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
