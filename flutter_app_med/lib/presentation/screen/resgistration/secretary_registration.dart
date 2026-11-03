import 'package:flutter/material.dart';
import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/recepcionista.dart';
import 'package:flutter_app_med/domain/repositories/recepcionist_repository.dart';

class SecretaryRegistration extends StatefulWidget {
  static const String routName = "/RecepcionistaForm";

  const SecretaryRegistration({super.key});

  @override
  _SecretaryRegistrationState createState() =>
      _SecretaryRegistrationState();
}

class _SecretaryRegistrationState
    extends State<SecretaryRegistration> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _cpfController = TextEditingController();
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _phoneController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();

  final RecepcionistaRepository recepcionistaRepository =
      RecepcionistaRepository();

  Future<void> save() async {
    if (!_formKey.currentState!.validate()) return;

    final recepcionista = Recepcionista(
      cpf: _cpfController.text.trim(),
      nome: _nameController.text.trim(),
      telefone: _phoneController.text.trim(),
      email: _emailController.text.trim(),
      senhaTemp: _passwordController.text,
    );

    try {
      await recepcionistaRepository.insert(recepcionista);

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Recepcionista cadastrado com sucesso'),
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
        SnackBar(content: Text('Erro ao salvar usuário: $e')),
      );
    }
  }

  @override
  void dispose() {
    _cpfController.dispose();
    _nameController.dispose();
    _phoneController.dispose();
    _emailController.dispose();
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
                controller: _passwordController,
                decoration: const InputDecoration(labelText: 'Senha'),
                obscureText: true,
                validator: (value) => value == null || value.isEmpty
                    ? 'Por favor, insira a senha'
                    : null,
              ),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: save,
                child: const Text('Registrar'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}