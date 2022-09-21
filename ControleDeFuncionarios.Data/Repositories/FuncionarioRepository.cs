using ControleDeFuncionarios.Data.Entities;
using ControleDeFuncionarios.Data.Settings;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeFuncionarios.Data.Repositories
{
    public class FuncionarioRepository
    {
        public void Create(Funcionario funcionario)
        {
            var sql = @"INSERT INTO FUNCIONARIO(
                            IDFUNCIONARIO,
                            NOME,
                            EMAIL,
                            TELEFONE,
                            FOTO,
                            DATANASCIMENTO,
                            DATAADMISSAO,
                            SALARIO,     
                            CPF,         
                            CARGO,
                            IDUSUARIO)
                            VALUES(
                            @IdFuncionario,
                            @Nome,
                            @Email,
                            @Telefone,
                            @Foto,
                            @DataNascimento,
                            @DataAdmissao,
                            @Salario,
                            @Cpf,
                            @Cargo,
                            @IdUsuario
                            )";
            using (var connection = new SqlConnection(ConnectionSettings
                                        .GetConnectionString()))
            {
                connection.Execute(sql, funcionario);
            }
        }
        public void Update(Funcionario funcionario)
        {
            var sql = @"UPDATE FUNCIONARIO
                        SET
                        NOME = @Nome,
                        EMAIL = @Email,
                        TELEFONE = @Telefone,
                        FOTO = @Foto,
                        DATANASCIMENTO = @DataNascimento,
                        DATAADMISSAO = @DataAdmissao,
                        SALARIO = @Salario,     
                        CPF = @Cpf,         
                        CARGO = @Cargo 
                        WHERE 
                            IDFUNCIONARIO = @IdFuncionario
                        AND
                             IDUSUARIO = @IdUsuario";
            using (var connection = new SqlConnection(
                            ConnectionSettings.GetConnectionString()))
            {
                connection.Execute(sql,funcionario);
            }

        }
        public void Delete(Funcionario funcionario)
        {
            var sql = @"DELETE FROM FUNCIONARIO
                        WHERE 
                            IDFUNCIONARIO = @IdFuncionario
                        AND
                            IDUSUARIO = @IdUsuario ";
            using (var connection = new SqlConnection(
                             ConnectionSettings.GetConnectionString()))
            {
                connection.Execute(sql,funcionario);
            }
        }
        public List<Funcionario> GetByUsuario(Guid IdUsuario)
        {
            var sql = @"SELECT * FROM FUNCIONARIO f
                        INNER JOIN USUARIO u
                        ON u.IDUSUARIO = f.IDUSUARIO
                        WHERE f.IDUSUARIO= @IdUsuario
                        ORDER BY f.NOME ASC";

            using (var connection = new SqlConnection
                          (ConnectionSettings.GetConnectionString()))
            {
                return connection
                    .Query(sql,(Funcionario f, Usuario u) =>
                {
                    f.Usuario = u;
                    return f;
                },
                new { IdUsuario },
                splitOn: "IdUsuario").ToList();
            }
        }

        public Funcionario GetById(Guid IdFuncionario, Guid IdUsuario)
        {
            var sql = @"SELECT * FROM FUNCIONARIO f
                        INNER JOIN USUARIO u
                        ON u.IDUSUARIO = f.IDUSUARIO
                        WHERE f.IDFUNCIONARIO = @IdFuncionario
                        AND f.IDUSUARIO= @IdUsuario";

            using (var connection= new SqlConnection(
                        ConnectionSettings.GetConnectionString()))
            {
                return connection
                    .Query(sql,(Funcionario f, Usuario u) =>
                    {
                        f.Usuario = u;
                        return f;
                    },
                    new { IdFuncionario, IdUsuario },
                    splitOn: "IdUsuario").FirstOrDefault();
            }
                        
        }
    }

}