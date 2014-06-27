using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boy8.Models;

namespace Boy8.ViewModels
{
    public class StoryViewModel
    {
        public Story Story { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}