using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Boy7.Models
{
    public class Story
    {
        public int StoryId { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(200)] 
        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Body { get; set; }

        //Abstract of the story. Default length = 100.
        public string Abstract { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        [DataType(DataType.Url)]
        public string Vedio { get; set; }

        public string OtherResources { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime StoryCreatedTime { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime SyncTime { get; set; }

        //where the story come from.
        [Required]
        public string Source { get; set; }
        public string SyncAppID { get; set; }

        public string SyncAccount { get; set; }

        [DataType(DataType.MultilineText)]
        public string SyncComment { get; set; }

        [DataType(DataType.MultilineText)]
        public string SyncOther { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Rating { get; set; }
    }
}