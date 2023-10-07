using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00010
{
    public class Mrs00010Processor : AbstractProcessor
    {
        //HisExamServiceReqViewFilterQuery castFilter = null; 
        List<Mrs00010RDO> listExam = new List<Mrs00010RDO>();
        List<Mrs00010RDO> listInPatient = new List<Mrs00010RDO>();
        Mrs00010Filter mrs00002Filter;
        List<HIS_SERVICE_REQ> listTemp;
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

        public Mrs00010Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00010Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                mrs00002Filter = ((Mrs00010Filter)this.reportFilter);
                CommonParam getParam = new CommonParam();
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.CREATE_TIME_FROM = mrs00002Filter.CREATE_TIME_FROM;
                filter.CREATE_TIME_TO = mrs00002Filter.CREATE_TIME_TO;
                filter.INTRUCTION_DATE_FROM = mrs00002Filter.INTRUCTION_TIME_FROM;
                filter.INTRUCTION_DATE_TO = mrs00002Filter.INTRUCTION_TIME_TO;
                filter.EXECUTE_ROOM_IDs = mrs00002Filter.EXAM_ROOM_IDs;
                filter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                listTemp = new HisServiceReqManager(getParam).Get(filter);
                if (listTemp != null && this.mrs00002Filter.ROOM_IDs != null && this.mrs00002Filter.ROOM_IDs.Count > 0)
                    listTemp = listTemp.Where(o => this.mrs00002Filter.ROOM_IDs.Contains(o.EXECUTE_ROOM_ID)).ToList();

                HisPatientTypeAlterViewFilterQuery patiFilter = new HisPatientTypeAlterViewFilterQuery();
                patiFilter.TREATMENT_IDs = listTemp.Select(s => s.TREATMENT_ID).Distinct().ToList();
                var listPatientTypeAlter = new HisPatientTypeAlterManager(getParam).GetView(patiFilter);
                if (IsNotNullOrEmpty(listPatientTypeAlter))
                {
                    foreach (var item in listPatientTypeAlter)
                    {
                        if (dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                        {
                            if (dicPatientTypeAlter[item.TREATMENT_ID].LOG_TIME < item.LOG_TIME)
                            {
                                dicPatientTypeAlter[item.TREATMENT_ID] = item;
                            }
                        }
                        else
                        {
                            dicPatientTypeAlter[item.TREATMENT_ID] = item;
                        }
                    }
                }

                if (getParam.HasException)
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessInPatient(List<HIS_SERVICE_REQ> listTemp)
        {
            List<Mrs00010RDO> listDetailInPatient = (from r in listTemp select new Mrs00010RDO(r, dicPatientTypeAlter)).ToList();
            Dictionary<long, Mrs00010RDO> dicGroupByRoomInPatient = new Dictionary<long, Mrs00010RDO>();
            foreach (var data in listDetailInPatient)
            {
                if (data.FINISH_TIME == null || data.FINISH_TIME <= 0)
                {
                    if (!dicGroupByRoomInPatient.ContainsKey(data.EXECUTE_ROOM_ID))
                    {
                        dicGroupByRoomInPatient[data.EXECUTE_ROOM_ID] = data;
                    }
                    Mrs00010RDO temp = dicGroupByRoomInPatient[data.EXECUTE_ROOM_ID];
                    //temp.SetExtendField(data); 
                    temp.CountPatientTypeGroup(temp.currentPatientTypeAlter.PATIENT_TYPE_ID);
                }
            }
            listInPatient = dicGroupByRoomInPatient.Values.OrderBy(r => r.EXECUTE_ROOM_NAME).ToList();
        }

        private void ProcessExam(List<HIS_SERVICE_REQ> listTemp)
        {
            List<Mrs00010RDO> listDetailExam = (from r in listTemp select new Mrs00010RDO(r, dicPatientTypeAlter)).ToList();
            Dictionary<long, Mrs00010RDO> dicGroupByRoomExam = new Dictionary<long, Mrs00010RDO>();
            foreach (var data in listDetailExam)
            {
                if (!dicGroupByRoomExam.ContainsKey(data.EXECUTE_ROOM_ID))
                {
                    dicGroupByRoomExam[data.EXECUTE_ROOM_ID] = data;
                }
                Mrs00010RDO temp = dicGroupByRoomExam[data.EXECUTE_ROOM_ID];
                if (IsNotNull(data.currentPatientTypeAlter))
                {
                    temp.CountPatientTypeGroup(data.currentPatientTypeAlter.PATIENT_TYPE_ID);
                }
            }
            listExam = dicGroupByRoomExam.Values.OrderBy(r => r.EXECUTE_ROOM_NAME).ToList();
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTemp))
                {
                    ProcessExam(listTemp);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (mrs00002Filter.CREATE_TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("CREATE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(mrs00002Filter.CREATE_TIME_FROM.Value));
                }
                if (mrs00002Filter.CREATE_TIME_TO.HasValue)
                {
                    dicSingleTag.Add("CREATE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(mrs00002Filter.CREATE_TIME_TO.Value));
                }

                objectTag.AddObjectData(store, "Mrs00002", listExam);
                objectTag.SetUserFunction(store, "FuncSameExecuteRoomName", new CustomerFuncMergeSameData(listExam, 1));
                objectTag.SetUserFunction(store, "FuncSameExecuteDepartmentName", new CustomerFuncMergeSameData(listExam, 2));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        #region Orders Implementation
        class CustomerFuncMergeSameData : TFlexCelUserFunction
        {
            List<Mrs00010RDO> ListExam;
            int SameType;
            public CustomerFuncMergeSameData(List<Mrs00010RDO> listExam, int sameType)
            {
                ListExam = listExam;
                SameType = sameType;
            }
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                bool result = false;
                try
                {
                    int rowIndex = (int)parameters[0];
                    if (rowIndex > 0)
                    {
                        switch (SameType)
                        {
                            case 1:
                                result = (ListExam[rowIndex].EXECUTE_ROOM_NAME == ListExam[rowIndex - 1].EXECUTE_ROOM_NAME);
                                break;
                            case 2:
                                result = (ListExam[rowIndex].EXECUTE_DEPARTMENT_NAME == ListExam[rowIndex - 1].EXECUTE_DEPARTMENT_NAME);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return result;
            }
        }
        #endregion
    }
}
