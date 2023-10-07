using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00118
{
    public class Mrs00118Processor : AbstractProcessor
    {
        List<Mrs00118RDO> ListSereServRdo = new List<Mrs00118RDO>();
        Mrs00118Filter CastFilter = null;
        private long? DATE_TO_PATIENT_OLD;
        private string departmentName;
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTranViews;
        List<HIS_TREATMENT> listTreatmentViews = new List<HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterViews = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();

        public Mrs00118Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00118Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00118Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter MRS00152: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                LogSystem.Info("Bat dau lay du lieu filter MRS00118 ===============================================================");
                //-------------------------------------------------------------------------------------------------- HIS_DEPARTMENT
                departmentName = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).GetById(CastFilter.DEPARTMENT_ID).DEPARTMENT_NAME;
                //-------------------------------------------------------------------------------------------------- V_HIS_DEPARTMENT_TRAN
                var metyFilterDepartmentTran = new HisDepartmentTranViewFilterQuery
                {
                    DEPARTMENT_IN_TIME_FROM = CastFilter.DATE_FROM,
                    DEPARTMENT_IN_TIME_TO = CastFilter.DATE_TO,
                    DEPARTMENT_ID = CastFilter.DEPARTMENT_ID
                };
                listDepartmentTranViews = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartmentTran);
                //-------------------------------------------------------------------------------------------------- V_HIS_DEPARTMENT_TRAN
                //Lấy danh sách các bệnh nhân cũ (những bệnh nhân ngoài khoảng thời gian ng dùng chọn mà chưa kết thúc điều trị)
                //var metyFilterDepartmentTran2 = new HisDepartmentTranViewFilterQuery
                //{
                //    LOG_TIME_TO = DATE_TO_PATIENT_OLD,
                //    DEPARTMENT_ID = CastFilter.DEPARTMENT_ID
                //}; 
                //var listDepartmentTranViews2 = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartmentTran2); 
                var listTreatmentIds = listDepartmentTranViews.Select(s => s.TREATMENT_ID).Distinct().ToList();
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //-------------------------------------------------------------------------------------------------- V_HIS_TREATMENT
                    var metyFilterTreatment = new HisTreatmentFilterQuery
                    {
                        IDs = listIds
                    };
                    var treatmentViews = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).Get(metyFilterTreatment);
                    listTreatmentViews.AddRange(treatmentViews);

                    //-------------------------------------------------------------------------------------------------- V_HIS_PATIENT_TYPE_ALTER
                    var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var PatientTypeAlterViews = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);
                    listPatientTypeAlterViews.AddRange(PatientTypeAlterViews);
                }

                if (IsNotNullOrEmpty(listTreatmentViews))
                {
                    var patientId = listTreatmentViews.Select(o => o.PATIENT_ID).Distinct().ToList();
                    skip = 0;
                    while (patientId.Count - skip > 0)
                    {
                        var listIds = patientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var filterPatient = new HisPatientFilterQuery
                        {
                            IDs = listIds
                        };
                        var patients = new MOS.MANAGER.HisPatient.HisPatientManager(paramGet).Get(filterPatient);
                        listPatient.AddRange(patients);
                    }
                }

                //--------------------------------------------------------------------------------------------------
                LogSystem.Info("Ket thuc lay du lieu filter MRS00118 ===============================================================");
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00152." +
                        LogUtil.TraceData(
                            LogUtil.GetMemberName(() => paramGet), paramGet));
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
            var result = false;
            try
            {
                ProcessFilterData(listDepartmentTranViews, listTreatmentViews, listPatientTypeAlterViews);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans, List<HIS_TREATMENT> listTreatments,
            List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00152 ===============================================================");
                //Tạo các list chứa các đối tượng bệnh nhân
                var doiTuongQuan = new List<V_HIS_DEPARTMENT_TRAN>();
                var baoHiemQuanDoi = new List<V_HIS_DEPARTMENT_TRAN>();
                var baoHiemQuanHuu = new List<V_HIS_DEPARTMENT_TRAN>();
                var baoHiemThanNhanQuanDoi = new List<V_HIS_DEPARTMENT_TRAN>();
                var baoHiemYTe = new List<V_HIS_DEPARTMENT_TRAN>();
                var doiTuongChinhSach = new List<V_HIS_DEPARTMENT_TRAN>();
                var doiTuongTreEm = new List<V_HIS_DEPARTMENT_TRAN>();
                var baoHiemKhongThe = new List<V_HIS_DEPARTMENT_TRAN>();
                var dichVuYTe = new List<V_HIS_DEPARTMENT_TRAN>();
                //kiểm tra thuộc đối tượng nào thì add vào list đối tượng đó
                foreach (var listDepartmentTran in listDepartmentTrans)
                {
                    var treatment = listTreatments.FirstOrDefault(s => s.ID == listDepartmentTran.TREATMENT_ID);
                    var patient = listPatient.FirstOrDefault(o => o.ID == treatment.PATIENT_ID) ?? new HIS_PATIENT();
                    var PatientTypeAlter = PatientTypeAlters.FirstOrDefault(s => s.TREATMENT_ID == listDepartmentTran.TREATMENT_ID);
                    if (PatientTypeAlter == null) continue;

                    if (PatientTypeAlter.HEIN_CARD_NUMBER != null)
                    {
                        string HeinCardNumber = PatientTypeAlter.HEIN_CARD_NUMBER;
                        if (BHQD(HeinCardNumber))
                            baoHiemQuanDoi.Add(listDepartmentTran);
                        if (BHQH(patient, HeinCardNumber))
                            baoHiemQuanHuu.Add(listDepartmentTran);
                        if (BHTQ(HeinCardNumber))
                            baoHiemThanNhanQuanDoi.Add(listDepartmentTran);
                        if (TreEm(HeinCardNumber))
                            doiTuongTreEm.Add(listDepartmentTran);
                    }
                    if (QUAN(PatientTypeAlter))
                        doiTuongQuan.Add(listDepartmentTran);
                    if (ChinhSach(PatientTypeAlter))
                        doiTuongChinhSach.Add(listDepartmentTran);
                    if (BHKT(PatientTypeAlter))
                        baoHiemKhongThe.Add(listDepartmentTran);
                    if (DVYT(PatientTypeAlter))
                        dichVuYTe.Add(listDepartmentTran);
                }
                //Lấy các đối tượng BHYT
                var BHYT = listDepartmentTrans.Where(s => !doiTuongQuan.Contains(s) && !baoHiemQuanDoi.Contains(s) && !baoHiemQuanHuu.Contains(s) && !baoHiemThanNhanQuanDoi.Contains(s)
                     && !doiTuongChinhSach.Contains(s) && !doiTuongTreEm.Contains(s) && !baoHiemKhongThe.Contains(s) && !dichVuYTe.Contains(s)).Select(s => s.TREATMENT_ID).ToList();
                var hhh = PatientTypeAlters.Where(s => BHYT.Contains(s.TREATMENT_ID)).Select(s => s.TREATMENT_ID).ToList();
                baoHiemYTe = listDepartmentTrans.Where(s => hhh.Contains(s.TREATMENT_ID)).ToList();
                //Add các đối tượng này vào 1 list
                var listUnitTypes = new List<List<V_HIS_DEPARTMENT_TRAN>>
                {
                    doiTuongQuan,
                    baoHiemQuanDoi,
                    baoHiemQuanHuu,
                    baoHiemThanNhanQuanDoi,
                    baoHiemYTe,
                    doiTuongChinhSach,
                    doiTuongTreEm,
                    baoHiemKhongThe,
                    dichVuYTe
                };
                //list tên các đối tượng
                var listName = new List<string>
                {
                    "Quân",
                    "BHQ(CNVQP)",
                    "BHQH",
                    "BHTQ",
                    "BHYT",
                    "Chính Sách",
                    "Trẻ em",
                    "BH Không thẻ",
                    "Viện phí"
                };
                //tính toán số lượng bệnh nhân cũ, vào, chuyển khoa....
                var number = 0;
                foreach (var listUnitType in listUnitTypes)
                {
                    var rrr = listUnitType.Select(s => s.TREATMENT_ID).ToList();
                    var bbb = listTreatments.Where(s => rrr.Contains(s.ID)).ToList();
                    var rdo = new Mrs00118RDO
                    {
                        TEN_DANH_MUC = listName[number],
                        BENH_NHAN_CU = string.Format("Tổng ĐT: {0}", listUnitType.Count),
                        BENH_NHAN_VAO = listUnitType.Count(s => s.CREATE_TIME >= CastFilter.DATE_FROM && s.CREATE_TIME <= CastFilter.DATE_TO),
                        //WARNING
                        //CHUYEN_BAN_DEN = listUnitType.Count(s => s.NEXT_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID && IsNotNull(s.IS_RECEIVE)),
                        //CHUYEN_BAN_DI = listUnitType.Count(s => s.NEXT_DEPARTMENT_ID != CastFilter.DEPARTMENT_ID && s.IS_RECEIVE == null),
                        BENH_NHAN_RA = bbb.Count(s => s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN),
                        //MRS.MANAGER.Config.HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN),
                        CHUYEN_VIEN = bbb.Count(s => s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN),
                        //MRS.MANAGER.Config.HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN),
                        CON_LAI = "Chưa làm"
                    };
                    ListSereServRdo.Add(rdo);
                    number = number + 1;
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00152 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        #region Phân loại đối tượng
        //Đối tượng quân(Có giấy giới thiệu của quân đội)
        private bool QUAN(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__GTQD;
        }

        //Bảo hiểm quân đội(Có thẻ BHQĐ (là thẻ BHYT gồm những đầu mã: DN497,QN597, HC497))
        private bool BHQD(string HeinCardNumber)
        {
            var result = false;
            foreach (var value in MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS)
            {
                if (HeinCardNumber.Contains(value))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        //Bảo hiểm quân hưu(Có thẻ BHYT (đầu mã HT2,HT3 và có nghề nghiệp là Quân đội))
        private bool BHQH(HIS_PATIENT patient, string HeinCardNumber)
        {
            var result = false;
            foreach (var value in MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS)
            {
                if (HeinCardNumber.Contains(value) && patient.CAREER_ID == MRS.MANAGER.Config.HisCareerCFG.CAREER_ID__RETIRED_MILITARY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        //Bảo hiểm thân nhân quân đội(Có thẻ BHYT đầu mã TQ497, TE197, BT497, BT297, TY497)
        private bool BHTQ(string HeinCardNumber)
        {
            var result = false;
            foreach (var value in MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS)
            {
                if (HeinCardNumber.Contains(value))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        //Chính sách (bệnh nhân thuộc đối tượng chính sách)
        private bool ChinhSach(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DTCS;
        }

        //Bảo hiểm trẻ em (Mã BHYT đầu thẻ TE)
        private bool TreEm(string HeinCardNumber)
        {
            return HeinCardNumber.Contains(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TE);
        }

        //Bảo hiểm không thẻ(Có giấy giới thiệu của công an)
        private bool BHKT(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__GTCA;
        }

        //Dịch vụ y tế(Đối tượng viện phí)
        private bool DVYT(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        }
        #endregion

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                dicSingleTag.Add("DEPARTMENT_NAME", departmentName);
                objectTag.AddObjectData(store, "Report", ListSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
