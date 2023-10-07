using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FlexCel.Core;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00303
{
    public class Mrs00303Processor : AbstractProcessor
    {
        private List<Mrs00303RDO> listRdo = new List<Mrs00303RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00303RDO> ListRdo = new List<Mrs00303RDO>();
        List<Mrs00303RDO> ListDetailRdo = new List<Mrs00303RDO>();
        List<Mrs00303MedicineRDO> ListMedicineType = new List<Mrs00303MedicineRDO>();
        List<HIS_MEDI_STOCK> medistock = new List<HIS_MEDI_STOCK>();
        List<HIS_MEDICINE_GROUP> listMedicineGroup = new List<HIS_MEDICINE_GROUP>();
        List<string> listDate = new List<string>();
        //List<V_HIS_EXP_MEST> listPrescription = new List<V_HIS_EXP_MEST>(); 
        //List<V_HIS_EXP_MEST> listHisSaleExpMest = new List<V_HIS_EXP_MEST>(); 
        //List<HIS_EXP_MEST_TYPE> listHisExpMestType = new List<HIS_EXP_MEST_TYPE>(); 
        List<V_HIS_EXP_MEST_MEDICINE> listHisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<HIS_TREATMENT> listTreatment1 = new List<HIS_TREATMENT>();
        //List<V_HIS_EXP_MEST_MATERIAL> listHisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineTH = new List<V_HIS_IMP_MEST_MEDICINE>();//thuốc thu hồi
        List<HIS_VACCINATION> ListVaccination = new List<HIS_VACCINATION>();
        string DATE_STR = "";
        List<DATE_STR> listDateStr = new List<DATE_STR>();

        public Mrs00303Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00303Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            try
            {
                string query0 = "select * from his_medicine_group";
                listMedicineGroup = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICINE_GROUP>(query0);
                string query = "select pr.id parent_id, pr.medicine_type_code, pr.medicine_type_name, mt.id medicine_type_id from his_medicine_type pr join his_medicine_type mt on pr.id = mt.parent_id";
                ListMedicineType = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00303MedicineRDO>(query);
                listDate = ConvertToListStringTime(((Mrs00303Filter)this.reportFilter).TIME_FROM, ((Mrs00303Filter)this.reportFilter).TIME_TO);
                DATE_STR = String.Join("\t", listDate);
                //get dữ liệu:
                HisMediStockFilterQuery stock = new HisMediStockFilterQuery();
                stock.IDs = ((Mrs00303Filter)this.reportFilter).MEDI_STOCK_IDs;
                medistock = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).Get(stock);

                HisExpMestMedicineViewFilterQuery medifilter = new HisExpMestMedicineViewFilterQuery();
                medifilter.EXP_TIME_FROM = ((Mrs00303Filter)this.reportFilter).TIME_FROM;
                medifilter.EXP_TIME_TO = ((Mrs00303Filter)this.reportFilter).TIME_TO;
                medifilter.IS_EXPORT = true;
                medifilter.MEDI_STOCK_IDs = ((Mrs00303Filter)this.reportFilter).MEDI_STOCK_IDs;
                if (((Mrs00303Filter)this.reportFilter).IS_ALL_EXP_MEST_TYPE != true)
                {
                    medifilter.EXP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                    };
                }
                else

                    medifilter.EXP_MEST_TYPE_IDs = ((Mrs00303Filter)this.reportFilter).EXP_MEST_TYPE_IDs;
                listHisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(medifilter);
                int skip = 0;
                var treatmentIds = listHisExpMestMedicine.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                while (treatmentIds.Count - skip > 0)
                {
                    var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                    treatFilter.IDs = listIds;
                    var treatmentSub = new HisTreatmentManager().Get(treatFilter);
                    if (treatmentSub != null)
                    {
                        listTreatment.AddRange(treatmentSub);
                    }

                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
                int skip1 = 0;
                var vaccinationIds = listHisExpMestMedicine.Where(p => p.TDL_VACCINATION_ID != null).Select(p => p.TDL_VACCINATION_ID).Distinct().ToList();
                while (vaccinationIds.Count - skip1 > 0)
                {
                    var listIds = vaccinationIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    string queryVaccine = string.Format("select * from his_vaccination where id in ({0})", string.Join(",", listIds));
                    var listVaccineSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_VACCINATION>(queryVaccine);
                    if (listVaccineSub != null)
                    {
                        ListVaccination.AddRange(listVaccineSub);
                    }
                    
                    skip1 += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }

                
                //HisExpMestMaterialViewFilterQuery matefilter = new HisExpMestMaterialViewFilterQuery(); 
                //matefilter.EXP_TIME_FROM = ((Mrs00303Filter)this.reportFilter).TIME_FROM; 
                //matefilter.EXP_TIME_TO = ((Mrs00303Filter)this.reportFilter).TIME_TO; 
                //medifilter.MEDI_STOCK_IDs = ((Mrs00303Filter)this.reportFilter).MEDI_STOCK_IDs; 
                //medifilter.EXP_MEST_TYPE_IDs = new List<long>()
                //    {
                //        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL,
                //        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                //    }; 
                //listHisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(matefilter); 

                //Dữ liệu thu hồi
                GetImpMestMediMateTH();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetImpMestMediMateTH()
        {
            List<long> listTHExpMestId = new List<long>();
            listTHExpMestId.AddRange(listHisExpMestMedicine.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).ToList());
            //listTHExpMestId.AddRange(listExpMestMaterialView.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).ToList());
            listTHExpMestId = listTHExpMestId.Distinct().ToList();
            if (listTHExpMestId != null && listTHExpMestId.Count > 0)
            {
                int start = 0;
                int count = listTHExpMestId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    List<long> listTHExpMestIdSub = listTHExpMestId.Skip(start).Take(limit).ToList();
                    HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                    impMestFilter.MOBA_EXP_MEST_IDs = listTHExpMestIdSub;
                    impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    var mobaImpMestSub = new HisImpMestManager().Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
                    HisImpMestMedicineViewFilterQuery impMestMediFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMediFilter.IMP_MEST_IDs = mobaImpMestSub.Select(o => o.ID).ToList();
                    var impMestMedicineTHSub = new HisImpMestMedicineManager().GetView(impMestMediFilter);
                    if (impMestMedicineTHSub != null)
                    {
                        this.listImpMestMedicineTH.AddRange(impMestMedicineTHSub);
                    }
                    //HisImpMestMaterialViewFilterQuery impMestMateFilter = new HisImpMestMaterialViewFilterQuery();
                    //impMestMateFilter.IMP_MEST_IDs = mobaImpMestSub.Select(o => o.ID).ToList();
                    //var impMestMaterialTHSub = new HisImpMestMaterialManager().GetView(impMestMateFilter);
                    //if (impMestMaterialTHSub != null)
                    //{
                    //    this.listImpMestMaterialTH.AddRange(impMestMaterialTHSub);
                    //}
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
        }

        private List<string> ConvertToListStringTime(long p1, long p2)
        {
            List<string> result = new List<string>();
            try
            {
                DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p1);
                DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p2);
                DateTime IndexTime = StartTime.Date;
                while (IndexTime < FinishTime)
                {
                    result.Add("\'" + Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(IndexTime));
                    DATE_STR dateStr = new Mrs00303.DATE_STR();
                    dateStr.DATE_STRING = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(IndexTime);
                    listDateStr.Add(dateStr);
                    var daysInMonth = DateTime.DaysInMonth(IndexTime.Year, IndexTime.Month);
                    //if (IndexTime.Day != daysInMonth)
                    IndexTime = IndexTime.AddDays(1);
                    //else
                    //{
                    //   IndexTime.; 
                    //  IndexTime = IndexTime.AddMonths(1); 
                    // }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = new List<string>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                if (IsNotNullOrEmpty(listHisExpMestMedicine))
                {
                    var GroupbyMedicineTypeIDs = listHisExpMestMedicine.GroupBy(o => string.Format("{0}_{1}",o.MEDICINE_TYPE_ID, ((Mrs00303Filter)this.reportFilter).IS_IMP_PRICE==true?o.IMP_PRICE:o.PRICE )).ToList();

                    foreach (var group in GroupbyMedicineTypeIDs)
                    {
                        var Sub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        var medicineGroup = listMedicineGroup.FirstOrDefault(p => p.ID == Sub.First().MEDICINE_GROUP_ID);
                        var medicineType = ListMedicineType.FirstOrDefault(p => p.MEDICINE_TYPE_ID == Sub.First().MEDICINE_TYPE_ID);
                        Mrs00303RDO rdo = new Mrs00303RDO();
                        rdo.MEDICINE_TYPE_NAME = Sub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = Sub.First().SERVICE_UNIT_NAME;
                        if (medicineGroup != null)
                        {
                            rdo.MEDICINE_GROUP_CODE = medicineGroup.MEDICINE_GROUP_CODE;
                            rdo.MEDICINE_GROUP_NAME = medicineGroup.MEDICINE_GROUP_NAME;
                        }

                        if (medicineType != null)
                        {
                            rdo.PARENT_MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
                            rdo.PARENT_MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
                        }
                        rdo.PRICE =  ((Mrs00303Filter)this.reportFilter).IS_IMP_PRICE == true ? Sub.First().IMP_PRICE : Sub.First().PRICE ?? 0;
                        rdo.AMOUNT_EXP_SUM = Sub.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = rdo.PRICE * (1 + (((Mrs00303Filter)this.reportFilter).IS_IMP_PRICE == true ? Sub.First().IMP_VAT_RATIO : Sub.First().VAT_RATIO ?? 0)) * rdo.AMOUNT_EXP_SUM;
                        var impMestMediTHSub = listImpMestMedicineTH.Where(o => Sub.Select(p => p.ID).ToList().Contains(o.TH_EXP_MEST_MEDICINE_ID ?? 0)).ToList();
                        rdo.AMOUNT_MOBA_SUM = impMestMediTHSub.Sum(x => x.AMOUNT);
                        rdo.AMOUNT_STR = ConvertToStringListAmount(((Mrs00303Filter)this.reportFilter).TIME_FROM, ((Mrs00303Filter)this.reportFilter).TIME_TO, Sub);
                        rdo.DIC_DATE_AMOUNT = Sub.GroupBy(o => Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.EXP_TIME ?? 0)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_DATE_MOBA_AMOUNT = Sub.GroupBy(o => Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.EXP_TIME ?? 0)).ToDictionary(p => p.Key, q => impMestMediTHSub.Where(o => q.Select(p => p.ID).ToList().Contains(o.TH_EXP_MEST_MEDICINE_ID ?? 0)).Sum(s => s.AMOUNT));
                        ListRdo.Add(rdo);
                    }
                    
                    ProcessDetail(listHisExpMestMedicine.Where(p => p.TDL_TREATMENT_ID != null || ListVaccination.Exists(o => o.ID == p.TDL_VACCINATION_ID)).ToList());
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        private void ProcessDetail(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine)
        {
            if (listExpMestMedicine != null)
            {
                foreach (var item in listExpMestMedicine)
                {
                    var treatment = listTreatment.FirstOrDefault(p => p.ID == item.TDL_TREATMENT_ID);
                    var vaccine = ListVaccination.FirstOrDefault(p => p.ID == item.TDL_VACCINATION_ID);
                    
                    Mrs00303RDO rdo = new Mrs00303RDO();
                    if (treatment != null || vaccine != null)
                    {
                        if (treatment != null)
                        {
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        }
                        else if (vaccine != null)
                        {
                            rdo.PATIENT_NAME = vaccine.TDL_PATIENT_NAME;
                        }
                    }
                    
                    rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                    rdo.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME + " " + item.CONCENTRA;
                    rdo.NATIONAL_NAME = item.NATIONAL_NAME;
                    rdo.MANUFACTURER_NAME = item.MANUFACTURER_NAME;
                    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                    rdo.AMOUNT = item.AMOUNT;
                    rdo.VAT_RATIO = ((Mrs00303Filter)this.reportFilter).IS_IMP_PRICE == true ? item.IMP_VAT_RATIO : item.VAT_RATIO ?? 0;
                    rdo.PRICE = ((Mrs00303Filter)this.reportFilter).IS_IMP_PRICE == true ? item.IMP_PRICE : item.PRICE ?? 0;
                    rdo.VIR_TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE * (1 + rdo.VAT_RATIO ?? 0);
                    ListDetailRdo.Add(rdo);
                }
            }
        }

        private string ConvertToStringListAmount(long p1, long p2, List<V_HIS_EXP_MEST_MEDICINE> Sub)
        {
            string result = "";
            try
            {
                DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p1);
                DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p2);
                DateTime IndexTime = StartTime.Date;
                while (IndexTime < FinishTime)
                {
                    result += ConvertToStringAmount(IndexTime, Sub) + "\t";
                    var daysInMonth = DateTime.DaysInMonth(IndexTime.Year, IndexTime.Month);
                    //if (IndexTime.Day != daysInMonth)
                    IndexTime = IndexTime.AddDays(1);
                    //else
                    //{
                    //IndexTime.AddDays(1); 
                    //IndexTime = IndexTime.AddMonths(1); 
                    // }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ConvertToStringAmount(DateTime IndexTime, List<V_HIS_EXP_MEST_MEDICINE> Sub)
        {
            string result = "";
            try
            {
                Decimal IndexAmount = Sub.Where(p => ((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p.EXP_TIME ?? 0)).Date == IndexTime).Sum(o => o.AMOUNT);
                result = string.Format("{0:000000000000}", Convert.ToInt64(IndexAmount));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00303Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00303Filter)this.reportFilter).TIME_TO));
            dicSingleTag.Add("DATE_STR", DATE_STR);
            dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", medistock.Select(o => o.MEDI_STOCK_NAME).ToList()));
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SplitColumn;
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "listDateStr", listDateStr);
            objectTag.AddObjectData(store, "Detail", ListDetailRdo);
        }

        private void SplitColumn(ref Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {
                //MemoryStream result = new MemoryStream();
                //resultStream.Position = 0;
                //store.flexCel.Run(resultStream, result);
                //if (result != null)
                //{
                //    result.Position = 0;
                //}
                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);
                var rountCount = xls.RowCount;
                TFlxFormat fmt = xls.GetCellVisibleFormatDef(7, 9);
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                for (int i = 7; i <= rountCount; i++)
                {
                    string valueCell = "";
                    try
                    {
                        valueCell=(string)xls.GetCellValue(i, 9);
                        if (string.IsNullOrWhiteSpace(valueCell))
                        {
                            continue;
                        }
                    }
                    catch(Exception)
                    {}
                    xls.PasteFromTextClipboardFormat(i, 9, TFlxInsertMode.NoneRight, valueCell);
                    for (int j = 0; j < listDate.Count; j++)
                    {
                        xls.SetCellFormat(i, j + 9, xls.AddFormat(fmt));
                    }
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