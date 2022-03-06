﻿using CMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ContentType = CMS.Core.Enums.ContentType;

namespace CMS.Data.Models
{
    public class ContentChangeLog
    {
        public int Id { get; set; }

        public int ContentId { get; set; }

        public ContentType Type { get; set; }

        public DateTime ChangeAt { get; set; }

        public ContentStatus Old { get; set; }

        public ContentStatus New { get; set; }
    }
}
