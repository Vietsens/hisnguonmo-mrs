using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
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
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisEmployee;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisOtherPaySource;

namespace MRS.Processor.Mrs00166
{
    public class Mrs00166Processor : AbstractProcessor
    {
        Mrs00166Filter filter = null;
        List<Mrs00166RDO> ListRdo = new List<Mrs00166RDO>();
        List<Mrs00166RDO> ListTreatmentService = new List<Mrs00166RDO>();
        List<HIS_EMPLOYEE> listEmployee = new List<HIS_EMPLOYEE>();

        Dictionary<long, V_HIS_SERVICE> dicParent = new Dictionary<long, V_HIS_SERVICE>();

        Dictionary<long, V_HIS_SERVICE> dicService = new Dictionary<long, V_HIS_SERVICE>();

        List<HIS_OTHER_PAY_SOURCE> listOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();

        const long VAO = 1;//vào viện
        const long CHIDINH = 2;//chỉ định
        const long BATDAU = 3;//bắt đầu
        const long KETTHUC = 4;//kết thúc
        const long RAVIEN = 5;//ra viện
        const long THANHTOAN = 6;//thanh toán
        const long KHOAVIENPHI = 7;//khóa viện phí
        const long GIAMDINHBHYT = 8;//giám định bhyt

        public Mrs00166Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00166Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00166Filter)reportFilter);
            var result = true;
            try
            {
                ListRdo = new ManagerSql().GetSS(filter);
                HisEmployeeFilterQuery employeeFilter = new HisEmployeeFilterQuery();
                listEmployee = new HisEmployeeManager().Get(employeeFilter);
                GetParentAndService();
                GetOtherPaySource();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetParentAndService()
        {
            HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
            var services = new HisServiceManager(new CommonParam()).GetView(serviceFilter);
            dicService = services.ToDictionary(o => o.ID);
            if (services != null)
            {
                foreach (var item in services)
                {
                    if (item.PARENT_ID != null)
                    {
                        var parent = services.FirstOrDefault(o => o.ID == item.PARENT_ID);
                        if (parent != null)
                        {
                            dicParent.Add(item.ID, parent);
                        }
                        else
                        {
                            dicParent.Add(item.ID, new V_HIS_SERVICE());
                        }
                    }
                }
            }
        }

        private void GetOtherPaySource()
        {
            listOtherPaySource = new HisOtherPaySourceManager().Get(new HisOtherPaySourceFilterQuery()) ?? new List<HIS_OTHER_PAY_SOURCE>();
        }
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                foreach (var item in ListRdo)
                {
                    AddInfo(item);
                }
                ListTreatmentService = ProcessListRDO(ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
                ListRdo.Clear();
            }
            return result;
        }


        void AddInfo(Mrs00166RDO data)
        {
            try
            {

                HIS_EMPLOYEE employee = (listEmployee ?? new List<HIS_EMPLOYEE>()).FirstOrDefault(o => o.LOGINNAME == data.REQUEST_LOGINNAME);
                HIS_EMPLOYEE employeeExe = (listEmployee ?? new List<HIS_EMPLOYEE>()).FirstOrDefault(o => o.LOGINNAME == data.EXECUTE_LOGINNAME);

                SetExtendField(data, employee, employeeExe);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(Mrs00166RDO r, HIS_EMPLOYEE employee, HIS_EMPLOYEE employeeExe)
        {
            try
            {
                r.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                r.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(r.IN_TIME);
                r.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(r.OUT_TIME??0);
                r.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.TDL_PATIENT_DOB);
                if (employee != null)
                {
                    r.DIPLOMA = employee.DIPLOMA;
                }
                if (employeeExe != null)
                {
                    r.EXE_DIPLOMA = employeeExe.DIPLOMA;
                }
                r.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                r.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                r.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == r.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;

                r.REQUEST_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                r.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                r.TREATMENT_TYPE_CODE = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == r.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE;
                r.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                r.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                r.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == r.TDL_SERVICE_TYPE_ID);
                if (serviceType != null)
                {
                    r.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                    r.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                }
                var parent = dicParent.ContainsKey(r.SERVICE_ID) ? dicParent[r.SERVICE_ID] : null;
                if (parent != null)
                {
                    r.PARENT_SERVICE_CODE = parent.SERVICE_CODE;
                    r.PARENT_SERVICE_NAME = parent.SERVICE_NAME;
                }
                var service = dicService.ContainsKey(r.SERVICE_ID) ? dicService[r.SERVICE_ID] : null;
                if (service != null)
                {
                    r.PTTT_GROUP_CODE = service.PTTT_GROUP_CODE;
                    r.PTTT_GROUP_NAME = service.PTTT_GROUP_NAME;
                }
                var otherPaySource = listOtherPaySource.FirstOrDefault(o => o.ID == r.SS_OTHER_PAY_SOURCE_ID);
                if (otherPaySource != null)
                {
                    r.OTHER_PAY_SOURCE_CODE = otherPaySource.OTHER_PAY_SOURCE_CODE;
                    r.OTHER_PAY_SOURCE_NAME = otherPaySource.OTHER_PAY_SOURCE_NAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<Mrs00166RDO> ProcessListRDO(List<Mrs00166RDO> listRdo)
        {
            List<Mrs00166RDO> listCurrent = new List<Mrs00166RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.TREATMENT_ID, o.PRICE, o.PATIENT_TYPE_ID }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00166RDO> listsub = group.ToList<Mrs00166RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00166RDO rdo = new Mrs00166RDO(listsub[0]);
                            rdo.AMOUNT= listsub.Sum(s => s.AMOUNT);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);

                            if (rdo.AMOUNT > 0)
                            {
                                listCurrent.Add(rdo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.IN_TIME).ToList();
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM??0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.OUT_TIME_TO ?? filter.FEE_LOCK_TIME_TO ?? filter.HEIN_LOCK_TIME_TO ?? 0));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME ?? string.Join(" - ", (HisDepartmentCFG.DEPARTMENTs.Where(o => (filter.DEPARTMENT_IDs??new List<long>()).Contains(o.ID)) ?? new List<HIS_DEPARTMENT>()).Select(o => o.DEPARTMENT_NAME).ToList()));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());
            objectTag.AddObjectData(store, "Report", ListTreatmentService);
            objectTag.AddObjectData(store, "Service", ListTreatmentService.GroupBy(o => o.SERVICE_ID).Select(p => p.First()).ToList());
            //if (filter.INPUT_DATA_ID_TIME_TYPE == VAO)// vào
            //{
            //    ListRdo = ListRdo.Where(x => x.IN_TIME >= filter.FEE_LOCK_TIME_FROM && x.IN_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            //if (filter.INPUT_DATA_ID_TIME_TYPE == RAVIEN)//ra
            //{
            //    ListRdo = ListRdo.Where(x => x.OUT_TIME >= filter.FEE_LOCK_TIME_FROM && x.OUT_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            //if (filter.INPUT_DATA_ID_TIME_TYPE == BATDAU)//bd
            //{
            //    ListRdo = ListRdo.Where(x => x.EXECUTE_TIME >= filter.FEE_LOCK_TIME_FROM && x.EXECUTE_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            //if (filter.INPUT_DATA_ID_TIME_TYPE == KETTHUC)//kt
            //{
            //    ListRdo = ListRdo.Where(x => x.TDL_FINISH_TIME >= filter.FEE_LOCK_TIME_FROM && x.TDL_FINISH_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            //if (filter.INPUT_DATA_ID_TIME_TYPE == CHIDINH)//cd
            //{
            //    ListRdo = ListRdo.Where(x => x.TDL_INTRUCTION_TIME >= filter.FEE_LOCK_TIME_FROM && x.TDL_INTRUCTION_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            //if (filter.INPUT_DATA_ID_TIME_TYPE == THANHTOAN)//tt
            //{
            //    ListRdo = ListRdo.Where(x => x.TDL_FINISH_TIME >= filter.FEE_LOCK_TIME_FROM && x.TDL_FINISH_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            //if (filter.INPUT_DATA_ID_TIME_TYPE == KHOAVIENPHI)//khoa vien phi
            //{
            //    ListRdo = ListRdo.Where(x => x.FEE_LOCK_TIME >= filter.FEE_LOCK_TIME_FROM && x.FEE_LOCK_TIME <= filter.FEE_LOCK_TIME_TO).ToList();
            //}
            objectTag.AddObjectData(store, "Detail", ListRdo);

        }


    }
}
