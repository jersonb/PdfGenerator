
namespace PdfGenerator.Models.Boddy.Components
{
    internal class Client : BoddyElemment
    {
        internal Client(string requester, string cnpjRequester, string affiliate, string cnpjAffiliate)
        {
            Requester = requester;
            CnpjRequester = cnpjRequester;
            Affiliate = affiliate;
            CnpjAffiliate = cnpjAffiliate;
        }

        internal Client()
        {
           
        }

        public string Requester { get; private set; }
        public string CnpjRequester { get; private set; }
        public string Affiliate { get; private set; }
        public string CnpjAffiliate { get; private set; }
    }
}
