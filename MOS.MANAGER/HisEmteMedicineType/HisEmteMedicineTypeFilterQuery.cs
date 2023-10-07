using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMedicineType
{
    public class HisEmteMedicineTypeFilterQuery : HisEmteMedicineTypeFilter
    {
        public HisEmteMedicineTypeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MEDICINE_TYPE, bool>>> listHisEmteMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMTE_MEDICINE_TYPE, bool>>>();

        

        internal HisEmteMedicineTypeSO Query()
        {
            HisEmteMedicineTypeSO search = new HisEmteMedicineTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisEmteMedicineTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.EXP_MEST_TEMPLATE_ID.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.EXP_MEST_TEMPLATE_ID == this.EXP_MEST_TEMPLATE_ID.Value);
                }
                
                #endregion

                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    search.listHisEmteMedicineTypeExpression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                search.listHisEmteMedicineTypeExpression.AddRange(listHisEmteMedicineTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisEmteMedicineTypeExpression.Clear();
                search.listHisEmteMedicineTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
