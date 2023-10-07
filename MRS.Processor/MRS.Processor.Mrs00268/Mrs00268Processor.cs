using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using FlexCel.Report;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMaterialType;
using System.Reflection;
using Inventec.Common.Repository;


namespace MRS.Processor.Mrs00268
{
    public class Mrs00268Processor : AbstractProcessor
    {
        private Mrs00268Filter castFilter;
        List<Mrs00268RDO> ListRdo = new List<Mrs00268RDO>();
        List<Mrs00268RDO> listParentRdo = new List<Mrs00268RDO>();

        //Cac list dau ki
        List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMate = new List<V_HIS_MEST_PERIOD_MATE>(); // DS vat tu chot ki

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialBefore = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu nhap truoc ki

        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialBefore = new List<V_HIS_EXP_MEST_MATERIAL>(); // DS vat tu xuat truoc ki

        //Cac list nhap xuat trong ki
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialOn = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu nhap trong ki

        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialOn = new List<V_HIS_EXP_MEST_MATERIAL>(); // DS vat tu xuat trong ki

        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>(); // loai vat tu

        List<ExpMestIdReason> ExpMestIdReasons = new List<ExpMestIdReason>(); // DS phieu xuat va li do xuat trong ky


        public Mrs00268Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00268Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00268Filter)reportFilter;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00268: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                #region ky gan nhat
                CommonParam paramGet = new CommonParam();
                HisMediStockPeriodFilterQuery HisMediStockPeriodfilter = new HisMediStockPeriodFilterQuery();
                HisMediStockPeriodfilter.CREATE_TIME_TO = castFilter.TIME_FROM;
                HisMediStockPeriodfilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                HisMediStockPeriodfilter.ORDER_DIRECTION = "DESC";
                HisMediStockPeriodfilter.ORDER_FIELD = "CREATE_TIME";
                var listPeriod = new HisMediStockPeriodManager(paramGet).Get(HisMediStockPeriodfilter);
                #endregion

                if (IsNotNullOrEmpty(listPeriod))
                {
                    #region Du lieu khi chot ki
                    HisMestPeriodMateViewFilterQuery mestPeriodMateFilter = new HisMestPeriodMateViewFilterQuery();
                    mestPeriodMateFilter.MEDI_STOCK_PERIOD_ID = listPeriod.First().ID;
                    listMestPeriodMate.AddRange(new HisMestPeriodMateManager(paramGet).GetView(mestPeriodMateFilter) ?? new List<V_HIS_MEST_PERIOD_MATE>());
                    #endregion

                    #region Du lieu tu sau chot ki den truoc ki bao cao
                    GetMestMate(castFilter.MEDI_STOCK_ID, listPeriod.First().CREATE_TIME, castFilter.TIME_FROM, ref listImpMestMaterialBefore, ref listExpMestMaterialBefore);
                    #endregion
                }
                else
                {
                    #region Du lieu truoc ki bao cao
                    GetMestMate(castFilter.MEDI_STOCK_ID, null, castFilter.TIME_FROM, ref listImpMestMaterialBefore, ref listExpMestMaterialBefore);
                    #endregion
                }

                #region Du lieu trong ki bao cao
                GetMestMate(castFilter.MEDI_STOCK_ID, castFilter.TIME_FROM, castFilter.TIME_TO, ref listImpMestMaterialOn, ref listExpMestMaterialOn);
                #endregion

                #region loai vat tu
                listMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
                #endregion

