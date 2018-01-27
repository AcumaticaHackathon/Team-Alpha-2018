using PX.Common;
using PX.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Jamis.Web.Face
{
    public class FaceApiStore
    {
        private readonly PXGraph Graph;

        internal FaceApiStore(PXGraph graph)
        {
            this.Graph = graph;
        }

        public IEnumerable<Table> Cache<Table>(Func<IEnumerable<Table>> apiMethod) where Table : class, IBqlTable, new()
        {
            return Cache<Table>(string.Empty, apiMethod);
        }

        public IEnumerable<Table> Cache<Table>(string parentKey, Func<string, IEnumerable<Table>> apiMethod) where Table : class, IBqlTable, new()
        {
            return Cache<Table>(parentKey, () => apiMethod(parentKey));
        }

        public IEnumerable<Table> Cache<Table>(string parentKey, Func<IEnumerable<Table>> apiMethod) where Table : class, IBqlTable, new()
        {
            var inCache = new Dictionary<string, Table>();

            var cache = Graph.Caches<Table>();

            foreach (Table item in cache.Cached)
            {
                var name = (string)cache.GetValue(item, "name");

                if (name != null && inCache.ContainsKey(name) == false)
                {
                    inCache.Add(name, item);
                }

                var status = cache.GetStatus(item);

                if (status == PXEntryStatus.Inserted || status == PXEntryStatus.Updated || status == PXEntryStatus.Notchanged)
                {
                    yield return item;
                }
            }

            bool isDirty = cache.IsDirty;

            foreach (object item in GetRecords(parentKey, apiMethod))
            {
                var key = (cache.GetValue(item, "id") ?? cache.GetValue(item, "name")).ToString();

                if (!inCache.ContainsKey(key))
                {
                    var record = cache.Insert(item) as Table;

                    if (record != null)
                    {
                        cache.SetStatus(record, PXEntryStatus.Notchanged);

                        yield return record;
                    }
                }
            }

            cache.IsDirty = isDirty;
        }

        public void Clear<Table>() where Table : class, IBqlTable, new()
        {
            var recordsKey = $"{typeof(Table).FullName}.Cache";

            PXContext.SetSlot<IEnumerable<Table>>(recordsKey, null);
        }

        public void Clear<Table>(string parentKey) where Table : class, IBqlTable, new()
        {
            var recordsKey = $"{typeof(Table).FullName}.Cache";

            var store = PXContext.GetSlot<IDictionary<string, IEnumerable<Table>>>(recordsKey);

            if (store != null)
            {
                if (store.ContainsKey(parentKey))
                {
                    store.Remove(parentKey);
                }
            }
        }

        private IEnumerable GetRecords<Table>(string parentKey, Func<IEnumerable<Table>> apiMethod) where Table : class, IBqlTable, new()
        {
            var recordsKey = $"{typeof(Table).FullName}.Cache";

            var store = PXContext.GetSlot<IDictionary<string, IEnumerable<Table>>>(recordsKey);

            if (store == null)
            {
                store = new Dictionary<string, IEnumerable<Table>>();

                PXContext.SetSlot<IDictionary<string, IEnumerable<Table>>>(recordsKey, store);
            }

            IEnumerable<Table> records;

            if (store.TryGetValue(parentKey, out records) == false)
            {
                records = apiMethod().ToArray<Table>();

                store[parentKey] = records;
            }

            return records;
        }
    }
}