using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisBed;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisTreatment; 
using SDA.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq;
using MOS.MANAGER.HisTreatmentBedRoom;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBedRoom; 

namespace MRS.Processor.Mrs00493
{
    class Mrs00493Processor : AbstractProcessor
    {
        Mrs00493Filter castFilter = null; 
        List<Mrs00493RDO> listRdo = new List<Mrs00493RDO>(); 

        long DATE_TIME = 0; 
        List<V_HIS_BED_ROOM> listBedRooms = new List<V_HIS_BED_ROOM>(); 
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<HIS_PATIENT> listPatients = new List<HIS_PATIENT>(); 
        List<V_HIS_TREATMENT_BED_ROOM> listTreatmentBedRooms = new List<V_HIS_TREATMENT_BED_ROOM>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_BED> listBeds = new List<V_HIS_BED>(); 


        public Mrs00493Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00493Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00493Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao Mrs00493: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                ////==================================== cấu hình lấy bn chưa vào buồng: 
                ////1 -- ko lấy bn chưa vào buồng, 2 -- lấy cả bn chưa vào buồng
                //SDA_CONFIG config = Loader.dictionaryConfig["MRS.TREATMENT_BED_ROOM.SELECT_IN_ROOM"]; 
                //if (config == null) throw new ArgumentNullException("MRS.TREATMENT_BED_ROOM.SELECT_IN_ROOM"); 
                //string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE; 
                //try
                //{
                //    this.SELECT_IN_ROOM = Convert.ToInt32(value); 
                //}
                //catch
                //{
                //    this.SELECT_IN_ROOM = 1; 
                //}
                //Inventec.Common.Logging.LogSystem.Info("SELECT_IN_ROOM :" + SELECT_IN_ROOM); 
                //==================================== lấy thời gian báo cáo
                this.DATE_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now).Value; 
                if (IsNotNull(castFilter.DATE_TIME))
                    this.DATE_TIME = castFilter.DATE_TIME.Value; 
                Inventec.Common.Logging.LogSystem.Info("DATE_TIME :" + this.DATE_TIME);
                this.DATE_TIME = this.DATE_TIME - this.DATE_TIME % 1000000 + 235959;
                Inventec.Common.Logging.LogSystem.Info("DATE_TIME :" + this.DATE_TIME);
                //==================================== lấy danh sách buồng bệnh của khoa
                if (IsNotNullOrEmpty(castFilter.DEPARTMENT_IDs))
                {
                    foreach (var departmentId in castFilter.DEPARTMENT_IDs)
                    {
                        HisBedRoomViewFilterQuery bedRoomViewFilter = new HisBedRoomViewFilterQuery(); 
                        bedRoomViewFilter.DEPARTMENT_ID = departmentId; 
                        bedRoomViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                        listBedRooms.AddRange(new MOS.MANAGER.HisBedRoom.HisBedRoomManager(param).GetView(bedRoomViewFilter));  
                    }

                    if (IsNotNullOrEmpty(castFilter.ROOM_IDs))
                        listBedRooms = listBedRooms.Where(w => castFilter.ROOM_IDs.Contains(w.ROOM_ID)).ToList(); 
                }
                //==================================== lấy danh sách bệnh nhân trong buồng bệnh
                //HisTreatmentBedRoomViewFilterQuery treatmentBedRoomViewFilter = new HisTreatmentBedRoomViewFilterQuery(); 
                //treatmentBedRoomViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                //treatmentBedRoomViewFilter.ADD_TIME_TO = this.DATE_TIME; 
                //treatmentBedRoomViewFilter.REMOVE_TIME_FROM = this.DATE_TIME; 
                //listTreatmentBedRooms.AddRange(new MOS.MANAGER.HisTreatmentBedRoom.HisTreatmentBedRoomManager(param).GetView(treatmentBedRoomViewFilter)); 

                HisTreatmentBedRoomViewFilterQuery treatmentBedRoomViewFilter2 = new HisTreatmentBedRoomViewFilterQuery(); 
                treatmentBedRoomViewFilter2.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                treatmentBedRoomViewFilter2.ADD_TIME_TO = this.DATE_TIME; 
                treatmentBedRoomViewFilter2.IS_IN_ROOM = true;
                listTreatmentBedRooms.AddRange(new MOS.MANAGER.HisTreatmentBedRoom.HisTreatmentBedRoomManager(param).GetView(treatmentBedRoomViewFilter2)); 

                Inventec.Common.Logging.LogSystem.Error("Số BN: " + listTreatmentBedRooms.Count()); 

                listTreatmentBedRooms = listTreatmentBedRooms.Distinct().ToList(); 
                listTreatmentBedRooms = listTreatmentBedRooms.Where(w => listBedRooms.Select(s => s.ID).Contains(w.BED_ROOM_ID)).Distinct().ToList(); 
                listTreatmentBedRooms = listTreatmentBedRooms.Where(w => w.ADD_TIME <= this.DATE_TIME && (w.REMOVE_TIME == null || (w.REMOVE_TIME != null && w.REMOVE_TIME >= this.DATE_TIME))).ToList(); 

