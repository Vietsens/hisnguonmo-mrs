using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisMilitaryRank;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
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

namespace MRS.Processor.Mrs00121
{
    public class Mrs00121Processor : AbstractProcessor
    {
        List<Mrs00121RDO> ListSereServRdo = new List<Mrs00121RDO>();
        Mrs00121Filter CastFilter = null;
        private long startDateTime;
        private long finishDateTime;
        private long odlDateDateTime;
        private List<V_HIS_TREATMENT> listTreatmentIntroduceMilitarys = new List<V_HIS_TREATMENT>();
        private List<int?> tongSoQuans = new List<int?> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private List<int?> tongSoBHYT = new List<int?> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private List<int?> tongSoBaoCao = new List<int?> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private List<string> stringNumber = new List<string>();
        private string DEPARTMENT_NAME;
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_TREATMENT> listTrearment = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTyprAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_CAREER> listCareer = new List<HIS_CAREER>();

        public Mrs00121Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00121Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00121Filter)this.reportFilter;
                var paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu, filter: " +
                                                        Inventec.Common.Logging.LogUtil.TraceData(
                                                            Inventec.Common.Logging.LogUtil.GetMemberName(
                                                                () => CastFilter), CastFilter));
                //khoa duoc chon
                var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).GetById(CastFilter.DEPARTMENT_ID);
                DEPARTMENT_NAME = department.DEPARTMENT_NAME;

                //ngay bao cao
                var dateReport = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(CastFilter.DATE_TIME);
                var startDate = new DateTime(dateReport.Value.Year, dateReport.Value.Month,
                    dateReport.Value.Day, 00, 00, 00);
                startDateTime =
                    Inventec.Common.TypeConvert.Parse.ToInt64(Convert.ToDateTime(startDate).ToString("yyyyMMddHHmmss"));
                var finishDate = new DateTime(dateReport.Value.Year, dateReport.Value.Month,
                    dateReport.Value.Day, 23, 59, 59);
                finishDateTime =
                    Inventec.Common.TypeConvert.Parse.ToInt64(Convert.ToDateTime(finishDate).ToString("yyyyMMddHHmmss"));
                var odlDate = dateReport.Value.AddDays(-1);
                var oldFinishDate = new DateTime(odlDate.Year, odlDate.Month, odlDate.Day, 23, 59, 59);
                odlDateDateTime =
                    Inventec.Common.TypeConvert.Parse.ToInt64(
                        Convert.ToDateTime(oldFinishDate).ToString("yyyyMMddHHmmss"));

                //toi khoa trong ngay
                var metyFilterDepartmnetTran1 = new HisDepartmentTranViewFilterQuery
                {
                    DEPARTMENT_ID = CastFilter.DEPARTMENT_ID,
                    DEPARTMENT_IN_TIME_FROM = startDateTime,
                    DEPARTMENT_IN_TIME_TO = finishDateTime
                };
                var listDepartmentTran1 =
                    new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartmnetTran1);
                listDepartmentTrans.AddRange(listDepartmentTran1);

                //cac lan chuyen toi khoa ngay truoc do
                var metyFilterDepartmnetTran2 = new HisDepartmentTranViewFilterQuery
                {
                    DEPARTMENT_ID = CastFilter.DEPARTMENT_ID,
                    DEPARTMENT_IN_TIME_TO = odlDateDateTime
                    //IN_OUT = 1
                };
                var listDepartmentTran2 =
                    new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartmnetTran2);
                listDepartmentTrans.AddRange(listDepartmentTran2);

                //cac hsdt cua cac bn tuong ung
                var listTreatmentIds = listDepartmentTrans.Select(s => s.TREATMENT_ID).ToList();

                listTrearment = GetTreatment(listTreatmentIds);

                //cac lan chuyen doi tuong cua hsdt

                listPatientTyprAlter = GetPatientTypeAlter(listTreatmentIds);

                //Nghe nghiep
                var filterCareer = new HisCareerFilterQuery();
                filterCareer.ID = MRS.MANAGER.Config.HisCareerCFG.CAREER_ID__RETIRED_MILITARY;
                listCareer = new MOS.MANAGER.HisCareer.HisCareerManager().Get(filterCareer);

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai trong qua trinh lay du lieu V_HIS_DEPARTMENT_TRAN, HIS_DEPARTMENT MRS00121." + Inventec.Common.Logging.LogUtil.TraceData("castFilter", CastFilter));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<V_HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(List<long> listTreatmentIds)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = new List<V_HIS_PATIENT_TYPE_ALTER>();
            try
            {
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    var paramGet = new CommonParam();
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterfilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listId
                        };
                        var patientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterfilter);
                        result.AddRange(patientTypeAlterSub);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<V_HIS_PATIENT_TYPE_ALTER>();
            }
            return result;
        }

        private List<V_HIS_TREATMENT> GetTreatment(List<long> listTreatmentIds)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();
            try
            {
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    var paramGet = new CommonParam();
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listId = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentViewFilterQuery treatmentfilter = new HisTreatmentViewFilterQuery()
                        {
                            IDs = listId
                        };
                        var treatmentSub = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentfilter);
                        result.AddRange(treatmentSub);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<V_HIS_TREATMENT>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ProcessFillterData(listDepartmentTrans, listTrearment, listPatientTyprAlter, listCareer);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFillterData(List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans,
            List<V_HIS_TREATMENT> listTreatments,
            List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters,
            List<HIS_CAREER> listCareer)
        {
            DoiTuongQuan(listTreatments, listDepartmentTrans, listCareer);
            DoiTuongBHYT(listDepartmentTrans, listTreatments, listPatientTypeAlters);
        }

        private void DoiTuongQuan(List<V_HIS_TREATMENT> listTreatments,
            List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans,
            List<HIS_CAREER> listCareer)
        {
            try
            {
                //listTreatmentIntroduceMilitarys = listTreatments.Where(s => s.TDL_PATIENT_CAREER_NAME == listCareer.FirstOrDefault().CAREER_NAME).ToList(); 
                ////   Tướng
                //var listTreatmentIds1 = listTreatments.Where(s => s.TDL_PATIENT_MILITARY_RANK_NAME != null &&
                //    HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL.Contains(s.MILITARY_RANK_ID.Value)).Select(s => s.ID).ToList(); 
                //var listDepartmentTrans1 = listDepartmentTrans.Where(s => listTreatmentIds1.Contains(s.TREATMENT_ID)).ToList(); 
                //TinhSoLuong(listDepartmentTrans1, listTreatments, "Tướng", true); 
                ////   3//-4//
                //var listTreatmentIds2 = listTreatments.Where(s => s.MILITARY_RANK_ID.HasValue &&
                //    HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL.Contains(s.MILITARY_RANK_ID.Value)).Select(s => s.ID).ToList(); 
                //var listDepartmentTrans2 = listDepartmentTrans.Where(s => listTreatmentIds2.Contains(s.TREATMENT_ID)).ToList(); 
                //TinhSoLuong(listDepartmentTrans2, listTreatments, "3//-4//", true); 
                ////   1//-2//
                //var listTreatmentIds3 = listTreatments.Where(s => s.MILITARY_RANK_ID.HasValue &&
                //    HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL.Contains(s.MILITARY_RANK_ID.Value)).Select(s => s.ID).ToList(); 
                //var listDepartmentTrans3 = listDepartmentTrans.Where(s => listTreatmentIds3.Contains(s.TREATMENT_ID)).ToList(); 
                //TinhSoLuong(listDepartmentTrans3, listTreatments, "1//-2//", true); 
                ////   Úy
                //var listTreatmentIds4 = listTreatments.Where(s => s.MILITARY_RANK_ID.HasValue &&
                //    HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL.Contains(s.MILITARY_RANK_ID.Value)).Select(s => s.ID).ToList(); 
                //var listDepartmentTrans4 = listDepartmentTrans.Where(s => listTreatmentIds4.Contains(s.TREATMENT_ID)).ToList(); 
                //TinhSoLuong(listDepartmentTrans4, listTreatments, "Úy", true); 
                ////   HSQ-CS
                //var listTreatmentIds5 = listTreatments.Where(s => s.MILITARY_RANK_ID.HasValue &&
                //    HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL.Contains(s.MILITARY_RANK_ID.Value)).Select(s => s.ID).ToList(); 
                //var listDepartmentTrans5 = listDepartmentTrans.Where(s => listTreatmentIds5.Contains(s.TREATMENT_ID)).ToList(); 
                //TinhSoLuong(listDepartmentTrans5, listTreatments, "HSQ-CS", true); 
                //   Tổng
                stringNumber.Add(string.Join(",", tongSoQuans));
                //var rdo = new Mrs00121RDO(tongSoQuans, "Tổng số"); 
                //ListSereServRdo.Add(rdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void DoiTuongBHYT(List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans,
            List<V_HIS_TREATMENT> listTreatments,
            List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters)
        {
            try
            {
                //BHYT quân đội
                var PatientTypeAlters1 = listPatientTypeAlters.Where(s => IsBhQd(s.HEIN_CARD_NUMBER)).Select(ss => ss.TREATMENT_ID).ToList();
                var departmentTrans1 = listDepartmentTrans.Where(s => PatientTypeAlters1.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans1, listTreatments, "BHQD", false);
                //chính sách
                var PatientTypeAlters2 = listPatientTypeAlters.Where(s => s.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DTCS).Select(ss => ss.TREATMENT_ID).ToList();
                var departmentTrans2 = listDepartmentTrans.Where(s => PatientTypeAlters2.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans2, listTreatments, "Chính Sách", false);
                //BHYT thân nhân

                var PatientTypeAlters3 = listPatientTypeAlters.Where(s => IsBhTq(s.HEIN_CARD_NUMBER)).Select(ss => ss.TREATMENT_ID).ToList();
                var departmentTrans3 = listDepartmentTrans.Where(s => PatientTypeAlters3.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans3, listTreatments, "BHYT thân nhân", false);
                //BHYT quân đội hưu
                var dtBHQH = MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS;
                var treatmentIds = listTreatments.Select(s => s.ID).ToList();
                var PatientTypeAlterIds = listPatientTypeAlters.Where(s => dtBHQH.Contains(s.HEIN_CARD_NUMBER) && treatmentIds.Contains(s.TREATMENT_ID)).Select(s => s.TREATMENT_ID).ToList();
                var departmentTrans4 = listDepartmentTrans.Where(s => PatientTypeAlterIds.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans4, listTreatments, "BHQH", false);
                //trẻ em dưới 6 tuổi

                var PatientTypeAlters5 = listPatientTypeAlters.Where(s => IsBhTe(s.HEIN_CARD_NUMBER)).Select(ss => ss.TREATMENT_ID).ToList();
                var departmentTrans5 = listDepartmentTrans.Where(s => PatientTypeAlters5.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans5, listTreatments, "Trẻ em dưới 6 tuổi", false);
                //Dịch vụ y tế
                var PatientTypeAlters7 = listPatientTypeAlters.Where(s => s.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Select(ss => ss.TREATMENT_ID).ToList();
                var departmentTrans7 = listDepartmentTrans.Where(s => PatientTypeAlters7.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans7, listTreatments, "Dịch Vụ Y Tế", false);
                //Quốc tế
                var PatientTypeAlters8 = listPatientTypeAlters.Where(s => s.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DTQT).Select(ss => ss.TREATMENT_ID).ToList();
                var departmentTrans8 = listDepartmentTrans.Where(s => PatientTypeAlters8.Contains(s.TREATMENT_ID)).ToList();
                TinhSoLuong(departmentTrans8, listTreatments, "Đối tượng quốc tế", false);
                //BHYT khác
                var departmentTrans6 = listDepartmentTrans.Where(s => !departmentTrans1.Contains(s) && !departmentTrans2.Contains(s) && !departmentTrans4.Contains(s) &&
                    !departmentTrans5.Contains(s) && !departmentTrans7.Contains(s) && !departmentTrans8.Contains(s)).ToList(); //departmentTrans3
                TinhSoLuong(departmentTrans6, listTreatments, "BHYT khác", false);
                //Đối tượng khác
                var departmentTrans9 = listDepartmentTrans.Where(s => !departmentTrans1.Contains(s) && !departmentTrans2.Contains(s) && !departmentTrans4.Contains(s) &&
                    !departmentTrans5.Contains(s) && !departmentTrans6.Contains(s) && !departmentTrans7.Contains(s) && !departmentTrans8.Contains(s)).ToList(); //departmentTrans3
                TinhSoLuong(departmentTrans9, listTreatments, "Đối tượng khác", false);
                //Tổng
                for (var i = 0; i < tongSoBaoCao.Count; i++)
                {
                    tongSoBaoCao[i] = tongSoQuans[i] + tongSoBHYT[i];
                }
                var rdo = new Mrs00121RDO(tongSoBaoCao, "Tổng số báo cáo");
                ListSereServRdo.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void TinhSoLuong(List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans,
            List<V_HIS_TREATMENT> listTreatments,
            string tenLoai, bool a)
        {
            #region-------------------------- list kết quả số lượng đối tượng trả về
            var listTotal = new List<int?>
            {
                0,// 0.SO_CU
                0,// 1.DON_VI_DEN
                0,// 2.GIA_DINH_DEN
                0,// 3.CHUYEN_VIEN_DEN
                0,// 4.CHUYEN_KHOA_DEN
                0,// 5.TONG_BENH_NHAN_TANG
                0,// 6.KHOI_BENH
                0,// 7.CHUYEN_VIEN
                0,// 8.CHUYEN_KHOA
                0,// 9.NANG_XIN_VE
                0,// 10.TU_VONG
                0,// 11.LY_DO_KHAC
                0,// 12.TONG_BENH_NHAN_GIAM
                0//  13.BENH_NHAN_CON_LAI
            };
            #endregion
            #region   -------------------------- tính số lượng đối tượng quân

            if (a)
            {
                try
                {
                    var khoiBenh = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__KHOI.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    var chuyenVien = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__CV.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    var xinVe = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__XV.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    var tuVong = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH == s.END_DEPARTMENT_ID.Value).Select(s => s.ID).ToList();
                    var khac = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__KHAC.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    foreach (var departmentTran in listDepartmentTrans)
                    {
                        if (departmentTran.DEPARTMENT_IN_TIME <= odlDateDateTime)
                        {
                            //0.số cũ
                            if (departmentTran.DEPARTMENT_IN_TIME <= odlDateDateTime)
                                listTotal[0] = listTotal[0] + 1;
                        }
                        else if (departmentTran.DEPARTMENT_IN_TIME >= startDateTime || departmentTran.DEPARTMENT_IN_TIME >= finishDateTime)
                        {
                            //1.đơn vị đến
                            if (listTreatmentIntroduceMilitarys.Select(s => s.ID).Contains(departmentTran.TREATMENT_ID))
                                listTotal[1] = listTotal[1] + 1;
                            //2.gia đình đến
                            if (!listTreatmentIntroduceMilitarys.Select(s => s.ID).Contains(departmentTran.TREATMENT_ID))
                                listTotal[2] = listTotal[2] + 1;
                            ////3.chyển viện đến
                            //WARNING
                            //if (departmentTran.IN_OUT == 1)
                            //    listTotal[3] = listTotal[3] + 1; 
                            ////4.chuyển khoa đến
                            //if (departmentTran.IN_OUT == 1 && departmentTran.NEXT_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID && departmentTran.IS_RECEIVE == 1)
                            //    listTotal[4] = listTotal[4] + 1; 
                            //6.khỏi bệnh
                            if (khoiBenh.Contains(departmentTran.TREATMENT_ID))
                                listTotal[6] = listTotal[6] + 1;
                            //7.chuyển viện
                            if (chuyenVien.Contains(departmentTran.TREATMENT_ID))
                                listTotal[7] = listTotal[7] + 1;
                            ////8.chuyển khoa
                            //WARNING
                            //if (departmentTran.IN_OUT != 1 && departmentTran.NEXT_DEPARTMENT_ID != CastFilter.DEPARTMENT_ID)
                            //    listTotal[8] = listTotal[8] + 1; 
                            //9.nặng xin về
                            if (xinVe.Contains(departmentTran.TREATMENT_ID))
                                listTotal[9] = listTotal[9] + 1;
                            //10.tử vong
                            if (tuVong.Contains(departmentTran.TREATMENT_ID))
                                listTotal[10] = listTotal[10] + 1;
                            //11.khác
                            if (khac.Contains(departmentTran.TREATMENT_ID))
                                listTotal[11] = listTotal[11] + 1;
                        }
                    }
                    //5.tổng (bệnh nhân tăng)
                    listTotal[5] = listTotal[0] + listTotal[1] + listTotal[2] + listTotal[3] + listTotal[4];
                    //12.tổng (bệnh nhân giảm)
                    listTotal[12] = listTotal[6] + listTotal[7] + listTotal[8] + listTotal[9] + listTotal[10] + listTotal[11];
                    //13.còn lại
                    listTotal[13] = listTotal[5] - listTotal[12];

                    for (var i = 0; i < tongSoQuans.Count; i++)
                    {
                        tongSoQuans[i] = tongSoQuans[i] + listTotal[i];
                    }

                    var listTotalJoinString = string.Join(",", listTotal);
                    stringNumber.Add(listTotalJoinString);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
            #endregion
            #region   -------------------------- tính số lượng đối tượng BHYT

            else
            {
                try
                {
                    var khoiBenh = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__KHOI.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    var chuyenVien = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__CV.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    var xinVe = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__XV.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    var tuVong = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__DEATH == s.END_DEPARTMENT_ID.Value).Select(s => s.ID).ToList();
                    var khac = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue && MRS.MANAGER.Config.HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__KHAC.Contains(s.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList();
                    foreach (var departmentTran in listDepartmentTrans)
                    {
                        if (departmentTran.DEPARTMENT_IN_TIME <= odlDateDateTime)
                        {
                            //0.số cũ
                            //WARNING  //if (departmentTran.DEPARTMENT_IN_TIME <= odlDateDateTime && departmentTran.IN_OUT == 1 && departmentTran.NEXT_DEPARTMENT_ID == null)
                            //listTotal[0] = listTotal[0] + 1; 
                        }
                        else if (departmentTran.DEPARTMENT_IN_TIME >= startDateTime || departmentTran.DEPARTMENT_IN_TIME >= finishDateTime)
                        {
                            //1.đơn vị đến
                            if (listTreatmentIntroduceMilitarys.Select(s => s.ID).Contains(departmentTran.TREATMENT_ID))
                                listTotal[1] = listTotal[1] + 1;
                            //2.gia đình đến
                            if (!listTreatmentIntroduceMilitarys.Select(s => s.ID).Contains(departmentTran.TREATMENT_ID))
                                listTotal[2] = listTotal[2] + 1;
                            //3.chyển viện đến
                            //WARNING
                            //if (departmentTran.IN_OUT == 1)
                            //    listTotal[3] = listTotal[3] + 1; 
                            ////4.chuyển khoa đến
                            //if (departmentTran.IN_OUT == 1 && departmentTran.NEXT_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID &&
                            //    departmentTran.IS_RECEIVE == 1)
                            //    listTotal[4] = listTotal[4] + 1; 
                            //5.tổng (bệnh nhân tăng)
                            //--------------------------------listTotal[5]
                            //6.khỏi bệnh
                            if (khoiBenh.Contains(departmentTran.TREATMENT_ID))
                                listTotal[6] = listTotal[6] + 1;
                            //7.chuyển viện
                            if (chuyenVien.Contains(departmentTran.TREATMENT_ID))
                                listTotal[7] = listTotal[7] + 1;
                            //8.chuyển khoa
                            //WARNNING
                            //if (departmentTran.IN_OUT != 1 && departmentTran.NEXT_DEPARTMENT_ID != CastFilter.DEPARTMENT_ID)
                            //    listTotal[8] = listTotal[8] + 1; 
                            //9.nặng xin về
                            if (xinVe.Contains(departmentTran.TREATMENT_ID))
                                listTotal[9] = listTotal[9] + 1;
                            //10.tử vong
                            if (tuVong.Contains(departmentTran.TREATMENT_ID))
                                listTotal[10] = listTotal[10] + 1;
                            //11.khác
                            if (khac.Contains(departmentTran.TREATMENT_ID))
                                listTotal[11] = listTotal[11] + 1;
                            //12.tổng (bệnh nhân giảm)
                            //-----------------------------listTotal[12]
                            //13.còn lại
                            //-----------------------------listTotal[13]
                        }
                    }
                    //5.tổng (bệnh nhân tăng)
                    listTotal[5] = listTotal[0] + listTotal[1] + listTotal[2] + listTotal[3] + listTotal[4];
                    //12.tổng (bệnh nhân giảm)
                    listTotal[12] = listTotal[6] + listTotal[7] + listTotal[8] + listTotal[9] + listTotal[10] + listTotal[11];
                    //13.còn lại
                    listTotal[13] = listTotal[5] - listTotal[12];

                    for (var i = 0; i < tongSoBHYT.Count; i++)
                    {
                        tongSoBHYT[i] = tongSoBHYT[i] + listTotal[i];
                    }

                    var rdo = new Mrs00121RDO(listTotal, tenLoai);
                    ListSereServRdo.Add(rdo);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
            #endregion
        }

        #region----------- phân loại đối tượng thẻ BHYT
        private bool IsBhQd(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber);
        }

        private bool IsBhTq(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber);
        }

        private bool IsBhTe(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TE, HeinCardNumber);
        }

        //private bool IsBhQh(V_HIS_TREATMENT treatment, string HeinCardNumber)
        //{
        //    return treatment != null
        //        && treatment.CAREER_ID == HisCareerCFG.CAREER_ID__RETIRED_MILITARY
        //        && this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS, HeinCardNumber); 
        //}

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

        private bool IsHeinCardNumberPrefixWith(string prefix, string HeinCardNumber)
        {
            return !string.IsNullOrWhiteSpace(prefix)
                && !string.IsNullOrWhiteSpace(HeinCardNumber)
                && HeinCardNumber.StartsWith(prefix);
        }
        #endregion

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_TIME_REPORT", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TIME));
                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);
                for (var i = 0; i < stringNumber.Count; i++)
                {
                    var gg = stringNumber[i].Split(',').ToList();
                    if (i == 0)
                    {
                        dicSingleTag.Add("SO_CU1", gg[0]);
                        dicSingleTag.Add("DON_VI_DEN1", gg[1]);
                        dicSingleTag.Add("GIA_DINH_DEN1", gg[2]);
                        dicSingleTag.Add("CHUYEN_VIEN_DEN1", gg[3]);
                        dicSingleTag.Add("CHUYEN_KHOA_DEN1", gg[4]);
                        dicSingleTag.Add("TONG_BENH_NHAN_TANG1", gg[5]);
                        dicSingleTag.Add("KHOI_BENH1", gg[6]);
                        dicSingleTag.Add("CHUYEN_VIEN1", gg[7]);
                        dicSingleTag.Add("CHUYEN_KHOA1", gg[8]);
                        dicSingleTag.Add("NANG_XIN_VE1", gg[9]);
                        dicSingleTag.Add("TU_VONG1", gg[10]);
                        dicSingleTag.Add("LY_DO_KHAC1", gg[11]);
                        dicSingleTag.Add("TONG_BENH_NHAN_GIAM1", gg[12]);
                        dicSingleTag.Add("BENH_NHAN_CON_LAI1", gg[13]);
                    }
                    else if (i == 1)
                    {
                        dicSingleTag.Add("SO_CU2", gg[0]);
                        dicSingleTag.Add("DON_VI_DEN2", gg[1]);
                        dicSingleTag.Add("GIA_DINH_DEN2", gg[2]);
                        dicSingleTag.Add("CHUYEN_VIEN_DEN2", gg[3]);
                        dicSingleTag.Add("CHUYEN_KHOA_DEN2", gg[4]);
                        dicSingleTag.Add("TONG_BENH_NHAN_TANG2", gg[5]);
                        dicSingleTag.Add("KHOI_BENH2", gg[6]);
                        dicSingleTag.Add("CHUYEN_VIEN2", gg[7]);
                        dicSingleTag.Add("CHUYEN_KHOA2", gg[8]);
                        dicSingleTag.Add("NANG_XIN_VE2", gg[9]);
                        dicSingleTag.Add("TU_VONG2", gg[10]);
                        dicSingleTag.Add("LY_DO_KHAC2", gg[11]);
                        dicSingleTag.Add("TONG_BENH_NHAN_GIAM2", gg[12]);
                        dicSingleTag.Add("BENH_NHAN_CON_LAI2", gg[13]);
                    }
                    else if (i == 2)
                    {
                        dicSingleTag.Add("SO_CU3", gg[0]);
                        dicSingleTag.Add("DON_VI_DEN3", gg[1]);
                        dicSingleTag.Add("GIA_DINH_DEN3", gg[2]);
                        dicSingleTag.Add("CHUYEN_VIEN_DEN3", gg[3]);
                        dicSingleTag.Add("CHUYEN_KHOA_DEN3", gg[4]);
                        dicSingleTag.Add("TONG_BENH_NHAN_TANG3", gg[5]);
                        dicSingleTag.Add("KHOI_BENH3", gg[6]);
                        dicSingleTag.Add("CHUYEN_VIEN3", gg[7]);
                        dicSingleTag.Add("CHUYEN_KHOA3", gg[8]);
                        dicSingleTag.Add("NANG_XIN_VE3", gg[9]);
                        dicSingleTag.Add("TU_VONG3", gg[10]);
                        dicSingleTag.Add("LY_DO_KHAC3", gg[11]);
                        dicSingleTag.Add("TONG_BENH_NHAN_GIAM3", gg[12]);
                        dicSingleTag.Add("BENH_NHAN_CON_LAI3", gg[13]);
                    }
                    else if (i == 3)
                    {
                        dicSingleTag.Add("SO_CU4", gg[0]);
                        dicSingleTag.Add("DON_VI_DEN4", gg[1]);
                        dicSingleTag.Add("GIA_DINH_DEN4", gg[2]);
                        dicSingleTag.Add("CHUYEN_VIEN_DEN4", gg[3]);
                        dicSingleTag.Add("CHUYEN_KHOA_DEN4", gg[4]);
                        dicSingleTag.Add("TONG_BENH_NHAN_TANG4", gg[5]);
                        dicSingleTag.Add("KHOI_BENH4", gg[6]);
                        dicSingleTag.Add("CHUYEN_VIEN4", gg[7]);
                        dicSingleTag.Add("CHUYEN_KHOA4", gg[8]);
                        dicSingleTag.Add("NANG_XIN_VE4", gg[9]);
                        dicSingleTag.Add("TU_VONG4", gg[10]);
                        dicSingleTag.Add("LY_DO_KHAC4", gg[11]);
                        dicSingleTag.Add("TONG_BENH_NHAN_GIAM4", gg[12]);
                        dicSingleTag.Add("BENH_NHAN_CON_LAI4", gg[13]);
                    }
                    else if (i == 4)
                    {
                        dicSingleTag.Add("SO_CU5", gg[0]);
                        dicSingleTag.Add("DON_VI_DEN5", gg[1]);
                        dicSingleTag.Add("GIA_DINH_DEN5", gg[2]);
                        dicSingleTag.Add("CHUYEN_VIEN_DEN5", gg[3]);
                        dicSingleTag.Add("CHUYEN_KHOA_DEN5", gg[4]);
                        dicSingleTag.Add("TONG_BENH_NHAN_TANG5", gg[5]);
                        dicSingleTag.Add("KHOI_BENH5", gg[6]);
                        dicSingleTag.Add("CHUYEN_VIEN5", gg[7]);
                        dicSingleTag.Add("CHUYEN_KHOA5", gg[8]);
                        dicSingleTag.Add("NANG_XIN_VE5", gg[9]);
                        dicSingleTag.Add("TU_VONG5", gg[10]);
                        dicSingleTag.Add("LY_DO_KHAC5", gg[11]);
                        dicSingleTag.Add("TONG_BENH_NHAN_GIAM5", gg[12]);
                        dicSingleTag.Add("BENH_NHAN_CON_LAI5", gg[13]);
                    }
                    else if (i == 5)
                    {
                        dicSingleTag.Add("SO_CU6", gg[0]);
                        dicSingleTag.Add("DON_VI_DEN6", gg[1]);
                        dicSingleTag.Add("GIA_DINH_DEN6", gg[2]);
                        dicSingleTag.Add("CHUYEN_VIEN_DEN6", gg[3]);
                        dicSingleTag.Add("CHUYEN_KHOA_DEN6", gg[4]);
                        dicSingleTag.Add("TONG_BENH_NHAN_TANG6", gg[5]);
                        dicSingleTag.Add("KHOI_BENH6", gg[6]);
                        dicSingleTag.Add("CHUYEN_VIEN6", gg[7]);
                        dicSingleTag.Add("CHUYEN_KHOA6", gg[8]);
                        dicSingleTag.Add("NANG_XIN_VE6", gg[9]);
                        dicSingleTag.Add("TU_VONG6", gg[10]);
                        dicSingleTag.Add("LY_DO_KHAC6", gg[11]);
                        dicSingleTag.Add("TONG_BENH_NHAN_GIAM6", gg[12]);
                        dicSingleTag.Add("BENH_NHAN_CON_LAI6", gg[13]);
                    }
                }

                objectTag.AddObjectData(store, "Report", ListSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
