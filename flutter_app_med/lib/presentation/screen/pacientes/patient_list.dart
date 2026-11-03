import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/patient.dart';

class PatientList extends StatelessWidget {
  final List<Patient> patients;

  const PatientList({Key? key, required this.patients}) : super(key: key);

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
              label: Text('CPF'),
            ),
            DataColumn(
              label: Text('Telefone'),
            ),
            DataColumn(
              label: Text('Email'),
            ),
            DataColumn(
              label: Text('Histórico'),
            ),
          ],
          rows: patients.map((patient) {
            return DataRow(
              cells: <DataCell>[
                DataCell(Text(patient.nome ?? 'Nome não disponível')),
                DataCell(Text(patient.cpf ?? 'CPF não disponível')),
                DataCell(Text(patient.telefone ?? 'Telefone não disponível')),
                DataCell(Text(patient.email ?? 'Email não disponível')),
                DataCell(
                  IconButton(
                    icon: Icon(Icons.book),
                    onPressed: () {
                      showDialog(
                        context: context,
                        builder: (context) => AlertDialog(
                          title: Text('Histórico de ${patient.nome}'),
                          content: SingleChildScrollView(
                            child: Text(patient.historico ?? 'Histórico não cadastrado'),
                          ),
                          actions: <Widget>[
                            TextButton(
                              onPressed: () => Navigator.pop(context),
                              child: Text('Fechar'),
                            ),
                          ],
                        ),
                      );
                    },
                  ),
                ),
              ],
            );
          }).toList(),
        ),
      ),
    );
  }
}
