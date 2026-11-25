
<template>
    <section class="pacientes-page">
        <div class="page-header">
            <div>
                <h1>Pacientes</h1>
                <p>Gerenciamento dos pacientes cadastrados</p>
            </div>

            <button class="btn-primary" @click="abrirFormulario">
                Novo paciente
            </button>
        </div>

        <!-- Modal de cadastro e edição -->
        <div v-if="mostrarFormulario" class="modal-overlay">
            <div class="modal-container">

                <div class="modal-header">
                    <div>
                        <h2>
                            {{ modoEdicao ? 'Editar paciente' : 'Cadastrar paciente' }}
                        </h2>

                        <p>Preencha os dados do paciente.</p>
                    </div>

                    <button
                        type="button"
                        class="btn-fechar"
                        @click="mostrarFormulario = false"
                    >
                        ×
                    </button>
                </div>

                <form @submit.prevent="salvarCadastro">

                    <p v-if="erroCadastro" class="erro">
                        {{ erroCadastro }}
                    </p>

                    <div class="form-grid">

                        <div class="form-group">
                            <label>Nome completo</label>
                            <input v-model="novoPaciente.nome" required />
                        </div>

                        <div class="form-group">
                            <label>CPF</label>
                            <input v-model="novoPaciente.cpf" required />
                        </div>

                        <div class="form-group">
                            <label>Data de nascimento</label>
                            <input
                                v-model="novoPaciente.dataNasc"
                                type="date"
                                required
                            />
                        </div>

                        <div class="form-group">
                            <label>Sexo</label>
                            <input v-model="novoPaciente.sexo" required />
                        </div>

                        <div class="form-group">
                            <label>Telefone</label>
                            <input v-model="novoPaciente.telefone" required />
                        </div>

                        <div class="form-group">
                            <label>E-mail</label>
                            <input
                                v-model="novoPaciente.email"
                                type="email"
                            />
                        </div>

                        <div class="form-group">
                            <label>Cidade</label>
                            <input v-model="novoPaciente.cidade" required />
                        </div>

                        <div class="form-group">
                            <label>Bairro</label>
                            <input v-model="novoPaciente.bairro" required />
                        </div>

                        <div class="form-group">
                            <label>Rua</label>
                            <input v-model="novoPaciente.rua" required />
                        </div>

                        <div class="form-group">
                            <label>Plano de saúde</label>
                            <input v-model="novoPaciente.plano" />
                        </div>

                        <div class="form-group campo-completo">
                            <label>Histórico</label>
                            <textarea
                                v-model="novoPaciente.historico"
                                rows="3"
                            ></textarea>
                        </div>

                    </div>

                    <div class="form-actions">
                        <button
                            type="button"
                            class="btn-secondary"
                            @click="mostrarFormulario = false"
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
                                    : (modoEdicao ? 'Atualizar' : 'Cadastrar')
                            }}
                        </button>
                    </div>

                </form>
            </div>
        </div>

        <!-- Carregamento -->
        <p v-if="carregando">
            Carregando pacientes...
        </p>

        <!-- Erro -->
        <p v-else-if="erro" class="erro">
            {{ erro }}
        </p>

        <!-- Tabela -->
        <div v-else class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>CPF</th>
                        <th>Data de nascimento</th>
                        <th>Sexo</th>
                        <th>Telefone</th>
                        <th>E-mail</th>
                        <th>Endereço</th>
                        <th>Plano</th>
                        <th>Histórico</th>
                        <th>Ações</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="paciente in pacientes" :key="paciente.idPaciente">
                        <td>{{ paciente.nome }}</td>
                        <td>{{ paciente.cpf }}</td>
                        <td>{{ paciente.dataNasc }}</td>
                        <td>{{ paciente.sexo }}</td>
                        <td>{{ paciente.telefone }}</td>
                        <td>{{ paciente.email || 'Não informado' }}</td>

                        <td>
                            {{
                                [paciente.rua, paciente.bairro, paciente.cidade]
                                    .filter(Boolean)
                                    .join(', ') || 'Não informado'
                            }}
                        </td>

                        <td>{{ paciente.plano }}</td>
                        <td>{{ paciente.historico || 'Não informado' }}</td>

                        <td class="acoes">
                            <button
                                class="btn-acao btn-editar"
                                title="Editar médico"
                                @click="abrirEdicao(paciente)"
                            >
                                <Pencil :size="18" />
                            </button>

                            <button
                                class="btn-acao btn-excluir"
                                title="Excluir médico"
                                @click="excluir(paciente)"
                            >
                                <Trash2 :size="18" />
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>
</template>

