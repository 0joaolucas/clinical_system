import api from './api';

export async function listarConsultas() {
    const response = await api.get('/Consulta');
    return response.data;
}

export async function buscarConsultaPorId(id) {
    const response = await api.get(`/Consulta/${id}`);
    return response.data;
}

export async function cadastrarConsulta(consulta) {
    const response = await api.post('/Consulta', consulta);
    return response.data;
}

export async function atualizarConsulta(consulta) {
    const response = await api.put('/Consulta', consulta);
    return response.data;
}

export async function excluirConsulta(id) {
    const response = await api.delete(`/Consulta/${id}`);
    return response.data;
}
