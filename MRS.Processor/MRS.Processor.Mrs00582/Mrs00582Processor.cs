using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MRS.MANAGER.Config;
using System.Reflection;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00582
{
    public class Mrs00582Processor : AbstractProcessor
    {
        List<Mrs00582RDO> listRdo = new List<Mrs00582RDO>();
        List<Mrs00582RDO> listRdoService = new List<Mrs00582RDO>();
        List<SereServDO> listHisSereServ = new List<SereServDO>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>();
        long PatientTypeIdBHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        Mrs00582Filter filter = null;
        CommonParam paramGet = new CommonParam();
        string mainKeyGroup = "{0}_{1}";
        public Mrs00582Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00582Filter);
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00582Filter)reportFilter);
            var result = true;
            try
            {
                if (PatientTypeIdBHYT == 0)
                {
                    PatientTypeIdBHYT = 1;
                }
                
             
                //Lấy danh sách dịch vụ yc
                listHisSereServ = new ManagerSql().GetSereServ(this.filter);
              
                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00582",
                    ID = filter.REPORT_TYPE_CAT_ID,
                    IDs = filter.REPORT_TYPE_CAT_IDs,
                };

                reportTypeCats = new HisReportTypeCatManager().Get(HisReportTypeCatfilter);
                Inventec.Common.Logging.LogSystem.Info("reportTypeCats done");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                //khi có điều kiện lọc từ template thì đổi sang key từ template
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SV") && this.dicDataFilter["KEY_GROUP_SV"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_SV"].ToString()))
                {
                    mainKeyGroup = mainKeyGroup.Replace("{1}",this.dicDataFilter["KEY_GROUP_SV"].ToString());
                }
                foreach (var item in reportTypeCats)
                {
                    var listSubSereServ = listHisSereServ.Where(o => o.REPORT_TYPE_CAT_ID == item.ID).ToList() ?? new List<SereServDO>();
                    
                    if (listSubSereServ.Count == 0) continue;
                    var first = listSubSereServ.First();
                    Mrs00582RDO rdo = new Mrs00582RDO();
                    rdo.SERVICE_TYPE_ID = first.TDL_SERVICE_TYPE_ID;
                    rdo.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == first.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                    rdo.CATEGORY_NAME = item.CATEGORY_NAME;
                    rdo.PRICE = first.PRICE;
                    rdo.AMOUNT_BH_NT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_BH_NGT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_VP_NT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID != PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_VP_NGT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID != PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    //rdo.AMOUNT_CA_NT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    //rdo.AMOUNT_CA_NGT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_NN_NT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == 62 && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_NN_NGT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == 62 && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_KSK = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == 43).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_CHILD_LESS5 = listSubSereServ.Where(o =>  Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_LESS5_NT = listSubSereServ.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6 && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_LESS5_NGT = listSubSereServ.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6 && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_OLDSTER_MORE60 = listSubSereServ.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_FEMALE = listSubSereServ.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Sum(s => s.AMOUNT);
                    rdo.AMOUNT_ETHNIC = listSubSereServ.Where(o => o.TDL_PATIENT_ETHNIC_NAME != null && o.TDL_PATIENT_ETHNIC_NAME.Length > 0 && o.TDL_PATIENT_ETHNIC_NAME.ToLower() == "kinh").Sum(s => s.AMOUNT);
                    listRdo.Add(rdo);
                    AddToListRdoDetail(rdo, listSubSereServ);

                    Inventec.Common.Logging.LogSystem.Info(item.CATEGORY_NAME+" done");
                }

                Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);

                
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

        //private long PatientTypeId(long treatmentId)
        //{
        //    return (listHisPatientTypeAlter.Where(q => q.TREATMENT_ID == treatmentId).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).LastOrDefault() ?? new HIS_PATIENT_TYPE_ALTER()).PATIENT_TYPE_ID;
        //}
        private void AddToListRdoDetail(Mrs00582RDO rdo, List<SereServDO> listSubSereServ)
        {
            try
            {
                var group = listSubSereServ.GroupBy(o => string.Format(mainKeyGroup,o.REPORT_TYPE_CAT_ID,o.SERVICE_ID,o.REQUEST_DEPARTMENT_ID,o.EXECUTE_DEPARTMENT_ID)).ToList();
                foreach (var item in group)
                {
                    List<SereServDO> listSub = item.ToList<SereServDO>();
                    var rdo1 = new Mrs00582RDO();
                    rdo1.SERVICE_TYPE_ID = listSub.First().TDL_SERVICE_TYPE_ID;
                    rdo1.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == listSub.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                    rdo1.CATEGORY_NAME = rdo.CATEGORY_NAME;
                    rdo1.PRICE = listSub.First().PRICE;
                    rdo1.SERVICE_NAME = listSub.First().TDL_SERVICE_NAME;
                    rdo1.PR_SERVICE_CODE = listSub.First().PARENT_SERVICE_CODE;
                    rdo1.PR_SERVICE_NAME = listSub.First().PARENT_SERVICE_NAME;
                    var requestDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub.First().REQUEST_DEPARTMENT_ID);
                    if (requestDepartment != null)
                    {
                        rdo1.REQUEST_DEPARTMENT_CODE = requestDepartment.DEPARTMENT_CODE;
                        rdo1.REQUEST_DEPARTMENT_NAME = requestDepartment.DEPARTMENT_NAME;
                    }
                    var executeDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub.First().EXECUTE_DEPARTMENT_ID);
                    if (executeDepartment != null)
                    {
                        rdo1.EXECUTE_DEPARTMENT_CODE = executeDepartment.DEPARTMENT_CODE;
                        rdo1.EXECUTE_DEPARTMENT_NAME = executeDepartment.DEPARTMENT_NAME;
                    }
                    rdo1.AMOUNT_BH_NT = listSub.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_BH_NGT = listSub.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_VP_NT = listSub.Where(o => o.TDL_PATIENT_TYPE_ID != PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_VP_NGT = listSub.Where(o => o.TDL_PATIENT_TYPE_ID != PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    //rdo1.AMOUNT_CA_NT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    //rdo1.AMOUNT_CA_NGT = listSubSereServ.Where(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_NN_NT = listSub.Where(o => o.TDL_PATIENT_TYPE_ID == 62 && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_NN_NGT = listSub.Where(o => o.TDL_PATIENT_TYPE_ID == 62 && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_KSK = listSub.Where(o => o.TDL_PATIENT_TYPE_ID == 43).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_CHILD_LESS5 = listSub.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_LESS5_NT = listSub.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6 && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_LESS5_NGT = listSub.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6 && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_OLDSTER_MORE60 = listSub.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_FEMALE = listSub.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Sum(s => s.AMOUNT);
                    rdo1.AMOUNT_ETHNIC = listSub.Where(o => o.TDL_PATIENT_ETHNIC_NAME != null && o.TDL_PATIENT_ETHNIC_NAME.Length > 0 && o.TDL_PATIENT_ETHNIC_NAME.ToLower() == "kinh").Sum(s => s.AMOUNT);
                    listRdoService.Add(rdo1);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_FROM ?? filter.START_TIME_FROM ?? filter.FINISH_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_TO ?? filter.START_TIME_TO ?? filter.FINISH_TIME_TO ?? 0));

            dicSingleTag.Add("REPORT_NAME", string.Join(",", reportTypeCats.Select(o => o.CATEGORY_NAME).ToList()));

            Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
            objectTag.AddObjectData(store, "Parent", listRdo.OrderBy(o => o.SERVICE_TYPE_ID).GroupBy(p => p.SERVICE_TYPE_ID).Select(q => q.First()).ToList());
            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.SERVICE_TYPE_ID).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            objectTag.AddObjectData(store, "ReportDetail", listRdoService.OrderBy(o => o.SERVICE_TYPE_ID).ToList());
            objectTag.AddRelationship(store, "Report", "ReportDetail", "CATEGORY_NAME", "CATEGORY_NAME");

            //ReportParent
            objectTag.AddObjectData(store, "ReportParent", listRdo.OrderBy(o => o.SERVICE_TYPE_ID).GroupBy(p => p.SERVICE_TYPE_ID).Select(q => q.First()).ToList());
            objectTag.AddObjectData(store, "Report1", listRdo.OrderBy(o => o.SERVICE_TYPE_ID).ToList());
            objectTag.AddRelationship(store, "ReportParent", "Report1", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            objectTag.AddObjectData(store, "Report1Detail", listRdoService.OrderBy(o => o.SERVICE_TYPE_ID).ToList());
            objectTag.AddRelationship(store, "Report1", "Report1Detail", "CATEGORY_NAME", "CATEGORY_NAME");

        }
    }
}
