function normalizarDigitosAdmin(valor) {
  return (valor || '').replace(/\D/g, '');
}

// ==========================================
// 1. BUSCA DE PESSOA POR CPF E CEP NO ADMIN
// ==========================================
async function buscarPessoaPorCpfNoAdmin() {
  const cpfInput = document.getElementById('resp-cpf');
  if (!cpfInput) return;

  const cpfLimpo = normalizarDigitosAdmin(cpfInput.value);
  if (cpfLimpo.length !== 11) return;

  try {
    const resp = await fetch(`/Admin/BuscarPessoaPorCpf?cpf=${encodeURIComponent(cpfLimpo)}`);
    if (!resp.ok) return;
    const resultado = await resp.json();
    if (!resultado.encontrado || !resultado.dados) return;

    const d = resultado.dados;
    if (d.responsavelId) {
      document.getElementById('resp-id').value = d.responsavelId;
    }
    if (d.nome) document.getElementById('resp-nome').value = d.nome;
    if (d.dataNascimento) document.getElementById('resp-nascimento').value = d.dataNascimento;
    if (d.telefone) document.getElementById('resp-telefone').value = d.telefone;
    if (d.email) document.getElementById('resp-email').value = d.email;
    if (d.descricaoAtuacao) document.getElementById('resp-descricao').value = d.descricaoAtuacao;
    if (d.cep) document.getElementById('resp-cep').value = d.cep;
    if (d.logradouro) document.getElementById('resp-logradouro').value = d.logradouro;
    if (d.numeroImovel) document.getElementById('resp-numero').value = d.numeroImovel;
    if (d.complemento) document.getElementById('resp-complemento').value = d.complemento;
    if (d.bairro) document.getElementById('resp-bairro').value = d.bairro;
    if (d.municipio) document.getElementById('resp-municipio').value = d.municipio;
    if (d.uf) document.getElementById('resp-uf').value = d.uf;
  } catch {
    // Ignora falha de rede
  }
}

async function consultarCepResponsavelAdmin() {
  const cepInput = document.getElementById('resp-cep');
  const statusCep = document.getElementById('resp-status-cep');
  if (!cepInput) return;

  const cepLimpo = normalizarDigitosAdmin(cepInput.value);
  if (cepLimpo.length !== 8) return;

  try {
    if (statusCep) statusCep.textContent = 'Consultando CEP...';
    const resp = await fetch(`https://viacep.com.br/ws/${cepLimpo}/json/`);
    if (!resp.ok) throw new Error();
    const dados = await resp.json();
    if (dados.erro) {
      if (statusCep) {
        statusCep.style.color = 'var(--vermelho)';
        statusCep.textContent = 'CEP não encontrado.';
      }
      return;
    }
    if (dados.localidade) document.getElementById('resp-municipio').value = dados.localidade;
    if (dados.uf) document.getElementById('resp-uf').value = dados.uf;
    if (dados.logradouro) document.getElementById('resp-logradouro').value = dados.logradouro;
    if (dados.bairro) document.getElementById('resp-bairro').value = dados.bairro;
    if (statusCep) {
      statusCep.style.color = 'var(--verde)';
      statusCep.textContent = '✓ Endereço preenchido pelo CEP.';
    }
  } catch {
    if (statusCep) statusCep.textContent = '';
  }
}

