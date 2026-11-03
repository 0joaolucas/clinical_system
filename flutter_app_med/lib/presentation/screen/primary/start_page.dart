import 'package:flutter/material.dart';
import 'package:flutter_app_med/presentation/screen/primary/login.dart';
import 'package:flutter_app_med/presentation/screen/primary/sign.dart';

class StartPage extends StatelessWidget {
  const StartPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea( //o conteúdo dentro do SafeArea será exibido dentro dos limites visiveis da tela
        child: Container(
          width: double.infinity, //Indica que o container deve ocupar todo espaço disponível na horizontal
          height: MediaQuery.of(context).size.height, //define altura do container como altura total da tela
          padding: const EdgeInsets.symmetric(horizontal: 30, vertical: 50), //cria um espaço simétrico entre o conteúdo do container e as bordas do container
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween, //os filhos dentro desse widget serão distribuídos uniformemente ao longo do eixo principal
            crossAxisAlignment: CrossAxisAlignment.center, //os filhos dentro desse widget serão centralizados verticalmente (para Row) ou horizontalmente (para Column)
            children: <Widget>[
              const Column(
                children: <Widget>[
                  Text('Welcome',
                    style: TextStyle(
                      fontWeight: FontWeight.bold, //negrito
                      fontSize: 30.0,
                    ),
                  ),
                  SizedBox( //espaçamento entre widgets
                    height: 20.0,
                  ),
                  Text(
                    'Bem vindo(a) ao CuidaMed, o sistema feito para sua clínica!',
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      color: Colors.grey,
                      fontSize: 15.0
                    )
                  ),
                ],
              ),
              Container(
                height: MediaQuery.of(context).size.height / 3, //a altura do Container será de 1/3 da altura total do dispositivo
                decoration: const BoxDecoration( //define uma decoração para o Container(nesse caso, a imagem)
                  image: DecorationImage(
                    image: AssetImage("assets/welcome.png")
                  )
                ),
              ),
              Column(
                children: <Widget>[
                  MaterialButton(
                    minWidth: double.infinity, //fará com que o botão ocupe toda a largura disponivel
                    height: 60, //altura do botão
                    onPressed: () { //quando o botão for pressionado, navegará para página de login usando o Navigator
                      Navigator.push(context, MaterialPageRoute(builder: (context) => LoginPage() ));
                    },
                    shape: RoundedRectangleBorder( //definindo a forma do botão (arredondado)
                      side: const BorderSide( //definindo o estilo da borda do botão
                        color: Colors.black
                      ),
                      borderRadius: BorderRadius.circular(50) //raio de curvatura da borda
                    ),
                    child: const Text(
                      "Login",
                      style: TextStyle(
                        fontWeight: FontWeight.w600,
                        fontSize: 18
                      ),
                    ),
                  ),
                  const SizedBox(height: 20,),
                  MaterialButton(
                    minWidth: double.infinity,
                    height: 60,
                    onPressed: () {
                      Navigator.push(context, MaterialPageRoute(builder: (context) => const SignupPage()));
                    },
                    color: const Color(0xff0095FF),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(50)
                    ),
                    child: const Text(
                      "Sign Up",
                      style: TextStyle(
                        color: Colors.white,
                        fontWeight: FontWeight.w600,
                        fontSize: 18.0
                      )
                    )
                  ),
                  const SizedBox(height: 20,),
                ]
              )
            ],
          ),
        ),
      ),
    );
  }
}