using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boy8.Models
{
    public class Resource
    {
        public enum ResourceType { 
            Picture = 0,
            Vedio = 1,
            Text = 2,
            Audio = 3,
        }

        public int ResourceId { get; set; }

        [Display(Name = "Resource URL")]
        [DataType(DataType.Url)]
        public string Res_URL { get; set; }

        [Display(Name = "Resource Type")]
        public ResourceType Res_Type { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Created Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedTime { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Updated Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedTime { get; set; }

        //[ForeignKey("Comment")]
        //public int? CommentID { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
    }
}