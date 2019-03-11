using JSONPlaceholderAPI.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPlaceholderAPI.Models.ViewModel
{
    public class AlbumViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long UserId { get; set; }
        public UserViewModel User { get; set; }
        public ICollection<LinkHATEOAS> Links { get; set; }
    }
}