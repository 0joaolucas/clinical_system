import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/patient.dart';
import 'package:flutter_app_med/domain/repositories/patient_repository.dart';
import 'package:flutter_app_med/presentation/screen/pacientes/patient_form.dart';
import 'package:flutter_app_med/presentation/screen/pacientes/patient_list.dart';

class PatientScreen extends StatefulWidget {
  static const String routName = "/patients";

  const PatientScreen({Key? key}) : super(key: key);

  @override
  _PatientScreenState createState() => _PatientScreenState();
}

class _PatientScreenState extends State<PatientScreen> {
  List<Patient> _patients = [];
  final PatientRepository patientRepository = PatientRepository();

  @override
  void initState() {
    super.initState();
    _fetchPatients();
  }

  Future<void> _fetchPatients() async {
    try {
      final patients = await patientRepository.findAll();

      if (!mounted) return;

      setState(() => _patients = patients);
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao carregar pacientes: $e')),
      );
    }
  }

  Future<void> _savePatient(Patient patient) async {
    if (patient.id == null) {
      await patientRepository.insert(patient);
    } else {
      await patientRepository.update(patient);
    }

    await _fetchPatients();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Pacientes')),
      body: PatientList(patients: _patients),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          showModalBottomSheet(
            context: context,
            isScrollControlled: true,
            builder: (context) => Padding(
              padding: EdgeInsets.only(
                bottom: MediaQuery.of(context).viewInsets.bottom,
              ),
              child: PatientForm(
                onSave: (patient) => _savePatient(patient),
              ),
            ),
          );
        },
        child: const Icon(Icons.add),
      ),
    );
  }
}