using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTranPatiReason;
using MOS.MANAGER.HisTranPatiForm;
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

using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
using ACS.Filter;

using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisCareer;

namespace MRS.Processor.Mrs00191
{
    public class Mrs00191Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Mrs00191Filter CastFilter = null;
        private List<Mrs00191RDO> ListRdo = new List<Mrs00191RDO>();
        private List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        private Dictionary<string, HIS_CAREER> dicCareers = new Dictionary<string, HIS_CAREER>();
        private List<V_HIS_PATIENT_TYPE_ALTER> listPatienttypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<string, ACS_USER> dicAcsUser = new Dictionary<string, ACS_USER>();
        public Mrs00191Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00191Filter);
        }

        protected override bool GetData()
        {
            CastFilter = ((Mrs00191Filter)reportFilter);
            var result = true;
            try
            {
                var HisTreatmentFilterQuery = new HisTreatmentViewFilterQuery()
                {
                    OUT_TIME_FROM = CastFilter.OUT_TIME_FROM, //lay thoi gian duyet khoa
                    OUT_TIME_TO = CastFilter.OUT_TIME_TO,  //lay thoi gian duyet khoa
                    FEE_LOCK_TIME_FROM = CastFilter.FEE_LOCK_TIME_FROM, //lay thoi gian duyet khoa
                    FEE_LOCK_TIME_TO = CastFilter.FEE_LOCK_TIME_TO,  //lay thoi gian duyet khoa
                    TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN, //lay BN chuyển viên
                };
                listTreatments = new HisTreatmentManager(paramGet).GetView(HisTreatmentFilterQuery);
                if (CastFilter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatments = listTreatments.Where(o => CastFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                var treatmentIDs = listTreatments.Select(s => s.ID).ToList();
                if (IsNotNullOrEmpty(treatmentIDs))
                {
                    var skip = 0;
                    while (treatmentIDs.Count() - skip > 0)
                    {
                        var ListDSs = treatmentIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var patienttypeAlterFilters = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = ListDSs,
                        };
                        var listPatienttypeAlter = new HisPatientTypeAlterManager(paramGet).GetView(patienttypeAlterFilters);
                        listPatienttypeAlters.AddRange(listPatienttypeAlter);
                    }
                }
                //--------------------------------------------------------------------------------------------------//V_HIS_Careers
                var careerFilters = new HisCareerFilterQuery();
                var listCareers = new HisCareerManager(paramGet).Get(careerFilters);
                dicCareers = listCareers.GroupBy(o => o.CAREER_NAME).ToDictionary(p => p.Key, p => p.First());
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
            var result = true;
            try
            {
                ListRdo.Clear();
                int i = 0;
                //lay nhung doi tuong chuyen vien (02 la chyen vien)
                listTreatments = listTreatments.Where(s => HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__CV.Contains(s.TREATMENT_END_TYPE_ID ?? 0)).ToList();
                foreach (var tranPatiTreatment in listTreatments)
                {
                    //HÌNH THUC CHUYEN VIEN
                    var TRAN_PATI_FORM_CODE_1 = "";
                    var TRAN_PATI_FORM_CODE_2 = "";
                    var TRAN_PATI_FORM_CODE_3 = "";
                    var TRAN_PATI_FORM_CODE_4 = "";
                    if (tranPatiTreatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT)
                    {
                        TRAN_PATI_FORM_CODE_1 = "X";
                    }
                    if (tranPatiTreatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT)
                    {
                        TRAN_PATI_FORM_CODE_2 = "X";
                    }

                    if (tranPatiTreatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN)
                    {
                        TRAN_PATI_FORM_CODE_3 = "X";
                    }
                    if (tranPatiTreatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__EQUAL)
                    {
                        TRAN_PATI_FORM_CODE_4 = "X";
                    }

                    // LY DO CHUYEN
                    var REASON_NAME_1 = "";
                    var REASON_NAME_2 = "";
                    if (tranPatiTreatment.TRAN_PATI_REASON_ID == HisTranPatiReasonCFG.HIS_TRAN_PATI_REASON___01)
                    {
                        REASON_NAME_1 = "X";
                    }
                    if (tranPatiTreatment.TRAN_PATI_REASON_ID == HisTranPatiReasonCFG.HIS_TRAN_PATI_REASON___02)
                    {
                        REASON_NAME_2 = "X";
                    }

                    //lấy chuẩn đoán
                    var ICD_TEX = "";
                    var ICD_SUB_CODE = "";
                    if (tranPatiTreatment.ICD_TEXT != null || tranPatiTreatment.ICD_SUB_CODE != null)
                    {
                        ICD_TEX = tranPatiTreatment.ICD_TEXT;
                        ICD_SUB_CODE = tranPatiTreatment.ICD_SUB_CODE;
                    }
                    var ICD_NAME = tranPatiTreatment.ICD_NAME;
                    var ICD_CODE = tranPatiTreatment.ICD_CODE;

                    //lấy noi chuyen den
                    var MEDI_ORG_NAME = tranPatiTreatment.MEDI_ORG_NAME;

                    // số chuyển viện
                    var OUT_ORDER = tranPatiTreatment.OUT_CODE;

                    var PATIENT_NAME = tranPatiTreatment.TDL_PATIENT_NAME;
                    var AGE_MEN = "";
                    var AGE_WOMEN = "";
                    if (tranPatiTreatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        AGE_MEN = Inventec.Common.DateTime.Convert.TimeNumberToDateString(tranPatiTreatment.TDL_PATIENT_DOB);
                    }
                    else if (tranPatiTreatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        AGE_WOMEN = Inventec.Common.DateTime.Convert.TimeNumberToDateString(tranPatiTreatment.TDL_PATIENT_DOB);
                    }

                    //dia chi                      
                    var ADDRESS = tranPatiTreatment.TDL_PATIENT_ADDRESS != null ? tranPatiTreatment.TDL_PATIENT_ADDRESS : "";
                    //var ADDRESS = Treatment.VIR_ADDRESS ?? ""; 

                    //khoa kết thúc
                    var END_DEPARTMENT_NAME = tranPatiTreatment.END_DEPARTMENT_NAME;

                    //thoi gian chuyen (ket thuc)
                    string OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(tranPatiTreatment.OUT_TIME.Value);

                    //người chuyển
                    var END_USERNAME = tranPatiTreatment.END_USERNAME;

                    //lay BHYT

                    var HEIN_CARD_NUMBER = "";
                    HEIN_CARD_NUMBER = tranPatiTreatment.TDL_HEIN_CARD_NUMBER ?? "";
                    //lấy  nghề nghiệp
                    var CaReers = dicCareers.ContainsKey(tranPatiTreatment.TDL_PATIENT_CAREER_NAME??"") ? dicCareers[tranPatiTreatment.TDL_PATIENT_CAREER_NAME] : new HIS_CAREER();
                    var CAREER = string.Empty;
                    CAREER = CaReers.CAREER_NAME;
                    var rdo = new Mrs00191RDO
                    {
                        STT = ++i,
                        OUT_ORDER = OUT_ORDER,
                        PATIENT_NAME = PATIENT_NAME,
                        AGE_MEN = AGE_MEN,
                        AGE_WOMEN = AGE_WOMEN,
                        HEIN_CARD_NUMBER = HEIN_CARD_NUMBER,
                        ADDRESS = ADDRESS,
                        CAREER = CAREER,
                        END_DEPARTMENT_NAME = END_DEPARTMENT_NAME,
                        OUT_TIME = OUT_TIME,
                        ICD_TEX = ICD_TEX,
                        ICD_NAME = ICD_NAME,
                        ICD_NAME_NEW = ICD_NAME + ICD_TEX,
                        ICD_CODE = ICD_CODE + ICD_SUB_CODE,
                        TRAN_PATI_FORM_CODE_1 = TRAN_PATI_FORM_CODE_1,
                        TRAN_PATI_FORM_CODE_2 = TRAN_PATI_FORM_CODE_2,
                        TRAN_PATI_FORM_CODE_3 = TRAN_PATI_FORM_CODE_3,
                        TRAN_PATI_FORM_CODE_4 = TRAN_PATI_FORM_CODE_4,
                        REASON_NAME_1 = REASON_NAME_1,
                        REASON_NAME_2 = REASON_NAME_2,
                        MEDI_ORG_NAME = MEDI_ORG_NAME,
                        END_USERNAME = END_USERNAME,
                    };
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.OUT_TIME_FROM ?? CastFilter.FEE_LOCK_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.OUT_TIME_TO ?? CastFilter.FEE_LOCK_TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o=>o.OUT_ORDER).ToList());
        }

    }


}
