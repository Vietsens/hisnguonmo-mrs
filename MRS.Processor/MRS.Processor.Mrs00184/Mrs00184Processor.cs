using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.Filter;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisDepartmentTran;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00184
{
    internal class Mrs00184Processor : AbstractProcessor
    {
        List<VSarReportMrs00184RDO> _listSarReportMrs00184Rdos = new List<VSarReportMrs00184RDO>();
        Mrs00184Filter CastFilter;
        public Mrs00184Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00184Filter);
        }
        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00184Filter)this.reportFilter;
                //cac hsdt trong khoa
                var departmentTranFilter = new HisDepartmentTranViewFilterQuery
                {
                    DEPARTMENT_ID = CastFilter.DEPARTMENT_ID,
                    DEPARTMENT_IN_TIME_FROM = CastFilter.DATE_FROM,
                    DEPARTMENT_IN_TIME_TO = CastFilter.DATE_TO
                };

                var listTreatmentDepartmentViews = new HisDepartmentTranManager(paramGet).GetView(departmentTranFilter);

                var listTreatmentIds = listTreatmentDepartmentViews.Select(s => s.TREATMENT_ID).Distinct().ToList();
                var listPatientTypeAlterViews = new List<V_HIS_PATIENT_TYPE_ALTER>();
                var listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var Ids = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                    filter.TREATMENT_IDs = Ids;
                    //chuyen doi doi tuong
                    var list = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter);
                    if (IsNotNullOrEmpty(list))
                    {
                        listPatientTypeAlter.AddRange(list);
                    }
                }

                foreach (var item in listTreatmentDepartmentViews)
                {
                    listPatientTypeAlterViews.Add(this.PatientTypeAlter(item, listPatientTypeAlter));
                }
                
                //HSDT
                var listTreamentIds = listTreatmentDepartmentViews.Select(s => s.TREATMENT_ID).Distinct().ToList();
                var lisTreatmetViews = new List<V_HIS_TREATMENT>();
                skip = 0;
                while (listTreamentIds.Count - skip > 0)
                {
                    var listIds = listTreamentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyTreatment = new HisTreatmentViewFilterQuery
                    {
                        IDs = listIds
                    };
                    var TreatmentViews = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyTreatment);
                    lisTreatmetViews.AddRange(TreatmentViews);
                }

                ProcessFilterData(listPatientTypeAlterViews, lisTreatmetViews);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter(V_HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            V_HIS_PATIENT_TYPE_ALTER result = new V_HIS_PATIENT_TYPE_ALTER();
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last();
                }
                else
                {
                    patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                        && o.LOG_TIME > departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).First();
                    }
                }
            }
            catch (Exception ex)
            {
                return new V_HIS_PATIENT_TYPE_ALTER();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        protected override bool ProcessData()
        {
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00184Rdos);

        }

        private void ProcessFilterData(List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterViews, List<V_HIS_TREATMENT> lisTreatmetViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00184 ===============================================================");

                var bhytDungTuyen = listPatientTypeAlterViews.Count(s => s.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE);
                var bhytTraiTuyen = listPatientTypeAlterViews.Count(s => s.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE);
                var chuongTrinh = lisTreatmetViews.Count(s => s.PROGRAM_ID.HasValue);
                var vienPhi = listPatientTypeAlterViews.Count(s => s.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE);
                var capCuu = listPatientTypeAlterViews.Count(s => s.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY);
                var gioiThieu = listPatientTypeAlterViews.Count(s => s.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT);
                var henKham = lisTreatmetViews.Count(s => s.APPOINTMENT_CODE != null);
                VSarReportMrs00184RDO rdo = new VSarReportMrs00184RDO()
                {
                    BHYT_KCB = bhytDungTuyen,
                    BHYT_GT = gioiThieu,
                    PROGRAM = chuongTrinh,
                    APPOINTMENT = henKham,
                    CC = capCuu,
                    BHYT_TT = bhytTraiTuyen,
                    VIEN_PHI = vienPhi,
                };
                _listSarReportMrs00184Rdos.Add(rdo);

                LogSystem.Info("Ket thuc xu ly du lieu MRS00184 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        public int BHYT_KCB { get; set; }

        public int BHYT_GT { get; set; }
    }
}
