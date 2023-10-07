using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisBedType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentBedRoom;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatmentResult;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00814
{
    public class Mrs00814Processor: AbstractProcessor
    {
        public Mrs00814Filter filter;
        public List<Mrs00814RDO> listRdo = new List<Mrs00814RDO>();
        public CommonParam paramCommon = new CommonParam();
        public List<HIS_BED> listBed = new List<HIS_BED>();
        public List<HIS_BED_TYPE> listBedType = new List<HIS_BED_TYPE>();
        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<HIS_TREATMENT_BED_ROOM> listTreatmentBedRoom = new List<HIS_TREATMENT_BED_ROOM>();
        public List<HIS_TREATMENT_END_TYPE> listTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        public List<HIS_TREATMENT_RESULT> listTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        public List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();

        public List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
        public List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        public Mrs00814Processor(CommonParam param, string reportTypeCode):base(param,reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00814Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00814Filter)this.reportFilter;
            bool result = false;
            try
            {
                listTreatmentEndType = new HisTreatmentEndTypeManager().Get(new HisTreatmentEndTypeFilterQuery());
                listTreatmentResult = new HisTreatmentResultManager().Get(new HisTreatmentResultFilterQuery());
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
                //HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                //treatmentFilter.IN_TIME_FROM = filter.TIME_FROM;
                //treatmentFilter.IN_TIME_TO = filter.TIME_TO;
                //listTreatment = new HisTreatmentManager().Get(treatmentFilter);
                //var treatmentIds = listTreatment.Select(x => x.ID).ToList();
                //var skip2 = 0;
                //while (treatmentIds.Count-skip2>0)
                //{
                //    var limit = treatmentIds.Skip(skip2).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    skip2 = skip2 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //    HisTreatmentBedRoomFilterQuery treatmentBedRoomFilter = new HisTreatmentBedRoomFilterQuery();
                //    treatmentBedRoomFilter.TREATMENT_IDs = limit;
                //    treatmentBedRoomFilter.BED_IDs = listBed.Select(x => x.ID).ToList();
                //    var treatmentBedRooms = new HisTreatmentBedRoomManager().Get(treatmentBedRoomFilter);
                //    listTreatmentBedRoom.AddRange(treatmentBedRooms);
                //}

                HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery();
                serviceReqFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM;
                serviceReqFilter.INTRUCTION_TIME_TO = filter.TIME_TO;
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G;
                listServiceReq = new HisServiceReqManager().GetView(serviceReqFilter);
                var serviceReqIds = listServiceReq.Select(x => x.ID).ToList();
                
                var skip = 0;
                while (serviceReqIds.Count - skip > 0)
                {
                    var limit = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                    sereServFilter.SERVICE_REQ_IDs = limit;
                    var sereServ = new HisSereServManager().Get(sereServFilter);
                    listSereServ.AddRange(sereServ);
                    var treatmentIds = listSereServ.Select(x => x.TDL_TREATMENT_ID??0).ToList();
                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.IDs = treatmentIds;
                    var treatmemts =new HisTreatmentManager().Get(treatmentFilter);
                    listTreatment.AddRange(treatmemts);
                }
                listSereServ = listSereServ.Where(x => x.TDL_SERVICE_NAME.Contains("Giường Hồi sức cấp cứu Hạng II - Khoa Lao")).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var item in listSereServ)
                    {
                        Mrs00814RDO rdo = new Mrs00814RDO();
                        rdo.TREATMENT_ID = item.TDL_TREATMENT_ID??0;
                        var treatment = listTreatment.Where(x =>x.ID == item.TDL_TREATMENT_ID).FirstOrDefault();
                        if (treatment!=null)
                        {
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.ICD_CODE = treatment.ICD_CODE;
                            rdo.ICD_NAME = treatment.ICD_NAME;
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.IN_TIME);
                            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(treatment.OUT_TIME??0);
                            rdo.TREATMENT_END_TYPE_ID = treatment.TREATMENT_END_TYPE_ID??0;
                            var treatmentEndType = listTreatmentEndType.Where(x => x.ID == treatment.TREATMENT_END_TYPE_ID).FirstOrDefault();
                            if (treatmentEndType!=null)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = treatmentEndType.TREATMENT_END_TYPE_CODE;
                                rdo.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                            }
                            var treatmentRessult = listTreatmentResult.Where(x => x.ID == treatment.TREATMENT_RESULT_ID).FirstOrDefault();

                            rdo.TREATMENT_RESULT_ID = treatment.TREATMENT_RESULT_ID??0;
                            if (treatmentRessult != null)
                            {
                                rdo.TREATMENT_RESULT_CODE = treatmentRessult.TREATMENT_RESULT_CODE;
                                rdo.TREATMENT_RESULT_NAME = treatmentRessult.TREATMENT_RESULT_NAME;
                            }
                            rdo.PATIENT_ID = treatment.PATIENT_ID;
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.PATIENT_GENDER = treatment.TDL_PATIENT_GENDER_NAME;
                            rdo.PATIENT_DOB = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.TDL_PATIENT_DOB);
                            rdo.AGE = Inventec.Common.DateTime.Calculation.Age(treatment.TDL_PATIENT_DOB);
                        }
                        var department = listDepartment.Where(x => x.ID == item.TDL_REQUEST_DEPARTMENT_ID).FirstOrDefault();
                        if (department!=null)
                        {
                            rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        }
                        rdo.DAY_USE_BED = item.AMOUNT;
                        listRdo.Add(rdo);
                    }
                    var group = listRdo.GroupBy(x => new { x.TREATMENT_ID, x.DEPARTMENT_CODE }).ToList();
                    listRdo.Clear();
                    foreach (var item in group)
                    {
                        Mrs00814RDO rdo = new Mrs00814RDO();
                        rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                        rdo.PATIENT_ID = item.First().PATIENT_ID;
                        rdo.PATIENT_CODE = item.First().PATIENT_CODE;
                        rdo.PATIENT_NAME = item.First().PATIENT_NAME;
                        rdo.PATIENT_GENDER = item.First().PATIENT_GENDER;
                        rdo.PATIENT_DOB = item.First().PATIENT_DOB;
                        rdo.AGE = item.First().AGE;
                        rdo.TREATMENT_CODE = item.First().TREATMENT_CODE;
                        rdo.TREATMENT_ID = item.First().TREATMENT_ID;
                        rdo.TREATMENT_NAME = item.First().TREATMENT_NAME;
                        rdo.IN_TIME = item.First().IN_TIME;
                        rdo.OUT_TIME = item.First().OUT_TIME;
                        rdo.ICD_CODE = item.First().ICD_CODE;
                        rdo.ICD_NAME = item.First().ICD_NAME;
                        rdo.TREATMENT_END_TYPE_ID = item.First().TREATMENT_END_TYPE_ID;
                        rdo.TREATMENT_END_TYPE_CODE = item.First().TREATMENT_END_TYPE_CODE;
                        rdo.TREATMENT_END_TYPE_NAME = item.First().TREATMENT_END_TYPE_NAME;
                        rdo.TREATMENT_RESULT_ID = item.First().TREATMENT_RESULT_ID;
                        rdo.TREATMENT_RESULT_CODE = item.First().TREATMENT_RESULT_CODE;
                        rdo.TREATMENT_RESULT_NAME = item.First().TREATMENT_RESULT_NAME;
                        rdo.DAY_USE_BED = item.Sum(x => x.DAY_USE_BED);
                        listRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", listRdo.Distinct().ToList());
            objectTag.AddObjectData(store, "Department", listRdo.GroupBy(x => x.DEPARTMENT_NAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Department", "Report", "DEPARTMENT_NAME", "DEPARTMENT_NAME");
        }
    }
}
