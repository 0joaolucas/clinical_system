import 'package:flutter/material.dart';
import 'package:flutter_app_med/presentation/screen/pages/secretary_screen.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/consultation_registration.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/patient_registration_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/patient_search_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/secretary_registration.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/stock_management_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/transaction_registration_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/user_registration_page.dart';

class UserIdentify extends StatefulWidget {
  static const String routName = "/UserIdentify";

  const UserIdentify({Key? key}) : super(key: key);

  @override
  _UserIdentifyState createState() => _UserIdentifyState();
}

class _UserIdentifyState extends State<UserIdentify> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.blue,
        title: Text("IDENTIFICAÇÃO DE USUÁRIO"),
      ),
      body: Container(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                ElevatedButton(
                  onPressed: () {
                    Navigator.pushNamed(context, UserRegistrationPage.routName);
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Color.fromARGB(255, 53, 102, 141),
                    elevation: 10,
                    shadowColor: Colors.black,
                    minimumSize: Size(180, 180),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                    ),
                  ),
                  child: const Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Icon(
                        Icons.people,
                        color: Colors.white,
                        size: 40,
                      ),
                      SizedBox(height: 5),
                      Text(
                        "Médicos",
                        style: TextStyle(color: Colors.white),
                      ),
                    ],
                  ),
                ),
                ElevatedButton(
                  onPressed: () {
                    Navigator.pushNamed(
                        context, SecretaryScreen.routName);
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Color.fromARGB(255, 53, 102, 141),
                    elevation: 10,
                    shadowColor: Colors.black,
                    minimumSize: Size(180, 180),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                    ),
                  ),
                  child: const Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Icon(
                        Icons.person,
                        color: Colors.white,
                        size: 40,
                      ),
                      SizedBox(height: 5),
                      Text(
                        "Recepcionista",
                        style: TextStyle(color: Colors.white),
                      ),
                    ],
                  ),
                ),
              ],
            ),
            // Você pode adicionar mais linhas e botões aqui, se necessário
          ],
        ),
      ),
    );
  }
}
