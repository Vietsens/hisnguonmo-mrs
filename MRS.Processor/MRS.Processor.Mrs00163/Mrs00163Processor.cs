using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using AutoMapper;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00163
{
    class Mrs00163Processor : AbstractProcessor
    {
        Mrs00163Filter castFilter = null;

        List<Mrs00163RDO> ListMedicineRdo = new List<Mrs00163RDO>();
        List<Mrs00163RDO> ListMaterialRdo = new List<Mrs00163RDO>();

        List<V_HIS_MEDICINE_TYPE> hisMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> hisMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();

        DateTime currentDate;

        public Mrs00163Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00163Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                //cần xem lại cách lấy dữ liệu
                CommonParam paramGet = new CommonParam();
                castFilter = (Mrs00163Filter)this.reportFilter;

                currentDate = DateTime.Now;

                HisMedicineBeanViewFilterQuery mediBeanFilter = new HisMedicineBeanViewFilterQuery();
                mediBeanFilter.IN_STOCK = 0;
                mediBeanFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;

                HisMaterialBeanViewFilterQuery mateBeanFilter = new HisMaterialBeanViewFilterQuery();
                mateBeanFilter.IN_STOCK = 0;
                mateBeanFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                hisMedicineTypes = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());

                hisMaterialTypes = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());

                //HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery();
                //expMestFilter.FINISH_TIME_TO = Convert.ToInt64(currentDate.ToString("yyyyMMddHHmmss"));
                //expMestFilter.EXP_MEST_STT_IDs = new List<long>();
                //expMestFilter.EXP_MEST_TYPE_IDs = new List<long>();
                //expMestFilter.EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK);
                //expMestFilter.EXP_MEST_STT_IDs.AddRange(new List<long>
                //{
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__EXECUTE,
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DRAFT,
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REJECT,
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REQUEST
                //});

                List<V_HIS_MEDICINE_BEAN> ListMedicineBean = new MOS.MANAGER.HisMedicineBean.HisMedicineBeanManager(paramGet).GetView(mediBeanFilter);

                List<V_HIS_MATERIAL_BEAN> ListMaterialBean = new MOS.MANAGER.HisMaterialBean.HisMaterialBeanManager(paramGet).GetView(mateBeanFilter);

                //List<V_HIS_EXP_MEST> ListExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestFilter);

                ProcessListMedicineBean(ListMedicineBean);//, ListExpMest);
                ProcessListMaterialBean(ListMaterialBean);//, ListExpMest);
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
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            #region Cac the Single
            if (currentDate != null)
            {
                dicSingleTag.Add("CREATE_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Convert.ToInt64(currentDate.ToString("yyyyMMddHHmmss"))));
            }
            #endregion
            objectTag.AddObjectData(store, "Medicines", ListMedicineRdo);
            objectTag.AddObjectData(store, "Materials", ListMaterialRdo);
        }

        private void ProcessListMedicineBean(List<V_HIS_MEDICINE_BEAN> ListMedicineBean)//, List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                if (IsNotNullOrEmpty(ListMedicineBean))
                {
                    ListMedicineRdo = (from r in ListMedicineBean select new Mrs00163RDO(r, ref hisMedicineTypes)).ToList();
                }
                //if (IsNotNullOrEmpty(ListExpMest))
                //{
                //    ProcessListMediExpMest(paramGet, ListExpMest);
                //}
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00163.");
                }
                if (IsNotNullOrEmpty(ListMedicineRdo))
                {
                    List<Mrs00163RDO> ListRdo = new List<Mrs00163RDO>();
                    var Group = ListMedicineRdo.GroupBy(g => new { g.MEDICINE_TYPE_ID,g.MEDI_STOCK_ID, g.IMP_PRICE, g.PACKAGE_NUMBER, g.EXPIRED_DATE }).ToList();
                    foreach (var group in Group)
                    {
                        var listSubRdo = group.ToList<Mrs00163RDO>();
                        if (IsNotNullOrEmpty(listSubRdo))
                        {
                            Mrs00163RDO rdo = new Mrs00163RDO();
                            rdo.MEDICINE_TYPE_ID = listSubRdo.First().MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_CODE = listSubRdo.First().MEDICINE_TYPE_CODE;
                            rdo.MEDI_STOCK_ID = listSubRdo.First().MEDI_STOCK_ID;
                            rdo.MEDI_STOCK_NAME = listSubRdo.First().MEDI_STOCK_NAME;
                            rdo.MEDICINE_TYPE_NAME = listSubRdo.First().MEDICINE_TYPE_NAME;
                            rdo.IMP_PRICE = listSubRdo.First().IMP_PRICE;
                            rdo.PACKAGE_NUMBER = listSubRdo.First().PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE = listSubRdo.First().EXPIRED_DATE;
                            rdo.EXPIRED_DATE_STR = listSubRdo.First().EXPIRED_DATE_STR;
                            rdo.SERVICE_UNIT_NAME = listSubRdo.First().SERVICE_UNIT_NAME;
                            rdo.ACTIVE_INGR_BHYT_NAME = listSubRdo.First().ACTIVE_INGR_BHYT_NAME;
                            rdo.NUM_ORDER = listSubRdo.First().NUM_ORDER;
                            rdo.TOTAL_AMOUNT = listSubRdo.Sum(o => o.TOTAL_AMOUNT);
                            rdo.MEDI_STOCK_AMOUNT = "Tổng tồn:" + rdo.TOTAL_AMOUNT;
                            var GroupMediStock = listSubRdo.Where(o => o.MEDI_STOCK_ID != null).GroupBy(g => g.MEDI_STOCK_ID);
                            foreach (var item in GroupMediStock)
                            {
                                var listSubMedi = item.ToList<Mrs00163RDO>();
                                if (IsNotNullOrEmpty(listSubMedi))
                                {
                                    rdo.MEDI_STOCK_AMOUNT = rdo.MEDI_STOCK_AMOUNT + ", " + listSubMedi.First().MEDI_STOCK_NAME + ":" + listSubMedi.Sum(s => s.TOTAL_AMOUNT);
                                }
                            }
                            ListRdo.Add(rdo);
                        }
                    }
                    ListMedicineRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t => t.MEDICINE_TYPE_NAME).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListMedicineRdo.Clear();
                ListMaterialRdo.Clear();
            }
        }

        //private void ProcessListMediExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> ListExpMest)
        //{
        //    try
        //    {
        //        if (IsNotNullOrEmpty(ListExpMest))
        //        {
        //            int start = 0;
        //            int count = ListExpMest.Count;
        //            while (count > 0)
        //            {
        //                int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
        //                expMediFilter.EXP_MEST_IDs = ListExpMest.Skip(start).Take(limit).Select(s => s.ID).ToList();
        //                //expMediFilter.IS_EXPORT = true;
        //                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
        //                if (IsNotNullOrEmpty(hisExpMestMedicine))
        //                {
        //                    HisMedicineBeanViewFilterQuery mediBeanFilter = new HisMedicineBeanViewFilterQuery();
        //                    mediBeanFilter.MEDICINE_IDs = hisExpMestMedicine.Select(s => s.MEDICINE_ID).Distinct().ToList();
        //                    var hisMedicineBeans = new MOS.MANAGER.HisMedicineBean.HisMedicineBeanManager(paramGet).GetView(mediBeanFilter);
        //                    if (IsNotNullOrEmpty(hisMedicineBeans))
        //                    {
        //                        ListMedicineRdo.AddRange((from r in hisMedicineBeans select new Mrs00163RDO(r, ref hisMedicineTypes)).ToList());
        //                    }

        //                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //    }
        //}

        private void ProcessListMaterialBean(List<V_HIS_MATERIAL_BEAN> ListMaterialBean)//, List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                if (IsNotNullOrEmpty(ListMaterialBean))
                {
                    ListMaterialRdo = (from r in ListMaterialBean select new Mrs00163RDO(r, ref hisMaterialTypes)).ToList();
                }
                //if (IsNotNullOrEmpty(ListExpMest))
                //{
                //    ProcessListMateExpMest(paramGet, ListExpMest);
                //}

                if (param.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00163.");
                }

                if (IsNotNullOrEmpty(ListMaterialRdo))
                {
                    List<Mrs00163RDO> ListRdo = new List<Mrs00163RDO>();
                    var Group = ListMaterialRdo.GroupBy(g => new { g.MATERIAL_TYPE_ID,g.MEDI_STOCK_ID, g.IMP_PRICE, g.PACKAGE_NUMBER, g.EXPIRED_DATE }).ToList();
                    foreach (var group in Group)
                    {
                        var listSubRdo = group.ToList<Mrs00163RDO>();
                        if (IsNotNullOrEmpty(listSubRdo))
                        {
                            Mrs00163RDO rdo = new Mrs00163RDO();
                            rdo.MATERIAL_TYPE_ID = listSubRdo.First().MATERIAL_TYPE_ID;
                            rdo.MATERIAL_TYPE_CODE = listSubRdo.First().MATERIAL_TYPE_CODE;
                            rdo.MEDI_STOCK_ID = listSubRdo.First().MEDI_STOCK_ID;
                            rdo.MEDI_STOCK_NAME = listSubRdo.First().MEDI_STOCK_NAME;
                            rdo.MATERIAL_TYPE_NAME = listSubRdo.First().MATERIAL_TYPE_NAME;
                            rdo.IMP_PRICE = listSubRdo.First().IMP_PRICE;
                            rdo.PACKAGE_NUMBER = listSubRdo.First().PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE = listSubRdo.First().EXPIRED_DATE;
                            rdo.EXPIRED_DATE_STR = listSubRdo.First().EXPIRED_DATE_STR;
                            rdo.SERVICE_UNIT_NAME = listSubRdo.First().SERVICE_UNIT_NAME;
                            //rdo.ACTIVE_INGR_BHYT_NAME = listSubRdo.First().ACTIVE_INGR_BHYT_NAME; 
                            rdo.NUM_ORDER = listSubRdo.First().NUM_ORDER;
                            rdo.TOTAL_AMOUNT = listSubRdo.Sum(o => o.TOTAL_AMOUNT);
                            rdo.MEDI_STOCK_AMOUNT = "Tổng tồn:" + rdo.TOTAL_AMOUNT;
                            var GroupMediStock = listSubRdo.Where(o => o.MEDI_STOCK_ID != null).GroupBy(g => g.MEDI_STOCK_ID);
                            foreach (var item in GroupMediStock)
                            {
                                var listSubMedi = item.ToList<Mrs00163RDO>();
                                if (IsNotNullOrEmpty(listSubMedi))
                                {
                                    rdo.MEDI_STOCK_AMOUNT = rdo.MEDI_STOCK_AMOUNT + ", " + listSubMedi.First().MEDI_STOCK_NAME + ":" + listSubMedi.Sum(s => s.TOTAL_AMOUNT);
                                }
                            }
                            ListRdo.Add(rdo);
                        }
                    }
                    ListMaterialRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t => t.MATERIAL_TYPE_NAME).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListMedicineRdo.Clear();
                ListMaterialRdo.Clear();
            }
        }

        //private void ProcessListMateExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> ListExpMest)
        //{
        //    try
        //    {
        //        if (IsNotNullOrEmpty(ListExpMest))
        //        {
        //            int start = 0;
        //            int count = ListExpMest.Count;
        //            while (count > 0)
        //            {
        //                int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
        //                expMateFilter.EXP_MEST_IDs = ListExpMest.Skip(start).Take(limit).Select(s => s.ID).ToList();
        //                //expMateFilter.IS_EXPORT = true;
        //                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
        //                if (IsNotNullOrEmpty(hisExpMestMaterial))
        //                {
        //                    HisMaterialBeanViewFilterQuery mateBeanFilter = new HisMaterialBeanViewFilterQuery();
        //                    mateBeanFilter.MATERIAL_IDs = hisExpMestMaterial.Select(s => s.MATERIAL_ID).Distinct().ToList();
        //                    var hisMaterialBeans = new MOS.MANAGER.HisMaterialBean.HisMaterialBeanManager(paramGet).GetView(mateBeanFilter);
        //                    if (IsNotNullOrEmpty(hisMaterialBeans))
        //                    {
        //                        ListMaterialRdo.AddRange((from r in hisMaterialBeans select new Mrs00163RDO(r, ref hisMaterialTypes)).ToList());
        //                    }

        //                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //    }
        //}
    }
}
