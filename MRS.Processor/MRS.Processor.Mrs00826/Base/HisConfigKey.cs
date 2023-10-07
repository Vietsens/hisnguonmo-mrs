using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.Base
{
    class HisConfigKey
    {
        internal const string MOS__BHYT__CALC_MATERIAL_PACKAGE_PRICE_OPTION = "MOS.BHYT.CALC_MATERIAL_PACKAGE_PRICE_OPTION";
        internal const string XML__4210__MATERIAL_PRICE_OPTION = "XML.EXPORT.4210.MATERIAL_PRICE_OPTION";
        internal const string XML__4210__MATERIAL_STENT_RATIO_OPTION = "XML.EXPORT.4210.MATERIAL_STENT_RATIO_OPTION";
        internal const string XML__4210__XML3__NGAY_KQ_OPTION = "XML.EXPORT.4210.XML3.NGAY_KQ_OPTION";
        internal const string XML__4210__XML4__DATA_OPTION = "XML.EXPORT.4210.XML4.DATA_OPTION";
        internal const string TEN_BENH_OPTION = "HIS.Desktop.Plugins.ExportXml.TenBenhOption";
        internal const string MaThuocOption = "HIS.Desktop.Plugins.ExportXml.HeinServiceTypeCodeNoTutorial";
        internal const string XmlNumbers = "HIS.Desktop.Plugins.ExportXml.XmlNumbers";
        internal const string Stent2LimitOption = "XML.EXPORT.4210.MATERIAL_STENT2_LIMIT_OPTION";
        internal const string IS_TREATMENT_DAY_COUNT_6556 = "XML.EXPORT.4210.IS_TREATMENT_DAY_COUNT_6556";
        internal const string MaBacSiOption = "XML.EXPORT.4210.XML3.MA_BAC_SI_EXAM_OPTION";
        internal const string CollinearThoiGianQt = "XML.EXPORT.4210.COLLINEAR.THOI_GIAN_QT";
        internal const string TutorialFormatCFG = "HIS.Desktop.Plugins.AssignPrescription.TutorialFormat";
        internal const string TransferOptionCFG = "XML.EXPORT.4210.XML1.TRANSFER_OPTION";
        internal const string ThoiGianQtCFG = "XML.EXPORT.4210.THOI_GIAN_QT";
        internal const string GayTeOptionCFG = "XML.EXPORT.4210.GAY_TE_OPTION";

        internal const string NdsIcdCodeOtherCFG = "MOS.BHYT.NDS_ICD_CODE__OTHER";
        internal const string NdsIcdCodeTeCFG = "MOS.BHYT.NDS_ICD_CODE__TE";
        internal const string TNguonkhacOptionCFG = "XML.EXPORT.4210.T_NGUONKHAC_OPTION";

        internal const string giuongGhepOptionCFG = "XML.EXPORT.4210.GIUONG_GHEP_OPTION";

        internal const string CollinearNgayTToan = "XML.EXPORT.4210.COLLINEAR.NGAY_TTOAN_OPTION";
        internal const string TtThauQd5937OptionCFG = "XML.EXPORT.4210.TT_THAU.QD5937_OPTION";
        internal const string AddressOptionCFG = "XML.EXPORT.4210.ADDRESS_OPTION";

        /// <summary>
        /// Cấu hình thời gian y lệnh của y lệnh khám
        /// 1: lấy thời gian xử lý khám START_TIME trong HIS_SERVICE_REQ
        /// khác 1: lấy thời gian chỉ định
        /// </summary>
        internal const string NgayYlenhOption = "XML.EXPORT.4210.XML3.NGAY_YL_OPTION";

        internal static string GetConfigData(List<HIS_CONFIG> datas, string key)
        {
            string result = "";
            try
            {
                if (datas != null && datas.Count > 0)
                {
                    var cfg = datas.FirstOrDefault(o => o.KEY == key);
                    if (cfg != null)
                    {
                        result = !String.IsNullOrWhiteSpace(cfg.VALUE) ? cfg.VALUE : cfg.DEFAULT_VALUE;
                    }
                }

                if (result != null)
                {
                    result = result.Trim();
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
