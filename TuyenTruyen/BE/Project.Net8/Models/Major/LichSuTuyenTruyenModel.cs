using DTC.DefaultRepository.Constants;
using DTC.DefaultRepository.Models.Base;
using DTC.T;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BaiThi.Models.Major
{

    public class LichSuTuyenTruyenModel : Audit, TEntity<string>
    {
        public string NoiDungTuyenTruyen { get; set; } // có cái name nữa níu muốn thì lấy name , xoá dòng này luôn cũng dc
        public DateTime? NgayTuyenTruyen { get; set; }

        [BsonIgnore]
        public string NgayTuyenTruyenShow// lấy ra này nè nghen 
        {
            get { return NgayTuyenTruyen.HasValue ? NgayTuyenTruyen.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE) : ""; }
        }
   
        public int SoLanPhat {  get; set; }
        public string IdBaiThi {  get; set; }// truyền id bài viết vào đây nè
    }

    
}
