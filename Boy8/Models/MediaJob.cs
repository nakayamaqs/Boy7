using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boy8.Models
{
    public class MediaJob
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Key]
        public int ID { get; set; }

        [Display(Name = "Job Name")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Created Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedTime { get; set; }
    }
}