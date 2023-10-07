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
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisSereServTein;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTestIndex;

namespace MRS.Processor.Mrs00041
{
    public class Mrs00041Processor : AbstractProcessor
    {
        Mrs00041Filter castFilter = null;
        private List<Mrs00041RDO> ListRdo = new List<Mrs00041RDO>();
        List<SSE> ListSereServExt = new List<SSE>();
        List<Mrs00041RDO> ListSereServ = new List<Mrs00041RDO>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        Dictionary<long, HIS_SERVICE> dicParService = new Dictionary<long, HIS_SERVICE>();
        List<V_HIS_TEST_INDEX> ListTestIndex = new List<V_HIS_TEST_INDEX>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();
        const string TEMP_1 = "MRS0004101";
        const string TEMP_2 = "MRS0004102";

        CommonParam paramGet = new CommonParam();
        public Mrs00041Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00041Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00041Filter)reportFilter);
            var result = true;
            try
            {
                ListSereServ = new Mrs00041RDOManager().GetRdo(castFilter);
                ListSereServExt = new Mrs00041RDOManager().GetRdoExt(castFilter);
                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                ListService = new HisServiceManager(param).Get(serviceFilter);
                dicParService = ListService.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => ListService.FirstOrDefault(r => r.ID == q.First().PARENT_ID) ?? new HIS_SERVICE());
                HisTestIndexViewFilterQuery TestIndexFilter = new HisTestIndexViewFilterQuery();
                ListTestIndex = new HisTestIndexManager(param).GetView(TestIndexFilter);

                //get dịch vụ máy
                GetServiceMachine();

                //get máy
                GetMachine();

                //lọc theo máy
                FilterByMachine();

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
                ListSereServ = ListSereServ.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
                var sereServIds = ListSereServ.Select(o => o.SERE_SERV_ID).ToList();
                ListSereServExt = ListSereServExt.Where(o => sereServIds.Contains(o.SERE_SERV_ID ?? 0)).ToList();
            }
            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                ListSereServExt = ListSereServExt.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.MACHINE_ID ?? 0)).ToList();
                var sereServIds = ListSereServExt.Select(o => o.SERE_SERV_ID).ToList();
                ListSereServ = ListSereServ.Where(o => sereServIds.Contains(o.SERE_SERV_ID)).ToList();
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
                if (ListSereServ != null && ListSereServ.Count > 0)
                {
                    ListSereServ = ListSereServ.OrderBy(b => b.PATIENT_CODE).ToList();
                    if (this.dicDataFilter.ContainsKey("KEY_MERGE_TREATMENT") && this.dicDataFilter["KEY_MERGE_TREATMENT"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_MERGE_TREATMENT"].ToString()))
                    {
                        if (this.dicDataFilter["KEY_MERGE_TREATMENT"].ToString() == "TREATMENT")
                            ProcessWithTreatment(ListSereServ);
                        if (this.dicDataFilter["KEY_MERGE_TREATMENT"].ToString() == "PARENT")
                            ProcessWithIntructionAndParent(ListSereServ);
                        if (this.dicDataFilter["KEY_MERGE_TREATMENT"].ToString() == "SERVICE_REQ")
                            ProcessWithServiceReq(ListSereServ);
                        if (this.dicDataFilter["KEY_MERGE_TREATMENT"].ToString() == "SERE_SERV")
                            ProcessOneSereServFromList(ListSereServ);
                    }
                    else if (castFilter.IS_MERGE_INT_PAR == true)
                    {
                        ProcessWithIntructionAndParent(ListSereServ);//group theo mã điều trị, ngày chỉ định và dịch vụ cha
                        ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();
                    }
                    else if (castFilter.IS_MERGE_TREATMENT == true)
                    {
                        ProcessWithTreatment(ListSereServ);
                    }
                    else
                    {
                        switch (ReportTemplateCode)
                        {
                            case TEMP_2:
                                ProcessWithServiceReq(ListSereServ);
                                break;
                            case TEMP_1:
                                ProcessOneSereServFromList(ListSereServ);
                                break;
                            default:
                                ProcessOneSereServFromList(ListSereServ);
                                break;
                        }
                    }
                    ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessOneSereServFromList(List<Mrs00041RDO> sereServs)
        {
            try
            {
                foreach (var sereServ in sereServs)
                {

                    Mrs00041RDO rdo = new Mrs00041RDO();

                    rdo.PATIENT_NAME = sereServ.PATIENT_NAME;
                    rdo.PATIENT_CODE = sereServ.PATIENT_CODE;
                    rdo.TREATMENT_CODE = sereServ.TREATMENT_CODE;
                    rdo.IN_TIME = sereServ.IN_TIME;
                    rdo.OUT_TIME = sereServ.OUT_TIME;

                    rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.IN_TIME);
                    rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.OUT_TIME ?? 0);
                    rdo.TDL_PATIENT_PHONE = sereServ.TDL_PATIENT_PHONE;

                    rdo.TDL_PATIENT_DOB = sereServ.TDL_PATIENT_DOB;
                    rdo.DOB_YEAR = sereServ.TDL_PATIENT_DOB != null && sereServ.TDL_PATIENT_DOB > 1000 ? sereServ.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    var patientType1 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.PATIENT_TYPE_NAME_2 = patientType1.PATIENT_TYPE_NAME;
                    rdo.TDL_PATIENT_TYPE_NAME = patientType1.PATIENT_TYPE_NAME;
                    rdo.TDL_PATIENT_TYPE_CODE = patientType1.PATIENT_TYPE_NAME;
                    if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                    }
                    else if (patientType.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_01 = "VP";
                    else
                    {
                        rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "XHH";
                    }
                    var treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == sereServ.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
                    if (treatmentType != null)
                    {
                        rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                        rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                    }
                    rdo.IS_EXPEND_SERVICE = sereServ.IS_EXPEND == 1 ? "X" : "";
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == sereServ.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.REQUEST_USERNAME = sereServ.REQUEST_USERNAME;
                    rdo.EXECUTE_USERNAME = sereServ.EXECUTE_USERNAME;
                    rdo.SERVICE_REQ_CODE = sereServ.SERVICE_REQ_CODE;
                    rdo.REQUEST_LOGINNAME = sereServ.REQUEST_LOGINNAME;
                    rdo.EXECUTE_LOGINNAME = sereServ.EXECUTE_LOGINNAME;
                    rdo.BLOCK = sereServ.BLOCK;
                    rdo.SERVICE_NAME = sereServ.SERVICE_NAME;
                    rdo.SERVICE_CODE = sereServ.SERVICE_CODE;
                    rdo.PAR_SERVICE_NAME = this.ParentNameOf(sereServ.SERVICE_ID);
                    rdo.PAR_SERVICE_CODE = this.ParentCodeOf(sereServ.SERVICE_ID);
                    rdo.TDL_INTRUCTION_TIME = sereServ.TDL_INTRUCTION_TIME;
                    rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.TDL_INTRUCTION_TIME ?? 0);
                    rdo.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.EXECUTE_TIME ?? 0);
                    rdo.EXECUTE_TIME = sereServ.EXECUTE_TIME ?? 0;
                    rdo.START_TIME = sereServ.START_TIME;
                    rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.START_TIME ?? 0);
                    rdo.VIR_PRICE = sereServ.VIR_PRICE;
                    rdo.FINISH_TIME = sereServ.FINISH_TIME;
                    rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.FINISH_TIME ?? 0);
                    rdo.RESULT_TIME = sereServ.RESULT_TIME;
                    rdo.BARCODE = sereServ.BARCODE;
                    rdo.SAMPLE_LOGINNAME = sereServ.SAMPLE_LOGINNAME;
                    rdo.SAMPLE_USERNAME = sereServ.SAMPLE_USERNAME;
                    rdo.SAMPLE_NOTE = sereServ.SAMPLE_NOTE;
                    rdo.SAMPLE_TIME = sereServ.SAMPLE_TIME;
                    rdo.AMOUNT = sereServ.AMOUNT;

                    //group.Sum(item => item.AMOUNT)

                    var serviceMachine = dicServiceMachine.ContainsKey(sereServ.SERVICE_ID) ? dicServiceMachine[sereServ.SERVICE_ID] : null;
                    if (serviceMachine != null && serviceMachine.Count > 0)
                    {
                        var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machine.Count > 0)
                        {
                            rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }

                    rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.ICD_TEST = sereServ.ICD_TEST;
                    rdo.DIC_TEST_RESULT = new Dictionary<string, string>();
                    var ssTeins = ListSereServExt.Where(f => f.SERE_SERV_ID == sereServ.SERE_SERV_ID).ToList();

                    if (ssTeins != null && ssTeins.Count > 0)
                    {
                        var machineExt = ListMachine.Where(p => ssTeins.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machineExt.Count > 0)
                        {
                            rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }

                        rdo.NOTE = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.NOTE)).Select(p => p.NOTE).ToList());
                        rdo.LEAVEN = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.LEAVEN)).Select(p => p.LEAVEN).ToList());
                        string value = "";
                        if (ssTeins.Exists(o => !string.IsNullOrWhiteSpace(o.CONCLUDE)))
                        {
                            value = string.Join(";", ssTeins.Select(o => o.CONCLUDE).Distinct().ToList());
                            var serviceCode = sereServ.SERVICE_CODE ?? "";
                            if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                            {
                                rdo.DIC_TEST_RESULT[serviceCode] = "";
                            }
                            rdo.DIC_TEST_RESULT[serviceCode] += value;
                        }
                        else
                        {
                            foreach (var item in ssTeins)
                            {
                                var testIndex = ListTestIndex.FirstOrDefault(o => o.ID == item.TEST_INDEX_ID);
                                if (testIndex != null)
                                {
                                    value += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                    var serviceCode = sereServ.SERVICE_CODE ?? "";
                                    if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                                    {
                                        rdo.DIC_TEST_RESULT[serviceCode] = "";
                                    }
                                    rdo.DIC_TEST_RESULT[serviceCode] += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                }
                            }
                        }
                        rdo.TEST_RESULT = value;

                    }
                    CalcuatorAge(rdo, sereServ);
                    IsBhyt(rdo, sereServ.PATIENT_TYPE_ID ?? 0);
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessWithServiceReq(List<Mrs00041RDO> sereServs)
        {
            try
            {
                var groupbyReq = sereServs.GroupBy(p => p.SERVICE_REQ_ID).ToList();
                foreach (var group in groupbyReq)
                {
                    Mrs00041RDO rdo = new Mrs00041RDO();
                    List<Mrs00041RDO> listSub = group.ToList<Mrs00041RDO>();

                    rdo.SERVICE_REQ_ID = listSub.First().SERVICE_REQ_ID ?? 0;
                    if (listSub.First().PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.HEIN_CARD_NUMBER = listSub.First().HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                    }
                    else if (listSub.First().PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_01 = "VP";
                    else
                    {
                        rdo.HEIN_CARD_NUMBER = listSub.First().HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "XHH";
                    }
                    var treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == listSub.First().TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
                    if (treatmentType != null)
                    {
                        rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                        rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                    }
                    rdo.PATIENT_NAME = listSub.First().PATIENT_NAME;
                    rdo.PATIENT_CODE = listSub.First().PATIENT_CODE;
                    rdo.TREATMENT_CODE = listSub.First().TREATMENT_CODE;
                    rdo.IN_TIME = listSub.First().IN_TIME;
                    rdo.OUT_TIME = listSub.First().OUT_TIME;

                    rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.IN_TIME);
                    rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.OUT_TIME ?? 0);
                    rdo.TDL_PATIENT_PHONE = listSub.First().TDL_PATIENT_PHONE;

                    rdo.TDL_PATIENT_DOB = listSub.First().TDL_PATIENT_DOB;
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == listSub.First().PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    var tdlPatientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == listSub.First().TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.TDL_PATIENT_TYPE_NAME = tdlPatientType.PATIENT_TYPE_NAME;
                    rdo.TDL_PATIENT_TYPE_CODE = tdlPatientType.PATIENT_TYPE_CODE;
                    rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub.First().REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub.First().EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == listSub.First().REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.REQUEST_USERNAME = listSub.First().REQUEST_USERNAME;
                    rdo.EXECUTE_USERNAME = listSub.First().EXECUTE_USERNAME;
                    rdo.SERVICE_REQ_CODE = listSub.First().SERVICE_REQ_CODE;
                    rdo.REQUEST_LOGINNAME = listSub.First().REQUEST_LOGINNAME;
                    rdo.EXECUTE_LOGINNAME = listSub.First().EXECUTE_LOGINNAME;
                    rdo.BLOCK = listSub.First().BLOCK;
                    rdo.SERVICE_NAME = string.Join(", ", listSub.Select(o => o.SERVICE_NAME).Distinct().ToList());
                    rdo.SERVICE_CODE = listSub.First().SERVICE_CODE;
                    rdo.PAR_SERVICE_NAME = string.Join(", ", listSub.Select(o => ParentNameOf(o.SERVICE_ID)).Distinct().ToList());
                    rdo.PAR_SERVICE_CODE = string.Join(", ", listSub.Select(o => ParentCodeOf(o.SERVICE_ID)).Distinct().ToList());
                    rdo.TIME_TEST_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().START_TIME ?? 0);
                    rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().TDL_INTRUCTION_TIME ?? 0);
                    rdo.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().EXECUTE_TIME ?? 0);
                    rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().FINISH_TIME ?? 0);
                    rdo.TDL_INTRUCTION_TIME = listSub.First().TDL_INTRUCTION_TIME;
                    rdo.RESULT_TIME = listSub.First().RESULT_TIME;
                    rdo.SAMPLE_TIME = listSub.First().SAMPLE_TIME;
                    rdo.AMOUNT = listSub.First().AMOUNT;
                    // rdo.SUMAMOUNT = listSub.Sum(item => item.AMOUNT);


                    rdo.ICD_TEST = listSub.First().ICD_TEST;
                    rdo.DIC_TEST_RESULT = new Dictionary<string, string>();
                    var ssTeins = ListSereServExt.Where(f => listSub.Exists(o => o.SERE_SERV_ID == f.SERE_SERV_ID)).ToList();
                    if (ssTeins != null && ssTeins.Count > 0)
                    {
                        var machineExt = ListMachine.Where(p => ssTeins.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machineExt.Count > 0)
                        {
                            rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                        rdo.NOTE = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.NOTE)).Select(p => p.NOTE).ToList());
                        rdo.LEAVEN = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.LEAVEN)).Select(p => p.LEAVEN).ToList());
                        string value = "";
                        if (ssTeins.Exists(o => !string.IsNullOrWhiteSpace(o.DESCRIPTION)))
                        {
                            value = string.Join(";", ssTeins.Select(o => o.DESCRIPTION).Distinct().ToList());
                            var serviceCode = (listSub.FirstOrDefault(o => o.SERE_SERV_ID == ssTeins.First().SERE_SERV_ID) ?? new Mrs00041RDO()).SERVICE_CODE ?? "";
                            if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                            {
                                rdo.DIC_TEST_RESULT[serviceCode] = "";
                            }
                            rdo.DIC_TEST_RESULT[serviceCode] += value;
                        }
                        else
                        {
                            foreach (var item in ssTeins)
                            {
                                var testIndex = ListTestIndex.FirstOrDefault(o => o.ID == item.TEST_INDEX_ID);
                                if (testIndex != null)
                                {
                                    value += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                    var serviceCode = (listSub.FirstOrDefault(o => o.SERE_SERV_ID == item.SERE_SERV_ID) ?? new Mrs00041RDO()).SERVICE_CODE ?? "";
                                    if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                                    {
                                        rdo.DIC_TEST_RESULT[serviceCode] = "";
                                    }
                                    rdo.DIC_TEST_RESULT[serviceCode] += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                    //LogSystem.Info("check");
                                }
                            }
                        }

                        rdo.TEST_RESULT = value;
                    }
                    var serviceMachine = dicServiceMachine.ContainsKey(group.First().SERVICE_ID) ? dicServiceMachine[group.First().SERVICE_ID] : null;
                    if (serviceMachine != null && serviceMachine.Count > 0)
                    {
                        var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machine.Count > 0)
                        {
                            rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }

                    CalcuatorAge(rdo, listSub.First());
                    IsBhyt(rdo, listSub.First().PATIENT_TYPE_ID ?? 0);
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessWithTreatment(List<Mrs00041RDO> sereServs)
        {
            try
            {
                var groupbyTrea = sereServs.GroupBy(p => p.TREATMENT_CODE).ToList();
                foreach (var group in groupbyTrea)
                {
                    Mrs00041RDO rdo = new Mrs00041RDO();
                    List<Mrs00041RDO> listSub = group.ToList<Mrs00041RDO>();
                    //rdo.SERVICE_REQ_ID = listSub.First().SERVICE_REQ_ID ?? 0;
                    rdo.PATIENT_NAME = listSub.First().PATIENT_NAME;
                    rdo.TREATMENT_CODE = listSub.First().TREATMENT_CODE;
                    rdo.IN_TIME = listSub.First().IN_TIME;
                    rdo.OUT_TIME = listSub.First().OUT_TIME;
                    rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.IN_TIME);
                    rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.OUT_TIME ?? 0);
                    rdo.TDL_PATIENT_PHONE = listSub.First().TDL_PATIENT_PHONE;
                    //rdo.HEIN_CARD_NUMBER = listSub.First().HEIN_CARD_NUMBER;
                    rdo.TDL_PATIENT_DOB = listSub.First().TDL_PATIENT_DOB;

                    rdo.MEDI_ORG_CODE = listSub.First().MEDI_ORG_CODE;
                    rdo.VIR_ADDRESS = listSub.First().VIR_ADDRESS;
                    rdo.ICD_TEST = listSub.First().ICD_TEST;
                    rdo.PATIENT_CODE = listSub.First().PATIENT_CODE;
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == listSub.First().PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    var patientType1 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == listSub.First().TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    if (listSub.First().PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.HEIN_CARD_NUMBER = listSub.First().HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                    }
                    else if (listSub.First().PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_01 = "VP";
                    else
                    {
                        rdo.HEIN_CARD_NUMBER = listSub.First().HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "XHH";
                    }
                    var treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == listSub.First().TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
                    if (treatmentType != null)
                    {
                        rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                        rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                    }
                    rdo.PATIENT_TYPE_NAME_2 = patientType1.PATIENT_TYPE_NAME;
                    rdo.TDL_PATIENT_TYPE_CODE = patientType1.PATIENT_TYPE_CODE;
                    rdo.TDL_PATIENT_TYPE_NAME = patientType1.PATIENT_TYPE_NAME;
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub.First().EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub.First().REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == listSub.First().REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    //rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub.First().TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.REQUEST_USERNAME = listSub.First().REQUEST_USERNAME;
                    rdo.EXECUTE_USERNAME = listSub.First().EXECUTE_USERNAME;
                    rdo.SERVICE_REQ_CODE = listSub.First().SERVICE_REQ_CODE;
                    rdo.REQUEST_LOGINNAME = listSub.First().REQUEST_LOGINNAME;
                    rdo.EXECUTE_LOGINNAME = listSub.First().EXECUTE_LOGINNAME;
                    rdo.BLOCK = listSub.First().BLOCK;
                    rdo.SERVICE_NAME = string.Join(", ", listSub.Select(o => o.SERVICE_NAME).ToList());
                    rdo.BARCODE = string.Join(", ", listSub.Select(o => o.BARCODE).ToList());
                    rdo.SAMPLE_LOGINNAME = string.Join(", ", listSub.Select(o => o.SAMPLE_LOGINNAME).ToList());
                    rdo.SAMPLE_USERNAME = string.Join(", ", listSub.Select(o => o.SAMPLE_USERNAME).ToList());
                    rdo.SAMPLE_NOTE = string.Join(", ", listSub.Select(o => o.SAMPLE_NOTE).ToList());
                    rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().TDL_INTRUCTION_TIME ?? 0);
                    rdo.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().START_TIME ?? 0);
                    rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().FINISH_TIME ?? 0);
                    //rdo.TIME_TEST_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().START_TIME ?? 0);
                    //rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().TDL_INTRUCTION_TIME);
                    rdo.TDL_INTRUCTION_TIME = listSub.First().TDL_INTRUCTION_TIME;
                    rdo.RESULT_TIME = listSub.First().RESULT_TIME;
                    rdo.AMOUNT = listSub.First().AMOUNT;
                    // rdo.SUMAMOUNT = listSub.Sum(item => item.AMOUNT);

                    //rdo.ICD_TEST = listSub.First().ICD_NAME ?? listSub.First().ICD_TEXT;
                    rdo.DIC_TEST_RESULT = new Dictionary<string, string>();
                    var ssTeins = ListSereServExt.Where(f => listSub.Exists(o => o.SERE_SERV_ID == f.SERE_SERV_ID)).ToList();
                    if (ssTeins != null && ssTeins.Count > 0)
                    {
                        var machineExt = ListMachine.Where(p => ssTeins.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machineExt.Count > 0)
                        {
                            rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                        rdo.NOTE = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.NOTE)).Select(p => p.NOTE).ToList());
                        rdo.LEAVEN = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.LEAVEN)).Select(p => p.LEAVEN).ToList());
                        string value = "";
                        if (ssTeins.Exists(o => !string.IsNullOrWhiteSpace(o.DESCRIPTION)))
                        {
                            value = string.Join(";", ssTeins.Select(o => o.DESCRIPTION).Distinct().ToList());
                            var serviceCode = (listSub.FirstOrDefault(o => o.SERE_SERV_ID == ssTeins.First().SERE_SERV_ID) ?? new Mrs00041RDO()).SERVICE_CODE ?? "";
                            if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                            {
                                rdo.DIC_TEST_RESULT[serviceCode] = "";
                            }
                            rdo.DIC_TEST_RESULT[serviceCode] += value;
                        }
                        else
                        {
                            foreach (var item in ssTeins)
                            {
                                var testIndex = ListTestIndex.FirstOrDefault(o => o.ID == item.TEST_INDEX_ID);
                                if (testIndex != null)
                                {

                                    value += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                    var serviceCode = (listSub.FirstOrDefault(o => o.SERE_SERV_ID == item.SERE_SERV_ID) ?? new Mrs00041RDO()).SERVICE_CODE ?? "";
                                    if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                                    {
                                        rdo.DIC_TEST_RESULT[serviceCode] = "";
                                    }
                                    rdo.DIC_TEST_RESULT[serviceCode] += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                    //LogSystem.Info("check");
                                }
                            }
                        }
                        rdo.TEST_RESULT = value;
                    }

                    if (string.IsNullOrWhiteSpace(rdo.EXECUTE_MACHINE_CODE))
                    {
                        var serviceMachine = dicServiceMachine.ContainsKey(group.First().SERVICE_ID) ? dicServiceMachine[group.First().SERVICE_ID] : null;
                        if (serviceMachine != null && serviceMachine.Count > 0)
                        {
                            var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                            if (machine.Count > 0)
                            {
                                rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                                rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                            }
                        }
                    }
                    CalcuatorAge(rdo, listSub.First());
                    IsBhyt(rdo, listSub.First().PATIENT_TYPE_ID ?? 0);
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }


        private void ProcessWithIntructionAndParent(List<Mrs00041RDO> sereServs)
        {
            try
            {
                Dictionary<string, Mrs00041RDO> dicRdo = new Dictionary<string, Mrs00041RDO>();
                foreach (var ss in sereServs)
                {
                    Mrs00041RDO rdo = new Mrs00041RDO();
                    string key = string.Format("{0}_{1}_{2}", ss.TREATMENT_CODE, ss.TDL_INTRUCTION_DATE, ParentIdOf(ss.SERVICE_ID));
                    if (!dicRdo.ContainsKey(key))
                    {
                        rdo.SERVICE_REQ_ID = ss.SERVICE_REQ_ID ?? 0;
                        rdo.PATIENT_NAME = ss.PATIENT_NAME;
                        rdo.PATIENT_CODE = ss.PATIENT_CODE;
                        rdo.TREATMENT_CODE = ss.TREATMENT_CODE;
                        rdo.IN_TIME = ss.IN_TIME;
                        rdo.OUT_TIME = ss.OUT_TIME;
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.IN_TIME);
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.OUT_TIME ?? 0);
                        rdo.TDL_PATIENT_PHONE = ss.TDL_PATIENT_PHONE;
                        var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                        if (ss.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.HEIN_CARD_NUMBER = ss.HEIN_CARD_NUMBER;
                            rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                        }
                        else if (ss.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            rdo.PATIENT_TYPE_NAME_01 = "VP";
                        else
                        {
                            rdo.HEIN_CARD_NUMBER = ss.HEIN_CARD_NUMBER;
                            rdo.PATIENT_TYPE_NAME_01 = "XHH";
                        }
                        var treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == ss.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE();
                        if (treatmentType != null)
                        {
                            rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                            rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                        }
                        rdo.MEDI_ORG_CODE = ss.MEDI_ORG_CODE;
                        rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                        rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                        var tdlPatientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                        rdo.TDL_PATIENT_TYPE_NAME = tdlPatientType.PATIENT_TYPE_NAME;
                        rdo.TDL_PATIENT_TYPE_CODE = tdlPatientType.PATIENT_TYPE_CODE;
                        rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == ss.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ss.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ss.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

                        // rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == ss.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQUEST_USERNAME = ss.REQUEST_USERNAME;
                        rdo.EXECUTE_USERNAME = ss.EXECUTE_USERNAME;
                        rdo.SERVICE_REQ_CODE = ss.SERVICE_REQ_CODE;
                        rdo.REQUEST_LOGINNAME = ss.REQUEST_LOGINNAME;
                        rdo.EXECUTE_LOGINNAME = ss.EXECUTE_LOGINNAME;
                        rdo.BLOCK = ss.BLOCK;
                        IsBhyt(rdo, ss.PATIENT_TYPE_ID ?? 0);
                        CalcuatorAge(rdo, ss);
                        rdo.SERVICE_NAME = ParentNameOf(ss.SERVICE_ID) ?? ss.SERVICE_NAME;
                        rdo.SERVICE_CODE = ss.SERVICE_CODE;
                        rdo.DIC_TEST_RESULT = new Dictionary<string, string>();
                        rdo.TEST_RESULT = "";
                        rdo.AMOUNT = ss.AMOUNT;
                        //LogSystem.Info("code:" + rdo.SERVICE_CODE);
                        dicRdo[key] = rdo;
                    }
                    rdo = dicRdo[key];
                    //rdo.SERVICE_NAME += ss.TDL_SERVICE_NAME;
                    //rdo.TIME_TEST_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.START_TIME ?? 0);
                    //rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.TDL_INTRUCTION_TIME);

                    //rdo.ICD_TEST = ss.ICD_NAME ?? ss.ICD_TEXT;
                    var serviceMachine = dicServiceMachine.ContainsKey(ss.SERVICE_ID) ? dicServiceMachine[ss.SERVICE_ID] : null;
                    if (serviceMachine != null && serviceMachine.Count > 0)
                    {
                        var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machine.Count > 0)
                        {
                            rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }
                    var ssTeins = ListSereServExt.Where(f => ss.SERE_SERV_ID == f.SERE_SERV_ID).ToList();
                    if (ssTeins != null && ssTeins.Count > 0)
                    {
                        var machineExt = ListMachine.Where(p => ssTeins.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                        if (machineExt.Count > 0)
                        {
                            rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                        rdo.NOTE = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.NOTE)).Select(p => p.NOTE).ToList());
                        rdo.LEAVEN = string.Join(";", ssTeins.Where(o => !string.IsNullOrWhiteSpace(o.LEAVEN)).Select(p => p.LEAVEN).ToList());
                        string value = "";
                        foreach (var item in ssTeins)
                        {
                            var testIndex = ListTestIndex.FirstOrDefault(o => o.ID == item.TEST_INDEX_ID);
                            if (testIndex != null)
                            {

                                value += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);

                                var serviceCode = ss.SERVICE_CODE ?? "";
                                if (!rdo.DIC_TEST_RESULT.ContainsKey(serviceCode))
                                {
                                    rdo.DIC_TEST_RESULT[serviceCode] = "";
                                }
                                dicRdo[key].DIC_TEST_RESULT[serviceCode] += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, item.VALUE, testIndex.TEST_INDEX_UNIT_NAME);
                                //LogSystem.Info("check");
                            }
                        }
                        dicRdo[key].TEST_RESULT += value;
                    }
                }

                ListRdo = dicRdo.Values.ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private long ParentIdOf(long serviceId)
        {
            var service = dicParService.ContainsKey(serviceId) ? dicParService[serviceId] : null;
            if (service != null)
            {
                return service.ID;

            }
            else return serviceId;
        }

        private string ParentNameOf(long serviceId)
        {
            var service = dicParService.ContainsKey(serviceId) ? dicParService[serviceId] : null;
            if (service != null)
            {
                return service.SERVICE_NAME;
            }
            return "";
        }
        private string ParentCodeOf(long serviceId)
        {
            var service = dicParService.ContainsKey(serviceId) ? dicParService[serviceId] : null;
            if (service != null)
            {
                return service.SERVICE_CODE;
            }
            return "";
        }
        private void CalcuatorAge(Mrs00041RDO rdo, Mrs00041RDO sereServ)
        {
            try
            {
                rdo.VIR_ADDRESS = sereServ.VIR_ADDRESS;

                int? tuoi = RDOCommon.CalculateAge(sereServ.TDL_PATIENT_DOB ?? 0);
                if (tuoi >= 0)
                {
                    if (sereServ.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(sereServ.TDL_PATIENT_DOB ?? 0);
                        rdo.TDL_PATIENT_GENDER_NAME = "Nam";
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(sereServ.TDL_PATIENT_DOB ?? 0);
                        rdo.TDL_PATIENT_GENDER_NAME = "Nữ";
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

        private void IsBhyt(Mrs00041RDO rdo, long patientTypeId)
        {
            try
            {
                if (patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                    //rdo.PATIENT_TYPE_NAME = "BHYT";
                }
                //if (patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                //rdo.PATIENT_TYPE_NAME = "Viện phí";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00041Filter)this.reportFilter).TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00041Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00041Filter)this.reportFilter).TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00041Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
            if (((Mrs00041Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00041Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            if (((Mrs00041Filter)this.reportFilter).EXECUTE_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => ((Mrs00041Filter)this.reportFilter).EXECUTE_ROOM_ID == o.ID);
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
            }
            if (((Mrs00041Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00041Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderByDescending(o => o.TDL_PATIENT_DOB).ToList());
            objectTag.AddObjectData(store, "Depa", ListRdo.OrderByDescending(o => o.REQUEST_DEPARTMENT_CODE).GroupBy(p => p.REQUEST_DEPARTMENT_ID).Select(q => q.First()).ToList());
            objectTag.AddRelationship(store, "Depa", "Report", "REQUEST_DEPARTMENT_ID", "REQUEST_DEPARTMENT_ID");
            objectTag.AddObjectData(store, "Services", ListSereServ.GroupBy(o => o.SERVICE_CODE).Select(p => p.First()).ToList());

            





           


            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                var machine = this.ListMachine.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("EXECUTE_MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
            }
            if (castFilter.MACHINE_IDs != null)
            {
                var machine = this.ListMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
            }
            if (castFilter.PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
            if (castFilter.TDL_PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("TDL_PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }

        }
    }
}
