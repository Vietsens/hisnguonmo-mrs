using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
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
using MOS.MANAGER.HisPtttMethod;
using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaProvince.Get;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Core.SdaCommune.Get;
using SDA.MANAGER.Manager;

namespace MRS.Processor.Mrs00300
{
    class Mrs00300Processor : AbstractProcessor
    {
        Mrs00300Filter castFilter = null;

        List<Mrs00300RDO> listRdo = new List<Mrs00300RDO>();
        List<HIS_PTTT_METHOD> listPtttMethod = new List<HIS_PTTT_METHOD>();
        public Mrs00300Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_SDA_PROVINCE> listProvince = new List<V_SDA_PROVINCE>();
        List<V_SDA_DISTRICT> listDistrict = new List<V_SDA_DISTRICT>();
        List<V_SDA_COMMUNE> listCommune = new List<V_SDA_COMMUNE>();

        List<V_HIS_TREATMENT> listTreatment = null;
        HIS_BRANCH branch = null;
        HIS_DEPARTMENT department = null;
        List<PTTT_INFO> listPtttInfoAll = new List<PTTT_INFO>();
        long PtttGroupIdGroup1;
        long PtttGroupIdGroup2;
        long PtttGroupIdGroup3;
        long PtttGroupIdGroup4;
        public override Type FilterType()
        {
            return typeof(Mrs00300Filter);
        }

