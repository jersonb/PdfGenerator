using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfGenerator.Test
{
    [TestClass]
    public class GeneratePdfTest
    {
        //[TestMethod]
        //public void GenerateTest()
        //{
        //    const string path = @"..\..\..\test\test.pdf";

        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //    }

        //    using (var ms = PdfGenerator.Generator.DocumentGenerator.Generate())
        //    using (var fs = new FileStream(path, FileMode.Create))
        //    {
        //        ms.CopyTo(fs);
        //        fs.CopyTo(ms);
        //    }

        //    Assert.IsTrue(File.Exists(path));
        //}

        [TestMethod]
        public void GenerateCertify()
        {
          
            var list = File.ReadAllLines(@"C:\Users\jerso\Downloads\PRODEPE\lista.txt", Encoding.Default);

            list.ToList().ForEach(x =>
            {
                using (var ms = PdfGenerator.Generator.Certificados.Generate(x.Trim()))
                using (var fs = new FileStream(@"C:\Users\jerso\Downloads\PRODEPE\CERTIFICADOS ASSINADOS\" + x.ToUpper().Trim().Replace(" ","_") + ".pdf", FileMode.Create))
                {
                    ms.CopyTo(fs);
                    fs.CopyTo(ms);
                }

            });




        }
    }
}
