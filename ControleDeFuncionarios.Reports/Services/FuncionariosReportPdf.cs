using ControleDeFuncionarios.Reports.Interfaces;
using ControleDeFuncionarios.Reports.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeFuncionarios.Reports.Services
{
    public class FuncionariosReportPdf : IFuncionariosReport
    {
        public byte[] Create(FuncionariosReportModels model)
        {
            //criando o arquivo em memória que irá armazenar o relatório
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            //abrindo o documento PDF
            using (var document = new Document(pdf))
            {
                #region Título do relatório
                document.Add(new Paragraph("Relatório de Contatos").AddStyle(new Style().SetFontSize(24)));

                document.Add(new Paragraph($"Gerado em:{model.DataHora.Value.ToString("dd/MM/yyyy HH:mm")}"));

                document.Add(new Paragraph($"Nome do usuário: {model.Usuario.Nome}"));

                document.Add(new Paragraph($"Email do usuário: {model.Usuario.Email}"));

                document.Add(new Paragraph("\n"));
                #endregion

                #region Tabela com os dados
                var table = new Table(8);

                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.AddHeaderCell(new Paragraph("Nome do Contato").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("Email").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("Telefone").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("Data de Nascimento").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("CPF do Funcionário").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("Data de Admissão").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("Cargo do Funcionário").AddStyle(new Style().SetFontSize(8)));
                table.AddHeaderCell(new Paragraph("Salário do Funcionário").AddStyle(new Style().SetFontSize(8)));
                foreach (var item in model.Funcionarios)
                {
                    table.AddCell(new Paragraph(item.Nome).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(item.Email).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(item.Telefone).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(item.DataNascimento.ToString("dd/MM/yyyy")).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(item.Cpf).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(item.DataAdmissao.ToString("dd/MM/yyyy")).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(item.Cargo).AddStyle(new Style().SetFontSize(8)));
                    table.AddCell(new Paragraph(Convert.ToString(item.Salario)).AddStyle(new Style().SetFontSize(8)));
                }
                document.Add(table);
                document.Add(new Paragraph($"Quantidade de Funcionários: {model.Funcionarios.Count}"));
                #endregion
            }
            return memoryStream.ToArray();
        }
    }
}