﻿using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.ImageToolsModule.Core.Models
{
    using System;

    public class ThumbnailOption : AuditableEntity
    {
        public string Name { get; set; }

        public string FileSuffix { get; set; }

        public ResizeMethod ResizeMethod { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }
    }
}