        protected override bool GetData()
        {
            bool valid = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00300Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu GetData Mrs00300, V_HIS_TREATMENT: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                listProvince = new SdaProvinceManager(new CommonParam()).Get<List<V_SDA_PROVINCE>>(new SdaProvinceViewFilterQuery()) ?? new List<V_SDA_PROVINCE>();
                listDistrict = new SdaDistrictManager(new CommonParam()).Get<List<V_SDA_DISTRICT>>(new SdaDistrictViewFilterQuery()) ?? new List<V_SDA_DISTRICT>();
                listCommune = new SdaCommuneManager(new CommonParam()).Get<List<V_SDA_COMMUNE>>(new SdaCommuneViewFilterQuery()) ?? new List<V_SDA_COMMUNE>();
                this.PtttGroupIdGroup1 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1;
                this.PtttGroupIdGroup2 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2;
                this.PtttGroupIdGroup3 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3;
                this.PtttGroupIdGroup4 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4;

                if (castFilter.BRANCH_ID.HasValue)
                {
                    branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID.Value);
                    if (branch == null)
                    {
                        //BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__ThieuThongTinBatBuoc);
                        throw new Exception("Kho lay duoc branch theo id MRS00300: " + castFilter.BRANCH_ID);
                    }
                }

                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID.Value);
                    if (department == null)
                    {
                        //BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__ThieuThongTinBatBuoc);
                        throw new Exception("Kho lay duoc department theo id MRS00300 : " + castFilter.DEPARTMENT_ID);
                    }
                    if (branch != null && department.BRANCH_ID != branch.ID)
                    {
                        //BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                        throw new Exception("Khoa khong thuoc chi nhanh nguoi dung chon MRS00300: " + Inventec.Common.Logging.LogUtil.TraceData("Department", department) + Inventec.Common.Logging.LogUtil.TraceData("Branch", branch));
                    }
                }

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                if (castFilter.TRUE_FALSE.HasValue && !castFilter.TRUE_FALSE.Value)
                {
                    treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                }
                else
                {
                    treatmentFilter.STORE_TIME_FROM = castFilter.TIME_FROM;
                    treatmentFilter.STORE_TIME_TO = castFilter.TIME_TO;
                }
                treatmentFilter.END_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);
                if (paramGet.HasException)
                {
                    //BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00300: V_HIS_TREATMENT");
                }
                listPtttMethod = this.GetAllPtttMethod();
                if (this.castFilter.TAKE_PTTT_INFO == true)
                {
                    this.listPtttInfoAll = new ManagerSql().GetSum(this.castFilter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {



                if (IsNotNullOrEmpty(listTreatment))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = listTreatment.Count;
                    while (count > 0)
                    {

                        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
                        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>();
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = listTreatment.Skip(start).Take(limit).ToList();

                        List<PTTT_INFO> listPtttInfo = listPtttInfoAll.Where(o => listSub.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        patyAlterFilter.TREATMENT_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU };
                        var listPatyAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter);

                        HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery();
                        serviceReqFilter.TREATMENT_IDs = patyAlterFilter.TREATMENT_IDs;
                        serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        var listServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(serviceReqFilter);

                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00300, V_HIS_PATIENT_TYPE_ALTER,V_HIS_TRAN_PATI,V_HIS_SERVICE_REQ");
                        }

                        if (IsNotNullOrEmpty(listPatyAlter))
                        {
                            listPatyAlter = listPatyAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                            var Groups = listPatyAlter.GroupBy(g => g.TREATMENT_ID).ToList();
                            foreach (var group in Groups)
                            {
                                var listGroup = group.ToList<V_HIS_PATIENT_TYPE_ALTER>();
                                var currentPatyAlter = listGroup.First();
                                if (currentPatyAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                    continue;
                                dicCurrentPatyAlter[currentPatyAlter.TREATMENT_ID] = currentPatyAlter;
                            }
                        }

                        List<long> ExamDepartmentIds = HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM ?? new List<long>();
                        if (IsNotNullOrEmpty(listServiceReq))
                        {
                            listServiceReq = listServiceReq.OrderBy(o => o.CREATE_TIME).ToList();
                            var Groups = listServiceReq.GroupBy(g => g.TREATMENT_ID).ToList();
                            foreach (var group in Groups)
                            {
                                var listGroup = group.ToList<V_HIS_SERVICE_REQ>();
                                foreach (var item in listGroup)
                                {
                                    if (ExamDepartmentIds.Contains(item.EXECUTE_DEPARTMENT_ID))
                                    {
                                        dicServiceReq[item.TREATMENT_ID] = item;
                                        break;
                                    }
                                }
                            }
                        }

                        this.ProcessDataDetail(listSub, dicCurrentPatyAlter, dicServiceReq, listPtttInfo);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        void ProcessDataDetail(List<V_HIS_TREATMENT> _listTreatment, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> _dicCurrentPatyAlter, Dictionary<long, V_HIS_SERVICE_REQ> _dicServiceReq, List<PTTT_INFO> _listPtttInfo)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("danh sach in: " + string.Join(", ", _listTreatment.Select(o => o.TREATMENT_CODE).ToList()));
                foreach (var treatment in _listTreatment)
                {
                    if (this.castFilter.ADD_DEATH != true)
                    {
                        if (treatment.TREATMENT_END_TYPE_ID.HasValue && treatment.TREATMENT_END_TYPE_ID.Value == MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH)
                            continue;
                    }

                    if (!_dicCurrentPatyAlter.ContainsKey(treatment.ID))
                        continue;
                    Mrs00300RDO rdo = new Mrs00300RDO(treatment);

                    if (!string.IsNullOrEmpty(treatment.TDL_PATIENT_COMMUNE_CODE))
                    {
                        var sdaCommune = (listCommune ?? new List<V_SDA_COMMUNE>()).FirstOrDefault(o => o.COMMUNE_CODE == treatment.TDL_PATIENT_COMMUNE_CODE);
                        if (sdaCommune != null)
                        {
                            if (!string.IsNullOrEmpty(sdaCommune.INITIAL_NAME) && (sdaCommune.INITIAL_NAME.ToLower().Contains("phường") || sdaCommune.INITIAL_NAME.ToLower().Contains("thị trấn")) || !string.IsNullOrEmpty(sdaCommune.DISTRICT_INITIAL_NAME) && (sdaCommune.DISTRICT_INITIAL_NAME.ToLower().Contains("quận") || sdaCommune.DISTRICT_INITIAL_NAME.ToLower().Contains("thành phố") || sdaCommune.DISTRICT_INITIAL_NAME.ToLower().Contains("thị xã")))
                            {
                                rdo.IS_CITY = "X";
                            }
                            //else
                            //{
                            //    rdo.IS_COUNTRYSIDE = "X";
                            //}
                        }

                    }
                    if (_dicCurrentPatyAlter[treatment.ID].PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.IS_BHYT = "X";
                    }
                    if (treatment.TRANSFER_IN_MEDI_ORG_CODE != null)
                    {
                        rdo.TRAN_PATI_NAME = treatment.TRANSFER_IN_MEDI_ORG_NAME;
                        rdo.TRAN_PATI_CODE = treatment.TRANSFER_IN_MEDI_ORG_CODE;
                        rdo.DIAGNOSE_DOWNLINE = treatment.TRANSFER_IN_ICD_NAME;
                        rdo.TRAN_ICD_CODE = treatment.ICD_CODE;
                        rdo.TRAN_ICD_NAME = treatment.ICD_NAME;
                        rdo.TRAN_ICD_SUB_CODE = treatment.ICD_SUB_CODE;
                        rdo.TRAN_ICD_TEXT = treatment.ICD_TEXT;
                    }

                    if (_dicServiceReq.ContainsKey(treatment.ID))
                    {
                        var req = _dicServiceReq[treatment.ID];
                        rdo.DIAGNOSE_EXAM = req.ICD_NAME;
                        rdo.TRAN_ICD_CODE = req.ICD_CODE;
                        rdo.TRAN_ICD_NAME = req.ICD_NAME;
                        rdo.TRAN_ICD_SUB_CODE = req.ICD_SUB_CODE;
                        rdo.TRAN_ICD_TEXT = req.ICD_TEXT;
                    }
                    if (_listPtttInfo != null)
                    {
                        var patientInfoSub = _listPtttInfo.Where(o => o.ID == treatment.ID).ToList();
                        if (patientInfoSub != null && patientInfoSub.Count > 0)
                        {
                            rdo.RELATIVE_MOBILE = patientInfoSub.First().RELATIVE_MOBILE;
                            rdo.RELATIVE_PHONE = patientInfoSub.First().RELATIVE_PHONE;
                        }
                        var ptttInfoSub = _listPtttInfo.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                        if (ptttInfoSub != null)
                        {
                            rdo.SURG_PTTT_GROUP_NAMEs = string.Join(", ", HisPtttGroupCFG.PTTT_GROUPs.Where(o => ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == o.ID && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)).OrderBy(r => r.PTTT_GROUP_CODE).Select(q => q.PTTT_GROUP_NAME).Distinct().ToList());
                            rdo.IS_SURG_PTTT_GROUP_1 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup1 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT) ? "X" : "";
                            rdo.IS_SURG_PTTT_GROUP_2 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup2 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT) ? "X" : "";
                            rdo.IS_SURG_PTTT_GROUP_3 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup3 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT) ? "X" : "";
                            rdo.IS_SURG_PTTT_GROUP_4 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup4 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT) ? "X" : "";
                            rdo.MISU_PTTT_GROUP_NAMEs = string.Join(", ", HisPtttGroupCFG.PTTT_GROUPs.Where(o => ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == o.ID && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)).OrderBy(r => r.PTTT_GROUP_CODE).Select(q => q.PTTT_GROUP_NAME).Distinct().ToList());
                            rdo.IS_MISU_PTTT_GROUP_1 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup1 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT) ? "X" : "";
                            rdo.IS_MISU_PTTT_GROUP_2 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup2 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT) ? "X" : "";
                            rdo.IS_MISU_PTTT_GROUP_3 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup3 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT) ? "X" : "";
                            rdo.IS_MISU_PTTT_GROUP_4 = ptttInfoSub.Exists(p => p.PTTT_GROUP_ID == PtttGroupIdGroup4 && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT) ? "X" : "";
                            if (listPtttMethod != null)
                            {
                                rdo.PTTT_METHOD_NAMEs = string.Join(", ", listPtttMethod.Where(o => ptttInfoSub.Exists(p => p.PTTT_METHOD_ID == o.ID)).OrderBy(r => r.PTTT_METHOD_CODE).Select(q => q.PTTT_METHOD_NAME).Distinct().ToList());
                            }
                            rdo.EMOTIONLESS_METHOD_NAMEs = string.Join(", ", HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.Where(o => ptttInfoSub.Exists(p => p.EMOTIONLESS_METHOD_ID == o.ID)).OrderBy(r => r.EMOTIONLESS_METHOD_CODE).Select(q => q.EMOTIONLESS_METHOD_NAME).Distinct().ToList());
                            rdo.TDL_SERVICE_NAMEs = string.Join(", ", ptttInfoSub.Select(q => q.TDL_SERVICE_NAME).Distinct().ToList());
                            rdo.MANNERs = string.Join(", ", ptttInfoSub.Select(q => q.MANNER).Distinct().ToList());
                            rdo.USERNAMEs = string.Join(", ", ptttInfoSub.Where(p => !string.IsNullOrWhiteSpace(p.USERNAME)).Select(o => o.USERNAME).Distinct().ToList());
                        }
                    }
                    listRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                listRdo = listRdo.OrderBy(o => o.TREATMENT_CODE).ToList();
                dicSingleTag.Add("OUT_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("OUT_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (branch != null)
                {
                    dicSingleTag.Add("BRANCH_NAME", branch.BRANCH_NAME);
                }
                if (department != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
                }
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAMEs", string.Join(",", (HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList())));
                }

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.STORE_CODE).ToList());
                objectTag.AddObjectData(store, "Department", listRdo.GroupBy(o => o.END_DEPARTMENT_ID).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Department", "Report", "END_DEPARTMENT_ID", "END_DEPARTMENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<MOS.EFMODEL.DataModels.HIS_PTTT_METHOD> GetAllPtttMethod()
        {
            List<MOS.EFMODEL.DataModels.HIS_PTTT_METHOD> result = null;
            try
            {
                HisPtttMethodFilterQuery filter = new HisPtttMethodFilterQuery();
                result = new HisPtttMethodManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

    }
}
