using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMedicineType
{
    public class HisBidMedicineTypeViewFilterQuery : HisBidMedicineTypeViewFilter
    {
        public HisBidMedicineTypeViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MEDICINE_TYPE, bool>>> listVHisBidMedicineTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BID_MEDICINE_TYPE, bool>>>();

        

        internal HisBidMedicineTypeSO Query()
        {
            HisBidMedicineTypeSO search = new HisBidMedicineTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisBidMedicineTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisBidMedicineTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BID_ID.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.BID_ID == this.BID_ID.Value);
                }
                if (this.BID_IDs != null)
                {
                    listVHisBidMedicineTypeExpression.Add(o => this.BID_IDs.Contains(o.BID_ID));
                }
                if (this.MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisBidMedicineTypeExpression.Add(o => o.MEDICINE_TYPE_ID == this.MEDICINE_TYPE_ID.Value);
                }
                if (this.MEDICINE_TYPE_IDs != null)
                {
                    listVHisBidMedicineTypeExpression.Add(o => this.MEDICINE_TYPE_IDs.Contains(o.MEDICINE_TYPE_ID));
                }

                search.listVHisBidMedicineTypeExpression.AddRange(listVHisBidMedicineTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisBidMedicineTypeExpression.Clear();
                search.listVHisBidMedicineTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
