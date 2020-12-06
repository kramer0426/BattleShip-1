using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class SoundTable : MonoBehaviour
{
    public DataEntity tableData_;

    public void LoadData(string tableName)
    {
        TextAsset xmlFile = (TextAsset)Resources.Load(tableName);

        string xmlData = xmlFile.text;

        XmlSerializer toData = new XmlSerializer(typeof(DataEntity));

        StringReader reader = new StringReader(xmlData);

        tableData_ = toData.Deserialize(reader) as DataEntity;

    }

    public int GetDataSize()
    {
        return tableData_.Datas.Length;
    }

    public DataEntity GetData()
    {
        return tableData_;
    }


    [XmlRoot("SoundList")]
    public class DataEntity
    {
        private List<SoundEntity> datas_ = new List<SoundEntity>();

        [XmlArray("Sounds")]
        [XmlArrayItem("sound")]
        public SoundEntity[] Datas
        {
            get { return datas_.ToArray(); }
            set { datas_ = new List<SoundEntity>(value); }
        }
    }

    [XmlRoot("sound")]
    public class SoundEntity
    {
        private int id;
        private int type;
        private string resourceName;


        [XmlAttribute("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlAttribute("type")]
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        [XmlAttribute("resourceName")]
        public string ResourceName
        {
            get { return resourceName; }
            set { resourceName = value; }
        }


    }
}
