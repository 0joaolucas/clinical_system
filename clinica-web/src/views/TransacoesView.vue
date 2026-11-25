<script setup>
import { ref, onMounted } from 'vue';

import {
    listarTransacoes,
    cadastrarTransacao,
    atualizarTransacao,
    excluirTransacao
} from '@/services/transacaoService';

import { listarPacientes } from '@/services/pacienteService';
import { listarRemedios } from '@/services/remedioService';

import { Pencil, Trash2 } from 'lucide-vue-next';

// Listas
const transacoes = ref([]);
const pacientes = ref([]);
const remedios = ref([]);

// Estados da tela
const modalAberto = ref(false);
const editando = ref(false);
const carregando = ref(false);
const salvando = ref(false);
const erro = ref('');

// Formulário
const transacao = ref({
    id: 0,
    idPaciente: '',
    idRemedio: '',
    quantidade: 1,
    valor: '',
    dataTransacao: new Date().toISOString().split('T')[0]
});

// Identificadores dos pacientes e remédios
function obterIdPaciente(paciente) {
    return paciente.idPaciente ?? paciente.id;
}

function obterIdRemedio(remedio) {
    return remedio.idRemedio ?? remedio.id;
}

// Mensagem de erro
function mensagem(error) {
    const erros = error.response?.data?.errors;

    if (Array.isArray(erros) && erros.length > 0) {
        return erros.join(', ');
    }

    if (erros && typeof erros === 'object') {
        return Object.values(erros).flat().join(', ');
    }

    return error.response?.data?.message
        || error.message
        || 'Não foi possível realizar a operação.';
}

// Carregar transações, pacientes e remédios
async function carregarDados() {
    try {
        carregando.value = true;
        erro.value = '';

        const [
            resTransacoes,
            resPacientes,
            resRemedios
        ] = await Promise.all([
            listarTransacoes(),
            listarPacientes(),
            listarRemedios()
        ]);

        if (!resTransacoes.success) {
            throw new Error(resTransacoes.message);
        }

        if (!resPacientes.success) {
            throw new Error(resPacientes.message);
        }

        if (!resRemedios.success) {
            throw new Error(resRemedios.message);
        }

        transacoes.value = resTransacoes.data || [];
        pacientes.value = resPacientes.data || [];
        remedios.value = resRemedios.data || [];

        // Verificar os dados retornados pela API
        console.log('Pacientes carregados:', pacientes.value);
        console.log('Remédios carregados:', remedios.value);
        console.log('Transações carregadas:', transacoes.value);

    } catch (error) {
        console.error('Erro ao carregar dados:', error);
        erro.value = mensagem(error);

    } finally {
        carregando.value = false;
    }
}

function nomePaciente(id) {
    return pacientes.value.find(
        p => p.idPaciente === id
    )?.nome || `ID ${id ?? ''}`;
}

function nomeRemedio(id) {
    return remedios.value.find(
        r => r.id === id
    )?.nome || `ID ${id ?? ''}`;
}

// Abrir formulário para nova transação
function abrirModal() {
    editando.value = false;

    transacao.value = {
        id: 0,
        idPaciente: '',
        idRemedio: '',
        quantidade: 1,
        valor: '',
        dataTransacao: new Date().toISOString().split('T')[0]
    };

    erro.value = '';
    modalAberto.value = true;
}

// Editar transação
function editarTransacao(item) {
    editando.value = true;

    transacao.value = {
        id: item.id,

        idPaciente:
            item.idPaciente ??
            item.paciente?.idPaciente ??
            item.paciente?.id ??
            '',

        idRemedio:
            item.idRemedio ??
            item.remedio?.idRemedio ??
            item.remedio?.id ??
            '',

        quantidade: item.quantidade,
        valor: item.valor,
        dataTransacao: item.dataTransacao?.split('T')[0] || ''
    };

    erro.value = '';
    modalAberto.value = true;
}

// Fechar formulário
function fecharModal() {
    modalAberto.value = false;
    erro.value = '';
}

