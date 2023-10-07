using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisRoom;

namespace MRS.Processor.Mrs00167
{
    internal class Mrs00167Processor : AbstractProcessor
    {

        List<VSarReportMRS00167RDO> _listSarReportMrs00167Rdos = new List<VSarReportMRS00167RDO>();
        List<S_HIS_SERE_SERV> ListSereServViews = new List<S_HIS_SERE_SERV>();
        Dictionary<long, decimal> dicSereServHPPrice = new Dictionary<long, decimal>();
        public List<MRS00167RDO> ListRdoDepartment = new List<MRS00167RDO>();
        public List<MRS00167RDO> ListRdoTreatment = new List<MRS00167RDO>();
        public List<MRS00167RDO> ListRdoService = new List<MRS00167RDO>();
        public List<MRS00167RDO> ListRdoTreatmentRoom = new List<MRS00167RDO>();
        public List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        public List<MRS00167RDO> ListRdoRoom = new List<MRS00167RDO>();
        public List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();
        public List<V_HIS_IMP_MEST_BLOOD> ImpMestBlood = new List<V_HIS_IMP_MEST_BLOOD>();
        public List<HIS_SERVICE_TYPE> ListServiceType = new List<HIS_SERVICE_TYPE>();
        Mrs00167Filter CastFilter;

        public Mrs00167Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00167Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                this.CastFilter = (Mrs00167Filter)this.reportFilter;

                var SereServViews = new ManagerSql().GetSum(this.CastFilter) ?? new List<S_HIS_SERE_SERV>();// MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).Get(ssFilter);
                dicSereServHPPrice = SereServViews.Where(o => o.PARENT_ID != null && o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).GroupBy(o => o.PARENT_ID??0).ToDictionary(p => p.Key, q => q.Sum(s =>( s.AMOUNT??0) * (s.VIR_PRICE_NO_EXPEND ?? 0)));
                //SereServViews = SereServViews.Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
               
                ListSereServViews.AddRange(SereServViews);
                ListSereServViews = ListSereServViews.Where(x => x.IS_DELETE == 0).ToList();
                

                ListDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
                ListRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
                ListServiceType = new HisServiceTypeManager().Get(new HisServiceTypeFilterQuery());
                ProcessFilterData();
                ProcessGroup();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleData, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleData.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleData.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
            dicSingleData.Add("DEPARTMENT_ID", string.Join(", ", HisDepartmentCFG.DEPARTMENTs.Where(o => CastFilter.REQ_DEPARTMENT_IDs != null && CastFilter.REQ_DEPARTMENT_IDs.Contains(o.ID)).ToList()));

