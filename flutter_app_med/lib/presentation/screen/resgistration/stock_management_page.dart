import 'package:flutter/material.dart';
import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/medicine.dart';
import 'package:flutter_app_med/domain/repositories/medicine_repository.dart';

class StockManagementPage extends StatefulWidget {
  static const String routName = "/stockManagement";

  const StockManagementPage({Key? key}) : super(key: key);

  @override
  _StockManagementPageState createState() =>
      _StockManagementPageState();
}

class _StockManagementPageState extends State<StockManagementPage> {
  final _searchController = TextEditingController();
  final _nameController = TextEditingController();
  final _quantityController = TextEditingController();

  List<Medicine> _medicines = [];
  bool _isSearching = false;

  Medicine _medicine = Medicine(nome: "", quantidade: 0);
  List<Medicine> _results = [];

  final MedicineRepository medicineRepository = MedicineRepository();

  @override
  void initState() {
    super.initState();
    _fetchMedicines();
  }

  Future<void> _fetchMedicines() async {
    try {
      final medicines = await medicineRepository.findAll();

      if (!mounted) return;

      setState(() => _medicines = medicines);
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao carregar estoque: $e')),
      );
    }
  }

  Future<void> save() async {
    final quantidade = int.tryParse(_quantityController.text);

    if (_nameController.text.trim().isEmpty ||
        quantidade == null ||
        quantidade < 0) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Informe um nome e uma quantidade válida.'),
        ),
      );
      return;
    }

    _medicine.nome = _nameController.text.trim();
    _medicine.quantidade = quantidade;

    try {
      if (_medicine.id == null) {
        await medicineRepository.insert(_medicine);
      } else {
        await medicineRepository.update(_medicine);
      }

      await _fetchMedicines();

      if (!mounted) return;

      Navigator.of(context).pop();

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Estoque salvo com sucesso')),
      );
    } on ApiException catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(e.message)),
      );
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao salvar medicamento: $e')),
      );
    }
  }

  Future<void> _searchMedicines(String query) async {
    try {
      final results = await medicineRepository.searchByName(query);

      if (!mounted) return;

      setState(() {
        _results = results;
        _isSearching = query.trim().isNotEmpty;
      });
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao pesquisar medicamentos: $e')),
      );
    }
  }

  @override
  void dispose() {
    _searchController.dispose();
    _nameController.dispose();
    _quantityController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final displayedMedicines =
        _isSearching ? _results : _medicines;

    return Scaffold(
      appBar: AppBar(title: const Text('Controle de Estoque')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            TextField(
              controller: _searchController,
              decoration: InputDecoration(
                labelText: 'Pesquisar Medicamento',
                suffixIcon: IconButton(
                  icon: const Icon(Icons.search),
                  onPressed: () =>
                      _searchMedicines(_searchController.text),
                ),
              ),
              onChanged: (value) {
                if (value.isEmpty) {
                  setState(() => _isSearching = false);
                }
              },
            ),
            const SizedBox(height: 20),
            Expanded(
              child: ListView.builder(
                itemCount: displayedMedicines.length,
                itemBuilder: (context, index) {
                  final medicine = displayedMedicines[index];

                  return ListTile(
                    title: Text(medicine.nome),
                    subtitle: Text(
                      'Quantidade: ${medicine.quantidade}',
                    ),
                    trailing: IconButton(
                      icon: const Icon(Icons.edit),
                      onPressed: () {
                        _nameController.text = medicine.nome;
                        _quantityController.text =
                            medicine.quantidade.toString();

                        _medicine = medicine;

                        showDialog(
                          context: context,
                          builder: (dialogContext) => AlertDialog(
                            title: const Text('Editar Medicamento'),
                            content: Column(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                TextField(
                                  controller: _nameController,
                                  decoration: const InputDecoration(
                                    labelText: 'Nome',
                                  ),
                                ),
                                TextField(
                                  controller: _quantityController,
                                  decoration: const InputDecoration(
                                    labelText: 'Quantidade',
                                  ),
                                  keyboardType: TextInputType.number,
                                ),
                              ],
                            ),
                            actions: [
                              TextButton(
                                onPressed: () =>
                                    Navigator.of(dialogContext).pop(),
                                child: const Text('Cancelar'),
                              ),
                              TextButton(
                                onPressed: save,
                                child: const Text('Salvar'),
                              ),
                            ],
                          ),
                        );
                      },
                    ),
                  );
                },
              ),
            ),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: () {
                _nameController.clear();
                _quantityController.clear();

                _medicine = Medicine(nome: "", quantidade: 0);

                showDialog(
                  context: context,
                  builder: (dialogContext) => AlertDialog(
                    title: const Text('Adicionar Medicamento'),
                    content: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        TextField(
                          controller: _nameController,
                          decoration: const InputDecoration(
                            labelText: 'Nome',
                          ),
                        ),
                        TextField(
                          controller: _quantityController,
                          decoration: const InputDecoration(
                            labelText: 'Quantidade',
                          ),
                          keyboardType: TextInputType.number,
                        ),
                      ],
                    ),
                    actions: [
                      TextButton(
                        onPressed: () =>
                            Navigator.of(dialogContext).pop(),
                        child: const Text('Cancelar'),
                      ),
                      TextButton(
                        onPressed: save,
                        child: const Text('Adicionar'),
                      ),
                    ],
                  ),
                );
              },
              child: const Text('Adicionar Novo Medicamento'),
            ),
          ],
        ),
      ),
    );
  }
}