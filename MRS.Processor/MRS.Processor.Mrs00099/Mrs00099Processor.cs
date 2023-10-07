using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServExt;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00099
{
    public class Mrs00099Processor : AbstractProcessor
    {
        Mrs00099Filter castFilter = null;
        List<Mrs00099RDO> ListRdo = new List<Mrs00099RDO>();

        CommonParam paramGet = new CommonParam();
        string Treatment_Type_Names;

        public Mrs00099Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00099Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00099Filter)this.reportFilter);
                ListRdo = new ManagerSql().GetSereServ(castFilter);

                result = true;
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
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    foreach (var item in ListRdo)
                    {
                        item.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                        item.EXEXUTE_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                        item.INTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.INTRUCTION_TIME);
                        if (item.TDL_PATIENT_DOB > 0)
                        {
                            item.DOB_YEAR = (item.TDL_PATIENT_DOB ?? 0).ToString().Substring(0, 4);
                        }


                        var filmSize = MANAGER.Config.HisFilmSizeCFG.FILM_SIZEs.FirstOrDefault(o => o.ID == item.FILM_SIZE_ID);
                        if (filmSize != null)
                        {
                            item.FILM_SIZE_NAME = filmSize.FILM_SIZE_NAME;
                        }
                    }

                }

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetTreatmentType()
        {
            try
            {
                HisTreatmentTypeFilterQuery filter = new HisTreatmentTypeFilterQuery();
                if (castFilter.TREATMENT_TYPE_ID.HasValue)
                {
                    filter.ID = castFilter.TREATMENT_TYPE_ID.Value;
                }
                else if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                {
                    filter.IDs = castFilter.TREATMENT_TYPE_IDs;
                }
                var treatmentTypes = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager().Get(filter);
                if (IsNotNullOrEmpty(treatmentTypes))
                {
                    foreach (var treatmentType in treatmentTypes)
                    {
                        if (String.IsNullOrEmpty(Treatment_Type_Names))
                        {
                            Treatment_Type_Names = treatmentType.TREATMENT_TYPE_NAME.ToUpper();
                        }
                        else
                        {
                            Treatment_Type_Names = Treatment_Type_Names + " - " + treatmentType.TREATMENT_TYPE_NAME.ToUpper();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? castFilter.FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? castFilter.FINISH_TIME_TO ?? 0));
            GetTreatmentType();
            dicSingleTag.Add("TREATMENT_TYPE_NAMEs", Treatment_Type_Names);

            ListRdo = ListRdo.OrderBy(o => o.INTRUCTION_TIME).ThenBy(t => t.PATIENT_ID).ToList();
            objectTag.AddObjectData(store, "Treatments", ListRdo.GroupBy(o=>o.TREATMENT_CODE).Select(p=>p.First()).ToList());
            objectTag.AddObjectData(store, "Services", ListRdo.GroupBy(o => o.SERVICE_CODE).Select(p => p.First()).OrderBy(q => q.TDL_SERVICE_TYPE_ID).ToList());
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
