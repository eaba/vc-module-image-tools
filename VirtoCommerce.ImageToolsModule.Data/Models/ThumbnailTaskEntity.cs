﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.ImageToolsModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.ImageToolsModule.Data.Models
{
    public class ThumbnailTaskEntity : AuditableEntity
    {
        public DateTime? LastRun { get; set; }

        [Required]
        [StringLength(1024)]
        public string Name { get; set; }
        public ObservableCollection<ThumbnailTaskOptionEntity> ThumbnailTaskOptionEntities { get; set; }

        [Required]
        [StringLength(2048)]
        public string WorkPath { get; set; }
        public ThumbnailTaskEntity FromModel(ThumbnailTask task, PrimaryKeyResolvingMap pkMap)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            pkMap.AddPair(task, this);

            Name = task.Name;
            LastRun = task.LastRun;
            WorkPath = task.WorkPath;
            CreatedBy = task.CreatedBy;
            CreatedDate = task.CreatedDate;
            ModifiedBy = task.ModifiedBy;
            ModifiedDate = task.ModifiedDate;

            var newOptionEntitys = task.ThumbnailOptions.Select(o =>
            {
                var optionEntity = new ThumbnailOptionEntity();
                return optionEntity.FromModel(o, pkMap);
            });

            var existingOptionEntityIds = ThumbnailTaskOptionEntities.Select(e => e.ThumbnailOptionEntityId);

            var newTaskOptionEntities = newOptionEntitys.Where(e => !existingOptionEntityIds.Contains(e.Id)).Select(
                e => new ThumbnailTaskOptionEntity()
                {
                    ThumbnailTaskEntity = this,
                    ThumbnailTaskEntityId = this.Id,
                    ThumbnailOptionEntity = e,
                    ThumbnailOptionEntityId = e.Id
                });

            ThumbnailTaskOptionEntities.AddRange(newTaskOptionEntities);

            return this;
        }

        public void Patch(ThumbnailTaskEntity target)
        {
            target.CreatedBy = CreatedBy;
            target.CreatedDate = CreatedDate;
            target.Id = Id;
            target.LastRun = LastRun;
            target.ModifiedBy = ModifiedBy;
            target.ModifiedDate = ModifiedDate;
            target.Name = Name;
            target.ThumbnailTaskOptionEntities = ThumbnailTaskOptionEntities;
            target.WorkPath = WorkPath;
        }

        public ThumbnailTask ToModel(ThumbnailTask task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            task.CreatedBy = CreatedBy;
            task.CreatedDate = CreatedDate;
            task.LastRun = LastRun;
            task.ModifiedBy = ModifiedBy;
            task.ModifiedDate = ModifiedDate;
            task.Name = Name;
            task.WorkPath = WorkPath;

            task.ThumbnailOptions = this.ThumbnailTaskOptionEntities.Select(o => o.ThumbnailOptionEntity.ToModel(new ThumbnailOption())).ToArray();

            return task;
        }
    }
}