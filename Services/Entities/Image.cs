﻿using System;
using Microsoft.WindowsAzure.Storage.Table;
using Services.Entities.JSON;

namespace Services.Entities
{
    public class Image : TableEntity
    {
        public Image() { }

        public Image(string name, Guid rowKey) : base(rowKey.ToString(), rowKey.ToString())
        {
            CreatedDate = DateTime.UtcNow;

            Name = name;
        }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Uri { get; set; }

        public string ThumbUri { get; set; }

        public string Description { get; set; }

        public ImageDescription OtherDescription => ImageDescription.FromJson(Description);
    }
}
