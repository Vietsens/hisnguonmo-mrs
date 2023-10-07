using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentBedRoom;

namespace MRS.Processor.Mrs00513
{
    class Mrs00513Processor : AbstractProcessor
    {
        Mrs00513Filter castFilter = null;

        public Mrs00513Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        List<Mrs00513RDO> listRdo = new List<Mrs00513RDO>();

        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<V_HIS_TREATMENT_BED_ROOM> listTreatmentBedRoom = new List<V_HIS_TREATMENT_BED_ROOM>();

        List<long> listServiceQtIds = new List<long>();

        string thisReportTypeCode = "";

        public override Type FilterType()
        {
            return typeof(Mrs00513Filter);
        }

        protected override bool GetData()
        {
            bool valid = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00513Filter)this.reportFilter;
                // getData
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = this.thisReportTypeCode;
                var listServiceRetyCats = new HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>();
                //Inventec.Common.Logging.LogSystem.Info("listServiceRetyCats" + listServiceRetyCats.Count);
                listServiceQtIds = listServiceRetyCats.Where(w => w.CATEGORY_CODE.Equals("513QT")).Select(s => s.SERVICE_ID).ToList();

                var skip = 0;
                while (listServiceQtIds.Count - skip > 0)
                {
                    var listIDs = listServiceQtIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                    sereServFilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                    sereServFilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                    if (IsNotNull(castFilter.REQ_DEPARMTENT_ID))
                        sereServFilter.TDL_REQUEST_DEPARTMENT_ID = castFilter.REQ_DEPARMTENT_ID;
                    if (IsNotNull(castFilter.EXE_DEPARMTENT_ID))
                        sereServFilter.TDL_EXECUTE_DEPARTMENT_ID = castFilter.EXE_DEPARMTENT_ID;
                    sereServFilter.SERVICE_IDs = listIDs;
                    listSereServs = new HisSereServManager(param).Get(sereServFilter) ?? new List<HIS_SERE_SERV>();
                }
                //Inventec.Common.Logging.LogSystem.Info("listSereServs" + listSereServs.Count);
                skip = 0;
                List<long> listTreatmentIds = listSereServs.Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList();
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentBedRoomViewFilterQuery treatmentBedRoomFilter = new HisTreatmentBedRoomViewFilterQuery();
                    treatmentBedRoomFilter.TREATMENT_IDs = listIDs;
                    listTreatmentBedRoom.AddRange(new HisTreatmentBedRoomManager(param).GetView(treatmentBedRoomFilter) ?? new List<V_HIS_TREATMENT_BED_ROOM>());
                }
                //Inventec.Common.Logging.LogSystem.Info("listTreatments" + listTreatments.Count);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {
                listRdo.Clear();

                //process
                foreach (var sereServ in listSereServs)
                {
                    var rdo = new Mrs00513RDO();
                    rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID.Value;
                    rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;
                    var treatmentBedRoom = listTreatmentBedRoom.FirstOrDefault(o => o.TREATMENT_ID == sereServ.TDL_TREATMENT_ID && o.ADD_TIME < sereServ.TDL_INTRUCTION_TIME) ?? new V_HIS_TREATMENT_BED_ROOM();
                    rdo.PATIENT_NAME = treatmentBedRoom.TDL_PATIENT_NAME;
                    rdo.DOB = treatmentBedRoom.TDL_PATIENT_DOB;
                    rdo.SERVICE_ID = sereServ.SERVICE_ID;
                    rdo.SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
                    rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;

                    rdo.ROOM_NAME = treatmentBedRoom.BED_ROOM_NAME;
                    rdo.BED_NAME = treatmentBedRoom.BED_NAME;
                    rdo.AMOUNT = sereServ.AMOUNT;
                    listRdo.Add(rdo);
                }
                //Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                listRdo = listRdo.GroupBy(g => new { g.TREATMENT_ID, g.SERVICE_ID }).Select(s => new Mrs00513RDO()
                {
                    TREATMENT_ID = s.First().TREATMENT_ID,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,
                    ROOM_NAME = s.First().ROOM_NAME,
                    BED_NAME = s.First().BED_NAME,

                    PATIENT_NAME = s.First().PATIENT_NAME,
                    DOB = s.First().DOB,

                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,

                    AMOUNT = s.Sum(su => su.AMOUNT)

                }).ToList();
                //Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                //Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (IsNotNull(castFilter.REQ_DEPARMTENT_ID))
                {
                    var listDepartments = new HisDepartmentManager(param).Get(new HisDepartmentFilterQuery() { ID = castFilter.REQ_DEPARMTENT_ID }) ?? new List<HIS_DEPARTMENT>();
                    if (IsNotNullOrEmpty(listDepartments))
                        dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", listDepartments.First().DEPARTMENT_NAME);
                }
                //Inventec.Common.Logging.LogSystem.Info("1ListRdo" + ListRdo.Count);
                if (IsNotNull(castFilter.EXE_DEPARMTENT_ID))
                {
                    var listDepartments = new HisDepartmentManager(param).Get(new HisDepartmentFilterQuery() { ID = castFilter.EXE_DEPARMTENT_ID }) ?? new List<HIS_DEPARTMENT>();
                    if (IsNotNullOrEmpty(listDepartments))
                        dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", listDepartments.First().DEPARTMENT_NAME);
                }
                objectTag.AddObjectData(store, "Rdo", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

}
