using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisBranch;
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
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientTypeAlter;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisRoom;
using Inventec.Common.Logging;
using System.Text.RegularExpressions;
using MOS.MANAGER.HisServiceRetyCat;


namespace MRS.Processor.Mrs00296
{
    class Mrs00296Processor : AbstractProcessor
    {
        Mrs00296Filter castFilter = null;

        List<Mrs00296RDO> listRdo = new List<Mrs00296RDO>();
        List<Mrs00296RDO> listRdo1 = new List<Mrs00296RDO>();
        List<Mrs00296RDO> listSereServRdo = new List<Mrs00296RDO>();
        List<Mrs00296RDO> listServcieTypeRdo = new List<Mrs00296RDO>();
        List<Mrs00296RDO> listGroupRdo = new List<Mrs00296RDO>();
        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        List<HIS_AREA> listArea = new List<HIS_AREA>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        Dictionary<long, string> dicSvCode = new Dictionary<long, string>();
        Dictionary<long, string> dicPrCode = new Dictionary<long, string>();
        Dictionary<long, string> dicSvName = new Dictionary<long, string>();
        Dictionary<long, string> dicPrName = new Dictionary<long, string>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, string> dicCateCode = new Dictionary<long, string>();
        Dictionary<long, string> dicCateName = new Dictionary<long, string>();
        List<long> departmentKD = new List<long>();
        List<long> departmentKLS = new List<long>();
        List<long> departmentKKB = new List<long>();

        List<long> Holidays = new List<long>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        List<HIS_PATIENT_CLASSIFY> listClassify = new List<HIS_PATIENT_CLASSIFY>();
        public Mrs00296Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        string Branch_Name = "";

        public override Type FilterType()
        {
            return typeof(Mrs00296Filter);
        }

