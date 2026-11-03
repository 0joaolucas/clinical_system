import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/consulta.dart';
import 'package:flutter_app_med/domain/models/medico.dart';

class MedicoList extends StatelessWidget {
  final List<Medicos> medicos;

  const MedicoList({Key? key, required this.medicos}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.vertical,
      child: SingleChildScrollView(
        scrollDirection: Axis.horizontal,
        child: DataTable(
          columns: const <DataColumn>[
            DataColumn(
              label: Text('Nome'),
            ),
            DataColumn(
              label: Text('CRM'),
            ),
            DataColumn(
              label: Text('Especialidade'),
            ),
            
          ],
          rows: medicos.map((medico) {
            return DataRow(
              cells: <DataCell>[
                DataCell(Text(medico.cpf?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.nome?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.crm?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.especialidade?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.telefone?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.email?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.funcao?.toString() ?? 'Não disponível')),
                DataCell(Text(medico.senhaTemp?.toString() ?? 'Não disponível')),
              ],
            );
          }).toList(),
        ),
      ),
    );
  }
}
