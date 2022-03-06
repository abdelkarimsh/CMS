using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Models
{
    public class PostAttachment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        [Required]
        public string AttachmentUrl { get; set; }
    }
}
