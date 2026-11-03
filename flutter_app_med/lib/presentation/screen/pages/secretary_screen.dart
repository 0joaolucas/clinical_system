import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/recepcionista.dart';
import 'package:flutter_app_med/domain/repositories/recepcionist_repository.dart';
import 'package:flutter_app_med/presentation/screen/listas/secretary_list.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/secretary_registration.dart';

class SecretaryScreen extends StatefulWidget {
  const SecretaryScreen({Key? key}) : super(key: key);

  static const String routName = "/secretary";

  @override
  _SecretaryScreenState createState() =>
      _SecretaryScreenState();
}

class _SecretaryScreenState extends State<SecretaryScreen> {
  List<Recepcionista> _recepcionistas = [];

  final RecepcionistaRepository recepcionistaRepository =
      RecepcionistaRepository();

  @override
  void initState() {
    super.initState();
    _fetchRecepcionistas();
  }

  Future<void> _fetchRecepcionistas() async {
    try {
      final recepcionistas =
          await recepcionistaRepository.findAll();

      if (!mounted) return;

      setState(() => _recepcionistas = recepcionistas);
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Erro ao carregar recepcionistas: $e'),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Consultas')),
      body: SecretaryList(
        recepcionista: _recepcionistas,
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () async {
          final saved = await showModalBottomSheet<bool>(
            context: context,
            isScrollControlled: true,
            builder: (context) => Padding(
              padding: EdgeInsets.only(
                bottom: MediaQuery.of(context).viewInsets.bottom,
              ),
              child: const SecretaryRegistration(),
            ),
          );

          if (saved == true) {
            _fetchRecepcionistas();
          }
        },
        child: const Icon(Icons.add),
      ),
    );
  }
}