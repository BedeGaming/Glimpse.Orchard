using System;
using System.Collections.Generic;
using Autofac;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Models;
using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.PerformanceMonitors;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.Records;
using Orchard.Data;
using Orchard.Data.Providers;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;
using Orchard;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Indexing;
using System.Xml.Linq;

namespace Glimpse.Orchard.AlternateImplementations
{
    [OrchardFeature("Glimpse.Orchard.ContentManager")]
    //[OrchardSuppressDependency("Orchard.ContentManagement.DefaultContentManager")]
    public class GlimpseContentManager : IDecorator<IContentManager>, IContentManager
    {
        private readonly IContentManager _decoratedService;
        private readonly IPerformanceMonitor _performanceMonitor;

        public GlimpseContentManager(IPerformanceMonitor performanceMonitor, IContentManager decoratedService)
        {
            _performanceMonitor = performanceMonitor;
            _decoratedService = decoratedService;
        }

        public IEnumerable<ContentTypeDefinition> GetContentTypeDefinitions()
        {
            return _decoratedService.GetContentTypeDefinitions();
        }

        public ContentItem New(string contentType)
        {
            return _decoratedService.New(contentType);
        }

        public void Create(ContentItem contentItem)
        {
            _decoratedService.Create(contentItem);
        }

        public void Create(ContentItem contentItem, VersionOptions options)
        {
            _decoratedService.Create(contentItem, options);
        }

        public ContentItem Clone(ContentItem contentItem)
        {
            return _decoratedService.Clone(contentItem);
        }

        public ContentItem Restore(ContentItem contentItem, VersionOptions options)
        {
            return _decoratedService.Restore(contentItem, options);
        }

        public virtual ContentItem Get(int id)
        {
            return Get(id, VersionOptions.Published);
        }

        public virtual ContentItem Get(int id, VersionOptions options)
        {
            return Get(id, options, QueryHints.Empty);
        }

        public ContentItem Get(int id, VersionOptions options, QueryHints hints)
        {
            return _performanceMonitor.PublishTimedAction(() => _decoratedService.Get(id, options, hints), (r, t) => new ContentManagerMessage
            {
                ContentId = id,
                ContentType = GetContentType(id, r, options),
                Name = r.GetContentName(),
                Duration = t.Duration,
                //VersionOptions = options
            }, TimelineCategories.ContentManagement, r => "Get: " + GetContentType(id, r, options), r => r.GetContentName()).ActionResult;
        }

        public IEnumerable<ContentItem> GetAllVersions(int id)
        {
            return _decoratedService.GetAllVersions(id);
        }

        public IEnumerable<T> GetMany<T>(IEnumerable<int> ids, VersionOptions options, QueryHints hints) where T : class, IContent
        {
            return _decoratedService.GetMany<T>(ids, options, hints);
        }

        public IEnumerable<T> GetManyByVersionId<T>(IEnumerable<int> versionRecordIds, QueryHints hints) where T : class, IContent
        {
            return _decoratedService.GetManyByVersionId<T>(versionRecordIds, hints);
        }

        public IEnumerable<ContentItem> GetManyByVersionId(IEnumerable<int> versionRecordIds, QueryHints hints)
        {
            return _decoratedService.GetManyByVersionId(versionRecordIds, hints);
        }

        public void Publish(ContentItem contentItem)
        {
            _decoratedService.Publish(contentItem);
        }

        public void Unpublish(ContentItem contentItem)
        {
            _decoratedService.Unpublish(contentItem);
        }

        public void Remove(ContentItem contentItem)
        {
            _decoratedService.Remove(contentItem);
        }

        public void Destroy(ContentItem contentItem)
        {
            _decoratedService.Destroy(contentItem);
        }

        public void Index(ContentItem contentItem, IDocumentIndex documentIndex)
        {
            _decoratedService.Index(contentItem, documentIndex);
        }

        public XElement Export(ContentItem contentItem)
        {
            return _decoratedService.Export(contentItem);
        }

        public void Import(XElement element, ImportContentSession importContentSession)
        {
            _decoratedService.Import(element, importContentSession);
        }

        public void Clear()
        {
            _decoratedService.Clear();
        }

        public IContentQuery<ContentItem> Query()
        {
            return _decoratedService.Query();
        }

        public IHqlQuery HqlQuery()
        {
            return _decoratedService.HqlQuery();
        }

        public ContentItemMetadata GetItemMetadata(IContent contentItem)
        {
            return _decoratedService.GetItemMetadata(contentItem);
        }

        public IEnumerable<GroupInfo> GetEditorGroupInfos(IContent contentItem)
        {
            return _decoratedService.GetEditorGroupInfos(contentItem);
        }

        public IEnumerable<GroupInfo> GetDisplayGroupInfos(IContent contentItem)
        {
            return _decoratedService.GetDisplayGroupInfos(contentItem);
        }

        public GroupInfo GetEditorGroupInfo(IContent contentItem, string groupInfoId)
        {
            return _decoratedService.GetEditorGroupInfo(contentItem, groupInfoId);
        }

        public GroupInfo GetDisplayGroupInfo(IContent contentItem, string groupInfoId)
        {
            return _decoratedService.GetDisplayGroupInfo(contentItem, groupInfoId);
        }

        public ContentItem ResolveIdentity(ContentIdentity contentIdentity)
        {
            return _decoratedService.ResolveIdentity(contentIdentity);
        }

        public dynamic BuildDisplay(IContent content, string displayType = "", string groupId = "")
        {
            return _decoratedService.BuildDisplay(content, displayType, groupId);
        }

        public dynamic BuildEditor(IContent content, string groupId = "")
        {
            return _decoratedService.BuildEditor(content, groupId);
        }

        public dynamic UpdateEditor(IContent content, IUpdateModel updater, string groupId = "")
        {
            return _decoratedService.UpdateEditor(content, updater, groupId);
        }

        private string GetContentType(int id, ContentItem item, VersionOptions options = null)
        {
            if (item != null)
            {
                return item.ContentType;
            }

            if (options == null)
            {
                return "Unknown content type.";
            }

            return (options.VersionRecordId == 0) ? string.Format("Content item: {0} is not published.", id) : "Unknown content type.";
        }
    }
}
