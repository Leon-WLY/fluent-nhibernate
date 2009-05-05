using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IJoin : IClasslike, IMappingPart
    {
        void WithKeyColumn(string column);
    }
    /// <summary>
    /// Maps to the Join element in NH 2.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JoinPart<T> : ClasslikeMapBase<T>, IJoin
    {
        private readonly Cache<string, string> localProperties = new Cache<string, string>();
        private string keyColumnName;

        public JoinPart(string tableName)
        {
            localProperties.Store("table", tableName);
            keyColumnName = GetType().GetGenericArguments()[0].Name + "ID";
        }

        public void SetAttribute(string name, string value)
        {
            localProperties.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            var joinElement = classElement.AddElement("join")
                .WithProperties(localProperties);

            joinElement.AddElement("key")
                .SetAttribute("column", keyColumnName);

            WriteTheParts(joinElement, visitor);
        }

        public int LevelWithinPosition
        {
            get { return 3; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Last; }
        }

        public void WithKeyColumn(string column)
        {
            keyColumnName = column;
        }
    }
}