using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MRS.Processor.Mrs00381;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using Inventec.Common.Logging;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00381
{
    public class Mrs00381Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Mrs00381Filter castFilter = null;
        List<Mrs00381RDO> ListRdo = new List<Mrs00381RDO>();
        List<Mrs00381RDO> ListRdoParent = new List<Mrs00381RDO>();
        List<Mrs00381RDO_> ListRdo_ = new List<Mrs00381RDO_>();
        List<Mrs00381RDO_> ListRdoParent_ = new List<Mrs00381RDO_>();

        List<Mrs00381RDO> MediDetail = new List<Mrs00381RDO>();
        List<Mrs00381RDO_> MateDetail = new List<Mrs00381RDO_>();

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
        List<HIS_IMP_MEST> listImpMest = new List<HIS_IMP_MEST>();
        List<HIS_MEDICINE_TYPE> listMedicineType = new List<HIS_MEDICINE_TYPE>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();

        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
           IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL

        };
        public Mrs00381Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00381Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00381Filter)this.reportFilter;
                listMedicineType = new HisMedicineTypeManager().Get(new HisMedicineTypeFilterQuery());
                listMaterialType = new HisMaterialTypeManager().Get(new HisMaterialTypeFilterQuery());
                var ExpMestMedicineFilter = new HisExpMestMedicineViewFilterQuery
                {
                    EXP_TIME_FROM = castFilter.TIME_FROM,
                    EXP_TIME_TO = castFilter.TIME_TO,
                    MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs,
                    EXP_MEST_TYPE_IDs = this.CHMS_EXP_MEST_TYPE_IDs,
                    IS_EXPORT = true

                };
                listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicineFilter);
                LogSystem.Info("listExpMestMedicine" + listExpMestMedicine.Count.ToString());

                var ExpMestMaterialFilter = new HisExpMestMaterialViewFilterQuery
                {
                    EXP_TIME_FROM = castFilter.TIME_FROM,
                    EXP_TIME_TO = castFilter.TIME_TO,
                    MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs,
                    EXP_MEST_TYPE_IDs = this.CHMS_EXP_MEST_TYPE_IDs,
                    IS_EXPORT = true
                };
                listExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialFilter);
                LogSystem.Info("listExpMestMaterial" + listExpMestMaterial.Count.ToString());

                var expMestId = new List<long>();
                if (listExpMestMedicine != null)
                {
                    expMestId.AddRange(listExpMestMedicine.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                if (listExpMestMaterial != null)
                {
                    expMestId.AddRange(listExpMestMaterial.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                expMestId = expMestId.Distinct().ToList();
                if (expMestId != null && expMestId.Count > 0)
                {
                    var skip = 0;

                    while (expMestId.Count - skip > 0)
                    {
                        var limit = expMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestFilterQuery HisExpMestfilter = new HisExpMestFilterQuery();
                        HisExpMestfilter.IDs = limit;
                        HisExpMestfilter.IMP_MEDI_STOCK_IDs = castFilter.IMP_MEDI_STOCK_IDs;
                        HisExpMestfilter.ORDER_FIELD = "ID";
                        HisExpMestfilter.ORDER_DIRECTION = "ASC";

                        var listExpMestSub = new HisExpMestManager(param).Get(HisExpMestfilter);
                        if (listExpMestSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listExpMestSub Get null");
                        else
                            listExpMest.AddRange(listExpMestSub);
                        HisImpMestFilterQuery HisImpMestfilter = new HisImpMestFilterQuery();
                        HisImpMestfilter.CHMS_EXP_MEST_IDs = limit;
                        HisImpMestfilter.MEDI_STOCK_IDs = castFilter.IMP_MEDI_STOCK_IDs;
                        HisImpMestfilter.ORDER_FIELD = "ID";
                        HisImpMestfilter.ORDER_DIRECTION = "ASC";

                        var listImpMestSub = new HisImpMestManager(param).Get(HisImpMestfilter);
                        if (listImpMestSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listImpMestSub Get null");
                        else
                            listImpMest.AddRange(listImpMestSub);
                    }
                    Inventec.Common.Logging.LogSystem.Info("listExpMest" + listExpMest.Count);
                }

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
                ListRdo.Clear();
                ListRdo_.Clear();
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    listExpMestMedicine = listExpMestMedicine.Where(o => listExpMest.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                    var lismedi = (from r in listExpMestMedicine select new Mrs00381RDO(r, listExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID), listImpMest.FirstOrDefault(o => o.CHMS_EXP_MEST_ID == r.EXP_MEST_ID), listMedicineType.FirstOrDefault(o=>o.ID==r.MEDICINE_TYPE_ID))).ToList();
                    MediDetail = lismedi;
                    LogSystem.Info("lismedi" + lismedi.Count.ToString());

                    var groupById = lismedi.GroupBy(o => new { o.SERVICE_ID, o.SERVICE_STOCK_ID, o.MEDI_STOCK_ID }).ToList();
                    foreach (var item in groupById)
                    {
                        List<Mrs00381RDO> listSub = item.ToList<Mrs00381RDO>();
                        Mrs00381RDO rdo = new Mrs00381RDO();
                        rdo = listSub.First();
                        rdo.AMOUNT = listSub.Sum(q => q.AMOUNT);
                        rdo.IMP_PRICE = listSub.First().IMP_PRICE * (1 + listSub.First().IMP_VAT_RATIO);
                        rdo.TOTAL_IMP_PRICE = listSub.First().IMP_PRICE * listSub.Sum(q => q.AMOUNT);
                        ListRdo.Add(rdo);
                    }

                    LogSystem.Info("ListRdo" + ListRdo.Count.ToString());

                }

                if (IsNotNullOrEmpty(listExpMestMaterial))
                {
                    listExpMestMaterial = listExpMestMaterial.Where(o => listExpMest.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                    var lismate = (from r in listExpMestMaterial select new Mrs00381RDO_(r, listExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID), listImpMest.FirstOrDefault(o => o.CHMS_EXP_MEST_ID == r.EXP_MEST_ID), listMaterialType.FirstOrDefault(o => o.ID == r.MATERIAL_TYPE_ID))).ToList();
                    MateDetail = lismate;
                    LogSystem.Info("lismate" + lismate.Count.ToString());
                    var groupById = lismate.GroupBy(o => new { o.SERVICE_ID, o.SERVICE_STOCK_ID, o.MEDI_STOCK_ID }).ToList();
                    foreach (var item in groupById)
                    {
                        List<Mrs00381RDO_> listSub = item.ToList<Mrs00381RDO_>();
                        Mrs00381RDO_ rdo = new Mrs00381RDO_();
                        rdo = listSub.First();
                        rdo.AMOUNT = listSub.Sum(q => q.AMOUNT);
                        rdo.IMP_PRICE = listSub.First().IMP_PRICE * (1 + listSub.First().IMP_VAT_RATIO);
                        rdo.TOTAL_IMP_PRICE = listSub.First().IMP_PRICE * listSub.Sum(q => q.AMOUNT);
                        ListRdo_.Add(rdo);
                    }
                }
                LogSystem.Info("ListRdo_" + ListRdo_.Count.ToString());

                result = true;
                ListRdoParent = ListRdo.GroupBy(o => o.SERVICE_STOCK_ID).Select(o => o.First()).ToList();
                LogSystem.Info("ListRdoParent_" + ListRdoParent_.Count.ToString());
                ListRdoParent_ = ListRdo_.GroupBy(o => o.SERVICE_STOCK_ID).Select(o => o.First()).ToList();
                LogSystem.Info("ListRdoParent_" + ListRdoParent_.Count.ToString());

            }

            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                ListRdo_.Clear();

            }
            return result;

        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "ReportMedi", ListRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "ParentMedi", ListRdoParent);
                objectTag.AddObjectData(store, "ReportMate", ListRdo_.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "ParentMate", ListRdoParent_);
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(",", HisMediStockCFG.HisMediStocks.Where(o => castFilter.MEDI_STOCK_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToList()));
                }
                objectTag.AddRelationship(store, "ParentMedi", "ReportMedi", "SERVICE_STOCK_ID", "SERVICE_STOCK_ID");
                objectTag.AddRelationship(store, "ParentMate", "ReportMate", "SERVICE_STOCK_ID", "SERVICE_STOCK_ID");

                //Chi tiet theo phieu nhap
                objectTag.AddObjectData(store, "MediDetail", MediDetail);
                objectTag.AddObjectData(store, "MateDetail", MateDetail);
                objectTag.AddObjectData(store, "listImpMestMe", MediDetail.GroupBy(o=>o.IMP_MEST_ID).Select(p=>p.First()).ToList());
                objectTag.AddObjectData(store, "listImpMestMa", MateDetail.GroupBy(o => o.IMP_MEST_ID).Select(p => p.First()).ToList());
                if (IsNotNullOrEmpty(castFilter.IMP_MEDI_STOCK_IDs))
                {
                    dicSingleTag.Add("IMP_MEDI_STOCK_NAME", String.Join(",", HisMediStockCFG.HisMediStocks.Where(o => castFilter.IMP_MEDI_STOCK_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToList()));
                }
                objectTag.AddRelationship(store, "listImpMestMe", "MediDetail", "IMP_MEST_ID", "IMP_MEST_ID");
                objectTag.AddRelationship(store, "listImpMestMa", "MateDetail", "IMP_MEST_ID", "IMP_MEST_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }

        }
    }
}