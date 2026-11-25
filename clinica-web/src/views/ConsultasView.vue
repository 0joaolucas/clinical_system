<template>
    <section class="entity-page">
        <header class="page-header">
            <div>
                <h1>Consultas</h1>
                <p>Agendamento e gerenciamento de consultas</p>
            </div>

            <button class="btn-primary" @click="novo">
                Nova consulta
            </button>
        </header>

        <p v-if="carregando">
            Carregando consultas...
        </p>

        <p v-else-if="erro" class="erro">
            {{ erro }}
        </p>

        <div v-else class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>Paciente</th>
                        <th>Médico</th>
                        <th>Data</th>
                        <th>Horário</th>
                        <th>Tipo</th>
                        <th>Ações</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="c in lista" :key="c.idConsulta">
                        <td>
                            {{ c.paciente?.nome || nomePaciente(c.idPaciente) }}
                        </td>

                        <td>
                            {{ c.medico?.nome || nomeMedico(c.idMedico) }}
                        </td>

                        <td>{{ c.dataConsulta }}</td>

                        <td>
                            {{ String(c.horaConsulta || '').slice(0, 5) }}
                        </td>

                        <td>{{ c.tipoConsulta }}</td>

                        <td class="acoes">
                            <button @click="editar(c)" class="btn-edit">
                                <Pencil :size="18"/>
                            </button>

                            <button class="btn-danger" @click="excluir(c)">
                                <Trash2 :size="18"/>
                            </button>
                        </td>
                    </tr>

                    <tr v-if="!lista.length">
                        <td colspan="6">
                            Nenhuma consulta cadastrada.
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
                            {{ editando ? 'Editar consulta' : 'Cadastrar consulta' }}
                        </h2>

                        <p>
                            Informe o paciente, médico e os dados do agendamento.
                        </p>
                    </div>

                    <button
                        class="btn-fechar"
                        type="button"
                        @click="modal = false"
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
                            Paciente

                            <select
                                v-model.number="form.idPaciente"
                                required
                            >
                                <option value="" disabled>
                                    Selecione
                                </option>

                                <option
                                    v-for="p in pacientes"
                                    :key="p.idPaciente"
                                    :value="p.idPaciente"
                                >
                                    {{ p.nome }} — {{ p.cpf }}
                                </option>
                            </select>
                        </label>

                        <label>
                            Médico

                            <select
                                v-model.number="form.idMedico"
                                required
                            >
                                <option value="" disabled>
                                    Selecione
                                </option>

                                <option
                                    v-for="m in medicos"
                                    :key="m.idMedico"
                                    :value="m.idMedico"
                                >
                                    {{ m.nome }} — {{ m.crm }}
                                </option>
                            </select>
                        </label>

                        <label>
                            Data

                            <input
                                v-model="form.dataConsulta"
                                type="date"
                                required
                            >
                        </label>

                        <label>
                            Horário

                            <input
                                v-model="form.horaConsulta"
                                type="time"
                                required
                            >
                        </label>

                        <label>
                            Tipo de consulta

                            <input
                                v-model="form.tipoConsulta"
                                required
                            >
                        </label>

                        <label class="campo-largo">
                            Observação

                            <textarea
                                v-model="form.observacao"
                                rows="3"
                            ></textarea>
                        </label>
                    </div>

                    <div class="form-actions">
                        <button
                            type="button"
                            class="btn-secondary"
                            @click="modal = false"
                        >
                            Cancelar
                        </button>

                        <button
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
    listarConsultas,
    cadastrarConsulta,
    atualizarConsulta,
    excluirConsulta
} from '@/services/consultaService';

import { listarPacientes } from '@/services/pacienteService';
import { listarMedicos } from '@/services/medicoService';
import { AwardIcon, Pencil, Trash2 } from 'lucide-vue-next'

// Estados da tela
const lista = ref([]);
const pacientes = ref([]);
const medicos = ref([]);

const carregando = ref(true);
const erro = ref('');

const modal = ref(false);
const editando = ref(false);
const salvando = ref(false);
const erroForm = ref('');

// Formulário
const vazio = () => ({
    idPaciente: '',
    idMedico: '',
    dataConsulta: '',
    horaConsulta: '',
    observacao: '',
    tipoConsulta: ''
});

const form = ref(vazio());

// Mensagens de erro
const msg = (e) =>
    e.response?.data?.message ||
    e.response?.data?.errors?.join?.(' ') ||
    e.message ||
    'Ocorreu um erro.';

// Carregar consultas, pacientes e médicos
async function carregar() {
    carregando.value = true;
    erro.value = '';

    try {
        const [a, b, c] = await Promise.all([
            listarConsultas(),
            listarPacientes(),
            listarMedicos()
        ]);


        for (const r of [a, b, c]) {
            if (!r.success) {
                throw Error(r.message);
            }
        }

        lista.value = a.data || [];
        pacientes.value = b.data || [];
        medicos.value = c.data || [];

        // Verificar as listas carregadas
        console.log('Lista de pacientes:', pacientes.value);
        console.log('Lista de médicos:', medicos.value);

    } catch (e) {
        erro.value = msg(e);

    } finally {
        carregando.value = false;
    }
}

// Buscar nome do paciente pelo ID
function nomePaciente(id) {
    return pacientes.value.find(
        x => x.idPaciente === id
    )?.nome || `ID ${id ?? ''}`;
}

// Buscar nome do médico pelo ID
function nomeMedico(id) {
    return medicos.value.find(
        x => x.idMedico === id
    )?.nome || `ID ${id ?? ''}`;
}

// Abrir formulário para nova consulta
function novo() {
    editando.value = false;
    form.value = vazio();
    erroForm.value = '';
    modal.value = true;
}

// Abrir formulário para editar consulta
function editar(c) {
    editando.value = true;

    form.value = {
        idConsulta: c.idConsulta,
        idPaciente: c.idPaciente || c.paciente?.idPaciente || '',
        idMedico: c.idMedico || c.medico?.idMedico || '',
        dataConsulta: String(c.dataConsulta || '').slice(0, 10),
        horaConsulta: String(c.horaConsulta || '').slice(0, 5),
        observacao: c.observacao || '',
        tipoConsulta: c.tipoConsulta || ''
    };

    erroForm.value = '';
    modal.value = true;
}

// Salvar consulta
async function salvar() {
    salvando.value = true;
    erroForm.value = '';

    try {
        const payload = {
            ...form.value,
            horaConsulta: form.value.horaConsulta.length === 5
                ? `${form.value.horaConsulta}:00`
                : form.value.horaConsulta
        };

        const r = editando.value
            ? await atualizarConsulta(payload)
            : await cadastrarConsulta(payload);

        if (!r.success) {
            throw Error(r.message || r.errors?.join(' '));
        }

        modal.value = false;

        await carregar();

    } catch (e) {
        erroForm.value = msg(e);

    } finally {
        salvando.value = false;
    }
}

// Excluir consulta
async function excluir(c) {
    if (!confirm('Deseja realmente excluir esta consulta?')) {
        return;
    }

    try {
        const r = await excluirConsulta(c.idConsulta);

        if (!r.success) {
            throw Error(r.message);
        }

        await carregar();

    } catch (e) {
        erro.value = msg(e);
    }
}

// Carregar dados ao abrir a tela
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
    width: min(760px, 100%);
    max-height: 90vh;
    overflow: auto;
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

.form-grid input,
.form-grid select,
.form-grid textarea {
    padding: 10px;
    border: 1px solid #cbd5e1;
    border-radius: 6px;
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
}

.btn-edit{
    background: #dbeafe;
    color: #2563eb;
}
</style>