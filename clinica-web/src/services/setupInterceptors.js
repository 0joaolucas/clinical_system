import api from './api'
import { useAuthStore } from '@/stores/auth'

export function setupInterceptors(pinia){

    api.interceptors.request.use(
        (config) => {
            const authStore = useAuthStore(pinia)

            if(authStore.token){
                config.headers.Authorization = `Bearer ${authStore.token}`
            }
            return config
        },
        (error) => {
            return Promise.reject(error)
        }
    )

}