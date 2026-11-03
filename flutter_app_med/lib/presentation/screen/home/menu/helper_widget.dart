import 'package:flutter/material.dart';

class HelperWidget {

  static Widget createItemMenu(BuildContext context, title, icon){
    return ListTile(
      title: Text(title),
      leading: Icon(icon),
      onTap: () {
        Navigator.pop(context);
      },
    );
  }

}