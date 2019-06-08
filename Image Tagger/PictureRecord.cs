namespace Image_Tagger
{
    using System;
    using System.Collections.Generic;

    /// <summary>Represents an idividual picture and its tags.</summary>
    public struct PictureRecord
    {
        /// <summary>A unique identifier for this record.</summary>
        public Guid Id;

        /// <summary>The path to the location of the image on disk.</summary>
        public string FileLocation;

        /// <summary>The tags that the image has.</summary>
        public HashSet<string> Tags;
    }
}
