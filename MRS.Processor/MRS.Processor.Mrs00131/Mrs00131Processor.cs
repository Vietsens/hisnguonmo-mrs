using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisEmergencyWtime;
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

namespace MRS.Processor.Mrs00131
{
    public class Mrs00131Processor : AbstractProcessor
    {
        Mrs00131Filter castFilter = null; 

        //Du lieu cap cuu phan loai theo thoi gian
        private List<Mrs00131ByTimeRDO> emergencyByTimes = new List<Mrs00131ByTimeRDO>(); 

        //Du lieu cap cuu phan loai theo hinh thuc den vien
        private Mrs00131ByTypeRDO noiKhoaDonViDen = new Mrs00131ByTypeRDO(); 
        private Mrs00131ByTypeRDO noiKhoaGiaDinhDen = new Mrs00131ByTypeRDO(); 
        private Mrs00131ByTypeRDO noiKhoaChuyenVienDen = new Mrs00131ByTypeRDO(); 

        private Mrs00131ByTypeRDO ngoaiKhoaDonViDen = new Mrs00131ByTypeRDO(); 
        private Mrs00131ByTypeRDO ngoaiKhoaGiaDinhDen = new Mrs00131ByTypeRDO(); 
        private Mrs00131ByTypeRDO ngoaiKhoaChuyenVienDen = new Mrs00131ByTypeRDO(); 

        private Mrs00131ByTypeRDO chuyenKhoaDonViDen = new Mrs00131ByTypeRDO(); 
        private Mrs00131ByTypeRDO chuyenKhoaGiaDinhDen = new Mrs00131ByTypeRDO(); 
        private Mrs00131ByTypeRDO chuyenKhoaChuyenVienDen = new Mrs00131ByTypeRDO(); 

        List<HIS_EMERGENCY_WTIME> emergencyWtimes; 
        List<V_HIS_TREATMENT> treatments; 
        List<V_HIS_DEPARTMENT_TRAN> departmentTrans; 
        List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters; 

        public Mrs00131Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00131Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00131Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPARTMENT_TRAN, HIS_DEPARTMENT MRS00123 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                //Lay danh sach emergency_wtime
                emergencyWtimes = new MOS.MANAGER.HisEmergencyWtime.HisEmergencyWtimeManager(paramGet).Get(new HisEmergencyWtimeFilterQuery()); 

                //Lay danh sach ho so dieu tri theo thoi gian nguoi dung truyen vao
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.IN_TIME_FROM = castFilter.TIME_FROM; 
                treatmentFilter.IN_TIME_TO = castFilter.TIME_TO; 
                treatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 

                List<long> treatmentIds = IsNotNullOrEmpty(treatments) ? treatments.Select(o => o.ID).ToList() : null; 


                //Lay danh sach department_tran tuong ung voi treatment
                departmentTrans = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetViewByTreatmentIds(treatmentIds); 

                //Lay danh sach patient_tye_alter tuong ung
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

        private void PrepareData(List<HIS_EMERGENCY_WTIME> emergencyWtimes, List<V_HIS_TREATMENT> treatments, List<V_HIS_DEPARTMENT_TRAN> departmentTrans, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters)
        {
            if (IsNotNullOrEmpty(treatments) && IsNotNullOrEmpty(departmentTrans) && IsNotNullOrEmpty(PatientTypeAlters))
            {
                //Khoi tao ban dau cho du lieu phan loai theo thoi gian
                this.InitEmergencyByTimes(emergencyWtimes, ref this.emergencyByTimes); 

                foreach (var treatment in treatments)
                {

                    //chi xu ly neu co thong tin cap cuu
                    if (treatment.EMERGENCY_WTIME_ID != null)
                    {
                        //Ban ghi vao vien (ban ghi vao khoa dau tien)
                        V_HIS_DEPARTMENT_TRAN firstDepartmentTran = departmentTrans
                            .Where(o => o.TREATMENT_ID == treatment.ID)
                            .OrderBy(o => o.DEPARTMENT_IN_TIME).FirstOrDefault(); 
                        //Ban ghi dien doi tuong cuoi cung
                        V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = PatientTypeAlters.Where(o => o.TREATMENT_ID == treatment.ID)
                            .OrderByDescending(o => o.LOG_TIME)
                            .FirstOrDefault(); 
                        string HeinCardNumber = PatientTypeAlter != null
                            ? PatientTypeAlter.HEIN_CARD_NUMBER : ""; 

                        this.CountByType(treatment, firstDepartmentTran.DEPARTMENT_ID, PatientTypeAlter, HeinCardNumber); 
                        this.CountByTime(treatment, firstDepartmentTran.DEPARTMENT_ID, PatientTypeAlter, HeinCardNumber); 
                    }
                }
            }
        }