<script setup>

import { ref, onMounted } from 'vue'

import {
    listarPacientes,
    salvarPacientes,
    atualizarPaciente,
    excluirPaciente
} from '@/services/pacienteService'

import { Pencil, Trash2 } from 'lucide-vue-next'

// Lista de pacientes
const pacientes = ref([])

// Estados da tela
const carregando = ref(true)
const erro = ref('')
const mostrarFormulario = ref(false)
const modoEdicao = ref(false)
const salvando = ref(false)
const erroCadastro = ref('')

// Dados do formulário
const novoPaciente = ref({
    nome: '',
    cpf: '',
    dataNasc: '',
    sexo: '',
    telefone: '',
    email: '',
    cidade: '',
    bairro: '',
    rua: '',
    plano: '',
    historico: ''
})

// Limpa os campos do formulário
function limparFormulario() {
    novoPaciente.value = {
        nome: '',
        cpf: '',
        dataNasc: '',
        sexo: '',
        telefone: '',
        email: '',
        cidade: '',
        bairro: '',
        rua: '',
        plano: '',
        historico: ''
    }
}

// Formata a data para exibição
function formatarData(data) {
    if (!data) return ''

    const [ano, mes, dia] = data.split('T')[0].split('-')

    return `${dia}/${mes}/${ano}`
}

// Carrega os pacientes da API
async function carregarPacientes() {
    try {
        carregando.value = true
        erro.value = ''

        const response = await listarPacientes()

        if (!response.success) {
            throw new Error(response.message || 'Erro ao carregar pacientes.')
        }

        pacientes.value = response.data || []

    } catch (error) {
        erro.value =
            error.response?.data?.message ||
            error.message ||
            'Erro ao carregar pacientes.'

    } finally {
        carregando.value = false
    }
}

// Abre o formulário para cadastrar
function abrirFormulario() {
    modoEdicao.value = false
    erroCadastro.value = ''

    limparFormulario()

    mostrarFormulario.value = true
}

// Cadastra ou atualiza um paciente
async function salvarCadastro() {
    try {
        salvando.value = true
        erroCadastro.value = ''

        let response

        if (modoEdicao.value) {
            response = await atualizarPaciente(novoPaciente.value)
        } else {
            response = await salvarPacientes(novoPaciente.value)
        }

        if (!response.success) {
            throw new Error(
                response.errors?.join(', ') ||
                response.message ||
                'Não foi possível salvar o paciente.'
            )
        }

        // Fecha o formulário
        mostrarFormulario.value = false
        modoEdicao.value = false

        // Limpa os campos
        limparFormulario()

        // Atualiza a listagem
        await carregarPacientes()

    } catch (error) {
        erroCadastro.value =
            error.response?.data?.errors?.join(', ') ||
            error.response?.data?.message ||
            error.message ||
            'Erro ao salvar paciente.'

    } finally {
        salvando.value = false
    }
}

// Abre o formulário para editar
function abrirEdicao(paciente) {
    modoEdicao.value = true

    novoPaciente.value = {
        idPaciente: paciente.idPaciente,
        nome: paciente.nome,
        cpf: paciente.cpf,
        dataNasc: paciente.dataNasc?.split('T')[0] || '',
        sexo: paciente.sexo,
        telefone: paciente.telefone,
        email: paciente.email,
        cidade: paciente.cidade,
        bairro: paciente.bairro,
        rua: paciente.rua,
        plano: paciente.plano,
        historico: paciente.historico
    }

    erroCadastro.value = ''
    mostrarFormulario.value = true
}

