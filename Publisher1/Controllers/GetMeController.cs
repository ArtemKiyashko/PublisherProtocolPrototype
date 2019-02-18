using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Publisher1.Contracts;

namespace Publisher1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/publisher")]
    [ApiController]
    public class GetmeController : Controller
    {

        private static Dictionary<string, Dictionary<int,object>> _allDistricts
        {
            get
            {
                var all = new Dictionary<string, Dictionary<int, object>>();
                all.Add("Warsaw", new Dictionary<int, object> { { 0, "Warsaw District 1" }, { 1, "Warsaw District 2" } });
                all.Add("Wroclaw", new Dictionary<int, object> { { 2, "Wroclaw District 1" }, { 3, "Wroclaw District 2" } });
                all.Add("Moscow", new Dictionary<int, object> { { 4, "Moscow District 1" }, { 5, "Moscow District 2" } });
                all.Add("New-York", new Dictionary<int, object> { { 6, "New-York District 1" }, { 7, "New-York District 2" } });
                return all;
            }
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var introduce = new Publisher1Contract
            {
                PublisherName = "Publisher 1",
                PublisherId = "myPublisher1Id",
                AuthorizeEndpoint = new Publisher1Endpoint
                {
                    Url = "authorize",
                    Method = "GET"
                },
                FieldsetEndpoint = new Publisher1Endpoint
                {
                    Url = "fields",
                    Method = "GET"
                },
                StatusEndpoint = new Publisher1Endpoint
                {
                    Url = "statusEndpointRelativeUrl",
                    Method = "GET"
                }
            };
            return Json(introduce);
        }

        [Route("referencedfields")]
        [HttpGet]
        public IActionResult ReferencedFields([FromBody]Publisher1Field filter)
        {
            foreach(var field in filter.ReferencedFields)
            {
                if (field.FieldName == "district")
                {
                    field.FieldOptions = _allDistricts.ContainsKey((string)filter.FieldValue) ? _allDistricts[(string)filter.FieldValue] : null;
                }
            }
            return Json(filter);
        }

        [Route("fields")]
        [HttpGet]
        public IActionResult Fields()
        {
            var rowSet = new Publisher1Rowset();

            var row1 = new Publisher1Fieldset {
                Fields = new List<Publisher1Field>{
                    new Publisher1Field
                    {
                        FieldName = "city",
                        IsRequired = true,
                        FieldDisplayName = "City",
                        FieldType = FieldType.Collection,
                        FieldOrderNumber = 0,
                        FieldOptions = new Dictionary<int, object>
                        {
                            {0, "Warsaw"},
                            {1, "Wroclaw"},
                            {2, "Moscow"},
                            {3, "New-York"},
                        },
                        ReferencedFields = new List<Publisher1Field>
                        {
                            new Publisher1Field
                            {
                                FieldName = "district",
                                FieldDisplayName = "District",
                                IsRequired = false,
                                FieldType = FieldType.Collection,
                                FieldOrderNumber = 1,
                                GetEndpoint = new Publisher1Endpoint
                                {
                                    Url = "referencedfields",
                                    Method = "GET"
                                }
                            }
                        }
                    },
                    new Publisher1Field
                    {
                        FieldName = "profession",
                        FieldDisplayName = "Profession",
                        FieldType = FieldType.Text,
                        FieldOrderNumber = 2
                    },
                    new Publisher1Field
                    {
                        FieldName = "location",
                        FieldDisplayName = "Location",
                        FieldType = FieldType.Text,
                        FieldOrderNumber = 3
                    }
                }
            };

            var row2 = new Publisher1Fieldset
            {
                Fields = new List<Publisher1Field>{
                    new Publisher1Field
                    {
                        FieldName = "dateFrom",
                        FieldDisplayName = "From",
                        FieldType = FieldType.Date,
                        FieldOrderNumber = 4
                    },
                    new Publisher1Field
                    {
                        FieldName = "dateTo",
                        FieldDisplayName = "To",
                        FieldType = FieldType.Date,
                        FieldOrderNumber = 5
                    }
                }
            };

            rowSet.Rows = new List<IPublisherFieldset> { row1, row2 };
            rowSet.SetEndpoint = new Publisher1Endpoint
            {
                Url = "fields",
                Method = "POST",
                Parameters = row1.Fields.Union(row2.Fields)
            };
            return Json(rowSet);
        }

        [Route("fields")]
        [HttpPost]
        public IActionResult Fields([FromBody]IEnumerable<Publisher1Field> fields)
        {
            Console.WriteLine(JsonConvert.SerializeObject(fields, Formatting.Indented));
            return Json(new Publisher1Result {
                Code = "CREATED",
                ResultId = Guid.NewGuid().ToString()
            });
        }

        [Route("authorize")]
        [HttpGet]
        public IActionResult Authorize()
        {
            var rowSet = new Publisher1Rowset();
            var Fields = new List<Publisher1Field>
            {
                new Publisher1Field
                {
                    FieldName = "login",
                    FieldDisplayName = "Login",
                    FieldType = FieldType.Text,
                    FieldOrderNumber = 0
                },
                new Publisher1Field
                {
                    FieldName = "pass",
                    FieldDisplayName = "Password",
                    FieldType = FieldType.Text,
                    FieldOrderNumber = 1
                },
                new Publisher1Field
                {
                    FieldName = "appsecret",
                    FieldDisplayName = "App Secret",
                    FieldType = FieldType.Text,
                    FieldOrderNumber = 2
                }
            };
            var fieldSet = new Publisher1Fieldset
            {
                Fields = new List<Publisher1Field>(Fields),
            };
            rowSet.Rows = new List<IPublisherFieldset> { fieldSet };
            rowSet.SetEndpoint = new Publisher1Endpoint
            {
                Url = "login",
                Method = "POST",
                Parameters = Fields
            };
            return Json(rowSet);
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody]IEnumerable<Publisher1Field> fields)
        {
            Console.WriteLine(JsonConvert.SerializeObject(fields, Formatting.Indented));
            return Ok();
        }
    }
}