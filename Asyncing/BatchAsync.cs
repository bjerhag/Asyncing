using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asyncing
{
    public static class BatchAsync
    {
        public static async Task<IEnumerable<T>> ExecuteInBatchAsync<T, TY>(Func<IEnumerable<TY>, Task<IEnumerable<T>>> asyncCall, IEnumerable<TY> data, int batchSize)
        {
            var result = new List<T>();
            var batches = data.Batch(batchSize);
            foreach (var batch in batches)
            {
                var batchList = batch.ToList();
                var tasks = batchList.Select(b => asyncCall(batchList)).ToList();
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    result = result.Concat(task.Result).ToList();
                }
            }
            return result;
        }
        public static async Task<IEnumerable<T>> ExecuteInBatchAsync<T, TY>(Func<IEnumerable<TY>, Task<T>> asyncCall, IEnumerable<TY> data, int batchSize)
        {
            var result = new List<T>();
            var batches = data.Batch(batchSize);
            foreach (var batch in batches)
            {
                var batchList = batch.ToList();
                var tasks = batchList.Select(b => asyncCall(batchList)).ToList();
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    result.Add(task.Result);
                }
            }
            return result;
        }

    }
}