        protected override bool GetData()///
        {
            bool valid = true;
            try
            {
                this.castFilter = (Mrs00296Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("castFilter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
                listSereServRdo = new ManagerSql().GetSereServDO(this.castFilter, listRoom);

                GetHolidays();
                if (castFilter.IS_HOLIDAYS == true && castFilter.IS_NOT_HOLIDAYS != true)
                {
                    listSereServRdo = listSereServRdo.Where(o => Holidays.Contains(o.TRANSACTION_DATE)).ToList();
                }
                if (castFilter.IS_NOT_HOLIDAYS == true && castFilter.IS_HOLIDAYS != true)
                {
                    listSereServRdo = listSereServRdo.Where(o => !Holidays.Contains(o.TRANSACTION_DATE)).ToList();
                }
                listCashierRoom = new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery());
                listArea = new ManagerSql().GetArea();
                listService = new HisServiceManager().Get(new HisServiceFilterQuery());
                dicSvCode = listService.ToDictionary(o => o.ID, p => p.SERVICE_CODE);
                dicPrCode = listService.ToDictionary(o => o.ID, p => dicSvCode.ContainsKey(p.PARENT_ID ?? 0) ? dicSvCode[p.PARENT_ID ?? 0] : "");
                dicSvName = listService.ToDictionary(o => o.ID, p => p.SERVICE_NAME);
                dicPrName = listService.ToDictionary(o => o.ID, p => dicSvName.ContainsKey(p.PARENT_ID ?? 0) ? dicSvName[p.PARENT_ID ?? 0] : "");
                listServiceRetyCat = new HisServiceRetyCatManager().GetView(new HisServiceRetyCatViewFilterQuery() { REPORT_TYPE_CODE__EXACT = "MRS00296" });
                dicCateCode = listServiceRetyCat.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.First().CATEGORY_CODE);
                dicCateName = listServiceRetyCat.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.First().CATEGORY_NAME);
                GetDepartmentType();

                //thêm đối tượng chi tiết
                GetPatientClassify();

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu bao cao MRS00296: V_HIS_BILL, V_HIS_DEPOSIT_2, V_HIS_REPAY_1, V_HIS_CASHIER_ROOM");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void GetPatientClassify()
        {
            listClassify = new ManagerSql().GetPatientClassify() ?? new List<HIS_PATIENT_CLASSIFY>();
        }

        private void GetHolidays()
        {

            try
            {
                List<string> strHolidays = new List<string>() { "1_1", "30_4", "1_5", "2_9" };
                List<string> strLunarHolidays = new List<string>() { "10_3", "30_12", "29_12", "1_1", "2_1", "3_1" };
                List<DateTime> dateHolidays = new List<DateTime>();
                //List<string> STRHOLIDAYSNEW = new List<string>();
                if (((Mrs00296Filter)this.reportFilter).STR_HOLIDAYS_NEWs != null)
                {
                    string[] a = ((Mrs00296Filter)this.reportFilter).STR_HOLIDAYS_NEWs.Split(',');
                    strHolidays.AddRange(a);
                }
                if (((Mrs00296Filter)this.reportFilter).STR_LUNAR_HOLIDAYS_NEWs != null)
                {
                    string[] b = ((Mrs00296Filter)this.reportFilter).STR_LUNAR_HOLIDAYS_NEWs.Split(',');
                    strLunarHolidays.AddRange(b);
                }




                int year = DateTime.Today.Year;
                DateTime startDate = new DateTime(year - 1, 1, 1);
                DateTime endDate = new DateTime(year + 1, 1, 1);
                for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
                {
                    if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
                    //var vcal = new convertSolar2Lunar();
                    //int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    //var tempDay = arr[0] + "_" + arr[1];
                    if (strHolidays.Contains(string.Format("{0}_{1}", i.Day, i.Month)))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }

                    var vcal = new convertSolar2Lunar();
                    int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    var tempDay = arr[0] + "_" + arr[1];
                    if (strLunarHolidays.Contains(tempDay))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        private void GetDepartmentType()
        {
            foreach (var item in HisDepartmentCFG.DEPARTMENTs)
            {
                if (listRoom.Exists(o => o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && item.ID == o.DEPARTMENT_ID))
                {
                    departmentKKB.Add(item.ID);
                }
                if (!listRoom.Exists(o => o.ROOM_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__KHO && item.ID == o.DEPARTMENT_ID))
                {
                    departmentKD.Add(item.ID);
                }
                if (listRoom.Exists(o => o.ROOM_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__BUONG && item.ID == o.DEPARTMENT_ID))
                {
                    departmentKLS.Add(item.ID);
                }
            }
        }

        private bool filterDepartment(Mrs00296RDO o)
        {
            bool result = true;//
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    if (o.TDL_REQUEST_DEPARTMENT_ID != castFilter.DEPARTMENT_ID.Value)
                        result = false;
                }
                if (castFilter.EXAM_ROOM_ID.HasValue)
                {
                    if (o.TDL_REQUEST_ROOM_ID != castFilter.EXAM_ROOM_ID.Value)
                        result = false;
                }
                if (castFilter.EXAM_ROOM_IDs != null)
                {
                    if (!castFilter.EXAM_ROOM_IDs.Contains(o.TDL_REQUEST_ROOM_ID))
                        result = false;
                }
                if (castFilter.AREA_IDs != null)
                {
                    if (!castFilter.AREA_IDs.Contains(o.AREA_ID ?? 0))
                        result = false;
                }
                if (castFilter.REQUEST_DEPARTMENT_IDs != null)
                {
                    if (!castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID))
                        result = false;
                }
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
            bool valid = true;
            try
            {
                LogSystem.Info("COUNT: " + listSereServRdo.Count);
                if (listSereServRdo != null && listSereServRdo.Count > 0)
                {
                    //them thong tin id phong, id khoa, id khu vuc
                    AddInforIdArea(ref listSereServRdo);
                    //loc theo phong, khoa, khu vuc
                    listSereServRdo = listSereServRdo.Where(o => filterDepartment(o)).ToList();
                    if (castFilter.HOUR_TIME_TO != null && castFilter.HOUR_TIME_FROM != null)
                    {
                        listSereServRdo = ProcessByTransactionTime(ref listSereServRdo);
                    }
                    //them thong tin khoa phong, khu vuc, nhom cha, ma va ten dich vu, phong thu ngan
                    //AddInForMore(ref listSereServRdo);
                    ProcessBranchById();

                    ProcessGroup(ref listSereServRdo);


                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private List<Mrs00296RDO> ProcessByTransactionTime(ref List<Mrs00296RDO> listSereServRdo)
        {
            List<Mrs00296RDO> result = new List<Mrs00296RDO>();
            foreach (var item in listSereServRdo)
            {

                if (item.TRANSACTION_TIME < item.TRANSACTION_DATE + castFilter.HOUR_TIME_FROM || item.TRANSACTION_TIME > item.TRANSACTION_DATE + castFilter.HOUR_TIME_TO)
                {
                    continue;
                }

                result.Add(item);
            }
            return result;
        }

        private List<Mrs00296RDO> GroupByDemension(List<Mrs00296RDO> listSereServRdo)
        {
            List<Mrs00296RDO> result = new List<Mrs00296RDO>();
            string KeyGroupSS = "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}_{16}_{17}";
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_SS") && this.dicDataFilter["KEY_GROUP_SS"] != null)
            {
                KeyGroupSS = this.dicDataFilter["KEY_GROUP_SS"].ToString();
            }
            var groupByDemension = listSereServRdo.GroupBy(o => string.Format(KeyGroupSS, o.TDL_REQUEST_ROOM_ID,
                o.TDL_REQUEST_DEPARTMENT_ID,
                o.AREA_ID,
                o.CONSULTANT_LOGINNAME,
                o.TDL_PATIENT_TYPE_ID,
                o.TDL_TREATMENT_CODE,
                o.SERVICE_ID,
                o.ICD_CODE,
                o.TRANSACTION_NUM_ORDER,
                o.TRANSACTION_DATE,
                o.PRICE,
                o.TRANSACTION_ID,
                o.TDL_EXECUTE_DEPARTMENT_ID,
                o.TDL_EXECUTE_ROOM_ID,
                o.SERVICE_TYPE_ID,
                o.HEIN_RATIO,
                o.EXECUTE_TIME,
                o.SS_PATIENT_TYPE_ID)).ToList();
            foreach (var group in groupByDemension)
            {
                Mrs00296RDO rdo = new Mrs00296RDO()
                   {

                       SERVICE_ID = group.First().SERVICE_ID,
                       CASHIER_LOGINNAME = group.First().CASHIER_LOGINNAME,
                       HEIN_RATIO = group.First().HEIN_RATIO,
                       CASHIER_USERNAME = group.First().CASHIER_USERNAME,
                       REQUEST_USERNAME = group.First().REQUEST_USERNAME,
                       EXECUTE_USERNAME = group.First().EXECUTE_USERNAME,
                       CONSULTANT_LOGINNAME = group.First().CONSULTANT_LOGINNAME,
                       CONSULTANT_USERNAME = group.First().CONSULTANT_USERNAME,
                       PAY_FORM_ID = group.First().PAY_FORM_ID,
                       SERVICE_TYPE_ID = group.First().SERVICE_TYPE_ID,
                       CATEGORY_CODE = dicCateCode.ContainsKey(group.First().SERVICE_ID)?dicCateCode[group.First().SERVICE_ID]:"",
                       CATEGORY_NAME = dicCateName.ContainsKey(group.First().SERVICE_ID) ? dicCateName[group.First().SERVICE_ID] : "",
                       TDL_TREATMENT_CODE = group.First().TDL_TREATMENT_CODE,
                       TDL_PATIENT_CODE = group.First().TDL_PATIENT_CODE,
                       TDL_PATIENT_NAME = group.First().TDL_PATIENT_NAME,
                       TDL_PATIENT_GENDER_ID = group.First().TDL_PATIENT_GENDER_ID,
                       TDL_PATIENT_DOB = group.First().TDL_PATIENT_DOB,
                       ACCOUNT_BOOK_CODE = group.First().ACCOUNT_BOOK_CODE,
                       ACCOUNT_BOOK_NAME = group.First().ACCOUNT_BOOK_NAME,
                       TRANSACTION_NUM_ORDER = group.First().TRANSACTION_NUM_ORDER,
                       TRANSACTION_DATE = group.First().TRANSACTION_DATE,
                       TRANSACTION_TIME = group.First().TRANSACTION_TIME,
                       TDL_REQUEST_ROOM_ID = group.First().TDL_REQUEST_ROOM_ID,
                       TDL_REQUEST_DEPARTMENT_ID = group.First().TDL_REQUEST_DEPARTMENT_ID,
                       EXECUTE_TIME = group.First().EXECUTE_TIME,
                       TDL_PATIENT_CLASSIFY_ID = group.First().TDL_PATIENT_CLASSIFY_ID,
                       PATIENT_CLASSIFY_CODE = group.First().PATIENT_CLASSIFY_CODE,
                       PATIENT_CLASSIFY_NAME = group.First().PATIENT_CLASSIFY_NAME,
                       ICD_CODE = group.First().ICD_CODE,
                       ICD_NAME = group.First().ICD_NAME,
                       TDL_PATIENT_TYPE_ID = group.First().TDL_PATIENT_TYPE_ID,
                       SS_PATIENT_TYPE_ID = group.First().SS_PATIENT_TYPE_ID,
                       AREA_ID = group.First().AREA_ID,
                       TDL_EXECUTE_DEPARTMENT_ID = group.First().TDL_EXECUTE_DEPARTMENT_ID,
                       TDL_EXECUTE_ROOM_ID = group.First().TDL_EXECUTE_ROOM_ID,
                       PRICE = group.First().PRICE,
                       CASHIER_ROOM_ID = group.First().CASHIER_ROOM_ID,
                       AMOUNT_DEPOSIT_BILL = group.Sum(s => s.AMOUNT_DEPOSIT_BILL),
                       AMOUNT_REPAY = group.Sum(s => s.AMOUNT_REPAY),
                       TOTAL_DEPOSIT_BILL_PRICE = group.Sum(s => s.TOTAL_DEPOSIT_BILL_PRICE),
                       TOTAL_REPAY_PRICE = group.Sum(s => s.TOTAL_REPAY_PRICE),
                       VIR_TOTAL_PATIENT_PRICE_BHYT = group.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT)
                   };
                rdo.DIC_PRICE = new Dictionary<string, decimal>();
                rdo.DIC_PRICE_TUTRA = new Dictionary<string, decimal>();
                foreach (var item in (castFilter.KEY_PRICEs??"{0}").Split(';'))
                {
                    var dicPrice = group.GroupBy(o => string.Format(item, o.SERVICE_TYPE_ID, dicPrCode[o.SERVICE_ID], o.CATEGORY_CODE, dicSvCode[o.SERVICE_ID], o.SS_PATIENT_TYPE_ID, o.PRIMARY_PATIENT_TYPE_ID)).ToDictionary(p => p.Key, q => q.Sum(s => (s.TOTAL_DEPOSIT_BILL_PRICE) - (s.TOTAL_REPAY_PRICE)));
                    foreach (var dp in dicPrice)
                    {
                        rdo.DIC_PRICE.Add(dp.Key, dp.Value);
                    } var dicPriceTuTra = group.GroupBy(o => string.Format(item, o.SERVICE_TYPE_ID, dicPrCode[o.SERVICE_ID], o.CATEGORY_CODE, dicSvCode[o.SERVICE_ID], o.SS_PATIENT_TYPE_ID, o.PRIMARY_PATIENT_TYPE_ID)).ToDictionary(p => p.Key, q => q.Sum(s => (s.TOTAL_DEPOSIT_BILL_PRICE) - (s.TOTAL_REPAY_PRICE) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                    foreach (var dp in dicPriceTuTra)
                    {
                        rdo.DIC_PRICE_TUTRA.Add(dp.Key, dp.Value);
                    }
                }
                
                result.Add(rdo);
            }

            return result;
        }
        private string SvCode(long serviceId, Dictionary<long, HIS_SERVICE> dicSv)
        {
            if (dicSv.ContainsKey(serviceId))
            {
                var sv = dicSv[serviceId];
                if (sv != null && sv.SERVICE_CODE != null)
                {
                    return sv.SERVICE_CODE;
                }
            }
            return "NONE";
        }

        private string ParentCode(long serviceId, Dictionary<long, HIS_SERVICE> dicSv)
        {
            if (dicSv.ContainsKey(serviceId))
            {
                var sv = dicSv[serviceId];
                if (sv != null && sv.PARENT_ID != null)
                {
                    if (dicSv.ContainsKey(sv.PARENT_ID ?? 0))
                    {
                        var pr = dicSv[sv.PARENT_ID ?? 0];
                        if (pr != null && pr.SERVICE_CODE != null)
                        {
                            return pr.SERVICE_CODE;
                        }
                    }
                }
            }
            return "NONE";
        }


        private void AddInforIdArea(ref List<Mrs00296RDO> listSereServRdo)
        {
            foreach (var item in listSereServRdo)
            {
                var exRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                var requestRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                {
                    item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                    item.TDL_REQUEST_DEPARTMENT_ID = exRoom.DEPARTMENT_ID;
                    item.AREA_ID = exRoom.AREA_ID ?? 0;
                }

                else
                {
                    item.TDL_REQUEST_ROOM_ID = requestRoom.ID;
                    item.TDL_REQUEST_DEPARTMENT_ID = requestRoom.DEPARTMENT_ID;
                    item.AREA_ID = requestRoom.AREA_ID ?? 0;

                }
            }
        }

        private void AddInForMore(ref List<Mrs00296RDO> listSereServRdo)
        {
            foreach (var item in listSereServRdo)
            {

                HIS_SERVICE_TYPE serviceType = MANAGER.Config.HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                if (serviceType != null)
                {
                    if (string.IsNullOrWhiteSpace(item.SERVICE_TYPE_NAME))
                    {
                        item.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        item.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                    }
                }
                var service = listService.FirstOrDefault(o => o.ID == item.SERVICE_ID) ?? new HIS_SERVICE();
                if (service != null)
                {
                    item.SERVICE_CODE = service.SERVICE_CODE;
                    item.SERVICE_NAME = service.SERVICE_NAME;
                    var parent = listService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                    if (parent != null)
                    {
                        item.PARENT_SERVICE_CODE = parent.SERVICE_CODE;
                        item.PARENT_SERVICE_NAME = parent.SERVICE_NAME;
                        item.MEDIUM_SERVICE_CODE = parent.SERVICE_CODE;
                        item.MEDIUM_SERVICE_NAME = parent.SERVICE_NAME;
                        var par = listService.FirstOrDefault(o => o.ID == parent.PARENT_ID);
                        if (par != null)
                        {
                            item.PARENT_SERVICE_CODE = par.SERVICE_CODE;
                            item.PARENT_SERVICE_NAME = par.SERVICE_NAME;
                        }
                    }
                }
                //doi tuong bn
                HIS_PATIENT_TYPE patientType = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == item.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                if (patientType != null)
                {
                    item.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    item.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                }

                //doi tuong thanh toan
                HIS_PATIENT_TYPE sspatientType = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == item.SS_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                if (sspatientType != null)
                {
                    item.SS_PATIENT_TYPE_CODE = sspatientType.PATIENT_TYPE_CODE;
                    item.SS_PATIENT_TYPE_NAME = sspatientType.PATIENT_TYPE_NAME;
                }

                item.TDL_PATIENT_AGE = RDOCommon.CalculateAge(item.TDL_PATIENT_DOB ?? 0); //tuoi
                item.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXECUTE_TIME ?? 0);

                //sua lai phong chi dinh de tinh doanh thu
                var requestRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                item.TDL_REQUEST_ROOM_ID = requestRoom.ID;
                item.TDL_REQUEST_DEPARTMENT_ID = requestRoom.DEPARTMENT_ID;
                item.TDL_REQUEST_ROOM_CODE = requestRoom.ROOM_CODE;
                item.TDL_REQUEST_ROOM_NAME = requestRoom.ROOM_NAME;
                item.TDL_REQUEST_DEPARTMENT_CODE = requestRoom.DEPARTMENT_CODE;
                item.TDL_REQUEST_DEPARTMENT_NAME = requestRoom.DEPARTMENT_NAME;
                item.AREA_ID = requestRoom.AREA_ID ?? 0;


                item.CASHIER_ROOM_NAME = (listCashierRoom.FirstOrDefault(o => item.CASHIER_ROOM_ID == o.ID) ?? new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME;
                var payForm = HisPayFormCFG.ListPayForm.FirstOrDefault(o => item.PAY_FORM_ID == o.ID) ?? new HIS_PAY_FORM();
                if (payForm != null)
                {
                    item.PAY_FORM_CODE = payForm.PAY_FORM_CODE;
                    item.PAY_FORM_NAME = payForm.PAY_FORM_NAME;

                }
                if (castFilter.IS_ONLY_SPLIT_CASH == true)
                {
                    if (item.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM && item.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TMCK && item.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TMQT)
                    {
                        item.PAY_FORM_ID = 0;
                        item.PAY_FORM_CODE = "HTK";
                        item.PAY_FORM_NAME = "Các hình thức khác";
                    }
                }

                var executeRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                if (executeRoom != null)
                {
                    item.TDL_EXECUTE_ROOM_CODE = executeRoom.ROOM_CODE;
                    item.TDL_EXECUTE_ROOM_NAME = executeRoom.ROOM_NAME;
                }
                var executeDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_DEPARTMENT_ID);
                if (executeDepartment != null)
                {
                    item.EXDEPA_NUM_ORRDER = executeDepartment.NUM_ORDER ?? 9999;
                    item.TDL_EXECUTE_DEPARTMENT_CODE = executeDepartment.DEPARTMENT_CODE;
                    item.TDL_EXECUTE_DEPARTMENT_NAME = executeDepartment.DEPARTMENT_NAME;
                }
                var area = this.listArea.FirstOrDefault(o => o.ID == item.AREA_ID);
                if (area != null)
                {
                    item.AREA_CODE = area.AREA_CODE;
                    item.AREA_NAME = area.AREA_NAME;
                }
                var patientClassify = this.listClassify.FirstOrDefault(o => o.ID == item.TDL_PATIENT_CLASSIFY_ID);
                if (patientClassify != null)
                {
                    item.PATIENT_CLASSIFY_CODE = patientClassify.PATIENT_CLASSIFY_CODE;
                    item.PATIENT_CLASSIFY_NAME = patientClassify.PATIENT_CLASSIFY_NAME;
                }
            }
        }
        //void ProcessListRdo()
        //{
        //    if (listSereServRdo.Count > 0)
        //    {

        //        listServcieTypeRdo = listSereServRdo.GroupBy(g => g.SERVICE_TYPE_CODE).Select(s =>
        //            new Mrs00296RDO()
        //            {
        //                SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
        //                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
        //                AMOUNT_DEPOSIT_BILL = s.Sum(s1 => s1.AMOUNT_DEPOSIT_BILL),
        //                AMOUNT_REPAY = s.Sum(s2 => s2.AMOUNT_REPAY),
        //                TOTAL_DEPOSIT_BILL_PRICE = s.Sum(s3 => s3.TOTAL_DEPOSIT_BILL_PRICE),
        //                TOTAL_REPAY_PRICE = s.Sum(s4 => s4.TOTAL_REPAY_PRICE)
        //            }).ToList();


        //    }
        //}

        void ProcessGroup(ref List<Mrs00296RDO> listRdoNew)
        {
            if (listRdoNew.Count > 0)
            {
                listSereServRdo = GroupByDemension(listRdoNew);
                AddInForMore(ref listSereServRdo);


                listServcieTypeRdo = listSereServRdo.GroupBy(g => g.SERVICE_TYPE_ID).Select(s =>
                    new Mrs00296RDO()
                    {
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        AMOUNT_DEPOSIT_BILL = s.Sum(s1 => s1.AMOUNT_DEPOSIT_BILL),
                        AMOUNT_REPAY = s.Sum(s2 => s2.AMOUNT_REPAY),
                        TOTAL_DEPOSIT_BILL_PRICE = s.Sum(s3 => s3.TOTAL_DEPOSIT_BILL_PRICE),
                        TOTAL_REPAY_PRICE = s.Sum(s4 => s4.TOTAL_REPAY_PRICE),
                        VIR_TOTAL_PATIENT_PRICE_BHYT = s.Sum(s5 => s5.VIR_TOTAL_PATIENT_PRICE_BHYT)
                    }).ToList();

                var Groups1 = listSereServRdo.GroupBy(g => new { g.SERVICE_ID, g.PRICE }).ToList();
                foreach (var group in Groups1)
                {
                    var listSub = group.ToList<Mrs00296RDO>();
                    Mrs00296RDO rdo = new Mrs00296RDO();
                    rdo.SERVICE_ID = listSub.First().SERVICE_ID;
                    rdo.SERVICE_CODE = listSub.First().SERVICE_CODE;
                    rdo.SERVICE_NAME = listSub.First().SERVICE_NAME;

                    rdo.TDL_EXECUTE_DEPARTMENT_CODE = listSub.First().TDL_EXECUTE_DEPARTMENT_CODE;
                    rdo.TDL_EXECUTE_DEPARTMENT_NAME = listSub.First().TDL_EXECUTE_DEPARTMENT_NAME;
                    rdo.PARENT_SERVICE_CODE = listSub.First().PARENT_SERVICE_CODE;
                    rdo.PARENT_SERVICE_NAME = listSub.First().PARENT_SERVICE_NAME;
                    rdo.MEDIUM_SERVICE_CODE = listSub.First().MEDIUM_SERVICE_CODE;
                    rdo.MEDIUM_SERVICE_NAME = listSub.First().MEDIUM_SERVICE_NAME;
                    rdo.SERVICE_TYPE_CODE = listSub.First().SERVICE_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = listSub.First().SERVICE_TYPE_NAME;
                    rdo.PRICE = listSub.First().PRICE;
                    rdo.AMOUNT_DEPOSIT_BILL = listSub.Sum(s => s.AMOUNT_DEPOSIT_BILL);
                    rdo.AMOUNT_REPAY = listSub.Sum(s => s.AMOUNT_REPAY);
                    rdo.TOTAL_DEPOSIT_BILL_PRICE = listSub.Sum(s => s.TOTAL_DEPOSIT_BILL_PRICE);
                    rdo.TOTAL_REPAY_PRICE = listSub.Sum(s => s.TOTAL_REPAY_PRICE);
                    rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT);
                    listRdo.Add(rdo);
                    rdo.HEIN_RATIO = listSub.First().HEIN_RATIO;
                    listRdo1.Add(rdo);
                }

            }
        }

        void ProcessBranchById()
        {
            try
            {
                var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID);
                if (branch != null)
                {
                    this.Branch_Name = branch.BRANCH_NAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                var listParentService = listSereServRdo.GroupBy(o => o.SERVICE_TYPE_CODE).Select(p => p.First()).OrderBy(q => q.SERVICE_TYPE_CODE).ToList();
                for (int i = 0; i < 1000; i++)
                {
                    if (i < listParentService.Count)
                    {
                        dicSingleTag.Add(string.Format("SERVICE_TYPE_CODE_{0}", i + 1), listParentService[i].SERVICE_TYPE_CODE);
                        dicSingleTag.Add(string.Format("SERVICE_TYPE_NAME_{0}", i + 1), listParentService[i].SERVICE_TYPE_NAME);
                    }
                }
                listRdo = listRdo.OrderBy(o => o.SERVICE_TYPE_CODE).ThenBy(t => t.SERVICE_CODE).ToList();
                listServcieTypeRdo = listServcieTypeRdo.OrderBy(o => o.SERVICE_TYPE_CODE).ToList();
                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (castFilter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    dicSingleTag.Add("CASHIER_ROOM_NAME", string.Join(" - ", listCashierRoom.Where(o => castFilter.EXACT_CASHIER_ROOM_IDs.Contains(o.ID)).Select(r => r.CASHIER_ROOM_NAME).ToList()));
                }
                dicSingleTag.Add("BRANCH_NAME", this.Branch_Name);
                dicSingleTag.Add("PATIENT_TYPE_NAME", string.Join(" - ", HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs == null || castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).Select(o => o.PATIENT_TYPE_NAME).ToList()));
                dicSingleTag.Add("PATIENT_CLASSIFY_NAME", string.Join(" - ", listClassify.Where(o => castFilter.PATIENT_CLASSIFY_IDs != null && castFilter.PATIENT_CLASSIFY_IDs.Contains(o.ID)).Select(o => o.PATIENT_CLASSIFY_NAME).ToList()));
                objectTag.AddObjectData(store, "ServiceType", listServcieTypeRdo);
                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "parent", listRdo.GroupBy(o => o.PARENT_SERVICE_CODE).Select(q => q.First()).ToList());
                objectTag.AddRelationship(store, "parent", "Report", "PARENT_SERVICE_CODE", "PARENT_SERVICE_CODE");
                List<Mrs00296RDO> listSum = new List<Mrs00296RDO>();
                listSum.Add(new Mrs00296RDO() { AMOUNT_DEPOSIT_BILL = listRdo.Sum(o => o.AMOUNT_DEPOSIT_BILL), AMOUNT_REPAY = listRdo.Sum(o => o.AMOUNT_REPAY) });
                objectTag.AddObjectData(store, "ReportSum", listSum);
                objectTag.AddRelationship(store, "ServiceType", "Report", "SERVICE_TYPE_CODE", "SERVICE_TYPE_CODE");

                objectTag.SetUserFunction(store, "FuncRowNumber", new FuncManyRownumberData());
                objectTag.AddObjectData(store, "ReportDetail", listSereServRdo.OrderByDescending(p => p.PAY_FORM_ID).ToList());
                objectTag.AddObjectData(store, "ReportDetail1", listSereServRdo.Where(p => p.CONSULTANT_LOGINNAME != null).OrderByDescending(p => p.PAY_FORM_ID).ToList());
                objectTag.AddObjectData(store, "ExamRooms", listSereServRdo.GroupBy(o => new { o.TDL_REQUEST_ROOM_ID, o.CASHIER_ROOM_NAME }).Select(p => p.First()).OrderBy(q => q.TDL_REQUEST_ROOM_NAME).ToList());
                objectTag.AddRelationship(store, "ExamRooms", "ReportDetail", new string[] { "TDL_REQUEST_ROOM_ID", "CASHIER_ROOM_NAME" }, new string[] { "TDL_REQUEST_ROOM_ID", "CASHIER_ROOM_NAME" });
                objectTag.AddObjectData(store, "ExamDepartments", listSereServRdo.GroupBy(o => new { o.TDL_REQUEST_DEPARTMENT_ID, o.CASHIER_ROOM_NAME }).Select(p => p.First()).OrderBy(q => q.TDL_REQUEST_DEPARTMENT_NAME).ToList());
                objectTag.AddRelationship(store, "ExamDepartments", "ReportDetail", new string[] { "TDL_REQUEST_DEPARTMENT_ID", "CASHIER_ROOM_NAME" }, new string[] { "TDL_REQUEST_DEPARTMENT_ID", "CASHIER_ROOM_NAME" });

                objectTag.AddObjectData(store, "CashierRoom", listSereServRdo.GroupBy(o => o.CASHIER_ROOM_NAME).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "CashierRoom", "ExamDepartments", "CASHIER_ROOM_NAME", "CASHIER_ROOM_NAME");
                objectTag.AddRelationship(store, "CashierRoom", "ExamRooms", "CASHIER_ROOM_NAME", "CASHIER_ROOM_NAME");

                dicSingleTag.Add("AREA_NAMEs", castFilter.AREA_IDs == null ? "" : string.Join(" - ", (this.listArea ?? new List<HIS_AREA>()).Where(o => castFilter.AREA_IDs.Contains(o.ID)).Select(o => o.AREA_NAME).ToList()));
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAMEs", castFilter.REQUEST_DEPARTMENT_IDs == null ? "" : string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.ID)).Select(o => o.DEPARTMENT_NAME).ToList()));
                objectTag.AddObjectData(store, "ReportDetail2", listSereServRdo.OrderBy(p => p.SERVICE_TYPE_CODE).ThenBy(P => P.SERVICE_CODE).ThenBy(P => P.TDL_PATIENT_CODE).ToList());
                objectTag.AddObjectData(store, "Report1", listRdo1.OrderBy(p => p.TDL_REQUEST_DEPARTMENT_CODE).ThenBy(p => p.SERVICE_NAME).ThenBy(p => p.HEIN_RATIO).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class FuncManyRownumberData : TFlexCelUserFunction
    {
        long ServiceTypeId = 0;
        long num_order = 0;

        public FuncManyRownumberData() { }

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long typeId = Convert.ToInt64(parameters[0]);

                if (ServiceTypeId == typeId)
                {
                    num_order = num_order + 1;
                }
                else
                {
                    ServiceTypeId = typeId;
                    num_order = 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return num_order;
        }
    }
}
