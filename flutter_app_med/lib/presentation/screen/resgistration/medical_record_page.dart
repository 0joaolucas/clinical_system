import 'package:flutter/material.dart';

class MedicalRecordPage extends StatefulWidget {
  final Map<String, String> patient;

  MedicalRecordPage({required this.patient});

  static String routName = "medicalRecordPage";

  @override
  _MedicalRecordPageState createState() => _MedicalRecordPageState();
}

class _MedicalRecordPageState extends State<MedicalRecordPage> {
  final List<Map<String, String>> _consultations = [];
  final List<Map<String, String>> _medications = [];
  final List<Map<String, String>> _exams = [];

  final TextEditingController _consultationController = TextEditingController();
  final TextEditingController _medicationController = TextEditingController();
  final TextEditingController _examController = TextEditingController();

  void _addConsultation() {
    setState(() {
      _consultations.add({'consultation': _consultationController.text});
      _consultationController.clear();
    });
  }

  void _addMedication() {
    setState(() {
      _medications.add({'medication': _medicationController.text});
      _medicationController.clear();
    });
  }

  void _addExam() {
    setState(() {
      _exams.add({'exam': _examController.text});
      _examController.clear();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Prontuário de ${widget.patient['name']}'),
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text('Histórico de Consultas',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
            ListView.builder(
              shrinkWrap: true,
              physics: NeverScrollableScrollPhysics(),
              itemCount: _consultations.length,
              itemBuilder: (context, index) {
                return ListTile(
                  title: Text(_consultations[index]['consultation']!),
                );
              },
            ),
            TextFormField(
              controller: _consultationController,
              decoration: InputDecoration(labelText: 'Adicionar Consulta'),
            ),
            ElevatedButton(
              onPressed: _addConsultation,
              child: Text('Adicionar Consulta'),
            ),
            SizedBox(height: 20),
            Text('Medicamentos',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
            ListView.builder(
              shrinkWrap: true,
              physics: NeverScrollableScrollPhysics(),
              itemCount: _medications.length,
              itemBuilder: (context, index) {
                return ListTile(
                  title: Text(_medications[index]['medication']!),
                );
              },
            ),
            TextFormField(
              controller: _medicationController,
              decoration: InputDecoration(labelText: 'Adicionar Medicamento'),
            ),
            ElevatedButton(
              onPressed: _addMedication,
              child: Text('Adicionar Medicamento'),
            ),
            SizedBox(height: 20),
            Text('Exames',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
            ListView.builder(
              shrinkWrap: true,
              physics: NeverScrollableScrollPhysics(),
              itemCount: _exams.length,
              itemBuilder: (context, index) {
                return ListTile(
                  title: Text(_exams[index]['exam']!),
                );
              },
            ),
            TextFormField(
              controller: _examController,
              decoration: InputDecoration(labelText: 'Adicionar Exame'),
            ),
            ElevatedButton(
              onPressed: _addExam,
              child: Text('Adicionar Exame'),
            ),
          ],
        ),
      ),
    );
  }
}
