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
            try
            {
                LoadRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
            }catch(Exception e)
            {
                // TODO: log error
            }
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
        internal void SaveRecords()
        {
            SaveRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
        }

        internal void SaveRecords(string path)
        {
            XElement root = new XElement("Records");
            foreach(PictureRecord record in records)
            {
                XElement entry = new XElement("Record");
                root.Add(entry);
                entry.SetAttributeValue("GUID", record.guid.ToString());
                entry.Add(new XElement("FileURI",record.fileLocation));
                XElement tagList = new XElement("Tags");
                foreach(string tag in record.tags)
                {
                    tagList.Add(new XElement("Tag", tag));
                }
                entry.Add(tagList);
            }
            root.Save(path);
        }

        internal PictureRecord AddPicture(string fileName)
        {
            if (!File.Exists(fileName)) { throw new ArgumentException("File does not exists"); }
            if (records.Exists(record => record.fileLocation == fileName))
            {
                return records.First(record => record.fileLocation == fileName);
            }
            PictureRecord newRecord = new PictureRecord
            {
                guid = Guid.NewGuid(),
                fileLocation = fileName,
                tags = new HashSet<string>()
            };
            records.Add(newRecord);
            return newRecord;
        }

        internal void AddFolder(string path)
        {
            if (!Directory.Exists(path)) { throw new ArgumentException("Folder does not exist"); }
            foreach(var file in Directory.GetFiles(path))
            {
                try
                {
                    AddPicture(file);
                }
                catch (ArgumentException e)
                {
                    // TODO: log error
                }
            }
            foreach(var folder in Directory.GetDirectories(path))
            {
                AddFolder(folder);
            }
        }
    }
}
