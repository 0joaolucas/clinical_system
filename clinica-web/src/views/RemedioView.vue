
<template>
    <section class="entity-page">
        <header class="page-header">
            <div>
                <h1>Remédios</h1>
                <p>Gerenciamento do estoque de remédios</p>
            </div>

            <button class="btn-primary" @click="novo">
                Novo remédio
            </button>
        </header>

        <p v-if="carregando">
            Carregando...
        </p>

        <p v-else-if="erro" class="erro">
            {{ erro }}
        </p>

        <div v-else class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>Quantidade em estoque</th>
                        <th>Ações</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="remedio in lista" :key="remedio.id">
                        <td>{{ remedio.nome }}</td>
                        <td>{{ remedio.quantidade }}</td>

                        <td class="acoes">
                            <button @click="editar(remedio)" class="btn-edit">
                                <Pencil :size="18"/>
                            </button>

                            <button class="btn-danger" @click="excluir(remedio)">
                                <Trash2 :size="18"/>
                            </button>
                        </td>
                    </tr>

                    <tr v-if="!lista.length">
                        <td colspan="3">
                            Nenhum remédio cadastrado.
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div v-if="modal" class="modal-overlay">
            <div class="modal-container">
                <div class="modal-header">
                    <div>
                        <h2>
                            {{ editando ? 'Editar remédio' : 'Cadastrar remédio' }}
                        </h2>

                        <p>Informe o nome e a quantidade.</p>
                    </div>

                    <button
                        class="btn-fechar"
                        @click="fechar"
                    >
                        ×
                    </button>
                </div>

                <form @submit.prevent="salvar">
                    <p v-if="erroForm" class="erro">
                        {{ erroForm }}
                    </p>

                    <div class="form-grid">
                        <label>
                            Nome
                            <input
                                v-model="form.nome"
                                type="text"
                                required
                            >
                        </label>

                        <label>
                            Quantidade
                            <input
                                v-model.number="form.quantidade"
                                type="number"
                                min="0"
                                required
                            >
                        </label>
                    </div>

                    <div class="form-actions">
                        <button
                            type="button"
                            class="btn-secondary"
                            @click="fechar"
                        >
                            Cancelar
                        </button>

                        <button
                            type="submit"
                            class="btn-primary"
                            :disabled="salvando"
                        >
                            {{
                                salvando
                                    ? 'Salvando...'
                                    : editando
                                        ? 'Atualizar'
                                        : 'Cadastrar'
                            }}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </section>
</template>

<script setup>
import { ref, onMounted } from 'vue';

import {
    listarRemedios,
    cadastrarRemedio,
    atualizarRemedio,
    excluirRemedio
} from '@/services/remedioService';

import { AwardIcon, Pencil, Trash2 } from 'lucide-vue-next'

const lista = ref([]);
const carregando = ref(true);
const erro = ref('');

const modal = ref(false);
const editando = ref(false);
const salvando = ref(false);
const erroForm = ref('');

const vazio = () => ({
    nome: '',
    quantidade: 0
});

const form = ref(vazio());

function mensagem(error) {
    const data = error.response?.data;

    if (Array.isArray(data?.errors)) {
        return data.errors.join(' ');
    }

    return data?.message || error.message || 'Ocorreu um erro.';
}

async function carregar() {
    carregando.value = true;
    erro.value = '';

    try {
        const response = await listarRemedios();

        if (!response.success) {
            throw new Error(response.message || 'Não foi possível carregar os remédios.');
        }

        lista.value = response.data || [];
    } catch (error) {
        erro.value = mensagem(error);
    } finally {
        carregando.value = false;
    }
}

function novo() {
    editando.value = false;
    form.value = vazio();
    erroForm.value = '';
    modal.value = true;
}

function editar(remedio) {
    editando.value = true;

    form.value = {
        id: remedio.id,
        nome: remedio.nome,
        quantidade: remedio.quantidade
    };

    erroForm.value = '';
    modal.value = true;
}

function fechar() {
    modal.value = false;
    erroForm.value = '';
}

async function salvar() {
    salvando.value = true;
    erroForm.value = '';

    try {
        const response = editando.value
            ? await atualizarRemedio(form.value)
            : await cadastrarRemedio(form.value);

        if (!response.success) {
            throw new Error(
                response.errors?.join(' ') ||
                response.message ||
                'Não foi possível salvar o remédio.'
            );
        }

        fechar();
        await carregar();
    } catch (error) {
        erroForm.value = mensagem(error);
    } finally {
        salvando.value = false;
    }
}

async function excluir(remedio) {
    const confirmar = confirm(
        `Deseja realmente excluir o remédio ${remedio.nome}?`
    );

    if (!confirmar) {
        return;
    }

    try {
        const response = await excluirRemedio(remedio.id);

        if (!response.success) {
            throw new Error(
                response.message || 'Não foi possível excluir o remédio.'
            );
        }

        await carregar();
    } catch (error) {
        erro.value = mensagem(error);
    }
}

onMounted(carregar);
</script>

<style scoped>
.entity-page {
    padding: 24px;
}

.page-header,
.modal-header,
.form-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.page-header {
    margin-bottom: 24px;
}

.page-header h1,
.modal-header h2 {
    margin: 0;
}

.page-header p,
.modal-header p {
    color: #64748b;
    margin: 6px 0;
}

.table-container {
    overflow-x: auto;
}

table {
    width: 100%;
    border-collapse: collapse;
    background: white;
}

th,
td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid #e5e7eb;
}

th {
    background: #f8fafc;
}

.acoes {
    white-space: nowrap;
}

.acoes button {
    margin-right: 6px;
}

.btn-primary,
.btn-secondary,
.btn-danger,
.acoes button {
    border: 0;
    border-radius: 6px;
    padding: 9px 13px;
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
}

.btn-danger {
    background: #fee2e2;
    color: #b91c1c;
}

.modal-overlay {
    position: fixed;
    inset: 0;
    background: #0008;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 18px;
    z-index: 1000;
}

.modal-container {
    background: white;
    border-radius: 10px;
    padding: 24px;
    width: min(650px, 100%);
}

.btn-fechar {
    font-size: 24px;
    border: 0;
    background: none;
    cursor: pointer;
}

.form-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 14px;
    margin-top: 18px;
}

.form-grid label {
    display: flex;
    flex-direction: column;
    gap: 6px;
}

.form-grid input {
    padding: 10px;
    border: 1px solid #cbd5e1;
    border-radius: 6px;
}

.form-actions {
    justify-content: flex-end;
    margin-top: 20px;
}

.erro {
    color: #b91c1c;
}

@media (max-width: 600px) {
    .form-grid {
        grid-template-columns: 1fr;
    }
}

.btn-edit{
    background: #dbeafe;
    color: #2563eb;
}
</style>
