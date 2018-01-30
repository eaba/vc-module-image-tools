﻿using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.ImageToolsModule.Core.Models
{
    public class ThumbnailTask// : AuditableEntity
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public DateTime? LastRun { get; set; }

        public string WorkPath { get; set; }

        public IList<ThumbnailOption> ThumbnailOptions { get; set; }
    }
}