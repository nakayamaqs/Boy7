using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boy8.Models
{
    public class Baby
    {
        public enum MaleType
        {
            Girl = 0,
            Boy = 1,
        }

        [Required]
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "宝宝姓名")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "宝宝性别")]
        [Required]
        public MaleType Male { get; set; }  //1 -> boy, 0 -> Girl

        [Display(Name = "宝宝生日")]
        [DataType(DataType.DateTime)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Display(Name = "宝宝图像")]
        public Resource Thumbnail { get; set; }

        public string BabyImageFolder
        {
            get { return "Images/Avatar/" + ID; }
        }

        [Display(Name = "宝宝父母")]
        public virtual ICollection<Boy7User> Parents { get; set; }

        public Baby() {
            this.ID = Guid.NewGuid(); //auto-generate GUID.
        }
    }
}