using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClinicaApi.Domain.Entities;

namespace ClinicaApi.Domain.Entities;

public class Medico
{
    [Key]
    public int IdMedico {get; set;}
    public string Cpf {get;set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;
    public string Crm {get; set;} = string.Empty;
    public string Especialidade {get; set;} = string.Empty;
    public string Telefone {get; set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public string Funcao {get; set;} = string.Empty;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

}