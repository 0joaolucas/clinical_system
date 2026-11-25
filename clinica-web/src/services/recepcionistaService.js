
import api from './api'

export async function listarRecepcionistas() {
    const response = await api.get('/recepcionista')
    return response.data
}

export async function salvarRecepcionista(recepcionista) {
    const response = await api.post('/recepcionista', recepcionista)
    return response.data
}

export async function atualizarRecepcionista(recepcionista) {
    const response = await api.put('/recepcionista', recepcionista)
    return response.data
}

export async function excluirRecepcionista(id) {
    const response = await api.delete(`/recepcionista/${id}`)
    return response.data
}