                //Lay cac li do xuat cua cac phieu xuat trong ki
                GetExpMestOn();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }


        private void GetExpMestOn()
        {
            try
            {
                ExpMestIdReasons = new ManagerSql().Get(this.castFilter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListRdo.Clear();
                var listSub = (from r in listMestPeriodMate select new Mrs00268RDO(r, listMaterialType)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialBefore select new Mrs00268RDO(r, listMaterialType, true)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialBefore select new Mrs00268RDO(r, listMaterialType, ExpMestIdReasons, true)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialOn select new Mrs00268RDO(r, listMaterialType, false)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialOn select new Mrs00268RDO(r, listMaterialType, ExpMestIdReasons, false)).ToList();
                ListRdo.AddRange(listSub);

                //Gộp theo thuốc, theo giá
                GroupByPrice();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        private void GroupByPrice()
        {
            //try
            //{
            var group = ListRdo.GroupBy(o => new { o.SERVICE_CODE, o.IMP_PRICE }).ToList();
            ListRdo.Clear();
            Decimal sum = 0;
            Mrs00268RDO rdo;
            List<Mrs00268RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00268RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00268RDO();
                listSub = item.ToList<Mrs00268RDO>();
                foreach (var i in listSub)
                {
                    if (i.DIC_EXP_MEST_REASON != null)
                    {
                        if (rdo.DIC_EXP_MEST_REASON == null)
                        {
                            rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_MEST_REASON)
                        {
                            if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                            }
                        }

                    }
                }

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT") || field.Name.Contains("_MONEY"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                    }
                }
                if (!hide) ListRdo.Add(rdo);
            }
        }

        private Mrs00268RDO IsMeaningful(List<Mrs00268RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00268RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

            listParentRdo = ListRdo.GroupBy(o => o.PARENT_NAME).Select(p => p.First()).ToList();
            dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == castFilter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
            objectTag.AddObjectData(store, "ParentServices", listParentRdo.OrderBy(o=>o.PARENT_NAME).ToList());
            objectTag.AddObjectData(store, "Services", ListRdo.OrderBy(o => o.SERVICE_NAME).ToList());
            objectTag.AddRelationship(store, "ParentServices", "Services", "PARENT_NAME", "PARENT_NAME");
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        private void GetMestMate(long MediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MATERIAL> impMestMaterials, ref List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials)
        {
            //Nhap vat tu
            GetImpMestMate(MediStockId, timeFrom, timeTo, ref impMestMaterials);
            //Xuat vat tu
            GetExpMestMate(MediStockId, timeFrom, timeTo, ref expMestMaterials);
        }

        private void GetExpMestMate(long MediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
            expMestFilter.MEDI_STOCK_ID = MediStockId;
            expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
            expMestFilter.FINISH_TIME_FROM = timeFrom;
            expMestFilter.FINISH_TIME_TO = timeTo;
            var listExpMest = new HisExpMestManager(paramGet).Get(expMestFilter) ?? new List<HIS_EXP_MEST>();
            var listExpMestId = listExpMest.Select(o => o.ID).Distinct().ToList();
            if (listExpMestId != null && listExpMestId.Count > 0)
            {
                var skip = 0;

                while (listExpMestId.Count - skip > 0)
                {
                    var limit = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMaterialViewFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
                    ExpMestMaterialfilter.EXP_MEST_IDs = limit;
                    ExpMestMaterialfilter.IS_EXPORT = true;
                    var ExpMestMaterialSub = new HisExpMestMaterialManager().GetView(ExpMestMaterialfilter);
                    expMestMaterials.AddRange(ExpMestMaterialSub);
                }
            }

        }

        private void GetImpMestMate(long MediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MATERIAL> impMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
            impMestFilter.MEDI_STOCK_ID = MediStockId;
            impMestFilter.IMP_TIME_FROM = timeFrom;
            impMestFilter.IMP_TIME_TO = timeTo;
            impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            var listImpMest = new HisImpMestManager(paramGet).Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
            var listImpMestId = listImpMest.Select(o => o.ID).Distinct().ToList();
            if (listImpMestId != null && listImpMestId.Count > 0)
            {
                var skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var limit = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery ImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery();
                    ImpMestMaterialfilter.IMP_MEST_IDs = limit;
                    var ImpMestMaterialSub = new HisImpMestMaterialManager().GetView(ImpMestMaterialfilter);
                    impMestMaterials.AddRange(ImpMestMaterialSub);
                }
            }

        }
    }
}
