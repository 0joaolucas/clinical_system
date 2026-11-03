import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/patient.dart';

class PatientForm extends StatefulWidget {
  final Function onSave;

  const PatientForm({
    Key? key,
    required this.onSave,
  }) : super(key: key);

  @override
  _PatientFormState createState() => _PatientFormState();
}

class _PatientFormState extends State<PatientForm> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _dateController = TextEditingController();
  final TextEditingController _genderController = TextEditingController();
  final TextEditingController _cpfController = TextEditingController();
  final TextEditingController _phoneController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _cityController = TextEditingController();
  final TextEditingController _neighborhoodController = TextEditingController();
  final TextEditingController _streetController = TextEditingController();
  final TextEditingController _planIdController = TextEditingController();
  final TextEditingController _historyController = TextEditingController();

  DateTime? _dataNascimento;

  Future<void> _selecionarDataNascimento() async {
    final DateTime? dataSelecionada = await showDatePicker(
      context: context,
      initialDate: DateTime(2000),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
    );

    if (dataSelecionada != null) {
      setState(() {
        _dataNascimento = dataSelecionada;

        _dateController.text =
            '${dataSelecionada.day.toString().padLeft(2, '0')}/'
            '${dataSelecionada.month.toString().padLeft(2, '0')}/'
            '${dataSelecionada.year}';
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: Form(
        key: _formKey,
        child: ListView(
          shrinkWrap: true,
          children: <Widget>[
            TextFormField(
              controller: _nameController,
              decoration: const InputDecoration(
                labelText: 'Nome',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira o nome';
                }
                return null;
              },
            ),

            // DATA DE NASCIMENTO
            TextFormField(
              controller: _dateController,
              readOnly: true,
              decoration: const InputDecoration(
                labelText: 'Data de nascimento',
                hintText: 'Selecione a data',
                suffixIcon: Icon(Icons.calendar_month),
              ),
              onTap: _selecionarDataNascimento,
              validator: (value) {
                if (_dataNascimento == null) {
                  return 'Por favor, selecione a data de nascimento';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _genderController,
              decoration: const InputDecoration(
                labelText: 'Sexo',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira o sexo';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _cpfController,
              decoration: const InputDecoration(
                labelText: 'CPF',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira o CPF';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _phoneController,
              decoration: const InputDecoration(
                labelText: 'Telefone',
              ),
              keyboardType: TextInputType.phone,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira o telefone';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _emailController,
              decoration: const InputDecoration(
                labelText: 'Email',
              ),
              keyboardType: TextInputType.emailAddress,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira o email';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _cityController,
              decoration: const InputDecoration(
                labelText: 'Cidade',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira a cidade';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _neighborhoodController,
              decoration: const InputDecoration(
                labelText: 'Bairro',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira o bairro';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _streetController,
              decoration: const InputDecoration(
                labelText: 'Rua',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Por favor, insira a rua';
                }
                return null;
              },
            ),

            TextFormField(
              controller: _planIdController,
              decoration: const InputDecoration(
                labelText: 'Plano',
              ),
            ),

            TextFormField(
              controller: _historyController,
              decoration: const InputDecoration(
                labelText: 'Histórico',
              ),
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Insira informações médicas do paciente';
                }
                return null;
              },
            ),

            const SizedBox(height: 20),

            ElevatedButton(
              onPressed: () async {
                if (_formKey.currentState!.validate()) {
                  final patient = Patient(
                    nome: _nameController.text,
                    dataNasc: _dataNascimento,
                    sexo: _genderController.text,
                    cpf: _cpfController.text,
                    telefone: _phoneController.text,
                    email: _emailController.text,
                    cidade: _cityController.text,
                    bairro: _neighborhoodController.text,
                    rua: _streetController.text,
                    plano: _planIdController.text,
                    historico: _historyController.text,
                  );

                  await widget.onSave(patient);

                  if (mounted) {
                    Navigator.pop(context);
                  }
                }
              },
              child: const Text('Cadastrar'),
            ),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _nameController.dispose();
    _dateController.dispose();
    _genderController.dispose();
    _cpfController.dispose();
    _phoneController.dispose();
    _emailController.dispose();
    _cityController.dispose();
    _neighborhoodController.dispose();
    _streetController.dispose();
    _planIdController.dispose();
    _historyController.dispose();

    super.dispose();
  }
}
