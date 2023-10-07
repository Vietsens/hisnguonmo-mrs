using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBlood
{
    public class HisBloodViewFilterQuery : HisBloodViewFilter
    {
        public HisBloodViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_BLOOD, bool>>> listVHisBloodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BLOOD, bool>>>();



        internal HisBloodSO Query()
        {
            HisBloodSO search = new HisBloodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisBloodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisBloodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisBloodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisBloodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.BLOOD_TYPE_ID.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_TYPE_ID == this.BLOOD_TYPE_ID.Value);
                }
                if (this.BLOOD_TYPE_IDs != null)
                {
                    listVHisBloodExpression.Add(o => this.BLOOD_TYPE_IDs.Contains(o.BLOOD_TYPE_ID));
                }
                if (!String.IsNullOrEmpty(this.BLOOD_CODE__EXACT))
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_CODE == this.BLOOD_CODE__EXACT);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.MEDI_STOCK_ID == this.MEDI_STOCK_ID.Value);
                }
                if (this.MEDI_STOCK_IDs != null)
                {
                    listVHisBloodExpression.Add(o => o.MEDI_STOCK_ID.HasValue && this.MEDI_STOCK_IDs.Contains(o.MEDI_STOCK_ID.Value));
                }
                if (this.BLOOD_RH_ID.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_RH_ID.HasValue && o.BLOOD_RH_ID.Value == this.BLOOD_RH_ID.Value);
                }
                if (this.BLOOD_RH_IDs != null)
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_RH_ID.HasValue && this.BLOOD_RH_IDs.Contains(o.BLOOD_RH_ID.Value));
                }
                if (this.BLOOD_ABO_ID.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_ABO_ID == this.BLOOD_ABO_ID.Value);
                }
                if (this.BLOOD_ABO_IDs != null)
                {
                    listVHisBloodExpression.Add(o => this.BLOOD_ABO_IDs.Contains(o.BLOOD_ABO_ID));
                }
                if (this.BLOOD_VOLUME_ID.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_VOLUME_ID == this.BLOOD_VOLUME_ID.Value);
                }
                if (this.BLOOD_VOLUME_IDs != null)
                {
                    listVHisBloodExpression.Add(o => this.BLOOD_VOLUME_IDs.Contains(o.BLOOD_VOLUME_ID));
                }
                if (this.BLOOD_TYPE_IS_ACTIVE.HasValue)
                {
                    listVHisBloodExpression.Add(o => o.BLOOD_TYPE_IS_ACTIVE == this.BLOOD_TYPE_IS_ACTIVE.Value);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisBloodExpression.Add(o =>
                        o.BLOOD_ABO_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BLOOD_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BLOOD_RH_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BLOOD_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.BLOOD_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.GIVE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.GIVE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.IMP_SOURCE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.IMP_SOURCE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.PACKAGE_NUMBER.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listVHisBloodExpression.AddRange(listVHisBloodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisBloodExpression.Clear();
                search.listVHisBloodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
