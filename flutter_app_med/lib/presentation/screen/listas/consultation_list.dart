import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/consulta.dart';

class ConsultationList extends StatelessWidget {
  final List<Consulta> consultations;

  const ConsultationList({Key? key, required this.consultations}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.vertical,
      child: SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        child: DataTable(
          columns: const <DataColumn>[
            DataColumn(
              label: Text('Paciente ID'),
            ),
            DataColumn(
              label: Text('Médico ID'),
            ),
            DataColumn(
              label: Text('Data'),
            ),
            DataColumn(
              label: Text('Hora'),
            ),
            DataColumn(
              label: Text('Observações'),
            ),
            DataColumn(
              label: Text('Tipo da consulta'),
            ),
          ],
          rows: consultations.map((consulta) {
            return DataRow(
              cells: <DataCell>[
                DataCell(Text(consulta.idPaciente?.toString() ?? 'Não disponível')),
                DataCell(Text(consulta.idMedico?.toString() ?? 'Não disponível')),
                DataCell(Text(consulta.dataConsulta?.toLocal().toString().split(' ')[0] ?? 'Não disponível')),
                DataCell(Text(consulta.horaConsulta?.format(context) ?? 'Não disponível')),
                DataCell(Text(consulta.observacoes ?? 'Sem observações')),
                DataCell(Text(consulta.tipoConsulta ?? 'Não disponível')),
              ],
            );
          }).toList(),
        ),
      ),
    );
  }
}
