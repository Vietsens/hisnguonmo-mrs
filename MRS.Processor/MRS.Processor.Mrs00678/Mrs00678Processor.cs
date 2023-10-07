using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
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
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisIcdGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisExecuteRoom;
using MRS.MANAGER.SarReport.RDO;
using System.Threading;
using MOS.MANAGER.HisExpMestMedicine;

namespace MRS.Processor.Mrs00678
{
    public class Mrs00678Processor : AbstractProcessor
    {
        private Mrs00678Filter filter;
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
    //    Dictionary<long, List<HIS_DEPARTMENT>> dicDepartment = new Dictionary<long, List<HIS_DEPARTMENT>>();
   //     Dictionary<long, List<HIS_DEPARTMENT>> dicDepartment = new Dictionary<long, List<HIS_DEPARTMENT>>();
        List<Mrs00678RDO> listRdo = new List<Mrs00678RDO>();
        List<D_HIS_SERVICE_REQ> ListServiceReq = new List<D_HIS_SERVICE_REQ>();
        Dictionary<long,List<HIS_SERE_SERV>> dicSereServ = new Dictionary<long,List<HIS_SERE_SERV>>();
        Dictionary<long,V_HIS_TREATMENT_4> dicTreatment = new Dictionary<long,V_HIS_TREATMENT_4>();
        Dictionary<long, List<SALE_MEDICINE>> dicSaleExpMestMedicine = new Dictionary<long, List<SALE_MEDICINE>>();
        Dictionary<long, List<D_HIS_SERVICE_REQ>> dicServiceReq = new Dictionary<long, List<D_HIS_SERVICE_REQ>>();
        CommonParam paramGet = new CommonParam();
        Dictionary<long, HIS_SERVICE> dicService = new Dictionary<long, HIS_SERVICE>();

        Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom;

        long patientTypeBhyt;

        long patientTypeFree;

        List<HIS_PATIENT_TYPE> listPatientType;
    //    List<HIS_DEPARTMENT> listDepartment;
       

