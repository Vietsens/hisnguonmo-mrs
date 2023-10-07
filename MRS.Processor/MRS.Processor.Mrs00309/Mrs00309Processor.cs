using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
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
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00309
{
    class Mrs00309Processor : AbstractProcessor
    {
        List<Mrs00309RDO> ListRdoEX_HEIN = new List<Mrs00309RDO>();
        List<Mrs00309RDO> ListRdoOUT_FEE = new List<Mrs00309RDO>();
        List<Mrs00309RDO> ListRdoOUT_HEIN = new List<Mrs00309RDO>();
        List<Mrs00309RDO> ListRdoIN_HEIN = new List<Mrs00309RDO>();
        List<Mrs00309RDO> ListRdoIN_FEE = new List<Mrs00309RDO>();
        List<string> listExamRoomCode = new List<string>();
        CommonParam paramGet = new CommonParam();
        List<HIS_EXECUTE_ROOM> listExcuteRoom = new List<HIS_EXECUTE_ROOM>();
        List<string> listClinicalDepartmentCode = new List<string>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_TREATMENT_4> listTreatment = new List<V_HIS_TREATMENT_4>();


        Decimal COUNT_TREATMENTKSK = 0;
        Decimal TOTAL_PRICEKSK = 0;
        Decimal HEIN_TOTAL_PRICEKSK = 0;
        Decimal MEDICINE_PRICEKSK = 0;
        Decimal HEIN_PATIENT_TOTAL_PRICEKSK = 0;
        Decimal TOTAL_PRICE_FEEKSK = 0;


        Decimal COUNT_TREATMENTEXFEE = 0;
        Decimal TOTAL_PRICEEXFEE = 0;
        Decimal HEIN_TOTAL_PRICEEXFEE = 0;
        Decimal MEDICINE_PRICEEXFEE = 0;
        Decimal HEIN_PATIENT_TOTAL_PRICEEXFEE = 0;
        Decimal TOTAL_PRICE_FEEEXFEE = 0;


        private List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();

        public Mrs00309Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00309Filter);
        }


        protected override bool GetData()///
        {
            var filter = ((Mrs00309Filter)reportFilter);
            bool result = true;
            try
            {
                //HSDT ra vien
                HisTreatmentView4FilterQuery TreatFilter = new HisTreatmentView4FilterQuery();
                TreatFilter.OUT_TIME_FROM = filter.TIME_FROM;
                TreatFilter.OUT_TIME_TO = filter.TIME_TO;
                if (filter.END_DEPARTMENT_ID != null)
                {
                    TreatFilter.END_ROOM_IDs = (HisRoomCFG.HisRooms.Where(o => o.DEPARTMENT_ID == filter.END_DEPARTMENT_ID) ?? new List<V_HIS_ROOM>()).Select(o => o.ID).ToList();
                }
                listTreatment = new HisTreatmentManager(paramGet).GetView4(TreatFilter);
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    listTreatment = listTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID == filter.TREATMENT_TYPE_ID).ToList();
                }
                var listTreatmentId = listTreatment.Select(o => o.ID).ToList();

                //YC-DV
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var ssfilter = new HisSereServViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var ListSereServSub = new HisSereServManager(paramGet).GetView(ssfilter);
                        ListSereServ.AddRange(ListSereServSub);
                    }
                }

                //Chuyen doi tuong dieu tri
                LisPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(listTreatmentId);
                //Danh sách phòng xử lý
                listExcuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());
                //Danh sách khoa xử lý
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
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
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    //--Kham suc khoe
                    long roomIdKsk = HisRoomCFG.ROOM_ID__KSK;
                    List<string> serviceCodeKsks = HisServiceCFG.getList_SERVICE_CODE__KSK;
                    long patientTypeKSK = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;
                    var ID_KSK = ListSereServ.Where(o => serviceCodeKsks.Contains(o.TDL_SERVICE_CODE) || o.TDL_EXECUTE_ROOM_ID == roomIdKsk || o.PATIENT_TYPE_ID == patientTypeKSK).ToList();
                    //tong tien
                    TOTAL_PRICEKSK = ID_KSK.Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                    //Bao hiem chi tra
                    HEIN_TOTAL_PRICEKSK = 0; //
                    //benh nhan chi tra
                    HEIN_PATIENT_TOTAL_PRICEKSK = ID_KSK.Sum(p => p.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0); //
                    //tien thuoc
                    MEDICINE_PRICEKSK = ID_KSK.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                    //tien thanh toan tu tuc
                    TOTAL_PRICE_FEEKSK = ID_KSK.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                    //so benh nhan
                    COUNT_TREATMENTKSK = ID_KSK.GroupBy(q => q.TDL_TREATMENT_ID).Select(p => p.First()).ToList().Count;

                    //--kham vien phi                               
                    var ID_EXFEE = ListSereServ.Where(o => patientTypeId(o, LisPatientTypeAlter) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatmentTypeId(o, LisPatientTypeAlter) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && !(serviceCodeKsks.Contains(o.TDL_SERVICE_CODE) || o.TDL_EXECUTE_ROOM_ID == roomIdKsk || o.PATIENT_TYPE_ID == patientTypeKSK)).ToList();
                    //tong tien
                    TOTAL_PRICEEXFEE = ID_EXFEE.Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                    //bao hiem chi tra
                    HEIN_TOTAL_PRICEEXFEE = ID_EXFEE.Sum(p => p.VIR_TOTAL_HEIN_PRICE ?? 0); //
                    //benh nhan chi tra
                    HEIN_PATIENT_TOTAL_PRICEEXFEE = ID_EXFEE.Sum(p => p.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    //tien thuoc
                    MEDICINE_PRICEEXFEE = ID_EXFEE.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                    //thanh toan tu tuc
                    TOTAL_PRICE_FEEEXFEE = ID_EXFEE.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                    //so benh nhan
                    COUNT_TREATMENTEXFEE = ID_EXFEE.GroupBy(q => q.TDL_TREATMENT_ID).Select(p => p.First()).ToList().Count;

                    listExamRoomCode = listExcuteRoom.Where(o => o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.EXECUTE_ROOM_CODE).ToList();


                    //--Kham BHYT
                    //Danh sach kham bao hiem
                    var ListDataEX_HEIN = ListSereServ.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatmentTypeId(o, LisPatientTypeAlter) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                    ListRdoEX_HEIN = AddToRdo(ListDataEX_HEIN, true);

                    //nam vien BH ngoai tru
                    ListRdoOUT_HEIN.Clear();
                    listClinicalDepartmentCode = listDepartment.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.DEPARTMENT_CODE).ToList();
                    var ListDataOUT_HEIN = ListSereServ.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatmentTypeId(o, LisPatientTypeAlter) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();

                    ListRdoOUT_HEIN = AddToRdo(ListDataOUT_HEIN, false);
                    //nam vien VP ngoai tru
                    var ListDataOUT_FEE = ListSereServ.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE && treatmentTypeId(o, LisPatientTypeAlter) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    ListRdoOUT_FEE = AddToRdo(ListDataOUT_FEE, false);

                    //nam vien BH noi tru
                    var ListDataIN_HEIN = ListSereServ.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatmentTypeId(o, LisPatientTypeAlter) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    ListRdoIN_HEIN = AddToRdo(ListDataIN_HEIN, false);

                    //nam vien VP noi tru
                    var ListDataIN_FEE = ListSereServ.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE && treatmentTypeId(o, LisPatientTypeAlter) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    ListRdoIN_FEE = AddToRdo(ListDataIN_FEE, false);
                }

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private List<Mrs00309RDO> AddToRdo(List<V_HIS_SERE_SERV> ListSereServSub, bool IsListRoomElseListDepartment)
        {
            List<Mrs00309RDO> Result = new List<Mrs00309RDO>();
            try
            {
                Dictionary<V_HIS_ROOM, List<V_HIS_SERE_SERV>> dicSere = new Dictionary<V_HIS_ROOM, List<V_HIS_SERE_SERV>>();
                if (IsNotNullOrEmpty(ListSereServSub))
                {
                    foreach (var sereServ in ListSereServSub)
                    {
                        V_HIS_ROOM KeyRoom = new V_HIS_ROOM();
                        V_HIS_ROOM KeyDepartment = new V_HIS_ROOM();
                        if (IsListRoomElseListDepartment)
                        {
                            if (listExamRoomCode.Contains(sereServ.REQUEST_ROOM_CODE))
                            {
                                KeyRoom.ROOM_CODE = sereServ.REQUEST_ROOM_CODE;
                                KeyRoom.ROOM_NAME = sereServ.REQUEST_ROOM_NAME;
                            }
                            else if (listExamRoomCode.Contains(sereServ.EXECUTE_ROOM_CODE))
                            {
                                KeyRoom.ROOM_CODE = sereServ.EXECUTE_ROOM_CODE;
                                KeyRoom.ROOM_NAME = sereServ.EXECUTE_ROOM_NAME;
                            }
                            if (KeyRoom.ROOM_CODE != null)
                            {
                                if (!dicSere.Keys.Select(o => o.ROOM_CODE).Contains(KeyRoom.ROOM_CODE))
                                    dicSere[KeyRoom] = new List<V_HIS_SERE_SERV>();
                                dicSere[dicSere.Keys.Where(o => o.ROOM_CODE == KeyRoom.ROOM_CODE).First()].Add(sereServ);
                            }

                        }
                        else
                        {
                            if (listClinicalDepartmentCode.Contains(sereServ.REQUEST_DEPARTMENT_CODE))
                            {
                                KeyDepartment.DEPARTMENT_CODE = sereServ.REQUEST_DEPARTMENT_CODE;
                                KeyDepartment.DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME;
                            }
                            else if (listClinicalDepartmentCode.Contains(sereServ.EXECUTE_DEPARTMENT_CODE))
                            {
                                KeyDepartment.DEPARTMENT_CODE = sereServ.EXECUTE_DEPARTMENT_CODE;
                                KeyDepartment.DEPARTMENT_NAME = sereServ.EXECUTE_DEPARTMENT_NAME;
                            }
                            if (KeyDepartment.DEPARTMENT_CODE != null)
                            {
                                if (!dicSere.Keys.Select(o => o.DEPARTMENT_CODE).Contains(KeyDepartment.DEPARTMENT_CODE))
                                    dicSere[KeyDepartment] = new List<V_HIS_SERE_SERV>();
                                dicSere[dicSere.Keys.Where(o => o.DEPARTMENT_CODE == KeyDepartment.DEPARTMENT_CODE).First()].Add(sereServ);
                            }
                        }

                    }

                    foreach (var room in dicSere.Keys)
                    {

                        Mrs00309RDO rdo = new Mrs00309RDO();

                        rdo.EXECUTE_ROOM_NAME = room.ROOM_NAME;
                        rdo.EXECUTE_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                        rdo.TOTAL_PRICE = dicSere[room].Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                        rdo.MEDICINE_PRICE = dicSere[room].Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                        rdo.HEIN_PATIENT_TOTAL_PRICE = dicSere[room].Sum(p => p.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0); //
                        rdo.TOTAL_PRICE_FEE = dicSere[room].Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(p => p.VIR_TOTAL_PRICE ?? 0); //
                        rdo.COUNT_TREATMENT = dicSere[room].GroupBy(q => q.TDL_TREATMENT_ID).Select(p => p.First()).ToList().Count;
                        Result.Add(rdo);

                    }


                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return Result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (HisPatientTypeCFG.PATIENT_TYPE_ID__KSK > 0)
            {
                var patientTypeKsk = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__KSK);
                if (patientTypeKsk != null)
                {
                    dicSingleTag.Add("PATIENT_TYPE_CODE_KSK", patientTypeKsk.PATIENT_TYPE_CODE);
                }
            }
            if (HisRoomCFG.ROOM_ID__KSK > 0)
            {
                var roomKsk = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == HisRoomCFG.ROOM_ID__KSK);
                if (roomKsk != null)
                {
                    dicSingleTag.Add("ROOM_CODE_KSK", roomKsk.ROOM_CODE);
                }
            }
            if (HisServiceCFG.getList_SERVICE_CODE__KSK != null && HisServiceCFG.getList_SERVICE_CODE__KSK.Count>0)
            {
                dicSingleTag.Add("SERVICE_CODE_KSK", string.Join(",", HisServiceCFG.getList_SERVICE_CODE__KSK));
            }
            if (((Mrs00309Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00309Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00309Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00309Filter)reportFilter).TIME_TO));
            }
            dicSingleTag.Add("COUNT_TREATMENTKSK", COUNT_TREATMENTKSK);
            dicSingleTag.Add("TOTAL_PRICEKSK", TOTAL_PRICEKSK);
            dicSingleTag.Add("HEIN_TOTAL_PRICEKSK", HEIN_TOTAL_PRICEKSK);
            dicSingleTag.Add("MEDICINE_PRICEKSK", MEDICINE_PRICEKSK);
            dicSingleTag.Add("HEIN_PATIENT_TOTAL_PRICEKSK", HEIN_PATIENT_TOTAL_PRICEKSK);
            dicSingleTag.Add("TOTAL_PRICE_FEEKSK", TOTAL_PRICE_FEEKSK);
            dicSingleTag.Add("COUNT_TREATMENTEXFEE", COUNT_TREATMENTEXFEE);
            dicSingleTag.Add("TOTAL_PRICEEXFEE", TOTAL_PRICEEXFEE);
            dicSingleTag.Add("HEIN_TOTAL_PRICEEXFEE", HEIN_TOTAL_PRICEEXFEE);
            dicSingleTag.Add("MEDICINE_PRICEEXFEE", MEDICINE_PRICEEXFEE);
            dicSingleTag.Add("HEIN_PATIENT_TOTAL_PRICEEXFEE", HEIN_PATIENT_TOTAL_PRICEEXFEE);
            dicSingleTag.Add("TOTAL_PRICE_FEEEXFEE", TOTAL_PRICE_FEEEXFEE);


            objectTag.AddObjectData(store, "ReportEX_HEIN", ListRdoEX_HEIN);
            objectTag.AddObjectData(store, "ReportOUT_FEE", ListRdoOUT_FEE);
            objectTag.AddObjectData(store, "ReportOUT_HEIN", ListRdoOUT_HEIN);
            objectTag.AddObjectData(store, "ReportIN_HEIN", ListRdoIN_HEIN);
            objectTag.AddObjectData(store, "ReportIN_FEE", ListRdoIN_FEE);
            objectTag.AddObjectData(store, "Treatment", listTreatment);

        }

        private long treatmentTypeId(V_HIS_SERE_SERV thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TDL_TREATMENT_ID && o.LOG_TIME <= thisData.TDL_INTRUCTION_TIME).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID;
            }

            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

                result = 0;
            }
            return result;
        }

        private long patientTypeId(V_HIS_SERE_SERV thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TDL_TREATMENT_ID && o.LOG_TIME <= thisData.TDL_INTRUCTION_TIME).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().PATIENT_TYPE_ID;
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
