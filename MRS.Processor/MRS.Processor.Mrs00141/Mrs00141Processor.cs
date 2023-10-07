using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00141
{
    public class Mrs00141Processor : AbstractProcessor
    {
        Mrs00141Filter castFilter = null; 

        //Du lieu cap cuu phan loai theo thoi gian
        private List<Mrs00141RDO> others = new List<Mrs00141RDO>(); 
        private Mrs00141RDO bhtq = new Mrs00141RDO(); 
        private Mrs00141RDO bhqd = new Mrs00141RDO(); 
        private Mrs00141RDO otherBhyt = new Mrs00141RDO(); 

        List<V_HIS_DEPARTMENT_TRAN> departmentTrans; 
        List<HIS_SERVICE_REQ> examServiceReqs; 
        List<V_HIS_SERE_SERV> surgSereServs; 
        List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters; 
        List<HIS_SERE_SERV_PTTT> sereServPttts; 

        public Mrs00141Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00141Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00141Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPARTMENT_TRAN, HIS_DEPARTMENT MRS00141 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                //Lay danh sach vao khoa
                HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery(); 
                departmentTranFilter.DEPARTMENT_IN_TIME_FROM = castFilter.TIME_FROM; 
                departmentTranFilter.DEPARTMENT_IN_TIME_TO = castFilter.TIME_TO; 
                departmentTranFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                departmentTrans = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(departmentTranFilter); 

                //Lay danh sach kham theo thoi gian nguoi dung truyen vao
                HisServiceReqFilterQuery examServiceReqFilter = new HisServiceReqFilterQuery(); 
                examServiceReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                examServiceReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                examServiceReqFilter.EXECUTE_DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                examServiceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                examServiceReqFilter.HAS_EXECUTE = true;
                examServiceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(examServiceReqFilter); 

                //Lay danh sach phau thuat theo thoi gian nguoi dung truyen vao
                HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                sereServViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sereServViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                sereServViewFilter.EXECUTE_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                sereServViewFilter.HAS_EXECUTE = true; 
                sereServViewFilter.SERVICE_TYPE_IDs = new List<long>(){IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT,IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT}; 
                surgSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServViewFilter); 

                //Lay danh sach his_sere_serv_pttt tuong ung
                List<long> surgSereServIds = surgSereServs != null ? surgSereServs.Select(o => o.ID).ToList() : null;
                sereServPttts = new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(paramGet).GetBySereServIds(surgSereServIds); 

                //Lay danh sach patient_tye_alter tuong ung
                List<long> treatmentIds = new List<long>(); 
                if (IsNotNullOrEmpty(departmentTrans))
                {
                    treatmentIds.AddRange(departmentTrans.Select(o => o.TREATMENT_ID).ToList()); 
                }
                if (IsNotNullOrEmpty(examServiceReqs))
                {
                    treatmentIds.AddRange(examServiceReqs.Select(o => o.TREATMENT_ID).ToList()); 
                }
                if (IsNotNullOrEmpty(surgSereServs))
                {
                    treatmentIds.AddRange(surgSereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).ToList()); 
                }
                treatmentIds = treatmentIds.Distinct().ToList(); 

                PatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(treatmentIds); 

                //Lay danh sach paty_alter_bhyt tuong ung
                List<long> PatientTypeAlterIds = IsNotNullOrEmpty(PatientTypeAlters) ? PatientTypeAlters.Select(o => o.ID).ToList() : null; 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai trong qua trinh lay du lieu V_HIS_DEPARTMENT_TRAN, HIS_DEPARTMENT MRS00123." + Inventec.Common.Logging.LogUtil.TraceData("castFilter", castFilter)); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void PrepareData(List<V_HIS_DEPARTMENT_TRAN> departmentTrans, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters,
            List<HIS_SERVICE_REQ> examServiceReqs, List<V_HIS_SERE_SERV> surgSereServs, List<HIS_SERE_SERV_PTTT> sereServPttts)
        {
            //Khoi tao ban dau cho du lieu "others"
            this.InitOthers(ref this.others); 

            if (IsNotNullOrEmpty(PatientTypeAlters))
            {
                this.ProcessExamServiceReq(PatientTypeAlters, examServiceReqs); 
                this.ProcessSurgServiceReq(PatientTypeAlters, surgSereServs, sereServPttts); 
                this.ProcessDepartmentTran(departmentTrans, PatientTypeAlters); 
            }
        }

        private void ProcessDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> departmentTrans, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters)
        {
            if (IsNotNullOrEmpty(departmentTrans))
            {
                foreach (V_HIS_DEPARTMENT_TRAN d in departmentTrans)
                {
                    //Ban ghi dien doi tuong truoc khi chi dinh
                    V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = null; 
                    string HeinCardNumber = ""; 
                    this.GetPatientTypeAlterByTime(PatientTypeAlters, d.TREATMENT_ID, d.DEPARTMENT_IN_TIME ?? 0, ref PatientTypeAlter, ref HeinCardNumber); 
                    Mrs00141RDO rdo = this.GetRdoToUse(HeinCardNumber, PatientTypeAlter); 
                    if (rdo != null)
                    {
                        if (this.IsIn(PatientTypeAlter))
                        {
                            rdo.TREAT_IN_COUNT++; 
                        }
                        else
                        {
                            rdo.TREAT_OUT_COUNT++; 
                        }
                    }
                }
            }
        }

        private void ProcessSurgServiceReq(List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, List<V_HIS_SERE_SERV> surgSereServs, List<HIS_SERE_SERV_PTTT> sereServPttts)
        {
            if (IsNotNullOrEmpty(sereServPttts))
            {
                foreach (V_HIS_SERE_SERV sere in surgSereServs)
                {
                    //Ban ghi dien doi tuong truoc khi ket thuc
                    V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = null; 
                    string HeinCardNumber = ""; 
                    this.GetPatientTypeAlterByTime(PatientTypeAlters, sere.TDL_TREATMENT_ID ?? 0, sere.TDL_INTRUCTION_TIME, ref PatientTypeAlter, ref HeinCardNumber); 

                    HIS_SERE_SERV_PTTT pttt = sereServPttts.Where(o => o.SERE_SERV_ID == sere.ID).FirstOrDefault(); 

                    Mrs00141RDO rdo = this.GetRdoToUse(HeinCardNumber, PatientTypeAlter); 

                    if (rdo != null && pttt != null)
                    {
                        if (this.IsIn(PatientTypeAlter))
                        {
                            if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1)
                            {
                                rdo.SURG_L1_IN_COUNT++; 
                            }
                            else if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2)
                            {
                                rdo.SURG_L2_IN_COUNT++; 
                            }
                            else if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3)
                            {
                                rdo.SURG_L3_IN_COUNT++; 
                            }
                            else if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4)
                            {
                                rdo.SURG_L4_IN_COUNT++; 
                            }
                        }
                        else if (!this.IsIn(PatientTypeAlter))
                        {
                            if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1)
                            {
                                rdo.SURG_L1_OUT_COUNT++; 
                            }
                            else if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2)
                            {
                                rdo.SURG_L2_OUT_COUNT++; 
                            }
                            else if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3)
                            {
                                rdo.SURG_L3_OUT_COUNT++; 
                            }
                            else if (pttt.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4)
                            {
                                rdo.SURG_L4_OUT_COUNT++; 
                            }
                        }
                    }
                }
            }
        }

        private void ProcessExamServiceReq(List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, List<HIS_SERVICE_REQ> examServiceReqs)
        {
            if (IsNotNullOrEmpty(examServiceReqs))
            {
                foreach (HIS_SERVICE_REQ ex in examServiceReqs)
                {
                    //Ban ghi dien doi tuong truoc khi chi dinh
                    V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = null; 
                    string HeinCardNumber = ""; 
                    this.GetPatientTypeAlterByTime(PatientTypeAlters, ex.TREATMENT_ID, ex.INTRUCTION_TIME, ref PatientTypeAlter, ref HeinCardNumber); 

                    Mrs00141RDO rdo = this.GetRdoToUse(HeinCardNumber, PatientTypeAlter); 
                    if (rdo != null)
                    {
                        if (this.IsIn(PatientTypeAlter))
                        {
                            rdo.EXAM_IN_COUNT++; 
                        }
                        else
                        {
                            rdo.EXAM_OUT_COUNT++; 
                        }
                    }
                }
            }
        }

        private Mrs00141RDO GetRdoToUse(string HeinCardNumber, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            Mrs00141RDO rdo = null; 
            if (this.IsBhTq(HeinCardNumber))
            {
                rdo = this.bhtq; 
            }
            else if (this.IsBhQd(HeinCardNumber))
            {
                rdo = this.bhqd; 
            }
            else if (this.IsOtherBhyt(HeinCardNumber))
            {
                rdo = this.otherBhyt; 
            }
            else
            {
                rdo = PatientTypeAlter == null ? null : this.others.Where(o => o.PATIENT_TYPE_ID == PatientTypeAlter.PATIENT_TYPE_ID).FirstOrDefault(); 
            }
            return rdo; 
        }

        private void GetPatientTypeAlterByTime(List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, long treatmentId, long time, ref V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, ref string HeinCardNumber)
        {
            V_HIS_PATIENT_TYPE_ALTER p = PatientTypeAlters
                        .Where(o => o.TREATMENT_ID == treatmentId && o.LOG_TIME < time)
                        .OrderByDescending(o => o.LOG_TIME)
                        .FirstOrDefault(); 
            p = p == null ? PatientTypeAlters
                .Where(o => o.TREATMENT_ID == treatmentId && o.LOG_TIME >= time)
                .OrderBy(o => o.LOG_TIME)
                .FirstOrDefault() : p; 
            PatientTypeAlter = p; 
            HeinCardNumber = PatientTypeAlter != null
                        ? PatientTypeAlter.HEIN_CARD_NUMBER : ""; 
        }

        /// <summary>
        /// Co phai noi tru hay khong
        /// </summary>
        /// <returns></returns>
        private bool IsIn(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null
                && !string.IsNullOrWhiteSpace(PatientTypeAlter.HEIN_TREATMENT_TYPE_CODE)
                && PatientTypeAlter.HEIN_TREATMENT_TYPE_CODE.Equals(MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT); 
        }

        /// <summary>
        /// Thiet lap gia tri ban dau cho danh sach cac dien doi tuong (ko phai la BHYT va vien phi)
        /// </summary>
        /// <param name="others"></param>
        private void InitOthers(ref List<Mrs00141RDO> data)
        {
            data = HisPatientTypeCFG.PATIENT_TYPEs
                .Where(o => o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                .Select(t => new Mrs00141RDO
                {
                    PATIENT_TYPE_NAME = t.PATIENT_TYPE_NAME,
                    PATIENT_TYPE_ID = t.ID
                }).ToList(); 
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan doi hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhQd(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber); 
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan huu hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhTq(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber); 
        }

        /// <summary>
        /// Co phai doi tuong BHYT khac (ngoai TQ, QĐ)
        /// </summary>
        /// <param name="treatment"></param>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsOtherBhyt(string HeinCardNumber)
        {
            return !this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber)
            && !this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber); 
        }

        /// <summary>
        /// Co so BHYT bat dau voi cac ma trong danh sach hay khong
        /// </summary>
        /// <param name="prefixs"></param>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsHeinCardNumberPrefixWith(List<string> prefixs, string HeinCardNumber)
        {
            if (IsNotNullOrEmpty(prefixs) && !string.IsNullOrWhiteSpace(HeinCardNumber))
            {
                foreach (string s in prefixs)
                {
                    if (HeinCardNumber.StartsWith(s))
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                this.PrepareData(departmentTrans, PatientTypeAlters, examServiceReqs, surgSereServs, sereServPttts); 
                result = true; 
            }
            catch (Exception ex)
            {
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
                    dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                objectTag.AddObjectData(store, "BHTQ", new List<Mrs00141RDO> { this.bhtq }); 
                objectTag.AddObjectData(store, "BHQD", new List<Mrs00141RDO> { this.bhqd }); 
                objectTag.AddObjectData(store, "BHYT", new List<Mrs00141RDO> { this.otherBhyt }); 
                objectTag.AddObjectData(store, "OTHERS", this.others); 
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

    class RDOCustomerFuncRownumberData : FlexCel.Report.TFlexCelUserFunction
    {
        public RDOCustomerFuncRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            long result = 0; 
            try
            {
                long rownumber = Convert.ToInt64(parameters[0]); 
                result = (rownumber + 1); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex); 
            }

            return result; 
        }
    }
}
