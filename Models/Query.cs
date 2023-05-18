using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace BDLab2.Models
{
    public class Query
    {
        public string QueryId { get; set; }
        public string Error { get; set; }
        public int ErrorFlag { get; set; }

        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public List<int> LabelIds { get; set; }
        public List<string> LabelNames { get; set; }

        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public List<int> ArtistIds { get; set; }
        public List<string> ArtistNames { get; set; }

        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public decimal AlbumPrice { get; set; }
        public string AlbumDescription { get; set; }
        public List<int> AlbumIds { get; set; }
        public List<string> AlbumTitles{ get; set; }
        public List<decimal> AlbumPrices { get; set; }
        public List<string> AlbumDescriptions { get; set; }
    }
}
