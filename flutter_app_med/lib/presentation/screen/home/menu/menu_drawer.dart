import 'package:flutter/material.dart';
import 'package:flutter_app_med/presentation/screen/home/menu/helper_widget.dart';

class MenuDrawer extends StatelessWidget {
const MenuDrawer({ Key? key }) : super(key: key);

  @override
  Widget build(BuildContext context){
    return Drawer(child: createMenuDrawer(context),);
  }

  Widget createMenuDrawer(BuildContext context){
    return ListView(
      children: [
        const DrawerHeader(child: Text("Menu")),
        HelperWidget.createItemMenu(context, "Pacientes", Icons.people),
        HelperWidget.createItemMenu(context, "Consultas", Icons.abc),
        HelperWidget.createItemMenu(context, "Financeiro", Icons.monetization_on),
        HelperWidget.createItemMenu(context, "Estoque", Icons.house),
      ],
    );
  }
}