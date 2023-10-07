using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTransaction;

namespace MRS.Processor.Mrs00043
{
    public class Mrs00043Processor : AbstractProcessor
    {
        Mrs00043Filter castFilter = null;
        private List<Mrs00043RDO> ListRdo = new List<Mrs00043RDO>();
        //List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        //List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
        //List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        //List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();

        private List<List<string>> listAreas = new List<List<string>>();

        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();

        CommonParam paramGet = new CommonParam();
        public Mrs00043Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00043Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00043Filter)reportFilter);
            var result = true;
            try
            {
                //DV - nhom loai
                var listServiceId = new List<long>();
                if (IsNotNullOrEmpty(castFilter.REPORT_TYPE_CAT_IDs))
                {
                    HisServiceRetyCatFilterQuery srFilter = new HisServiceRetyCatFilterQuery();
                    srFilter.REPORT_TYPE_CAT_IDs = castFilter.REPORT_TYPE_CAT_IDs;
                    var resultRetyCat = new HisServiceRetyCatManager().Get(srFilter);
                    listServiceId = IsNotNullOrEmpty(resultRetyCat) ? resultRetyCat.Select(o => o.SERVICE_ID).Distinct().ToList() : null;

                }
              
                ListRdo = new ManagerSql().GetRdo(castFilter, new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS });
                
                if (!string.IsNullOrWhiteSpace(castFilter.DE_AREAs))
                {
                    string[] deAreas = castFilter.DE_AREAs.Split(',');
                    if (deAreas.Length > 0)
                    {
                        for (int i = 0; i < deAreas.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(deAreas[i]))
                            {
                                int index = deAreas[i].IndexOf("->");
                                if (index > 0 && index < deAreas[i].Length - 2)
                                {
                                    List<string> area = new List<string>();
                                    string Begin = deAreas[i].Substring(0, index);
                                    string End = deAreas[i].Substring(index + 2);
                                    if (!string.IsNullOrWhiteSpace(Begin) && !string.IsNullOrWhiteSpace(End))
                                    {
                                        area.Add(Begin);
                                        area.Add(End);
                                        listAreas.Add(area);
                                    }
                                }
                            }
                        }
                    }
                }


                //máy
                GetMachine();

                //dịch vụ máy
                GetServiceMachine();

                //lọc theo máy ở danh mục dịch vụ
                FilterByMachine();
