using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisMilitaryRank;
using AutoMapper; 
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MOS.LibraryHein.Bhyt.HeinTreatmentType; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisTreatment; 
using MRS.SDO; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00270
{
    class Mrs00270Processor : AbstractProcessor
    {
        private Mrs00270RDO examGgtCbcc = new Mrs00270RDO(); 
        private Mrs00270RDO examGgtT1T2 = new Mrs00270RDO(); 
        private Mrs00270RDO examGgtKhac = new Mrs00270RDO(); 
        private Mrs00270RDO examTheCbcc = new Mrs00270RDO(); 
        private Mrs00270RDO examTheT1T2 = new Mrs00270RDO(); 
        private Mrs00270RDO examTheKhac = new Mrs00270RDO(); 
        private Mrs00270RDO examTq497 = new Mrs00270RDO(); 
        private Mrs00270RDO examTA4 = new Mrs00270RDO(); 
        private Mrs00270RDO examTY4 = new Mrs00270RDO(); 
        private Mrs00270RDO examBhqh = new Mrs00270RDO(); 
        private Mrs00270RDO examCcb = new Mrs00270RDO(); 
        private Mrs00270RDO examNcc = new Mrs00270RDO(); ///
        private Mrs00270RDO examDn497 = new Mrs00270RDO(); 
        private Mrs00270RDO examCs = new Mrs00270RDO(); 
        private Mrs00270RDO examFee = new Mrs00270RDO(); 
        private List<Mrs00270RDO> examOthers = new List<Mrs00270RDO>(); 

        private Mrs00270RDO treatGgtCbcc = new Mrs00270RDO(); 
        private Mrs00270RDO treatGgtT1T2 = new Mrs00270RDO(); 
        private Mrs00270RDO treatGgtKhac = new Mrs00270RDO(); 
        private Mrs00270RDO treatTheCbcc = new Mrs00270RDO(); 
        private Mrs00270RDO treatTheT1T2 = new Mrs00270RDO(); 
        private Mrs00270RDO treatTheKhac = new Mrs00270RDO(); 
        private Mrs00270RDO treatTq497 = new Mrs00270RDO(); 
        private Mrs00270RDO treatTA4 = new Mrs00270RDO(); 
        private Mrs00270RDO treatTY4 = new Mrs00270RDO(); 
        private Mrs00270RDO treatBhqh = new Mrs00270RDO(); 
        private Mrs00270RDO treatCcb = new Mrs00270RDO(); 
        private Mrs00270RDO treatNcc = new Mrs00270RDO(); ///
        private Mrs00270RDO treatDn497 = new Mrs00270RDO(); 
        private Mrs00270RDO treatCs = new Mrs00270RDO(); 
        private Mrs00270RDO treatFee = new Mrs00270RDO(); 
        private List<Mrs00270RDO> treatOthers = new List<Mrs00270RDO>(); 

        private Mrs00270RDO presGgtCbcc = new Mrs00270RDO(); 
        private Mrs00270RDO presGgtT1T2 = new Mrs00270RDO(); 
        private Mrs00270RDO presGgtKhac = new Mrs00270RDO(); 
        private Mrs00270RDO presTheCbcc = new Mrs00270RDO(); 
        private Mrs00270RDO presTheT1T2 = new Mrs00270RDO(); 
        private Mrs00270RDO presTheKhac = new Mrs00270RDO(); 
        private Mrs00270RDO presTq497 = new Mrs00270RDO(); 
        private Mrs00270RDO presTA4 = new Mrs00270RDO(); 
        private Mrs00270RDO presTY4 = new Mrs00270RDO(); 
        private Mrs00270RDO presBhqh = new Mrs00270RDO(); 
        private Mrs00270RDO presCcb = new Mrs00270RDO(); 
        private Mrs00270RDO presNcc = new Mrs00270RDO(); ///
        private Mrs00270RDO presDn497 = new Mrs00270RDO(); 
        private Mrs00270RDO presCs = new Mrs00270RDO(); 
        private Mrs00270RDO presFee = new Mrs00270RDO(); 
        private List<Mrs00270RDO> presOthers = new List<Mrs00270RDO>(); 

        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>(); 
        List<HIS_SERE_SERV> sereServs = new List<HIS_SERE_SERV>();
        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
        Mrs00270Filter filter = null;
        public Mrs00270Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00270Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                filter = ((Mrs00270Filter)reportFilter); 
                //Lay danh sach treatment tuong ung theo thoi gian vao vien
                var treatmentFilter = new HisTreatmentViewFilterQuery
                {
                    IN_TIME_FROM = filter.TIME_FROM,
                    IN_TIME_TO = filter.TIME_TO
                }; 
                listTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter); 
                //Lấy danh sách bệnh nhân theo khoa
                var listTreatmentIds = listTreatment.Select(s => s.ID).ToList();
                if (filter.DEPARTMENT_ID != null)
                {
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var departmentTranFilter = new HisDepartmentTranFilterQuery
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var departmentTrans = new HisDepartmentTranManager(paramGet).Get(departmentTranFilter);
                        listDepartmentTrans.AddRange(departmentTrans);
                    }
                }

                var treatmentIds = listDepartmentTrans.Select(s => s.TREATMENT_ID).Distinct().ToList(); 

                //Lay danh sach V_HIS_PATIENT_TYPE_ALTER tuong ung
                patientTypeAlters = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(treatmentIds);

                //Lay danh sach HIS_SERE_SERV tuong ung
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listSub = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisSereServfilter = new HisSereServFilterQuery()
                        {
                            TREATMENT_IDs = listSub,
                            TDL_SERVICE_TYPE_ID=IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC

                        };
                        var listSereServSub = new HisSereServManager(paramGet).Get(HisSereServfilter);
                        sereServs.AddRange(listSereServSub??new List<HIS_SERE_SERV>());

                    }
                }

                //lay danh sách HIS_PATIENT
                HisPatientFilterQuery patientFilter = new HisPatientFilterQuery(); 
                patientFilter.IDs = treatmentIds.Distinct().ToList(); 
                listPatient = new HisPatientManager(paramGet).Get(patientFilter); 

                result = true; 
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
            bool result = false; 
            try
            {
                this.InitOthers(ref examOthers); 
                this.InitOthers(ref treatOthers); 
                this.InitOthers(ref presOthers); 
               
                if (IsNotNullOrEmpty(patientTypeAlters))
                {
                    foreach (V_HIS_PATIENT_TYPE_ALTER patientTypeAlter in patientTypeAlters)
                    {
                        V_HIS_TREATMENT treatment = listTreatment.Where(o => o.ID == patientTypeAlter.TREATMENT_ID)
                            .FirstOrDefault(); 
                        HIS_PATIENT patient = listPatient.Where(o => o.ID == treatment.PATIENT_ID).FirstOrDefault();

                        var listDepartmentTranSub = listDepartmentTrans.Where(o => o.TREATMENT_ID == patientTypeAlter.TREATMENT_ID && o.DEPARTMENT_ID == this.filter.DEPARTMENT_ID).ToList();
                        List<HIS_SERE_SERV> medicines = sereServs.Where(o => o.TDL_TREATMENT_ID == patientTypeAlter.TREATMENT_ID).ToList();
                        //xac dinh thoi gian vao ra khoa

                        string HeinCardNumber = patientTypeAlter.HEIN_CARD_NUMBER; 
                        //List<HIS_SERE_SERV> treatmentSereServs = sereServs != null ? sereServs.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList() : null; 
                        //Dem du lieu ngoai tru
                        if (this.IsExam(patientTypeAlter))
                            {//Trường hợp lọc theo khoa, nếu các log chuyển khoa của HSDT đó có diện điều trị là khám thì lấy.

                                if (this.filter.DEPARTMENT_ID == null || listDepartmentTranSub.Exists(o => TreatmentTypeId(o, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM))
                                {
                                    this.CountData(patient, patientTypeAlter, HeinCardNumber, ref examGgtCbcc, ref examGgtT1T2, ref examGgtKhac, ref examTheCbcc, ref examTheT1T2, ref examTheKhac, ref examTq497, ref examTA4, ref examTY4, ref examBhqh, ref examCcb, ref examNcc, ref examDn497, ref examCs, ref examFee, ref examOthers); ///
                                    //Dem du lieu cap phat thuoc
                                    if (IsNotNullOrEmpty(medicines))
                                    {
                                        this.CountData(patient, patientTypeAlter, HeinCardNumber, ref presGgtCbcc, ref presGgtT1T2, ref presGgtKhac, ref presTheCbcc, ref presTheT1T2, ref presTheKhac, ref presTq497, ref presTA4, ref presTY4, ref presBhqh, ref presCcb, ref presNcc, ref presDn497, ref presCs, ref presFee, ref presOthers); ///
                                    }
                                }
                            }
                        //Dem du lieu noi tru
                        if (this.IsTreat(patientTypeAlter))
                        {
                            if (this.filter.DEPARTMENT_ID == null || listDepartmentTranSub.Exists(o => TreatmentTypeId(o, patientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                            {
                                this.CountData(patient, patientTypeAlter, HeinCardNumber, ref treatGgtCbcc, ref treatGgtT1T2, ref treatGgtKhac, ref treatTheCbcc, ref treatTheT1T2, ref treatTheKhac, ref treatTq497, ref treatTA4, ref treatTY4, ref treatBhqh, ref treatCcb, ref treatNcc, ref treatDn497, ref treatCs, ref treatFee, ref treatOthers); ///
                            }
                        }
                    }
                    result = true; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 

            }
            return result; 
        }
        //Dien dieu tri
        private long TreatmentTypeId(HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            long result = 0;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID;
                }
                else
                {
                    patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                        && o.LOG_TIME > departmentTran.DEPARTMENT_IN_TIME
                        && o.LOG_TIME < (NextDepartment(departmentTran).DEPARTMENT_IN_TIME ?? 99999999999999)).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().TREATMENT_TYPE_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return listDepartmentTrans.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }
        //Dem du lieu
        private void CountData(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter, string HeinCardNumber,
            ref Mrs00270RDO GgtCbccCount, ref Mrs00270RDO GgtT1T2Count, ref Mrs00270RDO GgtKhacCount, ref Mrs00270RDO TheCbccCount, ref Mrs00270RDO TheT1T2Count, ref Mrs00270RDO TheKhacCount, ref Mrs00270RDO Tq497Count, ref Mrs00270RDO TA4Count, ref Mrs00270RDO TY4Count, ref Mrs00270RDO BhqhCount, ref Mrs00270RDO CcbCount, ref Mrs00270RDO NccCount, ref Mrs00270RDO Dn497Count,
            ref Mrs00270RDO CsCount, ref Mrs00270RDO FeeCount, ref List<Mrs00270RDO> othersCount)///
        {
            if (patient != null && patientTypeAlter != null)
            {
                if (this.IsGgtCbcc(patient, patientTypeAlter))
                {
                    GgtCbccCount.Amount++; 
                }
                if (this.IsGgtT1T2(patient, patientTypeAlter))
                {
                    GgtT1T2Count.Amount++; 
                }
                if (this.IsGgtKhac(patient, patientTypeAlter))
                {
                    GgtKhacCount.Amount++; 
                }
                if (this.IsTheCbcc(patient, patientTypeAlter))
                {
                    TheCbccCount.Amount++; 
                }
                if (this.IsTheT1T2(patient, patientTypeAlter))
                {
                    TheT1T2Count.Amount++; 
                }
                if (this.IsTheKhac(patient, patientTypeAlter))
                {
                    TheKhacCount.Amount++; 
                }
                if (this.IsTq497(HeinCardNumber))
                {
                    Tq497Count.Amount++; 
                }
                if (this.IsTA4(HeinCardNumber))
                {
                    TA4Count.Amount++; 
                }
                if (this.IsTY4(HeinCardNumber))
                {
                    TY4Count.Amount++; 
                }
                if (this.IsBhqh(patient, HeinCardNumber))
                {
                    BhqhCount.Amount++; 
                }
                else if (this.IsCcb(HeinCardNumber))
                {
                    CcbCount.Amount++; 
                }
                else if (this.IsNcc(HeinCardNumber))
                {
                    NccCount.Amount++; 
                }
                else if (this.IsDn497(HeinCardNumber))///
                {
                    Dn497Count.Amount++; ///
                }
                //else if (bhyt != null)
                //{
                //    CsCount.Amount++; 
                //}
                else if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    FeeCount.Amount++; 
                }
                else if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DTCS)
                {
                    FeeCount.Amount++; 
                }
                else
                {
                    Mrs00270RDO rdo = othersCount.Where(o => o.PatientTypeId == patientTypeAlter.PATIENT_TYPE_ID).FirstOrDefault(); 
                    if (rdo != null)
                    {
                        rdo.Amount++; 
                    }
                }
            }
        }
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            try
            {
                if (((Mrs00270Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("IN_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00270Filter)reportFilter).TIME_FROM)); 
                }
                if (((Mrs00270Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("IN_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00270Filter)reportFilter).TIME_TO)); 
                }

                dicSingleTag.Add("EXAM_GGTCBCC_AMOUNT", examGgtCbcc.Amount); 
                dicSingleTag.Add("EXAM_GGTT1_T2_AMOUNT", examGgtT1T2.Amount); 
                dicSingleTag.Add("EXAM_GGTKHAC_AMOUNT", examGgtKhac.Amount); 
                dicSingleTag.Add("EXAM_THECBCC_AMOUNT", examTheCbcc.Amount); 
                dicSingleTag.Add("EXAM_THET1_T2_AMOUNT", examTheT1T2.Amount); 
                dicSingleTag.Add("EXAM_THEKHAC_AMOUNT", examTheKhac.Amount); 
                dicSingleTag.Add("EXAM_TQ497_AMOUNT", examTq497.Amount); 
                dicSingleTag.Add("EXAM_TA4_AMOUNT", examTA4.Amount); 
                dicSingleTag.Add("EXAM_TY4_AMOUNT", examTY4.Amount); 
                dicSingleTag.Add("EXAM_BH_QH_AMOUNT", examTY4.Amount); 
                dicSingleTag.Add("EXAM_CCB_AMOUNT", examCcb.Amount); 
                dicSingleTag.Add("EXAM_NCC_AMOUNT", examNcc.Amount); 
                dicSingleTag.Add("EXAM_DN497_AMOUNT", examDn497.Amount); ///
                dicSingleTag.Add("EXAM_CS_AMOUNT", examCs.Amount); 
                dicSingleTag.Add("EXAM_FEE_AMOUNT", examFee.Amount); 

                dicSingleTag.Add("TREAT_GGTCBCC_AMOUNT", treatGgtCbcc.Amount); 
                dicSingleTag.Add("TREAT_GGTT1_T2_AMOUNT", treatGgtT1T2.Amount); 
                dicSingleTag.Add("TREAT_GGTKHAC_AMOUNT", treatGgtKhac.Amount); 
                dicSingleTag.Add("TREAT_THECBCC_AMOUNT", treatTheCbcc.Amount); 
                dicSingleTag.Add("TREAT_THET1_T2_AMOUNT", treatTheT1T2.Amount); 
                dicSingleTag.Add("TREAT_THEKHAC_AMOUNT", treatTheKhac.Amount); 
                dicSingleTag.Add("TREAT_TQ497_AMOUNT", treatTq497.Amount); 
                dicSingleTag.Add("TREAT_TA4_AMOUNT", treatTA4.Amount); 
                dicSingleTag.Add("TREAT_TY4_AMOUNT", treatTY4.Amount); 
                dicSingleTag.Add("TREAT_BH_QH_AMOUNT", treatTY4.Amount); 
                dicSingleTag.Add("TREAT_CCB_AMOUNT", treatCcb.Amount); 
                dicSingleTag.Add("TREAT_NCC_AMOUNT", treatNcc.Amount); 
                dicSingleTag.Add("TREAT_DN497_AMOUNT", treatDn497.Amount); ///
                dicSingleTag.Add("TREAT_CS_AMOUNT", treatCs.Amount); 
                dicSingleTag.Add("TREAT_FEE_AMOUNT", treatFee.Amount); 

                dicSingleTag.Add("PRES_GGTCBCC_AMOUNT", presGgtCbcc.Amount); 
                dicSingleTag.Add("PRES_GGTT1_T2_AMOUNT", presGgtT1T2.Amount); 
                dicSingleTag.Add("PRES_GGTKHAC_AMOUNT", presGgtKhac.Amount); 
                dicSingleTag.Add("PRES_THECBCC_AMOUNT", presTheCbcc.Amount); 
                dicSingleTag.Add("PRES_THET1_T2_AMOUNT", presTheT1T2.Amount); 
                dicSingleTag.Add("PRES_THEKHAC_AMOUNT", presTheKhac.Amount); 
                dicSingleTag.Add("PRES_TQ497_AMOUNT", presTq497.Amount); 
                dicSingleTag.Add("PRES_TA4_AMOUNT", presTA4.Amount); 
                dicSingleTag.Add("PRES_TY4_AMOUNT", presTY4.Amount); 
                dicSingleTag.Add("PRES_BH_QH_AMOUNT", presTY4.Amount); 
                dicSingleTag.Add("PRES_CCB_AMOUNT", presCcb.Amount); 
                dicSingleTag.Add("PRES_NCC_AMOUNT", presNcc.Amount); 
                dicSingleTag.Add("PRES_DN497_AMOUNT", presDn497.Amount); ///
                dicSingleTag.Add("PRES_CS_AMOUNT", presCs.Amount); 
                dicSingleTag.Add("PRES_FEE_AMOUNT", presFee.Amount); 

                objectTag.AddObjectData(store, "examOthers", examOthers); 
                objectTag.AddObjectData(store, "treatOthers", treatOthers); 
                objectTag.AddObjectData(store, "presOthers", presOthers); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


        /// Co phai doi tuong Giay gioi thieu Can bo cao cap hay khong
        private bool IsGgtCbcc(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patient != null
                   &&
   this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__GTQD, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR, patientTypeAlter); 
        }

        /// Co phai doi tuong Giay gioi thieu T1 T2 hay khong
        private bool IsGgtT1T2(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patient != null
                   &&
   this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__GTQD, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__1AND2SLASH, patientTypeAlter); 
        }

        /// Co phai doi tuong Giay gioi thieu khac hay khong
        private bool IsGgtKhac(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patient != null
                   &&
   (!this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__GTQD, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__1AND2SLASH, patientTypeAlter) && !this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__GTQD, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR, patientTypeAlter)); 
        }

        /// Co phai doi tuong The Can bo cao cap hay khong
        private bool IsTheCbcc(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patient != null
                 &&
   this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR, patientTypeAlter); 
        }

        /// Co phai doi tuong the T1 T2 hay khong
        private bool IsTheT1T2(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patient != null
                   &&
   this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__1AND2SLASH, patientTypeAlter); 
        }

        /// Co phai doi tuong The Khac hay khong
        private bool IsTheKhac(HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patient != null
                   &&
   (!this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__1AND2SLASH, patientTypeAlter) && !this.IsPatientTypeAndMilitary(patient, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR, patientTypeAlter)); 
        }

        /// Co phai doi tuong BHYT TQ497 hay khong
        private bool IsTq497(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQ497S, HeinCardNumber); 
        }

        /// Co phai doi tuong BHYT TA4 hay khong
        private bool IsTA4(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TA4S, HeinCardNumber); 
        }

        /// Co phai doi tuong BHYT TY4 hay khong
        private bool IsTY4(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TY4S, HeinCardNumber); 
        }

        /// Co phai doi tuong BHYT quan hưu hay khong
        private bool IsBhqh(HIS_PATIENT patient, string HeinCardNumber)
        {
            return patient != null
                && patient.CAREER_ID == HisCareerCFG.CAREER_ID__RETIRED_MILITARY
                && this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS, HeinCardNumber); 
        }

        /// Co phai doi tuong BHYT Cuu chien binh hay khong
        private bool IsCcb(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__CB, HeinCardNumber); 
        }

        /// Co phai doi tuong BHYT Nguoi co cong hay khong
        private bool IsNcc(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__CC, HeinCardNumber); 
        }

        /// Co phai doi tuong BHYT DN497 hay khong
        private bool IsDn497(string HeinCardNumber)///
        {
            return this.IsHeinCardNumberLeverWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__AS, HeinCardNumber); ///
        }

        ///// Co phai doi tuong Chinh sach hay khong
        //private bool IsBhTe(V_HIS_PATY_ALTER_BHYT bhyt)
        //{
        //    return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TE, bhyt); 
        //}

      
        /// La noi tru hay ko
        private bool IsExam(V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patientTypeAlter != null && !string.IsNullOrWhiteSpace(patientTypeAlter.TREATMENT_TYPE_CODE) && patientTypeAlter.TREATMENT_TYPE_CODE.Equals(HTreatmentTypeCode.EXAM); 
        }

        private bool IsTreat(V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patientTypeAlter != null && !string.IsNullOrWhiteSpace(patientTypeAlter.TREATMENT_TYPE_CODE) && patientTypeAlter.TREATMENT_TYPE_CODE.Equals(HTreatmentTypeCode.I_TREAT); 
        }

        /// Co so BHYT bat dau voi cac ma trong danh sach hay khong
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

        /// Co la loai doi tương va quan ham hay khong
        private bool IsPatientTypeAndMilitary(HIS_PATIENT patient, long patientType, List<long> Military, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            if (patientTypeAlter != null && patientTypeAlter.PATIENT_TYPE_ID != null && patientTypeAlter.PATIENT_TYPE_ID == patientType)
                if (patient != null && IsNotNullOrEmpty(Military) && patient.MILITARY_RANK_ID != null)
                {
                    foreach (long s in Military)
                    {
                        if (patient.MILITARY_RANK_ID == s)
                        {
                            return true; 
                        }
                    }
                }
            return false; 
        }

        private bool IsHeinCardNumberLeverWith(List<string> prefixs, string HeinCardNumber)
        {
            if (IsNotNullOrEmpty(prefixs) && !string.IsNullOrWhiteSpace(HeinCardNumber))
            {
                foreach (string s in prefixs)
                {
                    if (HeinCardNumber.Substring(2, 1) == s)
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }

        /// Co so BHYT bat dau voi ma da cho hay khong
        private bool IsHeinCardNumberPrefixWith(string prefix, string HeinCardNumber)
        {
            return !string.IsNullOrWhiteSpace(prefix)
                && !string.IsNullOrWhiteSpace(HeinCardNumber)
                && HeinCardNumber.StartsWith(prefix); 
        }

        /// Thiet lap gia tri ban dau cho danh sach cac dien doi tuong (ko phai la BHYT va vien phi)
        private void InitOthers(ref List<Mrs00270RDO> others)
        {
            others = HisPatientTypeCFG.PATIENT_TYPEs
                .Where(o => o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                .Select(t => new Mrs00270RDO
                {
                    Amount = 0,
                    PatientTypeId = t.ID,
                    PatientTypeName = t.PATIENT_TYPE_NAME
                }).ToList(); 
        }
    }

    class CustomerFuncRownumberServiceType : TFlexCelUserFunction
    {
        int numOrder = 0; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]); 
                if (serviceTypeId > 0)
                {
                    numOrder++; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return numOrder; 
        }
    }

    class CustomerFuncRownumberService : TFlexCelUserFunction
    {
        long ServiceTypeId = 0; 
        int Num_Order = 0; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]); 
                long serviceId = Convert.ToInt64(parameters[1]); 
                if (serviceTypeId == ServiceTypeId)
                {
                    Num_Order = Num_Order + 1; 
                }
                else
                {
                    ServiceTypeId = serviceTypeId; 
                    Num_Order = 1; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return Num_Order; 
        }
    }

    class CustomerFuncRownumberTreatment : TFlexCelUserFunction
    {
        long ServiceTypeId = 0; 
        long ServiceId = 0; 
        int Num_Order = 0; 
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]); 
                long serviceId = Convert.ToInt64(parameters[1]); 
                long treatmentId = Convert.ToInt64(parameters[2]); 
                if ((serviceTypeId == ServiceTypeId) && (serviceId == ServiceId))
                {
                    Num_Order = Num_Order + 1; 
                }
                else
                {
                    ServiceTypeId = serviceTypeId; 
                    ServiceId = serviceId; 
                    Num_Order = 1; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return Num_Order; 
        }
    }

    public class HTreatmentTypeCode
    {
        public const string EXAM = "01"; 
        public const string O_TREAT = "02"; 
        public const string I_TREAT = "03"; 
    }


}
