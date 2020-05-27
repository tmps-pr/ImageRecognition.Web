using System;
using System.Collections.Generic;
using System.Text;

namespace FaceId.Web.Core.Models
{
    public class InMemoryImageData
    {
        public InMemoryImageData(byte[] image, string label, string name)
        {
            Image = image;
            Label = label;
            Name = name;
        }

        public readonly byte[] Image;

        public readonly string Label;

        public readonly string Name;
    }
}
