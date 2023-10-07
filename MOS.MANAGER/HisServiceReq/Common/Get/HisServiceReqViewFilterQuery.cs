using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReq
{
    public class HisServiceReqViewFilterQuery : HisServiceReqViewFilter
    {
        public HisServiceReqViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ, bool>>> listVHisServiceReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_REQ, bool>>>();

        internal HisServiceReqSO Query()
        {
            HisServiceReqSO search = new HisServiceReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisServiceReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisServiceReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisServiceReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisServiceReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.PATIENT_CODE))
                {
                    search.listVHisServiceReqExpression.Add(o => o.TDL_PATIENT_CODE.ToLower().Contains(this.PATIENT_CODE.ToLower().Trim()));
                }
                if (!String.IsNullOrEmpty(this.SERVICE_REQ_CODE))
                {
                    search.listVHisServiceReqExpression.Add(o => o.SERVICE_REQ_CODE.ToLower().Contains(this.SERVICE_REQ_CODE.ToLower().Trim()));
                }
                if (!String.IsNullOrEmpty(this.PATIENT_NAME))
                {
                    search.listVHisServiceReqExpression.Add(o => o.TDL_PATIENT_NAME.ToLower().Contains(this.PATIENT_NAME.ToLower().Trim()));
                }
                if (this.EXECUTE_ROOM_ID.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.EXECUTE_ROOM_ID == this.EXECUTE_ROOM_ID.Value);
                }
                if (this.REQUEST_ROOM_ID.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.REQUEST_ROOM_ID == this.REQUEST_ROOM_ID.Value);
                }
                if (this.SERVICE_REQ_STT_IDs != null)
                {
                    search.listVHisServiceReqExpression.Add(o => this.SERVICE_REQ_STT_IDs.Contains(o.SERVICE_REQ_STT_ID));
                }
                if (this.INTRUCTION_TIME_FROM.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.INTRUCTION_TIME >= this.INTRUCTION_TIME_FROM.Value);
                }
                if (this.INTRUCTION_TIME_TO.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.INTRUCTION_TIME <= this.INTRUCTION_TIME_TO.Value);
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.EXECUTE_DEPARTMENT_ID == this.EXECUTE_DEPARTMENT_ID.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.FINISH_TIME >= this.FINISH_TIME_FROM);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.FINISH_TIME <= this.FINISH_TIME_TO);
                }
                if (this.EXECUTE_DEPARTMENT_IDs != null)
                {
                    search.listVHisServiceReqExpression.Add(s => EXECUTE_DEPARTMENT_IDs.Contains(s.EXECUTE_DEPARTMENT_ID));
                }
                if (this.START_TIME_FROM != null)
                {
                    search.listVHisServiceReqExpression.Add(s => s.START_TIME.HasValue && s.START_TIME.Value >= this.START_TIME_FROM.Value);
                }
                if (this.START_TIME_TO != null)
                {
                    search.listVHisServiceReqExpression.Add(s => s.START_TIME.HasValue && s.START_TIME.Value <= this.START_TIME_TO.Value);
                }
                if (this.REQUEST_DEPARTMENT_IDs != null)
                {
                    search.listVHisServiceReqExpression.Add(s => this.REQUEST_DEPARTMENT_IDs.Contains(s.REQUEST_DEPARTMENT_ID));
                }
                if (this.REQUEST_DEPARTMENT_ID != null)
                {
                    search.listVHisServiceReqExpression.Add(s => s.REQUEST_DEPARTMENT_ID == this.REQUEST_DEPARTMENT_ID);
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisServiceReqExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_BHYT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_ROOM_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_USERNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_GENDER_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.GROUP_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_TEXT.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_BHYT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_ROOM_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.REQUEST_USERNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_REQ_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_REQ_STT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_REQ_STT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_REQ_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_REQ_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.TREATMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_ADDRESS.ToLower().Contains(this.KEY_WORD) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD__SERVICE_REQ_CODE__PATIENT_CODE__TREATMENT_CODE__PATIENT_NAME))
                {
                    string keyword = this.KEY_WORD__SERVICE_REQ_CODE__PATIENT_CODE__TREATMENT_CODE__PATIENT_NAME.ToLower().Trim();
                    listVHisServiceReqExpression.Add(o =>
                        o.TDL_PATIENT_CODE.ToLower().Contains(keyword) ||
                        o.SERVICE_REQ_CODE.ToLower().Contains(keyword) ||
                        o.TREATMENT_CODE.ToLower().Contains(keyword) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(keyword)
                        );
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD__SERVICE_REQ_CODE__PATIENT_CODE__TREATMENT_CODE__PATIENT_NAME__SERVICE_REQ_TYPE_CODE))
                {
                    string keyword = this.KEY_WORD__SERVICE_REQ_CODE__PATIENT_CODE__TREATMENT_CODE__PATIENT_NAME__SERVICE_REQ_TYPE_CODE.ToLower().Trim();
                    listVHisServiceReqExpression.Add(o =>
                        o.TDL_PATIENT_CODE.ToLower().Contains(keyword) ||
                        o.SERVICE_REQ_CODE.ToLower().Contains(keyword) ||
                        o.SERVICE_REQ_TYPE_CODE.ToLower().Contains(keyword) ||
                        o.TREATMENT_CODE.ToLower().Contains(keyword) ||
                        o.TDL_PATIENT_NAME.ToLower().Contains(keyword)
                        );
                }

                if (this.TREATMENT_IDs != null)
                {
                    search.listVHisServiceReqExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.SERVICE_REQ_TYPE_ID.HasValue)
                {
                    search.listVHisServiceReqExpression.Add(o => o.SERVICE_REQ_TYPE_ID == this.SERVICE_REQ_TYPE_ID.Value);
                }
                if (this.SERVICE_REQ_TYPE_IDs != null)
                {
                    search.listVHisServiceReqExpression.Add(o => this.SERVICE_REQ_TYPE_IDs.Contains(o.SERVICE_REQ_TYPE_ID));
                }
                if (this.TRACKING_ID.HasValue)
                {
                    listVHisServiceReqExpression.Add(o => o.TRACKING_ID.HasValue && o.TRACKING_ID.Value == this.TRACKING_ID.Value);
                }
                if (this.TRACKING_IDs != null)
                {
                    listVHisServiceReqExpression.Add(o => o.TRACKING_ID.HasValue && this.TRACKING_IDs.Contains(o.TRACKING_ID.Value));
                }
                if (this.HAS_EXECUTE.HasValue && this.HAS_EXECUTE.Value)
                {
                    listVHisServiceReqExpression.Add(o => !o.IS_NO_EXECUTE.HasValue || o.IS_NO_EXECUTE.Value != ManagerConstant.IS_TRUE);
                }
                if (this.HAS_EXECUTE.HasValue && !this.HAS_EXECUTE.Value)
                {
                    listVHisServiceReqExpression.Add(o => o.IS_NO_EXECUTE.HasValue && o.IS_NO_EXECUTE.Value == ManagerConstant.IS_TRUE);
                }
                if (this.INTRUCTION_DATE_FROM.HasValue)
                {
                    listVHisServiceReqExpression.Add(o => o.INTRUCTION_DATE >= this.INTRUCTION_DATE_FROM.Value);
                }
                if (this.INTRUCTION_DATE_TO.HasValue)
                {
                    listVHisServiceReqExpression.Add(o => o.INTRUCTION_DATE <= this.INTRUCTION_DATE_TO.Value);
                }

                search.listVHisServiceReqExpression.AddRange(listVHisServiceReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
                search.ExtraOrderField1 = ORDER_FIELD1;
                search.ExtraOrderDirection1 = ORDER_DIRECTION1;
                search.ExtraOrderField2 = ORDER_FIELD2;
                search.ExtraOrderDirection2 = ORDER_DIRECTION2;
                search.ExtraOrderField3 = ORDER_FIELD3;
                search.ExtraOrderDirection3 = ORDER_DIRECTION3;
                search.ExtraOrderField4 = ORDER_FIELD4;
                search.ExtraOrderDirection4 = ORDER_DIRECTION4;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisServiceReqExpression.Clear();
                search.listVHisServiceReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
