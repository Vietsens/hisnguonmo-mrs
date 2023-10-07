using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.DAO.Sql;

namespace MRS.Processor.Mrs00805
{
    public class Mrs00805Processor : AbstractProcessor
    {
        public Mrs00805Filter filter;
        public List<Mrs00805RDO> ListRdo = new List<Mrs00805RDO>();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>();
        List<HIS_PATIENT_CLASSIFY> listPatientClassify = new List<HIS_PATIENT_CLASSIFY>();
        public Mrs00805Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00805Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            filter = ((Mrs00805Filter)this.reportFilter);
            try
            {
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                treatmentFilter.CREATE_TIME_TO = filter.TIME_TO;
                ListTreatment = new HisTreatmentManager(param).Get(treatmentFilter);
                if (filter.DEPARTMENT_IDs != null)
                {
                    ListTreatment.Where(x => filter.DEPARTMENT_IDs.Contains(x.END_DEPARTMENT_ID??0)).ToList();
                }
                ListPatientType = new HisPatientTypeManager(new CommonParam()).Get(new HisPatientTypeFilterQuery());
                string query = "SELECT * FROM HIS_PATIENT_CLASSIFY";
                listPatientClassify =  new SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query);
                ListDepartment = new HisDepartmentManager(new CommonParam()).Get(new HisDepartmentFilterQuery());
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                var groups = ListTreatment.GroupBy(x => new { x.END_DEPARTMENT_ID}).ToList();
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    foreach (var item in groups)
                    {
                        Mrs00805RDO rdo = new Mrs00805RDO();
                        var patient_type = ListPatientType.ToList();
                        var depart = ListDepartment.Where(x => x.ID == item.First().LAST_DEPARTMENT_ID).First();
                        var patientClassifyIds = listPatientClassify.Select(x => x.ID).ToList();
                        if (depart!=null)
                        {
                            rdo.DEPARTMENT_CODE = depart.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = depart.DEPARTMENT_NAME;
                        }
                        rdo.REALITY_PATIENT_COUNT = depart.REALITY_PATIENT_COUNT??0;
                        rdo.THEORY_PATIENT_COUNT = depart.THEORY_PATIENT_COUNT??0;
                        rdo.TREATMENT_DAY_COUNT = item.Sum(x=>x.TREATMENT_DAY_COUNT??0);
                        rdo.COUNT_NOITRU = item.Where(x=>x.TDL_TREATMENT_TYPE_ID== IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                        rdo.COUNT_CV = item.Where(x=>x.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Count();
                        var countRaVien = item.Where(x => x.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN).Count();
                        var countDieuTri = item.Where(x => x.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                        rdo.COUNT_END = item.Where(x => x.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN).Count();
                        rdo.COUNT_OLD = item.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                        rdo.COUNT_NEW = item.Count() - countRaVien - rdo.COUNT_CV;
                        rdo.COUNT_CA_BHYT = item.Where(x=>x.TDL_PATIENT_TYPE_ID==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT&& listPatientClassify.Where(o=>o.PATIENT_CLASSIFY_NAME=="Công an").Select(p=>p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID??0)).Count();
                        rdo.COUNT_CBCA = item.Where(x => x.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Công an").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();
                        rdo.COUNT_BHYT_KHAC = item.Where(x => x.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Count() - rdo.COUNT_CA_BHYT;
                        rdo.COUNT_CB_DUONG_CHUC = item.Where(x => listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Cán bộ đương chức").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();
                        rdo.COUNT_CB_HUU_TRI = item.Where(x => listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Cán bộ hưu trí").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();
                        rdo.COUNT_NGUOI_NUOC_NGOAI = item.Where(x => listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Nước ngoài").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();
                        rdo.COUNT_PHAM_NHAN = item.Where(x =>listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Phạm nhân").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();
                        rdo.COUNT_VP_CHINH_SACH = item.Where(x => patient_type.Where(z=>z.PATIENT_TYPE_NAME=="Viện phí").Select(z=>z.ID).ToList().Contains(x.TDL_PATIENT_TYPE_ID??0)&& listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Chính sách").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();
                        rdo.COUNT_VP_DAN = item.Where(x => patient_type.Where(z => z.PATIENT_TYPE_NAME == "Viện phí").Select(z => z.ID).ToList().Contains(x.TDL_PATIENT_TYPE_ID ?? 0) && listPatientClassify.Where(o => o.PATIENT_CLASSIFY_NAME == "Dân").Select(p => p.ID).ToList().Contains(x.TDL_PATIENT_CLASSIFY_ID ?? 0)).Count();

                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FORM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
