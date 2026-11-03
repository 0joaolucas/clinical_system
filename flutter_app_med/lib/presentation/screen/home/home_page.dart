import 'package:flutter/material.dart';
import 'package:flutter_app_med/domain/models/patient.dart';

import 'package:flutter_app_med/presentation/screen/home/user_identify.dart';
import 'package:flutter_app_med/presentation/screen/pacientes/patient_screen.dart';
import 'package:flutter_app_med/presentation/screen/pages/consultation_screen.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/consultation_registration.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/patient_registration_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/patient_search_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/stock_management_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/transaction_registration_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/user_registration_page.dart';

class HomePage extends StatefulWidget {
  static const String routName = "/home";

  const HomePage({Key? key}) : super(key: key);

  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.blue,
        title: Text("Home"),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceAround,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(
                      context, PatientScreen.routName);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor:
                      Color.fromARGB(255, 53, 102, 141), //cor de fundo
                  elevation: 10,
                  shadowColor: Colors.black,
                  minimumSize: Size(180, 180), //tamanho mínimo
                  shape: RoundedRectangleBorder(
                    borderRadius:
                        BorderRadius.circular(10), //bordas arredondadas
                  ),
                ),
                child: const Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(
                      Icons.people,
                      color: Colors.white,
                      size: 40,
                    ), //ícone do botão
                    SizedBox(height: 5), //espaçamento entre ícone e texto
                    Text(
                      "Pacientes",
                      style: TextStyle(color: Colors.white),
                    ),
                  ],
                ),
              ),
              ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(
                      context, ConsultationScreen.routName);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor:
                      Color.fromARGB(255, 53, 102, 141), //cor de fundo
                  elevation: 10,
                  shadowColor: Colors.black,
                  minimumSize: Size(180, 180), //tamanho mínimo
                  shape: RoundedRectangleBorder(
                    borderRadius:
                        BorderRadius.circular(10), //bordas arredondadas
                  ),
                ),
                child: const Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(
                      Icons.book,
                      color: Colors.white,
                      size: 40,
                    ), //ícone do botão
                    SizedBox(height: 5), //espaçamento entre ícone e texto
                    Text(
                      "Consultas",
                      style: TextStyle(color: Colors.white),
                    ),
                  ],
                ),
              ),
            ],
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceAround,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(context, StockManagementPage.routName);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor:
                      Color.fromARGB(255, 53, 102, 141), //cor de fundo
                  elevation: 10,
                  shadowColor: Colors.black,
                  minimumSize: Size(180, 180), //tamanho mínimo
                  shape: RoundedRectangleBorder(
                    borderRadius:
                        BorderRadius.circular(10), //bordas arredondadas
                  ),
                ),
                child: const Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(
                      Icons.house,
                      color: Colors.white,
                      size: 40,
                    ), //ícone do botão
                    SizedBox(height: 5), //espaçamento entre ícone e texto
                    Text(
                      "Estoque",
                      style: TextStyle(color: Colors.white),
                    ),
                  ],
                ),
              ),
              ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(
                      context, FinancialTransactionPage.routName);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor:
                      Color.fromARGB(255, 53, 102, 141), //cor de fundo
                  elevation: 10,
                  shadowColor: Colors.black,
                  minimumSize: Size(180, 180), //tamanho mínimo
                  shape: RoundedRectangleBorder(
                    borderRadius:
                        BorderRadius.circular(10), //bordas arredondadas
                  ),
                ),
                child: const Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(
                      Icons.monetization_on,
                      color: Colors.white,
                      size: 40,
                    ), //ícone do botão
                    SizedBox(height: 5), //espaçamento entre ícone e texto
                    Text(
                      "Financeiro",
                      style: TextStyle(color: Colors.white),
                    ),
                  ],
                ),
              ),
            ],
          )
        ],
      ),
      bottomNavigationBar: BottomNavigationBar(
        items: [
          BottomNavigationBarItem(
              icon: Icon(Icons.people),
              label: "Pacientes",
              backgroundColor: Colors.blue),
          BottomNavigationBarItem(
            icon: Icon(Icons.book),
            label: "Consultas",
          ),
          BottomNavigationBarItem(icon: Icon(Icons.house), label: "Estoque"),
          BottomNavigationBarItem(
              icon: Icon(Icons.monetization_on), label: "Financeiro"),
        ],
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.pushNamed(context, UserIdentify.routName);
        },
        tooltip: 'Adicionar',
        child: Icon(Icons.add),
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerDocked,
    );
  }
}
