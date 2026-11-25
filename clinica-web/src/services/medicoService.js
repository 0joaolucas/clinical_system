import api from './api'

export async function listarMedicos(){
    const response = await api.get('/medico')
    return response.data
}

export async function salvarMedico(medico){
    const response = await api.post('medico', medico)
    return response.data
}

export async function atualizarMedico(medico) {
    const response = await api.put('/medico', medico)
    return response.data
}

export async function excluirMedico(id){
    const response = await api.delete(`medico/${id}`)
    return response.data
}