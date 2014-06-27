using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Boy8.Models
{
    public class SyncSource
    {
        [Display(Name = "Sync Source ID")]
        public int ID { get; set; }

        //where the story come from.
        [Required]
        [Display(Name = "Sync Source Name")]
        public string Name { get; set; }

        [Required]
        //[Column(TypeName = "Description")]
        [Display(Name = "Sync Description")]
        public string Description { get; set; }


    }
}