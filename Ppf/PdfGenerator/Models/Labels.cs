﻿
namespace PdfGenerator.Models.Label
{
    internal abstract class LabelHeader
    {
        public const string DATA_DA_SOLICITACAO = "DATA DA SOLICITAÇÃO";
        public const string ID_DA_SOLICITACAO = "ID DA SOLICITAÇÃO";
        public const string NOME_DO_EVENTO = "NOME DO EVENTO";

        private LabelHeader()
        {
        }
    }

    internal abstract class LabelClient
    {
        public const string CLIENTE_SOLICITANTE = "CLIENTE SOLICITANTE";
        public const string AFILIADO_CONTRATADO = "AFILIADO CONTRATADO";
        public const string CNPJ = "CNPJ";
        private LabelClient()
        {
        }
    }

    internal abstract class LabelDocumentTitle
    {
        public const string ORDEM_SERVICO = "ORDEM DE SERVICO";
        public const string DETALHAMENTO_SERVICO = "DETALHAMENTO DO SERVICO";

       private LabelDocumentTitle()
        {
        }
    }

    internal abstract class LabelBoddyTitle
    {
        public const string RESUMO_GERAL = "RESUMO GERAL DO EVENTO";

        private LabelBoddyTitle()
        {
        }
    }

  
}
