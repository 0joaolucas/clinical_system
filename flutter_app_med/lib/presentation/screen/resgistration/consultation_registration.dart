import 'package:flutter/material.dart';
import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/consulta.dart';
import 'package:flutter_app_med/domain/models/medico.dart';
import 'package:flutter_app_med/domain/models/patient.dart';
import 'package:flutter_app_med/domain/repositories/consulta_repository.dart';
import 'package:flutter_app_med/domain/repositories/medico_repository.dart';
import 'package:flutter_app_med/domain/repositories/patient_repository.dart';

class ConsultationRegistration extends StatefulWidget {
  const ConsultationRegistration({Key? key}) : super(key: key);

  static const String routName = "/consultaForm";

  @override
  _ConsultationRegistrationState createState() =>
      _ConsultationRegistrationState();
}

class _ConsultationRegistrationState
    extends State<ConsultationRegistration> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _dateController = TextEditingController();
  final TextEditingController _timeController = TextEditingController();
  final TextEditingController _observationsController =
      TextEditingController();
  final TextEditingController _tipoConsultaController =
      TextEditingController();

  int? _selectedPacienteId;
  int? _selectedMedicoId;
  TimeOfDay? _selectedTime;

  List<Patient> _pacientes = [];
  List<Medicos> _medicos = [];

  final ConsultaRepository consultaRepository = ConsultaRepository();
  final PatientRepository patientRepository = PatientRepository();
  final MedicoRepository medicoRepository = MedicoRepository();

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  Future<void> _loadData() async {
    try {
      final pacientes = await patientRepository.findAll();
      final medicos = await medicoRepository.findAll();

      if (!mounted) return;

      setState(() {
        _pacientes = pacientes;
        _medicos = medicos;
      });
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Erro ao carregar pacientes e médicos: $e'),
        ),
      );
    }
  }

  Future<void> save() async {
    if (!_formKey.currentState!.validate()) return;

    final consulta = Consulta(
      idPaciente: _selectedPacienteId,
      idMedico: _selectedMedicoId,
      dataConsulta: DateTime.tryParse(_dateController.text),
      horaConsulta: _selectedTime,
      observacoes: _observationsController.text,
      tipoConsulta: _tipoConsultaController.text,
    );

    try {
      await consultaRepository.insert(consulta);

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Consulta cadastrada com sucesso'),
        ),
      );

      Navigator.of(context).pop(true);
    } on ApiException catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(e.message)),
      );
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao cadastrar consulta: $e')),
      );
    }
  }

  @override
  void dispose() {
    _dateController.dispose();
    _timeController.dispose();
    _observationsController.dispose();
    _tipoConsultaController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Cadastro de Consultas')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: <Widget>[
              DropdownButtonFormField<int>(
                value: _selectedPacienteId,
                decoration: const InputDecoration(
                  labelText: 'Paciente',
                ),
                items: _pacientes.map((paciente) {
                  return DropdownMenuItem<int>(
                    value: paciente.id,
                    child: Text(paciente.nome ?? 'Sem nome'),
                  );
                }).toList(),
                onChanged: (value) {
                  setState(() => _selectedPacienteId = value);
                },
                validator: (value) => value == null
                    ? 'Por favor, selecione um paciente'
                    : null,
              ),
              DropdownButtonFormField<int>(
                value: _selectedMedicoId,
                decoration: const InputDecoration(
                  labelText: 'Médico',
                ),
                items: _medicos.map((medico) {
                  return DropdownMenuItem<int>(
                    value: medico.id,
                    child: Text(medico.nome ?? 'Sem nome'),
                  );
                }).toList(),
                onChanged: (value) {
                  setState(() => _selectedMedicoId = value);
                },
                validator: (value) => value == null
                    ? 'Por favor, selecione um médico'
                    : null,
              ),
              TextFormField(
                controller: _dateController,
                readOnly: true,
                decoration: const InputDecoration(
                  labelText: 'Data da Consulta',
                  suffixIcon: Icon(Icons.calendar_today),
                ),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira a data da consulta'
                    : null,
                onTap: () async {
                  final pickedDate = await showDatePicker(
                    context: context,
                    initialDate: DateTime.now(),
                    firstDate: DateTime(1900),
                    lastDate: DateTime(2100),
                  );

                  if (pickedDate != null) {
                    _dateController.text =
                        '${pickedDate.year.toString().padLeft(4, '0')}-'
                        '${pickedDate.month.toString().padLeft(2, '0')}-'
                        '${pickedDate.day.toString().padLeft(2, '0')}';
                  }
                },
              ),
              TextFormField(
                controller: _timeController,
                readOnly: true,
                decoration: const InputDecoration(
                  labelText: 'Hora da Consulta',
                  suffixIcon: Icon(Icons.access_time),
                ),
                validator: (value) => _selectedTime == null
                    ? 'Por favor, insira a hora da consulta'
                    : null,
                onTap: () async {
                  final pickedTime = await showTimePicker(
                    context: context,
                    initialTime: TimeOfDay.now(),
                  );

                  if (pickedTime != null) {
                    setState(() {
                      _selectedTime = pickedTime;
                      _timeController.text = pickedTime.format(context);
                    });
                  }
                },
              ),
              TextFormField(
                controller: _observationsController,
                decoration: const InputDecoration(
                  labelText: 'Observações',
                ),
              ),
              TextFormField(
                controller: _tipoConsultaController,
                decoration: const InputDecoration(
                  labelText: 'Tipo da consulta',
                ),
              ),
              const SizedBox(height: 20),
              Center(
                child: ElevatedButton(
                  onPressed: save,
                  child: const Text('Cadastrar Consulta'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}