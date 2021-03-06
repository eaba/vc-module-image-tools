﻿using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ImageToolsModule.Core.Models;
using VirtoCommerce.ImageToolsModule.Data.Models;
using VirtoCommerce.ImageToolsModule.Data.Repositories;
using VirtoCommerce.ImageToolsModule.Data.Services;
using Xunit;

namespace VirtoCommerce.ImageToolsModule.Tests
{
    public class ThumbnailTaskSearchServiceTests
    {
        [Fact]
        public void Search_ThumbnailOptionSearchCriteria_ReturnsGenericSearchResponseOfTasksInExpectedOrder()
        {
            var repoMock = GetTaskRepositoryMock();
            var target = new ThumbnailTaskSearchService(() => repoMock.Object);
            var criteria = new ThumbnailTaskSearchCriteria { Sort = "Name:desc;WorkPath:desc" };
            var resultTasks = target.Search(criteria);

            var expectedTasks = ThumbnailTaskEntitysDataSource.Select(x => x.ToModel(new ThumbnailTask())).OrderByDescending(t => t.Name).ThenByDescending(t => t.WorkPath).ToArray();
            Assert.Equal(expectedTasks, resultTasks.Results);
        }

        [Fact]
        public void Search_SearchByExistingKeyword_TasksFound()
        {
            var keyword = "NameLong";
            var repoMock = GetTaskRepositoryMock();
            var target = new ThumbnailTaskSearchService(() => repoMock.Object);

            var resultTasks = target.Search(new ThumbnailTaskSearchCriteria { Keyword = keyword });

            var count = ThumbnailTaskEntitysDataSource.Count(x => x.Name.Contains(keyword));
            Assert.Equal(resultTasks.Results.Count(), count);
        }

        public Mock<IThumbnailRepository> GetTaskRepositoryMock()
        {
            var entites = ThumbnailTaskEntitysDataSource.ToList();
            var entitesQuerableMock = TestUtils.CreateQuerableMock(entites);
            var repoMock = new Mock<IThumbnailRepository>();

            repoMock.Setup(x => x.ThumbnailTasks).Returns(entitesQuerableMock.Object);

            repoMock.Setup(x => x.GetThumbnailTasksByIds(It.IsAny<string[]>()))
                .Returns((string[] ids) =>
                {
                    return entitesQuerableMock.Object.Where(t => ids.Contains(t.Id)).ToArray();
                });

            return repoMock;
        }

        public IEnumerable<ThumbnailTaskEntity> ThumbnailTaskEntitysDataSource
        {
            get
            {
                yield return new ThumbnailTaskEntity() { Id = "Task1", Name = "Name 1", WorkPath = "Path 4" };
                yield return new ThumbnailTaskEntity() { Id = "Task2", Name = "NameLong 2", WorkPath = "Path 3" };
                yield return new ThumbnailTaskEntity() { Id = "Task3", Name = "Name 3", WorkPath = "Path 2" };
                yield return new ThumbnailTaskEntity() { Id = "Task4", Name = "NameLong 4", WorkPath = "Path 1" };
            }
        }
    }
}