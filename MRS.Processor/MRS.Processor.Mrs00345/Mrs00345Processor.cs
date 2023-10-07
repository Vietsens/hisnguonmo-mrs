using MOS.MANAGER.HisService;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBedRoom;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;
using SDA.EFMODEL.DataModels;
using SDA.Filter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00345
{
    class Mrs00345Processor : AbstractProcessor
    {
        Mrs00345Filter castFilter = null;

        List<Mrs00345RDO> listRdo = new List<Mrs00345RDO>();

        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_ROOM> listRooms = new List<HIS_ROOM>();
        List<HIS_BED_ROOM> listBedRooms = new List<HIS_BED_ROOM>();
        List<HIS_EXECUTE_ROOM> listExecuteRooms = new List<HIS_EXECUTE_ROOM>();
        List<V_HIS_SERVICE_REQ> listSereReq = new List<V_HIS_SERVICE_REQ>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_EXECUTE_ROOM> listExamRooms = new List<V_HIS_EXECUTE_ROOM>();

        public Mrs00345Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00345Filter);
        }
        //get dữ liệu từ data base
        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00345Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();

                //HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                //departmentFilter.IS_CLINICAL = true; 
                //listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter); 

                //HisRoomFilterQuery roomFilter = new HisRoomFilterQuery(); 
                //listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(param).Get(roomFilter); 

                // buồng điều trị nội trú
                HisBedRoomFilterQuery bedRoomFilter = new HisBedRoomFilterQuery();
                listBedRooms = new HisBedRoomManager(param).Get(bedRoomFilter);
                //listBedRooms = new MOS.MANAGER.HisBedRoom.HisBedRoomManager(param).Get(bedRoomFilter);
                
                // phòng khám
                HisExecuteRoomFilterQuery executeRoomFilter = new HisExecuteRoomFilterQuery();
                executeRoomFilter.IS_EXAM = true;
                listExecuteRooms = new MOS.MANAGER.HisExecuteRoom.HisExecuteRoomManager(param).Get(executeRoomFilter);

                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
               
               // List<HIS_BED_ROOM> listExamBedRooms = new List<HIS_BED_ROOM>();
                List<V_HIS_EXECUTE_ROOM> listViewExecuteRooms = new List<V_HIS_EXECUTE_ROOM>();
               // List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
                HisServiceReqViewFilterQuery sereServViewFilter = new HisServiceReqViewFilterQuery();
                sereServViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sereServViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                sereServViewFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
            

                if (IsNotNullOrEmpty(castFilter.DEPARTMENT_IDs))
                    sereServViewFilter.EXECUTE_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                if (IsNotNullOrEmpty(castFilter.SERVICE_REQ_STT_IDs))
                    sereServViewFilter.SERVICE_REQ_STT_IDs = castFilter.SERVICE_REQ_STT_IDs;
              //  if (IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                
                listSereReq = new HisServiceReqManager(param).GetView(sereServViewFilter);

                

              
                if (castFilter.EXECUTE_ROOM_IDs != null)
                {
                    listSereReq = listSereReq.Where(o => castFilter.EXECUTE_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID)).ToList();
                }
      
             
                    



                if (IsNotNullOrEmpty(listSereReq))
                {
                    //var listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
                    var skip = 0;
                    while (listSereReq.Count - skip > 0)
                    {
                        var listIds = listSereReq.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery();
                        patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.TREATMENT_ID).ToList();
                        patientTypeAlterViewFilter.ORDER_FIELD = "LOG_TIME";
                        patientTypeAlterViewFilter.ORDER_DIRECTION = "DESC";
                        listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));
                    }

                    skip = 0;
                    while (listSereReq.Count - skip > 0)
                    {
                        var listIds = listSereReq.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                        treatmentViewFilter.PATIENT_IDs = listIds.Select(s => s.TDL_PATIENT_ID).ToList();
                        listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter));
                    }
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00336");
                }
            }

            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        //Xử lý dữ liệu để tạo listRdo
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listSereReq))
                {
                    if (castFilter.NEW_REGISTRATION || castFilter.APPOINTMENT || castFilter.TRANSFER_ROOM || castFilter.TRANSFER_CLINICAL)
                    {
                        var listSereServFilter = new List<V_HIS_SERVICE_REQ>();

                        if (castFilter.NEW_REGISTRATION)
                        {
                            var listSereServNewRegis = new List<V_HIS_SERVICE_REQ>();
                            foreach (var i in listSereReq)
                            {
                                var patient = listTreatments.Where(s => s.PATIENT_ID == i.TDL_PATIENT_ID).ToList();
                                if (patient.Count == 1)
                                    listSereServNewRegis.Add(i);
                            }
                            listSereServFilter.AddRange(listSereServNewRegis);
                        }

                        if (castFilter.APPOINTMENT)
                        {
                            var listSereServAppointments = new List<V_HIS_SERVICE_REQ>();
                            foreach (var i in listSereReq)
                            {
                                var treatment = listTreatments.Where(s => s.ID == i.TREATMENT_ID).ToList();
                                if (treatment.First().APPOINTMENT_CODE != null)
                                    listSereServAppointments.Add(i);
                            }
                            listSereServFilter.AddRange(listSereServAppointments);
                        }

                        if (castFilter.TRANSFER_ROOM)
                        {

                            var listSereServTranferRooms = new List<V_HIS_SERVICE_REQ>();
                            listSereServTranferRooms = listSereReq.Where(w => listExecuteRooms.Select(s => s.ROOM_ID).Contains(w.REQUEST_ROOM_ID)).ToList();
                            listSereServFilter.AddRange(listSereServTranferRooms);
                        }

                        if (castFilter.TRANSFER_CLINICAL)
                        {

                            var listSereServTranferClinicals = new List<V_HIS_SERVICE_REQ>();
                            listSereServTranferClinicals = listSereReq.Where(w => listBedRooms.Select(s => s.ROOM_ID).Contains(w.REQUEST_ROOM_ID)).ToList();
                            listSereServFilter.AddRange(listSereServTranferClinicals);
                        }

                        listSereServFilter = listSereServFilter.GroupBy(o=>o.ID).Select(p=>p.First()).ToList();
                        var srIds = listSereServFilter.Select(s => s.ID);
                        listSereReq = listSereReq.Where(w => srIds.Contains(w.ID)).ToList();
                    }
                    var dicPatientTypeAlter = listPatientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                    foreach (var sereServ in listSereReq)
                    {
                        var rdo = new Mrs00345RDO();
                        rdo.PATIENT_CODE = sereServ.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = sereServ.TDL_PATIENT_NAME;
                        rdo.TREATMENT_CODE = sereServ.TREATMENT_CODE;
                        rdo.DOB = sereServ.TDL_PATIENT_DOB;
                        rdo.GENDER_NAME = sereServ.TDL_PATIENT_GENDER_NAME;

                        rdo.ICD_CODE = sereServ.ICD_CODE;
                        rdo.ICD_NAME = sereServ.ICD_NAME;
                        rdo.ICD_TEXT = sereServ.ICD_TEXT;
                        rdo.ICD_SUB_CODE = sereServ.ICD_SUB_CODE;

                        rdo.HOSPITALIZATION_REASON = listTreatments.Where(w => w.ID == sereServ.TREATMENT_ID).First().HOSPITALIZATION_REASON;
                        rdo.EXECUTE_USERNAME = sereServ.EXECUTE_USERNAME;
                        //rdo.EXECUTE_LOGINNAME = listSereReq.Where(w => w.ID == sereServ.TREATMENT_ID).First().HOSPITALIZATION_REASON;

                        rdo.ADDRESS = listTreatments.Where(w => w.ID == sereServ.TREATMENT_ID).First().TDL_PATIENT_ADDRESS;
                        rdo.EXECUTE_ROOM_NAME = sereServ.EXECUTE_ROOM_NAME;
                        rdo.IS_EXECUTE = sereServ.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT ? "x" : "";
                        rdo.IN_PROCESS = sereServ.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL ? "x" : "";
                        if (dicPatientTypeAlter.ContainsKey(sereServ.TREATMENT_ID))
                        {
                            var patientTypeAlter = dicPatientTypeAlter[sereServ.TREATMENT_ID];

                            if (IsNotNullOrEmpty(patientTypeAlter))
                            {
                                rdo.HEIN_CARD_NUMBER = patientTypeAlter.First().HEIN_CARD_NUMBER;
                                rdo.HEIN_CARD_FROM_TIME = patientTypeAlter.First().HEIN_CARD_FROM_TIME ?? 0;
                                rdo.HEIN_CARD_TO_TIME = patientTypeAlter.First().HEIN_CARD_TO_TIME ?? 0;
                                rdo.PATIENT_TYPE_NAME = patientTypeAlter.First().PATIENT_TYPE_NAME;

                            }
                        }
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        // xuất ra báo cáo
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM >= 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));

                }
                if (castFilter.TIME_TO >= 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.EXECUTE_ROOM_NAME).ThenBy(o => o.EXECUTE_ROOM_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
