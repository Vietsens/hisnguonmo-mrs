using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBedRoom;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using MOS.MANAGER.HisTreatmentBedRoom;

namespace MRS.Processor.Mrs00500
{
    class Mrs00500Processor : AbstractProcessor
    {
        Mrs00500Filter castFilter = null;
        List<Mrs00500RDO> listRdo = new List<Mrs00500RDO>();

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_TREATMENT_BED_ROOM> listTreatmentBedRooms = new List<V_HIS_TREATMENT_BED_ROOM>();
        List<V_HIS_BED_ROOM> listBedRooms = new List<V_HIS_BED_ROOM>();
        List<SERVICE> listServices = new List<SERVICE>();
        List<HIS_TREATMENT> listTreatments = new List<HIS_TREATMENT>();

        public Mrs00500Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00500Filter);
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00500Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao Mrs00500: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                // lấy các buồng thuộc khoa
                HisBedRoomViewFilterQuery bedRoomViewFilter = new HisBedRoomViewFilterQuery(); 
                bedRoomViewFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                listBedRooms = new MOS.MANAGER.HisBedRoom.HisBedRoomManager(param).GetView(bedRoomViewFilter) ?? new List<V_HIS_BED_ROOM>(); 
                //
                HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                sereServViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                sereServViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                sereServViewFilter.REQUEST_DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                sereServViewFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                if (castFilter.IS_DIIM)
                {
                    sereServViewFilter.SERVICE_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                    }; 
                }
                if (castFilter.IS_EXAM)
                {
                    sereServViewFilter.SERVICE_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                    }; 
                }
                if (castFilter.IS_MEDI_MATE)
                {
                    sereServViewFilter.SERVICE_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT
                    }; 
                }
                if (castFilter.IS_MISU)
                {
                    sereServViewFilter.SERVICE_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
                    }; 
                }
                if (castFilter.IS_SURG)
                {
                    sereServViewFilter.SERVICE_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                    }; 
                }
                if (castFilter.IS_TEST)
                {
                    sereServViewFilter.SERVICE_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN
                    }; 
                }
                //sereServViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                //sereServViewFilter.HAS_EXECUTE = true; 
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter); 

                listSereServs = listSereServs.Where(w => w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 

                var skip = 0; 
                while (listSereServs.Count - skip > 0)
                {
                    var listIDs = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisTreatmentBedRoomViewFilterQuery treatmentBedRoomViewfilter = new HisTreatmentBedRoomViewFilterQuery(); 
                    treatmentBedRoomViewfilter.TREATMENT_IDs = listIDs.Select(s => s.TDL_TREATMENT_ID.Value).ToList(); 
                    treatmentBedRoomViewfilter.ADD_TIME_TO = castFilter.TIME_TO; 
                    listTreatmentBedRooms.AddRange(new MOS.MANAGER.HisTreatmentBedRoom.HisTreatmentBedRoomManager(param).GetView(treatmentBedRoomViewfilter) ?? new List<V_HIS_TREATMENT_BED_ROOM>()); 

                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery(); 
                    treatmentFilter.IDs = listIDs.Select(s => s.TDL_TREATMENT_ID.Value).ToList(); 
                    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).Get(treatmentFilter)); 
                }

                listTreatmentBedRooms = listTreatmentBedRooms.Where(w => listBedRooms.Select(s => s.ID).Contains(w.BED_ROOM_ID)).ToList(); 

                listTreatments.Distinct(); 
                //
                if (IsNotNullOrEmpty(listSereServs))
                {
                    var listSereServGroupByServiceIds = listSereServs.OrderBy(o => o.TDL_SERVICE_NAME).GroupBy(g => g.SERVICE_ID); 
                    int i = 1; 
                    foreach (var service in listSereServGroupByServiceIds)
                    {
                        var s = new SERVICE(); 
                        s.ID = i; 
                        s.SERVICE_ID = service.FirstOrDefault().SERVICE_ID; 
                        s.SERVICE_CODE = service.FirstOrDefault().TDL_SERVICE_CODE; 
                        s.SERVICE_NAME = service.FirstOrDefault().TDL_SERVICE_NAME; 
                        listServices.Add(s); 
                        i++; 
                    }
                }
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
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listSereServs))
                {
                    var listTreatmentss = listSereServs.GroupBy(g => g.TDL_TREATMENT_ID).ToList();
                    foreach (var treatment in listTreatmentss)
                    {
                        var rdo = new Mrs00500RDO();
                        var patient = listTreatments.Where(w => w.ID == treatment.FirstOrDefault().TDL_TREATMENT_ID.Value).ToList();
                        if (IsNotNullOrEmpty(patient))
                        {
                            rdo.PATIENT_CODE = patient.FirstOrDefault().TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = patient.FirstOrDefault().TDL_PATIENT_NAME;

                            rdo.DOB = patient.FirstOrDefault().TDL_PATIENT_DOB;
                        }
                        rdo.TREATMENT_CODE = treatment.FirstOrDefault().TDL_TREATMENT_CODE;

                        var treatmentBedRooms = listTreatmentBedRooms.Where(w => w.TREATMENT_ID == treatment.FirstOrDefault().TDL_TREATMENT_ID.Value).ToList();
                        if (IsNotNullOrEmpty(treatmentBedRooms))
                        {
                            rdo.BED_ROOM_CODE = treatmentBedRooms.OrderByDescending(o => o.ADD_TIME).FirstOrDefault().BED_ROOM_CODE;
                            rdo.BED_ROOM_NAME = treatmentBedRooms.OrderByDescending(o => o.ADD_TIME).FirstOrDefault().BED_ROOM_NAME;
                        }

                        foreach (var sereServ in treatment.GroupBy(g => g.SERVICE_ID))
                        {
                            var sub = listServices.Where(w => w.SERVICE_ID == sereServ.FirstOrDefault().SERVICE_ID).ToList();
                            if (IsNotNullOrEmpty(sub))
                            {
                                switch (sub.FirstOrDefault().ID)
                                {
                                    case 01: rdo.SERVICE_01 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 02: rdo.SERVICE_02 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 03: rdo.SERVICE_03 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 04: rdo.SERVICE_04 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 05: rdo.SERVICE_05 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 06: rdo.SERVICE_06 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 07: rdo.SERVICE_07 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 08: rdo.SERVICE_08 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 09: rdo.SERVICE_09 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 10: rdo.SERVICE_10 = sereServ.Sum(su => su.AMOUNT); break;

                                    case 11: rdo.SERVICE_11 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 12: rdo.SERVICE_12 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 13: rdo.SERVICE_13 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 14: rdo.SERVICE_14 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 15: rdo.SERVICE_15 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 16: rdo.SERVICE_16 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 17: rdo.SERVICE_17 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 18: rdo.SERVICE_18 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 19: rdo.SERVICE_19 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 20: rdo.SERVICE_20 = sereServ.Sum(su => su.AMOUNT); break;

                                    case 21: rdo.SERVICE_21 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 22: rdo.SERVICE_22 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 23: rdo.SERVICE_23 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 24: rdo.SERVICE_24 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 25: rdo.SERVICE_25 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 26: rdo.SERVICE_26 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 27: rdo.SERVICE_27 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 28: rdo.SERVICE_28 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 29: rdo.SERVICE_29 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 30: rdo.SERVICE_30 = sereServ.Sum(su => su.AMOUNT); break;

                                    case 31: rdo.SERVICE_31 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 32: rdo.SERVICE_32 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 33: rdo.SERVICE_33 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 34: rdo.SERVICE_34 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 35: rdo.SERVICE_35 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 36: rdo.SERVICE_36 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 37: rdo.SERVICE_37 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 38: rdo.SERVICE_38 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 39: rdo.SERVICE_39 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 40: rdo.SERVICE_40 = sereServ.Sum(su => su.AMOUNT); break;

                                    case 41: rdo.SERVICE_41 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 42: rdo.SERVICE_42 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 43: rdo.SERVICE_43 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 44: rdo.SERVICE_44 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 45: rdo.SERVICE_45 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 46: rdo.SERVICE_46 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 47: rdo.SERVICE_47 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 48: rdo.SERVICE_48 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 49: rdo.SERVICE_49 = sereServ.Sum(su => su.AMOUNT); break;
                                    case 50: rdo.SERVICE_50 = sereServ.Sum(su => su.AMOUNT); break;

                                    default: rdo.OTHER = sereServ.Sum(su => su.AMOUNT); break;
                                }
                            }
                        }
                        if (treatment.FirstOrDefault().TDL_TREATMENT_ID != null)
                            listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Lỗi xảy ra tại ProcessData: " + ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.ID = castFilter.DEPARTMENT_ID;
                var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);
                if (IsNotNullOrEmpty(department))
                    dicSingleTag.Add("DEPARTMENT_NAME", department.FirstOrDefault().DEPARTMENT_NAME.ToUpper());

                int i = 1;
                foreach (var service in listServices.OrderBy(s => s.ID).ToList())
                {
                    dicSingleTag.Add(i < 10 ? "SERVICE_NAME_0" + i : "SERVICE_NAME_" + i, service.SERVICE_NAME);
                    i++;
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    public class SERVICE
    {
        public long ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
    }
}
