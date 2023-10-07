using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTranPatiForm;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00134
{
    public class Mrs00134Processor : AbstractProcessor
    {
        List<Mrs00134RDO> _lisMrs00134RDO = new List<Mrs00134RDO>();
        Mrs00134Filter CastFilter;
        private string DEPARTMENT_NAME;
        List<V_HIS_TREATMENT> listTreatmentViews;
        List<V_HIS_SERE_SERV> listSereServViews = new List<V_HIS_SERE_SERV>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTranViews = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterViews = new List<V_HIS_PATIENT_TYPE_ALTER>();

        public Mrs00134Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00134Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00134Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                //-------------------------------------------------------------------------------------------------- HIS_DEPARTMENT
                var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).GetById(CastFilter.DEPARTMENT_ID);
                DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                //-------------------------------------------------------------------------------------------------- V_HIS_TREATMENT
                var metyFilterTreatment = new HisTreatmentViewFilterQuery
                {
                    CREATE_TIME_FROM = CastFilter.DATE_FROM,
                    CREATE_TIME_TO = CastFilter.DATE_TO
                };
                listTreatmentViews = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyFilterTreatment);
                //-------------------------------------------------------------------------------------------------- V_HIS_SERE_SERV
                var listTreatmentViewIds = listTreatmentViews.Select(s => s.ID).ToList();
                var skip = 0;
                while (listTreatmentViewIds.Count - skip > 0)
                {
                    var listIds = listTreatmentViewIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterSereServ = new HisSereServViewFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                    };
                    var sereServViews = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(metyFilterSereServ);
                    listSereServViews.AddRange(sereServViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_DEPARTMENT_TRAN
                var skip1 = 0;
                while (listTreatmentViewIds.Count - skip1 > 0)
                {
                    var listIds = listTreatmentViewIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterDepartment = new HisDepartmentTranViewFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        DEPARTMENT_ID = CastFilter.DEPARTMENT_ID
                    };
                    var departmentTranViews = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartment);
                    listDepartmentTranViews.AddRange(departmentTranViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_PATIENT_TYPE_ALTER
                var skip2 = 0;
                while (listTreatmentViewIds.Count - skip2 > 0)
                {
                    var listIds = listTreatmentViewIds.Skip(skip2).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip2 = skip2 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var PatientTypeAlterViews = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);
                    listPatientTypeAlterViews.AddRange(PatientTypeAlterViews);
                }


                //--------------------------------------------------------------------------------------------------
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00134." +
                        LogUtil.TraceData(
                            LogUtil.GetMemberName(() => paramGet), paramGet));
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
            var result = false;
            try
            {
                ProcessFilterData(listTreatmentViews, listSereServViews, listDepartmentTranViews, listPatientTypeAlterViews);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_TREATMENT> listTreatmentViews, List<V_HIS_SERE_SERV> listSereServs, List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans,
            List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters)
        {
            if (!IsNotNullOrEmpty(listPatientTypeAlters)) return;

            var treatmentIdFromListSereServs = listSereServs.Select(s => s.TDL_TREATMENT_ID ?? 0).ToList();
            var treatmentIdFromListDepartmentTrans = listDepartmentTrans.Select(s => s.TREATMENT_ID).ToList();
            var listTreatmentView = listTreatmentViews.Where(s => treatmentIdFromListSereServs.Contains(s.ID) &&
                treatmentIdFromListDepartmentTrans.Contains(s.ID)).ToList();
            var listGroupByInTimes = listTreatmentView.OrderBy(s => s.IN_TIME).Select(treatmentView => new NewTreatmentView
            {
                DATE_EXAM = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatmentView.IN_TIME),
                TreatmentView = treatmentView
            }).GroupBy(s => s.DATE_EXAM).ToList();

            foreach (var listGroupByInTime in listGroupByInTimes)
            {
                var listTreatmentIds = listGroupByInTime.Select(s => s.TreatmentView.ID).ToList();
                var listPatientTypes = listPatientTypeAlters.Where(s => listTreatmentIds.Contains(s.TREATMENT_ID)).OrderByDescending(o => o.LOG_TIME).GroupBy(g => g.TREATMENT_ID).Select(o => o.First()).ToList();
                var listPatientTypeGroupByTreamentIds = listPatientTypes.GroupBy(s => s.TREATMENT_ID).ToList();
                //lấy đối tượng thanh toán cuối cùng
                var lastTypeOfPatient = listPatientTypeGroupByTreamentIds.Select(listPatientTypeGroupByTreamentId => listPatientTypeGroupByTreamentId.OrderByDescending(s => s.LOG_TIME).FirstOrDefault()).ToList();
                var listPatientTypeIds = lastTypeOfPatient.Select(s => s.ID);
                //đúng tuyến BHYT
                var isRightRoute = listPatientTypes.Count(s =>
                    s.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE &&
                    s.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                //trái tuyến BHYT
                var notRightRoute = listPatientTypes.Count(s =>
                    s.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT &&
                    s.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE);
                //viện phí
                var patientCareersPeople = lastTypeOfPatient.Count(s => s.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE);
                //nhập viện
                var nhapVien = listPatientTypes.Where(s => !string.IsNullOrEmpty(s.HEIN_TREATMENT_TYPE_CODE) &&
                    s.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT).ToList();
                //chuyển tuyến trên
                var totalMoveUp = listGroupByInTime.ToList<NewTreatmentView>().Where(s => s.TreatmentView != null && s.TreatmentView.TRAN_PATI_FORM_ID != null && (s.TreatmentView.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT ||
                    s.TreatmentView.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT)).ToList();
                var rdo = new Mrs00134RDO
                {
                    DATE_EXAM = listGroupByInTime.Key,
                    TOTAL_PATIENT_EXAM_IN_DATE = listGroupByInTime.Count(),
                    IS_RIGHT_ROUTE = isRightRoute,
                    NOT_RIGHT_ROUTE = notRightRoute,
                    PATIENT_CAREERS_PEOPLE = patientCareersPeople,
                    TOTAL_TREAT = nhapVien.Count,
                    TOTAL_MOVE_UP = totalMoveUp.Count
                };
                _lisMrs00134RDO.Add(rdo);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);
                objectTag.AddObjectData(store, "Report", _lisMrs00134RDO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
