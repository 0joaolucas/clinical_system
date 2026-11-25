<template>
    <section class="medicos-page">
        <div class="page-header">
            <div>
                <h1>Médicos</h1>
                <p>Gerenciamento dos médicos cadastrados</p>
            </div>

            <button class="btn-primary" @click="abrirFormulario">
                Novo médico
            </button>
        </div>

        <div v-if="mostrarFormulario" class="modal-overlay">
            <div class="modal-container">
                
                    <div class="modal-container">

                        <div class="modal-header">
                            <div>
                                <h2>{{ modoEdicao ? 'Editar médico' : 'Cadastrar médico'}}</h2>
                                <p>Preencha os dados do profissional.</p>
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
                                    <input v-model="novoMedico.nome" required />
                                </div>

                                <div class="form-group">
                                    <label>CPF</label>
                                    <input v-model="novoMedico.cpf" required />
                                </div>

                                <div class="form-group">
                                    <label>CRM</label>
                                    <input v-model="novoMedico.crm" required />
                                </div>

                                <div class="form-group">
                                    <label>Especialidade</label>
                                    <input v-model="novoMedico.especialidade" required />
                                </div>

                                <div class="form-group">
                                    <label>Telefone</label>
                                    <input v-model="novoMedico.telefone" required />
                                </div>

                                <div class="form-group">
                                    <label>E-mail</label>
                                    <input v-model="novoMedico.email" type="email" required />
                                </div>

                                <div class="form-group">
                                    <label>Função</label>
                                    <input v-model="novoMedico.funcao" required />
                                </div>

                                <div class="form-group" v-if="!modoEdicao">
                                    <label>Senha</label>
                                    <input v-model="novoMedico.senha" type="password" required />
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

                                <button type="submit" class="btn-primary" :disabled="salvando">
                                    {{ salvando ? 'Salvando...' : (modoEdicao ? 'Atualizar': 'Cadastrar')}}
                                </button>
                            </div>

                        </form>
                    </div>
                </div>

        </div>

        <p v-if="carregando">Carregando médicos...</p>

        <p v-else-if="erro">
            {{ erro }}
        </p>

        <div v-else class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>CPF</th>
                        <th>CRM</th>
                        <th>Especialidade</th>
                        <th>Telefone</th>
                        <th>E-mail</th>
                        <th>Ações</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="medico in medicos" :key="medico.idMedico">
                        <td>{{ medico.nome }}</td>
                        <td>{{ medico.cpf }}</td>
                        <td>{{ medico.crm }}</td>
                        <td>{{ medico.especialidade }}</td>
                        <td>{{ medico.telefone }}</td>
                        <td>{{ medico.email }}</td>
                        <td class="acoes">
                            <button
                                class="btn-acao btn-editar"
                                title="Editar médico"
                                @click="abrirEdicao(medico)"
                            >
                                <Pencil :size="18" />
                            </button>

                            <button
                                class="btn-acao btn-excluir"
                                title="Excluir médico"
                                @click="excluir(medico)"
                            >
                                <Trash2 :size="18" />
                            </button>
                        </td>
                                            </tr>

                    <tr v-if="medicos.length === 0">
                        <td colspan="7">
                            Nenhum médico cadastrado.
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>
</template>

<script setup>

import { ref, onMounted } from 'vue'
import { listarMedicos, salvarMedico, atualizarMedico, excluirMedico } from '@/services/medicoService';
import { AwardIcon, Pencil, Trash2 } from 'lucide-vue-next'

const medicos = ref([])
const carregando = ref(true)
const erro = ref('')
const mostrarFormulario = ref(false)
const modoEdicao = ref(false)


const novoMedico = ref({
    nome: '',
    cpf: '',
    crm: '',
    especialidade: '',
    telefone: '',
    email: '',
    funcao: '',
    senha: ''
})

async function carregarMedicos(){
    try{
        carregando.value = true
        erro.value = ''

        const response = await listarMedicos()

        if(!response.success){
            throw new Error(response.message)
        }

        medicos.value = response.data
    }catch(error){
        erro.value = error.response?.data?.message || error.message || 'Erro ao carregar médicos'
    }finally{
        carregando.value = false
    }
}

