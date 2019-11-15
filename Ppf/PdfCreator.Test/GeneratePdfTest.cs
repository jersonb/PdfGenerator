using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PdfGenerator.Test
{
    [TestClass]
    public class GeneratePdfTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            const string path = @"..\..\..\test\test.pdf";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var ms = PdfGenerator.Generator.DocumentGenerator.Generate())
            using (var fs = new FileStream(path, FileMode.Create))
            {
                ms.CopyTo(fs);
                fs.CopyTo(ms);
            }

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void GenerateCertify()
        {
            var list = new List<string>
            {
                "Aleixo López",
                //"André Parreira",
                //"Bernardete Gracia",
                //"Ermelinda Valentín",
                //"Filinto Themes",
                //"Rodolfo Gama",
                //"Sancho Montero",
                //"Socorro Botelho",
                //"Virgílio Cairu",
                //"Virgílio Pegado"
            };

            var pathRoot = @"..\..\..\test\";
            var pathPhoto = @"..\..\..\test\image_certicate.jpg";

            list.ToList().ForEach(name =>
            {
                var pathFile = $"{pathRoot}{name.ToUpper().Trim().Replace(" ", "_")}.pdf";

                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                using (var fs = new FileStream(pathFile, FileMode.Create))
                using (var ms = PdfGenerator.Generator.Certificados.Generate(name,pathPhoto))
                {
                    ms.CopyTo(fs);
                    fs.CopyTo(ms);

                    Assert.IsTrue(File.Exists(pathFile));
                }

            });

        }
    }
}
