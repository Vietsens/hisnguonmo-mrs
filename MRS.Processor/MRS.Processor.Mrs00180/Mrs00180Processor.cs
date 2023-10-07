using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatmentBedRoom;
using MOS.MANAGER.HisBedRoom;

namespace MRS.Processor.Mrs00180
{
    internal class Mrs00180Processor : AbstractProcessor
    {
        List<VSarReportMrs00180RDO> _listSarReportMrs00180Rdos = new List<VSarReportMrs00180RDO>();
        Mrs00180Filter CastFilter;
        private long odlDateDateTime;
        private long startDateTime;
        private long finishDateTime;
        string DEPARTMENT_NAME = "";

        public Mrs00180Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00180Filter);
        }
        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00180Filter)this.reportFilter;

                var metyFilterDepartment = new HisDepartmentFilterQuery
                {
                    ID = CastFilter.DEPARTMENT_ID
                };
                var departmentName = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(metyFilterDepartment);
                if (IsNotNullOrEmpty(departmentName))
                {
                    DEPARTMENT_NAME = departmentName.FirstOrDefault().DEPARTMENT_NAME;
                }

                var departmentTranViewFilter = new HisDepartmentTranViewFilterQuery
                {
                    DEPARTMENT_IN_TIME_FROM = CastFilter.DEPARTMENT_IN_TIME_FROM
                };
                var listDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(departmentTranViewFilter);
                if (IsNotNullOrEmpty(listDepartmentTran))
                {
                    //chuyen den khoa lay bao cao
                    var listDepartmentInTrans = listDepartmentTran.Where(o => o.DEPARTMENT_ID == CastFilter.DEPARTMENT_ID && o.DEPARTMENT_IN_TIME.HasValue
                        && o.DEPARTMENT_IN_TIME < CastFilter.DEPARTMENT_IN_TIME_TO).ToList();

                    var listDepartmentOutTrans = listDepartmentTran.Where(o => o.PREVIOUS_ID.HasValue &&
                        (listDepartmentInTrans.Select(s => s.ID).ToList()).Contains(o.PREVIOUS_ID ?? 0)).ToList();

                    var listTreatmentId = new List<long>();
                    listTreatmentId = listDepartmentTran.Select(s => s.TREATMENT_ID).Distinct().ToList();

                    //-------------------------------------------------------------------------------------- V_HIS_PATIENT_TYPE_ALTER
                    var listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
                    var listTreatments = new List<HIS_TREATMENT>();

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            var listPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).GetByTreatmentIds(listIDs);

                            listPatientTypeAlterSub = listPatientTypeAlterSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                            listPatientTypeAlters.AddRange(listPatientTypeAlterSub);

                            var listTreatmentIDs = listPatientTypeAlters.Select(s => s.TREATMENT_ID).Distinct().ToList();

                            //---------------------------------------------------------------------------------------V_HIS_TREATMENT
                            var metyFilterTreatment = new HisTreatmentFilterQuery
                            {
                                IDs = listTreatmentIDs
                            };
                            var listTreatment = new HisTreatmentManager(paramGet).Get(metyFilterTreatment);
                            listTreatments.AddRange(listTreatment);
                        }
                    }

                    listPatientTypeAlters = listPatientTypeAlters.OrderByDescending(o => o.LOG_TIME).ThenByDescending(o => o.ID).GroupBy(o => o.TREATMENT_ID).Select(o => o.First()).ToList();

                    List<HIS_TREATMENT_BED_ROOM> currentTreatmentInDepas = new List<HIS_TREATMENT_BED_ROOM>();
                    HisBedRoomViewFilterQuery bedRoomFilter = new HisBedRoomViewFilterQuery();
                    bedRoomFilter.DEPARTMENT_ID = CastFilter.DEPARTMENT_ID;
                    List<V_HIS_BED_ROOM> listBedRoom = new MOS.MANAGER.HisBedRoom.HisBedRoomManager(paramGet).GetView(bedRoomFilter);

                    HisTreatmentBedRoomFilterQuery TreatmentBedRoomFilter = new HisTreatmentBedRoomFilterQuery();
                    TreatmentBedRoomFilter.IS_IN_ROOM = true;
                    currentTreatmentInDepas = new HisTreatmentBedRoomManager(paramGet).Get(TreatmentBedRoomFilter);


                    if (IsNotNullOrEmpty(listBedRoom) && IsNotNullOrEmpty(currentTreatmentInDepas))
                    {
                        currentTreatmentInDepas = currentTreatmentInDepas.Where(o => listBedRoom.Exists(p => p.ID == o.BED_ROOM_ID)).ToList();
                    }
                    //Nhung BN trong buong nhung thoi gian vao khoa < TIME_FROM
                    if (IsNotNullOrEmpty(currentTreatmentInDepas))
                    {
                        currentTreatmentInDepas = currentTreatmentInDepas.Where(o => !(listTreatments.Select(s => s.ID).ToList()).Contains(o.TREATMENT_ID)).ToList();
                    }
                    var listPatientTypeAlters2 = new List<HIS_PATIENT_TYPE_ALTER>();
                    if (IsNotNullOrEmpty(currentTreatmentInDepas))
                    {
                        var treatIds = currentTreatmentInDepas.Select(o => o.TREATMENT_ID).Distinct().ToList();

                        var listPatientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetByTreatmentIds(treatIds);
                        listPatientTypeAlters2 = listPatientTypeAlterSub.OrderByDescending(o => o.LOG_TIME).ThenByDescending(o => o.ID).GroupBy(o => o.TREATMENT_ID).Select(o => o.First()).ToList();
                    }

                    ProcessFilterData(listDepartmentInTrans, listDepartmentOutTrans, listPatientTypeAlters, listTreatments, listPatientTypeAlters2);

                }
                result = true;

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
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DEPARTMENT_IN_TIME_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DEPARTMENT_IN_TIME_TO));
            dicSingleTag.Add("DEPARTMENT_ID", DEPARTMENT_NAME);

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00180Rdos);

        }

        private void ProcessFilterData(List<V_HIS_DEPARTMENT_TRAN> listDepartmentInTrans,
            List<V_HIS_DEPARTMENT_TRAN> listDepartmentOutTrans,
            List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters,
            List<HIS_TREATMENT> listTreatments,
            List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters2)
        {
            try
            {

                var listPatientTypeAlterGroupByPatientTypes = listPatientTypeAlters.GroupBy(s => s.PATIENT_TYPE_ID).ToList();
                foreach (var listPatientTypeAlterGroupByPatientType in listPatientTypeAlterGroupByPatientTypes)
                {
                    var listTreatment = listPatientTypeAlterGroupByPatientType.Select(s => s.TREATMENT_ID).ToList();
                    var listDepartmentInTranSub = listDepartmentInTrans.Where(s => listTreatment.Contains(s.TREATMENT_ID)).ToList();
                    var listDepartmentOutTranSub = listDepartmentOutTrans.Where(s => listTreatment.Contains(s.TREATMENT_ID)).ToList();
                    var chuyenDen = listDepartmentInTranSub.Where(s => s.PREVIOUS_ID != null).Count();
                    var chuyenKhoa = listDepartmentOutTranSub.Count();

                    var _raVien = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue &&
                        s.END_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID &&
                        s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN
                        ).Select(s => s.ID).ToList();
                    var raVien = listPatientTypeAlters.Count(s => _raVien.Contains(s.TREATMENT_ID) &&
                        s.PATIENT_TYPE_ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);

                    var _xinRaVien = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue &&
                        s.END_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID &&
                        s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN
                        ).Select(s => s.ID).ToList();
                    var xinRaVien = listPatientTypeAlters.Count(s => _xinRaVien.Contains(s.TREATMENT_ID) &&
                        s.PATIENT_TYPE_ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);

                    var _henKhamLai = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue &&
                        s.END_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID &&
                        s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN
                        ).Select(s => s.ID).ToList();
                    var henKhamLai = listPatientTypeAlters.Count(s => _henKhamLai.Contains(s.TREATMENT_ID) &&
                        s.PATIENT_TYPE_ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);

                    var _tuVong = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue &&
                        s.END_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID &&
                        s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET
                        ).Select(s => s.ID).ToList();
                    var tuVong = listPatientTypeAlters.Count(s => _tuVong.Contains(s.TREATMENT_ID) &&
                        s.PATIENT_TYPE_ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);

                    var patient = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);
                    var doiTuong = patient != null ? patient.PATIENT_TYPE_NAME : "";

                    var soCu = listPatientTypeAlters2.Count(s => s.PATIENT_TYPE_ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);

                    var _chuyenVien = listTreatments.Where(s => s.END_DEPARTMENT_ID.HasValue &&
                        s.END_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID &&
                        s.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN
                        ).Select(s => s.ID).ToList();
                    var chuyenVien = listPatientTypeAlters.Count(s => _chuyenVien.Contains(s.TREATMENT_ID) &&
                        s.PATIENT_TYPE_ID == listPatientTypeAlterGroupByPatientType.First().PATIENT_TYPE_ID);

                    VSarReportMrs00180RDO rdo = new VSarReportMrs00180RDO()
                    {
                        DOI_TUONG = doiTuong,
                        RA_VIEN = raVien + henKhamLai + xinRaVien,
                        CHUYEN_DEN = chuyenDen,
                        CHUYEN_KHOA = chuyenKhoa,
                        TU_VONG = tuVong,
                        CHUYEN_VIEN = chuyenVien,
                        SO_CU = soCu
                    };
                    _listSarReportMrs00180Rdos.Add(rdo);
                }
                //--------------------------------------------------------------------------------------------------
                LogSystem.Info("Ket thuc xu ly du lieu MRS00180 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }
    }
}