        /// <summary>
        /// Dem phan loai theo loai (don vi den, gia dinh den, chuyen vien den)
        /// </summary>
        /// <param name="treatment"></param>
        /// <param name="departmentId"></param>
        /// <param name="PatientTypeAlter"></param>
        /// <param name="bhyt"></param>
        private void CountByType(V_HIS_TREATMENT treatment, long departmentId, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, string HeinCardNumber)
        {
            Mrs00131ByTypeRDO data = null; 

            #region Xac dinh don vi den, chuyen vien den hay gia dinh den
            if (this.IsInternalMedicineDepartment(departmentId) && this.IsOrgTran(treatment))
            {
                data = noiKhoaDonViDen; 
            }
            else if (this.IsInternalMedicineDepartment(departmentId) && this.IsHospitalTran(treatment))
            {
                data = noiKhoaChuyenVienDen; 
            }
            else if (this.IsInternalMedicineDepartment(departmentId) && this.IsHomeTran(treatment))
            {
                data = noiKhoaGiaDinhDen; 
            }
            else if (this.IsSurgeryDepartment(departmentId) && this.IsOrgTran(treatment))
            {
                data = ngoaiKhoaDonViDen; 
            }
            else if (this.IsSurgeryDepartment(departmentId) && this.IsHospitalTran(treatment))
            {
                data = ngoaiKhoaChuyenVienDen; 
            }
            else if (this.IsSurgeryDepartment(departmentId) && this.IsHomeTran(treatment))
            {
                data = ngoaiKhoaGiaDinhDen; 
            }
            else if (this.IsSpecialtyDepartment(departmentId) && this.IsOrgTran(treatment))
            {
                data = chuyenKhoaDonViDen; 
            }
            else if (this.IsSpecialtyDepartment(departmentId) && this.IsHospitalTran(treatment))
            {
                data = chuyenKhoaChuyenVienDen; 
            }
            else if (this.IsSpecialtyDepartment(departmentId) && this.IsHomeTran(treatment))
            {
                data = chuyenKhoaGiaDinhDen; 
            }
            #endregion

            #region Dem so luong theo dien doi tuong
            if (data != null)
            {
                //Xac dinh dien doi tuong
                if (this.IsQu(PatientTypeAlter) || this.IsCs(PatientTypeAlter))
                {
                    data.QuanChinhSach++; 
                }
                else if (this.IsBhTq(HeinCardNumber) || this.IsBhQd(HeinCardNumber))
                {
                    data.TqQd++; 
                }
                else if (this.IsBh(HeinCardNumber))
                {
                    data.QhBh++; 
                }
                else if (this.IsDv(PatientTypeAlter))
                {
                    data.DichVu++; 
                }
            }
            #endregion
        }

        /// <summary>
        /// Dem so luong theo thoi gian dau
        /// </summary>
        /// <param name="treatment"></param>
        /// <param name="emergency"></param>
        /// <param name="departmentId"></param>
        /// <param name="PatientTypeAlter"></param>
        /// <param name="bhyt"></param>
        private void CountByTime(V_HIS_TREATMENT treatment, long departmentId, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, string HeinCardNumber)
        {
            Mrs00131ByTimeRDO data = this.emergencyByTimes != null ?
                this.emergencyByTimes.Where(o => o.EmergencyWtimeId == treatment.EMERGENCY_WTIME_ID).FirstOrDefault() : null; 
            if (data != null)
            {
                if ((this.IsQu(PatientTypeAlter) || this.IsCs(PatientTypeAlter)) && this.IsInternalMedicineDepartment(departmentId))
                {
                    data.QuanChinhSach_Noi++; 
                }
                else if ((this.IsQu(PatientTypeAlter) || this.IsCs(PatientTypeAlter)) && this.IsSurgeryDepartment(departmentId))
                {
                    data.QuanChinhSach_Ngoai++; 
                }
                else if ((this.IsQu(PatientTypeAlter) || this.IsCs(PatientTypeAlter)) && this.IsSpecialtyDepartment(departmentId))
                {
                    data.QuanChinhSach_CK++; 
                }
                else if ((this.IsBhTq(HeinCardNumber) || this.IsBhQd(HeinCardNumber)) && this.IsInternalMedicineDepartment(departmentId))
                {
                    data.TqQd_Noi++; 
                }
                else if ((this.IsBhTq(HeinCardNumber) || this.IsBhQd(HeinCardNumber)) && this.IsSurgeryDepartment(departmentId))
                {
                    data.TqQd_Ngoai++; 
                }
                else if ((this.IsBhTq(HeinCardNumber) || this.IsBhQd(HeinCardNumber)) && this.IsSpecialtyDepartment(departmentId))
                {
                    data.TqQd_CK++; 
                }
                else if (this.IsBh(HeinCardNumber) && this.IsInternalMedicineDepartment(departmentId))
                {
                    data.QhBh_Noi++; 
                }
                else if (this.IsBh(HeinCardNumber) && this.IsSurgeryDepartment(departmentId))
                {
                    data.QhBh_Ngoai++; 
                }
                else if (this.IsBh(HeinCardNumber) && this.IsSpecialtyDepartment(departmentId))
                {
                    data.QhBh_CK++; 
                }
                else if (this.IsDv(PatientTypeAlter) && this.IsInternalMedicineDepartment(departmentId))
                {
                    data.DichVu_Noi++; 
                }
                else if (this.IsDv(PatientTypeAlter) && this.IsSurgeryDepartment(departmentId))
                {
                    data.DichVu_Ngoai++; 
                }
                else if (this.IsDv(PatientTypeAlter) && this.IsSpecialtyDepartment(departmentId))
                {
                    data.DichVu_CK++; 
                }
            }
        }

