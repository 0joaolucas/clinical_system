import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/consulta.dart';
import 'package:flutter_app_med/domain/models/recepcionista.dart';

class SecretaryList extends StatelessWidget {
  final List<Recepcionista> recepcionista;

  const SecretaryList({Key? key, required this.recepcionista}) : super(key: key);

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
              label: Text('Telefone'),
            ),
            DataColumn(
              label: Text('Email'),
            ),
          ],
          rows: recepcionista.map((recepcionista) {
            return DataRow(
              cells: <DataCell>[
                DataCell(Text(recepcionista.nome ?? 'Nome não disponível')),
                DataCell(Text(recepcionista.telefone ?? 'Telefone não disponível')),
                DataCell(Text(recepcionista.email ?? 'Email não disponível')),
              ],
            );
          }).toList(),
        ),
      ),
    );
  }
}
