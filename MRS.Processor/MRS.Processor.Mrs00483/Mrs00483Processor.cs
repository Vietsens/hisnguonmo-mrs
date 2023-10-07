using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00483
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00483Processor : AbstractProcessor
    {
        Mrs00483Filter castFilter = new Mrs00483Filter();
        List<Mrs00483RDO> listRdo = new List<Mrs00483RDO>();

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();

        public Mrs00483Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00483Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00483Filter)this.reportFilter;
                //=======================================================================
                HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                sereServViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sereServViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                sereServViewFilter.SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT };
                if (castFilter.EXE_ROOM_ID != null || castFilter.REQ_ROOM_ID != null)
                {
                    if (castFilter.EXE_ROOM_ID != null)
                        sereServViewFilter.EXECUTE_ROOM_ID = castFilter.EXE_ROOM_ID;
                    if (castFilter.REQ_ROOM_ID != null)
                        sereServViewFilter.REQUEST_ROOM_ID = castFilter.REQ_ROOM_ID;
                }
                else if (castFilter.EXECUTE_DEPARTMENT_ID != null || castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    if (castFilter.EXECUTE_DEPARTMENT_ID != null)
                        sereServViewFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                    if (castFilter.REQUEST_DEPARTMENT_ID != null)
                        sereServViewFilter.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID;
                }
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter);
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
            bool result = true;
            try
            {
                listRdo.Clear();
                if (IsNotNullOrEmpty(listSereServs))
                {
                    listSereServs = listSereServs.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                    if (IsNotNullOrEmpty(listSereServs))
                    {
                        var patient_id = listSereServs.Select(o => o.TDL_PATIENT_ID ?? 0).Distinct().ToList();
                        var dicPatient = new Dictionary<long, V_HIS_PATIENT>();
                        var dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
                        if (IsNotNullOrEmpty(patient_id))
                        {
                            var skip = 0;
                            while (patient_id.Count - skip > 0)
                            {
                                var listSub = patient_id.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                                HisPatientViewFilterQuery patiFilter = new HisPatientViewFilterQuery();
                                patiFilter.IDs = listSub;
                                var data = new MOS.MANAGER.HisPatient.HisPatientManager(new CommonParam()).GetView(patiFilter);
                                if (IsNotNullOrEmpty(data))
                                {
                                    foreach (var item in data)
                                    {
                                        if (!dicPatient.ContainsKey(item.ID))
                                        {
                                            dicPatient.Add(item.ID, item);
                                        }
                                    }
                                }
                            }


                        }

                        var serviceReqIds = listSereServs.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                        if (IsNotNullOrEmpty(serviceReqIds))
                        {
                            var skip = 0;
                            while (serviceReqIds.Count - skip > 0)
                            {
                                var listSub = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                                HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery();
                                srFilter.IDs = listSub;
                                var serviceReq = new HisServiceReqManager(new CommonParam()).Get(srFilter);
                                if (IsNotNullOrEmpty(serviceReq))
                                {
                                    foreach (var item in serviceReq)
                                    {
                                        if (!dicServiceReq.ContainsKey(item.ID))
                                        {
                                            dicServiceReq.Add(item.ID, item);
                                        }
                                    }
                                }
                            }


                        }

                       
                        foreach (var item in listSereServs)
                        {
                            var ado = new Mrs00483RDO();
                            ado.SERE_SERV = item;
                            ado.INTRUCTION_TIME = item.TDL_INTRUCTION_TIME;
                            var serviceReq = dicServiceReq.ContainsKey(item.SERVICE_REQ_ID ?? 0) ? dicServiceReq[item.SERVICE_REQ_ID ?? 0] : new HIS_SERVICE_REQ();
                            ado.ICD_CODE = serviceReq.ICD_CODE;
                            ado.ICD_NAME = serviceReq.ICD_NAME;
                            ado.INTRUCTION_TIME = item.TDL_INTRUCTION_TIME;
                            ado.HIS_PATIENT = dicPatient.ContainsKey(item.TDL_PATIENT_ID ?? 0) ? dicPatient[item.TDL_PATIENT_ID ?? 0] : new V_HIS_PATIENT();
                            listRdo.Add(ado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listRdo.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                long EXE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID ?? 0;
                long REQ_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID ?? 0;

                long EXE_ROOM_ID = castFilter.EXE_ROOM_ID ?? 0;
                long REQ_ROOM_ID = castFilter.REQ_ROOM_ID ?? 0;

                if (castFilter.EXE_ROOM_ID != null || castFilter.REQ_ROOM_ID != null)
                {
                    HisRoomFilterQuery roomFilter = new HisRoomFilterQuery();
                    roomFilter.IDs = new List<long>() { EXE_ROOM_ID, REQ_ROOM_ID };
                    var listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(param).Get(roomFilter);

                    if (IsNotNullOrEmpty(listRooms))
                    {
                        try
                        {
                            EXE_DEPARTMENT_ID = listRooms.Where(w => w.ID == EXE_ROOM_ID).First().DEPARTMENT_ID;
                            REQ_DEPARTMENT_ID = listRooms.Where(w => w.ID == REQ_ROOM_ID).First().DEPARTMENT_ID;
                        }
                        catch (Exception ex)
                        {
                            Inventec.Common.Logging.LogSystem.Error(ex);
                        }
                    }
                }

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.IDs = new List<long>() { EXE_DEPARTMENT_ID, REQ_DEPARTMENT_ID };
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);
                if (IsNotNullOrEmpty(listDepartments))
                {
                    try
                    {
                        dicSingleTag.Add("EXE_DEPARTMENT_NAME", listDepartments.Where(w => w.ID == EXE_DEPARTMENT_ID).First().DEPARTMENT_NAME);
                        dicSingleTag.Add("REQ_DEPARTMENT_NAME", listDepartments.Where(w => w.ID == REQ_DEPARTMENT_ID).First().DEPARTMENT_NAME);
                    }
                    catch (Exception ex)
                    {
                        Inventec.Common.Logging.LogSystem.Error(ex);
                    }
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.INTRUCTION_TIME).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
