import 'dart:convert';

import 'package:flutter_app_med/data/api/api_config.dart';
import 'package:http/http.dart' as http;

class ApiException implements Exception {
  final String message;
  final int? statusCode;

  ApiException(this.message, [this.statusCode]);

  @override
  String toString() => message;
}

class ApiService {
  ApiService._();

  static final ApiService instance = ApiService._();

  String? token;

  Map<String, String> get headers => {
        'Content-Type': 'application/json; charset=UTF-8',
        'Accept': 'application/json',
        if (token != null && token!.isNotEmpty)
          'Authorization': 'Bearer $token',
      };

  Uri createUri(
    String endpoint, [
    Map<String, String>? queryParameters,
  ]) {
    final base = ApiConfig.baseUrl.replaceFirst(RegExp(r'/+$'), '');
    final path = endpoint.replaceFirst(RegExp(r'^/+'), '');

    return Uri.parse('$base/$path').replace(
      queryParameters: queryParameters,
    );
  }

  Future<dynamic> get(String endpoint) async {
    final response = await http.get(
      createUri(endpoint),
      headers: headers,
    );
    return _handleResponse(response);
  }

  Future<dynamic> post(
    String endpoint,
    Map<String, dynamic> body,
  ) async {
    final response = await http.post(
      createUri(endpoint),
      headers: headers,
      body: jsonEncode(body),
    );
    return _handleResponse(response);
  }

  Future<dynamic> postWithQuery(
    String endpoint,
    Map<String, String> query,
  ) async {
    final response = await http.post(
      createUri(endpoint, query),
      headers: headers,
    );
    return _handleResponse(response);
  }

  Future<dynamic> put(
    String endpoint,
    Map<String, dynamic> body,
  ) async {
    final response = await http.put(
      createUri(endpoint),
      headers: headers,
      body: jsonEncode(body),
    );
    return _handleResponse(response);
  }

  Future<dynamic> delete(String endpoint) async {
    final response = await http.delete(
      createUri(endpoint),
      headers: headers,
    );
    return _handleResponse(response);
  }

  dynamic _handleResponse(http.Response response) {
    dynamic decoded;

    if (response.bodyBytes.isNotEmpty) {
      final body = utf8.decode(response.bodyBytes);

      try {
        decoded = jsonDecode(body);
      } on FormatException {
        decoded = body;
      }
    }

    if (response.statusCode < 200 || response.statusCode >= 300) {
      if (decoded is Map) {
        final errors = decoded['errors'];

        if (errors is List && errors.isNotEmpty) {
          throw ApiException(
            errors.map((e) => e.toString()).join('\n'),
            response.statusCode,
          );
        }

        final message = decoded['message'];

        if (message != null) {
          throw ApiException(
            message.toString(),
            response.statusCode,
          );
        }
      }

      throw ApiException(
        decoded is String && decoded.isNotEmpty
            ? decoded
            : 'Erro HTTP ${response.statusCode}.',
        response.statusCode,
      );
    }

    if (decoded is Map && decoded.containsKey('success')) {
      if (decoded['success'] != true) {
        final errors = decoded['errors'];

        throw ApiException(
          errors is List && errors.isNotEmpty
              ? errors.map((e) => e.toString()).join('\n')
              : decoded['message']?.toString() ??
                  'A operação não foi realizada.',
          response.statusCode,
        );
      }

      return decoded['data'];
    }

    return decoded;
  }
}