using MOS.MANAGER.HisMediStock;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisExpMest; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00393
{
    class Mrs00393Processor : AbstractProcessor
    {
        Mrs00393Filter castFilter = null; 
        List<Mrs00393RDO> listRdo = new List<Mrs00393RDO>(); 
        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
            //IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HCS

        }; 
        public Mrs00393Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_EXP_MEST> listChmsExpMest = null; 

        string ExpMediStockNames = ""; 
        string ImpMediStockName = ""; 
        int ExpMest_Count = 0; 

        public override Type FilterType()
        {
            return typeof(Mrs00393Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00393Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("MRS00393" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    var expMediStocks = MANAGER.Config.HisMediStockCFG.HisMediStocks.Where(o => castFilter.MEDI_STOCK_IDs.Contains(o.ID)).ToList(); 
                    foreach (var item in expMediStocks)
                    {
                        if (ExpMediStockNames == "")
                            ExpMediStockNames = item.MEDI_STOCK_NAME; 
                        else
                            ExpMediStockNames = ExpMediStockNames + ", " + item.MEDI_STOCK_NAME; 
                    }
                }

                if (castFilter.IMP_MEDI_STOCK_ID.HasValue)
                {
                    var impMediStock = MANAGER.Config.HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == castFilter.IMP_MEDI_STOCK_ID.Value); 
                    if (impMediStock != null && impMediStock.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        ImpMediStockName = impMediStock.MEDI_STOCK_NAME; 
                    }
                    else
                    {
                        throw new Exception("ImpMediStockId khong dung: " + castFilter.IMP_MEDI_STOCK_ID); 
                    }
                }

                HisExpMestViewFilterQuery chmsFilter = new HisExpMestViewFilterQuery(); 
                chmsFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                chmsFilter.FINISH_TIME_TO = castFilter.TIME_TO; 
                chmsFilter.EXP_MEST_TYPE_IDs = this.CHMS_EXP_MEST_TYPE_IDs; 
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    chmsFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs; 
                }
                chmsFilter.ORDER_DIRECTION = "ASC"; 
                chmsFilter.ORDER_FIELD = "EXP_MEST_CODE"; 
                if (castFilter.IMP_MEDI_STOCK_ID.HasValue)
                {
                    chmsFilter.IMP_MEDI_STOCK_IDs = new List<long>() { castFilter.IMP_MEDI_STOCK_ID.Value }; 
                }
                else
                {
                    chmsFilter.IMP_MEDI_STOCK_IDs = MANAGER.Config.HisMediStockCFG.HisMediStocks.Where(o => o.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(s => s.ID).ToList(); 
                }
                listChmsExpMest = new HisExpMestManager(paramGet).GetView(chmsFilter); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00393"); 
                }
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
            bool result = true; 
            try
            {
                if (IsNotNullOrEmpty(listChmsExpMest))
                {
                    var listExpMestCode = listChmsExpMest.Select(s => s.EXP_MEST_CODE).ToList(); 
                    ExpMest_Count = listExpMestCode.Count; 
                    int index = ExpMest_Count / 8; 
                    int soDu = ExpMest_Count % 8; 
                    int index1 = index; 
                    int index2 = index; 
                    int index3 = index; 
                    int index4 = index; 
                    int index5 = index; 
                    int index6 = index; 
                    int index7 = index; 
                    int index8 = index; 
                    if (soDu > 0)
                    {
                        for (int i = 0;  i < soDu;  i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    index1++; 
                                    break; 
                                case 1:
                                    index2++; 
                                    break; 
                                case 2:
                                    index3++; 
                                    break; 
                                case 3:
                                    index4++; 
                                    break; 
                                case 4:
                                    index5++; 
                                    break; 
                                case 5:
                                    index6++; 
                                    break; 
                                case 6:
                                    index7++; 
                                    break; 
                                default:
                                    break; 
                            }
                        }
                    }
                    int start = 0; 
                    var listCode_1 = listExpMestCode.Skip(start).Take(index1).ToList(); 
                    start += index1; 
                    var listCode_2 = listExpMestCode.Skip(start).Take(index2).ToList(); 
                    start += index2; 
                    var listCode_3 = listExpMestCode.Skip(start).Take(index3).ToList(); 
                    start += index3; 
                    var listCode_4 = listExpMestCode.Skip(start).Take(index4).ToList(); 
                    start += index4; 
                    var listCode_5 = listExpMestCode.Skip(start).Take(index5).ToList(); 
                    start += index5; 
                    var listCode_6 = listExpMestCode.Skip(start).Take(index6).ToList(); 
                    start += index6; 
                    var listCode_7 = listExpMestCode.Skip(start).Take(index7).ToList(); 
                    start += index7; 
                    var listCode_8 = listExpMestCode.Skip(start).Take(index8).ToList(); 
                    for (int i = 0;  i < index1;  i++)
                    {
                        Mrs00393RDO rdo = new Mrs00393RDO(); 
                        rdo.EXP_MEST_CODE_1 = listCode_1[i]; 
                        if (i < index2)
                        {
                            rdo.EXP_MEST_CODE_2 = listCode_2[i]; 
                        }
                        if (i < index3)
                        {
                            rdo.EXP_MEST_CODE_3 = listCode_3[i]; 
                        }
                        if (i < index4)
                        {
                            rdo.EXP_MEST_CODE_4 = listCode_4[i]; 
                        }
                        if (i < index5)
                        {
                            rdo.EXP_MEST_CODE_5 = listCode_5[i]; 
                        }
                        if (i < index6)
                        {
                            rdo.EXP_MEST_CODE_6 = listCode_6[i]; 
                        }
                        if (i < index7)
                        {
                            rdo.EXP_MEST_CODE_7 = listCode_7[i]; 
                        }
                        if (i < index8)
                        {
                            rdo.EXP_MEST_CODE_8 = listCode_8[i]; 
                        }
                        listRdo.Add(rdo); 
                    }
                }
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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    dicSingleTag.Add("EXP_MEDI_STOCK_NAMEs", string.Join(",",(HisMediStockCFG.HisMediStocks.Where(o=>castFilter.MEDI_STOCK_IDs.Contains(o.ID)).ToList()??new List<V_HIS_MEDI_STOCK>()).Select(o=>o.MEDI_STOCK_NAME).ToList()));
                }
                if(castFilter.IMP_MEDI_STOCK_ID!=null)
                {
                dicSingleTag.Add("IMP_MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o=>castFilter.IMP_MEDI_STOCK_ID==o.ID)??new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME); 
                }
                dicSingleTag.Add("EXP_MEST_COUNT", ExpMest_Count); 
                objectTag.AddObjectData(store, "Report", listRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
