using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class LocalizeTextTable : MonoBehaviour
{

    public DataEntity data_;

    public void LoadData(string tableName)
    {
        TextAsset xmlFile = (TextAsset)Resources.Load(tableName);

        string xmlData = xmlFile.text;

        XmlSerializer toData = new XmlSerializer(typeof(DataEntity));

        StringReader reader = new StringReader(xmlData);

        data_ = toData.Deserialize(reader) as DataEntity;
    }

    public int GetDataSize()
    {
        return data_.Texts.Length;
    }

    public DataEntity GetData()
    {
        return data_;
    }


    [XmlRoot("LocalizeList")]
    public class DataEntity
    {
        private List<TextEntity> texts_ = new List<TextEntity>();

        [XmlArray("Texts")]
        [XmlArrayItem("Text")]
        public TextEntity[] Texts
        {
            get { return texts_.ToArray(); }
            set { texts_ = new List<TextEntity>(value); }
        }
    }

    [XmlRoot("Text")]
    public class TextEntity
    {
        private int id_;
        private string krString_;
        private string engString_;
        private string jpString_;
        private string cnString_;
        private string twString_;

        [XmlAttribute("textId")]
        public int Id
        {
            get { return id_; }
            set { id_ = value; }
        }

        [XmlAttribute("krString")]
        public string KrString
        {
            get { return krString_; }
            set { krString_ = value; }
        }

        [XmlAttribute("engString")]
        public string EngString
        {
            get { return engString_; }
            set { engString_ = value; }
        }

        [XmlAttribute("jpString")]
        public string JpString
        {
            get { return jpString_; }
            set { jpString_ = value; }
        }

        [XmlAttribute("cnString")]
        public string CnString
        {
            get { return cnString_; }
            set { cnString_ = value; }
        }

        [XmlAttribute("twString")]
        public string TwString
        {
            get { return twString_; }
            set { twString_ = value; }
        }


    }
}
