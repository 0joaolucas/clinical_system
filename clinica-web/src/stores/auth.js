import { ref } from 'vue'
import { defineStore } from 'pinia'
import { login } from '../services/authService'

export const useAuthStore = defineStore('auth', () => {
    
    const token = ref(null)
    const usuario = ref(null)
    const autenticado = ref(false)

    async function realizarLogin(cpf, senha){
        const response = await login(cpf, senha)

        if(!response.success){
            throw new Error(response.message)
        }

        token.value = response.data
        autenticado.value = true
    }

    function logout(){
        token.value = null
        usuario.value = null
        autenticado.value = false
    }

    return {
        token,
        usuario,
        autenticado,
        realizarLogin,
        logout
    }

})