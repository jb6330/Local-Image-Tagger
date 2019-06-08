namespace Image_Tagger
{
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

    /// <summary>Retrieves and stores records from the file system.</summary>
    public class Database
    {
        /// <summary>The list of all tags in the database.</summary>
        private HashSet<string> allTags;
        private List<PictureRecord> records;

        /// <summary>Initializes a new instance of the <see cref="Database"/> class.</summary>
        public Database()
        {
            this.records = new List<PictureRecord>();
            this.PictureRecords = this.records.AsReadOnly();
            this.allTags = new HashSet<string>();
        }

        /// <summary>Gets a value indicating whether changes have been made that are not yet saved.</summary>
        public bool HasUnsavedChanges
        {
            get;
            private set;
        }

        /// <summary>Gets the records that the database has loaded.</summary>
        public ReadOnlyCollection<PictureRecord> PictureRecords { get; private set; }

        /// <summary>Load a new set of records.</summary>
        /// <param name="path">The file to load the records from.</param>
        public void LoadRecords(string path)
        {
            XElement root = XElement.Load(path);
            foreach (var record in root.Elements())
            {
                PictureRecord newRecord = new PictureRecord
                {
                    Id = Guid.Parse(record.Attribute("GUID").Value),
                    FileLocation = record.Element("FileURI").Value,
                    Tags = new HashSet<string>(),
                };

                foreach (var tag in record.Element("Tags").Elements())
                {
                    this.allTags.Add(tag.Value.ToLowerInvariant());
                    newRecord.Tags.Add(tag.Value.ToLowerInvariant());
                }

                this.records.Add(newRecord);
            }
        }

        /// <summary>Save any changes to the records using the default directory.</summary>
        internal void SaveRecords()
        {
            this.SaveRecords(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.xml"));
        }

        /// <summary>Save any changes to a specific file.</summary>
        /// <param name="path">The file to change the changes to.</param>
        internal void SaveRecords(string path)
        {
            XElement root = new XElement("Records");
            foreach (PictureRecord record in this.records)
            {
                XElement entry = new XElement("Record");
                root.Add(entry);
                entry.SetAttributeValue("GUID", record.Id.ToString());
                entry.Add(new XElement("FileURI", record.FileLocation));
                XElement tagList = new XElement("Tags");
                foreach (string tag in record.Tags)
                {
                    tagList.Add(new XElement("Tag", tag));
                }

                entry.Add(tagList);
            }

            root.Save(path);
        }

        /// <summary>Load a new picture.</summary>
        /// <param name="fileName">The location of the picture.</param>
        /// <returns>The record of the picture. If the picture is already loaded, returns the existing record.</returns>
        internal PictureRecord AddPicture(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("File does not exists");
            }

            if (this.records.Exists(record => record.FileLocation == fileName))
            {
                return this.records.First(record => record.FileLocation == fileName);
            }

            PictureRecord newRecord = new PictureRecord
            {
                Id = Guid.NewGuid(),
                FileLocation = fileName,
                Tags = new HashSet<string>(),
            };

            this.records.Add(newRecord);
            return newRecord;
        }

        /// <summary>Add an entire folder to the database.</summary>
        /// <param name="path">The directory to add.</param>
        internal void AddFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Folder does not exist");
            }

            foreach (var file in Directory.GetFiles(path))
            {
                try
                {
                    this.AddPicture(file);
                }
                catch (ArgumentException)
                {
                    // TODO: log error
                }
            }

            foreach (var folder in Directory.GetDirectories(path))
            {
                this.AddFolder(folder);
            }
        }
    }
}
