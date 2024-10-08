﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.Models.Contracts
{
    public interface IEnqueuingCollection<T> : IList<T>
    {
        public Task EnqueueEnumerationAsync(IEnumerable<T> items, CancellationToken token);
    }
}
