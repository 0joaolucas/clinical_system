import 'dart:convert';

import 'package:flutter/material.dart';

class Consulta {
  int? id;
  int? idPaciente;
  int? idMedico;
  DateTime? dataConsulta;
  TimeOfDay? horaConsulta;
  String? observacoes;
  String? tipoConsulta;

  Consulta({
    this.id,
    this.idPaciente,
    this.idMedico,
    this.dataConsulta,
    this.horaConsulta,
    this.observacoes,
    this.tipoConsulta,
  });

  Map<String, dynamic> toMap() {
    return {
      if (id != null) 'idConsulta': id,
      'idPaciente': idPaciente,
      'idMedico': idMedico,
      'dataConsulta':
          dataConsulta == null ? null : _formatDate(dataConsulta!),
      'horaConsulta':
          horaConsulta == null ? null : _formatTime(horaConsulta!),

      // No Flutter antigo era "observacoes".
      // Na API é "Observacao".
      'observacao': observacoes ?? '',

      'tipoConsulta': tipoConsulta ?? '',
    };
  }

  factory Consulta.fromMap(Map<String, dynamic> map) {
    TimeOfDay? hora;

    if (map['horaConsulta'] != null) {
      final partes =
          map['horaConsulta'].toString().split(':');

      if (partes.length >= 2) {
        hora = TimeOfDay(
          hour: int.parse(partes[0]),
          minute: int.parse(partes[1]),
        );
      }
    }

    return Consulta(
      id: map['idConsulta'] ?? map['id'],
      idPaciente: map['idPaciente'],
      idMedico: map['idMedico'],
      dataConsulta: map['dataConsulta'] == null
          ? null
          : DateTime.tryParse(
              map['dataConsulta'].toString(),
            ),
      horaConsulta: hora,
      observacoes:
          (map['observacao'] ?? map['observacoes'])
              ?.toString(),
      tipoConsulta: map['tipoConsulta']?.toString(),
    );
  }

  static String _formatDate(DateTime data) {
    final ano = data.year.toString().padLeft(4, '0');
    final mes = data.month.toString().padLeft(2, '0');
    final dia = data.day.toString().padLeft(2, '0');

    return '$ano-$mes-$dia';
  }

  static String _formatTime(TimeOfDay hora) {
    final horas = hora.hour.toString().padLeft(2, '0');
    final minutos =
        hora.minute.toString().padLeft(2, '0');

    return '$horas:$minutos:00';
  }

  String toJson() {
    return jsonEncode(toMap());
  }

  factory Consulta.fromJson(String source) {
    return Consulta.fromMap(jsonDecode(source));
  }
}