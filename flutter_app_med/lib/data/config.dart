class Config{
  static String sql = '''
    CREATE TABLE IF NOT EXISTS Medicos (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    cpf TEXT NOT NULL,
    nome TEXT NOT NULL,
    crm TEXT NOT NULL UNIQUE,
    especialidade TEXT NOT NULL,
    telefone TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE,
    funcao TEXT NOT NULL,
    senhaTemp TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Recepcionista (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    cpf TEXT NOT NULL,
    nome TEXT NOT NULL,
    telefone TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE,
    senhaTemp TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Login (
    idLogin INTEGER PRIMARY KEY AUTOINCREMENT,
    cpf TEXT NOT NULL,
    nome TEXT NOT NULL,
    senha TEXT NOT NULL,
    funcao TEXT NOT NULL,
    FOREIGN KEY (cpf) REFERENCES Medicos (cpf) ON DELETE CASCADE ,
    FOREIGN KEY (cpf) REFERENCES Recepcionista (cpf) ON DELETE CASCADE
);


CREATE TABLE IF NOT EXISTS Medicamentos (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    nome TEXT NOT NULL,
    quantidade INTEGER NOT NULL
);



CREATE TABLE IF NOT EXISTS Pacientes (
       id INTEGER PRIMARY KEY AUTOINCREMENT,
       nome TEXT NOT NULL,
       idade INTEGER NOT NULL,
       sexo TEXT NOT NULL,
       cpf TEXT NOT NULL UNIQUE,
       telefone TEXT NOT NULL,
       email TEXT NOT NULL,
       cidade TEXT NOT NULL,
       bairro TEXT NOT NULL,
       rua TEXT NOT NULL,
       plano TEXT,
       historico TEXT
);

CREATE TABLE IF NOT EXISTS Consultas (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    idPaciente INTEGER NOT NULL,
    idMedico INTEGER NOT NULL,
    dataConsulta DATETIME NOT NULL,
    horaConsulta TIME NOT NULL,
    observacoes TEXT,
    tipoConsulta TEXT NOT NULL,
    FOREIGN KEY (idPaciente) REFERENCES Pacientes (id) ON DELETE CASCADE,
    FOREIGN KEY (idMedico) REFERENCES Medicos (cpf) ON DELETE CASCADE
);




CREATE TABLE IF NOT EXISTS Transacoes (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    idPaciente INTEGER NOT NULL,
    idRemedio INTEGER,
    quantidade INTEGER ,
    valor DECIMAL(10, 2) NOT NULL,
    dataTransacao DATE NOT NULL,
    FOREIGN KEY (idRemedio) REFERENCES Medicamentos (id) ON DELETE CASCADE,
    FOREIGN KEY (idPaciente) REFERENCES Pacientes (id) ON DELETE CASCADE
);

CREATE TRIGGER AtualizarQuantidadeRemedio
AFTER INSERT ON Transacoes
FOR EACH ROW
BEGIN
    UPDATE Medicamentos
    SET quantidade = quantidade - NEW.quantidade
    WHERE id = NEW.idRemedio;
END;

  ''';
}