        /// <summary>
        /// Thiet lap gia tri ban dau cho danh sach cac dien doi tuong (ko phai la BHYT va vien phi)
        /// </summary>
        /// <param name="others"></param>
        private void InitEmergencyByTimes(List<HIS_EMERGENCY_WTIME> emergencyWtimes, ref List<Mrs00131ByTimeRDO> emergencyByTimeRDOs)
        {
            if (IsNotNullOrEmpty(emergencyWtimes))
            {
                emergencyByTimeRDOs = emergencyWtimes
                .Select(t => new Mrs00131ByTimeRDO
                {
                    EmergencyWtimeId = t.ID,
                    EmergencyWtimeName = t.EMERGENCY_WTIME_NAME
                }).ToList(); 
            }
        }

        //Doi tuong quan
        private bool IsQu(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__QU; 
        }

        //Doi tuong chinh sach
        private bool IsCs(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DTCS; 
        }

        //Doi tuong dich vu
        private bool IsDv(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE; 
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
        private bool IsBh(string HeinCardNumber)
        {
            return !this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber)
            && !this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber); 
        }

        /// <summary>
        /// Co phai noi khoa hay khong
        /// </summary>
        /// <returns></returns>
        private bool IsInternalMedicineDepartment(long departmentId)
        {
            return HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_INTERNAL_MEDICINE != null
                && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_INTERNAL_MEDICINE.Contains(departmentId); 
        }

        /// <summary>
        /// Co phai ngoai khoa hay khong
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        private bool IsSurgeryDepartment(long departmentId)
        {
            return HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_SURGERY != null
                && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_SURGERY.Contains(departmentId); 
        }

        /// <summary>
        /// Co phai chuyen khoa hay khong
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        private bool IsSpecialtyDepartment(long departmentId)
        {
            return HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_SPECIALTY != null
                && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_SPECIALTY.Contains(departmentId); 
        }

        /// <summary>
        /// Don vi den
        /// </summary>
        /// <returns></returns>
        private bool IsOrgTran(V_HIS_TREATMENT treatment)
        {
            return true;  //tam thoi tat ca deu la don vi den (do BV chua xac dinh duoc nghiep vu cu the)
        }

        /// <summary>
        /// Chuyen vien den
        /// </summary>
        /// <returns></returns>
        private bool IsHospitalTran(V_HIS_TREATMENT treatment)
        {
            return false; //tam thoi tat ca deu la don vi den (do BV chua xac dinh duoc nghiep vu cu the)
        }

        /// <summary>
        /// Gia dinh den
        /// </summary>
        /// <returns></returns>
        private bool IsHomeTran(V_HIS_TREATMENT treatment)
        {
            return false; //tam thoi tat ca deu la don vi den (do BV chua xac dinh duoc nghiep vu cu the)
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
                this.PrepareData(emergencyWtimes, treatments, departmentTrans, PatientTypeAlters); 
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
                    dicSingleTag.Add("IN_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("IN_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                objectTag.AddObjectData(store, "NoiKhoa_DonViDen", new List<Mrs00131ByTypeRDO> { this.noiKhoaDonViDen }); 
                objectTag.AddObjectData(store, "NoiKhoa_GiaDinhDen", new List<Mrs00131ByTypeRDO> { this.noiKhoaGiaDinhDen }); 
                objectTag.AddObjectData(store, "NoiKhoa_ChuyenVienDen", new List<Mrs00131ByTypeRDO> { this.noiKhoaChuyenVienDen }); 

                objectTag.AddObjectData(store, "NgoaiKhoa_DonViDen", new List<Mrs00131ByTypeRDO> { this.ngoaiKhoaDonViDen }); 
                objectTag.AddObjectData(store, "NgoaiKhoa_GiaDinhDen", new List<Mrs00131ByTypeRDO> { this.ngoaiKhoaGiaDinhDen }); 
                objectTag.AddObjectData(store, "NgoaiKhoa_ChuyenVienDen", new List<Mrs00131ByTypeRDO> { this.ngoaiKhoaChuyenVienDen }); 

                objectTag.AddObjectData(store, "ChuyenKhoa_DonViDen", new List<Mrs00131ByTypeRDO> { this.chuyenKhoaDonViDen }); 
                objectTag.AddObjectData(store, "ChuyenKhoa_GiaDinhDen", new List<Mrs00131ByTypeRDO> { this.chuyenKhoaGiaDinhDen }); 
                objectTag.AddObjectData(store, "ChuyenKhoa_ChuyenVienDen", new List<Mrs00131ByTypeRDO> { this.chuyenKhoaChuyenVienDen }); 
                objectTag.AddObjectData(store, "EmergencyByTime", this.emergencyByTimes); 

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
