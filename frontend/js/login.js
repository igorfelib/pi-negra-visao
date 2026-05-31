document.addEventListener('DOMContentLoaded', function () {
  const formulario = document.getElementById('login-formulario');
  const campoUsuario = document.getElementById('usuario');
  const campoSenha = document.getElementById('senha');
  const mensagensErro = document.querySelectorAll('.mensagem-erro');

  campoUsuario.addEventListener('input', function () {
    limparMensagemErroIndividual(0);
  });

  campoSenha.addEventListener('input', function () {
    limparMensagemErroIndividual(1);
  });

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

  function limparMensagensErro() {
    mensagensErro.forEach(function (mensagem) {
      mensagem.textContent = '';
    });
  }

  function limparMensagemErroIndividual(indice) {
    if (mensagensErro[indice]) {
      mensagensErro[indice].textContent = '';
    }
  }

  function validarCamposObrigatorios() {
    return campoUsuario.value.trim() !== '' && campoSenha.value.trim() !== '';
  }

  function exibirMensagemCampoObrigatorio() {
    mensagensErro.forEach(function (mensagem) {
      mensagem.textContent = '* campo obrigatorio';
    });
  }

  function exibirMensagemCredenciaisInvalidas() {
    mensagensErro.forEach(function (mensagem) {
      mensagem.textContent = 'usuario e senha não correspondem';
    });
  }
});
