using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publisher1.Contracts
{
    public class Publisher1Contract : IPublisherContract
    {
        public string PublisherName { get; set; }
        public IPublisherEndpoint AuthorizeEndpoint { get; set; }
        public IPublisherEndpoint PublishEndpoint { get; set; }
        public IPublisherEndpoint StatusEndpoint { get; set; }
        public IPublisherEndpoint FieldsetEndpoint { get; set; }
        public string PublisherDescription { get; set; }
        public string PublisherId { get; set; }
    }

    public class Publisher1Field : IPublisherField
    {
        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }
        public FieldType FieldType { get; set; }
        public object FieldValue { get; set; }
        public IDictionary<int, object> FieldOptions { get; set; }
        public int FieldOrderNumber { get; set; }
        public IPublisherEndpoint GetEndpoint { get; set; }
        public IEnumerable<IPublisherField> ReferencedFields { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }

        public Publisher1Field(IPublisherEndpoint getEndpoint, IEnumerable<IPublisherField> referencedFields)
        {
            GetEndpoint = getEndpoint;
            ReferencedFields = referencedFields;
        }

        public Publisher1Field()
        {
            GetEndpoint = new Publisher1Endpoint();
            ReferencedFields = new List<Publisher1Field>();
        }
    }

    public class Publisher1Endpoint : IPublisherEndpoint
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public IEnumerable<IPublisherField> Parameters { get; set; }

        public Publisher1Endpoint(IEnumerable<IPublisherField> parameters)
        {
            Parameters = parameters;
        }

        public Publisher1Endpoint()
        {
            Parameters = new List<Publisher1Field>();
        }
    }

    public class Publisher1Rowset : IPublisherRowset
    {
        public IEnumerable<IPublisherFieldset> Rows { get; set; }
        public IPublisherEndpoint SetEndpoint { get; set; }
    }

    public class Publisher1Fieldset : IPublisherFieldset
    {
        public IEnumerable<IPublisherField> Fields { get; set; }
    }

    public class Publisher1Result : IPublisherResult
    {
        public string Code { get; set; }
        public string ResultId { get; set; }
    }
}
