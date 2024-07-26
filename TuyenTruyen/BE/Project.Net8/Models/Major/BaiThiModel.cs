using BaiThi.Models.Major;
using DTC.DefaultRepository.Constants;
using DTC.DefaultRepository.Models.Base;
using DTC.T;
using MongoDB.Bson.Serialization.Attributes;
using Project.Net8.Models.Core;
using CoreModel = DTC.DefaultRepository.Models.Core.CoreModel;

namespace Project.Net8.Models.Major;

public class BaiThiModel: Audit, TEntity<string>
{

    // Tiêu đề là name á nghen
    public string MoTa {  get; set; }
    public DateTime? NgayBatDauTuyenTruyen { get; set; }

    [BsonIgnore]
    public string NgayBatDauTuyenTruyenShow// lấy ra này nè nghen 
    {
        get { return NgayBatDauTuyenTruyen.HasValue ? NgayBatDauTuyenTruyen.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE) : ""; }
    }
    public DateTime? NgayKetThuc { get; set; }

    [BsonIgnore]
    public string NgayKetThucShow// lấy ra này nè nghen 
    {
        get { return NgayKetThuc.HasValue ? NgayKetThuc.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE) : ""; }
    }
    public string TenDonVi {  get; set; }

    public bool IsDuyet { get; set; } = false;// trường này để duyệt nè

    public List<LichSuTuyenTruyenModel> LichSu { get; set; } = new List<LichSuTuyenTruyenModel>();
}