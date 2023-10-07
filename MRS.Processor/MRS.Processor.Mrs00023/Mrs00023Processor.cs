using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00023
{
    public class Mrs00023Processor : AbstractProcessor
    {
        Mrs00023Filter castFilter = null;
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, HIS_PATIENT> dicPatient = new Dictionary<long, HIS_PATIENT>();
        List<Mrs00023RDO> listInPatient = new List<Mrs00023RDO>();
        List<HIS_SERVICE_REQ> listTemp = null;
        Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom = new Dictionary<long, V_HIS_EXECUTE_ROOM>();

        public Mrs00023Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00023Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00023Filter)this.reportFilter);
                CommonParam getParam = new CommonParam();
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.CREATE_TIME_FROM = castFilter.CREATE_TIME_FROM;
                filter.CREATE_TIME_TO = castFilter.CREATE_TIME_TO;
                filter.INTRUCTION_DATE_FROM = castFilter.INTRUCTION_TIME_FROM;
                filter.INTRUCTION_DATE_TO = castFilter.INTRUCTION_TIME_TO;
                filter.EXECUTE_ROOM_IDs = castFilter.ROOM_IDs;
                filter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                listTemp = new HisServiceReqManager(getParam).Get(filter);

                if (listTemp != null && this.castFilter.ROOM_IDs != null && this.castFilter.ROOM_IDs.Count > 0)
                    listTemp = listTemp.Where(o => this.castFilter.ROOM_IDs.Contains(o.EXECUTE_ROOM_ID)).ToList();

                HisExecuteRoomViewFilterQuery exroom = new HisExecuteRoomViewFilterQuery();
                exroom.IS_EXAM = true;
                var ExamRooms = new HisExecuteRoomManager(param).GetView(exroom);
                if (IsNotNullOrEmpty(ExamRooms))
                {
                    foreach (var item in ExamRooms)
                    {
                        if (!dicRoom.ContainsKey(item.ROOM_ID)) dicRoom[item.ROOM_ID] = item;
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

        private void getPatient(List<HIS_SERVICE_REQ> listTemp)
        {
            try
            {
                List<HIS_PATIENT> LisPatient = new List<HIS_PATIENT>();
                CommonParam paramGet = new CommonParam();
                if (IsNotNullOrEmpty(listTemp))
                {
                    List<long> listPatientId = listTemp.Select(o => o.TDL_PATIENT_ID).ToList().Distinct().ToList();
                    var skip = 0;
                    while (listPatientId.Count - skip > 0)
                    {
                        var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var LisPatientLib = new HisPatientManager(paramGet).Get(patientFilter);
                        LisPatient.AddRange(LisPatientLib);

                    }
                    LisPatient = LisPatient.OrderByDescending(p => p.ID).ToList();
                    foreach (var item in LisPatient)
                    {
                        if (!dicPatient.ContainsKey(item.ID)) dicPatient[item.ID] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void getPatientTypeAlter(List<HIS_SERVICE_REQ> listTemp)
        {
            try
            {
                List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
                CommonParam paramGet = new CommonParam();
                if (IsNotNullOrEmpty(listTemp))
                {
                    List<long> listTreatmentId = listTemp.Select(o => o.TREATMENT_ID).ToList().Distinct().ToList();
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        LisPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }
                    LisPatientTypeAlter = LisPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ThenByDescending(p => p.ID).ToList();
                    foreach (var item in LisPatientTypeAlter)
                    {
                        if (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID)) dicPatientTypeAlter[item.TREATMENT_ID] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessInPatient(List<HIS_SERVICE_REQ> listTemp, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypaAlter)
        {
            List<Mrs00023RDO> listDetailInPatient = (from r in listTemp select new Mrs00023RDO(r, dicPatientTypaAlter)).ToList();
            Dictionary<long, Mrs00023RDO> dicGroupByRoomInPatient = new Dictionary<long, Mrs00023RDO>();
            foreach (var data in listDetailInPatient)
            {
                if (data.FINISH_TIME == null || data.FINISH_TIME <= 0)
                {
                    if (!dicGroupByRoomInPatient.ContainsKey(data.TREATMENT_ID))
                    {
                        dicGroupByRoomInPatient[data.TREATMENT_ID] = data;
                    }
                    Mrs00023RDO temp = dicGroupByRoomInPatient[data.TREATMENT_ID];
                    if (data.currentPatientTypeAlter != null)
                    {
                        temp.CountPatientTypeGroup(data.currentPatientTypeAlter.PATIENT_TYPE_ID);
                    }
                }
            }
            listInPatient = dicGroupByRoomInPatient.Values.OrderBy(r => r.EXECUTE_ROOM_ID).ThenBy(s => s.TDL_PATIENT_ID).ToList();
            addInfo(ref listInPatient);
        }

        private void addInfo(ref List<Mrs00023RDO> listInPatient)
        {
            try
            {
                //phong kham
                //them thong tin phong kham
                foreach (var item in listInPatient)
                {
                    item.EXECUTE_ROOM_NAME = dicRoom.ContainsKey(item.EXECUTE_ROOM_ID) ? dicRoom[item.EXECUTE_ROOM_ID].EXECUTE_ROOM_NAME : "";
                    item.EXECUTE_DEPARTMENT_NAME = dicRoom.ContainsKey(item.EXECUTE_ROOM_ID) ? dicRoom[item.EXECUTE_ROOM_ID].DEPARTMENT_NAME : "";
                    item.PATIENT_NAME = dicPatient.ContainsKey(item.TDL_PATIENT_ID) ? dicPatient[item.TDL_PATIENT_ID].VIR_PATIENT_NAME : "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTemp))
                {
                    getPatientTypeAlter(listTemp);
                    getPatient(listTemp);
                    ProcessInPatient(listTemp, dicPatientTypeAlter);
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
                if (castFilter.CREATE_TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.CREATE_TIME_FROM.Value));
                }
                if (castFilter.CREATE_TIME_TO.HasValue)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.CREATE_TIME_TO.Value));
                }

                objectTag.AddObjectData(store, "Report", listInPatient);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
