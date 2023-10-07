using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentBedRoom;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisWorkPlace;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaCommune.Get;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Core.SdaProvince.Get;
using SDA.MANAGER.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00038
{
    class Mrs00038Processor : AbstractProcessor
    {
        Mrs00038Filter castFilter = null;
        List<Mrs00038RDO> listRdo = new List<Mrs00038RDO>();
        List<Mrs00038RDO> listRdo1 = new List<Mrs00038RDO>();
        List<Mrs00038RDO> listServiceReqRdo = new List<Mrs00038RDO>();
        List<Mrs00038RDO> listSereServBillRdo = new List<Mrs00038RDO>();
        List<Mrs00038RDO> listAllRdo = new List<Mrs00038RDO>();
        List<Mrs00038RDO> listDepartment = new List<Mrs00038RDO>();
        List<PATIENT_TREATMENT_EXAMREQ> listTreaPatientExamReq = new List<PATIENT_TREATMENT_EXAMREQ>();
        List<HIS_WORK_PLACE> listHisWorkPlace = new List<HIS_WORK_PLACE>();
        List<HIS_CAREER> listCareer = new List<HIS_CAREER>();
        List<HIS_SERE_SERV_BILL> listSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_TREATMENT_TYPE> listTreatmentType = new List<HIS_TREATMENT_TYPE>();
        CommonParam paramGet = new CommonParam();
        decimal totalPatientKH = 0;
        List<HIS_TREATMENT_END_TYPE> listHisTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<V_SDA_PROVINCE> listProvince = new List<V_SDA_PROVINCE>();
        List<V_SDA_DISTRICT> listDistrict = new List<V_SDA_DISTRICT>();
        List<V_SDA_COMMUNE> listCommune = new List<V_SDA_COMMUNE>();
        Dictionary<long, List<V_HIS_TREATMENT>> dicTranPati = new Dictionary<long, List<V_HIS_TREATMENT>>();
        List<HIS_BED_ROOM> ListBedRoom = new List<HIS_BED_ROOM>();

        List<HIS_PATIENT_CASE> ListPatientCase = new List<HIS_PATIENT_CASE>();

        public Mrs00038Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00038Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00038Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00038: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                listProvince = new SdaProvinceManager(new CommonParam()).Get<List<V_SDA_PROVINCE>>(new SdaProvinceViewFilterQuery()) ?? new List<V_SDA_PROVINCE>();
                listDistrict = new SdaDistrictManager(new CommonParam()).Get<List<V_SDA_DISTRICT>>(new SdaDistrictViewFilterQuery()) ?? new List<V_SDA_DISTRICT>();
                listCommune = new SdaCommuneManager(new CommonParam()).Get<List<V_SDA_COMMUNE>>(new SdaCommuneViewFilterQuery()) ?? new List<V_SDA_COMMUNE>();
                string query = "select * from his_career";
                listCareer = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_CAREER>(query);
                listHisTreatmentEndType = new HisTreatmentEndTypeManager(new CommonParam()).Get(new HisTreatmentEndTypeFilterQuery());
                listTreatmentType = new HisTreatmentTypeManager().Get(new HisTreatmentTypeFilterQuery());

                listTreaPatientExamReq.Clear();

                listTreaPatientExamReq = new ManagerSql().GetMain(this.castFilter, listProvince, listDistrict, listCommune);

                HisWorkPlaceFilterQuery WorkPlaceFilter = new HisWorkPlaceFilterQuery();
                listHisWorkPlace = new HisWorkPlaceManager(param).Get(WorkPlaceFilter);

                //danh sách buồng bệnh
                HisBedRoomFilterQuery brfilter = new HisBedRoomFilterQuery();
                ListBedRoom = new HisBedRoomManager(param).Get(brfilter) ?? new List<HIS_BED_ROOM>();


                string sql = "SELECT * FROM HIS_PATIENT_CASE WHERE IS_ACTIVE = 1";
                ListPatientCase = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CASE>(sql);

                GetSereServBill(castFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listTreaPatientExamReq.Clear();
                result = false;
            }
            return result;
        }

        private void GetSereServBill(Mrs00038Filter castFilter)
        {

            string query = string.Format("select ssb.* from his_sere_serv_bill ssb join his_transaction tran on ssb.bill_id = tran.id where tran.transaction_time between {0} and {1} and ssb.tdl_service_type_id=1\n", castFilter.TIME_FROM, castFilter.TIME_TO);
            listSereServBill = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV_BILL>(query);
        }


        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcesslistTreatment();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void ProcesslistTreatment()
        {
            try
            {
                if (listTreaPatientExamReq != null && listTreaPatientExamReq.Count > 0)
                {
                    CommonParam paramGet = new CommonParam();

                    int start = 0;
                    int count = listTreaPatientExamReq.Count;
                    List<long> TreatmentIds = listTreaPatientExamReq.Select(o => o.TREATMENT_ID??0).Distinct().ToList();
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<long> treatIds = TreatmentIds.Skip(start).Take(limit).ToList();
                        var listTreatPatientExamSub = listTreaPatientExamReq.Where(o => treatIds.Contains(o.TREATMENT_ID??0)).ToList();
                        var listSereServBillSub = listSereServBill.Where(o => treatIds.Contains(o.TDL_TREATMENT_ID)).ToList();
                        HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery();
                        patientTypeAlterFilter.TREATMENT_IDs = treatIds;
                        patientTypeAlterFilter.ORDER_DIRECTION = "DESC";
                        patientTypeAlterFilter.ORDER_FIELD = "LOG_TIME";
                        var listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter) ?? new List<HIS_PATIENT_TYPE_ALTER>();

                        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();
                        foreach (var tr in listPatientTypeAlter)
                        {
                            if (!dicPatientTypeAlter.ContainsKey(tr.TREATMENT_ID))
                                dicPatientTypeAlter[tr.TREATMENT_ID] = tr;
                        }

                        //lọc đúng tuyến trái tuyến
                        if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 2)
                        {
                            listTreatPatientExamSub = listTreatPatientExamSub.Where(p => dicPatientTypeAlter.ContainsKey(p.TREATMENT_ID??0) && dicPatientTypeAlter[p.TREATMENT_ID??0].RIGHT_ROUTE_CODE == "DT").ToList();
                        }
                        else if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 3)
                        {
                            listTreatPatientExamSub = listTreatPatientExamSub.Where(p => dicPatientTypeAlter.ContainsKey(p.TREATMENT_ID??0) && dicPatientTypeAlter[p.TREATMENT_ID??0].RIGHT_ROUTE_CODE == "TT").ToList();
                        }

                        HisDepartmentTranFilterQuery depaTranFilter = new HisDepartmentTranFilterQuery();
                        depaTranFilter.TREATMENT_IDs = treatIds;
                        var hisDepartmentTrans = new HisDepartmentTranManager(paramGet).Get(depaTranFilter);

                        HisTreatmentBedRoomFilterQuery bedRoomFilter = new HisTreatmentBedRoomFilterQuery();
                        bedRoomFilter.TREATMENT_IDs = treatIds;
                        var bedRoomSub = new HisTreatmentBedRoomManager(paramGet).Get(bedRoomFilter);

                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                        ssFilter.TREATMENT_IDs = listPatientTypeAlter.Select(s => s.TREATMENT_ID).ToList();
                        ssFilter.TDL_SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH };
                        var sereServs = new HisSereServManager().Get(ssFilter);
                        if (sereServs != null && sereServs.Count > 0)
                        {
                            totalPatientKH += sereServs.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.AMOUNT);
                        }


                        if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 1 || castFilter.INPUT_DATA_ID_EXAM_TYPE == 3 || castFilter.INPUT_DATA_ID_EXAM_TYPE == 2)
                        {
                            ProcessListServiceReq(paramGet, listTreatPatientExamSub, dicPatientTypeAlter, hisDepartmentTrans, bedRoomSub); //so luot kham benh + so luot kham benh dau tien +so luot kham da thanh toan
                        }
                        //else if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 2)chuyển sang xử lý ở sql
                        //{
                        //    ProcessListSereServBill(paramGet, listTreatPatientExamSub, listSereServBill, listPatientTypeAlter, hisDepartmentTrans); //so luot kham da thanh toan
                        //}
                        else if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 4 || castFilter.INPUT_DATA_ID_EXAM_TYPE == 0)
                        {
                            ProcessAllTreatment(paramGet, listTreatPatientExamSub, dicPatientTypeAlter, hisDepartmentTrans, bedRoomSub); //so luot dang ky kham
                        }
                        else if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 5)
                        {
                            ProcessCurrentListTreatment(paramGet, listTreatPatientExamSub, dicPatientTypeAlter, hisDepartmentTrans, bedRoomSub);
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void InforServiceReq(Mrs00038RDO rdo, PATIENT_TREATMENT_EXAMREQ serviceReq)
        {
            if (serviceReq != null)
            {
                rdo.EXECUTE_DEPARTMENT_CODE = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == serviceReq.EXECUTE_DEPARTMENT_ID)) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                rdo.EXECUTE_DEPARTMENT_NAME = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == serviceReq.EXECUTE_DEPARTMENT_ID)) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                rdo.EXAM_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                rdo.EXAM_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                rdo.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == serviceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                rdo.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                rdo.EXECUTE_LOGINNAME = serviceReq.EXECUTE_LOGINNAME;
                rdo.EXECUTE_USERNAME = serviceReq.EXECUTE_USERNAME;
                rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.INTRUCTION_TIME??0);
            }

        }
        private void InforTreatment(Mrs00038RDO rdo, PATIENT_TREATMENT_EXAMREQ trea, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<HIS_DEPARTMENT_TRAN> listDepartmentTran, List<HIS_TREATMENT_BED_ROOM> treatmentBedRooms)
        {
            rdo.ICD_CODE_TUYENDUOI = trea.ICD_CAUSE_CODE;
            rdo.ICD_NAME_TUYENDUOI = trea.ICD_CAUSE_NAME;
            rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(trea.IN_TIME);
            rdo.CREATOR = trea.TREATMENT_CREATOR;
            var treatmentEndType = listHisTreatmentEndType.FirstOrDefault(o => o.ID == trea.TREATMENT_END_TYPE_ID);
            rdo.PATIENT_NAME = trea.TDL_PATIENT_NAME;
            rdo.DIAGNOSE_TUYENDUOI = trea.ICD_TEXT;
            rdo.ICD_SUB_CODE = trea.ICD_SUB_CODE;
            rdo.JOB = trea.TDL_PATIENT_CAREER_NAME;
            if (trea.TDL_PATIENT_DOB > 1000)
            {
                rdo.DOB = trea.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.TDL_PATIENT_DOB ?? 0);
                rdo.AGE = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(trea.TDL_PATIENT_DOB ?? 0);
            }
            rdo.VIR_ADDRESS = trea.TDL_PATIENT_ADDRESS;

            //if (!string.IsNullOrEmpty(trea.TDL_PATIENT_ADDRESS))
            //{
            //    if ((trea.TDL_PATIENT_ADDRESS.ToLower().Contains("thành phố") || trea.TDL_PATIENT_ADDRESS.ToLower().Contains("tỉnh")) || (trea.TDL_PATIENT_ADDRESS.ToLower().Contains("thị xã") || trea.TDL_PATIENT_ADDRESS.ToLower().Contains("thị trấn")))
            //    {
            //        rdo.IS_CITY = "X";
            //    }
            //    else
            //    {
            //        rdo.IS_COUNTRYSIDE = "X";
            //    }

            //}

            if (!string.IsNullOrEmpty(trea.TDL_PATIENT_COMMUNE_CODE))
            {
                var sdaCommune = (listCommune ?? new List<V_SDA_COMMUNE>()).FirstOrDefault(o => o.COMMUNE_CODE == trea.TDL_PATIENT_COMMUNE_CODE);
                if (sdaCommune != null)
                {
                    if (!string.IsNullOrEmpty(sdaCommune.INITIAL_NAME) && (sdaCommune.INITIAL_NAME.ToLower().Contains("phường") || sdaCommune.INITIAL_NAME.ToLower().Contains("thị trấn")) || !string.IsNullOrEmpty(sdaCommune.DISTRICT_INITIAL_NAME) && (sdaCommune.DISTRICT_INITIAL_NAME.ToLower().Contains("quận") || sdaCommune.DISTRICT_INITIAL_NAME.ToLower().Contains("thành phố") || sdaCommune.DISTRICT_INITIAL_NAME.ToLower().Contains("thị xã")))
                    {
                        rdo.IS_CITY = "X";
                    }
                    else
                    {
                        rdo.IS_COUNTRYSIDE = "X";
                    }
                }

            }

            
            rdo.TDL_PATIENT_CAREER_NAME = trea.TDL_PATIENT_CAREER_NAME;
            rdo.PATIENT_CODE = trea.TDL_PATIENT_CODE;
            rdo.GENDER = trea.TDL_PATIENT_GENDER_NAME;
            rdo.PHONE = trea.TDL_PATIENT_PHONE;
            rdo.TDL_PATIENT_RELATIVE_PHONE = trea.TDL_PATIENT_RELATIVE_PHONE;
            rdo.ETHNIC_NAME = trea.ETHNIC_NAME;
            rdo.NATIONAL = trea.NATIONAL_NAME;
            rdo.AGE = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(trea.TDL_PATIENT_DOB ?? 0);

            rdo.RELATIVE = (trea.RELATIVE_TYPE ?? "") + ": " + (trea.RELATIVE_NAME ?? "");
            if (listHisWorkPlace != null)
            {
                rdo.WORK_PLACE_NAME = (listHisWorkPlace.FirstOrDefault(o => o.ID == trea.WORK_PLACE_ID) ?? new HIS_WORK_PLACE()).WORK_PLACE_NAME;
            }
            if (treatmentEndType != null)
            {
                rdo.TREATMENT_END_TYPE_CODE = treatmentEndType.TREATMENT_END_TYPE_CODE;
                rdo.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
            }
            //
            if (trea.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || trea.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
            {
                rdo.EXAM_END_TYPE_CODE = "DTNT";
                rdo.EXAM_END_TYPE_NAME = "Nhập viện Điều trị Nội trú";
            }
            else
            {
                if (treatmentEndType != null)
                {
                    rdo.EXAM_END_TYPE_CODE = treatmentEndType.TREATMENT_END_TYPE_CODE;
                    rdo.EXAM_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                }
            }
            rdo.TREATMENT_CODE = trea.TREATMENT_CODE;
            rdo.STORE_CODE = trea.STORE_CODE;
            rdo.IN_CODE = trea.IN_CODE;
            rdo.OUT_CODE = trea.OUT_CODE;
            rdo.HEIN_CARD_NUMBER = trea.TDL_HEIN_CARD_NUMBER;
            rdo.TDL_HEIN_MEDI_ORG_CODE = trea.TDL_HEIN_MEDI_ORG_CODE;
            rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.IN_TIME);
            rdo.TIME_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(trea.IN_TIME);
            rdo.CLINICAL_IN_TIME = trea.CLINICAL_IN_TIME;
            rdo.IN_TIME = trea.IN_TIME;
            IsBhyt(rdo, trea,dicPatientTypeAlter);
            CalcuatorAge(rdo, trea);
            //capcuu(rdo, trea);
            if (trea.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                rdo.IS_EMERGENCY = "X";
            }
            if (trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
            {
                rdo.IS_CURED = "X";
            }
            else if (trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
            {
                rdo.IS_ABATEMENT = "X";
            }
            else if (trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
            {
                rdo.IS_UNCHANGED = "X";
            }
            else if (trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
            {
                rdo.IS_AGGRAVATION = "X";
            }
           
            
        
            if (trea.OUT_TIME.HasValue)
            {
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                {
                    rdo.DATE_TRIP_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.OUT_TIME.Value);
                    rdo.TIME_TRIP_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(trea.OUT_TIME.Value);
                }
                else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                {
                    rdo.DATE_DEAD_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.OUT_TIME.Value);
                    rdo.TIME_DEAD_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(trea.OUT_TIME.Value);

                    if (trea.DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                    {
                        rdo.IS_DEAD_IN_24H = "X";
                    }
                }
                else
                {
                    rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(trea.OUT_TIME.Value);
                    rdo.TIME_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(trea.OUT_TIME.Value);
                }

                rdo.TOTAL_DATE_TREATMENT = DateDiff.diffDate(trea.IN_TIME, trea.OUT_TIME.Value);
                rdo.TREATMENT_DAY_COUNT = trea.TREATMENT_DAY_COUNT??0;
            }
            if (trea.END_DEPARTMENT_ID != null)
            {
                rdo.END_DEPARTMENT_ID = trea.END_DEPARTMENT_ID ?? 0;
                rdo.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == trea.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
            if (trea.END_ROOM_ID != null)
            {
                rdo.END_ROOM_ID = trea.END_ROOM_ID ?? 0;
                rdo.END_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == trea.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
            }
            List<HIS_DEPARTMENT_TRAN> trandepartment = new List<HIS_DEPARTMENT_TRAN>();

            rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == trea.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            rdo.DEPARTMENT_ID = trea.LAST_DEPARTMENT_ID ?? 0;
            var treatmentBedRoom = treatmentBedRooms.OrderBy(o => o.ADD_TIME).LastOrDefault(o => o.TREATMENT_ID == trea.TREATMENT_ID);
            if (treatmentBedRoom != null)
            {
                var bedRoom = ListBedRoom.FirstOrDefault(p => p.ID == treatmentBedRoom.BED_ROOM_ID);
                if (bedRoom != null)
                {
                    rdo.BED_ROOM_CODE = bedRoom.BED_ROOM_CODE;
                    rdo.BED_ROOM_NAME = bedRoom.BED_ROOM_NAME;
                }

            }

            trandepartment = listDepartmentTran.Where(o => o.TREATMENT_ID == trea.TREATMENT_ID && o.PREVIOUS_ID.HasValue).ToList();

            //Thoi gian chuyen khoa lay dung theo thoi gian khoa khac chuyen di/chuyen den khoa loc bc. ~ Thoi gian tao chuyen khoa
            if (trandepartment != null && trandepartment.Count > 0)
            {
                trandepartment = trandepartment.OrderBy(o => o.DEPARTMENT_IN_TIME).ToList();
                rdo.DEPARTMENT_TRAN_TIME_STR = string.Join("; ", trandepartment.Select(s => Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(s.CREATE_TIME ?? 0)).ToList());
            }
            if (trea.PATIENT_CASE_ID.HasValue)
            {
                var rc = ListPatientCase.FirstOrDefault(o => o.ID == trea.PATIENT_CASE_ID.Value);
                if (rc != null)
                {
                    rdo.PATIENT_CASE_NAME = rc.PATIENT_CASE_NAME;
                }
            }

            if (dicPatientTypeAlter[trea.TREATMENT_ID??0].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                rdo.DIAGNOSE_KKB = trea.ICD_NAME;
                rdo.ICD_CODE_KKB = trea.ICD_CODE;
                rdo.ICD_NAME_KKB = trea.ICD_NAME;
            }
            else
            {
                rdo.DIAGNOSE_KKB = trea.IN_ICD_NAME;
                rdo.ICD_CODE_KKB = trea.IN_ICD_CODE;
                rdo.ICD_NAME_KKB = trea.IN_ICD_NAME;
                rdo.DIAGNOSE_KDT = trea.ICD_NAME;
                rdo.ICD_CODE_KDT = trea.ICD_CODE;
            }

            var startDepartmentTran = listDepartmentTran.Where(o => o.TREATMENT_ID == trea.TREATMENT_ID).Where(p => p.DEPARTMENT_IN_TIME.HasValue).OrderBy(p => p.DEPARTMENT_IN_TIME.Value).ThenBy(q => q.ID).FirstOrDefault();
            if (startDepartmentTran != null)
            {
                rdo.START_DEPARTMENT_ID = startDepartmentTran.DEPARTMENT_ID;
                rdo.START_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == startDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }


            if (dicPatientTypeAlter != null)
            {
                var inDepartmentTran = listDepartmentTran.Where(o => o.TREATMENT_ID == trea.TREATMENT_ID && o.DEPARTMENT_ID == trea.HOSPITALIZE_DEPARTMENT_ID).OrderBy(p => p.DEPARTMENT_IN_TIME).ThenBy(q => q.ID).FirstOrDefault();
                if (inDepartmentTran != null)
                {
                    rdo.RX_IN_DEPARTMENT_ID = inDepartmentTran.DEPARTMENT_ID;
                    rdo.RX_IN_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                }
                if (trea.IN_DEPARTMENT_ID != null && (trea.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || trea.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                {
                    rdo.FX_IN_DEPARTMENT_ID = trea.IN_DEPARTMENT_ID ?? 0;
                    rdo.FX_IN_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == trea.IN_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.FX_IN_ROOM_ID = trea.IN_ROOM_ID ?? 0;
                    rdo.FX_IN_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == trea.IN_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                }
            }
            if (trea.TDL_TREATMENT_TYPE_ID != null)
            {
                rdo.TREATMENT_TYPE_ID = trea.TDL_TREATMENT_TYPE_ID ?? 0;
                var treatmentType = listTreatmentType.Where(x => x.ID == trea.TDL_TREATMENT_TYPE_ID).First();
                if (treatmentType != null)
                {
                    rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                    rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                }
            }

            rdo.TOTAL_HEIN_PRICE = trea.TOTAL_HEIN_PRICE;
            rdo.TOTAL_PRICE = trea.TOTAL_PRICE;
        }

        private void InForDepartmentTran(Mrs00038RDO rdo, PATIENT_TREATMENT_EXAMREQ trea, List<HIS_DEPARTMENT_TRAN> listDepartmentTran)
        {
            var departmentTran = listDepartmentTran.Where(o => o.TREATMENT_ID == trea.TREATMENT_ID).Where(p => p.DEPARTMENT_IN_TIME.HasValue).OrderBy(p => p.DEPARTMENT_IN_TIME.Value).ThenBy(q => q.ID).ToList();
            if (departmentTran != null)
            {
                foreach (var tran in departmentTran)
                {
                    rdo.DEPARTMENT_IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(tran.DEPARTMENT_IN_TIME ?? 0);
                    rdo.DEPARTMENT_TRAN_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == tran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                    rdo.DEPARTMENT_TRAN_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == tran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                }
            }
        }
        private void InfoPta(Mrs00038RDO rdo, HIS_PATIENT_TYPE_ALTER patientAlter, PATIENT_TREATMENT_EXAMREQ trea)
        {
            if (patientAlter != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == patientAlter.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                if (patientType != null)
                {
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;

                }
                if (patientAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                    rdo.HEIN_CARD_NUMBER = trea.TDL_HEIN_CARD_NUMBER;

                }
                rdo.RIGHT_ROUTE_CODE = patientAlter.RIGHT_ROUTE_CODE;
                rdo.RIGHT_ROUTE_TYPE_CODE = patientAlter.RIGHT_ROUTE_TYPE_CODE;

            }
        }
        private void ProcessAllTreatment(CommonParam paramGet, List<PATIENT_TREATMENT_EXAMREQ> TreatPatientExam, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<HIS_DEPARTMENT_TRAN> listDepartmentTran, List<HIS_TREATMENT_BED_ROOM> treatmentBedRooms)
        {
            try
            {
                var Treatment = TreatPatientExam.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).ToList();
                Inventec.Common.Logging.LogSystem.Info("Treatment" + Treatment.Count);

                if (Treatment != null)
                {
                    foreach (var trea in Treatment)
                    {
                        if (!checkTreatmentType(trea, dicPatientTypeAlter))
                            continue;
                        if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 4 || castFilter.INPUT_DATA_ID_EXAM_TYPE == 0)
                        {
                            Mrs00038RDO rdo = new Mrs00038RDO();
                            InforTreatment(rdo, trea, dicPatientTypeAlter, listDepartmentTran, treatmentBedRooms);
                            if (trea != null)
                            {
                                InforServiceReq(rdo, trea);
                            }
                            var patientAlter = dicPatientTypeAlter.ContainsKey(trea.TREATMENT_ID??0) ? dicPatientTypeAlter[trea.TREATMENT_ID??0]: new HIS_PATIENT_TYPE_ALTER();
                            if (patientAlter != null)
                            {
                                InfoPta(rdo, patientAlter, trea);
                            }
                            listRdo.Add(rdo);
                        }

                    }
                    Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListSereServBill(CommonParam paramGet, List<PATIENT_TREATMENT_EXAMREQ> ListTreaPatientExamReq, List<HIS_SERE_SERV_BILL> Bill, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<HIS_DEPARTMENT_TRAN> listDepartmentTran, List<HIS_TREATMENT_BED_ROOM> treatmentBedRooms)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Bill" + Bill.Count);

                if (Bill != null)
                {
                    foreach (var bill in Bill)
                    {
                        if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 2)
                        {
                            var serviceReq = ListTreaPatientExamReq.FirstOrDefault(p => p.ID == bill.TDL_SERVICE_REQ_ID);
                            var patientAlter = dicPatientTypeAlter.ContainsKey(bill.TDL_TREATMENT_ID) ? dicPatientTypeAlter[bill.TDL_TREATMENT_ID]:new HIS_PATIENT_TYPE_ALTER();
                            Mrs00038RDO rdo = new Mrs00038RDO();

                            if (serviceReq != null)
                            {
                                InforServiceReq(rdo, serviceReq);
                            }

                            if (serviceReq != null)
                            {
                                if (!checkTreatmentType(serviceReq, dicPatientTypeAlter))
                                    continue;
                                InforTreatment(rdo, serviceReq, dicPatientTypeAlter, listDepartmentTran, treatmentBedRooms);
                                if (patientAlter != null)
                                {
                                    InfoPta(rdo, patientAlter, serviceReq);
                                }
                            }


                            listRdo.Add(rdo);
                        }




                    }
                    Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListServiceReq(CommonParam paramGet, List<PATIENT_TREATMENT_EXAMREQ> TreaPatientExamReq, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<HIS_DEPARTMENT_TRAN> listDepartmentTran, List<HIS_TREATMENT_BED_ROOM> treatmentBedRooms)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("ServiceReq" + TreaPatientExamReq.Count);

                if (TreaPatientExamReq != null)
                {
                    foreach (var serviceReq in TreaPatientExamReq)
                    {
                        //if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 1) chuyển sang xử lý ở bên sql
                        //{
                        Mrs00038RDO rdo = new Mrs00038RDO();
                        if (serviceReq != null)
                        {
                            InforServiceReq(rdo, serviceReq);
                        }


                        if (serviceReq != null)
                        {
                            if (!checkTreatmentType(serviceReq, dicPatientTypeAlter))
                                continue;
                            InforTreatment(rdo, serviceReq, dicPatientTypeAlter, listDepartmentTran, treatmentBedRooms);
                            var patientAlter = dicPatientTypeAlter.ContainsKey(serviceReq.TREATMENT_ID??0) ? dicPatientTypeAlter[serviceReq.TREATMENT_ID??0] : new HIS_PATIENT_TYPE_ALTER();
                            if (patientAlter != null)
                            {
                                InfoPta(rdo, patientAlter, serviceReq);
                            }

                        }


                        listRdo.Add(rdo);
                        //}
                        //else if (castFilter.INPUT_DATA_ID_EXAM_TYPE == 3)
                        //{
                        //    var treatmentCode = new List<HIS_TREATMENT>().Where(p => p.TDL_PATIENT_CODE == serviceReq.TDL_PATIENT_CODE).OrderBy(o => o.ID).Select(q => q.ID);
                        //    if (treatmentCode.Count() == 1)
                        //    {
                        //        var trea = TreaPatientExamReq.FirstOrDefault(p => p.ID == serviceReq.TREATMENT_ID);
                        //        var patientAlter = dicPatientTypeAlter.FirstOrDefault(p => p.TREATMENT_ID == serviceReq.TREATMENT_ID);
                        //        Mrs00038RDO rdo = new Mrs00038RDO();

                        //        if (serviceReq != null)
                        //        {
                        //            InforServiceReq(rdo, serviceReq);
                        //        }

                        //        if (trea != null)
                        //        {
                        //            InforTreatment(rdo, trea, dicPatientTypeAlter,listDepartmentTran);
                        //            if (patientAlter != null)
                        //            {
                        //                InfoPta(rdo, patientAlter, trea);
                        //            }

                        //        }
                        //        listRdo.Add(rdo);
                        //    }
                        //}


                    }
                    Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessCurrentListTreatment(CommonParam paramGet, List<PATIENT_TREATMENT_EXAMREQ> TreatPatientExams, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<HIS_DEPARTMENT_TRAN> listDepartmentTran, List<HIS_TREATMENT_BED_ROOM> treatmentBedRooms)
        {
            try
            {
                var Treatments = TreatPatientExams.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).ToList();
                Inventec.Common.Logging.LogSystem.Info("Treatments" + Treatments.Count);

                if (Treatments != null)
                {
                    foreach (var trea in Treatments)
                    {
                        if (checkTreatmentType(trea, dicPatientTypeAlter))
                        {

                            Mrs00038RDO rdo = new Mrs00038RDO();

                            InforTreatment(rdo, trea, dicPatientTypeAlter, listDepartmentTran, treatmentBedRooms);


                            listRdo.Add(rdo);

                            InForDepartmentTran(rdo, trea, listDepartmentTran);

                            listRdo1.Add(rdo);


                        }
                    }
                    Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool checkTreatmentType(PATIENT_TREATMENT_EXAMREQ treatment, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (castFilter.TREATMENT_TYPE_IDs == null) return true;
                if (!dicPatientTypeAlter.ContainsKey(treatment.TREATMENT_ID??0)) return false;
                result = castFilter.TREATMENT_TYPE_IDs.Contains(dicPatientTypeAlter[treatment.TREATMENT_ID??0].TREATMENT_TYPE_ID);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void CalcuatorAge(Mrs00038RDO rdo, PATIENT_TREATMENT_EXAMREQ treatment)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB ?? 0);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = treatment.TDL_PATIENT_DOB > 1000 ? treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = treatment.TDL_PATIENT_DOB > 1000 ? treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                    }
                }
                if (tuoi == 0)
                {
                    rdo.IS_DUOI_12THANG = "X";
                }
                else if (tuoi >= 1 && tuoi <= 15)
                {
                    rdo.IS_1DEN15TUOI = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //private void capcuu(Mrs00038RDO rdo, PATIENT_TREATMENT_EXAMREQ treatment)
        //{
        //    try
        //    {
        //        int? capcuu = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(treatment.IS_EMERGENCY ?? 0);
               
        //        if (capcuu == 1)
        //        {
        //            rdo.IS_CAPCUU = "X";
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //    }
        //}

        private void IsBhyt(Mrs00038RDO rdo, PATIENT_TREATMENT_EXAMREQ treatment, Dictionary<long,HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter)
        {
            try
            {
                if (dicPatientTypeAlter.ContainsKey(treatment.TREATMENT_ID??0))
                {
                    if (dicPatientTypeAlter[treatment.TREATMENT_ID??0] != null)
                    {
                        rdo.RIGHT_ROUTE_TYPE_CODE = dicPatientTypeAlter[treatment.TREATMENT_ID??0].RIGHT_ROUTE_TYPE_CODE;
                        rdo.RIGHT_ROUTE_CODE = dicPatientTypeAlter[treatment.TREATMENT_ID??0].RIGHT_ROUTE_CODE;
                        if (dicPatientTypeAlter[treatment.TREATMENT_ID??0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.IS_BHYT = "X";
                            rdo.HEIN_CARD_NUMBER = dicPatientTypeAlter[treatment.TREATMENT_ID??0].HEIN_CARD_NUMBER;
                            rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
                            rdo.ICD_CODE_TUYENDUOI = treatment.TRANSFER_IN_ICD_CODE;
                            rdo.GIOITHIEU = treatment.TRANSFER_IN_MEDI_ORG_NAME;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

       

        private bool checkFirstTreatment(HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool checkTreatmentInDepartment(CommonParam paramGet, HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                //if (castFilter.DEPARTMENT_ID.HasValue)
                //{
                //    var departmentTranLast = listHisDepartmentTran.Where(o => o.TREATMENT_ID == treatment.ID).OrderBy(p => p.DEPARTMENT_IN_TIME).ThenBy(q => q.ID).LastOrDefault();
                //    var departmentTranFirst = listHisDepartmentTran.Where(o => o.TREATMENT_ID == treatment.ID).OrderBy(p => p.DEPARTMENT_IN_TIME).ThenBy(q => q.ID).FirstOrDefault();
                //    if (departmentTranLast != null && departmentTranLast.DEPARTMENT_ID == castFilter.DEPARTMENT_ID && castFilter.INPUT_DATA_ID_TIME_TYPE != 1)
                //    {
                //        result = true;
                //    }
                //    if (departmentTranFirst != null && departmentTranFirst.DEPARTMENT_ID == castFilter.DEPARTMENT_ID && castFilter.INPUT_DATA_ID_TIME_TYPE == 1)
                //    {
                //        result = true;
                //    }
                //}
                //else
                //{
                //    result = true;
                //}
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


            var benhNhanNV = listTreaPatientExamReq.Where(x => x.IN_TIME >= castFilter.TIME_FROM && x.IN_TIME <= castFilter.TIME_TO && x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).Count();
            var listTreatIn = listTreaPatientExamReq.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).Count();
            dicSingleTag.Add("TREATMENT_IN", listTreatIn);
            var toltalBN = listRdo.Select(x => x.TREATMENT_CODE).Distinct().Count();
            dicSingleTag.Add("KHAM", totalPatientKH);
            dicSingleTag.Add("TOTAL_PATIENT_AMOUNT", toltalBN);
            dicSingleTag.Add("TOTAL_PATIENT", listTreaPatientExamReq.Count);
            dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            dicSingleTag.Add("EXAM_TYPE", castFilter.INPUT_DATA_ID_EXAM_TYPE);
            dicSingleTag.Add("ROUTE_TYPE", castFilter.INPUT_DATA_ID_ROUTE_TYPE);
            dicSingleTag.Add("TIME_TYPE", castFilter.INPUT_DATA_ID_TIME_TYPE);
            dicSingleTag.Add("CREATOR", castFilter.CREATOR_LOGINNAME);


            if (castFilter.PATIENT_CODE != null)
            {
                dicSingleTag.Add("PATIENT_CODE", castFilter.PATIENT_CODE);
            }
            if (castFilter.PATIENT_NAME != null)
                dicSingleTag.Add("PATIENT_NAME", castFilter.PATIENT_NAME);
            if (castFilter.NATIONAL != null)
                dicSingleTag.Add("NATIONAL", castFilter.NATIONAL);
            if (castFilter.ETHNIC_NAME != null)
                dicSingleTag.Add("ETHNIC_NAME", castFilter.ETHNIC_NAME);
            if (castFilter.REQUEST_USERNAME != null)
                dicSingleTag.Add("REQUEST_USERNAME", castFilter.REQUEST_USERNAME);
            if (castFilter.EXAM_ROOM_NAME != null)
                dicSingleTag.Add("EXAM_ROOM_NAME", castFilter.EXAM_ROOM_NAME);
            if (castFilter.HEIN_CARD_NUMBER != null)
                dicSingleTag.Add("HEIN_CARD_NUMBER", castFilter.HEIN_CARD_NUMBER);


            dicSingleTag.Add("AGE_FROM", castFilter.AGE_FROM);
            dicSingleTag.Add("AGE_TO", castFilter.AGE_TO);
            if (castFilter.PATIENT_TYPE_IDs != null)
                dicSingleTag.Add("PATIENT_TYPE_IDs", string.Join(",", castFilter.PATIENT_TYPE_IDs));
            if (castFilter.PROVINCE_IDs != null)
                dicSingleTag.Add("PROVINCE_IDs", string.Join(",", castFilter.PROVINCE_IDs));
            if (castFilter.COMMUNE_IDs != null)
                dicSingleTag.Add("COMMUNE_IDs", string.Join(",", castFilter.COMMUNE_IDs));
            if (castFilter.DISTRICT_IDs != null)
                dicSingleTag.Add("DISTRICT_IDs", string.Join(",", castFilter.DISTRICT_IDs));
            if (castFilter.EXAM_ROOM_IDs != null)
                dicSingleTag.Add("EXAM_ROOM_IDs", string.Join(",", castFilter.EXAM_ROOM_IDs));
            if (castFilter.ICD_IDs != null)
                dicSingleTag.Add("ICD_IDs", string.Join(",", castFilter.ICD_IDs));
            if (castFilter.PATIENT_CAREER_IDs != null)
                dicSingleTag.Add("PATIENT_CAREER_IDs", string.Join(",", castFilter.PATIENT_CAREER_IDs));
            if (castFilter.PATIENT_GENDER_IDs != null)
                dicSingleTag.Add("PATIENT_GENDER_IDs", string.Join(",", castFilter.PATIENT_GENDER_IDs));
            if (castFilter.REQUEST_LOGINNAMEs != null)
                dicSingleTag.Add("REQUEST_LOGINNAMEs", string.Join(",", castFilter.REQUEST_LOGINNAMEs));
            if (castFilter.TREATMENT_END_TYPE_IDs != null)
                dicSingleTag.Add("TREATMENT_END_TYPE_IDs", string.Join(",", castFilter.TREATMENT_END_TYPE_IDs));
            if (castFilter.TREATMENT_TYPE_IDs != null)
                dicSingleTag.Add("TREATMENT_TYPE_IDs", string.Join(",", castFilter.TREATMENT_TYPE_IDs));

            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(x => x.TREATMENT_CODE).ThenBy(o => o.PATIENT_CODE).ToList());
            objectTag.AddObjectData(store, "ReportFull", listRdo1.OrderBy(x => x.TREATMENT_CODE).ThenBy(o => o.PATIENT_CODE).ToList());
        }
    }
}
