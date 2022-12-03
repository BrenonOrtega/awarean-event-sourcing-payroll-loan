using System;
using System.Text.Json.Serialization;
using Awarean.EventSourcing.PayrollLoans.Api.Extensions;

namespace Awarean.EventSourcing.PayrollLoans.Api.Entities.Base
{
    public class Event<T, TEntity>
    {
        public T EntityId { get; set; }
        public string EntityType { get => typeof(TEntity).FullName; }
        public virtual string EventType { get => throw new NotImplementedException(); }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string SerializedEvent { get => this.AsJson(); }
    }
}