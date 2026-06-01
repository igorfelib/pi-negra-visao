function normalizarCpf(cpf) {
  return cpf.replace(/\D/g, '');
}

function validacaoCpf(cpf) {
  const cpfLimpo = normalizarCpf(cpf);

  if (cpfLimpo.length !== 11) {
    return false;
  }

  if (/^(\d)\1+$/.test(cpfLimpo)) {
    return false;
  }

  let soma = 0;
  for (let i = 0; i < 9; i++) {
    soma += Number(cpfLimpo.charAt(i)) * (10 - i);
  }

  let resto = (soma * 10) % 11;
  if (resto === 10) {
    resto = 0;
  }

  if (resto !== Number(cpfLimpo.charAt(9))) {
    return false;
  }

  soma = 0;
  for (let i = 0; i < 10; i++) {
    soma += Number(cpfLimpo.charAt(i)) * (11 - i);
  }

  resto = (soma * 10) % 11;
  if (resto === 10) {
    resto = 0;
  }

  return resto === Number(cpfLimpo.charAt(10));
}

function limparMensagemCpf() {
  const mensagemCpf = document
    .getElementById('cpf')
    .parentElement.querySelector('.mensagem-erro');
  if (mensagemCpf) {
    mensagemCpf.textContent = '';
  }
}

function obterMensagemErroDoCampo(campo) {
  return campo.parentElement.querySelector('.mensagem-erro');
}

function limparMensagensObrigatorias() {
  document.querySelectorAll('.obrigatorio').forEach((campo) => {
    const mensagemErro = obterMensagemErroDoCampo(campo);
    if (mensagemErro) {
      mensagemErro.textContent = '';
    }
  });
}

function validarCamposObrigatorios() {
  const camposObrigatorios = document.querySelectorAll('.obrigatorio');
  let formularioValido = true;

  camposObrigatorios.forEach((campo) => {
    const mensagemErro = obterMensagemErroDoCampo(campo);

    if (!campo.value.trim()) {
      if (mensagemErro) {
        mensagemErro.textContent = '* campo obrigatorio';
      }
      formularioValido = false;
    }
  });

  return formularioValido;
}

function exibirMensagemCpf(mensagem) {
  const mensagemCpf = document
    .getElementById('cpf')
    .parentElement.querySelector('.mensagem-erro');
  if (mensagemCpf) {
    mensagemCpf.textContent = mensagem;
  }
}

function obterAlunos() {
  return JSON.parse(localStorage.getItem('alunos')) || [];
}

function salvarAlunos(alunos) {
  localStorage.setItem('alunos', JSON.stringify(alunos));
}

function preencherFormularioComAluno(aluno) {
  document.getElementById('nome').value = aluno.nome || '';
  document.getElementById('telefone').value = aluno.telefone || '';
  document.getElementById('email').value = aluno.email || '';
  document.getElementById('cep').value = aluno.cep || '';
  document.getElementById('cidade').value = aluno.cidade || '';
  document.getElementById('bairro').value = aluno.bairro || '';
  document.getElementById('rua').value = aluno.rua || '';
  document.getElementById('numero').value = aluno.numeroresidencia || '';
}

function buscarAlunoPorCpf(cpf) {
  const cpfLimpo = normalizarCpf(cpf);
  const alunos = obterAlunos();

  for (let i = alunos.length - 1; i >= 0; i -= 1) {
    if (normalizarCpf(alunos[i].cpf || '') === cpfLimpo) {
      return alunos[i];
    }
  }

  return null;
}

function preencherFormularioSeCpfExistente() {
  const cpfInput = document.getElementById('cpf');
  const cpf = cpfInput.value.trim();

  if (!validacaoCpf(cpf)) {
    return;
  }

  const aluno = buscarAlunoPorCpf(cpf);
  if (aluno) {
    preencherFormularioComAluno(aluno);
  }
}

function validarCpfAoPreencher() {
  const cpfInput = document.getElementById('cpf');
  const cpf = cpfInput.value.trim();
  const cpfLimpo = normalizarCpf(cpf);

  if (!cpf) {
    return;
  }

  if (cpfLimpo.length < 11) {
    return;
  }

  if (!validacaoCpf(cpf)) {
    exibirMensagemCpf('CPF inválido');
  }
}

function savealuno(event) {
  event.preventDefault();

  const cpfInput = document.getElementById('cpf');
  const cpf = cpfInput.value.trim();
  const eventoSelecionado = document.getElementById('evento').value;

  limparMensagensObrigatorias();
  limparMensagemCpf();

  if (!validarCamposObrigatorios()) {
    return;
  }

  if (!validacaoCpf(cpf)) {
    exibirMensagemCpf('CPF inválido');
    return;
  }

  const alunos = obterAlunos();
  const cpfLimpo = normalizarCpf(cpf);

  if (
    alunos.some(
      (c) =>
        normalizarCpf(c.cpf || '') === cpfLimpo &&
        c.evento === eventoSelecionado,
    )
  ) {
    alert('Aluno ja cadastrado para este evento');
    return;
  }

  const valor = {
    cpf,
    nome: document.getElementById('nome').value,
    telefone: document.getElementById('telefone').value,
    email: document.getElementById('email').value,
    cep: document.getElementById('cep').value,
    cidade: document.getElementById('cidade').value,
    bairro: document.getElementById('bairro').value,
    rua: document.getElementById('rua').value,
    numeroresidencia: document.getElementById('numero').value,
    evento: eventoSelecionado,
  };

  alunos.push(valor);
  salvarAlunos(alunos);
  document.getElementById('inscricao-formulario').reset();
}

document.addEventListener('DOMContentLoaded', () => {
  const formulario = document.getElementById('inscricao-formulario');

  formulario.addEventListener('submit', savealuno);

  document.querySelectorAll('.obrigatorio').forEach((campo) => {
    campo.addEventListener('input', () => {
      const mensagemErro = obterMensagemErroDoCampo(campo);
      if (mensagemErro) {
        mensagemErro.textContent = '';
      }
    });
  });

  document.getElementById('cpf').addEventListener('input', limparMensagemCpf);
  document
    .getElementById('cpf')
    .addEventListener('blur', validarCpfAoPreencher);
  document
    .getElementById('cpf')
    .addEventListener('blur', preencherFormularioSeCpfExistente);
});
