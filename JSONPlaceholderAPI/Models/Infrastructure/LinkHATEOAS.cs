using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JSONPlaceholderAPI.Models.Infrastructure
{
    [DataContract]
    public class LinkHATEOAS
    {
        [DataMember]
        public string Href { get; set; }
        [DataMember]
        public string Method { get; set; }
        [DataMember]
        public string Rel { get; set; }
        public LinkHATEOAS(string rel, string href, string reqMethod)
        {
            Href = href;
            Method = reqMethod;
            Rel = rel;
        }
    }
}