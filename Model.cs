using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autohana
{
    public class ModelProfile
    {
        public string linkAnhBia { get; set; }
        public string linkAvata { get; set; }
        public List<ModelHocVan> listHocVans { get; set; }
        public List<ModelNoiSong> listNoiSongs { get; set; }
        public List<ModelCongViec> listCongVIec { get; set; }
        public string gioiTinh { get; set; }
        public string motaBanThan { get; set; }
        public List<string> listimage { get; set; }
    }

    public class ModelCongViec
    {
        public string name { get; set; }
        public DateTime? timeFrom { get; set; }
        public DateTime? timeTo { get; set; }
        public bool? isToNow { get; set; }
    }
    public class ModelHocVan
    {
        public string name { get; set; }
        public TypeHocVan type { get; set; }
        public DateTime? timeFrom { get; set; }
        public DateTime? timeTo { get; set; }
        public bool? isToNow { get; set; }
    }
    public enum TypeHocVan
    {
        daihoc,
        trunghoc
    }

    public enum VitriGhiEnum
    {
        stt,
        tendangnhap,
        matkhau,
        tennguoidung,
        tenTDS,
        passTDS,
        matKhau2Fa,
        cookie,
        RunTDS,
        AnChrome,
        TamDung,
        TrangThai,
    }

    public class ModelNoiSong
    {
        public string name { get; set; }
        public TypeDiaChi type { get; set; }
    }
    public enum TypeDiaChi
    {
        ThanhPhoHienTai,
        QueQuan
    }


    public class Model
    {
        public string userId { get; set; }
        public int typeRun { get; set; }
        public bool isRun { get; set; }
        public Process processId { get; set; }
    }

    public class JobLeaned
    {
        public string userId { get; set; }
        public string jobOf { get; set; }
        public int type { get; set; }
        public int coin { get; set; }
        public string status { get; set; }
        public string notification { get; set; }
        public DateTime time { get; set; }
    }

    public class ModelAccount
    {
        public string Stt { get; set; }
        public string Id { get; set; }
        public string Pass { get; set; }
        public string Fa { get; set; }
        public string Cookie { get; set; }
        public string Name { get; set; }
        public string Golike { get; set; }
        public string PassGolike { get; set; }
        public string Hana { get; set; }
        public string PassHana { get; set; }
        public string NameTDS { get; set; }
        public string PassTDS { get; set; }
        public bool RunHana { get; set; }
        public bool RunTDS { get; set; }
        public bool RunGolike { get; set; }
        public bool An { get; set; }
        public bool Stop { get; set; }
        public int Total { get; set; }
        public int Done { get; set; }
        public int Error { get; set; }
        public int ReWork { get; set; }
        public string Action { get; set; }
        public string DKhana { get; set; }
        public string UserAgent { get; set; }
        public string BackUp { get; set; }
    }


    public class ModelLamJob
    {
        public bool isFinishTotalJob { get; set; } = false;
        public bool isFinishOneJob { get; set; } = false;
        public bool isCheckpoint { get; set; } = false;
        public bool isBlockaction { get; set; } = false;
        public bool isError5Finish { get; set; } = false;
        public int numberJobFinish { get; set; } = 0;
    }

    public enum Modelfb
    {
        isLoginOk,
        isloginNotOk,
        isCheckpoint,
        isBlockaction
    }


    public enum ActionFb
    {
        [Display(Name = "like bài viết")]
        LikePost = 2,
        [Display(Name = "like Page")]
        Likepage = 4,
        [Display(Name = "Theo dõi")]
        Follow = 5,
        [Display(Name = "Comment")]
        Comment,
        [Display(Name = "Angry")]
        Angry,
        [Display(Name = "Haha")]
        Haha,
        [Display(Name = "Love")]
        Love,
        [Display(Name = "Care")]
        Care,
        [Display(Name = "Wow")]
        Wow,
        [Display(Name = "Sad")]
        Sad,
    }

    public enum ActionNuoiFbEnum
    {
        [Display(Name = "Love")]
        Love,
        [Display(Name = "Care")]
        Care,
        [Display(Name = "Wow")]
        Wow,
        [Display(Name = "Haha")]
        Haha,
    }

    public enum ActionMenu
    {
        OpenChrome,
        OpenFacebook,
        BackUpFacebookOnlyImageFriend,
        BackUpFacebookAll,
        ChayDangKiHana,
        QuetThanhVienGroup,
        CommentGroup,
    }

}
