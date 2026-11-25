<template>
    <main class="login-container">

        <!-- Painel de apresentação -->
        <section class="brand-panel">
            <img :src="logo" alt="Logo da clínica" class="logo"/>

            <h1>Clínica Médica</h1>

            <p>
                Cuidando de pessoas, organizando processos e facilitando o atendimento.
            </p>

            <span class="brand-footer">
                Sistema de gerenciamento
            </span>
        </section>

        <!--Painel de login -->
        <section class="form-panel">
            <div class="login-content">

                <h2>Bem-vindo!</h2>
                <p class="subtitle">
                    Entre com suas credenciais para acessar o sistema.
                </p>

                <form @submit.prevent="handleLogin">

                    <div class="input-group">
                        <label for="cpf">CPF</label>
                        <input
                            id="cpf"
                            type="text"
                            placeholder="Digite seu CPF"
                            v-model="cpf"
                            required
                        />
                    </div>

                    <div class="input-group">
                        <label for="senha">Senha</label>
                            <input 
                                id="senha"
                                type="password"
                                placeholder="Digite sua senha"
                                v-model="senha"
                                required
                            />
                    </div>

                    <p v-if="erro" class="error-message">
                        {{ erro }}
                    </p>

                    <button type="submit" :disabled="carregando">
                        {{ carregando ? 'Entrando...' : 'Entrar' }}
                    </button>

                </form>

            </div>

            <footer>Clínica Médica © 2026</footer>
        </section>

    </main>
</template>

<script setup>
import logo from '../assets/logo.png'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const cpf = ref('')
const senha = ref('')
const erro = ref('')
const carregando = ref(false)

const authStore = useAuthStore()
const router = useRouter()

async function handleLogin() {

    erro.value = ''
    carregando.value = true

    try{
        await authStore.realizarLogin(cpf.value, senha.value)

        router.push('/dashboard')
    }catch(error){
        erro.value = 
            error.response?.data?.errors?.[0] ||
            error.response?.data?.message ||
            error.message ||
            'Erro ao realizar login.'
    } finally {
        carregando.value = false
    }

}
</script>

<style scoped>

    * {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
    }

    body{
        margin: 0;
        padding: 0;
        min-width: 320px;
        min-height: 100vh;
    }

    .login-container{
        min-height: 100vh;
        display: flex;
        background-color: #f4f8fa;
    }

    /* Painel esquerdo */

    .brand-panel{
        width: 45%;
        background-color: #3c9bf5;
        color: white;

        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;

        padding: 50px;
        text-align: center;
    }

    .logo{
        width: 300px;
        height: 300px;
        object-fit: contain;
        margin-bottom: 25px;

        animation: heartbeat 1.5s infinite;
    }

    @keyframes heartbeat {
        0% {
            transform: scale(1.08);
        }

        10% {
            transform: scale(1.15);
        }

        20% {
            transform: scale(1.08);
        }

        30% {
            transform: scale(1.15);
        }

        40% {
            transform: scale(1.08);
        }

        100% {
            transform: scale(1);
        }
    }

    .brand-panel h1{
        font-size: 40px;
        margin-bottom: 15px;
    }

    .brand-panel p{
        max-width: 350px;
        line-height: 1.7;
        font-size: 20px;
        color: #edf5f8;
    }

    .brand-footer{
        position: absolute;
        bottom: 30px;
        font-size: 15px;
        opacity: 0.8;
    }

    /* Painel direito */

    .form-panel{
        flex: 1;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;

        padding: 50px;
        position: relative;
    }

    .login-content{
        width: 100%;
        max-width: 400px;
    }

    .login-content h2{
        color: #243b53;
        font-size: 40px;
        margin-bottom: 10px;
    }

    .subtitle{
        color: #718096;
        font-size: 20px;
        margin-bottom: 35px;
        line-height: 1.5;
    }

    /* Campos */

    .input-group{
        display: flex;
        flex-direction: column;
        margin-bottom: 22px;
    }

    .input-group label{
        color: #243b53;
        font-size: 16px;
        font-weight: 600;
        margin-bottom: 5px;
    }

    .input-group input{
        height: 32px;
        padding: 12px;

        border: 1px solid #d5e0e7;
        border-radius: 8px;

        font-size: 14px;
        outline: none;

        transition: border-color 0.2s;
    }

    .input-group input:focus{
        border-color: #6f9fb5;
    }

    /* Botão */

    button{
        width: 100%;
        height: 32px;

        background-color: #49a2f5;
        color: white;

        border: none;
        border-radius: 8px;

        font-size: 18px;
        font-weight: 530;

        cursor: pointer;
        transition: background-color 0.2s;
    }

    button:hover{
        background-color: #243b53;
    }

    .form-panel footer{
        position: absolute;
        bottom: 30px;
        color: #a0aec0;
        font-size: 16px;
    }

    /* Responsividade */

    @media (max-width: 768px){
        .login-container{
            flex-direction: column;
        }

        .brand-panel{
            width: 100%;
            height: 35px 20px
        }

        .logo {
            width: 110px;
            height: 110px;
        }

        .brand-panel h1{
            font-size: 25px;
        }

        .brand-footer{
            display: none;
        }

        .form-panel{
            min-height: 500px;
            padding: 35px 25px 70px;
        }
    }

</style>