using System;
using Awarean.EventSourcing.PayrollLoans.Api.Extensions;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.Base
{
    public class Event<T, TEntity>
    {
        public T EntityId { get; set; }
        public string Type { get => typeof(TEntity).FullName; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string SerializedEvent { get => this.AsJson(); }
    }
}