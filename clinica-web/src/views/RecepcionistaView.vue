
<template>
    <section class="entity-page">
        <header class="page-header">
            <div>
                <h1>Recepcionistas</h1>
                <p>Gerenciamento dos recepcionistas cadastrados</p>
            </div>

            <button class="btn-primary" @click="abrirNovo">
                Novo recepcionista
            </button>
        </header>

        <p v-if="carregando">
            Carregando recepcionistas...
        </p>

        <p v-else-if="erro" class="erro">
            {{ erro }}
        </p>

        <div v-else class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>CPF</th>
                        <th>Telefone</th>
                        <th>E-mail</th>
                        <th>Ações</th>
                    </tr>
                </thead>

                <tbody>
                    <tr
                        v-for="r in recepcionistas"
                        :key="r.id"
                    >
                        <td>{{ r.nome }}</td>
                        <td>{{ r.cpf }}</td>
                        <td>{{ r.telefone }}</td>
                        <td>{{ r.email }}</td>

                        <td class="acoes">
                            <button @click="abrirEdicao(r)" class="btn-edit">
                                <Pencil :size="18"/>
                            </button>

                            <button
                                class="btn-danger"
                                @click="excluir(r)"
                            >
                                <Trash2 :size="18"/>
                            </button>
                        </td>
                    </tr>

                    <tr v-if="!recepcionistas.length">
                        <td colspan="5">
                            Nenhum recepcionista cadastrado.
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Modal de cadastro e edição -->
        <div v-if="modal" class="modal-overlay">
            <div class="modal-container">
                <div class="modal-header">
                    <div>
                        <h2>
                            {{
                                editando
                                    ? 'Editar recepcionista'
                                    : 'Cadastrar recepcionista'
                            }}
                        </h2>

                        <p>Preencha os dados do recepcionista.</p>
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
                            Nome completo
                            <input
                                v-model="form.nome"
                                required
                            />
                        </label>

                        <label>
                            CPF
                            <input
                                v-model="form.cpf"
                                required
                            />
                        </label>

                        <label>
                            Telefone
                            <input
                                v-model="form.telefone"
                                required
                            />
                        </label>

                        <label>
                            E-mail
                            <input
                                v-model="form.email"
                                type="email"
                                required
                            />
                        </label>

                        <label
                            v-if="!editando"
                            class="campo-largo"
                        >
                            Senha
                            <input
                                v-model="form.senha"
                                type="password"
                                required
                            />
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
                            class="btn-primary"
                            type="submit"
                            :disabled="salvando"
                        >
                            {{
                                salvando
                                    ? 'Salvando...'
                                    : (editando ? 'Atualizar' : 'Cadastrar')
                            }}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'

import {
    listarRecepcionistas,
    salvarRecepcionista,
    atualizarRecepcionista,
    excluirRecepcionista
} from '@/services/recepcionistaService'

import { AwardIcon, Pencil, Trash2 } from 'lucide-vue-next'

const recepcionistas = ref([])
const carregando = ref(true)
const erro = ref('')

const modal = ref(false)
const editando = ref(false)
const salvando = ref(false)
const erroForm = ref('')

function formularioVazio() {
    return {
        nome: '',
        cpf: '',
        telefone: '',
        email: '',
        senha: ''
    }
}

const form = ref(formularioVazio())

function mensagem(error) {
    return (
        error.response?.data?.errors?.join(' ') ||
        error.response?.data?.message ||
        error.message ||
        'Ocorreu um erro.'
    )
}

// Carrega os recepcionistas
async function carregar() {
    carregando.value = true
    erro.value = ''

    try {
        const response = await listarRecepcionistas()

        if (!response.success) {
            throw new Error(mensagem({
                response: { data: response }
            }))
        }

        recepcionistas.value = response.data || []

    } catch (error) {
        erro.value = mensagem(error)
    } finally {
        carregando.value = false
    }
}

// Abre o formulário para novo cadastro
function abrirNovo() {
    editando.value = false
    form.value = formularioVazio()
    erroForm.value = ''
    modal.value = true
}

// Abre o formulário preenchido para edição
function abrirEdicao(recepcionista) {
    editando.value = true

    form.value = {
        id: recepcionista.id,
        nome: recepcionista.nome ,
        cpf: recepcionista.cpf ,
        telefone: recepcionista.telefone ,
        email: recepcionista.email,
        senha: ''
    }

    erroForm.value = ''
    modal.value = true
}

// Fecha o formulário
function fechar() {
    modal.value = false
}

// Cadastra ou atualiza
async function salvar() {
    salvando.value = true
    erroForm.value = ''

    try {
        let response

        if (editando.value) {
            // A API recebe a entidade Recepcionista na atualização.
            const dadosAtualizacao = {
                id: form.value.id,
                nome: form.value.nome,
                cpf: form.value.cpf,
                telefone: form.value.telefone,
                email: form.value.email
            }

            response = await atualizarRecepcionista(dadosAtualizacao)

        } else {
            // A API recebe RecepcionistaDto, incluindo a senha.
            response = await salvarRecepcionista(form.value)
        }

        if (!response.success) {
            throw new Error(mensagem({
                response: { data: response }
            }))
        }

        fechar()
        await carregar()

    } catch (error) {
        erroForm.value = mensagem(error)
    } finally {
        salvando.value = false
    }
}

// Exclui um recepcionista
async function excluir(recepcionista) {
    const confirmar = window.confirm(
        `Deseja realmente excluir o recepcionista ${recepcionista.nome}?`
    )

    if (!confirmar) return

    try {
        const response = await excluirRecepcionista(
            recepcionista.id
        )

        if (!response.success) {
            throw new Error(mensagem({
                response: { data: response }
            }))
        }

        await carregar()

    } catch (error) {
        erro.value = mensagem(error)
    }
}

onMounted(carregar)
</script>

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
    margin: 0;
}

.page-header p {
    margin: 6px 0;
    color: #64748b;
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

.btn-secondary {
    background: #e5e7eb;
    color: #111827;
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
    width: min(760px, 100%);
    max-height: 90vh;
    overflow: auto;
}

.modal-header,
.form-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.modal-header {
    margin-bottom: 18px;
}

.modal-header h2 {
    margin: 0;
}

.modal-header p {
    margin: 5px 0;
    color: #64748b;
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
}

.form-grid label {
    display: flex;
    flex-direction: column;
    gap: 6px;
    font-size: 14px;
}

.form-grid input {
    width: 100%;
    box-sizing: border-box;
    border: 1px solid #cbd5e1;
    border-radius: 6px;
    padding: 10px;
    font: inherit;
}

.campo-largo {
    grid-column: 1 / -1;
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

    .page-header {
        align-items: flex-start;
        gap: 12px;
        flex-direction: column;
    }
}

.btn-edit{
    background: #dbeafe;
    color: #2563eb;
}
</style>
