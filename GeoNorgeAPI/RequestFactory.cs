﻿using System.Xml;
using www.opengis.net;

namespace GeoNorgeAPI
{
    public class RequestFactory
    {
        public GetRecordsType GetRecordsFreeTextSearch(string searchString, int startPosition = 1, int limit = 20, string outputSchema = "csw:Record")
        {
            var filters = new object[]
                {
                    new PropertyIsLikeType
                        {
                            escapeChar = "\\",
                            singleChar = "_",
                            wildCard = "%",
                            PropertyName = new PropertyNameType {Text = new[] {"AnyText"}},
                            Literal = new LiteralType {Text = new[] {searchString}}
                        }
                };

            var filterNames = new ItemsChoiceType23[]
                {
                    ItemsChoiceType23.PropertyIsLike, 
                };

            return GetRecordsWithFilter(filters, filterNames, startPosition, limit, outputSchema);
        }

        public GetRecordByIdType GetRecordById(string uuid)
        {
            GetRecordByIdType getRecordbyId = new GetRecordByIdType();
            getRecordbyId.service = "CSW";
            getRecordbyId.version = "2.0.2";
            getRecordbyId.outputSchema = "csw:IsoRecord";
            getRecordbyId.Id = new[] { uuid };
            getRecordbyId.ElementSetName = new ElementSetNameType { Value = ElementSetType.full };
            return getRecordbyId;
        }

        public GetRecordsType GetRecordsOrganisationNameSearch(string searchString, int startPosition = 1, int limit = 20)
        {
            searchString = searchString.Replace(" ", "_");

            var filters = new object[]
                {
                    new PropertyIsLikeType
                        {
                            escapeChar = "\\",
                            singleChar = "_",
                            wildCard = "%",
                            PropertyName = new PropertyNameType {Text = new[] {"OrganisationName"}},
                            Literal = new LiteralType {Text = new[] { searchString }}
                        }
                };
            var filterNames = new ItemsChoiceType23[]
                {
                    ItemsChoiceType23.PropertyIsLike, 
                };

            return GetRecordsWithFilter(filters, filterNames, startPosition, limit);
        }

        public GetRecordsType GetRecordsWithFilter(object[] filters, ItemsChoiceType23[] filterNames, int startPosition = 1, int limit = 20, string outputSchema = "csw:Record")
        {
            var getRecords = new GetRecordsType();
            getRecords.resultType = ResultType1.results;
            getRecords.startPosition = startPosition.ToString();
            getRecords.maxRecords = limit.ToString();
            getRecords.outputSchema = outputSchema;

            var query = new QueryType();
            query.typeNames = new[] { new XmlQualifiedName("Record", "http://www.opengis.net/cat/csw/2.0.2") };
            var queryConstraint = new QueryConstraintType();
            queryConstraint.version = "1.1.0";
            queryConstraint.Item = new FilterType
            {
                Items = filters,
                ItemsElementName = filterNames
            };
            query.Constraint = queryConstraint;
            query.Items = new object[] { new ElementSetNameType { Value = ElementSetType.full } };
            getRecords.Item = query;

            return getRecords;
        }

        public TransactionType MetadataInsert(MD_Metadata_Type metadata)
        {
            TransactionType cswTransaction = new TransactionType
            {
                service = "CSW",
                version = "2.0.2",
                Items = new object[]
                    {
                        new InsertType()
                            {
                                Items = new object[]
                                    {
                                        metadata
                                    }
                            }
                    }
            };

            return cswTransaction;
        }

        public TransactionType MetadataUpdate(MD_Metadata_Type metadata)
        {
            TransactionType cswTransaction = new TransactionType
            {
                service = "CSW",
                version = "2.0.2",
                Items = new object[]
                    {
                        new UpdateType()
                            {
                                Items = new object[]
                                    {
                                        metadata
                                    }
                            }
                    }
            };
            return cswTransaction;

        }

        public TransactionType MetadataDelete(string uuid)
        {
            TransactionType cswTransaction = new TransactionType
                {
                    service = "CSW",
                    version = "2.0.2",
                    Items = new object[]
                        {
                            new DeleteType()
                                {
                                    Constraint = new QueryConstraintType()
                                        {
                                            version = "1.0.0",
                                            Item = new FilterType()
                                                {
                                                    Items = new object[]
                                                        {
                                                            new PropertyIsLikeType()
                                                            {
                                                                wildCard = "%",
                                                                singleChar = "_",
                                                                escapeChar = "/",
                                                                PropertyName = new PropertyNameType()
                                                                    {
                                                                        Text = new []
                                                                            {
                                                                                "apiso:identifier"
                                                                            }
                                                                    },
                                                                Literal = new LiteralType()
                                                                    {
                                                                        Text = new []
                                                                            {
                                                                                uuid
                                                                            }
                                                                    }
                                                            }   
                                                        },
                                                        ItemsElementName = new ItemsChoiceType23[]
                                                            {
                                                                ItemsChoiceType23.PropertyIsLike,
                                                            }
                                                }
                                        }
                                }
                        }
                };
            return cswTransaction;
        }
    }
}