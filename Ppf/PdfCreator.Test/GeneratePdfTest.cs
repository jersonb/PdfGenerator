using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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
    }
}
