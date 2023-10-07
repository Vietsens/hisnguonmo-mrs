using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMestPeriodBlood;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisBlood;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
//using MOS.MANAGER.HisDeath; 
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisIcdGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
//using MOS.MANAGER.HisExamServiceReq; 
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisMaterialBean;
using FlexCel.Report;

using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisBloodType;

namespace MRS.Processor.Mrs00259
{
    public class Mrs00259Processor : AbstractProcessor
    {
        private Mrs00259Filter filter;
        List<Mrs00259RDO> listRdo = new List<Mrs00259RDO>();

        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>();
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();
        List<V_HIS_BLOOD_TYPE> listBloodTypes = new List<V_HIS_BLOOD_TYPE>();

        CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
        public Mrs00259Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00259Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            filter = (Mrs00259Filter)reportFilter;
            try
            {
                HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
                impMestBloodViewFilter.IMP_TIME_FROM = filter.TIME_FROM;
                impMestBloodViewFilter.IMP_TIME_TO = filter.TIME_TO;
                impMestBloodViewFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                impMestBloodViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                listImpMestBloods = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter);

                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.EXP_TIME_FROM = filter.TIME_FROM;
                expMestBloodViewFilter.EXP_TIME_TO = filter.TIME_TO;
                expMestBloodViewFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                expMestBloodViewFilter.IS_EXPORT = true;
                listExpMestBloods = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter);
                HisBloodTypeViewFilterQuery HisBloodTypefilter = new HisBloodTypeViewFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listBloodTypes = new HisBloodTypeManager().GetView(HisBloodTypefilter);
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
            var result = true;
            try
            {
                ProcessBegin();

                if (IsNotNullOrEmpty(listImpMestBloods))
                {
                    foreach (var blood in listImpMestBloods)
                    {
                        var bloodType = listBloodTypes.FirstOrDefault(o => o.ID == blood.BLOOD_TYPE_ID) ?? new V_HIS_BLOOD_TYPE();
                        var rdo = new Mrs00259RDO();
                        rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                        rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                        rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;
                        rdo.BLOOD_CODE = blood.BLOOD_CODE;

                        rdo.VOLUME = blood.VOLUME;
                        rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;
                        rdo.IMP_PRICE = blood.IMP_PRICE;
                        rdo.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                        rdo.IMP_AMOUNT = 1;
                        if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC) rdo.IMP_MANU = 1;
                        else if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK) rdo.IMP_CHMS = 1;
                        else rdo.IMP_OTHER = 1;

                        if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL)
                        {
                            rdo.IMP_MOBA_PRES = 1;
                        }

