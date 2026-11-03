import 'package:flutter/material.dart';
import 'package:flutter_app_med/presentation/screen/home/home_page.dart';
import 'package:flutter_app_med/presentation/screen/home/user_identify.dart';
import 'package:flutter_app_med/presentation/screen/pacientes/patient_screen.dart';
import 'package:flutter_app_med/presentation/screen/pages/consultation_screen.dart';
import 'package:flutter_app_med/presentation/screen/pages/secretary_screen.dart';
import 'package:flutter_app_med/presentation/screen/primary/login.dart';

import 'package:flutter_app_med/presentation/screen/resgistration/medical_record_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/patient_registration_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/stock_management_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/transaction_registration_page.dart';
import 'package:flutter_app_med/presentation/screen/resgistration/user_registration_page.dart';
import 'package:path/path.dart';

class RoutesNames{

  static Map<String, Widget Function(BuildContext context)> routesNames = {
    HomePage.routName : (context) => HomePage(),
    
    PatientRegistrationPage.routName : (context) => PatientRegistrationPage(),
    UserRegistrationPage.routName:(context) => const UserRegistrationPage(),
    FinancialTransactionPage.routName:(context) => FinancialTransactionPage(),
    MedicalRecordPage.routName:(context) => MedicalRecordPage(patient: {},),
    StockManagementPage.routName:(context) => StockManagementPage(),
    UserIdentify.routName:(context) => UserIdentify(),
    PatientScreen.routName:(context)=> PatientScreen(),
    LoginPage.routName:(context) => LoginPage(),
    ConsultationScreen.routName:(context) => ConsultationScreen(),
    SecretaryScreen.routName:(context) => SecretaryScreen(),
  };

}