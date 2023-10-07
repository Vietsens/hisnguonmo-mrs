using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
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
using HIS.Treatment.DateTime;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisPatientTypeAlter;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisSereServFile;
using MOS.MANAGER.HisBedLog;


namespace MRS.Processor.Mrs00547
{

    class Mrs00547Processor : AbstractProcessor
    {
        Mrs00547Filter castFilter = null;

        List<Mrs00547RDO> listRdo = new List<Mrs00547RDO>();

        public string TITLE_REPORT;

        public Mrs00547Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00547Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00547Filter)this.reportFilter;





                var listTemp = new ManagerSql().GetAll(this.castFilter);
                if (listTemp != null)
                {
                    listRdo.AddRange(listTemp);
                }
                this.TITLE_REPORT = string.Join(" - ", listRdo.GroupBy(p => p.CATEGORY_CODE).Select(o => o.First().CATEGORY_NAME).ToList());
                if (this.castFilter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.TITLE_REPORT += " BHYT";
                }

                if (this.castFilter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    this.TITLE_REPORT += " VIỆN PHÍ";
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
                CommonParam paramGet = new CommonParam();

                if (IsNotNullOrEmpty(listRdo))
                {
                    foreach (var rdo in listRdo)
                    {
                        //rdo.HIS_SERVICE_REQ = listHisServiceReq.FirstOrDefault(s => s.ID == sereServ.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        //rdo.HIS_SERE_SERV_EXT = listHisSereServExt.FirstOrDefault(s => s.SERE_SERV_ID == sereServ.ID) ?? new HIS_SERE_SERV_EXT();
                        rdo.PATIENT_ID = rdo.TDL_PATIENT_ID ?? 0;
                        rdo.PATIENT_CODE = rdo.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = rdo.TDL_PATIENT_NAME;
                        rdo.INTRUCTION_DATE = rdo.INTRUCTION_DATE;

                        rdo.TREATMENT_ID = rdo.TDL_TREATMENT_ID ?? 0;//
                        rdo.TREATMENT_CODE = rdo.TDL_TREATMENT_CODE;

                        rdo.GENDER = rdo.TDL_PATIENT_GENDER_NAME;
                        rdo.DOB = rdo.TDL_PATIENT_DOB;

                        if (rdo.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.DOB_YEAR_MALE = rdo.DOB.ToString().Substring(0, 4);
                        }
                        else if (rdo.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.DOB_YEAR_FEMALE = rdo.DOB.ToString().Substring(0, 4);
                        }
                        rdo.ADDRESS = rdo.TDL_PATIENT_ADDRESS;

                        rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

                        rdo.REQUEST_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                        rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                        rdo.SERVICE_CODE = rdo.TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = rdo.TDL_SERVICE_NAME;
                        rdo.GROUP_HEIN_CARD = this.HeinCardTypeName(rdo.HEIN_CARD_NUMBER);
                        

                        if (rdo.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.TREATMENT_TYPE_NAME_IN = "Nội trú";
                        }
                        else
                        {
                            rdo.TREATMENT_TYPE_NAME_OUT = "Ngoại trú";
                        }
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

        private string HeinCardTypeName(string HEIN_CARD_NUMBER)
        {
            string result = "";
            try
            {
                if (HEIN_CARD_NUMBER != null && HEIN_CARD_NUMBER != "")
                {
                    if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01 != null
                        && MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01.Exists(o => HEIN_CARD_NUMBER.StartsWith(o)))
                    {
                        result = this.castFilter.TYPE_01;
                    }
                    else if (MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02 != null
                        && MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__02.Exists(o => HEIN_CARD_NUMBER.StartsWith(o)))
                    {
                        result = this.castFilter.TYPE_02;
                    }
                    else
                        result = this.castFilter.TYPE_OTHER;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", castFilter.INTRUCTION_TIME_FROM ?? castFilter.FINISH_TIME_FROM ?? castFilter.START_TIME_FROM ?? 0);

            dicSingleTag.Add("TIME_TO", castFilter.INTRUCTION_TIME_TO ?? castFilter.FINISH_TIME_TO ?? castFilter.START_TIME_TO ?? 0);

            dicSingleTag.Add("TITLE_REPORT", this.TITLE_REPORT);

            objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.INTRUCTION_TIME).ToList());

            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(s => s.INTRUCTION_TIME).GroupBy(p => new { p.TREATMENT_ID,p.SERVICE_ID,p.PATIENT_TYPE_ID,p.TDL_REQUEST_ROOM_ID,p.BED_CODE}).Select(q=>q.First()).ToList());
            objectTag.AddRelationship(store, "Report", "Rdo", new string[] { "TREATMENT_ID", "SERVICE_ID", "PATIENT_TYPE_ID", "TDL_REQUEST_ROOM_ID", "BED_CODE" }, new string[] { "TREATMENT_ID", "SERVICE_ID", "PATIENT_TYPE_ID", "TDL_REQUEST_ROOM_ID", "BED_CODE" });


            objectTag.AddObjectData(store, "TreatmentCates", listRdo.GroupBy(p => new { p.TREATMENT_ID, p.CATEGORY_CODE}).Select(o=>o.First()).OrderBy(s => s.TREATMENT_CODE).ToList());

            DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.INTRUCTION_TIME_FROM ?? castFilter.FINISH_TIME_FROM ?? castFilter.START_TIME_FROM ?? 0);
            DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.INTRUCTION_TIME_TO ?? castFilter.FINISH_TIME_TO ?? castFilter.START_TIME_TO ?? 0);
            DateTime IndexTime = StartTime.Date;
            int val = 1;
            while (IndexTime < FinishTime)
            {
                dicSingleTag.Add(string.Format("DAY_{0}",val), Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(IndexTime));
                val += 1;
                IndexTime = IndexTime.AddDays(1);
               
            }
        }
    }
}
