import 'package:flutter/material.dart';
import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/medico.dart';
import 'package:flutter_app_med/domain/repositories/medico_repository.dart';

class UserRegistrationPage extends StatefulWidget {
  static const String routName = "/userForm";

  const UserRegistrationPage({super.key});

  @override
  _UserRegistrationPageState createState() =>
      _UserRegistrationPageState();
}

class _UserRegistrationPageState
    extends State<UserRegistrationPage> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _cpfController = TextEditingController();
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _crmController = TextEditingController();
  final TextEditingController _specialtyController =
      TextEditingController();
  final TextEditingController _phoneController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _functionController =
      TextEditingController();
  final TextEditingController _passwordController =
      TextEditingController();

  final MedicoRepository medicoRepository = MedicoRepository();

  bool _isSaving = false;

  Future<void> saveMedico() async {
    if (!_formKey.currentState!.validate()) return;

    final medico = Medicos(
      cpf: _cpfController.text.trim(),
      nome: _nameController.text.trim(),
      crm: _crmController.text.trim(),
      especialidade: _specialtyController.text.trim(),
      telefone: _phoneController.text.trim(),
      email: _emailController.text.trim(),
      funcao: _functionController.text.trim(),
      senhaTemp: _passwordController.text,
    );

    setState(() => _isSaving = true);

    try {
      // O POST /Medico também cria o registro de Login no backend.
      await medicoRepository.insert(medico);

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Médico e login cadastrados com sucesso'),
        ),
      );

      _cpfController.clear();
      _nameController.clear();
      _crmController.clear();
      _specialtyController.clear();
      _phoneController.clear();
      _emailController.clear();
      _functionController.clear();
      _passwordController.clear();
    } on ApiException catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(e.message)),
      );
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao salvar o médico: $e')),
      );
    } finally {
      if (mounted) {
        setState(() => _isSaving = false);
      }
    }
  }

  @override
  void dispose() {
    _cpfController.dispose();
    _nameController.dispose();
    _crmController.dispose();
    _specialtyController.dispose();
    _phoneController.dispose();
    _emailController.dispose();
    _functionController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Registro de Usuário')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: <Widget>[
              TextFormField(
                controller: _cpfController,
                decoration: const InputDecoration(labelText: 'CPF'),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira o CPF'
                    : null,
              ),
              TextFormField(
                controller: _nameController,
                decoration: const InputDecoration(labelText: 'Nome'),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira o nome'
                    : null,
              ),
              TextFormField(
                controller: _crmController,
                decoration: const InputDecoration(labelText: 'CRM'),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira o CRM'
                    : null,
              ),
              TextFormField(
                controller: _specialtyController,
                decoration: const InputDecoration(
                  labelText: 'Especialidade',
                ),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira a área de atuação'
                    : null,
              ),
              TextFormField(
                controller: _phoneController,
                decoration: const InputDecoration(labelText: 'Telefone'),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira o telefone'
                    : null,
              ),
              TextFormField(
                controller: _emailController,
                decoration: const InputDecoration(labelText: 'Email'),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira o email'
                    : null,
              ),
              TextFormField(
                controller: _functionController,
                decoration: const InputDecoration(labelText: 'Função'),
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira a função'
                    : null,
              ),
              TextFormField(
                controller: _passwordController,
                decoration: const InputDecoration(labelText: 'Senha'),
                obscureText: true,
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira a senha'
                    : null,
              ),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: _isSaving ? null : saveMedico,
                child: Text(
                  _isSaving ? 'Salvando...' : 'Registrar',
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}