using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyModel_CodeFirst.Models
{
    public class ReBook
    {
        [Key]
        public long RId { get; set; }

        [Required(ErrorMessage = "必填")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "回覆內容")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "必填")]
        [StringLength(10, ErrorMessage = "姓名最多10個字")]
        [Display(Name = "回覆人")]
        public string Author { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [Display(Name = "回覆時間")]
        public DateTime TimeStamp { get; set; }



        //1.1.5 撰寫兩個類別間的關聯屬性做為未來資料表之間的關聯
        //這是來自Book的外來鍵
        [ForeignKey("Book")]
        public long GId { get; set; }

        public virtual Book? Book { get; set; } = null!;
    }
}
