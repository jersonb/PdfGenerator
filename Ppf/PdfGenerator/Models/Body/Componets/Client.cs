
namespace PdfGenerator.Models.Body.Components
{
    public class Client : BodyElemment
    {
        private Client(string requester, string cnpjRequester, string affiliate, string cnpjAffiliate)
        {
            Requester = requester;
            CnpjRequester = cnpjRequester;
            Affiliate = affiliate;
            CnpjAffiliate = cnpjAffiliate;
        }

        public static Client Create(string requester, string cnpjRequester, string affiliate, string cnpjAffiliate)
        {
            return new Client(requester, cnpjRequester, affiliate, cnpjAffiliate);
        }

        public string Requester { get; private set; }
        public string CnpjRequester { get; private set; }
        public string Affiliate { get; private set; }
        public string CnpjAffiliate { get; private set; }
    }
}
