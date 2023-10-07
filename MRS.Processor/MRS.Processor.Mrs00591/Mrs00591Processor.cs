using MOS.MANAGER.HisAccidentHurt;
using MOS.MANAGER.HisTreatment;
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
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisAccidentBodyPart;
using MOS.MANAGER.HisDeathCause;
using MOS.MANAGER.HisPatient;
using System.Reflection;
using Inventec.Common.Logging;
using SDA.MANAGER.Manager;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Core.SdaCommune.Get;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00591
{
    class Mrs00591Processor : AbstractProcessor
    {
        Mrs00591Filter castFilter = null;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_DEPARTMENT_TRAN> lastHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE_REQ> listHisServiceReqSurg = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> listHisServiceReqExam= new List<HIS_SERVICE_REQ>();
        List<HIS_PATIENT> listPatientEthnic = new List<HIS_PATIENT>();
        List<Mrs00591RDO> listRdo = new List<Mrs00591RDO>();
        string thisReportTypeCode = "";
        public Mrs00591Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }
        private string DTT_STR = "Đục thủy tinh thể";
        private string GLU_STR = "Glucôm";
        private string TKX_STR = "Tật khúc xạ";
        private string VKMC_STR = "Viêm kết mạc (cấp, mãn)";
        private string MON_STR = "Mộng thịt";
        private string QUA_STR = "Quặm";
        private string VLG_STR = "Viêm loét giác mạc";
        private string VTL_STR = "Viêm tắc lệ bộ";
        private string CHA_STR = "Chắp";
        private string LEO_STR = "Lẹo";
        private string LAC_STR = "Lắc mắt";
        private string CTM_STR = "Chấn thương mắt";
        private string DVG_STR = "Dị vật giác mạc";
        private string DVK_STR = "Dị vật kết mạc";
        private string XHT_STR = "Xuất huyết tiền phòng";
        private string UKM_STR = "U kết mạc";
        private string UMM_STR = "U mi mắt";
        private string UHM_STR = "U hốc mắt";
        private string VMB_STR = "Viêm màng bồ đào";
        private string VKMS_STR = "Viêm kết mạc sơ sinh";
        private string BDM_STR = "Bệnh đáy mắt";
        private string VBM_STR = "Viêm bờ mi";
        private string BON_STR = "Bỏng mắt";
        private string VTH_STR = "Viêm tấy hốc mắt";
        List<string> listIcdCodeAll = new List<string>();
        public override Type FilterType()
        {
            return typeof(Mrs00591Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00591Filter)this.reportFilter;

                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter = this.MapFilter<Mrs00591Filter, HisTreatmentFilterQuery>(castFilter, listHisTreatmentfilter);
                listHisTreatment = new HisTreatmentManager(new CommonParam()).Get(listHisTreatmentfilter);
                var listPatientIds = listHisTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                //Danh sách BN
                List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
                if (listPatientIds != null && listPatientIds.Count > 0)
                {
                    var skip = 0;

                    while (listPatientIds.Count - skip > 0)
                    {
                        var limit = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = limit;
                        patientFilter.ORDER_FIELD = "ID";
                        patientFilter.ORDER_DIRECTION = "ASC";

                        var listPatientSub = new HisPatientManager(param).Get(patientFilter);
                        if (listPatientSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientSub Get null");
                        else
                            listPatient.AddRange(listPatientSub);
                    }
                    listPatientEthnic = listPatient.Where(o => (!string.IsNullOrWhiteSpace(o.ETHNIC_NAME)) && o.ETHNIC_NAME.Trim().ToUpper() != "KINH").ToList();
                    //Inventec.Common.Logging.LogSystem.Info("lastPatienttypeAlter" + lastPatienttypeAlter.Count);
                }
                var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {

                    List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
                    List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = Ids;
                        HisDepartmentTranfilter.ORDER_DIRECTION = "ID";
                        HisDepartmentTranfilter.ORDER_FIELD = "ASC";
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);

                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = Ids;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ID";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ASC";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub GetView null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);

                        HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                        HisServiceReqfilter.TREATMENT_IDs = Ids;
                        HisServiceReqfilter.ORDER_DIRECTION = "ID";
                        HisServiceReqfilter.ORDER_FIELD = "ASC";
                        HisServiceReqfilter.HAS_EXECUTE = true;
                        HisServiceReqfilter.SERVICE_REQ_TYPE_IDs = new List<long>() 
                         { 
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT
                         };
                        var listHisServiceReqSub = new HisServiceReqManager(param).Get(HisServiceReqfilter);
                        if (listHisServiceReqSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisServiceReqSub GetView null");
                        else
                            listHisServiceReq.AddRange(listHisServiceReqSub);
                    }

                    lastHisDepartmentTran = listHisDepartmentTran.Where(p => p.DEPARTMENT_IN_TIME.HasValue).OrderBy(o => o.DEPARTMENT_IN_TIME.Value).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                    lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                    listHisServiceReqSurg = listHisServiceReq.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT).ToList();
                    listHisServiceReqExam = listHisServiceReq.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList();
                }
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(p => lastHisDepartmentTran.Exists(o => o.TREATMENT_ID == p.ID && castFilter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID))).ToList();

                }
                if (castFilter.EXAM_ROOM_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(p => listHisServiceReqExam.Exists(o => o.TREATMENT_ID == p.ID && castFilter.EXAM_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID))).ToList();

                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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
            bool result = true;
            try
            {
               
                Contructor(castFilter.DTT,this.DTT_STR);
                Contructor(castFilter.GLU,this.GLU_STR);
                Contructor(castFilter.TKX,this.TKX_STR);
                Contructor(castFilter.VKMC,this.VKMC_STR);
                Contructor(castFilter.MON,this.MON_STR);
                Contructor(castFilter.QUA,this.QUA_STR);
                Contructor(castFilter.VLG,this.VLG_STR);
                Contructor(castFilter.VTL,this.VTL_STR);
                Contructor(castFilter.CHA,this.CHA_STR);
                Contructor(castFilter.LEO,this.LEO_STR);
                Contructor(castFilter.LAC,this.LAC_STR);
                Contructor(castFilter.CTM,this.CTM_STR);
                Contructor(castFilter.DVG,this.DVG_STR);
                Contructor(castFilter.DVK,this.DVK_STR);
                Contructor(castFilter.XHT,this.XHT_STR);
                Contructor(castFilter.UKM,this.UKM_STR);
                Contructor(castFilter.UMM,this.UMM_STR);
                Contructor(castFilter.UHM,this.UHM_STR);
                Contructor(castFilter.VMB,this.VMB_STR);
                Contructor(castFilter.VKMS,this.VKMS_STR);
                Contructor(castFilter.BDM,this.BDM_STR);
                Contructor(castFilter.VBM,this.VBM_STR);
                Contructor(castFilter.BON,this.BON_STR);
                Contructor(castFilter.VTH,this.VTH_STR);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void Contructor(List<string> listIcdCode, string name)
        {
            if (listIcdCode != null)
            {
                listIcdCodeAll.AddRange(listIcdCode);
                Mrs00591RDO rdo = new Mrs00591RDO();

                bool HasData = false;
                rdo.NAME = name;
                rdo.EX_M = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && listIcdCode.Contains(o.ICD_CODE) && TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                if (rdo.EX_M > 0)
                {
                    HasData = true;
                }
                rdo.EX_F = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && listIcdCode.Contains(o.ICD_CODE) && TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                if (rdo.EX_F > 0)
                {
                    HasData = true;
                }

                rdo.TR_M = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && listIcdCode.Contains(o.ICD_CODE) && TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                if (rdo.TR_M > 0)
                {
                    HasData = true;
                }
                rdo.TR_F = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && listIcdCode.Contains(o.ICD_CODE) && TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                if (rdo.TR_F > 0)
                {
                    HasData = true;
                }

                rdo.PT_M = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && listIcdCode.Contains(o.ICD_CODE) && IsSurg(o.ID));
                if (rdo.PT_M > 0)
                {
                    HasData = true;
                }
                rdo.PT_F = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && listIcdCode.Contains(o.ICD_CODE) && IsSurg(o.ID));
                if (rdo.PT_F > 0)
                {
                    HasData = true;
                }

                rdo.CT_M = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && listIcdCode.Contains(o.ICD_CODE) && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                if (rdo.CT_M > 0)
                {
                    HasData = true;
                }
                rdo.CT_F = listHisTreatment.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && listIcdCode.Contains(o.ICD_CODE) && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                if (rdo.CT_F > 0)
                {
                    HasData = true;
                }



                //if (HasData)
                {
                    listRdo.Add(rdo);
                }
            }
        }

        private bool IsSurg(long treatmentId)
        {
            return listHisServiceReqSurg.Exists(o => o.TREATMENT_ID == treatmentId);
        }

        private long TreatmentTypeId(long treatmentId)
        {
            var patientTypeAlter = lastHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == treatmentId);
            if (patientTypeAlter != null)
            {
                return patientTypeAlter.TREATMENT_TYPE_ID;
            }
            else
            {
                return 0;
            }
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_FROM ?? castFilter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_TO ?? castFilter.OUT_TIME_TO ?? 0));
            if (castFilter.DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", string.Join(",", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }
            if (castFilter.EXAM_ROOM_IDs != null)
            {
                dicSingleTag.Add("ROOM_NAME", string.Join(",", HisRoomCFG.HisRooms.Where(o => castFilter.EXAM_ROOM_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }
            var listHisTreatmentIcd = listHisTreatment.Where(o => listIcdCodeAll.Contains(o.ICD_CODE)).ToList();
            dicSingleTag.Add("EX", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM));
            dicSingleTag.Add("EX_M", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE));
            dicSingleTag.Add("EX_F", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            dicSingleTag.Add("EX_KI", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("EX_KH", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && !listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("EX_TE", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));

            dicSingleTag.Add("TR", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM));
            dicSingleTag.Add("TR_M", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE));
            dicSingleTag.Add("TR_F", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            dicSingleTag.Add("TR_KI", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("TR_KH", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && !listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("TR_TE", listHisTreatmentIcd.Count(o => TreatmentTypeId(o.ID) != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));

            dicSingleTag.Add("PT", listHisTreatmentIcd.Count(o => IsSurg(o.ID)));
            dicSingleTag.Add("PT_M", listHisTreatmentIcd.Count(o => IsSurg(o.ID) && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE));
            dicSingleTag.Add("PT_F", listHisTreatmentIcd.Count(o => IsSurg(o.ID) && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            dicSingleTag.Add("PT_KI", listHisTreatmentIcd.Count(o => IsSurg(o.ID) && listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("PT_KH", listHisTreatmentIcd.Count(o => IsSurg(o.ID) && !listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("PT_TE", listHisTreatmentIcd.Count(o => IsSurg(o.ID) && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));

            dicSingleTag.Add("CT", listHisTreatmentIcd.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN));
            dicSingleTag.Add("CT_M", listHisTreatmentIcd.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE));
            dicSingleTag.Add("CT_F", listHisTreatmentIcd.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            dicSingleTag.Add("CT_KI", listHisTreatmentIcd.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("CT_KH", listHisTreatmentIcd.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && !listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)));
            dicSingleTag.Add("CT_TE", listHisTreatmentIcd.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));
            objectTag.AddObjectData(store, "Report", listRdo);
        }


    }
}
