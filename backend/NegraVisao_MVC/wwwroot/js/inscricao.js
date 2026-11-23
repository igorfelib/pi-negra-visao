function normalizarDigitos(valor) {
  return (valor || '').replace(/\D/g, '');
}

let cpfTemErro = false;
let cepInvalidoViaCep = false;

function validacaoCpf(cpf) {
  const cpfLimpo = normalizarDigitos(cpf);

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
  if (resto === 10) resto = 0;
  if (resto !== Number(cpfLimpo.charAt(9))) {
    return false;
  }

  soma = 0;
  for (let i = 0; i < 10; i++) {
    soma += Number(cpfLimpo.charAt(i)) * (11 - i);
  }

  resto = (soma * 10) % 11;
  if (resto === 10) resto = 0;

  return resto === Number(cpfLimpo.charAt(10));
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
        mensagemErro.textContent = '* campo obrigatório';
      }
      formularioValido = false;
    }
  });

  return formularioValido;
}

// RN26-RN28: Busca CPF no servidor e preenche os campos de forma mascarada se já existir
async function consultarCpfNoServidor() {
  const cpfInput = document.getElementById('cpf');
  const erroCpf = document.getElementById('erro-cpf');
  const avisoMascara = document.getElementById('aviso-mascara-cpf');
  const botaoInscrever = document.getElementById('buttonInscrever');
  const cpf = cpfInput.value.trim();

  if (!cpf) return;

  if (!validacaoCpf(cpf)) {
    if (erroCpf) erroCpf.textContent = 'CPF inválido (informe 11 dígitos válidos)';
    cpfTemErro = true;
    return;
  }

  cpfTemErro = false;
  if (erroCpf) erroCpf.textContent = '';

  try {
    const resp = await fetch(`/Inscricao/BuscarPorCpf?cpf=${encodeURIComponent(cpf)}`);
    if (!resp.ok) return;
    const resultado = await resp.json();

    if (!resultado.encontrado) {
      if (avisoMascara) avisoMascara.style.display = 'none';
      if (botaoInscrever) botaoInscrever.disabled = false;
      const inputData = document.getElementById('dataNascimento');
      if (inputData && inputData.value.includes('*')) {
        inputData.value = '';
        inputData.type = 'date';
      }
      return;
    }

    if (resultado.inativo) {
      if (erroCpf) erroCpf.textContent = resultado.mensagem || 'Cadastro inativo.';
      if (botaoInscrever) botaoInscrever.disabled = true;
      return;
    }

    if (botaoInscrever) botaoInscrever.disabled = false;
    if (avisoMascara) avisoMascara.style.display = 'block';

    const d = resultado.dados || {};
    document.getElementById('nome').value = d.nome || '';

    const inputData = document.getElementById('dataNascimento');
    if (inputData) {
      inputData.type = 'text';
      inputData.value = d.dataNascimento || '';
    }

    const inputEmail = document.getElementById('email');
    if (inputEmail) {
      inputEmail.type = 'text';
      inputEmail.value = d.email || '';
    }

    document.getElementById('telefone').value = d.telefone || '';
    document.getElementById('cep').value = d.cep || '';
    document.getElementById('rua').value = d.logradouro || '';
    document.getElementById('numero').value = d.numeroImovel || '';
    document.getElementById('complemento').value = d.complemento || '';
    document.getElementById('bairro').value = d.bairro || '';
    document.getElementById('cidade').value = d.municipio || '';
    document.getElementById('uf').value = d.uf || '';
  } catch {
    // Em caso de falha de rede, mantém preenchimento manual
  }
}

// RN30-RN31: Consulta automática de CEP via ViaCEP
async function consultarCepViaCep() {
  const cepInput = document.getElementById('cep');
  const statusCep = document.getElementById('status-cep');
  const valorCep = cepInput.value.trim();

  if (!valorCep || valorCep.includes('*')) {
    cepInvalidoViaCep = false;
    return;
  }

  const cepLimpo = normalizarDigitos(valorCep);
  if (cepLimpo.length !== 8) {
    if (statusCep) {
      statusCep.style.color = 'var(--vermelho)';
      statusCep.textContent = 'CEP deve conter 8 dígitos (RN30).';
    }
    cepInvalidoViaCep = true;
    return;
  }

  try {
    if (statusCep) {
      statusCep.style.color = 'var(--marrom)';
      statusCep.textContent = 'Consultando CEP...';
    }

    const resp = await fetch(`https://viacep.com.br/ws/${cepLimpo}/json/`);
    if (!resp.ok) throw new Error('Indisponível');
    const dados = await resp.json();

    if (dados.erro) {
      cepInvalidoViaCep = true;
      if (statusCep) {
        statusCep.style.color = 'var(--vermelho)';
        statusCep.textContent = 'CEP não encontrado. Corrija o CEP para prosseguir (RN31).';
      }
      return;
    }

    cepInvalidoViaCep = false;
    if (dados.localidade) document.getElementById('cidade').value = dados.localidade;
    if (dados.uf) document.getElementById('uf').value = dados.uf;
    if (dados.logradouro) document.getElementById('rua').value = dados.logradouro;
    if (dados.bairro) document.getElementById('bairro').value = dados.bairro;

    if (statusCep) {
      statusCep.style.color = 'var(--verde)';
      statusCep.textContent =
        dados.logradouro && dados.bairro
          ? '✓ CEP localizado. Informe o número do imóvel.'
          : '✓ CEP parcial localizado. Complete logradouro, bairro e número manualmente (RN30).';
    }
  } catch {
    cepInvalidoViaCep = false;
    if (statusCep) {
      statusCep.style.color = 'var(--marrom)';
      statusCep.textContent =
        'Serviço de CEP indisponível no momento. Preencha o endereço manualmente (RN31).';
    }
  }
}

document.addEventListener('DOMContentLoaded', () => {
  const formulario = document.getElementById('inscricao-formulario');
  const cpfInput = document.getElementById('cpf');
  const cepInput = document.getElementById('cep');
  const dataInput = document.getElementById('dataNascimento');

  if (dataInput) {
    dataInput.addEventListener('focus', () => {
      if (dataInput.value.includes('*')) {
        dataInput.value = '';
      }
      dataInput.type = 'date';
    });
  }

  if (cpfInput) {
    cpfInput.addEventListener('blur', consultarCpfNoServidor);
  }

  if (cepInput) {
    cepInput.addEventListener('blur', consultarCepViaCep);
  }

  if (formulario) {
    formulario.addEventListener('submit', (event) => {
      limparMensagensObrigatorias();
      let valido = validarCamposObrigatorios();

      if (!validacaoCpf(cpfInput.value)) {
        const erroCpf = document.getElementById('erro-cpf');
        if (erroCpf) erroCpf.textContent = 'CPF inválido';
        valido = false;
      }

      if (cepInvalidoViaCep) {
        const statusCep = document.getElementById('status-cep');
        if (statusCep) {
          statusCep.style.color = 'var(--vermelho)';
          statusCep.textContent = 'Corrija o CEP antes de confirmar a inscrição (RN31).';
        }
        valido = false;
      }

      if (!valido) {
        event.preventDefault();
      }
    });
  }
});
