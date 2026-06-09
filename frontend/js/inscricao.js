// Função para normalizar o CPF, removendo caracteres não numéricos(tive que separar essa função para aproveitar na validação e na busca do CPF)
function normalizarCpf(cpf) {
  return cpf.replace(/\D/g, '');
}

// Guarda se o CPF está com erro para a validação do envio do formulário
let cpfTemErro = false;
// Chama a função de normalização para garantir que o CPF seja comparado corretamente e verifica se o CPF é válido, retornando true se for válido ou false se for inválido, seguindo as regras de validação do CPF, incluindo a verificação dos dígitos verificadores e evitando CPFs com todos os dígitos iguais
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

// Limpa a mensagem de erro específica do campo de CPF e remove o estado de erro do campo para permitir que o usuário corrija o CPF sem confusão
function limparMensagemCpf() {
  const mensagemCpf = document
    .getElementById('cpf')
    .parentElement.querySelector('.mensagem-erro');
  if (mensagemCpf) {
    mensagemCpf.textContent = '';
  }
  cpfTemErro = false;
}

// Retorna o elemento de mensagem de erro associado a um campo específico, assumindo que a estrutura HTML coloca a mensagem de erro como um elemento irmão dentro do mesmo contêiner pai do campo
function obterMensagemErroDoCampo(campo) {
  return campo.parentElement.querySelector('.mensagem-erro');
}

// Limpa as mensagens de erro de todos os campos obrigatórios para evitar confusão ao corrigir os erros
function limparMensagensObrigatorias() {
  document.querySelectorAll('.obrigatorio').forEach((campo) => {
    const mensagemErro = obterMensagemErroDoCampo(campo);
    if (mensagemErro) {
      mensagemErro.textContent = '';
    }
  });
}

// Verifica se os campos obrigatórios estão preenchidos e exibe mensagens de erro específicas para cada campo, retornando false se algum campo obrigatório estiver vazio ou true se todos estiverem preenchidos corretamente
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

// Exibe uma mensagem de erro específica para o campo de CPF
function exibirMensagemCpf(mensagem) {
  const mensagemCpf = document
    .getElementById('cpf')
    .parentElement.querySelector('.mensagem-erro');
  if (mensagemCpf) {
    mensagemCpf.textContent = mensagem;
  }
}

// Retorna um array de alunos do localStorage ou um array vazio se não houver dados
function obterAlunos() {
  return JSON.parse(localStorage.getItem('alunos')) || [];
}

// Salva o array de alunos no localStorage como uma string JSON
function salvarAlunos(alunos) {
  localStorage.setItem('alunos', JSON.stringify(alunos));
}

// Preenche os campos do formulário com os dados do aluno encontrado
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

//Chama a função de normalização para garantir que o CPF seja comparado corretamente e retorna o aluno correspondente, ou null se não encontrado ou se o CPF for inválido, buscando do final para o início do array para encontrar a inscrição mais recente primeiro e evitar problemas com CPFs duplicados em inscrições antigas
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

// Chama a função de normalização para garantir que o CPF seja comparado corretamente e preenche o formulário se um aluno correspondente for encontrado quando o campo de CPF perder o foco, ou seja, quando o usuário terminar de digitar o CPF e clicar fora do campo
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

// Verifica se há um evento selecionado salvo no localStorage e, se houver, seleciona a opção correspondente no dropdown de eventos e remove o item do localStorage para evitar seleção futura indesejada
function selecionarEventoPorUrl() {
  const selectEvento = document.getElementById('evento');
  const eventoSalvo = localStorage.getItem('eventoSelecionadoInscricao');

  if (!eventoSalvo || !selectEvento) {
    return;
  }

  const opcaoExiste = Array.from(selectEvento.options).some(
    (opcao) => opcao.value === eventoSalvo,
  );

  if (opcaoExiste) {
    selectEvento.value = eventoSalvo;
    localStorage.removeItem('eventoSelecionadoInscricao');
  }
}

// Chama a função de normalização para garantir que o CPF seja comparado corretamente e exibe uma mensagem de erro se o CPF for inválido, ou limpa a mensagem de erro se for válido
function validarCpfAoPreencher() {
  const cpfInput = document.getElementById('cpf');
  const cpf = cpfInput.value.trim();
  const cpfLimpo = normalizarCpf(cpf);

  if (!cpf) {
    return;
  }

  if (cpfLimpo.length < 11) {
    exibirMensagemCpf('CPF inválido');
    cpfTemErro = true;
    return;
  }

  if (!validacaoCpf(cpf)) {
    exibirMensagemCpf('CPF inválido');
    cpfTemErro = true;
  } else {
    cpfTemErro = false;
  }
}

// Evita o comportamento padrão de envio do formulário para processar os dados manualmente, valida os campos obrigatórios e o CPF, verifica se o aluno já está cadastrado para o evento selecionado e, se tudo estiver correto, salva os dados do aluno no localStorage e limpa o formulário para uma nova inscrição
function savealuno(event) {
  event.preventDefault();

  const cpfInput = document.getElementById('cpf');
  const cpf = cpfInput.value.trim();
  const eventoSelecionado = document.getElementById('evento').value;

  limparMensagensObrigatorias();
  limparMensagemCpf();

  let formularioValido = validarCamposObrigatorios();

  if (cpfTemErro || (cpf && !validacaoCpf(cpf))) {
    exibirMensagemCpf('CPF inválido');
    formularioValido = false;
  }

  if (!formularioValido) {
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
  alert('Inscrição realizada com sucesso!');
}

// Seleciona o formulário de inscrição e adiciona um evento de submit para processar a inscrição, além de adicionar eventos de input e blur para validação em tempo real dos campos obrigatórios e do CPF, garantindo que o usuário receba feedback imediato sobre os erros de preenchimento e possa corrigi-los antes de tentar enviar o formulário
document.addEventListener('DOMContentLoaded', () => {
  const formulario = document.getElementById('inscricao-formulario');

  selecionarEventoPorUrl();
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
