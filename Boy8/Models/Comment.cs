using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Boy8.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        [StringLength(500, ErrorMessage = "Comments cannot be longer than 500 characters.")]
        public string Message { get; set; }

        public CommentStatus Status { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedTime { get; set; }

        //Comment can map to 0-1 user.
        [ForeignKey("Owner")]
        public string Boy7UserId { get; set; }

        public virtual Boy7User Owner { get; set; }

        public enum CommentStatus
        {
            Public = 0,
            FamilyOnly = 1,
            Private = 2,
        }
    }
}
