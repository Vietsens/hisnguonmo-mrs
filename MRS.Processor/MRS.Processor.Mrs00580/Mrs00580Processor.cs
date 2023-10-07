using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00580;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestType;
using FlexCel.Core;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;

namespace MRS.Processor.Mrs00580
{
    public class Mrs00580Processor : AbstractProcessor
    {
        private List<Mrs00580RDO> ListRdo = new List<Mrs00580RDO>();
        Mrs00580Filter filter = null;
        string thisReportTypeCode = "";

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        //List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        //List<V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        //List<HIS_IMP_MEST> ListMobaImpMest = new List<HIS_IMP_MEST>();

        List<V_HIS_SERVICE> listHisService = new List<V_HIS_SERVICE>();

        List<HIS_EXP_MEST> listAggrExpMest = new List<HIS_EXP_MEST>();
        List<HIS_EXP_MEST_TYPE> listHisExpMestType = new List<HIS_EXP_MEST_TYPE>();

        Dictionary<long, Mrs00580RDO> dicRdoAggr = new Dictionary<long, Mrs00580RDO>();

        public Mrs00580Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00580Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00580Filter)this.reportFilter;
            try
            {
                
                //if (filter.EXP_MEST_TYPE_IDs == null || filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL))
                //{
                //    filter.EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT);
                //}
                //Danh sach xuat thuoc
                 //Danh sách phiếu tổng hợp
                if (filter.EXP_MEST_TYPE_IDs != null && filter.EXP_MEST_TYPE_IDs.Count == 1 && filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL))
                {

                    HisExpMestFilterQuery AggrExpMestFilter = new HisExpMestFilterQuery()
                    {
                        FINISH_TIME_FROM = filter.TIME_FROM,
                        FINISH_TIME_TO = filter.TIME_TO,
                        EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL,
                        MEDI_STOCK_ID = filter.MEDI_STOCK_ID,
                        REQ_DEPARTMENT_ID = filter.REQ_DEPARTMENT_ID,
                        EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE
                    };
                    listAggrExpMest = new HisExpMestManager().Get(AggrExpMestFilter);
                   
                    //Chi tiết thuốc vật tư trong phiếu tổng họp

                    var ListAggrExpMestId = listAggrExpMest.Select(o => o.ID).ToList();
                    if (ListAggrExpMestId.Count > 0)
                    {
                        var skip = 0;
                        while (ListAggrExpMestId.Count - skip > 0)
                        {
                            var limit = ListAggrExpMestId.Skip(skip).Take(500).ToList();
                            skip = skip + 500;

                            HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                            expMediFilter.AGGR_EXP_MEST_IDs = limit;
                            expMediFilter.IS_EXPORT = true;
                            var ListExpMestMedicineSub = new HisExpMestMedicineManager().GetView(expMediFilter);
                            if (ListExpMestMedicineSub != null)
                            {
                                listExpMestMedicine.AddRange(ListExpMestMedicineSub);
                            }
                            HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                            expMateFilter.AGGR_EXP_MEST_IDs = limit;
                            expMateFilter.IS_EXPORT = true;
                            var ListExpMestMaterialSub = new HisExpMestMaterialManager().GetView(expMateFilter);
                            if (ListExpMestMaterialSub != null)
                            {
                                listExpMestMaterial.AddRange(ListExpMestMaterialSub);
                            }
                        }
                    }

                }
                else
                {
                    HisExpMestMedicineViewFilterQuery HisExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery()
                    {
                        EXP_TIME_FROM = filter.TIME_FROM,
                        EXP_TIME_TO = filter.TIME_TO,
                        MEDI_STOCK_ID = filter.MEDI_STOCK_ID,
                        REQ_DEPARTMENT_ID = filter.REQ_DEPARTMENT_ID,
                        EXP_MEST_TYPE_IDs = filter.EXP_MEST_TYPE_IDs,
                        IS_EXPORT = true
                    };
                    listExpMestMedicine = new HisExpMestMedicineManager().GetView(HisExpMestMedicinefilter);

                    //Danh sach xuat vat tu
                    HisExpMestMaterialViewFilterQuery HisExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery()
                    {
                        EXP_TIME_FROM = filter.TIME_FROM,
                        EXP_TIME_TO = filter.TIME_TO,
                        MEDI_STOCK_ID = filter.MEDI_STOCK_ID,
                        REQ_DEPARTMENT_ID = filter.REQ_DEPARTMENT_ID,
                        EXP_MEST_TYPE_IDs = filter.EXP_MEST_TYPE_IDs,
                        IS_EXPORT = true
                    };
                    listExpMestMaterial = new HisExpMestMaterialManager().GetView(HisExpMestMaterialfilter);
                }
                listHisExpMestType = new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery());

                ////Danh sách thu hồi
                //var listMobaExpMestId = new List<long>();
                //if (listExpMestMedicine != null)
                //{
                //    listMobaExpMestId.AddRange(listExpMestMedicine.Select(o => o.EXP_MEST_ID).ToList());
                //}

                //if (listExpMestMaterial != null)
                //{
                //    listMobaExpMestId.AddRange(listExpMestMaterial.Select(o => o.EXP_MEST_ID).ToList());
                //}
                //listMobaExpMestId = listMobaExpMestId.Distinct().ToList();
                //if (listMobaExpMestId.Count > 0)
                //{
                //    var skip = 0;
                //    while (listMobaExpMestId.Count - skip > 0)
                //    {
                //        var limit = listMobaExpMestId.Skip(skip).Take(500).ToList();
                //        skip = skip + 500;
                //        HisImpMestFilterQuery mobaFilter = new HisImpMestFilterQuery();
                //        mobaFilter.MOBA_EXP_MEST_IDs = limit;
                //        mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                //        var ListMobaImpMestSub = new HisImpMestManager().Get(mobaFilter);
                //        if (ListMobaImpMestSub != null)
                //        {
                //            ListMobaImpMest.AddRange(ListMobaImpMestSub);
                //        }
                //    }
                //}
                ////Chi tiết thuốc vật tư thu hồi

                //var listImpMest = ListMobaImpMest.Select(o => o.ID).ToList();
                //if (listImpMest.Count > 0)
                //{
                //    var skip = 0;
                //    while (listImpMest.Count - skip > 0)
                //    {
                //        var limit = listImpMest.Skip(skip).Take(500).ToList();
                //        skip = skip + 500;
                //        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                //        impMediFilter.IMP_MEST_IDs = limit;
                //        var ListImpMestMedicineSub = new HisImpMestMedicineManager().GetView(impMediFilter);
                //        if (ListImpMestMedicineSub != null)
                //        {
                //            ListImpMestMedicine.AddRange(ListImpMestMedicineSub);
                //        }
                //        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                //        impMateFilter.IMP_MEST_IDs = limit;
                //        var ListImpMestMaterialSub = new HisImpMestMaterialManager().GetView(impMateFilter);
                //        if (ListImpMestMaterialSub != null)
                //        {
                //            ListImpMestMaterial.AddRange(ListImpMestMaterialSub);
                //        }
                //    }
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
            bool result = false;
            try
            {
                var listExpMest = new List<HIS_EXP_MEST>();
                if (listExpMestMedicine != null)
                {
                    listExpMest.AddRange(listExpMestMedicine.Select(p => new HIS_EXP_MEST() { FINISH_TIME = p.EXP_TIME, EXP_MEST_CODE = p.EXP_MEST_CODE, EXP_MEST_TYPE_ID = p.EXP_MEST_TYPE_ID, ID = p.EXP_MEST_ID ?? 0, AGGR_EXP_MEST_ID = p.AGGR_EXP_MEST_ID }).OrderBy(q => q.EXP_MEST_CODE).ToList());

                    listHisService.AddRange(listExpMestMedicine.Select(p => new V_HIS_SERVICE() { SERVICE_CODE = p.MEDICINE_TYPE_CODE, SERVICE_NAME = p.MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = p.SERVICE_UNIT_NAME, ID = p.SERVICE_ID }).OrderBy(q => q.SERVICE_NAME).ToList());
                }

                if (listExpMestMaterial != null)
                {
                    listExpMest.AddRange(listExpMestMaterial.Select(p => new HIS_EXP_MEST() { FINISH_TIME = p.EXP_TIME, EXP_MEST_CODE = p.EXP_MEST_CODE, EXP_MEST_TYPE_ID = p.EXP_MEST_TYPE_ID, ID = p.EXP_MEST_ID ?? 0, AGGR_EXP_MEST_ID = p.AGGR_EXP_MEST_ID }).OrderBy(q => q.EXP_MEST_CODE).ToList());

                    listHisService.AddRange(listExpMestMaterial.Select(p => new V_HIS_SERVICE() { SERVICE_CODE = p.MATERIAL_TYPE_CODE, SERVICE_NAME = p.MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = p.SERVICE_UNIT_NAME, ID = p.SERVICE_ID }).OrderBy(q => q.SERVICE_NAME).ToList());
                }
              
                listExpMest = listExpMest.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                listHisService = listHisService.GroupBy(o => o.ID).Select(p => p.First()).ToList();

                foreach (var item in listExpMest)
                {
                    var expMestType = listHisExpMestType.FirstOrDefault(o => o.ID == item.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                    Mrs00580RDO rdo = new Mrs00580RDO();
                    rdo.EXP_TIME = item.FINISH_TIME;
                    rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.FINISH_TIME ?? 0);
                    rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                    rdo.AGGR_EXP_MEST_ID = item.AGGR_EXP_MEST_ID;
                    rdo.EXP_MEST_TYPE_NAME = expMestType.EXP_MEST_TYPE_NAME;
                    int i = 0;
                    bool hasAmount = false;
                    foreach (var sv in listHisService)
                    {
                        i++;
                        if (rdo.JSON_AMOUNT == null)
                        {
                            rdo.JSON_AMOUNT = "";
                        }
                        var listExpMestMedicineSub = listExpMestMedicine.Where(o => o.EXP_MEST_ID == item.ID && o.SERVICE_ID == sv.ID).ToList();
                        var listExpMestMaterialSub = listExpMestMaterial.Where(o => o.EXP_MEST_ID == item.ID && o.SERVICE_ID == sv.ID).ToList();
                        //var listImpMestMedicineSub = ListImpMestMedicine.Where(o => ListMobaImpMest.Exists(p => p.ID == o.IMP_MEST_ID && p.MOBA_EXP_MEST_ID == item.ID) && o.SERVICE_ID == sv.ID).ToList();
                        //var listImpMestMaterialSub = ListImpMestMaterial.Where(o => ListMobaImpMest.Exists(p => p.ID == o.IMP_MEST_ID && p.MOBA_EXP_MEST_ID == item.ID) && o.SERVICE_ID == sv.ID).ToList();
                        rdo.DIC_AMOUNT.Add(i.ToString(), listExpMestMedicineSub.Sum(o => o.AMOUNT) + listExpMestMaterialSub.Sum(o => o.AMOUNT));
                            //- (listImpMestMedicineSub.Sum(o => o.AMOUNT) + listImpMestMaterialSub.Sum(o => o.AMOUNT)));

                        if (rdo.DIC_AMOUNT[i.ToString()] > 0)
                        {
                            hasAmount = true;
                        }
                    }
                    if (hasAmount)
                    {
                        rdo.JSON_AMOUNT = string.Join("", rdo.DIC_AMOUNT.Values.Select(o=>string.Format("{0} \t ",o)));
                        ListRdo.Add(rdo);
                    }
                }
                if (filter.EXP_MEST_TYPE_IDs != null && filter.EXP_MEST_TYPE_IDs.Count == 1 && filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL))
                {
                    foreach (var item in ListRdo)
                    {
                        var AggrExpMest = listAggrExpMest.FirstOrDefault(o => o.ID == item.AGGR_EXP_MEST_ID);
                        var expMestType = listHisExpMestType.FirstOrDefault(o => o.ID == AggrExpMest.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                        if ((!item.AGGR_EXP_MEST_ID.HasValue) || (AggrExpMest == null))
                        {
                            continue;
                        }

                        if (dicRdoAggr.ContainsKey(item.AGGR_EXP_MEST_ID.Value))
                        {
                            int i = 0;
                            foreach (var sv in listHisService)
                            {
                                i++;
                                dicRdoAggr[item.AGGR_EXP_MEST_ID.Value].DIC_AMOUNT[i.ToString()] += item.DIC_AMOUNT[i.ToString()];
                            }
                            dicRdoAggr[item.AGGR_EXP_MEST_ID.Value].JSON_AMOUNT = string.Join("", dicRdoAggr[item.AGGR_EXP_MEST_ID.Value].DIC_AMOUNT.Values.Select(o => string.Format("{0} \t ", o)));
                        }
                        else
                        {
                            dicRdoAggr[item.AGGR_EXP_MEST_ID.Value] = item;
                            item.EXP_MEST_CODE = AggrExpMest.EXP_MEST_CODE;
                            item.EXP_MEST_TYPE_NAME = expMestType.EXP_MEST_TYPE_NAME;
                            item.EXP_TIME = AggrExpMest.FINISH_TIME;
                            item.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(AggrExpMest.FINISH_TIME ?? 0);
                        }
                        
                    }
                    ListRdo = new List<Mrs00580RDO>();
                    ListRdo = dicRdoAggr.Values.ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00580RDO>();
                result = false;
            }
            return result;
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == filter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("SERVICE_INFOs", string.Join(" \t ", listHisService.Select(o => o.SERVICE_CODE + " - " + o.SERVICE_NAME + " - " + o.SERVICE_UNIT_NAME).ToList()));
            List<string> listSum = new List<string>();
            if (listExpMestMedicine != null)
            {
                listSum.AddRange(listExpMestMedicine.OrderBy(p => p.MEDICINE_TYPE_NAME).GroupBy(o => o.MEDICINE_TYPE_ID).Select(q => q.Sum(s => s.AMOUNT - (s.TH_AMOUNT ?? 0)).ToString()).ToList());
            }
            if (listExpMestMaterial != null)
            {
                listSum.AddRange(listExpMestMaterial.OrderBy(p => p.MATERIAL_TYPE_NAME).GroupBy(o => o.MATERIAL_TYPE_ID).Select(q => q.Sum(s => s.AMOUNT - (s.TH_AMOUNT ?? 0)).ToString()).ToList());
            }
            dicSingleTag.Add("SERVICE_SUMs", string.Join(" \t ", listSum));
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SplitColumn;
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(O => O.EXP_MEST_CODE).ToList());

        }
        private void SplitColumn(ref Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {
                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);
                var rountCount = xls.RowCount;
                TFlxFormat fmt = xls.GetCellVisibleFormatDef(11, 9);
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                for (int i = 11; i <= rountCount; i++)
                {
                    string cellvalue = "";
                    try
                    {
                        cellvalue = (string)xls.GetStringFromCell(i, 9);
                    }
                    catch (Exception)
                    {
                        
                    }
                    xls.PasteFromTextClipboardFormat(i, 9, TFlxInsertMode.NoneRight, cellvalue);
                    for (int j = 0; j < listHisService.Count; j++)
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
