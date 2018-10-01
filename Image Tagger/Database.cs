using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Image_Tagger
{
    struct PictureRecord
    {
        public Guid guid;
        public string fileLocation;
        public HashSet<string> tags;
    }

    class Database
    {
        private List<PictureRecord> records;
        public ReadOnlyCollection<PictureRecord> PictureRecords;

        public Database()
        {
            records = new List<PictureRecord>();
            LoadRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
            PictureRecords = records.AsReadOnly();
        }


        public bool HasUnsavedChanges
        {
            get;
            set;
        }

         

        public void LoadRecords(string path)
        {
            HashSet<string> foundTags = new HashSet<string>();

            XElement root = XElement.Load(path);
            foreach (var record in root.Elements())
            {
                PictureRecord newRecord = new PictureRecord();
                newRecord.guid = Guid.Parse(record.Attribute("GUID").Value);
                newRecord.fileLocation = record.Element("FileURI").Value;
                newRecord.tags = new HashSet<string>();
                foreach (var tag in record.Element("Tags").Elements())
                {
                    foundTags.Add(tag.Value);
                    newRecord.tags.Add(tag.Value);
                }
                records.Add(newRecord);
            }
        }
    }
}
