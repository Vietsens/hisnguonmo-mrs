using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MRS.Processor.Mrs00313
{
    class Mrs00313Processor : AbstractProcessor
    {
        Dictionary<long, Mrs00313RDO> dicRdoEX = new Dictionary<long, Mrs00313RDO>();
        Dictionary<long, Mrs00313RDO> dicRdoOUT = new Dictionary<long, Mrs00313RDO>();
        Dictionary<long, Mrs00313RDO> dicRdoOVER = new Dictionary<long, Mrs00313RDO>();
        Dictionary<long, Mrs00313RDO> dicRdoTreat = new Dictionary<long, Mrs00313RDO>();
        Dictionary<long, long> dicHospitalizeDepartmentId = new Dictionary<long, long>();

        CommonParam paramGet = new CommonParam();

        Dictionary<long, List<V_HIS_SERVICE_REQ>> dicMisuServiceReq = new Dictionary<long, List<V_HIS_SERVICE_REQ>>();
        Dictionary<long, List<V_HIS_SERVICE_REQ>> dicExamServiceReq = new Dictionary<long, List<V_HIS_SERVICE_REQ>>();
        Dictionary<long, List<V_HIS_TREATMENT>> dicTreatment = new Dictionary<long, List<V_HIS_TREATMENT>>();
        Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran = new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>();
        List<HIS_EXECUTE_ROOM> listExcuteRoom = new List<HIS_EXECUTE_ROOM>();
        private Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
        private Dictionary<long, List<V_HIS_PATIENT>> dicPatient = new Dictionary<long, List<V_HIS_PATIENT>>();
        Dictionary<long, DepartmentAmount> dicDepartmentAmount = new Dictionary<long, DepartmentAmount>();
        List<long> BHYTs = new List<long>();
        string departmentExamCode = "KKB";
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();

        List<DATE_MONTH_AMOUNT> listDateMonthAmount = new List<DATE_MONTH_AMOUNT>();
        Mrs00313Filter filter;
        public Mrs00313Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00313Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00313Filter)reportFilter);
            bool result = true;
            try
            {
                try
                {
                    var config = Loader.dictionaryConfig["MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__EXAM"];
                    if (config == null) throw new ArgumentNullException("MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__EXAM");
                    string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    if (value != null) departmentExamCode = value;
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }

                //HSDT thoi gian vao
                HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                filterTreatment.IN_TIME_FROM = filter.TIME_FROM;
                var ListTreatment = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                if (filter.BRANCH_ID != null)
                {
                    ListTreatment = ListTreatment.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                }
                dicTreatment = ListTreatment.GroupBy(o => o.IN_DATE).ToDictionary(p => p.Key, q => q.ToList());
                dicHospitalizeDepartmentId = ListTreatment.ToDictionary(p => p.ID, q => q.HOSPITALIZE_DEPARTMENT_ID ?? FirstDepa(q.DEPARTMENT_IDS));
                foreach (var item in dicTreatment)
                {
                    dicPatientTypeAlter.Add(item.Key, new List<V_HIS_PATIENT_TYPE_ALTER>());
                    dicPatient.Add(item.Key, new List<V_HIS_PATIENT>());
                    dicExamServiceReq.Add(item.Key, new List<V_HIS_SERVICE_REQ>());
                    dicMisuServiceReq.Add(item.Key, new List<V_HIS_SERVICE_REQ>());
                    dicDepartmentTran.Add(item.Key, new List<V_HIS_DEPARTMENT_TRAN>());

                    var treatmentSub = item.Value;
                    var listTreatmentId = treatmentSub.Select(o => o.ID).ToList();
                    var listPatientId = treatmentSub.Select(o => o.PATIENT_ID).Distinct().ToList();

                    //chuyen doi tuong
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
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
                            dicPatientTypeAlter[item.Key].AddRange(LisPatientTypeAlterLib);
                        }
                    }

                    //Benh nhan
                    if (IsNotNullOrEmpty(listPatientId))
                    {
                        var skip = 0;
                        while (listPatientId.Count - skip > 0)
                        {
                            var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientViewFilterQuery PatientViewFilter = new HisPatientViewFilterQuery()
                            {
                                IDs = listIDs
                            };
                            var ListPatientSub = new HisPatientManager(paramGet).GetView(PatientViewFilter);
                            dicPatient[item.Key].AddRange(ListPatientSub);
                        }
                    }

                    //yeu cau kham
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var HisServiceReqfilter = new HisServiceReqViewFilterQuery()
                            {
                                TREATMENT_IDs = listIDs,
                                SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                                HAS_EXECUTE = true
                            };

                            var ListExamServiceReqSub = new HisServiceReqManager(paramGet).GetView(HisServiceReqfilter);
                            dicExamServiceReq[item.Key].AddRange(ListExamServiceReqSub);
                        }
                    }
                    //yeu cau thu thuat
                    var HisMisuServiceReqfilter = new HisServiceReqViewFilterQuery()
                    {
                        SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT,
                        INTRUCTION_TIME_FROM = filter.TIME_FROM > item.Key ? filter.TIME_FROM : item.Key,
                        INTRUCTION_TIME_TO = filter.TIME_TO < item.Key + 235959 ? filter.TIME_TO : item.Key + 235959
                    };
                    dicMisuServiceReq[item.Key] = new HisServiceReqManager(paramGet).GetView(HisMisuServiceReqfilter);

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        //chuyen khoa
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery()
                            {
                                TREATMENT_IDs = listIDs
                            };
                            var ListDepartmentTranLib = new HisDepartmentTranManager(paramGet).GetView(departmentTranFilter);
                            dicDepartmentTran[item.Key].AddRange(ListDepartmentTranLib);
                        }
                    }
                }


                //Danh sách phòng xử lý
                listExcuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());

                //Danh sách khoa xử lý
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());

                //Danh sách phòng
                listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());

                //Tạo danh sách thời gian
                CreateDateMonth();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private long FirstDepa(string DepartmentIds)
        {
            long result = 0;
            try
            {
                if (DepartmentIds != null && DepartmentIds.Length > 0)
                {
                    string[] dps = DepartmentIds.Split(',');
                    long.TryParse(dps[0], out result);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return result;
        }

        private void CreateDateMonth()
        {
            DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_FROM);
            DateTime StartMonth = new DateTime(StartTime.Year, StartTime.Month, 1);
            DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_TO);
            DateTime EndMonth = new DateTime(FinishTime.Year, FinishTime.Month, DateTime.DaysInMonth(FinishTime.Year, FinishTime.Month));
            DateTime IndexTime = StartMonth;
            while (IndexTime <= EndMonth)
            {
                DATE_MONTH_AMOUNT dateMonth = new DATE_MONTH_AMOUNT();
                dateMonth.DATE_MONTH_STR = string.Format("Ngày {0}", IndexTime.Day);
                dateMonth.DATE_MONTH_KEY = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(IndexTime) ?? 0;
                dateMonth.DIC_AMOUNT = new Dictionary<string, decimal>();
                dateMonth.DIC_AMOUNT.Add("COUNT_TOTAL", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_IN", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_IN_1", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_TT", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_TE", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_DV", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_BHYT_1", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_TE_1", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_DV_1", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_NGT", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_EXAM_TRAN_PATI", 0);
                dateMonth.DIC_AMOUNT.Add("COUNT_TREAT_TRAN_PATI", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_SUM", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_MALE_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_FEMALE_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_UNDER6_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_UNDER16_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_OVER60_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_VP", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_MALE_VP", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_FEMALE_VP", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_UNDER6_VP", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_UNDER16_VP", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_OVER60_VP", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_IN", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_TRAN", 0);
                dateMonth.DIC_AMOUNT.Add("AMOUNT_BHYT_TRAN", 0);

                //số liệu nhập viện nội trú
                dateMonth.DIC_AMOUNT.Add("TOTAL_IN_BHYT", 0);
                dateMonth.DIC_AMOUNT.Add("TOTAL_IN_VP", 0);

                listDateMonthAmount.Add(dateMonth);
                IndexTime = IndexTime.AddDays(1);

            }
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {

                dicRdoEX.Clear();
                dicRdoOUT.Clear();
                dicRdoOVER.Clear();
                var filter = ((Mrs00313Filter)reportFilter);
                foreach (var treatmentInDate in dicTreatment)
                {
                    var ListTreatment = treatmentInDate.Value;
                    var LisPatientTypeAlter = dicPatientTypeAlter[treatmentInDate.Key];
                    var ListMisuServiceReq = dicMisuServiceReq[treatmentInDate.Key];
                    var ListExamServiceReq = dicExamServiceReq[treatmentInDate.Key];
                    var ListDepartmentTran = dicDepartmentTran[treatmentInDate.Key];
                    #region khai bao kham
                    //BN ngoại trú
                    var ListTreatmentIdNGT = ListTreatment.Where(o => treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)).Select(p => p.ID).ToList() ?? new List<long>();

                    //BN nội trú
                    var ListTreatmentIdNT = ListTreatment.Where(o => treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Select(p => p.ID).ToList() ?? new List<long>();

                    //BN nội trú
                    var ListTreatmentIdBN = ListTreatment.Where(o => treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)).Select(p => p.ID).ToList() ?? new List<long>();
                    //BN tre em
                    var listTreatmentTE = ListTreatment.Where(o => Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB))).Select(p => p.ID).ToList() ?? new List<long>();
                    //Chuyển tuyến - khám
                    var listTreatmentIdTRAN_PATIEX = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.OUT_TIME != null && o.OUT_TIME != 0 && !ListTreatmentIdNGT.Contains(o.ID) && !ListTreatmentIdNT.Contains(o.ID) && !ListTreatmentIdBN.Contains(o.ID)).Select(p => p.ID).ToList() ?? new List<long>();

                    //Chuyển tuyến - điều trị
                    var listTreatmentIdTRAN_PATITR = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.OUT_TIME != null && o.OUT_TIME != 0 && (ListTreatmentIdNGT.Contains(o.ID) || ListTreatmentIdNT.Contains(o.ID) || ListTreatmentIdBN.Contains(o.ID))).Select(p => p.ID).ToList() ?? new List<long>();

                    //Thủ thuật - phòng khám
                    var ListServiceReqTTEX = ListMisuServiceReq.ToList();

                    //Thủ thuật - điều trị
                    var ListServiceReqTTTR = ListMisuServiceReq.Where(o => ListTreatment.Where(n => n.CLINICAL_IN_TIME != null && n.CLINICAL_IN_TIME <= o.INTRUCTION_TIME).Select(m => m.ID).Contains(o.TREATMENT_ID)).ToList();

                    //DV khám BHYT
                    var ListExamServiceReqIdBHYT = ListExamServiceReq.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(p => p.ID).ToList() ?? new List<long>();
                    //DV khám viện phí
                    var ListExamServiceReqIdDV = ListExamServiceReq.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Select(p => p.ID).ToList() ?? new List<long>();

                    //DV khám TE
                    var ListExamServiceReqIdTE = ListExamServiceReq.Where(o => Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB))).Select(p => p.ID).ToList() ?? new List<long>();


                    //BN khám
                    var ListTreatmentIdKH = ListTreatment.Where(o => treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)).Select(p => p.ID).ToList() ?? new List<long>();

                    //BN nhập viện
                    var ListTreatmentIdIN = ListTreatment.Where(o => ListTreatmentIdNGT.Contains(o.ID) || ListTreatmentIdNT.Contains(o.ID) || ListTreatmentIdBN.Contains(o.ID)).Select(p => p.ID).ToList() ?? new List<long>();

                    #endregion

                    #region kham
                    //Phòng khám
                    var activeExamRoom = listExcuteRoom.Where(o => o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Distinct().ToList();

                    var ListData1 = ListExamServiceReq.Where(o => activeExamRoom.Select(p=>p.EXECUTE_ROOM_CODE).Contains(o.EXECUTE_ROOM_CODE)).ToList();
                    var groupbyExecuteRoom = ListData1.GroupBy(o => o.EXECUTE_ROOM_CODE).ToList();
                        var dateMonthAmount = listDateMonthAmount.FirstOrDefault(o => o.DATE_MONTH_KEY == treatmentInDate.Key);

                    if (IsNotNullOrEmpty(groupbyExecuteRoom))
                    {

                        foreach (var group in groupbyExecuteRoom)
                        {

                            List<V_HIS_SERVICE_REQ> listSub = group.ToList<V_HIS_SERVICE_REQ>();
                            var listCountIn = listSub.Where(r => ListTreatmentIdIN.Contains(r.TREATMENT_ID) && getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList();
                            foreach (var item in listCountIn)
                            {
                                long departmentId = DepartmentId(item, LisPatientTypeAlter, ListDepartmentTran);
                                if (dicDepartmentAmount.ContainsKey(departmentId))
                                {
                                    dicDepartmentAmount[departmentId].AMOUNT += 1;
                                    if (ListExamServiceReqIdBHYT.Contains(item.ID))
                                    {
                                        dicDepartmentAmount[departmentId].AMOUNT_VP += 1;
                                    }
                                    else
                                    {
                                        dicDepartmentAmount[departmentId].AMOUNT_BHYT += 1;
                                    }
                                }
                                else
                                {
                                    dicDepartmentAmount[departmentId] = new DepartmentAmount();
                                    dicDepartmentAmount[departmentId].EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                    dicDepartmentAmount[departmentId].AMOUNT = 1;
                                    if (ListExamServiceReqIdBHYT.Contains(item.ID))
                                    {
                                        dicDepartmentAmount[departmentId].AMOUNT_VP = 1;
                                    }
                                    else
                                    {
                                        dicDepartmentAmount[departmentId].AMOUNT_BHYT = 1;
                                    }
                                }
                            }
                            Mrs00313RDO rdo = new Mrs00313RDO();

                            rdo.EXECUTE_ROOM_NAME = listSub.First().EXECUTE_ROOM_NAME;
                            rdo.EXECUTE_ROOM_CODE = listSub.First().EXECUTE_ROOM_CODE;
                            //Inventec.Common.Logging.LogSystem.Info("Phong: " + listSub.First().EXECUTE_ROOM_NAME);

                            //Tong HSDT ket luan kham tai phong (BVNTH)
                            rdo.COUNT_TOTAL = listSub.Where(r => getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Tong kham: " + String.Join(", ", listSub.Where(r => getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));

                            //Tổng nhập viện
                            rdo.COUNT_IN = IsNotNullOrEmpty(ListTreatmentIdIN) ? listCountIn.Count : 0;
                            //Inventec.Common.Logging.LogSystem.Info("Vao vien: " + String.Join(", ", listSub.Where(r => ListTreatmentIdIN.Contains(r.TREATMENT_ID) && getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Select(z => z.TREATMENT_CODE).ToList()));

                            //Tong nam vien noi tru
                            rdo.COUNT_IN_1 = listSub.Where(r => ListTreatmentIdIN.Contains(r.TREATMENT_ID) && (ListTreatmentIdNT.Contains(r.TREATMENT_ID) || ListTreatmentIdBN.Contains(r.TREATMENT_ID)) && getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Vao vien1: " + String.Join(", ", listSub.Where(r => ListTreatmentIdIN.Contains(r.TREATMENT_ID) && (ListTreatmentIdNT.Contains(r.TREATMENT_ID) || ListTreatmentIdBN.Contains(r.TREATMENT_ID)) && getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng thủ thuật
                            rdo.COUNT_TT = IsNotNullOrEmpty(ListServiceReqTTEX) ? ListServiceReqTTEX.Where(o => o.REQUEST_ROOM_ID == listSub.First().EXECUTE_ROOM_ID).ToList().Count : 0;
                            //Inventec.Common.Logging.LogSystem.Info("Thu Thuat: " + String.Join(", ", ListServiceReqTTEX.Where(o => o.REQUEST_ROOM_ID == listSub.First().EXECUTE_ROOM_ID).ToList()));

                            //Tổng phiếu cấp BHYT
                            rdo.COUNT_BHYT = listSub.Where(o => ListExamServiceReqIdBHYT.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Cap phieu BHYT: " + String.Join(", ", listSub.Where(o => ListExamServiceReqIdBHYT.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng phiếu cấp TE
                            rdo.COUNT_TE = listSub.Where(o => ListExamServiceReqIdTE.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count; //Tổng BN vào khám là TE
                            //Inventec.Common.Logging.LogSystem.Info("Cap phieu TE: " + String.Join(", ", listSub.Where(o => ListExamServiceReqIdTE.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng phiếu cấp DV
                            rdo.COUNT_DV = listSub.Where(o => !ListExamServiceReqIdBHYT.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count; //Tổng BN vào khám là VP
                            //Inventec.Common.Logging.LogSystem.Info("Cap phieu VP: " + String.Join(", ", listSub.Where(o => !ListExamServiceReqIdBHYT.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng phiếu cấp BHYT1
                            rdo.COUNT_BHYT_1 = listSub.Where(o => ListExamServiceReqIdBHYT.Contains(o.ID) && (getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID) && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count; //Tổng BN vào khám là BHYT
                            //Inventec.Common.Logging.LogSystem.Info("Cap phieu BHYT1: " + String.Join(", ", listSub.Where(o => ListExamServiceReqIdBHYT.Contains(o.ID) && !ListTreatmentIdIN.Contains(o.TREATMENT_ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Select(z => z.TREATMENT_CODE).ToList()));


                            //Tổng phiếu cấp TE1
                            rdo.COUNT_TE_1 = listSub.Where(o => ListExamServiceReqIdTE.Contains(o.ID) && !ListTreatmentIdIN.Contains(o.TREATMENT_ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Cap phieu TE1: " + String.Join(", ", listSub.Where(o => ListExamServiceReqIdTE.Contains(o.ID) && !ListTreatmentIdIN.Contains(o.TREATMENT_ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng BN vào khám là DV1
                            rdo.COUNT_DV_1 = listSub.Where(o => !ListExamServiceReqIdBHYT.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Cap phieu VP1: " + String.Join(", ", listSub.Where(o => !ListExamServiceReqIdBHYT.Contains(o.ID) && getExamRoomId(o.TREATMENT_ID, ListExamServiceReq) == o.EXECUTE_ROOM_ID && !ListTreatmentIdIN.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng kết thúc ngoại trú
                            rdo.COUNT_NGT = listSub.Where(o => ListTreatmentIdIN.Contains(o.TREATMENT_ID) && ListTreatmentIdNGT.Contains(o.TREATMENT_ID) && o.EXECUTE_ROOM_ID == getExamRoomId(o.TREATMENT_ID, ListExamServiceReq)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("KT Ngoai tru: " + String.Join(", ", listSub.Where(o => ListTreatmentIdIN.Contains(o.TREATMENT_ID) && ListTreatmentIdNGT.Contains(o.TREATMENT_ID) && o.EXECUTE_ROOM_ID == getExamRoomId(o.TREATMENT_ID, ListExamServiceReq)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng khám chuyển tuyến
                            rdo.COUNT_EXAM_TRAN_PATI = listSub.Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Kham Chuyen Tuyen: " + String.Join(", ", listSub.Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng điều trị ngoại trú chuyển tuyến
                            rdo.COUNT_TREAT_TRAN_PATI = listSub.Where(o => ListTreatmentIdIN.Contains(o.TREATMENT_ID) && listTreatmentIdTRAN_PATITR.Contains(o.TREATMENT_ID) && o.EXECUTE_ROOM_ID == getExamRoomId(o.TREATMENT_ID, ListExamServiceReq)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT Chuyen Tuyen: " + String.Join(", ", listSub.Where(o => ListTreatmentIdIN.Contains(o.TREATMENT_ID) && listTreatmentIdTRAN_PATITR.Contains(o.TREATMENT_ID) && o.EXECUTE_ROOM_ID == getExamRoomId(o.TREATMENT_ID, ListExamServiceReq)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));
                            //Tổng khám
                            rdo.AMOUNT_SUM = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Tổng khám BHYT - TT
                            var Bhyt = listSub.Where(o => ListExamServiceReqIdBHYT.Contains(o.ID)).ToList();
                            rdo.AMOUNT_BHYT = Bhyt.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Tong kham BHYT TT: " + String.Join(", ", Bhyt.GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            #region BHYT theo gioi tinh va do tuoi
                            rdo.AMOUNT_MALE_BHYT = Bhyt.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT Nam TT: " + String.Join(", ", Bhyt.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_FEMALE_BHYT = Bhyt.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT Nu TT: " + String.Join(", ", Bhyt.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER6_BHYT = Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT duoi6 TT: " + String.Join(", ", Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER16_BHYT = Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT duoi16 TT: " + String.Join(", ", Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_OVER60_BHYT = Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT tren60 TT: " + String.Join(", ", Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            #endregion
                            BHYTs.AddRange(listSub.Where(o => ListExamServiceReqIdBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList());
                            //Tổng khám VP - TT
                            var Vp = listSub.Where(o => ListExamServiceReqIdDV.Contains(o.ID)).ToList();
                            rdo.AMOUNT_VP = Vp.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Tong kham VP TT: " + String.Join(", ", Vp.GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));

                            #region VP theo gioi tinh va do tuoi
                            rdo.AMOUNT_MALE_VP = Vp.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP Nam TT: " + String.Join(", ", Vp.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_FEMALE_VP = Vp.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP Nu TT: " + String.Join(", ", Vp.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER6_VP = Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP duoi6 TT: " + String.Join(", ", Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER16_VP = Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP duoi16 TT: " + String.Join(", ", Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_OVER60_VP = Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP tren60 TT: " + String.Join(", ", Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            #endregion
                            //Tổng nhập viện - TT
                            rdo.AMOUNT_IN = listSub.Where(r => ListTreatmentIdIN.Contains(r.TREATMENT_ID) && getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Vao vien TT: " + String.Join(", ", listSub.Where(r => ListTreatmentIdIN.Contains(r.TREATMENT_ID) && getExamRoomId(r.TREATMENT_ID, ListExamServiceReq) == r.EXECUTE_ROOM_ID).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Select(z => z.TREATMENT_CODE).ToList()));

                            //Tổng khám chuyển tuyến - TT
                            rdo.AMOUNT_TRAN = listSub.Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Kham Chuyen Tuyen TT: " + String.Join(", ", listSub.Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));
                            //Tổng khám chuyển tuyến - TT
                            rdo.AMOUNT_BHYT_TRAN = listSub.Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID) && ListExamServiceReqIdBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Kham Chuyen Tuyen TT: " + String.Join(", ", listSub.Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_CODE).ToList()));
                            if (!dicRdoEX.ContainsKey(listSub.First().EXECUTE_ROOM_ID))
                                dicRdoEX.Add(listSub.First().EXECUTE_ROOM_ID, new Mrs00313RDO());

                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].EXECUTE_ROOM_NAME = rdo.EXECUTE_ROOM_NAME;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].TDL_EXECUTE_ROOM_ID = listSub.First().EXECUTE_ROOM_ID;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].EXECUTE_ROOM_CODE = rdo.EXECUTE_ROOM_CODE;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_TOTAL += rdo.COUNT_TOTAL;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_IN += rdo.COUNT_IN;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_IN_1 += rdo.COUNT_IN_1;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_TT += rdo.COUNT_TT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_BHYT += rdo.COUNT_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_TE += rdo.COUNT_TE;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_DV += rdo.COUNT_DV;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_BHYT_1 += rdo.COUNT_BHYT_1;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_TE_1 += rdo.COUNT_TE_1;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_DV_1 += rdo.COUNT_DV_1;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_NGT += rdo.COUNT_NGT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_EXAM_TRAN_PATI += rdo.COUNT_EXAM_TRAN_PATI;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].COUNT_TREAT_TRAN_PATI += rdo.COUNT_TREAT_TRAN_PATI;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_SUM += rdo.AMOUNT_SUM;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_BHYT += rdo.AMOUNT_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_MALE_BHYT += rdo.AMOUNT_MALE_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_FEMALE_BHYT += rdo.AMOUNT_FEMALE_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_UNDER6_BHYT += rdo.AMOUNT_UNDER6_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_UNDER16_BHYT += rdo.AMOUNT_UNDER16_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_OVER60_BHYT += rdo.AMOUNT_OVER60_BHYT;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_VP += rdo.AMOUNT_VP;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_MALE_VP += rdo.AMOUNT_MALE_VP;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_FEMALE_VP += rdo.AMOUNT_FEMALE_VP;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_UNDER6_VP += rdo.AMOUNT_UNDER6_VP;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_UNDER16_VP += rdo.AMOUNT_UNDER16_VP;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_OVER60_VP += rdo.AMOUNT_OVER60_VP;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_IN += rdo.AMOUNT_IN;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_TRAN += rdo.AMOUNT_TRAN;
                            dicRdoEX[listSub.First().EXECUTE_ROOM_ID].AMOUNT_BHYT_TRAN += rdo.AMOUNT_BHYT_TRAN;

                            if (dateMonthAmount != null)
                            {
                                dateMonthAmount.DIC_AMOUNT["COUNT_TOTAL"] += rdo.COUNT_TOTAL;
                                dateMonthAmount.DIC_AMOUNT["COUNT_IN"] += rdo.COUNT_IN;
                                dateMonthAmount.DIC_AMOUNT["COUNT_IN_1"] += rdo.COUNT_IN_1;
                                dateMonthAmount.DIC_AMOUNT["COUNT_TT"] += rdo.COUNT_TT;
                                dateMonthAmount.DIC_AMOUNT["COUNT_BHYT"] += rdo.COUNT_BHYT;
                                dateMonthAmount.DIC_AMOUNT["COUNT_TE"] += rdo.COUNT_TE;
                                dateMonthAmount.DIC_AMOUNT["COUNT_DV"] += rdo.COUNT_DV;
                                dateMonthAmount.DIC_AMOUNT["COUNT_BHYT_1"] += rdo.COUNT_BHYT_1;
                                dateMonthAmount.DIC_AMOUNT["COUNT_TE_1"] += rdo.COUNT_TE_1;
                                dateMonthAmount.DIC_AMOUNT["COUNT_DV_1"] += rdo.COUNT_DV_1;
                                dateMonthAmount.DIC_AMOUNT["COUNT_NGT"] += rdo.COUNT_NGT;
                                dateMonthAmount.DIC_AMOUNT["COUNT_EXAM_TRAN_PATI"] += rdo.COUNT_EXAM_TRAN_PATI;
                                dateMonthAmount.DIC_AMOUNT["COUNT_TREAT_TRAN_PATI"] += rdo.COUNT_TREAT_TRAN_PATI;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_SUM"] += rdo.AMOUNT_SUM;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_BHYT"] += rdo.AMOUNT_BHYT;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_MALE_BHYT"] += rdo.AMOUNT_MALE_BHYT;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_FEMALE_BHYT"] += rdo.AMOUNT_FEMALE_BHYT;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_UNDER6_BHYT"] += rdo.AMOUNT_UNDER6_BHYT;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_UNDER16_BHYT"] += rdo.AMOUNT_UNDER16_BHYT;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_OVER60_BHYT"] += rdo.AMOUNT_OVER60_BHYT;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_VP"] += rdo.AMOUNT_VP;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_MALE_VP"] += rdo.AMOUNT_MALE_VP;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_FEMALE_VP"] += rdo.AMOUNT_FEMALE_VP;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_UNDER6_VP"] += rdo.AMOUNT_UNDER6_VP;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_UNDER16_VP"] += rdo.AMOUNT_UNDER16_VP;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_OVER60_VP"] += rdo.AMOUNT_OVER60_VP;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_IN"] += rdo.AMOUNT_IN;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_TRAN"] += rdo.AMOUNT_TRAN;
                                dateMonthAmount.DIC_AMOUNT["AMOUNT_BHYT_TRAN"] += rdo.AMOUNT_BHYT_TRAN;
                            }
                        }
                    }

                    foreach (var examRoom in activeExamRoom)
                    {
                        if (!dicRdoEX.ContainsKey(examRoom.ROOM_ID))
                        {
                            Mrs00313RDO rdo = new Mrs00313RDO();
                            rdo.EXECUTE_ROOM_CODE = examRoom.EXECUTE_ROOM_CODE;
                            rdo.EXECUTE_ROOM_NAME = examRoom.EXECUTE_ROOM_NAME;
                            rdo.COUNT_TT = IsNotNullOrEmpty(ListServiceReqTTEX) ? ListServiceReqTTEX.Where(o => o.REQUEST_ROOM_CODE == examRoom.EXECUTE_ROOM_CODE).ToList().Count : 0;
                            //Inventec.Common.Logging.LogSystem.Info("Thu Thuat: " + String.Join(", ", ListServiceReqTTEX.Where(o => o.REQUEST_ROOM_CODE == examRoom).ToList()));
                            dicRdoEX.Add(examRoom.ROOM_ID, rdo);
                        }
                    }

                    #endregion

                    #region khai bao dieu tri
                    //Chuyển khoa BHYT:
                    var departmentTranBHYT = ListDepartmentTran.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(p => p.ID).ToList() ?? new List<long>();
                    //Chuyển khoa TE:
                    var departmentTranTE = ListDepartmentTran.Where(o => IsNotNullOrEmpty(listTreatmentTE) && listTreatmentTE.Contains(o.TREATMENT_ID)).Select(p => p.ID).ToList() ?? new List<long>();
                    //Chuyển khoa DV:
                    var departmentTranDV = ListDepartmentTran.Where(o => patientTypeId(o, LisPatientTypeAlter) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(p => p.ID).ToList() ?? new List<long>();
                    //khoa lam sang
                    var listClnDepartmentCode = listDepartment.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.DEPARTMENT_CODE).ToList();
                    #endregion

                    #region dieu tri
                    //co nam vien o khoa && nam luc lay bao cao && nam o khoa lam sang
                    var DepartmentTranTreat = ListDepartmentTran.Where(o => (IsTreatOut(o, LisPatientTypeAlter, ListDepartmentTran) || IsTreatIn(o, LisPatientTypeAlter, ListDepartmentTran) || IsTreatLightDay(o, LisPatientTypeAlter, ListDepartmentTran)) && IsStayingDepartment(NextDepartment(o, ListDepartmentTran), o) && listClnDepartmentCode.Contains(o.DEPARTMENT_CODE)).ToList();

                    //loc chi lay khoa nhap vien dau tien
                    if (filter.IS_HOSPITALIZE_DEPARTMENT == true)
                    {
                        DepartmentTranTreat = DepartmentTranTreat.Where(o => dicHospitalizeDepartmentId.ContainsKey(o.TREATMENT_ID) && dicHospitalizeDepartmentId[o.TREATMENT_ID] == o.DEPARTMENT_ID).ToList();
                    }

                    var groupbyDepartmentId = DepartmentTranTreat.GroupBy(o => o.DEPARTMENT_ID).ToList();

                    if (IsNotNullOrEmpty(groupbyDepartmentId))
                    {

                        foreach (var group in groupbyDepartmentId)
                        {
                            List<V_HIS_DEPARTMENT_TRAN> listSub = group.ToList<V_HIS_DEPARTMENT_TRAN>();
                            Mrs00313RDO rdo = new Mrs00313RDO();
                            rdo.EXECUTE_DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                            //Inventec.Common.Logging.LogSystem.Info("Khoa dieu tri: " + listSub.First().DEPARTMENT_NAME);

                            rdo.AMOUNT_SUM = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //So HSDT BHYT(BVTT)
                            var Bhyt = listSub.Where(o => patientTypeId(o, LisPatientTypeAlter) == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                            rdo.AMOUNT_BHYT = Bhyt.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Tong DT BHYT TT: " + String.Join(", ", Bhyt.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList()));
                            #region BHYT theo gioi tinh va do tuoi
                            rdo.AMOUNT_MALE_BHYT = Bhyt.Where(o => Treatment(o.TREATMENT_ID, ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT BHYT Nam TT: " + String.Join(", ", Bhyt.Where(o => Treatment(o.TREATMENT_ID,ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_FEMALE_BHYT = Bhyt.Where(o => Treatment(o.TREATMENT_ID, ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT BHYT Nu TT: " + String.Join(", ", Bhyt.Where(o => Treatment(o.TREATMENT_ID,ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER6_BHYT = Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT BHYT duoi6 TT: " + String.Join(", ", Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER16_BHYT = Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT BHYT duoi16 TT: " + String.Join(", ", Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_OVER60_BHYT = Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT BHYT tren60 TT: " + String.Join(", ", Bhyt.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            #endregion
                            //Tổng điều trị VP - TT
                            var Vp = listSub.Where(o => patientTypeId(o, LisPatientTypeAlter) != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                            rdo.AMOUNT_VP = Vp.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Tong DT VP TT: " + String.Join(", ", Vp.GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList()));

                            #region VP theo gioi tinh va do tuoi
                            rdo.AMOUNT_MALE_VP = Vp.Where(o => Treatment(o.TREATMENT_ID, ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT VP Nam TT: " + String.Join(", ", Vp.Where(o => Treatment(o.TREATMENT_ID,ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_FEMALE_VP = Vp.Where(o => Treatment(o.TREATMENT_ID, ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT VP Nu TT: " + String.Join(", ", Vp.Where(o => Treatment(o.TREATMENT_ID,ListTreatment).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER6_VP = Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT VP duoi6 TT: " + String.Join(", ", Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_UNDER16_VP = Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT VP duoi16 TT: " + String.Join(", ", Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) <= 16 && Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) > 6).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            rdo.AMOUNT_OVER60_VP = Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("DT VP tren60 TT: " + String.Join(", ", Vp.Where(o => Inventec.Common.DateTime.Calculation.Age(o.TDL_PATIENT_DOB) >= 60).GroupBy(q => q.TREATMENT_ID).Select(p => p.First().TREATMENT_CODE).ToList()));
                            #endregion
                            if (!dicRdoTreat.ContainsKey(listSub.First().DEPARTMENT_ID))
                                dicRdoTreat.Add(listSub.First().DEPARTMENT_ID, new Mrs00313RDO());

                            dicRdoTreat[listSub.First().DEPARTMENT_ID].EXECUTE_DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_SUM += rdo.AMOUNT_SUM;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_BHYT += rdo.AMOUNT_BHYT;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_MALE_BHYT += rdo.AMOUNT_MALE_BHYT;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_FEMALE_BHYT += rdo.AMOUNT_FEMALE_BHYT;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_UNDER6_BHYT += rdo.AMOUNT_UNDER6_BHYT;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_UNDER16_BHYT += rdo.AMOUNT_UNDER16_BHYT;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_OVER60_BHYT += rdo.AMOUNT_OVER60_BHYT;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_VP += rdo.AMOUNT_VP;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_MALE_VP += rdo.AMOUNT_MALE_VP;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_FEMALE_VP += rdo.AMOUNT_FEMALE_VP;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_UNDER6_VP += rdo.AMOUNT_UNDER6_VP;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_UNDER16_VP += rdo.AMOUNT_UNDER16_VP;
                            dicRdoTreat[listSub.First().DEPARTMENT_ID].AMOUNT_OVER60_VP += rdo.AMOUNT_OVER60_VP;
                            if (dateMonthAmount != null)
                            {
                                dateMonthAmount.DIC_AMOUNT["TOTAL_IN_BHYT"] += rdo.AMOUNT_BHYT;
                                dateMonthAmount.DIC_AMOUNT["TOTAL_IN_VP"] += rdo.AMOUNT_VP;
                            }
                        }
                    }
                    #endregion

                    #region dieu tri ngoai tru
                    var DepartmentTranOutTreat = ListDepartmentTran.Where(o => o.DEPARTMENT_IN_TIME >= filter.TIME_FROM && IsTreatOut(o, LisPatientTypeAlter, ListDepartmentTran) && IsStayingDepartment(NextDepartment(o, ListDepartmentTran), o) && listClnDepartmentCode.Contains(o.DEPARTMENT_CODE)).ToList();
                    var groupbyDepartmentIdNGT = DepartmentTranOutTreat.GroupBy(o => o.DEPARTMENT_ID).ToList();

                    if (IsNotNullOrEmpty(groupbyDepartmentIdNGT))
                    {

                        foreach (var group in groupbyDepartmentIdNGT)
                        {

                            List<V_HIS_DEPARTMENT_TRAN> listSub = group.ToList<V_HIS_DEPARTMENT_TRAN>();
                            Mrs00313RDO rdo = new Mrs00313RDO();
                            rdo.EXECUTE_DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME + " (Ngoại trú)";
                            //Inventec.Common.Logging.LogSystem.Info("Khoa ngoai tru: " + listSub.First().DEPARTMENT_NAME);

                            //Tổng ngoại trú
                            rdo.COUNT_NGT = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            rdo.COUNT_IN = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;

                            rdo.COUNT_IN_1 = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Ngoai tru: " + String.Join(", ", listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            //Thủ thuật - Ngoại trú
                            rdo.COUNT_TT = IsNotNullOrEmpty(ListServiceReqTTTR) ? ListServiceReqTTTR.Where(o => o.REQUEST_DEPARTMENT_ID == listSub.First().DEPARTMENT_ID && listSub.Select(n => n.TREATMENT_ID).Contains(o.TREATMENT_ID)).ToList().Count : 0;
                            //Inventec.Common.Logging.LogSystem.Info("Thu thuat: " + String.Join(", ", ListServiceReqTTTR.Where(o => o.REQUEST_DEPARTMENT_ID == listSub.First().DEPARTMENT_ID && listSub.Select(n => n.TREATMENT_ID).Contains(o.TREATMENT_ID)).ToList().Select(z => z.TREATMENT_CODE).ToList()));
                            rdo.COUNT_BHYT = listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT: " + String.Join(", ", listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_TE = listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("TE: " + String.Join(", ", listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_DV = listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP: " + String.Join(", ", listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_BHYT_1 = listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BN BHYT: " + String.Join(", ", listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_TE_1 = listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BN TE: " + String.Join(", ", listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList()));
                            rdo.COUNT_DV_1 = listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BN DV: " + String.Join(", ", listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().ToList()));

                            rdo.COUNT_EXAM_TRAN_PATI = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).ToList().Count; //Tổng BN vào khám chuyển tuyến sau khi khám

                            //Inventec.Common.Logging.LogSystem.Info("chuyen tuyen Kham: " + String.Join(", ", listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            //Điều trị chuyển tuyến
                            rdo.COUNT_TREAT_TRAN_PATI = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATITR.Contains(o.TREATMENT_ID) && ListTreatmentIdNGT.Contains(o.TREATMENT_ID)).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("chuyen tuyen DT: " + String.Join(", ", listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATITR.Contains(o.TREATMENT_ID)).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            if (!dicRdoOUT.ContainsKey(listSub.First().DEPARTMENT_ID))
                                dicRdoOUT.Add(listSub.First().DEPARTMENT_ID, new Mrs00313RDO());

                            dicRdoOUT[listSub.First().DEPARTMENT_ID].EXECUTE_DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME + " (Ngoại trú)";
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_NGT += rdo.COUNT_NGT;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_IN += rdo.COUNT_IN;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_TT += rdo.COUNT_TT;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_BHYT += rdo.COUNT_BHYT;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_TE += rdo.COUNT_TE;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_DV += rdo.COUNT_DV;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_BHYT_1 += rdo.COUNT_BHYT_1;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_TE_1 += rdo.COUNT_TE_1;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_DV_1 += rdo.COUNT_DV_1;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_EXAM_TRAN_PATI += rdo.COUNT_EXAM_TRAN_PATI;
                            dicRdoOUT[listSub.First().DEPARTMENT_ID].COUNT_TREAT_TRAN_PATI += rdo.COUNT_TREAT_TRAN_PATI;

                        }
                    }

                    #endregion

                    #region kham ngoai gio
                    var DepartmentTranOver = ListDepartmentTran.Where(o => o.DEPARTMENT_IN_TIME >= filter.TIME_FROM && IsOver(o, LisPatientTypeAlter, ListDepartmentTran) && listClnDepartmentCode.Contains(o.DEPARTMENT_CODE)).ToList();
                    var groupbyDepartmentIdOver = DepartmentTranOver.GroupBy(o => o.DEPARTMENT_ID).ToList();

                    if (IsNotNullOrEmpty(groupbyDepartmentIdOver))
                    {

                        foreach (var group in groupbyDepartmentIdOver)
                        {

                            List<V_HIS_DEPARTMENT_TRAN> listSub = group.ToList<V_HIS_DEPARTMENT_TRAN>();
                            Mrs00313RDO rdo = new Mrs00313RDO();
                            rdo.EXECUTE_DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                            //Inventec.Common.Logging.LogSystem.Info("Khoa ngoai tru: " + listSub.First().DEPARTMENT_NAME);


                            rdo.COUNT_IN_1 = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("Ngoai gio: " + String.Join(", ", listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));

                            rdo.COUNT_BHYT = listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BHYT NG: " + String.Join(", ", listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_TE = listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("TE NG: " + String.Join(", ", listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_DV = listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("VP NG: " + String.Join(", ", listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_BHYT_1 = listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BN BHYT NG: " + String.Join(", ", listSub.Where(o => departmentTranBHYT.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            rdo.COUNT_TE_1 = listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BN TE NG: " + String.Join(", ", listSub.Where(o => departmentTranTE.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList()));
                            rdo.COUNT_DV_1 = listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("BN DV NG: " + String.Join(", ", listSub.Where(o => departmentTranDV.Contains(o.ID)).GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).ToList().ToList()));

                            rdo.COUNT_EXAM_TRAN_PATI = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).ToList().Count; //Tổng BN vào khám chuyển tuyến sau khi khám

                            //Inventec.Common.Logging.LogSystem.Info("chuyen tuyen Kham NG: " + String.Join(", ", listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATIEX.Contains(o.TREATMENT_ID)).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            //Điều trị chuyển tuyến
                            rdo.COUNT_TREAT_TRAN_PATI = listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATITR.Contains(o.TREATMENT_ID) && ListTreatmentIdNGT.Contains(o.TREATMENT_ID)).ToList().Count;
                            //Inventec.Common.Logging.LogSystem.Info("chuyen tuyen DT NG: " + String.Join(", ", listSub.GroupBy(q => q.TREATMENT_ID).Select(p => p.First()).Where(o => listTreatmentIdTRAN_PATITR.Contains(o.TREATMENT_ID)).ToList().Select(z => z.TREATMENT_ID).ToList()));
                            if (!dicRdoOVER.ContainsKey(listSub.First().DEPARTMENT_ID))
                                dicRdoOVER.Add(listSub.First().DEPARTMENT_ID, new Mrs00313RDO());

                            dicRdoOVER[listSub.First().DEPARTMENT_ID].EXECUTE_DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_IN_1 += rdo.COUNT_IN_1;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_BHYT += rdo.COUNT_BHYT;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_TE += rdo.COUNT_TE;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_DV += rdo.COUNT_DV;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_BHYT_1 += rdo.COUNT_BHYT_1;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_TE_1 += rdo.COUNT_TE_1;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_DV_1 += rdo.COUNT_DV_1;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_EXAM_TRAN_PATI += rdo.COUNT_EXAM_TRAN_PATI;
                            dicRdoOVER[listSub.First().DEPARTMENT_ID].COUNT_TREAT_TRAN_PATI += rdo.COUNT_TREAT_TRAN_PATI;

                        }
                    }
                    #endregion

                }



            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }


        private long DepartmentId(V_HIS_SERVICE_REQ item, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter, List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            long result = 0;
            try
            {
                var patientTypeAlterTreatIn = LisPatientTypeAlter.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TREATMENT_ID == item.TREATMENT_ID).OrderBy(p => p.LOG_TIME).LastOrDefault();
                if (patientTypeAlterTreatIn != null)
                {
                    var DepartmentIn = ListDepartmentTran.Where(o => o.TREATMENT_ID == item.TREATMENT_ID && o.DEPARTMENT_IN_TIME <= patientTypeAlterTreatIn.LOG_TIME).OrderBy(p => p.DEPARTMENT_IN_TIME).LastOrDefault()
                        ?? ListDepartmentTran.Where(o => o.TREATMENT_ID == item.TREATMENT_ID && o.DEPARTMENT_IN_TIME > patientTypeAlterTreatIn.LOG_TIME).OrderBy(p => p.DEPARTMENT_IN_TIME).FirstOrDefault();
                    if (DepartmentIn != null)
                    {
                        result = DepartmentIn.DEPARTMENT_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
                LogSystem.Error(ex);
            }
            return result;
        }

        private V_HIS_TREATMENT Treatment(long p, List<V_HIS_TREATMENT> ListTreatment)
        {
            return ListTreatment.FirstOrDefault(o => o.ID == p) ?? new V_HIS_TREATMENT();
        }

        //BVNTH: kham ngoai gio: khong vao nam vien, ko phai chuyen tu khoa khac toi, ko phai khoa kham benh
        private bool IsOver(V_HIS_DEPARTMENT_TRAN indepartment, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter, List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            bool result = false;
            try
            {
                var lisDepartmentTranSub = ListDepartmentTran.Where(o => o.TREATMENT_ID == indepartment.TREATMENT_ID && o.DEPARTMENT_IN_TIME < indepartment.DEPARTMENT_IN_TIME).ToList();
                var listPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == indepartment.TREATMENT_ID && (o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)).ToList();
                if (IsNotNullOrEmpty(listPatientTypeAlterSub)) return false;
                else if (IsNotNullOrEmpty(lisDepartmentTranSub)) return false;
                else if (indepartment.DEPARTMENT_CODE == departmentExamCode) return false;
                else return true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return false;
            }
            return result;
        }

        //khoa lien ke
        private V_HIS_DEPARTMENT_TRAN NextDepartment(V_HIS_DEPARTMENT_TRAN o, List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            V_HIS_DEPARTMENT_TRAN result = new V_HIS_DEPARTMENT_TRAN();
            try
            {
                var ListDepartmentTranSub = ListDepartmentTran.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID).ToList();
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

        //Nam vien tai khoa?
        private bool IsStayingDepartment(V_HIS_DEPARTMENT_TRAN outDepartment, V_HIS_DEPARTMENT_TRAN inDepartment)
        {
            bool result = false;
            try
            {

                var filter = ((Mrs00313Filter)reportFilter);
                result = inDepartment.DEPARTMENT_IN_TIME <= filter.TIME_TO && (outDepartment.DEPARTMENT_IN_TIME >= filter.TIME_FROM || outDepartment.DEPARTMENT_IN_TIME == null) ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        //phong ket thuc kham cuoi cung
        private long getExamRoomId(long treatmentId, List<V_HIS_SERVICE_REQ> listExamServiceReq)
        {
            long result = -1;
            try
            {
                var listExamServiceReqSub = listExamServiceReq.Where(o => o.TREATMENT_ID == treatmentId).ToList();
                if (IsNotNullOrEmpty(listExamServiceReqSub))
                    if (listExamServiceReqSub.Where(o => o.FINISH_TIME == null).ToList().Count > 0) result = listExamServiceReqSub.Where(o => o.FINISH_TIME == null).OrderBy(p => p.ID).Last().EXECUTE_ROOM_ID;
                    else result = listExamServiceReqSub.OrderBy(p => p.FINISH_TIME).Last().EXECUTE_ROOM_ID;
            }
            catch (Exception ex)
            {
                result = -1;
                LogSystem.Error(ex);
            }
            return result;
        }

        //Dieu tri ngoai tru o khoa?
        private bool IsTreatOut(V_HIS_DEPARTMENT_TRAN o, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter, List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            bool Result = false;
            try
            {

                var listOutTreat = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && (p.LOG_TIME < NextDepartment(o, ListDepartmentTran).DEPARTMENT_IN_TIME || NextDepartment(o, ListDepartmentTran).DEPARTMENT_IN_TIME == null)).ToList();
                if (IsNotNullOrEmpty(listOutTreat))
                {

                    Result = listOutTreat.OrderBy(p => p.LOG_TIME).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                Result = false;
            }
            return Result;
        }

        //Dieu tri noi tru o khoa?
        private bool IsTreatIn(V_HIS_DEPARTMENT_TRAN o, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter, List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            bool Result = false;
            try
            {
                var listInTreat = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && (NextDepartment(o, ListDepartmentTran).DEPARTMENT_IN_TIME > p.LOG_TIME || NextDepartment(o, ListDepartmentTran).DEPARTMENT_IN_TIME == null)).ToList();
                if (IsNotNullOrEmpty(listInTreat))
                {

                    Result = listInTreat.OrderBy(p => p.LOG_TIME).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                Result = false;
            }
            return Result;
        }

        //Dieu tri ban ngay o khoa?
        private bool IsTreatLightDay(V_HIS_DEPARTMENT_TRAN o, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter, List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            bool Result = false;
            try
            {
                var listInTreat = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && (NextDepartment(o, ListDepartmentTran).DEPARTMENT_IN_TIME > p.LOG_TIME || NextDepartment(o, ListDepartmentTran).DEPARTMENT_IN_TIME == null)).ToList();
                if (IsNotNullOrEmpty(listInTreat))
                {

                    Result = listInTreat.OrderBy(p => p.LOG_TIME).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                Result = false;
            }
            return Result;
        }

        private List<long> treatmentTypeId(V_HIS_TREATMENT thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            List<long> result = new List<long>();
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.ID).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.Select(o => o.TREATMENT_TYPE_ID).ToList();
            }

            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

                result = new List<long>();
            }
            return result;
        }

        //Đối tượng
        private long patientTypeId(V_HIS_DEPARTMENT_TRAN thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TREATMENT_ID && o.LOG_TIME <= thisData.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().PATIENT_TYPE_ID;
                else
                {
                    LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TREATMENT_ID && o.LOG_TIME >= thisData.DEPARTMENT_IN_TIME).ToList();
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

        private long patientTypeId(V_HIS_SERVICE_REQ thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TREATMENT_ID && o.LOG_TIME <= thisData.INTRUCTION_TIME).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().PATIENT_TYPE_ID;
                else
                {
                    LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TREATMENT_ID && o.LOG_TIME > thisData.INTRUCTION_TIME).ToList();
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            if (((Mrs00313Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00313Filter)reportFilter).TIME_FROM));
                dicSingleTag.Add("IN_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00313Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00313Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00313Filter)reportFilter).TIME_TO));
                dicSingleTag.Add("IN_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00313Filter)reportFilter).TIME_TO));
            }
            var Branch = new HisBranchManager(paramGet).GetById(branch_id) ?? new HIS_BRANCH();

            //đúng tuyến
            var RightRoute = dicPatientTypeAlter.SelectMany(o => o.Value.ToList()).Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && o.HEIN_CARD_NUMBER != null).Select(p => p.TREATMENT_ID).ToList();
            long RIGHT_ROUTE = IsNotNullOrEmpty(RightRoute) ? BHYTs.Where(o => RightRoute.Contains(o)).GroupBy(p => p).Select(q => q.First()).ToList().Count : 0;

            //trái tuyến
            long LEFT_ROUTE = BHYTs.Where(o => !RightRoute.Contains(o)).GroupBy(p => p).Select(q => q.First()).ToList().Count;
            var ListPatientINP = dicPatient.SelectMany(o => o.Value.ToList()).Where(o => o.PROVINCE_CODE != Branch.PROVINCE_CODE && o.PROVINCE_CODE != null).Select(p => p.ID).ToList();
            var OutProvince = dicTreatment.SelectMany(o => o.Value.ToList()).Where(o => ListPatientINP.Contains(o.PATIENT_ID)).Select(p => p.ID).ToList();
            long OUT_PROVINCE = IsNotNullOrEmpty(OutProvince) ? dicExamServiceReq.SelectMany(o => o.Value.ToList()).Where(o => OutProvince.Contains(o.TREATMENT_ID)).GroupBy(p => p.TREATMENT_ID).Select(q => q.First()).ToList().Count : 0;

            long TOTAL_EXAM_SUM = dicRdoEX.Values.Sum(o => o.AMOUNT_SUM);
            long TOTAL_EXAM_BHYT = dicRdoEX.Values.Sum(o => o.AMOUNT_BHYT);
            long TOTAL_EXAM_VP = dicRdoEX.Values.Sum(o => o.AMOUNT_VP);
            long TOTAL_IN_BHYT = dicRdoTreat.Values.Sum(o => o.AMOUNT_BHYT);
            long TOTAL_IN_VP = dicRdoTreat.Values.Sum(o => o.AMOUNT_VP);
            long TOTAL_TRAN_BHYT = dicRdoEX.Values.Sum(o => o.AMOUNT_BHYT_TRAN);
            long TOTAL_TRAN_VP = dicRdoEX.Values.Sum(o => o.AMOUNT_TRAN - o.AMOUNT_BHYT_TRAN);
            dicSingleTag.Add("TOTAL_EXAM_SUM", TOTAL_EXAM_SUM);
            dicSingleTag.Add("TOTAL_EXAM_BHYT", TOTAL_EXAM_BHYT);
            dicSingleTag.Add("TOTAL_EXAM_VP", TOTAL_EXAM_VP);
            dicSingleTag.Add("TOTAL_IN_BHYT", TOTAL_IN_BHYT);
            dicSingleTag.Add("TOTAL_IN_VP", TOTAL_IN_VP);
            dicSingleTag.Add("TOTAL_TRAN_BHYT", TOTAL_TRAN_BHYT);
            dicSingleTag.Add("TOTAL_TRAN_VP", TOTAL_TRAN_VP);
            dicSingleTag.Add("RIGHT_ROUTE", RIGHT_ROUTE);
            dicSingleTag.Add("LEFT_ROUTE", LEFT_ROUTE);
            dicSingleTag.Add("OUT_PROVINCE", OUT_PROVINCE);


            objectTag.AddObjectData(store, "ExamRoom", dicRdoEX.Values.ToList());
            objectTag.AddObjectData(store, "Department", dicRdoTreat.Values.ToList());
            objectTag.AddObjectData(store, "DepartmentAmount", dicDepartmentAmount.Values.ToList());
            objectTag.AddObjectData(store, "ReportEX", dicRdoEX.Values.ToList());
            objectTag.AddObjectData(store, "ReportOUT", dicRdoOUT.Values.ToList());
            objectTag.AddObjectData(store, "ReportOVER", dicRdoOVER.Values.ToList());

            objectTag.AddObjectData(store, "DateMonthAmount", listDateMonthAmount);

        }
    }
}
