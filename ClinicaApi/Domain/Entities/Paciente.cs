using System.ComponentModel.DataAnnotations;

namespace ClinicaApi.Domain.Entities;

public class Paciente
{
    [Key]
    public int IdPaciente {get; set;}
    public string Nome {get; set;} = string.Empty;
    public DateOnly DataNasc {get; set;} 
    public string Sexo {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
    public string Telefone {get; set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public string Cidade {get; set;} = string.Empty;
    public string Bairro {get; set;} = string.Empty;
    public string Rua {get; set;} = string.Empty;
    public string Plano {get; set;} = string.Empty;
    public string Historico {get; set;} = string.Empty;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    public ICollection<Transacao> Transacoes {get; set;} = new List<Transacao>();
}