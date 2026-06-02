// =========================================
// 0. INICIALIZAÇÃO E ESTADO GLOBAL
// ==========================================

let responsaveis = JSON.parse(localStorage.getItem('nv_responsaveis')) || [];
let eventos = JSON.parse(localStorage.getItem('nv_eventos')) || [];
let alunos = JSON.parse(localStorage.getItem('alunos')) || []; // Chave usada no inscricao.js

document.addEventListener('DOMContentLoaded', () => {
  // Escuta os botões de envio dos formulários
  document
    .getElementById('form-responsavel')
    .addEventListener('submit', salvarResponsavel);
  document
    .getElementById('form-evento')
    .addEventListener('submit', salvarEvento);

  // Roda as funções de tela logo que a página carrega
  renderizarResponsaveis();
  atualizarSelectResponsaveis();
  renderizarEventos();
  renderizarAlunos();
});

window.addEventListener('storage', (event) => {
  if (event.key === 'alunos') {
    alunos = JSON.parse(localStorage.getItem('alunos')) || [];
    renderizarAlunos();
  }
});

// ==========================================
// 1. ESPAÇO RESPONSÁVEIS
// ==========================================

function salvarResponsavel(e) {
  e.preventDefault();

  let index = document.getElementById('resp-index').value;
  let novoResp = {
    cpf: document.getElementById('resp-cpf').value,
    nome: document.getElementById('resp-nome').value,
    telefone: document.getElementById('resp-telefone').value,
    email: document.getElementById('resp-email').value,
  };

  if (index === '') {
    responsaveis.push(novoResp);
  } else {
    responsaveis[index] = novoResp;
    document.getElementById('resp-index').value = '';
  }

  localStorage.setItem('nv_responsaveis', JSON.stringify(responsaveis));
  document.getElementById('form-responsavel').reset();

  renderizarResponsaveis();
  atualizarSelectResponsaveis(); // Atualiza o select de eventos na hora
}

function renderizarResponsaveis() {
  let corpo = document.getElementById('corpo-responsaveis');
  corpo.innerHTML = '';

  responsaveis.forEach((resp, i) => {
    corpo.innerHTML += `
            <tr>
                <td>${resp.cpf}</td>
                <td>${resp.nome}</td>
                <td>${resp.telefone}</td>
                <td>${resp.email}</td>
                <td>
                    <button class="btn-tabela btn-editar" onclick="editarResponsavel(${i})">Editar</button>
                    <button class="btn-tabela btn-excluir" onclick="excluirResponsavel(${i})">Excluir</button>
                </td>
            </tr>
        `;
  });
}

function editarResponsavel(i) {
  document.getElementById('resp-index').value = i;
  document.getElementById('resp-cpf').value = responsaveis[i].cpf;
  document.getElementById('resp-nome').value = responsaveis[i].nome;
  document.getElementById('resp-telefone').value = responsaveis[i].telefone;
  document.getElementById('resp-email').value = responsaveis[i].email;
}

function excluirResponsavel(i) {
  if (confirm('Excluir este responsável?')) {
    responsaveis.splice(i, 1);
    localStorage.setItem('nv_responsaveis', JSON.stringify(responsaveis));
    renderizarResponsaveis();
    atualizarSelectResponsaveis();
  }
}

// ==========================================
// 2. ESPAÇO EVENTOS
// ==========================================

function atualizarSelectResponsaveis() {
  let select = document.getElementById('evento-responsavel');
  select.innerHTML = '<option value="">Selecione um responsável...</option>';

  responsaveis.forEach((resp) => {
    select.innerHTML += `<option value="${resp.nome}">${resp.nome}</option>`;
  });
}

function salvarEvento(e) {
  e.preventDefault();

  let index = document.getElementById('evento-index').value;
  let novoEvento = {
    nome: document.getElementById('evento-nome').value,
    responsavel: document.getElementById('evento-responsavel').value,
    descricao: document.getElementById('evento-descricao').value,
    dataInicio: document.getElementById('evento-data-inicio').value,
    dataFim: document.getElementById('evento-data-fim').value,
    horario: document.getElementById('evento-horario').value,
    inscInicio: document.getElementById('evento-insc-inicio').value,
    inscFim: document.getElementById('evento-insc-fim').value,
    capacidade: document.getElementById('evento-capacidade').value,
    recorrente: document.getElementById('evento-recorrente').value,
    diaDaSemana: document.getElementById('evento-dias').value,
  };

  if (index === '') {
    eventos.push(novoEvento);
  } else {
    eventos[index] = novoEvento;
    document.getElementById('evento-index').value = '';
  }

  localStorage.setItem('nv_eventos', JSON.stringify(eventos));
  document.getElementById('form-evento').reset();
  renderizarEventos();
}

function renderizarEventos() {
  let corpo = document.getElementById('corpo-eventos');
  corpo.innerHTML = '';

  eventos.forEach((ev, i) => {
    corpo.innerHTML += `
            <tr>
                <td>${ev.nome}</td>
                <td>${ev.responsavel}</td>
                <td>${ev.dataInicio} até ${ev.dataFim}</td>
                <td>${ev.horario} ${ev.recorrente === 'Sim' ? '(' + ev.diaDaSemana + ')' : ''}</td>
                <td>${ev.capacidade}</td>
                <td>
                    <button class="btn-tabela btn-editar" onclick="editarEvento(${i})">Editar</button>
                    <button class="btn-tabela btn-excluir" onclick="excluirEvento(${i})">Excluir</button>
                </td>
            </tr>
        `;
  });
}

function editarEvento(i) {
  document.getElementById('evento-index').value = i;
  document.getElementById('evento-nome').value = eventos[i].nome;
  document.getElementById('evento-responsavel').value = eventos[i].responsavel;
  document.getElementById('evento-descricao').value = eventos[i].descricao;
  document.getElementById('evento-data-inicio').value = eventos[i].dataInicio;
  document.getElementById('evento-data-fim').value = eventos[i].dataFim;
  document.getElementById('evento-horario').value = eventos[i].horario;
  document.getElementById('evento-insc-inicio').value = eventos[i].inscInicio;
  document.getElementById('evento-insc-fim').value = eventos[i].inscFim;
  document.getElementById('evento-capacidade').value = eventos[i].capacidade;
  document.getElementById('evento-recorrente').value = eventos[i].recorrente;
  document.getElementById('evento-dias').value = eventos[i].diaDaSemana;
}

function excluirEvento(i) {
  if (confirm('Excluir este evento?')) {
    eventos.splice(i, 1);
    localStorage.setItem('nv_eventos', JSON.stringify(eventos));
    renderizarEventos();
  }
}

// ==========================================
// 3. ESPAÇO INSCRIÇÕES (ALUNOS)
// ==========================================

function renderizarAlunos() {
  let corpo = document.getElementById('corpo-alunos');
  corpo.innerHTML = '';
  alunos = JSON.parse(localStorage.getItem('alunos')) || [];

  // Apenas itera o array de alunos salvo pelo inscricao.js e joga na tela
  alunos.forEach((aluno) => {
    // Se a chave do evento não existir ainda no objeto aluno, exibe 'Indefinido'
    let eventoInscrito = aluno.evento ? aluno.evento : 'Indefinido/Nenhum';

    corpo.innerHTML += `
            <tr>
                <td>${aluno.cpf}</td>
                <td>${aluno.nome}</td>
                <td>${aluno.telefone}</td>
                <td>${aluno.email}</td>
                <td><strong>${eventoInscrito}</strong></td>
            </tr>
        `;
  });
}
