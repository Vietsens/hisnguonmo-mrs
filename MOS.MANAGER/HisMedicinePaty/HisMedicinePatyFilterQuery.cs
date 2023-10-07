using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicinePaty
{
    public class HisMedicinePatyFilterQuery : HisMedicinePatyFilter
    {
        public HisMedicinePatyFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_PATY, bool>>> listHisMedicinePatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDICINE_PATY, bool>>>();

        

        internal HisMedicinePatySO Query()
        {
            HisMedicinePatySO search = new HisMedicinePatySO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMedicinePatyExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisMedicinePatyExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisMedicinePatyExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisMedicinePatyExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.MEDICINE_ID == this.MEDICINE_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    search.listHisMedicinePatyExpression.Add(o => this.MEDICINE_IDs.Contains(o.MEDICINE_ID));
                }
                if (this.ID__NOT_EQUAL.HasValue)
                {
                    search.listHisMedicinePatyExpression.Add(o => o.ID != this.ID__NOT_EQUAL.Value);
                }

                search.listHisMedicinePatyExpression.AddRange(listHisMedicinePatyExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMedicinePatyExpression.Clear();
                search.listHisMedicinePatyExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
