using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeAcin
{
    public class HisMedicineTypeAcinFilterQuery : HisMedicineTypeAcinFilter
    {
        public HisMedicineTypeAcinFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_TYPE_ACIN, bool>>> listHisMedicineTypeAcinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_TYPE_ACIN, bool>>>();

        

        internal HisMedicineTypeAcinSO Query()
        {
            HisMedicineTypeAcinSO search = new HisMedicineTypeAcinSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisMedicineTypeAcinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.ACTIVE_INGREDIENT_ID.HasValue)
                {
                    listHisMedicineTypeAcinExpression.Add(o => o.ACTIVE_INGREDIENT_ID == this.ACTIVE_INGREDIENT_ID.Value);
                }

                search.listHisMedicineTypeAcinExpression.AddRange(listHisMedicineTypeAcinExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMedicineTypeAcinExpression.Clear();
                search.listHisMedicineTypeAcinExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