                        listRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestBloods))
                {
                    foreach (var blood in listExpMestBloods)
                    {
                        var bloodType = listBloodTypes.FirstOrDefault(o => o.ID == blood.BLOOD_TYPE_ID) ?? new V_HIS_BLOOD_TYPE();
                        var rdo = new Mrs00259RDO();
                        rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                        rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                        rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;
                        rdo.BLOOD_CODE = blood.BLOOD_CODE;

                        rdo.VOLUME = blood.VOLUME;
                        rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;
                        rdo.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = blood.IMP_PRICE;

                        rdo.EXP_AMOUNT = 1;
                        if ((blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK || blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM))
                        {
                            rdo.EXP_PRES = 1;
                        }
                        else if (blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK)
                        {
                            rdo.EXP_CHMS = 1;
                        }
                        else if (blood.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC)
                        {
                            rdo.EXP_MANU = 1;
                        }
                        else
                        {
                            rdo.EXP_OTHER = 1;
                        }

                        listRdo.Add(rdo);
                    }
                }

                OptimizeList(ref listRdo);

                listRdo = listRdo.Where(s => s.BEGIN_AMOUNT != 0 || s.IMP_AMOUNT != 0 || s.END_AMOUNT != 0).ToList();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected void ProcessBegin()
        {
            HisMediStockPeriodFilterQuery mediStockPeriodFilter = new HisMediStockPeriodFilterQuery();
            mediStockPeriodFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
            mediStockPeriodFilter.TO_TIME_TO = filter.TIME_FROM - 1;
            var listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodFilter);
            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                HisMestPeriodBloodFilterQuery mestPeriodBloodFilter = new HisMestPeriodBloodFilterQuery();
                mestPeriodBloodFilter.MEDI_STOCK_PERIOD_ID = listMediStockPeriods.OrderBy(o => o.TO_TIME).Last().ID;
                var listMestPeriodBloods = new MOS.MANAGER.HisMestPeriodBlood.HisMestPeriodBloodManager(param).Get(mestPeriodBloodFilter);

                if (IsNotNullOrEmpty(listMestPeriodBloods))
                {
                    var listBloodIds = listMestPeriodBloods.Select(s => s.BLOOD_ID).ToList();
                    var skip = 0;
                    var listBloods = new List<V_HIS_BLOOD>();
                    while (listBloodIds.Count - skip > 0)
                    {
                        var listIds = listBloodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisBloodViewFilterQuery bloodFilter = new HisBloodViewFilterQuery();
                        bloodFilter.IDs = listIds;
                        listBloods.AddRange(new MOS.MANAGER.HisBlood.HisBloodManager(param).GetView(bloodFilter));
                    }

                    if (IsNotNullOrEmpty(listBloods))
                    {
                        foreach (var blood in listMestPeriodBloods)
                        {
                            var bloods = listBloods.Where(s => s.ID == blood.BLOOD_ID).ToList();
                            var bloodType = listBloodTypes.FirstOrDefault(o => bloods.Exists(p=>p.BLOOD_TYPE_ID==o.ID)) ?? new V_HIS_BLOOD_TYPE();
                            if (IsNotNullOrEmpty(bloods))
                            {
                                var rdo = new Mrs00259RDO();
                                rdo.BLOOD_TYPE_ID = bloods.First().ID;
                                rdo.BLOOD_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE;
                                rdo.BLOOD_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME;
                                rdo.BLOOD_CODE = bloods.First().BLOOD_CODE;

                                rdo.VOLUME = bloods.First().VOLUME;
                                rdo.BLOOD_ABO_CODE = bloods.First().BLOOD_ABO_CODE;
                                rdo.IMP_PRICE = bloods.First().IMP_PRICE;
                                rdo.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                                rdo.BEGIN_AMOUNT = 1;

                                listRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
           
            HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
            impMestBloodViewFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
            if (IsNotNullOrEmpty(listMediStockPeriods))
                impMestBloodViewFilter.IMP_TIME_FROM = listMediStockPeriods.OrderBy(o => o.TO_TIME).Last().TO_TIME;
            impMestBloodViewFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
            impMestBloodViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            var listImpMestOutTimes = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter);

            if (IsNotNullOrEmpty(listImpMestOutTimes))
            {
                foreach (var blood in listImpMestOutTimes)
                {
                    var rdo = new Mrs00259RDO();
                    var bloodType = listBloodTypes.FirstOrDefault(o => blood.BLOOD_TYPE_ID == o.ID) ?? new V_HIS_BLOOD_TYPE();
                    rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                    rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                    rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;
                    rdo.BLOOD_CODE = blood.BLOOD_CODE;

                    rdo.VOLUME = blood.VOLUME;
                    rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;
                    rdo.IMP_PRICE = blood.IMP_PRICE;
                    rdo.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                    rdo.BEGIN_AMOUNT = 1;

                    listRdo.Add(rdo);
                }
            }

            HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
            if (IsNotNullOrEmpty(listMediStockPeriods))
                expMestBloodViewFilter.EXP_TIME_FROM = listMediStockPeriods.OrderBy(o => o.TO_TIME).Last().TO_TIME;
            expMestBloodViewFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
            expMestBloodViewFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
            expMestBloodViewFilter.IS_EXPORT = true;
            var listExpMestOutTimes = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter);


            if (IsNotNullOrEmpty(listExpMestOutTimes))
            {
                foreach (var blood in listExpMestOutTimes)
                {
                    var rdo = new Mrs00259RDO();
                    var bloodType = listBloodTypes.FirstOrDefault(o => blood.BLOOD_TYPE_ID == o.ID) ?? new V_HIS_BLOOD_TYPE();
                    rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                    rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                    rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;
                    rdo.BLOOD_CODE = blood.BLOOD_CODE;

                    rdo.VOLUME = blood.VOLUME;
                    rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;
                    rdo.IMP_PRICE = blood.IMP_PRICE;

                    rdo.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                    rdo.BEGIN_AMOUNT = -1;

                    listRdo.Add(rdo);
                }
            }

            OptimizeList(ref listRdo);
        }

        protected void OptimizeList(ref List<Mrs00259RDO> _listRdo)
        {
            string KeyGroupInv = "{0}_{1}_{2}";
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_INV") && this.dicDataFilter["KEY_GROUP_INV"] != null)
            {
                KeyGroupInv = this.dicDataFilter["KEY_GROUP_INV"].ToString();
            }
            if (IsNotNullOrEmpty(_listRdo))
                _listRdo = _listRdo.GroupBy(g => string.Format(KeyGroupInv,g.BLOOD_TYPE_ID, g.BLOOD_ABO_CODE, g.IMP_PRICE,g.BLOOD_CODE )).Select(s => new Mrs00259RDO
                {
                    BLOOD_TYPE_ID = s.First().BLOOD_TYPE_ID,
                    BLOOD_TYPE_CODE = s.First().BLOOD_TYPE_CODE,
                    BLOOD_TYPE_NAME = s.First().BLOOD_TYPE_NAME,
                    BLOOD_CODE = s.First().BLOOD_CODE,

                    VOLUME = s.First().VOLUME,
                    BLOOD_ABO_CODE = s.First().BLOOD_ABO_CODE,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,

                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT),

                    IMP_AMOUNT = s.Sum(su => su.IMP_AMOUNT),
                    IMP_MANU = s.Sum(su => su.IMP_MANU),
                    IMP_CHMS = s.Sum(su => su.IMP_CHMS),
                    IMP_OTHER = s.Sum(su => su.IMP_OTHER),
                    IMP_MOBA_PRES = s.Sum(su => su.IMP_MOBA_PRES),

                    EXP_AMOUNT = s.Sum(su => su.EXP_AMOUNT),
                    EXP_PRES = s.Sum(su => su.EXP_PRES),
                    EXP_CHMS = s.Sum(su => su.EXP_CHMS),
                    EXP_MANU = s.Sum(su => su.EXP_MANU),
                    EXP_OTHER = s.Sum(su => su.EXP_OTHER),

                    END_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) + s.Sum(su => su.IMP_AMOUNT) - s.Sum(su => su.EXP_AMOUNT)
                }).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                HisMediStockFilterQuery FilterHisMediStock = new HisMediStockFilterQuery();
                FilterHisMediStock.ID = filter.MEDI_STOCK_ID;
                var ListHisMediStock = new HisMediStockManager(paramGet).Get(FilterHisMediStock);
                if (IsNotNullOrEmpty(ListHisMediStock)) dicSingleTag.Add("MEDI_STOCK_NAME", ListHisMediStock.First().MEDI_STOCK_NAME);

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));

                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "BloodType", listBloodTypes);
                //objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
