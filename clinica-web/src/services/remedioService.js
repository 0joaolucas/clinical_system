
import api from './api';

export async function listarRemedios() {
    const response = await api.get('/Remedio');
    return response.data;
}

export async function cadastrarRemedio(remedio) {
    const response = await api.post('/Remedio', remedio);
    return response.data;
}

export async function atualizarRemedio(remedio) {
    const response = await api.put('/Remedio', remedio);
    return response.data;
}

export async function excluirRemedio(id) {
    const response = await api.delete(`/Remedio/${id}`);
    return response.data;
}
