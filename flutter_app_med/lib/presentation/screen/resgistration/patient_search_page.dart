/**import 'package:flutter/material.dart';
import 'package:flutter_app_med/data/database_provider.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/medical_record_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/patient_registration_page.dart';


class PatientSearchPage extends StatefulWidget {
  static const String routName = "/patientSearch";

  @override
  _PatientSearchPageState createState() => _PatientSearchPageState();
}

class _PatientSearchPageState extends State<PatientSearchPage> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _cpfController = TextEditingController();
  Map<String, dynamic>? _patient;

  

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Pesquisar Paciente'),
      ),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: <Widget>[
              TextFormField(
                controller: _cpfController,
                decoration: InputDecoration(labelText: 'CPF'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Por favor, insira o CPF';
                  }
                  return null;
                },
              ),
              SizedBox(height: 20),
              ElevatedButton(
                onPressed:,
                child: Text('Pesquisar'),
              ),
              SizedBox(height: 20),
              if (_patient != null)
                Column(
                  children: [
                    Text(
                      'Paciente encontrado: ${_patient!['nome']}',
                      style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                    ),
                    SizedBox(height: 20),
                    ElevatedButton(
                      onPressed: () {
                        Navigator.pushNamed(context, MedicalRecordPage.routName);
                      },
                      child: Text('Consultar Prontuário'),
                    ),
                  ],
                )
              else if (_patient == null && _cpfController.text.isNotEmpty)
                Column(
                  children: [
                    Text(
                      'Paciente não encontrado',
                      style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                    ),
                    SizedBox(height: 20),
                    ElevatedButton(
                      onPressed: () {
                        Navigator.pushNamed(context, PatientRegistrationPage.routName);
                      },
                      child: Text('Cadastrar Paciente'),
                    ),
                  ],
                ),
            ],
          ),
        ),
      ),
    );
  }
}
*/
