using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MRS.Processor.Mrs00547
{
    public class HisServiceReqFilterQuery : HisServiceReqFilter
    {
        public HisServiceReqFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ, bool>>> listHisServiceReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_REQ, bool>>>();



        internal HisServiceReqSO Query()
        {
            HisServiceReqSO search = new HisServiceReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisServiceReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisServiceReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisServiceReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.PARENT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.PARENT_ID == this.PARENT_ID);
                }
                if (this.PAAN_POSITION_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.PAAN_POSITION_ID.HasValue && o.PAAN_POSITION_ID.Value == this.PAAN_POSITION_ID.Value);
                }
                if (this.PAAN_POSITION_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => o.PAAN_POSITION_ID.HasValue && this.PAAN_POSITION_IDs.Contains(o.PAAN_POSITION_ID.Value));
                }
                if (this.REHA_SUM_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.REHA_SUM_ID.HasValue && o.REHA_SUM_ID.Value == this.REHA_SUM_ID.Value);
                }
                if (this.REHA_SUM_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => o.REHA_SUM_ID.HasValue && this.REHA_SUM_IDs.Contains(o.REHA_SUM_ID.Value));
                }
                if (this.TDL_PATIENT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.TDL_PATIENT_ID == this.TDL_PATIENT_ID);
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID);
                }
                if (this.SERVICE_REQ_TYPE_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.SERVICE_REQ_TYPE_ID == this.SERVICE_REQ_TYPE_ID);
                }
                if (this.REQUEST_DEPARTMENT_ID__OR__EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.REQUEST_DEPARTMENT_ID == this.REQUEST_DEPARTMENT_ID__OR__EXECUTE_DEPARTMENT_ID.Value || o.EXECUTE_DEPARTMENT_ID == this.REQUEST_DEPARTMENT_ID__OR__EXECUTE_DEPARTMENT_ID.Value);
                }
                if (this.REQUEST_ROOM_ID__OR__EXECUTE_ROOM_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.REQUEST_ROOM_ID == this.REQUEST_ROOM_ID__OR__EXECUTE_ROOM_ID.Value || o.EXECUTE_ROOM_ID == this.REQUEST_ROOM_ID__OR__EXECUTE_ROOM_ID.Value);
                }
                if (this.SERVICE_REQ_STT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.SERVICE_REQ_STT_ID == this.SERVICE_REQ_STT_ID.Value);
                }
                if (this.SERVICE_REQ_STT_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => this.SERVICE_REQ_STT_IDs.Contains(o.SERVICE_REQ_STT_ID));
                }
                if (this.PARENT_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => o.PARENT_ID.HasValue && this.PARENT_IDs.Contains(o.PARENT_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.SERVICE_REQ_CODE__ENDS_WITH))
                {
                    search.listHisServiceReqExpression.Add(o => o.SERVICE_REQ_CODE.EndsWith(this.SERVICE_REQ_CODE__ENDS_WITH));
                }
                if (this.EXECUTE_GROUP_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.EXECUTE_GROUP_ID.HasValue && o.EXECUTE_GROUP_ID.Value == this.EXECUTE_GROUP_ID.Value);
                }
                if (this.INTRUCTION_TIME_FROM.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.INTRUCTION_TIME >= this.INTRUCTION_TIME_FROM.Value);
                }
                if (this.INTRUCTION_TIME_TO.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.INTRUCTION_TIME <= this.INTRUCTION_TIME_TO.Value);
                }
                if (this.START_TIME_FROM.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.START_TIME.HasValue && o.START_TIME >= this.START_TIME_FROM.Value);
                }
                if (this.START_TIME_TO.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.START_TIME.HasValue && o.START_TIME <= this.START_TIME_TO.Value);
                }
                if (this.FINISH_TIME_FROM.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME >= this.FINISH_TIME_FROM.Value);
                }
                if (this.FINISH_TIME_TO.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.FINISH_TIME.HasValue && o.FINISH_TIME <= this.FINISH_TIME_TO.Value);
                }
                if (this.EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.EXECUTE_DEPARTMENT_ID == this.EXECUTE_DEPARTMENT_ID.Value);
                }
                if (this.REQUEST_DEPARTMENT_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.REQUEST_DEPARTMENT_ID == this.REQUEST_DEPARTMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.TRACKING_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.TRACKING_ID.HasValue && o.TRACKING_ID.Value == this.TRACKING_ID.Value);
                }
                if (this.TRACKING_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => o.TRACKING_ID.HasValue && this.TRACKING_IDs.Contains(o.TRACKING_ID.Value));
                }
                if (this.HAS_EXECUTE.HasValue && this.HAS_EXECUTE.Value)
                {
                    listHisServiceReqExpression.Add(o => !o.IS_NO_EXECUTE.HasValue || o.IS_NO_EXECUTE.Value != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (this.HAS_EXECUTE.HasValue && !this.HAS_EXECUTE.Value)
                {
                    listHisServiceReqExpression.Add(o => o.IS_NO_EXECUTE.HasValue && o.IS_NO_EXECUTE.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (!String.IsNullOrEmpty(this.BARCODE__EXACT))
                {
                    search.listHisServiceReqExpression.Add(o => o.BARCODE == this.BARCODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.SERVICE_REQ_CODE__EXACT))
                {
                    search.listHisServiceReqExpression.Add(o => o.SERVICE_REQ_CODE == this.SERVICE_REQ_CODE__EXACT);
                }
                if (this.PAAN_LIQUID_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.PAAN_LIQUID_ID.HasValue && o.PAAN_LIQUID_ID.Value == this.PAAN_LIQUID_ID.Value);
                }
                if (this.PAAN_LIQUID_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => o.PAAN_LIQUID_ID.HasValue && this.PAAN_LIQUID_IDs.Contains(o.PAAN_LIQUID_ID.Value));
                }
                if (this.DHST_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.DHST_ID.HasValue && o.DHST_ID.Value == this.DHST_ID.Value);
                }
                if (this.DHST_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => o.DHST_ID.HasValue && this.DHST_IDs.Contains(o.DHST_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.TREATMENT_CODE__EXACT))
                {
                    listHisServiceReqExpression.Add(o => o.TDL_TREATMENT_CODE == this.TREATMENT_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.SERVICE_REQ_CODE__EXACT))
                {
                    listHisServiceReqExpression.Add(o => o.SERVICE_REQ_CODE == this.SERVICE_REQ_CODE__EXACT);
                }
                if (this.SERVICE_REQ_TYPE_IDs != null)
                {
                    listHisServiceReqExpression.Add(o => this.SERVICE_REQ_TYPE_IDs.Contains(o.SERVICE_REQ_TYPE_ID));
                }
                if (this.REQUEST_ROOM_IDs != null)
                {
                    listHisServiceReqExpression.Add(o => this.REQUEST_ROOM_IDs.Contains(o.REQUEST_ROOM_ID));
                }
                if (this.REQUEST_ROOM_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.REQUEST_ROOM_ID == this.REQUEST_ROOM_ID.Value);
                }
                if (this.EXECUTE_ROOM_ID.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.EXECUTE_ROOM_ID == this.EXECUTE_ROOM_ID.Value);
                }
                if (this.INTRUCTION_DATE_FROM.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.INTRUCTION_DATE >= this.INTRUCTION_DATE_FROM.Value);
                }
                if (this.INTRUCTION_DATE_TO.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.INTRUCTION_DATE <= this.INTRUCTION_DATE_TO.Value);
                }
                if (this.INTRUCTION_DATE__EQUAL.HasValue)
                {
                    search.listHisServiceReqExpression.Add(o => o.INTRUCTION_DATE == this.INTRUCTION_DATE__EQUAL.Value);
                }
                if (!String.IsNullOrEmpty(this.KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME))
                {
                    this.KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME = this.KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME.ToLower().Trim();
                    listHisServiceReqExpression.Add(o =>
                        o.TDL_PATIENT_NAME.ToLower().Contains(this.KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME)
                        || o.SERVICE_REQ_CODE.ToLower().Contains(this.KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME)
                        || o.TDL_TREATMENT_CODE.ToLower().Contains(this.KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME)
                        );
                }
                if (this.EXECUTE_ROOM_IDs != null)
                {
                    listHisServiceReqExpression.Add(o => this.EXECUTE_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID));
                }
                if (this.NOT_IN_SERVICE_REQ_TYPE_IDs != null)
                {
                    search.listHisServiceReqExpression.Add(o => !this.NOT_IN_SERVICE_REQ_TYPE_IDs.Contains(o.SERVICE_REQ_TYPE_ID));
                }
                if (this.IS_SEND_LIS.HasValue && this.IS_SEND_LIS.Value)
                {
                    listHisServiceReqExpression.Add(o => o.LIS_STT_ID.HasValue);
                }
                if (this.IS_SEND_LIS.HasValue && !this.IS_SEND_LIS.Value)
                {
                    listHisServiceReqExpression.Add(o => !o.LIS_STT_ID.HasValue);
                }

                search.listHisServiceReqExpression.AddRange(listHisServiceReqExpression);
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
                search.listHisServiceReqExpression.Clear();
                search.listHisServiceReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
