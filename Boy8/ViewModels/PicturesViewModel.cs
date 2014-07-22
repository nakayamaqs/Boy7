using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boy8.ViewModels
{
    public class PicturesViewModel
    {
        public string URL { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        /// <summary>
        /// The author's images folder.
        /// </summary>
        public string AuthorURL { get; set; }

        //public string TitleForAuthor { get; set; }

        public string CommentCount { get; set; }

        public string LikeCount { get; set; }

        public string TitleForLike { get; set; }

        public string CreatedTime { get; set; }

    }
}