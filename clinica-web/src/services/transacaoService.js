import api from './api';

export async function listarTransacoes() {
    const response = await api.get('/Transacao');
    return response.data;
}

export async function buscarTransacaoPorId(id) {
    const response = await api.get(`/Transacao/${id}`);
    return response.data;
}

export async function cadastrarTransacao(transacao) {
    const response = await api.post('/Transacao', transacao);
    return response.data;
}

export async function atualizarTransacao(transacao) {
    const response = await api.put('/Transacao', transacao);
    return response.data;
}

export async function excluirTransacao(id) {
    const response = await api.delete(`/Transacao/${id}`);
    return response.data;
}