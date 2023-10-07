using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisOtherPaySource;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00725
{
    class Mrs00725Processor : AbstractProcessor
    {
		Mrs00725Filter filter = null;

		List<Mrs00725RDO> listPacsRdo = new List<Mrs00725RDO>();
        List<Mrs00725RDO> listRdo = new List<Mrs00725RDO>();
        List<Mrs00725RDO> listGroupRdo = new List<Mrs00725RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
		List<ManagerSql.SERVICE_REQ> listData = new List<ManagerSql.SERVICE_REQ>();

		List<ManagerSql.BED_LOG> listBedLog = new List<ManagerSql.BED_LOG>();

		List<V_HIS_BED_ROOM> listBedRoom = new List<V_HIS_BED_ROOM>();

		List<V_HIS_BED> listBed = new List<V_HIS_BED>();

        //List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();

        //List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();

        //List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        //List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<HIS_MEDICINE_USE_FORM> listMedicineUseForm = new List<HIS_MEDICINE_USE_FORM>();

        //List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();

        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();

        List<V_HIS_SERVICE> listServices = new List<V_HIS_SERVICE>();

        List<HIS_OTHER_PAY_SOURCE> listOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();
        CommonParam paramGet = new CommonParam();



		public Mrs00725Processor(CommonParam param, string reportTypeCode)
				: base(param, reportTypeCode)
		{
		}


		public override Type FilterType()
		{
			return typeof(Mrs00725Filter);
		}

		protected override bool GetData()
		{
			bool result = true;
			filter = (Mrs00725Filter)base.reportFilter;
			try
			{
                CommonParam paramGet = new CommonParam();
				listData = new ManagerSql().GetServiceReq(filter) ?? new List<ManagerSql.SERVICE_REQ>();
				listBedLog = new ManagerSql().GetBedLog(filter) ?? new List<ManagerSql.BED_LOG>();
                //Danh sach giuong
                HisBedViewFilterQuery HisBedfilter = new HisBedViewFilterQuery();
                //HisBedfilter.IS_ACTIVE = 1;
                listBed = new HisBedManager().GetView(HisBedfilter);

                //Danh sach nguon chi tra
                HisOtherPaySourceFilterQuery HisOtherPaySourcefilter = new HisOtherPaySourceFilterQuery();
                //HisBedfilter.IS_ACTIVE = 1;
                listOtherPaySource = new HisOtherPaySourceManager().Get(HisOtherPaySourcefilter);

                //Danh sach buong
                HisBedRoomViewFilterQuery HisBedRoomfilter = new HisBedRoomViewFilterQuery();
				//HisBedRoomfilter.IS_ACTIVE = 1;
                listBedRoom = new HisBedRoomManager().GetView(HisBedRoomfilter);
                var listServiceCodes = listData.Select(p => p.SERVICE_CODE).Distinct().ToList();
                    listMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery()).Where(p => listServiceCodes.Contains(p.MEDICINE_TYPE_CODE)).ToList();
                    listMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery()).Where(p => listServiceCodes.Contains(p.MATERIAL_TYPE_CODE)).ToList();
                    listServices = new HisServiceManager().GetView(new HisServiceViewFilterQuery()).Where(p => listServiceCodes.Contains(p.SERVICE_CODE)).ToList();

                var listServiceReqIds = listData.Select(p => p.SERVICE_REQ_ID).Distinct().ToList();
                int skip = 0;
                while (listServiceReqIds.Count - skip > 0)
                {
                    var listIds = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                    expMediFilter.TDL_SERVICE_REQ_IDs = listIds;
                    var listSub = new HisExpMestMedicineManager().GetView(expMediFilter);
                    if (listSub != null)
                    {
                        listExpMedicine.AddRange(listSub);
                    }
                }
                //int skip = 0;
                //var medicineIds = listMedicineType.Select(p => p.ID).Distinct().ToList();
                //while (medicineIds.Count - skip > 0)
                //{
                //    var listIds = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    HisExpMestMedicineViewFilterQuery mediFilter = new HisExpMestMedicineViewFilterQuery();
                //    mediFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                //    mediFilter.CREATE_TIME_TO = filter.TIME_TO;
                //    mediFilter.MEDICINE_TYPE_IDs = listIds;
                //    mediFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP;
                //    var mediSub = new HisExpMestMedicineManager().GetView(mediFilter);
                //    if (mediSub != null)
                //    {
                //        listExpMestMedicine.AddRange(mediSub);
                //    }
                //    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //}

                //int mateSkip = 0;
                //var materialIds = listMaterialType.Select(p => p.ID).Distinct().ToList();
                //while (materialIds.Count - mateSkip > 0)
                //{
                //    var listIds = materialIds.Skip(mateSkip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    HisExpMestMaterialViewFilterQuery mateFilter = new HisExpMestMaterialViewFilterQuery();
                //    mateFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                //    mateFilter.CREATE_TIME_TO = filter.TIME_TO;
                //    mateFilter.MATERIAL_TYPE_IDs = listIds;
                //    mateFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP;
                //    var mateSub = new HisExpMestMaterialManager().GetView(mateFilter);
                //    if (mateSub != null)
                //    {
                //        listExpMestMaterial.AddRange(mateSub);
                //    }
                //    mateSkip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //}
               
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
			
			bool result = true;
			try
			{
				if (IsNotNullOrEmpty(listData))
				{
                    var listPacs = listData.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN && p.IS_SENT_EXT == 1).ToList();
                    if (listPacs != null)
                    {
                        ProcessPacs(listPacs);
                    }
                    ProcessListService(listData);
                    ProcessGroupByRequestLoginname(listData);
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

        private void ProcessPacs(List<ManagerSql.SERVICE_REQ> listPacs)
        {
            try
            {
                foreach (var item in listPacs)
                {
                    var bedroom = listBedRoom.FirstOrDefault(p => p.ROOM_ID == item.EXAM_ROOM_ID) ?? new V_HIS_BED_ROOM();
                    //var treatment = listTreatment.FirstOrDefault(p => p.ID == item.TREATMENT_ID) ?? new HIS_TREATMENT();
                    var bed = listBedLog.LastOrDefault(p => p.BED_ROOM_ID == bedroom.ID && p.TREATMENT_ID == item.TREATMENT_ID && p.START_TIME <= item.INTRUCTION_TIME) ?? listBedLog.FirstOrDefault(p => p.BED_ROOM_ID == bedroom.ID && p.TREATMENT_ID == item.TREATMENT_ID) ?? new ManagerSql.BED_LOG();
                    Mrs00725RDO mrs00725RDO = new Mrs00725RDO();
                    mrs00725RDO.PATIENT_CODE = item.TDL_PATIENT_CODE;
                    mrs00725RDO.PATIENT_NAME = item.TDL_PATIENT_NAME;
                    CalculatorAge(mrs00725RDO, item);
                    mrs00725RDO.BED_NAME = string.Join(";", listBed.Where(o => bed.BED_ID == o.ID).Select(p => p.BED_NAME).ToList());
                    mrs00725RDO.BED_ROOM_NAME = bedroom != null ? bedroom.BED_ROOM_NAME : "";
                    mrs00725RDO.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.REQUEST_DEPARTMENT_ID)?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    mrs00725RDO.ACCESSION_NUMBER = ((item.SERE_SERV_ID > 0) ? item.SERE_SERV_ID.ToString() : "");
                    listPacsRdo.Add(mrs00725RDO);
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void ProcessListService(List<ManagerSql.SERVICE_REQ> listService)
        {
            try
            {
                if (listService != null)
                {
                    var listCheck = listService;
                    
                  
                    foreach (var item in listCheck)
                    {
                        //var treatment = listTreatment.FirstOrDefault(p => p.ID == item.TREATMENT_ID);
                        //var sereServ = listSereServ.FirstOrDefault(p => p.ID == item.SERE_SERV_ID);
                        //var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == item.SERVICE_REQ_ID);
                        var medicineType = listMedicineType.FirstOrDefault(p => p.MEDICINE_TYPE_CODE == item.SERVICE_CODE);
                        var expMedi = listExpMedicine.FirstOrDefault(p => p.TDL_SERVICE_REQ_ID == item.SERVICE_REQ_ID);
                        Mrs00725RDO rdo = new Mrs00725RDO();
                        
                        //if (treatment != null)
                        {
                            rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                            rdo.PATIENT_CODE = item.TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                            rdo.PATIENT_DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_PATIENT_DOB??0);
                            rdo.PATIENT_GENDER_ID = item.TDL_PATIENT_GENDER_ID??0;
                            rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IN_TIME);
                            rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.OUT_TIME ?? 0);
                            rdo.OUT_TIME = item.OUT_TIME;
                            rdo.TREATMENT_DAY_COUNT = item.TREATMENT_DAY_COUNT ?? 0;
                            rdo.ICD_CODE = item.ICD_CODE;
                            rdo.ICD_NAME = item.ICD_NAME;
                            rdo.ICD_SUB_CODE = item.ICD_SUB_CODE;
                        }
                        
                        //if (sereServ != null)
                        {
                            rdo.SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(p => p.ID == item.TDL_SERVICE_TYPE_ID).SERVICE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(p => p.ID == item.TDL_SERVICE_TYPE_ID).SERVICE_TYPE_NAME;
                            rdo.PARENT_SERVICE_CODE = string.IsNullOrEmpty(item.PR_CODE) ? "OTHER_PR" : item.PR_CODE;
                            rdo.PARENT_SERVICE_NAME = string.IsNullOrEmpty(item.PR_NAME) ? "NHÓM KHÁC" : item.PR_NAME;
                            
                            if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                            {
                                if (medicineType != null)
                                {
                                    
                                    rdo.TT30BYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE;
                                    rdo.ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                    rdo.SERVICE_CODE = medicineType.MEDICINE_TYPE_CODE;
                                    rdo.SERVICE_NAME = medicineType.MEDICINE_TYPE_NAME;
                                    rdo.SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                                    rdo.MEDICINE_USE_FORM_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                                    rdo.REGISTER_NUMBER = medicineType.REGISTER_NUMBER;
                                    rdo.CONCENTRA = medicineType.CONCENTRA;
                                    //rdo.TUTORIAL = medicineType.TUTORIAL;
                                    //var expMestMedicine = listExpMestMedicine.Where(p => p.TDL_TREATMENT_ID == item.TREATMENT_ID
                                    //                                                  && p.TDL_SERVICE_REQ_ID == item.SERVICE_REQ_ID
                                    //                                                  && p.MEDICINE_TYPE_ID == medicineType.ID).ToList();
                                    //if (expMestMedicine != null)
                                    //{
                                    //    rdo.TOTAL_HP_MEDI_PRICE = expMestMedicine.First().IMP_PRICE * expMestMedicine.Sum(p => p.AMOUNT);
                                    //}
                                    if (item.IS_EXPEND == 1)
                                    {
                                        rdo.TOTAL_HP_MEDI_PRICE = item.VIR_TOTAL_PRICE_NO_EXPEND??0;
                                    }
                                }
                                
                            }
                            else
                            {
                                var service = listServices.FirstOrDefault(p => p.SERVICE_CODE == item.SERVICE_CODE);
                                rdo.TT30BYT_CODE = item.TDL_HEIN_SERVICE_BHYT_CODE;
                                rdo.SERVICE_CODE = item.SERVICE_CODE;
                                rdo.SERVICE_NAME = item.SERVICE_NAME;
                                if (service != null) 
                                    rdo.SERVICE_UNIT_NAME = service.SERVICE_UNIT_NAME;
                                
                            }
                            rdo.AMOUNT = item.AMOUNT;
                            rdo.PRICE = item.VIR_PRICE ?? 0;
                            if(filter.PATIENT_TYPE_IDs != null)
                            {
                                if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.PRICE_BY_PT = item.HEIN_PRICE ?? 0;
                                }
                                else if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE)
                                {
                                    rdo.PRICE_BY_PT = 0;
                                }
                                else
                                {
                                    rdo.PRICE_BY_PT = item.PRICE;
                                }
                            }
                            else
                            {
                                rdo.PRICE_BY_PT = item.PRICE;
                            }
                            rdo.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
                            var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID ==item.PATIENT_TYPE_ID);
                            if (patientType != null)
                            {
                                rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                                rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                            }
                            rdo.OTHER_PAY_SOURCE_ID = item.OTHER_PAY_SOURCE_ID;
                            rdo.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
                            var otherPaySource = listOtherPaySource.FirstOrDefault(o => o.ID ==item.OTHER_PAY_SOURCE_ID);
                            if (otherPaySource != null)
                            {
                                rdo.OTHER_PAY_SOURCE_CODE = otherPaySource.OTHER_PAY_SOURCE_CODE;
                                rdo.OTHER_PAY_SOURCE_NAME = otherPaySource.OTHER_PAY_SOURCE_NAME;
                            }
                            rdo.VAT = item.VAT_RATIO??0;
                            rdo.TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE_BY_PT * (1 + rdo.VAT);
                            rdo.TOTAL_HEIN_PRICE = item.VIR_TOTAL_HEIN_PRICE ?? 0;
                            rdo.TOTAL_PATIENT_PRICE_BHYT = item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                            rdo.TOTAL_PATIENT_PRICE = item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                            rdo.IS_EXPEND = item.IS_EXPEND ?? 0;
                            rdo.TOTAL_PRICE_NO_EXPEND = item.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                            rdo.EXPEND_TYPE_ID = item.EXPEND_TYPE_ID ?? 0;
                            rdo.DISCOUNT = item.DISCOUNT ?? 0;
                        }
                        
                        //if (serviceReq != null)
                        {
                            rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.INTRUCTION_TIME);
                            rdo.SERVICE_REQ_CODE = item.SERVICE_REQ_CODE;
                            rdo.REQUEST_LOGINNAME = item.REQUEST_LOGINNAME;
                            rdo.REQUEST_USERNAME = item.REQUEST_USERNAME;
                            rdo.EXECUTE_LOGINNAME = item.EXECUTE_LOGINNAME;
                            rdo.EXECUTE_USERNAME = item.EXECUTE_USERNAME;
                            rdo.EXECUTE_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                            rdo.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                            rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            var executeRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID ==item.EXECUTE_ROOM_ID);
                            if (executeRoom != null)
                            {
                                rdo.EXECUTE_ROOM_CODE = executeRoom.ROOM_CODE;
                                rdo.EXECUTE_ROOM_NAME = executeRoom.ROOM_NAME;
                            }
                            var requestRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID ==item.REQUEST_ROOM_ID);
                            if (requestRoom != null)
                            {
                                rdo.REQUEST_ROOM_CODE = requestRoom.ROOM_CODE;
                                rdo.EXAM_ROOM_NAME = requestRoom.ROOM_NAME;
                            }
                            var examRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID ==item.EXAM_ROOM_ID);
                            if (examRoom != null)
                            {
                                rdo.EXAM_ROOM_CODE = examRoom.ROOM_CODE;
                                rdo.EXAM_ROOM_NAME = examRoom.ROOM_NAME;
                            }
                        }
                        if (expMedi != null)
                        {
                            rdo.TUTORIAL = expMedi.TUTORIAL;
                        }
                        
                        listRdo.Add(rdo);
                    }
                }
                
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void ProcessGroupByRequestLoginname(List<ManagerSql.SERVICE_REQ> listService)
        {
            try
            {
                if (listService != null)
                {
                    var listCheck = listService.GroupBy(p => p.REQUEST_LOGINNAME).ToList();

                    foreach (var item in listCheck)
                    {
                        List<ManagerSql.SERVICE_REQ> listSub = item.ToList<ManagerSql.SERVICE_REQ>();

                        Mrs00725RDO rdo = new Mrs00725RDO();
                        rdo.REQUEST_LOGINNAME = listSub[0].REQUEST_LOGINNAME;
                        rdo.REQUEST_USERNAME = listSub[0].REQUEST_USERNAME;
                        rdo.EXAM_AMOUNT = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                        rdo.XN_AMOUNT = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                        rdo.CDHA_AMOUNT = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                        rdo.ALL_SERVICE_AMOUNT = listSub.Select(p => p.SERVICE_REQ_ID).Count();
                        rdo.TOTAL_PRICE = listSub.Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.TOTAL_PATIENT = listSub.Select(p => p.TDL_PATIENT_CODE).Distinct().Count();
                        rdo.AVERAGE_XN = rdo.XN_AMOUNT / rdo.TOTAL_PATIENT;
                        rdo.AVERAGE_CDHA = rdo.CDHA_AMOUNT / rdo.TOTAL_PATIENT;
                        listGroupRdo.Add(rdo);
                    }
                }

            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void CalculatorAge(Mrs00725RDO rdo, MRS.Processor.Mrs00725.ManagerSql.SERVICE_REQ treatment)
		{
			try
			{
                int? num = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB??0);
				if (num >= 0)
				{
                    if (treatment.TDL_PATIENT_GENDER_ID == 2)
					{
						rdo.MALE_AGE = ((num >= 1) ? num : new int?(1));
					}
					else
					{
						rdo.FEMALE_AGE = ((num >= 1) ? num : new int?(1));
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
		{
			dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
			dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("TOTAL_XN_AMOUNT", listRdo.Where(p => p.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Select(P => P.SERVICE_REQ_CODE).Distinct().Count());
            dicSingleTag.Add("TOTAL_CDHA_AMOUNT", listRdo.Where(p => p.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Select(P => P.SERVICE_REQ_CODE).Distinct().Count());
            objectTag.AddObjectData<Mrs00725RDO>(store, "Report", listPacsRdo);
            objectTag.AddObjectData(store, "Detail", listRdo.Where(p => !string.IsNullOrEmpty(p.SERVICE_CODE) && p.OUT_TIME != null).ToList());
            objectTag.AddObjectData(store, "DetailAll", listRdo.Where(p => !string.IsNullOrEmpty(p.SERVICE_CODE)).OrderBy(p => p.EXECUTE_DEPARTMENT_NAME).ThenBy(P=> P.SERVICE_TYPE_NAME).ThenBy(P => P.PARENT_SERVICE_NAME).ThenBy(P => P.SERVICE_NAME).ToList());
            objectTag.AddObjectData(store, "MediService", listRdo.Where(p => !string.IsNullOrEmpty(p.SERVICE_CODE) && p.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).ToList());
            objectTag.AddObjectData(store, "ReqUser", listGroupRdo.OrderBy(p => p.REQUEST_LOGINNAME).ToList());
		}

	}
}