function abrirFormulario(){
    modoEdicao.value = false
    erroCadastro.value = ''

    novoMedico.value = {
        nome: '',
        cpf: '',
        crm: '',
        especialidade: '',
        telefone: '',
        email: '',
        funcao: '',
        senha: ''
    }

    mostrarFormulario.value = true
}

const erroCadastro = ref('')
const salvando = ref(false)

async function salvarCadastro() {
    try{
        salvando.value = true
        erroCadastro.value = ''

        let response

        if(modoEdicao.value){
            response = await atualizarMedico(novoMedico.value)
        }else{
            response = await salvarMedico(novoMedico.value)
        }

        if(!response.success){
            throw new Error(response.message || 'Não foi possível salvar o médico.')
        }

        //fechar formulário
        mostrarFormulario.value = false
        modoEdicao.value = false

        //limpar os campos
        novoMedico.value = {
            nome: '',
            cpf: '',
            crm: '',
            especialidade: '',
            telefone: '',
            email: '',
            funcao: '',
            senha: ''
        }

        //atualizar listagem
        await carregarMedicos()

    }catch(error){
        erroCadastro.value = error.response?.data?.message || error.message || 'Erro ao salvar médico.'
    }finally{
        salvando.value = false
    }
}

function abrirEdicao(medico){
    modoEdicao.value = true

    novoMedico.value = {
        idMedico: medico.idMedico,
        nome: medico.nome,
        cpf: medico.cpf,
        crm: medico.crm,
        especialidade: medico.especialidade,
        telefone: medico.telefone,
        email: medico.email,
        funcao: medico.funcao,
        senha: ''
    }

    erroCadastro.value = ''
    mostrarFormulario.value = true
}

async function excluir(medico){
    const confirmar = window.confirm(`Deseja realmente excluir o médico ${medico.nome}?`)

    if(!confirmar) return

    try{
        const response = await excluirMedico(medico.idMedico)

        if(!response.success){
            throw new Error(response.message || 'Não foi possível excluir o médico.')
        }

        await carregarMedicos()

    }catch(error){
        erro.value = error.response?.data?.message || error.message || 'Erro ao excluir médico.'
    }
}

onMounted(() => {
    carregarMedicos()
})

</script>

<style scoped>

.medicos-page{
    width: 100%;
}

.page-header{
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
}

.page-header h1{
    margin: 0 0 6px;
}

.page-header p{
    margin: 0;
    color: #64748b;
}

.btn-primary{
    background: #2563eb;
    color: white;
    border: none;
    padding: 10px 18px;
    border-radius: 6px;
    cursor: pointer;
}

.table-container{
    width: 100%;
    overflow-x: auto;
    background: white;
    border-radius: 8px;
}

table{
    width: 100%;
    border-collapse: collapse;
    text-align: left;
}

th, td{
    padding: 14px;
    border-bottom: 1px solid #e2e8f0;
    white-space: nowrap;
}

th{
    background: #f8fafc;
    color: #475569;
}

.erro{
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

.form-group input {
    box-sizing: border-box;
    width: 100%;
    padding: 11px 12px;
    border: 1px solid #cbd5e1;
    border-radius: 6px;
    outline: none;
    font-size: 14px;
}

.form-group input:focus {
    border-color: #2563eb;
    box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
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

@media (max-width: 600px) {
    .form-grid {
        grid-template-columns: 1fr;
    }

    .modal-container {
        padding: 22px;
    }
}

.acoes{
    display: flex;
    align-items: center;
    gap: 8px;
}

.btn-acao{
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

.btn-editar{
    background: #dbeafe;
    color: #2563eb;
}

.btn-editar:hover{
    background: #bfdbfe;
}

.btn-excluir{
    background: #fee2e2;
    color: #dc2626
}

.btn-excluir:hover:not(:disabled){
    background: #fecaca;
}

.btn-acao:disabled{
    opacity: 0.4;
    cursor: not-allowed;
}


</style>