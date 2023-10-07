using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisCoTreatment;

namespace MRS.Processor.Mrs00316
{
    class Mrs00316Processor : AbstractProcessor
    {
        List<Mrs00316RDO> ListRdo = new List<Mrs00316RDO>();
        List<Mrs00316RDO> ListRdoIn = new List<Mrs00316RDO>();

        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> ListExamRequest = new List<HIS_SERVICE_REQ>();
        List<HIS_CO_TREATMENT> ListCoTreatment = new List<HIS_CO_TREATMENT>();

        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_EXECUTE_ROOM> listExcuteRoom = new List<HIS_EXECUTE_ROOM>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();

        Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
        Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran = new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>();
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();

        long PatientTypeIdFree = 0;
        List<long> DepartmentExamIds = new List<long>();

        CommonParam paramGet = new CommonParam();

        public Mrs00316Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00316Filter);
        }


        protected override bool GetData()///
        {
            var filter = ((Mrs00316Filter)reportFilter);
            bool result = true;
            try
            {
                PatientTypeIdFree = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
                if (HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM != null)
                {
                    DepartmentExamIds = HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM;
                }

                //HSDT hien dien trong thoi gian lay bao cao
                HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.IS_PAUSE = false;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                var listTreatment = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                if (this.IsNotNullOrEmpty(listTreatment))
                {
                    ListTreatment.AddRange(listTreatment);
                }
                filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                filterTreatment.IS_PAUSE = true;
                filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                var listTreatmentOut = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                if (this.IsNotNullOrEmpty(listTreatmentOut))
                {
                    ListTreatment.AddRange(listTreatmentOut);
                }
                dicTreatment = ListTreatment.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                var listTreatmentId = dicTreatment.Keys.ToList();
                //Chuyen doi tuong
                if (IsNotNullOrEmpty(listTreatmentId))
                {

                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            TREATMENT_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY }
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        if (this.IsNotNullOrEmpty(LisPatientTypeAlterLib))
                        {
                            LisPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                        }
                    }
                    dicPatientTypeAlter = LisPatientTypeAlter.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList());
                }

                //chuyen khoa
                if (IsNotNullOrEmpty(listTreatmentId))
                {

                    var skip = 0;
                    //Danh sách khoa xử lý
                    HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                    departmentFilter.IS_CLINICAL = true;
                    listDepartment = new HisDepartmentManager().Get(departmentFilter);
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var ListDepartmentTranLib = new HisDepartmentTranManager(paramGet).GetView(departmentTranFilter);
                        if (this.IsNotNullOrEmpty(ListDepartmentTranLib))
                        {
                            ListDepartmentTran.AddRange(ListDepartmentTranLib);
                        }
                    }
                    dicDepartmentTran = ListDepartmentTran.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList());
                }

                //phẫu thuật đã kết thúc

                var HisServiceReqfilter = new HisServiceReqFilterQuery()
                {
                    FINISH_TIME_FROM = filter.TIME_FROM,
                    FINISH_TIME_TO = filter.TIME_TO,
                    SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,
                    REQUEST_DEPARTMENT_ID = filter.CLINICAL_DEPARTMENT_ID
                };
                ListServiceReq = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);



                //Danh sach dieu tri ket hop trong khoang thoi gian bao cao
                HisCoTreatmentFilterQuery filterCoTreatment = new HisCoTreatmentFilterQuery();
                filterCoTreatment.HAS_START_TIME = true;
                filterCoTreatment.HAS_FINISH_TIME = false;
                filterCoTreatment.START_TIME_TO = filter.TIME_TO;
                filterCoTreatment.DEPARTMENT_ID = filter.CLINICAL_DEPARTMENT_ID;
                var listCoTreatment = new HisCoTreatmentManager(paramGet).Get(filterCoTreatment);
                if (this.IsNotNullOrEmpty(listCoTreatment))
                {
                    ListCoTreatment.AddRange(listCoTreatment);
                }
                filterCoTreatment = new HisCoTreatmentFilterQuery();
                filterCoTreatment.HAS_START_TIME = true;
                filterCoTreatment.START_TIME_TO = filter.TIME_TO;
                filterCoTreatment.HAS_FINISH_TIME = true;
                filterCoTreatment.FINISH_TIME_FROM = filter.TIME_TO;
                filterCoTreatment.DEPARTMENT_ID = filter.CLINICAL_DEPARTMENT_ID;
                var listCoTreatmentOut = new HisCoTreatmentManager(paramGet).Get(filterCoTreatment);
                if (this.IsNotNullOrEmpty(listCoTreatmentOut))
                {
                    ListCoTreatment.AddRange(listCoTreatmentOut);
                }
                ListCoTreatment = ListCoTreatment.GroupBy(o => o.ID).Select(o => o.First()).ToList();
                var CoTreatmentIds = ListCoTreatment.Select(o => o.TDL_TREATMENT_ID).ToList();
                //Chuyen doi tuong dieu tri ket hop
                if (IsNotNullOrEmpty(CoTreatmentIds))
                {

                    var skip = 0;
                    while (CoTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = CoTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            TREATMENT_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY}
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        if (this.IsNotNullOrEmpty(LisPatientTypeAlterLib))
                        {
                            foreach (var item in LisPatientTypeAlterLib)
                            {
                               
                                if (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                {
                                    dicPatientTypeAlter[item.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                                }
                                if (!dicPatientTypeAlter[item.TREATMENT_ID].Exists(o => o.ID == item.ID))
                                {
                                    dicPatientTypeAlter[item.TREATMENT_ID].Add(item);
                                    LisPatientTypeAlter.Add(item);
                                }
                            }
                        }
                    }
                }

                //Tong so kham benh da chi dinh trong thoi gian bao cao

                var ExamRequestfilter = new HisServiceReqFilterQuery()
                {
                    INTRUCTION_TIME_FROM = filter.TIME_FROM,
                    INTRUCTION_TIME_TO = filter.TIME_TO,
                    SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                    HAS_EXECUTE = true,
                    EXECUTE_DEPARTMENT_ID = filter.CLINICAL_DEPARTMENT_ID
                };
                ListExamRequest = new HisServiceReqManager(paramGet).Get(ExamRequestfilter) ?? new List<HIS_SERVICE_REQ>();

                ListExamRequest = ListExamRequest.GroupBy(o => new { o.EXECUTE_DEPARTMENT_ID, o.TREATMENT_ID }).Select(p => p.First()).ToList();


                //Danh sách phòng xử lý
                listExcuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());
                //Danh sách phòng xử lý
                listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
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
            var result = true;
            try
            {
                var filter = ((Mrs00316Filter)reportFilter);
                List<long> treatmentEndTypeIdsKhoi = HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__KHOI ?? new List<long>();
                List<long> treatmentEndTypeIdsKhac = HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__KHAC ?? new List<long>();
                List<long> treatmentEndTypeIdsXinVe = HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__XV ?? new List<long>();
                long patientTypeBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                long patientTypeFee = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListDepartmentTran))
                {
                    //Danh sách các Vào khoa trươc TIME_FROM:
                    var departmentTranIdOld = ListDepartmentTran.Where(o => o.DEPARTMENT_IN_TIME < filter.TIME_FROM).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các Vào khoa sau TIME_FROM:
                    var departmentTranIdNew = ListDepartmentTran.Where(o => o.DEPARTMENT_IN_TIME >= filter.TIME_FROM).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các Vào khoa từ khoa khác đến:
                    var departmentTranIdFromOtherDepartment = ListDepartmentTran.Where(o => IsOrtherDepartmentIn(o.TREATMENT_ID, o.PREVIOUS_ID)).Select(p => p.ID).ToList() ?? new List<long>();

                    #region Các loại ra viện
                    //Danh sách các HSDT chuyển tuyến:
                    var treatmentTranPati = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME >= filter.TIME_FROM && o.OUT_TIME < filter.TIME_TO).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các HSDT Ra viện Tổng:
                    var treatmentRV_CM = ListTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các HSDT Ra viện:
                    var treatmentKhoi = ListTreatment.Where(o => treatmentEndTypeIdsKhoi.Contains(o.TREATMENT_END_TYPE_ID ?? 0) && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO).Select(p => p.ID).ToList() ?? new List<long>();
                    ////Danh sách các HSDT Ra viện khác:
                    var treatmentRVOrther = ListTreatment.Where(o => treatmentEndTypeIdsKhac.Contains(o.TREATMENT_END_TYPE_ID ?? 0) && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO).Select(p => p.ID).ToList() ?? new List<long>();
                    ////Danh sách các HSDT xin về:
                    var treatmentXV = ListTreatment.Where(o => treatmentEndTypeIdsXinVe.Contains(o.TREATMENT_END_TYPE_ID ?? 0) && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các HSDT kết quả là tử vong:
                    var treatmentDie = ListTreatment.Where(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO).Select(p => p.ID).ToList() ?? new List<long>();
                    #endregion

                    //Danh sách các HSDT có đối tượng là TE:
                    var treatmentTE = ListTreatment.Where(o => Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB))).Select(p => p.ID).ToList() ?? new List<long>();

                    //Danh sách các chuyển khoa có đối tượng là BHYT:
                    var departmentTranBHYT = ListDepartmentTran.Where(o => patientTypeId(o, LisPatientTypeAlter) == patientTypeBhyt).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các chuyển khoa có đối tượng là DV:
                    var departmentTranDV = ListDepartmentTran.Where(o => patientTypeId(o, LisPatientTypeAlter) == patientTypeFee).Select(p => p.ID).ToList() ?? new List<long>();
                    //Danh sách các chuyển khoa có đối tượng là miễn phí:
                    var departmentTranFREE = ListDepartmentTran.Where(o => patientTypeId(o, LisPatientTypeAlter) == PatientTypeIdFree).Select(p => p.ID).ToList() ?? new List<long>();

                    //Lọc theo khoa

                    if (filter.CLINICAL_DEPARTMENT_CODEs != null)
                    {
                        listDepartment = listDepartment.Where(o => filter.CLINICAL_DEPARTMENT_CODEs.Contains(o.DEPARTMENT_CODE)).ToList();
                    }

                    if (filter.CLINICAL_DEPARTMENT_ID != null)
                    {
                        listDepartment = listDepartment.Where(o => o.ID == filter.CLINICAL_DEPARTMENT_ID).ToList();
                    }


                    if (IsNotNullOrEmpty(listDepartment))
                    {
                        listDepartment = listDepartment.OrderBy(o => o.NUM_ORDER ?? 10000).ToList();
                        foreach (var item in listDepartment)
                        {

                            List<V_HIS_DEPARTMENT_TRAN> listSubAll = ListDepartmentTran.Where(o => o.DEPARTMENT_ID == item.ID).ToList();
                            for (int i = 0; i < 2; i++)
                            {
                                Mrs00316RDO rdo = new Mrs00316RDO();
                                rdo.EXECUTE_DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                                rdo.EXECUTE_DEPARTMENT_ID = item.ID;
                                rdo.EXECUTE_DEPARTMENT_CODE = item.DEPARTMENT_CODE;

                                if (listSubAll.Count > 0)
                                {

                                    var listSub = listSubAll.Where(p => IsStayingDepartment(NextDepartment(p), p)).Where(o => HasTreatOut(o, LisPatientTypeAlter) || HasTreatIn(o, LisPatientTypeAlter) || HasTreatLightDay(o, LisPatientTypeAlter)).ToList();
                                    if (i == 1)
                                    {
                                        listSub = listSubAll.Where(p => IsStayingDepartment(NextDepartment(p), p)).Where(o => HasTreatIn(o, LisPatientTypeAlter)).ToList();
                                    }
                                    if (listSub.Count > 0)
                                    {
                                        Inventec.Common.Logging.LogSystem.Info(string.Join(", ", rdo.EXECUTE_DEPARTMENT_NAME));
                                        //Bệnh nhân cũ 
                                        rdo.COUNT_OLD_TREAT = listSub.Count(o => departmentTranIdOld.Contains(o.ID));
                                        if (IsNotNullOrEmpty(departmentTranIdOld))
                                            Inventec.Common.Logging.LogSystem.Info("1: " + string.Join(", ", listSub.Where(o => departmentTranIdOld.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));

                                        //Bệnh nhân mới - NTH
                                        rdo.COUNT_NEW_TREAT = listSub.Count(o => departmentTranIdNew.Contains(o.ID) && !departmentTranIdFromOtherDepartment.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("2: " + string.Join(", ", listSub.Where(o => departmentTranIdNew.Contains(o.ID) && !departmentTranIdFromOtherDepartment.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));

                                        //Bệnh nhân khoa khác đến - NTH
                                        rdo.COUNT_ORTHER_DEPARTMENT_IN = listSub.Count(o => departmentTranIdFromOtherDepartment.Contains(o.ID) && !departmentTranIdOld.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("3: " + string.Join(", ", listSub.Where(o => departmentTranIdFromOtherDepartment.Contains(o.ID) && !departmentTranIdOld.Contains(o.ID)).Select(z => z.TREATMENT_CODE).ToList()));

                                        //Bệnh nhân mới + khoa khác đến - TT
                                        rdo.COUNT_ORTHER_NEW = listSub.Count(o => departmentTranIdNew.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4: " + string.Join(", ", listSub.Where(o => departmentTranIdNew.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        #region BN moi tong hop
                                        //Bệnh nhân mới + khoa khác đến BHYT - DKTP
                                        rdo.COUNT_ORTHER_NEW_BHYT = listSub.Count(o => departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4a: " + string.Join(", ", listSub.Where(o => departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân mới + khoa khác đến VP - DKTP
                                        rdo.COUNT_ORTHER_NEW_VP = listSub.Count(o => departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4b: " + string.Join(", ", listSub.Where(o => departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        #endregion
                                        #region BN moi dieu tri noi tru
                                        //Bệnh nhân mới + khoa khác đến BHYT - DKTP
                                        rdo.COUNT_ORTHER_NEW_BHYT_TREAT_IN = listSub.Count(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4a1: " + string.Join(", ", listSub.Where(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân mới + khoa khác đến VP - DKTP
                                        rdo.COUNT_ORTHER_NEW_VP_TREAT_IN = listSub.Count(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4b1: " + string.Join(", ", listSub.Where(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        #endregion
                                        #region BN moi dieu tri ngoai tru
                                        //Bệnh nhân mới + khoa khác đến BHYT - DKTP
                                        rdo.COUNT_ORTHER_NEW_BHYT_TREAT_OUT = listSub.Count(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4a2: " + string.Join(", ", listSub.Where(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân mới + khoa khác đến VP - DKTP
                                        rdo.COUNT_ORTHER_NEW_VP_TREAT_OUT = listSub.Count(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4b2: " + string.Join(", ", listSub.Where(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        #endregion
                                        #region BN moi dieu tri ban ngay
                                        //Bệnh nhân mới + khoa khác đến BHYT - DKTP
                                        rdo.COUNT_ORTHER_NEW_BHYT_TREAT_LIGHT = listSub.Count(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4a3: " + string.Join(", ", listSub.Where(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranBHYT.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân mới + khoa khác đến VP - DKTP
                                        rdo.COUNT_ORTHER_NEW_VP_TREAT_LIGHT = listSub.Count(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("4b3: " + string.Join(", ", listSub.Where(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranIdNew.Contains(o.ID) && departmentTranDV.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        #endregion
                                        //Bệnh nhân ra viện NTH: ra vien = khoi
                                        rdo.COUNT_OUT = listSub.Count(o => treatmentKhoi.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null);
                                        Inventec.Common.Logging.LogSystem.Info("5: " + string.Join(", ", listSub.Where(o => treatmentKhoi.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân ra viện - DKTP: ra vien =tong-khac-tuvong-chuyentuyen
                                        rdo.COUNT_OUT_NE = listSub.Count(o => treatmentRV_CM.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null && (!treatmentRVOrther.Contains(o.TREATMENT_ID)) && (!treatmentDie.Contains(o.TREATMENT_ID)) && !treatmentTranPati.Contains(o.TREATMENT_ID));
                                        Inventec.Common.Logging.LogSystem.Info("5a: " + string.Join(", ", listSub.Where(o => treatmentRV_CM.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null && (!treatmentRVOrther.Contains(o.TREATMENT_ID)) && (!treatmentDie.Contains(o.TREATMENT_ID)) && !treatmentTranPati.Contains(o.TREATMENT_ID)).ToList().Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân ra viện xin về - TT
                                        rdo.COUNT_OUT_XV = listSub.Count(o => treatmentXV.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null);
                                        Inventec.Common.Logging.LogSystem.Info("6: " + string.Join(", ", listSub.Where(o => treatmentXV.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân ra viện khác - TT
                                        rdo.COUNT_OUT_ORTHER = listSub.Count(o => treatmentRVOrther.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null);
                                        Inventec.Common.Logging.LogSystem.Info("8: " + string.Join(", ", listSub.Where(o => treatmentRVOrther.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null).Select(z => z.TREATMENT_CODE).ToList()));

                                        //Bệnh nhân ra khoa - NTH
                                        var thisMOV = listSub.Where(o => NextDepartment(o).DEPARTMENT_IN_TIME != null && NextDepartment(o).DEPARTMENT_IN_TIME < filter.TIME_TO).Select(z => z.TREATMENT_CODE).ToList();
                                        if (thisMOV.Count > 0)
                                            Inventec.Common.Logging.LogSystem.Info("9: " + string.Join(", ", thisMOV));

                                        rdo.COUNT_MOV = thisMOV.Count;
                                        //Bệnh nhân chuyển tuyến - NTH
                                        rdo.COUNT_TRAN_PATI = listSub.Count(o => treatmentTranPati.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null);
                                        //Bệnh nhân tử vong - NTH
                                        rdo.COUNT_DIE = listSub.Count(o => treatmentDie.Contains(o.TREATMENT_ID) && NextDepartment(o).DEPARTMENT_IN_TIME == null);
                                        //Bệnh nhân trẻ em - NTH
                                        rdo.COUNT_TE = listSub.Count(o => treatmentTE.Contains(o.TREATMENT_ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE)));
                                        #region BN con lai tong hop
                                        //Bệnh nhân BHYT - NTH
                                        rdo.COUNT_BHYT = listSub.Count(o => departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("BHYT - tong con lai: " + String.Join(", ", listSub.Where(o => departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân viện phí - NTH
                                        rdo.COUNT_DV = listSub.Count(o => departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE)));
                                        Inventec.Common.Logging.LogSystem.Info("VP - tong con lai: " + String.Join(", ", listSub.Where(o => departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân còn lại - NTH
                                        rdo.COUNT_CURRENT_TREAT = listSub.Count(o => (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("tong con lai: " + String.Join(", ", listSub.Where(o => (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        #endregion
                                        #region BN con lai dieu tri noi tru
                                        //Bệnh nhân BHYT - NTH
                                        rdo.COUNT_BHYT_TREAT_IN = listSub.Count(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("BHYT - noi tru con lai: " + String.Join(", ", listSub.Where(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân viện phí - NTH
                                        rdo.COUNT_DV_TREAT_IN = listSub.Count(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE)));
                                        Inventec.Common.Logging.LogSystem.Info("VP - noi tru con lai: " + String.Join(", ", listSub.Where(o => HasTreatIn(o, LisPatientTypeAlter) && departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân còn lại - NTH
                                        rdo.COUNT_CURRENT_TREAT_TREAT_IN = listSub.Count(o => HasTreatIn(o, LisPatientTypeAlter) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("noi tru con lai: " + String.Join(", ", listSub.Where(o => HasTreatIn(o, LisPatientTypeAlter) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        #endregion
                                        #region BN con lai dieu tri ngoai tru
                                        //Bệnh nhân BHYT - NTH
                                        rdo.COUNT_BHYT_TREAT_OUT = listSub.Count(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("BHYT - ngoai tru con lai: " + String.Join(", ", listSub.Where(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân viện phí - NTH
                                        rdo.COUNT_DV_TREAT_OUT = listSub.Count(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE)));
                                        Inventec.Common.Logging.LogSystem.Info("VP - ngoai tru con lai: " + String.Join(", ", listSub.Where(o => HasTreatOut(o, LisPatientTypeAlter) && departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân còn lại - NTH
                                        rdo.COUNT_CURRENT_TREAT_TREAT_OUT = listSub.Count(o => HasTreatOut(o, LisPatientTypeAlter) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("ngoai tru con lai: " + String.Join(", ", listSub.Where(o => HasTreatOut(o, LisPatientTypeAlter) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        #endregion
                                        #region BN con lai dieu tri ban ngay
                                        //Bệnh nhân BHYT - NTH
                                        rdo.COUNT_BHYT_TREAT_LIGHT = listSub.Count(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("BHYT - ban ngay con lai: " + String.Join(", ", listSub.Where(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranBHYT.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân viện phí - NTH
                                        rdo.COUNT_DV_TREAT_LIGHT = listSub.Count(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE)));
                                        Inventec.Common.Logging.LogSystem.Info("VP - ban ngay con lai: " + String.Join(", ", listSub.Where(o => HasTreatLightDay(o, LisPatientTypeAlter) && departmentTranDV.Contains(o.ID) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        //Bệnh nhân còn lại - NTH
                                        rdo.COUNT_CURRENT_TREAT_TREAT_LIGHT = listSub.Count(o => HasTreatLightDay(o, LisPatientTypeAlter) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))); //
                                        Inventec.Common.Logging.LogSystem.Info("ban ngay con lai: " + String.Join(", ", listSub.Where(o => HasTreatLightDay(o, LisPatientTypeAlter) && (!treatmentRV_CM.Contains(o.TREATMENT_ID)) && (!thisMOV.Contains(o.TREATMENT_CODE))).Select(z => z.TREATMENT_CODE).ToList()));
                                        #endregion
                                        //Bệnh nhân BHYT tổng - TT
                                        rdo.COUNT_BHYT_SUM = listSub.Count(o => departmentTranBHYT.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("10: " + string.Join(", ", listSub.Where(o => departmentTranBHYT.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân viện phí tổng - TT
                                        rdo.COUNT_FEE_SUM = listSub.Count(o => departmentTranDV.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("11: " + string.Join(", ", listSub.Where(o => departmentTranDV.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân miễn phí tổng - TT
                                        rdo.COUNT_FREE_SUM = listSub.Count(o => departmentTranFREE.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("12: " + string.Join(", ", listSub.Where(o => departmentTranFREE.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));

                                        var listSubNow = listSubAll.Where(o => HasTreatOut(o, LisPatientTypeAlter) || HasTreatIn(o, LisPatientTypeAlter) || HasTreatLightDay(o, LisPatientTypeAlter)).ToList();
                                        if (i == 1)
                                        {
                                            listSubNow = listSubAll.Where(o => HasTreatIn(o, LisPatientTypeAlter)).ToList();
                                        }
                                        //Bệnh nhân BHYT hiện tại - TT
                                        rdo.COUNT_BHYT_NOW = listSubNow.Count(o => IsStayingDepartmentNow(NextDepartment(o), o) && departmentTranBHYT.Contains(o.ID));
                                        Inventec.Common.Logging.LogSystem.Info("13: " + string.Join(", ", listSubNow.Where(o => IsStayingDepartmentNow(NextDepartment(o), o) && departmentTranBHYT.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân viện phí hiện tại - TT
                                        rdo.COUNT_FEE_NOW = IsNotNullOrEmpty(departmentTranDV) ? listSubNow.Count(o => IsStayingDepartmentNow(NextDepartment(o), o) && departmentTranDV.Contains(o.ID)) : 0;
                                        Inventec.Common.Logging.LogSystem.Info("14: " + string.Join(", ", listSubNow.Where(o => IsStayingDepartmentNow(NextDepartment(o), o) && departmentTranDV.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));
                                        //Bệnh nhân miễn phí hiện tại - TT
                                        rdo.COUNT_FREE_NOW = IsNotNullOrEmpty(departmentTranFREE) ? listSubNow.Count(o => IsStayingDepartmentNow(NextDepartment(o), o) && departmentTranFREE.Contains(o.ID)) : 0;
                                        Inventec.Common.Logging.LogSystem.Info("15: " + string.Join(", ", listSubNow.Where(o => IsStayingDepartmentNow(NextDepartment(o), o) && departmentTranFREE.Contains(o.ID)).ToList().Select(o => o.TREATMENT_CODE)));


                                    }

                                    rdo.COUNT_SURG = ListServiceReq.Count(o => o.REQUEST_DEPARTMENT_ID == item.ID);

                                    // so benh nhan chi dinh kham tai khoa
                                    rdo.COUNT_EXAM_REQUEST = this.ListExamRequest.Count(o => o.EXECUTE_DEPARTMENT_ID == item.ID);

                                    // so benh nhan dieu tri ket hop noi tru
                                    rdo.COUNT_CO_TREATMENT_IN = this.ListCoTreatment.Count(o => o.DEPARTMENT_ID == item.ID && this.TreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                                    // so benh nhan dieu tri ket hop ngoai tru
                                    rdo.COUNT_CO_TREATMENT_OUT = this.ListCoTreatment.Count(o => o.DEPARTMENT_ID == item.ID && this.TreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                                    // so benh nhan dieu tri ket hop ban ngay
                                    rdo.COUNT_CO_TREATMENT_LIGHT = this.ListCoTreatment.Count(o => o.DEPARTMENT_ID == item.ID && this.TreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY);

                                    rdo.NUM_ORDER = item.NUM_ORDER ?? 9999;
                                }

                                if (i == 1)
                                {
                                    ListRdoIn.Add(rdo);
                                }
                                else
                                {
                                    ListRdo.Add(rdo);
                                }
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }



        //Co dieu tri noi tru
        private bool HasTreatIn(V_HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                   && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
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

        //Co dieu tri ngoai tru
        private bool HasTreatOut(V_HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                  && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU;
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

        //Co dieu tri ban ngay
        private bool HasTreatLightDay(V_HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                  && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY;
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

        //Diện điều trị của điều trị kết hợp
        private long TreatmentType(HIS_CO_TREATMENT o)
        {
            long result = 0;
            try
            {
                var listInTreat = dicPatientTypeAlter.ContainsKey(o.TDL_TREATMENT_ID) ? dicPatientTypeAlter[o.TDL_TREATMENT_ID].Where(p => o.FINISH_TIME == null || p.LOG_TIME <= o.FINISH_TIME).ToList() : new List<V_HIS_PATIENT_TYPE_ALTER>();
                if (IsNotNullOrEmpty(listInTreat))
                {

                    result = listInTreat.OrderBy(q => q.LOG_TIME).Last().TREATMENT_TYPE_ID;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        //khoa lien ke
        private V_HIS_DEPARTMENT_TRAN NextDepartment(V_HIS_DEPARTMENT_TRAN o)
        {
            V_HIS_DEPARTMENT_TRAN result = new V_HIS_DEPARTMENT_TRAN();
            try
            {
                var ListDepartmentTranSub = dicDepartmentTran.ContainsKey(o.TREATMENT_ID) ? dicDepartmentTran[o.TREATMENT_ID].Where(p => p.PREVIOUS_ID == o.ID).ToList() : new List<V_HIS_DEPARTMENT_TRAN>();
                if (IsNotNullOrEmpty(ListDepartmentTranSub))
                    result = ListDepartmentTranSub.First();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new V_HIS_DEPARTMENT_TRAN();
            }
            return result;
        }

        //o khoa trong thoi gian bao cao?
        private bool IsStayingDepartment(V_HIS_DEPARTMENT_TRAN NextDepartment, V_HIS_DEPARTMENT_TRAN inDepartment)
        {
            bool result = false;
            try
            {
                //Ngày ra, ngày vào
                var filter = ((Mrs00316Filter)reportFilter);
                result = inDepartment.DEPARTMENT_IN_TIME < filter.TIME_TO && (NextDepartment.DEPARTMENT_IN_TIME == null || NextDepartment.DEPARTMENT_IN_TIME >= filter.TIME_FROM) ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        //hien tai o khoa?
        private bool IsStayingDepartmentNow(V_HIS_DEPARTMENT_TRAN NextDepartment, V_HIS_DEPARTMENT_TRAN inDepartment)
        {
            bool result = false;
            try
            {
                //Now
                long timeNow = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(System.DateTime.Now) ?? 0;
                result = inDepartment.DEPARTMENT_IN_TIME <= timeNow && (NextDepartment.DEPARTMENT_IN_TIME == null || NextDepartment.DEPARTMENT_IN_TIME > timeNow) ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        //tu khoa khac den?
        private bool IsOrtherDepartmentIn(long TreatmentId, long? previousId)
        {
            bool result = false;
            try
            {

                result = dicDepartmentTran.ContainsKey(TreatmentId) && dicDepartmentTran[TreatmentId].Where(o => previousId.HasValue && o.ID == previousId.Value && (HasTreatIn(o, LisPatientTypeAlter) || HasTreatOut(o, LisPatientTypeAlter) || HasTreatLightDay(o, LisPatientTypeAlter)) && !DepartmentExamIds.Contains(o.DEPARTMENT_ID)).ToList().Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            if (((Mrs00316Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00316Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00316Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00316Filter)reportFilter).TIME_TO));
            }
            if (((Mrs00316Filter)reportFilter).CLINICAL_DEPARTMENT_CODEs != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", string.Join(" - ", listDepartment.Where(o => ((Mrs00316Filter)reportFilter).CLINICAL_DEPARTMENT_CODEs.Contains(o.DEPARTMENT_CODE)).Select(o => o.DEPARTMENT_NAME).ToList()));
            }
            if (((Mrs00316Filter)reportFilter).CLINICAL_DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", listDepartment.First().DEPARTMENT_NAME);
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportIn", ListRdoIn);

        }
        //doi tuong benh nhan
        private long patientTypeId(V_HIS_DEPARTMENT_TRAN thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = dicPatientTypeAlter.ContainsKey(thisData.TREATMENT_ID) ? dicPatientTypeAlter[thisData.TREATMENT_ID].Where(o => o.LOG_TIME <= thisData.DEPARTMENT_IN_TIME).ToList() : new List<V_HIS_PATIENT_TYPE_ALTER>();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().PATIENT_TYPE_ID;
                else
                {
                    LisPatientTypeAlterSub = dicPatientTypeAlter.ContainsKey(thisData.TREATMENT_ID) ? dicPatientTypeAlter[thisData.TREATMENT_ID].Where(o => o.LOG_TIME >= thisData.DEPARTMENT_IN_TIME).ToList() : new List<V_HIS_PATIENT_TYPE_ALTER>();
                    if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                        if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                            result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().PATIENT_TYPE_ID;
                }
            }

            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

                result = 0;
            }
            return result;
        }
    }

}