                var skip = 0; 
                while (listTreatmentBedRooms.Count - skip > 0)
                {
                    var listIDs = listTreatmentBedRooms.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisPatientFilterQuery patientFilter = new HisPatientFilterQuery(); 
                    patientFilter.IDs = listIDs.Select(s => s.PATIENT_ID).ToList(); 
                    listPatients.AddRange(new MOS.MANAGER.HisPatient.HisPatientManager(param).Get(patientFilter)); 

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterViewFilter.TREATMENT_IDs = listIDs.Select(s => s.TREATMENT_ID).ToList(); 
                    patientTypeAlterViewFilter.LOG_TIME_TO = this.DATE_TIME; 
                    listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter)); 

                    HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                    treatmentViewFilter.IDs = listIDs.Select(s => s.TREATMENT_ID).ToList(); 
                    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter)); 
                }
                //danh sach giuong
                HisBedViewFilterQuery bedViewFilter = new HisBedViewFilterQuery();
                listBeds = new HisBedManager(param).GetView(bedViewFilter); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }
        
        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                foreach (var treatmentBedRoom in listTreatmentBedRooms)
                {
                    var rdo = new Mrs00493RDO(); 
                    rdo.TREATMENT_BED_ROOM = treatmentBedRoom; 

                    rdo.ADD_TIME = treatmentBedRoom.ADD_TIME; 
                    rdo.REMOVE_TIME = treatmentBedRoom.REMOVE_TIME; 

                    rdo.ROOM_ID = treatmentBedRoom.BED_ROOM_ID; 
                    rdo.ROOM_NAME = treatmentBedRoom.BED_ROOM_NAME; 

                    rdo.PATIENT_CODE = treatmentBedRoom.TDL_PATIENT_CODE; 
                    rdo.PATIENT_NAME = treatmentBedRoom.TDL_PATIENT_NAME; 
                    rdo.TREATMENT_CODE = treatmentBedRoom.TREATMENT_CODE; 
                    var treatment = listTreatments.Where(w => w.ID == treatmentBedRoom.TREATMENT_ID).ToList(); 
                    if (IsNotNullOrEmpty(treatment))
                    {
                        rdo.TREATMENT = treatment.First(); 
                    }

                    rdo.DOB = treatmentBedRoom.TDL_PATIENT_DOB; 
                    rdo.ADDRESS = treatmentBedRoom.TDL_PATIENT_ADDRESS; 
                    rdo.GENDER_NAME = treatmentBedRoom.TDL_PATIENT_GENDER_NAME; 
                    rdo.HEIN_CARD_NUMBER = treatmentBedRoom.TDL_HEIN_CARD_NUMBER; 
                    var patientTypeAlter = listPatientTypeAlters.Where(w => w.TREATMENT_ID == treatmentBedRoom.TREATMENT_ID).ToList(); 
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        patientTypeAlter = patientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList(); 
                        rdo.HEIN_CARD_FROM_TIME = patientTypeAlter.First().HEIN_CARD_FROM_TIME ?? 0; 
                        rdo.HEIN_CARD_TO_TIME = patientTypeAlter.First().HEIN_CARD_TO_TIME ?? 0; 
                    }

                    rdo.BED_NAME = treatmentBedRoom.BED_NAME;
                    var bed = listBeds.FirstOrDefault(o => o.ID == treatmentBedRoom.BED_ID);
                    if (bed != null)
                    {
                        rdo.BED_TYPE_CODE = bed.BED_TYPE_CODE;
                        rdo.BED_TYPE_NAME = bed.BED_TYPE_NAME;
                    }
                    var bedRoom = listBedRooms.FirstOrDefault(o => o.ID == treatmentBedRoom.BED_ROOM_ID);
                    if (bedRoom != null)
                    {
                        rdo.DEPARTMENT_CODE = bedRoom.DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = bedRoom.DEPARTMENT_NAME;
                    }
                    var patient = listPatients.Where(w => w.ID == treatmentBedRoom.PATIENT_ID).ToList(); 
                    if (IsNotNullOrEmpty(patient))
                        rdo.PHONE_NUMBER = patient.First().PHONE; 

                    listRdo.Add(rdo); 
                }

                listRdo = listRdo.Distinct().ToList(); 
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Lỗi xảy ra tại ProcessData: " + ex); 
                result = false; 
            }
            return result; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                //dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM); 
                //dicSingleTag.Add("TIME_TO", castFilter.TIME_TO); 
                if (IsNotNull(castFilter.DATE_TIME))
                    dicSingleTag.Add("DATE_TIME", castFilter.DATE_TIME); 

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                departmentFilter.IDs = castFilter.DEPARTMENT_IDs; 
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter); 
                if (IsNotNullOrEmpty(listDepartments))
                    dicSingleTag.Add("DEPARTMENT_NAME", String.Join(", ", listDepartments.Select(s => s.DEPARTMENT_NAME).ToList())); 

                if (IsNotNullOrEmpty(listBedRooms))
                    dicSingleTag.Add("BED_ROOM_NAME", String.Join(", ", listBedRooms.Select(s => s.BED_ROOM_NAME).ToList()));
                dicSingleTag.Add("COUNT_TREATMENT", listRdo.Count);
                bool exportSuccess = true; 
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.ROOM_NAME).ThenBy(t => t.BED_NAME).ToList()); 
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new MergeManyRowData()); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

    }

    class MergeManyRowData : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData; 
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                string s_Data = parameters[0].ToString(); 
                if (s_Data == s_CurrentData)
                {
                    return true; 
                }
                else
                {
                    s_CurrentData = s_Data; 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return false; 
            }
            throw new NotImplementedException(); 
        }
    }
}
