using MOS.MANAGER.HisExpMestMedicine;
using FlexCel.Report; 
using Inventec.Common.Logging; 
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

namespace MRS.Processor.Mrs00129
{
    public class Mrs00129Processor : AbstractProcessor
    {
        List<Mrs00129RDO> _lisMrs00129RDO = new List<Mrs00129RDO>(); 
        Mrs00129Filter CastFilter; 

        public Mrs00129Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00129Filter); 
        }

        protected override bool GetData()
        {
            var result = false; 
            try
            {
                CastFilter = ((Mrs00129Filter)reportFilter); 
                var paramGet = new CommonParam(); 
                LogSystem.Debug("Bat dau lay du lieu filter: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter)); 
                //////////////////////////////////////////////////////////////////////////////////-V_HIS_SEREVICE
                var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                {
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN,
                    EXP_TIME_FROM = CastFilter.DATE_FROM,
                    EXP_TIME_TO = CastFilter.DATE_TO,
                    IS_EXPORT = true
                }; 
                var listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine); 
                ////////////////////////////////////////////////////////////////////////////////// -V_HIS_SALE_EXP_MEST
                var listExpMestIds = listExpMestMedicine.Select(s => s.EXP_MEST_ID ?? 0).ToList(); 
                var listSaleExpMest = new List<V_HIS_EXP_MEST>(); 
                var number1 = listExpMestIds.Count / 100; 
                var remainder1 = listExpMestIds.Count % 100; 
                if (remainder1 > 0)
                    number1 = number1 + 1; 
                var skip1 = 0; 
                var take1 = 100; 
                for (var i = 0;  i < number1;  i++)
                {
                    var list = listExpMestIds.Skip(skip1).Take(take1).ToList(); 
                    skip1 = skip1 + 100; 
                    if (listExpMestIds.Count - skip1 < 100)
                        take1 = listExpMestIds.Count - skip1; 
                    var metyFilterSaleExpMest = new HisExpMestViewFilterQuery
                    {
                        IDs = list
                    }; 
                    var treatmentView = new HisExpMestManager(paramGet).GetView(metyFilterSaleExpMest); 
                    listSaleExpMest.AddRange(treatmentView); 
                }
                ProcessFilterData(listExpMestMedicine, listSaleExpMest); 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00129." +
                        LogUtil.TraceData(
                            LogUtil.GetMemberName(() => paramGet), paramGet)); 
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
            var result = false; 
            try
            {
                
                result = true; 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessFilterData(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines, List<V_HIS_EXP_MEST> listSaleExpMests)
        {
            var groupByExpTimes = (from listExpMestMedicine in listExpMestMedicines
                                   let hh = listSaleExpMests.FirstOrDefault(s => s.ID == listExpMestMedicine.EXP_MEST_ID)
                                   select new Mrs00129RDO
                                   {
                                       EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listExpMestMedicine.EXP_TIME.ToString()),
                                       PATIENR_CODE = hh.TDL_PATIENT_CODE,
                                       PATIENR_NAME = hh.TDL_PATIENT_NAME ,
                                       CLIENT_NAME = hh.TDL_PATIENT_NAME,
                                       MEDICINE_TYPE_CODE = listExpMestMedicine.MEDICINE_TYPE_CODE,
                                       MEDICINE_TYPE_NAME = listExpMestMedicine.MEDICINE_TYPE_NAME,
                                       TOTAL_AMOUNT = listExpMestMedicine.AMOUNT,
                                       TOTAL_DISCOUNT = listExpMestMedicine.DISCOUNT,
                                       TOTAL_PRICE = listExpMestMedicine.PRICE,
                                       VAT_RATIO = listExpMestMedicine.VAT_RATIO
                                   }).OrderBy(s => s.EXP_TIME).GroupBy(s => s.EXP_TIME).ToList(); 
            #region
            //foreach (var listRdo in listRdos)
            //{
            //    var groupByPatientNames = listRdo.GroupBy(s => s.PATIENR_NAME).ToList(); 
            //    foreach (var groupByPatientName in groupByPatientNames)
            //    {
            //        var groupByMedicineNames = groupByPatientName.GroupBy(s => s.MEDICINE_TYPE_NAME).ToList(); 
            //        foreach (var groupByMedicineName in groupByMedicineNames)
            //        {
            //            var groupByTotalPrices = groupByMedicineName.GroupBy(s => s.TOTAL_PRICE).ToList(); 
            //            var listTotalPrice = new List<decimal>(); 
            //            foreach (var groupByTotalPrice in groupByTotalPrices)
            //            {
            //                var amount = groupByTotalPrice.Select(s => s.TOTAL_AMOUNT).Sum(); 
            //                var price = groupByTotalPrice.Key; 
            //                listTotalPrice.Add(amount * (decimal)price); 
            //            }

            //            var totalAmout = groupByMedicineName.Select(s => s.TOTAL_AMOUNT).Sum(); 
            //            var totalDiscount = groupByMedicineName.Select(s => s.TOTAL_DISCOUNT).Sum(); 
            //            var patientName = groupByMedicineName.Select(s => s.PATIENR_NAME).FirstOrDefault() != null ? groupByMedicineName.Select(s => s.PATIENR_NAME).FirstOrDefault() : groupByMedicineName.Select(s => s.CLIENT_NAME).FirstOrDefault(); 

            //            var rdo = new Mrs00129RDO
            //            {
            //                EXP_TIME = groupByMedicineName.Select(s => s.EXP_TIME).FirstOrDefault(),
            //                PATIENR_CODE = groupByMedicineName.Select(s => s.PATIENR_CODE).FirstOrDefault(),
            //                PATIENR_NAME = patientName,
            //                MEDICINE_TYPE_CODE = groupByMedicineName.Select(s => s.MEDICINE_TYPE_CODE).FirstOrDefault(),
            //                MEDICINE_TYPE_NAME = groupByMedicineName.Select(s => s.MEDICINE_TYPE_NAME).FirstOrDefault(),
            //                TOTAL_AMOUNT = totalAmout,
            //                TOTAL_DISCOUNT = totalDiscount,
            //                TOTAL_PRICE = listTotalPrice.Sum()
            //            }; 
            //            _lisMrs00129RDO.Add(rdo); 
            //        }
            //    }
            //}
            #endregion

            foreach (var groupByExpTime in groupByExpTimes)
            {
                var groupByMedicineNames = groupByExpTime.GroupBy(s => s.MEDICINE_TYPE_NAME).OrderBy(s => s.Key).ToList(); 
                foreach (var groupByMedicineName in groupByMedicineNames)
                {
                    var groupByPatientNames = groupByMedicineName.GroupBy(s => s.PATIENR_NAME).OrderBy(s => s.Key).ToList(); 
                    foreach (var groupByPatientName in groupByPatientNames)
                    {
                        //var groupByPrices = groupByPatientName.GroupBy(s => s.TOTAL_PRICE).ToList(); 
                        //var listTotalPrice = new List<decimal>(); 
                        //foreach (var groupByPrice in groupByPrices)
                        //{
                        //    var amount = groupByPrice.Select(s => s.TOTAL_AMOUNT).Sum(); 
                        //    var price = groupByPrice.Key; 
                        //    listTotalPrice.Add(amount * (decimal)price); 
                        //}
                        var listPrice = new List<decimal?>(); 
                        foreach (var patientName in groupByPatientName)
                        {
                            var price = (patientName.TOTAL_PRICE * patientName.TOTAL_AMOUNT) * (1 + patientName.VAT_RATIO) - patientName.TOTAL_DISCOUNT; 
                            listPrice.Add(price); 
                        }


                        var totalAmout = groupByPatientName.Select(s => s.TOTAL_AMOUNT).Sum(); 
                        var totalDiscount = groupByPatientName.Select(s => s.TOTAL_DISCOUNT).Sum(); 
                        //var totalVatRatio = groupByPatientName.Select(s => s.VAT_RATIO).Sum(); 
                        //var totalPrice = listTotalPrice.Sum() * (1 + totalVatRatio) - totalDiscount; 
                        var rdo = new Mrs00129RDO
                        {
                            EXP_TIME = groupByExpTime.Select(s => s.EXP_TIME.Trim()).FirstOrDefault(),
                            MEDICINE_TYPE_CODE = groupByPatientName.Select(s => s.MEDICINE_TYPE_CODE).FirstOrDefault(),
                            MEDICINE_TYPE_NAME = groupByPatientName.Select(s => s.MEDICINE_TYPE_NAME).FirstOrDefault(),
                            PATIENR_CODE = groupByPatientName.Select(s => s.PATIENR_CODE).FirstOrDefault(),
                            PATIENR_NAME = groupByPatientName.Select(s => s.PATIENR_NAME).FirstOrDefault(),
                            TOTAL_AMOUNT = totalAmout,
                            TOTAL_DISCOUNT = totalDiscount,
                            //TOTAL_PRICE = totalPrice
                            TOTAL_PRICE = listPrice.Sum()
                        }; 
                        _lisMrs00129RDO.Add(rdo); 
                    }
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM)); 
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO)); 

                objectTag.AddObjectData(store, "Report", _lisMrs00129RDO); 
                objectTag.SetUserFunction(store, "FuncSameTitleRow", new CustomerFuncMergeSameDataExpTime(_lisMrs00129RDO)); 
                objectTag.SetUserFunction(store, "FuncSameTitleRowMedicineName", new CustomerFuncMergeSameDataMedicine(_lisMrs00129RDO)); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
    class CustomerFuncMergeSameDataExpTime : TFlexCelUserFunction
    {
        List<Mrs00129RDO> sereServRdos; 
        int SameType; 
        public CustomerFuncMergeSameDataExpTime(List<Mrs00129RDO> sereServRdos)
        {
            this.sereServRdos = sereServRdos; 
        }
        public override object Evaluate(object[] parameters)
        {
            bool result = false; 
            try
            {
                if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

                string currentValue = ""; 
                string nextValue = ""; 

                int currentIdx = (int)parameters[0]; 

                currentValue = sereServRdos[currentIdx].EXP_TIME; 
                if (currentIdx + 1 < sereServRdos.Count)
                {
                    nextValue = sereServRdos[currentIdx + 1].EXP_TIME; 

                    if (!String.IsNullOrEmpty((currentValue))
                    && !String.IsNullOrEmpty((nextValue))
                    && currentValue.Equals((nextValue)))
                    {
                        result = true; 
                    }
                }
            }
            catch (Exception ex)
            {
                result = false; 
            }

            return result; 
        }
    }

    class CustomerFuncMergeSameDataMedicine : TFlexCelUserFunction
    {
        List<Mrs00129RDO> sereServRdos; 
        int SameType; 
        public CustomerFuncMergeSameDataMedicine(List<Mrs00129RDO> sereServRdos)
        {
            this.sereServRdos = sereServRdos; 
        }
        public override object Evaluate(object[] parameters)
        {
            bool result = false; 
            try
            {
                if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

                string currentValue = ""; 
                string nextValue = ""; 

                int currentIdx = (int)parameters[0]; 

                currentValue = sereServRdos[currentIdx].MEDICINE_TYPE_NAME; 
                if (currentIdx + 1 < sereServRdos.Count)
                {
                    nextValue = sereServRdos[currentIdx + 1].MEDICINE_TYPE_NAME; 

                    if (!String.IsNullOrEmpty((currentValue))
                    && !String.IsNullOrEmpty((nextValue))
                    && currentValue.Equals((nextValue)))
                    {
                        result = true; 
                    }
                }
            }
            catch (Exception ex)
            {
                result = false; 
            }

            return result; 
        }
    }
}
