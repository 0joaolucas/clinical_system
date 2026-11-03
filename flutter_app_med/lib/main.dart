import 'package:flutter/material.dart';

import 'package:flutter_app_med/presentation/screen/home/home_page.dart';
import 'package:flutter_app_med/presentation/screen/primary/load_page.dart';
import 'package:flutter_app_med/presentation/screen/primary/login.dart';
import 'package:flutter_app_med/routes/routes_names.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,

      initialRoute: LoadPage.routName,

      routes: {
        LoadPage.routName: (context) => const LoadPage(),
        LoginPage.routName: (context) => const LoginPage(),
        HomePage.routName: (context) => const HomePage(),
        ...RoutesNames.routesNames,
      },
    );
  }
}