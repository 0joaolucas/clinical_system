import api from './api'

export async function listarPacientes(){
    const response = await api.get('/paciente')
    return response.data
}

export async function salvarPacientes(paciente){
    const response = await api.post('paciente', paciente)
    return response.data
}

export async function atualizarPaciente(paciente) {
    const response = await api.put('/paciente', paciente)
    return response.data
}

export async function excluirPaciente(id){
    const response = await api.delete(`paciente/${id}`)
    return response.data
}