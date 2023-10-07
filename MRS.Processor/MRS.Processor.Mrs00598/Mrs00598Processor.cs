using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00598;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestType;
using FlexCel.Core;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMest;

namespace MRS.Processor.Mrs00598
{
    public class Mrs00598Processor : AbstractProcessor
    {
        private List<Mrs00598RDO> ListRdo = new List<Mrs00598RDO>();
        Mrs00598Filter filter = null;
        string thisReportTypeCode = "";

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<HIS_IMP_MEST> listImpMestMoba = new List<HIS_IMP_MEST>();
        List<HIS_EXP_MEST> listExpMestMoba = new List<HIS_EXP_MEST>();

        List<V_HIS_SERVICE> listHisService = new List<V_HIS_SERVICE>();

        List<HIS_IMP_MEST_TYPE> listHisImpMestType = new List<HIS_IMP_MEST_TYPE>();
        List<long> IMP_MEST_TYPE_IDs = new List<long>() 
        {
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL
        };
        public Mrs00598Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00598Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00598Filter)this.reportFilter;
            try
            {
                //Danh sách phiếu tổng hợp trả

                HisImpMestMedicineViewFilterQuery HisImpMestMedicinefilter = new HisImpMestMedicineViewFilterQuery()
                {
                    IMP_TIME_FROM = filter.TIME_FROM,
                    IMP_TIME_TO = filter.TIME_TO,
                    MEDI_STOCK_ID = filter.MEDI_STOCK_ID
                };
                listImpMestMedicine = new HisImpMestMedicineManager().GetView(HisImpMestMedicinefilter) ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                if (filter.REQ_DEPARTMENT_ID != null)
                {
                    listImpMestMedicine = listImpMestMedicine.Where(o => o.REQ_DEPARTMENT_ID == filter.REQ_DEPARTMENT_ID).ToList();
                }

                listImpMestMedicine = listImpMestMedicine.Where(o => this.IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                //Danh sach xuat vat tu
                HisImpMestMaterialViewFilterQuery HisImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery()
                {
                    IMP_TIME_FROM = filter.TIME_FROM,
                    IMP_TIME_TO = filter.TIME_TO,
                    MEDI_STOCK_ID = filter.MEDI_STOCK_ID
                };
                listImpMestMaterial = new HisImpMestMaterialManager().GetView(HisImpMestMaterialfilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
                if (filter.REQ_DEPARTMENT_ID != null)
                {
                    listImpMestMaterial = listImpMestMaterial.Where(o => o.REQ_DEPARTMENT_ID == filter.REQ_DEPARTMENT_ID).ToList();
                }

                listImpMestMaterial = listImpMestMaterial.Where(o => this.IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                var ListImpMestMobaId = new List<long>();
                ListImpMestMobaId.AddRange(listImpMestMedicine.Select(o => o.IMP_MEST_ID).ToList());
                ListImpMestMobaId.AddRange(listImpMestMaterial.Select(o => o.IMP_MEST_ID).ToList());
                ListImpMestMobaId = ListImpMestMobaId.Distinct().ToList();
                //phiếu nhập

                if (IsNotNullOrEmpty(ListImpMestMobaId))
                {
                    var skip = 0;
                    while (ListImpMestMobaId.Count - skip > 0)
                    {
                        var listIDs = ListImpMestMobaId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestFilterQuery HisImpMestfilter = new HisImpMestFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var ListHisImpMestSub = new HisImpMestManager().Get(HisImpMestfilter);
                        if (IsNotNullOrEmpty(ListHisImpMestSub))
                            listImpMestMoba.AddRange(ListHisImpMestSub);
                    }
                }

                var ListExpMestMobaId = listImpMestMoba.Where(p => p.MOBA_EXP_MEST_ID.HasValue).Select(o => o.MOBA_EXP_MEST_ID.Value).Distinct().ToList();

                //xuất từ trả

                if (IsNotNullOrEmpty(ListExpMestMobaId))
                {
                    var skip = 0;
                    while (ListExpMestMobaId.Count - skip > 0)
                    {
                        var listIDs = ListExpMestMobaId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestFilterQuery ExpMestfilter = new HisExpMestFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var ListExpMestSub = new HisExpMestManager().Get(ExpMestfilter);
                        if (IsNotNullOrEmpty(ListExpMestSub))
                            listExpMestMoba.AddRange(ListExpMestSub);
                    }
                }
                listHisImpMestType = new HisImpMestTypeManager().Get(new HisImpMestTypeFilterQuery());

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
               
                    listImpMestMoba = listImpMestMoba.OrderBy(q => q.IMP_MEST_CODE).ToList();

                    listHisService.AddRange(listImpMestMedicine.Select(p => new V_HIS_SERVICE() { SERVICE_CODE = p.MEDICINE_TYPE_CODE, SERVICE_NAME = p.MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = p.SERVICE_UNIT_NAME, ID = p.SERVICE_ID }).OrderBy(q => q.SERVICE_NAME).ToList());
                    listHisService.AddRange(listImpMestMaterial.Select(p => new V_HIS_SERVICE() { SERVICE_CODE = p.MATERIAL_TYPE_CODE, SERVICE_NAME = p.MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = p.SERVICE_UNIT_NAME, ID = p.SERVICE_ID }).OrderBy(q => q.SERVICE_NAME).ToList());
                listHisService = listHisService.GroupBy(o => o.ID).Select(p => p.First()).ToList();

                foreach (var item in listImpMestMoba)
                {
                    var impMestType = listHisImpMestType.FirstOrDefault(o => o.ID == item.IMP_MEST_TYPE_ID) ?? new HIS_IMP_MEST_TYPE();
                    var expMestMoba = listExpMestMoba.FirstOrDefault(o => o.ID == item.MOBA_EXP_MEST_ID) ?? new HIS_EXP_MEST();
                    Mrs00598RDO rdo = new Mrs00598RDO();
                    rdo.IMP_TIME = item.IMP_TIME;
                    rdo.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IMP_TIME ?? 0);
                    rdo.IMP_MEST_CODE = item.IMP_MEST_CODE;
                    rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.AGGR_EXP_MEST_CODE = expMestMoba.TDL_AGGR_EXP_MEST_CODE;
                    rdo.AGGR_IMP_MEST_ID = item.AGGR_IMP_MEST_ID;
                    rdo.IMP_MEST_TYPE_NAME = impMestType.IMP_MEST_TYPE_NAME;
                    int i = 0;
                    bool hasAmount = false;
                    foreach (var sv in listHisService)
                    {
                        i++;
                        if (rdo.JSON_AMOUNT == null)
                        {
                            rdo.JSON_AMOUNT = "";
                        }
                        var listImpMestMedicineSub = listImpMestMedicine.Where(o => o.IMP_MEST_ID == item.ID && o.SERVICE_ID == sv.ID).ToList();
                        var listImpMestMaterialSub = listImpMestMaterial.Where(o => o.IMP_MEST_ID == item.ID && o.SERVICE_ID == sv.ID).ToList();
                        rdo.DIC_AMOUNT.Add(i.ToString(), listImpMestMedicineSub.Sum(o => o.AMOUNT) + listImpMestMaterialSub.Sum(o => o.AMOUNT));

                        if (rdo.DIC_AMOUNT[i.ToString()] > 0)
                        {
                            hasAmount = true;
                        }
                    }
                    if (hasAmount)
                    {
                        rdo.JSON_AMOUNT = string.Join("", rdo.DIC_AMOUNT.Values.Select(o => string.Format("{0} \t ", o)));
                        ListRdo.Add(rdo);
                    }
                }
              
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00598RDO>();
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
            if (listImpMestMedicine != null)
            {
                listSum.AddRange(listImpMestMedicine.OrderBy(p => p.MEDICINE_TYPE_NAME).GroupBy(o => o.MEDICINE_TYPE_ID).Select(q => q.Sum(s => s.AMOUNT).ToString()).ToList());
            }
            if (listImpMestMaterial != null)
            {
                listSum.AddRange(listImpMestMaterial.OrderBy(p => p.MATERIAL_TYPE_NAME).GroupBy(o => o.MATERIAL_TYPE_ID).Select(q => q.Sum(s => s.AMOUNT).ToString()).ToList());
            }
            dicSingleTag.Add("SERVICE_SUMs", string.Join(" \t ", listSum));
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SplitColumn;
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(O => O.IMP_MEST_CODE).ToList());

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
