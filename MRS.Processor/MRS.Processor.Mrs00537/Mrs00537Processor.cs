using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00537;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisCashierRoom;
using AutoMapper;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00537
{
    public class Mrs00537Processor : AbstractProcessor
    {
        private List<Mrs00537RDO> ListRdoDetail = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoParent = new List<Mrs00537RDO>();

        private List<Mrs00537RDO> ListRdoType01 = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoType02 = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoTypeOt = new List<Mrs00537RDO>();

        private List<Mrs00537RDO> ListRdoType01_A  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoType01_B1  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoType01_B2  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoType02_A  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoType02_B1  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoType02_B2  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoTypeOt_A  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoTypeOt_B  = new List<Mrs00537RDO>();
        private List<Mrs00537RDO> ListRdoTypeOt_C  = new List<Mrs00537RDO>();

        private Mrs00537Filter filter;
        private List<HIS_HEIN_APPROVAL> listHisHeinApproval = new List<HIS_HEIN_APPROVAL>();
        private List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        private List<HIS_TREATMENT_TYPE> listHisTreatmentType = new List<HIS_TREATMENT_TYPE>();
        private List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        HIS_DEPARTMENT department = new HIS_DEPARTMENT();
        string thisReportTypeCode = "";

        private List<long> HeinServiceTypeId_Service = null;
        public Mrs00537Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00537Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            var result = true;
            filter = (Mrs00537Filter)this.reportFilter;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00537: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));

            try
            {
                HeinServiceTypeId_Service = new List<long>()        
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,// 1;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC,// 2;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,// 4;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,// 3;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH,// 5;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT,// 7;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,// 8;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN,// 18;
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT
                };

                if (filter.BLOOD_IS_SV)
                {
                    HeinServiceTypeId_Service.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                }
                if (filter.TRAN_IS_SV)
                {
                    HeinServiceTypeId_Service.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                }
                if ((filter.DEPARTMENT_ID??0)!=0)
                {
                    department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                }
                //Danh sách HSDT

                HisHeinApprovalFilterQuery HisHeinApprovalfilter = new HisHeinApprovalFilterQuery();
                HisHeinApprovalfilter = this.MapFilter<Mrs00537Filter, HisHeinApprovalFilterQuery>(filter, HisHeinApprovalfilter);
                listHisHeinApproval = new HisHeinApprovalManager().Get(HisHeinApprovalfilter);
                if (filter.BRANCH_ID != null)
                {
                    var HisCashierRoom = new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery() { BRANCH_ID = filter.BRANCH_ID });
                    listHisHeinApproval = listHisHeinApproval.Where(o => HisCashierRoom.Exists(p => p.ID == o.CASHIER_ROOM_ID)).ToList();
                }
                Inventec.Common.Logging.LogSystem.Info("listHisHeinApproval" + listHisHeinApproval.Count);

                List<long> treatmentIds = listHisHeinApproval.Select(o => o.TREATMENT_ID).Distinct().ToList();

                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;

                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                        HisTreatmentfilter.IDs = limit;
                        HisTreatmentfilter.ORDER_FIELD = "ID";
                        HisTreatmentfilter.ORDER_DIRECTION = "ASC";
                        var listHisTreatmentSub = new HisTreatmentManager(param).Get(HisTreatmentfilter);
                        if (listHisTreatmentSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisTreatmentSub Get null");
                        else
                            listHisTreatment.AddRange(listHisTreatmentSub);
                    }
                    if ((filter.DEPARTMENT_ID ?? 0) != 0)
                    {
                        listHisTreatment = listHisTreatment.Where(o => o.END_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                        listHisHeinApproval = listHisHeinApproval.Where(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID)).ToList();
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listHisTreatment" + listHisTreatment.Count);
                }

                List<long> heinApprovalIds = listHisHeinApproval.Select(o => o.ID).Distinct().ToList();

                if (heinApprovalIds != null && heinApprovalIds.Count > 0)
                {
                    var skip = 0;

                    while (heinApprovalIds.Count - skip > 0)
                    {
                        var limit = heinApprovalIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.HEIN_APPROVAL_IDs = limit;
                        HisSereServfilter.ORDER_FIELD = "ID";
                        HisSereServfilter.ORDER_DIRECTION = "ASC";
                        HisSereServfilter.HAS_EXECUTE = true;
                        HisSereServfilter.IS_EXPEND = false;
                        var listHisSereServSub = new HisSereServManager(param).Get(HisSereServfilter);
                        if (listHisSereServSub != null)
                        {
                            listHisSereServSub = listHisSereServSub.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listHisSereServSub);
                    }
                    listHisSereServ = listHisSereServ.Where(o =>HeinServiceTypeId_Service.Contains(o.TDL_HEIN_SERVICE_TYPE_ID ?? 0)
                        && o.AMOUNT > 0 && o.PRICE > 0).ToList();
                    
                    Inventec.Common.Logging.LogSystem.Info("listHisSereServ" + listHisSereServ.Count);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
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

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {

                ListRdoDetail = (from r in listHisSereServ select new Mrs00537RDO(NumDigits,r, listHisHeinApproval, filter)).ToList();

                ListRdoDetail = ListRdoDetail.Where(o => o.HEIN_SERVICE_TYPE_ID != null).OrderBy(p => p.ROUTE_CODE).ThenBy(p => p.IS_RIGHT_ROUTE).ThenBy(p => p.HEIN_SERVICE_TYPE_NUM_ORDER).ThenBy(q => q.HEIN_SERVICE_BHYT_NAME).ToList();
                
                ListRdoParent = ListRdoDetail.GroupBy(o => o.HEIN_SERVICE_TYPE_ID).Select(p => p.First()).ToList();
                List<string> keySums = new List<string>() { "SUM" };
                
                ListRdoType01 = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 1).GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);
                
                ListRdoType02 = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 2).GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);
                
                ListRdoTypeOt = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 3).GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoType01_A = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 1 && p.ROUTE_CODE == "A").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoType01_B1 = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 1 && p.ROUTE_CODE == "B" && p.IS_RIGHT_ROUTE == "DT").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoType01_B2 = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 1 && p.ROUTE_CODE == "B" && p.IS_RIGHT_ROUTE == "TT").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoType02_A = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 2 && p.ROUTE_CODE == "A").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoType02_B1 = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 2 && p.ROUTE_CODE == "B" && p.IS_RIGHT_ROUTE == "DT").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoType02_B2 = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 2 && p.ROUTE_CODE == "B" && p.IS_RIGHT_ROUTE == "TT").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoTypeOt_A = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 3 && p.ROUTE_CODE == "A").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoTypeOt_B = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 3 && p.ROUTE_CODE == "B").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);

                ListRdoTypeOt_C = GroupSum(ListRdoDetail.Where(p => p.HEIN_CARD_NUMBER_TYPE_ID == 3 && p.ROUTE_CODE == "C").GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList().ToDictionary(p => p, p => new Mrs00537RDO()), keySums);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00537RDO>();
                result = false;
            }
            return result;
        }

        private List<R> GroupSum<T, R>(Dictionary<IGrouping<T, R>, R> p, List<string> keySums)
        {
            List<R> result = new List<R>();
            string errorField = "";
            try
            {
                var group = p.Keys;
                Decimal sum = 0;

                List<R> listSub = new List<R>();
                PropertyInfo[] pi = typeof(R).GetProperties();
                foreach (var item in group)
                {
                    listSub = item.ToList<R>();
                    R Rdo = p[item];
                    bool hide = true;
                    foreach (var field in pi)
                    {

                        errorField = field.Name;

                        if (keySums != null && keySums.Exists(o => field.Name.Contains(o)))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(Rdo, sum);
                        }
                        else if (FirstValue(listSub, field) != null)
                        {

                            field.SetValue(Rdo, field.GetValue(FirstValue(listSub, field)));
                        }

                    }
                    if (!hide) result.Add(Rdo);

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<R>();
            }
            return result;
        }

        private R FirstValue<R>(List<R> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            AddGeneral(dicSingleTag, objectTag, store);
            AddType01(dicSingleTag, objectTag, store);
            AddType02(dicSingleTag, objectTag, store);
            AddTypeOt(dicSingleTag, objectTag, store);
        }

        private void AddGeneral(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.EXECUTE_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.EXECUTE_TIME_TO ?? 0));
            
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);

            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            objectTag.AddObjectData(store, "ReportParentA", ListRdoParent);
            objectTag.AddObjectData(store, "ReportParentB", ListRdoParent);
            objectTag.AddObjectData(store, "ReportParentC", ListRdoParent);
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;
            
        }

        private void AddType01(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "ReportType01", ListRdoType01);
            objectTag.AddObjectData(store, "ReportType01_A", ListRdoType01_A);
            objectTag.AddObjectData(store, "ReportType01_B1", ListRdoType01_B1);
            objectTag.AddObjectData(store, "ReportType01_B2", ListRdoType01_B2);
            objectTag.AddRelationship(store, "ReportParentA", "ReportType01", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentA", "ReportType01_A", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentB", "ReportType01_B1", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentC", "ReportType01_B2", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
        }

        private void AddType02(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "ReportType02", ListRdoType02);
            objectTag.AddObjectData(store, "ReportType02_A", ListRdoType02_A);
            objectTag.AddObjectData(store, "ReportType02_B1", ListRdoType02_B1);
            objectTag.AddObjectData(store, "ReportType02_B2", ListRdoType02_B2);
            objectTag.AddRelationship(store, "ReportParentA", "ReportType02", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentA", "ReportType02_A", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentB", "ReportType02_B1", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentC", "ReportType02_B2", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
        }

        private void AddTypeOt(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "ReportTypeOt", ListRdoTypeOt);
            objectTag.AddObjectData(store, "ReportTypeOt_A", ListRdoTypeOt_A);
            objectTag.AddObjectData(store, "ReportTypeOt_B", ListRdoTypeOt_B);
            objectTag.AddObjectData(store, "ReportTypeOt_C", ListRdoTypeOt_C);
            objectTag.AddRelationship(store, "ReportParentA", "ReportTypeOt", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentA", "ReportTypeOt_A", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentB", "ReportTypeOt_B", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
            objectTag.AddRelationship(store, "ReportParentC", "ReportTypeOt_C", "HEIN_SERVICE_TYPE_ID", "HEIN_SERVICE_TYPE_ID");
        }

        private void SelectSheet(ref Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {

                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);
                if (filter.IS_ROUTE ?? false)
                {
                    xls.ActiveSheet = 1;
                }
                else
                {
                    xls.ActiveSheet = 2;
                }


                xls.Save(resultStream);
                //resultStream = result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    

    }

}
