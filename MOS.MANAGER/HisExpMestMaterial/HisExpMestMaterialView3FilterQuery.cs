using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMaterial
{
    public class HisExpMestMaterialView3FilterQuery : HisExpMestMaterialView3Filter
    {
        public HisExpMestMaterialView3FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_3, bool>>> listVHisExpMestMaterial3Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MATERIAL_3, bool>>>();



        internal HisExpMestMaterialSO Query()
        {
            HisExpMestMaterialSO search = new HisExpMestMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMaterial3Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.EXP_MEST_ID.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.EXP_MEST_ID != null && this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID.Value));
                }
                if (this.MATERIAL_TYPE_ID.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.MATERIAL_TYPE_ID == this.MATERIAL_TYPE_ID.Value);
                }
                if (this.MATERIAL_ID.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.MATERIAL_ID != null && this.MATERIAL_IDs.Contains(o.MATERIAL_ID.Value));
                }
                if (this.EXP_TIME_FROM.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.EXP_TIME.Value >= this.EXP_TIME_FROM.Value);
                }
                if (this.EXP_TIME_TO.HasValue)
                {
                    listVHisExpMestMaterial3Expression.Add(o => o.EXP_TIME.Value <= this.EXP_TIME_TO.Value);
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower();
                    listVHisExpMestMaterial3Expression.Add(o => o.APP_CREATOR.Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.Contains(this.KEY_WORD) ||
                        o.CREATOR.Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_CODE.Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_CODE.Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_NAME.Contains(this.KEY_WORD) ||
                        o.NATIONAL_NAME.Contains(this.KEY_WORD) ||
                        o.PACKAGE_NUMBER.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_CODE.Contains(this.KEY_WORD) ||
                        o.SERVICE_UNIT_NAME.Contains(this.KEY_WORD)
                        );
                }

                search.listVHisExpMestMaterial3Expression.AddRange(listVHisExpMestMaterial3Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestMaterial3Expression.Clear();
                search.listVHisExpMestMaterial3Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
