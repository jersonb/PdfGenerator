using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Componets
{
    // Esta classe não está sendo utilizada, apagar no final do projeto
    internal class TableValues : BodyElemment
    {

        private TableValues(string titleTable, List<string> tableHeader, List<string> tableBody)
        {
            TitleTable = titleTable;
            TableHeader = tableHeader;
            TableBody = tableBody;
        }

        public static TableValues Create(string titleTable, List<string> tableHeader, List<string> tableBody)
        {
            return new TableValues( titleTable, tableHeader, tableBody);
        }

        public string TitleTable { get; private set; }
        public List<string> TableHeader { get; private set; }
        public List<string> TableBody { get; private set; }
    }
}
