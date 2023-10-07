using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.Filter;
using MOS.MANAGER.HisDepartmentTran;
using AutoMapper;
using MOS.MANAGER.HisHeinApproval;

namespace MOS.MANAGER.HisTreatment
{
    partial class HisTreatmentGet : GetBase
    {

        internal List<V_HIS_TREATMENT_FEE_1> GetFeeView1(HisTreatmentFeeView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetFeeView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TREATMENT_FEE_1> GetFeeView1ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisTreatmentFeeView1FilterQuery filter = new HisTreatmentFeeView1FilterQuery();
                filter.IDs = ids;
                return this.GetFeeView1(filter);
            }
            return null;
        }

        /// <summary>
        /// Lay so tien con phai tra tuong ung voi ho so dieu tri cua benh nhan
        /// </summary>
        /// <param name="treatmentId">Ho so dieu tri</param>
        /// <returns></returns>
        internal decimal? GetUnpaid(long treatmentId)
        {
            V_HIS_TREATMENT_FEE_1 hisTreatmentFee = this.GetFeeView1ById(treatmentId);
            return this.GetUnpaid(hisTreatmentFee);
        }

        /// <summary>
        /// Lay so tien con phai tra tuong ung voi ho so dieu tri cua benh nhan
        /// </summary>
        /// <param name="treatmentId">Ho so dieu tri</param>
        /// <returns></returns>
        internal decimal? GetUnpaid(long treatmentId, List<V_HIS_TREATMENT_FEE_1> hisTreatmentFees)
        {
            V_HIS_TREATMENT_FEE_1 hisTreatmentFee = IsNotNullOrEmpty(hisTreatmentFees) ? hisTreatmentFees.Where(o => o.ID == treatmentId).FirstOrDefault() : null;
            return this.GetUnpaid(hisTreatmentFee);
        }

        internal V_HIS_TREATMENT_FEE_1 GetFeeView1ById(long id)
        {
            try
            {
                return GetFeeView1ById(id, new HisTreatmentFeeView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_FEE_1 GetFeeView1ById(long id, HisTreatmentFeeView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentDAO.GetFeeView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        //So tien can thu them
        internal decimal? GetUnpaid(V_HIS_TREATMENT_FEE_1 treatmentFee)
        {
            try
            {
                //Tong tien benh nhan can tra
                decimal totalPatientPrice = NotNullValue(treatmentFee.TOTAL_PATIENT_PRICE);
                //Tong tien benh nhan da thanh toan
                decimal totalBill = NotNullValue(treatmentFee.TOTAL_BILL_AMOUNT);
                //Tong so tien da ung
                decimal totalDeposit = NotNullValue(treatmentFee.TOTAL_DEPOSIT_AMOUNT);
                //Tong so tien da ket chuyen ke toan
                decimal totalBillTransfer = NotNullValue(treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT);
                //Tong tien da hoan tra
                decimal totalRepay = NotNullValue(treatmentFee.TOTAL_REPAY_AMOUNT);

                //Tong so tien con phai tra
                return totalPatientPrice - totalDeposit - totalBill + totalBillTransfer + totalRepay;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal decimal? GetAvailableAmount(long treatmentId)
        {
            try
            {
                V_HIS_TREATMENT_FEE_1 treatmentFee = this.GetFeeView1ById(treatmentId);
                return this.GetAvailableAmount(treatmentFee);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        //So tien hien du
        internal decimal? GetAvailableAmount(V_HIS_TREATMENT_FEE_1 treatmentFee)
        {
            try
            {
                //Tong so tien da ung
                decimal totalDeposit = NotNullValue(treatmentFee.TOTAL_DEPOSIT_AMOUNT);
                //Tong so tien da ket chuyen ke toan
                decimal totalBillTransfer = NotNullValue(treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT);
                //Tong tien da hoan tra
                decimal totalRepay = NotNullValue(treatmentFee.TOTAL_REPAY_AMOUNT);

                //Tong so tien con phai tra
                return totalDeposit - totalBillTransfer - totalRepay;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        public static decimal NotNullValue(decimal? value)
        {
            return value.HasValue ? value.Value : 0;
        }
    }
}
