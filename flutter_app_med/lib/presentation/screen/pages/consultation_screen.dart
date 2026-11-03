import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/consulta.dart';
import 'package:flutter_app_med/domain/repositories/consulta_repository.dart';
import 'package:flutter_app_med/presentation/screen/listas/consultation_list.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/consultation_registration.dart';

class ConsultationScreen extends StatefulWidget {
  const ConsultationScreen({Key? key}) : super(key: key);

  static const String routName = "/consultation";

  @override
  _ConsultationScreenState createState() =>
      _ConsultationScreenState();
}

class _ConsultationScreenState extends State<ConsultationScreen> {
  List<Consulta> _consultas = [];

  final ConsultaRepository consultaRepository =
      ConsultaRepository();

  @override
  void initState() {
    super.initState();
    _fetchConsultas();
  }

  Future<void> _fetchConsultas() async {
    try {
      final consultas = await consultaRepository.findAll();

      if (!mounted) return;

      setState(() => _consultas = consultas);
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao carregar consultas: $e')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Consultas')),
      body: ConsultationList(consultations: _consultas),
      floatingActionButton: FloatingActionButton(
        onPressed: () async {
          final saved = await showModalBottomSheet<bool>(
            context: context,
            isScrollControlled: true,
            builder: (context) => Padding(
              padding: EdgeInsets.only(
                bottom: MediaQuery.of(context).viewInsets.bottom,
              ),
              child: const ConsultationRegistration(),
            ),
          );

          if (saved == true) {
            _fetchConsultas();
          }
        },
        child: const Icon(Icons.add),
      ),
    );
  }
}