// Salvar transação
async function salvarTransacao() {
    try {
        erro.value = '';

        const idPaciente = Number(transacao.value.idPaciente);
        const idRemedio = Number(transacao.value.idRemedio);
        const quantidade = Number(transacao.value.quantidade);
        const valor = Number(transacao.value.valor);

        // Validar paciente
        if (!Number.isInteger(idPaciente) || idPaciente <= 0) {
            erro.value = 'Selecione um paciente válido.';
            return;
        }

        // Validar remédio
        if (!Number.isInteger(idRemedio) || idRemedio <= 0) {
            erro.value = 'Selecione um remédio válido.';
            return;
        }

        // Validar quantidade
        if (!Number.isInteger(quantidade) || quantidade <= 0) {
            erro.value = 'Informe uma quantidade válida.';
            return;
        }

        // Validar valor
        if (!Number.isFinite(valor) || valor < 0) {
            erro.value = 'Informe um valor válido.';
            return;
        }

        // Dados enviados para a API
        const dados = {
            id: Number(transacao.value.id) || 0,
            idPaciente,
            idRemedio,
            quantidade,
            valor,
            dataTransacao: transacao.value.dataTransacao
        };

        console.log('Dados enviados para a API:', dados);

        let response;

        if (editando.value) {
            response = await atualizarTransacao(dados);
        } else {
            response = await cadastrarTransacao(dados);
        }

        if (!response.success) {
            erro.value = response.message
                || 'Não foi possível salvar a transação.';

            return;
        }

        fecharModal();
        await carregarDados();

    } catch (error) {
        console.error('Erro ao salvar transação:', error);
        erro.value = mensagem(error);
    }
}

// Excluir transação
async function removerTransacao(id) {
    const confirmar = confirm(
        'Deseja realmente excluir esta transação?'
    );

    if (!confirmar) {
        return;
    }

    try {
        const response = await excluirTransacao(id);

        if (!response.success) {
            alert(
                response.message ||
                'Não foi possível excluir a transação.'
            );

            return;
        }

        await carregarDados();

    } catch (error) {
        console.error('Erro ao excluir transação:', error);
        alert(mensagem(error));
    }
}

// Formatar data
function formatarData(data) {
    if (!data) {
        return '';
    }

    const [ano, mes, dia] = data.split('T')[0].split('-');

    return `${dia}/${mes}/${ano}`;
}

// Formatar valor monetário
function formatarValor(valor) {
    return Number(valor).toLocaleString('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    });
}

onMounted(carregarDados);
</script>

<template>
    <div class="entity-page">

        <!-- Cabeçalho -->
        <div class="page-header">
            <div>
                <h1>Transações</h1>
                <p>Gerencie as transações de medicamentos.</p>
            </div>

            <button
                class="btn-primary"
                @click="abrirModal"
            >
                Nova transação
            </button>
        </div>

        <!-- Mensagem de erro -->
        <p
            v-if="erro && !modalAberto"
            class="erro"
        >
            {{ erro }}
        </p>

        <!-- Tabela -->
        <div class="table-container">

            <p v-if="carregando">
                Carregando transações...
            </p>

            <table v-else>
                <thead>
                    <tr>
                        <th>Paciente</th>
                        <th>Remédio</th>
                        <th>Quantidade</th>
                        <th>Valor</th>
                        <th>Data</th>
                        <th>Ações</th>
                    </tr>
                </thead>

                <tbody>
                    <tr
                        v-for="item in transacoes"
                        :key="item.id"
                    >
                        <td>
                            {{ item.paciente?.nome || nomePaciente(item.idPaciente) }}
                        </td>

                        <td>
                            {{ item.remedio?.nome || nomeRemedio(item.idRemedio) }}
                        </td>

                        <td>
                            {{ item.quantidade }}
                        </td>

                        <td>
                            {{ formatarValor(item.valor) }}
                        </td>

                        <td>
                            {{ formatarData(item.dataTransacao) }}
                        </td>

                        <td class="acoes">
                            <button
                                class="btn-edit"
                                title="Editar"
                                @click="editarTransacao(item)"
                            >
                                <Pencil :size="18" />
                            </button>

                            <button
                                class="btn-danger"
                                title="Excluir"
                                @click="removerTransacao(item.id)"
                            >
                                <Trash2 :size="18" />
                            </button>
                        </td>
                    </tr>

                    <tr v-if="transacoes.length === 0">
                        <td
                            colspan="6"
                            class="sem-registros"
                        >
                            Nenhuma transação cadastrada.
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Modal -->
        <div
            v-if="modalAberto"
            class="modal-overlay"
        >
            <div class="modal-container">

                <h2>
                    {{ editando ? 'Editar transação' : 'Nova transação' }}
                </h2>

                <form @submit.prevent="salvarTransacao">

                    <div class="form-grid">

                        <!-- Paciente -->
                        <div class="form-group">
                            <label>Paciente</label>

                            <select
                                v-model="transacao.idPaciente"
                                required
                            >
                                <option
                                    value=""
                                    disabled
                                >
                                    Selecione um paciente
                                </option>

                                <option
                                    v-for="paciente in pacientes"
                                    :key="obterIdPaciente(paciente)"
                                    :value="obterIdPaciente(paciente)"
                                >
                                    {{ paciente.nome }}
                                </option>
                            </select>
                        </div>

                        <!-- Remédio -->
                        <div class="form-group">
                            <label>Remédio</label>

                            <select
                                v-model="transacao.idRemedio"
                                required
                            >
                                <option
                                    value=""
                                    disabled
                                >
                                    Selecione um remédio
                                </option>

                                <option
                                    v-for="remedio in remedios"
                                    :key="obterIdRemedio(remedio)"
                                    :value="obterIdRemedio(remedio)"
                                >
                                    {{ remedio.nome }}
                                    (Estoque: {{ remedio.quantidade }})
                                </option>
                            </select>
                        </div>

                        <!-- Quantidade -->
                        <div class="form-group">
                            <label>Quantidade</label>

                            <input
                                type="number"
                                v-model.number="transacao.quantidade"
                                min="1"
                                required
                            />
                        </div>

                        <!-- Valor -->
                        <div class="form-group">
                            <label>Valor</label>

                            <input
                                type="number"
                                v-model.number="transacao.valor"
                                min="0"
                                step="0.01"
                                required
                            />
                        </div>

                        <!-- Data -->
                        <div class="form-group">
                            <label>Data da transação</label>

                            <input
                                type="date"
                                v-model="transacao.dataTransacao"
                                required
                            />
                        </div>

                    </div>

                    <!-- Erro do formulário -->
                    <p
                        v-if="erro"
                        class="erro"
                    >
                        {{ erro }}
                    </p>

                    <!-- Botões -->
                    <div class="form-actions">
                        <button
                            type="button"
                            class="btn-secondary"
                            @click="fecharModal"
                        >
                            Cancelar
                        </button>

                        <button
                            type="submit"
                            class="btn-primary"
                            :disabled="salvando"
                        >
                            {{ salvando ? 'Salvando...' : 'Salvar' }}
                        </button>
                    </div>

                </form>
            </div>
        </div>

    </div>
</template>

<style scoped>
.entity-page {
    padding: 24px;
}

.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
}

