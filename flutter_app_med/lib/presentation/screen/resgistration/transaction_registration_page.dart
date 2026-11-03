import 'package:flutter/material.dart';
import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/medicine.dart';
import 'package:flutter_app_med/domain/models/transaction.dart';
import 'package:flutter_app_med/domain/repositories/medicine_repository.dart';
import 'package:flutter_app_med/domain/repositories/transaction_repository.dart';
import 'package:flutter_app_med/domain/models/patient.dart';
import 'package:flutter_app_med/domain/repositories/patient_repository.dart';

class FinancialTransactionPage extends StatefulWidget {
  const FinancialTransactionPage({Key? key}) : super(key: key);

  static const String routName = "/transactionForm";

  @override
  _FinancialTransactionPageState createState() =>
      _FinancialTransactionPageState();
}

class _FinancialTransactionPageState
    extends State<FinancialTransactionPage> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _quantidadeController =
      TextEditingController();
  final TextEditingController _valorController =
      TextEditingController();
  final TextEditingController _dataTransacaoController =
      TextEditingController();

  int? _selectedPacienteId;
  int? _selectedRemedioId;

  List<Patient> _pacientes = [];
  List<Medicine> _remedios = [];

  final TransactionRepository _transactionRepository =
      TransactionRepository();
  final PatientRepository _patientRepository =
      PatientRepository();
  final MedicineRepository _medicamentoRepository =
      MedicineRepository();

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  Future<void> _loadData() async {
    try {
      final pacientes = await _patientRepository.findAll();
      final remedios = await _medicamentoRepository.findAll();

      if (!mounted) return;

      setState(() {
        _pacientes = pacientes;
        _remedios = remedios;
      });
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Erro ao carregar pacientes e remédios: $e'),
        ),
      );
    }
  }

  Future<void> _save() async {
    if (!_formKey.currentState!.validate()) return;

    final transacao = Transacao(
      idPaciente: _selectedPacienteId,
      idRemedio: _selectedRemedioId,
      quantidade: int.tryParse(_quantidadeController.text),
      valor: double.tryParse(
        _valorController.text.replaceAll(',', '.'),
      ),
      dataTransacao: DateTime.tryParse(
        _dataTransacaoController.text,
      ),
    );

    try {
      await _transactionRepository.insert(transacao);

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Transação salva com sucesso'),
        ),
      );

      _formKey.currentState?.reset();

      setState(() {
        _selectedPacienteId = null;
        _selectedRemedioId = null;
      });

      _quantidadeController.clear();
      _valorController.clear();
      _dataTransacaoController.clear();

      await _loadData();
    } on ApiException catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(e.message)),
      );
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao salvar a transação: $e')),
      );
    }
  }

  @override
  void dispose() {
    _quantidadeController.dispose();
    _valorController.dispose();
    _dataTransacaoController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Cadastro de Transação Financeira'),
      ),
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
                value: _selectedRemedioId,
                decoration: const InputDecoration(
                  labelText: 'Remédio',
                ),
                items: _remedios.map((remedio) {
                  return DropdownMenuItem<int>(
                    value: remedio.id,
                    child: Text(remedio.nome),
                  );
                }).toList(),
                onChanged: (value) {
                  setState(() => _selectedRemedioId = value);
                },
                validator: (value) => value == null
                    ? 'Por favor, selecione um remédio'
                    : null,
              ),
              TextFormField(
                controller: _quantidadeController,
                decoration: const InputDecoration(
                  labelText: 'Quantidade',
                ),
                keyboardType: TextInputType.number,
                validator: (value) =>
                    value == null ||
                            int.tryParse(value) == null ||
                            int.parse(value) <= 0
                        ? 'Por favor, insira uma quantidade válida'
                        : null,
              ),
              TextFormField(
                controller: _valorController,
                decoration: const InputDecoration(
                  labelText: 'Valor',
                ),
                keyboardType:
                    const TextInputType.numberWithOptions(
                  decimal: true,
                ),
                validator: (value) =>
                    value == null ||
                            double.tryParse(
                                  value.replaceAll(',', '.'),
                                ) ==
                                null
                        ? 'Por favor, insira um valor válido'
                        : null,
              ),
              TextFormField(
                controller: _dataTransacaoController,
                readOnly: true,
                decoration: const InputDecoration(
                  labelText: 'Data da Transação (YYYY-MM-DD)',
                  suffixIcon: Icon(Icons.calendar_today),
                ),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira a data da transação'
                    : null,
                onTap: () async {
                  final pickedDate = await showDatePicker(
                    context: context,
                    initialDate: DateTime.now(),
                    firstDate: DateTime(1900),
                    lastDate: DateTime(2100),
                  );

                  if (pickedDate != null) {
                    _dataTransacaoController.text =
                        '${pickedDate.year.toString().padLeft(4, '0')}-'
                        '${pickedDate.month.toString().padLeft(2, '0')}-'
                        '${pickedDate.day.toString().padLeft(2, '0')}';
                  }
                },
              ),
              const SizedBox(height: 20),
              Center(
                child: ElevatedButton(
                  onPressed: _save,
                  child: const Text('Cadastrar Transação'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}