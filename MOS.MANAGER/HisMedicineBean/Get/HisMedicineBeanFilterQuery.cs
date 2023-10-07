using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    public class HisMedicineBeanFilterQuery : HisMedicineBeanFilter
    {
        public HisMedicineBeanFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_BEAN, bool>>> listHisMedicineBeanExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_BEAN, bool>>>();

        

        internal HisMedicineBeanSO Query()
        {
            HisMedicineBeanSO search = new HisMedicineBeanSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMedicineBeanExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisMedicineBeanExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisMedicineBeanExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisMedicineBeanExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MEDICINE_ID.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.MEDI_STOCK_ID.HasValue && o.MEDI_STOCK_ID.Value == this.MEDI_STOCK_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    search.listHisMedicineBeanExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (!String.IsNullOrEmpty(this.DETACH_KEY__EXACT))
                {
                    search.listHisMedicineBeanExpression.Add(o => o.DETACH_KEY == this.DETACH_KEY__EXACT);
                }
                if (this.DETACH_KEYs != null)
                {
                    search.listHisMedicineBeanExpression.Add(o => this.DETACH_KEYs.Contains(o.DETACH_KEY));
                }
                if (this.MEDICINE_IS_ACTIVE.HasValue)
                {
                    search.listHisMedicineBeanExpression.Add(o => o.TDL_MEDICINE_IS_ACTIVE == this.MEDICINE_IS_ACTIVE.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    search.listHisMedicineBeanExpression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.TDL_MEDICINE_TYPE_ID));
                }

                search.listHisMedicineBeanExpression.AddRange(listHisMedicineBeanExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMedicineBeanExpression.Clear();
                search.listHisMedicineBeanExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
