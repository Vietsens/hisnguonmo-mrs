using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisTreatment;
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

namespace MRS.Processor.Mrs00136
{
    public class Mrs00136Processor : AbstractProcessor
    {
        List<Mrs00136RDO> _lisMrs00136RDO = new List<Mrs00136RDO>();
        Mrs00136Filter CastFilter;
        List<V_HIS_TREATMENT> listTreatmentViews = new List<V_HIS_TREATMENT>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTranViews = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterViews = new List<V_HIS_PATIENT_TYPE_ALTER>();

        public Mrs00136Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00136Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00136Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter MRS00136: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                ////////////////////////////////////////////////////////////////////////////////// -HIS_ICD
                var listIcdCodeGroups = new List<string>();
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_01);
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_02);
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_03);
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_04);
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_05);
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_06);
                listIcdCodeGroups.AddRange(HisIcdMedicalGroupCFG.ReportMdeicalGroup_07);

                var listIcd = GetIcd(listIcdCodeGroups);
                var listIcdCodes = listIcd.Select(s => s.ICD_CODE).ToList();
                if (IsNotNullOrEmpty(listIcd))
                {
                    listTreatmentViews = GetTreatment(listIcdCodes);
                }
                var listTreatmentIds = listTreatmentViews.Select(s => s.ID).Distinct().ToList();

                listPatientTypeAlterViews = GetPatientTypeAlter(listTreatmentIds);
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    listDepartmentTranViews = GetDepartmentTran(listTreatmentIds);

                }
                //////////////////////////////////////////////////////////////////////////////////
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00136." +
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

        private List<HIS_ICD> GetIcd(List<string> listIcdCodes)
        {
            List<HIS_ICD> result = new List<HIS_ICD>();
            try
            {
                CommonParam paramGet = new CommonParam();
                var metyFilterIcd = new HisIcdFilterQuery
                {
                    ICD_CODEs = listIcdCodes
                };
                result = new MOS.MANAGER.HisIcd.HisIcdManager(paramGet).Get(metyFilterIcd);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<HIS_ICD>();
            }
            return result;
        }
        ////////////////////////////////////////////////////////////////////////////////// -V_HIS_TREATMENT

        private List<V_HIS_TREATMENT> GetTreatment(List<string> listIcdCodes)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam();
                //var skip = 0;
                //while (listIcdCodes.Count - skip > 0)
                //{
                //    var listCodes = listIcdCodes.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //    var metyFilterTreatment = new HisTreatmentViewFilterQuery
                //    {
                //        ICD_CODEs = listCodes,
                //        IN_TIME_FROM = CastFilter.DATE_FROM,
                //        IN_TIME_TO = CastFilter.DATE_TO
                //    };
                //    var treatmentViews = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyFilterTreatment);
                //    result.AddRange(treatmentViews);
                //}

                var metyFilterTreatment = new HisTreatmentViewFilterQuery
                {
                    //ICD_CODEs = listCodes,
                    IN_TIME_FROM = CastFilter.DATE_FROM,
                    IN_TIME_TO = CastFilter.DATE_TO
                };
                var treatmentViews = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(metyFilterTreatment);
                if (IsNotNullOrEmpty(listIcdCodes))
                    treatmentViews = treatmentViews.Where(o => listIcdCodes.Contains(o.ICD_CODE)).ToList();

                if (IsNotNullOrEmpty(treatmentViews))
                    result.AddRange(treatmentViews);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<V_HIS_TREATMENT>();
            }
            return result;
        }

        //////////////////////////////////////////////////////////////////////////////////// -V_HIS_DEPARTMENT_TRAN
        private List<V_HIS_DEPARTMENT_TRAN> GetDepartmentTran(List<long> listTreatmentId)
        {
            List<V_HIS_DEPARTMENT_TRAN> result = new List<V_HIS_DEPARTMENT_TRAN>();
            try
            {
                CommonParam paramGet = new CommonParam();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterDepartmentTran = new HisDepartmentTranViewFilterQuery
                    {
                        TREATMENT_IDs = listIDs,
                        DEPARTMENT_ID = CastFilter.DEPARTMENT_ID
                    };
                    var departmentTranViews = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartmentTran);
                    result.AddRange(departmentTranViews);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<V_HIS_DEPARTMENT_TRAN>();
            }
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////// -V_HIS_PATIENT_TYPE_ALTER
        private List<V_HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(List<long> listTreatmentId)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = new List<V_HIS_PATIENT_TYPE_ALTER>();
            try
            {
                CommonParam paramGet = new CommonParam();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_IDs = listIDs
                    };
                    var PatientTypeAlterVews = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);
                    result.AddRange(PatientTypeAlterVews);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<V_HIS_PATIENT_TYPE_ALTER>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (IsNotNullOrEmpty(listDepartmentTranViews))
                {
                    ProcessFilterData(listTreatmentViews, listDepartmentTranViews, listPatientTypeAlterViews);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_TREATMENT> listTreatmentViews,
            List<V_HIS_DEPARTMENT_TRAN> listDepartmentTranViews,
            List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterViews)
        {
            var listMedicalGroupName = new List<string>
            {
                "Hô hấp (Phổi màng phổi)",
                "Dạng lao khác (thận, màng não, hạch, ...)",
                "Di chứng lao (tâm phế mạn)",
                "U phổi, phế quản, viêm phổi",
                "Viêm phế quản, viêm phổi",
                "Giãn phế quản, hen phế quản, COPD",
                "Bệnh khác của đường hô hấp (viêm hạch, viêm khớp, viêm đại tràng, ...)"
            };
            var listMedicalGroupCode = new List<string>
            {
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_01),
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_02),
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_03),
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_04),
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_05),
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_06),
                string.Join(" - ", HisIcdMedicalGroupCFG.ReportMdeicalGroup_07),
            };

            var treatmentIdFromDepartmentTrans = listDepartmentTranViews.Select(s => s.TREATMENT_ID).ToList();
            var treatmentViews = listTreatmentViews.Where(s => treatmentIdFromDepartmentTrans.Contains(s.ID)).ToList();
            for (var i = 0; i < listMedicalGroupName.Count; i++)
            {
                var treatments = treatmentViews.Where(s => listMedicalGroupCode[i].Contains(s.ICD_CODE)).ToList();
                var totalMedicalExamination = listPatientTypeAlterViews.Where(s => treatments.Select(ss => ss.ID).Contains(s.TREATMENT_ID)).ToList();
                var yearNow = DateTime.Now.Year;
                var year = yearNow - HisIcdMedicalGroupCFG.ReportAgeChildren;
                var dateAgeChildren = long.Parse(string.Format("{0}0101000000", year));
                var totalChildren = treatments.Where(s => s.TDL_PATIENT_DOB >= dateAgeChildren).ToList();
                var rdo = new Mrs00136RDO
                {
                    MEDICAL_GROUP_NAME = listMedicalGroupName[i],
                    MEDICAL_GROUP_CODE = listMedicalGroupCode[i],
                    TOTAL_MEDICAL_EXAMINATION = totalMedicalExamination.Count,
                    TOTAL_CHILDREN = totalChildren.Count,
                    TOTAL_DEATH = treatments.Where(o => o.TREATMENT_END_TYPE_ID != null && o.TREATMENT_END_TYPE_ID == HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH).ToList().Count
                };
                _lisMrs00136RDO.Add(rdo);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                dicSingleTag.Add("AGE_CHILDREN", HisIcdMedicalGroupCFG.ReportAgeChildren);
                objectTag.AddObjectData(store, "Report", _lisMrs00136RDO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
