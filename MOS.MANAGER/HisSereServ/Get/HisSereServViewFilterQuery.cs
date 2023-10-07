using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
    public class HisSereServViewFilterQuery : HisSereServViewFilter
    {
        public HisSereServViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV, bool>>> listVHisSereServExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV, bool>>>();



        internal HisSereServSO Query()
        {
            HisSereServSO search = new HisSereServSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSereServExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSereServExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSereServExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.HAS_EXECUTE.HasValue && this.HAS_EXECUTE.Value)
                {
                    listVHisSereServExpression.Add(o => !o.IS_NO_EXECUTE.HasValue);
                }
                if (this.HAS_EXECUTE.HasValue && !this.HAS_EXECUTE.Value)
                {
                    listVHisSereServExpression.Add(o => o.IS_NO_EXECUTE.HasValue && o.IS_NO_EXECUTE == ManagerConstant.IS_TRUE);
                }
                if (this.SERVICE_REQ_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.SERVICE_REQ_ID == this.SERVICE_REQ_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER__EXACT))
                {
                    listVHisSereServExpression.Add(o => o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.Equals(this.HEIN_CARD_NUMBER__EXACT));
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER))
                {
                    listVHisSereServExpression.Add(o => o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.ToLower().Contains(this.HEIN_CARD_NUMBER.ToLower().Trim()));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_TREATMENT_ID.HasValue && o.TDL_TREATMENT_ID.Value == this.TREATMENT_ID.Value);
                }
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.SERVICE_REQ_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.SERVICE_REQ_ID.HasValue && this.SERVICE_REQ_IDs.Contains(o.SERVICE_REQ_ID.Value));
                }
                if (this.SERVICE_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.SERVICE_IDs.Contains(o.SERVICE_ID));
                }
                if (this.SERVICE_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (this.SERVICE_TYPE_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID));
                }
                if (this.IS_SPECIMEN.HasValue && this.IS_SPECIMEN.Value)
                {
                    listVHisSereServExpression.Add(o => o.IS_SPECIMEN.HasValue && o.IS_SPECIMEN.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_SPECIMEN.HasValue && !this.IS_SPECIMEN.Value)
                {
                    listVHisSereServExpression.Add(o => !o.IS_SPECIMEN.HasValue || o.IS_SPECIMEN.Value != ManagerConstant.IS_TRUE);
                }
                if (this.ID__NOT_EQUAL.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.ID != this.ID__NOT_EQUAL.Value);
                }
                if (this.IS_EXPEND.HasValue && this.IS_EXPEND.Value)
                {
                    listVHisSereServExpression.Add(o => o.IS_EXPEND.HasValue && o.IS_EXPEND.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPEND.HasValue && !this.IS_EXPEND.Value)
                {
                    listVHisSereServExpression.Add(o => !o.IS_EXPEND.HasValue || o.IS_EXPEND.Value != ManagerConstant.IS_TRUE);
                }
                if (this.SERVICE_TYPE_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_SERVICE_TYPE_ID == this.SERVICE_TYPE_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.TDL_TREATMENT_ID.HasValue && this.TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID.Value));
                }
                if (this.INVOICE_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.INVOICE_ID.HasValue && o.INVOICE_ID.Value == this.INVOICE_ID.Value);
                }
                if (this.PATIENT_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_PATIENT_ID.HasValue && o.TDL_PATIENT_ID == this.PATIENT_ID.Value);
                }
                if (this.HAS_INVOICE.HasValue && !this.HAS_INVOICE.Value)
                {
                    listVHisSereServExpression.Add(o => !o.INVOICE_ID.HasValue);
                }
                if (this.HAS_INVOICE.HasValue && this.HAS_INVOICE.Value)
                {
                    listVHisSereServExpression.Add(o => o.INVOICE_ID.HasValue);
                }
                if (this.SERVICE_REQ_CODEs != null)
                {
                    listVHisSereServExpression.Add(o => this.SERVICE_REQ_CODEs.Contains(o.TDL_SERVICE_REQ_CODE));
                }
                if (this.EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_EXECUTE_DEPARTMENT_ID == this.EXECUTE_DEPARTMENT_ID.Value);
                }
                //if (this.EXECUTE_TIME_FROM.HasValue)
                //{
                //    listVHisSereServExpression.Add(o => o.EXECUTE_TIME >= this.EXECUTE_TIME_FROM.Value);
                //}
                //if (this.EXECUTE_TIME_TO.HasValue)
                //{
                //    listVHisSereServExpression.Add(o => o.EXECUTE_TIME <= this.EXECUTE_TIME_TO.Value);
                //}
                if (this.MATERIAL_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.MATERIAL_ID.HasValue && o.MATERIAL_ID.Value == this.MATERIAL_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.MATERIAL_ID.HasValue && this.MATERIAL_IDs.Contains(o.MATERIAL_ID.Value));
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.MEDICINE_ID.HasValue && o.MEDICINE_ID.Value == this.MEDICINE_ID.Value);
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.MEDICINE_ID.HasValue && this.MEDICINE_IDs.Contains(o.MEDICINE_ID.Value));
                }
                if (this.HEIN_RATIO.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.HEIN_RATIO.HasValue && o.HEIN_RATIO.Value == this.HEIN_RATIO.Value);
                }
                if (this.HEIN_RATIOs != null)
                {
                    listVHisSereServExpression.Add(s => this.HEIN_RATIOs.Contains(s.HEIN_RATIO.Value));
                }
                if (this.PACKAGE_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.PACKAGE_ID.HasValue && o.PACKAGE_ID.Value == this.PACKAGE_ID.Value);
                }
                if (this.PACKAGE_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.PACKAGE_ID.HasValue && this.PACKAGE_IDs.Contains(o.PACKAGE_ID.Value));
                }
                if (this.REQUEST_DEPARTMENT_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.REQUEST_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID));
                }
                if (this.PATIENT_TYPE_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID));
                }
                if (this.EXECUTE_ROOM_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_EXECUTE_ROOM_ID == this.EXECUTE_ROOM_ID.Value);
                }
                if (this.EXECUTE_ROOM_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.EXECUTE_ROOM_IDs.Contains(o.TDL_EXECUTE_ROOM_ID));
                }
                if (this.REQUEST_ROOM_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_REQUEST_ROOM_ID == this.REQUEST_ROOM_ID.Value);
                }
                if (this.REQUEST_DEPARTMENT_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_REQUEST_DEPARTMENT_ID == this.REQUEST_DEPARTMENT_ID.Value);
                }
                if (this.EKIP_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.EKIP_ID.HasValue && this.EKIP_IDs.Contains(o.EKIP_ID.Value));
                }
                if (this.EKIP_ID.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.EKIP_ID.HasValue && o.EKIP_ID.Value == this.EKIP_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.TDL_TREATMENT_ID.HasValue && this.TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID.Value));
                }
                if (this.REQUEST_ROOM_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.REQUEST_ROOM_IDs.Contains(o.TDL_REQUEST_ROOM_ID));
                }
                if (this.PARENT_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.PARENT_ID.HasValue && this.PARENT_IDs.Contains(o.PARENT_ID.Value));
                }
                if (this.EXECUTE_DEPARTMENT_IDs != null)
                {
                    listVHisSereServExpression.Add(o => this.EXECUTE_DEPARTMENT_IDs.Contains(o.TDL_EXECUTE_DEPARTMENT_ID));
                }
                if (this.PATIENT_IDs != null)
                {
                    listVHisSereServExpression.Add(o => o.TDL_PATIENT_ID.HasValue && this.PATIENT_IDs.Contains(o.TDL_PATIENT_ID.Value));
                }
                if (this.INTRUCTION_DATE_FROM.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_INTRUCTION_DATE >= this.INTRUCTION_DATE_FROM.Value);
                }
                if (this.INTRUCTION_DATE_TO.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_INTRUCTION_DATE <= this.INTRUCTION_DATE_TO.Value);
                }
                if (this.INTRUCTION_TIME_FROM.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_INTRUCTION_TIME >= this.INTRUCTION_TIME_FROM.Value);
                }
                if (this.INTRUCTION_TIME_TO.HasValue)
                {
                    listVHisSereServExpression.Add(o => o.TDL_INTRUCTION_TIME <= this.INTRUCTION_TIME_TO.Value);
                }

                search.listVHisSereServExpression.AddRange(listVHisSereServExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSereServExpression.Clear();
                search.listVHisSereServExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
