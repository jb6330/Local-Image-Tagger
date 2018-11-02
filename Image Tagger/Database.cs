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
        public HashSet<string> AllTags;

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
            AllTags = new HashSet<string>();

            XElement root = XElement.Load(path);
            foreach (var record in root.Elements())
            {
                PictureRecord newRecord = new PictureRecord
                {
                    guid = Guid.Parse(record.Attribute("GUID").Value),
                    fileLocation = record.Element("FileURI").Value,
                    tags = new HashSet<string>()
                };
                foreach (var tag in record.Element("Tags").Elements())
                {
                    AllTags.Add(tag.Value.ToLowerInvariant());
                    newRecord.tags.Add(tag.Value.ToLowerInvariant());
                }
                records.Add(newRecord);
            }
        }
    }
}