// Exclui um paciente
async function excluir(paciente) {
    const confirmar = window.confirm(
        `Deseja realmente excluir o paciente ${paciente.nome}?`
    )

    if (!confirmar) return

    try {
        const response = await excluirPaciente(paciente.idPaciente)

        if (!response.success) {
            throw new Error(
                response.errors?.join(', ') ||
                response.message ||
                'Não foi possível excluir o paciente.'
            )
        }

        await carregarPacientes()

    } catch (error) {
        erro.value =
            error.response?.data?.errors?.join(', ') ||
            error.response?.data?.message ||
            error.message ||
            'Erro ao excluir paciente.'
    }
}

// Carrega os pacientes quando a tela é aberta
onMounted(() => {
    carregarPacientes()
})

</script>

<style>

.pacientes-page {
    width: 100%;
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
    color: #64748b;
}

.btn-primary {
    background: #2563eb;
    color: white;
    border: none;
    padding: 10px 18px;
    border-radius: 6px;
    cursor: pointer;
}

.table-container {
    width: 100%;
    overflow-x: auto;
    background: white;
    border-radius: 8px;
}

table {
    width: 100%;
    border-collapse: collapse;
    text-align: left;
}

th,
td {
    padding: 14px;
    border-bottom: 1px solid #e2e8f0;
    white-space: nowrap;
}

th {
    background: #f8fafc;
    color: #475569;
}

.erro {
    color: #dc2626;
}

.modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(15, 23, 42, 0.55);
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 20px;
    z-index: 1000;
}

.modal-container {
    display: block;
    box-sizing: border-box;
    background: #ffffff;
    width: 100%;
    max-width: 650px;
    max-height: 90vh;
    overflow-y: auto;
    padding: 30px;
    border-radius: 12px;
    box-shadow: 0 15px 40px rgba(0, 0, 0, 0.2);
}

.modal-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 28px;
}

.modal-header h2 {
    margin: 0 0 6px;
    color: #1e293b;
    font-size: 22px;
}

.modal-header p {
    margin: 0;
    color: #64748b;
    font-size: 14px;
}

.btn-fechar {
    border: none;
    background: transparent;
    font-size: 28px;
    color: #64748b;
    cursor: pointer;
}

.form-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 20px;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 7px;
}

.form-group label {
    font-size: 14px;
    font-weight: 600;
    color: #334155;
}

.form-group input,
.form-group textarea {
    box-sizing: border-box;
    width: 100%;
    padding: 11px 12px;
    border: 1px solid #cbd5e1;
    border-radius: 6px;
    outline: none;
    font-size: 14px;
    font-family: inherit;
}

.form-group input:focus,
.form-group textarea:focus {
    border-color: #2563eb;
    box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
}

.form-group textarea {
    resize: vertical;
}

.campo-completo {
    grid-column: 1 / -1;
}

.form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 12px;
    margin-top: 28px;
}

.btn-primary,
.btn-secondary {
    padding: 11px 18px;
    border-radius: 6px;
    cursor: pointer;
    font-size: 14px;
    font-weight: 600;
}

.btn-primary {
    background: #2563eb;
    color: white;
    border: none;
}

.btn-secondary {
    background: rgb(238, 16, 16);
    color: #ffffff;
    border: 1px solid #cbd5e1;
}

.acoes {
    display: flex;
    align-items: center;
    gap: 8px;
}

.btn-acao {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 30px;
    height: 30px;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    transition: background 0.2s;
}

.btn-editar {
    background: #dbeafe;
    color: #2563eb;
}

.btn-editar:hover {
    background: #bfdbfe;
}

.btn-excluir {
    background: #fee2e2;
    color: #dc2626;
}

.btn-excluir:hover:not(:disabled) {
    background: #fecaca;
}

.btn-acao:disabled {
    opacity: 0.4;
    cursor: not-allowed;
}

@media (max-width: 600px) {
    .form-grid {
        grid-template-columns: 1fr;
    }

    .campo-completo {
        grid-column: auto;
    }

    .modal-container {
        padding: 22px;
    }
}

</style>
