using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00704
{
    class Mrs00704Processor : AbstractProcessor
    {
        public List<Loai_PTTT_Model> List_Loai_PTTT_Model { get; set; }
        public List<DT_PCD_Model> List_DT_PCD_Model { get; set; }
        public List<DT_KCD_Model> List_DT_KCD_Model { get; set; }

        public Mrs00704Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00704Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                List_Loai_PTTT_Model = new List<Loai_PTTT_Model>();
                List_DT_PCD_Model = new List<DT_PCD_Model>();
                List_DT_KCD_Model = new List<DT_KCD_Model>();

                Loai_PTTT_Model pt = new Loai_PTTT_Model();
                pt.LoaiPTTT = "1";
                pt.List_PTTT_Model = ManagerSql.GetLoaiPTTT(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                List_Loai_PTTT_Model.Add(pt);

                Loai_PTTT_Model tt = new Loai_PTTT_Model();
                tt.LoaiPTTT = "2";
                tt.List_PTTT_Model = ManagerSql.GetLoaiPTTT(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                List_Loai_PTTT_Model.Add(tt);

                List_DT_PCD_Model = ManagerSql.GetDtPcd();

                DT_KCD_Model dangdt = new DT_KCD_Model();
                dangdt.LoaiBaoCao = "1";
                dangdt.List_Khoa_DT_Model = ManagerSql.GetKhoaDt(1);
                List_DT_KCD_Model.Add(dangdt);

                DT_KCD_Model ketThuc = new DT_KCD_Model();
                ketThuc.LoaiBaoCao = "2";
                ketThuc.List_Khoa_DT_Model = ManagerSql.GetKhoaDt(2);
                List_DT_KCD_Model.Add(ketThuc);

                DT_KCD_Model thanhToan = new DT_KCD_Model();
                thanhToan.LoaiBaoCao = "3";
                thanhToan.List_Khoa_DT_Model = ManagerSql.GetKhoaDt(3);
                List_DT_KCD_Model.Add(thanhToan);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                objectTag.AddObjectData(store, "List_Loai_PTTT_Model", List_Loai_PTTT_Model);
                objectTag.AddObjectData(store, "List_DT_PCD_Model", List_DT_PCD_Model);
                objectTag.AddObjectData(store, "List_DT_KCD_Model", List_DT_KCD_Model);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //*Hoạt động phẫu thuật,thủ thuật
        //-LoaiPTTT: Loại phẫu thuật hoặc thủ thuật (1: Phẫu thuật, 2: Thủ
        //thuật)
        //-TenPTTT: Loại : Đặc biệt, 1, 2, 3
        //-DoanhThu: Doanh thu của phẫu thuật ứng theo từng loại, Doanh thu 
        //của thủ thuật ứng theo từng loại
        //-ChiPhi: Chi phí theo từng loại 
        //-LoiNhuan: Lợi nhuận theo từng loại
        //Mục này lấy theo thời gian chỉ định

        //*Doanh thu phòng khám
        //-TenPhong: Tên phòng khám
        //-TongCong: Doanh thu tổng cộng của phòng khám
        //-BHYT: Doanh thu tổng cộng của BN có đối tượng BHYT, sử dụng DV được BHYT chi trả
        //-ThuPhi: Doanh thu tổng cộng của BN có đối tượng ko thẻ BHYT,sử dụng các DV BHYT không chi trả
        //Mục này thời gian lấy x- y (x,y là thời gian thanh toán từ đến)

        //*Doanh thu khoa
        //- TenKhoa: Tên khoa trong Bệnh viện
        //- DoanhThu: Doanh thu theo khoa
        //- ChiPhi: Chi phí hao phí khoa phòng, hao phí dịch vụ kỹ thuật
        //- LoiNhuan: LoaiBaoCao = Doanh thu khoa - chi phí
        //1:Đang điều trị: Ko quan tâm thời gian,tính tổng theo BN đang hiện diện
        //2:Đã ra viện chưa thanh toán: lấy theo thời gian ra viện từ đến
        //3:Đã thanh toán: lấy theo thời gian thanh toán từ đến
        //Lưu ý: Tổng doanh thu của các khoa theo từng loại(Đang điều trị, ra viện chưa thanh toán, đã thanh toán) phải logic với doanh thu Bệnh viện)

        //doanh thu: giá dịch vụ
        //chi phi: Giá dịch vụ hao phí
    }
}
