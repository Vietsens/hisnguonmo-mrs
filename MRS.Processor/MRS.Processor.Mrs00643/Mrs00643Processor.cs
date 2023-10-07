using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00643
{
    public class Mrs00643Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Mrs00643Filter filter = null;
        List<Mrs00643RDOTreatment> ListRdoTreatment = new List<Mrs00643RDOTreatment>();
        List<Mrs00643RDOService> ListRdoService = new List<Mrs00643RDOService>();
        List<Mrs00643RDOMedicine> ListRdoMedicine = new List<Mrs00643RDOMedicine>();
        List<Mrs00643RDOMaterial> ListRdoMaterial = new List<Mrs00643RDOMaterial>();
        List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERE_SERV> ListHisSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_MEDICINE_TYPE> ListHisMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListHisMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<HIS_PTTT_GROUP> ListPtttGroup = new List<HIS_PTTT_GROUP>(); 

        public Mrs00643Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00643Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00643Filter)reportFilter);
            var result = true;
            try
            {
                filter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;
                Mrs00643GDO gdo = new ManagerSql().GetSereServDO(this.filter);
                if (gdo != null && gdo.listHisSereServ != null)
                {
                    ListHisSereServ = gdo.listHisSereServ;
                }
                if (gdo != null && gdo.listHisTreatment != null)
                {
                    ListHisTreatment = gdo.listHisTreatment;
                }

                HisServiceRetyCatViewFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                ServiceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00643";
                ListHisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(ServiceRetyCatFilter);

                ListHisMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());

                ListHisMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                serviceFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListService = new HisServiceManager(paramGet).Get(serviceFilter);

                HisPtttGroupFilterQuery ptttGroupFilter = new HisPtttGroupFilterQuery();
                ptttGroupFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListPtttGroup = new HisPtttGroupManager(paramGet).Get(ptttGroupFilter);
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
            bool result = false;
            try
            {

                ListRdoTreatment.Clear();

                if (IsNotNullOrEmpty(ListHisTreatment))
                {
                    ListRdoTreatment = (from b in ListHisTreatment select new Mrs00643RDOTreatment(b, ListHisSereServ, ListHisServiceRetyCat, filter)).ToList();
                }
                if (IsNotNullOrEmpty(ListHisSereServ))
                {
                    var ListHisSereServService = ListHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                    ListRdoService = (from b in ListHisSereServService select new Mrs00643RDOService(b, ListHisServiceRetyCat, ListHisTreatment, ListService, ListPtttGroup)).ToList();
                    ListRdoService = ProcessListRDO(ListRdoService);
                }
                if (IsNotNullOrEmpty(ListHisSereServ))
                {
                    var ListHisSereServMedicine = ListHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).ToList();
                    ListRdoMedicine = (from b in ListHisSereServMedicine select new Mrs00643RDOMedicine(b, ListHisTreatment, ListHisMedicineType)).ToList();
                    ListRdoMedicine = ProcessListRDO(ListRdoMedicine);
                }
                if (IsNotNullOrEmpty(ListHisSereServ))
                {
                    var ListHisSereServMaterial = ListHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                    ListRdoMaterial = (from b in ListHisSereServMaterial select new Mrs00643RDOMaterial(b, ListHisTreatment, ListHisMaterialType)).ToList();
                    ListRdoMaterial = ProcessListRDO(ListRdoMaterial);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

                ListRdoTreatment.Clear();
            }
            return result;
        }

        private List<Mrs00643RDOService> ProcessListRDO(List<Mrs00643RDOService> listRdo)
        {
            List<Mrs00643RDOService> listCurrent = new List<Mrs00643RDOService>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.TREAT_PATIENT_TYPE_ID, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00643RDOService> listsub = group.ToList<Mrs00643RDOService>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00643RDOService rdo = new Mrs00643RDOService();
                            rdo = listsub[0];
                            rdo.AMOUNT_NOITRU = listsub.Sum(s => s.AMOUNT_NOITRU);
                            rdo.AMOUNT_NGOAITRU = listsub.Sum(s => s.AMOUNT_NGOAITRU);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);

                            if (rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0)
                            {
                                listCurrent.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.SERVICE_TYPE_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        private List<Mrs00643RDOMedicine> ProcessListRDO(List<Mrs00643RDOMedicine> listRdo)
        {
            List<Mrs00643RDOMedicine> listCurrent = new List<Mrs00643RDOMedicine>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.TREAT_PATIENT_TYPE_ID, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00643RDOMedicine> listsub = group.ToList<Mrs00643RDOMedicine>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00643RDOMedicine rdo = new Mrs00643RDOMedicine();
                            rdo = listsub[0];
                            rdo.AMOUNT_NOITRU = listsub.Sum(s => s.AMOUNT_NOITRU);
                            rdo.AMOUNT_NGOAITRU = listsub.Sum(s => s.AMOUNT_NGOAITRU);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);

                            if (rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0)
                            {
                                listCurrent.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.MEDICINE_TYPE_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        private List<Mrs00643RDOMaterial> ProcessListRDO(List<Mrs00643RDOMaterial> listRdo)
        {
            List<Mrs00643RDOMaterial> listCurrent = new List<Mrs00643RDOMaterial>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.TREAT_PATIENT_TYPE_ID, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00643RDOMaterial> listsub = group.ToList<Mrs00643RDOMaterial>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00643RDOMaterial rdo = new Mrs00643RDOMaterial();
                            rdo = listsub[0];
                            rdo.AMOUNT_NOITRU = listsub.Sum(s => s.AMOUNT_NOITRU);
                            rdo.AMOUNT_NGOAITRU = listsub.Sum(s => s.AMOUNT_NGOAITRU);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);

                            if (rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0)
                            {
                                listCurrent.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.MATERIAL_TYPE_NAME).ThenByDescending(o => o.PRICE).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var InSide = ListRdoTreatment.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.TOTAL_PRICE > 0).ToList();
            var OutSide = ListRdoTreatment.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.TOTAL_PRICE > 0).ToList();

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.INTRUCTION_TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.INTRUCTION_TIME_TO));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());
            dicSingleTag.Add("INSIDE_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(InSide.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("OUTSIDE_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(OutSide.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("SERV_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(ListRdoService.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("MEDI_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(ListRdoMedicine.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("MATE_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(ListRdoMaterial.Sum(s => s.TOTAL_PRICE)).ToString()));

            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.AddObjectData(store, "ParentService", ListRdoService.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ParentMaterial", ListRdoService.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ParentMedicine", ListRdoService.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ParentInSide", ListRdoService.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ParentOutSide", ListRdoService.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "InSide", InSide);
            objectTag.AddRelationship(store, "ParentInSide", "InSide", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
            objectTag.AddObjectData(store, "OutSide", OutSide);
            objectTag.AddRelationship(store, "ParentOutSide", "OutSide", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
            var groupByServiceType = ListRdoService.GroupBy(o => o.SERVICE_TYPE_CODE).ToDictionary(p => p.Key ?? "", q => q.ToList<Mrs00643RDOService>());
            foreach (var item in HisServiceTypeCFG.HisServiceTypes)
            {
                if (groupByServiceType.ContainsKey(item.SERVICE_TYPE_CODE))
                {
                    objectTag.AddObjectData(store, item.SERVICE_TYPE_CODE, groupByServiceType[item.SERVICE_TYPE_CODE]);
                    objectTag.AddRelationship(store, "ParentService", item.SERVICE_TYPE_CODE, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                }
                else
                {
                    objectTag.AddObjectData(store, item.SERVICE_TYPE_CODE, new List<Mrs00643RDOService>());

                    objectTag.AddRelationship(store, "ParentService", item.SERVICE_TYPE_CODE, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                }
            }

            var groupByCategory = ListRdoService.GroupBy(o => o.CATEGORY_CODE).ToDictionary(p => p.Key ?? "", q => q.ToList<Mrs00643RDOService>());
            foreach (var item in ListHisServiceRetyCat.Select(o => o.CATEGORY_CODE).Distinct().ToList())
            {
                if (groupByCategory.ContainsKey(item))
                {
                    objectTag.AddObjectData(store, item, groupByCategory[item]);
                    objectTag.AddRelationship(store, "ParentService", item, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                }
                else
                {
                    objectTag.AddObjectData(store, item, new List<Mrs00643RDOService>());
                    objectTag.AddRelationship(store, "ParentService", item, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                }
            }

            objectTag.AddObjectData(store, "Medicine", ListRdoMedicine);
            objectTag.AddRelationship(store, "ParentMedicine", "Medicine", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
            objectTag.AddObjectData(store, "Material", ListRdoMaterial);
            objectTag.AddRelationship(store, "ParentMaterial", "Material", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;

        }

        private void SelectSheet(ref Inventec.Common.FlexCellExport.Store store, ref System.IO.MemoryStream resultStream)
        {

            resultStream.Position = 0;
            FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
            xls.Open(resultStream);
            try
            {

                if (!String.IsNullOrWhiteSpace(this.ReportTemplateCode))
                {
                    xls.ActiveSheetByName = this.ReportTemplateCode;
                }
                else
                {
                    xls.ActiveSheet = 1;
                    Inventec.Common.Logging.LogSystem.Error("Khong ton tai sheet co ten giong ma mau bao cao");
                }


                xls.Save(resultStream);
                //resultStream = result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                xls.ActiveSheet = 1;
            }
        }

    }
}
