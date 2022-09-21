using ControleDeFuncionarios.Reports.Interfaces;
using ControleDeFuncionarios.Reports.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeFuncionarios.Reports.Services
{
    public class FuncionariosReportExcel : IFuncionariosReport
    {
        public byte[] Create(FuncionariosReportModels model)
        {
            //definindo o tipo de uso da biblioteca (free)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //Criando um arquivo Excel
            using (var excelPackage= new ExcelPackage())
            {
                //criando uma planilha
                var planilha = excelPackage.Workbook.Worksheets.Add("Funcionarios");

                #region Título da Planilha

                planilha.Cells["A1"].Value = "Relatório de Funcionários";
                var titulo = planilha.Cells["A1:D1"];
                titulo.Merge = true;
                titulo.Style.Font.Size = 18;
                titulo.Style.Font.Bold = true;

                planilha.Cells["A3"].Value = "Gerado em:";
                planilha.Cells["B3"].Value = model.DataHora.Value.ToString("dd/MM/yyyy");

                planilha.Cells["A4"].Value = "Nome do usuário:";
                planilha.Cells["B4"].Value = model.Usuario.Nome;

                planilha.Cells["A5"].Value = "Email do usuário:";
                planilha.Cells["B5"].Value = model.Usuario.Email;

                #endregion

                #region Dados da Planilha
                
                planilha.Cells["A7"].Value = "Nome do Contato";
                planilha.Cells["B7"].Value = "Email";
                planilha.Cells["C7"].Value = "Telefone";
                planilha.Cells["D7"].Value = "Data de Nascimento";
                planilha.Cells["E7"].Value = "CPF do Funcionario";
                planilha.Cells["F7"].Value = "Data de Admissão";
                planilha.Cells["G7"].Value = "Cargo do Funcionário";
                planilha.Cells["H7"].Value = "Sálario do Funcionário R$";

                var cabecalho = planilha.Cells["A7:H7"];
                cabecalho.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
                cabecalho.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cabecalho.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#000000"));

                var linha = 8;

                foreach (var item in model.Funcionarios)
                {
                    planilha.Cells[$"A{linha}"].Value = item.Nome;
                    planilha.Cells[$"B{linha}"].Value = item.Email;
                    planilha.Cells[$"C{linha}"].Value = item.Telefone;
                    planilha.Cells[$"D{linha}"].Value = item.DataNascimento.ToString("dd/MM/yyyy");
                    planilha.Cells[$"E{linha}"].Value = item.Cpf;
                    planilha.Cells[$"F{linha}"].Value = item.DataAdmissao.ToString("dd/MM/yyyy");
                    planilha.Cells[$"G{linha}"].Value = item.Cargo;
                    planilha.Cells[$"H{linha}"].Value = item.Salario;

                    if (linha % 2 == 0)
                    {
                        var conteudo = planilha.Cells[$"A{linha}:H{linha}"];
                        conteudo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        conteudo.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#eeeeeee"));
                    }


                    linha++;
                }
                #endregion

                #region Finalização da planilha
                planilha.Cells["A:H"].AutoFitColumns();

                var borda = planilha.Cells[$"A7:H{linha - 1}"];
                borda.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                //retornando o conteúdo do arquivo..
                return excelPackage.GetAsByteArray();
                #endregion
            }
        }
    }
}
