using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMestType;
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
using MOS.MANAGER.HisMaterialBean;
using FlexCel.Report;

using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicineBean;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMedi;

namespace MRS.Processor.Mrs00267
{
    public class Mrs00267Processor : AbstractProcessor
    {
        private Mrs00267Filter castFilter;
        List<Mrs00267RDO> listRdo = new List<Mrs00267RDO>();
        List<Mrs00267RDO> listParentRdo = new List<Mrs00267RDO>();
        ///List<V_HIS_EXAM_SERVICE_REQ> listTemp = new List<V_HIS_EXAM_SERVICE_REQ>(); 
        List<V_HIS_MEDICINE_BEAN> ListMedicineBean = new List<V_HIS_MEDICINE_BEAN>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        private List<HIS_MEDI_STOCK> ListHisMediStock = new List<HIS_MEDI_STOCK>();
        CommonParam paramGet = new CommonParam(); 

        List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedi = new List<V_HIS_MEST_PERIOD_MEDI>(); // DS thuoc chot ki
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineBefore = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap truoc ki
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineBefore = new List<V_HIS_EXP_MEST_MEDICINE>(); // DS thuoc xuat truoc ki

        List<ExpMestIdReason> ExpMestIdReasons = new List<ExpMestIdReason>(); // DS phieu xuat va li do xuat trong ky

        public Mrs00267Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00267Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            HisMediStockFilterQuery FilterHisMediStock = new HisMediStockFilterQuery()
            {
                ID = castFilter.MEDI_STOCK_ID
            };
            ListHisMediStock = new HisMediStockManager(paramGet).Get(FilterHisMediStock);
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

            listParentRdo = listRdo.GroupBy(o => o.PARENT_NAME).Select(p => p.First()).ToList();
            if (IsNotNullOrEmpty(ListHisMediStock)) dicSingleTag.Add("MEDI_STOCK_NAME", ListHisMediStock.First().MEDI_STOCK_NAME);
            objectTag.AddObjectData(store, "ParentServices", listParentRdo.OrderBy(o => o.PARENT_NAME).ToList());
            objectTag.AddObjectData(store, "Services", listRdo.OrderBy(o => o.SERVICE_NAME).ToList());
            objectTag.AddRelationship(store, "ParentServices", "Services", "PARENT_NAME", "PARENT_NAME");
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00267Filter)reportFilter;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00267: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

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
                    HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new HisMestPeriodMediViewFilterQuery();
                    mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = listPeriod.First().ID;
                    listMestPeriodMedi.AddRange(new HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter) ?? new List<V_HIS_MEST_PERIOD_MEDI>());
                    #endregion

                    #region Du lieu tu sau chot ki den truoc ki bao cao
                    GetMestMedi(castFilter.MEDI_STOCK_ID, listPeriod.First().CREATE_TIME, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listExpMestMedicineBefore);
                    #endregion
                }
                else
                {
                    #region Du lieu truoc ki bao cao
                    GetMestMedi(castFilter.MEDI_STOCK_ID, null, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listExpMestMedicineBefore);
                    #endregion
                }

                #region Du lieu trong ki bao cao
                GetMestMedi(castFilter.MEDI_STOCK_ID, castFilter.TIME_FROM, castFilter.TIME_TO, ref ListImpMestMedicine, ref ListExpMestMedicine);
                #endregion

                #region loai vat tu
                ListMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
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
                listRdo.Clear();
                var listSub = (from r in listMestPeriodMedi select new Mrs00267RDO(r, ListMedicineType)).ToList();
                listRdo.AddRange(listSub);
                listSub = (from r in listImpMestMedicineBefore select new Mrs00267RDO(r, ListMedicineType, true)).ToList();
                listRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineBefore select new Mrs00267RDO(r, ListMedicineType,ExpMestIdReasons, true)).ToList();
                listRdo.AddRange(listSub);
                listSub = (from r in ListImpMestMedicine select new Mrs00267RDO(r, ListMedicineType, false)).ToList();
                listRdo.AddRange(listSub);
                listSub = (from r in ListExpMestMedicine select new Mrs00267RDO(r, ListMedicineType, ExpMestIdReasons, false)).ToList();
                listRdo.AddRange(listSub);

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
            var group = listRdo.GroupBy(o => new { o.SERVICE_CODE, o.IMP_PRICE }).ToList();
            listRdo.Clear();
            Decimal sum = 0;
            Mrs00267RDO rdo;
            List<Mrs00267RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00267RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00267RDO();
                listSub = item.ToList<Mrs00267RDO>();
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
                if (!hide) listRdo.Add(rdo);
            }
        }

        private Mrs00267RDO IsMeaningful(List<Mrs00267RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00267RDO();
        }

        private void GetMestMedi(long? MediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines, ref List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines)
        {
            //Nhap vat tu
            GetImpMestMedi(MediStockId, timeFrom, timeTo, ref impMestMedicines);
            //Xuat vat tu
            GetExpMestMedi(MediStockId, timeFrom, timeTo, ref expMestMedicines);
        }

        private void GetExpMestMedi(long? MediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines)
        {
            try
            {
                HisExpMestMedicineViewFilterQuery ExpMestMaterialfilter = new HisExpMestMedicineViewFilterQuery();
                ExpMestMaterialfilter.MEDI_STOCK_ID = MediStockId;
                ExpMestMaterialfilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                ExpMestMaterialfilter.EXP_TIME_FROM = timeFrom;
                ExpMestMaterialfilter.EXP_TIME_TO = timeTo;
                ExpMestMaterialfilter.IS_EXPORT = true;
                expMestMedicines = new HisExpMestMedicineManager().GetView(ExpMestMaterialfilter);
            }
            catch (Exception ex)
            {
                expMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetImpMestMedi(long? MediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines)
        {
            try
            {
                HisImpMestMedicineViewFilterQuery ImpMestMaterialfilter = new HisImpMestMedicineViewFilterQuery();
                ImpMestMaterialfilter.MEDI_STOCK_ID = MediStockId;
                ImpMestMaterialfilter.IMP_TIME_FROM = timeFrom;
                ImpMestMaterialfilter.IMP_TIME_TO = timeTo;
                ImpMestMaterialfilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impMestMedicines = new HisImpMestMedicineManager().GetView(ImpMestMaterialfilter);
            }
            catch (Exception ex)
            {
                impMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
