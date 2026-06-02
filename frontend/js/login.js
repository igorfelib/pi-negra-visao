document.addEventListener('DOMContentLoaded', function () {
  const formulario = document.getElementById('login-formulario');
  const campoUsuario = document.getElementById('usuario');
  const campoSenha = document.getElementById('senha');
  const mensagensErro = document.querySelectorAll('.mensagem-erro');

  // Limpa a mensagem de erro específica do campo de usuário quando o usuário começa a digitar, garantindo que o feedback de erro seja atualizado em tempo real e que o usuário saiba que o erro foi corrigido
  campoUsuario.addEventListener('input', function () {
    limparMensagemErroIndividual(0);
  });

  // Limpa a mensagem de erro específica do campo de senha quando o usuário começa a digitar, garantindo que o feedback de erro seja atualizado em tempo real e que o usuário saiba que o erro foi corrigido
  campoSenha.addEventListener('input', function () {
    limparMensagemErroIndividual(1);
  });

  // Adiciona um evento de blur (perda de foco) ao campo de usuário para validar se o campo está vazio e exibir a mensagem de erro
  formulario.addEventListener('submit', function (event) {
    event.preventDefault();
    limparMensagensErro();

    if (!validarCamposObrigatorios()) {
      exibirMensagemCampoObrigatorio();
      return;
    }

    if (
      campoUsuario.value.trim() === 'admin' &&
      campoSenha.value.trim() === 'admin'
    ) {
      window.location.href = './admin.html';
      return;
    }

    exibirMensagemCredenciaisInvalidas();
  });

  // Limpa todas as mensagens de erro, garantindo que o formulário esteja limpo de mensagens antigas antes de validar novamente
  function limparMensagensErro() {
    mensagensErro.forEach(function (mensagem) {
      mensagem.textContent = '';
    });
  }

  // Limpa a mensagem de erro específica para o campo correspondente, garantindo que o feedback de erro seja atualizado em tempo real e que o usuário saiba que o erro foi corrigido
  function limparMensagemErroIndividual(indice) {
    if (mensagensErro[indice]) {
      mensagensErro[indice].textContent = '';
    }
  }

  // Verifica se os campos de usuário e senha não estão vazios, garantindo que o formulário só seja enviado quando ambos os campos estiverem preenchidos
  function validarCamposObrigatorios() {
    return campoUsuario.value.trim() !== '' && campoSenha.value.trim() !== '';
  }

  // Exibe uma mensagem de erro para cada campo obrigatório que estiver vazio, garantindo que o usuário saiba quais campos precisam ser preenchidos para prosseguir
  function exibirMensagemCampoObrigatorio() {
    mensagensErro.forEach(function (mensagem) {
      mensagem.textContent = '* campo obrigatorio';
    });
  }

  // Exibe uma mensagem de erro indicando que as credenciais são inválidas, garantindo que o usuário saiba que o nome de usuário ou senha estão incorretos e possa tentar novamente
  function exibirMensagemCredenciaisInvalidas() {
    mensagensErro.forEach(function (mensagem) {
      mensagem.textContent = 'usuario e senha não correspondem';
    });
  }
});
