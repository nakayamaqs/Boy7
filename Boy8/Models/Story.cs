using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Boy8.Models
{
    public class Story
    {

        [Display(Name = "Story ID")]
        public int StoryId { get; set; }

        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.", MinimumLength = 1)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]

        public string Body { get; set; }

        //Abstract of the story. Default length = 100.
        [StringLength(50, ErrorMessage = "Abstract cannot be longer than 50 characters.")]
        public string Abstract { get; set; }

        public string SyncAccount { get; set; }

        [DataType(DataType.MultilineText)]
        public string SyncComment { get; set; }

        [DataType(DataType.MultilineText)]
        public string SyncOther { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StoryCreatedOrSyncTime { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StoryUpdatedTime { get; set; }

        public int Rating { get; set; }

        [ForeignKey("SyncSource")]
        public int? SyncSourceID { get; set; }
        public virtual SyncSource SyncSource { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}