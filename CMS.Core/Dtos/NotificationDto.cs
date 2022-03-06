using CMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Core.Dtos
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationAction Action { get; set; }
        public string ActionId { get; set; }
    }
}
