using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00585;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisServiceRetyCat;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00585
{
    public class Mrs00585Processor : AbstractProcessor
    {
        private List<Mrs00585RDO> ListRdo = new List<Mrs00585RDO>();
        private List<Mrs00585RDO> ListRdoPatient = new List<Mrs00585RDO>();
        private List<Mrs00585RDO> ListRdoTreatment = new List<Mrs00585RDO>();
        private List<Mrs00585RDO> ListRdoDepartment = new List<Mrs00585RDO>();
        private List<HIS_TRANSACTION> listHisTransaction = new List<HIS_TRANSACTION>();
        Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety = new Dictionary<long, List<HIS_SERVICE_METY>>();
        Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty = new Dictionary<long, List<HIS_SERVICE_MATY>>();
        Mrs00585Filter filter = null;
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_EXECUTE_ROOM> listHisRoom = new List<HIS_EXECUTE_ROOM>();
        string thisReportTypeCode = "";

        List<V_HIS_SERE_SERV> listHisSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV> listHisSereServFee = new List<V_HIS_SERE_SERV>();
        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
		
        public Mrs00585Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00585Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00585Filter)this.reportFilter;
            try
            {
                //Danh sach phong
                HisExecuteRoomFilterQuery listHisRoomfilter = new HisExecuteRoomFilterQuery();
                listHisRoom = new HisExecuteRoomManager(new CommonParam()).Get(listHisRoomfilter);
                Inventec.Common.Logging.LogSystem.Info("listHisRoom" + listHisRoom.Count);
                // Ho so dieu tri
                HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                HisTransactionfilter.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                HisTransactionfilter.TRANSACTION_TIME_TO = filter.TIME_TO;
                HisTransactionfilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                HisTransactionfilter.IS_CANCEL = false;
                HisTransactionfilter.HAS_SALL_TYPE = false;
                listHisTransaction = new HisTransactionManager(new CommonParam()).Get(HisTransactionfilter);
                var treatmentIds = listHisTransaction.Select(o => o.TREATMENT_ID??0).Distinct().ToList();
                // dien dieu tri cuoi cung
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = Ids;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ID";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ASC";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub GetView null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    }
                }
                lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                //Lay danh sach BN theo dien dieu tri
                if (filter.TREATMENT_TYPE_ID != null)
                    treatmentIds = treatmentIds.Where(o => lastHisPatientTypeAlter.Exists(p => p.TREATMENT_ID == o && p.TREATMENT_TYPE_ID == filter.TREATMENT_TYPE_ID)).ToList();

                if (filter.TREATMENT_TYPE_IDs != null)
                    treatmentIds = treatmentIds.Where(o => lastHisPatientTypeAlter.Exists(p => p.TREATMENT_ID == o &&  filter.TREATMENT_TYPE_IDs.Contains(p.TREATMENT_TYPE_ID))).ToList();

                // sere_serv_bill tuong ung
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
                        sereServBillFilter.TDL_TREATMENT_IDs = Ids;
                        var listSereServBillSub = new HisSereServBillManager(param).Get(sereServBillFilter);
                        if (listSereServBillSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listSereServBillSub Get null");
                        else
                            listHisSereServBill.AddRange(listSereServBillSub);
                    }
                }
                listHisSereServBill = listHisSereServBill.Where(o => listHisTransaction.Exists(p => p.ID == o.BILL_ID)).ToList();
                // sere_serv tuong ung
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                        sereServFilter.TREATMENT_IDs = Ids;
                        //sereServFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                        var listSereServSub = new HisSereServManager(param).GetView(sereServFilter);
                        if (listSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listSereServViewSub Get null");
                        else
                        listHisSereServ.AddRange(listSereServSub);
                    }
                }
                listHisSereServFee = listHisSereServ.Where(o => o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                listHisSereServ = listHisSereServ.Where(o => o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
              
                if (IsNotNullOrEmpty(listHisSereServBill))
                {
                    listHisSereServ = listHisSereServ.Where(o => listHisSereServBill.Exists(p=>p.SERE_SERV_ID==o.ID)).ToList();
                }
                //Dinh muc thuoc hao phi
                List<HIS_SERVICE_METY> listServiceMety = new List<HIS_SERVICE_METY>();
                HisServiceMetyFilterQuery serviceMetyFilter = new HisServiceMetyFilterQuery();
                listServiceMety = new HisServiceMetyManager().Get(serviceMetyFilter);

                //neu la kham thi lay phong chi dinh la phong thuc hien
                foreach (var item in listHisSereServ)
                {
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                        item.REQUEST_DEPARTMENT_CODE = item.EXECUTE_DEPARTMENT_CODE;
                        item.REQUEST_ROOM_CODE = item.EXECUTE_ROOM_CODE;
                        item.REQUEST_DEPARTMENT_NAME = item.EXECUTE_DEPARTMENT_NAME;
                        item.REQUEST_ROOM_NAME = item.EXECUTE_ROOM_NAME;
                    }

                }
                foreach (var item in listServiceMety)
                {
                    if (!dicServiceMety.ContainsKey(item.SERVICE_ID)) dicServiceMety[item.SERVICE_ID] = new List<HIS_SERVICE_METY>();
                    dicServiceMety[item.SERVICE_ID].Add(item);
                }

                //Dinh muc vat tu hao phi
                List<HIS_SERVICE_MATY> listServiceMaty = new List<HIS_SERVICE_MATY>();
                HisServiceMatyFilterQuery serviceMatyFilter = new HisServiceMatyFilterQuery();
                listServiceMaty = new HisServiceMatyManager().Get(serviceMatyFilter);
                foreach (var item in listServiceMaty)
                {
                    if (!dicServiceMaty.ContainsKey(item.SERVICE_ID)) dicServiceMaty[item.SERVICE_ID] = new List<HIS_SERVICE_MATY>();
                    dicServiceMaty[item.SERVICE_ID].Add(item);
                }
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatViewfilter = new HisServiceRetyCatViewFilterQuery();
                HisServiceRetyCatViewfilter.REPORT_TYPE_CODE__EXACT = "MRS00556";
                listServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatViewfilter);

                //lay loc theo khoa
                if (filter.DEPARTMENT_ID != null)
                {
                    listHisSereServ = listHisSereServ.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                    listHisSereServFee = listHisSereServFee.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                }
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
            bool result = false;
            try
            {
                ListRdo = (from r in listHisSereServ select new Mrs00585RDO(r, dicServiceMety, dicServiceMaty, listServiceRetyCat ?? new List<V_HIS_SERVICE_RETY_CAT>(), this.filter, lastHisPatientTypeAlter, listHisRoom, listHisTransaction)).ToList();

                ListRdo = ListRdo.Where(o => o.TOTAL_PRICE > 0).OrderByDescending(o => o.ROOM_NUM_ORDER).ToList();
                GroupByTreatment();
                GroupByPatient();
                AddFee();
                GroupByRequestDepartmentID();
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00585RDO>();
                result = false;
            }
            return result;
        }

        private void AddFee()
        {
            if (listHisSereServFee != null)
            {
                foreach (var item in ListRdoPatient)
                {
                    item.TOTAL_PRICE_FEE = listHisSereServFee.Where(o => o.TDL_TREATMENT_ID == item.TDL_TREATMENT_ID).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                }
            }
        }

       
        private void GroupByTreatment()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.TDL_REQUEST_DEPARTMENT_ID }).ToList();

                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                Decimal sum = 0;
                Mrs00585RDO rdo;
                List<Mrs00585RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00585RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00585RDO();
                    listSub = item.ToList<Mrs00585RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE") || field.Name.Contains("_AM_"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoTreatment.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private void GroupByPatient()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID}).ToList();

                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                Decimal sum = 0;
                Mrs00585RDO rdo;
                List<Mrs00585RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00585RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00585RDO();
                    listSub = item.ToList<Mrs00585RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE") || field.Name.Contains("_AM_"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoPatient.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private void GroupByRequestDepartmentID()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_REQUEST_DEPARTMENT_ID }).ToList();

                Decimal sum = 0;
                Mrs00585RDO rdo;
                List<Mrs00585RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00585RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00585RDO();
                    listSub = item.ToList<Mrs00585RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE") || field.Name.Contains("_AM_"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoDepartment.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00585RDO IsMeaningful(List<Mrs00585RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00585RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("TREATMENT_TYPE_NAME", (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == filter.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME);

            objectTag.AddObjectData(store, "Report", ListRdoPatient.OrderBy(p => p.TDL_TREATMENT_ID).ToList());
            objectTag.AddObjectData(store, "ReportTreatment", ListRdoTreatment.OrderBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ThenBy(p => p.TDL_TREATMENT_ID).ToList());
           
            objectTag.AddObjectData(store, "ReportDepartment", ListRdoDepartment);
        }

    }

}
