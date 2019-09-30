using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerator.Models.Body.Componets
{
    internal class Accommodation : BodyElemment
    {
        public Accommodation(string title)
        {
            this.TitleBody = title;

        }
    }
}
