import 'dart:convert';

class Medicos {
  int? id;
  String? cpf;
  String? nome;
  String? crm;
  String? especialidade;
  String? telefone;
  String? email;
  String? funcao;

  // Usada apenas durante o cadastro.
  // Não pertence à entidade Medico da API.
  String? senhaTemp;

  Medicos({
    this.id,
    this.cpf,
    this.nome,
    this.crm,
    this.especialidade,
    this.telefone,
    this.email,
    this.funcao,
    this.senhaTemp,
  });

  Map<String, dynamic> toMap() {
    return {
      if (id != null) 'idMedico': id,
      'cpf': cpf ?? '',
      'nome': nome ?? '',
      'crm': crm ?? '',
      'especialidade': especialidade ?? '',
      'telefone': telefone ?? '',
      'email': email ?? '',
      'funcao': funcao ?? '',
    };
  }

  factory Medicos.fromMap(Map<String, dynamic> map) {
    return Medicos(
      id: map['idMedico'] ?? map['id'],
      cpf: map['cpf']?.toString(),
      nome: map['nome']?.toString(),
      crm: map['crm']?.toString(),
      especialidade: map['especialidade']?.toString(),
      telefone: map['telefone']?.toString(),
      email: map['email']?.toString(),
      funcao: map['funcao']?.toString(),
    );
  }

  String toJson() {
    return jsonEncode(toMap());
  }

  factory Medicos.fromJson(String source) {
    return Medicos.fromMap(jsonDecode(source));
  }
}