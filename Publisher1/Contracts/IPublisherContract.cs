using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Publisher1.Contracts
{
    public interface IPublisherField
    {
        
        string FieldName { get; set; }
        
        string FieldDisplayName { get; set; }
        
        FieldType FieldType { get; set; }
        object FieldValue { get; set; }
        IDictionary<int, object> FieldOptions { get; set; }
        
        int FieldOrderNumber { get; set; }
        
        bool IsRequired { get; set; }
        
        bool IsVisible { get; set; }
        IPublisherEndpoint GetEndpoint { get; set; }
        IEnumerable<IPublisherField> ReferencedFields { get; set; }
    }

    public interface IPublisherEndpoint
    {
        
        string Url { get; set; }
        
        string Method { get; set; }
        IEnumerable<IPublisherField> Parameters { get; set; }
    }

    public interface IPublisherContract
    {
        string PublisherName { get; set; }
        string PublisherDescription { get; set; }
        
        string PublisherId { get; set; }
        
        IPublisherEndpoint AuthorizeEndpoint { get; set; }
        
        IPublisherEndpoint StatusEndpoint { get; set; }
        
        IPublisherEndpoint FieldsetEndpoint { get; set; }
    }

    public interface IPublisherRowset
    {
        
        IEnumerable<IPublisherFieldset> Rows { get; set; }
        IPublisherEndpoint SetEndpoint { get; set; }
    }

    public interface IPublisherFieldset
    {
        IEnumerable<IPublisherField> Fields { get; set; }
    }

    public interface IPublisherResult
    {
        string Code { get; set; }
        string ResultId { get; set; }
    }

    public enum FieldType
    {
        Integer,
        Text,
        Boolean,
        Decimal,
        Collection,
        Date
    }
}
