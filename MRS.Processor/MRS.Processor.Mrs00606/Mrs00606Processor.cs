using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisServiceReq;
using SDA.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisExecuteRoom;

namespace MRS.Processor.Mrs00606
{
    public class Mrs00606Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00606RDO> listRdo = new List<Mrs00606RDO>();
        private List<Mrs00606RDO> listRdoExamRoom = new List<Mrs00606RDO>();
        private List<HIS_TRANSACTION> ListHisTransaction = new List<HIS_TRANSACTION>();
        Mrs00606Filter filter = null;
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERVICE_METY> listHisServiceMety = new List<HIS_SERVICE_METY>();
        List<HIS_SERVICE_MATY> listHisServiceMaty = new List<HIS_SERVICE_MATY>();
        List<HIS_SERVICE_MATY> listHisServiceChty = new List<HIS_SERVICE_MATY>();
        List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_EXECUTE_ROOM> ListHisExecuteRoom = new List<HIS_EXECUTE_ROOM>();

        public Mrs00606Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00606Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00606Filter)reportFilter);
            var result = true;
            CommonParam param = new CommonParam();
            try
            {
               
                //Nếu có lọc theo thời gian thanh toán thì xử lý:
                if (filter.TRANSACTION_TIME_FROM != null && filter.TRANSACTION_TIME_TO != null)
                {
                    HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                    HisTransactionfilter.TRANSACTION_TIME_FROM = filter.TRANSACTION_TIME_FROM;
                    HisTransactionfilter.TRANSACTION_TIME_TO = filter.TRANSACTION_TIME_TO;
                    HisTransactionfilter.IS_CANCEL = false;
                    ListHisTransaction = new HisTransactionManager(paramGet).Get(HisTransactionfilter);
                    if (ListHisTransaction != null && ListHisTransaction.Count > 0)
                    {
                        var treatmentIds = ListHisTransaction.Where(o => o.TREATMENT_ID.HasValue).Select(p => p.TREATMENT_ID.Value).Distinct().ToList();
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                            sereServFilter.TREATMENT_IDs = Ids;
                            sereServFilter.HAS_EXECUTE = true;
                            sereServFilter.PATIENT_TYPE_ID = filter.PATIENT_TYPE_ID;
                            var listSereServSub = new HisSereServManager(param).Get(sereServFilter);
                            listHisSereServ.AddRange(listSereServSub);

                            HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
                            sereServBillFilter.TDL_TREATMENT_IDs = Ids;
                            sereServBillFilter.IS_NOT_CANCEL = true;
                            var listSereServBillSub = new HisSereServBillManager(param).Get(sereServBillFilter);
                            listHisSereServBill.AddRange(listSereServBillSub);
                            listHisSereServ = listHisSereServ.Where(o => listHisSereServBill.Exists(p => p.SERE_SERV_ID == o.ID && ListHisTransaction.Exists(q => q.ID == p.BILL_ID))).ToList();
                        }
                    }
                }
                //Nếu lọc theo thời gian chỉ định thì xử lí:
                else
                {
                    HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                    HisServiceReqfilter.INTRUCTION_TIME_FROM = filter.INTRUCTION_TIME_FROM;
                    HisServiceReqfilter.INTRUCTION_TIME_TO = filter.INTRUCTION_TIME_TO;
                    HisServiceReqfilter.FINISH_TIME_FROM = filter.FINISH_TIME_FROM;
                    HisServiceReqfilter.FINISH_TIME_TO = filter.FINISH_TIME_TO;
                    HisServiceReqfilter.HAS_EXECUTE = true;
                    listHisServiceReq = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);
                    if (listHisServiceReq != null && listHisServiceReq.Count > 0)
                    {
                        var treatmentIds = listHisServiceReq.Select(p => p.TREATMENT_ID).Distinct().ToList();
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                            sereServFilter.TREATMENT_IDs = Ids;
                            sereServFilter.HAS_EXECUTE = true;
                            sereServFilter.PATIENT_TYPE_ID = filter.PATIENT_TYPE_ID;
                            var listSereServSub = new HisSereServManager(param).Get(sereServFilter);
                            listHisSereServ.AddRange(listSereServSub);

                            HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
                            sereServBillFilter.TDL_TREATMENT_IDs = Ids;
                            sereServBillFilter.IS_NOT_CANCEL = true;
                            var listSereServBillSub = new HisSereServBillManager(param).Get(sereServBillFilter);
                            listHisSereServBill.AddRange(listSereServBillSub);
                            listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                        }
                    }
                }
            //Nếu lọc các dịch vụ đã thanh toán
                if (filter.HAS_BILL == true)
                {
                    listHisSereServ = listHisSereServ.Where(o => listHisSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                }
              
                #region cac danh sach phu tro

                //Dinh muc thuoc hao phi
                HisServiceMetyFilterQuery serviceMetyFilter = new HisServiceMetyFilterQuery();
                serviceMetyFilter.IS_ACTIVE = 1;
                listHisServiceMety = new HisServiceMetyManager().Get(serviceMetyFilter);

                //Dinh muc vat tu hao phi
                HisServiceMatyFilterQuery serviceMatyFilter = new HisServiceMatyFilterQuery();
                serviceMatyFilter.IS_ACTIVE = 1;
                listHisServiceMaty = new HisServiceMatyManager().Get(serviceMatyFilter);
                //Vật tư
                HisMaterialTypeFilterQuery materialTypeFilter = new HisMaterialTypeFilterQuery()
                {
                    IS_ACTIVE = 1
                };
                var materialType = new HisMaterialTypeManager(paramGet).Get(materialTypeFilter);
                //Dinh muc Hoa chat hao phi
                if (IsNotNullOrEmpty(listHisServiceMaty) && IsNotNullOrEmpty(materialType))
                {
                    listHisServiceChty = listHisServiceMaty.Where(o => materialType.Exists(p => p.ID == o.MATERIAL_TYPE_ID && p.IS_CHEMICAL_SUBSTANCE == 1)).ToList();
                    listHisServiceMaty = listHisServiceMaty.Where(o => materialType.Exists(p => p.ID == o.MATERIAL_TYPE_ID && p.IS_CHEMICAL_SUBSTANCE != 1)).ToList();
                 
                }

                //nhóm loại báo cáo
                HisServiceRetyCatViewFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                ServiceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00606";
                ListHisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(ServiceRetyCatFilter);
                
                //Danh sách phòng khám
                HisExecuteRoomFilterQuery HisExecuteRoomfilter = new HisExecuteRoomFilterQuery();
                HisExecuteRoomfilter.IS_EXAM = true;
                ListHisExecuteRoom = new HisExecuteRoomManager().Get(HisExecuteRoomfilter);
                #endregion
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
            var result = true;
            listRdo.Clear();
            try
            {
                listRdo.Clear();
                if (IsNotNullOrEmpty(listHisSereServ))
                {
                    var groupByDepartmentId = listHisSereServ.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList();
                    foreach (var item in groupByDepartmentId)
                    {
                        List<HIS_SERE_SERV> listSub = item.ToList<HIS_SERE_SERV>();
                        Mrs00606RDO rdo = new Mrs00606RDO();
                        rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==item.First().TDL_REQUEST_DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        if (ListHisServiceRetyCat.Count > 0)
                        {
                            rdo.DIC_TREAT = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Select(r => r.TDL_TREATMENT_ID).Distinct().Count());
                            rdo.DIC_AMOUNT = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                            rdo.DIC_TOTAL_PRICE = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                            rdo.DIC_EXPEND = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => SumExpend(q.ToList()));

                        }
                        rdo.DIC_TYPE_TREAT = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t=>t.ID==o.TDL_SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => q.Select(r => r.TDL_TREATMENT_ID).Distinct().Count());
                        rdo.DIC_TYPE_AMOUNT = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_TYPE_TOTAL_PRICE = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                        rdo.DIC_TYPE_EXPEND = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => SumExpend(q.ToList()));
                        listRdo.Add(rdo);
                    }
                    var groupByRoomId = listHisSereServ.GroupBy(o => o.TDL_REQUEST_ROOM_ID).ToList();
                    foreach (var item in groupByRoomId)
                    {
                        if (!ListHisExecuteRoom.Exists(o => o.ROOM_ID == item.Key))
                        {
                            continue;
                        }
                        List<HIS_SERE_SERV> listSub = item.ToList<HIS_SERE_SERV>();
                        Mrs00606RDO rdo = new Mrs00606RDO();
                        rdo.DEPARTMENT_NAME = (ListHisExecuteRoom.FirstOrDefault(o => o.ROOM_ID == item.First().TDL_REQUEST_ROOM_ID) ?? new HIS_EXECUTE_ROOM()).EXECUTE_ROOM_NAME;
                        if (ListHisServiceRetyCat.Count > 0)
                        {
                            rdo.DIC_TREAT = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Select(r => r.TDL_TREATMENT_ID).Distinct().Count());
                            rdo.DIC_AMOUNT = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                            rdo.DIC_TOTAL_PRICE = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                            rdo.DIC_EXPEND = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => SumExpend(q.ToList()));

                        }
                        rdo.DIC_TYPE_TREAT = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => q.Select(r => r.TDL_TREATMENT_ID).Distinct().Count());
                        rdo.DIC_TYPE_AMOUNT = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_TYPE_TOTAL_PRICE = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                        rdo.DIC_TYPE_EXPEND = listSub.GroupBy(o => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(t => t.ID == o.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE).ToDictionary(p => p.Key, q => SumExpend(q.ToList()));
                        listRdoExamRoom.Add(rdo);
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

        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return (listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "";

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }


        private decimal SumExpend(List<HIS_SERE_SERV> ss)
        {
            decimal result = 0;
            try
            {
                foreach (var item in ss)
                {
                    result += listHisServiceMety.Where(o => o.SERVICE_ID == item.SERVICE_ID).Sum(s => (s.EXPEND_PRICE ?? 0) * s.EXPEND_AMOUNT);
                    result += listHisServiceMaty.Where(o => o.SERVICE_ID == item.SERVICE_ID).Sum(s => (s.EXPEND_PRICE ?? 0) * s.EXPEND_AMOUNT);
                    result += listHisServiceChty.Where(o => o.SERVICE_ID == item.SERVICE_ID).Sum(s => (s.EXPEND_PRICE ?? 0) * s.EXPEND_AMOUNT);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return 0;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_FROM ?? filter.FINISH_TIME_FROM ?? filter.TRANSACTION_TIME_FROM??0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_TO ?? filter.FINISH_TIME_TO ?? filter.TRANSACTION_TIME_TO ?? 0));
            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "ReportRoomExam", listRdoExamRoom);
        }
    }
}