// ==========================================
// 2. CAMPOS CONDICIONAIS DE RECORRÊNCIA (RN01-RN04, RN48)
// ==========================================
function atualizarCamposRecorrenciaEvento() {
  const selectClassif = document.getElementById('evento-classificacao');
  const selectFreq = document.getElementById('evento-frequencia');
  const selectModalidade = document.getElementById('evento-modalidade');
  const inputQtd = document.getElementById('evento-qtd-ocorrencias');
  const inputDataFinal = document.getElementById('evento-data-final');
  const inputLimiteFaltas = document.getElementById('evento-limite-faltas');
  const inputEncerramentoInsc = document.getElementById('evento-encerramento-insc');

  if (!selectClassif || !selectFreq || !selectModalidade) return;

  const ehRecorrente = selectClassif.value === 'RECORRENTE';
  selectFreq.disabled = !ehRecorrente;
  selectModalidade.disabled = !ehRecorrente;

  if (!ehRecorrente) {
    selectFreq.value = '';
    selectModalidade.value = '';
  } else {
    if (!selectFreq.value) selectFreq.value = 'SEMANAL';
    if (!selectModalidade.value) selectModalidade.value = 'QUANTIDADE_OCORRENCIAS';
  }

  const modalidade = selectModalidade.value;
  if (inputQtd) {
    inputQtd.disabled = !ehRecorrente || modalidade !== 'QUANTIDADE_OCORRENCIAS';
    if (inputQtd.disabled) inputQtd.value = '';
  }
  if (inputDataFinal) {
    inputDataFinal.disabled = !ehRecorrente || modalidade !== 'DATA_FINAL';
    if (inputDataFinal.disabled) inputDataFinal.value = '';
  }
  if (inputLimiteFaltas) {
    inputLimiteFaltas.disabled = !ehRecorrente || modalidade !== 'SEM_PREVISAO_DE_FIM';
    if (inputLimiteFaltas.disabled) inputLimiteFaltas.value = '';
  }
  if (inputEncerramentoInsc) {
    inputEncerramentoInsc.required = !ehRecorrente || modalidade !== 'SEM_PREVISAO_DE_FIM';
  }
}

