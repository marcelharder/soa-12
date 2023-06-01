using System.Collections.Generic;
using System.Xml.Serialization;
using api.Implementations;

namespace api.Entities
{
    public class InstitutionalReport
    {
        public Soort soort { get; set; }
        public TextByTypeOfSurgery tbt { get; set; }
        public Items it { get; set; }
        public CirculationSupport circ { get; set; }
        public Iabp ia { get; set; }
        public Pmwires pm { get; set; }
        public Reports rep { get; set; }
        public Hospital hos { get; set; }
    }


    [XmlRoot(ElementName = "soort")]
    public class Soort
    {

        [XmlElement(ElementName = "regel_1_a")]
        public string Regel1A { get; set; }

        [XmlElement(ElementName = "regel_1_b")]
        public string Regel1B { get; set; }

        [XmlElement(ElementName = "regel_1_c")]
        public string Regel1C { get; set; }

        [XmlElement(ElementName = "regel_2_a")]
        public string Regel2A { get; set; }

        [XmlElement(ElementName = "regel_2_b")]
        public object Regel2B { get; set; }

        [XmlElement(ElementName = "regel_2_c")]
        public string Regel2C { get; set; }

        [XmlElement(ElementName = "regel_3_a")]
        public string Regel3A { get; set; }

        [XmlElement(ElementName = "regel_3_b")]
        public object Regel3B { get; set; }

        [XmlElement(ElementName = "regel_3_c")]
        public object Regel3C { get; set; }

        [XmlElement(ElementName = "regel_4_a")]
        public string Regel4A { get; set; }

        [XmlElement(ElementName = "regel_4_b")]
        public object Regel4B { get; set; }

        [XmlElement(ElementName = "regel_4_c")]
        public object Regel4C { get; set; }

        [XmlElement(ElementName = "regel_5_a")]
        public string Regel5A { get; set; }

        [XmlElement(ElementName = "regel_5_b")]
        public object Regel5B { get; set; }

        [XmlElement(ElementName = "regel_5_c")]
        public string Regel5C { get; set; }

        [XmlElement(ElementName = "regel_6_a")]
        public string Regel6A { get; set; }

        [XmlElement(ElementName = "regel_6_b")]
        public object Regel6B { get; set; }

        [XmlElement(ElementName = "regel_6_c")]
        public string Regel6C { get; set; }

        [XmlElement(ElementName = "regel_7_a")]
        public string Regel7A { get; set; }

        [XmlElement(ElementName = "regel_7_b")]
        public object Regel7B { get; set; }

        [XmlElement(ElementName = "regel_7_c")]
        public object Regel7C { get; set; }

        [XmlElement(ElementName = "regel_8_a")]
        public string Regel8A { get; set; }

        [XmlElement(ElementName = "regel_8_b")]
        public object Regel8B { get; set; }

        [XmlElement(ElementName = "regel_8_c")]
        public object Regel8C { get; set; }

        [XmlElement(ElementName = "regel_9_a")]
        public object Regel9A { get; set; }

        [XmlElement(ElementName = "regel_9_b")]
        public object Regel9B { get; set; }

        [XmlElement(ElementName = "regel_9_c")]
        public object Regel9C { get; set; }

        [XmlElement(ElementName = "regel_10_a")]
        public object Regel10A { get; set; }

        [XmlElement(ElementName = "regel_10_b")]
        public object Regel10B { get; set; }

        [XmlElement(ElementName = "regel_10_c")]
        public object Regel10C { get; set; }

        [XmlElement(ElementName = "regel_11_a")]
        public object Regel11A { get; set; }

        [XmlElement(ElementName = "regel_11_b")]
        public object Regel11B { get; set; }

        [XmlElement(ElementName = "regel_11_c")]
        public object Regel11C { get; set; }

        [XmlElement(ElementName = "regel_12_a")]
        public object Regel12A { get; set; }

        [XmlElement(ElementName = "regel_12_b")]
        public object Regel12B { get; set; }

        [XmlElement(ElementName = "regel_12_c")]
        public object Regel12C { get; set; }

