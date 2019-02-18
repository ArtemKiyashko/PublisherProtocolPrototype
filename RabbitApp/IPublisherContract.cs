using System;
using System.Collections.Generic;
using System.Text;

namespace Sender
{
    public interface IPublisherField
    {
        string FieldName { get; set; }
        string FieldDisplayName { get; set; }
        string FieldType { get; set; }
        IDictionary<int, object> FieldValues { get; set; }
        int FieldOrderNumber { get; set; }
        IPublisherEndpoint GetEndpoint { get; set; }
        IPublisherEndpoint SetEndpoint { get; set; }
        IEnumerable<IPublisherField> ReferencedFields { get; set; }
    }

    public interface IPublisherEndpoint
    {
        string Url { get; set; }
        string Type { get; set; }
        IEnumerable<IPublisherField> Parameters { get; set; }
    }

    public interface IPublisherContract
    {
        IEnumerable<IPublisherField> Fields { get; set; }
    }
}