document.addEventListener('DOMContentLoaded', () => {
  const respCpf = document.getElementById('resp-cpf');
  const respCep = document.getElementById('resp-cep');
  if (respCpf) respCpf.addEventListener('blur', buscarPessoaPorCpfNoAdmin);
  if (respCep) respCep.addEventListener('blur', consultarCepResponsavelAdmin);

  const selectClassif = document.getElementById('evento-classificacao');
  const selectModalidade = document.getElementById('evento-modalidade');
  if (selectClassif) selectClassif.addEventListener('change', atualizarCamposRecorrenciaEvento);
  if (selectModalidade) selectModalidade.addEventListener('change', atualizarCamposRecorrenciaEvento);
  atualizarCamposRecorrenciaEvento();

  // Inicializa data/hora padrão de abertura de inscrição se vazio
  const aberturaInput = document.getElementById('evento-abertura-insc');
  if (aberturaInput && !aberturaInput.value) {
    const agora = new Date();
    agora.setMinutes(agora.getMinutes() - agora.getTimezoneOffset());
    aberturaInput.value = agora.toISOString().slice(0, 16);
  }

  // Botões de Editar Responsável
  document.querySelectorAll('.btn-editar-resp').forEach((btn) => {
    btn.addEventListener('click', () => {
      document.getElementById('resp-id').value = btn.dataset.id || '';
      document.getElementById('resp-cpf').value = btn.dataset.cpf || '';
      document.getElementById('resp-nome').value = btn.dataset.nome || '';
      document.getElementById('resp-nascimento').value = btn.dataset.nascimento || '';
      document.getElementById('resp-telefone').value = btn.dataset.telefone || '';
      document.getElementById('resp-email').value = btn.dataset.email || '';
      document.getElementById('resp-descricao').value = btn.dataset.descricao || '';
      document.getElementById('resp-cep').value = btn.dataset.cep || '';
      document.getElementById('resp-logradouro').value = btn.dataset.logradouro || '';
      document.getElementById('resp-numero').value = btn.dataset.numero || '';
      document.getElementById('resp-complemento').value = btn.dataset.complemento || '';
      document.getElementById('resp-bairro').value = btn.dataset.bairro || '';
      document.getElementById('resp-municipio').value = btn.dataset.municipio || '';
      document.getElementById('resp-uf').value = btn.dataset.uf || '';

      document.getElementById('btn-cancelar-edicao-resp').style.display = 'inline-block';
      document.getElementById('gerenciar-responsaveis').scrollIntoView({ behavior: 'smooth' });
    });
  });

  const btnCancelarResp = document.getElementById('btn-cancelar-edicao-resp');
  if (btnCancelarResp) {
    btnCancelarResp.addEventListener('click', () => {
      document.getElementById('form-responsavel').reset();
      document.getElementById('resp-id').value = '';
      btnCancelarResp.style.display = 'none';
    });
  }

  // Botões de Editar Evento
  document.querySelectorAll('.btn-editar-evento').forEach((btn) => {
    btn.addEventListener('click', () => {
      document.getElementById('evento-id').value = btn.dataset.id || '';
      document.getElementById('evento-titulo').value = btn.dataset.titulo || '';
      document.getElementById('evento-tipo').value = btn.dataset.tipo || 'OFICINA';
      document.getElementById('evento-responsavel').value = btn.dataset.responsavel || '';
      document.getElementById('evento-local').value = btn.dataset.local || '';
      document.getElementById('evento-descricao').value = btn.dataset.descricao || '';
      document.getElementById('evento-data-primeira').value = btn.dataset.dataPrimeira || '';
      document.getElementById('evento-hora-inicio').value = btn.dataset.horaInicio || '';
      document.getElementById('evento-hora-fim').value = btn.dataset.horaFim || '';
      document.getElementById('evento-capacidade').value = btn.dataset.capacidade || '20';
      document.getElementById('evento-classificacao').value = btn.dataset.classificacao || 'UNICO';
      atualizarCamposRecorrenciaEvento();

      if (btn.dataset.frequencia) {
        document.getElementById('evento-frequencia').value = btn.dataset.frequencia;
      }
      if (btn.dataset.modalidade) {
        document.getElementById('evento-modalidade').value = btn.dataset.modalidade;
      }
      atualizarCamposRecorrenciaEvento();

      document.getElementById('evento-qtd-ocorrencias').value = btn.dataset.qtdOcorrencias || '';
      document.getElementById('evento-data-final').value = btn.dataset.dataFinal || '';
      document.getElementById('evento-limite-faltas').value = btn.dataset.limiteFaltas || '';
      document.getElementById('evento-abertura-insc').value = btn.dataset.abertura || '';
      document.getElementById('evento-encerramento-insc').value = btn.dataset.encerramento || '';

      document.getElementById('btn-cancelar-edicao-evento').style.display = 'inline-block';
      document.getElementById('gerenciar-eventos').scrollIntoView({ behavior: 'smooth' });
    });
  });

  const btnCancelarEvento = document.getElementById('btn-cancelar-edicao-evento');
  if (btnCancelarEvento) {
    btnCancelarEvento.addEventListener('click', () => {
      document.getElementById('form-evento').reset();
      document.getElementById('evento-id').value = '';
      atualizarCamposRecorrenciaEvento();
      btnCancelarEvento.style.display = 'none';
    });
  }

  // Botão de Cancelamento de Evento com motivo obrigatório (RN43)
  document.querySelectorAll('.btn-acionar-cancelamento-evento').forEach((btn) => {
    btn.addEventListener('click', () => {
      const motivo = prompt('Informe o motivo obrigatório para cancelar este evento (RN43):');
      if (motivo === null) return;
      if (!motivo.trim()) {
        alert('O motivo do cancelamento é obrigatório.');
        return;
      }
      const form = btn.closest('form');
      form.querySelector('input[name="motivo"]').value = motivo.trim();
      form.submit();
    });
  });

  // Botão de Cancelamento de Inscrição a pedido com motivo opcional (RN45)
  document.querySelectorAll('.btn-acionar-cancelamento-inscricao').forEach((btn) => {
    btn.addEventListener('click', () => {
      const motivo = prompt('Motivo do cancelamento da inscrição a pedido (opcional - RN45):');
      if (motivo === null) return;
      const form = btn.closest('form');
      form.querySelector('input[name="motivo"]').value = motivo.trim();
      form.submit();
    });
  });
});
