
import { createRouter, createWebHistory } from 'vue-router'

import LoginView from '@/views/LoginView.vue'
import DashboardView from '@/views/DashboardView.vue'
import MainLayout from '@/layouts/MainLayout.vue'
import ModulePlaceholderView from '@/views/ModulePlaceholderView.vue'
import { useAuthStore } from '../stores/auth'
import MedicosView from '@/views/MedicosView.vue'
import PacientesView from '@/views/PacientesView.vue'
import RecepcionistaView from '@/views/RecepcionistaView.vue'
import RemedioView from '@/views/RemedioView.vue'
import TransacoesView from '@/views/TransacoesView.vue'
import ConsultasView from '@/views/ConsultasView.vue'

const routes = [
    {
        path: '/',
        name: 'login',
        component: LoginView
    },
    {
        path: '/dashboard',
        component: MainLayout,
        meta: {
            requiresAuth: true
        },
        children: [
            {
                path: '',
                name: 'dashboard',
                component: DashboardView,
                meta: { title: 'Dashboard' }
            },
            {
                path: 'medicos',
                name: 'medicos',
                component: MedicosView,
                meta: { title: 'Médicos' }
            },
            {
                path: 'pacientes',
                name: 'pacientes',
                component: PacientesView,
                meta: { title: 'Pacientes' }
            },
            {
                path: 'recepcionistas',
                name: 'recepcionistas',
                component: RecepcionistaView,
                meta: { title: 'Recepcionistas' }
            },
            {
                path: 'consultas',
                name: 'consultas',
                component: ConsultasView,
                meta: { title: 'Consultas' }
            },
            {
                path: 'remedios',
                name: 'remedios',
                component: RemedioView,
                meta: { title: 'Remédios' }
            },
            {
                path: 'transacoes',
                name: 'transacoes',
                component: TransacoesView,
                meta: { title: 'Transações' }
            }
        ]
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to) => {
    const authStore = useAuthStore()

    if (to.meta.requiresAuth && !authStore.autenticado) {
        return { name: 'login' }
    }

    return true
})

export default router
