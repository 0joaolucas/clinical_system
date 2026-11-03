import 'package:flutter/foundation.dart';

class ApiConfig {
  static const int port = 5293;

  static String get baseUrl {
    if (kIsWeb) {
      return 'http://localhost:$port/api';
    }

    if (defaultTargetPlatform == TargetPlatform.android) {
      return 'http://10.0.2.2:$port/api';
    }

    return 'http://localhost:$port/api';
  }
}