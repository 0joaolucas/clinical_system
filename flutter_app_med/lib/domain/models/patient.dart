import 'dart:convert';

class Patient {
  int? id;
  String? nome;
  DateTime? dataNasc;
  String? sexo;
  String? cpf;
  String? telefone;
  String? email;
  String? cidade;
  String? bairro;
  String? rua;
  String? plano;
  String? historico;

  Patient({
    this.id,
    this.nome,
    this.dataNasc,
    this.sexo,
    this.cpf,
    this.telefone,
    this.email,
    this.cidade,
    this.bairro,
    this.rua,
    this.plano,
    this.historico,
  });

  String? get dataNascApi {
    if (dataNasc == null) {
      return null;
    }

    final ano = dataNasc!.year.toString().padLeft(4, '0');
    final mes = dataNasc!.month.toString().padLeft(2, '0');
    final dia = dataNasc!.day.toString().padLeft(2, '0');

    return '$ano-$mes-$dia';
  }

  Map<String, dynamic> toMap() {
    return {
      if (id != null) 'idPaciente': id,
      'nome': nome ?? '',
      'dataNasc': dataNascApi,
      'sexo': sexo ?? '',
      'cpf': cpf ?? '',
      'telefone': telefone ?? '',
      'email': email ?? '',
      'cidade': cidade ?? '',
      'bairro': bairro ?? '',
      'rua': rua ?? '',
      'plano': plano ?? '',
      'historico': historico ?? '',
    };
  }

  factory Patient.fromMap(Map<String, dynamic> map) {
    return Patient(
      id: map['idPaciente'] ?? map['id'],
      nome: map['nome']?.toString(),
      dataNasc: map['dataNasc'] == null
          ? null
          : DateTime.tryParse(
              map['dataNasc'].toString(),
            ),
      sexo: map['sexo']?.toString(),
      cpf: map['cpf']?.toString(),
      telefone: map['telefone']?.toString(),
      email: map['email']?.toString(),
      cidade: map['cidade']?.toString(),
      bairro: map['bairro']?.toString(),
      rua: map['rua']?.toString(),
      plano: map['plano']?.toString(),
      historico: map['historico']?.toString(),
    );
  }

  String toJson() {
    return jsonEncode(toMap());
  }

  factory Patient.fromJson(String source) {
    return Patient.fromMap(jsonDecode(source));
  }
}