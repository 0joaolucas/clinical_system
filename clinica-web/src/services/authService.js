import api from './api'

export async function login(cpf, senha){
    const response = await api.post('/Auth/login', {cpf, senha})

    return response.data
}