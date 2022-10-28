using FluentNHibernate.Mapping;
using HARS.Shared.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration.BaseComponents
{
    public class BaseEntityMapping<T> : ClassMap<T> where T : IEntity
    {
        public BaseEntityMapping()
        {
            Id(entity => entity.Id).GeneratedBy.GuidComb();
        }
    }

    public static class MappingExtensions
    {
        public static PropertyPart IsRequiredColumn(this PropertyPart mapping)
        {
            mapping.Not.Nullable();
            return mapping;
        }


        public static ManyToOnePart<T> MustHave<T>(this ManyToOnePart<T> mapping)
        {
            mapping.Not.Nullable();
            return mapping;
        }
    }
}