.page-header h1 {
    margin: 0 0 6px;
}

.page-header p {
    margin: 0;
    color: #666;
}

.table-container {
    overflow-x: auto;
    background: white;
    border-radius: 8px;
    padding: 16px;
}

table {
    width: 100%;
    border-collapse: collapse;
}

th,
td {
    padding: 14px;
    text-align: left;
    border-bottom: 1px solid #eee;
}

th {
    background: #f8f8f8;
}

.acoes {
    display: flex;
    gap: 8px;
    align-items: center;
}

.btn-primary,
.btn-secondary,
.btn-danger,
.btn-edit {
    border: none;
    border-radius: 5px;
    padding: 9px 14px;
    cursor: pointer;
}

.btn-primary {
    background: #2563eb;
    color: white;
}

.btn-primary:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.btn-secondary {
    background: #e5e7eb;
    color: #222;
}

.btn-danger {
    background: #fee2e2;
    color: #b91c1c;
}

.btn-edit {
    background: #dbeafe;
    color: #2563eb;
    display: flex;
    align-items: center;
    justify-content: center;
}

.modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 16px;
    z-index: 1000;
}

.modal-container {
    background: white;
    width: 100%;
    max-width: 600px;
    padding: 24px;
    border-radius: 8px;
    max-height: 90vh;
    overflow-y: auto;
}

.modal-container h2 {
    margin-top: 0;
    margin-bottom: 20px;
}

.form-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 16px;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 7px;
}

.form-group label {
    font-weight: 600;
}

.form-group input,
.form-group select {
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    width: 100%;
    box-sizing: border-box;
}

.form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
    margin-top: 24px;
}

.erro {
    color: #dc2626;
    margin-top: 12px;
}

.sem-registros {
    text-align: center;
    color: #777;
    padding: 24px;
}

@media (max-width: 600px) {
    .page-header {
        flex-direction: column;
        align-items: flex-start;
        gap: 16px;
    }

    .form-grid {
        grid-template-columns: 1fr;
    }
}
</style>