;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterByMachine()
        {
            if (castFilter.MACHINE_IDs != null)
            {
                var serviceIds = dicServiceMachine.Values.SelectMany(p => p.ToList()).Where(o => castFilter.MACHINE_IDs.Contains(o.MACHINE_ID)).Select(q => q.SERVICE_ID).Distinct().ToList();
                ListRdo = ListRdo.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
            }
        }

        private void GetMachine()
        {
            string query = "select * from his_machine where is_delete=0";
            ListMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MACHINE>(query) ?? new List<HIS_MACHINE>();
        }

        private void GetServiceMachine()
        {
            string query = "select * from his_service_machine where is_delete=0";
            var ListServiceMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_MACHINE>(query);
            if (ListServiceMachine != null && ListServiceMachine.Count > 0)
            {
                dicServiceMachine = ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.ToList());
            }
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (ListRdo != null && ListRdo.Count > 0)
                {
                    ListRdo = ListRdo.ToList().OrderBy(d => d.TDL_PATIENT_ID).ToList();
                    if (ListRdo != null && ListRdo.Count > 0)
                    {
                        CommonParam param = new CommonParam();///
                        int start = 0;
                        int count = ListRdo.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                            List<Mrs00043RDO> sereServs = ListRdo.Skip(start).Take(limit).ToList();
                            ProcessOneSereServFromList(sereServs);
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }

                        ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();
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
        private void ProcessOneSereServFromList(List<Mrs00043RDO> sereServs)
        {
            try
            {
                foreach (var rdo in sereServs)
                {
                    rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.INTRUCTION_TIME_NUM ?? 0);
                    rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.FINISH_TIME ?? 0);
                    rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => rdo.REQUEST_ROOM_ID==o.ID)??new V_HIS_ROOM()).ROOM_NAME;
                    rdo.EXECUTE_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => rdo.EXECUTE_ROOM_ID == o.ID) ?? new V_HIS_ROOM()).DEPARTMENT_NAME;
                    rdo.REQUEST_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => rdo.REQUEST_ROOM_ID == o.ID) ?? new V_HIS_ROOM()).DEPARTMENT_NAME;
                    if (this.listAreas.Count > 0 && !string.IsNullOrWhiteSpace(rdo.DESCRIPTION))
                    {
                        foreach (List<string> item in this.listAreas)
                        {
                            int indexBegin = rdo.DESCRIPTION.IndexOf(item[0]);
                            int indexEnd = rdo.DESCRIPTION.IndexOf(item[1], indexBegin > 0 ? indexBegin : 0);
                            if (indexBegin > 0 && indexEnd > 0 && indexBegin < indexEnd)
                            {
                                rdo.DE_AREA += rdo.DESCRIPTION.Substring(indexBegin + item[0].Length, indexEnd - (indexBegin + item[0].Length));
                            }
                        }

                    }

                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;

                    var patientType1 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.TDL_PATIENT_TYPE_CODE = patientType1.PATIENT_TYPE_CODE;
                    rdo.TDL_PATIENT_TYPE_NAME = patientType1.PATIENT_TYPE_NAME;

                    if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        rdo.PATIENT_TYPE_NAME_1 = "BHYT";
                    if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_1 = "Viện phí";
                    //if (serviceReq != null)
                    {
                        rdo.ICD_ENDO = rdo.ICD_NAME + ";" + rdo.ICD_TEXT;
                    }
                    rdo.PATIENT_TYPE_NAME_2 = patientType1.PATIENT_TYPE_NAME;

                    var serviceMachine = dicServiceMachine.ContainsKey(rdo.SERVICE_ID) ? dicServiceMachine[rdo.SERVICE_ID] : null;
                    if (serviceMachine != null && serviceMachine.Count > 0)
                    {
                        var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machine.Count > 0)
                        {
                            rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }
                    var machineExt = ListMachine.Where(p => p.ID == rdo.MACHINE_ID).ToList();
                    if (machineExt.Count > 0)
                    {
                        rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                        rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                    }
                    CalcuatorAge(rdo);
                    IsBhyt(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void CalcuatorAge(Mrs00043RDO rdo)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(rdo.TDL_PATIENT_DOB ?? 0);
                if (tuoi >= 0)
                {
                    if (rdo.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB ?? 0);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private void IsBhyt(Mrs00043RDO rdo)
        {
            try
            {
                //if (treatmentId > 0)
                {
                    //var result = listPatientTypeAlter.Where(o => o.TREATMENT_ID == treatmentId).ToList();
                    //if (result != null && result.Count > 0)
                    {
                        if (rdo.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.IS_BHYT = "X";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00043Filter)this.reportFilter).TIME_FROM ?? castFilter.FINISH_TIME_FROM?? 0)); //+ Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00043Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00043Filter)this.reportFilter).TIME_TO ?? castFilter.FINISH_TIME_TO ?? 0)); //+ Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00043Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
            if (((Mrs00043Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00043Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            if (((Mrs00043Filter)this.reportFilter).EXECUTE_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => ((Mrs00043Filter)this.reportFilter).EXECUTE_ROOM_ID == o.ID);
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
            }
            if (((Mrs00043Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00043Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            if (((Mrs00043Filter)this.reportFilter).EXECUTE_ROOM_IDs != null)
            {
                var room = HisRoomCFG.HisRooms.Where(o => ((Mrs00043Filter)this.reportFilter).EXECUTE_ROOM_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("EXECUTE_ROOM_NAME", string.Join(" - ", room.Select(o => o.ROOM_NAME).ToList()));
            }
            if (((Mrs00043Filter)this.reportFilter).PATIENT_TYPE_ID != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ((Mrs00043Filter)this.reportFilter).PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                dicSingleTag.Add("PATIENT_TYPE_NAME", patientType.PATIENT_TYPE_NAME);
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.FINISH_TIME).ToList());
        }
    }
}
