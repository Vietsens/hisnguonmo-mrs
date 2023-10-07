using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMaterial
{
    public class HisExpMestMaterialFilterQuery : HisExpMestMaterialFilter
    {
        public HisExpMestMaterialFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATERIAL, bool>>> listHisExpMestMaterialExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_MATERIAL, bool>>>();



        internal HisExpMestMaterialSO Query()
        {
            HisExpMestMaterialSO search = new HisExpMestMaterialSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisExpMestMaterialExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.MATERIAL_ID.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.EXP_MEST_ID.HasValue)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.EXP_MEST_ID != null && this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID.Value));
                }
                if (this.MATERIAL_IDs != null)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.MATERIAL_ID != null && this.MATERIAL_IDs.Contains(o.MATERIAL_ID.Value));
                }
                if (this.IS_EXPORT != null && this.IS_EXPORT.Value)
                {
                    search.listHisExpMestMaterialExpression.Add(o => o.IS_EXPORT.HasValue && o.IS_EXPORT.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPORT != null && !this.IS_EXPORT.Value)
                {
                    search.listHisExpMestMaterialExpression.Add(o => !o.IS_EXPORT.HasValue || o.IS_EXPORT.Value != ManagerConstant.IS_TRUE);
                }

                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listHisExpMestMaterialExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && o.PATIENT_TYPE_ID.Value == this.PATIENT_TYPE_ID.Value);
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    listHisExpMestMaterialExpression.Add(o => o.PATIENT_TYPE_ID.HasValue && this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID.Value));
                }

                search.listHisExpMestMaterialExpression.AddRange(listHisExpMestMaterialExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExpMestMaterialExpression.Clear();
                search.listHisExpMestMaterialExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