        public Mrs00678Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00678Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00678Filter)reportFilter;
            try
            {
                dicRoom = GetExecuteRoom();
                patientTypeBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                patientTypeFree = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
                listPatientType = HisPatientTypeCFG.PATIENT_TYPEs ?? new List<HIS_PATIENT_TYPE>();

                //serviceReq
                ListServiceReq = GetServiceReq();

                //treatment
                dicTreatment = GetTreatment();

                //sereServ
                dicSereServ = GetSereServ();
                
                //saleExp
                dicSaleExpMestMedicine = GetSaleExpMestMedicine();


                //serviceReq
                dicService = GetService();

                //depatment
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                var department = new HisDepartmentManager(param).Get(departmentFilter);
                if (department != null)
                {
                    listDepartment.AddRange(department);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private Dictionary<long,List<SALE_MEDICINE>> GetSaleExpMestMedicine()
        {
            List<SALE_MEDICINE> result = new List<SALE_MEDICINE>();
            try
            {
                result = new ManagerSql().GetSaleExpMestMedicine(filter) ?? new List<SALE_MEDICINE>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<SALE_MEDICINE>();
            }
            return result.GroupBy(o=>o.TREATMENT_ID).ToDictionary(p=>p.Key,q=>q.ToList());

        }

        private Dictionary<long, List<HIS_SERE_SERV>> GetSereServ()
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                result = new ManagerSql().GetSereServ(filter) ?? new List<HIS_SERE_SERV>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<HIS_SERE_SERV>();
            }
            return result.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());

        }

        private Dictionary<long, HIS_SERVICE> GetService()
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
            try
            {
                result = new HisServiceManager().Get(new HisServiceFilterQuery()) ?? new List<HIS_SERVICE>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<HIS_SERVICE>();
            }
            return result.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());

        }

        private Dictionary<long, HIS_DEPARTMENT> GetDepartment()

        {
            List<HIS_DEPARTMENT> result = new List<HIS_DEPARTMENT>();
            try
            {
                result = new HisDepartmentManager().Get(new HisDepartmentFilterQuery()) ?? new List<HIS_DEPARTMENT>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<HIS_DEPARTMENT>();
            }
            return result.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());

        }



        private Dictionary<long,V_HIS_TREATMENT_4> GetTreatment()
        {
            List<V_HIS_TREATMENT_4> result = new List<V_HIS_TREATMENT_4>();
            try
            {
                result = new ManagerSql().GetTreatment(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<V_HIS_TREATMENT_4>();
            }
            return result.GroupBy(g=>g.ID).ToDictionary(p=>p.Key,q=>q.First());

        }

        private List<D_HIS_SERVICE_REQ> GetServiceReq()
        {
            List<D_HIS_SERVICE_REQ> result = new List<D_HIS_SERVICE_REQ>();
            try
            {

                result = new ManagerSql().Get(filter);

                //dicServiceReq
                dicServiceReq = result.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
               
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result =  new List<D_HIS_SERVICE_REQ>();
            }
            return result;
        }


        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    
                    ListServiceReq = ListServiceReq.Where(o =>o.IS_DELETE != 1).ToList();
                    List<Thread> listThread = new List<Thread>();
                    int count = (int)ListServiceReq.Count / 4;
                    for (int i = 0; i <= 4; i++)
                    {
                        var ListServiceReqSub = ListServiceReq.Skip(count * i).Take(i==4?i:count).ToList();
                        Thread thread = new Thread(processorRdo);
                        thread.Start(ListServiceReqSub);
                        listThread.Add(thread);
                    }
                    foreach (var item in listThread)
                    {
                        item.Join();
                    }
                    //Mrs00678RDO.i = 0;
                    Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsExam(long requestRoomId)
        {
            return dicRoom.ContainsKey(requestRoomId) && dicRoom[requestRoomId].IS_EXAM == 1;
        }

        private void processorRdo(object ListServiceReqSub)
        {
            try
            {
                listRdo.AddRange((from r in (ListServiceReqSub as List<D_HIS_SERVICE_REQ>) select new Mrs00678RDO(r,
                    dicTreatment.ContainsKey(r.TREATMENT_ID) ? dicTreatment[r.TREATMENT_ID] : new V_HIS_TREATMENT_4(), 
                    dicSereServ.ContainsKey(r.TREATMENT_ID) ? dicSereServ[r.TREATMENT_ID] : new List<HIS_SERE_SERV>(),
                    listDepartment,
                    dicRoom,
                    patientTypeBhyt, 
                    patientTypeFree,
                    listPatientType,
                    
                
                    dicSaleExpMestMedicine.ContainsKey(r.TREATMENT_ID) ? dicSaleExpMestMedicine[r.TREATMENT_ID] : new List<SALE_MEDICINE>(), 
                    filter,
                    dicServiceReq.ContainsKey(r.TREATMENT_ID) ? dicServiceReq[r.TREATMENT_ID] : new List<D_HIS_SERVICE_REQ>(),
                    dicService
                    )).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Dictionary<long, V_HIS_EXECUTE_ROOM> GetExecuteRoom()
        {
            Dictionary<long, V_HIS_EXECUTE_ROOM> result = new Dictionary<long, V_HIS_EXECUTE_ROOM>();
            try
            {

                HisExecuteRoomViewFilterQuery exroom = new HisExecuteRoomViewFilterQuery();
                var ExamRooms = new MOS.MANAGER.HisExecuteRoom.HisExecuteRoomManager(param).GetView(exroom);
                foreach (var item in ExamRooms)
                {
                    if (!result.ContainsKey(item.ROOM_ID)) result[item.ROOM_ID] = item;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((filter.FINISH_TIME_FROM ?? 0)));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((filter.FINISH_TIME_TO ?? 0)));
            if (this.filter.EXAM_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.filter.EXAM_ROOM_ID) ?? new V_HIS_ROOM();
                dicSingleTag.Add("ROOM_NAME", room.ROOM_NAME);

                dicSingleTag.Add("DEPARTMENT_NAME", room.DEPARTMENT_NAME);

            }
            if (filter.IS_FINISH == true)
            {
                listRdo = listRdo.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
            }
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "Parent", listRdo.GroupBy(o => o.EXECUTE_ROOM_ID).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "EXECUTE_ROOM_ID", "EXECUTE_ROOM_ID");

            objectTag.AddObjectData(store, "GrandParentReport", listRdo.GroupBy(o => o.EXECUTE_ROOM_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ParentReport", listRdo.OrderBy(p => p.INTRUCTION_DATE).GroupBy(o => new { o.EXECUTE_ROOM_ID, o.INTRUCTION_DATE }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParentReport", "ParentReport", "EXECUTE_ROOM_ID", "EXECUTE_ROOM_ID");
            objectTag.AddRelationship(store, "ParentReport", "Report", new string[] { "EXECUTE_ROOM_ID", "INTRUCTION_DATE" }, new string[] { "EXECUTE_ROOM_ID", "INTRUCTION_DATE" });
        }
    }
}
