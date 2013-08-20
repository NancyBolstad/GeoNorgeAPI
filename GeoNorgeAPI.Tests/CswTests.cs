﻿using NUnit.Framework;
using www.opengis.net;

namespace GeoNorgeAPI.Tests
{
    
    public class CswTests
    {
        GeoNorge _geonorge;

        [SetUp]
        public void Init()
        {
            _geonorge = new GeoNorge();
        }

        [Test]
        public void ShouldReturnRecordsWhenRunningASimpleSearch()
        {
            
            var result = _geonorge.Search("wms");

            Assert.Greater(int.Parse(result.numberOfRecordsMatched), 0, "A search on 'wms' should return records.");
        }
        
        [Test]
        public void ShouldReturnSingleIsoRecord()
        {
            MD_Metadata_Type record = _geonorge.GetRecordByUuid("63c672fa-e180-4601-a176-6bf163e0929d"); // Matrikkelen WMS
            
            Assert.NotNull(record, "Record does not exist.");
        }

        [Test]
        public void ShouldReturnRecordsWhenSearchingWithOrganisationName()
        {
            var result = _geonorge.SearchWithOrganisationName("%Kartverket%");

            Assert.Greater(int.Parse(result.numberOfRecordsMatched), 0, "An organization name search on '%Kartverket%' should return lots of records.");
        }

        [Test]
        public void ShouldReturnServicesFromKartverket()
        {
            var filters = new object[]
                {
                    
                    new BinaryLogicOpType()
                        {
                            Items = new object[]
                                {
                                    new PropertyIsLikeType
                                    {
                                        escapeChar = "\\",
                                        singleChar = "_",
                                        wildCard = "%",
                                        PropertyName = new PropertyNameType {Text = new[] {"OrganisationName"}},
                                        Literal = new LiteralType {Text = new[] { "%Kartverket%" }}
                                    },
                                    new PropertyIsLikeType
                                    {
                                        PropertyName = new PropertyNameType {Text = new[] {"Type"}},
                                        Literal = new LiteralType {Text = new[] { "service" }}
                                    }
                                },
                                ItemsElementName = new ItemsChoiceType22[]
                                    {
                                        ItemsChoiceType22.PropertyIsLike, ItemsChoiceType22.PropertyIsLike, 
                                    }
                        },
                    
                };

            var filterNames = new ItemsChoiceType23[]
                {
                    ItemsChoiceType23.And
                };

            var result = _geonorge.SearchWithFilters(filters, filterNames);

            Assert.Greater(int.Parse(result.numberOfRecordsMatched), 0, "Should have return more than zero datasets from Kartverket.");
        }

        /*
        [Test]
        public void InsertMetadata()
        {
            _geonorge = new GeoNorge("", "", "http://beta.geonorge.no/geonetwork/");

            MD_Metadata_Type metadata = MetadataExample.CreateMetadataExample();

            _geonorge.MetadataInsert(metadata);
        }

        [Test]
        public void UpdateMetadata()
        {
            _geonorge = new GeoNorge("", "", "http://beta.geonorge.no/geonetwork/");

            MD_Metadata_Type metadata = MetadataExample.CreateMetadataExample();

            _geonorge.MetadataUpdate(metadata);
        }
        */
    }
}
