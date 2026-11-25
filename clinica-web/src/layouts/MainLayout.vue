<template>

    <div class="app-layout">

        <!-- Menu lateral -->
        <aside class="sidebar">
            <div class="brand">
                <h2>Clínica Médica</h2>
                <span>Sistema de gerenciamento</span>
            </div>

            <nav class="menu">
                <p class="menu-title">MENU PRINCIPAL</p>

                <router-link to="/dashboard" class="menu-link">
                    Dashboard
                </router-link>

                <p class="menu-title">GERENCIAMENTO</p>

                <router-link to="/dashboard/medicos" class="menu-link">
                    Médicos
                </router-link>

                <router-link to="/dashboard/pacientes" class="menu-link">
                    Pacientes
                </router-link>

                <router-link to="/dashboard/recepcionistas" class="menu-link">
                    Recepcionistas
                </router-link>

                <router-link to="/dashboard/consultas" class="menu-link">
                    Consultas
                </router-link>

                <router-link to="/dashboard/remedios" class="menu-link">
                    Remédios
                </router-link>

                <router-link to="/dashboard/transacoes" class="menu-link">
                    Transações
                </router-link>
            </nav>

            <button class="logout-button" @click="sair">
                Sair do sistema
            </button>
        </aside>

        <!-- Área principal  -->
         <main class="main-content">
            <header class="topbar">
                <h2>{{ titulo }}</h2>
                <span>Sistema da Clínica</span>
            </header>

            <section class="page-content">
                <router-view/>
            </section>
         </main>

    </div>

</template>

<script setup>

import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const titulo = computed(() => route.meta.title || 'Dashboard')

function sair(){
    authStore.logout()
    router.push('/')
}

</script>

<style scoped>

    .app-layout{
        display: flex;
        min-height: 100vh;
        background-color: #f4f7fb;
    }

    /* Menu lateral */
    .sidebar{
        width: 250px;
        min-height: 100vh;
        background-color: #243b53;
        color: white;
        display: flex;
        flex-direction: column;
        padding: 25px 18px;
    }

    .brand{
        padding: 10px 8px 30px;
        border-bottom: 1px solid rbga(255, 255, 255, 0.15);
    }

    .brand h2{
        font-size: 21px;
        margin-bottom: 6px;
    }

    .brand span{
        font-size: 12px;
        color: #cbd5e0;
    }

    .menu{
        display: flex;
        flex-direction: column;
        gap: 7px;
        margin-top: 25px;
    }

    .menu-title{
        font-size: 11px;
        color: #a0aec0;
        margin: 15px 8px 5px;
        letter-spacing: 1px;
    }

    .menu-link{
        color: #e2e8f0;
        text-decoration: none;
        padding: 12px;
        border-radius: 7px;
        transition: background-color 0.2s;
    }

    .menu-link:hover,
    .menu-link.router-link-exact-active{
        background-color: #3c9bf5;
        color: white;
    }

    .logout-button{
        margin-top: auto;
        padding: 12px;
        border: none;
        border-radius: 9px;
        background-color: #dc4c4c;
        color: white;
        font-size: 14px;
        cursor: pointer;
    }

    .logout-button:hover{
        background-color: #bd3636;
    }

    /* Conteúdo */
    .main-content{
        flex: 1;
        min-width: 0;
    }

    .topbar{
        height: 75px;
        background-color: white;
        border-bottom: 1px solid #e2e8f0;
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 0 35px;
    }

    .topbar h2{
        color: #243b53;
        font-size: 21px;
    }

    .topbar span{
        color: #718096;
        font-size: 14px;
    }

    .page-content{
        padding: 35px;
    }

    /* Responsividade */
    @media (max-width: 768px) {
        .app-layout {
            flex-direction: column;
        }

        .sidebar {
            width: 100%;
            min-height: auto;
        }

        .menu {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }

        .menu-title {
            grid-column: 1 / -1;
        }

        .logout-button {
            margin-top: 20px;
        }

        .topbar {
            padding: 0 20px;
        }

        .page-content {
            padding: 20px;
        }
    }
</style>