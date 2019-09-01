using PdfGenerator.Models.HeaderAndFooter;

namespace PdfGenerator.Interface
{
    interface IGenericStructure
    {
        void SetHeader(HeaderElemment header);
        void SetFooter(FooterElemment footer);
       
    }
}
