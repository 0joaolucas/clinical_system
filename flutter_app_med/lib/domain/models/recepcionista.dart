import 'dart:convert';

class Recepcionista {
  int? id;
  String? cpf;
  String? nome;
  String? telefone;
  String? email;

  String? senhaTemp;

  Recepcionista({
    this.id,
    this.cpf,
    this.nome,
    this.telefone,
    this.email,
    this.senhaTemp,
  });

  Map<String, dynamic> toMap() {
    return {
      if (id != null) 'id': id,
      'cpf': cpf ?? '',
      'nome': nome ?? '',
      'telefone': telefone ?? '',
      'email': email ?? '',
    };
  }

  factory Recepcionista.fromMap(
    Map<String, dynamic> map,
  ) {
    return Recepcionista(
      id: map['id'],
      cpf: map['cpf']?.toString(),
      nome: map['nome']?.toString(),
      telefone: map['telefone']?.toString(),
      email: map['email']?.toString(),
    );
  }

  String toJson() {
    return jsonEncode(toMap());
  }

  factory Recepcionista.fromJson(String source) {
    return Recepcionista.fromMap(
      jsonDecode(source),
    );
  }
}