        [XmlElement(ElementName = "regel_13_a")]
        public object Regel13A { get; set; }

        [XmlElement(ElementName = "regel_13_b")]
        public object Regel13B { get; set; }

        [XmlElement(ElementName = "regel_13_c")]
        public object Regel13C { get; set; }

        [XmlElement(ElementName = "regel_14_a")]
        public object Regel14A { get; set; }

        [XmlElement(ElementName = "regel_14_b")]
        public object Regel14B { get; set; }

        [XmlElement(ElementName = "regel_14_c")]
        public object Regel14C { get; set; }

        [XmlElement(ElementName = "regel_15")]
        public string Regel15 { get; set; }

        [XmlElement(ElementName = "regel_16")]
        public string Regel16 { get; set; }

        [XmlElement(ElementName = "regel_17")]
        public object Regel17 { get; set; }

        [XmlElement(ElementName = "regel_18")]
        public object Regel18 { get; set; }

        [XmlElement(ElementName = "regel_19")]
        public object Regel19 { get; set; }

        [XmlElement(ElementName = "regel_20")]
        public object Regel20 { get; set; }

        [XmlElement(ElementName = "regel_21")]
        public object Regel21 { get; set; }

        [XmlElement(ElementName = "regel_22")]
        public object Regel22 { get; set; }

        [XmlElement(ElementName = "regel_23")]
        public object Regel23 { get; set; }

        [XmlElement(ElementName = "regel_24")]
        public object Regel24 { get; set; }

        [XmlElement(ElementName = "regel_25")]
        public object Regel25 { get; set; }

        [XmlElement(ElementName = "regel_26")]
        public object Regel26 { get; set; }

        [XmlElement(ElementName = "regel_27")]
        public object Regel27 { get; set; }

        [XmlElement(ElementName = "regel_28")]
        public object Regel28 { get; set; }

        [XmlElement(ElementName = "regel_29")]
        public object Regel29 { get; set; }

        [XmlElement(ElementName = "regel_30")]
        public object Regel30 { get; set; }

        [XmlElement(ElementName = "regel_31")]
        public object Regel31 { get; set; }

        [XmlElement(ElementName = "regel_32")]
        public object Regel32 { get; set; }

        [XmlElement(ElementName = "regel_33")]
        public object Regel33 { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "text_by_type_of_surgery")]
    public class TextByTypeOfSurgery
    {

        [XmlElement(ElementName = "soort")]
        public List<Soort> Soort { get; set; }
    }

    [XmlRoot(ElementName = "items")]
    public class Items
    {

        [XmlElement(ElementName = "regel_21")]
        public string Regel21 { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlElement(ElementName = "regel_22")]
        public object Regel22 { get; set; }

        [XmlElement(ElementName = "regel_23")]
        public string Regel23 { get; set; }
    }

    [XmlRoot(ElementName = "circulation_support")]
    public class CirculationSupport
    {

        [XmlElement(ElementName = "items")]
        public List<Items> Items { get; set; }
    }

    [XmlRoot(ElementName = "iabp")]
    public class Iabp
    {

        [XmlElement(ElementName = "items")]
        public List<Items> Items { get; set; }
    }

    [XmlRoot(ElementName = "pmwires")]
    public class Pmwires
    {

        [XmlElement(ElementName = "items")]
        public List<Items> Items { get; set; }
    }

    [XmlRoot(ElementName = "reports")]
    public class Reports
    {

        [XmlElement(ElementName = "text_by_type_of_surgery")]
        public TextByTypeOfSurgery TextByTypeOfSurgery { get; set; }

        [XmlElement(ElementName = "circulation_support")]
        public CirculationSupport CirculationSupport { get; set; }

        [XmlElement(ElementName = "iabp")]
        public Iabp Iabp { get; set; }

        [XmlElement(ElementName = "pmwires")]
        public Pmwires Pmwires { get; set; }
    }

    [XmlRoot(ElementName = "hospital")]
    public class Hospital
    {

        [XmlElement(ElementName = "reports")]
        public Reports Reports { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

}