            objectTag.AddObjectData(store, "Parent", _listSarReportMrs00167Rdos.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Report", _listSarReportMrs00167Rdos);
            objectTag.AddObjectData(store, "ServiceRoom", ListRdoTreatmentRoom.OrderBy(x => x.TREATMENT_CODE).ToList());

            //nối cha con
            objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            objectTag.AddObjectData(store, "Service", ListRdoDepartment);
            objectTag.AddObjectData(store, "TreatmentService", ListRdoTreatment);
            objectTag.AddObjectData(store, "ReportService", ListRdoService);
            objectTag.AddObjectData(store, "GroupByRoom", ListRdoRoom);
            if (CastFilter.IS_PATIENT_TYPE.HasValue && CastFilter.IS_PATIENT_TYPE == 0)
            {
                dicSingleData.Add("REPORT_TITLE_NAME", "BHYT");
            }
            else if (CastFilter.IS_PATIENT_TYPE.HasValue && CastFilter.IS_PATIENT_TYPE == 1)
            {
                dicSingleData.Add("REPORT_TITLE_NAME", "DỊCH VỤ");
            }
            else
            {
                dicSingleData.Add("REPORT_TITLE_NAME", "TỔNG HỢP");
            }

        }
        private void ProcessGroup()
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServViews))
                {

                    List<MRS00167RDO> ListRdo = new List<MRS00167RDO>();
                    var SereServViews = ListSereServViews;
                    foreach (var item in SereServViews)
                    {


                        MRS00167RDO rdo = new MRS00167RDO();
                        rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        rdo.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
                        var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == item.PATIENT_TYPE_ID);
                        if (patientType != null)
                        {
                            rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                        }

                        var roomR = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID);
                        var roomE = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID);
                        if (CastFilter.IS_EXECUTE_DEPARTMENT != null && CastFilter.IS_EXECUTE_DEPARTMENT == true)
                        {
                            rdo.DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID ?? 0;
                            rdo.ROOM_ID = item.TDL_EXECUTE_ROOM_ID ?? 0;
                            if (roomE != null)
                            {
                                rdo.ROOM_CODE = roomE.ROOM_CODE;
                                rdo.ROOM_NAME = roomE.ROOM_NAME;
                            }
                        }

                        else
                        {
                            rdo.DEPARTMENT_ID = item.TDL_REQUEST_DEPARTMENT_ID ?? 0;
                            rdo.ROOM_ID = item.TDL_REQUEST_ROOM_ID ?? 0;
                            if (roomR != null)
                            {
                                rdo.ROOM_CODE = roomR.ROOM_CODE;
                                rdo.ROOM_NAME = roomR.ROOM_NAME;
                            }
                        }
                        if (roomE != null && roomR != null && roomR.ROOM_NAME != null && roomR.ROOM_NAME.ToLower().Contains("tiếp đón"))
                        {
                            rdo.ROOM_NAME = roomE.ROOM_NAME;
                        }
                        var serviceType = ListServiceType.Where(x => x.ID == item.TDL_SERVICE_TYPE_ID).FirstOrDefault();
                        if (serviceType != null)
                        {
                            rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                            rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        }
                        //rdo.EXECUTTE_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                        var department = ListDepartment.Where(x => x.ID == rdo.DEPARTMENT_ID).FirstOrDefault();
                        rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.PATIENT_ID = item.PATIENT_ID;
                        rdo.PATIENT_CODE = item.TDL_PATIENT_CODE;
                        rdo.TREATMENT_ID = item.TDL_TREATMENT_ID;
                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.IN_TIME = item.IN_TIME;
                        rdo.OUT_TIME = item.OUT_TIME ?? 0;
                        if (department != null)
                        {
                            rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        }
                        decimal expend = 0;
                        if (dicSereServHPPrice.ContainsKey(item.ID))
                        {
                            expend = dicSereServHPPrice[item.ID];
                        }

                        rdo.IS_EXPEND = item.IS_EXPEND;
                        //if (item.IS_EXPEND==null)
                        //{
                        if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            rdo.KH_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.EXAM_PRICE_HP = expend;
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                        {
                            rdo.BED_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                        {
                            rdo.MEDICINE_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.MEDICINE_PRICE_HP = expend;
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                        {
                            rdo.MATERIAL_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            if (item.PARENT_ID > 0)
                            {
                                rdo.MATERIAL_PRICE_IN_PACK = item.AMOUNT * item.VIR_PRICE ?? 0;
                            }
                            else
                            {
                                rdo.MATERIAL_PRICE_OUT_PACK = item.AMOUNT * item.VIR_PRICE ?? 0;
                            }
                            rdo.MATERIAL_PRICE_HP = expend;
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                        {
                            rdo.BLOOD_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.BLOOD_PRICE_HP = expend;
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                        {
                            rdo.XN_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.XN_PRICE_HP += expend;
                        }
                        else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
                        {
                            rdo.DIMM_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.DIMM_PRICE_HP += expend;
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)
                        {
                            rdo.PTTT_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.PTTT_PRICE_HP += expend;
                        }
                        else
                        {
                            rdo.KHAC_PRICE = item.AMOUNT * item.VIR_PRICE ?? 0;
                            rdo.KHAC_PRICE_HP += expend;
                        }
                        //}

                        rdo.AMOUNT = item.AMOUNT ?? 0;
                        ListRdo.Add(rdo);
                    }
                    GroupByDepartment(ListRdo);
                    GroupByTreatment(ListRdo);
                    GroupByService(ListRdo);
                    GroupByTreatmentRoom(ListRdo);
                    GroupByRoom(ListRdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }
        private void GroupByRoom(List<MRS00167RDO> ListData)
        {
            try
            {
                var groupByRoom = ListData.GroupBy(x => x.ROOM_ID).ToList();
                foreach (var item in groupByRoom)
                {
                    MRS00167RDO rdo = new MRS00167RDO();
                    rdo.DEPARTMENT_ID = item.First().DEPARTMENT_ID;
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.ROOM_ID = item.First().ROOM_ID;
                    rdo.ROOM_CODE = item.First().ROOM_CODE;
                    rdo.ROOM_NAME = item.First().ROOM_NAME;
                    rdo.KH_PRICE = item.Sum(x => x.KH_PRICE);
                    rdo.EXAM_PRICE_HP = item.Sum(x => x.EXAM_PRICE_HP);
                    rdo.BED_PRICE = item.Sum(x => x.BED_PRICE);
                    rdo.MEDICINE_PRICE = item.Sum(x => x.MEDICINE_PRICE);
                    rdo.MATERIAL_PRICE = item.Sum(x => x.MATERIAL_PRICE);
                    rdo.MATERIAL_PRICE_IN_PACK = item.Sum(x => x.MATERIAL_PRICE_IN_PACK);
                    rdo.MATERIAL_PRICE_OUT_PACK = item.Sum(x => x.MATERIAL_PRICE_OUT_PACK);
                    rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                    rdo.MEDICINE_PRICE_HP = item.Sum(x => x.MEDICINE_PRICE_HP);
                    rdo.MATERIAL_PRICE_HP = item.Sum(x => x.MATERIAL_PRICE_HP);
                    rdo.BLOOD_PRICE_HP = item.Sum(x => x.BLOOD_PRICE_HP);
                    rdo.XN_PRICE = item.Sum(x => x.XN_PRICE);
                    rdo.XN_PRICE_HP = item.Sum(x => x.XN_PRICE_HP);
                    rdo.DIMM_PRICE = item.Sum(x => x.DIMM_PRICE);
                    rdo.DIMM_PRICE_HP = item.Sum(x => x.DIMM_PRICE_HP);
                    rdo.PTTT_PRICE = item.Sum(x => x.PTTT_PRICE);
                    rdo.PTTT_PRICE_HP = item.Sum(x => x.PTTT_PRICE_HP);
                    rdo.KHAC_PRICE = item.Sum(x => x.KHAC_PRICE);
                    rdo.KHAC_PRICE_HP = item.Sum(x => x.KHAC_PRICE_HP);
                    ListRdoRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoRoom = null;
            }
        }
        private void GroupByDepartment(List<MRS00167RDO> ListData)
        {
            try
            {
                var groupByDepartment = ListData.GroupBy(x => x.DEPARTMENT_ID).ToList();
                foreach (var item in groupByDepartment)
                {
                    MRS00167RDO rdo = new MRS00167RDO();
                    rdo.DEPARTMENT_ID = item.First().DEPARTMENT_ID;
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.KH_PRICE = item.Sum(x => x.KH_PRICE);
                    rdo.EXAM_PRICE_HP = item.Sum(x => x.EXAM_PRICE_HP);
                    rdo.BED_PRICE = item.Sum(x => x.BED_PRICE);
                    rdo.MEDICINE_PRICE = item.Sum(x => x.MEDICINE_PRICE);
                    rdo.MATERIAL_PRICE = item.Sum(x => x.MATERIAL_PRICE);
                    rdo.MATERIAL_PRICE_IN_PACK = item.Sum(x => x.MATERIAL_PRICE_IN_PACK);
                    rdo.MATERIAL_PRICE_OUT_PACK = item.Sum(x => x.MATERIAL_PRICE_OUT_PACK);
                    rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                    rdo.MEDICINE_PRICE_HP = item.Sum(x => x.MEDICINE_PRICE_HP);
                    rdo.MATERIAL_PRICE_HP = item.Sum(x => x.MATERIAL_PRICE_HP);
                    rdo.BLOOD_PRICE_HP = item.Sum(x => x.BLOOD_PRICE_HP);
                    rdo.XN_PRICE = item.Sum(x => x.XN_PRICE);
                    rdo.XN_PRICE_HP = item.Sum(x => x.XN_PRICE_HP);
                    rdo.DIMM_PRICE = item.Sum(x => x.DIMM_PRICE);
                    rdo.DIMM_PRICE_HP = item.Sum(x => x.DIMM_PRICE_HP);
                    rdo.PTTT_PRICE = item.Sum(x => x.PTTT_PRICE);
                    rdo.PTTT_PRICE_HP = item.Sum(x => x.PTTT_PRICE_HP);
                    rdo.KHAC_PRICE = item.Sum(x => x.KHAC_PRICE);
                    rdo.KHAC_PRICE_HP = item.Sum(x => x.KHAC_PRICE_HP);
                    ListRdoDepartment.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDepartment = null;
            }
        }

        private void GroupByTreatment(List<MRS00167RDO> ListData)
        {
            try
            {
                var groupByTreatment = ListData.GroupBy(x => new { x.TREATMENT_CODE, x.PATIENT_CODE }).ToList();
                foreach (var item in groupByTreatment)
                {
                    MRS00167RDO rdo = new MRS00167RDO();
                    rdo.TREATMENT_CODE = item.First().TREATMENT_CODE;
                    rdo.PATIENT_CODE = item.First().PATIENT_CODE;
                    rdo.IN_TIME = item.First().IN_TIME;
                    rdo.OUT_TIME = item.First().OUT_TIME;
                    rdo.PATIENT_NAME = item.First().PATIENT_NAME;
                    rdo.KH_PRICE = item.Sum(x => x.KH_PRICE);
                    rdo.EXAM_PRICE_HP = item.Sum(x => x.EXAM_PRICE_HP);
                    rdo.BED_PRICE = item.Sum(x => x.BED_PRICE);
                    rdo.MEDICINE_PRICE = item.Sum(x => x.MEDICINE_PRICE);
                    rdo.MATERIAL_PRICE = item.Sum(x => x.MATERIAL_PRICE);
                    rdo.MATERIAL_PRICE_IN_PACK = item.Sum(x => x.MATERIAL_PRICE_IN_PACK);
                    rdo.MATERIAL_PRICE_OUT_PACK = item.Sum(x => x.MATERIAL_PRICE_OUT_PACK);
                    rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                    rdo.MEDICINE_PRICE_HP = item.Sum(x => x.MEDICINE_PRICE_HP);
                    rdo.MATERIAL_PRICE_HP = item.Sum(x => x.MATERIAL_PRICE_HP);
                    rdo.BLOOD_PRICE_HP = item.Sum(x => x.BLOOD_PRICE_HP);
                    rdo.XN_PRICE = item.Sum(x => x.XN_PRICE);
                    rdo.XN_PRICE_HP = item.Sum(x => x.XN_PRICE_HP);
                    rdo.DIMM_PRICE = item.Sum(x => x.DIMM_PRICE);
                    rdo.DIMM_PRICE_HP = item.Sum(x => x.DIMM_PRICE_HP);
                    rdo.PTTT_PRICE = item.Sum(x => x.PTTT_PRICE);
                    rdo.PTTT_PRICE_HP = item.Sum(x => x.PTTT_PRICE_HP);
                    rdo.KHAC_PRICE = item.Sum(x => x.KHAC_PRICE);
                    rdo.KHAC_PRICE_HP = item.Sum(x => x.KHAC_PRICE_HP);
                    rdo.COUNT_SS = item.Count();
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.ROOM_NAME = item.First().ROOM_NAME;
                    rdo.ROOM_CODE = item.First().ROOM_CODE;
                    ListRdoTreatment.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoTreatment = null;
            }
        }
        private void GroupByTreatmentRoom(List<MRS00167RDO> ListData)
        {
            try
            {
                var groupByTreatment = ListData.GroupBy(x => new { x.TREATMENT_CODE, x.PATIENT_CODE, x.DEPARTMENT_ID, x.ROOM_ID }).ToList();
                foreach (var item in groupByTreatment)
                {
                    MRS00167RDO rdo = new MRS00167RDO();
                    rdo.TREATMENT_CODE = item.First().TREATMENT_CODE;
                    rdo.PATIENT_CODE = item.First().PATIENT_CODE;
                    rdo.IN_TIME = item.First().IN_TIME;
                    rdo.OUT_TIME = item.First().OUT_TIME;
                    rdo.PATIENT_NAME = item.First().PATIENT_NAME;
                    rdo.KH_PRICE = item.Sum(x => x.KH_PRICE);
                    rdo.EXAM_PRICE_HP = item.Sum(x => x.EXAM_PRICE_HP);
                    rdo.BED_PRICE = item.Sum(x => x.BED_PRICE);
                    rdo.MEDICINE_PRICE = item.Sum(x => x.MEDICINE_PRICE);
                    rdo.MATERIAL_PRICE = item.Sum(x => x.MATERIAL_PRICE);
                    rdo.MATERIAL_PRICE_IN_PACK = item.Sum(x => x.MATERIAL_PRICE_IN_PACK);
                    rdo.MATERIAL_PRICE_OUT_PACK = item.Sum(x => x.MATERIAL_PRICE_OUT_PACK);
                    rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                    rdo.MEDICINE_PRICE_HP = item.Sum(x => x.MEDICINE_PRICE_HP);
                    rdo.MATERIAL_PRICE_HP = item.Sum(x => x.MATERIAL_PRICE_HP);
                    rdo.BLOOD_PRICE_HP = item.Sum(x => x.BLOOD_PRICE_HP);
                    rdo.XN_PRICE = item.Sum(x => x.XN_PRICE);
                    rdo.XN_PRICE_HP = item.Sum(x => x.XN_PRICE_HP);
                    rdo.DIMM_PRICE = item.Sum(x => x.DIMM_PRICE);
                    rdo.DIMM_PRICE_HP = item.Sum(x => x.DIMM_PRICE_HP);
                    rdo.PTTT_PRICE = item.Sum(x => x.PTTT_PRICE);
                    rdo.PTTT_PRICE_HP = item.Sum(x => x.PTTT_PRICE_HP);
                    rdo.KHAC_PRICE = item.Sum(x => x.KHAC_PRICE);
                    rdo.KHAC_PRICE_HP = item.Sum(x => x.KHAC_PRICE_HP);
                    rdo.COUNT_SS = item.Count();
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.ROOM_NAME = item.First().ROOM_NAME;
                    rdo.ROOM_CODE = item.First().ROOM_CODE;
                    ListRdoTreatmentRoom.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoTreatmentRoom = null;
            }
        }
        private void GroupByService(List<MRS00167RDO> ListData)
        {
            try
            {
                var groupByService = ListData.GroupBy(x => new { x.TREATMENT_CODE, x.PATIENT_CODE, x.DEPARTMENT_ID, x.SERVICE_NAME, x.SERVICE_CODE, x.SERVICE_TYPE_NAME }).ToList();
                foreach (var item in groupByService)
                {
                    MRS00167RDO rdo = new MRS00167RDO();

                    rdo.TREATMENT_CODE = item.First().TREATMENT_CODE;
                    rdo.PATIENT_CODE = item.First().PATIENT_CODE;
                    rdo.IN_TIME = item.First().IN_TIME;
                    rdo.OUT_TIME = item.First().OUT_TIME;
                    rdo.PATIENT_NAME = item.First().PATIENT_NAME;
                    rdo.KH_PRICE = item.Sum(x => x.KH_PRICE);
                    rdo.EXAM_PRICE_HP = item.Sum(x => x.EXAM_PRICE_HP);
                    rdo.BED_PRICE = item.Sum(x => x.BED_PRICE);
                    rdo.MEDICINE_PRICE = item.Sum(x => x.MEDICINE_PRICE);
                    rdo.MATERIAL_PRICE = item.Sum(x => x.MATERIAL_PRICE);
                    rdo.MATERIAL_PRICE_IN_PACK = item.Sum(x => x.MATERIAL_PRICE_IN_PACK);
                    rdo.MATERIAL_PRICE_OUT_PACK = item.Sum(x => x.MATERIAL_PRICE_OUT_PACK);
                    rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                    rdo.MEDICINE_PRICE_HP = item.Sum(x => x.MEDICINE_PRICE_HP);
                    rdo.MATERIAL_PRICE_HP = item.Sum(x => x.MATERIAL_PRICE_HP);
                    rdo.BLOOD_PRICE_HP = item.Sum(x => x.BLOOD_PRICE_HP);
                    rdo.XN_PRICE = item.Sum(x => x.XN_PRICE);
                    rdo.XN_PRICE_HP = item.Sum(x => x.XN_PRICE_HP);
                    rdo.DIMM_PRICE = item.Sum(x => x.DIMM_PRICE);
                    rdo.DIMM_PRICE_HP = item.Sum(x => x.DIMM_PRICE_HP);
                    rdo.PTTT_PRICE = item.Sum(x => x.PTTT_PRICE);
                    rdo.PTTT_PRICE_HP = item.Sum(x => x.PTTT_PRICE_HP);
                    rdo.KHAC_PRICE = item.Sum(x => x.KHAC_PRICE);
                    rdo.KHAC_PRICE_HP = item.Sum(x => x.KHAC_PRICE_HP);
                    rdo.COUNT_SS = item.Count();
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.SERVICE_NAME = item.First().SERVICE_NAME;
                    rdo.SERVICE_CODE = item.First().SERVICE_CODE;
                    rdo.SERVICE_TYPE_NAME = item.First().SERVICE_TYPE_NAME;
                    rdo.SERVICE_TYPE_CODE = item.First().SERVICE_TYPE_CODE;
                    ListRdoService.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoService = null;
            }
        }

        private void ProcessFilterData()
        {
            try
            {
                var gruopNames = ListSereServViews.GroupBy(s => s.SERVICE_ID).ToList();
                foreach (var gruopName in gruopNames)
                {
                    var price = gruopName.Select(s => s.VIR_PRICE ?? 0).First();
                    var amount = gruopName.Sum(s => s.AMOUNT);
                    VSarReportMRS00167RDO rdo = new VSarReportMRS00167RDO
                    {
                        SERVICE_TYPE_ID = gruopName.First().TDL_SERVICE_TYPE_ID ?? 0,

                        SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == gruopName.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME,
                        SERVICE_NAME = gruopName.First().TDL_SERVICE_NAME,
                        PRICE = price,
                        AMOUNT = amount,
                        TOTAL_PRICE = price * amount
                    };
                    _listSarReportMrs00167Rdos.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
