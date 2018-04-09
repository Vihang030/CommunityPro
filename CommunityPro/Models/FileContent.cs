using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityPro.Models
{
    public class FileContent
    {

        [Key, ForeignKey("aFile")]
        public int FileIContentD { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Content { get; set; }

        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string MimeType { get; set; }

        public virtual aFile aFile { get; set; }
    }
}