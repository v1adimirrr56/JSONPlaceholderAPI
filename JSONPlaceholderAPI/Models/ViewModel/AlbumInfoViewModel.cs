using JSONPlaceholderAPI.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPlaceholderAPI.Models.ViewModel
{
    public class AlbumInfoViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<LinkHATEOAS> Links { get; set; }
    }
}