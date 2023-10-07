using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisMestPeriodBlood;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisBloodType;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;

namespace MRS.Processor.Mrs00323
{
    class Mrs00323Processor : AbstractProcessor
    {
        Mrs00323Filter castFilter = null;
        List<Mrs00323RDO> ListRdo = new List<Mrs00323RDO>();
        List<Mrs00323RDO> listRdoBloodABO = new List<Mrs00323RDO>();

        List<HIS_MEDI_STOCK_PERIOD> listMediStockPeriods = new List<HIS_MEDI_STOCK_PERIOD>();
        List<HIS_MEST_PERIOD_BLOOD> listMestPeriodBloods = new List<HIS_MEST_PERIOD_BLOOD>();

        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloodInTimes = new List<V_HIS_IMP_MEST_BLOOD>();
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloodInTimes = new List<V_HIS_EXP_MEST_BLOOD>();

        List<V_HIS_IMP_MEST> listMobaImpMests = new List<V_HIS_IMP_MEST>();

        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();

        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();
        string MEDI_STOCK_NAME = "";

        public Mrs00323Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00323Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00323Filter)this.reportFilter;

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                var mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);
                if (IsNotNullOrEmpty(mediStock))
                    MEDI_STOCK_NAME = mediStock.First().MEDI_STOCK_NAME;
                //Tao loai nhap xuat
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
                //Danh sach loai SL nhap, loai SL xuat
                PropertyInfo[] piAmount = Properties.Get<Mrs00323RDO>();

                foreach (var item in piAmount)
                {
                    if (dicImpMestType.ContainsKey(item.Name))
                    {
                        if (!dicImpAmountType.ContainsKey(dicImpMestType[item.Name])) dicImpAmountType[dicImpMestType[item.Name]] = item;
                    }
                    else if (dicExpMestType.ContainsKey(item.Name))
                    {
                        if (!dicExpAmountType.ContainsKey(dicExpMestType[item.Name])) dicExpAmountType[dicExpMestType[item.Name]] = item;
                    }
                }

                //HisMediStockPeriodFilterQuery mediStockPeriodFilter = new HisMediStockPeriodFilterQuery(); 
                //mediStockPeriodFilter.CREATE_TIME_TO = castFilter.TIME_FROM - 1; 
                //mediStockPeriodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodFilter); 

                #region chưa có API
                //if (IsNotNullOrEmpty(listMediStockPeriods))
                //{
                //    #region có kiểm kê
                //    var mediStockPeriod = listMediStockPeriods.OrderByDescending(o => o.CREATE_TIME).First(); 

                //    HisMestPeriodBloodFilterQuery mestPeriodBloodFilter = new HisMestPeriodBloodFilterQuery(); 
                //    mestPeriodBloodFilter.MEDI_STOCK_PERIOD_ID = mediStockPeriod.ID; 
                //    listMestPeriodBloods = new MOS.MANAGER.HisMestPeriodBlood.HisMestPeriodBloodManager(param).Get(mestPeriodBloodFilter); 

                //    var skip = 0; 
                //    var listBloods = new List<V_HIS_BLOOD>(); 
                //    while (listMestPeriodBloods.Count - skip > 0)
                //    {
                //        var listIds = listMestPeriodBloods.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                //        HisBloodViewFilterQuery bloodViewFilter = new HisBloodViewFilterQuery(); 
                //        bloodViewFilter.IDs = listIds.Select(s => s.BLOOD_ID).ToList(); 
                //        listBloods.AddRange(new MOS.MANAGER.HisBlood.HisBloodManager(param).GetView(bloodViewFilter); 
                //    }

                //    skip = 0; 
                //    var listBloodTypes = new List<V_HIS_BLOOD_TYPE>(); 
                //    while (listBloods.Count - skip > 0)
                //    {
                //        var listIds = listBloods.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                //        HisBloodTypeViewFilterQuery bloodTypeViewFilter = new HisBloodTypeViewFilterQuery(); 
                //        bloodTypeViewFilter.IDs = listIds.Select(s => s.BLOOD_TYPE_ID).ToList(); 
                //        listBloodTypes.AddRange(new MOS.MANAGER.HisBloodType.HisBloodTypeManager(param).GetView(bloodTypeViewFilter); 
                //    }

                //    if (IsNotNullOrEmpty(listMestPeriodBloods))
                //    {
                //        foreach (var blood in listMestPeriodBloods)
                //        {
                //            var bloods = listBloods.Where(w => w.ID == blood.BLOOD_ID).ToList(); 
                //            if (IsNotNullOrEmpty(bloods))
                //            {
                //                var bloodType = listBloodTypes.Where(s => s.ID == bloods.First().BLOOD_TYPE_ID).ToList(); 
                //                if (IsNotNullOrEmpty(bloodType))
                //                {
                //                    var rdo = new Mrs00323RDO(); 
                //                    rdo.BLOOD_ABO_CODE = bloods.First().BLOOD_ABO_CODE; 

                //                    rdo.BLOOD_TYPE_ID = bloods.First().BLOOD_TYPE_ID; 
                //                    rdo.BLOOD_TYPE_CODE = bloods.First().BLOOD_TYPE_CODE; 
                //                    rdo.BLOOD_TYPE_NAME = bloods.First().BLOOD_TYPE_NAME; 

                //                    rdo.BLOOD_ID = blood.BLOOD_ID; 

                //                    rdo.IMP_PRICE = bloods.First().IMP_PRICE; 

                //                    rdo.SERVICE_UNIT_NAME = bloodType.First().SERVICE_UNIT_NAME; 

                //                    rdo.VOLUME = bloodType.First().VOLUME; 

                //                    rdo.BEGIN_AMOUNT = 1; 

                //                    ListRdo.Add(rdo); 
                //                }
                //            }
                //        }
                //    }

                //    HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery(); 
                //    //impMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //    impMestBloodViewFilter.IMP_TIME_FROM = mediStockPeriod.CREATE_TIME; 
                //    impMestBloodViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1; 
                //    var listImpMestBloods = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter); 

                //    // here
                //    listImpMestBloods = listImpMestBloods.Where(s => s.MEDI_STOCK_ID == castFilter.MEDI_STOCK_ID).ToList(); 

                //    if (IsNotNullOrEmpty(listImpMestBloods))
                //    {
                //        foreach (var blood in listImpMestBloods)
                //        {
                //            var rdo = new Mrs00323RDO(); 
                //            rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE; 

                //            rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID; 
                //            rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE; 
                //            rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME; 

                //            rdo.BLOOD_ID = blood.BLOOD_ID; 

                //            rdo.IMP_PRICE = blood.IMP_PRICE; 

                //            rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME; 
                //            rdo.VOLUME = blood.VOLUME; 

                //            rdo.BEGIN_AMOUNT = 1; 

                //            ListRdo.Add(rdo); 
                //        }
                //    }

                //    HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery(); 
                //    //expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //    expMestBloodViewFilter.EXP_TIME_FROM = mediStockPeriod.CREATE_TIME; 
                //    expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1; 
                //    var listExpMestBloods = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter); 

                //    // here
                //    listExpMestBloods = listExpMestBloods.Where(s => s.MEDI_STOCK_ID == castFilter.MEDI_STOCK_ID).ToList(); 

                //    listExpMestBloods = listExpMestBloods.Where(w => w.IN_EXECUTE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_MEDICINE.IN_EXECUTE__TRUE).ToList(); 

                //    if (IsNotNullOrEmpty(listExpMestBloods))
                //    {
                //        foreach (var blood in listExpMestBloods)
                //        {
                //            var rdo = new Mrs00323RDO(); 
                //            rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE; 

                //            rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID; 
                //            rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE; 
                //            rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME; 

                //            rdo.BLOOD_ID = blood.BLOOD_ID; 

                //            rdo.IMP_PRICE = blood.IMP_PRICE; 

                //            rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME; 
                //            rdo.VOLUME = blood.VOLUME; 

                //            rdo.BEGIN_AMOUNT = -1; 

                //            ListRdo.Add(rdo); 
                //        }
                //    }

                //    ProcessOptimizeList(); 

                //    #endregion
                //}
                //else
                #endregion
                {
                    #region không có kiểm kê
                    //HisImpMestViewFilterQuery impMestViewFilterOutTimes = new HisImpMestViewFilterQuery();
                    //impMestViewFilterOutTimes.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    //impMestViewFilterOutTimes.IMP_MEST_STT_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT };
                    //impMestViewFilterOutTimes.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    //var listImpMestOuttimes = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilterOutTimes);
                    //var ssskip = 0;
                    //var listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>();
                    //while (listImpMestOuttimes.Count - ssskip > 0)
                    //{
                    //    var listIds = listImpMestOuttimes.Skip(ssskip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    ssskip = ssskip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
                    //    impMestBloodViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    //    listImpMestBloods.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter));
                    //}

                    HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
                    impMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impMestBloodViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    impMestBloodViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    var listImpMestBloods = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter);

                    if (IsNotNullOrEmpty(listImpMestBloods))
                    {
                        foreach (var blood in listImpMestBloods)
                        {
                            var rdo = new Mrs00323RDO();
                            rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;

                            rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                            rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                            rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;

                            rdo.BLOOD_ID = blood.BLOOD_ID;

                            rdo.IMP_PRICE = blood.IMP_PRICE;

                            rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME;
                            rdo.VOLUME = blood.VOLUME;

                            rdo.BEGIN_AMOUNT = 1;

                            ListRdo.Add(rdo);
                        }
                    }
                    var listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();

                    HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                    expMestBloodViewFilter.IS_EXPORT = true;
                    expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter) ?? new List<V_HIS_EXP_MEST_BLOOD>());
                    //HisExpMestViewFilterQuery expMestViewFilterOutTimes = new HisExpMestViewFilterQuery(); 
                    //expMestViewFilterOutTimes.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                    //expMestViewFilterOutTimes.EXP_TIME_TO = castFilter.TIME_FROM - 1; 
                    //expMestViewFilterOutTimes.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //var listExpMestOutTimes = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(expMestViewFilterOutTimes); 
                    //ssskip = 0; 
                    //while (listExpMestOutTimes.Count - ssskip > 0)
                    //{
                    //    var listIds = listExpMestOutTimes.Skip(ssskip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //    ssskip = ssskip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    //    HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery(); 
                    //    expMestBloodViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                    //    listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter); 
                    //}

                    //HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery(); 
                    ////expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //expMestBloodViewFilter.EXP_MEST_ID = castFilter.MEDI_STOCK_ID; 
                    //expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1; 
                    //var listExpMestBloods = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter); 

                    // here
                    //listExpMestBloods = listExpMestBloods.Where(s => s.MEDI_STOCK_ID == castFilter.MEDI_STOCK_ID).ToList(); 

                    //listExpMestBloods = listExpMestBloods.Where(w => w.IN_EXECUTE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_MEDICINE.IN_EXECUTE__TRUE).ToList(); 

                    if (IsNotNullOrEmpty(listExpMestBloods))
                    {
                        foreach (var blood in listExpMestBloods)
                        {
                            var rdo = new Mrs00323RDO();
                            rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;

                            rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                            rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                            rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;

                            rdo.BLOOD_ID = blood.BLOOD_ID;

                            rdo.IMP_PRICE = blood.IMP_PRICE;

                            rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME;
                            rdo.VOLUME = blood.VOLUME;

                            rdo.BEGIN_AMOUNT = -1;

                            ListRdo.Add(rdo);
                        }
                    }

                    ProcessOptimizeList();
                    #endregion
                }

                //HisImpMestViewFilterQuery impMestViewFilterInTime = new HisImpMestViewFilterQuery();
                //impMestViewFilterInTime.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                //impMestViewFilterInTime.IMP_TIME_FROM = castFilter.TIME_FROM;
                //impMestViewFilterInTime.IMP_TIME_TO = castFilter.TIME_TO;
                //impMestViewFilterInTime.IMP_MEST_STT_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT };
                //var listImpMestInTimes = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilterInTime);
                //var sskip = 0;
                //while (listImpMestInTimes.Count - sskip > 0)
                //{
                //    var listIds = listImpMestInTimes.Skip(sskip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    sskip = sskip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                //    HisImpMestBloodViewFilterQuery impMestBloodInTimeViewFilter = new HisImpMestBloodViewFilterQuery();
                //    impMestBloodInTimeViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                //    impMestBloodInTimeViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                //    listImpMestBloodInTimes.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodInTimeViewFilter));
                //}

                HisImpMestBloodViewFilterQuery impMestBloodInTimeViewFilter = new HisImpMestBloodViewFilterQuery();
                impMestBloodInTimeViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestBloodInTimeViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestBloodInTimeViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestBloodInTimeViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                listImpMestBloodInTimes = new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodInTimeViewFilter);

                var sskip = 0;
                while (listImpMestBloodInTimes.Count - sskip > 0)
                {
                    var listIds = listImpMestBloodInTimes.Skip(sskip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    sskip = sskip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                    mobaImpMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                    mobaImpMestViewFilter.IMP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
                    };
                    listMobaImpMests.AddRange(new HisImpMestManager(param).GetView(mobaImpMestViewFilter));
                }
                HisExpMestBloodViewFilterQuery expMestBloodInTimeViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodInTimeViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestBloodInTimeViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestBloodInTimeViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestBloodInTimeViewFilter.IS_EXPORT = true;
                listExpMestBloodInTimes.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodInTimeViewFilter) ?? new List<V_HIS_EXP_MEST_BLOOD>());

                //HisExpMestBloodViewFilterQuery expMestBloodInTimeViewFilter = new HisExpMestBloodViewFilterQuery(); 
                ////expMestBloodInTimeViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //expMestBloodInTimeViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                //expMestBloodInTimeViewFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                //listExpMestBloodInTimes = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodInTimeViewFilter); 

                // here
                //listExpMestBloodInTimes = listExpMestBloodInTimes.Where(s => s.MEDI_STOCK_ID == castFilter.MEDI_STOCK_ID).ToList(); 

                //listExpMestBloodInTimes = listExpMestBloodInTimes.Where(s => s.IN_EXECUTE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_MEDICINE.IN_EXECUTE__TRUE).ToList(); 

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessOptimizeList()
        {
            //try
            //{
            var group = ListRdo.GroupBy(g => new { g.BLOOD_ABO_CODE, g.BLOOD_TYPE_ID, g.IMP_PRICE }).ToList();
            ListRdo.Clear();
            decimal sum = 0;
            Mrs00323RDO rdo;
            List<Mrs00323RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00323RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00323RDO();
                listSub = item.ToList<Mrs00323RDO>();

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT"))
                    {
                        sum = listSub.Sum(s => (decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00323RDO()));
                    }
                }
                if (!hide) ListRdo.Add(rdo);
            }
        }

        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listImpMestBloodInTimes))
                {
                    LogSystem.Info(LogUtil.TraceData("Count:", listImpMestBloodInTimes.Count));
                    foreach (var blood in listImpMestBloodInTimes)
                    {
                        var rdo = new Mrs00323RDO();
                        rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;

                        rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                        rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                        rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;

                        rdo.BLOOD_ID = blood.BLOOD_ID;

                        rdo.IMP_PRICE = blood.IMP_PRICE;

                        rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME;
                        rdo.VOLUME = blood.VOLUME;

                        if (dicImpAmountType.ContainsKey(blood.IMP_MEST_TYPE_ID))
                            dicImpAmountType[blood.IMP_MEST_TYPE_ID].SetValue(rdo, (decimal)1);

                        ListRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestBloodInTimes))
                {
                    foreach (var blood in listExpMestBloodInTimes)
                    {
                        var rdo = new Mrs00323RDO();
                        rdo.BLOOD_ABO_CODE = blood.BLOOD_ABO_CODE;

                        rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                        rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                        rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;

                        rdo.BLOOD_ID = blood.BLOOD_ID;

                        rdo.IMP_PRICE = blood.IMP_PRICE;

                        rdo.SERVICE_UNIT_NAME = blood.SERVICE_UNIT_NAME;
                        rdo.VOLUME = blood.VOLUME;

                        if (dicExpAmountType.ContainsKey(blood.EXP_MEST_TYPE_ID))
                            dicExpAmountType[blood.EXP_MEST_TYPE_ID].SetValue(rdo, (decimal)1);

                        ListRdo.Add(rdo);
                    }
                }

                ProcessOptimizeList();

                listRdoBloodABO = ListRdo.GroupBy(g => g.BLOOD_ABO_CODE).Select(s => new Mrs00323RDO { BLOOD_ABO_CODE = s.First().BLOOD_ABO_CODE }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(o => o.BLOOD_TYPE_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoABO", listRdoBloodABO.OrderBy(s => s.BLOOD_ABO_CODE).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoABO", "Rdo", "BLOOD_ABO_CODE", "BLOOD_ABO_CODE");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
