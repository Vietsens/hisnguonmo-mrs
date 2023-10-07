using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using SDA.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00469
{
    class Mrs00469Processor : AbstractProcessor
    {
        Mrs00469Filter castFilter = null; 
        //List<Mrs00469RDO> listRdo = new List<Mrs00469RDO>(); 

        protected long ROOM_CODE_01 = -1; 
        protected long ROOM_CODE_02 = -1; 
        protected long ROOM_CODE_03 = -1; 

        //=================================================================
        // khám
        protected decimal DK_KHAM = 0;               // tổng đăng ký khám
        protected decimal NHAP_VIEN = 0;             // nhập viện
        protected decimal CHUYEN_VIEN = 0;           // chuyển viện
        protected decimal KHAM_CO_DON = 0;           // có đơn thuốc/ vật tư
        protected decimal KHAM_CO_THUOC = 0;         // có thuốc
        protected decimal SD_DICH_VU = 0;            // sử dụng dv kỹ thuật (ko thuốc / vật tư)
        protected decimal KO_CO_DON = 0;             // ko có đơn thuốc / vật tư (có thể ko sử dụng dịch vụ)
        protected decimal TREO = 0;                  // treo, chưa khóa tài chính
        // nọi trú
        protected decimal NT_DANG_DT = 0;            // số BN nội trú đang điều trị (TIME_TO)
        protected decimal NT_DANG_DT_HT = 0;         // bn đang điều trị đến hiện tại (tg lấy báo cáo)
        protected decimal NT_CHUYEN_VIEN = 0;        // nội trú chuyển viện
        protected decimal NT_HOM_TRƯƠC = 0;          // BN đang điều trị đầu kỳ báo cáo
        protected decimal NT_BN_MOI = 0;             // bn vào trong ngày
        protected decimal NT_BN_NGOAI_GIO = 0;       // bn vào ngoài giờ (tại các phòng khám ngoài giờ)
        protected decimal NT_RA_VIEN = 0;            // bn nội trú ra viện
        protected decimal NT_CHUA_THANH_TOAN = 0;    // ra viện chưa thanh toán (chưa duyệt khóa tài chính)
        // ngoại trú
        protected decimal NGT_DANG_DT = 0;            // số BN ngoại trú đang điều trị (TIME_TO)
        protected decimal NGT_DANG_DT_HT = 0;         // bn đang điều trị đến hiện tại (tg lấy báo cáo)
        protected decimal NGT_CHUYEN_VIEN = 0;        // ngoại trú chuyển viện
        protected decimal NGT_HOM_TRƯƠC = 0;          // BN đang điều trị đầu kỳ báo cáo
        protected decimal NGT_BN_MOI = 0;             // bn vào trong ngày
        protected decimal NGT_BN_NGOAI_GIO = 0;       // bn vào ngoài giờ (tại các phòng khám ngoài giờ)
        protected decimal NGT_RA_VIEN = 0;            // bn ngoại trú ra viện
        protected decimal NGT_CHUA_THANH_TOAN = 0;    // ra viện chưa thanh toán (chưa duyệt khóa tài chính)
        //=================================================================
        public Mrs00469Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00469Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00469Filter)this.reportFilter; 
                Inventec.Common.Logging.LogSystem.Info("1"); 
                GetConfig(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("MRS00469 Lỗi xảy ra tại GetData: " + ex); 
                result = false; 
            }
            return result; 
        }

        protected void GetConfig()
        {
            Inventec.Common.Logging.LogSystem.Info("2"); 
            //Tao cau hinh 
            var config = Loader.dictionaryConfig["MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KTNG"];
            if (config == null) throw new ArgumentNullException("MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KTNG"); 
            string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE; 
            var listROOM_CODE__KTNG = value.Split(',').ToList(); 

            config = Loader.dictionaryConfig["MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KTS"];
            if (config == null) throw new ArgumentNullException("MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KTS"); 
            value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE; 
            var listROOM_CODE__KTS = value.Split(',').ToList(); 

            config = Loader.dictionaryConfig["MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KCC"];
            if (config == null) throw new ArgumentNullException("MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KCC"); 
            value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE; 
            var listROOM_CODE__KCC = value.Split(',').ToList(); 

            var listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(param).GetView(new HisRoomViewFilterQuery()); 
            if (IsNotNullOrEmpty(listROOM_CODE__KTNG))
            {
                var listROOM_ID__KTNG = listRooms.Where(w => listROOM_CODE__KTNG.Contains(w.ROOM_CODE)).ToList(); 
                if (IsNotNullOrEmpty(listROOM_ID__KTNG))
                    this.ROOM_CODE_01 = listROOM_ID__KTNG.First().ID; 
            }
            if (IsNotNullOrEmpty(listROOM_CODE__KTS))
            {
                var listROOM_ID__KTS = listRooms.Where(w => listROOM_CODE__KTS.Contains(w.ROOM_CODE)).ToList(); 
                if (IsNotNullOrEmpty(listROOM_ID__KTS))
                    this.ROOM_CODE_02 = listROOM_ID__KTS.First().ID; 
            }
            if (IsNotNullOrEmpty(listROOM_CODE__KCC))
            {
                var listROOM_ID__KCC = listRooms.Where(w => listROOM_CODE__KCC.Contains(w.ROOM_CODE)).ToList(); 
                if (IsNotNullOrEmpty(listROOM_ID__KCC))
                    this.ROOM_CODE_03 = listROOM_ID__KCC.First().ID; 
            }
        }

        protected void GetDataExam()
        {
            Inventec.Common.Logging.LogSystem.Info("3"); 
            HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
            treatmentViewFilter.IN_TIME_FROM = castFilter.TIME_FROM; 
            treatmentViewFilter.IN_TIME_TO = castFilter.TIME_TO; 
            var listTreatmentExams = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter); 


            var skip = 0; 
            var listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>(); 
            while (listTreatmentExams.Count - skip > 0)
            {
                var listIds = listTreatmentExams.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery(); 
                patientTypeAlterFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                patientTypeAlterFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                patientTypeAlterFilter.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM; 
                patientTypeAlterFilter.LOG_TIME_FROM = castFilter.TIME_FROM; 
                patientTypeAlterFilter.LOG_TIME_TO = castFilter.TIME_TO;
                listPatientTypeAlter.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).Get(patientTypeAlterFilter)); 
            }

            listTreatmentExams = listTreatmentExams.Where(w => listPatientTypeAlter.Select(s => s.TREATMENT_ID).Contains(w.ID)).ToList(); 

            if (IsNotNullOrEmpty(listTreatmentExams))
            {
                skip = 0; 
                var listSereServs = new List<V_HIS_SERE_SERV>(); 
                while (listTreatmentExams.Count - skip > 0)
                {
                    var listIds = listTreatmentExams.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                    sereServViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                    sereServViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                    sereServViewFilter.HAS_EXECUTE = true; 
                    sereServViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                    sereServViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter)); 
                }
                var sereServ = new List<long>(); 

                this.DK_KHAM = listTreatmentExams.Distinct().Count(); 

                this.NHAP_VIEN = listTreatmentExams.Where(w => w.CLINICAL_IN_TIME <= castFilter.TIME_TO).Count(); 

                this.CHUYEN_VIEN = listTreatmentExams.Where(w => w.OUT_TIME <= castFilter.TIME_TO && w.CLINICAL_IN_TIME == null && w.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV).Count(); 

                sereServ = listSereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Select(s => s.TDL_TREATMENT_ID.Value).ToList(); 
                this.KHAM_CO_THUOC = listTreatmentExams.Where(w => w.CLINICAL_IN_TIME == null && sereServ.Contains(w.ID)).Count(); 

                sereServ = listSereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList(); 
                this.KHAM_CO_DON = listTreatmentExams.Where(w => w.CLINICAL_IN_TIME == null && sereServ.Contains(w.ID)).Count(); 

                var usingService = listSereServs.Where(w => !sereServ.Contains(w.TDL_TREATMENT_ID.Value)).Select(s => s.TDL_TREATMENT_ID.Value).ToList(); 
                this.SD_DICH_VU = listTreatmentExams.Where(w => w.CLINICAL_IN_TIME == null && usingService.Contains(w.ID)).Count(); 

                this.KO_CO_DON = listTreatmentExams.Where(w => w.CLINICAL_IN_TIME == null && !sereServ.Contains(w.ID)).Count(); 

                this.TREO = listTreatmentExams.Where(w => w.CLINICAL_IN_TIME == null && w.FEE_LOCK_TIME == null).Count(); 


            }
        }

        protected void GetDataClinical()
        {
            Inventec.Common.Logging.LogSystem.Info("4"); 
            var listTreatments = new List<V_HIS_TREATMENT>(); 
            // bn vào trước kỳ + ra viện trong hoặc sau kỳ
            HisTreatmentViewFilterQuery treatmentViewFilter1 = new HisTreatmentViewFilterQuery(); 
            treatmentViewFilter1.CLINICAL_IN_TIME_TO = castFilter.TIME_FROM - 1; 
            treatmentViewFilter1.OUT_TIME_FROM = castFilter.TIME_FROM;
            listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter1)); 
            // bn vào trước kỳ và chưa ra viện
            HisTreatmentViewFilterQuery treatmentViewFilter2 = new HisTreatmentViewFilterQuery(); 
            treatmentViewFilter2.CLINICAL_IN_TIME_TO = castFilter.TIME_FROM - 1; 
            treatmentViewFilter2.IS_OUT = false; 
            listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter2)); 
            // bn vào trong kỳ
            HisTreatmentViewFilterQuery treatmentViewFilter3 = new HisTreatmentViewFilterQuery(); 
            treatmentViewFilter3.CLINICAL_IN_TIME_FROM = castFilter.TIME_FROM; 
            treatmentViewFilter3.CLINICAL_IN_TIME_TO = castFilter.TIME_TO; 
            listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter3)); 

            var skip = 0; 
            var listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
            while (listTreatments.Count - skip > 0)
            {
                var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                patientTypeAlterFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                patientTypeAlterFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                patientTypeAlterFilter.TREATMENT_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU,IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU}; 
                patientTypeAlterFilter.LOG_TIME_TO = castFilter.TIME_TO; 
                listPatientTypeAlter.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFilter)); 
            }

            #region not fast
            //var listPatyAlters = new List<HIS_PATY_ALTER>(); 
            //var listPatientTypeAlterGroupByTreatments = listPatientTypeAlter.GroupBy(g => g.TREATMENT_ID); 
            //foreach (var listPatientTypeAlterGroupByTreatment in listPatientTypeAlterGroupByTreatments)
            //{
            //    var outTime = castFilter.TIME_TO + 1; 
            //    var patyAlters = listPatientTypeAlterGroupByTreatment.OrderByDescending(o => o.LOG_TIME).ToList(); 
            //    foreach (var patyAlter in patyAlters)
            //    {
            //        HIS_PATY_ALTER his = new HIS_PATY_ALTER(); 
            //        his.TREATMENT_ID = patyAlter.TREATMENT_ID; 
            //        his.LOG_TIME = patyAlter.LOG_TIME; 
            //        his.OUT_TIME = outTime; 
            //        his.TREATMENT_TYPE_ID = patyAlter.TREATMENT_TYPE_ID; 
            //        his.PATIENT_TYPE_ID = patyAlter.PATIENT_TYPE_ID; 
            //        his.REQUEST_ROOM_ID = patyAlter.REQUEST_ROOM_ID; 

            //        outTime = patyAlter.LOG_TIME; 
            //        listPatyAlters.Add(his); 
            //    }
            //}

            //var listInOuts = new List<HIS_PATY_ALTER>(); 
            //var listTreatmantIds = new List<long>(); 
            //// nội trú
            ////listInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == HisTreatmentTypeCFG.TREATMENT_TYPE_ID__TREAT_IN).ToList(); 
            //listInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == HisTreatmentTypeCFG.TREATMENT_TYPE_ID__TREAT_IN && w.OUT_TIME >= castFilter.TIME_TO).ToList(); 
            //this.NT_DANG_DT = listTreatments.Where(w => (w.OUT_TIME == null || w.OUT_TIME > castFilter.TIME_TO) && listInOuts.Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //this.NT_DANG_DT_HT = listTreatments.Where(w => w.OUT_TIME == null && listInOuts.Where(ww => ww.OUT_TIME >= castFilter.TIME_TO).Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //listInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == HisTreatmentTypeCFG.TREATMENT_TYPE_ID__TREAT_IN && w.LOG_TIME < castFilter.TIME_FROM && w.OUT_TIME >= castFilter.TIME_FROM).ToList(); 
            //this.NT_HOM_TRƯƠC = listTreatments.Where(w => w.CLINICAL_IN_TIME < castFilter.TIME_FROM && listInOuts.Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //listInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == HisTreatmentTypeCFG.TREATMENT_TYPE_ID__TREAT_IN && w.LOG_TIME >= castFilter.TIME_FROM).ToList(); 
            //this.NT_BN_MOI = listTreatments.Where(w => w.CLINICAL_IN_TIME >= castFilter.TIME_FROM && w.CLINICAL_IN_TIME <= castFilter.TIME_TO && listPatyAlters.Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //listInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == HisTreatmentTypeCFG.TREATMENT_TYPE_ID__TREAT_IN && (w.REQUEST_ROOM_ID == this.ROOM_CODE_01 || w.REQUEST_ROOM_ID == this.ROOM_CODE_02 || w.REQUEST_ROOM_ID == this.ROOM_CODE_03)).ToList(); 
            //this.NGT_BN_NGOAI_GIO = listTreatments.Where(w => w.CLINICAL_IN_TIME >= castFilter.TIME_FROM && w.CLINICAL_IN_TIME <= castFilter.TIME_TO && listPatyAlters.Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //listInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == HisTreatmentTypeCFG.TREATMENT_TYPE_ID__TREAT_IN).ToList(); 
            //this.NT_CHUYEN_VIEN = listTreatments.Where(w => w.OUT_TIME != null && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN && listInOuts.Where(ww => ww.LOG_TIME <= w.OUT_TIME && ww.OUT_TIME >= w.OUT_TIME).Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //this.NT_RA_VIEN = listTreatments.Where(w => w.OUT_TIME != null && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN && listInOuts.Where(ww => ww.LOG_TIME <= w.OUT_TIME && ww.OUT_TIME >= w.OUT_TIME).Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //this.NT_RA_VIEN = listTreatments.Where(w => w.OUT_TIME != null && w.FEE_LOCK_TIME == null && listInOuts.Where(ww => ww.LOG_TIME <= w.OUT_TIME && ww.OUT_TIME >= w.OUT_TIME).Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            #endregion

            try
            {
                // nội trú
                var listLongs = listPatientTypeAlter.Where(ww => ww.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(s => s.TREATMENT_ID).ToList(); 
                var listInOuts = listTreatments.Where(w => listLongs.Contains(w.ID)).ToList(); 
                this.NT_DANG_DT = listInOuts.Where(w => w.OUT_TIME == null || w.OUT_TIME >= castFilter.TIME_TO).Count(); 
                this.NT_DANG_DT_HT = listInOuts.Where(w => w.OUT_TIME == null).Count(); 
                this.NT_CHUYEN_VIEN = listInOuts.Where(w => w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME <= castFilter.TIME_TO && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN).Count(); 
                this.NT_HOM_TRƯƠC = listInOuts.Where(w => (w.OUT_TIME == null || w.OUT_TIME >= castFilter.TIME_FROM) && w.CLINICAL_IN_TIME < castFilter.TIME_FROM).Count(); 
                this.NT_BN_MOI = listInOuts.Where(w => w.CLINICAL_IN_TIME <= castFilter.TIME_TO && w.CLINICAL_IN_TIME >= castFilter.TIME_FROM).Count(); 
                this.NT_RA_VIEN = listInOuts.Where(w => w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME <= castFilter.TIME_TO && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN).Count(); 
                this.NT_CHUA_THANH_TOAN = listInOuts.Where(w => w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME <= castFilter.TIME_TO && w.FEE_LOCK_TIME == null && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN).Count(); 
                listLongs = listPatientTypeAlter.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && w.EXECUTE_ROOM_ID != null && (w.EXECUTE_ROOM_ID == ROOM_CODE_01 || w.EXECUTE_ROOM_ID == ROOM_CODE_02 || w.EXECUTE_ROOM_ID == ROOM_CODE_03)).Select(s => s.TREATMENT_ID).ToList(); 
                this.NT_BN_NGOAI_GIO = listInOuts.Where(w => w.CLINICAL_IN_TIME <= castFilter.TIME_TO && w.CLINICAL_IN_TIME >= castFilter.TIME_FROM && listLongs.Contains(w.ID)).Count(); 
                // ngoại trú
                listLongs = listPatientTypeAlter.Where(ww => ww.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(s => s.TREATMENT_ID).ToList(); 
                listInOuts = listTreatments.Where(w => listLongs.Contains(w.ID)).ToList(); 
                this.NGT_DANG_DT = listInOuts.Where(w => w.OUT_TIME == null || w.OUT_TIME >= castFilter.TIME_TO).Count(); 
                this.NGT_DANG_DT_HT = listInOuts.Where(w => w.OUT_TIME == null).Count(); 
                this.NGT_CHUYEN_VIEN = listInOuts.Where(w => w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME <= castFilter.TIME_TO && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN).Count(); 
                this.NGT_HOM_TRƯƠC = listInOuts.Where(w => (w.OUT_TIME == null || w.OUT_TIME >= castFilter.TIME_FROM) && w.CLINICAL_IN_TIME < castFilter.TIME_FROM).Count(); 
                this.NGT_BN_MOI = listInOuts.Where(w => w.CLINICAL_IN_TIME <= castFilter.TIME_TO && w.CLINICAL_IN_TIME >= castFilter.TIME_FROM).Count(); 
                this.NGT_RA_VIEN = listInOuts.Where(w => w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME <= castFilter.TIME_TO && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN).Count(); 
                this.NGT_CHUA_THANH_TOAN = listInOuts.Where(w => w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME <= castFilter.TIME_TO && w.FEE_LOCK_TIME == null && w.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN).Count(); 
                listLongs = listPatientTypeAlter.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && w.EXECUTE_ROOM_ID != null && (w.EXECUTE_ROOM_ID == ROOM_CODE_01 || w.EXECUTE_ROOM_ID == ROOM_CODE_02 || w.EXECUTE_ROOM_ID == ROOM_CODE_03)).Select(s => s.TREATMENT_ID).ToList(); 
                this.NGT_BN_NGOAI_GIO = listInOuts.Where(w => w.CLINICAL_IN_TIME <= castFilter.TIME_TO && w.CLINICAL_IN_TIME >= castFilter.TIME_FROM && listLongs.Contains(w.ID)).Count(); 

                Inventec.Common.Logging.LogSystem.Info("MRS00469 Kết thúc xử lý nội trú / ngoại trú."); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("MRS00469 Lỗi xảy ra tại GetDataClinical: " + ex); 
            }
        }

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                Inventec.Common.Logging.LogSystem.Info("5"); 
                CommonParam paramGet = new CommonParam(); 

                GetDataExam(); 
                GetDataClinical(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("MRS00469 Lỗi xảy ra tại ProcessData: " + ex); 
                result = false; 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("6"); 
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO); 
                }
                // khám
                dicSingleTag.Add("DK_KHAM", this.DK_KHAM); 
                dicSingleTag.Add("NHAP_VIEN", this.NHAP_VIEN); 
                dicSingleTag.Add("CHUYEN_VIEN", this.CHUYEN_VIEN); 
                dicSingleTag.Add("KHAM_CO_DON", this.KHAM_CO_DON); 
                dicSingleTag.Add("KHAM_CO_THUOC", this.KHAM_CO_THUOC); 
                dicSingleTag.Add("SD_DICH_VU", this.SD_DICH_VU); 
                dicSingleTag.Add("KO_CO_DON", this.KO_CO_DON); 
                dicSingleTag.Add("TREO", this.TREO); 
                // nội trú
                dicSingleTag.Add("NT_DANG_DT", this.NT_DANG_DT); 
                dicSingleTag.Add("NT_DANG_DT_HT", this.NT_DANG_DT_HT); 
                dicSingleTag.Add("NT_CHUYEN_VIEN", this.NT_CHUYEN_VIEN); 
                dicSingleTag.Add("NT_HOM_TRƯƠC", this.NT_HOM_TRƯƠC); 
                dicSingleTag.Add("NT_BN_MOI", this.NT_BN_MOI); 
                dicSingleTag.Add("NT_BN_NGOAI_GIO", this.NT_BN_NGOAI_GIO); 
                dicSingleTag.Add("NT_RA_VIEN", this.NT_RA_VIEN); 
                dicSingleTag.Add("NT_CHUA_THANH_TOAN", this.NT_CHUA_THANH_TOAN); 
                // ngoại trú
                dicSingleTag.Add("NGT_DANG_DT", this.NGT_DANG_DT); 
                dicSingleTag.Add("NGT_DANG_DT_HT", this.NGT_DANG_DT_HT); 
                dicSingleTag.Add("NGT_CHUYEN_VIEN", this.NGT_CHUYEN_VIEN); 
                dicSingleTag.Add("NGT_HOM_TRƯƠC", this.NGT_HOM_TRƯƠC); 
                dicSingleTag.Add("NGT_BN_MOI", this.NGT_BN_MOI); 
                dicSingleTag.Add("NGT_BN_NGOAI_GIO", this.NGT_BN_NGOAI_GIO); 
                dicSingleTag.Add("NGT_RA_VIEN", this.NGT_RA_VIEN); 
                dicSingleTag.Add("NGT_CHUA_THANH_TOAN", this.NGT_CHUA_THANH_TOAN); 

                bool exportSuccess = true; 
                //objectTag.AddObjectData(store, "Report", listRdo); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
