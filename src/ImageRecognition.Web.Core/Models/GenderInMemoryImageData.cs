using System;
using System.Collections.Generic;
using System.Text;

namespace FaceId.Web.Core.Models
{
    public class GenderInMemoryImageData : InMemoryImageData
    {
        public GenderInMemoryImageData(byte[] image, string label, string name) : base(image, label, name)
        {

        }
    }
}
